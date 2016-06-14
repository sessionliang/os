using System;
using System.Collections;
using System.Web.UI.WebControls;
using SiteServer.CMS.Core;

using BaiRong.Model;

using BaiRong.Core;

namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundTrackerBrowser : BackgroundBasePage
	{
		public DataGrid dgContents;
		private Hashtable accessNumHashtable = new Hashtable();
		private Hashtable uniqueAccessNumHashtable = new Hashtable();
		private Hashtable todayAccessNumHashtable = new Hashtable();
		private Hashtable todayUniqueAccessNumHashtable = new Hashtable();
		private int totalAccessNum = 0;

		public int GetAccessNum(string browser)
		{
			int accessNum = 0;
			if (this.accessNumHashtable[browser] != null)
			{
				accessNum = Convert.ToInt32(this.accessNumHashtable[browser]);
			}
			return accessNum;
		}

		public double GetAccessNumBarWidth(int accessNum)
		{
			double width = 0;
            if (this.totalAccessNum > 0)
			{
                width = Convert.ToDouble(accessNum) / Convert.ToDouble(this.totalAccessNum);
				width = Math.Round(width, 2) * 100;
			}
			return width;
		}

		public int GetUniqueAccessNum(string browser)
		{
			int uniqueAccessNum = 0;
			if (this.uniqueAccessNumHashtable[browser] != null)
			{
				uniqueAccessNum = Convert.ToInt32(this.uniqueAccessNumHashtable[browser]);
			}
			return uniqueAccessNum;
		}

		public int GetTodayAccessNum(string browser)
		{
			int todayAccessNum = 0;
			if (this.todayAccessNumHashtable[browser] != null)
			{
				todayAccessNum = Convert.ToInt32(this.todayAccessNumHashtable[browser]);
			}
			return todayAccessNum;
		}

		public int GetTodayUniqueAccessNum(string browser)
		{
			int todayUniqueAccessNum = 0;
			if (this.todayUniqueAccessNumHashtable[browser] != null)
			{
				todayUniqueAccessNum = Convert.ToInt32(this.todayUniqueAccessNumHashtable[browser]);
			}
			return todayUniqueAccessNum;
		}

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Tracking, "浏览器统计", AppManager.CMS.Permission.WebSite.Tracking);

                this.accessNumHashtable = DataProvider.TrackingDAO.GetBrowserAccessNumHashtable(base.PublishmentSystemID);
                this.uniqueAccessNumHashtable = DataProvider.TrackingDAO.GetBrowserUniqueAccessNumHashtable(base.PublishmentSystemID);
                this.todayAccessNumHashtable = DataProvider.TrackingDAO.GetBrowserTodayAccessNumHashtable(base.PublishmentSystemID);
                this.todayUniqueAccessNumHashtable = DataProvider.TrackingDAO.GetBrowserTodayUniqueAccessNumHashtable(base.PublishmentSystemID);

                foreach (int accessNum in this.accessNumHashtable.Values)
                {
                    this.totalAccessNum += accessNum;
                }

                BindGrid();
            }
		}

		public void BindGrid()
		{
			try
			{
				ArrayList arraylist = new ArrayList();
				foreach (string browser in this.accessNumHashtable.Keys)
				{
					int accessNum = this.GetAccessNum(browser);
					BrowserWithAccessNum browserWithAccessNum = new BrowserWithAccessNum(browser, accessNum);
					arraylist.Add(browserWithAccessNum);
				}

                dgContents.DataSource = arraylist;
                dgContents.DataBind();
			}
			catch(Exception ex)
			{
                PageUtils.RedirectToErrorPage(ex.Message);
			}
		}

		public class BrowserWithAccessNum
		{
			protected string m_strBrowser;
			protected int m_intAccessNum;

			public BrowserWithAccessNum(string browser, int accessNum)
			{
				this.m_strBrowser = browser;
				this.m_intAccessNum = accessNum;
			}

			public string Browser
			{
				get
				{
					return m_strBrowser;
				}
				set
				{
					m_strBrowser =  value;
				}
			}

			public int AccessNum
			{
				get
				{
					return m_intAccessNum;
				}
				set
				{
					m_intAccessNum =  value;
				}
			}
		}
	}
}
