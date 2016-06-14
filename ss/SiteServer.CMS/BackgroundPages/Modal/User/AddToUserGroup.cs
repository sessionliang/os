using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using System.Collections.Specialized;


using System.Collections.Generic;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class AddToUserGroup : BackgroundBasePage
    {
        protected DropDownList UserGroupIDDropDownList;

        private List<int> userIDList;

        public static string GetOpenWindowString(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            return PageUtility.GetOpenWindowStringWithCheckBoxValue("设置用户组", "modal_addToUserGroup.aspx", arguments, "UserIDCollection", "请选择需要设置组别的用户！", 400, 220);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.userIDList = TranslateUtils.StringCollectionToIntList(base.GetQueryString("UserIDCollection"));

            if (!IsPostBack)
            {
                ArrayList userGroupInfoArrayList = UserGroupManager.GetGroupInfoArrayList(base.PublishmentSystemInfo.GroupSN);
                foreach (UserGroupInfo theUserGroupInfo in userGroupInfoArrayList)
                {
                    ListItem listItem = new ListItem(theUserGroupInfo.GroupName, theUserGroupInfo.GroupID.ToString());
                    this.UserGroupIDDropDownList.Items.Add(listItem);
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            int groupID = TranslateUtils.ToInt(this.UserGroupIDDropDownList.SelectedValue);

            UserGroupInfo groupInfo = UserGroupManager.GetGroupInfo(base.PublishmentSystemInfo.GroupSN, groupID);

            foreach (int userID in this.userIDList)
            {
                UserInfo userInfo = BaiRongDataProvider.UserDAO.GetUserInfo(userID);

                userInfo.GroupID = groupID;

                if (groupInfo.GroupType == EUserGroupType.Credits)
                {
                    if (userInfo.Credits < groupInfo.CreditsFrom)
                    {
                        userInfo.Credits = groupInfo.CreditsFrom;
                    }
                    else if (userInfo.Credits >= groupInfo.CreditsTo)
                    {
                        userInfo.Credits = groupInfo.CreditsTo - 1;
                    }
                }

                BaiRongDataProvider.UserDAO.Update(userInfo);
            }

            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "设置用户组", string.Empty);

            JsUtils.OpenWindow.CloseModalPage(Page);
        }

    }
}
