using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using System.Web.UI;


namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundRequest : BackgroundBasePage
    {
        public DropDownList ddlTaxis;
        public DropDownList ddlStatus;
        public TextBox tbKeyword;

        public HyperLink hlAdd;

        public Repeater rptContents;
        public SqlPager spContents;

        public void Page_Load(object sender, EventArgs E)
        {
            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = this.GetSelectString();
            this.spContents.SortField = DataProvider.RequestDAO.GetSortFieldName();
            this.spContents.SortMode = this.GetSortMode();
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
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
                ERequestStatusUtils.AddListItems(this.ddlStatus);
                if (!string.IsNullOrEmpty(base.Request.QueryString["status"]))
                {
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlStatus, base.Request.QueryString["status"]);
                }

                this.tbKeyword.Text = base.Request.QueryString["keyword"];

                this.spContents.DataBind();

                this.hlAdd.NavigateUrl = BackgroundRequestAdd.GetAddUrl(this.PageUrl);
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                RequestInfo requestInfo = new RequestInfo(e.Item.DataItem);

                Literal ltlRequestSN = e.Item.FindControl("ltlRequestSN") as Literal;
                Literal ltlSubject = e.Item.FindControl("ltlSubject") as Literal;
                Literal ltlChargeUserName = e.Item.FindControl("ltlChargeUserName") as Literal;
                Literal ltlRequestType = e.Item.FindControl("ltlRequestType") as Literal;
                Literal ltlAccount = e.Item.FindControl("ltlAccount") as Literal;
                Literal ltlWebsite = e.Item.FindControl("ltlWebsite") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlStatus = e.Item.FindControl("ltlStatus") as Literal;
                Literal ltlAnswerUrl = e.Item.FindControl("ltlAnswerUrl") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlRequestSN.Text = requestInfo.RequestSN;

                ltlSubject.Text = string.Format(@"<a href=""{0}"">{1}</a>", BackgroundRequestAnswer.GetRedirectUrl(requestInfo.ID, this.PageUrl), requestInfo.Subject);
                ltlChargeUserName.Text = AdminManager.GetDisplayName(requestInfo.ChargeUserName, true);
                ltlRequestType.Text = requestInfo.RequestType;
                if (requestInfo.AccountID > 0)
                {
                    ltlAccount.Text = string.Format(@"<a href='javascript:;' onclick=""{0}"">{1}</a>", Modal.AccountView.GetShowPopWinString(requestInfo.AccountID), DataProvider.AccountDAO.GetAccountName(requestInfo.AccountID));
                }
                ltlWebsite.Text = requestInfo.Website;
                ltlAddDate.Text = DateUtils.GetDateAndTimeString(requestInfo.AddDate);
                
                ltlStatus.Text = ERequestStatusUtils.GetText(requestInfo.Status);
                if (requestInfo.Status == ERequestStatus.Processing)
                {
                    ltlStatus.Text = string.Format("<span style='color:red'>{0}</span>", ltlStatus.Text);
                }

                ltlAnswerUrl.Text = string.Format(@"<a href=""{0}"">回复</a>", BackgroundRequestAnswer.GetRedirectUrl(requestInfo.ID, this.PageUrl));

                ltlEditUrl.Text = string.Format(@"<a href=""{0}"">编辑</a>", BackgroundRequestAdd.GetEditUrl(requestInfo.ID, this.PageUrl));
            }
        }

        public void Search_OnClick(object sender, System.EventArgs e)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        protected string GetSelectString()
        {
            if (base.Request.QueryString["keyword"] != null)
            {
                return DataProvider.RequestDAO.GetSelectString(AdminManager.Current.UserName, base.Request.QueryString["status"], base.Request.QueryString["keyword"]);
            }
            else
            {
                return DataProvider.RequestDAO.GetSelectString(AdminManager.Current.UserName);
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
                    _pageUrl = BackgroundRequest.GetRedirectUrl(TranslateUtils.ToBool(this.ddlTaxis.SelectedValue), this.ddlStatus.SelectedValue, this.tbKeyword.Text, TranslateUtils.ToInt(Request.QueryString["page"], 1));
                }
                return _pageUrl;
            }
        }

        public static string GetRedirectUrl(bool isTaxisDESC, string status, string keyword, int page)
        {
            return string.Format("background_request.aspx?isTaxisDESC={0}&status={1}&keyword={2}&page={3}", isTaxisDESC, status, keyword, page);
        }
    }
}
