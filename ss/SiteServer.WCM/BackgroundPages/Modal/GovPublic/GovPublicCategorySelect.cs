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
using SiteServer.CMS.BackgroundPages;
using SiteServer.WCM.Core;

namespace SiteServer.WCM.BackgroundPages.Modal
{
	public class GovPublicCategorySelect : BackgroundBasePage
	{
        public Literal ltlCategoryName;
        public Repeater rptCategory;

        private string classCode = string.Empty;

        public static string GetOpenWindowString(int publishmentSystemID, string classCode)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ClassCode", classCode);
            return PageUtilityWCM.GetOpenWindowString("设置分类", "modal_govPublicCategorySelect.aspx", arguments, 460, 360, true);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.classCode = base.Request.QueryString["ClassCode"];

			if (!IsPostBack)
			{
                GovPublicCategoryClassInfo categoryClassInfo = DataProvider.GovPublicCategoryClassDAO.GetCategoryClassInfo(this.classCode, base.PublishmentSystemID);
                this.ltlCategoryName.Text = categoryClassInfo.ClassName;

                if (!string.IsNullOrEmpty(base.Request.QueryString["CategoryID"]))
                {
                    int categoryID = TranslateUtils.ToInt(base.Request.QueryString["CategoryID"]);
                    string categoryName = DataProvider.GovPublicCategoryDAO.GetCategoryName(categoryID);
                    string scripts = string.Format("window.parent.showCategory{0}('{1}', '{2}');", this.classCode, categoryName, categoryID);
                    JsUtils.OpenWindow.CloseModalPageWithoutRefresh(base.Page, scripts);
                }
                else
                {
                    JsManager.RegisterClientScriptBlock(Page, "NodeTreeScript", GovPublicCategoryTreeItem.GetScript(this.classCode, base.PublishmentSystemID, EGovPublicCategoryLoadingType.Select, null));
                    this.BindGrid();
                }                

				
			}
		}

        public void BindGrid()
        {
            try
            {
                this.rptCategory.DataSource = DataProvider.GovPublicCategoryDAO.GetCategoryIDArrayListByParentID(this.classCode, base.PublishmentSystemID, 0);
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
            int categoryID = (int)e.Item.DataItem;
            bool enabled = true;
            GovPublicCategoryInfo categoryInfo = DataProvider.GovPublicCategoryDAO.GetCategoryInfo(categoryID);

            Literal ltlHtml = e.Item.FindControl("ltlHtml") as Literal;

            ltlHtml.Text = BackgroundGovPublicCategory.GetCategoryRowHtml(categoryInfo, enabled, EGovPublicCategoryLoadingType.Select, null);
        }
	}
}
