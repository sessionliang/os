using System;
using System.Collections;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Model;


namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundRestrictionOptions : BackgroundBasePage
	{
        public RadioButtonList IsRestriction;
        public PlaceHolder PlaceHolder_Options;
        public Repeater rptContents;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Restriction, "访问限制设置", AppManager.CMS.Permission.WebSite.Restriction);

                EBooleanUtils.AddListItems(this.IsRestriction);
                ControlUtils.SelectListItemsIgnoreCase(this.IsRestriction, base.PublishmentSystemInfo.Additional.IsRestriction.ToString());
                this.PlaceHolder_Options.Visible = base.PublishmentSystemInfo.Additional.IsRestriction;
                JsManager.RegisterClientScriptBlock(Page, "NodeTreeScript", NavigationTreeItem.GetNodeTreeScript());

                this.rptContents.DataSource = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(base.PublishmentSystemID);
                this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
                this.rptContents.DataBind();
			}
		}

        public void IsRestriction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EBooleanUtils.Equals(this.IsRestriction.SelectedValue, EBoolean.True))
            {
                this.PlaceHolder_Options.Visible = true;
            }
            else
            {
                this.PlaceHolder_Options.Visible = false;
            }
            base.PublishmentSystemInfo.Additional.IsRestriction = this.PlaceHolder_Options.Visible;
            DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);

            StringUtility.AddLog(base.PublishmentSystemID, "设置页面访问限制功能", (base.PublishmentSystemInfo.Additional.IsRestriction ? "启用" : "禁用"));
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
            string showPopWinString = Modal.RestrictionOptions.GetOpenWindowString(base.PublishmentSystemID, nodeInfo.NodeID);

            NoTagText trHtml = (NoTagText)e.Item.FindControl("TrHtml");
            NoTagText editLink = (NoTagText)e.Item.FindControl("EditLink");
            NoTagText nodeTitle = (NoTagText)e.Item.FindControl("NodeTitle");
            NoTagText restrictionTypeOfChannel = (NoTagText)e.Item.FindControl("RestrictionTypeOfChannel");
            NoTagText restrictionTypeOfContent = (NoTagText)e.Item.FindControl("RestrictionTypeOfContent");

            trHtml.Text = string.Format(@"<tr treeItemLevel=""{0}"" style=""{1}"">", nodeInfo.ParentsCount + 1, this.GetNodeElementStyle(nodeInfo));
            editLink.Text = GetEditUrl(nodeInfo.NodeType, showPopWinString, enabled);
            nodeTitle.Text = this.GetTitle(nodeInfo, enabled, showPopWinString);

            ERestrictionType typeOfChannel = ERestrictionType.NoRestriction;
            ERestrictionType typeOfContnet = ERestrictionType.NoRestriction;
            if (nodeInfo.Additional.Attributes.Count > 0)
            {
                typeOfChannel = nodeInfo.Additional.RestrictionTypeOfChannel;
                typeOfContnet = nodeInfo.Additional.RestrictionTypeOfContent;
            }
            restrictionTypeOfChannel.Text = ERestrictionTypeUtils.GetShortText(typeOfChannel);
            restrictionTypeOfContent.Text = ERestrictionTypeUtils.GetShortText(typeOfContnet);
        }

        private string GetNodeElementStyle(NodeInfo nodeInfo)
        {
            if (base.PublishmentSystemID == nodeInfo.NodeID || nodeInfo.ParentsCount <= 2)
            {
                return Constants.SHOW_ELEMENT_STYLE;
            }
            else
            {
                return Constants.HIDE_ELEMENT_STYLE;
            }
        }

        private static string GetEditUrl(ENodeType nodeType, string showPopWinString, bool enabled)
        {
            if (enabled)
            {
                return string.Format("<a href=\"javascript:;\" onclick=\"{0}\">更改</a>", showPopWinString);
            }
            return string.Empty;
        }

        private string GetTitle(NodeInfo nodeInfo, bool enabled, string showPopWinString)
        {
            bool selected = (base.PublishmentSystemID == nodeInfo.NodeID || nodeInfo.ParentsCount == 1) ? true : false;
            bool hasChildren = (nodeInfo.ChildrenCount > 0) ? true : false;

            NodeNaviTreeItem nodeTreeItem = NodeNaviTreeItem.CreateNodeTreeItem(true, selected, nodeInfo.ParentsCount, hasChildren, nodeInfo.NodeName, string.Empty, showPopWinString, string.Empty, enabled, false, base.PublishmentSystemID, nodeInfo.NodeID, nodeInfo.NodeType, nodeInfo.ContentNum);
            return nodeTreeItem.GetItemHtml();
        }
	}
}
