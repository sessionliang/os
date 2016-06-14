using System;
using System.Text;
using System.ComponentModel;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Core.Security;
using System.Collections;

namespace SiteServer.CMS.Controls
{
    public class NodeTree : Control
    {
        private PublishmentSystemInfo publishmentSystemInfo;
        string rightPageURL = string.Empty;

        protected override void Render(HtmlTextWriter writer)
        {
            StringBuilder builder = new StringBuilder();

            int publishmentSystemID = int.Parse(base.Page.Request.QueryString["PublishmentSystemID"]);
            this.publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            string scripts = ChannelLoading.GetScript(this.publishmentSystemInfo, ELoadingType.ContentTree, null);
            builder.Append(scripts);
            if (base.Page.Request.QueryString["PublishmentSystemID"] != null)
            {
                this.rightPageURL = base.Page.Request.QueryString["RightPageURL"];
                try
                {
                    ArrayList nodeIDArrayList  = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(this.publishmentSystemInfo.PublishmentSystemID, 0);
                    foreach (int nodeID in nodeIDArrayList)
                    {
                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(this.publishmentSystemInfo.PublishmentSystemID, nodeID);
                        bool enabled = (AdminUtility.IsOwningNodeID(nodeInfo.NodeID)) ? true : false;
                        if (!enabled)
                        {
                            if (!AdminUtility.IsHasChildOwningNodeID(nodeInfo.NodeID)) continue;
                        }

                        builder.Append(ChannelLoading.GetChannelRowHtml(this.publishmentSystemInfo, nodeInfo, enabled, ELoadingType.ContentTree, null));
                    }
                }
                catch (Exception ex)
                {
                    PageUtils.RedirectToErrorPage(ex.Message);
                }
            }
            writer.Write(builder);
        }
    }
}