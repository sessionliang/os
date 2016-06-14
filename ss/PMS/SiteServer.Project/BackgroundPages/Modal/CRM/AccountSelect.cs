using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using System.Collections.Specialized;


using SiteServer.Project.Model;
using SiteServer.Project.Core;
using BaiRong.Controls;

namespace SiteServer.Project.BackgroundPages.Modal
{
	public class AccountSelect : BackgroundBasePage
	{
        public DropDownList ddlTaxis;
        public DropDownList ddlStatus;
        public TextBox tbKeyword;

        public Repeater rptContents;
        public SqlPager spContents;

        private string scriptName;
        private bool isAllAccount;

        public static string GetShowPopWinString(string scriptName, bool isAllAccount)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("scriptName", scriptName);
            arguments.Add("isAllAccount", isAllAccount.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("选择客户", "modal_accountSelect.aspx", arguments, true);
        }

		public void Page_Load(object sender, EventArgs E)
		{
            this.scriptName = base.Request.QueryString["scriptName"];
            this.isAllAccount = TranslateUtils.ToBool(base.Request.QueryString["isAllAccount"]);

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = this.GetSelectString();
            this.spContents.SortField = DataProvider.AccountDAO.GetSortFieldName();
            this.spContents.SortMode = this.GetSortMode();
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

			if (!IsPostBack)
			{
                if (!string.IsNullOrEmpty(base.Request.QueryString["AccountID"]))
                {
                    int accountID = TranslateUtils.ToInt(base.Request.QueryString["AccountID"]);
                    string accountName = DataProvider.AccountDAO.GetAccountName(accountID);
                    string scripts = string.Format("window.parent.{0}('{1}', '{2}');", this.scriptName, accountName, accountID);
                    JsUtils.OpenWindow.CloseModalPageWithoutRefresh(base.Page, scripts);
                }
                else
                {
                    bool isTaxisDESC = true;
                    EBooleanUtils.AddListItems(this.ddlTaxis, "倒序", "正序");
                    if (!string.IsNullOrEmpty(base.Request.QueryString["isTaxisDESC"]))
                    {
                        isTaxisDESC = TranslateUtils.ToBool(base.Request.QueryString["isTaxisDESC"]);
                        ControlUtils.SelectListItemsIgnoreCase(this.ddlTaxis, isTaxisDESC.ToString());
                    }

                    ListItem listItem = new ListItem("全部", string.Empty);
                    this.ddlStatus.Items.Add(listItem);
                    EAccountStatusUtils.AddListItems(this.ddlStatus);
                    if (!string.IsNullOrEmpty(base.Request.QueryString["status"]))
                    {
                        ControlUtils.SelectListItemsIgnoreCase(this.ddlStatus, base.Request.QueryString["status"]);
                    }

                    this.tbKeyword.Text = base.Request.QueryString["keyword"];

                    this.spContents.DataBind();
                }
			}
		}

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                AccountInfo accountInfo = new AccountInfo(e.Item.DataItem);

                Literal ltlID = e.Item.FindControl("ltlID") as Literal;
                Literal ltlAccountName = e.Item.FindControl("ltlAccountName") as Literal;
                Literal ltlBusinessType = e.Item.FindControl("ltlBusinessType") as Literal;
                Literal ltlLocation = e.Item.FindControl("ltlLocation") as Literal;
                Literal ltlWebsite = e.Item.FindControl("ltlWebsite") as Literal;
                Literal ltlChargeUserName = e.Item.FindControl("ltlChargeUserName") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;

                Literal ltlStatus = e.Item.FindControl("ltlStatus") as Literal;

                ltlID.Text = accountInfo.ID.ToString();

                ltlAccountName.Text = string.Format(@"<a href=""modal_accountSelect.aspx?scriptName={0}&AccountID={1}"">{2}</a>", this.scriptName, accountInfo.ID, accountInfo.AccountName);
                ltlChargeUserName.Text = AdminManager.GetDisplayName(accountInfo.ChargeUserName, true);
                ltlBusinessType.Text = accountInfo.BusinessType;
                ltlLocation.Text = accountInfo.Province + " " + accountInfo.City + " " + accountInfo.Area;
                ltlWebsite.Text = accountInfo.Website;
                ltlAddDate.Text = DateUtils.GetDateAndTimeString(accountInfo.AddDate);

                ltlStatus.Text = EAccountStatusUtils.GetText(accountInfo.Status);
            }
        }

        public void Search_OnClick(object sender, System.EventArgs e)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        protected string GetSelectString()
        {
            string userName = string.Empty;
            if (isAllAccount == false)
            {
                userName = AdminManager.Current.UserName;
            }
            if (base.Request.QueryString["keyword"] != null)
            {
                return DataProvider.AccountDAO.GetSelectString(userName, base.Request.QueryString["status"], base.Request.QueryString["keyword"]);
            }
            else
            {
                return DataProvider.AccountDAO.GetSelectString(userName);
            }
        }

        protected SortMode GetSortMode()
        {
            bool isTaxisDESC = true;
            if (!string.IsNullOrEmpty(base.Request.QueryString["isTaxisDESC"]))
            {
                isTaxisDESC = TranslateUtils.ToBool(base.Request.QueryString["isTaxisDESC"]);
            }
            return isTaxisDESC ? SortMode.DESC : SortMode.ASC;
        }

        private string _pageUrl;
        protected string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = AccountSelect.GetRedirectUrl(this.scriptName, this.isAllAccount, TranslateUtils.ToBool(this.ddlTaxis.SelectedValue), this.ddlStatus.SelectedValue, this.tbKeyword.Text, TranslateUtils.ToInt(Request.QueryString["page"], 1));
                }
                return _pageUrl;
            }
        }

        public static string GetRedirectUrl(string scriptName, bool isAllAccount, bool isTaxisDESC, string status, string keyword, int page)
        {
            return string.Format("modal_accountSelect.aspx?scriptName={0}&isAllAccount={1}&isTaxisDESC={0}&status={1}&keyword={2}&page={3}", isTaxisDESC, status, keyword, page);
        }
	}
}
