using System;
using System.Text;
using SiteServer.BBS.Model;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Model;

namespace SiteServer.BBS.Core
{
    public class UserUtils
    {
        private PublishmentSystemInfo publishmentSystemInfo;

        private UserUtils(int publishmentSystemID)
        {
            this.publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
        }

        public static UserUtils GetInstance(int publishmentSystemID)
        {
            return new UserUtils(publishmentSystemID);
        }

        public string UserName
        {
            get
            {
                if (string.IsNullOrEmpty(BaiRongDataProvider.UserDAO.CurrentUserName))
                {
                    return "游客";
                }
                else
                {
                    return BaiRongDataProvider.UserDAO.CurrentUserName;
                }
            }
        }

        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(BaiRongDataProvider.UserDAO.CurrentUserName))
                {
                    return "游客";
                }
                else
                {
                    return BaiRongDataProvider.UserDAO.CurrentUserName.TrimStart('_');
                }
            }
        }

        public string GetUserUrl(string userName)
        {
            return PageUtilityBBS.GetBBSUrl(this.publishmentSystemInfo.PublishmentSystemID, string.Format("user.aspx?publishmentSystemID={0}&userName={1}", this.publishmentSystemInfo.PublishmentSystemID, PageUtils.FilterXSS(userName)));
        }

        public string GetUserAvatarLargeUrl(string userName)
        {
            return PageUtils.GetUserAvatarUrl(this.publishmentSystemInfo.GroupSN, userName, EAvatarSize.Large);
        }

        public string GetUserAvatarMiddleUrl(string userName)
        {
            return PageUtils.GetUserAvatarUrl(this.publishmentSystemInfo.GroupSN, userName, EAvatarSize.Middle);
        }

        public string GetUserAvatarSmallUrl(string userName)
        {
            return PageUtils.GetUserAvatarUrl(this.publishmentSystemInfo.GroupSN, userName, EAvatarSize.Small);
        }

        public string GetGroupName(string userName)
        {
            UserGroupInfo groupInfo = UserManager.GetGroupInfo(this.publishmentSystemInfo.GroupSN, userName);
            if (groupInfo != null)
            {
                return groupInfo.GroupName;
            }
       
            return "游客";
        }

        public int GetCredits(string userName)
        {
            return UserManager.GetCredits(this.publishmentSystemInfo.GroupSN, userName);
        }

        public int GetPostCount(string userName)
        {
            return BBSUserManager.GetPostCount(this.publishmentSystemInfo.GroupSN, userName);
        }

        public int GetPrestige(string userName)
        {
            return BBSUserManager.GetPrestige(this.publishmentSystemInfo.GroupSN, userName);
        }

        public int GetContribution(string userName)
        {
            return BBSUserManager.GetContribution(this.publishmentSystemInfo.GroupSN, userName);
        }

        public int GetCurrency(string userName)
        {
            return BBSUserManager.GetCurrency(this.publishmentSystemInfo.GroupSN, userName);
        }

        public bool IsOnline(string userName)
        {
            return OnlineManager.IsOnlineByUserName(this.publishmentSystemInfo.PublishmentSystemID, userName);
        }

        public string GetOnlineTotal(string userName)
        {
            int seconds = UserManager.GetOnlineSeconds(this.publishmentSystemInfo.GroupSN, userName);
            int minutes = seconds / 60;
            if (minutes == 0)
            {
                minutes = 1;
            }
            if (minutes < 60)
            {
                return minutes + " 分钟";
            }
            else
            {
                return minutes / 60 + " 小时";
            }
        }

        public string GetStars(string userName)
        {
            UserGroupInfo groupInfo = UserManager.GetGroupInfo(this.publishmentSystemInfo.GroupSN, userName);
            if (groupInfo != null)
            {
                return UserManager.GetUserLevelHtml(groupInfo.Stars);
            }
            return string.Empty;
        }

        public bool IsAnonymous
        {
            get
            {
                return BaiRongDataProvider.UserDAO.IsAnonymous;
            }
        }

        public bool IsAllowReplyPost
        {
            get
            {
                UserGroupInfo groupInfo = UserGroupManager.GetCurrent(this.publishmentSystemInfo.GroupSN);
                return groupInfo.Additional.IsAllowReply;
            }
        }

        public string GetLogoutUrl(string returnUrl)
        {
            return PageUtilityBBS.GetLogoutUrl(this.publishmentSystemInfo.PublishmentSystemID, returnUrl);
        }

        public bool IsEditable(string userName)
        {
            if (userName == BaiRongDataProvider.UserDAO.CurrentUserName)
            {
                return true;
            }
            return false;
        }

        public bool IsSignature(bool isSignature, string userName)
        {
            if (isSignature)
            {
                UserInfo userInfo = UserManager.GetUserInfo(this.publishmentSystemInfo.GroupSN, userName);
                if (userInfo != null)
                {
                    return !string.IsNullOrEmpty(userInfo.Signature);
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public string GetSignature(string userName)
        {
            UserInfo userInfo = UserManager.GetUserInfo(this.publishmentSystemInfo.GroupSN, userName);
            if (userInfo != null)
            {
                return userInfo.Signature;
            }
            return string.Empty;
        }

        public bool IsVerifyCodeThread
        {
            get
            {
                ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(this.publishmentSystemInfo.PublishmentSystemID);

                if (additional.IsVerifyCodeThread)
                {
                    if (!BaiRongDataProvider.UserDAO.IsAnonymous)
                    {
                        BBSUserInfo userInfo = BBSUserManager.GetCurrentUserInfo();

                        if (additional.PostVerifyCodeCount > 0 && userInfo.PostCount > additional.PostVerifyCodeCount)
                        {
                            return false;
                        }
                        return true;
                    }
                }
                return false;
            }
        }

        public bool IsVerifyCodePost
        {
            get
            {
                ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(this.publishmentSystemInfo.PublishmentSystemID);

                if (additional.IsVerifyCodePost)
                {
                    if (!BaiRongDataProvider.UserDAO.IsAnonymous)
                    {
                        BBSUserInfo userInfo = BBSUserManager.GetCurrentUserInfo();
                        if (additional.PostVerifyCodeCount > 0 && userInfo.PostCount > additional.PostVerifyCodeCount)
                        {
                            return false;
                        }
                        return true;
                    }
                }
                return false;
            }
        }

        public static string PostVerifyCodeImageUrl
        {
            get
            {
                VCManager vcManager = VCManager.GetInstanceOfLogin();
                return vcManager.GetImageUrl(true) + "&_r=" + StringUtils.GetRandomInt(1, 100);
            }
        }

        public bool IsModerator
        {
            get
            {
                if (!BaiRongDataProvider.UserDAO.IsAnonymous)
                {
                    UserGroupInfo groupInfo = UserManager.GetGroupInfo(BaiRongDataProvider.UserDAO.CurrentGroupSN, BaiRongDataProvider.UserDAO.CurrentUserName);
                    if (groupInfo != null)
                    {
                        if (groupInfo.GroupType == EUserGroupType.Administrator || groupInfo.GroupType == EUserGroupType.SuperModerator || groupInfo.GroupType == EUserGroupType.Moderator)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public bool IsThirdLogin
        {
            get
            {
                if (FileConfigManager.Instance.SSOConfig.IntegrationType == EIntegrationType.GeXia)
                {
                    return true;
                }
                return false;
            }
        }

        public string GetThirdLoginUrl(string returnUrl)
        {
            return FileConfigManager.Instance.SSOConfig.LoginUrl + "?returnUrl=" + PageUtils.AddProtocolToUrl(returnUrl);
        }
    }
}
