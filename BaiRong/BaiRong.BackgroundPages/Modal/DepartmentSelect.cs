using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using System.Collections.Specialized;



namespace BaiRong.BackgroundPages.Modal
{
	public class DepartmentSelect : BackgroundBasePage
	{
        public Repeater rptCategory;

        private NameValueCollection additional = new NameValueCollection();

        public static string GetShowPopWinString(int projectID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("ProjectID", projectID.ToString());
            return PageUtilityPF.GetOpenWindowString("…Ë÷√∑÷¿‡", "modal_departmentSelect.aspx", arguments, 460, 260, true);
        }

		public void Page_Load(object sender, EventArgs E)
		{
            this.additional.Add("UrlFormatString", PageUtils.GetPlatformUrl("modal_departmentSelect.aspx?DepartmentID={0}"));

			if (!IsPostBack)
			{
                if (!string.IsNullOrEmpty(base.GetQueryString("DepartmentID")))
                {
                    int departmentID = TranslateUtils.ToInt(base.GetQueryString("DepartmentID"));
                    string departmentName = DepartmentManager.GetDepartmentName(departmentID);
                    string scripts = string.Format("window.parent.showCategoryDepartment('{0}', '{1}');", departmentName, departmentID);
                    JsUtils.OpenWindow.CloseModalPageWithoutRefresh(base.Page, scripts);
                }
                else
                {
                    Page.RegisterClientScriptBlock("NodeTreeScript", DepartmentTreeItem.GetScript(EDepartmentLoadingType.DepartmentSelect, this.additional));
                    this.BindGrid();
                }
			}
		}

        public void BindGrid()
        {
            try
            {
                this.rptCategory.DataSource = BaiRongDataProvider.DepartmentDAO.GetDepartmentIDArrayListByParentID(0);
                this.rptCategory.ItemDataBound += new RepeaterItemEventHandler(rptCategory_ItemDataBound);
                this.rptCategory.DataBind();
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(ex.Message);
            }
        }

        private void rptCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int departmentID = (int)e.Item.DataItem;
            DepartmentInfo departmentInfo = DepartmentManager.GetDepartmentInfo(departmentID);

            Literal ltlHtml = e.Item.FindControl("ltlHtml") as Literal;

            ltlHtml.Text = BaiRong.BackgroundPages.BackgroundDepartment.GetDepartmentRowHtml(departmentInfo, EDepartmentLoadingType.DepartmentSelect, this.additional);
        }
	}
}
