using System;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Core.Configuration;
using BaiRong.Model;
using BaiRong.Core;
using System.Web.UI;
using System.Collections;



namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundUserGroup : BackgroundBasePage
	{
        public DataGrid MyDataGrid1;
        public Button AddButton1;
        public DataGrid MyDataGrid2;
        public Button AddButton2;
        public DataGrid MyDataGrid3;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetCMSUrl(string.Format("background_userGroup.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (base.GetQueryString("Delete") != null)
			{
                int groupID = base.GetIntQueryString("GroupID");
			
				try
				{
                    string groupName = UserGroupManager.GetGroupName(base.PublishmentSystemInfo.GroupSN, groupID);
                    BaiRongDataProvider.UserGroupDAO.Delete(base.PublishmentSystemInfo.GroupSN, groupID);

                    //LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "删除用户组", string.Format("用户组:{0}", groupName));

					base.SuccessDeleteMessage();
				}
				catch(Exception ex)
				{
					base.FailDeleteMessage(ex);
				}
            }

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_User, "用户组管理", AppManager.BBS.Permission.BBS_User);

				BindGrid();
                this.AddButton1.Attributes.Add("onclick", Modal.UserGroupAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID, 0, true));
                this.AddButton2.Attributes.Add("onclick", Modal.UserGroupAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID, 0, false));
			}
		}

        public void BindGrid()
		{
            ArrayList userGroupInfoArrayList = UserGroupManager.GetGroupInfoArrayList(base.PublishmentSystemInfo.GroupSN);
            if (userGroupInfoArrayList.Count == 0)
            {
                BaiRongDataProvider.UserGroupDAO.CreateDefaultUserGroup(base.PublishmentSystemInfo.GroupSN);
                userGroupInfoArrayList = UserGroupManager.GetGroupInfoArrayList(base.PublishmentSystemInfo.GroupSN);
            }
            ArrayList creditsArrayList = new ArrayList();
            ArrayList specialsArrayList = new ArrayList();
            ArrayList systemsArrayList = new ArrayList();
            foreach (UserGroupInfo userGroupInfo in userGroupInfoArrayList)
            {
                if (userGroupInfo.GroupType == EUserGroupType.Credits)
                {
                    creditsArrayList.Add(userGroupInfo);
                }
                else if (userGroupInfo.GroupType == EUserGroupType.Specials)
                {
                    specialsArrayList.Add(userGroupInfo);
                }
                else
                {
                    systemsArrayList.Add(userGroupInfo);
                }
            }

            MyDataGrid1.DataSource = creditsArrayList;
            MyDataGrid1.ItemDataBound += new DataGridItemEventHandler(MyDataGrid1_ItemDataBound);
            MyDataGrid1.DataBind();

            MyDataGrid2.DataSource = specialsArrayList;
            MyDataGrid2.ItemDataBound += new DataGridItemEventHandler(MyDataGrid2_ItemDataBound);
            MyDataGrid2.DataBind();

            MyDataGrid3.DataSource = systemsArrayList;
            MyDataGrid3.ItemDataBound += new DataGridItemEventHandler(MyDataGrid3_ItemDataBound);
            MyDataGrid3.DataBind();
		}

        public void MyDataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                UserGroupInfo groupInfo = e.Item.DataItem as UserGroupInfo;

                Literal ltlGroupName = e.Item.FindControl("ltlGroupName") as Literal;
                Literal ltlCreditsFrom = e.Item.FindControl("ltlCreditsFrom") as Literal;
                Literal ltlCreditsTo = e.Item.FindControl("ltlCreditsTo") as Literal;
                Literal ltlAddUrl = e.Item.FindControl("ltlAddUrl") as Literal;
                Literal ltlStars = e.Item.FindControl("ltlStars") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlPermissionUrl = e.Item.FindControl("ltlPermissionUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ltlGroupName.Text = groupInfo.GroupName;
                ltlCreditsFrom.Text = groupInfo.CreditsFrom.ToString();
                ltlCreditsTo.Text = groupInfo.CreditsTo.ToString();

                ltlStars.Text = string.Format("{0}", UserManager.GetUserLevelHtml(groupInfo.Stars));

                ltlAddUrl.Text = string.Format(@"<a href='javascript:;' onclick=""{0}"">添加近似</a>", Modal.UserGroupAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID, groupInfo.GroupID, true));
                ltlEditUrl.Text = string.Format(@"<a href='javascript:;' onclick=""{0}"">编辑</a>", Modal.UserGroupAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, groupInfo.GroupID, true));

                if (base.PublishmentSystemInfo.PublishmentSystemType == EPublishmentSystemType.BBS)
                {
                    string permissionUrl = PageUtils.GetBBSUrl(string.Format("background_permission.aspx?publishmentSystemID={0}&groupID={1}", base.PublishmentSystemID, groupInfo.GroupID));
                    ltlPermissionUrl.Text = string.Format(@"<a href=""{0}"">用户组权限</a>", permissionUrl);
                }
                
                ltlDeleteUrl.Text = string.Format("<a href=\"{0}&Delete=True&GroupID={1}\" onClick=\"javascript:return confirm('此操作将删除用户组“{2}”，确认吗？');\">删除</a>", BackgroundUserGroup.GetRedirectUrl(base.PublishmentSystemID), groupInfo.GroupID, groupInfo.GroupName);
            }
        }

        public void MyDataGrid2_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                UserGroupInfo groupInfo = e.Item.DataItem as UserGroupInfo;

                Literal ltlGroupName = e.Item.FindControl("ltlGroupName") as Literal;
                Literal ltlStars = e.Item.FindControl("ltlStars") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlPermissionUrl = e.Item.FindControl("ltlPermissionUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ltlGroupName.Text = groupInfo.GroupName;

                ltlStars.Text = string.Format("{0}", UserManager.GetUserLevelHtml(groupInfo.Stars));

                ltlEditUrl.Text = string.Format(@"<a href='javascript:;' onclick=""{0}"">编辑</a>", Modal.UserGroupAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, groupInfo.GroupID, false));

                if (base.PublishmentSystemInfo.PublishmentSystemType == EPublishmentSystemType.BBS)
                {
                    string permissionUrl = PageUtils.GetBBSUrl(string.Format("background_permission.aspx?publishmentSystemID={0}&groupID={1}", base.PublishmentSystemID, groupInfo.GroupID));
                    ltlPermissionUrl.Text = string.Format(@"<a href=""{0}"">用户组权限</a>", permissionUrl);
                }

                ltlDeleteUrl.Text = string.Format("<a href=\"{0}&Delete=True&GroupID={1}\" onClick=\"javascript:return confirm('此操作将删除用户组“{2}”，确认吗？');\">删除</a>", BackgroundUserGroup.GetRedirectUrl(base.PublishmentSystemID), groupInfo.GroupID, groupInfo.GroupName);
            }
        }

        public void MyDataGrid3_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                UserGroupInfo groupInfo = e.Item.DataItem as UserGroupInfo;

                Literal ltlGroupName = e.Item.FindControl("ltlGroupName") as Literal;
                Literal ltlStars = e.Item.FindControl("ltlStars") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlPermissionUrl = e.Item.FindControl("ltlPermissionUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ltlGroupName.Text = groupInfo.GroupName;

                ltlStars.Text = string.Format("{0}", UserManager.GetUserLevelHtml(groupInfo.Stars));

                ltlEditUrl.Text = string.Format(@"<a href='javascript:;' onclick=""{0}"">编辑</a>", Modal.UserGroupAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, groupInfo.GroupID, false));

                if (base.PublishmentSystemInfo.PublishmentSystemType == EPublishmentSystemType.BBS)
                {
                    string permissionUrl = PageUtils.GetBBSUrl(string.Format("background_permission.aspx?publishmentSystemID={0}&groupID={1}", base.PublishmentSystemID, groupInfo.GroupID));
                    ltlPermissionUrl.Text = string.Format(@"<a href=""{0}"">用户组权限</a>", permissionUrl);
                }

                ltlDeleteUrl.Text = string.Format("<a href=\"{0}&Delete=True&GroupID={1}\" onClick=\"javascript:return confirm('此操作将删除用户组“{2}”，确认吗？');\">删除</a>", BackgroundUserGroup.GetRedirectUrl(base.PublishmentSystemID), groupInfo.GroupID, groupInfo.GroupName);
            }
        }
	}
}
