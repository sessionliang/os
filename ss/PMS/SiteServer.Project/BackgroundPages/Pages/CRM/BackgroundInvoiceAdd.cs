using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using System.Text;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.Project.Controls;
using System.Collections.Specialized;


using System.Web;

namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundInvoiceAdd : BackgroundBasePage
    {
        public Literal ltlPageTitle;

        public Literal ltlInvoiceType;
        public PlaceHolder phSiteYun;
        public Literal ltlOrderSN;
        public Literal ltlLoginName;
        public PlaceHolder phSiteServer;
        public Literal ltlAccountName;
        public Literal ltlChargeUserName;

        private InvoiceInfo invoiceInfo;
        private OrderInfo orderInfo;
        private AccountInfo accountInfo;
        private string returnUrl;

        public static string GetAddUrlToSiteServer(int accountID, string returnUrl)
        {
            return string.Format("background_invoiceAdd.aspx?AccountID={0}&ReturnUrl={1}", accountID, StringUtils.ValueToUrl(returnUrl));
        }

        public static string GetAddUrlToSiteYun(int orderID, string returnUrl)
        {
            return string.Format("background_invoiceAdd.aspx?OrderID={0}&ReturnUrl={1}", orderID, StringUtils.ValueToUrl(returnUrl));
        }

        public static string GetEditUrlToSiteYun(int orderID, string returnUrl)
        {
            return string.Format("background_invoiceAdd.aspx?IsEdit=True&OrderID={0}&ReturnUrl={1}", orderID, StringUtils.ValueToUrl(returnUrl));
        }

        public static string GetEditUrl(int invoiceID, string returnUrl)
        {
            return string.Format("background_invoiceAdd.aspx?IsEdit=True&InvoiceID={0}&ReturnUrl={1}", invoiceID, StringUtils.ValueToUrl(returnUrl));
        }

        public override string GetValue(string attributeName)
        {
            if (this.invoiceInfo == null)
            {
                if (attributeName == InvoiceAttribute.SN)
                {
                    return ProjectManager.GetInvoiceSN(this.orderInfo != null ? EInvoiceType.SiteYun : EInvoiceType.SiteServer);
                }

                if (this.orderInfo != null)
                {
                    if (attributeName == InvoiceAttribute.InvoiceTitle)
                    {
                        return this.orderInfo.InvoiceTitle;
                    }
                    else if (attributeName == InvoiceAttribute.InvoiceReceiver)
                    {
                        return this.orderInfo.InvoiceReceiver;
                    }
                    else if (attributeName == InvoiceAttribute.InvoiceTel)
                    {
                        return this.orderInfo.InvoiceTel;
                    }
                    else if (attributeName == InvoiceAttribute.InvoiceAddress)
                    {
                        return this.orderInfo.InvoiceAddress;
                    }
                    else if (attributeName == InvoiceAttribute.Amount)
                    {
                        return this.orderInfo.Amount.ToString();
                    }
                }
            }
            else
            {
                if (attributeName == InvoiceAttribute.SN)
                {
                    if (string.IsNullOrEmpty(base.GetValue(attributeName)))
                    {
                        return ProjectManager.GetInvoiceSN(this.orderInfo != null ? EInvoiceType.SiteYun : EInvoiceType.SiteServer);
                    }
                }
            }
            return base.GetValue(attributeName);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            int accountID = TranslateUtils.ToInt(Request.QueryString["AccountID"]);
            int orderID = TranslateUtils.ToInt(Request.QueryString["OrderID"]);
            int invoiceID = TranslateUtils.ToInt(Request.QueryString["InvoiceID"]);
            bool isEdit = TranslateUtils.ToBool(Request.QueryString["IsEdit"]);
            this.returnUrl = StringUtils.ValueFromUrl(Request.QueryString["ReturnUrl"]);
            if (string.IsNullOrEmpty(this.returnUrl))
            {
                this.returnUrl = BackgroundInvoice.GetRedirectUrl(true, string.Empty, 1);
            }

            if (isEdit)
            {
                if (invoiceID > 0)
                {
                    this.invoiceInfo = DataProvider.InvoiceDAO.GetInvoiceInfo(invoiceID);
                }
                else if (orderID > 0)
                {
                    this.invoiceInfo = DataProvider.InvoiceDAO.GetInvoiceInfoByOrderID(orderID);
                }

                if (this.invoiceInfo != null)
                {
                    orderID = this.invoiceInfo.OrderID;
                    accountID = this.invoiceInfo.AccountID;
                }
            }
            
            if (orderID > 0)
            {
                this.orderInfo = DataProvider.OrderDAO.GetOrderInfo(orderID);
                this.ltlInvoiceType.Text = EInvoiceTypeUtils.GetText(EInvoiceType.SiteYun);
            }
            else if (accountID > 0)
            {
                this.accountInfo = DataProvider.AccountDAO.GetAccountInfo(accountID);
                this.ltlInvoiceType.Text = EInvoiceTypeUtils.GetText(EInvoiceType.SiteServer);
            }

            if (this.invoiceInfo != null)
            {
                base.AddAttributes(this.invoiceInfo);
                this.ltlPageTitle.Text = "修改发票";
            }
            else
            {
                this.ltlPageTitle.Text = "新增发票";
            }

            if (!IsPostBack)
            {
                if (this.orderInfo != null)
                {
                    this.phSiteYun.Visible = true;
                    this.ltlOrderSN.Text = this.orderInfo.SN;
                    this.ltlLoginName.Text = this.orderInfo.LoginName;
                }
                else if (this.accountInfo != null)
                {
                    this.phSiteServer.Visible = true;
                    this.ltlAccountName.Text = this.accountInfo.AccountName;
                    this.ltlChargeUserName.Text = AdminManager.GetDisplayName(this.accountInfo.ChargeUserName, true);
                }
            }
        }

        public void Return_OnClick(object sender, System.EventArgs e)
        {
            PageUtils.Redirect(this.returnUrl);
        }

        public override void Submit_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                if (this.invoiceInfo != null)
                {
                    InvoiceInfo invoiceInfoToEdit = DataProvider.InvoiceDAO.GetInvoiceInfo(base.Request.Form);
                    foreach (string attributeName in InvoiceAttribute.BasicAttributes)
                    {
                        if (base.Request.Form[attributeName] != null)
                        {
                            this.invoiceInfo.SetExtendedAttribute(attributeName, invoiceInfoToEdit.GetExtendedAttribute(attributeName));
                        }
                    }

                    if (this.orderInfo != null)
                    {
                        this.invoiceInfo.InvoiceType = EInvoiceType.SiteYun;
                        this.invoiceInfo.OrderID = this.orderInfo.ID;
                    }
                    else if (this.accountInfo != null)
                    {
                        this.invoiceInfo.InvoiceType = EInvoiceType.SiteServer;
                        this.invoiceInfo.AccountID = this.accountInfo.ID;
                    }

                    DataProvider.InvoiceDAO.Update(this.invoiceInfo);

                    base.SuccessMessage("发票修改成功");
                }
                else
                {
                    InvoiceInfo invoiceInfoToAdd = DataProvider.InvoiceDAO.GetInvoiceInfo(base.Request.Form);

                    if (this.orderInfo != null)
                    {
                        invoiceInfoToAdd.InvoiceType = EInvoiceType.SiteYun;
                        invoiceInfoToAdd.OrderID = this.orderInfo.ID;
                    }
                    else if (this.accountInfo != null)
                    {
                        invoiceInfoToAdd.InvoiceType = EInvoiceType.SiteServer;
                        invoiceInfoToAdd.AccountID = this.accountInfo.ID;
                    }

                    DataProvider.InvoiceDAO.Insert(invoiceInfoToAdd);

                    base.SuccessMessage("发票添加成功");
                }

                if (this.orderInfo != null)
                {
                    DataProvider.OrderDAO.UpdateIsInvoice(this.orderInfo.ID, true);
                }
                
                base.AddWaitAndRedirectScript(this.returnUrl);
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }
    }
}
