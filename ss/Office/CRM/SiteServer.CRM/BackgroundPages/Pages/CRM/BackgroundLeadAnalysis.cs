using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using SiteServer.CRM.Core;
using SiteServer.CRM.Model;



using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using System.Collections.Generic;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.CRM.BackgroundPages
{
	public class BackgroundLeadAnalysis : BackgroundBasePage
	{
        public DateTimeTextBox StartDate;
        public DateTimeTextBox EndDate;

		public Repeater rptContents;

		public void Page_Load(object sender, EventArgs E)
		{
			if(!IsPostBack)
            {
                this.StartDate.DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                this.EndDate.Now = true;

                ProjectManager.RegisterClientScriptBlock(Page, "TreeScript", DepartmentTreeItem.GetScript(EDepartmentLoadingType.List, null));

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

                ArrayList arraylist = new ArrayList();
                foreach (int departmentID in ConfigurationManager.Additional.CRMDepartmentIDCollection)
                {
                    ArrayList userNameArrayList = BaiRongDataProvider.AdministratorDAO.GetUserNameArrayList(departmentID, false);
                    if (userNameArrayList.Count > 0)
                    {
                        arraylist.Add("department:" + departmentID);
                        foreach (string userName in userNameArrayList)
                        {
                            arraylist.Add("userName:" + userName);
                        }
                    }
                }

                this.rptContents.DataSource = arraylist;
                this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
                this.rptContents.DataBind();
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(ex.Message);
            }
        }

        private void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string item = (string)e.Item.DataItem;
            string type = item.Split(':')[0];
            string value = item.Split(':')[1];

            Literal ltlTrHtml = e.Item.FindControl("ltlTrHtml") as Literal;
            Literal ltlTarget = e.Item.FindControl("ltlTarget") as Literal;
            Literal ltlLeadTotalCount = e.Item.FindControl("ltlLeadTotalCount") as Literal;
            Literal ltlRequestTotalCount = e.Item.FindControl("ltlRequestTotalCount") as Literal;
            Literal ltlInvalidCount = e.Item.FindControl("ltlInvalidCount") as Literal;
            Literal ltlSource = e.Item.FindControl("ltlSource") as Literal;
            Literal ltlFailureCount = e.Item.FindControl("ltlFailureCount") as Literal;
            Literal ltlSuccessCount = e.Item.FindControl("ltlSuccessCount") as Literal;
            Literal ltlPercentage = e.Item.FindControl("ltlPercentage") as Literal;
            Literal ltlBar = e.Item.FindControl("ltlBar") as Literal;

            int leadTotalCount = 0;
            int requestTotalCount = 0;
            int invalidCount = 0;
            Dictionary<string, int> sourceDictionary = null;
            int failureCount = 0;
            int successCount = 0;

            if (type == "department")
            {
                int departmentID = TranslateUtils.ToInt(value);
                DepartmentInfo departmentInfo = DepartmentManager.GetDepartmentInfo(departmentID);
                ltlTrHtml.Text = @"<tr treeItemLevel=""1"">";
                ltlTarget.Text = string.Format(@"<img align=""absmiddle"" src=""../../SiteFiles/bairong/icons/tree/folder.gif"">&nbsp;{0}", departmentInfo.DepartmentName);

                leadTotalCount = DataProvider.LeadDAO.GetCountByDepartmentID(departmentID, this.StartDate.DateTime, this.EndDate.DateTime);
                requestTotalCount = DataProvider.RequestDAO.GetCountByDepartmentID(departmentID, this.StartDate.DateTime, this.EndDate.DateTime);
                invalidCount = DataProvider.LeadDAO.GetCountByDepartmentIDAndStatus(departmentID, ELeadStatus.Invalid, this.StartDate.DateTime, this.EndDate.DateTime);
                sourceDictionary = DataProvider.LeadDAO.GetSourceDictionaryByDepartmentID(departmentID, this.StartDate.DateTime, this.EndDate.DateTime);
                failureCount = DataProvider.LeadDAO.GetCountByDepartmentIDAndStatus(departmentID, ELeadStatus.Failure, this.StartDate.DateTime, this.EndDate.DateTime);
                successCount = DataProvider.LeadDAO.GetCountByDepartmentIDAndStatus(departmentID, ELeadStatus.Success, this.StartDate.DateTime, this.EndDate.DateTime);
            }
            else
            {
                ltlTrHtml.Text = @"<tr treeItemLevel=""1"">";
                ltlTarget.Text = string.Format(@"<img align=""absmiddle"" src=""../../SiteFiles/bairong/icons/tree/empty.gif""><img align=""absmiddle"" src=""../../sitefiles/bairong/Icons/menu/administrator.gif"">&nbsp;{0}", AdminManager.GetDisplayName(value, false));

                leadTotalCount = DataProvider.LeadDAO.GetCountByChargeUserName(value, this.StartDate.DateTime, this.EndDate.DateTime);
                requestTotalCount = DataProvider.RequestDAO.GetCountByAddUserName(value, this.StartDate.DateTime, this.EndDate.DateTime);
                invalidCount = DataProvider.LeadDAO.GetCountByChargeUserNameAndStatus(value, ELeadStatus.Invalid, this.StartDate.DateTime, this.EndDate.DateTime);
                sourceDictionary = DataProvider.LeadDAO.GetSourceDictionaryByChargeUserName(value, this.StartDate.DateTime, this.EndDate.DateTime);
                failureCount = DataProvider.LeadDAO.GetCountByChargeUserNameAndStatus(value, ELeadStatus.Failure, this.StartDate.DateTime, this.EndDate.DateTime);
                successCount = DataProvider.LeadDAO.GetCountByChargeUserNameAndStatus(value, ELeadStatus.Success, this.StartDate.DateTime, this.EndDate.DateTime);
            }

            ltlLeadTotalCount.Text = leadTotalCount > 0 ? leadTotalCount.ToString() : string.Empty;
            ltlRequestTotalCount.Text = requestTotalCount > 0 ? requestTotalCount.ToString() : string.Empty;
            ltlInvalidCount.Text = invalidCount > 0 ? invalidCount.ToString() : string.Empty;
            foreach (var val in sourceDictionary)
            {
                ltlSource.Text += string.Format("{0}£º<code>{1}</code>", val.Key, val.Value);
            }
            ltlFailureCount.Text = failureCount > 0 ? failureCount.ToString() : string.Empty;
            ltlSuccessCount.Text = successCount > 0 ? successCount.ToString() : string.Empty;
            double percentage = this.GetBarWidth(successCount, leadTotalCount - invalidCount + requestTotalCount);
            if (percentage > 0)
            {
                ltlPercentage.Text = percentage + "%";
                ltlBar.Text = string.Format(@"<div class=""progress""><div class=""bar"" style=""width: {0};""></div></div>", ltlPercentage.Text);
            }
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
