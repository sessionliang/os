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
    public class Request : BackgroundBasePage
    {
        public DropDownList ddlStatus;
        public TextBox tbKeyword;

        public Repeater rptContents;
        public SqlPager spContents;

        private string domain = string.Empty;
        private int licenseID = 0;

        protected override bool IsAccessable
        {
            get
            {
                return true;
            }
        }

        public void Page_Load(object sender, EventArgs E)
        {
            this.domain = base.GetQueryString("domain");
            this.licenseID = TranslateUtils.ToInt(base.GetQueryString("licenseID"));
            bool isValid = !string.IsNullOrEmpty(this.domain) && this.licenseID > 0;
            if (isValid)
            {
                isValid = this.licenseID == DataProvider.DBLicenseDAO.GetLicenseID(domain);
            }

            if (isValid)
            {
                this.spContents.ControlToPaginate = this.rptContents;
                this.spContents.ItemsPerPage = 30;
                this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
                this.spContents.SelectCommand = this.GetSelectString();
                this.spContents.SortField = DataProvider.RequestDAO.GetSortFieldName();
                this.spContents.SortMode = SortMode.DESC;
                this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

                if (!IsPostBack)
                {
                    ListItem listItem = new ListItem("È«²¿", string.Empty);
                    this.ddlStatus.Items.Add(listItem);
                    ERequestStatusUtils.AddListItems(this.ddlStatus);
                    if (!string.IsNullOrEmpty(base.GetQueryString("status")))
                    {
                        ControlUtils.SelectListItemsIgnoreCase(this.ddlStatus, base.GetQueryString("status"));
                    }

                    this.tbKeyword.Text = base.GetQueryString("keyword");

                    this.spContents.DataBind();
                }
            }
            else
            {
                Page.Response.Write(string.Empty);
                Page.Response.End();
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                RequestInfo requestInfo = new RequestInfo(e.Item.DataItem);

                Literal ltlRequestSN = e.Item.FindControl("ltlRequestSN") as Literal;
                Literal ltlSubject = e.Item.FindControl("ltlSubject") as Literal;
                Literal ltlRequestType = e.Item.FindControl("ltlRequestType") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlStatus = e.Item.FindControl("ltlStatus") as Literal;

                ltlRequestSN.Text = requestInfo.RequestSN;

                ltlSubject.Text = string.Format(@"<a href=""{0}"">{1}</a>", RequestAnswer.GetRedirectUrl(this.domain, this.licenseID, requestInfo.ID, this.PageUrl), requestInfo.Subject);
                ltlRequestType.Text = requestInfo.RequestType;
                ltlAddDate.Text = DateUtils.GetDateAndTimeString(requestInfo.AddDate);
                
                ltlStatus.Text = ERequestStatusUtils.GetText(requestInfo.Status);
                if (requestInfo.Status == ERequestStatus.Processing)
                {
                    ltlStatus.Text = string.Format("<span style='color:red'>{0}</span>", ltlStatus.Text);
                }
            }
        }

        public void Search_OnClick(object sender, System.EventArgs e)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        protected string GetSelectString()
        {
            if (base.GetQueryString("keyword") != null)
            {
                return DataProvider.RequestDAO.GetSelectString(this.licenseID, this.domain, base.GetQueryString("status"), base.GetQueryString("keyword"));
            }
            else
            {
                return DataProvider.RequestDAO.GetSelectString(this.licenseID, this.domain);
            }
        }

        private string _pageUrl;
        protected string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = GetRedirectUrl(this.domain, this.licenseID, this.ddlStatus.SelectedValue, this.tbKeyword.Text, TranslateUtils.ToInt(base.GetQueryString("page"), 1));
                }
                return _pageUrl;
            }
        }

        public static string GetRedirectUrl(string domain, int licenseID, string status, string keyword, int page)
        {
            return string.Format("request.aspx?domain={0}&licenseID={1}&status={2}&keyword={3}&page={4}", domain, licenseID, status, keyword, page);
        }
    }
}
