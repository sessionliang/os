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

        public NameValueCollection GetCountArray(string userKeyPrefix)//进度及显示
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
                Response.Write("对不起，验证链接已过期！");
                Response.End();
            }
        }

        private NameValueCollection InitSMSTemplate(string smsServerEName, string userKeyPrefix)
        {
            string cacheTotalCountKey = userKeyPrefix + CACHE_TOTAL_COUNT;
            string cacheCurrentCountKey = userKeyPrefix + CACHE_CURRENT_COUNT;
            string cacheMessageKey = userKeyPrefix + CACHE_MESSAGE;

            //总数
            int templateCount = 0;
            //当前数
            int currentNum = 0;

            //需要初始化的模板数目
            List<string> userNoticeList = EUserNoticeTypeUtils.GetAllVaules();
            templateCount = userNoticeList.Count;


            CacheUtils.Max(cacheTotalCountKey, templateCount.ToString());//存储需要的页面总数
            CacheUtils.Max(cacheCurrentCountKey, currentNum.ToString());//存储当前的页面总数
            CacheUtils.Max(cacheMessageKey, string.Empty);//存储消息

            //返回“运行结果”和“错误信息”的字符串数组
            NameValueCollection retval = new NameValueCollection();

            try
            {
                string templateID = string.Empty;
                string errorMessage = string.Empty;
                SMSServerManager.InitSMSServerType = smsServerEName;
                ESMSServerType smsServerType = ESMSServerTypeUtils.GetEnumType(SMSServerManager.SMSServerInstance.SMSServerEName);
                SMSServerManager.InsertTemplate(ESMSServerTypeUtils.GetTemplate("短信服务商短信发送！", smsServerType), out errorMessage, out templateID);
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
                            CacheUtils.Max(cacheMessageKey, string.Format("{0}短信模板初始化成功...", EUserNoticeTypeUtils.GetText(userNoticeType)));//存储消息
                        }
                        else
                        {
                            CacheUtils.Max(cacheMessageKey, string.Format("{0}短信模板初始化失败...,原因：{1}", EUserNoticeTypeUtils.GetText(userNoticeType), errorMessage));//存储消息
                        }

                    }
                    catch (Exception ex)
                    {
                        CacheUtils.Max(cacheMessageKey, string.Format("{0}短信模板初始化失败...,原因：{1}", EUserNoticeTypeUtils.GetText(userNoticeType), ex.Message));//存储消息
                    }
                    currentNum++;
                    CacheUtils.Max(cacheCurrentCountKey, currentNum.ToString());//存储当前的页面总数

                }

                retval.Add("resultMessage", string.Format("初始化短信模板成功"));
                retval.Add("errorMessage", string.Empty);
            }
            catch (Exception ex)
            {
                retval.Add("resultMessage", string.Empty);
                retval.Add("errorMessage", string.Format("初始化短信模板失敗"));
            }

            CacheUtils.Remove(cacheTotalCountKey);//取消存储需要的页面总数
            CacheUtils.Remove(cacheCurrentCountKey);//取消存储当前的页面总数
            CacheUtils.Remove(cacheMessageKey);//取消存储消息

            return retval;
        }
    }
}
