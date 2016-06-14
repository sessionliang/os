using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;
using System.Collections;
using BaiRong.Controls;
using SiteServer.CMS.Model;


namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundConfigurationCreateTrigger : BackgroundBasePage
	{
        public Repeater rptContents;

        private int currentNodeID;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (!IsPostBack)
			{
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Configration, AppManager.CMS.LeftMenu.Configuration.ID_ConfigurationCreate, "页面生成触发器", AppManager.CMS.Permission.WebSite.Configration);

                JsManager.RegisterClientScriptBlock(Page, "NodeTreeScript", ChannelLoading.GetScript(base.PublishmentSystemInfo, ELoadingType.ConfigurationCreateDetails, null));

                if (!string.IsNullOrEmpty(base.GetQueryString("CurrentNodeID")))
                {
                    this.currentNodeID = TranslateUtils.ToInt(base.GetQueryString("CurrentNodeID"));
                    string onLoadScript = ChannelLoading.GetScriptOnLoad(base.PublishmentSystemID, this.currentNodeID);
                    if (!string.IsNullOrEmpty(onLoadScript))
                    {
                        JsManager.RegisterClientScriptBlock(Page, "NodeTreeScriptOnLoad", onLoadScript);
                    }
                }

                BindGrid();
			}
		}

        public void BindGrid()
        {
            try
            {
                this.rptContents.DataSource = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(base.PublishmentSystemID, 0);
                this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
                this.rptContents.DataBind();
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(ex.Message);
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int nodeID = (int)e.Item.DataItem;
            bool enabled = (base.IsOwningNodeID(nodeID)) ? true : false;
            if (!enabled)
            {
                if (!base.IsHasChildOwningNodeID(nodeID)) e.Item.Visible = false;
            }

            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            Literal ltlHtml = e.Item.FindControl("ltlHtml") as Literal;
            ltlHtml.Text = ChannelLoading.GetChannelRowHtml(base.PublishmentSystemInfo, nodeInfo, enabled, ELoadingType.ConfigurationCreateDetails, null);
        }
	}
}
