using System.Web.UI;
using BaiRong.Core;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System;
using SiteServer.CMS.Model;
using BaiRong.Core.Net;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.CMS.Core;


namespace SiteServer.CMS.Core
{
    public class MessageManager
    {
        private MessageManager()
        {
        }

        public static void SendMailByInput(PublishmentSystemInfo publishmentSystemInfo, InputInfo inputInfo, ArrayList relatedIdentities, InputContentInfo contentInfo)
        {
            try
            {
                if (inputInfo.Additional.IsMail && !string.IsNullOrEmpty(inputInfo.Additional.MailTo))
                {

                    ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.InputContent, DataProvider.InputContentDAO.TableName, relatedIdentities);

                    ArrayList mailToArrayList = new ArrayList();
                    if (inputInfo.Additional.MailReceiver == ETriState.All || inputInfo.Additional.MailReceiver == ETriState.True)
                    {
                        if (!string.IsNullOrEmpty(inputInfo.Additional.MailTo))
                        {
                            mailToArrayList.AddRange(TranslateUtils.StringCollectionToArrayList(inputInfo.Additional.MailTo, ';'));
                        }
                    }
                    if (inputInfo.Additional.MailReceiver == ETriState.All || inputInfo.Additional.MailReceiver == ETriState.False)
                    {
                        string mailTo = contentInfo.GetExtendedAttribute(inputInfo.Additional.MailFiledName);
                        if (!string.IsNullOrEmpty(mailTo))
                        {
                            mailToArrayList.AddRange(TranslateUtils.StringCollectionToArrayList(mailTo, ';'));
                        }
                    }
                    StringBuilder builder = new StringBuilder();

                    if (inputInfo.Additional.IsMailTemplate && !string.IsNullOrEmpty(inputInfo.Additional.MailContent))
                    {
                        builder.Append(inputInfo.Additional.MailContent);
                    }
                    else
                    {
                        builder.Append(MessageManager.GetMailContent(styleInfoArrayList));
                    }

                    string content = builder.ToString();
                    content = StringUtils.ReplaceIgnoreCase(content, "[addDate]", contentInfo.AddDate.ToShortDateString());
                    content = StringUtils.ReplaceIgnoreCase(content, "[ipAddress]", contentInfo.IPAddress);

                    foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                    {
                        string theValue = InputTypeParser.GetContentByTableStyle(contentInfo.GetExtendedAttribute(styleInfo.AttributeName), publishmentSystemInfo, ETableStyle.InputContent, styleInfo);

                        content = StringUtils.ReplaceIgnoreCase(content, string.Format("[{0}]", styleInfo.AttributeName), theValue);
                    }
                    string errorMessage;

                    UserMailManager.SendMail(TranslateUtils.ObjectCollectionToString(mailToArrayList), inputInfo.Additional.MailTitle, content, out errorMessage);

                }
            }
            catch { }
        }

        public static string GetMailContent(ArrayList styleInfoArrayList)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(@"
<table cellSpacing=""3"" cellPadding=""3"" width=""95%"" align=""center""><tbody>");

            foreach (TableStyleInfo styleInfo in styleInfoArrayList)
            {
                if (styleInfo.IsVisible == false) continue;

                builder.AppendFormat(@"
<tr><td width=""118"" height=""25""><strong>{0}：</strong> </td><td><p>[{1}]</p></td></tr>", styleInfo.DisplayName, styleInfo.AttributeName);
            }

            builder.Append(@"
</tbody></table>");
            return builder.ToString();
        }

        public static void SendSMSByInput(PublishmentSystemInfo publishmentSystemInfo, InputInfo inputInfo, ArrayList relatedIdentities, InputContentInfo contentInfo)
        {
            try
            {
                if (inputInfo.Additional.IsSMS)
                {
                    ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.InputContent, DataProvider.InputContentDAO.TableName, relatedIdentities);

                    ArrayList smsToArrayList = new ArrayList();
                    if (inputInfo.Additional.SMSReceiver == ETriState.All || inputInfo.Additional.SMSReceiver == ETriState.True)
                    {
                        if (!string.IsNullOrEmpty(inputInfo.Additional.SMSTo))
                        {
                            string[] mobiles = inputInfo.Additional.SMSTo.Split(';', ',');
                            foreach (string mobile in mobiles)
                            {
                                if (!string.IsNullOrEmpty(mobile) && StringUtils.IsMobile(mobile) && !smsToArrayList.Contains(mobile))
                                {
                                    smsToArrayList.Add(mobile);
                                }
                            }
                        }
                    }
                    if (inputInfo.Additional.SMSReceiver == ETriState.All || inputInfo.Additional.SMSReceiver == ETriState.False)
                    {
                        string smsTo = contentInfo.GetExtendedAttribute(inputInfo.Additional.SMSFiledName);
                        if (!string.IsNullOrEmpty(smsTo))
                        {
                            string[] mobiles = smsTo.Split(';', ',');
                            foreach (string mobile in mobiles)
                            {
                                if (!string.IsNullOrEmpty(mobile) && StringUtils.IsMobile(mobile) && !smsToArrayList.Contains(mobile))
                                {
                                    smsToArrayList.Add(mobile);
                                }
                            }
                        }
                    }

                    StringBuilder builder = new StringBuilder();

                    if (inputInfo.Additional.IsSMSTemplate && !string.IsNullOrEmpty(inputInfo.Additional.SMSContent))
                    {
                        builder.Append(inputInfo.Additional.SMSContent);
                    }
                    else
                    {
                        builder.Append(MessageManager.GetSMSContent(styleInfoArrayList));
                    }

                    string content = builder.ToString();
                    content = StringUtils.ReplaceIgnoreCase(content, "[addDate]", contentInfo.AddDate.ToShortDateString());
                    content = StringUtils.ReplaceIgnoreCase(content, "[ipAddress]", contentInfo.IPAddress);

                    foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                    {
                        string theValue = InputTypeParser.GetContentByTableStyle(contentInfo.GetExtendedAttribute(styleInfo.AttributeName), publishmentSystemInfo, ETableStyle.InputContent, styleInfo);

                        content = StringUtils.ReplaceIgnoreCase(content, string.Format("[{0}]", styleInfo.AttributeName), theValue);
                    }

                    if (content.Length > 60)
                    {
                        content = content.Substring(0, 60);
                    }

                    string errorMessage = string.Empty;
                    SMSServerManager.Send(smsToArrayList, content, out errorMessage);
                }
            }
            catch { }
        }

        public static string GetSMSContent(ArrayList styleInfoArrayList)
        {
            StringBuilder builder = new StringBuilder();

            foreach (TableStyleInfo styleInfo in styleInfoArrayList)
            {
                if (styleInfo.IsVisible == false) continue;

                builder.AppendFormat(@"{0}:[{1}],", styleInfo.DisplayName, styleInfo.AttributeName);
            }

            if (builder.Length > 0)
            {
                builder.Length = builder.Length - 1;
            }

            return builder.ToString();
        }


        #region 网站留言
        public static void SendMailByWebsiteMessageReply(PublishmentSystemInfo publishmentSystemInfo, WebsiteMessageInfo websiteMessageInfo, ArrayList relatedIdentities, WebsiteMessageContentInfo contentInfo)
        {
            try
            {
                if (websiteMessageInfo.Additional.IsMail && !string.IsNullOrEmpty(websiteMessageInfo.Additional.MailTo))
                {

                    ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.WebsiteMessageContent, DataProvider.WebsiteMessageContentDAO.TableName, relatedIdentities);

                    ArrayList mailToArrayList = new ArrayList();
                    if (websiteMessageInfo.Additional.MailReceiver == ETriState.All || websiteMessageInfo.Additional.MailReceiver == ETriState.True)
                    {
                        if (!string.IsNullOrEmpty(websiteMessageInfo.Additional.MailTo))
                        {
                            mailToArrayList.AddRange(TranslateUtils.StringCollectionToArrayList(websiteMessageInfo.Additional.MailTo, ';'));
                        }
                    }
                    if (websiteMessageInfo.Additional.MailReceiver == ETriState.All || websiteMessageInfo.Additional.MailReceiver == ETriState.False)
                    {
                        string mailTo = contentInfo.GetExtendedAttribute(websiteMessageInfo.Additional.MailFiledName);
                        if (!string.IsNullOrEmpty(mailTo))
                        {
                            mailToArrayList.AddRange(TranslateUtils.StringCollectionToArrayList(mailTo, ';'));
                        }
                    }
                    StringBuilder builder = new StringBuilder();

                    if (websiteMessageInfo.Additional.IsMailTemplate && !string.IsNullOrEmpty(websiteMessageInfo.Additional.MailContent))
                    {
                        builder.Append(websiteMessageInfo.Additional.MailContent);
                    }
                    else
                    {
                        builder.Append(MessageManager.GetMailContentWebsiteMessageReply(styleInfoArrayList));
                    }

                    string content = builder.ToString();
                    content = StringUtils.ReplaceIgnoreCase(content, "[addDate]", contentInfo.AddDate.ToShortDateString());
                    content = StringUtils.ReplaceIgnoreCase(content, "[ipAddress]", contentInfo.IPAddress);
                    content = StringUtils.ReplaceIgnoreCase(content, "[Reply]", contentInfo.Reply);

                    foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                    {
                        string theValue = InputTypeParser.GetContentByTableStyle(contentInfo.GetExtendedAttribute(styleInfo.AttributeName), publishmentSystemInfo, ETableStyle.WebsiteMessageContent, styleInfo);

                        content = StringUtils.ReplaceIgnoreCase(content, string.Format("[{0}]", styleInfo.AttributeName), theValue);
                    }
                    string errorMessage;

                    UserMailManager.SendMail(TranslateUtils.ObjectCollectionToString(mailToArrayList), websiteMessageInfo.Additional.MailTitle, content, out errorMessage);

                }
            }
            catch { }
        }

        public static string GetMailContentWebsiteMessageReply(ArrayList styleInfoArrayList)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(@"
<table cellSpacing=""3"" cellPadding=""3"" width=""95%"" align=""center""><tbody>");

            foreach (TableStyleInfo styleInfo in styleInfoArrayList)
            {
                if (styleInfo.IsVisible == false) continue;

                builder.AppendFormat(@"
<tr><td width=""118"" height=""25""><strong>{0}：</strong> </td><td><p>[{1}]</p></td></tr>", styleInfo.DisplayName, styleInfo.AttributeName);
            }

            builder.Append(@"
</tbody></table>
<div>回复内容：[Reply]</div>");
            return builder.ToString();
        }

        public static void SendSMSByWebsiteMessageWebsiteMessageReply(PublishmentSystemInfo publishmentSystemInfo, WebsiteMessageInfo websiteMessageInfo, ArrayList relatedIdentities, WebsiteMessageContentInfo contentInfo)
        {
            try
            {
                if (websiteMessageInfo.Additional.IsSMS)
                {
                    ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.WebsiteMessageContent, DataProvider.WebsiteMessageContentDAO.TableName, relatedIdentities);

                    ArrayList smsToArrayList = new ArrayList();
                    if (websiteMessageInfo.Additional.SMSReceiver == ETriState.All || websiteMessageInfo.Additional.SMSReceiver == ETriState.True)
                    {
                        if (!string.IsNullOrEmpty(websiteMessageInfo.Additional.SMSTo))
                        {
                            string[] mobiles = websiteMessageInfo.Additional.SMSTo.Split(';', ',');
                            foreach (string mobile in mobiles)
                            {
                                if (!string.IsNullOrEmpty(mobile) && StringUtils.IsMobile(mobile) && !smsToArrayList.Contains(mobile))
                                {
                                    smsToArrayList.Add(mobile);
                                }
                            }
                        }
                    }
                    if (websiteMessageInfo.Additional.SMSReceiver == ETriState.All || websiteMessageInfo.Additional.SMSReceiver == ETriState.False)
                    {
                        string smsTo = contentInfo.GetExtendedAttribute(websiteMessageInfo.Additional.SMSFiledName);
                        if (!string.IsNullOrEmpty(smsTo))
                        {
                            string[] mobiles = smsTo.Split(';', ',');
                            foreach (string mobile in mobiles)
                            {
                                if (!string.IsNullOrEmpty(mobile) && StringUtils.IsMobile(mobile) && !smsToArrayList.Contains(mobile))
                                {
                                    smsToArrayList.Add(mobile);
                                }
                            }
                        }
                    }

                    StringBuilder builder = new StringBuilder();

                    if (websiteMessageInfo.Additional.IsSMSTemplate && !string.IsNullOrEmpty(websiteMessageInfo.Additional.SMSContent))
                    {
                        builder.Append(websiteMessageInfo.Additional.SMSContent);
                    }
                    else
                    {
                        builder.Append(MessageManager.GetSMSContentWebsiteMessageReply(styleInfoArrayList));
                    }

                    string content = builder.ToString();
                    content = StringUtils.ReplaceIgnoreCase(content, "[addDate]", contentInfo.AddDate.ToShortDateString());
                    content = StringUtils.ReplaceIgnoreCase(content, "[ipAddress]", contentInfo.IPAddress);
                    content = StringUtils.ReplaceIgnoreCase(content, "[Reply]", contentInfo.Reply);

                    foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                    {
                        string theValue = InputTypeParser.GetContentByTableStyle(contentInfo.GetExtendedAttribute(styleInfo.AttributeName), publishmentSystemInfo, ETableStyle.WebsiteMessageContent, styleInfo);

                        content = StringUtils.ReplaceIgnoreCase(content, string.Format("[{0}]", styleInfo.AttributeName), theValue);
                    }

                    if (content.Length > 60)
                    {
                        content = content.Substring(0, 60);
                    }

                    string errorMessage = string.Empty;
                    SMSServerManager.Send(smsToArrayList, content, out errorMessage);
                }
            }
            catch { }
        }

        public static string GetSMSContentWebsiteMessageReply(ArrayList styleInfoArrayList)
        {
            StringBuilder builder = new StringBuilder();

            foreach (TableStyleInfo styleInfo in styleInfoArrayList)
            {
                if (styleInfo.IsVisible == false) continue;

                builder.AppendFormat(@"{0}:[{1}],", styleInfo.DisplayName, styleInfo.AttributeName);
            }

            if (builder.Length > 0)
            {
                builder.Length = builder.Length - 1;
            }
            
            builder.Append(@"\r\n回复内容：[Reply]");

            return builder.ToString();
        } 
        #endregion


        public static void SendMailByContentInput(PublishmentSystemInfo publishmentSystemInfo, TagStyleContentInputInfo inputInfo, ArrayList relatedIdentities, ContentInfo contentInfo)
        {
            try
            {
                if (inputInfo.IsMail && !string.IsNullOrEmpty(inputInfo.MailTo))
                {

                    ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.BackgroundContent, publishmentSystemInfo.AuxiliaryTableForContent, relatedIdentities);

                    string[] usernames = inputInfo.MailTo.Split(new char[] { ',', ';' });
                    StringBuilder builder = new StringBuilder();
                    builder.Append(@"
<TABLE cellSpacing=""3"" cellPadding=""3"" width=""95%"" align=""center""><TBODY>");

                    foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                    {
                        if (styleInfo.IsVisible == false) continue;

                        string theValue = InputTypeParser.GetContentByTableStyle(contentInfo.GetExtendedAttribute(styleInfo.AttributeName), publishmentSystemInfo, ETableStyle.BackgroundContent, styleInfo);

                        builder.AppendFormat(@"
<TR><TD width=""118"" height=""25""><STRONG>{0}：</STRONG> </TD><TD><P>{1}</P></TD></TR>", styleInfo.DisplayName, theValue);
                    }

                    builder.Append(@"
</TBODY></TABLE>");

                    string errorMessage;
                    UserMailManager.SendMail(TranslateUtils.ObjectCollectionToString(usernames), inputInfo.MailTitleFormat, builder.ToString(), out errorMessage);
                }
            }
            catch { }
        }

        public static void SendSMSByContentInput(PublishmentSystemInfo publishmentSystemInfo, TagStyleContentInputInfo inputInfo, ArrayList relatedIdentities, ContentInfo contentInfo)
        {
            try
            {
                if (inputInfo.IsSMS && !string.IsNullOrEmpty(inputInfo.SMSTo))
                {

                    ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.BackgroundContent, publishmentSystemInfo.AuxiliaryTableForContent, relatedIdentities);

                    string[] mobiles = inputInfo.SMSTo.Split(';', ',');
                    ArrayList mobileArrayList = new ArrayList();

                    foreach (string mobile in mobiles)
                    {
                        if (!string.IsNullOrEmpty(mobile) && StringUtils.IsMobile(mobile) && !mobileArrayList.Contains(mobile))
                        {
                            mobileArrayList.Add(mobile);
                        }
                    }

                    StringBuilder builder = new StringBuilder(inputInfo.SMSTitle);

                    foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                    {
                        if (styleInfo.IsVisible == false) continue;

                        string theValue = InputTypeParser.GetContentByTableStyle(contentInfo.GetExtendedAttribute(styleInfo.AttributeName), publishmentSystemInfo, ETableStyle.BackgroundContent, styleInfo);

                        builder.AppendFormat(@"{0}：{1},", styleInfo.DisplayName, theValue);
                    }

                    if (builder.Length > 0)
                    {
                        builder.Length = builder.Length - 1;
                    }

                    if (builder.Length > 60)
                    {
                        builder.Length = 60;
                    }

                    string errorMessage = string.Empty;
                    SMSServerManager.Send(mobileArrayList, builder.ToString(), out errorMessage);
                }
            }
            catch { }
        }

        public static void SendMailByResumeInput(PublishmentSystemInfo publishmentSystemInfo, TagStyleResumeInfo resumeInfo, ResumeContentInfo contentInfo)
        {
            try
            {
                if (resumeInfo.IsMail && !string.IsNullOrEmpty(resumeInfo.MailTo))
                {
                    string[] usernames = resumeInfo.MailTo.Split(new char[] { ',', ';' });
                    StringBuilder builder = new StringBuilder();
                    builder.Append(@"
<TABLE cellSpacing=""3"" cellPadding=""3"" width=""95%"" align=""center""><TBODY>");

                    NameValueCollection attributes = new NameValueCollection();
                    attributes["姓名"] = contentInfo.GetExtendedAttribute(ResumeContentAttribute.RealName);
                    attributes["性别"] = contentInfo.GetExtendedAttribute(ResumeContentAttribute.Gender);
                    attributes["手机号码"] = contentInfo.GetExtendedAttribute(ResumeContentAttribute.MobilePhone);
                    attributes["邮箱"] = contentInfo.GetExtendedAttribute(ResumeContentAttribute.Email);
                    attributes["学历"] = contentInfo.GetExtendedAttribute(ResumeContentAttribute.Education);
                    attributes["毕业院校"] = contentInfo.GetExtendedAttribute(ResumeContentAttribute.LastSchoolName);
                    attributes["申请职位"] = contentInfo.GetExtendedAttribute(ResumeContentAttribute.ExpectSalary);
                    attributes["添加时间"] = contentInfo.GetExtendedAttribute(ResumeContentAttribute.AddDate);
                    foreach (string key in attributes.Keys)
                    {
                        string theValue = attributes[key];

                        builder.AppendFormat(@"
<TR><TD width=""118"" height=""25""><STRONG>{0}：</STRONG> </TD><TD><P>{1}</P></TD></TR>", key, theValue);
                    }

                    builder.Append(@"
</TBODY></TABLE>");

                    string errorMessage;
                    UserMailManager.SendMail(TranslateUtils.ObjectCollectionToString(usernames), resumeInfo.MailTitleFormat, builder.ToString(), out errorMessage);
                }
            }
            catch { }
        }

        public static void SendSMSByResumeInput(PublishmentSystemInfo publishmentSystemInfo, TagStyleResumeInfo resumeInfo, ResumeContentInfo contentInfo)
        {
            try
            {
                if (resumeInfo.IsSMS && !string.IsNullOrEmpty(resumeInfo.SMSTo))
                {



                    string[] mobiles = resumeInfo.SMSTo.Split(';', ',');
                    ArrayList mobileArrayList = new ArrayList();

                    foreach (string mobile in mobiles)
                    {
                        if (!string.IsNullOrEmpty(mobile) && StringUtils.IsMobile(mobile) && !mobileArrayList.Contains(mobile))
                        {
                            mobileArrayList.Add(mobile);
                        }
                    }

                    StringBuilder builder = new StringBuilder(resumeInfo.SMSTitle);
                    NameValueCollection attributes = new NameValueCollection();
                    attributes["姓名"] = contentInfo.GetExtendedAttribute(ResumeContentAttribute.RealName);
                    attributes["性别"] = contentInfo.GetExtendedAttribute(ResumeContentAttribute.Gender);
                    attributes["手机号码"] = contentInfo.GetExtendedAttribute(ResumeContentAttribute.MobilePhone);
                    attributes["邮箱"] = contentInfo.GetExtendedAttribute(ResumeContentAttribute.Email);
                    attributes["学历"] = contentInfo.GetExtendedAttribute(ResumeContentAttribute.Education);
                    attributes["毕业院校"] = contentInfo.GetExtendedAttribute(ResumeContentAttribute.LastSchoolName);
                    attributes["申请职位"] = contentInfo.GetExtendedAttribute(ResumeContentAttribute.ExpectSalary);
                    attributes["添加时间"] = contentInfo.GetExtendedAttribute(ResumeContentAttribute.AddDate);
                    foreach (string key in attributes.Keys)
                    {
                        string theValue = attributes[key];

                        builder.AppendFormat(@"{0}：{1},", key, theValue);
                    }

                    if (builder.Length > 0)
                    {
                        builder.Length = builder.Length - 1;
                    }

                    if (builder.Length > 60)
                    {
                        builder.Length = 60;
                    }

                    string errorMessage = string.Empty;
                    SMSServerManager.Send(mobileArrayList, builder.ToString(), out errorMessage);
                }
            }
            catch { }
        }

        public static bool SendMailByPublishmentSystemID(int publishmentSystemID, string emailList, string mailTitle, string mailBody, out string errorMessage)
        {
            try
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                string[] usernames = emailList.Split(new char[] { ',', ';' });

                return UserMailManager.SendMail(TranslateUtils.ObjectCollectionToString(usernames), mailTitle, "<pre style=\"width:100%;word-wrap:break-word\">" + mailBody + "</pre>", out errorMessage);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public static void SendMailByGovPublicApply(PublishmentSystemInfo publishmentSystemInfo, TagStyleGovPublicApplyInfo tagStyleInfo, GovPublicApplyInfo applyInfo)
        {
            try
            {
                if (tagStyleInfo.IsMail && !string.IsNullOrEmpty(tagStyleInfo.MailTo))
                {
                    
                    string[] usernames = tagStyleInfo.MailTo.Split(new char[] { ',', ';' });
                    
                    StringBuilder builder = new StringBuilder();
                    builder.Append(@"
<TABLE cellSpacing=""3"" cellPadding=""3"" width=""95%"" align=""center""><TBODY>");

                    foreach (string key in applyInfo.Attributes.Keys)
                    {
                        if (GovPublicApplyAttribute.BasicAttributes.Contains(key.ToLower()))
                        {
                            string theValue = applyInfo.Attributes[key];

                            builder.AppendFormat(@"
<TR><TD width=""118"" height=""25""><STRONG>{0}：</STRONG> </TD><TD><P>{1}</P></TD></TR>", key, theValue);
                        }
                    }

                    builder.Append(@"
</TBODY></TABLE>");

                    string errorMessage;
                    UserMailManager.SendMail(TranslateUtils.ObjectCollectionToString(usernames), tagStyleInfo.MailTitleFormat, builder.ToString(), out errorMessage);
                }
            }
            catch { }
        }

        public static void SendSMSByGovPublicApply(PublishmentSystemInfo publishmentSystemInfo, TagStyleGovPublicApplyInfo tagStyleInfo, GovPublicApplyInfo applyInfo)
        {
            try
            {
                if (tagStyleInfo.IsSMS && !string.IsNullOrEmpty(tagStyleInfo.SMSTo))
                {
                    string[] mobiles = tagStyleInfo.SMSTo.Split(';', ',');
                    ArrayList mobileArrayList = new ArrayList();

                    foreach (string mobile in mobiles)
                    {
                        if (!string.IsNullOrEmpty(mobile) && StringUtils.IsMobile(mobile) && !mobileArrayList.Contains(mobile))
                        {
                            mobileArrayList.Add(mobile);
                        }
                    }

                    StringBuilder builder = new StringBuilder(tagStyleInfo.SMSTitle);
                    NameValueCollection attributes = new NameValueCollection();
                    attributes["申请人类型"] = "公民";
                    if (TranslateUtils.ToBool(applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.IsOrganization)))
                    {
                        attributes["申请人类型"] = "法人/其他组织";
                    }
                    attributes["申请时间"] = DateUtils.GetDateAndTimeString(applyInfo.AddDate);
                    foreach (string key in attributes.Keys)
                    {
                        string theValue = attributes[key];

                        builder.AppendFormat(@"{0}：{1},", key, theValue);
                    }

                    if (builder.Length > 0)
                    {
                        builder.Length = builder.Length - 1;
                    }

                    if (builder.Length > 60)
                    {
                        builder.Length = 60;
                    }

                    string errorMessage = string.Empty;
                    SMSServerManager.Send(mobileArrayList, builder.ToString(), out errorMessage);
                }
            }
            catch { }
        }

        public static void SendSMS(PublishmentSystemInfo publishmentSystemInfo, ITagStyleMailSMSBaseInfo mailSMSInfo, ETableStyle tableStyle, string tableName, int relatedIdentity, ExtendedAttributes contentInfo)
        {
            try
            {
                if (mailSMSInfo.IsSMS)
                {
                    ArrayList styleInfoArrayList = RelatedIdentities.GetTableStyleInfoArrayList(publishmentSystemInfo, tableStyle, relatedIdentity);

                    ArrayList smsToArrayList = new ArrayList();
                    if (mailSMSInfo.SMSReceiver == ETriState.All || mailSMSInfo.SMSReceiver == ETriState.True)
                    {
                        if (!string.IsNullOrEmpty(mailSMSInfo.SMSTo))
                        {
                            string[] mobiles = mailSMSInfo.SMSTo.Split(';', ',');
                            foreach (string mobile in mobiles)
                            {
                                if (!string.IsNullOrEmpty(mobile) && StringUtils.IsMobile(mobile) && !smsToArrayList.Contains(mobile))
                                {
                                    smsToArrayList.Add(mobile);
                                }
                            }
                        }
                    }
                    if (mailSMSInfo.SMSReceiver == ETriState.All || mailSMSInfo.SMSReceiver == ETriState.False)
                    {
                        string smsTo = contentInfo.GetExtendedAttribute(mailSMSInfo.SMSFiledName);
                        if (!string.IsNullOrEmpty(smsTo))
                        {
                            string[] mobiles = smsTo.Split(';', ',');
                            foreach (string mobile in mobiles)
                            {
                                if (!string.IsNullOrEmpty(mobile) && StringUtils.IsMobile(mobile) && !smsToArrayList.Contains(mobile))
                                {
                                    smsToArrayList.Add(mobile);
                                }
                            }
                        }
                    }

                    StringBuilder builder = new StringBuilder();

                    if (mailSMSInfo.IsSMSTemplate && !string.IsNullOrEmpty(mailSMSInfo.SMSContent))
                    {
                        builder.Append(mailSMSInfo.SMSContent);
                    }
                    else
                    {
                        builder.Append(MessageManager.GetSMSContent(styleInfoArrayList));
                    }

                    string content = builder.ToString();
                    content = StringUtils.ReplaceIgnoreCase(content, "[AddDate]", DateUtils.GetDateAndTimeString(TranslateUtils.ToDateTime(contentInfo.GetExtendedAttribute(ContentAttribute.AddDate))));

                    foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                    {
                        string theValue = InputTypeParser.GetContentByTableStyle(contentInfo.GetExtendedAttribute(styleInfo.AttributeName), publishmentSystemInfo, tableStyle, styleInfo);
                        content = StringUtils.ReplaceIgnoreCase(content, string.Format("[{0}]", styleInfo.AttributeName), theValue);
                    }

                    ArrayList attributeNameArrayList = TableManager.GetAttributeNameArrayList(tableStyle, tableName);
                    foreach (string attributeName in attributeNameArrayList)
                    {
                        string theValue = contentInfo.GetExtendedAttribute(attributeName);
                        content = StringUtils.ReplaceIgnoreCase(content, string.Format("[{0}]", attributeName), theValue);
                    }

                    if (content.Length > 60)
                    {
                        content = content.Substring(0, 60);
                    }

                    string errorMessage = string.Empty;
                    SMSServerManager.Send(smsToArrayList, content, out errorMessage);
                }
            }
            catch { }
        }

        public static void SendMail(PublishmentSystemInfo publishmentSystemInfo, ITagStyleMailSMSBaseInfo mailSMSInfo, ETableStyle tableStyle, string tableName, int relatedIdentity, ExtendedAttributes contentInfo)
        {
            try
            {
                if (mailSMSInfo.IsMail && !string.IsNullOrEmpty(mailSMSInfo.MailTo))
                {
                    ArrayList styleInfoArrayList = RelatedIdentities.GetTableStyleInfoArrayList(publishmentSystemInfo, tableStyle, relatedIdentity);

                    ArrayList mailToArrayList = new ArrayList();
                    if (mailSMSInfo.MailReceiver == ETriState.All || mailSMSInfo.MailReceiver == ETriState.True)
                    {
                        if (!string.IsNullOrEmpty(mailSMSInfo.MailTo))
                        {
                            mailToArrayList.AddRange(TranslateUtils.StringCollectionToArrayList(mailSMSInfo.MailTo, ';'));
                        }
                    }
                    if (mailSMSInfo.MailReceiver == ETriState.All || mailSMSInfo.MailReceiver == ETriState.False)
                    {
                        string mailTo = contentInfo.GetExtendedAttribute(mailSMSInfo.MailFiledName);
                        if (!string.IsNullOrEmpty(mailTo))
                        {
                            mailToArrayList.AddRange(TranslateUtils.StringCollectionToArrayList(mailTo, ';'));
                        }
                    }
                    StringBuilder builder = new StringBuilder();

                    if (mailSMSInfo.IsMailTemplate && !string.IsNullOrEmpty(mailSMSInfo.MailContent))
                    {
                        builder.Append(mailSMSInfo.MailContent);
                    }
                    else
                    {
                        builder.Append(MessageManager.GetMailContent(styleInfoArrayList));
                    }

                    string content = builder.ToString();
                    content = StringUtils.ReplaceIgnoreCase(content, "[AddDate]", DateUtils.GetDateAndTimeString(TranslateUtils.ToDateTime(contentInfo.GetExtendedAttribute(ContentAttribute.AddDate))));

                    foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                    {
                        string theValue = InputTypeParser.GetContentByTableStyle(contentInfo.GetExtendedAttribute(styleInfo.AttributeName), publishmentSystemInfo, tableStyle, styleInfo);
                        content = StringUtils.ReplaceIgnoreCase(content, string.Format("[{0}]", styleInfo.AttributeName), theValue);
                    }

                    ArrayList attributeNameArrayList = TableManager.GetAttributeNameArrayList(tableStyle, tableName);
                    foreach (string attributeName in attributeNameArrayList)
                    {
                        string theValue = contentInfo.GetExtendedAttribute(attributeName);
                        content = StringUtils.ReplaceIgnoreCase(content, string.Format("[{0}]", attributeName), theValue);
                    }

                    string errorMessage;
                    UserMailManager.SendMail(TranslateUtils.ObjectCollectionToString(mailToArrayList), mailSMSInfo.MailTitle, content, out errorMessage);
                }
            }
            catch { }
        }

        public static string GetFormattedSendMailString(string str, PublishmentSystemInfo publishmentSystemInfo, ContentInfo contentInfo, string receiver, string sender, string pageTitle, string pageUrl)
        {
            string retval = string.Empty;
            if (!string.IsNullOrEmpty(str))
            {
                retval = str;
                retval = StringUtils.ReplaceIgnoreCase(retval, "{receiver}", receiver);
                retval = StringUtils.ReplaceIgnoreCase(retval, "{sender}", sender);
                retval = StringUtils.ReplaceIgnoreCase(retval, "{time}", DateUtils.GetDateAndTimeString(DateTime.Now));
                retval = StringUtils.ReplaceIgnoreCase(retval, "{sitename}", publishmentSystemInfo.PublishmentSystemName);
                retval = StringUtils.ReplaceIgnoreCase(retval, "{siteurl}", publishmentSystemInfo.PublishmentSystemUrl);
                retval = StringUtils.ReplaceIgnoreCase(retval, "{pageurl}", pageUrl);

                if (contentInfo != null)
                {
                    retval = StringUtils.ReplaceIgnoreCase(retval, "{title}", contentInfo.Title);
                    retval = StringUtils.ReplaceIgnoreCase(retval, "{content}", contentInfo.GetExtendedAttribute(BackgroundContentAttribute.Content));
                }
                else
                {
                    retval = StringUtils.ReplaceIgnoreCase(retval, "{title}", pageTitle);
                    if (StringUtils.ContainsIgnoreCase(retval, "{content}"))
                    {
                        retval = StringUtils.ReplaceIgnoreCase(retval, "{content}", WebClientUtils.GetRemoteFileSource(pageUrl, ECharsetUtils.GetEnumType(publishmentSystemInfo.Additional.Charset)));
                    }
                }
            }
            return retval;
        }

        public static string GetFormattedMailSubscribeString(string str, PublishmentSystemInfo publishmentSystemInfo, string receiver)
        {
            string retval = string.Empty;
            if (!string.IsNullOrEmpty(str))
            {
                retval = str;
                retval = StringUtils.ReplaceIgnoreCase(retval, "{receiver}", receiver);
                retval = StringUtils.ReplaceIgnoreCase(retval, "{time}", DateUtils.GetDateAndTimeString(DateTime.Now));
                retval = StringUtils.ReplaceIgnoreCase(retval, "{sitename}", publishmentSystemInfo.PublishmentSystemName);
                retval = StringUtils.ReplaceIgnoreCase(retval, "{siteurl}", publishmentSystemInfo.PublishmentSystemUrl);
                retval = StringUtils.ReplaceIgnoreCase(retval, "{unsubscribeurl}", string.Empty);
            }
            return retval;
        }
    }
}
