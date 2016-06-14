using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

using BaiRong.Model;


namespace SiteServer.CMS.BackgroundPages
{

	public class BackgroundTracker : BackgroundBasePage
	{
		public Label StartDateTime;
		public Label TrackerPageView;
        public Label TrackerUniqueVisitor;
		public Label CurrentVisitorNum;
		public Label TrackingDayNum;
		public Label TotalAccessNum;
		public Label TotalUniqueAccessNum;
		public Label AverageDayAccessNum;
		public Label AverageDayUniqueAccessNum;
		public Label MaxAccessNumOfDay;
		public Label MaxAccessNumOfMonth;
		public Label MaxAccessDay;
		public Label MaxUniqueAccessNumOfDay;
		public Label MaxUniqueAccessNumOfMonth;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Tracking, "网站统计摘要", AppManager.CMS.Permission.WebSite.Tracking);

                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, base.PublishmentSystemID);
                TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks - nodeInfo.AddDate.Ticks);
                int trackerPageView = base.PublishmentSystemInfo.Additional.TrackerPageView;//原有访问量
                int trackerUniqueVisitor = base.PublishmentSystemInfo.Additional.TrackerUniqueVisitor;//原有访客数
                int trackingdayNumInt = (timeSpan.Days == 0) ? 1 : timeSpan.Days;//总统计天数
                trackingdayNumInt = trackingdayNumInt + base.PublishmentSystemInfo.Additional.TrackerDays;
                int currentVisitorNumInt = DataProvider.TrackingDAO.GetCurrentVisitorNum(base.PublishmentSystemID, base.PublishmentSystemInfo.Additional.TrackerCurrentMinute);//当前在线人数
                int totalAccessNumInt = DataProvider.TrackingDAO.GetTotalAccessNum(base.PublishmentSystemID, DateUtils.SqlMinValue);//总访问量
                int totalUniqueAccessNumInt = DataProvider.TrackingDAO.GetTotalUniqueAccessNum(base.PublishmentSystemID, DateUtils.SqlMinValue);//总唯一访客
                string maxAccessDay = string.Empty;//访问量最大的日期
                int maxAccessNumOfDayInt = DataProvider.TrackingDAO.GetMaxAccessNumOfDay(base.PublishmentSystemID, out maxAccessDay);//最大访问量（日）
                int maxAccessNumOfMonthInt = DataProvider.TrackingDAO.GetMaxAccessNumOfMonth(base.PublishmentSystemID);//最大访问量（月）
                int maxUniqueAccessNumOfDayInt = DataProvider.TrackingDAO.GetMaxUniqueAccessNumOfDay(base.PublishmentSystemID);//最大访问量（日）
                int maxUniqueAccessNumOfMonthInt = DataProvider.TrackingDAO.GetMaxUniqueAccessNumOfMonth(base.PublishmentSystemID);//最大访问量（月）


                this.StartDateTime.Text = DateUtils.GetDateAndTimeString(nodeInfo.AddDate);
                this.TrackerPageView.Text = trackerPageView.ToString();
                this.TrackerUniqueVisitor.Text = trackerUniqueVisitor.ToString();
                this.CurrentVisitorNum.Text = currentVisitorNumInt.ToString();

                this.TrackingDayNum.Text = trackingdayNumInt.ToString();
                this.TotalAccessNum.Text = totalAccessNumInt.ToString();
                this.TotalUniqueAccessNum.Text = totalUniqueAccessNumInt.ToString();
                this.AverageDayAccessNum.Text = System.Convert.ToString(Math.Round(System.Convert.ToDouble(totalAccessNumInt / trackingdayNumInt)));
                this.AverageDayUniqueAccessNum.Text = System.Convert.ToString(Math.Round(System.Convert.ToDouble(totalUniqueAccessNumInt / trackingdayNumInt)));
                this.MaxAccessDay.Text = maxAccessDay;
                this.MaxAccessNumOfDay.Text = maxAccessNumOfDayInt.ToString();
                this.MaxAccessNumOfMonth.Text = maxAccessNumOfMonthInt.ToString();
                this.MaxUniqueAccessNumOfDay.Text = maxUniqueAccessNumOfDayInt.ToString();
                this.MaxUniqueAccessNumOfMonth.Text = maxUniqueAccessNumOfMonthInt.ToString();
            }		
		}

	}
}
