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
    public class BackgroundAnalysisContentDownloads : BackgroundBasePage
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
            return PageUtils.GetCMSUrl(PageUtils.AddQueryString("background_analysisContentDownloads.aspx", nvc));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.spContents.ControlToPaginate = this.rptContents;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;

            this.spContents.SelectCommand = DataProvider.BackgroundContentDAO.GetSelectCommendByDownloads(base.PublishmentSystemInfo.AuxiliaryTableForContent, base.PublishmentSystemID);

            this.spContents.SortField = BaiRongDataProvider.ContentDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;

            this.pageUrl = PageUtils.GetCMSUrl("background_analysisContentDownloads.aspx?publishmentSystemID=" + base.PublishmentSystemID);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, AppManager.CMS.LeftMenu.Content.ID_SiteAnalysis, "文件下载量排名", AppManager.CMS.Permission.WebSite.SiteAnalysis);
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
                Literal ltlFileUrl = e.Item.FindControl("ltlFileUrl") as Literal;

                BackgroundContentInfo contentInfo = new BackgroundContentInfo(e.Item.DataItem);

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

                ltlFileUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, contentInfo.FileUrl), contentInfo.FileUrl);
            }
        }
    }
}
