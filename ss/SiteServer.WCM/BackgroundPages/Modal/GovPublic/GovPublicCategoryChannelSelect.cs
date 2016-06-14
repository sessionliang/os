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
	public class GovPublicCategoryChannelSelect : BackgroundBasePage
	{
        public Repeater rptCategory;

        public static string GetOpenWindowString(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            return PageUtilityWCM.GetOpenWindowString("设置分类", "modal_govPublicCategoryChannelSelect.aspx", arguments, 460, 360, true);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
			{
                if (!string.IsNullOrEmpty(base.Request.QueryString["NodeID"]))
                {
                    int nodeID = TranslateUtils.ToInt(base.Request.QueryString["NodeID"]);
                    string nodeNames = NodeManager.GetNodeNameNavigationByGovPublic(base.PublishmentSystemID, nodeID);
                    string scripts = string.Format("window.parent.showCategoryChannel('{0}', '{1}');", nodeNames, nodeID);
                    JsUtils.OpenWindow.CloseModalPageWithoutRefresh(base.Page, scripts);
                }
                else
                {
                    JsManager.RegisterClientScriptBlock(Page, "NodeTreeScript", ChannelLoading.GetScript(base.PublishmentSystemInfo, ELoadingType.GovPublicChannelAdd, null));
                    this.BindGrid();
                }
			}
		}

        public void BindGrid()
        {
            try
            {
                this.rptCategory.DataSource = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(base.PublishmentSystemID, base.PublishmentSystemInfo.Additional.GovPublicNodeID);
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
            int nodeID = (int)e.Item.DataItem;
            bool enabled = (base.IsOwningNodeID(nodeID)) ? true : false;
            if (!enabled)
            {
                if (!base.IsHasChildOwningNodeID(nodeID)) e.Item.Visible = false;
            }
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);

            Literal ltlHtml = e.Item.FindControl("ltlHtml") as Literal;

            ltlHtml.Text = ChannelLoading.GetChannelRowHtml(base.PublishmentSystemInfo, nodeInfo, enabled, ELoadingType.GovPublicChannelAdd, null);
        }
	}
}
