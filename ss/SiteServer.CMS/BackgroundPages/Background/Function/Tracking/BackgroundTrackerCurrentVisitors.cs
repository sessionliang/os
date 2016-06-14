using System;
using System.Web.UI.WebControls;
using SiteServer.CMS.Core;


using BaiRong.Core;

namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundTrackerCurrentVisitors : BackgroundBasePage
	{
		public DataGrid dgContents;
		public LinkButton pageFirst;
		public LinkButton pageLast;
		public LinkButton pageNext;
		public LinkButton pagePrevious;
		public Label currentPage;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Tracking, "最近访问统计", AppManager.CMS.Permission.WebSite.Tracking);

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
                this.dgContents.DataSource = DataProvider.TrackingDAO.GetDataSource(base.PublishmentSystemID, base.PublishmentSystemInfo.Additional.TrackerCurrentMinute);
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
	}
}
