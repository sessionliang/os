using System;
using System.Collections;
using System.Text;
using System.Web.UI.WebControls;
using SiteServer.Project.Model;
using SiteServer.Project.Core;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Controls;

namespace SiteServer.Project.BackgroundPages
{
	public class BackgroundLicenseAdd : BackgroundBasePage
	{
        public DropDownList ddlProductID;
        public TextBox Domain;
        public TextBox MacAddress;

        public PlaceHolder phProductType;
        public DropDownList ddlProductType;
        public PlaceHolder phMaxSiteNumber;
        public TextBox tbMaxSiteNumber;

        public TextBox SiteName;
        public TextBox ClientName;
        public PlaceHolder phExpireDate;
        public DateTimeTextBox tbExpireDate;
        public TextBox Summary;

        private int id;
        private int accountID;
        private string accountName;
        private int orderID;
        private string orderName;
        private string returnUrl;

        public static string GetAddUrlToSiteYun(int orderID, string returnUrl)
        {
            return string.Format("product_licenseAdd.aspx?OrderID={0}&ReturnUrl={1}", orderID, StringUtils.ValueToUrl(returnUrl));
        }

		public void Page_Load(object sender, EventArgs E)
		{
            this.id = TranslateUtils.ToInt(base.Request.QueryString["id"]);
            this.orderID = TranslateUtils.ToInt(base.Request.QueryString["orderID"]);
            this.returnUrl = StringUtils.ValueFromUrl(base.Request.QueryString["returnUrl"]);

			if (!IsPostBack)
			{
                foreach (string productID in AppManager.GetAppIDList())
                {
                    ListItem listitem = new ListItem(AppManager.GetAppName(productID, true), productID);
                    this.ddlProductID.Items.Add(listitem);
                }

                ListItem listItem = new ListItem("自助建站版本（Self Service）", "self-service");
                this.ddlProductType.Items.Add(listItem);
                listItem = new ListItem("单站点版本", "1");
                this.ddlProductType.Items.Add(listItem);
                listItem = new ListItem("多站点版本（6个站点）", "6");
                this.ddlProductType.Items.Add(listItem);
                listItem = new ListItem("多站点版本（50个站点）", "50");
                this.ddlProductType.Items.Add(listItem);
                listItem = new ListItem("网站群版本", "0");
                this.ddlProductType.Items.Add(listItem);
                listItem = new ListItem("自定义站点数", "-1");
                this.ddlProductType.Items.Add(listItem);

                this.tbExpireDate.DateTime = DateTime.Now.AddMonths(1);

                if (this.orderID > 0)
                {
                    OrderInfo orderInfo = DataProvider.OrderDAO.GetOrderInfo(this.orderID);
                    this.Domain.Text = orderInfo.DomainFormal;
                    this.orderName = orderInfo.SN;

                    this.id = DataProvider.DBLicenseDAO.GetLicenseIDByOrderID(this.orderID);
                }

                if (this.id > 0)
                {
                    DBLicenseInfo licenseInfo = DataProvider.DBLicenseDAO.GetLicenseInfo(this.id);

                    //this.ddlProductID.SelectedValue = licenseInfo.ProductID;
                    this.ddlProductID.SelectedValue = AppManager.CMS.AppID;
                    this.Domain.Text = licenseInfo.Domain;
                    this.MacAddress.Text = licenseInfo.MacAddress;
                    //if (licenseInfo.LicenseType == ELicenseType.SelfService)
                    //{
                    //    this.ddlProductType.SelectedValue = "self-service";
                    //}
                    //else
                    //{
                        this.ddlProductType.SelectedValue = licenseInfo.MaxSiteNumber.ToString();
                    //}
                    this.tbMaxSiteNumber.Text = licenseInfo.MaxSiteNumber.ToString();
                    this.SiteName.Text = licenseInfo.SiteName;
                    this.ClientName.Text = licenseInfo.ClientName;
                    this.Summary.Text = licenseInfo.Summary;

                    this.accountID = licenseInfo.AccountID;
                    if (this.accountID > 0)
                    {
                        this.accountName = DataProvider.AccountDAO.GetAccountName(this.accountID);
                    }

                    this.orderID = licenseInfo.OrderID;
                    if (this.orderID > 0)
                    {
                        this.orderName = DataProvider.OrderDAO.GetOrderSN(this.orderID);
                    }
                }
                else
                {
                    
                    if (this.orderID > 0)
                    {
                        
                    }
                }
			}
		}

        public string GetLicenseValue(string type)
        {
            string retval = string.Empty;
            if (type == "AccountID")
            {
                return this.accountID.ToString();
            }
            else if (type == "AccountName")
            {
                return string.IsNullOrEmpty(this.accountName) ? "选择客户" : this.accountName;
            }
            else if (type == "SelectAccount")
            {
                return Modal.AccountSelect.GetShowPopWinString("selectAccount", true);
            }
            else if (type == "OrderID")
            {
                return this.orderID.ToString();
            }
            else if (type == "OrderName")
            {
                return string.IsNullOrEmpty(this.orderName) ? "选择订单" : this.orderName;
            }
            else if (type == "SelectOrder")
            {
                return Modal.OrderSelect.GetShowPopWinString("selectOrder");
            }
            return retval;
        }

        public void ddlProductID_SelectedIndexChanged(object sender, EventArgs e)
        {
            string productID = this.ddlProductID.SelectedValue;
            if (StringUtils.EqualsIgnoreCase(productID, AppManager.WCM.AppID))
            {
                this.phProductType.Visible = false;
                this.phMaxSiteNumber.Visible = true;
                this.phExpireDate.Visible = true;
            }
            else if (StringUtils.EqualsIgnoreCase(productID, AppManager.CMS.AppID))
            {
                this.phProductType.Visible = true;
                this.phMaxSiteNumber.Visible = false;
                this.phExpireDate.Visible = false;
                this.ddlProductType_SelectedIndexChanged(null, EventArgs.Empty);
            }
            else
            {
                this.phProductType.Visible = false;
                this.phMaxSiteNumber.Visible = false;
                this.phExpireDate.Visible = false;
            }
        }

        public void ddlProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlProductType.SelectedValue == "-1" || this.ddlProductType.SelectedValue == "self-service")
            {
                this.phMaxSiteNumber.Visible = true;
            }
            else
            {
                this.phMaxSiteNumber.Visible = false;
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                if (StringUtils.IsNullorEmpty(this.Domain.Text) && StringUtils.IsNullorEmpty(this.MacAddress.Text))
                {
                    base.FailMessage("Domain与MacAddress至少需要填写一项！");
                }
                else
                {
                    int maxSiteNumber = TranslateUtils.ToInt(this.tbMaxSiteNumber.Text);
                    if (!this.phMaxSiteNumber.Visible)
                    {
                        maxSiteNumber = TranslateUtils.ToInt(this.ddlProductType.SelectedValue);
                    }

                    this.accountID = TranslateUtils.ToInt(base.Request.Form["accountID"]);
                    this.orderID = TranslateUtils.ToInt(base.Request.Form["orderID"]);

                    if (this.orderID > 0)
                    {
                        this.id = DataProvider.DBLicenseDAO.GetLicenseIDByOrderID(this.orderID);
                        DataProvider.OrderDAO.UpdateIsLicense(this.orderID, true);
                    }

                    if (this.id > 0)
                    {
                        DBLicenseInfo licenseInfo = DataProvider.DBLicenseDAO.GetLicenseInfo(id);
                        licenseInfo.Domain = this.Domain.Text;
                        licenseInfo.MacAddress = this.MacAddress.Text;
                        licenseInfo.MaxSiteNumber = maxSiteNumber;
                        licenseInfo.SiteName = this.SiteName.Text;
                        licenseInfo.ClientName = this.ClientName.Text;
                        licenseInfo.AccountID = this.accountID;
                        licenseInfo.OrderID = this.orderID;
                        licenseInfo.ExpireDate = this.tbExpireDate.DateTime;
                        licenseInfo.Summary = this.Summary.Text;

                        DataProvider.DBLicenseDAO.Update(licenseInfo);

                        base.SuccessMessage("许可证修改成功！");
                    }
                    else
                    {
                        DBLicenseInfo licenseInfo = new DBLicenseInfo(0, this.Domain.Text, this.MacAddress.Text, string.Empty, string.Empty, maxSiteNumber, DateTime.Now, this.SiteName.Text, this.ClientName.Text, this.accountID, this.orderID, this.tbExpireDate.DateTime, this.Summary.Text);

                        DataProvider.DBLicenseDAO.Insert(licenseInfo);

                        base.SuccessMessage("许可证新增成功！");
                    }

                    if (!string.IsNullOrEmpty(this.returnUrl))
                    {
                        base.AddWaitAndRedirectScript(this.returnUrl);
                    }
                }
            }
        }
	}
}
