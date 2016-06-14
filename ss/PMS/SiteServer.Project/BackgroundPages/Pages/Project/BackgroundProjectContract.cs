using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.Project.Core;
using System.Web.UI;
using SiteServer.Project.Model;
using System.Collections;
using BaiRong.Model;

namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundProjectContract : BackgroundBasePage
    {
        public DataGrid dgContents;

        public Literal ltlContractCount;
        public Literal ltlContractAmount;
        public Literal ltlAmountPaidPure;

        private string type;
        private int contractCount = 0;
        private int contractAmount = 0;
        private int amountPaidPure = 0;

        public void Page_Load(object sender, EventArgs E)
        {
            this.type = base.Request.QueryString["type"];

            if (!IsPostBack)
            {
                dgContents.DataSource = DataProvider.ProjectDAO.GetDataSource(this.type);
                dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                dgContents.DataBind();

                this.ltlContractCount.Text = this.contractCount.ToString();
                this.ltlContractAmount.Text = this.contractAmount.ToString("c");
                this.ltlAmountPaidPure.Text = this.amountPaidPure.ToString("c");
            }
        }

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int projectID = TranslateUtils.EvalInt(e.Item.DataItem, "ProjectID");
                string projectName = TranslateUtils.EvalString(e.Item.DataItem, "ProjectName");
                string projectType = TranslateUtils.EvalString(e.Item.DataItem, "ProjectType");
                DateTime addDate = TranslateUtils.EvalDateTime(e.Item.DataItem, "AddDate");
                bool isContract = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, "IsContract"));
                bool isClosed = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, "IsClosed"));
                int amountTotal = TranslateUtils.EvalInt(e.Item.DataItem, "AmountTotal");
                int amountCashBack = TranslateUtils.EvalInt(e.Item.DataItem, "AmountCashBack");
                string contractNO = TranslateUtils.EvalString(e.Item.DataItem, "ContractNO");
                string description = TranslateUtils.EvalString(e.Item.DataItem, "Description");
                string userNameAM = TranslateUtils.EvalString(e.Item.DataItem, "UserNameAM");
                string userNamePM = TranslateUtils.EvalString(e.Item.DataItem, "UserNamePM");
                string userNameCollection = TranslateUtils.EvalString(e.Item.DataItem, "UserNameCollection");

                int amount = amountTotal - amountCashBack;

                this.contractCount++;
                this.contractAmount += amount;
                this.amountPaidPure += DataProvider.PaymentDAO.GetAmountPaidPure(projectID);

                Literal ltlProjectName = e.Item.FindControl("ltlProjectName") as Literal;
                Literal ltlProjectType = e.Item.FindControl("ltlProjectType") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlAmountTotal = e.Item.FindControl("ltlAmountTotal") as Literal;
                Literal ltlAmountRemain = e.Item.FindControl("ltlAmountRemain") as Literal;
                Literal ltlContractNO = e.Item.FindControl("ltlContractNO") as Literal;
                Literal ltlUserNameCollection = e.Item.FindControl("ltlUserNameCollection") as Literal;
                Literal ltlDescription = e.Item.FindControl("ltlDescription") as Literal;
                Literal ltlDocumentUrl = e.Item.FindControl("ltlDocumentUrl") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlProjectName.Text = string.Format(@"<a name=""{0}""></a>{1}<code>{2}</code>", projectID, projectName, isClosed ? "已结束" : "进行中");
                ltlProjectType.Text = projectType;
                ltlAddDate.Text = DateUtils.GetDateString(addDate);

                string amountTotalError = string.Empty;
                string amountRemainError = string.Empty;
                int amountExpect = DataProvider.PaymentDAO.GetAmountExpect(projectID, false);
                int amountRemain = amountTotal - DataProvider.PaymentDAO.GetAmountPaid(projectID, false);
                PaymentManager.GetPaymentError(amountTotal, amountExpect, amountRemain, isClosed, out amountTotalError, out amountRemainError);

                ltlAmountTotal.Text = amountTotal == 0 ? string.Empty : amountTotal.ToString("c");
                int payCount = DataProvider.PaymentDAO.GetCount(projectID, ETriState.All);
                if (payCount > 0)
                {
                    ltlAmountTotal.Text += "<code>" + payCount + "笔</code>";
                }
                ltlAmountTotal.Text = string.Format(@"<a href=""{0}"">{1}</a>", BackgroundPayment.GetRedirectUrl(projectID, BackgroundProject.GetRedirectUrl(isClosed)), ltlAmountTotal.Text);

                ltlAmountRemain.Text = amountRemain.ToString("c");
                int notPayCount = DataProvider.PaymentDAO.GetCount(projectID, ETriState.False);
                if (notPayCount > 0)
                {
                    ltlAmountRemain.Text += "<code>" + notPayCount + "笔</code>";
                }

                if (!string.IsNullOrEmpty(amountTotalError))
                {
                    ltlAmountTotal.Text += "<br /><code>" + amountTotalError + "</code>";
                }
                if (!string.IsNullOrEmpty(amountRemainError))
                {
                    ltlAmountRemain.Text += "<br /><code>" + amountRemainError + "</code>";
                }

                ltlContractNO.Text = contractNO;

                ltlUserNameCollection.Text = ProjectManager.GetUserNameCollection(userNameAM, userNamePM, userNameCollection);

                ltlDescription.Text = description;

                int count = DataProvider.ProjectDocumentDAO.GetCount(projectID);
                if (count == 0)
                {
                    ltlDocumentUrl.Text = string.Format(@"<a href=""background_projectDocument.aspx?ProjectID={0}"">项目文档</a>", projectID);
                }
                else
                {
                    ltlDocumentUrl.Text = string.Format(@"<a href=""background_projectDocument.aspx?ProjectID={0}"">项目文档({1})</a>", projectID, count);
                }

                ltlEditUrl.Text = string.Format(@"<a href='javascript:undefined' onclick=""{0}"">编辑</a>", Modal.ProjectAdd.GetShowPopWinStringToEdit(isClosed, projectID));
            }
        }
    }
}
