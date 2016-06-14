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
    public class BackgroundPaymentCashBack : BackgroundBasePage
    {
        public DataGrid MyDataGrid;
        public Literal ltlProjectCount;
        public Literal ltlExpectCount;
        public Literal ltlExpectAmountCount;
        public Literal ltlPaymentCount;
        public Literal ltlPaymentAmountCount;

        private bool isPayment = false;
        private Hashtable hashtableProject = new Hashtable();
        private int expectCount = 0;
        private int expectAmountCount = 0;
        private int paymentCount = 0;
        private int paymentAmountCount = 0;

        public string GetActive(bool isPayment)
        {
            return this.isPayment == isPayment ? "active" : string.Empty;
        }

        public void Page_Load(object sender, EventArgs E)
        {
            this.isPayment = TranslateUtils.ToBool(base.Request.QueryString["isPayment"]);

            if (!IsPostBack)
            {
                MyDataGrid.DataSource = DataProvider.PaymentDAO.GetDataSourceCashBack(this.isPayment);
                MyDataGrid.ItemDataBound += new DataGridItemEventHandler(MyDataGrid_ItemDataBound);
                MyDataGrid.DataBind();

                this.ltlProjectCount.Text = this.hashtableProject.Count.ToString();
                this.ltlExpectCount.Text = this.expectCount.ToString();
                this.ltlExpectAmountCount.Text = this.expectAmountCount.ToString("c");
                this.ltlPaymentCount.Text = this.paymentCount.ToString();
                this.ltlPaymentAmountCount.Text = this.paymentAmountCount.ToString("c");
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
                DateTime paymentDate = TranslateUtils.EvalDateTime(e.Item.DataItem, "PaymentDate");
                int amountExpect = TranslateUtils.EvalInt(e.Item.DataItem, "AmountExpect");
                int amountPaid = TranslateUtils.EvalInt(e.Item.DataItem, "AmountPaid");
                bool isPayment = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, "IsPayment"));

                ProjectInfo projectInfo = DataProvider.ProjectDAO.GetProjectInfo(projectID);

                this.hashtableProject[projectID] = projectID;

                this.expectCount += 1;
                this.expectAmountCount += amountExpect;
                this.paymentCount += 1;
                this.paymentAmountCount += amountPaid;                

                Literal ltlProjectName = e.Item.FindControl("ltlProjectName") as Literal;
                Literal ltlContractNO = e.Item.FindControl("ltlContractNO") as Literal;
                Literal ltlPaymentOrder = e.Item.FindControl("ltlPaymentOrder") as Literal;
                Literal ltlPremise = e.Item.FindControl("ltlPremise") as Literal;
                Literal ltlExpectDate = e.Item.FindControl("ltlExpectDate") as Literal;
                Literal ltlAmountExpect = e.Item.FindControl("ltlAmountExpect") as Literal;
                Literal ltlPaymentDate = e.Item.FindControl("ltlPaymentDate") as Literal;
                Literal ltlAmountPaid = e.Item.FindControl("ltlAmountPaid") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlProjectName.Text = projectInfo.ProjectName;
                ltlContractNO.Text = projectInfo.ContractNO;
                ltlPaymentOrder.Text = paymentOrder.ToString();
                ltlPremise.Text = premise;
                ltlExpectDate.Text = DateUtils.GetDateString(expectDate);
                ltlAmountExpect.Text = amountExpect.ToString("c");
                ltlPaymentDate.Text = DateUtils.GetDateString(paymentDate);
                ltlAmountPaid.Text = amountPaid.ToString("c");
                ltlEditUrl.Text = string.Format(@"<a href onclick=""{0}"">修改</a>", Modal.PaymentAdd.GetShowPopWinStringToEdit(projectID, paymentID, "background_paymentCashBack.aspx?isPayment=" + this.isPayment));
            }
        }
    }
}
