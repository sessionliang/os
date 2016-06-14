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
    public class BackgroundAnalysisEvaluation : BackgroundBasePage
    {
        public DateTimeTextBox StartDate;
        public DateTimeTextBox EndDate;

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
            this.spContents.SelectCommand = DataProvider.EvaluationContentDAO.GetSelectCommendOfAnalysis(base.PublishmentSystemID, this.begin.ToString("yyyy-MM-dd"), this.end.ToString("yyyy-MM-dd"));
            this.spContents.SortField = "avgCompositeScore";
            this.spContents.SortMode = SortMode.DESC;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Evaluation, "评价统计分析", AppManager.CMS.Permission.WebSite.Evaluation);

                this.StartDate.Text = DateUtils.GetDateAndTimeString(this.begin);
                this.EndDate.Text = DateUtils.GetDateAndTimeString(this.end);

                this.additional = new NameValueCollection();
                additional["StartDate"] = this.StartDate.Text;
                additional["EndDate"] = this.EndDate.Text;

                this.Image.Attributes.Add("href", BackgroundAnalysisAdministratorImage.GetRedirectUrlString(base.PublishmentSystemID, this.PageUrl));

                JsManager.RegisterClientScriptBlock(Page, "NodeTreeScript", ChannelLoading.GetScript(base.PublishmentSystemInfo, ELoadingType.SiteAnalysis, this.additional));

                this.spContents.DataBind();
            }
        }

        private readonly Hashtable valueHashtable = new Hashtable();
        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int nodeID = TranslateUtils.EvalInt(e.Item.DataItem, "NodeID");
                int countnum = TranslateUtils.EvalInt(e.Item.DataItem, "countnum");
                double avgCompositeScore = TranslateUtils.ToDouble(TranslateUtils.EvalString(e.Item.DataItem, "avgCompositeScore"));

                Literal ltlNode = (Literal)e.Item.FindControl("ltlNode");
                Literal ltlContentCount = (Literal)e.Item.FindControl("ltlContentCount");
                Literal ltlContentEvaluation = (Literal)e.Item.FindControl("ltlContentEvaluation");

                string nodeName = valueHashtable[nodeID] as string;
                if (nodeName == null)
                {
                    nodeName = string.Format("<a href='{1}'>{0}</a>", NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, nodeID), PageUtils.GetCMSUrl(string.Format("background_evaluationAnalysisFiles.aspx?PublishmentSystemID={0}&NodeID={1}&StartDate={2}&EndDate={3}", base.PublishmentSystemID, nodeID, this.StartDate.Text, this.EndDate.Text)));
                    valueHashtable[nodeID] = nodeName;
                }
                ltlNode.Text = nodeName;
                ltlContentCount.Text = (countnum == 0) ? "0" : string.Format("<strong>{0}</strong>", countnum);
                ltlContentEvaluation.Text = (avgCompositeScore == 0) ? "0" : string.Format("<strong>{0}</strong>", avgCompositeScore);
            }
        }


        public void Analysis_OnClick(object sender, EventArgs E)
        {
            string pageUrl = PageUtils.GetCMSUrl(string.Format("background_analysisEvaluation.aspx?PublishmentSystemID={0}&StartDate={1}&EndDate={2}", base.PublishmentSystemID, this.StartDate.Text, this.EndDate.Text));
            PageUtils.Redirect(pageUrl);
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = PageUtils.GetCMSUrl(string.Format("background_analysisEvaluation.aspx?publishmentSystemID={0}", base.PublishmentSystemID));
                }
                return _pageUrl;
            }
        }
    }
}
