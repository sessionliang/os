using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;



namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundTrackerContentRank : BackgroundBasePage
    {
        public DataGrid dgContents;
        public LinkButton pageFirst;
        public LinkButton pageLast;
        public LinkButton pageNext;
        public LinkButton pagePrevious;
        public Label currentPage;

        private DictionaryEntryArrayList accessNumArrayList = new DictionaryEntryArrayList();
        private Hashtable todayAccessNumHashtable = new Hashtable();
        private int totalAccessNum = 0;

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

        public int GetTodayAccessNum(int pageContentID)
        {
            int todayAccessNum = 0;
            if (this.todayAccessNumHashtable[pageContentID] != null)
            {
                todayAccessNum = Convert.ToInt32(this.todayAccessNumHashtable[pageContentID]);
            }
            return todayAccessNum;
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.accessNumArrayList = DataProvider.TrackingDAO.GetContentAccessNumArrayList(base.PublishmentSystemID);
            this.todayAccessNumHashtable = DataProvider.TrackingDAO.GetTodayContentAccessNumHashtable(base.PublishmentSystemID);

            foreach (DictionaryEntry entry in this.accessNumArrayList)
            {
                int accessNum = (int)entry.Value;
                this.totalAccessNum += accessNum;
            }
            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Tracking, "内容流量排名", AppManager.CMS.Permission.WebSite.Tracking);

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
            ArrayList arraylist = new ArrayList();
            foreach (DictionaryEntry entry in this.accessNumArrayList)
            {
                int pageContentID = (int)entry.Key;
                int accessNum = (int)entry.Value;
                PageContentIDWithAccessNum pageContentIDWithAccessNum = new PageContentIDWithAccessNum(pageContentID, accessNum);
                arraylist.Add(pageContentIDWithAccessNum);
            }

            this.dgContents.DataSource = arraylist;
            this.dgContents.ItemDataBound += new DataGridItemEventHandler(MyDataGrid_ItemDataBound);
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

        void MyDataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;

                PageContentIDWithAccessNum pageContentIDWithAccessNum = e.Item.DataItem as PageContentIDWithAccessNum;

                if (pageContentIDWithAccessNum.PageContentID > 0)
                {
                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(ETableStyle.BackgroundContent, base.PublishmentSystemInfo.AuxiliaryTableForContent, pageContentIDWithAccessNum.PageContentID);
                    if (contentInfo != null && !string.IsNullOrEmpty(contentInfo.Title))
                    {
                        ltlTitle.Text = WebUtils.GetContentTitle(base.PublishmentSystemInfo, contentInfo, PageUtils.GetCMSUrl(string.Format("background_trackerContentRank.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
                    }
                    else
                    {
                        e.Item.Visible = false;
                    }
                }
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

        public class PageContentIDWithAccessNum
        {
            protected int m_intPageContentID;
            protected int m_intAccessNum;

            public PageContentIDWithAccessNum(int pageContentID, int accessNum)
            {
                this.m_intPageContentID = pageContentID;
                this.m_intAccessNum = accessNum;
            }

            public int PageContentID
            {
                get
                {
                    return m_intPageContentID;
                }
                set
                {
                    m_intPageContentID = value;
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
                    m_intAccessNum = value;
                }
            }
        }
    }
}
