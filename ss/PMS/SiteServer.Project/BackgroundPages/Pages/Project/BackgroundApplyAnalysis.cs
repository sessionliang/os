using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using SiteServer.Project.Core;
using SiteServer.Project.Model;



using BaiRong.Model;
using BaiRong.Core.Data.Provider;

namespace SiteServer.Project.BackgroundPages
{
	public class BackgroundApplyAnalysis : BackgroundBasePage
	{
        public DateTimeTextBox StartDate;
        public DateTimeTextBox EndDate;
        public DropDownList ddlInteractName;
		public Repeater rptContents;
        private int projectID = 0;

		public void Page_Load(object sender, EventArgs E)
		{
			if(!IsPostBack)
            {
                this.StartDate.Text = string.Empty;
                this.EndDate.Now = true;

                ArrayList interactInfoArrayList = DataProvider.ProjectDAO.GetProjectInfoArrayList(false);

                ListItem listItem = new ListItem("<<È«²¿>>", string.Empty);
                this.ddlInteractName.Items.Add(listItem);
                foreach (ProjectInfo projectInfo in interactInfoArrayList)
                {
                    listItem = new ListItem(projectInfo.ProjectName, projectInfo.ProjectID.ToString());
                    this.ddlInteractName.Items.Add(listItem);
                }

                ProjectManager.RegisterClientScriptBlock(Page, "TreeScript", DepartmentTreeItem.GetScript(EDepartmentLoadingType.List, null));
                BindGrid();
			}
		}

        public void BindGrid()
        {
            try
            {
                this.projectID = TranslateUtils.ToInt(this.ddlInteractName.SelectedValue);

                DateTime begin = DateUtils.SqlMinValue;
                if (!string.IsNullOrEmpty(this.StartDate.Text))
                {
                    begin = TranslateUtils.ToDateTime(this.StartDate.Text);
                }

                ArrayList departmentIDArrayList = BaiRongDataProvider.DepartmentDAO.GetDepartmentIDArrayListByFirstDepartmentIDArrayList(ProjectManager.GetFirstDepartmentIDArrayList());
                if (departmentIDArrayList.Count == 0)
                {
                    departmentIDArrayList = DepartmentManager.GetDepartmentIDArrayList();
                }

                this.rptContents.DataSource = departmentIDArrayList;
                this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
                this.rptContents.DataBind();
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(ex.Message);
            }
        }

        private void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int departmentID = (int)e.Item.DataItem;

            DepartmentInfo departmentInfo = DepartmentManager.GetDepartmentInfo(departmentID);

            Literal ltlTrHtml = e.Item.FindControl("ltlTrHtml") as Literal;
            Literal ltlTarget = e.Item.FindControl("ltlTarget") as Literal;
            Literal ltlTotalCount = e.Item.FindControl("ltlTotalCount") as Literal;
            Literal ltlDoCount = e.Item.FindControl("ltlDoCount") as Literal;
            Literal ltlUndoCount = e.Item.FindControl("ltlUndoCount") as Literal;
            Literal ltlPercentage = e.Item.FindControl("ltlPercentage") as Literal;
            Literal ltlBar = e.Item.FindControl("ltlBar") as Literal;

            ltlTrHtml.Text = string.Format(@"<tr treeItemLevel=""{0}"" style=""{1}"">", departmentInfo.ParentsCount + 1, "display:");
            ltlTarget.Text = this.GetTitle(departmentInfo);

            int totalCount = DataProvider.ApplyDAO.GetCountByDepartmentID(departmentID, this.projectID, this.StartDate.DateTime, this.EndDate.DateTime);
            int doCount = DataProvider.ApplyDAO.GetCountByDepartmentIDAndState(departmentID, this.projectID, EApplyState.Checked, this.StartDate.DateTime, this.EndDate.DateTime);
            int unDoCount = totalCount - doCount;

            ltlTotalCount.Text = totalCount.ToString();
            ltlDoCount.Text = doCount.ToString();
            ltlUndoCount.Text = unDoCount.ToString();
            double percentage = this.GetBarWidth(doCount, totalCount);
            ltlPercentage.Text = percentage + "%";
            ltlBar.Text = string.Format(@"<div class=""bar"" style=""width: {0};""></div>", ltlPercentage.Text);
        }

        private double GetBarWidth(int doCount, int totalCount)
        {
            double width = 0;
            if (totalCount > 0)
            {
                width = Convert.ToDouble(doCount) / Convert.ToDouble(totalCount);
                width = Math.Round(width, 2) * 100;
            }
            return width;
        }

		public void Analysis_OnClick(object sender, EventArgs E)
		{
			BindGrid();
		}

        private string GetTitle(DepartmentInfo departmentInfo)
        {
            DepartmentTreeItem treeItem = DepartmentTreeItem.CreateInstance(departmentInfo);
            return treeItem.GetItemHtml(EDepartmentLoadingType.List, null, true);
        }
	}
}
