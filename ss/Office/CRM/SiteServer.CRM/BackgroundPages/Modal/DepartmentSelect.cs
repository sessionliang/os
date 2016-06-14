using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using System.Collections.Specialized;


using SiteServer.CRM.Model;
using SiteServer.CRM.Core;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.CRM.BackgroundPages.Modal
{
	public class DepartmentSelect : BackgroundBasePage
	{
        public Repeater rptCategory;

        private NameValueCollection additional = new NameValueCollection();

        public static string GetShowPopWinString(int projectID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("ProjectID", projectID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("���÷���", "modal_departmentSelect.aspx", arguments, 460, 260, true);
        }

		public void Page_Load(object sender, EventArgs E)
		{
            this.additional.Add("UrlFormatString", string.Format("modal_departmentSelect.aspx?ProjectID={0}", base.PublishmentSystemID) + "&DepartmentID={0}");

			if (!IsPostBack)
			{
                if (!string.IsNullOrEmpty(base.Request.QueryString["DepartmentID"]))
                {
                    int departmentID = TranslateUtils.ToInt(base.Request.QueryString["DepartmentID"]);
                    string departmentName = DepartmentManager.GetDepartmentName(departmentID);
                    string scripts = string.Format("window.parent.showCategoryDepartment('{0}', '{1}');", departmentName, departmentID);
                    JsUtils.OpenWindow.CloseModalPageWithoutRefresh(base.Page, scripts);
                }
                else
                {
                    ProjectManager.RegisterClientScriptBlock(Page, "NodeTreeScript", DepartmentTreeItem.GetScript(EDepartmentLoadingType.DepartmentSelect, this.additional));
                    this.BindGrid();
                }
			}
		}

        public void BindGrid()
        {
            try
            {
                this.rptCategory.DataSource = ProjectManager.GetFirstDepartmentIDArrayList();
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
