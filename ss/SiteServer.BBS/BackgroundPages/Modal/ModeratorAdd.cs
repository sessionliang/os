using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using SiteServer.BBS.Model;
using System.Collections.Specialized;
using SiteServer.BBS.Core;
using BaiRong.Core;
using BaiRong.Model;
using System.Collections;



namespace SiteServer.BBS.BackgroundPages.Modal
{
    public class ModeratorAdd : BackgroundBasePage
    {
        protected TextBox tbUserName;
        protected RadioButtonList rblIsInherit;

        private int forumID;

        public static string GetOpenWindowString(int publishmentSystemID, int forumID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ForumID", forumID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("添加版主", PageUtils.GetBBSUrl("modal_moderatorAdd.aspx"), arguments, 500, 300);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.forumID = base.GetIntQueryString("ForumID");

            if (!IsPostBack)
            {
                EBooleanUtils.AddListItems(this.rblIsInherit, "继承", "不继承");

                ForumInfo forumInfo = ForumManager.GetForumInfo(base.PublishmentSystemID, this.forumID);
                this.tbUserName.Text = forumInfo.Additional.Moderators;
                ControlUtils.SelectListItems(this.rblIsInherit, forumInfo.Additional.IsModeratorsInherit.ToString());
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            try
            {
                ForumInfo forumInfo = ForumManager.GetForumInfo(base.PublishmentSystemID, this.forumID);

                ArrayList userNameArrayList = new ArrayList();
                if (!string.IsNullOrEmpty(this.tbUserName.Text))
                {
                    userNameArrayList = TranslateUtils.StringCollectionToArrayList(this.tbUserName.Text);
                    foreach (string userName in userNameArrayList)
                    {
                        if (!BaiRongDataProvider.UserDAO.IsUserExists(base.PublishmentSystemInfo.GroupSN, userName))
                        {
                            throw new Exception(string.Format("用户 {0} 不存在", userName));
                        }
                    }
                }

                int groupID = UserGroupManager.GetGroupIDByGroupType(base.PublishmentSystemInfo.GroupSN, EUserGroupType.Moderator);

                int administratorGroupID = UserGroupManager.GetGroupIDByGroupType(base.PublishmentSystemInfo.GroupSN, EUserGroupType.Administrator);
                int superModeratorGroupID = UserGroupManager.GetGroupIDByGroupType(base.PublishmentSystemInfo.GroupSN, EUserGroupType.SuperModerator);
                foreach (string userName in userNameArrayList)
                {
                    UserInfo userInfo = UserManager.GetUserInfo(base.PublishmentSystemInfo.GroupSN, userName);

                    if (userInfo.GroupID != administratorGroupID && userInfo.GroupID != superModeratorGroupID)
                    {
                        userInfo.GroupID = groupID;
                        BaiRongDataProvider.UserDAO.Update(userInfo);
                    }
                }

                forumInfo.Additional.Moderators = TranslateUtils.ObjectCollectionToString(userNameArrayList);
                forumInfo.Additional.IsModeratorsInherit = TranslateUtils.ToBool(this.rblIsInherit.SelectedValue);

                DataProvider.ForumDAO.UpdateForumInfo(base.PublishmentSystemID, forumInfo);

                if (forumInfo.Additional.IsModeratorsInherit)
                {
                    ArrayList forumIDArrayList = DataProvider.ForumDAO.GetForumIDArrayListForDescendant(base.PublishmentSystemID, forumID);
                    if (forumIDArrayList.Count > 0)
                    {
                        foreach (int subForumID in forumIDArrayList)
                        {
                            ForumInfo subForumInfo = ForumManager.GetForumInfo(base.PublishmentSystemID, subForumID);
                            if (string.IsNullOrEmpty(subForumInfo.Additional.Moderators))
                            {
                                subForumInfo.Additional.Moderators = forumInfo.Additional.Moderators;
                            }
                            else
                            {
                                ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(subForumInfo.Additional.Moderators);
                                foreach (string userName in userNameArrayList)
                                {
                                    if (!arraylist.Contains(userName))
                                    {
                                        arraylist.Add(userName);
                                    }
                                }
                                subForumInfo.Additional.Moderators = TranslateUtils.ObjectCollectionToString(arraylist);
                            }
                            DataProvider.ForumDAO.UpdateForumInfo(base.PublishmentSystemID, subForumInfo);
                        }
                    }
                }

                isChanged = true;
            }
            catch (Exception ex)
            {
                isChanged = false;
                base.FailMessage(ex, string.Format("添加版主出错:{0}", ex.Message));
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, BackgroundForum.GetRedirectUrl(base.PublishmentSystemID));
            }
        }
    }
}
