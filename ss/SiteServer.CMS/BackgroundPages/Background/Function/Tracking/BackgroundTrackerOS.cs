using System;
using System.Collections;
using System.Web.UI.WebControls;
using SiteServer.CMS.Core;

using BaiRong.Model;

using BaiRong.Core;

namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundTrackerOS : BackgroundBasePage
	{
		public DataGrid dgContents;
		private Hashtable accessNumHashtable = new Hashtable();
		private Hashtable uniqueAccessNumHashtable = new Hashtable();
		private Hashtable todayAccessNumHashtable = new Hashtable();
		private Hashtable todayUniqueAccessNumHashtable = new Hashtable();
		private int totalAccessNum = 0;

		public int GetAccessNum(string os)
		{
			int accessNum = 0;
			if (this.accessNumHashtable[os] != null)
			{
				accessNum = Convert.ToInt32(this.accessNumHashtable[os]);
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

		public int GetUniqueAccessNum(string os)
		{
			int uniqueAccessNum = 0;
			if (this.uniqueAccessNumHashtable[os] != null)
			{
				uniqueAccessNum = Convert.ToInt32(this.uniqueAccessNumHashtable[os]);
			}
			return uniqueAccessNum;
		}

		public int GetTodayAccessNum(string os)
		{
			int todayAccessNum = 0;
			if (this.todayAccessNumHashtable[os] != null)
			{
				todayAccessNum = Convert.ToInt32(this.todayAccessNumHashtable[os]);
			}
			return todayAccessNum;
		}

		public int GetTodayUniqueAccessNum(string os)
		{
			int todayUniqueAccessNum = 0;
			if (this.todayUniqueAccessNumHashtable[os] != null)
			{
				todayUniqueAccessNum = Convert.ToInt32(this.todayUniqueAccessNumHashtable[os]);
			}
			return todayUniqueAccessNum;
		}

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Tracking, "操作系统统计", AppManager.CMS.Permission.WebSite.Tracking);

                this.accessNumHashtable = DataProvider.TrackingDAO.GetOSAccessNumHashtable(base.PublishmentSystemID);
                this.uniqueAccessNumHashtable = DataProvider.TrackingDAO.GetOSUniqueAccessNumHashtable(base.PublishmentSystemID);
                this.todayAccessNumHashtable = DataProvider.TrackingDAO.GetOSTodayAccessNumHashtable(base.PublishmentSystemID);
                this.todayUniqueAccessNumHashtable = DataProvider.TrackingDAO.GetOSTodayUniqueAccessNumHashtable(base.PublishmentSystemID);

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
				foreach (string OS in this.accessNumHashtable.Keys)
				{
					int accessNum = this.GetAccessNum(OS);
					OSWithAccessNum osWithAccessNum = new OSWithAccessNum(OS, accessNum);
					arraylist.Add(osWithAccessNum);
				}

                this.dgContents.DataSource = arraylist;
                this.dgContents.DataBind();
			}
			catch(Exception ex)
			{
                PageUtils.RedirectToErrorPage(ex.Message);
			}
		}

		public class OSWithAccessNum
		{
			protected string m_strOS;
			protected int m_intAccessNum;

			public OSWithAccessNum(string os, int accessNum)
			{
				this.m_strOS = os;
				this.m_intAccessNum = accessNum;
			}

			public string OS
			{
				get
				{
					return m_strOS;
				}
				set
				{
					m_strOS =  value;
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
