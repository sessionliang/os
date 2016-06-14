using System;
using System.Collections;
using System.Text;
using System.Data;
using SiteServer.BBS.Model;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.BBS.Pages;


using SiteServer.CMS.Core;

namespace SiteServer.BBS.Core
{
    public class AccessManager
    {
        public static bool IsViewable(int publishmentSystemID, ForumInfo forumInfo, string rawUrl, out string redirectUrl)
        {
            bool isViewable = true;
            redirectUrl = string.Empty;
            if (!string.IsNullOrEmpty(forumInfo.Additional.AccessUserNames))
            {
                if (BaiRongDataProvider.UserDAO.IsAnonymous || !TranslateUtils.StringCollectionToArrayList(forumInfo.Additional.AccessUserNames).Contains(BaiRongDataProvider.UserDAO.CurrentUserName))
                {
                    isViewable = false;
                    redirectUrl = LoginPage.GetLoginUrl(publishmentSystemID, "本版块只有特定用户可以访问，请使用对应账号登录。", rawUrl);
                }
            }
            if (isViewable && !string.IsNullOrEmpty(forumInfo.Additional.AccessPassword))
            {
                string password = CookieUtils.GetCookie("access_password_" + forumInfo.ForumID);
                if (password != forumInfo.Additional.AccessPassword)
                {
                    isViewable = false;
                    redirectUrl = AccessPage.GetAccessUrl(publishmentSystemID, forumInfo.ForumID, rawUrl);
                }
            }
            if (isViewable)
            {
                string groupSN = PublishmentSystemManager.GetGroupSN(publishmentSystemID);
                UserGroupInfo groupInfo = UserGroupManager.GetCurrent(groupSN);
                string forbidden = PermissionManager.GetForbidden(forumInfo.PublishmentSystemID, groupInfo.GroupID, forumInfo.ForumID);
                if (StringUtils.In(forbidden, EPermissionUtils.GetValue(EPermission.View)))
                {
                    isViewable = false;
                    redirectUrl = LoginPage.GetLoginUrl(publishmentSystemID, AccessManager.GetErrorMessage(EPermission.View, groupInfo), rawUrl);
                }
            }
            return isViewable;
        }

        public static void SetViewPassword(int forumID, string password)
        {
            CookieUtils.SetCookie("view_password_" + forumID, password, DateTime.Now.AddDays(7));
        }

        public static bool IsThreadAddable(int publishmentSystemID, UserGroupInfo groupInfo, int forumID, string rawUrl, out string redirectUrl)
        {
            bool isAddable = true;
            redirectUrl = string.Empty;
            if (!groupInfo.Additional.IsAllowPost)
            {
                isAddable = false;
                redirectUrl = LoginPage.GetLoginUrl(publishmentSystemID, AccessManager.GetErrorMessage(EPermission.AddThread, groupInfo), rawUrl);
            }
            else
            {
                string forbidden = PermissionManager.GetForbidden(publishmentSystemID, groupInfo.GroupID, forumID);
                if (StringUtils.In(forbidden, EPermissionUtils.GetValue(EPermission.AddThread)) && !groupInfo.Additional.IsAllowPost)
                {
                    isAddable = false;
                    redirectUrl = LoginPage.GetLoginUrl(publishmentSystemID, AccessManager.GetErrorMessage(EPermission.AddThread, groupInfo), rawUrl);
                }
            }
            return isAddable;
        }

        public static bool IsPollAddable(int publishmentSystemID, UserGroupInfo groupInfo, int forumID, string rawUrl, out string redirectUrl)
        {
            bool isAddable = true;
            redirectUrl = string.Empty;
            if (!groupInfo.Additional.IsAllowPoll)
            {
                isAddable = false;
                redirectUrl = LoginPage.GetLoginUrl(publishmentSystemID, AccessManager.GetErrorMessage(EPermission.AddPoll, groupInfo), rawUrl);
            }
            else
            {
                isAddable = IsThreadAddable(publishmentSystemID, groupInfo, forumID, rawUrl, out redirectUrl);
            }
            return isAddable;
        }

        public static bool IsPostAddable(int publishmentSystemID, UserGroupInfo groupInfo, int forumID, string rawUrl, out string redirectUrl)
        {
            bool isAddable = true;
            redirectUrl = string.Empty;
            if (!groupInfo.Additional.IsAllowReply)
            {
                isAddable = false;
                redirectUrl = LoginPage.GetLoginUrl(publishmentSystemID, AccessManager.GetErrorMessage(EPermission.AddPost, groupInfo), rawUrl);
            }
            else
            {
                string forbidden = PermissionManager.GetForbidden(publishmentSystemID, groupInfo.GroupID, forumID);
                if (StringUtils.In(forbidden, EPermissionUtils.GetValue(EPermission.AddPost)) && !groupInfo.Additional.IsAllowReply)
                {
                    isAddable = false;
                    redirectUrl = LoginPage.GetLoginUrl(publishmentSystemID, AccessManager.GetErrorMessage(EPermission.AddPost, groupInfo), rawUrl);
                }
            }
            return isAddable;
        }

        public static string GetErrorMessage(EPermission permission, UserGroupInfo groupInfo)
        {
            string retval = string.Empty;
            if (permission == EPermission.View)
            {
                retval = string.Format("本版块{0}无法访问，请使用对应账号登录。", groupInfo.GroupName);
            }
            else if (permission == EPermission.AddThread)
            {
                retval = string.Format("本版块{0}禁止发新话题，请使用对应账号登录。", groupInfo.GroupName);
            }
            else if (permission == EPermission.AddPoll)
            {
                retval = string.Format("本版块{0}禁止发新投票，请使用对应账号登录。", groupInfo.GroupName);
            }
            else if (permission == EPermission.AddPost)
            {
                retval = string.Format("本版块{0}禁止发表回复，请使用对应账号登录。", groupInfo.GroupName);
            }
            return retval;
        }
    }
}
