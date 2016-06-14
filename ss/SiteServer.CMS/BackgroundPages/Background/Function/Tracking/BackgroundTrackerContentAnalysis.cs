using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;




namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundTrackerContentAnalysis : BackgroundBasePage
	{
        private int nodeID;
        private string startDateString;
        private string endDateString;

        private Hashtable accessNumHashtable = new Hashtable();

        public DropDownList NodeIDDropDownList;
        public DateTimeTextBox StartDate;
        public DateTimeTextBox EndDate;
        public Repeater rptContents;
        public SqlPager spContents;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (!string.IsNullOrEmpty(base.GetQueryString("NodeID")))
            {
                this.nodeID = int.Parse(base.GetQueryString("NodeID"));
                this.startDateString = base.GetQueryString("StartDateString");
                this.endDateString = base.GetQueryString("EndDateString");
            }
            else
            {
                this.nodeID = base.PublishmentSystemID;
                this.startDateString = string.Empty;
                this.endDateString = DateTime.Now.ToString(DateUtils.FormatStringDateOnly);
            }

            string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, this.nodeID);

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(this.nodeID, EScopeType.Self, string.Empty, string.Empty);
            this.spContents.SelectCommand = BaiRongDataProvider.ContentDAO.GetSelectCommend(tableName, nodeIDArrayList, ETriState.True);
            this.spContents.SortField = BaiRongDataProvider.ContentDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

			if(!IsPostBack)
            {
                base.BreadCrumbWithItemTitle(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Tracking, "内容流量统计", NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, this.nodeID), AppManager.CMS.Permission.WebSite.Tracking);

                this.StartDate.Text = this.startDateString;
                this.EndDate.Text = this.endDateString;

                NodeManager.AddListItems(this.NodeIDDropDownList.Items, base.PublishmentSystemInfo, true, true);
                ControlUtils.SelectListItems(this.NodeIDDropDownList, this.nodeID.ToString());

                DateTime begin = DateUtils.SqlMinValue;
                if (!string.IsNullOrEmpty(this.startDateString))
                {
                    begin = TranslateUtils.ToDateTime(this.startDateString);
                }
                this.accessNumHashtable = DataProvider.TrackingDAO.GetContentAccessNumHashtable(base.PublishmentSystemID, this.nodeID, begin, TranslateUtils.ToDateTime(this.endDateString));

                this.spContents.DataBind();
			}
		}

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal itemTitle = (Literal)e.Item.FindControl("ItemTitle");
                Literal itemView = (Literal)e.Item.FindControl("ItemView");
                Literal itemCount = (Literal)e.Item.FindControl("ItemCount") ;

                BackgroundContentInfo contentInfo = new BackgroundContentInfo(e.Item.DataItem);

                itemTitle.Text = WebUtils.GetContentTitle(base.PublishmentSystemInfo, contentInfo, string.Empty);

                int num = 0;
                if (this.accessNumHashtable[contentInfo.ID] != null)
                {
                    num = Convert.ToInt32(this.accessNumHashtable[contentInfo.ID]);
                }
                itemCount.Text = num.ToString();

                itemView.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">详细</a>", Modal.TrackerIPView.GetOpenWindowString(this.startDateString, this.endDateString, base.PublishmentSystemID, this.nodeID, contentInfo.ID, num));
            }
        }

        //private string GetTitle(BackgroundContentInfo contentInfo)
        //{
        //    string url = string.Empty;

        //    string displayString = string.Empty;

        //    if (contentInfo.IsColor)
        //    {
        //        displayString = string.Format("<span style='color:#ff0000;text-decoration:none' title='醒目'>{0}</span>", contentInfo.Title);
        //    }
        //    else
        //    {
        //        displayString = contentInfo.Title;
        //    }

        //    url = string.Format("<a href='{0}' target='blank'>{1}</a>", PageUtility.GetContentUrl(base.PublishmentSystemInfo, 0, contentInfo, base.PublishmentSystemInfo.Additional.VisualType), displayString);

        //    string image = string.Empty;
        //    if (!string.IsNullOrEmpty(contentInfo.ImageUrl))
        //    {
        //        image += "&nbsp;<img src='../../sitefiles/bairong/icons/img.gif' alt='图片' align='absmiddle' border=0 />";
        //    }
        //    if (contentInfo.IsRecommend)
        //    {
        //        image += "&nbsp;<img src='../pic/icon/recommend.gif' alt='推荐' align='absmiddle' border=0 />";
        //    }
        //    if (contentInfo.IsHot)
        //    {
        //        image += "&nbsp;<img src='../pic/icon/hot.gif' alt='热点' align='absmiddle' border=0 />";
        //    }
        //    if (contentInfo.IsTop)
        //    {
        //        image += "&nbsp;<img src='../pic/icon/top.gif' alt='置顶' align='absmiddle' border=0 />";
        //    }
        //    if (!string.IsNullOrEmpty(contentInfo.FileUrl))
        //    {
        //        image += "&nbsp;<img src='../pic/icon/attachment.gif' alt='附件' align='absmiddle' border=0 />";
        //        if (base.PublishmentSystemInfo.Additional.IsCountDownload)
        //        {
        //            int count = CountManager.GetCount(AppManager.CMS.AppID, base.PublishmentSystemInfo.AuxiliaryTableForContent, contentInfo.ID.ToString(), ECountType.Download);
        //            image += string.Format("下载次数:<strong>{0}</strong>", count);
        //        }
        //    }
        //    return url + image;
        //}

		public void Analysis_OnClick(object sender, EventArgs E)
		{
            PageUtils.Redirect(PageUtils.GetCMSUrl(string.Format("background_trackerContentAnalysis.aspx?PublishmentSystemID={0}&NodeID={1}&StartDateString={2}&EndDateString={3}", base.PublishmentSystemID, this.nodeID, this.StartDate.Text, this.EndDate.Text)));
		}

        public void NodeIDDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageUtils.Redirect(PageUtils.GetCMSUrl(string.Format("background_trackerContentAnalysis.aspx?PublishmentSystemID={0}&NodeID={1}&StartDateString={2}&EndDateString={3}", base.PublishmentSystemID, this.NodeIDDropDownList.SelectedValue, this.startDateString, this.endDateString)));
        }
	}
}
