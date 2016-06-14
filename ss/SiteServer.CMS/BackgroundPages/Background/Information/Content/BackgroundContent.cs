using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;


using System.Data;
using BaiRong.Model.Service;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundContent : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;
        public Literal ltlColumnHeadRows;
        public Literal ltlCommandHeadRows;

        public Literal ltlContentButtons;

        public DateTimeTextBox DateFrom;
        public DropDownList SearchType;
        public TextBox Keyword;

        private NodeInfo nodeInfo;
        private ETableStyle tableStyle;
        private string tableName;
        private StringCollection attributesOfDisplay;
        private ArrayList relatedIdentities;
        private ArrayList tableStyleInfoArrayList;
        private readonly Hashtable valueHashtable = new Hashtable();

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "NodeID");
            int nodeID = base.GetIntQueryString("NodeID");
            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, nodeID);
            this.nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            this.tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, this.nodeInfo);
            this.tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, this.nodeInfo);
            this.tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(this.tableStyle, this.tableName, this.relatedIdentities);

            new Action(() =>
            {
                VisualInfo.RemovePreviewContent(base.PublishmentSystemInfo, this.nodeInfo);
            }).BeginInvoke(null, null);

            if (!base.HasChannelPermissions(nodeID, AppManager.CMS.Permission.Channel.ContentView, AppManager.CMS.Permission.Channel.ContentAdd, AppManager.CMS.Permission.Channel.ContentEdit, AppManager.CMS.Permission.Channel.ContentDelete, AppManager.CMS.Permission.Channel.ContentTranslate))
            {
                if (!BaiRongDataProvider.AdministratorDAO.IsAuthenticated)
                {
                    PageUtils.RedirectToLoginPage();
                    return;
                }
                else
                {
                    PageUtils.RedirectToErrorPage("您无此栏目的操作权限！");
                    return;
                }
            }

            this.attributesOfDisplay = TranslateUtils.StringCollectionToStringCollection(NodeManager.GetContentAttributesOfDisplay(base.PublishmentSystemID, nodeID));

            //this.attributesOfDisplay = TranslateUtils.StringCollectionToStringCollection(this.nodeInfo.Additional.ContentAttributesOfDisplay);

            this.spContents.ControlToPaginate = this.rptContents;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;

            if (!string.IsNullOrEmpty(base.GetQueryString("SearchType")))
            {
                ArrayList owningNodeIDArrayList = new ArrayList();
                owningNodeIDArrayList.Add(nodeID);
                this.spContents.SelectCommand = DataProvider.ContentDAO.GetSelectCommend(this.tableStyle, this.tableName, base.PublishmentSystemID, nodeID, PermissionsManager.Current.IsSystemAdministrator, owningNodeIDArrayList, base.GetQueryString("SearchType"), base.GetQueryString("Keyword"), base.GetQueryString("DateFrom"), string.Empty, false, ETriState.All, false, false, AdminUtility.IsViewContentOnlySelf(base.PublishmentSystemID, nodeID));
            }
            else
            {
                this.spContents.SelectCommand = BaiRongDataProvider.ContentDAO.GetSelectCommend(this.tableName, nodeID, ETriState.All, AdminUtility.IsViewContentOnlySelf(base.PublishmentSystemID, nodeID));
            }

            this.spContents.SortField = BaiRongDataProvider.ContentDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;
            this.spContents.OrderByString = ETaxisTypeUtils.GetOrderByString(this.tableStyle, ETaxisType.OrderByTaxisDesc);

            //分页的时候，不去查询总条数，直接使用栏目的属性：ContentNum
            this.spContents.IsQueryTotalCount = false;
            this.spContents.TotalCount = this.nodeInfo.ContentNum;


            if (!IsPostBack)
            {
                string nodeName = NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, nodeID);
                base.BreadCrumbWithItemTitle(AppManager.CMS.LeftMenu.ID_Content, "内容管理", nodeName, string.Empty);

                this.ltlContentButtons.Text = WebUtils.GetContentCommands(base.PublishmentSystemInfo, this.nodeInfo, this.PageUrl, PageUtils.GetCMSUrl("background_content.aspx"), false);
                this.spContents.DataBind();

                if (this.tableStyleInfoArrayList != null)
                {
                    foreach (TableStyleInfo styleInfo in this.tableStyleInfoArrayList)
                    {
                        if (styleInfo.IsVisible)
                        {
                            ListItem listitem = new ListItem(styleInfo.DisplayName, styleInfo.AttributeName);
                            this.SearchType.Items.Add(listitem);
                        }
                    }
                }

                //添加隐藏属性
                this.SearchType.Items.Add(new ListItem("内容ID", ContentAttribute.ID));
                this.SearchType.Items.Add(new ListItem("添加者", ContentAttribute.AddUserName));
                this.SearchType.Items.Add(new ListItem("最后修改者", ContentAttribute.LastEditUserName));
                this.SearchType.Items.Add(new ListItem("内容组", ContentAttribute.ContentGroupNameCollection));

                if (!string.IsNullOrEmpty(base.GetQueryString("SearchType")))
                {
                    this.DateFrom.Text = base.GetQueryString("DateFrom");
                    ControlUtils.SelectListItems(this.SearchType, base.GetQueryString("SearchType"));
                    this.Keyword.Text = base.GetQueryString("Keyword");
                    this.ltlContentButtons.Text += @"
<script>
$(document).ready(function() {
	$('#contentSearch').show();
});
</script>
";
                }

                this.ltlColumnHeadRows.Text = ContentUtility.GetColumnHeadRowsHtml(this.tableStyleInfoArrayList, this.attributesOfDisplay, this.tableStyle, base.PublishmentSystemInfo);
                this.ltlCommandHeadRows.Text = ContentUtility.GetCommandHeadRowsHtml(this.tableStyle, base.PublishmentSystemInfo, this.nodeInfo);
                this.ltlCommandHeadRows.Text += ContentUtility.GetFunctionHeadRowsHtml(this.tableStyle, base.PublishmentSystemInfo, this.nodeInfo);
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlItemTitle = e.Item.FindControl("ltlItemTitle") as Literal;
                Literal ltlColumnItemRows = e.Item.FindControl("ltlColumnItemRows") as Literal;
                Literal ltlItemStatus = e.Item.FindControl("ltlItemStatus") as Literal;
                Literal ltlItemEditUrl = e.Item.FindControl("ltlItemEditUrl") as Literal;
                Literal ltlCommandItemRows = e.Item.FindControl("ltlCommandItemRows") as Literal;

                ContentInfo contentInfo = new ContentInfo(e.Item.DataItem);

                ltlItemTitle.Text = WebUtils.GetContentTitle(base.PublishmentSystemInfo, contentInfo, this.PageUrl);

                string showPopWinString = Modal.CheckState.GetOpenWindowString(base.PublishmentSystemID, contentInfo, this.PageUrl);


                int checkTaskId = TranslateUtils.ToInt(contentInfo.GetExtendedAttribute(ContentAttribute.Check_TaskID));
                int unCheckTaskId = TranslateUtils.ToInt(contentInfo.GetExtendedAttribute(ContentAttribute.UnCheck_TaskID));
                TaskInfo checkTaskInfo = BaiRongDataProvider.TaskDAO.GetTaskInfo(checkTaskId);
                TaskInfo unCheckTaskInfo = BaiRongDataProvider.TaskDAO.GetTaskInfo(unCheckTaskId);
                int task = 0;
                if (checkTaskInfo != null || unCheckTaskInfo != null)
                    task = 1;

                if ((task == 1) || (contentInfo.CheckTaskDate != DateUtils.SqlMinValue && contentInfo.UnCheckTaskDate != DateUtils.SqlMinValue))
                    ltlItemStatus.Text = string.Format(@"<a href=""javascript:;"" title=""设置内容状态"" onclick=""{0}""><img style='width:14px;' src='{2}'/>{1}</a>", showPopWinString, LevelManager.GetCheckState(base.PublishmentSystemInfo, contentInfo.IsChecked, contentInfo.CheckedLevel), PageUtils.GetIconUrl("clock.ico"));
                else
                    ltlItemStatus.Text = string.Format(@"<a href=""javascript:;"" title=""设置内容状态"" onclick=""{0}"">{1}</a>", showPopWinString, LevelManager.GetCheckState(base.PublishmentSystemInfo, contentInfo.IsChecked, contentInfo.CheckedLevel));


                if (base.HasChannelPermissions(contentInfo.NodeID, AppManager.CMS.Permission.Channel.ContentEdit) || AdminManager.Current.UserName == contentInfo.AddUserName)
                {
                    ltlItemEditUrl.Text = string.Format("<a href=\"{0}\">编辑</a>", WebUtils.GetContentAddEditUrl(base.PublishmentSystemID, this.nodeInfo, contentInfo.ID, this.PageUrl));
                }

                ltlColumnItemRows.Text = ContentUtility.GetColumnItemRowsHtml(this.tableStyleInfoArrayList, this.attributesOfDisplay, this.valueHashtable, this.tableStyle, base.PublishmentSystemInfo, contentInfo);

                ltlCommandItemRows.Text = ContentUtility.GetCommandItemRowsHtml(this.tableStyle, base.PublishmentSystemInfo, this.nodeInfo, contentInfo, this.PageUrl);
                ltlCommandItemRows.Text += ContentUtility.GetFunctionItemRowsHtml(this.tableStyle, base.PublishmentSystemInfo, this.nodeInfo, contentInfo, this.PageUrl); 
            }
        }

        public void Search_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = PageUtils.GetCMSUrl(string.Format("background_content.aspx?PublishmentSystemID={0}&NodeID={1}&DateFrom={2}&SearchType={3}&Keyword={4}&page={5}", base.PublishmentSystemID, this.nodeInfo.NodeID, this.DateFrom.Text, this.SearchType.SelectedValue, this.Keyword.Text, TranslateUtils.ToInt(base.GetQueryString("page"), 1)));
                }
                return _pageUrl;
            }
        }

        public static string GetRedirectUrl(int publishmentSystemID, int nodeID)
        {
            return PageUtils.GetCMSUrl(string.Format("background_content.aspx?PublishmentSystemID={0}&NodeID={1}", publishmentSystemID, nodeID));
        }
    }
}
