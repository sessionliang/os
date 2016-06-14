using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections;
using SiteServer.CMS.Model;
using System.Collections.Specialized;

using System.Web;
using BaiRong.Core.AuxiliaryTable;

using BaiRong.Core.Cryptography;
using SiteServer.CMS.Core;
using SiteServer.STL.Parser;
using SiteServer.STL.IO;
using SiteServer.STL.StlTemplate;

namespace SiteServer.CMS.Services
{
    public class Action : BasePage
    {
        public Literal ltlScript;

        public void Page_Load(object sender, System.EventArgs e)
        {
            if ((base.Page.Request.Form != null && base.Page.Request.Form.Count > 0) || (base.Request.Files != null && base.Request.Files.Count > 0))
            {
                bool isCrossDomain = PageUtility.IsCrossDomain(base.PublishmentSystemInfo);
                string type = base.Request.QueryString["type"];

                if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_Input))
                {
                    int inputID = TranslateUtils.ToInt(base.Request.QueryString["inputID"]);
                    this.InputAdd(isCrossDomain, inputID);
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_WebsiteMessage))
                {
                    int websiteMessageID = TranslateUtils.ToInt(base.Request.QueryString["websiteMessageID"]);
                    this.WebsiteMessageAdd(isCrossDomain, websiteMessageID);
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_Content))
                {
                    int styleID = TranslateUtils.ToInt(base.Request.QueryString["styleID"]);
                    int channelID = TranslateUtils.ToInt(base.Request.QueryString["channelID"]);
                    this.ContentInputAdd(isCrossDomain, styleID, channelID);
                }
                //else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_Comment))
                //{
                //    int styleID = TranslateUtils.ToInt(base.Request.QueryString["styleID"]);
                //    int channelID = TranslateUtils.ToInt(base.Request.QueryString["channelID"]);
                //    int contentID = TranslateUtils.ToInt(base.Request.QueryString["contentID"]);
                //    this.CommentInputAdd(isCrossDomain, styleID, channelID, contentID);
                //}
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_Login))
                {
                    int styleID = TranslateUtils.ToInt(base.Request.QueryString["styleID"]);
                    string userFrom = base.Request.Form["UserFrom"];
                    string userName = base.Request.Form["UserName"];
                    string password = base.Request.Form["Password"];

                    this.LoginInputAdd(isCrossDomain, styleID, userFrom, userName, password);
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_Register))
                {
                    int styleID = TranslateUtils.ToInt(base.Request.QueryString["styleID"]);
                    this.RegisterInputAdd(isCrossDomain, styleID);
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_Resume))
                {
                    int styleID = TranslateUtils.ToInt(base.Request.QueryString["styleID"]);
                    this.ResumeAdd(isCrossDomain, styleID);
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_GovPublicApply))
                {
                    int styleID = TranslateUtils.ToInt(base.Request.QueryString["styleID"]);
                    this.GovPublicApplyAdd(isCrossDomain, styleID);
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_GovPublicQuery))
                {
                    int styleID = TranslateUtils.ToInt(base.Request.QueryString["styleID"]);
                    this.GovPublicQueryAdd(isCrossDomain, styleID);
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_GovInteractApply))
                {
                    int nodeID = TranslateUtils.ToInt(base.Request.QueryString["nodeID"]);
                    int styleID = TranslateUtils.ToInt(base.Request.QueryString["styleID"]);
                    this.GovInteractApplyAdd(isCrossDomain, nodeID, styleID);
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_GovInteractQuery))
                {
                    int nodeID = TranslateUtils.ToInt(base.Request.QueryString["nodeID"]);
                    int styleID = TranslateUtils.ToInt(base.Request.QueryString["styleID"]);
                    this.GovInteractQueryAdd(isCrossDomain, nodeID, styleID);
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_Vote))
                {
                    int nodeID = TranslateUtils.ToInt(base.Request.QueryString["nodeID"]);
                    int contentID = TranslateUtils.ToInt(base.Request.QueryString["contentID"]);
                    this.VoteAdd(isCrossDomain, nodeID, contentID);
                }
                #region  by 20151127 sofuny   信息订阅
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_SubscribeApply))
                {
                    this.SaveSubscribeUser(isCrossDomain);
                }
                #endregion
            }
        }

        private void InputAdd(bool isCrossDomain, int inputID)
        {
            InputInfo inputInfo = null;
            if (inputID > 0)
            {
                inputInfo = DataProvider.InputDAO.GetInputInfo(inputID);
            }
            if (inputInfo != null)
            {
                bool isValidateCode = inputInfo.Additional.IsValidateCode;
                if (isValidateCode)
                {
                    isValidateCode = FileConfigManager.Instance.IsValidateCode;
                }

                ArrayList relatedIdentities = RelatedIdentities.GetRelatedIdentities(ETableStyle.InputContent, base.PublishmentSystemID, inputInfo.InputID);

                string ipAddress = PageUtils.GetIPAddress();
                string location = BaiRongDataProvider.IP2CityDAO.GetCity(ipAddress);

                InputContentInfo contentInfo = new InputContentInfo(0, inputInfo.InputID, 0, inputInfo.IsChecked, BaiRongDataProvider.UserDAO.CurrentUserName, ipAddress, location, DateTime.Now, string.Empty);

                try
                {
                    if (isValidateCode)
                    {
                        if (base.Request.Form[ValidateCodeManager.AttributeName] != null)
                        {
                            VCManager vcManager = ValidateCodeManager.GetInstance(base.PublishmentSystemID, inputInfo.InputID, isCrossDomain);

                            if (!vcManager.IsCodeValid(base.Request.Form[ValidateCodeManager.AttributeName]))
                            {
                                throw new Exception("验证码不正确!");
                            }
                        }
                    }

                    if (!inputInfo.Additional.IsAnomynous && BaiRongDataProvider.UserDAO.IsAnonymous)
                    {
                        throw new Exception("请先登录系统!");
                    }


                    InputTypeParser.AddValuesToAttributes(ETableStyle.InputContent, DataProvider.InputContentDAO.TableName, base.PublishmentSystemInfo, relatedIdentities, base.Request.Form, contentInfo.Attributes, false);

                    if (base.Request.Files != null && base.Request.Files.Count > 0)
                    {
                        foreach (string attributeName in base.Request.Files.AllKeys)
                        {
                            HttpPostedFile myFile = base.Request.Files[attributeName];
                            if (myFile != null && "" != myFile.FileName)
                            {
                                string fileUrl = this.UploadFile(myFile);
                                contentInfo.SetExtendedAttribute(attributeName, fileUrl);
                            }
                        }
                    }

                    #region 是否校验重复数据
                    if (inputInfo.Additional.IsUnique)
                    {
                        if (!string.IsNullOrEmpty(inputInfo.Additional.UniquePro) && DataProvider.InputContentDAO.IsExistsPro(inputInfo.InputID, inputInfo.Additional.UniquePro, contentInfo.GetExtendedAttribute(inputInfo.Additional.UniquePro)))
                            throw new Exception("请不要重复提交!");
                    }
                    #endregion

                    DataProvider.InputContentDAO.Insert(contentInfo);

                    MessageManager.SendMailByInput(base.PublishmentSystemInfo, inputInfo, relatedIdentities, contentInfo);

                    MessageManager.SendSMSByInput(base.PublishmentSystemInfo, inputInfo, relatedIdentities, contentInfo);

                    string message = string.Empty;
                    if (string.IsNullOrEmpty(base.Request.Form["successTemplateString"]))
                    {
                        if (string.IsNullOrEmpty(inputInfo.Additional.MessageSuccess))
                        {
                            message = "表单提交成功，正在审核。";
                            if (contentInfo.IsChecked)
                            {
                                message = "表单提交成功。";
                            }
                        }
                        else
                        {
                            message = inputInfo.Additional.MessageSuccess;
                        }
                    }
                    else
                    {
                        message = RuntimeUtils.DecryptStringByTranslate(base.Request.Form["successTemplateString"]);
                    }


                    this.ltlScript.Text = InputTemplate.GetInputCallbackScript(base.PublishmentSystemInfo, inputID, true, message);

                    //if (contentInfo.IsChecked == EBoolean.True)
                    //{
                    //    FileSystemObject FSO = new FileSystemObject(base.PublishmentSystemID);
                    //    FSO.CreateImmediately(EChangedType.Add, ETemplateTypeUtils.GetEnumType(templateType), channelID, contentID, fileTemplateID);
                    //}
                }
                catch (Exception ex)
                {
                    string message = string.Empty;
                    if (string.IsNullOrEmpty(base.Request.Form["failureTemplateString"]))
                    {
                        if (string.IsNullOrEmpty(inputInfo.Additional.MessageFailure))
                        {
                            message = "表单提交失败，" + ex.Message;
                        }
                        else
                        {
                            message = inputInfo.Additional.MessageFailure;
                        }
                    }
                    else
                    {
                        message = RuntimeUtils.DecryptStringByTranslate(base.Request.Form["failureTemplateString"]);
                    }

                    this.ltlScript.Text = InputTemplate.GetInputCallbackScript(base.PublishmentSystemInfo, inputID, false, message);
                }
            }
        }

        private void WebsiteMessageAdd(bool isCrossDomain, int websiteMessageID)
        {
            WebsiteMessageInfo websiteMessageInfo = null;
            if (websiteMessageID > 0)
            {
                websiteMessageInfo = DataProvider.WebsiteMessageDAO.GetWebsiteMessageInfo(websiteMessageID);
            }
            if (websiteMessageInfo != null)
            {
                bool isValidateCode = websiteMessageInfo.Additional.IsValidateCode;
                if (isValidateCode)
                {
                    isValidateCode = FileConfigManager.Instance.IsValidateCode;
                }

                ArrayList relatedIdentities = RelatedIdentities.GetRelatedIdentities(ETableStyle.WebsiteMessageContent, base.PublishmentSystemID, websiteMessageInfo.WebsiteMessageID);

                string ipAddress = PageUtils.GetIPAddress();
                string location = BaiRongDataProvider.IP2CityDAO.GetCity(ipAddress);

                WebsiteMessageContentInfo contentInfo = new WebsiteMessageContentInfo(0, websiteMessageInfo.WebsiteMessageID, 0, websiteMessageInfo.IsChecked, BaiRongDataProvider.UserDAO.CurrentUserName, ipAddress, location, DateTime.Now, string.Empty);

                try
                {
                    if (isValidateCode)
                    {
                        if (base.Request.Form[ValidateCodeManager.AttributeName] != null)
                        {
                            VCManager vcManager = ValidateCodeManager.GetInstance(base.PublishmentSystemID, websiteMessageInfo.WebsiteMessageID, isCrossDomain);

                            if (!vcManager.IsCodeValid(base.Request.Form[ValidateCodeManager.AttributeName]))
                            {
                                throw new Exception("验证码不正确!");
                            }
                        }
                    }

                    if (!websiteMessageInfo.Additional.IsAnomynous && BaiRongDataProvider.UserDAO.IsAnonymous)
                    {
                        throw new Exception(string.Format("请先登录系统!<a href='{0}' target='_blank'>点击登录</a>", PageUtility.ParseNavigationUrl(PublishmentSystemID, websiteMessageInfo.Additional.LoginUrl)));
                    }


                    InputTypeParser.AddValuesToAttributes(ETableStyle.WebsiteMessageContent, DataProvider.WebsiteMessageContentDAO.TableName, base.PublishmentSystemInfo, relatedIdentities, base.Request.Form, contentInfo.Attributes, false);

                    //分类
                    contentInfo.ClassifyID = TranslateUtils.ToInt(base.Request.Form["WebsiteMessageClassifyID"]);
                    if (contentInfo.ClassifyID == 0)
                    {
                        contentInfo.ClassifyID = DataProvider.WebsiteMessageClassifyDAO.GetDefaultClassifyID();
                    }

                    if (base.Request.Files != null && base.Request.Files.Count > 0)
                    {
                        foreach (string attributeName in base.Request.Files.AllKeys)
                        {
                            HttpPostedFile myFile = base.Request.Files[attributeName];
                            if (myFile != null && "" != myFile.FileName)
                            {
                                string fileUrl = this.UploadFile(myFile);
                                contentInfo.SetExtendedAttribute(attributeName, fileUrl);
                            }
                        }
                    }

                    DataProvider.WebsiteMessageContentDAO.Insert(contentInfo);

                    //MessageManager.SendMailByWebsiteMessage(base.PublishmentSystemInfo, websiteMessageInfo, relatedIdentities, contentInfo);

                    //MessageManager.SendSMSByWebsiteMessage(base.PublishmentSystemInfo, websiteMessageInfo, relatedIdentities, contentInfo);

                    string message = string.Empty;
                    if (string.IsNullOrEmpty(base.Request.Form["successTemplateString"]))
                    {
                        if (string.IsNullOrEmpty(websiteMessageInfo.Additional.MessageSuccess))
                        {
                            message = "表单提交成功，正在审核。";
                            if (contentInfo.IsChecked)
                            {
                                message = "表单提交成功。";
                            }
                        }
                        else
                        {
                            message = websiteMessageInfo.Additional.MessageSuccess;
                        }
                    }
                    else
                    {
                        message = RuntimeUtils.DecryptStringByTranslate(base.Request.Form["successTemplateString"]);
                    }


                    this.ltlScript.Text = WebsiteMessageTemplate.GetWebsiteMessageCallbackScript(base.PublishmentSystemInfo, websiteMessageID, true, message);

                    //if (contentInfo.IsChecked == EBoolean.True)
                    //{
                    //    FileSystemObject FSO = new FileSystemObject(base.PublishmentSystemID);
                    //    FSO.CreateImmediately(EChangedType.Add, ETemplateTypeUtils.GetEnumType(templateType), channelID, contentID, fileTemplateID);
                    //}
                }
                catch (Exception ex)
                {
                    string message = string.Empty;
                    if (string.IsNullOrEmpty(base.Request.Form["failureTemplateString"]))
                    {
                        if (string.IsNullOrEmpty(websiteMessageInfo.Additional.MessageFailure))
                        {
                            message = "表单提交失败，" + ex.Message;
                        }
                        else
                        {
                            message = websiteMessageInfo.Additional.MessageFailure;
                        }
                    }
                    else
                    {
                        message = RuntimeUtils.DecryptStringByTranslate(base.Request.Form["failureTemplateString"]);
                    }

                    this.ltlScript.Text = WebsiteMessageTemplate.GetWebsiteMessageCallbackScript(base.PublishmentSystemInfo, websiteMessageID, false, message);
                }
            }
        }

        private void ContentInputAdd(bool isCrossDomain, int styleID, int channelID)
        {
            if (base.Request.Form[ContentAttribute.NodeID] != null)
            {
                channelID = TranslateUtils.ToInt(base.Request.Form[ContentAttribute.NodeID], channelID);
            }
            TagStyleInfo tagStyleInfo = TagStyleManager.GetTagStyleInfo(styleID);
            if (tagStyleInfo == null)
            {
                tagStyleInfo = new TagStyleInfo();
            }
            TagStyleContentInputInfo inputInfo = new TagStyleContentInputInfo(tagStyleInfo.SettingsXML);
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, channelID);
            ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeInfo);
            string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);
            if (inputInfo != null && nodeInfo != null)
            {
                bool isValidateCode = inputInfo.IsValidateCode;
                if (isValidateCode)
                {
                    isValidateCode = FileConfigManager.Instance.IsValidateCode;
                }

                ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, nodeInfo.NodeID);

                ContentInfo contentInfo = ContentUtility.GetContentInfo(tableStyle);

                contentInfo.NodeID = nodeInfo.NodeID;
                contentInfo.PublishmentSystemID = base.PublishmentSystemID;
                if (BaiRongDataProvider.AdministratorDAO.IsAuthenticated)
                {
                    contentInfo.AddUserName = AdminManager.Current.UserName;
                }
                else
                {
                    contentInfo.AddUserName = BaiRongDataProvider.UserDAO.CurrentUserName;
                }

                try
                {
                    if (isValidateCode)
                    {
                        if (base.Request.Form[ValidateCodeManager.AttributeName] != null)
                        {
                            VCManager vcManager = ValidateCodeManager.GetInstance(base.PublishmentSystemID, styleID, isCrossDomain);

                            if (!vcManager.IsCodeValid(base.Request.Form[ValidateCodeManager.AttributeName]))
                            {
                                throw new Exception("验证码不正确!");
                            }
                        }
                    }

                    if (!inputInfo.IsAnomynous && BaiRongDataProvider.UserDAO.IsAnonymous)
                    {
                        throw new Exception("请先登录系统!");
                    }

                    InputTypeParser.AddValuesToAttributes(tableStyle, tableName, base.PublishmentSystemInfo, relatedIdentities, Request.Form, contentInfo.Attributes, false);

                    if (base.Request.Files != null && base.Request.Files.Count > 0)
                    {
                        foreach (string attributeName in base.Request.Files.AllKeys)
                        {
                            HttpPostedFile myFile = base.Request.Files[attributeName];
                            if (myFile != null && "" != myFile.FileName)
                            {
                                string fileUrl = this.UploadFile(myFile);
                                contentInfo.SetExtendedAttribute(attributeName, fileUrl);
                            }
                        }
                    }

                    contentInfo.AddDate = DateTime.Now;
                    contentInfo.LastEditUserName = contentInfo.AddUserName;
                    contentInfo.LastEditDate = DateTime.Now;
                    contentInfo.IsChecked = inputInfo.IsChecked;
                    contentInfo.CheckedLevel = 0;

                    DataProvider.ContentDAO.Insert(tableName, base.PublishmentSystemInfo, contentInfo);

                    MessageManager.SendMailByContentInput(base.PublishmentSystemInfo, inputInfo, relatedIdentities, contentInfo);

                    MessageManager.SendSMSByContentInput(base.PublishmentSystemInfo, inputInfo, relatedIdentities, contentInfo);

                    string message = string.Empty;

                    if (string.IsNullOrEmpty(base.Request.Form["successTemplateString"]))
                    {
                        if (string.IsNullOrEmpty(inputInfo.MessageSuccess))
                        {
                            message = "内容添加成功，正在审核。";
                            if (contentInfo.IsChecked)
                            {
                                message = "内容添加成功。";
                            }
                        }
                        else
                        {
                            message = inputInfo.MessageSuccess;
                        }
                    }
                    else
                    {
                        message = RuntimeUtils.DecryptStringByTranslate(base.Request.Form["successTemplateString"]);
                    }

                    this.ltlScript.Text = ContentInputTemplate.GetInputCallbackScript(base.PublishmentSystemInfo, styleID, true, message);

                    if (contentInfo.IsChecked)
                    {
                        FileSystemObject FSO = new FileSystemObject(base.PublishmentSystemID);
                        //FSO.CreateImmediately(EChangedType.Add, ETemplateTypeUtils.GetEnumType(templateType), channelID, contentID, fileTemplateID);
                    }
                }
                catch (Exception ex)
                {
                    string message = string.Empty;

                    if (string.IsNullOrEmpty(base.Request.Form["failureTemplateString"]))
                    {
                        if (string.IsNullOrEmpty(inputInfo.MessageFailure))
                        {
                            message = "内容添加失败，" + ex.Message;
                        }
                        else
                        {
                            message = inputInfo.MessageFailure;
                        }
                    }
                    else
                    {
                        message = RuntimeUtils.DecryptStringByTranslate(base.Request.Form["failureTemplateString"]);
                    }

                    this.ltlScript.Text = ContentInputTemplate.GetInputCallbackScript(base.PublishmentSystemInfo, styleID, false, message);
                }
            }
        }

        //private void CommentInputAdd(bool isCrossDomain, int styleID, int channelID, int contentID)
        //{
        //    TagStyleInfo tagStyleInfo = TagStyleManager.GetTagStyleInfo(styleID);
        //    if (tagStyleInfo == null)
        //    {
        //        tagStyleInfo = new TagStyleInfo();
        //    }
        //    TagStyleCommentInputInfo inputInfo = new TagStyleCommentInputInfo(tagStyleInfo.SettingsXML);
        //    NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, channelID);
        //    if (inputInfo != null && nodeInfo != null)
        //    {
        //        bool isValidateCode = inputInfo.IsValidateCode;
        //        if (isValidateCode)
        //        {
        //            isValidateCode = FileConfigManager.Instance.IsValidateCode;
        //        }

        //        ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, nodeInfo.NodeID);

        //        CommentInfo commentInfo = new CommentInfo();

        //        try
        //        {
        //            if (isValidateCode)
        //            {
        //                if (base.Request.Form[ValidateCodeManager.AttributeName] != null)
        //                {
        //                    VCManager vcManager = ValidateCodeManager.GetInstance(base.PublishmentSystemID, styleID, isCrossDomain);

        //                    if (!vcManager.IsCodeValid(base.Request.Form[ValidateCodeManager.AttributeName]))
        //                    {
        //                        throw new Exception("验证码不正确!");
        //                    }
        //                }
        //            }

        //            if (!inputInfo.IsAnomynous && BaiRongDataProvider.UserDAO.IsAnonymous)
        //            {
        //                throw new Exception("请先登录系统!");
        //            }

        //            InputTypeParser.AddValuesToAttributes(ETableStyle.Comment, DataProvider.CommentContentDAO.TableName, base.PublishmentSystemInfo, relatedIdentities, Request.Form, commentInfo.Attributes);

        //            if (base.Request.Files != null && base.Request.Files.Count > 0)
        //            {
        //                foreach (string attributeName in base.Request.Files.AllKeys)
        //                {
        //                    HttpPostedFile myFile = base.Request.Files[attributeName];
        //                    if (myFile != null && "" != myFile.FileName)
        //                    {
        //                        string fileUrl = this.UploadFile(myFile);
        //                        commentInfo.SetExtendedAttribute(attributeName, fileUrl);
        //                    }
        //                }
        //            }

        //            commentInfo.PublishmentSystemID = base.PublishmentSystemID;
        //            commentInfo.NodeID = channelID;
        //            commentInfo.ContentID = contentID;
        //            commentInfo.AddDate = DateTime.Now;
        //            commentInfo.IPAddress = PageUtils.GetIPAddress();
        //            commentInfo.Location = BaiRongDataProvider.IP2CityDAO.GetCity(commentInfo.IPAddress);
        //            commentInfo.UserName = BaiRongDataProvider.UserDAO.CurrentUserName;
        //            commentInfo.IsChecked = inputInfo.IsChecked;

        //            DataProvider.CommentContentDAO.Insert(base.PublishmentSystemID, commentInfo);

        //            MessageManager.SendMail(base.PublishmentSystemInfo, inputInfo, ETableStyle.Comment, DataProvider.CommentContentDAO.TableName, commentInfo.NodeID, commentInfo);

        //            MessageManager.SendSMS(base.PublishmentSystemInfo, inputInfo, ETableStyle.Comment, DataProvider.CommentContentDAO.TableName, commentInfo.NodeID, commentInfo);

        //            string message = string.Empty;

        //            if (string.IsNullOrEmpty(base.Request.Form["successTemplateString"]))
        //            {
        //                message = "评论添加成功，正在审核。";
        //                if (commentInfo.IsChecked)
        //                {
        //                    message = "评论添加成功。";
        //                }
        //            }
        //            else
        //            {
        //                message = RuntimeUtils.DecryptStringByTranslate(base.Request.Form["successTemplateString"]);
        //            }

        //            this.ltlScript.Text = CommentInputTemplate.GetCallbackScript(base.PublishmentSystemInfo, true, message);

        //            if (commentInfo.IsChecked && contentID > 0)
        //            {
        //                FileSystemObject FSO = new FileSystemObject(base.PublishmentSystemID);
        //                FSO.CreateImmediately(EChangedType.Edit, ETemplateType.ContentTemplate, channelID, contentID, 0);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            string message = string.Empty;

        //            if (string.IsNullOrEmpty(base.Request.Form["failureTemplateString"]))
        //            {
        //                message = ex.Message;
        //            }
        //            else
        //            {
        //                message = RuntimeUtils.DecryptStringByTranslate(base.Request.Form["failureTemplateString"]);
        //            }

        //            this.ltlScript.Text = CommentInputTemplate.GetCallbackScript(base.PublishmentSystemInfo, false, message);
        //        }
        //    }
        //}

        private void LoginInputAdd(bool isCrossDomain, int styleID, string userFrom, string userName, string password)
        {
            //TagStyleInfo tagStyleInfo = TagStyleManager.GetTagStyleInfo(styleID);
            //if (tagStyleInfo == null)
            //{
            //    tagStyleInfo = new TagStyleInfo();
            //}
            //TagStyleLoginInfo loginInfo = new TagStyleLoginInfo(tagStyleInfo.SettingsXML);
            //if (loginInfo != null)
            //{
            //    bool isValidateCode = loginInfo.IsValidateCode;
            //    if (isValidateCode)
            //    {
            //        isValidateCode = FileConfigManager.Instance.IsValidateCode;
            //    }

            //    try
            //    {
            //        if (isValidateCode)
            //        {
            //            if (base.Request.Form[ValidateCodeManager.AttributeName] != null)
            //            {
            //                VCManager vcManager = ValidateCodeManager.GetInstance(base.PublishmentSystemID, styleID, isCrossDomain);

            //                if (!vcManager.IsCodeValid(base.Request.Form[ValidateCodeManager.AttributeName]))
            //                {
            //                    throw new Exception("登录失败，验证码不正确!");
            //                }
            //            }
            //        }

            //        string errorMessage = string.Empty;
            //        if (BaiRongDataProvider.UserDAO.Validate(userName, password, out errorMessage))
            //        {
            //            BaiRongDataProvider.UserDAO.Login(userFrom, userName, true);
            //            this.ltlScript.Text = LoginTemplate.GetCallbackScript(base.PublishmentSystemInfo, styleID, true, string.Empty, userName);
            //        }
            //        else
            //        {
            //            throw new Exception("登录失败，" + errorMessage);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        this.ltlScript.Text = LoginTemplate.GetCallbackScript(base.PublishmentSystemInfo, styleID, false, ex.Message, string.Empty);
            //    }
            //}
        }

        private void RegisterInputAdd(bool isCrossDomain, int styleID)
        {
            //TagStyleInfo tagStyleInfo = TagStyleManager.GetTagStyleInfo(styleID);
            //if (tagStyleInfo == null)
            //{
            //    tagStyleInfo = new TagStyleInfo();
            //}
            //TagStyleRegisterInfo registerInfo = new TagStyleRegisterInfo(tagStyleInfo.SettingsXML);

            //bool isValidateCode = registerInfo.IsValidateCode;
            //if (isValidateCode)
            //{
            //    isValidateCode = FileConfigManager.Instance.IsValidateCode;
            //}

            //try
            //{
            //    if (isValidateCode)
            //    {
            //        if (base.Request.Form[ValidateCodeManager.AttributeName] != null)
            //        {
            //            VCManager vcManager = ValidateCodeManager.GetInstance(base.PublishmentSystemID, styleID, isCrossDomain);

            //            if (!vcManager.IsCodeValid(base.Request.Form[ValidateCodeManager.AttributeName]))
            //            {
            //                throw new Exception("验证码不正确!");
            //            }
            //        }
            //    }

            //    string userName = base.Request.Form[UserAttribute.UserName];
            //    string displayName = base.Request.Form[UserAttribute.DisplayName];
            //    string password = base.Request.Form[UserAttribute.Password];
            //    string comfirmPassword = base.Request.Form["ComfirmPassword"];
            //    string email = base.Request.Form[UserAttribute.Email];

            //    if (BaiRongDataProvider.UserDAO.IsExists(userName))
            //    {
            //        throw new Exception("用户名已被注册，请更换用户名!");
            //    }

            //    if (!BaiRongDataProvider.UserDAO.IsUserNameCompliant(userName))
            //    {
            //        throw new Exception("用户名包含不规则字符，请更换用户名!");
            //    }

            //    if (!UserConfigManager.Additional.IsEmailDuplicated && BaiRongDataProvider.UserDAO.IsEmailExists(email))
            //    {
            //        throw new Exception("电子邮件地址已被注册，请更换邮箱!");
            //    }

            //    if (password != comfirmPassword)
            //    {
            //        throw new Exception("确认密码与密码不一致!");
            //    }

            //    UserInfo userInfo = new UserInfo();
            //    ProductUserInfo productUserInfo = ProductUserInfo.Create(userName, userInfo);

            //    userInfo.UserName = userName;
            //    userInfo.DisplayName = displayName;
            //    userInfo.Password = password;
            //    if (UserConfigManager.Additional.RegisterVerifyType == EUserVerifyType.None)
            //    {
            //        userInfo.IsChecked = true;
            //    }
            //    else
            //    {
            //        userInfo.IsChecked = false;
            //    }
            //    userInfo.IsLockedOut = false;
            //    userInfo.Email = email;

            //    int typeID = registerInfo.TypeID;
            //    if (typeID > 0)
            //    {
            //        if (!UserTypeManager.IsExists(typeID))
            //        {
            //            typeID = 0;
            //        }
            //    }
            //    if (typeID == 0)
            //    {
            //        typeID = UserTypeManager.GetDefaultTypeID();
            //    }

            //    UserTypeInfo typeInfo = UserTypeManager.GetTypeInfo(typeID);

            //    if (typeInfo != null)
            //    {
            //        userInfo.TypeID = typeInfo.TypeID;

            //        ArrayList relatedIdentities = UserManager.GetRelatedIdentities(userInfo.TypeID);
            //        TableInputParser.AddValuesToAttributes(ETableStyle.User, BaiRongDataProvider.UserDAO.TableName, relatedIdentities, base.Request.Form, userInfo.Attributes);

            //        UserGroupInfo groupInfo = UserGroupManager.GetGroupInfoByCredits(0);

            //        if (groupInfo != null)
            //        {
            //            productUserInfo.GroupID = groupInfo.GroupID;
            //            productUserInfo.Credits = groupInfo.CreditsFrom;
            //        }
            //    }

            //    string errorMessage = string.Empty;
            //    string message = string.Empty;
            //    if (BaiRongDataProvider.UserDAO.Insert(userInfo, out errorMessage))
            //    {
            //        ProductUserInfo puInfo = DataProvider.ProductUserDAO.GetProductUserInfo(productUserInfo.UserName);
            //        if (puInfo != null)
            //        {
            //            DataProvider.ProductUserDAO.Delete(productUserInfo.UserName);
            //        }
            //        productUserInfo.Credits = registerInfo.Credits;
            //        productUserInfo.GroupID = registerInfo.GroupID;
            //        DataProvider.ProductUserDAO.Insert(productUserInfo);
            //        if (UserConfigManager.Additional.RegisterVerifyType == EUserVerifyType.Email)
            //        {
            //            string checkCode = EncryptUtils.Md5(userName);
            //            string mailBody = UserConfigManager.Additional.RegisterVerifyMailContent;
            //            mailBody = mailBody.Replace("[UserName]", displayName);
            //            mailBody = mailBody.Replace("[SiteUrl]", PageUtils.AddProtocolToUrl(PageUtils.GetHost()));
            //            mailBody = mailBody.Replace("[VerifyUrl]", PageUtils.AddProtocolToUrl(PageUtils.GetUserCenterUrl(string.Format("register.aspx?checkCode={0}&userName={1}", checkCode, userName))));

            //            UserMailManager.SendMail(userInfo.Email, "邮件注册确认", mailBody, out errorMessage);
            //        }

            //        if (string.IsNullOrEmpty(base.Request.Form["successTemplateString"]))
            //        {

            //            if (UserConfigManager.Additional.RegisterVerifyType == EUserVerifyType.None)
            //            {
            //                message = UserConfigManager.Additional.RegisterWelcome;
            //            }
            //            else if (UserConfigManager.Additional.RegisterVerifyType == EUserVerifyType.Email)
            //            {
            //                string emailUrl = "http://mail." + userInfo.Email.Substring(userInfo.Email.IndexOf('@') + 1);
            //                message = string.Format(@"注册激活邮件已发送到您的邮箱<a href=""{0}"" target=""_blank"">{1}</a>，点击进入邮箱激活。", emailUrl, userInfo.Email);
            //            }
            //            else if (UserConfigManager.Additional.RegisterVerifyType == EUserVerifyType.Manually)
            //            {
            //                message = "注册已提交，请等待审核，谢谢。";
            //            }
            //        }
            //        else
            //        {
            //            message = RuntimeUtils.DecryptStringByTranslate(base.Request.Form["successTemplateString"]);
            //        }

            //        //替换{User.UserName}等
            //        message = StlEntityParser.ReplaceStlUserEntities(message, userInfo);
            //    }
            //    else
            //    {
            //        throw new Exception(errorMessage);
            //    }

            //    this.ltlScript.Text = RegisterTemplate.GetInputCallbackScript(base.PublishmentSystemInfo, styleID, true, message);
            //}
            //catch (Exception ex)
            //{
            //    string message = string.Empty;
            //    if (string.IsNullOrEmpty(base.Request.Form["failureTemplateString"]))
            //    {
            //        message = "注册失败，" + ex.Message;
            //    }
            //    else
            //    {
            //        message = RuntimeUtils.DecryptStringByTranslate(base.Request.Form["failureTemplateString"]);
            //    }

            //    this.ltlScript.Text = RegisterTemplate.GetInputCallbackScript(base.PublishmentSystemInfo, styleID, false, message);
            //}
        }

        private void ResumeAdd(bool isCrossDomain, int styleID)
        {
            TagStyleInfo tagStyleInfo = TagStyleManager.GetTagStyleInfo(styleID);
            if (tagStyleInfo == null)
            {
                tagStyleInfo = new TagStyleInfo();
            }
            TagStyleResumeInfo resumeInfo = new TagStyleResumeInfo(tagStyleInfo.SettingsXML);

            try
            {
                if (!resumeInfo.IsAnomynous && BaiRongDataProvider.UserDAO.IsAnonymous)
                {
                    throw new Exception("请先登录系统!");
                }

                ResumeContentInfo contentInfo = DataProvider.ResumeContentDAO.GetContentInfo(base.PublishmentSystemID, styleID, base.Request.Form);

                DataProvider.ResumeContentDAO.Insert(contentInfo);

                MessageManager.SendMailByResumeInput(base.PublishmentSystemInfo, resumeInfo, contentInfo);

                MessageManager.SendSMSByResumeInput(base.PublishmentSystemInfo, resumeInfo, contentInfo);

                string message = string.Empty;

                if (string.IsNullOrEmpty(base.Request.Form["successTemplateString"]))
                {
                    if (string.IsNullOrEmpty(resumeInfo.MessageSuccess))
                    {
                        message = "简历添加成功。";
                    }
                    else
                    {
                        message = resumeInfo.MessageSuccess;
                    }
                }
                else
                {
                    message = RuntimeUtils.DecryptStringByTranslate(base.Request.Form["successTemplateString"]);
                }

                this.ltlScript.Text = ResumeTemplate.GetCallbackScript(base.PublishmentSystemInfo, true, message);
            }
            catch (Exception ex)
            {
                string message = string.Empty;

                if (string.IsNullOrEmpty(base.Request.Form["failureTemplateString"]))
                {
                    if (string.IsNullOrEmpty(resumeInfo.MessageFailure))
                    {
                        message = "简历添加失败，" + ex.Message;
                    }
                    else
                    {
                        message = resumeInfo.MessageFailure;
                    }
                }
                else
                {
                    message = RuntimeUtils.DecryptStringByTranslate(base.Request.Form["failureTemplateString"]);
                }

                this.ltlScript.Text = ResumeTemplate.GetCallbackScript(base.PublishmentSystemInfo, false, message);
            }
        }

        private void GovPublicApplyAdd(bool isCrossDomain, int styleID)
        {
            TagStyleInfo tagStyleInfo = TagStyleManager.GetTagStyleInfo(styleID);
            if (tagStyleInfo == null)
            {
                tagStyleInfo = new TagStyleInfo();
            }
            TagStyleGovPublicApplyInfo tagStyleGovPublicApplyInfo = new TagStyleGovPublicApplyInfo(tagStyleInfo.SettingsXML);

            try
            {
                GovPublicApplyInfo applyInfo = DataProvider.GovPublicApplyDAO.GetApplyInfo(base.PublishmentSystemID, styleID, base.Request.Form);

                int applyID = DataProvider.GovPublicApplyDAO.Insert(applyInfo);

                string fromName = applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.CivicName);
                if (applyInfo.IsOrganization)
                {
                    fromName = applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.OrgName);
                }
                string toDepartmentName = string.Empty;
                if (applyInfo.DepartmentID > 0)
                {
                    toDepartmentName = "至" + applyInfo.DepartmentName;
                }
                GovPublicApplyManager.LogNew(base.PublishmentSystemID, applyID, fromName, toDepartmentName);

                MessageManager.SendMailByGovPublicApply(base.PublishmentSystemInfo, tagStyleGovPublicApplyInfo, applyInfo);

                MessageManager.SendSMSByGovPublicApply(base.PublishmentSystemInfo, tagStyleGovPublicApplyInfo, applyInfo);

                this.ltlScript.Text = GovPublicApplyTemplate.GetCallbackScript(base.PublishmentSystemInfo, true, applyInfo.QueryCode, string.Empty);
            }
            catch (Exception ex)
            {
                this.ltlScript.Text = GovPublicApplyTemplate.GetCallbackScript(base.PublishmentSystemInfo, false, string.Empty, ex.Message);
            }
        }

        private void GovPublicQueryAdd(bool isCrossDomain, int styleID)
        {
            try
            {
                bool isOrganization = TranslateUtils.ToBool(base.Request.Form[GovPublicApplyAttribute.IsOrganization]);
                string queryName = base.Request.Form["queryName"];
                string queryCode = base.Request.Form[GovPublicApplyAttribute.QueryCode];
                GovPublicApplyInfo applyInfo = DataProvider.GovPublicApplyDAO.GetApplyInfo(base.PublishmentSystemID, isOrganization, queryName, queryCode);
                if (applyInfo != null)
                {
                    this.ltlScript.Text = GovPublicQueryTemplate.GetCallbackScript(base.PublishmentSystemInfo, true, applyInfo, string.Empty);
                }
                else
                {
                    this.ltlScript.Text = GovPublicQueryTemplate.GetCallbackScript(base.PublishmentSystemInfo, false, null, "系统找不到对应的申请，请确认您的输入值是否正确");
                }
            }
            catch (Exception ex)
            {
                this.ltlScript.Text = GovPublicQueryTemplate.GetCallbackScript(base.PublishmentSystemInfo, false, null, ex.Message);
            }
        }

        private void GovInteractApplyAdd(bool isCrossDomain, int nodeID, int styleID)
        {
            TagStyleInfo tagStyleInfo = TagStyleManager.GetTagStyleInfo(styleID);
            if (tagStyleInfo == null)
            {
                tagStyleInfo = new TagStyleInfo();
            }
            TagStyleGovInteractApplyInfo tagStyleGovInteractApplyInfo = new TagStyleGovInteractApplyInfo(tagStyleInfo.SettingsXML);

            try
            {
                bool isValidateCode = tagStyleGovInteractApplyInfo.IsValidateCode;
                if (isValidateCode)
                {
                    isValidateCode = FileConfigManager.Instance.IsValidateCode;
                }

                if (isValidateCode)
                {
                    if (base.Request.Form[ValidateCodeManager.AttributeName] != null)
                    {
                        VCManager vcManager = ValidateCodeManager.GetInstance(base.PublishmentSystemID, styleID, isCrossDomain);

                        if (!vcManager.IsCodeValid(base.Request.Form[ValidateCodeManager.AttributeName]))
                        {
                            throw new Exception("验证码不正确!");
                        }
                    }
                }

                if (!tagStyleGovInteractApplyInfo.IsAnomynous && BaiRongDataProvider.UserDAO.IsAnonymous)
                {
                    throw new Exception("请先登录系统!");
                }

                GovInteractContentInfo contentInfo = DataProvider.GovInteractContentDAO.GetContentInfo(base.PublishmentSystemInfo, nodeID, base.Request.Form);

                if (base.Request.Files != null && base.Request.Files.Count > 0)
                {
                    foreach (string attributeName in base.Request.Files.AllKeys)
                    {
                        HttpPostedFile myFile = base.Request.Files[attributeName];
                        if (myFile != null && "" != myFile.FileName)
                        {
                            string fileUrl = string.Empty;
                            fileUrl = this.UploadFile(myFile);
                            contentInfo.SetExtendedAttribute(attributeName, fileUrl);
                        }
                    }
                }

                int contentID = DataProvider.ContentDAO.Insert(base.PublishmentSystemInfo.AuxiliaryTableForGovInteract, base.PublishmentSystemInfo, contentInfo);

                string realName = contentInfo.RealName;
                string toDepartmentName = string.Empty;
                if (contentInfo.DepartmentID > 0)
                {
                    toDepartmentName = "至" + contentInfo.DepartmentName;
                }
                GovInteractApplyManager.LogNew(base.PublishmentSystemID, nodeID, contentID, realName, toDepartmentName);

                MessageManager.SendMail(base.PublishmentSystemInfo, tagStyleGovInteractApplyInfo, ETableStyle.GovInteractContent, base.PublishmentSystemInfo.AuxiliaryTableForGovInteract, nodeID, contentInfo);

                MessageManager.SendSMS(base.PublishmentSystemInfo, tagStyleGovInteractApplyInfo, ETableStyle.GovInteractContent, base.PublishmentSystemInfo.AuxiliaryTableForGovInteract, nodeID, contentInfo);

                this.ltlScript.Text = GovInteractApplyTemplate.GetCallbackScript(base.PublishmentSystemInfo, nodeID, true, contentInfo.QueryCode, string.Empty);
            }
            catch (Exception ex)
            {
                this.ltlScript.Text = GovInteractApplyTemplate.GetCallbackScript(base.PublishmentSystemInfo, nodeID, false, string.Empty, ex.Message);
            }
        }

        private void GovInteractQueryAdd(bool isCrossDomain, int nodeID, int styleID)
        {
            try
            {
                string queryCode = base.Request.Form[GovInteractContentAttribute.QueryCode];
                GovInteractContentInfo contentInfo = DataProvider.GovInteractContentDAO.GetContentInfo(base.PublishmentSystemInfo, nodeID, queryCode);
                if (contentInfo != null)
                {
                    this.ltlScript.Text = GovInteractQueryTemplate.GetCallbackScript(base.PublishmentSystemInfo, true, contentInfo, string.Empty);
                }
                else
                {
                    this.ltlScript.Text = GovInteractQueryTemplate.GetCallbackScript(base.PublishmentSystemInfo, false, null, "您输入的查询号不正确");
                }
            }
            catch (Exception ex)
            {
                this.ltlScript.Text = GovInteractQueryTemplate.GetCallbackScript(base.PublishmentSystemInfo, false, null, ex.Message);
            }
        }

        private void VoteAdd(bool isCrossDomain, int nodeID, int contentID)
        {
            try
            {
                VoteContentInfo contentInfo = DataProvider.VoteContentDAO.GetContentInfo(base.PublishmentSystemInfo, contentID);
                if ((contentInfo.EndDate - DateTime.Now).Seconds <= 0)
                {
                    throw new Exception("对不起，投票已经结束");
                }
                string cookieName = DataProvider.VoteOperationDAO.GetCookieName(this.PublishmentSystemID, nodeID, contentID);
                if (CookieUtils.IsExists(cookieName))
                {
                    throw new Exception("对不起，不能重复投票");
                }

                ArrayList optionIDArrayList = TranslateUtils.StringCollectionToIntArrayList(base.Request.Form["voteOption_" + contentID]);
                foreach (int optionID in optionIDArrayList)
                {
                    DataProvider.VoteOptionDAO.AddVoteNum(optionID);
                }
                DataProvider.VoteOperationDAO.Insert(new VoteOperationInfo(0, this.PublishmentSystemID, nodeID, contentID, PageUtils.GetIPAddress(), UserManager.Current.UserName, DateTime.Now));

                this.ltlScript.Text = VoteTemplate.GetCallbackScript(base.PublishmentSystemInfo, nodeID, contentID, true, string.Empty);
                CookieUtils.SetCookie(cookieName, true.ToString(), DateTime.MaxValue);
            }
            catch (Exception ex)
            {
                this.ltlScript.Text = VoteTemplate.GetCallbackScript(base.PublishmentSystemInfo, nodeID, contentID, false, ex.Message);
            }
        }

        private string UploadFile(HttpPostedFile myFile)
        {
            string fileUrl = string.Empty;

            string filePath = myFile.FileName;
            try
            {
                string fileExtName = PathUtils.GetExtension(filePath);
                string localDirectoryPath = PathUtility.GetUploadDirectoryPath(base.PublishmentSystemInfo, fileExtName);
                string localFileName = PathUtility.GetUploadFileName(base.PublishmentSystemInfo, filePath);

                string localFilePath = PathUtils.Combine(localDirectoryPath, localFileName);

                if (!PathUtility.IsFileExtenstionAllowed(base.PublishmentSystemInfo, fileExtName))
                {
                    return string.Empty;
                }
                if (!PathUtility.IsFileSizeAllowed(base.PublishmentSystemInfo, myFile.ContentLength))
                {
                    return string.Empty;
                }

                myFile.SaveAs(localFilePath);
                FileUtility.AddWaterMark(base.PublishmentSystemInfo, localFilePath);

                fileUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, localFilePath);
                fileUrl = PageUtility.GetVirtualUrl(base.PublishmentSystemInfo, fileUrl);
            }
            catch { }

            return fileUrl;
        }

        #region by  20151127 sofuny 培生

        public void SaveSubscribeUser(bool isCrossDomain)
        {
            try
            {
                string email = RequestUtils.GetRequestStringNoSqlAndXss("Email");
                string mobile = RequestUtils.GetRequestStringNoSqlAndXss("Mobile") == "undefined" ? "" : RequestUtils.GetRequestStringNoSqlAndXss("Mobile");
                string subscribeID = RequestUtils.GetRequestStringNoSqlAndXss("SubscribeID");
                if (RequestUtils.GetRequestStringNoSqlAndXss("Email") == null || RequestUtils.GetRequestStringNoSqlAndXss("Email") == "undefined")
                {
                    throw new Exception("请输入正确的邮箱!");
                }
                if (RequestUtils.GetRequestStringNoSqlAndXss("SubscribeID") == null || RequestUtils.GetRequestStringNoSqlAndXss("SubscribeID") == "undefined")
                {
                    throw new Exception("请选择订阅内容!");
                }
                UserInfo info = RequestUtils.Current;
                int userID = 0;
                if (info != null)
                {
                    userID = info.UserID;
                }

                if (DataProvider.SubscribeUserDAO.IsExists(email))
                {
                    SubscribeUserInfo subscribeUserInfo = DataProvider.SubscribeUserDAO.GetContentInfo(PublishmentSystemInfo.PublishmentSystemID, email);
                    string olduname = subscribeUserInfo.SubscribeName;
                    subscribeUserInfo.Mobile = mobile;
                    subscribeUserInfo.UserID = userID;
                    subscribeUserInfo.SubscribeStatu = EBoolean.True;
                    subscribeUserInfo.SubscribeName = subscribeID;
                    DataProvider.SubscribeUserDAO.Update(subscribeUserInfo, olduname);
                    string message = "您的邮箱订阅成功！";

                    this.ltlScript.Text = SubscribeTemplate.GetSubscribeCallbackScript(base.PublishmentSystemInfo, false, message);
                }
                else
                {
                    SubscribeUserInfo subscribeUserInfo = new SubscribeUserInfo();
                    subscribeUserInfo.Email = email;
                    subscribeUserInfo.Mobile = mobile;
                    subscribeUserInfo.UserID = userID;
                    subscribeUserInfo.SubscribeStatu = EBoolean.True;
                    subscribeUserInfo.SubscribeName = subscribeID;
                    subscribeUserInfo.PublishmentSystemID = PublishmentSystemInfo.PublishmentSystemID;
                    DataProvider.SubscribeUserDAO.Insert(subscribeUserInfo);
                    DataProvider.SubscribeDAO.UpdateSubscribeNum(PublishmentSystemInfo.PublishmentSystemID, subscribeID, 1);
                    sendText(PublishmentSystemInfo, subscribeUserInfo);

                    //统计顶级的数量
                    SubscribeInfo sinfo = DataProvider.SubscribeDAO.GetDefaultInfo(PublishmentSystemInfo.PublishmentSystemID);
                    DataProvider.SubscribeDAO.UpdateContentNum(PublishmentSystemInfo.PublishmentSystemID, sinfo.ItemID, DataProvider.SubscribeUserDAO.GetCount(PublishmentSystemInfo.PublishmentSystemID, string.Empty));
                    string message = "您的邮箱订阅成功！";
                    this.ltlScript.Text = SubscribeTemplate.GetSubscribeCallbackScript(base.PublishmentSystemInfo, false, message);

                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = "您的邮箱订阅失败：" + ex.Message;
                this.ltlScript.Text = SubscribeTemplate.GetSubscribeCallbackScript(base.PublishmentSystemInfo, false, ErrorMessage);
            }
        }


        public void sendText(PublishmentSystemInfo publishmentSystemInfo, SubscribeUserInfo info)
        {

            string strBody = "【" + publishmentSystemInfo.PublishmentSystemName + "】" + "您的信息订阅成功";
            bool isSuccess = true;
            //发送订阅成功邮件
            try
            {
                BaiRong.Core.Net.ISmtpMail smtpMail = BaiRong.Core.Net.MailUtils.GetInstance();
                string[] usernames = info.Email.Split(new char[] { ',' });
                smtpMail.AddRecipient(usernames);

                smtpMail.MailDomainPort = ConfigManager.Additional.MailDomainPort;
                smtpMail.IsHtml = true;
                smtpMail.Subject = "【" + publishmentSystemInfo.PublishmentSystemName + "】" + "信息订阅";


                smtpMail.Body = "<pre style=\"width:100%;word-wrap:break-word\">" + strBody + "</pre>";
                smtpMail.MailDomain = ConfigManager.Additional.MailDomain;
                smtpMail.MailServerUserName = ConfigManager.Additional.MailServerUserName;
                smtpMail.MailServerPassword = ConfigManager.Additional.MailServerPassword;

                //开始发送
                string errorMessage = string.Empty;
                isSuccess = smtpMail.Send(out errorMessage);
                if (isSuccess)
                {
                    StringUtility.AddLog(publishmentSystemInfo.PublishmentSystemID, "订阅信息成功发送邮件:", string.Format("接收邮件:{0},邮件内容：{1}", info.Email, strBody));
                }
            }
            catch (Exception ex)
            {
            }

            //记录邮件发送状态
            SubscribePushRecordInfo srinfo = new SubscribePushRecordInfo()
            {
                Email = info.Email,
                PushType = ESubscribePushType.ManualPush,
                SubscribeName = DataProvider.SubscribeDAO.GetName(publishmentSystemInfo.PublishmentSystemID, info.SubscribeName),
                SubscriptionTemplate = "订阅信息成功发送邮件",
                PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID,
                PushStatu = isSuccess ? EBoolean.True : EBoolean.False,
                UserName = BaiRongDataProvider.AdministratorDAO.UserName
            };
            DataProvider.SubscribePushRecordDAO.Insert(srinfo);


            //发送手机提醒
            if (!string.IsNullOrEmpty(info.Mobile))
            {
                ArrayList mobileArrayList = new ArrayList();
                mobileArrayList.Add(info.Mobile);

                if (mobileArrayList.Count > 0)
                {
                    try
                    {
                        string errorMessage = string.Empty;
                        isSuccess = SMSServerManager.Send(mobileArrayList, strBody, out errorMessage);

                        if (isSuccess)
                        {
                            StringUtility.AddLog(publishmentSystemInfo.PublishmentSystemID, "订阅信息发送短信", string.Format("接收号码:{0},短信内容：{1}", TranslateUtils.ObjectCollectionToString(mobileArrayList), strBody));

                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }

                //记录邮件发送状态
                srinfo = new SubscribePushRecordInfo()
                {
                    Mobile = info.Mobile,
                    PushType = ESubscribePushType.ManualPush,
                    SubscribeName = DataProvider.SubscribeDAO.GetName(publishmentSystemInfo.PublishmentSystemID, info.SubscribeName),
                    SubscriptionTemplate = "订阅信息成功发送手机提醒",
                    PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID,
                    PushStatu = isSuccess ? EBoolean.True : EBoolean.False,
                    UserName = BaiRongDataProvider.AdministratorDAO.UserName
                };
                DataProvider.SubscribePushRecordDAO.Insert(srinfo);
            }
        }

        #endregion
    }
}
