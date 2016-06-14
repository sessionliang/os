using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Controls;
using SiteServer.CMS.Core.Security;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;



using BaiRong.Model;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundAnalysisByDept : BackgroundBasePage
    {
        public DateTimeTextBox StartDate;
        public DateTimeTextBox EndDate;
        public DropDownList ddlDepartment;
        public TextBox tbUserName;

        public Repeater rptChannels;

        public Repeater rptContents;
        public SqlPager spContents;

        private NameValueCollection additional;

        private DateTime begin;
        private DateTime end;
        private int deptID;
        private string userName;
        public LinkButton Image;
        private bool[] isLastNodeArrayOfDepartment;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (string.IsNullOrEmpty(base.GetQueryString("StartDate")))
            {
                this.begin = DateTime.Now.AddMonths(-1);
                this.end = DateTime.Now;
                this.userName = string.Empty;
                this.deptID = 0;
            }
            else
            {
                this.begin = TranslateUtils.ToDateTime(base.GetQueryString("StartDate"));
                this.end = TranslateUtils.ToDateTime(base.GetQueryString("EndDate"));
                this.userName = base.GetQueryString("UserName");
                this.deptID = base.GetIntQueryString("DeptID");
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;

            this.spContents.SelectCommand = BaiRongDataProvider.ContentDAO.GetSelectCommendOfDeptExcludeRecycle(base.PublishmentSystemInfo.AuxiliaryTableForContent, base.PublishmentSystemID, this.begin, this.end, this.deptID, userName);
            this.spContents.SortField = "DepartmentID ,addCount";
            this.spContents.SortMode = SortMode.DESC;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, AppManager.CMS.LeftMenu.Content.ID_SiteAnalysis, "部门绩效统计", AppManager.CMS.Permission.WebSite.SiteAnalysis);

                this.ddlDepartment.Items.Add(new ListItem("全部", "0"));
                ArrayList departmentIDArrayList = BaiRongDataProvider.DepartmentDAO.GetDepartmentIDArrayListByParentID(0);
                int count = departmentIDArrayList.Count;
                this.isLastNodeArrayOfDepartment = new bool[count];
                foreach (int theDepartmentID in departmentIDArrayList)
                {
                    DepartmentInfo departmentInfo = DepartmentManager.GetDepartmentInfo(theDepartmentID);
                    ListItem listitem = new ListItem(this.GetDepartment(departmentInfo.DepartmentID, departmentInfo.DepartmentName, departmentInfo.ParentsCount, departmentInfo.IsLastNode), theDepartmentID.ToString());
                    if (theDepartmentID == this.deptID)
                        listitem.Selected = true;
                    this.ddlDepartment.Items.Add(listitem);
                }

                this.StartDate.Text = DateUtils.GetDateAndTimeString(this.begin);
                this.EndDate.Text = DateUtils.GetDateAndTimeString(this.end);
                this.ddlDepartment.SelectedValue = this.deptID.ToString();
                this.tbUserName.Text = this.userName;

                this.additional = new NameValueCollection();
                additional["StartDate"] = this.StartDate.Text;
                additional["EndDate"] = this.EndDate.Text;

                this.Image.Attributes.Add("href", BackgroundAnalysisAdministratorImage.GetRedirectUrlString(base.PublishmentSystemID, this.PageUrl));

                //JsManager.RegisterClientScriptBlock(Page, "NodeTreeScript", ChannelLoading.GetScript(base.PublishmentSystemInfo, ELoadingType.SiteAnalysis, this.additional));

                //BindGrid();
                this.spContents.DataBind();
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string userName = TranslateUtils.EvalString(e.Item.DataItem, "userName");
                int addCount = TranslateUtils.EvalInt(e.Item.DataItem, "addCount");
                int updateCount = TranslateUtils.EvalInt(e.Item.DataItem, "updateCount");

                Literal ltlUserName = (Literal)e.Item.FindControl("ltlUserName");
                Literal ltlDisplayName = (Literal)e.Item.FindControl("ltlDisplayName");
                Literal ltlContentAdd = (Literal)e.Item.FindControl("ltlContentAdd");
                Literal ltlContentUpdate = (Literal)e.Item.FindControl("ltlContentUpdate");

                ltlUserName.Text = userName;
                ltlDisplayName.Text = BaiRongDataProvider.AdministratorDAO.GetDisplayName(userName);

                ltlContentAdd.Text = (addCount == 0) ? "0" : string.Format("<a href=\"{1}\">{0}</a>", addCount, string.Format("background_contentByAdmin.aspx?PublishmentSystemID={0}&AdminName={1}&SelectType={2}&StartDate={3}&EndDate={4}&DeptID={5}", base.PublishmentSystemID, userName, "add", this.StartDate.Text, this.EndDate.Text, this.ddlDepartment.SelectedValue));
                ltlContentUpdate.Text = (updateCount == 0) ? "0" : string.Format("<a href=\"{1}\">{0}</a>", updateCount, string.Format("background_contentByAdmin.aspx?PublishmentSystemID={0}&AdminName={1}&SelectType={2}&StartDate={3}&EndDate={4}&DeptID={5}", base.PublishmentSystemID, userName, "edit", this.StartDate.Text, this.EndDate.Text, this.ddlDepartment.SelectedValue));
            }
        }

        public void BindGrid()
        {
            this.rptChannels.DataSource = BaiRongDataProvider.DepartmentDAO.GetDepartmentIDArrayListByParentID(0);
            this.rptChannels.ItemDataBound += new RepeaterItemEventHandler(rptChannels_ItemDataBound);
            this.rptChannels.DataBind();
        }

        void rptChannels_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int id = (int)e.Item.DataItem;
                bool enabled = (base.IsOwningNodeID(id)) ? true : false;
                if (!enabled)
                {
                    if (!base.IsHasChildOwningNodeID(id)) e.Item.Visible = false;
                }
                DepartmentInfo info = DepartmentManager.GetDepartmentInfo(id);

                NoTagText element = (NoTagText)e.Item.FindControl("ElHtml");

                element.Text = DeptLoading.GetDeptRowHtml(base.PublishmentSystemInfo, info, enabled, EDepartmentLoadingType.List, this.additional);
            }
        }

        public void Analysis_OnClick(object sender, EventArgs E)
        {
            //string pageUrl = PageUtils.GetCMSUrl(string.Format("background_analysisByDept.aspx?PublishmentSystemID={0}&StartDate={1}&EndDate={2}&DeptID={3}", base.PublishmentSystemID, this.StartDate.Text, this.EndDate.Text, this.ddlDepartment.SelectedValue));

            PageUtils.Redirect(PageUrl);
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pageUrl = PageUtils.GetCMSUrl(string.Format("background_analysisByDept.aspx?PublishmentSystemID={0}&StartDate={1}&EndDate={2}&DeptID={3}&UserName={4}", base.PublishmentSystemID, this.StartDate.Text, this.EndDate.Text, this.ddlDepartment.SelectedValue, this.tbUserName.Text));
            PageUtils.Redirect(pageUrl);
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = PageUtils.GetCMSUrl(string.Format("background_analysisByDept.aspx?publishmentSystemID={0}&StartDate={1}&EndDate={2}&DeptID={3}&UserName={4}", base.PublishmentSystemID, this.StartDate.Text, this.EndDate.Text, this.ddlDepartment.SelectedValue, this.tbUserName.Text));
                }
                return _pageUrl;
            }
        }


        /// <summary>
        /// by 20151208 sofuny 增加部门搜索  
        /// 部门下拉列表
        /// </summary>
        /// <param name="departmentID"></param>
        /// <param name="departmentName"></param>
        /// <param name="parentsCount"></param>
        /// <param name="isLastNode"></param>
        /// <returns></returns>
        public string GetDepartment(int departmentID, string departmentName, int parentsCount, bool isLastNode)
        {
            string str = "";
            if (isLastNode == false)
            {
                this.isLastNodeArrayOfDepartment[parentsCount] = false;
            }
            else
            {
                this.isLastNodeArrayOfDepartment[parentsCount] = true;
            }
            for (int i = 0; i < parentsCount; i++)
            {
                if (this.isLastNodeArrayOfDepartment[i])
                {
                    str = String.Concat(str, "　");
                }
                else
                {
                    str = String.Concat(str, "│");
                }
            }
            if (isLastNode)
            {
                str = String.Concat(str, "└");
            }
            else
            {
                str = String.Concat(str, "├");
            }
            str = String.Concat(str, departmentName);
            return str;
        }
    }
}
