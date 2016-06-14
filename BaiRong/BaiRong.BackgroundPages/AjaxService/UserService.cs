using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Collections;
using System.Web;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System.Web.UI;

using BaiRong.Core;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core.Net;
using BaiRong.Core.IO;
using BaiRong.Core.Data.Provider;


using BaiRong.BackgroundPages;
using BaiRong.Core.Cryptography;
using System.Collections.Generic;

namespace BaiRong.BackgroundPages.Service
{
    public class UserService : Page
    {

        public const string CACHE_TOTAL_COUNT = "_TotalCount";
        public const string CACHE_CURRENT_COUNT = "_CurrentCount";
        public const string CACHE_MESSAGE = "_Message";

        public void Page_Load(object sender, System.EventArgs e)
        {
            string type = base.Request.QueryString["type"];
            NameValueCollection retval = new NameValueCollection();
            string retString = string.Empty;

            if (type == "GetCountArray")
            {
                string userKeyPrefix = base.Request["userKeyPrefix"];
                retval = GetCountArray(userKeyPrefix);
            }
            if (type.ToLower() == "UserRegisterValidateEmail".ToLower())
            {
                UserRegisterValidateEmail();
            }
            if (type.ToLower() == "InitSMSTemplate".ToLower())
            {
                string userKeyPrefix = base.Request["userKeyPrefix"];
                string smsServerEName = base.Request["smsServerEName"];
                retval = InitSMSTemplate(smsServerEName, userKeyPrefix);
            }


            if (!string.IsNullOrEmpty(retString))
            {
                Page.Response.Write(retString);
                Page.Response.End();
            }
            else
            {
                string jsonString = TranslateUtils.NameValueCollectionToJsonString(retval);
                Page.Response.Write(jsonString);
                Page.Response.End();
            }
        }

        public NameValueCollection GetCountArray(string userKeyPrefix)//���ȼ���ʾ
        {
            NameValueCollection retval = new NameValueCollection();
            if (CacheUtils.Get(userKeyPrefix + CACHE_TOTAL_COUNT) != null && CacheUtils.Get(userKeyPrefix + CACHE_CURRENT_COUNT) != null && CacheUtils.Get(userKeyPrefix + CACHE_MESSAGE) != null)
            {
                int totalCount = TranslateUtils.ToInt((string)CacheUtils.Get(userKeyPrefix + CACHE_TOTAL_COUNT));
                int currentCount = TranslateUtils.ToInt((string)CacheUtils.Get(userKeyPrefix + CACHE_CURRENT_COUNT));
                string message = (string)CacheUtils.Get(userKeyPrefix + CACHE_MESSAGE);


                retval.Add("totalCount", totalCount.ToString());
                retval.Add("currentCount", currentCount.ToString());
                retval.Add("message", message);
            }
            return retval;
        }

        private void UserRegisterValidateEmail()
        {
            string checkCode = base.Request.QueryString["checkCode"];
            string userName = base.Request.QueryString["userName"];
            string groupSN = base.Request.QueryString["groupSN"];
            string returnUrl = StringUtils.ValueFromUrl(base.Request.QueryString["returnUrl"]);

            string[] urls = returnUrl.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            string indexUrl = string.Empty;
            string waitUrl = string.Empty;
            if (urls[0].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries).Length == 2)
            {
                indexUrl = urls[0].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1];
            }

            if (urls.Length > 1 && urls[1].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries).Length == 2)
            {
                waitUrl = urls[1].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1];
            }

            if (EncryptUtils.Md5(userName) == checkCode)
            {
                UserInfo user = UserManager.GetUserInfo(groupSN, userName);
                BaiRongDataProvider.UserDAO.Check(user.UserID);
                BaiRongDataProvider.UserDAO.Login(groupSN, userName, false);

                if (!string.IsNullOrEmpty(waitUrl))
                {
                    waitUrl = indexUrl + waitUrl + "?returnUrl=" + indexUrl;
                    Response.Redirect(waitUrl);
                }
                else
                    Response.Redirect(indexUrl);

            }
            else
            {
                Response.Write("�Բ�����֤�����ѹ��ڣ�");
                Response.End();
            }
        }

        private NameValueCollection InitSMSTemplate(string smsServerEName, string userKeyPrefix)
        {
            string cacheTotalCountKey = userKeyPrefix + CACHE_TOTAL_COUNT;
            string cacheCurrentCountKey = userKeyPrefix + CACHE_CURRENT_COUNT;
            string cacheMessageKey = userKeyPrefix + CACHE_MESSAGE;

            //����
            int templateCount = 0;
            //��ǰ��
            int currentNum = 0;

            //��Ҫ��ʼ����ģ����Ŀ
            List<string> userNoticeList = EUserNoticeTypeUtils.GetAllVaules();
            templateCount = userNoticeList.Count;


            CacheUtils.Max(cacheTotalCountKey, templateCount.ToString());//�洢��Ҫ��ҳ������
            CacheUtils.Max(cacheCurrentCountKey, currentNum.ToString());//�洢��ǰ��ҳ������
            CacheUtils.Max(cacheMessageKey, string.Empty);//�洢��Ϣ

            //���ء����н�����͡�������Ϣ�����ַ�������
            NameValueCollection retval = new NameValueCollection();

            try
            {
                string templateID = string.Empty;
                string errorMessage = string.Empty;
                SMSServerManager.InitSMSServerType = smsServerEName;
                ESMSServerType smsServerType = ESMSServerTypeUtils.GetEnumType(SMSServerManager.SMSServerInstance.SMSServerEName);
                SMSServerManager.InsertTemplate(ESMSServerTypeUtils.GetTemplate("���ŷ����̶��ŷ��ͣ�", smsServerType), out errorMessage, out templateID);
                foreach (string userNotice in userNoticeList)
                {
                    EUserNoticeType userNoticeType = EUserNoticeTypeUtils.GetEnumType(userNotice);
                    try
                    {
                        UserNoticeSettingInfo userNoticeSettingInfo = UserNoticeSettingManager.GetUserNoticeSettingInfo(userNotice);
                        
                        if (SMSServerManager.IsEnabled && userNoticeSettingInfo.ByPhone && !string.IsNullOrEmpty(userNoticeSettingInfo.PhoneTemplate))
                            SMSServerManager.InsertTemplate(ESMSServerTypeUtils.GetTemplate(userNoticeSettingInfo.PhoneTemplate, smsServerType), out errorMessage, out templateID);
                        if (string.IsNullOrEmpty(errorMessage) && !string.IsNullOrEmpty(templateID))
                        {
                            UserNoticeTemplateInfo userNoticeTemplateInfo = new UserNoticeTemplateInfo();
                            userNoticeTemplateInfo.Content = userNoticeSettingInfo.PhoneTemplate;
                            userNoticeTemplateInfo.IsEnable = true;
                            userNoticeTemplateInfo.IsSystem = true;
                            userNoticeTemplateInfo.Name = EUserNoticeTypeUtils.GetText(userNoticeType);
                            userNoticeTemplateInfo.RemoteTemplateID = templateID;
                            userNoticeTemplateInfo.Title = EUserNoticeTypeUtils.GetText(userNoticeType);
                            userNoticeTemplateInfo.Type = EUserNoticeTemplateType.Phone;
                            userNoticeTemplateInfo.RelatedIdentity = userNoticeType;
                            userNoticeTemplateInfo.RemoteType = smsServerType;
                            BaiRongDataProvider.UserNoticeTemplateDAO.Insert(userNoticeTemplateInfo);
                            CacheUtils.Max(cacheMessageKey, string.Format("{0}����ģ���ʼ���ɹ�...", EUserNoticeTypeUtils.GetText(userNoticeType)));//�洢��Ϣ
                        }
                        else
                        {
                            CacheUtils.Max(cacheMessageKey, string.Format("{0}����ģ���ʼ��ʧ��...,ԭ��{1}", EUserNoticeTypeUtils.GetText(userNoticeType), errorMessage));//�洢��Ϣ
                        }

                    }
                    catch (Exception ex)
                    {
                        CacheUtils.Max(cacheMessageKey, string.Format("{0}����ģ���ʼ��ʧ��...,ԭ��{1}", EUserNoticeTypeUtils.GetText(userNoticeType), ex.Message));//�洢��Ϣ
                    }
                    currentNum++;
                    CacheUtils.Max(cacheCurrentCountKey, currentNum.ToString());//�洢��ǰ��ҳ������

                }

                retval.Add("resultMessage", string.Format("��ʼ������ģ��ɹ�"));
                retval.Add("errorMessage", string.Empty);
            }
            catch (Exception ex)
            {
                retval.Add("resultMessage", string.Empty);
                retval.Add("errorMessage", string.Format("��ʼ������ģ��ʧ��"));
            }

            CacheUtils.Remove(cacheTotalCountKey);//ȡ���洢��Ҫ��ҳ������
            CacheUtils.Remove(cacheCurrentCountKey);//ȡ���洢��ǰ��ҳ������
            CacheUtils.Remove(cacheMessageKey);//ȡ���洢��Ϣ

            return retval;
        }
    }
}
