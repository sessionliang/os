using System;
using System.Collections;
using System.Web.UI.WebControls;
using SiteServer.CMS.Core;


using BaiRong.Core;

namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundTrackerReferrer : BackgroundBasePage
	{
		public DataGrid dgContents;
		public LinkButton pageFirst;
		public LinkButton pageLast;
		public LinkButton pageNext;
		public LinkButton pagePrevious;
		public Label currentPage;

		private Hashtable accessNumHashtable = new Hashtable();
		private Hashtable uniqueAccessNumHashtable = new Hashtable();
		private Hashtable todayAccessNumHashtable = new Hashtable();
		private Hashtable todayUniqueAccessNumHashtable = new Hashtable();
		private int totalAccessNum = 0;

		public string GetReferrerUrl(string referrer)
		{
			if (referrer != null && referrer.Trim().Length == 0)
			{
				return "直接输入网址";
			}
			else
			{
				return string.Format("<a HREF=\"{0}\" TARGET=\"_BLANK\">{0}</a>", referrer);
			}
		}

		public int GetAccessNum(string referrer)
		{
			int accessNum = 0;
			if (this.accessNumHashtable[referrer] != null)
			{
				accessNum = Convert.ToInt32(this.accessNumHashtable[referrer]);
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

		public int GetUniqueAccessNum(string referrer)
		{
			int uniqueAccessNum = 0;
			if (this.uniqueAccessNumHashtable[referrer] != null)
			{
				uniqueAccessNum = Convert.ToInt32(this.uniqueAccessNumHashtable[referrer]);
			}
			return uniqueAccessNum;
		}

		public int GetTodayAccessNum(string referrer)
		{
			int todayAccessNum = 0;
			if (this.todayAccessNumHashtable[referrer] != null)
			{
				todayAccessNum = Convert.ToInt32(this.todayAccessNumHashtable[referrer]);
			}
			return todayAccessNum;
		}

		public int GetTodayUniqueAccessNum(string referrer)
		{
			int todayUniqueAccessNum = 0;
			if (this.todayUniqueAccessNumHashtable[referrer] != null)
			{
				todayUniqueAccessNum = Convert.ToInt32(this.todayUniqueAccessNumHashtable[referrer]);
			}
			return todayUniqueAccessNum;
		}

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.accessNumHashtable = DataProvider.TrackingDAO.GetReferrerAccessNumHashtable(base.PublishmentSystemID);
            this.uniqueAccessNumHashtable = DataProvider.TrackingDAO.GetReferrerUniqueAccessNumHashtable(base.PublishmentSystemID);
            this.todayAccessNumHashtable = DataProvider.TrackingDAO.GetReferrerTodayAccessNumHashtable(base.PublishmentSystemID);
            this.todayUniqueAccessNumHashtable = DataProvider.TrackingDAO.GetReferrerTodayUniqueAccessNumHashtable(base.PublishmentSystemID);

            foreach (int accessNum in this.accessNumHashtable.Values)
            {
                this.totalAccessNum += accessNum;
            }

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Tracking, "来访网址统计", AppManager.CMS.Permission.WebSite.Tracking);

                BindGrid();
            }
		}

		public void MyDataGrid_Page(object sender, DataGridPageChangedEventArgs e)
		{
            this.dgContents.CurrentPageIndex = e.NewPageIndex;
			BindGrid();
		}

		public void BindGrid()
		{
			try
			{
				ArrayList arraylist = new ArrayList();
				foreach (string referrer in this.accessNumHashtable.Keys)
				{
					int accessNum = this.GetAccessNum(referrer);
					ReferrerWithAccessNum referrerWithAccessNum = new ReferrerWithAccessNum(referrer, accessNum);
					arraylist.Add(referrerWithAccessNum);
				}

                this.dgContents.DataSource = arraylist;
                this.dgContents.PageSize = base.PublishmentSystemInfo.Additional.PageSize;
                this.dgContents.DataBind();


                if (this.dgContents.CurrentPageIndex > 0)
				{
					pageFirst.Enabled = true;
					pagePrevious.Enabled = true;
				}
				else
				{
					pageFirst.Enabled = false;
					pagePrevious.Enabled = false;
				}

                if (this.dgContents.CurrentPageIndex + 1 == this.dgContents.PageCount)
				{
					pageLast.Enabled = false;
					pageNext.Enabled = false;
				}
				else
				{
					pageLast.Enabled = true;
					pageNext.Enabled = true;
				}

                currentPage.Text = string.Format("{0}/{1}", this.dgContents.CurrentPageIndex + 1, this.dgContents.PageCount);
			}
			catch (Exception ex)
			{
                PageUtils.RedirectToErrorPage(ex.Message);
			}

		}


		protected void NavigationButtonClick(object sender, System.EventArgs e)
		{
			LinkButton button = (LinkButton)sender;
			string direction = button.CommandName;

			switch (direction.ToUpper())
			{
				case "FIRST":
                    this.dgContents.CurrentPageIndex = 0;
					break;
				case "PREVIOUS":
                    this.dgContents.CurrentPageIndex =
                        Math.Max(this.dgContents.CurrentPageIndex - 1, 0);
					break;
				case "NEXT":
                    this.dgContents.CurrentPageIndex =
                        Math.Min(this.dgContents.CurrentPageIndex + 1,
                        this.dgContents.PageCount - 1);
					break;
				case "LAST":
                    this.dgContents.CurrentPageIndex = this.dgContents.PageCount - 1;
					break;
				default:
					break;
			}
			BindGrid();
		}

		public class ReferrerWithAccessNum
		{
			protected string m_strreferrer;
			protected int m_intAccessNum;

			public ReferrerWithAccessNum(string referrer, int accessNum)
			{
				this.m_strreferrer = referrer;
				this.m_intAccessNum = accessNum;
			}

			public string referrer
			{
				get
				{
					return m_strreferrer;
				}
				set
				{
					m_strreferrer =  value;
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
