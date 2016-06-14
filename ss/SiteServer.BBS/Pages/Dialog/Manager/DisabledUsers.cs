using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Collections.Specialized;
using SiteServer.BBS.Core;
using System.Web.UI.WebControls;

using BaiRong.Core;
using SiteServer.BBS.Model;
using System.Collections;

using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using BaiRong.Model;

namespace SiteServer.BBS.Pages.Dialog
{
    public class DisabledUsers : Page
    {
        private PublishmentSystemInfo publishmentSystemInfo;
        private int forumID;
        private int threadID;
        private string postIDArray;
        private bool isPostBack;
        private string action;
        private string userGroupType;

        public static string GetOpenWindowStringDisableUsers(int publishmentSystemID, int forumID, int threadID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("forumID", forumID.ToString());
            arguments.Add("threadID", threadID.ToString());
            arguments.Add("action", "UserPost");
            return DialogUtility.GetOpenWindowStringWithCheckBoxValue(PageUtilityBBS.GetDialogPageUrl(publishmentSystemID, "disabledusers.aspx"), arguments, "postIDArray", "请选择帖子进行操作", 300, 300, "禁止用户", string.Empty);
        }

        public static string GetOpenWindowStringDisableUsersSingle(int publishmentSystemID, int forumID, int threadID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("forumID", forumID.ToString());
            arguments.Add("threadID", threadID.ToString());
            arguments.Add("action", "UserThead");
            return DialogUtility.GetOpenWindowString(PageUtilityBBS.GetDialogPageUrl(publishmentSystemID, "disabledusers.aspx"), arguments, 300, 300, "禁止用户", string.Empty);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(TranslateUtils.ToInt(base.Request.QueryString["publishmentSystemID"]));
                this.forumID = TranslateUtils.ToInt(base.Request.QueryString["forumID"]);
                this.threadID = TranslateUtils.ToInt(base.Request.QueryString["threadID"]);
                this.postIDArray = PageUtils.FilterSqlAndXss(base.Request.QueryString["postIDArray"]);
                this.isPostBack = TranslateUtils.ToBool(base.Request.QueryString["isPostBack"]);
                this.action = PageUtils.FilterSqlAndXss(base.Request.QueryString["action"]);
                if (isPostBack)
                {
                    userGroupType = PageUtils.FilterSqlAndXss(base.Request.Form["userGroupType"]);
                    bool isMessage = TranslateUtils.ToBool(base.Request.Form["isMessage"]);
                    string reason = PageUtils.FilterSqlAndXss(base.Request.Form["reason"]);

                    NameValueCollection attributes = new NameValueCollection();

                    bool isForbid = false;
                    string errorMessage = string.Empty;
                    bool success = false;
                    string url = string.Empty;
                    if (action == "UserPost")
                    {
                        try
                        {
                            ArrayList postIDArrayList = TranslateUtils.StringCollectionToIntArrayList(this.postIDArray);

                            foreach (int thePostID in postIDArrayList)
                            {
                                PostInfo postInfo = DataProvider.PostDAO.GetPostInfo(this.publishmentSystemInfo.PublishmentSystemID, thePostID);
                                this.SetGroup(postInfo.UserName, userGroupType, isMessage, reason);
                            }
                            success = true;
                            url = PageUtilityBBS.GetThreadUrl(this.publishmentSystemInfo.PublishmentSystemID, this.forumID, this.threadID);
                        }
                        catch (Exception ex)
                        {
                            success = false;
                            errorMessage = "禁止用户/解除禁止失败，" + ex.Message;
                        }
                    }
                    else if (action == "UserThead")
                    {
                        try
                        {
                            ThreadInfo threadInfo = DataProvider.ThreadDAO.GetThreadInfo(this.publishmentSystemInfo.PublishmentSystemID, this.threadID);
                            isForbid = this.SetGroup(threadInfo.UserName, userGroupType, isMessage, reason);
                            success = true;
                            url = PageUtilityBBS.GetThreadUrl(this.publishmentSystemInfo.PublishmentSystemID, this.forumID, this.threadID);
                        }
                        catch (Exception ex)
                        {
                            success = false;
                            errorMessage = "禁止用户/解除禁止失败，" + ex.Message;
                        }
                    }
                    attributes.Add("success", success.ToString().ToLower());
                    attributes.Add("url", url);
                    attributes.Add("errorMessage", errorMessage);
                    attributes.Add("isForbid", isForbid.ToString().ToLower());
                    string json = TranslateUtils.NameValueCollectionToJsonString(attributes);
                    base.Response.Write(json);
                    base.Response.End();
                }   
            }
        }

        private bool SetGroup(string userName, string userGroupType, bool isMessage, string reason)
        {
            UserGroupInfo userGroupInfo = UserManager.GetGroupInfo(this.publishmentSystemInfo.GroupSN, userName);
            if ((userGroupInfo.GroupType == EUserGroupType.Administrator) || (userGroupInfo.GroupType == EUserGroupType.SuperModerator) || (userGroupInfo.GroupType == EUserGroupType.Moderator))
            {
                return true;
            }
            if (string.IsNullOrEmpty(userGroupType))  //正常访问
            {
                UserInfo userInfo = UserManager.GetUserInfo(this.publishmentSystemInfo.GroupSN, userName);
                int groupID = UserGroupManager.GetUserGroupIDByCredits(this.publishmentSystemInfo.GroupSN, userInfo.Credits);
                BaiRongDataProvider.UserDAO.SetGroupID(this.publishmentSystemInfo.GroupSN, userName, groupID);
                if (isMessage)
                {
                    string content = StringUtilityBBS.GetSystemMessageContent("更改用户组为正常访问", reason);
                    UserMessageManager.SendSystemMessage(userName, content);
                }
            }
            else  //禁止发言或访问
            {
                //更新用户组
                EUserGroupType groupType = EUserGroupTypeUtils.GetEnumType(userGroupType);
                int groupID = UserGroupManager.GetGroupIDByGroupType(this.publishmentSystemInfo.GroupSN, groupType);
                BaiRongDataProvider.UserDAO.SetGroupID(this.publishmentSystemInfo.GroupSN, userName, groupID);
                if (isMessage)
                {
                    string content = StringUtilityBBS.GetSystemMessageContent((groupType == EUserGroupType.ReadForbidden ? "更改用户组为禁止发言" : "更改用户组为禁止访问"), reason);
                    UserMessageManager.SendSystemMessage(userName, content);
                }
            }
            return false;
        }
      
    }
}
