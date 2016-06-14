using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using BaiRong.Core.Data.Provider;
using System.Collections.Specialized;
using BaiRong.Controls;
using BaiRong.Model;

namespace SiteServer.Project.BackgroundPages.Modal
{
	public class PaymentAdd : BackgroundBasePage
	{
        protected RadioButtonList rblIsCashBack;

        protected DropDownList ddlPaymentType;
        protected TextBox tbPaymentOrder;
        protected TextBox tbPremise;
        protected TextBox tbExpectDate;
        protected TextBox tbAmountExpect;

        protected RadioButtonList rblIsInvoice;
        protected PlaceHolder phIsInvoice;
        protected TextBox tbInvoiceNO;
        public TextBox tbInvoiceDate;

        protected RadioButtonList rblIsPayment;
        protected PlaceHolder phIsPayment;
        protected TextBox tbAmountPaid;
        protected TextBox tbPaymentDate;

        private int projectID;
        private PaymentInfo paymentInfo;
        private string returnUrl;
        private bool isCashBack = false;

        public static string GetShowPopWinStringToAdd(int projectID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("ProjectID", projectID.ToString());
            arguments.Add("returnUrl", StringUtils.ValueToUrl(returnUrl));
            return JsUtils.OpenWindow.GetOpenWindowString("添加款项", "modal_paymentAdd.aspx", arguments);
        }

        public static string GetShowPopWinStringToEdit(int projectID, int paymentID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("ProjectID", projectID.ToString());
            arguments.Add("PaymentID", paymentID.ToString());
            arguments.Add("returnUrl", StringUtils.ValueToUrl(returnUrl));
            return JsUtils.OpenWindow.GetOpenWindowString("修改款项", "modal_paymentAdd.aspx", arguments);
        }

		public void Page_Load(object sender, EventArgs E)
		{
            this.projectID = TranslateUtils.ToInt(base.Request.QueryString["ProjectID"]);
            int paymentID = TranslateUtils.ToInt(base.Request.QueryString["PaymentID"]);
            this.returnUrl = StringUtils.ValueFromUrl(base.Request.QueryString["returnUrl"]);

            if (paymentID > 0)
            {
                this.paymentInfo = DataProvider.PaymentDAO.GetPaymentInfo(paymentID);
            }

			if (!IsPostBack)
			{
                EBooleanUtils.AddListItems(this.rblIsCashBack, "返款", "回款");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsCashBack, false.ToString());

                EBooleanUtils.AddListItems(this.rblIsInvoice, "是", "否");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsInvoice, false.ToString());

                EBooleanUtils.AddListItems(this.rblIsPayment, "是", "否");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsPayment, false.ToString());

                if (paymentInfo != null)
                {
                    ControlUtils.SelectListItemsIgnoreCase(this.rblIsCashBack, paymentInfo.IsCashBack.ToString());

                    ControlUtils.SelectListItemsIgnoreCase(this.ddlPaymentType, paymentInfo.PaymentType);
                    this.tbPaymentOrder.Text = paymentInfo.PaymentOrder.ToString();
                    this.tbPremise.Text = paymentInfo.Premise;
                    this.tbExpectDate.Text = DateUtils.GetDateString(paymentInfo.ExpectDate);
                    this.tbAmountExpect.Text = paymentInfo.AmountExpect.ToString();

                    ControlUtils.SelectListItemsIgnoreCase(this.rblIsInvoice, paymentInfo.IsInvoice.ToString());
                    this.phIsInvoice.Visible = paymentInfo.IsInvoice;
                    this.tbInvoiceNO.Text = paymentInfo.InvoiceNO;
                    this.tbInvoiceDate.Text = DateUtils.GetDateString(paymentInfo.InvoiceDate);

                    ControlUtils.SelectListItemsIgnoreCase(this.rblIsPayment, paymentInfo.IsPayment.ToString());
                    this.phIsPayment.Visible = paymentInfo.IsPayment;
                    this.tbAmountPaid.Text = paymentInfo.AmountPaid.ToString();
                    this.tbPaymentDate.Text = DateUtils.GetDateString(paymentInfo.PaymentDate);
                }
                else
                {
                    this.tbPaymentOrder.Text = (DataProvider.PaymentDAO.GetMaxPaymentOrder(this.projectID) + 1).ToString();
                }

                this.SelectedIndexChanged(null, EventArgs.Empty);
                if (paymentInfo != null)
                {
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlPaymentType, paymentInfo.PaymentType);
                }
			}
		}

        public string GetName()
        {
            return this.isCashBack ? "返款" : "回款";
        }

        public void SelectedIndexChanged(object sender, EventArgs e)
        {
            this.isCashBack = TranslateUtils.ToBool(this.rblIsCashBack.SelectedValue);

            this.phIsPayment.Visible = TranslateUtils.ToBool(this.rblIsPayment.SelectedValue);
            this.phIsInvoice.Visible = TranslateUtils.ToBool(this.rblIsInvoice.SelectedValue);
            
            this.ddlPaymentType.Items.Clear();

            if (this.isCashBack)
            {
                this.phIsInvoice.Visible = false;

                ControlUtils.AddListControlItems(this.ddlPaymentType, ConfigurationManager.Additional.PaymentCashBackTypeCollection);
            }
            else
            {
                ControlUtils.AddListControlItems(this.ddlPaymentType, ConfigurationManager.Additional.PaymentTypeCollection);
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            if (this.paymentInfo != null)
            {
				try
				{
                    this.paymentInfo.IsCashBack = TranslateUtils.ToBool(this.rblIsCashBack.SelectedValue);
                    this.paymentInfo.PaymentType = this.ddlPaymentType.SelectedValue;

                    this.paymentInfo.PaymentOrder = TranslateUtils.ToInt(this.tbPaymentOrder.Text);
                    this.paymentInfo.Premise = this.tbPremise.Text;
                    this.paymentInfo.ExpectDate = TranslateUtils.ToDateTime(this.tbExpectDate.Text);
                    this.paymentInfo.AmountExpect = TranslateUtils.ToInt(this.tbAmountExpect.Text);

                    this.paymentInfo.IsInvoice = TranslateUtils.ToBool(this.rblIsInvoice.SelectedValue);
                    this.paymentInfo.InvoiceNO = this.tbInvoiceNO.Text;
                    this.paymentInfo.InvoiceDate = TranslateUtils.ToDateTime(this.tbInvoiceDate.Text);

                    this.paymentInfo.IsPayment = TranslateUtils.ToBool(this.rblIsPayment.SelectedValue);
                    this.paymentInfo.AmountPaid = TranslateUtils.ToInt(this.tbAmountPaid.Text);
                    this.paymentInfo.PaymentDate = TranslateUtils.ToDateTime(this.tbPaymentDate.Text);

                    DataProvider.PaymentDAO.Update(this.paymentInfo);

					isChanged = true;
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, this.GetName() + "修改失败！");
				}
			}
			else
			{
                try
                {
                    this.paymentInfo = new PaymentInfo();
                    this.paymentInfo.ProjectID = this.projectID;

                    this.paymentInfo.IsCashBack = TranslateUtils.ToBool(this.rblIsCashBack.SelectedValue);
                    this.paymentInfo.PaymentType = this.ddlPaymentType.SelectedValue;

                    this.paymentInfo.PaymentOrder = TranslateUtils.ToInt(this.tbPaymentOrder.Text);
                    this.paymentInfo.Premise = this.tbPremise.Text;
                    this.paymentInfo.ExpectDate = TranslateUtils.ToDateTime(this.tbExpectDate.Text);
                    this.paymentInfo.AmountExpect = TranslateUtils.ToInt(this.tbAmountExpect.Text);

                    this.paymentInfo.IsInvoice = TranslateUtils.ToBool(this.rblIsInvoice.SelectedValue);
                    this.paymentInfo.InvoiceNO = this.tbInvoiceNO.Text;
                    this.paymentInfo.InvoiceDate = TranslateUtils.ToDateTime(this.tbInvoiceDate.Text);

                    this.paymentInfo.IsPayment = TranslateUtils.ToBool(this.rblIsPayment.SelectedValue);
                    this.paymentInfo.AmountPaid = TranslateUtils.ToInt(this.tbAmountPaid.Text);
                    this.paymentInfo.PaymentDate = TranslateUtils.ToDateTime(this.tbPaymentDate.Text);

                    DataProvider.PaymentDAO.Insert(this.paymentInfo);

                    isChanged = true;
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, this.GetName() + "添加失败！");
                }
			}

			if (isChanged)
			{
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, BackgroundPayment.GetRedirectUrl(this.projectID, this.returnUrl));
			}
		}
	}
}
