using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.DirectoryServices;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Specialized;
using BaiRong.Core.Cryptography;
using BaiRong.Core.Net;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;

namespace BaiRong.Core
{
    public class SMSManager
    {
        public static bool GetTotalCount(out int totalCount, out string errorMessage)
        {
            bool isSuccess = false;
            totalCount = 0;
            errorMessage = string.Empty;

            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("message", string.Format(@"<?xml version=""1.0"" encoding=""UTF-8""?><message><account>{0}</account><password>{1}</password></message>", ConfigManager.Additional.SMSAccount, EncryptUtils.Md5(ConfigManager.Additional.SMSPassword).ToLower()));
            string xmlContent = string.Empty;

            if (WebClientUtils.Post("http://3tong.net:8090/http/sms/Balance", arguments, out xmlContent))
            {
                string result = RegexUtils.GetInnerContent("result", xmlContent);

                if (result == "0")
                {
                    totalCount = TranslateUtils.ToInt(RegexUtils.GetInnerContent("number", xmlContent));
                    isSuccess = true;
                }
                else
                {
                    errorMessage = GetTotalCountStatus(result);
                }
            }
            else
            {
                errorMessage = xmlContent;
            }

            return isSuccess;
        }

        private static string GetTotalCountStatus(string code)
        {
            string status = string.Empty;
            switch (code)
            {
                case "0":
                    status = "成功";
                    break;
                case "1":
                    status = "账号无效";
                    break;
                case "2":
                    status = "密码错误";
                    break;
                case "3":
                    status = "请求太快";
                    break;
                case "9":
                    status = "请求来源地址无效";
                    break;
                case "97":
                    status = "接入方式错误";
                    break;
                case "98":
                    status = "系统繁忙";
                    break;
                case "99":
                    status = "消息格式错误";
                    break;
                default:
                    status = string.Empty;
                    break;
            }
            return status;
        }

        public static bool Send(ArrayList mobileArrayList, string smsMessage, out string errorMessage)
        {
            bool isSuccess = false;
            errorMessage = string.Empty;

            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("message", string.Format(@"<?xml version=""1.0"" encoding=""UTF-8""?><message><account>{0}</account><password>{1}</password><msgid></msgid><phones>{2}</phones><content>{3}</content><subcode></subcode><sendtime></sendtime></message>", ConfigManager.Additional.SMSAccount, EncryptUtils.Md5(ConfigManager.Additional.SMSPassword).ToLower(), TranslateUtils.ObjectCollectionToString(mobileArrayList), StringUtils.MaxLengthText(smsMessage, 500, string.Empty)));
            string xmlContent = string.Empty;

            if (WebClientUtils.Post("http://3tong.net:8090/http/sms/Submit", arguments, out xmlContent))
            {
                string result = RegexUtils.GetInnerContent("result", xmlContent);

                if (result == "0")
                {
                    SMSMessageInfo smsMessageInfo = new SMSMessageInfo();
                    smsMessageInfo.SMSUserName = ConfigManager.Additional.SMSAccount;
                    smsMessageInfo.MobilesList = TranslateUtils.ObjectCollectionToString(mobileArrayList);
                    smsMessageInfo.SMSContent = smsMessage;
                    smsMessageInfo.SendDate = System.DateTime.Now;
                    BaiRongDataProvider.SMSMessageDAO.Insert(smsMessageInfo);

                    isSuccess = true;
                }
                else
                {
                    errorMessage = GetSendStatus(result);
                }
            }
            else
            {
                errorMessage = xmlContent;
            }

            return isSuccess;
        }

        private static string GetSendStatus(string code)
        {
            string status = string.Empty;
            switch (code)
            {
                case "0":
                    status = "提交成功";
                    break;
                case "1":
                    status = "账号无效";
                    break;
                case "2":
                    status = "密码错误";
                    break;
                case "3":
                    status = "msgid不唯一";
                    break;
                case "4":
                    status = "存在无效手机号码";
                    break;
                case "5":
                    status = "手机号码个数超过最大限制";
                    break;
                case "6":
                    status = "短信内容超过最大限制";
                    break;
                case "7":
                    status = "扩展子号码无效（验证格式和长度，不能超过20位）";
                    break;
                case "8":
                    status = "发送时间格式无效";
                    break;
                case "9":
                    status = "请求来源地址无效";
                    break;
                case "10":
                    status = "内容包含敏感词";
                    break;
                case "11":
                    status = "余额不足";
                    break;
                case "97":
                    status = "接入方式错误";
                    break;
                case "98":
                    status = "系统繁忙";
                    break;
                case "99":
                    status = "消息格式错误";
                    break;
                default:
                    status = string.Empty;
                    break;
            }
            return status;
        }
    }
}