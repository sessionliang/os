using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;

using BaiRong.Model;

using BaiRong.Controls;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using System.Collections.Specialized;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundAnalysisContentHits : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;

        private string pageUrl;
        private readonly Hashtable nodeNameNavigations = new Hashtable();
        public string returnUrl;

        public static string GetRedirectUrl(int publishmentSystemID, string returnUrl)
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("returnUrl", StringUtils.ValueToUrl(returnUrl));
            nvc.Add("publishmentSystemID", publishmentSystemID.ToString());
            return PageUtils.GetCMSUrl(PageUtils.AddQueryString("background_analysisContentHits.aspx", nvc));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.spContents.ControlToPaginate = this.rptContents;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;

            ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(base.PublishmentSystemID);
            this.spContents.SelectCommand = BaiRongDataProvider.ContentDAO.GetSelectCommend(base.PublishmentSystemInfo.AuxiliaryTableForContent, nodeIDArrayList, ETriState.True);

            this.spContents.SortField = ContentAttribute.Hits;
            this.spContents.SortMode = SortMode.DESC;

            this.pageUrl = PageUtils.GetCMSUrl("background_analysisContentHits.aspx?publishmentSystemID=" + base.PublishmentSystemID);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, AppManager.CMS.LeftMenu.Content.ID_SiteAnalysis, "内容点击量排名", AppManager.CMS.Permission.WebSite.SiteAnalysis);
                this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("returnUrl"));
                this.spContents.DataBind();
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlItemTitle = e.Item.FindControl("ltlItemTitle") as Literal;
                Literal ltlChannel = e.Item.FindControl("ltlChannel") as Literal;
                Literal ltlHits = e.Item.FindControl("ltlHits") as Literal;
                Literal ltlHitsByDay = e.Item.FindControl("ltlHitsByDay") as Literal;
                Literal ltlHitsByWeek = e.Item.FindControl("ltlHitsByWeek") as Literal;
                Literal ltlHitsByMonth = e.Item.FindControl("ltlHitsByMonth") as Literal;
                Literal ltlLastHitsDate = e.Item.FindControl("ltlLastHitsDate") as Literal;

                ContentInfo contentInfo = new ContentInfo(e.Item.DataItem);

                ltlItemTitle.Text = WebUtils.GetContentTitle(base.PublishmentSystemInfo, contentInfo, this.pageUrl);

                string nodeNameNavigation = string.Empty;
                if (!nodeNameNavigations.ContainsKey(contentInfo.NodeID))
                {
                    nodeNameNavigation = NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, contentInfo.NodeID);
                    nodeNameNavigations.Add(contentInfo.NodeID, nodeNameNavigation);
                }
                else
                {
                    nodeNameNavigation = nodeNameNavigations[contentInfo.NodeID] as string;
                }
                ltlChannel.Text = nodeNameNavigation;

                ltlHits.Text = contentInfo.Hits.ToString();
                ltlHitsByDay.Text = contentInfo.HitsByDay.ToString();
                ltlHitsByMonth.Text = contentInfo.HitsByMonth.ToString();
                ltlHitsByWeek.Text = contentInfo.HitsByWeek.ToString();
                ltlLastHitsDate.Text = DateUtils.GetDateAndTimeString(contentInfo.LastHitsDate);
            }
        }
    }
}
