using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;


using System.Collections.Generic;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundUserNewGroup : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;

        public Button Add;
        public Button Delete;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetPlatformUrl(string.Format("background_userNewGroup.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        public static string GetViewHtml(int publishmentSystemID, int userID, string userName)
        {
            string showPopWinString = Modal.UserView.GetOpenWindowString(publishmentSystemID, userID);
            return string.Format(@"<a href=""javascript:;"" onclick=""{0}"">{1}</a>", showPopWinString, userName);
        }

        public string GetDateTime(DateTime datetime)
        {
            string retval = string.Empty;
            if (datetime > DateUtils.SqlMinValue)
            {
                retval = DateUtils.GetDateString(datetime);
            }
            return retval;
        }

        public string GetSendMailHtml(string email)
        {
            string showPopWinString = Modal.UserSendMail.GetOpenWindowStringToEmails(base.PublishmentSystemID, email);
            return string.Format(@"<a href=""javascript:;"" onclick=""{0}"">{1}</a>", showPopWinString, email);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("Delete") != null)
            {
                ArrayList groupIDList = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("GroupIDCollection"));
                try
                {
                    DataProvider.UserNewGroupDAO.Delete(groupIDList);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "删除用户组", string.Empty);

                    base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.ItemsPerPage = 25;
            this.spContents.SelectCommand = DataProvider.UserNewGroupDAO.GetAllNewGroupString(string.Empty);
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.spContents.SortField = DataProvider.UserNewGroupDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;

            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_User, "用户组管理", AppManager.User.Permission.Usercenter_UserGroup);

                #region 默认创建一个全部用户组
                DataProvider.UserNewGroupDAO.SetDefaultInfo();
                #endregion

                this.spContents.DataBind();

                this.Add.Attributes.Add("onclick", JsUtils.GetRedirectString(PageUtils.GetPlatformUrl(string.Format("background_userNewGroupAdd.aspx"))));

                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetPlatformUrl(string.Format("background_userNewGroup.aspx?publishmentSystemID={0}&Delete=True", base.PublishmentSystemID)), "GroupIDCollection", "GroupIDCollection", "请选择需要删除的用户组！", "此操作将删除所选用户组，确认吗？"));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SiteServer.CMS.Model.UserNewGroupInfo userInfo = new SiteServer.CMS.Model.UserNewGroupInfo(e.Item.DataItem);

                Literal ltlItemName = (Literal)e.Item.FindControl("ltlItemName");
                Literal ltlCreateDate = (Literal)e.Item.FindControl("ltlCreateDate");
                Literal ltlUserCount = (Literal)e.Item.FindControl("ltlUserCount");
                Literal ltlSelect = (Literal)e.Item.FindControl("ltlSelect");
                HyperLink hlEditLink = (HyperLink)e.Item.FindControl("hlEditLink");
                HyperLink hlMLibLink = (HyperLink)e.Item.FindControl("hlMLibLink");
                HyperLink hlDelete = (HyperLink)e.Item.FindControl("hlDelete");
                HyperLink hlUserLink = (HyperLink)e.Item.FindControl("hlUserLink");

                ltlItemName.Text = userInfo.ItemName;
                ltlUserCount.Text = userInfo.ContentNum.ToString();

                ltlCreateDate.Text = DateUtils.GetDateAndTimeString(userInfo.AddDate);

                string userAddUrl = PageUtils.GetPlatformUrl(string.Format("background_userNewGroupAdd.aspx?ItemID={0}&GroupName={1}&PublishmentSystemID={2}", userInfo.ItemID, userInfo.ItemName,base.PublishmentSystemID));
                hlEditLink.NavigateUrl = userAddUrl;
                userAddUrl = PageUtils.GetPlatformUrl(string.Format("background_userNewGroupUser.aspx?PublishmentSystemID={0}&NewGroupID={1}&NewGroupName={2}", base.PublishmentSystemID, userInfo.ItemID, userInfo.ItemName));
                hlUserLink.NavigateUrl = userAddUrl;

                hlMLibLink.Visible = ConfigManager.Additional.IsUseMLib;
                if (ConfigManager.Additional.IsUnifiedMLibAddUser == true || ConfigManager.Additional.IsUnifiedMLibNum || ConfigManager.Additional.IsUnifiedMLibValidityDate)
                {
                    hlMLibLink.Visible = true;
                }
                else
                {
                    hlMLibLink.Visible = false;
                }
                string mlibUrl = PageUtils.GetPlatformUrl(string.Format("background_userNewGroupMLibSite.aspx?ItemID={0}&GroupName={1}&PublishmentSystemID={2}", userInfo.ItemID, userInfo.ItemName, base.PublishmentSystemID));
                hlMLibLink.NavigateUrl = mlibUrl;

                hlDelete.NavigateUrl = PageUtils.GetPlatformUrl(string.Format("background_userNewGroup.aspx?Delete=True&GroupIDCollection={0}&PublishmentSystemID={1}", userInfo.ItemID,base.PublishmentSystemID));

                ltlSelect.Text = string.Format(@"<input type=""checkbox"" name=""GroupIDCollection"" value=""{0}"" />", userInfo.ItemID);
            }
        }
    }
}
