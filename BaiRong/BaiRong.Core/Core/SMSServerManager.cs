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
using System.Net;
using System.Xml;
using System.Collections.Generic;

namespace BaiRong.Core
{
    public class SMSServerManager
    {


        private static SMSServerInfo smsServerInstance;
        public static SMSServerInfo SMSServerInstance
        {
            get { InitSmsServer(); return smsServerInstance; }
            set { smsServerInstance = value; }
        }

        private static string API_LastSmsCount = "GetLastSmsCount";
        private static string API_Send = "Send";
        private static string API_InsertTemplate = "InsertTemplate";
        private static string API_UpdateTemplate = "UpdateTemplate";
        private static string API_DeleteTemplate = "DeleteTemplate";
        private static string API_SearchTemplate = "SearchTemplate";

        //用户需要输入的参数
        public static string MOBILE = "mobile";
        public static string TEXT = "text";
        public static string TEMPLATE_TEXT = "templateText";
        public static string TEMPLATE_ID = "templateID";
        public static bool IsChange
        {
            get;
            set;
        }

        public static bool IsEnabled
        {
            get
            {
                if (SMSServerInstance != null && SMSServerInstance.SMSServerID > 0)
                    return true;
                return false;
            }
        }

        public static string InitSMSServerType
        {
            get;
            set;
        }

        private static SMSServerInfo InitSmsServer()
        {
            if (IsChange || smsServerInstance == null)
            {
                //从数据库中获取，发送短信服务类
                smsServerInstance = BaiRongDataProvider.SMSServerDAO.GetSMSServerInfo(string.IsNullOrEmpty(InitSMSServerType) ? ConfigManager.Additional.SMSServerType : InitSMSServerType);
            }

            return smsServerInstance;
        }

        /// <summary>
        /// 获取剩余短信条数
        /// </summary>
        /// <param name="lastCount"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static bool GetLastSmsCount(out int lastCount, out string errorMessage)
        {
            NameValueCollection sendParamsCollection = GetSendParams(API_LastSmsCount);

            bool isSuccess = false;
            lastCount = 0;
            errorMessage = string.Empty;
            string returnContent = string.Empty;
            string lastSmsSearchUrl = ParseAPIString(SMSServerInstance.Additional.LastSmsSearchUrl, SMSServerInstance.ParamCollection);
            if (SendRequest(lastSmsSearchUrl, GetSendRequestType(API_LastSmsCount), sendParamsCollection, GetSendParamsType(API_LastSmsCount), out returnContent))
            {
                string flag = GetOKFlag(API_LastSmsCount);
                string msgFlag = GetMsgFlag(API_LastSmsCount);
                string result = GetReturnValue(returnContent, flag, GetReturnContentType(API_LastSmsCount));
                string retvalKey = GetRetrunValueKey(API_LastSmsCount);
                if (GetOKValue(API_LastSmsCount) == result)
                {
                    lastCount = TranslateUtils.ToInt(GetReturnValue(returnContent, retvalKey, GetReturnContentType(API_LastSmsCount)));
                    isSuccess = true;
                }
                else
                {
                    if (string.IsNullOrEmpty(msgFlag))
                        errorMessage = "接口返回出现错误，返回标记为[" + flag + "]：" + result;
                    else
                        errorMessage = "接口返回出现错误，" + GetReturnValue(returnContent, msgFlag, GetReturnContentType(API_Send));
                    isSuccess = false;
                }
            }
            else
            {
                errorMessage = returnContent;
            }

            return isSuccess;
        }


        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="lastCount"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static bool Send(string mobile, string text, out string errorMessage)
        {
            ArrayList arrayList = new ArrayList();
            arrayList.Add(mobile);
            return Send(arrayList, text, out errorMessage);
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="lastCount"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static bool Send(ArrayList mobiles, string text, out string errorMessage)
        {
            NameValueCollection sendParamsCollection = GetSendParams(API_Send);
            NameValueCollection inputParams = new NameValueCollection();
            inputParams.Add(MOBILE, TranslateUtils.ObjectCollectionToString(mobiles));
            inputParams.Add(TEXT, text);
            sendParamsCollection = TranslateUtils.ToNameValueCollection(ParseAPIString(TranslateUtils.NameValueCollectionToString(sendParamsCollection), inputParams));
            bool isSuccess = false;
            errorMessage = string.Empty;
            string returnContent = string.Empty;
            string sendUrl = ParseAPIString(SMSServerInstance.Additional.SendUrl, SMSServerInstance.ParamCollection);
            if (SendRequest(sendUrl, GetSendRequestType(API_Send), sendParamsCollection, GetSendParamsType(API_Send), out returnContent))
            {
                string flag = GetOKFlag(API_Send);
                string msgFlag = GetMsgFlag(API_Send);
                string result = GetReturnValue(returnContent, flag, GetReturnContentType(API_Send));
                string retvalKey = GetRetrunValueKey(API_Send);
                if (GetOKValue(API_Send) == result)
                {
                    //发送短信日志
                    SMSMessageInfo smsMessageInfo = new SMSMessageInfo();
                    smsMessageInfo.SMSUserName = ConfigManager.Additional.SMSAccount;
                    smsMessageInfo.MobilesList = TranslateUtils.ObjectCollectionToString(mobiles);
                    smsMessageInfo.SMSContent = sendParamsCollection[TEXT];
                    smsMessageInfo.SendDate = System.DateTime.Now;
                    BaiRongDataProvider.SMSMessageDAO.Insert(smsMessageInfo);

                    isSuccess = true;
                }
                else
                {
                    if (string.IsNullOrEmpty(msgFlag))
                        errorMessage = "接口返回出现错误，返回标记为[" + flag + "]：" + result;
                    else
                        errorMessage = "接口返回出现错误，" + GetReturnValue(returnContent, msgFlag, GetReturnContentType(API_Send));
                    isSuccess = false;
                }
            }
            else
            {
                errorMessage = returnContent;
            }

            return isSuccess;
        }

        /// <summary>
        /// 添加短信模板
        /// </summary>
        /// <param name="lastCount"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static bool InsertTemplate(string text, out string errorMessage, out string templateID)
        {
            NameValueCollection sendParamsCollection = GetSendParams(API_InsertTemplate);

            bool isSuccess = false;
            errorMessage = string.Empty;
            templateID = string.Empty;
            string returnContent = string.Empty;
            NameValueCollection inputParams = new NameValueCollection();
            inputParams.Add(TEMPLATE_TEXT, StringUtils.ValueToUrl(text));
            sendParamsCollection = TranslateUtils.ToNameValueCollection(ParseAPIString(TranslateUtils.NameValueCollectionToString(sendParamsCollection), inputParams));
            string insertTemplateUrl = ParseAPIString(SMSServerInstance.Additional.InsertTemplateUrl, SMSServerInstance.ParamCollection);
            if (SendRequest(insertTemplateUrl, GetSendRequestType(API_InsertTemplate), sendParamsCollection, GetSendParamsType(API_InsertTemplate), out returnContent))
            {
                string flag = GetOKFlag(API_InsertTemplate);
                string msgFlag = GetMsgFlag(API_InsertTemplate);
                string result = GetReturnValue(returnContent, flag, GetReturnContentType(API_InsertTemplate));
                string retvalKey = GetRetrunValueKey(API_InsertTemplate);
                if (GetOKValue(API_InsertTemplate) == result)
                {
                    templateID = GetReturnValue(returnContent, retvalKey, GetReturnContentType(API_InsertTemplate));
                    isSuccess = true;
                }
                else
                {
                    if (string.IsNullOrEmpty(msgFlag))
                        errorMessage = "接口返回出现错误，返回标记为[" + flag + "]：" + result;
                    else
                        errorMessage = "接口返回出现错误，" + GetReturnValue(returnContent, msgFlag, GetReturnContentType(API_InsertTemplate));
                    isSuccess = false;
                }
            }
            else
            {
                errorMessage = returnContent;
            }

            return isSuccess;
        }

        /// <summary>
        /// 修改短信模板
        /// </summary>
        /// <param name="lastCount"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static bool UpdateTemplate(string templateID, string text, out string errorMessage)
        {
            NameValueCollection sendParamsCollection = GetSendParams(API_UpdateTemplate);

            bool isSuccess = false;
            errorMessage = string.Empty;
            string returnContent = string.Empty;
            NameValueCollection inputParams = new NameValueCollection();
            inputParams.Add(TEMPLATE_TEXT, StringUtils.ValueToUrl(text));
            inputParams.Add(TEMPLATE_ID, templateID);
            sendParamsCollection = TranslateUtils.ToNameValueCollection(ParseAPIString(TranslateUtils.NameValueCollectionToString(sendParamsCollection), inputParams));
            string updateTemplateUrl = ParseAPIString(SMSServerInstance.Additional.UpdateTemplateUrl, SMSServerInstance.ParamCollection);
            if (SendRequest(updateTemplateUrl, GetSendRequestType(API_UpdateTemplate), sendParamsCollection, GetSendParamsType(API_UpdateTemplate), out returnContent))
            {
                string flag = GetOKFlag(API_UpdateTemplate);
                string msgFlag = GetMsgFlag(API_UpdateTemplate);
                string result = GetReturnValue(returnContent, flag, GetReturnContentType(API_UpdateTemplate));
                string retvalKey = GetRetrunValueKey(API_UpdateTemplate);
                if (GetOKValue(API_UpdateTemplate) == result)
                {
                    templateID = GetReturnValue(returnContent, retvalKey, GetReturnContentType(API_UpdateTemplate));
                    isSuccess = true;
                }
                else
                {
                    if (string.IsNullOrEmpty(msgFlag))
                        errorMessage = "接口返回出现错误，返回标记为[" + flag + "]：" + result;
                    else
                        errorMessage = "接口返回出现错误，" + GetReturnValue(returnContent, msgFlag, GetReturnContentType(API_UpdateTemplate));
                    isSuccess = false;
                }
            }
            else
            {
                errorMessage = returnContent;
            }

            return isSuccess;
        }

        /// <summary>
        /// 删除短信模板
        /// </summary>
        /// <param name="lastCount"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static bool DeleteTemplate(string templateID, out string errorMessage)
        {
            NameValueCollection sendParamsCollection = GetSendParams(API_DeleteTemplate);

            bool isSuccess = false;
            errorMessage = string.Empty;
            string returnContent = string.Empty;
            NameValueCollection inputParams = new NameValueCollection();
            inputParams.Add(TEMPLATE_ID, templateID);
            sendParamsCollection = TranslateUtils.ToNameValueCollection(ParseAPIString(TranslateUtils.NameValueCollectionToString(sendParamsCollection), inputParams));
            string deleteTemplateUrl = ParseAPIString(SMSServerInstance.Additional.DeleteTemplateUrl, SMSServerInstance.ParamCollection);
            if (SendRequest(deleteTemplateUrl, GetSendRequestType(API_DeleteTemplate), sendParamsCollection, GetSendParamsType(API_DeleteTemplate), out returnContent))
            {
                string flag = GetOKFlag(API_DeleteTemplate);
                string msgFlag = GetMsgFlag(API_DeleteTemplate);
                string result = GetReturnValue(returnContent, flag, GetReturnContentType(API_DeleteTemplate));
                string retvalKey = GetRetrunValueKey(API_DeleteTemplate);
                if (GetOKValue(API_DeleteTemplate) == result)
                {
                    templateID = GetReturnValue(returnContent, retvalKey, GetReturnContentType(API_DeleteTemplate));
                    isSuccess = true;
                }
                else
                {
                    if (string.IsNullOrEmpty(msgFlag))
                        errorMessage = "接口返回出现错误，返回标记为[" + flag + "]：" + result;
                    else
                        errorMessage = "接口返回出现错误，" + GetReturnValue(returnContent, msgFlag, GetReturnContentType(API_DeleteTemplate));
                    isSuccess = false;
                }
            }
            else
            {
                errorMessage = returnContent;
            }

            return isSuccess;
        }

        /// <summary>
        /// 查询短信模板
        /// </summary>
        /// <param name="lastCount"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static bool SearchTemplate(string templateID, out string errorMessage, out ArrayList msgTemplateArray)
        {
            NameValueCollection sendParamsCollection = GetSendParams(API_SearchTemplate);

            bool isSuccess = false;
            errorMessage = string.Empty;
            msgTemplateArray = new ArrayList();
            string returnContent = string.Empty;
            NameValueCollection inputParams = new NameValueCollection();
            inputParams.Add(TEMPLATE_ID, templateID);
            sendParamsCollection = TranslateUtils.ToNameValueCollection(ParseAPIString(TranslateUtils.NameValueCollectionToString(sendParamsCollection), inputParams));
            string SearchTemplateUrl = ParseAPIString(SMSServerInstance.Additional.SearchTemplateUrl, SMSServerInstance.ParamCollection);
            if (SendRequest(SearchTemplateUrl, GetSendRequestType(API_SearchTemplate), sendParamsCollection, GetSendParamsType(API_SearchTemplate), out returnContent))
            {
                string flag = GetOKFlag(API_SearchTemplate);
                string msgFlag = GetMsgFlag(API_SearchTemplate);
                string result = GetReturnValue(returnContent, flag, GetReturnContentType(API_SearchTemplate));
                string retvalKey = GetRetrunValueKey(API_SearchTemplate);
                if (GetOKValue(API_SearchTemplate) == result)
                {
                    msgTemplateArray.Add(GetReturnValue(returnContent, retvalKey, GetReturnContentType(API_SearchTemplate)));
                    isSuccess = true;
                }
                else
                {
                    if (string.IsNullOrEmpty(msgFlag))
                        errorMessage = "接口返回出现错误，返回标记为[" + flag + "]：" + result;
                    else
                        errorMessage = "接口返回出现错误，" + GetReturnValue(returnContent, msgFlag, GetReturnContentType(API_SearchTemplate));
                    isSuccess = false;
                }
            }
            else
            {
                errorMessage = returnContent;
            }

            return isSuccess;
        }

        /// <summary>
        /// 得到短信服务商对应的模板内容
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string GetTemplate(string templateText)
        {
            return ESMSServerTypeUtils.GetTemplate(templateText, ESMSServerTypeUtils.GetEnumType(SMSServerInstance.SMSServerEName));
        }

        #region 辅助方法

        private static bool SendRequest(string url, string type, NameValueCollection parms, string parmsType, out string returnContent)
        {
            bool retval = true;
            string requestData = StringUtils.ValueFromUrl(TranslateUtils.NameValueCollectionToString(parms));
            returnContent = string.Empty;
            try
            {
                if (type.ToLower() == "get")
                {
                    //get发送请求，参数按照NameValueCollection格式
                    WebClientUtils.Get(url, requestData, out returnContent);
                }
                else if (type.ToLower() == "post")
                {
                    //post发送请求，参数格式：NameValueCollection , Json , Xml
                    if (parmsType == EDataFormat.Json.ToString())
                    {
                        requestData = TranslateUtils.NameValueCollectionToJsonString(parms);
                    }
                    else if (parmsType == EDataFormat.Xml.ToString())
                    {
                        requestData = TranslateUtils.NameValueCollectionToXmlString(parms);
                    }
                    WebClientUtils.Post(url, requestData, out returnContent);
                }
            }
            catch (Exception ex)
            {
                returnContent = ex.Message;
                retval = false;
            }

            return retval;
        }

        private static string ParseAPIString(string input, NameValueCollection paramsCollection)
        {
            //Demo:http://yunpian.com/{@Version}/{@User}/get.json
            //To:http://yunpian.com/V1/user/get.json
            string regex = "(?<element>{@[^}]+})";
            ArrayList elements = RegexUtils.GetContents("element", regex, input);
            foreach (string element in elements)
            {
                string key = element.Substring(2, element.Length - 3);
                if (!string.IsNullOrEmpty(paramsCollection[key]))
                {
                    input = StringUtils.ReplaceFirst(element, input, paramsCollection[key]);
                }
            }
            return input;
        }

        private static string GetSendSettings(string methodName, string paramName)
        {
            if (methodName == API_LastSmsCount)
            {
                return SMSServerInstance.Additional.LastSmsSearchSettings[paramName];
            }
            else if (methodName == API_Send)
            {
                return SMSServerInstance.Additional.SendSettings[paramName];
            }
            else if (methodName == API_InsertTemplate)
            {
                return SMSServerInstance.Additional.InsertTemplateSettings[paramName];
            }
            else if (methodName == API_UpdateTemplate)
            {
                return SMSServerInstance.Additional.UpdateTemplateSettings[paramName];
            }
            else if (methodName == API_DeleteTemplate)
            {
                return SMSServerInstance.Additional.DeleteTemplateSettings[paramName];
            }
            return string.Empty;
        }

        private static NameValueCollection GetSendParams(string methodName)
        {
            if (methodName == API_LastSmsCount)
            {
                return TranslateUtils.ToNameValueCollection(ParseAPIString(SMSServerInstance.Additional.LastSmsSearchParams, SMSServerInstance.ParamCollection));
            }
            else if (methodName == API_Send)
            {
                return TranslateUtils.ToNameValueCollection(ParseAPIString(SMSServerInstance.Additional.SendParams, SMSServerInstance.ParamCollection));
            }
            else if (methodName == API_InsertTemplate)
            {
                return TranslateUtils.ToNameValueCollection(ParseAPIString(SMSServerInstance.Additional.InsertTemplateParams, SMSServerInstance.ParamCollection));
            }
            else if (methodName == API_UpdateTemplate)
            {
                return TranslateUtils.ToNameValueCollection(ParseAPIString(SMSServerInstance.Additional.UpdateTemplateParams, SMSServerInstance.ParamCollection));
            }
            else if (methodName == API_DeleteTemplate)
            {
                return TranslateUtils.ToNameValueCollection(ParseAPIString(SMSServerInstance.Additional.DeleteTemplateParams, SMSServerInstance.ParamCollection));
            }

            return new NameValueCollection();
        }

        private static string GetMsgFlag(string methodName)
        {
            return GetSendSettings(methodName, "MsgFlag");
        }

        private static string GetSendRequestType(string methodName)
        {
            return GetSendSettings(methodName, "SendRequestType");
        }

        //发送参数类型：NameValueCollection , Json , Xml
        private static string GetSendParamsType(string methodName)
        {
            return GetSendSettings(methodName, "SendParamsType");
        }

        //接受内容类型：json, xml
        private static string GetReturnContentType(string methodName)
        {
            return GetSendSettings(methodName, "ReturnContentType");
        }

        //接受内容中的状态标记
        private static string GetOKFlag(string methodName)
        {
            return GetSendSettings(methodName, "OKFlag");
        }

        //接受内容中的成功状态
        private static string GetOKValue(string methodName)
        {
            return GetSendSettings(methodName, "OKValue");
        }

        //获取返回内容中的特定值
        private static string GetRetrunValueKey(string methodName)
        {
            return GetSendSettings(methodName, "RetrunValueKey");
        }

        //获取接受内容中的key对应的值
        private static string GetReturnValue(string returnContent, string key, string reciveParamsType)
        {
            string retval = string.Empty;
            if (EDataFormatUtils.Equals(reciveParamsType, EDataFormat.Json))
            {
                NameValueCollection nvcReturnContent = TranslateUtils.ParseJsonStringToNameValueCollection(returnContent, true);
                retval = nvcReturnContent[key];
            }
            else if (EDataFormatUtils.Equals(reciveParamsType, EDataFormat.Xml))
            {
                XmlDocument xmlDoc = XmlUtils.GetXmlDocument(returnContent);
                retval = XmlUtils.GetXmlNodeInnerText(xmlDoc, key);
            }
            return retval;
        }
        #endregion

    }
}