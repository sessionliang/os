using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;

using BaiRong.Model;


namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundTrackerPageUrl : BackgroundBasePage
	{
		public DataGrid dgContents;
		public LinkButton pageFirst;
		public LinkButton pageLast;
		public LinkButton pageNext;
		public LinkButton pagePrevious;
		public Label currentPage;

        private DictionaryEntryArrayList accessNumArrayList = new DictionaryEntryArrayList();
		private Hashtable uniqueAccessNumHashtable = new Hashtable();
		private Hashtable todayAccessNumHashtable = new Hashtable();
		private Hashtable todayUniqueAccessNumHashtable = new Hashtable();
		private int totalAccessNum = 0;

        //public int GetAccessNum(string pageUrl)
        //{
        //    int accessNum = 0;
        //    if (this.accessNumHashtable[pageUrl] != null)
        //    {
        //        accessNum = Convert.ToInt32(this.accessNumHashtable[pageUrl]);
        //    }
        //    return accessNum;
        //}

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

		public int GetUniqueAccessNum(string pageUrl)
		{
			int uniqueAccessNum = 0;
			if (this.uniqueAccessNumHashtable[pageUrl] != null)
			{
				uniqueAccessNum = Convert.ToInt32(this.uniqueAccessNumHashtable[pageUrl]);
			}
			return uniqueAccessNum;
		}

		public int GetTodayAccessNum(string pageUrl)
		{
			int todayAccessNum = 0;
			if (this.todayAccessNumHashtable[pageUrl] != null)
			{
				todayAccessNum = Convert.ToInt32(this.todayAccessNumHashtable[pageUrl]);
			}
			return todayAccessNum;
		}

		public int GetTodayUniqueAccessNum(string pageUrl)
		{
			int todayUniqueAccessNum = 0;
			if (this.todayUniqueAccessNumHashtable[pageUrl] != null)
			{
				todayUniqueAccessNum = Convert.ToInt32(this.todayUniqueAccessNumHashtable[pageUrl]);
			}
			return todayUniqueAccessNum;
		}

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.accessNumArrayList = DataProvider.TrackingDAO.GetPageUrlAccessArrayList(base.PublishmentSystemID);
            this.uniqueAccessNumHashtable = DataProvider.TrackingDAO.GetPageUrlUniqueAccessNumHashtable(base.PublishmentSystemID);
            this.todayAccessNumHashtable = DataProvider.TrackingDAO.GetPageUrlTodayAccessNumHashtable(base.PublishmentSystemID);
            this.todayUniqueAccessNumHashtable = DataProvider.TrackingDAO.GetPageUrlTodayUniqueAccessNumHashtable(base.PublishmentSystemID);

            foreach (DictionaryEntry entry in this.accessNumArrayList)
            {
                int accessNum = (int)entry.Value;
                this.totalAccessNum += accessNum;
            }
            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Tracking, "受访页面统计", AppManager.CMS.Permission.WebSite.Tracking);

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
				foreach (DictionaryEntry entry in this.accessNumArrayList)
				{
                    string pageUrl = (string)entry.Key;
                    int accessNum = (int)entry.Value;
					//int accessNum = this.GetAccessNum(pageUrl);
					PageUrlWithAccessNum pageUrlWithAccessNum = new PageUrlWithAccessNum(pageUrl, accessNum);
					arraylist.Add(pageUrlWithAccessNum);
				}

                this.dgContents.DataSource = arraylist;
                this.dgContents.PageSize = PublishmentSystemManager.GetPublishmentSystemInfo(base.PublishmentSystemID).Additional.PageSize;
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

		public class PageUrlWithAccessNum
		{
			protected string m_strPageUrl;
			protected int m_intAccessNum;

			public PageUrlWithAccessNum(string pageUrl, int accessNum)
			{
				this.m_strPageUrl = pageUrl;
				this.m_intAccessNum = accessNum;
			}

			public string PageUrl
			{
				get
				{
					return m_strPageUrl;
				}
				set
				{
					m_strPageUrl =  value;
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
