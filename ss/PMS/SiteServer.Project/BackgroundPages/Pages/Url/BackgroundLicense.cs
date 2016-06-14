using System;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using BaiRong.Controls;
using BaiRong.Core.Data.Provider;
using BaiRong.Core;
using BaiRong.Model;
using System.Collections;

namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundLicense : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;
        public Button AddButton;

        public TextBox tbDomain;
        public DropDownList ddlProductID;
        public TextBox tbSiteName;
        public TextBox tbClientName;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.Request.QueryString["Delete"] != null)
            {
                int id = int.Parse(base.Request.QueryString["ID"]);
                try
                {
                    DataProvider.DBLicenseDAO.Delete(id);
                    base.SuccessMessage("成功删除许可证");
                }
                catch
                {
                    base.FailMessage("删除许可证失败");
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 50;
            this.spContents.ConnectionString = ConfigurationManager.OuterConnectionString;
            if (base.Request.QueryString["productID"] == null)
            {
                this.spContents.SelectCommand = DataProvider.DBLicenseDAO.GetSelectSqlString();
            }
            else
            {
                string domain = base.Request.QueryString["domain"];
                string siteName = base.Request.QueryString["siteName"];
                string clientName = base.Request.QueryString["clientName"];

                this.spContents.SelectCommand = DataProvider.DBLicenseDAO.GetSelectSqlString(domain, siteName, clientName);
            }
            this.spContents.SortField = DataProvider.DBLicenseDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                ListItem listItem = new ListItem("全部产品", string.Empty);
                this.ddlProductID.Items.Add(listItem);

                //ArrayList arraylist = ProductManager.GetAllProductIDArrayList();
                //foreach (string productID in arraylist)
                //{
                //    listItem = new ListItem(ProductManager.GetProductName(productID, true), productID);
                //    this.ddlProductID.Items.Add(listItem);
                //}

                //if (base.Request.QueryString["productID"] != null)
                //{
                //    this.tbDomain.Text = base.Request.QueryString["domain"];
                //    ControlUtils.SelectListItemsIgnoreCase(this.ddlProductID, base.Request.QueryString["productID"]);
                //    this.tbSiteName.Text = base.Request.QueryString["siteName"];
                //    this.tbClientName.Text = base.Request.QueryString["clientName"];
                //}

                this.spContents.DataBind();

                this.AddButton.Attributes.Add("onclick", "location.href='product_licenseAdd.aspx';return false;");
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int id = (int)DataBinder.Eval(e.Item.DataItem, "ID");
                string domain = (string)DataBinder.Eval(e.Item.DataItem, "Domain");
                string macAddress = (string)DataBinder.Eval(e.Item.DataItem, "MacAddress");
                string processorID = (string)DataBinder.Eval(e.Item.DataItem, "ProcessorID");
                int maxSiteNumber = (int)DataBinder.Eval(e.Item.DataItem, "MaxSiteNumber");
                DateTime licenseDate = (DateTime)DataBinder.Eval(e.Item.DataItem, "LicenseDate");
                string siteName = (string)DataBinder.Eval(e.Item.DataItem, "SiteName");
                string clientName = (string)DataBinder.Eval(e.Item.DataItem, "ClientName");
                int accountID = (int)DataBinder.Eval(e.Item.DataItem, "AccountID");
                int orderID = (int)DataBinder.Eval(e.Item.DataItem, "OrderID");
                DateTime expireDate = (DateTime)DataBinder.Eval(e.Item.DataItem, "ExpireDate");
                string summary = (string)DataBinder.Eval(e.Item.DataItem, "Summary");

                Literal ltlLicenseString = e.Item.FindControl("ltlLicenseString") as Literal;
                Literal ltlProductID = e.Item.FindControl("ltlProductID") as Literal;
                Literal ltlMaxSiteNumber = e.Item.FindControl("ltlMaxSiteNumber") as Literal;
                Literal ltlLicenseDate = e.Item.FindControl("ltlLicenseDate") as Literal;
                Literal ltlSiteName = e.Item.FindControl("ltlSiteName") as Literal;
                Literal ltlClientName = e.Item.FindControl("ltlClientName") as Literal;
                Literal ltlAccountOrder = e.Item.FindControl("ltlAccountOrder") as Literal;
                Literal ltlExpireDate = e.Item.FindControl("ltlExpireDate") as Literal;
                Literal ltlSummary = e.Item.FindControl("ltlSummary") as Literal;

                //ltlProductID.Text = ProductManager.GetProductName(productID, false);

                ltlMaxSiteNumber.Text = maxSiteNumber == 0 ? "不限制" : maxSiteNumber.ToString();

                ltlLicenseString.Text = domain;
                //if (productID == ProductManager.CMS.ModuleID && licenseType == ELicenseType.SelfService)
                //{
                //    ltlLicenseString.Text += "<code>自助建站</code>";
                //}
                if (!StringUtils.IsNullorEmpty(macAddress))
                {
                    ltlLicenseString.Text += string.Format(":{0}", macAddress);
                }
                ltlLicenseString.Text = string.Format(@"<a href='javascript:;' onclick=""{0}"">{1}</a>", Modal.LicenseView.GetShowPopWinString(id), ltlLicenseString.Text);

                ltlLicenseDate.Text = DateUtils.GetDateString(licenseDate);
                ltlSiteName.Text = siteName;
                ltlClientName.Text = clientName;

                if (accountID > 0)
                {
                    ltlAccountOrder.Text = string.Format(@"客户：<a href='javascript:;' onclick=""{0}"">{1}</a>", Modal.AccountView.GetShowPopWinString(accountID), DataProvider.AccountDAO.GetAccountName(accountID));
                }
                if (orderID > 0)
                {
                    ltlAccountOrder.Text += string.Format(@"订单：<a href='javascript:;' onclick=""{0}"">{1}</a>", Modal.OrderView.GetShowPopWinString(orderID), DataProvider.OrderDAO.GetOrderSN(orderID));
                }

                ltlExpireDate.Text = DateUtils.GetDateString(expireDate);

                ltlSummary.Text = summary;
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
                    _pageUrl = BackgroundLicense.GetRedirectUrl(this.tbDomain.Text, this.ddlProductID.SelectedValue, this.tbSiteName.Text, this.tbClientName.Text);
                }
                return _pageUrl;
            }
        }

        private static string GetRedirectUrl(string domain, string productID, string siteName, string clientName)
        {
            return string.Format("product_license.aspx?domain={0}&productID={1}&siteName={2}&clientName={3}", domain, productID, siteName, clientName);
        }
    }
}
