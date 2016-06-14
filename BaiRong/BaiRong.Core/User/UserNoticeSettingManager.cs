using System.Collections;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System;
using BaiRong.Model;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BaiRong.Core
{
    public sealed class UserNoticeSettingManager
    {
        private UserNoticeSettingManager()
        {

        }

        //--------------��Ϣ������----------------
        private const string cacheKey = "BaiRong.Core.MessageCounts";

        /// <summary>
        /// Json���ݴ���ļ�������
        /// </summary>
        public const string JsonDirectoryName = "UserNoticeSettingInfo";

        public static UserNoticeSettingInfo GetUserNoticeSettingInfo(string noticeTypeStr)
        {
            UserNoticeSettingInfo info = CacheUtils.Get(noticeTypeStr) as UserNoticeSettingInfo;
            if (info == null)
            {
                string jsonFileName = noticeTypeStr;
                if (!jsonFileName.EndsWith(".json"))
                    jsonFileName += ".json";
                string jsonFile = PathUtils.GetAppPath(AppManager.Platform.AppID, JsonDirectoryName, jsonFileName);
                if (FileUtils.IsFileExists(jsonFile))
                {
                    string jsonStr = FileUtils.ReadText(jsonFile, ECharset.utf_8);
                    if (!string.IsNullOrEmpty(jsonStr))
                    {
                        info = JsonConvert.DeserializeObject<UserNoticeSettingInfo>(jsonStr);
                    }
                }
                CacheUtils.Max(noticeTypeStr, info, new System.Web.Caching.CacheDependency(jsonFile));
            }
            return info;
        }

        public static UserNoticeSettingInfo GetUserNoticeSettingInfoForAPI(string noticeTypeStr)
        {
            UserNoticeSettingInfo info = CacheUtils.Get(noticeTypeStr) as UserNoticeSettingInfo;
            if (info == null)
            {
                string jsonFileName = noticeTypeStr;
                if (!jsonFileName.EndsWith(".json"))
                    jsonFileName += ".json";
                string jsonFile = APIPageUtils.ParseUrl(PathUtils.GetAppPath(AppManager.Platform.AppID, JsonDirectoryName, jsonFileName));
                if (FileUtils.IsFileExists(jsonFile))
                {
                    string jsonStr = FileUtils.ReadText(jsonFile, ECharset.utf_8);
                    if (!string.IsNullOrEmpty(jsonStr))
                    {
                        info = JsonConvert.DeserializeObject<UserNoticeSettingInfo>(jsonStr);
                    }
                }
                CacheUtils.Max(noticeTypeStr, info, new System.Web.Caching.CacheDependency(jsonFile));
            }
            return info;
        }

        public static List<UserNoticeSettingInfo> GetUserNoticeSettingInfoList()
        {
            List<UserNoticeSettingInfo> list = new List<UserNoticeSettingInfo>();
            string jsonDirectory = PathUtils.GetAppPath(AppManager.Platform.AppID, JsonDirectoryName);
            string[] jsonFiles = DirectoryUtils.GetFilePaths(jsonDirectory);
            foreach (string jsonFile in jsonFiles)
            {
                UserNoticeSettingInfo info = GetUserNoticeSettingInfo(PathUtils.GetFileName(jsonFile));
                list.Add(info);
            }
            return list;
        }

        public static bool SetUserNoticeSettingInfo(UserNoticeSettingInfo info)
        {
            try
            {
                if (info == null)
                    return false;
                string jsonStr = JsonConvert.SerializeObject(info);
                string jsonFile = PathUtils.GetAppPath(AppManager.Platform.AppID, JsonDirectoryName, info.UserNoticeType + ".json");
                FileUtils.WriteText(jsonFile, ECharset.utf_8, jsonStr);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// ������Ϣ���������ͣ�
        /// </summary>
        /// <param name="noticeSettingInfo">��Ϣ����</param>
        /// <param name="targetList">Ŀ�꼯�ϣ����䣬�ֻ����û�����</param>
        /// <param name="errorMessage">������Ϣ</param>
        /// <returns></returns>
        public static bool SendMsg(UserNoticeSettingInfo noticeSettingInfo, List<string> targetList, out string errorMessage)
        {
            Dictionary<string, string> dicReplace = new Dictionary<string, string>();
            return SendMsg(noticeSettingInfo, targetList, dicReplace, out errorMessage);
        }

        /// <summary>
        /// ������Ϣ���������ͣ�
        /// </summary>
        /// <param name="noticeSettingInfo">��Ϣ����</param>
        /// <param name="targetList">Ŀ�꼯�ϣ����䣬�ֻ����û�����</param>
        /// <param name="dicReplace">�滻ֵ�����ⲿ�����ֵ�����Ը��ǵ������ڲ���ֵ��</param>
        /// <param name="errorMessage">������Ϣ</param>
        /// <returns></returns>
        public static bool SendMsg(UserNoticeSettingInfo noticeSettingInfo, List<string> targetList, Dictionary<string, string> dicReplace, out string errorMessage)
        {
            errorMessage = string.Empty;
            foreach (string target in targetList)
            {
                SendMsg(noticeSettingInfo, target, dicReplace, out errorMessage);
            }
            return true;
        }

        /// <summary>
        /// ������Ϣ��������
        /// </summary>
        /// <param name="noticeSettingInfo">��Ϣ����</param>
        /// <param name="target">Ŀ��</param>
        /// <param name="errorMessage">������Ϣ</param>
        /// <returns></returns>
        public static bool SendMsg(UserNoticeSettingInfo noticeSettingInfo, string target, out string errorMessage)
        {
            Dictionary<string, string> dicReplace = new Dictionary<string, string>();
            return SendMsg(noticeSettingInfo, target, dicReplace, out errorMessage);
        }

        /// <summary>
        /// ������Ϣ��������
        /// </summary>
        /// <param name="noticeSettingInfo">��Ϣ����</param>
        /// <param name="target">Ŀ��</param>
        /// <param name="dicReplace">�滻ֵ�����ⲿ�����ֵ�����Ը��ǵ������ڲ���ֵ��</param>
        /// <param name="errorMessage">������Ϣ</param>
        /// <returns></returns>
        public static bool SendMsg(UserNoticeSettingInfo noticeSettingInfo, string target, Dictionary<string, string> dicReplace, out string errorMessage)
        {
            bool isEmail = false;
            bool isPhone = false;
            bool isUserName = false;

            if (StringUtils.IsEmail(target))
                isEmail = true;
            if (StringUtils.IsMobile(target))
                isPhone = true;
            if (!isEmail && !isPhone)
            {
                isUserName = true;
                if (UserManager.GetUserInfo(string.Empty, target) == null)
                {
                    isUserName = false;
                }
            }

            bool retval = false;
            errorMessage = string.Empty;
            string title = string.Empty;
            string template = string.Empty;
            if (!noticeSettingInfo.ByEmail && !noticeSettingInfo.ByMessage && !noticeSettingInfo.ByPhone)
            {
                return true;
            }
            try
            {
                if (noticeSettingInfo.IsSignal)//��ѡ
                {
                    if (!string.IsNullOrEmpty(target) && isEmail && noticeSettingInfo.ByEmail)
                    {
                        title = ParseContentMessage(noticeSettingInfo.EmailTitle, dicReplace);
                        template = ParseContentMessage(noticeSettingInfo.EmailTemplate, dicReplace);
                        retval = retval || UserMailManager.SendMail(target, title, template, out errorMessage);
                        if (!retval)
                        {
                            LogUtils.AddErrorLog(new ErrorLogInfo(0, DateTime.Now, "�ʼ���������ͨ���޷�����", "", "�����ʼ����������ã������������Ƿ���ȷ��"));
                        }
                    }
                    else if (!string.IsNullOrEmpty(target) && isPhone && noticeSettingInfo.ByPhone)
                    {
                        template = ParseContentMessage(noticeSettingInfo.PhoneTemplate, dicReplace);
                        template = SMSServerManager.GetTemplate(template);
                        retval = retval || SMSServerManager.Send(new ArrayList() { target }, template, out errorMessage);
                    }
                    else if (!string.IsNullOrEmpty(target) && isUserName && noticeSettingInfo.ByMessage)
                    {
                        title = ParseContentMessage(noticeSettingInfo.MessageTitle, dicReplace);
                        template = ParseContentMessage(noticeSettingInfo.MessageTemplate, dicReplace);
                        UserMessageManager.SendSystemMessage(target, title, template);
                        retval = retval || true;
                    }

                }
                else//��ѡ
                {
                    if (!string.IsNullOrEmpty(target) && isEmail && noticeSettingInfo.ByEmail)
                    {
                        title = ParseContentMessage(noticeSettingInfo.EmailTitle, dicReplace);
                        template = ParseContentMessage(noticeSettingInfo.EmailTemplate, dicReplace);
                        retval = retval || UserMailManager.SendMail(target, title, template, out errorMessage);
                        if (!retval)
                        {
                            LogUtils.AddErrorLog(new ErrorLogInfo(0, DateTime.Now, "�ʼ���������ͨ���޷�����", "", "�����ʼ����������ã������������Ƿ���ȷ��"));
                        }
                    }
                    if (!string.IsNullOrEmpty(target) && isPhone && noticeSettingInfo.ByPhone)
                    {
                        template = ParseContentMessage(noticeSettingInfo.PhoneTemplate, dicReplace);
                        template = SMSServerManager.GetTemplate(template);
                        retval = retval || SMSServerManager.Send(new ArrayList() { target }, template, out errorMessage);
                    }
                    if (!string.IsNullOrEmpty(target) && isUserName && noticeSettingInfo.ByMessage)
                    {
                        title = ParseContentMessage(noticeSettingInfo.MessageTitle, dicReplace);
                        template = ParseContentMessage(noticeSettingInfo.MessageTemplate, dicReplace);
                        UserMessageManager.SendSystemMessage(target, title, template);
                        retval = retval || true;
                    }
                }
            }
            catch (Exception ex)
            {
                retval = false;
                errorMessage = ex.Message;
            }

            return retval;
        }

        private static Dictionary<string, string> templateFormatStringArray;

        /// <summary>
        /// ģ�����滻���ַ���
        /// </summary>
        public static Dictionary<string, string> TemplateFormatStringArray
        {
            get
            {
                if (templateFormatStringArray == null)
                {
                    templateFormatStringArray = new Dictionary<string, string>();
                    templateFormatStringArray.Add("[UserName]", "�û���");
                    templateFormatStringArray.Add("[DisplayName]", "�û�����");
                    templateFormatStringArray.Add("[AddDate]", "��ʾ����");
                    templateFormatStringArray.Add("[ValidateCode]", "��֤��");
                    templateFormatStringArray.Add("[SiteUrl]", "վ���ַ");
                    templateFormatStringArray.Add("[VerifyUrl]", "��֤��ַ");
                }
                return templateFormatStringArray;
            }
        }

        /// <summary>
        /// �滻������Ϣ���ݵ�ռλ��
        /// </summary>
        /// <param name="input"></param>
        /// <param name="dicReplace"></param>
        /// <returns></returns>
        private static string ParseContentMessage(string input, Dictionary<string, string> dicReplace)
        {
            #region UserName
            if (input.IndexOf("[UserName]") != -1)
            {
                string userName = UserManager.Current.UserName;
                if (dicReplace != null && dicReplace.ContainsKey("userName"))
                {
                    userName = dicReplace["userName"];
                }
                input = input.Replace("[UserName]", userName);
            }
            #endregion
            #region DisplayName
            if (input.IndexOf("[DisplayName]") != -1)
            {
                string displayName = UserManager.Current.DisplayName;
                if (dicReplace != null && dicReplace.ContainsKey("displayName"))
                {
                    displayName = dicReplace["displayName"];
                }
                input = input.Replace("[DisplayName]", displayName);
            }
            #endregion
            #region AddDate
            if (input.IndexOf("[AddDate]") != -1)
            {
                string addDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if (dicReplace != null && dicReplace.ContainsKey("addDate"))
                {
                    addDate = dicReplace["addDate"];
                }
                input = input.Replace("[AddDate]", addDate);
            }
            #endregion
            #region ValidateCode
            if (input.IndexOf("[ValidateCode]") != -1)
            {
                VCManager vc = VCManager.GetInstanceOfValidateCode();
                string validateCode = VCManager.CreateValidateCode(false, 6);
                string cookieName = vc.GetCookieName();
                CookieUtils.SetCookie(cookieName, validateCode, DateTime.Now.AddMinutes(5));//5����
                if (dicReplace != null && dicReplace.ContainsKey("validateCode"))
                {
                    validateCode = dicReplace["validateCode"];
                }
                input = input.Replace("[ValidateCode]", validateCode);
            }
            #endregion
            #region SiteUrl
            if (input.IndexOf("[SiteUrl]") != -1)
            {
                string siteUrl = "#";
                if (dicReplace != null && dicReplace.ContainsKey("siteUrl"))
                {
                    siteUrl = dicReplace["siteUrl"];
                }
                input = input.Replace("[SiteUrl]", siteUrl);
            }
            #endregion
            #region VerifyUrl
            if (input.IndexOf("[VerifyUrl]") != -1)
            {
                string verifyUrl = "#";
                if (dicReplace != null && dicReplace.ContainsKey("verifyUrl"))
                {
                    verifyUrl = dicReplace["verifyUrl"];
                }
                input = input.Replace("[VerifyUrl]", verifyUrl);
            }
            #endregion
            return input;
        }

    }
}
