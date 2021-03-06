﻿using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;



using BaiRong.Model;
using BaiRong.Core.Data.Provider;

namespace SiteServer.WCM.BackgroundPages
{
    public class BackgroundGovPublicAnalysisDepartment : BackgroundGovPublicBasePage
	{
        public DateTimeTextBox StartDate;
        public DateTimeTextBox EndDate;
		public Repeater rptContents;

        private int totalCount = 0;
        private Hashtable countHashtable = new Hashtable();

		public void Page_Load(object sender, EventArgs E)
		{
            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if(!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_GovPublic, AppManager.CMS.LeftMenu.GovPublic.ID_GovPublicAnalysis, "主动公开统计", AppManager.CMS.Permission.WebSite.GovPublicAnalysis);

                this.StartDate.Text = string.Empty;
                this.EndDate.Now = true;

                JsManager.RegisterClientScriptBlock(Page, "TreeScript", DepartmentTreeItem.GetScript(EDepartmentLoadingType.List, null));
                BindGrid();
			}
		}

        public void BindGrid()
        {
            try
            {
                DateTime begin = DateUtils.SqlMinValue;
                if (!string.IsNullOrEmpty(this.StartDate.Text))
                {
                    begin = TranslateUtils.ToDateTime(this.StartDate.Text);
                }

                ArrayList departmentIDArrayList = BaiRongDataProvider.DepartmentDAO.GetDepartmentIDArrayListByFirstDepartmentIDArrayList(GovPublicManager.GetFirstDepartmentIDArrayList(base.PublishmentSystemInfo));

                foreach (int departmentID in departmentIDArrayList)
                {
                    int count = DataProvider.GovPublicContentDAO.GetCountByDepartmentID(base.PublishmentSystemInfo, departmentID, this.StartDate.DateTime, this.EndDate.DateTime);
                    this.totalCount += count;
                    this.countHashtable[departmentID] = count;
                }

                this.rptContents.DataSource = departmentIDArrayList;
                this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
                this.rptContents.DataBind();
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(ex.Message);
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int departmentID = (int)e.Item.DataItem;

            DepartmentInfo departmentInfo = DepartmentManager.GetDepartmentInfo(departmentID);

            Literal ltlTrHtml = e.Item.FindControl("ltlTrHtml") as Literal;
            Literal ltlTarget = e.Item.FindControl("ltlTarget") as Literal;
            Literal ltlCount = e.Item.FindControl("ltlCount") as Literal;
            Literal ltlBar = e.Item.FindControl("ltlBar") as Literal;

            ltlTrHtml.Text = string.Format(@"<tr treeItemLevel=""{0}"" style=""{1}"">", departmentInfo.ParentsCount + 1, Constants.SHOW_ELEMENT_STYLE);
            ltlTarget.Text = this.GetTitle(departmentInfo);

            int count = (int)this.countHashtable[departmentID];

            ltlCount.Text = count.ToString();

            ltlBar.Text = string.Format(@"<div class=""progress progress-success progress-striped"">
            <div class=""bar"" style=""width: {0}%""></div>
          </div>", this.GetBarWidth(count, this.totalCount));
        }

        private double GetBarWidth(int doCount, int totalCount)
        {
            double width = 0;
            if (totalCount > 0)
            {
                width = Convert.ToDouble(doCount) / Convert.ToDouble(totalCount);
                width = Math.Round(width, 2) * 100;
            }
            return width;
        }

		public void Analysis_OnClick(object sender, EventArgs E)
		{
			BindGrid();
		}

        private string GetTitle(DepartmentInfo departmentInfo)
        {
            DepartmentTreeItem treeItem = DepartmentTreeItem.CreateInstance(departmentInfo);
            return treeItem.GetItemHtml(EDepartmentLoadingType.List, null, true);
        }
	}
}
