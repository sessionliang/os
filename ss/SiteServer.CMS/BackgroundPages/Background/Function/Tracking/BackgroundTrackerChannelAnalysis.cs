using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;




namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundTrackerChannelAnalysis : BackgroundBasePage
	{
        private Hashtable accessNumHashtableToChannel = new Hashtable();
        private Hashtable accessNumHashtableToContent = new Hashtable();
        private int totalAccessNum = 0;

        public DateTimeTextBox StartDate;
        public DateTimeTextBox EndDate;
		public Repeater rptContents;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if(!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Tracking, "栏目流量统计", AppManager.CMS.Permission.WebSite.Tracking);

                this.StartDate.Text = string.Empty;
                this.EndDate.Now = true;

                JsManager.RegisterClientScriptBlock(Page, "NodeTreeScript", NavigationTreeItem.GetNodeTreeScript());

                BindGrid();
			}
		}

        public void BindGrid()
        {
            try
            {
                DateTime begin = DateUtils.SqlMinValue;
                if (!string.IsNullOrEmpty(this.StartDate.Text))
                {
                    begin = TranslateUtils.ToDateTime(this.StartDate.Text);
                }
                this.accessNumHashtableToChannel = DataProvider.TrackingDAO.GetChannelAccessNumHashtable(base.PublishmentSystemID, begin, TranslateUtils.ToDateTime(this.EndDate.Text));
                this.accessNumHashtableToContent = DataProvider.TrackingDAO.GetChannelContentAccessNumHashtable(base.PublishmentSystemID, begin, TranslateUtils.ToDateTime(this.EndDate.Text));

                ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(base.PublishmentSystemID);
                foreach (int nodeID in nodeIDArrayList)
                {
                    int accessNum = 0;
                    if (this.accessNumHashtableToChannel[nodeID] != null)
                    {
                        accessNum = Convert.ToInt32(this.accessNumHashtableToChannel[nodeID]);
                    }
                    if (this.accessNumHashtableToContent[nodeID] != null)
                    {
                        accessNum += Convert.ToInt32(this.accessNumHashtableToContent[nodeID]);
                    }
                    this.totalAccessNum += accessNum;
                }

                this.rptContents.DataSource = nodeIDArrayList;
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

            Literal ltlTrHtml = (Literal)e.Item.FindControl("ltlTrHtml");
            Literal ltlNodeTitle = (Literal)e.Item.FindControl("ltlNodeTitle");
            Literal ltlAccessNumBar = (Literal)e.Item.FindControl("ltlAccessNumBar");
            Literal ltlItemView = (Literal)e.Item.FindControl("ltlItemView");
            Literal ltlChannelCount = (Literal)e.Item.FindControl("ltlChannelCount");
            Literal ltlContentCount = (Literal)e.Item.FindControl("ltlContentCount");
            Literal ltlTotalCount = (Literal)e.Item.FindControl("ltlTotalCount");

            ltlTrHtml.Text = string.Format(@"<tr treeItemLevel=""{0}"" style=""height:20px;line-height:20px;{1}"">", nodeInfo.ParentsCount + 1, Constants.SHOW_ELEMENT_STYLE);
            ltlNodeTitle.Text = this.GetTitle(nodeInfo, enabled);

            int accessNumToChannel = 0;
            if (this.accessNumHashtableToChannel[nodeID] != null)
            {
                accessNumToChannel = Convert.ToInt32(this.accessNumHashtableToChannel[nodeID]);
            }
            int accessNumToContent = 0;
            if (this.accessNumHashtableToContent[nodeID] != null)
            {
                accessNumToContent = Convert.ToInt32(this.accessNumHashtableToContent[nodeID]);
            }

            int accessNum = accessNumToChannel + accessNumToContent;

            ltlAccessNumBar.Text = string.Format(@"<div class=""progress progress-success progress-striped"">
            <div class=""bar"" style=""width: {0}%""></div>
          </div>", GetAccessNumBarWidth(accessNum));

            ltlItemView.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">详细</a>", Modal.TrackerIPView.GetOpenWindowString(this.StartDate.Text, this.EndDate.Text, base.PublishmentSystemID, nodeID, 0, accessNum));

            ltlChannelCount.Text = accessNumToChannel.ToString();
            ltlContentCount.Text = accessNumToContent.ToString();
            ltlTotalCount.Text = accessNum.ToString();
        }

        private double GetAccessNumBarWidth(int accessNum)
        {
            double width = 0;
            if (this.totalAccessNum > 0)
            {
                width = Convert.ToDouble(accessNum) / Convert.ToDouble(this.totalAccessNum);
                width = Math.Round(width, 2) * 200;
            }
            return width;
        }

		public void Analysis_OnClick(object sender, EventArgs E)
		{
			BindGrid();
		}

	    private string GetTitle(NodeInfo nodeInfo, bool enabled)
        {
            string showPopWinString = Modal.ChannelEdit.GetOpenWindowString(base.PublishmentSystemID, nodeInfo.NodeID, PageUtils.GetCMSUrl(string.Format("background_trackerChannelAnalysis.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));

            bool selected = true;
            bool hasChildren = (nodeInfo.ChildrenCount > 0) ? true : false;

            NodeNaviTreeItem nodeTreeItem = NodeNaviTreeItem.CreateNodeTreeItem(true, selected, nodeInfo.ParentsCount, hasChildren, nodeInfo.NodeName, string.Empty, showPopWinString, string.Empty, enabled, false, base.PublishmentSystemID, nodeInfo.NodeID, nodeInfo.NodeType, nodeInfo.ContentNum);
            return nodeTreeItem.GetItemHtml();
        }
	}
}
