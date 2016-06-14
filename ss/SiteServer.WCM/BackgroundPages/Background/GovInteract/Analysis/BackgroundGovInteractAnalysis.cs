using System;
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
    public class BackgroundGovInteractAnalysis : BackgroundGovInteractBasePage
	{
        public DateTimeTextBox StartDate;
        public DateTimeTextBox EndDate;
        public DropDownList ddlNodeID;
		public Repeater rptContents;
        private int nodeID = 0;

		public void Page_Load(object sender, EventArgs E)
		{
            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if(!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_GovInteract, AppManager.CMS.LeftMenu.GovInteract.ID_GovInteractAnalysis, "互动交流统计", AppManager.CMS.Permission.WebSite.GovInteractAnalysis);

                this.StartDate.Text = string.Empty;
                this.EndDate.Now = true;

                ArrayList nodeInfoArrayList = GovInteractManager.GetNodeInfoArrayList(base.PublishmentSystemInfo);

                ListItem listItem = new ListItem("<<全部>>", "0");
                this.ddlNodeID.Items.Add(listItem);
                foreach (NodeInfo nodeInfo in nodeInfoArrayList)
                {
                    listItem = new ListItem(nodeInfo.NodeName, nodeInfo.NodeID.ToString());
                    this.ddlNodeID.Items.Add(listItem);
                }

                JsManager.RegisterClientScriptBlock(Page, "TreeScript", DepartmentTreeItem.GetScript(EDepartmentLoadingType.List, null));
                BindGrid();
			}
		}

        public void BindGrid()
        {
            try
            {
                this.nodeID = TranslateUtils.ToInt(this.ddlNodeID.SelectedValue);

                DateTime begin = DateUtils.SqlMinValue;
                if (!string.IsNullOrEmpty(this.StartDate.Text))
                {
                    begin = TranslateUtils.ToDateTime(this.StartDate.Text);
                }

                ArrayList departmentIDArrayList = new ArrayList();

                if (this.nodeID > 0)
                {
                    GovInteractChannelInfo channelInfo = DataProvider.GovInteractChannelDAO.GetChannelInfo(base.PublishmentSystemID, this.nodeID);

                    departmentIDArrayList = BaiRongDataProvider.DepartmentDAO.GetDepartmentIDArrayListByFirstDepartmentIDArrayList(GovInteractManager.GetFirstDepartmentIDArrayList(channelInfo));
                }

                if (departmentIDArrayList.Count == 0)
                {
                    departmentIDArrayList = DepartmentManager.GetDepartmentIDArrayList();
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
            Literal ltlTotalCount = e.Item.FindControl("ltlTotalCount") as Literal;
            Literal ltlDoCount = e.Item.FindControl("ltlDoCount") as Literal;
            Literal ltlUndoCount = e.Item.FindControl("ltlUndoCount") as Literal;
            Literal ltlBar = e.Item.FindControl("ltlBar") as Literal;

            ltlTrHtml.Text = string.Format(@"<tr treeItemLevel=""{0}"" style=""{1}"">", departmentInfo.ParentsCount + 1, Constants.SHOW_ELEMENT_STYLE);
            ltlTarget.Text = this.GetTitle(departmentInfo);

            int totalCount = 0;
            int doCount = 0;
            if (this.nodeID == 0)
            {
                totalCount = DataProvider.GovInteractContentDAO.GetCountByDepartmentID(base.PublishmentSystemInfo, departmentID, this.StartDate.DateTime, this.EndDate.DateTime);
                doCount = DataProvider.GovInteractContentDAO.GetCountByDepartmentIDAndState(base.PublishmentSystemInfo, departmentID, EGovInteractState.Checked, this.StartDate.DateTime, this.EndDate.DateTime);
            }
            else
            {
                totalCount = DataProvider.GovInteractContentDAO.GetCountByDepartmentID(base.PublishmentSystemInfo, departmentID, this.nodeID, this.StartDate.DateTime, this.EndDate.DateTime);
                doCount = DataProvider.GovInteractContentDAO.GetCountByDepartmentIDAndState(base.PublishmentSystemInfo, departmentID, this.nodeID, EGovInteractState.Checked, this.StartDate.DateTime, this.EndDate.DateTime);
            
            }
            int unDoCount = totalCount - doCount; 

            ltlTotalCount.Text = totalCount.ToString();
            ltlDoCount.Text = doCount.ToString();
            ltlUndoCount.Text = unDoCount.ToString();

            ltlBar.Text = string.Format(@"<div class=""progress progress-success progress-striped"">
            <div class=""bar"" style=""width: {0}%""></div>
          </div>", this.GetBarWidth(doCount, totalCount));
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
