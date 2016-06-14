using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;


using System.Collections.Generic;

namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundUserCheck : BackgroundBasePage
	{
        public Repeater rptContents;
        public SqlPager spContents;

        public Button Check;
        public Button Delete;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetCMSUrl(string.Format("background_userCheck.aspx?publishmentSystemID={0}", publishmentSystemID));
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
				List<int> userIDList = TranslateUtils.StringCollectionToIntList(base.GetQueryString("UserIDCollection"));
				try
				{
                    foreach (int userID in userIDList)
                    {
                        BaiRongDataProvider.UserDAO.Delete(userID);
                    }

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "删除用户", string.Empty);

					base.SuccessDeleteMessage();
				}
				catch(Exception ex)
				{
                    base.FailDeleteMessage(ex);
				}
			}
            else if (base.GetQueryString("Check") != null)
			{
                List<int> userIDList = TranslateUtils.StringCollectionToIntList(base.GetQueryString("UserIDCollection"));
				try
				{
                    BaiRongDataProvider.UserDAO.Check(userIDList);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "审核用户", string.Empty);

                    base.SuccessCheckMessage();
				}
				catch(Exception ex)
				{
                    base.FailCheckMessage(ex);
				}
			}

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.ItemsPerPage = 25;
            this.spContents.SelectCommand = BaiRongDataProvider.UserDAO.GetSelectCommand(base.PublishmentSystemInfo.GroupSN, false);
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.spContents.SortField = BaiRongDataProvider.UserDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_User, "审核新用户", AppManager.CMS.Permission.WebSite.User);

                this.spContents.DataBind();

                this.Check.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(string.Format("background_userCheck.aspx?publishmentSystemID={0}&Check=True", base.PublishmentSystemID)), "UserIDCollection", "UserIDCollection", "请选择需要审核的会员！", "此操作将审核通过所选会员，确认吗？"));

                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(string.Format("background_userCheck.aspx?publishmentSystemID={0}&Delete=True", base.PublishmentSystemID)), "UserIDCollection", "UserIDCollection", "请选择需要删除的会员！", "此操作将删除所选会员，确认吗？"));
            }
		}

        private string GetUserNameHtml(int userID, string userName, bool isLockedOut)
        {
            string showPopWinString = Modal.UserView.GetOpenWindowString(base.PublishmentSystemID, userID);
            string state = string.Empty;
            if (isLockedOut)
            {
                state = @"<span style=""color:red;"">[已被锁定]</span>";
            }
            return string.Format(@"<a href=""javascript:;"" onclick=""{0}"">{1}</a>{2}", showPopWinString, userName, state);
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                UserInfo userInfo = new UserInfo(e.Item.DataItem);

                Literal ltlUserName = (Literal)e.Item.FindControl("ltlUserName");
                Literal ltlDisplayName = (Literal)e.Item.FindControl("ltlDisplayName");
                Literal ltlUserGroupName = (Literal)e.Item.FindControl("ltlUserGroupName");
                Literal ltlCreateDate = (Literal)e.Item.FindControl("ltlCreateDate");
                Literal ltlSelect = (Literal)e.Item.FindControl("ltlSelect");
                HyperLink hlEditLink = (HyperLink)e.Item.FindControl("hlEditLink");

                ltlUserName.Text = this.GetUserNameHtml(userInfo.UserID, userInfo.UserName, userInfo.IsLockedOut);
                ltlDisplayName.Text = userInfo.DisplayName;

                ltlCreateDate.Text = DateUtils.GetDateAndTimeString(userInfo.CreateDate);

                string userAddUrl = BackgroundUserAdd.GetRedirectUrlToEdit(base.PublishmentSystemID, userInfo.UserID, BackgroundUserCheck.GetRedirectUrl(base.PublishmentSystemID));
                hlEditLink.NavigateUrl = userAddUrl;

                ltlSelect.Text = string.Format(@"<input type=""checkbox"" name=""UserIDCollection"" value=""{0}"" />", userInfo.UserID);
            }
        }
	}
}
