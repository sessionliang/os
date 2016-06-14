using System;
using System.Collections;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections.Specialized;

using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages
{
	public class BackgroundTemplateFilePathRule : BackgroundBasePage
	{
        public Repeater rptContents;

        private int currentNodeID;
        private NameValueCollection additional;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.additional = new NameValueCollection();

			if (!IsPostBack)
			{
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Configration, AppManager.CMS.LeftMenu.Configuration.ID_ConfigurationCreate, "页面命名规则", AppManager.CMS.Permission.WebSite.Configration);

                JsManager.RegisterClientScriptBlock(Page, "NodeTreeScript", ChannelLoading.GetScript(base.PublishmentSystemInfo, ELoadingType.TemplateFilePathRule, this.additional));

                if (!string.IsNullOrEmpty(Request.QueryString["CurrentNodeID"]))
                {
                    this.currentNodeID = base.GetIntQueryString("CurrentNodeID");
                    string onLoadScript = ChannelLoading.GetScriptOnLoad(base.PublishmentSystemID, this.currentNodeID);
                    if (!string.IsNullOrEmpty(onLoadScript))
                    {
                        JsManager.RegisterClientScriptBlock(Page, "NodeTreeScriptOnLoad", onLoadScript);
                    }
                }

                this.rptContents.DataSource = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(base.PublishmentSystemID, 0);
                this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
                this.rptContents.DataBind();
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

            ltlHtml.Text = ChannelLoading.GetChannelRowHtml(base.PublishmentSystemInfo, nodeInfo, enabled, ELoadingType.TemplateFilePathRule, this.additional);
        }
	}
}
