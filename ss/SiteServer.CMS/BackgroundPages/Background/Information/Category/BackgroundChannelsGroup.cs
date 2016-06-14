using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;
using System.Text;

using BaiRong.Controls;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundChannelsGroup : BackgroundBasePage
    {
        public Literal ltlChannelGroupName;

        public Repeater rptContents;
        public SqlPager spContents;

        public static string GetRedirectUrl(int publishmentSystemID, string nodeGroupName)
        {
            return PageUtils.GetCMSUrl(string.Format("background_channelsGroup.aspx?publishmentSystemID={0}&nodeGroupName={1}", publishmentSystemID, nodeGroupName));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            string nodeGroupName = base.GetQueryString("nodeGroupName");

            this.spContents.ControlToPaginate = this.rptContents;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProvider.NodeDAO.GetSelectCommendByNodeGroupName(base.PublishmentSystemID, nodeGroupName);
            this.spContents.SortField = "AddDate";
            this.spContents.SortMode = SortMode.DESC;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, AppManager.CMS.LeftMenu.Content.ID_Category, "查看栏目组", AppManager.CMS.Permission.WebSite.Category);

                this.ltlChannelGroupName.Text = "栏目组：" + nodeGroupName;
                this.spContents.DataBind();
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlItemChannelName = e.Item.FindControl("ltlItemChannelName") as Literal;
                Literal ltlItemChannelIndex = e.Item.FindControl("ltlItemChannelIndex") as Literal;
                Literal ltlItemAddDate = e.Item.FindControl("ltlItemAddDate") as Literal;

                int nodeID = TranslateUtils.EvalInt(e.Item.DataItem, "NodeID");
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
                ltlItemChannelName.Text = nodeInfo.NodeName;
                ltlItemChannelIndex.Text = nodeInfo.NodeIndexName;
                ltlItemAddDate.Text = DateUtils.GetDateString(nodeInfo.AddDate);
            }
        }

        public void Return_OnClick(object sender, EventArgs e)
        {
            PageUtils.Redirect(BackgroundNodeGroup.GetRedirectUrl(base.PublishmentSystemID));
        }
    }
}
