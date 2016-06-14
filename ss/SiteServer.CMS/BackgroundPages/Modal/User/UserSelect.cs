using System;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using System.Collections;
using System.Text;

using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data;
using System.Data.OleDb;

using BaiRong.Controls;
using System.Collections.Generic;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class UserSelect : BackgroundBasePage
	{
        public TextBox Keyword;
        public DropDownList CreateDate;
        public DropDownList LastActivityDate;
        public DropDownList ddlGroupID;

        public Repeater rptContents;
        public SqlPager spContents;

        private string textBoxID;

        public static string GetOpenWindowString(int publishmentSystemID, string textBoxID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("textBoxID", textBoxID);
            return PageUtility.GetOpenWindowString("选择用户", "modal_userSelect.aspx", arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.textBoxID = base.GetQueryString("textBoxID");

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.ItemsPerPage = 25;

            if (string.IsNullOrEmpty(base.GetQueryString("groupID")))
            {
                this.spContents.SelectCommand = BaiRongDataProvider.UserDAO.GetSelectCommand(base.PublishmentSystemInfo.GroupSN, true);
            }
            else
            {
                bool isLockedOutSet = !string.IsNullOrEmpty(base.GetQueryString("isLockedOut"));
                this.spContents.SelectCommand = BaiRongDataProvider.UserDAO.GetSelectCommand(base.PublishmentSystemInfo.GroupSN, base.GetQueryString("keyword"), TranslateUtils.ToInt(base.GetQueryString("createDate")), TranslateUtils.ToInt(base.GetQueryString("lastActivityDate")), true, TranslateUtils.ToInt(base.GetQueryString("groupID")));
            }

            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.spContents.SortField = BaiRongDataProvider.UserDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;

            if (!IsPostBack)
            {
                List<int> groupIDList = BaiRongDataProvider.UserGroupDAO.GetGroupIDList(base.PublishmentSystemInfo.GroupSN);
                this.ddlGroupID.Items.Insert(0, new ListItem("<全部>", "0"));
                foreach (int groupID in groupIDList)
                {
                    string groupName = UserGroupManager.GetGroupName(base.PublishmentSystemInfo.GroupSN, groupID);
                    ListItem listItem = new ListItem(groupName, groupID.ToString());
                    this.ddlGroupID.Items.Add(listItem);
                }

                if (!string.IsNullOrEmpty(base.GetQueryString("groupID")))
                {
                    this.Keyword.Text = base.GetQueryString("keyword");
                    ControlUtils.SelectListItems(this.CreateDate, base.GetQueryString("createDate"));
                    ControlUtils.SelectListItems(this.LastActivityDate, base.GetQueryString("lastActivityDate"));
                    ControlUtils.SelectListItems(this.ddlGroupID, base.GetQueryString("groupID"));
                }

                this.spContents.DataBind();
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                UserInfo userInfo = new UserInfo(e.Item.DataItem);

                Literal ltlUserName = (Literal)e.Item.FindControl("ltlUserName");
                Literal ltlDisplayName = (Literal)e.Item.FindControl("ltlDisplayName");
                Literal ltlEmail = (Literal)e.Item.FindControl("ltlEmail");
                Literal ltlMobile = (Literal)e.Item.FindControl("ltlMobile");
                Literal ltlGroup = (Literal)e.Item.FindControl("ltlGroup");
                Literal ltlLastActivityDate = (Literal)e.Item.FindControl("ltlLastActivityDate");
                Literal ltlCreateDate = (Literal)e.Item.FindControl("ltlCreateDate");
                Literal ltlCreateIPAddress = (Literal)e.Item.FindControl("ltlCreateIPAddress");
                Literal ltlSelect = (Literal)e.Item.FindControl("ltlSelect");

                ltlUserName.Text = this.GetUserNameHtml(userInfo.UserID, userInfo.UserName, userInfo.IsLockedOut);
                ltlDisplayName.Text = userInfo.DisplayName;
                ltlEmail.Text = userInfo.Email;
                ltlMobile.Text = userInfo.Mobile;

                ltlGroup.Text = UserGroupManager.GetGroupName(base.PublishmentSystemInfo.GroupSN, userInfo.GroupID);

                ltlLastActivityDate.Text = DateUtils.GetDateAndTimeString(userInfo.LastActivityDate);
                ltlCreateDate.Text = DateUtils.GetDateAndTimeString(userInfo.CreateDate);
                ltlCreateIPAddress.Text = userInfo.CreateIPAddress;
                ltlSelect.Text = string.Format(@"<input type=""checkbox"" name=""UserNameCollection"" value=""{0}"" />", userInfo.UserName);
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

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            ArrayList userNameArrayList = TranslateUtils.StringCollectionToArrayList(base.Request.Form["UserNameCollection"]);

            if (userNameArrayList.Count == 0)
            {
                base.FailMessage("请勾选所需用户");
            }
            else
            {
                JsUtils.OpenWindow.CloseModalPageWithoutRefresh(base.Page, string.Format(@"
var textBox = parent.$('#{0}');
if (textBox.val()){{
    textBox.val(textBox.val() + ',{1}');
}}else{{
    textBox.val('{1}');
}}
", this.textBoxID, base.Request.Form["UserNameCollection"]));
            }
 
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    this._pageUrl = PageUtils.GetCMSUrl(string.Format("modal_userSelect.aspx?publishmentSystemID={0}&keyword={1}&createDate={2}&lastActivityDate={3}&groupID={4}", base.PublishmentSystemID, this.Keyword.Text, this.CreateDate.SelectedValue, this.LastActivityDate.SelectedValue, this.ddlGroupID.SelectedValue));
                }
                return this._pageUrl;
            }
        }
	}
}
