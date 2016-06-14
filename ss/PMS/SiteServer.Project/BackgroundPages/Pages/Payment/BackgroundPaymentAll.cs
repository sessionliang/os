using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.Project.Core;
using System.Web.UI;
using SiteServer.Project.Model;
using System.Collections;

namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundPaymentAll : BackgroundBasePage
    {
        public DataGrid MyDataGrid;
        public Literal ltlProjectCount;
        public Literal ltlPaymentCount;
        public Literal ltlPaymentAmountCount;
        public Literal ltlInvoiceCount;
        public Literal ltlInvoiceAmountCount;

        private string type;
        private Hashtable hashtableProject = new Hashtable();
        private int paymentCount = 0;
        private int paymentAmountCount = 0;
        private int invoiceCount = 0;
        private int invoiceAmountCount = 0;

        public void Page_Load(object sender, EventArgs E)
        {
            this.type = base.Request.QueryString["type"];

            if (!IsPostBack)
            {
                MyDataGrid.DataSource = DataProvider.PaymentDAO.GetDataSource(0, false);
                MyDataGrid.ItemDataBound += new DataGridItemEventHandler(MyDataGrid_ItemDataBound);
                MyDataGrid.DataBind();

                this.ltlProjectCount.Text = this.hashtableProject.Count.ToString();
                this.ltlPaymentCount.Text = this.paymentCount.ToString();
                this.ltlPaymentAmountCount.Text = "¥" + this.paymentAmountCount.ToString("N");
                this.ltlInvoiceCount.Text = this.invoiceCount.ToString();
                this.ltlInvoiceAmountCount.Text = "¥" + this.invoiceAmountCount.ToString("N");
            }
        }

        void MyDataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int paymentID = TranslateUtils.EvalInt(e.Item.DataItem, "PaymentID");
                int projectID = TranslateUtils.EvalInt(e.Item.DataItem, "ProjectID");
                int paymentOrder = TranslateUtils.EvalInt(e.Item.DataItem, "PaymentOrder");
                string premise = TranslateUtils.EvalString(e.Item.DataItem, "Premise");
                DateTime paymentDate = TranslateUtils.EvalDateTime(e.Item.DataItem, "PaymentDate");
                int amountPaid = TranslateUtils.EvalInt(e.Item.DataItem, "AmountPaid");
                bool isInvoice = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, "IsInvoice"));
                string invoiceNO = TranslateUtils.EvalString(e.Item.DataItem, "InvoiceNO");
                bool isPayment = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, "IsPayment"));

                ProjectInfo projectInfo = DataProvider.ProjectDAO.GetProjectInfo(projectID);

                this.hashtableProject[projectID] = projectID;
                this.paymentCount += 1;
                this.paymentAmountCount += amountPaid;
                if (isInvoice)
                {
                    this.invoiceCount += 1;
                    this.invoiceAmountCount += amountPaid;
                }

                Literal ltlProjectName = e.Item.FindControl("ltlProjectName") as Literal;
                Literal ltlContractNO = e.Item.FindControl("ltlContractNO") as Literal;
                Literal ltlInvoiceNO = e.Item.FindControl("ltlInvoiceNO") as Literal;
                Literal ltlPaymentOrder = e.Item.FindControl("ltlPaymentOrder") as Literal;
                Literal ltlPremise = e.Item.FindControl("ltlPremise") as Literal;
                Literal ltlPaymentDate = e.Item.FindControl("ltlPaymentDate") as Literal;
                Literal ltlAmountPaid = e.Item.FindControl("ltlAmountPaid") as Literal;
                Literal ltlIsInvoice = e.Item.FindControl("ltlIsInvoice") as Literal;

                ltlProjectName.Text = projectInfo.ProjectName;
                ltlContractNO.Text = projectInfo.ContractNO;
                ltlInvoiceNO.Text = invoiceNO;
                ltlPaymentOrder.Text = paymentOrder.ToString();
                ltlPremise.Text = premise;
                ltlPaymentDate.Text = DateUtils.GetDateString(paymentDate);
                ltlAmountPaid.Text = "¥" + amountPaid.ToString("N");
                ltlIsInvoice.Text = StringUtils.GetTrueImageHtml(isInvoice);
            }
        }
    }
}
