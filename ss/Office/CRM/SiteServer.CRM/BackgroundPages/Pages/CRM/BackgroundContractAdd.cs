using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CRM.Core;
using SiteServer.CRM.Model;
using System.Text;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CRM.Controls;
using System.Collections.Specialized;


using System.Web;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.CRM.BackgroundPages
{
    public class BackgroundContractAdd : BackgroundBasePage
    {
        public Literal ltlPageTitle;

        public PlaceHolder phOrder;
        public Literal ltlOrderSN;
        public Literal ltlLoginName;
        public PlaceHolder phAccount;
        public Literal ltlAccountName;
        public Literal ltlAccountChargeUserName;

        public Literal ltlChargeUserName;

        private ContractInfo contractInfo;
        private OrderInfo orderInfo;
        private AccountInfo accountInfo;
        private string returnUrl;

        public static string GetAddUrlToSiteServer(int accountID, string returnUrl)
        {
            return string.Format("background_contractAdd.aspx?AccountID={0}&ReturnUrl={1}", accountID, StringUtils.ValueToUrl(returnUrl));
        }

        public static string GetAddUrlToSiteYun(int orderID, string returnUrl)
        {
            return string.Format("background_contractAdd.aspx?OrderID={0}&ReturnUrl={1}", orderID, StringUtils.ValueToUrl(returnUrl));
        }

        public static string GetEditUrlToSiteYun(int orderID, string returnUrl)
        {
            return string.Format("background_contractAdd.aspx?IsEdit=True&OrderID={0}&ReturnUrl={1}", orderID, StringUtils.ValueToUrl(returnUrl));
        }

        public static string GetEditUrl(int contractID, string returnUrl)
        {
            return string.Format("background_contractAdd.aspx?IsEdit=True&ContractID={0}&ReturnUrl={1}", contractID, StringUtils.ValueToUrl(returnUrl));
        }

        public override string GetValue(string attributeName)
        {
            if (this.contractInfo == null)
            {
                if (attributeName == ContractAttribute.SN)
                {
                    return ProjectManager.GetContractSN(this.orderInfo != null ? EContractType.SiteYun_Order : EContractType.Other);
                }

                if (this.orderInfo != null)
                {
                    if (attributeName == ContractAttribute.ContractReceiver)
                    {
                        return this.orderInfo.InvoiceReceiver;
                    }
                    else if (attributeName == ContractAttribute.ContractTel)
                    {
                        return this.orderInfo.InvoiceTel;
                    }
                    else if (attributeName == ContractAttribute.ContractAddress)
                    {
                        return this.orderInfo.InvoiceAddress;
                    }
                    else if (attributeName == ContractAttribute.Amount)
                    {
                        return this.orderInfo.Amount.ToString();
                    }
                }
            }
            else
            {
                if (attributeName == ContractAttribute.SN)
                {
                    if (string.IsNullOrEmpty(base.GetValue(attributeName)))
                    {
                        return ProjectManager.GetContractSN(this.orderInfo != null ? EContractType.SiteYun_Order : EContractType.Other);
                    }
                }
            }
            return base.GetValue(attributeName);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            int accountID = TranslateUtils.ToInt(Request.QueryString["AccountID"]);
            int orderID = TranslateUtils.ToInt(Request.QueryString["OrderID"]);
            int contractID = TranslateUtils.ToInt(Request.QueryString["ContractID"]);
            bool isEdit = TranslateUtils.ToBool(Request.QueryString["IsEdit"]);
            this.returnUrl = StringUtils.ValueFromUrl(Request.QueryString["ReturnUrl"]);
            if (string.IsNullOrEmpty(this.returnUrl))
            {
                this.returnUrl = BackgroundContract.GetRedirectUrl(true, string.Empty, 1);
            }

            if (isEdit)
            {
                if (contractID > 0)
                {
                    this.contractInfo = DataProvider.ContractDAO.GetContractInfo(contractID);
                }
                else if (orderID > 0)
                {
                    this.contractInfo = DataProvider.ContractDAO.GetContractInfoByOrderID(orderID);
                }

                if (this.contractInfo != null)
                {
                    orderID = this.contractInfo.OrderID;
                    accountID = this.contractInfo.AccountID;
                }
            }
            
            if (orderID > 0)
            {
                this.orderInfo = DataProvider.OrderDAO.GetOrderInfo(orderID);
            }
            else if (accountID > 0)
            {
                this.accountInfo = DataProvider.AccountDAO.GetAccountInfo(accountID);
            }

            if (this.contractInfo != null)
            {
                base.AddAttributes(this.contractInfo.ToNameValueCollection());
                this.ltlPageTitle.Text = "修改合同";
            }
            else
            {
                this.ltlPageTitle.Text = "新增合同";
            }

            if (!IsPostBack)
            {
                if (this.orderInfo != null)
                {
                    this.phOrder.Visible = true;
                    this.ltlOrderSN.Text = this.orderInfo.SN;
                    this.ltlLoginName.Text = this.orderInfo.LoginName;

                    if (this.contractInfo == null)
                    {
                        this.ltlChargeUserName.Text = string.Format(@"<div class=""btn_pencil"" onclick=""{0}""><span class=""pencil""></span>　选择</div><script language=""javascript"">chargeUserName('{1}', '{2}');</script>", Modal.UserNameSelect.GetShowPopWinString(AdminManager.Current.DepartmentID, "chargeUserName"), AdminManager.GetDisplayName(AdminManager.Current.UserName, true), AdminManager.Current.UserName);
                    }
                }
                else if (this.accountInfo != null)
                {
                    this.phAccount.Visible = true;
                    this.ltlAccountName.Text = this.accountInfo.AccountName;
                    this.ltlAccountChargeUserName.Text = AdminManager.GetDisplayName(this.accountInfo.ChargeUserName, true);

                    if (this.contractInfo == null)
                    {
                        this.ltlChargeUserName.Text = string.Format(@"<div class=""btn_pencil"" onclick=""{0}""><span class=""pencil""></span>　选择</div><script language=""javascript"">chargeUserName('{1}', '{2}');</script>", Modal.UserNameSelect.GetShowPopWinString(AdminManager.Current.DepartmentID, "chargeUserName"), AdminManager.GetDisplayName(this.accountInfo.ChargeUserName, true), this.accountInfo.ChargeUserName);
                    }
                }

                if (this.contractInfo != null)
                {
                    this.ltlChargeUserName.Text = string.Format(@"<div class=""btn_pencil"" onclick=""{0}""><span class=""pencil""></span>　选择</div><script language=""javascript"">chargeUserName('{1}', '{2}');</script>", Modal.UserNameSelect.GetShowPopWinString(AdminManager.GetDepartmentID(this.contractInfo.ChargeUserName), "chargeUserName"), AdminManager.GetDisplayName(this.contractInfo.ChargeUserName, true), this.contractInfo.ChargeUserName);
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
                if (this.contractInfo != null)
                {
                    ContractInfo contractInfoToEdit = new ContractInfo(base.Request.Form);
                    foreach (string attributeName in ContractAttribute.AllAttributes)
                    {
                        if (base.Request.Form[attributeName] != null)
                        {
                            this.contractInfo.SetValue(attributeName, contractInfoToEdit.GetValue(attributeName));
                        }
                    }

                    if (this.orderInfo != null)
                    {
                        this.contractInfo.OrderID = this.orderInfo.ID;
                    }
                    else if (this.accountInfo != null)
                    {
                        this.contractInfo.AccountID = this.accountInfo.ID;
                    }

                    DataProvider.ContractDAO.Update(this.contractInfo);

                    base.SuccessMessage("合同修改成功");
                }
                else
                {
                    ContractInfo contractInfoToAdd = new ContractInfo(base.Request.Form);

                    if (this.orderInfo != null)
                    {
                        contractInfoToAdd.OrderID = this.orderInfo.ID;
                    }
                    else if (this.accountInfo != null)
                    {
                        contractInfoToAdd.AccountID = this.accountInfo.ID;
                    }

                    DataProvider.ContractDAO.Insert(contractInfoToAdd);

                    base.SuccessMessage("合同添加成功");
                }

                if (this.orderInfo != null)
                {
                    DataProvider.OrderDAO.UpdateIsContract(this.orderInfo.ID, true);
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
