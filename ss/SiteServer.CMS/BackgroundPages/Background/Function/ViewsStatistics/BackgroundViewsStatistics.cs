using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Web.UI;
using System.Text;


namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundViewsStatistics : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;
        public TextBox UserName;
        public DateTimeTextBox DateFrom;
        public DateTimeTextBox DateTo;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;
            PageUtils.CheckRequestParameter("PublishmentSystemID");


            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;

            if (base.PublishmentSystemID == 0)
            {
                this.spContents.SelectCommand = DataProvider.ViewsStatisticsDAO.GetAllString(base.PublishmentSystemID, string.Empty);//string.Empty
            }
            else
            {
                this.spContents.SelectCommand = DataProvider.ViewsStatisticsDAO.GetAllString(base.PublishmentSystemID, base.GetQueryString("UserName"), base.GetQueryString("DateFrom"), base.GetQueryString("DateTo"));
            }

            this.spContents.SortField = "sumCount";
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                this.spContents.DataBind();

                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_IntelligentPush, "会员浏览量统计", AppManager.CMS.Permission.WebSite.IntelligentPush);
                 
                this.UserName.Text = base.GetQueryString("UserName");
                this.DateFrom.Text = base.GetQueryString("DateFrom");
                this.DateTo.Text = base.GetQueryString("DateTo");

            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int id = TranslateUtils.EvalInt(e.Item.DataItem, "ID");
                int userID = TranslateUtils.EvalInt(e.Item.DataItem, "UserID");
                int nodeID = TranslateUtils.EvalInt(e.Item.DataItem, "NodeID");
                int stasCount = TranslateUtils.EvalInt(e.Item.DataItem, "sumCount");
                //int stasYear = TranslateUtils.EvalInt(e.Item.DataItem, "StasYear");
               // int stasMonth = TranslateUtils.EvalInt(e.Item.DataItem, "StasMonth");

                UserInfo uinfo = BaiRongDataProvider.UserDAO.GetUserInfo(userID);

                NodeInfo ninfo = DataProvider.NodeDAO.GetNodeInfo(nodeID);

                Literal itemUser = (Literal)e.Item.FindControl("ItemUser");
                Literal itemNode = (Literal)e.Item.FindControl("ItemNode");
               // Literal itemYearMonth = (Literal)e.Item.FindControl("ItemYearMonth");
                Literal itemStasCount = (Literal)e.Item.FindControl("ItemStasCount");


                itemUser.Text = uinfo.UserName;
                itemNode.Text = ninfo.NodeName;
               // itemYearMonth.Text = stasYear.ToString() + "-" + stasMonth.ToString();
                itemStasCount.Text = stasCount.ToString();

            }
        }

        public void Search_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = PageUtils.GetCMSUrl(string.Format("background_viewsStatistics.aspx?PublishmentSystemID={0}&UserName={1}&DateFrom={2}&DateTo={3}", base.PublishmentSystemID, this.UserName.Text, this.DateFrom.Text, this.DateTo.Text));
                }
                return _pageUrl;
            }
        }

    }
}
