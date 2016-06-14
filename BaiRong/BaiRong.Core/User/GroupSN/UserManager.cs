using System.Collections;
using System.Text;
using BaiRong.Core;
using BaiRong.Model;
using System;
using BaiRong.Core.Cryptography;
using System.Collections.Specialized;
using System.Collections.Generic;
using BaiRong.Core.Data.Provider;

namespace BaiRong.Core
{
    public class UserManager
    {
        private UserManager()
        {
        }

        private static object lockObject = new object();

        static public UserInfo Current
        {
            get
            {
                return GetUserInfo(BaiRongDataProvider.UserDAO.CurrentGroupSN, BaiRongDataProvider.UserDAO.CurrentUserName);
            }
        }

        public static string GetAccount(int userID, bool isEmail, bool isMobile)
        {
            string retval = string.Empty;
            if (userID > 0)
            {
                UserInfo userInfo = BaiRongDataProvider.UserDAO.GetUserInfo(userID);
                if (userInfo != null)
                {
                    StringBuilder builder = new StringBuilder();
                    if (isEmail && !string.IsNullOrEmpty(userInfo.Email))
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append("��");
                        }
                        builder.AppendFormat("{0}", userInfo.Email);
                    }
                    if (isEmail && !string.IsNullOrEmpty(userInfo.Mobile))
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append("��");
                        }
                        builder.AppendFormat("{0}", userInfo.Mobile);
                    }
                    if (builder.Length > 0)
                    {
                        retval = string.Format("{0}({1})", userInfo.UserName, builder.ToString());
                    }
                    else
                    {
                        retval = userInfo.DisplayName;
                    }
                }
            }
            return retval;
        }

        public static string GetDisplayName(string groupSN, string userName)
        {
            return GetDisplayName(groupSN, userName, false, false);
        }

        public static string GetDisplayName(string groupSN, string userName, bool isUserName, bool isGroup)
        {
            string retval = string.Empty;
            if (!string.IsNullOrEmpty(userName))
            {
                UserInfo userInfo = UserManager.GetUserInfo(groupSN, userName);
                if (userInfo != null)
                {
                    retval = userInfo.DisplayName;
                    StringBuilder builder = new StringBuilder();
                    if (isUserName)
                    {
                        builder.AppendFormat("�˺ţ�{0}", userName);
                    }
                    if (isGroup)
                    {
                        string groupName = UserGroupManager.GetGroupName(groupSN, userInfo.GroupID);
                        if (!string.IsNullOrEmpty(groupName))
                        {
                            if (builder.Length > 0)
                            {
                                builder.Append("��");
                            }
                            builder.AppendFormat("�û��飺{0}", groupName);
                        }
                    }
                    if (builder.Length > 0)
                    {
                        retval = string.Format("{0}({1})", userInfo.DisplayName, builder.ToString());
                    }
                    else
                    {
                        retval = userInfo.DisplayName;
                    }
                }
            }
            return retval;
        }

        public static string GetFullName(string groupSN, string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                UserInfo userInfo = UserManager.GetUserInfo(groupSN, userName);
                if (userInfo != null)
                {
                    string retval = string.Format("�˺ţ�{0}<br />������{1}", userName, userInfo.DisplayName);
                    string groupName = UserGroupManager.GetGroupName(groupSN, userInfo.GroupID);
                    if (!string.IsNullOrEmpty(groupName))
                    {
                        retval += string.Format("<br />�û��飺{0}", groupName);
                    }
                    return retval;
                }
                return userName;
            }
            return string.Empty;
        }

        public static int GetOnlineSeconds(string groupSN, string userName)
        {
            UserInfo userInfo = UserManager.GetUserInfo(groupSN, userName);
            if (userInfo != null)
            {
                return userInfo.OnlineSeconds;
            }
            return 0;
        }

        public static DateTime GetCreateDate(string groupSN, string userName)
        {
            UserInfo userInfo = UserManager.GetUserInfo(groupSN, userName);
            if (userInfo != null)
            {
                return userInfo.CreateDate;
            }
            return DateTime.Now;
        }

        public static int GetCredits(string groupSN, string userName)
        {
            UserInfo userInfo = UserManager.GetUserInfo(groupSN, userName);
            if (userInfo != null)
            {
                return userInfo.Credits;
            }
            return 0;
        }

        public static int GetGroupID(string groupSN, string userName)
        {
            UserInfo userInfo = UserManager.GetUserInfo(groupSN, userName);
            if (userInfo != null)
            {
                if (userInfo.GroupID == 0)
                {
                    userInfo.GroupID = UserGroupManager.GetUserGroupIDByCredits(groupSN, userInfo.Credits);
                }
                return userInfo.GroupID;
            }
            return 0;
        }

        public static int GetLevelID(string groupSN, string userName)
        {
            UserInfo userInfo = UserManager.GetUserInfo(groupSN, userName);
            if (userInfo != null)
            {
                if (userInfo.LevelID == 0)
                {
                    userInfo.LevelID = UserLevelManager.GetUserLevelIDByCredits(groupSN, userInfo.Credits);
                }
                return userInfo.LevelID;
            }
            return 0;
        }

        public static int GetCurrentGroupID(string groupSN)
        {
            if (BaiRongDataProvider.UserDAO.IsAnonymous)
            {
                return UserGroupManager.GetGroupIDByGroupType(groupSN, EUserGroupType.Guest);
            }
            return UserManager.GetGroupID(groupSN, BaiRongDataProvider.UserDAO.CurrentUserName);
        }

        public static int GetCurrentLevelID(string groupSN)
        {
            if (BaiRongDataProvider.UserDAO.IsAnonymous)
            {
                return UserLevelManager.GetLevelIDByLevelType(groupSN, EUserLevelType.Guest);
            }
            return UserManager.GetLevelID(groupSN, BaiRongDataProvider.UserDAO.CurrentUserName);
        }

        public static UserGroupInfo GetGroupInfo(string groupSN, string userName)
        {
            UserGroupInfo groupInfo = null;
            UserInfo userInfo = UserManager.GetUserInfo(groupSN, userName);
            if (userInfo != null)
            {
                if (userInfo.GroupID > 0)
                {
                    groupInfo = UserGroupManager.GetGroupInfo(groupSN, userInfo.GroupID);
                }
                else
                {
                    groupInfo = UserGroupManager.GetGroupInfoByCredits(groupSN, userInfo.Credits);
                }
            }
            return groupInfo;
        }

        public static UserLevelInfo GetLevelInfo(string groupSN, string userName)
        {
            UserLevelInfo levelInfo = null;
            UserInfo userInfo = UserManager.GetUserInfo(groupSN, userName);
            if (userInfo != null)
            {
                if (userInfo.LevelID > 0)
                {
                    levelInfo = UserLevelManager.GetLevelInfo(groupSN, userInfo.GroupID);
                }
                else
                {
                    levelInfo = UserLevelManager.GetLevelInfoByCredits(groupSN, userInfo.Credits);
                }
            }
            return levelInfo;
        }

        public static string GetUserLevelHtml(int stars)
        {
            StringBuilder builder = new StringBuilder();
            int starCount = 0;
            int leftCount = 0;
            if (stars >= 4)
            {
                starCount = stars / 4;
                leftCount = stars % 4;
            }
            else
            {
                leftCount = stars;
            }
            for (int i = 0; i < starCount; i++)
            {
                builder.AppendFormat(@"<img src=""{0}"" border=""0"" />", PageUtils.GetIconUrl("user/star_level3.gif"));
            }

            if (leftCount == 1)
            {
                builder.AppendFormat(@"<img src=""{0}"" border=""0"" />", PageUtils.GetIconUrl("user/star_level1.gif"));
            }
            else if (leftCount == 2)
            {
                builder.AppendFormat(@"<img src=""{0}"" border=""0"" />", PageUtils.GetIconUrl("user/star_level2.gif"));
            }
            else if (leftCount == 3)
            {
                builder.AppendFormat(@"<img src=""{0}"" border=""0"" />", PageUtils.GetIconUrl("user/star_level2.gif"));
                builder.AppendFormat(@"<img src=""{0}"" border=""0"" />", PageUtils.GetIconUrl("user/star_level1.gif"));
            }

            return builder.ToString();
        }

        public static UserInfo GetUserInfo(string groupSN, string userName)
        {
            return GetUserInfo(groupSN, userName, false);
        }

        public static UserInfo GetUserInfo(string groupSN, string userName, bool flush)
        {
            string userKey = UserManager.GetUserKey(groupSN, userName);

            Dictionary<string, UserInfo> dictionary = GetActiveUserInfo();

            UserInfo userInfo = null;

            if (!flush)
            {
                if (dictionary.ContainsKey(userKey))
                {
                    userInfo = dictionary[userKey];
                }
            }

            if (userInfo == null)
            {
                userInfo = BaiRongDataProvider.UserDAO.GetUserInfo(groupSN, userName);

                if (userInfo != null)
                {
                    UpdateUserInfoCache(dictionary, userKey, userInfo);
                }
            }

            if (userInfo == null)
            {
                userInfo = new UserInfo();
                userInfo.GroupSN = groupSN;
                userInfo.UserName = userName;
            }

            return userInfo;
        }

        private static void UpdateUserInfoCache(Dictionary<string, UserInfo> dictionary, string userKey, UserInfo userInfo)
        {
            lock (lockObject)
            {
                if (dictionary.ContainsKey(userKey))
                {
                    dictionary[userKey] = userInfo;
                }
                else
                {
                    dictionary.Add(userKey, userInfo);
                }
            }
        }

        public static void RemoveCache(string groupSN, string userName)
        {
            string userKey = UserManager.GetUserKey(groupSN, userName);
            Dictionary<string, UserInfo> dictionary = GetActiveUserInfo();

            lock (lockObject)
            {
                dictionary.Remove(userKey);
            }
        }

        public static void RemoveCache(bool isAjaxUrl, string groupSN, string userName)
        {
            string userKey = UserManager.GetUserKey(groupSN, userName);
            Dictionary<string, UserInfo> dictionary = GetActiveUserInfo();

            lock (lockObject)
            {
                dictionary.Remove(userKey);
            }
            if (isAjaxUrl)
            {
                AjaxUrlManager.AddAjaxUrl(PageUtils.API.GetUserClearCacheUrl(), "userName=" + userName);
            }
        }

        const string cacheKey = "BaiRong.Core.UserManager";

        public static void Clear()
        {
            CacheUtils.Remove(cacheKey);
        }

        public static string GetUserKey(string groupSN, string userName)
        {
            return string.Format("{0}.{1}", groupSN, userName);
        }

        public static Dictionary<string, UserInfo> GetActiveUserInfo()
        {
            Dictionary<string, UserInfo> dictionary = CacheUtils.Get(cacheKey) as Dictionary<string, UserInfo>;
            if (dictionary == null)
            {
                dictionary = new Dictionary<string, UserInfo>();
                CacheUtils.Insert(cacheKey, dictionary, null, CacheUtils.HourFactor * 12);
            }
            return dictionary;
        }


        public static bool Register(string groupSN, string userName, string password, string displayName, string email, string mobile, NameValueCollection form, string verifyCode, out string message)
        {
            bool success = false;
            string errorMessage = string.Empty;
            string successMessage = string.Empty;

            try
            {
                if (!UserConfigManager.Instance.Additional.IsRegisterAllowed)
                {
                    message = "����Ա�ѽ�ֹע���»�Ա";
                    return false;
                }

                if (!string.IsNullOrEmpty(verifyCode))
                {
                    if (FileConfigManager.Instance.IsValidateCode)
                    {
                        VCManager vcManager = VCManager.GetInstanceOfLogin();
                        if (!vcManager.IsCodeValid(verifyCode))
                        {
                            errorMessage = "��֤�벻��ȷ������������";
                        }
                    }
                }

                UserInfo userInfo = new UserInfo();
                userInfo.GroupSN = groupSN;
                userInfo.UserName = userName;
                userInfo.Password = password;
                userInfo.DisplayName = displayName;
                userInfo.Email = email;
                userInfo.Mobile = mobile;

                if (form != null)
                {
                    TableInputParser.AddValuesToAttributes(ETableStyle.User, groupSN, null, form, userInfo.Attributes);
                }
                if (UserConfigManager.Additional.RegisterVerifyType == EUserVerifyType.None)
                {
                    userInfo.IsChecked = true;
                }
                else
                {
                    userInfo.IsChecked = false;
                }
                userInfo.IsLockedOut = false;
                userInfo.CreateIPAddress = PageUtils.GetIPAddress();

                if (BaiRongDataProvider.UserDAO.Insert(userInfo, out errorMessage))
                {
                    if (UserConfigManager.Additional.RegisterVerifyType == EUserVerifyType.Email)
                    {
                        string checkCode = EncryptUtils.Md5(userName);
                        string mailBody = UserConfigManager.Additional.RegisterVerifyMailContent;
                        mailBody = mailBody.Replace("[UserName]", userName);
                        mailBody = mailBody.Replace("[DisplayName]", displayName);
                        mailBody = mailBody.Replace("[SiteUrl]", PageUtils.AddProtocolToUrl(PageUtils.GetHost()));
                        mailBody = mailBody.Replace("[AddDate]", DateUtils.GetDateAndTimeString(DateTime.Now));
                        //mailBody = mailBody.Replace("[VerifyUrl]", PageUtils.AddProtocolToUrl(PageUtils.GetUserCenterUrl(string.Format("register.aspx?checkCode={0}&userName={1}", checkCode, userName))));

                        UserMailManager.SendMail(userInfo.Email, "�ʼ�ע��ȷ��", mailBody, out errorMessage);
                    }

                    success = true;

                    if (UserConfigManager.Additional.RegisterVerifyType == EUserVerifyType.None)
                    {
                        successMessage = UserConfigManager.Additional.RegisterWelcome;

                        string content = UserConfigManager.Additional.RegisterWelcomeContent;
                        if (!string.IsNullOrEmpty(content))
                        {
                            content = content.Replace("[UserName]", userName);
                            content = content.Replace("[DisplayName]", displayName);
                            content = content.Replace("[SiteUrl]", PageUtils.AddProtocolToUrl(PageUtils.GetHost()));
                            content = content.Replace("[AddDate]", DateUtils.GetDateAndTimeString(DateTime.Now));

                            //ע��ɹ����Ƿ���û���վ���Ż��ʼ�
                            if (EUserWelcomeTypeUtils.Equals(EUserWelcomeType.Message, UserConfigManager.Additional.RegisterWelcomeType))
                            {
                                UserMessageInfo messageInfo = new UserMessageInfo(0, string.Empty, userName, EUserMessageType.System, 0, false, DateTime.Now, content, DateTime.Now, content);
                                BaiRongDataProvider.UserMessageDAO.Insert(messageInfo);
                            }
                            else if (EUserWelcomeTypeUtils.Equals(EUserWelcomeType.Email, UserConfigManager.Additional.RegisterWelcomeType))
                            {
                                UserMailManager.SendMail(userInfo.Email, UserConfigManager.Additional.RegisterWelcomeTitle, content, out errorMessage);
                            }
                        }
                    }
                    else if (UserConfigManager.Additional.RegisterVerifyType == EUserVerifyType.Email)
                    {
                        string emailUrl = "http://mail." + userInfo.Email.Substring(userInfo.Email.IndexOf('@') + 1);
                        successMessage = string.Format(@"ע�ἤ���ʼ��ѷ��͵���������<a href=""{0}"" target=""_blank"">{1}</a>������������伤�", emailUrl, userInfo.Email);
                    }
                    else if (UserConfigManager.Additional.RegisterVerifyType == EUserVerifyType.Manually)
                    {
                        successMessage = "ע�����ύ����ȴ���ˣ�лл��";
                    }
                }
            }
            catch (Exception ex)
            {
                success = false;
                errorMessage = ex.Message;
            }

            if (success)
            {
                message = successMessage;
            }
            else
            {
                message = "ע��ʧ�ܣ�" + errorMessage;
            }

            return success;
        }

        public static bool RegisterByUserController(string groupSN, string loginName, string password, string email, string mobile, string homeUrl, out bool isRedirectToLogin, out string successMessage, out string errorMessage)
        {
            bool success = false;
            isRedirectToLogin = false;
            errorMessage = string.Empty;
            successMessage = string.Empty;

            try
            {
                if (!UserConfigManager.Instance.Additional.IsRegisterAllowed)
                {
                    errorMessage = "����Ա�ѽ�ֹע���»�Ա";
                    return false;
                }
                UserNoticeSettingInfo validateForRegisterInfo = UserNoticeSettingManager.GetUserNoticeSettingInfoForAPI(EUserNoticeTypeUtils.GetValue(EUserNoticeType.ValidateForRegiste));
                UserInfo userInfo = new UserInfo();
                userInfo.GroupSN = groupSN;
                userInfo.UserName = loginName;
                userInfo.Password = password;
                userInfo.DisplayName = loginName;
                if (!string.IsNullOrEmpty(email))
                {
                    if (StringUtils.IsEmail(email))
                    {
                        userInfo.Email = email;
                    }
                }
                else { userInfo.Email = email; }
                if (!string.IsNullOrEmpty(mobile))
                {
                    if (StringUtils.IsMobile(mobile))
                    {
                        userInfo.Mobile = mobile;
                    }
                }
                else { userInfo.Mobile = mobile; }
                if (!UserConfigManager.Additional.RegisterAuditType && !validateForRegisterInfo.ByEmail && !validateForRegisterInfo.ByMessage && !validateForRegisterInfo.ByPhone)
                {
                    userInfo.IsChecked = true;
                    isRedirectToLogin = true;
                }
                else
                {
                    userInfo.IsChecked = false;
                }
                userInfo.IsLockedOut = false;
                userInfo.CreateIPAddress = PageUtils.GetIPAddress();

                if (BaiRongDataProvider.UserDAO.Insert(userInfo, out errorMessage))
                {
                    #region Old
                    //if (UserConfigManager.Additional.RegisterVerifyType == EUserVerifyType.Email)
                    //{
                    //string checkCode = EncryptUtils.Md5(loginName);
                    //string mailBody = UserConfigManager.Additional.RegisterVerifyMailContent;
                    //mailBody = mailBody.Replace("[UserName]", loginName);
                    //mailBody = mailBody.Replace("[DisplayName]", loginName);
                    //mailBody = mailBody.Replace("[SiteUrl]", PageUtils.AddProtocolToUrl(PageUtils.GetHost()));
                    //mailBody = mailBody.Replace("[AddDate]", DateUtils.GetDateAndTimeString(DateTime.Now));
                    //mailBody = mailBody.Replace("[VerifyUrl]", PageUtils.AddProtocolToUrl(PageUtils.GetUserCenterUrl(string.Format("register.aspx?checkCode={0}&userName={1}", checkCode, loginName))));

                    //UserMailManager.SendMail(userInfo.Email, "�ʼ�ע��ȷ��", mailBody, out errorMessage);
                    //}

                    //success = true;

                    //if (UserConfigManager.Additional.RegisterVerifyType == EUserVerifyType.None)
                    //{
                    //    successMessage = UserConfigManager.Additional.RegisterWelcome;

                    //    string content = UserConfigManager.Additional.RegisterWelcomeContent;
                    //    if (!string.IsNullOrEmpty(content))
                    //    {
                    //        content = content.Replace("[UserName]", loginName);
                    //        content = content.Replace("[DisplayName]", loginName);
                    //        content = content.Replace("[SiteUrl]", PageUtils.AddProtocolToUrl(PageUtils.GetHost()));
                    //        content = content.Replace("[AddDate]", DateUtils.GetDateAndTimeString(DateTime.Now));

                    //        //ע��ɹ����Ƿ���û���վ���Ż��ʼ�
                    //        if (EUserWelcomeTypeUtils.Equals(EUserWelcomeType.Message, UserConfigManager.Additional.RegisterWelcomeType))
                    //        {
                    //            UserMessageInfo messageInfo = new UserMessageInfo(0, string.Empty, loginName, EUserMessageType.System, 0, false, DateTime.Now, content, DateTime.Now, content);
                    //            BaiRongDataProvider.UserMessageDAO.Insert(messageInfo);
                    //        }
                    //        else if (EUserWelcomeTypeUtils.Equals(EUserWelcomeType.Email, UserConfigManager.Additional.RegisterWelcomeType))
                    //        {
                    //            UserMailManager.SendMail(userInfo.Email, UserConfigManager.Additional.RegisterWelcomeTitle, content, out errorMessage);
                    //        }
                    //    }
                    //}
                    //else if (UserConfigManager.Additional.RegisterVerifyType == EUserVerifyType.Email)
                    //{
                    //    string emailUrl = "http://mail." + userInfo.Email.Substring(userInfo.Email.IndexOf('@') + 1);
                    //    successMessage = string.Format(@"ע�ἤ���ʼ��ѷ��͵���������<a href=""{0}"" target=""_blank"">{1}</a>������������伤�", emailUrl, userInfo.Email);
                    //}
                    //else if (UserConfigManager.Additional.RegisterVerifyType == EUserVerifyType.Manually)
                    //{
                    //    successMessage = "ע�����ύ����ȴ���ˣ�лл��";
                    //} 
                    #endregion

                    //�����˹����
                    if (UserConfigManager.Additional.RegisterAuditType)
                    {
                        successMessage = "ע�����ύ����ȴ���ˣ�лл��";
                        success = true;
                    }
                    else
                    {

                        if (validateForRegisterInfo != null)
                        {
                            Dictionary<string, string> dicReplace = new Dictionary<string, string>();
                            string tempUrl = string.Format("index={0}", homeUrl);
                            dicReplace.Add("siteUrl", PageUtils.AddProtocolToUrl(PageUtils.GetHost()));
                            string checkCode = EncryptUtils.Md5(userInfo.UserName);
                            dicReplace.Add("verifyUrl", PageUtils.AddProtocolToUrl(APIPageUtils.ParseUrl(PageUtils.GetPlatformUserServiceUrl(string.Format("UserRegisterValidateEmail&checkCode={0}&userName={1}&groupSN={2}&returnUrl={3}", checkCode, userInfo.UserName, string.Empty, StringUtils.ValueToUrl(tempUrl))))));
                            List<string> targetList = new List<string>();
                            targetList.Add(userInfo.Email);
                            targetList.Add(userInfo.Mobile);
                            success = UserNoticeSettingManager.SendMsg(validateForRegisterInfo, targetList, dicReplace, out errorMessage);
                        }
                    }


                    if (validateForRegisterInfo != null && !validateForRegisterInfo.ByEmail && !validateForRegisterInfo.ByMessage && !validateForRegisterInfo.ByPhone)
                    {
                        //����Ա����ˣ�Ҳ���������䣬������֤��ע��ͨ��������ע�Ỷӭ��Ϣ
                        UserNoticeSettingInfo welcomeNoticeSettingInfo = UserNoticeSettingManager.GetUserNoticeSettingInfoForAPI(EUserNoticeTypeUtils.GetValue(EUserNoticeType.WelcomeAfterRegiste));
                        if (welcomeNoticeSettingInfo != null)
                        {
                            Dictionary<string, string> dicReplace = new Dictionary<string, string>();
                            dicReplace.Add("siteUrl", PageUtils.AddProtocolToUrl(PageUtils.GetHost()));
                            dicReplace.Add("userName", userInfo.UserName);
                            List<string> targetList = new List<string>();
                            targetList.Add(userInfo.Email);
                            targetList.Add(userInfo.Mobile);
                            targetList.Add(userInfo.UserName);
                            UserNoticeSettingManager.SendMsg(welcomeNoticeSettingInfo, targetList, dicReplace, out errorMessage);

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                success = false;
                errorMessage = ex.Message;
            }

            errorMessage = "ע��ʧ�ܣ�" + errorMessage;

            return success;
        }

        public static bool RegisterByUserApi(string groupSN, string loginName, string password, string email, string mobile, string verifyCode, string homeUrl, string waitUrl, out bool isRedirectToLogin, out string successMessage, out string errorMessage)
        {
            bool success = false;
            isRedirectToLogin = false;
            errorMessage = string.Empty;
            successMessage = string.Empty;

            try
            {
                if (!UserConfigManager.Instance.Additional.IsRegisterAllowed)
                {
                    errorMessage = "����Ա�ѽ�ֹע���»�Ա";
                    return false;
                }


                if (!string.IsNullOrEmpty(verifyCode))
                {
                    if (FileConfigManager.Instance.IsValidateCode)
                    {
                        VCManager vcManager = VCManager.GetInstanceOfLogin();
                        if (!vcManager.IsCodeValid(verifyCode))
                        {
                            errorMessage = "��֤�벻��ȷ������������";
                        }
                        else
                        {
                            UserNoticeSettingInfo validateForRegisterInfo = UserNoticeSettingManager.GetUserNoticeSettingInfoForAPI(EUserNoticeTypeUtils.GetValue(EUserNoticeType.ValidateForRegiste));
                            UserInfo userInfo = new UserInfo();
                            userInfo.GroupSN = groupSN;
                            userInfo.UserName = loginName;
                            userInfo.Password = password;
                            userInfo.DisplayName = loginName;
                            if (!string.IsNullOrEmpty(email))
                            {
                                if (StringUtils.IsEmail(email))
                                {
                                    userInfo.Email = email;
                                }
                            }
                            else { userInfo.Email = email; }
                            if (!string.IsNullOrEmpty(mobile))
                            {
                                if (StringUtils.IsMobile(mobile))
                                {
                                    userInfo.Mobile = mobile;
                                }
                            }
                            else { userInfo.Mobile = mobile; }
                            if (!UserConfigManager.Additional.RegisterAuditType && !validateForRegisterInfo.ByEmail && !validateForRegisterInfo.ByMessage && !validateForRegisterInfo.ByPhone)
                            {
                                userInfo.IsChecked = true;
                                isRedirectToLogin = true;
                            }
                            else
                            {
                                userInfo.IsChecked = false;
                            }
                            userInfo.IsLockedOut = false;
                            userInfo.CreateIPAddress = PageUtils.GetIPAddress();


                            if (BaiRongDataProvider.UserDAO.Insert(userInfo, out errorMessage))
                            {
                                //�����˹����
                                if (UserConfigManager.Additional.RegisterAuditType)
                                {
                                    successMessage = "ע�����ύ����ȴ���ˣ�лл��";
                                    success = true;
                                }
                                else
                                {

                                    if (validateForRegisterInfo != null)
                                    {
                                        Dictionary<string, string> dicReplace = new Dictionary<string, string>();
                                        string tempUrl = string.Format("index={0}&wait={1}", homeUrl, waitUrl); ;
                                        dicReplace.Add("siteUrl", PageUtils.AddProtocolToUrl(PageUtils.GetHost()));
                                        string checkCode = EncryptUtils.Md5(userInfo.UserName);
                                        dicReplace.Add("verifyUrl", PageUtils.AddProtocolToUrl(APIPageUtils.ParseUrl(PageUtils.GetPlatformUserServiceUrl(string.Format("UserRegisterValidateEmail&checkCode={0}&userName={1}&groupSN={2}&returnUrl={3}", checkCode, userInfo.UserName, string.Empty, StringUtils.ValueToUrl(tempUrl))))));
                                        List<string> targetList = new List<string>();
                                        targetList.Add(userInfo.Email);
                                        targetList.Add(userInfo.Mobile);
                                        success = UserNoticeSettingManager.SendMsg(validateForRegisterInfo, targetList, dicReplace, out errorMessage);
                                    }
                                }


                                if (validateForRegisterInfo != null && !validateForRegisterInfo.ByEmail && !validateForRegisterInfo.ByMessage && !validateForRegisterInfo.ByPhone)
                                {
                                    //����Ա����ˣ�Ҳ���������䣬������֤��ע��ͨ��������ע�Ỷӭ��Ϣ
                                    UserNoticeSettingInfo welcomeNoticeSettingInfo = UserNoticeSettingManager.GetUserNoticeSettingInfoForAPI(EUserNoticeTypeUtils.GetValue(EUserNoticeType.WelcomeAfterRegiste));
                                    if (welcomeNoticeSettingInfo != null)
                                    {
                                        Dictionary<string, string> dicReplace = new Dictionary<string, string>();
                                        dicReplace.Add("siteUrl", PageUtils.AddProtocolToUrl(PageUtils.GetHost()));
                                        dicReplace.Add("userName", userInfo.UserName);
                                        List<string> targetList = new List<string>();
                                        targetList.Add(userInfo.Email);
                                        targetList.Add(userInfo.Mobile);
                                        targetList.Add(userInfo.UserName);
                                        UserNoticeSettingManager.SendMsg(welcomeNoticeSettingInfo, targetList, dicReplace, out errorMessage);

                                    }
                                }
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                success = false;
                errorMessage = ex.Message;
            }

            if (success)
            {

            }
            else
            {
                errorMessage = "ע��ʧ�ܣ�" + errorMessage;
            }
            return success;
        }


        public static bool ForgetPassword(string groupSN, string loginName, out string successMessage, out string errorMessage)
        {
            bool success = false;
            errorMessage = string.Empty;
            successMessage = string.Empty;

            try
            {
                if (!BaiRongDataProvider.UserDAO.IsExists(groupSN, loginName))
                {
                    throw new Exception(string.Format("�û� {0} �����ڣ�����������", loginName));
                }
                else
                {
                    UserInfo info = BaiRongDataProvider.UserDAO.GetUserInfoByNameOrEmailOrMobile(groupSN, loginName);
                    if (info != null)
                    {
                        int userID = info.UserID;
                        string password = BaiRongDataProvider.UserDAO.GetPassword(userID);
                        string email = info.Email;
                        if (!string.IsNullOrEmpty(email))
                        {
                            success = UserMailManager.SendMail(email, "��վ��������", string.Format(@"
���� <a href=""{0}"" target=""_blank"">{0}</a> ��ע���û�������Ϊ <strong>{1}</strong>�����μǡ�<br />
�����������յ����ʼ����ܿ����������û��ڳ�����������ʱ�������������ĵ����ʼ���ַ�������û���������������������һ���Ĳ��������Է��ĵغ��Դ˵����ʼ���<br />
��л��ʹ�����ǵķ���<br />
���ʼ�Ϊȷ���ʼ������ǲ���Դ��ʼ��Ļظ����д𸴡�
", PageUtils.AddProtocolToUrl(PageUtils.GetHost()), password), out errorMessage);

                            string emailUrl = "http://mail." + email.Substring(email.IndexOf('@') + 1);
                            successMessage = string.Format(@"�����ѷ��͵��������� <a href=""{0}"" target=""_blank"">{1}</a> �������������鿴��", emailUrl, email);
                        }
                        else
                        {
                            string mobile = info.Mobile;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                success = false;
                errorMessage = ex.Message;
            }

            if (string.IsNullOrEmpty(errorMessage))
            {
                errorMessage = "�������Ա��ϵ";
            }
            errorMessage = "ȡ������ʧ�ܣ�" + errorMessage;

            return success;
        }

        public static bool IsAdministrator
        {
            get
            {
                if (!BaiRongDataProvider.UserDAO.IsAnonymous)
                {
                    return UserGroupManager.GetGroupType(UserManager.Current.GroupSN, UserManager.Current.GroupID) == EUserGroupType.Administrator;
                }
                return false;
            }
        }

        public static string GetUserName(int userID)
        {
            return BaiRongDataProvider.UserDAO.GetUserName(userID);
        }

        public static bool UpdateUserInfo(string groupSN, string userName, string email, string mobile, string signature, out string successMessage, out string errorMessage)
        {
            bool success = false;
            errorMessage = string.Empty;
            successMessage = string.Empty;

            try
            {
                UserInfo userInfo = new UserInfo();
                userInfo.GroupSN = groupSN;
                userInfo.UserName = userName;
                userInfo.DisplayName = userName;
                if (!string.IsNullOrEmpty(email))
                {
                    if (StringUtils.IsEmail(email))
                    {
                        userInfo.Email = email;
                    }
                }
                else { userInfo.Email = email; }
                if (!string.IsNullOrEmpty(mobile))
                {
                    if (StringUtils.IsMobile(mobile))
                    {
                        userInfo.Mobile = mobile;
                    }
                }
                else { userInfo.Mobile = mobile; }
                userInfo.Signature = signature;

                BaiRongDataProvider.UserDAO.Update(userInfo);
                success = true;
                successMessage = "�����Ѿ����³ɹ���";

            }
            catch (Exception ex)
            {
                success = false;
                errorMessage = ex.Message;
            }

            errorMessage = "����ʧ�ܣ�" + errorMessage;

            return success;
        }

        public static bool UpdateUserBasicInfo(string groupSN, string userName, string signature, out string successMessage, out string errorMessage)
        {
            bool success = false;
            errorMessage = string.Empty;
            successMessage = string.Empty;

            try
            {
                UserInfo userInfo = UserManager.Current;
                //userInfo.GroupSN = groupSN;
                //userInfo.UserName = userName;
                //if (!string.IsNullOrEmpty(email))
                //{
                //    if (StringUtils.IsEmail(email))
                //    {
                //        userInfo.Email = email;
                //    }
                //}
                //else { userInfo.Email = email; }
                //if (!string.IsNullOrEmpty(mobile))
                //{
                //    if (StringUtils.IsMobile(mobile))
                //    {
                //        userInfo.Mobile = mobile;
                //    }
                //}
                //else { userInfo.Mobile = mobile; }
                userInfo.Signature = signature;

                BaiRongDataProvider.UserDAO.UpdateBasic(userInfo, out errorMessage);
                success = true;
                successMessage = "�����Ѿ����³ɹ���";

            }
            catch (Exception ex)
            {
                success = false;
                errorMessage = ex.Message;
            }

            errorMessage = "����ʧ�ܣ�" + errorMessage;

            return success;
        }

        public static bool UpdateUserInfo(string groupSN, string bloodType, int height, string maritalStatus, string education, string graduation, string provinceValue, string postalAddress, string QQ, string WeiBo, string WeiXin, string gender, string organization, string department, string position, string interects, out string successMessage, out string errorMessage)
        {
            bool success = false;
            errorMessage = string.Empty;
            successMessage = string.Empty;

            try
            {
                UserInfo userInfo = UserManager.Current;
                userInfo.GroupSN = groupSN;

                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("bloodType", bloodType);
                nvc.Add("height", height.ToString());
                nvc.Add("maritalStatus", maritalStatus);
                nvc.Add("education", education);
                nvc.Add("graduation", graduation);
                nvc.Add("provinceValue", provinceValue);
                nvc.Add("address", postalAddress);
                nvc.Add("QQ", QQ);
                nvc.Add("WeiBo", WeiBo);
                nvc.Add("WeiXin", WeiXin);
                nvc.Add("gender", gender);
                nvc.Add("organization", organization);
                nvc.Add("department", department);
                nvc.Add("position", position);
                nvc.Add("interects", interects);

                TableInputParser.AddValuesToAttributes(ETableStyle.User, BaiRongDataProvider.UserDAO.TABLE_NAME, null, nvc, userInfo.Attributes);

                BaiRongDataProvider.UserDAO.Update(userInfo);
                success = true;
                successMessage = "�����Ѿ����³ɹ���";

            }
            catch (Exception ex)
            {
                success = false;
                errorMessage = ex.Message;
            }

            errorMessage = "����ʧ�ܣ�" + errorMessage;

            return success;
        }

        public static string CalculateAccountSafeLevel(string groupSN, string userName)
        {
            try
            {
                UserInfo info = GetUserInfo(groupSN, userName);
                if (info != null)
                {
                    int score = 0;

                    //���븴�Ӷ�40����60%����80%����100%
                    if (EUserPasswordRestrictionUtils.IsValid(info.Password, EUserPasswordRestriction.LetterAndDigitAndSymbol))
                    {
                        score += (int)Math.Round(40 * 1.0);
                    }
                    else if (EUserPasswordRestrictionUtils.IsValid(info.Password, EUserPasswordRestriction.LetterAndDigit))
                    {
                        score += (int)Math.Round(40 * 0.8);
                    }
                    else
                    {
                        score += (int)Math.Round(40 * 0.6);
                    }
                    //������֤20
                    if (info.IsBindEmail)
                    {
                        score += 20;
                    }
                    //�ֻ���20
                    if (info.IsBindPhone)
                    {
                        score += 20;
                    }
                    //�ܱ�20
                    if (info.IsSetSCQU)
                    {
                        score += 20;
                    }

                    //EUserAccountSafeLevelType level = EUserAccountSafeLevelTypeUtils.GetSaveLevel(score);
                    return EUserAccountSafeLevelTypeUtils.GetSaveNumLevel(score);
                }
                else
                    throw new Exception("û�и��û�");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string CalculateAccountSafeLevel()
        {
            UserInfo info = Current;
            return CalculateAccountSafeLevel(info.GroupSN, info.UserName);
        }

        public static string CalculatePasswordComplex(string groupSN, string userName)
        {
            try
            {
                string retval = "1";
                UserInfo info = GetUserInfo(groupSN, userName);
                if (info != null)
                {

                    //���븴�Ӷ�40����60%����80%����100%
                    if (EUserPasswordRestrictionUtils.IsValid(info.Password, EUserPasswordRestriction.LetterAndDigitAndSymbol))
                    {
                        retval = "3";
                    }
                    else if (EUserPasswordRestrictionUtils.IsValid(info.Password, EUserPasswordRestriction.LetterAndDigit))
                    {
                        retval = "2";
                    }
                    else
                    {
                        retval = "1";
                    }



                    return retval;
                }
                else
                    throw new Exception("û�и��û�");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string CalculatePasswordComplex()
        {
            UserInfo info = Current;
            return CalculatePasswordComplex(info.GroupSN, info.UserName);
        }

        #region Ͷ�����

        /// <summary>
        /// ��ȡ�û�����Ϣ
        /// </summary>
        static public UserNewGroupInfo CurrentNewGroup
        {
            get
            {
                if (Current.NewGroupID != 0)
                    return BaiRongDataProvider.UserNewGroupDAO.GetInfo(Current.NewGroupID);
                else
                    return null;
            }
        }

        /// <summary>
        /// ��ȡ�û���ĸ��������
        /// </summary>
        static public string CurrentNewGroupMLibAddUser
        {
            get
            {
                if (!ConfigManager.Additional.IsUnifiedMLibAddUser)
                {
                    return ConfigManager.Additional.UnifiedMLibAddUser;
                }
                else
                    if (CurrentNewGroup != null)
                        if (!string.IsNullOrEmpty(CurrentNewGroup.SetXML) && !string.IsNullOrEmpty(CurrentNewGroup.Additional.MLibAddUser))
                            return CurrentNewGroup.Additional.MLibAddUser;
                        else
                            return ConfigManager.Additional.UnifiedMLibAddUser;
                    else
                        return ConfigManager.Additional.UnifiedMLibAddUser;
            }
        }

        /// <summary>
        /// ��ǰ�û���ǰʱ���Ƿ����Ͷ��
        /// </summary>
        static public bool CurrentCanDoByValidityDate
        {
            get
            {
                bool canMlib = false;
                if (Current.MLibValidityDate != DateUtils.SqlMinValue)
                {
                    canMlib = (DateTime.Now - Current.MLibValidityDate).Days <= 0;
                }
                else
                {
                    if (!ConfigManager.Additional.IsUnifiedMLibValidityDate)
                    {
                        if (ConfigManager.Additional.UnifiedMLibValidityDate == 0)
                            canMlib = true;
                        else
                        {
                            if (Current.CreateDate > ConfigManager.Additional.MLibStartTime)
                            {
                                canMlib = (DateTime.Now - Current.CreateDate.AddMonths(ConfigManager.Additional.UnifiedMLibValidityDate)).Days <= 0;
                            }
                            else
                                canMlib = (DateTime.Now - ConfigManager.Additional.MLibStartTime.AddMonths(ConfigManager.Additional.UnifiedMLibValidityDate)).Days <= 0;

                        }
                    }
                    else
                    {
                        if (CurrentNewGroup != null)
                        {
                            if (!string.IsNullOrEmpty(CurrentNewGroup.SetXML))
                            {
                                if (CurrentNewGroup.Additional.MLibValidityDate == 0)
                                    canMlib = true;
                                else
                                    if (Current.CreateDate > ConfigManager.Additional.MLibStartTime)
                                    {
                                        canMlib = (DateTime.Now - Current.CreateDate.AddMonths(CurrentNewGroup.Additional.MLibValidityDate)).Days <= 0;
                                    }
                                    else
                                        canMlib = (DateTime.Now - ConfigManager.Additional.MLibStartTime.AddMonths(CurrentNewGroup.Additional.MLibValidityDate)).Days <= 0;
                            }
                            else
                                if (ConfigManager.Additional.UnifiedMLibValidityDate == 0)
                                    canMlib = true;
                                else
                                {
                                    if (Current.CreateDate > ConfigManager.Additional.MLibStartTime)
                                    {
                                        canMlib = (DateTime.Now - Current.CreateDate.AddMonths(ConfigManager.Additional.UnifiedMLibValidityDate)).Days <= 0;
                                    }
                                    else
                                        canMlib = (DateTime.Now - ConfigManager.Additional.MLibStartTime.AddMonths(ConfigManager.Additional.UnifiedMLibValidityDate)).Days <= 0;
                                }
                        }
                        else
                        {
                            if (ConfigManager.Additional.UnifiedMLibValidityDate == 0)
                                canMlib = true;
                            else
                            {
                                if (Current.CreateDate > ConfigManager.Additional.MLibStartTime)
                                {
                                    canMlib = (DateTime.Now - Current.CreateDate.AddMonths(ConfigManager.Additional.UnifiedMLibValidityDate)).Days <= 0;
                                }
                                else
                                    canMlib = (DateTime.Now - ConfigManager.Additional.MLibStartTime.AddMonths(ConfigManager.Additional.UnifiedMLibValidityDate)).Days <= 0;
                            }
                        }
                    }
                }
                return canMlib;
            }
        }


        static public bool CurrentCanDoByMLibNum
        {
            get
            {
                int mlibNum = 0;
                if (!ConfigManager.Additional.IsUnifiedMLibNum)
                {
                    mlibNum = ConfigManager.Additional.UnifiedMlibNum;
                }
                else
                {
                    if (!ConfigManager.Additional.IsUnifiedMLibNum)
                    {
                        if (ConfigManager.Additional.UnifiedMlibNum == 0)
                            mlibNum = ConfigManager.Additional.UnifiedMlibNum;
                        else
                            mlibNum = ConfigManager.Additional.UnifiedMlibNum;
                    }
                    else
                    {
                        if (CurrentNewGroup != null)
                        {
                            if (!string.IsNullOrEmpty(CurrentNewGroup.SetXML))
                                mlibNum = CurrentNewGroup.Additional.MlibNum;
                            else
                                mlibNum = ConfigManager.Additional.UnifiedMlibNum;
                        }
                        else
                            mlibNum = ConfigManager.Additional.UnifiedMlibNum;
                    }
                }
                if (mlibNum != 0)
                {
                    if (UserManager.Current.MLibNum >= mlibNum)
                    {
                        return false;
                    }
                    else
                        return true;
                }
                else
                    return true;
            }
        }

        /// <summary>
        /// ��ȡ�����û���ĸ��������
        /// </summary>
        static public ArrayList NewGroupMLibAddUserNames
        {
            get
            {
                ArrayList mLibAddUserNames = new ArrayList();
                if (!string.IsNullOrEmpty(ConfigManager.Additional.UnifiedMLibAddUser))
                    mLibAddUserNames.Add(ConfigManager.Additional.UnifiedMLibAddUser);

                ArrayList list = BaiRongDataProvider.UserNewGroupDAO.GetInfoList(" and SetXML!=''");
                foreach (UserNewGroupInfo info in list)
                {
                    if (!mLibAddUserNames.Contains(info.Additional.MLibAddUser))
                        mLibAddUserNames.Add(info.Additional.MLibAddUser);
                }
                return mLibAddUserNames;
            }
        }


        /// <summary>
        /// �����û���ĸ���������˺ű���������ǰ�û����µ��û�����Ͷ��
        /// </summary>
        static public bool CurrentCanDoByAdmin
        {
            get
            {
                string admin = CurrentNewGroupMLibAddUser;
                AdministratorInfo uinfo = BaiRongDataProvider.AdministratorDAO.GetAdministratorInfo(admin);
                if (uinfo.IsLockedOut)
                    return false;
                else
                    return true;
            }
        }
        #endregion
    }
}
