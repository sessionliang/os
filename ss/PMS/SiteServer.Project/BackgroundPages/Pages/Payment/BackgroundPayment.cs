using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.Project.Core;
using System.Web.UI;
using SiteServer.Project.Model;

namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundPayment : BackgroundBasePage
    {
        public Literal ltlProjectName;
        public Literal ltlContractNO;
        public Literal ltlAddDate;

        public DataGrid dgContents1;
        public PlaceHolder phCashBack;
        public DataGrid dgContents2;

        public Literal ltlAmountTotal;
        public Literal ltlAmountExpectAll;
        public Literal ltlAmountPaidAll;
        public Literal ltlAmountNotPaidAll;
        public Literal ltlAmountExpectAllCashBack;
        public Literal ltlAmountPaidAllCashBack;
        public Literal ltlAmountNotPaidAllCashBack;

        public Button btnAdd;
        public Button btnReturn;

        private ProjectInfo projectInfo;
        private string returnUrl;

        private int amountExpectCount;
        private int amountExpectAll;
        private int amountPaidCount;
        private int amountPaidAll;
        private int amountNotPaidCount;
        private int amountNotPaidAll;

        private int amountExpectCountCashBack;
        private int amountExpectAllCashBack;
        private int amountPaidCountCashBack;
        private int amountPaidAllCashBack;
        private int amountNotPaidCountCashBack;
        private int amountNotPaidAllCashBack;

        public static string GetRedirectUrl(int projectID, string returnUrl)
        {
            return string.Format("background_payment.aspx?projectID={0}&returnUrl={1}", projectID, StringUtils.ValueToUrl(returnUrl));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            int projectID = TranslateUtils.ToInt(base.Request.QueryString["projectID"]);
            this.projectInfo = DataProvider.ProjectDAO.GetProjectInfo(projectID);
            this.returnUrl = StringUtils.ValueFromUrl(base.Request.QueryString["returnUrl"]);

            if (base.Request.QueryString["Delete"] != null && base.Request.QueryString["PaymentID"] != null)
            {
                int paymentID = TranslateUtils.ToInt(base.Request.QueryString["PaymentID"]);
                try
                {
                    DataProvider.PaymentDAO.Delete(paymentID);
                    base.SuccessMessage("成功删除回款");
                }
                catch (Exception ex)
                {
                    base.SuccessMessage(string.Format("删除回款失败，{0}", ex.Message));
                }
            }

            if (!IsPostBack)
            {
                this.ltlProjectName.Text = this.projectInfo.ProjectName;
                this.ltlContractNO.Text = this.projectInfo.ContractNO;
                this.ltlAddDate.Text = DateUtils.GetDateString(this.projectInfo.AddDate);

                this.dgContents1.DataSource = DataProvider.PaymentDAO.GetDataSource(this.projectInfo.ProjectID, false);
                this.dgContents1.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents1.DataBind();

                this.dgContents2.DataSource = DataProvider.PaymentDAO.GetDataSource(this.projectInfo.ProjectID, true);
                this.dgContents2.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents2.DataBind();

                this.ltlAmountTotal.Text = this.projectInfo.AmountTotal.ToString("c");

                this.ltlAmountExpectAll.Text = this.amountExpectAll.ToString("c") + "<code>" + this.amountExpectCount + "笔</code>";
                this.ltlAmountPaidAll.Text = this.amountPaidAll.ToString("c") + "<code>" + this.amountPaidCount + "笔</code>";
                this.ltlAmountNotPaidAll.Text = this.amountNotPaidAll.ToString("c") + "<code>" + this.amountNotPaidCount + "笔</code>";

                this.ltlAmountExpectAllCashBack.Text = this.amountExpectAllCashBack.ToString("c") + "<code>" + this.amountExpectCountCashBack + "笔</code>";
                this.ltlAmountPaidAllCashBack.Text = this.amountPaidAllCashBack.ToString("c") + "<code>" + this.amountPaidCountCashBack + "笔</code>";
                this.ltlAmountNotPaidAllCashBack.Text = this.amountNotPaidAllCashBack.ToString("c") + "<code>" + this.amountNotPaidCountCashBack + "笔</code>";

                string amountTotalError = string.Empty;
                string amountRemainError = string.Empty;
                int amountRemain = this.projectInfo.AmountTotal - this.amountPaidAll;
                PaymentManager.GetPaymentError(this.projectInfo.AmountTotal, this.amountExpectAll, amountRemain, this.projectInfo.IsClosed, out amountTotalError, out amountRemainError);

                if (!string.IsNullOrEmpty(amountTotalError) || !string.IsNullOrEmpty(amountRemainError))
                {
                    base.FailMessage(amountTotalError + amountRemainError);
                }

                this.btnAdd.Attributes.Add("onclick", Modal.PaymentAdd.GetShowPopWinStringToAdd(projectID, this.returnUrl));

                if (string.IsNullOrEmpty(this.returnUrl))
                {
                    this.btnReturn.Visible = false;
                }
                else
                {
                    this.btnReturn.Attributes.Add("onclick", string.Format("location.href='{0}';return false;", this.returnUrl));
                }
            }
        }

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int paymentID = TranslateUtils.EvalInt(e.Item.DataItem, "PaymentID");
                int projectID = TranslateUtils.EvalInt(e.Item.DataItem, "ProjectID");
                bool isCashBack = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, "IsCashBack"));
                int paymentOrder = TranslateUtils.EvalInt(e.Item.DataItem, "PaymentOrder");
                string premise = TranslateUtils.EvalString(e.Item.DataItem, "Premise");
                DateTime expectDate = TranslateUtils.EvalDateTime(e.Item.DataItem, "ExpectDate");
                int amountExpect = TranslateUtils.EvalInt(e.Item.DataItem, "AmountExpect");
                DateTime paymentDate = TranslateUtils.EvalDateTime(e.Item.DataItem, "PaymentDate");
                int amountPaid = TranslateUtils.EvalInt(e.Item.DataItem, "AmountPaid");
                bool isInvoice = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, "IsInvoice"));
                string invoiceNO = TranslateUtils.EvalString(e.Item.DataItem, "InvoiceNO");
                bool isPayment = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, "IsPayment"));

                if (isCashBack)
                {
                    this.phCashBack.Visible = true;

                    this.amountExpectCountCashBack++;
                    this.amountExpectAllCashBack += amountExpect;
                    if (isPayment)
                    {
                        this.amountPaidCountCashBack++;
                        this.amountPaidAllCashBack += amountPaid;
                    }
                    else
                    {
                        this.amountNotPaidCountCashBack++;
                        this.amountNotPaidAllCashBack += amountExpect;
                    }
                }
                else
                {
                    this.amountExpectCount++;
                    this.amountExpectAll += amountExpect;
                    if (isPayment)
                    {
                        this.amountPaidCount++;
                        this.amountPaidAll += amountPaid;
                    }
                    else
                    {
                        this.amountNotPaidCount++;
                        this.amountNotPaidAll += amountExpect;
                    }
                }                

                Literal ltlInvoiceNO = e.Item.FindControl("ltlInvoiceNO") as Literal;
                Literal ltlPaymentOrder = e.Item.FindControl("ltlPaymentOrder") as Literal;
                Literal ltlPremise = e.Item.FindControl("ltlPremise") as Literal;
                Literal ltlExpectDate = e.Item.FindControl("ltlExpectDate") as Literal;
                Literal ltlAmountExpect = e.Item.FindControl("ltlAmountExpect") as Literal;
                Literal ltlIsInvoice = e.Item.FindControl("ltlIsInvoice") as Literal;
                Literal ltlIsPayment = e.Item.FindControl("ltlIsPayment") as Literal;
                Literal ltlPaymentDate = e.Item.FindControl("ltlPaymentDate") as Literal;
                Literal ltlAmountPaid = e.Item.FindControl("ltlAmountPaid") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                if (ltlInvoiceNO != null)
                {
                    ltlInvoiceNO.Text = invoiceNO;
                }
                ltlPaymentOrder.Text = paymentOrder.ToString();
                ltlPremise.Text = premise;
                ltlExpectDate.Text = DateUtils.GetDateString(expectDate);
                ltlAmountExpect.Text = amountExpect.ToString("c");
                ltlIsInvoice.Text = StringUtils.GetTrueImageHtml(isInvoice);
                
                ltlIsPayment.Text = StringUtils.GetTrueOrFalseImageHtml(isPayment);
                ltlPaymentDate.Text = DateUtils.GetDateString(paymentDate);
                ltlAmountPaid.Text = amountPaid.ToString("c");

                ltlEditUrl.Text = string.Format(@"<a href='javascript:undefined' onclick=""{0}"">编辑</a>", Modal.PaymentAdd.GetShowPopWinStringToEdit(projectID, paymentID, this.returnUrl));

                ltlDeleteUrl.Text = string.Format(@"<a href=""background_payment.aspx?ProjectID={0}&Delete=True&PaymentID={1}&ReturnUrl={2}"" onClick=""javascript:return confirm('此操作将删除此回款，确认吗？');"">删除</a>", projectID, paymentID, StringUtils.ValueToUrl(this.returnUrl));
            }
        }
    }
}
