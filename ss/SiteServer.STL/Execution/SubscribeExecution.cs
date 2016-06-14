using System;
using System.Collections;
using System.Text;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using BaiRong.Service;
using BaiRong.Service.Execution;
using BaiRong.Model.Service;
using SiteServer.STL.IO;
using BaiRong.Core.Net;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser;
using System.Collections.Specialized;

namespace SiteServer.STL.Execution
{
    /// <summary>
    /// ������Ϣ����
    /// </summary>
    public class SubscribeExecution : ExecutionBase, IExecution
    {
        public bool Execute(TaskInfo taskInfo)
        {
            base.Init();

            TaskSubscribeInfo taskCreateInfo = new TaskSubscribeInfo(taskInfo.ServiceParameters);
            if (!string.IsNullOrEmpty(taskCreateInfo.CreateTypes))
            {
                ArrayList createTypeArrayList = TranslateUtils.StringCollectionToArrayList(taskCreateInfo.CreateTypes);
                bool createChannel = createTypeArrayList.Contains(ECreateTypeUtils.GetValue(ECreateType.Channel));
                bool createContent = createTypeArrayList.Contains(ECreateTypeUtils.GetValue(ECreateType.Content));
                bool createFile = createTypeArrayList.Contains(ECreateTypeUtils.GetValue(ECreateType.File));
                if (taskInfo.PublishmentSystemID != 0)
                {
                    SubscribeExecution.Subscribe(taskInfo, taskInfo.PublishmentSystemID);
                }
            }

            return true;
        }

        private static void Subscribe(TaskInfo taskInfo, int publishmentSystemID)
        {
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            if (publishmentSystemInfo != null)
            {
                SortedList errorEmailSortedList = new SortedList();
                SortedList errorMobileSortedList = new SortedList();
                SortedList errorFileTemplateIDSortedList = new SortedList();

                //��ȡ��������
                SubscribeSetInfo sinfo = DataProvider.SubscribeSetDAO.GetSubscribeSetInfo(publishmentSystemID);


                //��ȡ���Ļ�Ա�б�(��������)
                ArrayList arrlist = DataProvider.SubscribeUserDAO.GetSubscribeUserList(publishmentSystemID, EBoolean.True);

                foreach (SubscribeUserInfo info in arrlist)
                {
                    if (!string.IsNullOrEmpty(info.SubscribeName))
                    {
                        #region �����ʼ�
                        ISmtpMail smtpMail = MailUtils.GetInstance();
                        smtpMail.AddRecipient(info.Email);

                        smtpMail.MailDomainPort = ConfigManager.Additional.MailDomainPort;
                        smtpMail.IsHtml = true;
                        smtpMail.Subject = "��" + publishmentSystemInfo.PublishmentSystemName + "�� " + "��Ϣ����";

                        string cvalue = "";
                        string lvalue = "";
                        DataProvider.SubscribeDAO.GetValueByUserID(publishmentSystemID, info.SubscribeName, out cvalue, out lvalue);

                        string strBody = "";
                        //��ȡ�ʼ�ģ��ľ�̬���� 
                        try
                        {

                            // TemplateInfo templateInfo = DataProvider.TemplateDAO.GetTemplateByURLType(publishmentSystemID, ETemplateType.FileTemplate, sinfo.EmailContentAddress);
                            TemplateInfo templateInfo = DataProvider.TemplateDAO.GetTemplateByTemplateID(publishmentSystemID, ETemplateType.FileTemplate, sinfo.EmailContentAddress);
                            if (templateInfo == null || templateInfo.TemplateType != ETemplateType.FileTemplate)
                            {
                                return;
                            }
                            StringBuilder contentBuilder = new StringBuilder(CreateCacheManager.FileContent.GetTemplateContent(publishmentSystemInfo, templateInfo));

                            NameValueCollection queryString = new NameValueCollection();
                            queryString.Remove("publishmentSystemID");
                            queryString.Add("channelIndex", TranslateUtils.ObjectCollectionToSqlInStringWithQuote(TranslateUtils.StringCollectionToArrayList(cvalue)));
                            queryString.Add("tags", lvalue);

                            strBody = StlUtility.ParseDynamicContent(publishmentSystemID, 0, 0, templateInfo.TemplateID, false, contentBuilder.ToString(), sinfo.EmailContentAddress, 1, "", queryString);
                        }
                        catch (Exception ex)
                        {
                            errorFileTemplateIDSortedList.Add(info.Email, ex.Message);
                        }

                        smtpMail.Body = strBody;
                        smtpMail.MailDomain = ConfigManager.Additional.MailDomain;
                        smtpMail.MailServerUserName = ConfigManager.Additional.MailServerUserName;
                        smtpMail.MailServerPassword = ConfigManager.Additional.MailServerPassword;

                        //��ʼ����
                        string errorMessage = string.Empty;
                        bool isSuccess = smtpMail.Send(out errorMessage);
                        if (isSuccess)
                        {
                            //�޸Ļ�Ա��Ϣ���ʹ��� 
                            DataProvider.SubscribeUserDAO.UpdatePushNum(publishmentSystemID, info.SubscribeUserID);
                        }
                        else
                            errorEmailSortedList.Add(info.Email, errorMessage);

                        //��¼�ʼ�����״̬
                        SubscribePushRecordInfo srinfo = new SubscribePushRecordInfo()
                        {
                            Email = info.Email,
                            PushType = ESubscribePushType.ManualPush,
                            SubscribeName = DataProvider.SubscribeDAO.GetName(publishmentSystemID, info.SubscribeName),
                            SubscriptionTemplate = "������Ϣ�ɹ������ʼ�,ģ��",
                            PublishmentSystemID = publishmentSystemID,
                            PushStatu = isSuccess ? EBoolean.True : EBoolean.False,
                            TaskID = taskInfo.TaskID,
                            UserName = BaiRongDataProvider.AdministratorDAO.UserName
                        };
                        DataProvider.SubscribePushRecordDAO.Insert(srinfo);

                        #endregion

                        #region �����ֻ�����

                        if (!string.IsNullOrEmpty(info.Mobile))
                        {
                            //��ȡ�ֻ�ģ��ҳ�������
                            TemplateInfo templateInfo = DataProvider.TemplateDAO.GetTemplateByTemplateID(publishmentSystemID, ETemplateType.FileTemplate, sinfo.MobileContentAddress);

                            strBody = "�����ĵ������ѷ��͵�������䣬�ֻ����ʵ�ַ" + publishmentSystemInfo.PublishmentSystemUrl + "/" + templateInfo.CreatedFileFullName.TrimStart('@') + " ��" + publishmentSystemInfo.PublishmentSystemName + "��";

                            ArrayList mobileArrayList = new ArrayList();

                            mobileArrayList.Add(info.Mobile);

                            if (mobileArrayList.Count > 0)
                            {
                                isSuccess = SMSServerManager.Send(mobileArrayList, strBody, out errorMessage);

                                if (!isSuccess)
                                {
                                    errorEmailSortedList.Add(info.Mobile, errorMessage);
                                }
                            }

                            //��¼�ʼ�����״̬
                            srinfo = new SubscribePushRecordInfo()
                            {
                                Mobile = info.Mobile,
                                PushType = ESubscribePushType.ManualPush,
                                SubscribeName = DataProvider.SubscribeDAO.GetName(publishmentSystemID, info.SubscribeName),
                                SubscriptionTemplate = strBody,
                                PublishmentSystemID = publishmentSystemID,
                                PushStatu = isSuccess ? EBoolean.True : EBoolean.False,
                                TaskID = taskInfo.TaskID,
                                UserName = BaiRongDataProvider.AdministratorDAO.UserName
                            };
                            DataProvider.SubscribePushRecordDAO.Insert(srinfo);
                        }

                        #endregion
                    }
                }

                if (errorEmailSortedList.Count > 0 || errorMobileSortedList.Count > 0 || errorFileTemplateIDSortedList.Count > 0)
                {
                    StringBuilder errorMessage = new StringBuilder();
                    foreach (string email in errorEmailSortedList.Keys)
                    {
                        errorMessage.AppendFormat(" Subscribe Email {0} error:{1}", email, errorEmailSortedList[email]).Append(StringUtils.Constants.ReturnAndNewline);
                    }
                    foreach (string mobile in errorMobileSortedList.Keys)
                    {
                        errorMessage.AppendFormat("Subscribe Mobile {0} error:{1}", mobile, errorMobileSortedList[mobile]).Append(StringUtils.Constants.ReturnAndNewline);
                    }
                    foreach (string email in errorFileTemplateIDSortedList.Keys)
                    {
                        errorMessage.AppendFormat("Get FilebyEmail {0} error:{1}", email, errorFileTemplateIDSortedList[email]).Append(StringUtils.Constants.ReturnAndNewline);
                    }
                }
                if (taskInfo.ServiceType == EServiceType.Subscribe && taskInfo.FrequencyType == EFrequencyType.OnlyOnce)
                {
                    BaiRongDataProvider.TaskDAO.Delete(taskInfo.TaskID);
                }
            }
        }
    }
}
