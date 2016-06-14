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
	public class GovInteractDepartmentSelect : SiteServer.CMS.BackgroundPages.BackgroundBasePage
	{
        public Repeater rptCategory;

        private int nodeID;
        private NameValueCollection additional = new NameValueCollection();

        protected override bool IsSinglePage
        {
            get
            {
                return true;
            }
        }

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            return PageUtilityWCM.GetOpenWindowString("选择部门", "modal_govInteractDepartmentSelect.aspx", arguments, 460, 360, true);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.nodeID = TranslateUtils.ToInt(base.Request.QueryString["NodeID"]);
            this.additional.Add("UrlFormatString", PageUtils.GetWCMUrl(string.Format("modal_govInteractDepartmentSelect.aspx?PublishmentSystemID={0}", base.PublishmentSystemID) + "&DepartmentID={0}"));

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
                GovInteractChannelInfo channelInfo = DataProvider.GovInteractChannelDAO.GetChannelInfo(base.PublishmentSystemID, this.nodeID);
                this.rptCategory.DataSource = GovInteractManager.GetFirstDepartmentIDArrayList(channelInfo);
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
