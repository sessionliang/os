using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.Project.Core;
using System.Web.UI;
using SiteServer.Project.Model;

using BaiRong.Model;

namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundProject : BackgroundBasePage
    {
        public DataGrid dgContents;
        public Button AddButton;

        private bool isClosed;

        public static string GetRedirectUrl(bool isClosed)
        {
            return string.Format("background_project.aspx?isClosed={0}", isClosed);
        }
        public void Page_Load(object sender, EventArgs E)
        {
            this.isClosed = TranslateUtils.ToBool(base.Request.QueryString["isClosed"]);

            if (base.Request.QueryString["Delete"] != null && base.Request.QueryString["ProjectID"] != null)
            {
                int projectID = TranslateUtils.ToInt(base.Request.QueryString["ProjectID"]);
                try
                {
                    DataProvider.ProjectDAO.Delete(projectID);
                    base.SuccessMessage("成功删除项目");
                }
                catch (Exception ex)
                {
                    base.SuccessMessage(string.Format("删除项目失败，{0}", ex.Message));
                }
            }

            if (!IsPostBack)
            {
                this.dgContents.DataSource = DataProvider.ProjectDAO.GetDataSource(this.isClosed);
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();

                if (this.isClosed)
                {
                    this.AddButton.Visible = false;
                }
                else
                {
                    this.AddButton.Attributes.Add("onclick", Modal.ProjectAdd.GetShowPopWinStringToAdd(this.isClosed));
                }
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
                int amountTotal = TranslateUtils.EvalInt(e.Item.DataItem, "AmountTotal");
                string contractNO = TranslateUtils.EvalString(e.Item.DataItem, "ContractNO");
                string description = TranslateUtils.EvalString(e.Item.DataItem, "Description");
                string userNameAM = TranslateUtils.EvalString(e.Item.DataItem, "UserNameAM");
                string userNamePM = TranslateUtils.EvalString(e.Item.DataItem, "UserNamePM");
                string userNameCollection = TranslateUtils.EvalString(e.Item.DataItem, "UserNameCollection");

                Literal ltlProjectName = e.Item.FindControl("ltlProjectName") as Literal;
                Literal ltlProjectType = e.Item.FindControl("ltlProjectType") as Literal;
                Literal ltlAddDate =  e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlAmountTotal = e.Item.FindControl("ltlAmountTotal") as Literal;
                Literal ltlAmountRemain = e.Item.FindControl("ltlAmountRemain") as Literal;
                Literal ltlContractNO = e.Item.FindControl("ltlContractNO") as Literal;
                Literal ltlUserNameCollection = e.Item.FindControl("ltlUserNameCollection") as Literal;
                Literal ltlDescription = e.Item.FindControl("ltlDescription") as Literal;
                Literal ltlDocumentUrl = e.Item.FindControl("ltlDocumentUrl") as Literal;
                Literal ltlApplyUrl = e.Item.FindControl("ltlApplyUrl") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ltlProjectName.Text = string.Format(@"<a name=""{0}""></a>{1}<code>{2}</code>", projectID, projectName, isContract ? "合同" : "非合同");
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
                ltlAmountTotal.Text = string.Format(@"<a href=""{0}"">{1}</a>", BackgroundPayment.GetRedirectUrl(projectID, BackgroundProject.GetRedirectUrl(this.isClosed)), ltlAmountTotal.Text);
                
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

                count = DataProvider.ApplyDAO.GetCountByProjectID(projectID);
                if (count == 0)
                {
                    ltlApplyUrl.Text = string.Format(@"<a href=""background_applyToAll.aspx?ProjectID={0}"">项目办件</a>", projectID);
                }
                else
                {
                    ltlApplyUrl.Text = string.Format(@"<a href=""background_applyToAll.aspx?ProjectID={0}"">项目办件({1})</a>", projectID, count);
                }

                ltlEditUrl.Text = string.Format(@"<a href='javascript:undefined' onclick=""{0}"">编辑</a>", Modal.ProjectAdd.GetShowPopWinStringToEdit(this.isClosed, projectID));

                ltlDeleteUrl.Text = string.Format(@"<a href=""background_project.aspx?Delete=True&ProjectID={0}"" onClick=""javascript:return confirm('此操作将删除项目“{1}”及其办件，确认吗？');"">删除</a>", projectID, projectName);
            }
        }
    }
}
