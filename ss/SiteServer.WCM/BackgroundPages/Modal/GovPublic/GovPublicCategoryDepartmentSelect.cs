using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using System.Collections.Specialized;


using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using SiteServer.WCM.Core;
using SiteServer.CMS.BackgroundPages;
using BaiRong.BackgroundPages;


namespace SiteServer.WCM.BackgroundPages.Modal
{
	public class GovPublicCategoryDepartmentSelect : SiteServer.CMS.BackgroundPages.BackgroundBasePage
	{
        public Repeater rptCategory;

        private NameValueCollection additional = new NameValueCollection();

        public static string GetOpenWindowString(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            return PageUtilityWCM.GetOpenWindowString("设置分类", "modal_govPublicCategoryDepartmentSelect.aspx", arguments, 460, 360, true);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.additional.Add("UrlFormatString", PageUtils.GetWCMUrl(string.Format("modal_govPublicCategoryDepartmentSelect.aspx?PublishmentSystemID={0}", base.PublishmentSystemID) + "&DepartmentID={0}"));

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
                    JsManager.RegisterClientScriptBlock(Page, "NodeTreeScript", DepartmentTreeItem.GetScript(EDepartmentLoadingType.DepartmentSelect, this.additional));
                    this.BindGrid();
                }
			}
		}

        public void BindGrid()
        {
            try
            {
                this.rptCategory.DataSource = GovPublicManager.GetFirstDepartmentIDArrayList(base.PublishmentSystemInfo);
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

            ltlHtml.Text = BackgroundDepartment.GetDepartmentRowHtml(departmentInfo, EDepartmentLoadingType.DepartmentSelect, this.additional);
        }
	}
}
