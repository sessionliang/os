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

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class ChannelSelect : BackgroundBasePage
	{
        public Literal ltlPublishmentSystem;
        public Repeater rptChannel;

        private NameValueCollection additional = new NameValueCollection();
        private bool isProtocol;
        private string jsMethod;
        private int itemIndex;

        public static string GetOpenWindowString(int publishmentSystemID)
        {
            return GetOpenWindowString(publishmentSystemID, false);
        }

        public static string GetOpenWindowString(int publishmentSystemID, bool isProtocol)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("isProtocol", isProtocol.ToString());
            return PageUtility.GetOpenWindowString("栏目选择", "modal_channelSelect.aspx", arguments, 460, 450, true);
        }

        public static string GetOpenWindowStringByItemIndex(int publishmentSystemID, string jsMethod, string itemIndex)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("jsMethod", jsMethod);
            arguments.Add("itemIndex", itemIndex);
            return PageUtility.GetOpenWindowString("栏目选择", "modal_channelSelect.aspx", arguments, 460, 450, true);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.isProtocol = TranslateUtils.ToBool(base.GetQueryString("isProtocol"));
            this.jsMethod = base.GetQueryString("jsMethod");
            this.itemIndex = TranslateUtils.ToInt(base.GetQueryString("itemIndex"));

            this.additional.Add("isProtocol", this.isProtocol.ToString());
            this.additional.Add("jsMethod", this.jsMethod);
            this.additional.Add("itemIndex", this.itemIndex.ToString());

			if (!IsPostBack)
			{
                if (!string.IsNullOrEmpty(base.GetQueryString("NodeID")))
                {
                    int nodeID = TranslateUtils.ToInt(base.GetQueryString("NodeID"));
                    string nodeNames = NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, nodeID);

                    if (!string.IsNullOrEmpty(this.jsMethod))
                    {
                        string scripts = string.Format("window.parent.{0}({1}, '{2}', {3});", this.jsMethod, this.itemIndex, nodeNames, nodeID);
                        JsUtils.OpenWindow.CloseModalPageWithoutRefresh(base.Page, scripts);
                    }
                    else
                    {
                        string pageUrl = PageUtility.GetChannelUrl(base.PublishmentSystemInfo, NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID), base.PublishmentSystemInfo.Additional.VisualType);
                        if (this.isProtocol)
                        {
                            pageUrl = PageUtils.AddProtocolToUrl(pageUrl);
                        }

                        string scripts = string.Format("window.parent.selectChannel('{0}', '{1}', '{2}');", nodeNames, nodeID, pageUrl);
                        JsUtils.OpenWindow.CloseModalPageWithoutRefresh(base.Page, scripts);
                    }
                }
                else
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, base.PublishmentSystemID);
                    string linkUrl = PageUtils.GetCMSUrl(string.Format("modal_channelSelect.aspx?PublishmentSystemID={0}&NodeID={1}&isProtocol={2}&jsMethod={3}&itemIndex={4}", base.PublishmentSystemID, nodeInfo.NodeID, this.isProtocol, this.jsMethod, this.itemIndex));
                    this.ltlPublishmentSystem.Text = string.Format("<a href='{0}'>{1}</a>", linkUrl, nodeInfo.NodeName);
                    JsManager.RegisterClientScriptBlock(Page, "NodeTreeScript", ChannelLoading.GetScript(base.PublishmentSystemInfo, ELoadingType.ChannelSelect, null));
                    this.BindGrid();
                }
			}
		}

        public void BindGrid()
        {
            try
            {
                this.rptChannel.DataSource = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(base.PublishmentSystemID, base.PublishmentSystemID);
                this.rptChannel.ItemDataBound += new RepeaterItemEventHandler(rptChannel_ItemDataBound);
                this.rptChannel.DataBind();
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(ex.Message);
            }
        }

        void rptChannel_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int nodeID = (int)e.Item.DataItem;
            bool enabled = (base.IsOwningNodeID(nodeID)) ? true : false;
            if (!enabled)
            {
                if (!base.IsHasChildOwningNodeID(nodeID)) e.Item.Visible = false;
            }
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);

            Literal ltlHtml = e.Item.FindControl("ltlHtml") as Literal;

            ltlHtml.Text = ChannelLoading.GetChannelRowHtml(base.PublishmentSystemInfo, nodeInfo, enabled, ELoadingType.ChannelSelect, this.additional);
        }
	}
}
