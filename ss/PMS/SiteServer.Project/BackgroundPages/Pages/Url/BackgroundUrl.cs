using System;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using SiteServer.Project.Core;
using BaiRong.Controls;
using BaiRong.Core.Data.Provider;
using BaiRong.Core;
using BaiRong.Model;

namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundUrl : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;
        public Button AddButton;

        public TextBox tbDomain;
        public DropDownList ddlIsLicense;

        public void Page_Load(object sender, EventArgs E)
        {
            this.spContents.ControlToPaginate = rptContents;
            this.spContents.ItemsPerPage = 50;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            if (base.Request.QueryString["domain"] == null)
            {
                this.spContents.SelectCommand = DataProvider.UrlDAO.GetSelectSqlString();
            }
            else
            {
                string domain = base.Request.QueryString["domain"];
                bool isLicense = TranslateUtils.ToBool(base.Request.QueryString["isLicense"]);

                this.spContents.SelectCommand = DataProvider.UrlDAO.GetSelectSqlString(domain, isLicense);
            }
            this.spContents.SortField = DataProvider.UrlDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;
            rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                EBooleanUtils.AddListItems(this.ddlIsLicense, "显示已授权", "显示全部");
                ControlUtils.SelectListItemsIgnoreCase(this.ddlIsLicense, false.ToString());

                if (base.Request.QueryString["domain"] != null)
                {
                    this.tbDomain.Text = base.Request.QueryString["domain"];
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlIsLicense, base.Request.QueryString["isLicense"]);
                }

                this.spContents.DataBind();

                this.AddButton.Attributes.Add("onclick", "location.href='background_urlAdd.aspx';return false;");
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int id = (int)DataBinder.Eval(e.Item.DataItem, "ID");
                string productID = (string)DataBinder.Eval(e.Item.DataItem, "ProductID");
                string version = (string)DataBinder.Eval(e.Item.DataItem, "Version");
                string databaseType = (string)DataBinder.Eval(e.Item.DataItem, "DatabaseType");
                string domain = (string)DataBinder.Eval(e.Item.DataItem, "Domain");
                DateTime addDate = (DateTime)DataBinder.Eval(e.Item.DataItem, "AddDate");
                DateTime lastActivityDate = (DateTime)DataBinder.Eval(e.Item.DataItem, "LastActivityDate");
                int countOfActivity = (int)DataBinder.Eval(e.Item.DataItem, "CountOfActivity");
                bool isExpire = TranslateUtils.ToBool((string)DataBinder.Eval(e.Item.DataItem, "IsExpire"));
                DateTime expireDate = (DateTime)DataBinder.Eval(e.Item.DataItem, "ExpireDate");
                string expireReason = (string)DataBinder.Eval(e.Item.DataItem, "ExpireReason");
                string summary = (string)DataBinder.Eval(e.Item.DataItem, "Summary");

                int licenseID = 0;
                try
                {
                    licenseID = (int)DataBinder.Eval(e.Item.DataItem, "LicenseID");
                }
                catch { }

                Literal ltlProductID = e.Item.FindControl("ltlProductID") as Literal;
                Literal ltlDomain = e.Item.FindControl("ltlDomain") as Literal;
                Literal ltlVersion = e.Item.FindControl("ltlVersion") as Literal;
                Literal ltlDbType = e.Item.FindControl("ltlDbType") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlLastActivity = e.Item.FindControl("ltlLastActivity") as Literal;
                Literal ltlCountOfActivity = e.Item.FindControl("ltlCountOfActivity") as Literal;
                Literal ltlSummary = e.Item.FindControl("ltlSummary") as Literal;
                Literal ltlIsLicense = e.Item.FindControl("ltlIsLicense") as Literal;

                ltlProductID.Text = productID;
                ltlDomain.Text = domain;
                ltlVersion.Text = version;
                ltlDbType.Text = databaseType;
                ltlAddDate.Text = DateUtils.GetDateString(addDate);
                ltlLastActivity.Text = DateUtils.GetDateAndTimeString(lastActivityDate);
                ltlCountOfActivity.Text = countOfActivity.ToString();

                string text = string.IsNullOrEmpty(summary) ? "添加备注" : "修改备注";
                string css = string.IsNullOrEmpty(summary) ? "" : "btn-success";
                ltlSummary.Text = string.Format(@"<a class=""btn {0}"" href=""javascript:;"" onclick=""{1}"">{2}</a>", css, Modal.UrlSummaryAdd.GetShowPopWinString(id, this.PageUrl), text);

                text = "设置许可限制";
                css = "";
                if (isExpire)
                {
                    if (expireDate > DateTime.Now)
                    {
                        text = DateUtils.DateDiff("day", new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), new DateTime(expireDate.Year, expireDate.Month, expireDate.Day)) + "天后到期";
                    }
                    else
                    {
                        text = "已到期，禁止登录";
                    }
                    
                    css = "btn btn-danger";
                }
                ltlIsLicense.Text += string.Format(@"<a class=""{0}"" href=""javascript:;"" onclick=""{1}"">{2}</a>", css, Modal.ExpireAdd.GetShowPopWinString(id, this.PageUrl), text);
            }
        }

        public void Search_OnClick(object sender, System.EventArgs e)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        private string _pageUrl;
        protected string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = BackgroundUrl.GetRedirectUrl(this.tbDomain.Text, TranslateUtils.ToBool(this.ddlIsLicense.SelectedValue));
                }
                return _pageUrl;
            }
        }

        public static string GetRedirectUrl(string domain, bool isLicense)
        {
            return string.Format("product_url.aspx?domain={0}&isLicense={1}", domain, isLicense);
        }
    }
}
