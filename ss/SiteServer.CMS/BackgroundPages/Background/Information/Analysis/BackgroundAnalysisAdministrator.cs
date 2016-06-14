using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Controls;
using SiteServer.CMS.Core.Security;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;



using BaiRong.Model;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundAnalysisAdministrator : BackgroundBasePage
    {
        public DateTimeTextBox StartDate;
        public DateTimeTextBox EndDate;

        public Repeater rptChannels;

        public Repeater rptContents;
        public SqlPager spContents;

        private NameValueCollection additional;

        private DateTime begin;
        private DateTime end;
        public LinkButton Image;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (string.IsNullOrEmpty(base.GetQueryString("StartDate")))
            {
                this.begin = DateTime.Now.AddMonths(-1);
                this.end = DateTime.Now;
            }
            else
            {
                this.begin = TranslateUtils.ToDateTime(base.GetQueryString("StartDate"));
                this.end = TranslateUtils.ToDateTime(base.GetQueryString("EndDate"));
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = BaiRongDataProvider.ContentDAO.GetSelectCommendOfAdminExcludeRecycle(base.PublishmentSystemInfo.AuxiliaryTableForContent, base.PublishmentSystemID, this.begin, this.end);
            this.spContents.SortField = "addCount";
            this.spContents.SortMode = SortMode.DESC;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, AppManager.CMS.LeftMenu.Content.ID_SiteAnalysis, "管理员工作量统计", AppManager.CMS.Permission.WebSite.SiteAnalysis);

                this.StartDate.Text = DateUtils.GetDateAndTimeString(this.begin);
                this.EndDate.Text = DateUtils.GetDateAndTimeString(this.end);

                this.additional = new NameValueCollection();
                additional["StartDate"] = this.StartDate.Text;
                additional["EndDate"] = this.EndDate.Text;

                this.Image.Attributes.Add("href", BackgroundAnalysisAdministratorImage.GetRedirectUrlString(base.PublishmentSystemID, this.PageUrl));

                JsManager.RegisterClientScriptBlock(Page, "NodeTreeScript", ChannelLoading.GetScript(base.PublishmentSystemInfo, ELoadingType.SiteAnalysis, this.additional));

                BindGrid();
                this.spContents.DataBind();
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string userName = TranslateUtils.EvalString(e.Item.DataItem, "userName");
                int addCount = TranslateUtils.EvalInt(e.Item.DataItem, "addCount");
                int updateCount = TranslateUtils.EvalInt(e.Item.DataItem, "updateCount");
                int commentCount = TranslateUtils.EvalInt(e.Item.DataItem, "commentCount");

                Literal ltlUserName = (Literal)e.Item.FindControl("ltlUserName");
                Literal ltlDisplayName = (Literal)e.Item.FindControl("ltlDisplayName");
                Literal ltlContentAdd = (Literal)e.Item.FindControl("ltlContentAdd");
                Literal ltlContentUpdate = (Literal)e.Item.FindControl("ltlContentUpdate");
                Literal ltlContentComment = (Literal)e.Item.FindControl("ltlContentComment");

                ltlUserName.Text = userName;
                ltlDisplayName.Text = BaiRongDataProvider.AdministratorDAO.GetDisplayName(userName);

                ltlContentAdd.Text = (addCount == 0) ? "0" : string.Format("<strong>{0}</strong>", addCount);
                ltlContentUpdate.Text = (updateCount == 0) ? "0" : string.Format("<strong>{0}</strong>", updateCount);
                ltlContentComment.Text = (commentCount == 0) ? "0" : string.Format("<strong>{0}</strong>", commentCount);
            }
        }

        public void BindGrid()
        {
            this.rptChannels.DataSource = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(base.PublishmentSystemID, 0);
            this.rptChannels.ItemDataBound += new RepeaterItemEventHandler(rptChannels_ItemDataBound);
            this.rptChannels.DataBind();
        }

        void rptChannels_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int nodeID = (int)e.Item.DataItem;
                bool enabled = (base.IsOwningNodeID(nodeID)) ? true : false;
                if (!enabled)
                {
                    if (!base.IsHasChildOwningNodeID(nodeID)) e.Item.Visible = false;
                }
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);

                NoTagText element = (NoTagText)e.Item.FindControl("ElHtml");

                element.Text = ChannelLoading.GetChannelRowHtml(base.PublishmentSystemInfo, nodeInfo, enabled, ELoadingType.SiteAnalysis, this.additional);
            }
        }

        public void Analysis_OnClick(object sender, EventArgs E)
        {
            string pageUrl = PageUtils.GetCMSUrl(string.Format("background_analysisAdministrator.aspx?PublishmentSystemID={0}&StartDate={1}&EndDate={2}", base.PublishmentSystemID, this.StartDate.Text, this.EndDate.Text));
            PageUtils.Redirect(pageUrl);
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = PageUtils.GetCMSUrl(string.Format("background_AnalysisAdministrator.aspx?publishmentSystemID={0}", base.PublishmentSystemID));
                }
                return _pageUrl;
            }
        }
    }
}
