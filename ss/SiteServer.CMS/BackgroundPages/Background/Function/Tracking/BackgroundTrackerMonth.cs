using System;
using System.Collections;
using SiteServer.CMS.Core;
using System.Web.UI.WebControls;
using SiteServer.CMS.BackgroundPages.Modal;

using BaiRong.Model;

using BaiRong.Core;

namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundTrackerMonth : BackgroundBasePage
	{
        public Button ExportTracking;
	    readonly Hashtable accessNumHashtable = new Hashtable();
		int maxAccessNum = 0;
	    readonly Hashtable uniqueAccessNumHashtable = new Hashtable();
		int uniqueMaxAccessNum = 0;

        public int count = 12;
        public EStatictisXType xType = EStatictisXType.Month;

        public string GetGraphicX(int index)
        {
            int xNum = 0;
            DateTime datetime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
            if (EStatictisXTypeUtils.Equals(this.xType, EStatictisXType.Day))
            {
                datetime = datetime.AddDays(-(this.count - index));
                xNum = datetime.Day;
            }
            else if (EStatictisXTypeUtils.Equals(this.xType, EStatictisXType.Month))
            {
                datetime = datetime.AddMonths(-(this.count - index));
                xNum = datetime.Month;
            }
            else if (EStatictisXTypeUtils.Equals(this.xType, EStatictisXType.Year))
            {
                datetime = datetime.AddYears(-(this.count - index));
                xNum = datetime.Year;
            }
            else if (EStatictisXTypeUtils.Equals(this.xType, EStatictisXType.Hour))
            {
                datetime = datetime.AddHours(-(this.count - index));
                xNum = datetime.Hour;
            }
            return xNum.ToString();
        }

        public string GetGraphicY(int index)
        {
            if (index <= 0 || index > this.count) return string.Empty;
            int accessNum = (int)accessNumHashtable[index];
            return accessNum.ToString();
        }

        public string GetUniqueGraphicY(int index)
        {
            if (index <= 0 || index > this.count) return string.Empty;
            int accessNum = (int)uniqueAccessNumHashtable[index];
            return accessNum.ToString();
        }

		public double GetAccessNum(int index)
		{
			double accessNum = 0;
			if (this.maxAccessNum > 0)
			{
				accessNum = (Convert.ToDouble(this.maxAccessNum) * Convert.ToDouble(index)) / 8;
				accessNum = Math.Round(accessNum, 2);
			}
			return accessNum;
		}

		public string GetGraphicHtml(int index)
		{
			if (index <= 0 || index > 12) return string.Empty;
			int accessNum = (int)accessNumHashtable[index];
			DateTime datetime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
			datetime = datetime.AddMonths(-(12 - index));
			double height = 0;
			if (this.maxAccessNum >0)
			{
				height = (Convert.ToDouble(accessNum) / Convert.ToDouble(this.maxAccessNum)) * 200.0;
			}
            string html = string.Format("<IMG title=访问量：{0} height={1} style=height:{1}px src=../pic/tracker_bar.gif width=20><BR>{2}", accessNum, height, datetime.Month);
			return html;
		}

		public double GetUniqueAccessNum(int index)
		{
			double uniqueAccessNum = 0;
			if (this.uniqueMaxAccessNum > 0)
			{
				uniqueAccessNum = (Convert.ToDouble(this.uniqueMaxAccessNum) * Convert.ToDouble(index)) / 8;
				uniqueAccessNum = Math.Round(uniqueAccessNum, 2);
			}
			return uniqueAccessNum;
		}

		public string GetUniqueGraphicHtml(int index)
		{
			if (index <= 0 || index > 12) return string.Empty;
			int uniqueAccessNum = (int)uniqueAccessNumHashtable[index];
			DateTime datetime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
			datetime = datetime.AddMonths(-(12 - index));
			double height = 0;
			if (this.uniqueMaxAccessNum >0)
			{
				height = (Convert.ToDouble(uniqueAccessNum) / Convert.ToDouble(this.uniqueMaxAccessNum)) * 200.0;
			}
            string html = string.Format("<IMG title=访客数：{0} height={1} style=height:{1}px src=../pic/tracker_bar.gif width=20><BR>{2}", uniqueAccessNum, height, datetime.Month);
			return html;
		}

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Tracking, "月统计", AppManager.CMS.Permission.WebSite.Tracking);

                Hashtable trackingMonthHashtable = DataProvider.TrackingDAO.GetTrackingMonthHashtable(base.PublishmentSystemID);
                Hashtable uniqueTrackingMonthHashtable = DataProvider.TrackingDAO.GetUniqueTrackingMonthHashtable(base.PublishmentSystemID);
                DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                for (int i = 0; i < 12; i++)
                {
                    DateTime datetime = now.AddMonths(-i);
                    int accessNum = 0;
                    if (trackingMonthHashtable[datetime] != null)
                    {
                        accessNum = (int)trackingMonthHashtable[datetime];
                    }
                    this.accessNumHashtable.Add(12 - i, accessNum);
                    if (accessNum > this.maxAccessNum)
                    {
                        this.maxAccessNum = accessNum;
                    }

                    int uniqueAccessNum = 0;
                    if (uniqueTrackingMonthHashtable[datetime] != null)
                    {
                        uniqueAccessNum = (int)uniqueTrackingMonthHashtable[datetime];
                    }
                    this.uniqueAccessNumHashtable.Add(12 - i, uniqueAccessNum);
                    if (uniqueAccessNum > this.uniqueMaxAccessNum)
                    {
                        this.uniqueMaxAccessNum = uniqueAccessNum;
                    }
                }

                this.ExportTracking.Attributes.Add("onclick", PageUtility.ModalSTL.ExportMessage.GetOpenWindowStringToExport(base.PublishmentSystemID, PageUtility.ModalSTL.ExportMessage.EXPORT_TYPE_TrackerMonth));
            }
		}

	}
}
