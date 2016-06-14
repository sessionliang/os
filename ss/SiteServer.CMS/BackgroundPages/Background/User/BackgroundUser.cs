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
    public class BackgroundUser : BackgroundBasePage
    {
        public DropDownList ddlUserGroup;
        public DropDownList ddlPageNum;
        public TextBox tbKeyword;
        public DropDownList ddlCreationDate;
        public DropDownList ddlLastActivityDate;

        public Literal ltlColumnHeader;
        public Repeater rptContents;
        public SqlPager spContents;

        public Button btnAdd;
        public Button AddToGroup;
        public Button Lock;
        public Button UnLock;
        public Button SendMail;
        public Button Delete;
        public Button Import;
        public Button Export;

        private string defaultGroupName = string.Empty;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetCMSUrl(string.Format("background_user.aspx?publishmentSystemID={0}", publishmentSystemID));
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
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }
            else if (base.GetQueryString("Lock") != null)
            {
                List<int> userIDList = TranslateUtils.StringCollectionToIntList(base.GetQueryString("UserIDCollection"));
                try
                {
                    BaiRongDataProvider.UserDAO.Lock(userIDList, true);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "锁定用户", string.Empty);

                    base.SuccessMessage("成功锁定所选会员！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "锁定所选会员失败！");
                }
            }
            else if (base.GetQueryString("UnLock") != null)
            {
                List<int> userIDList = TranslateUtils.StringCollectionToIntList(base.GetQueryString("UserIDCollection"));
                try
                {
                    BaiRongDataProvider.UserDAO.Lock(userIDList, false);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "解除锁定用户", string.Empty);

                    base.SuccessMessage("成功解除锁定所选会员！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "解除锁定所选会员失败！");
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;

            if (string.IsNullOrEmpty(base.GetQueryString("UserGroup")))
            {
                if (TranslateUtils.ToInt(this.ddlPageNum.SelectedValue) == 0)
                {
                    this.spContents.ItemsPerPage = 25;
                }
                else
                {
                    this.spContents.ItemsPerPage = TranslateUtils.ToInt(this.ddlPageNum.SelectedValue);
                }

                this.spContents.SelectCommand = BaiRongDataProvider.UserDAO.GetSelectCommand(base.PublishmentSystemInfo.GroupSN, true);
            }
            else
            {
                if (TranslateUtils.ToInt(base.GetQueryString("PageNum")) == 0)
                {
                    this.spContents.ItemsPerPage = 25;
                }
                else
                {
                    this.spContents.ItemsPerPage = TranslateUtils.ToInt(base.GetQueryString("PageNum"));
                }
                bool isLockedOutSet = !string.IsNullOrEmpty(base.GetQueryString("IsLockedOut"));
                this.spContents.SelectCommand = BaiRongDataProvider.UserDAO.GetSelectCommand(base.PublishmentSystemInfo.GroupSN, TranslateUtils.ToInt(base.GetQueryString("UserGroup")), base.GetQueryString("Keyword"), TranslateUtils.ToInt(base.GetQueryString("CreationDate")), TranslateUtils.ToInt(base.GetQueryString("LastActivityDate")), true);
            }

            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.spContents.SortField = BaiRongDataProvider.UserDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;

            UserGroupInfo groupInfo = UserGroupManager.GetGroupInfoByCredits(base.PublishmentSystemInfo.GroupSN, 0);
            if (groupInfo != null)
            {
                this.defaultGroupName = groupInfo.GroupName;
            }

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_User, "用户管理", AppManager.BBS.Permission.BBS_User);

                ListItem theListItem = new ListItem("全部", "0");
                theListItem.Selected = true;
                this.ddlUserGroup.Items.Add(theListItem);
                ArrayList userGroupInfoArrayList = UserGroupManager.GetGroupInfoArrayList(base.PublishmentSystemInfo.GroupSN);
                foreach (UserGroupInfo userGroupInfo in userGroupInfoArrayList)
                {
                    ListItem listitem = new ListItem(userGroupInfo.GroupName, userGroupInfo.GroupID.ToString());
                    this.ddlUserGroup.Items.Add(listitem);
                }

                if (!string.IsNullOrEmpty(base.GetQueryString("UserGroup")))
                {
                    ControlUtils.SelectListItems(this.ddlUserGroup, base.GetQueryString("UserGroup"));
                    ControlUtils.SelectListItems(this.ddlPageNum, base.GetQueryString("PageNum"));
                    this.tbKeyword.Text = base.GetQueryString("Keyword");
                    ControlUtils.SelectListItems(this.ddlCreationDate, base.GetQueryString("CreationDate"));
                    ControlUtils.SelectListItems(this.ddlLastActivityDate, base.GetQueryString("LastActivityDate"));
                }

                string showPopWinString = Modal.AddToUserGroup.GetOpenWindowString(base.PublishmentSystemID);
                this.AddToGroup.Attributes.Add("onclick", showPopWinString);

                string backgroundUrl = BackgroundUser.GetRedirectUrl(base.PublishmentSystemID);

                this.btnAdd.Attributes.Add("onclick", string.Format("location.href='{0}';return false;", BackgroundUserAdd.GetRedirectUrlToAdd(base.PublishmentSystemID, this.PageUrl)));

                this.Lock.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(string.Format("{0}&Lock=True", backgroundUrl), "UserIDCollection", "UserIDCollection", "请选择需要锁定的会员！", "此操作将锁定所选会员，确认吗？"));

                this.UnLock.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(string.Format("{0}&UnLock=True", backgroundUrl), "UserIDCollection", "UserIDCollection", "请选择需要解除锁定的会员！", "此操作将解除锁定所选会员，确认吗？"));

                this.SendMail.Attributes.Add("onclick", Modal.UserSendMail.GetOpenWindowString(base.PublishmentSystemID));

                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(string.Format("{0}&Delete=True", backgroundUrl), "UserIDCollection", "UserIDCollection", "请选择需要删除的会员！", "此操作将删除所选会员，确认吗？"));

                this.Import.Attributes.Add("onclick", Modal.UserImport.GetOpenWindowString(base.PublishmentSystemID));

                this.Export.Attributes.Add("onclick", Modal.UserExport.GetOpenWindowString(base.PublishmentSystemID));

                if (EPublishmentSystemTypeUtils.IsB2C(base.PublishmentSystemInfo.PublishmentSystemType))
                {
                    this.ltlColumnHeader.Text = "<td></td><td></td>";
                }

                this.spContents.DataBind();
            }
        }

        public void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                UserInfo userInfo = new UserInfo(e.Item.DataItem);

                Literal ltlUserName = (Literal)e.Item.FindControl("ltlUserName");
                Literal ltlDisplayName = (Literal)e.Item.FindControl("ltlDisplayName");
                Literal ltlUserGroupName = (Literal)e.Item.FindControl("ltlUserGroupName");
                Literal ltlLastActivityDate = (Literal)e.Item.FindControl("ltlLastActivityDate");
                Literal ltlCreationDate = (Literal)e.Item.FindControl("ltlCreationDate");
                Literal ltlCredits = (Literal)e.Item.FindControl("ltlCredits");
                Literal ltlColumns = (Literal)e.Item.FindControl("ltlColumns");
                Literal ltlSelect = (Literal)e.Item.FindControl("ltlSelect");
                HyperLink hlChangePassword = (HyperLink)e.Item.FindControl("hlChangePassword");
                HyperLink hlEditLink = (HyperLink)e.Item.FindControl("hlEditLink");

                ltlUserName.Text = this.GetUserNameHtml(userInfo.UserID, userInfo.UserName, userInfo.IsLockedOut);
                ltlDisplayName.Text = userInfo.DisplayName;

                if (userInfo.GroupID != 0)
                {
                    ltlUserGroupName.Text = UserGroupManager.GetGroupName(base.PublishmentSystemInfo.GroupSN, userInfo.GroupID);
                }
                if (string.IsNullOrEmpty(ltlUserGroupName.Text))
                {
                    ltlUserGroupName.Text = this.defaultGroupName;
                }

                ltlLastActivityDate.Text = DateUtils.GetDateAndTimeString(userInfo.LastActivityDate);
                ltlCreationDate.Text = DateUtils.GetDateAndTimeString(userInfo.CreateDate);
                ltlCredits.Text = userInfo.Credits.ToString();

                if (EPublishmentSystemTypeUtils.IsB2C(base.PublishmentSystemInfo.PublishmentSystemType))
                {
                    string urlConsignee = PageUtils.GetB2CUrl(string.Format("background_userConsignee.aspx?publishmentSystemID={0}&userName={1}&returnUrl={2}", base.PublishmentSystemID, userInfo.UserName, StringUtils.ValueToUrl(this.PageUrl)));
                    string urlInvoice = PageUtils.GetB2CUrl(string.Format("background_userInvoice.aspx?publishmentSystemID={0}&userName={1}&returnUrl={2}", base.PublishmentSystemID, userInfo.UserName, StringUtils.ValueToUrl(this.PageUrl)));
                    ltlColumns.Text = string.Format(@"<td class=""center""><a href=""{0}"">收件人信息</a></td><td class=""center""><a href=""{1}"">发票信息</a></td>", urlConsignee, urlInvoice);
                }

                hlEditLink.NavigateUrl = BackgroundUserAdd.GetRedirectUrlToEdit(base.PublishmentSystemID, userInfo.UserID, BackgroundUser.GetRedirectUrl(base.PublishmentSystemID));
                hlChangePassword.Attributes.Add("onclick", Modal.UserPassword.GetOpenWindowString(base.PublishmentSystemID, userInfo.UserID));
                ltlSelect.Text = string.Format(@"<input type=""checkbox"" name=""UserIDCollection"" value=""{0}"" />", userInfo.UserID);
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

        public void Search_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    this._pageUrl = string.Format("{0}&UserGroup={1}&PageNum={2}&Keyword={3}&CreationDate={4}&LastActivityDate={5}", BackgroundUser.GetRedirectUrl(base.PublishmentSystemID), this.ddlUserGroup.SelectedValue, this.ddlPageNum.SelectedValue, this.tbKeyword.Text, this.ddlCreationDate.SelectedValue, this.ddlLastActivityDate.SelectedValue);
                }
                return this._pageUrl;
            }
        }
    }
}
