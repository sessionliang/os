using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;


using System.Collections;
using BaiRong.Model;
using SiteServer.CMS.BackgroundPages;
using SiteServer.B2C.Core;

namespace SiteServer.B2C.BackgroundPages.Modal
{
    public class OrderUser : BackgroundBasePage
    {
        public DropDownList PageNum;
        public TextBox Keyword;
        public DropDownList CreateDate;
        public DropDownList LastActivityDate;
        public DropDownList ddlGroupID;

        public Repeater rptContents;
        public SqlPager spContents;

        public static string GetOpenWindowString(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            return PageUtilityB2C.GetOpenWindowString("选择订单用户", "modal_orderUser.aspx", arguments, true);
        }
       
        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;

            if (string.IsNullOrEmpty(base.GetQueryString("PageNum")))
            {
                if (TranslateUtils.ToInt(this.PageNum.SelectedValue) == 0)
                {
                    this.spContents.ItemsPerPage = 25;
                }
                else
                {
                    this.spContents.ItemsPerPage = TranslateUtils.ToInt(this.PageNum.SelectedValue);
                }

                this.spContents.SelectCommand = BaiRongDataProvider.UserDAO.GetSelectCommand(base.PublishmentSystemInfo.GroupSN, true);
            }
            else
            {
                if (base.GetIntQueryString("PageNum") == 0)
                {
                    this.spContents.ItemsPerPage = 25;
                }
                else
                {
                    this.spContents.ItemsPerPage = base.GetIntQueryString("PageNum");
                }
                bool isLockedOutSet = !string.IsNullOrEmpty(base.GetQueryString("IsLockedOut"));
                this.spContents.SelectCommand = BaiRongDataProvider.UserDAO.GetSelectCommand(base.PublishmentSystemInfo.GroupSN, base.GetQueryString("Keyword"), base.GetIntQueryString("CreateDate"), base.GetIntQueryString("LastActivityDate"), true, base.GetIntQueryString("GroupID"));
            }

            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.spContents.SortField = BaiRongDataProvider.UserDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;

            if (!IsPostBack)
            {
                List<int> groupIDList = BaiRongDataProvider.UserGroupDAO.GetGroupIDList(base.PublishmentSystemInfo.GroupSN);
                this.ddlGroupID.Items.Insert(0, new ListItem("<全部>", "0"));
                foreach (int userGroupID in groupIDList)
                {
                    string groupName = UserGroupManager.GetGroupName(base.PublishmentSystemInfo.GroupSN, userGroupID);
                    ListItem listItem = new ListItem(groupName, userGroupID.ToString());
                    this.ddlGroupID.Items.Add(listItem);
                }

                if (!string.IsNullOrEmpty(base.GetQueryString("PageNum")))
                {
                    ControlUtils.SelectListItems(this.PageNum, base.GetQueryString("PageNum"));
                    this.Keyword.Text = base.GetQueryString("Keyword");
                    ControlUtils.SelectListItems(this.CreateDate, base.GetQueryString("CreateDate"));
                    ControlUtils.SelectListItems(this.LastActivityDate, base.GetQueryString("LastActivityDate"));
                    ControlUtils.SelectListItems(this.ddlGroupID, base.GetQueryString("GroupID"));
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
                Literal ltlGroup = (Literal)e.Item.FindControl("ltlGroup");
                Literal ltlLastActivityDate = (Literal)e.Item.FindControl("ltlLastActivityDate");
                Literal ltlCreateDate = (Literal)e.Item.FindControl("ltlCreateDate");
                Literal ltlCreateIPAddress = (Literal)e.Item.FindControl("ltlCreateIPAddress");
                HyperLink hlSelect = (HyperLink)e.Item.FindControl("hlSelect");

                ltlUserName.Text = this.GetUserNameHtml(userInfo.UserID, userInfo.UserName, userInfo.IsLockedOut);
                ltlDisplayName.Text = userInfo.DisplayName;

                ltlGroup.Text = UserGroupManager.GetGroupName(base.PublishmentSystemInfo.GroupSN, userInfo.GroupID);

                ltlLastActivityDate.Text = DateUtils.GetDateAndTimeString(userInfo.LastActivityDate);
                ltlCreateDate.Text = DateUtils.GetDateAndTimeString(userInfo.CreateDate);
                ltlCreateIPAddress.Text = userInfo.CreateIPAddress;
                hlSelect.Attributes.Add("onclick", string.Format("parent.selectUser('{0}');{1}", userInfo.UserName, JsUtils.OpenWindow.HIDE_POP_WIN));
            }
        }

        private string GetUserNameHtml(int userID, string userName, bool isLockedOut)
        {
            string showPopWinString = SiteServer.CMS.BackgroundPages.Modal.UserView.GetOpenWindowString(base.PublishmentSystemID, userID);
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
                    this._pageUrl = PageUtils.GetB2CUrl(string.Format("modal_orderUser.aspx?PublishmentSystemID={0}&PageNum={1}&Keyword={2}&CreateDate={3}&LastActivityDate={4}&GroupID={5}", base.PublishmentSystemID, this.PageNum.SelectedValue, this.Keyword.Text, this.CreateDate.SelectedValue, this.LastActivityDate.SelectedValue, this.ddlGroupID.SelectedValue));
                }
                return this._pageUrl;
            }
        }
    }

}
