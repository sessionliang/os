using BaiRong.Core;
using BaiRong.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace BaiRong.BackgroundPages
{
    public class BackgroundUserLevel : BackgroundBasePage
    {
        public Literal ltlCreditCalculate;
        public Button SetButton;

        public DataGrid UserLevelDataGrid;
        public DataGrid SysLevelDataGrid;

        public Button UserLevelAddButton;

        public static string GetRedirectUrl()
        {
            return PageUtils.GetPlatformUrl(string.Format("background_userLevel.aspx"));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("Delete") != null)
            {
                int levelID = base.GetIntQueryString("LevelID");

                try
                {
                    string levelName = UserLevelManager.GetLevelName("", levelID);
                    BaiRongDataProvider.UserLevelDAO.Delete("", levelID);

                    base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }

            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_UserLevelCredit, "用户等级管理", AppManager.User.Permission.Usercenter_Setting);
                this.ltlCreditCalculate.Text = LevelRuleManager.GetLevelCalculate();            

                this.SetButton.Attributes.Add("onclick", Modal.UserLevelCalculate.GetOpenWindowString());
                BindGrid();
                this.UserLevelAddButton.Attributes.Add("onclick", Modal.UserLevelAdd.GetOpenWindowStringToAdd(0, true));
            }
        }

        public void BindGrid()
        {
            ArrayList userLevelInfoArrayList = UserLevelManager.GetLevelInfoArrayList("");
            if (userLevelInfoArrayList.Count == 0)
            {
                BaiRongDataProvider.UserLevelDAO.CreateDefaultUserLevel("");
                userLevelInfoArrayList = UserLevelManager.GetLevelInfoArrayList("");
            }
            ArrayList creditsArrayList = new ArrayList();
            ArrayList systemsArrayList = new ArrayList();

            foreach (UserLevelInfo userLevelInfo in userLevelInfoArrayList)
            {
                if (userLevelInfo.LevelType == EUserLevelType.Credits)
                {
                    creditsArrayList.Add(userLevelInfo);
                }
                else
                {
                    systemsArrayList.Add(userLevelInfo);
                }
            }

            UserLevelDataGrid.DataSource = creditsArrayList;
            UserLevelDataGrid.ItemDataBound += new DataGridItemEventHandler(UserLevelDataGrid_ItemDataBound);
            UserLevelDataGrid.DataBind();
            if (AppManager.IsDirectoryExists(AppManager.BBS.AppID))
            {
                SysLevelDataGrid.DataSource = systemsArrayList;
                SysLevelDataGrid.ItemDataBound += new DataGridItemEventHandler(SysLevelDataGrid_ItemDataBound);
                SysLevelDataGrid.DataBind();
            }
        }

        public void UserLevelDataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                UserLevelInfo levelInfo = e.Item.DataItem as UserLevelInfo;

                Literal ltlLevelName = e.Item.FindControl("ltlLevelName") as Literal;
                Literal ltlMinNum = e.Item.FindControl("ltlMinNum") as Literal;
                Literal ltlMaxNum = e.Item.FindControl("ltlMaxNum") as Literal;
                Literal ltlStars = e.Item.FindControl("ltlStars") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlPermissionUrl = e.Item.FindControl("ltlPermissionUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ltlLevelName.Text = levelInfo.LevelName.ToString();
                ltlMinNum.Text = levelInfo.MinNum.ToString();
                ltlMaxNum.Text = levelInfo.MaxNum.ToString();

                ltlStars.Text = string.Format("{0}", UserManager.GetUserLevelHtml(levelInfo.Stars));

                ltlEditUrl.Text = string.Format(@"<a href='javascript:;' onclick=""{0}"">编辑</a>", Modal.UserLevelAdd.GetOpenWindowStringToEdit(levelInfo.ID, true));

                if (AppManager.IsDirectoryExists(AppManager.BBS.AppID))
                {
                    string permissionUrl = PageUtils.GetPlatformUrl(string.Format("background_permission.aspx?levelID={0}", levelInfo.ID));
                    ltlPermissionUrl.Text = string.Format(@"<a href=""{0}"">用户组权限</a>", permissionUrl);
                }
                ltlDeleteUrl.Text = string.Format("<a href=\"{0}?Delete=True&LevelID={1}\" onClick=\"javascript:return confirm('此操作将删除用户等级“{2}”，确认吗？');\">删除</a>", BackgroundUserLevel.GetRedirectUrl(), levelInfo.ID, levelInfo.LevelName);
            }
        }
        public void SysLevelDataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                UserLevelInfo levelInfo = e.Item.DataItem as UserLevelInfo;

                Literal ltlLevelName = e.Item.FindControl("ltlLevelName") as Literal;
                Literal ltlStars = e.Item.FindControl("ltlStars") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlPermissionUrl = e.Item.FindControl("ltlPermissionUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ltlLevelName.Text = levelInfo.LevelName;

                ltlStars.Text = string.Format("{0}", UserManager.GetUserLevelHtml(levelInfo.Stars));

                ltlEditUrl.Text = string.Format(@"<a href='javascript:;' onclick=""{0}"">编辑</a>", Modal.UserLevelAdd.GetOpenWindowStringToEdit(levelInfo.ID, false));

                if (AppManager.IsDirectoryExists(AppManager.BBS.AppID))
                {
                    string permissionUrl = PageUtils.GetBBSUrl(string.Format("background_permission.aspx?publishmentSystemID={0}&levelID={1}", 0, levelInfo.ID));
                    ltlPermissionUrl.Text = string.Format(@"<a href=""{0}"">用户组权限</a>", permissionUrl);
                }

                ltlDeleteUrl.Text = string.Format("<a href=\"{0}&Delete=True&LevelID={1}\" onClick=\"javascript:return confirm('此操作将删除用户组“{2}”，确认吗？');\">删除</a>", BackgroundUserLevel.GetRedirectUrl(), levelInfo.ID, levelInfo.LevelName);
            }
        }
    }
}
