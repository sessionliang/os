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
    public class BackgroundPaymentNotPay : BackgroundBasePage
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
                MyDataGrid.DataSource = DataProvider.PaymentDAO.GetDataSourceNotPay(this.type);
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
                DateTime expectDate = TranslateUtils.EvalDateTime(e.Item.DataItem, "ExpectDate");
                int amountExpect = TranslateUtils.EvalInt(e.Item.DataItem, "AmountExpect");
                int amountPaid = TranslateUtils.EvalInt(e.Item.DataItem, "AmountPaid");
                bool isInvoice = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, "IsInvoice"));
                string invoiceNO = TranslateUtils.EvalString(e.Item.DataItem, "InvoiceNO");
                bool isPayment = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, "IsPayment"));

                ProjectInfo projectInfo = DataProvider.ProjectDAO.GetProjectInfo(projectID);

                this.hashtableProject[projectID] = projectID;
                this.paymentCount += 1;
                this.paymentAmountCount += amountExpect;
                if (isInvoice)
                {
                    this.invoiceCount += 1;
                    this.invoiceAmountCount += amountExpect;
                }

                Literal ltlProjectName = e.Item.FindControl("ltlProjectName") as Literal;
                Literal ltlContractNO = e.Item.FindControl("ltlContractNO") as Literal;
                Literal ltlInvoiceNO = e.Item.FindControl("ltlInvoiceNO") as Literal;
                Literal ltlUserNameCollection = e.Item.FindControl("ltlUserNameCollection") as Literal;
                Literal ltlPaymentOrder = e.Item.FindControl("ltlPaymentOrder") as Literal;
                Literal ltlPremise = e.Item.FindControl("ltlPremise") as Literal;
                Literal ltlExpectDate = e.Item.FindControl("ltlExpectDate") as Literal;
                Literal ltlAmountExpect = e.Item.FindControl("ltlAmountExpect") as Literal;
                Literal ltlIsInvoice = e.Item.FindControl("ltlIsInvoice") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlProjectName.Text = projectInfo.ProjectName;
                ltlContractNO.Text = projectInfo.ContractNO;
                ltlInvoiceNO.Text = invoiceNO;
                ltlUserNameCollection.Text = ProjectManager.GetUserNameCollection(projectInfo.UserNameAM, projectInfo.UserNamePM, projectInfo.UserNameCollection);
                ltlPaymentOrder.Text = paymentOrder.ToString();
                ltlPremise.Text = premise;
                ltlExpectDate.Text = DateUtils.GetDateString(expectDate);
                ltlAmountExpect.Text = amountExpect.ToString("c");
                ltlIsInvoice.Text = StringUtils.GetTrueImageHtml(isInvoice);
                ltlEditUrl.Text = string.Format(@"<a href onclick=""{0}"">修改</a>", Modal.PaymentAdd.GetShowPopWinStringToEdit(projectID, paymentID, "background_paymentNotPay.aspx?type=" + this.type));
            }
        }
    }
}
