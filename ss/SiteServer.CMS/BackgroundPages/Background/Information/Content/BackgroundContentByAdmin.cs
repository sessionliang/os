using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;
using SiteServer.CMS.Controls;



namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundContentByAdmin : BackgroundBasePage
    {
        public DropDownList NodeIDDropDownList;
        public DropDownList State;
        public CheckBox IsDuplicate;
        public DropDownList SearchType;
        public TextBox Keyword;
        public DateTimeTextBox DateFrom;
        public DateTimeTextBox DateTo;

        public Repeater rptContents;
        public SqlPager spContents;
        public Literal ltlColumnHeadRows;
        public Literal ltlCommandHeadRows;

        int nodeID = 0;
        NodeInfo nodeInfo;
        private ETableStyle tableStyle;
        private StringCollection attributesOfDisplay;
        ArrayList relatedIdentities;
        ArrayList tableStyleInfoArrayList;
        string adminName, selectType;
        private string begin;
        private string end;
        private string deptID;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            if (!string.IsNullOrEmpty(base.GetQueryString("NodeID")))
            {
                this.nodeID = base.GetIntQueryString("NodeID");
            }
            else
            {
                this.nodeID = base.PublishmentSystemID;
            }
            this.adminName = base.GetQueryString("AdminName");
            this.selectType = base.GetQueryString("SelectType");
            this.begin = base.GetQueryString("StartDate");
            this.end = base.GetQueryString("EndDate");
            this.deptID = base.GetQueryString("DeptID");

            this.nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            this.tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeInfo);
            string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);
            this.attributesOfDisplay = TranslateUtils.StringCollectionToStringCollection(NodeManager.GetContentAttributesOfDisplay(base.PublishmentSystemID, nodeID));
            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, this.nodeID);
            this.tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, this.relatedIdentities);

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            if (string.IsNullOrEmpty(base.GetQueryString("NodeID")) && string.IsNullOrEmpty(base.GetQueryString("DeptID")))
            {
                ETriState stateType = ETriStateUtils.GetEnumType(this.State.SelectedValue);
                this.spContents.SelectCommand = DataProvider.ContentDAO.GetSelectCommendBySelectType(tableStyle, tableName, base.PublishmentSystemID, this.nodeID, PermissionsManager.Current.IsSystemAdministrator, ProductPermissionsManager.Current.OwningNodeIDArrayList, this.SearchType.SelectedValue, this.Keyword.Text, this.DateFrom.Text, this.DateTo.Text, true, stateType, !this.IsDuplicate.Checked, false, this.selectType, this.adminName);
            }
            else
            {
                ETriState stateType = ETriStateUtils.GetEnumType(base.GetQueryString("State"));
                this.spContents.SelectCommand = DataProvider.ContentDAO.GetSelectCommendBySelectType(tableStyle, tableName, base.PublishmentSystemID, this.nodeID, PermissionsManager.Current.IsSystemAdministrator, ProductPermissionsManager.Current.OwningNodeIDArrayList, base.GetQueryString("SearchType"), base.GetQueryString("Keyword"), base.GetQueryString("StartDate"), base.GetQueryString("EndDate"), true, stateType, !TranslateUtils.ToBool(base.GetQueryString("IsDuplicate")), false, this.selectType, this.adminName);
            }
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.SortField = ContentAttribute.ID;
            this.spContents.SortMode = SortMode.DESC;
            this.spContents.OrderByString = ETaxisTypeUtils.GetOrderByString(this.tableStyle, ETaxisType.OrderByIDDesc);
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, "我的文章", string.Empty);

                NodeManager.AddListItems(this.NodeIDDropDownList.Items, base.PublishmentSystemInfo, true, true);

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

                ETriStateUtils.AddListItems(this.State, "全部", "已审核", "待审核");

                //添加隐藏属性
                this.SearchType.Items.Add(new ListItem("内容ID", ContentAttribute.ID));
                this.SearchType.Items.Add(new ListItem("添加者", ContentAttribute.AddUserName));
                this.SearchType.Items.Add(new ListItem("最后修改者", ContentAttribute.LastEditUserName));

                if (!string.IsNullOrEmpty(base.GetQueryString("NodeID")) || !string.IsNullOrEmpty(base.GetQueryString("DeptID")))
                {
                    if (base.PublishmentSystemID != this.nodeID)
                    {
                        ControlUtils.SelectListItems(this.NodeIDDropDownList, this.nodeID.ToString());
                    }
                    ControlUtils.SelectListItems(this.State, base.GetQueryString("State"));
                    this.IsDuplicate.Checked = TranslateUtils.ToBool(base.GetQueryString("IsDuplicate"));
                    ControlUtils.SelectListItems(this.SearchType, base.GetQueryString("SearchType"));
                    this.Keyword.Text = base.GetQueryString("Keyword");
                    this.DateFrom.Text = base.GetQueryString("StartDate");
                    this.DateTo.Text = base.GetQueryString("EndDate");
                }

                this.spContents.DataBind();


                this.ltlColumnHeadRows.Text = ContentUtility.GetColumnHeadRowsHtml(this.tableStyleInfoArrayList, this.attributesOfDisplay, this.tableStyle, base.PublishmentSystemInfo);
            }

        }

        private readonly Hashtable valueHashtable = new Hashtable();

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlItemTitle = e.Item.FindControl("ltlItemTitle") as Literal;
                Literal ltlChannel = e.Item.FindControl("ltlChannel") as Literal;
                Literal ltlColumnItemRows = e.Item.FindControl("ltlColumnItemRows") as Literal;
                Literal ltlItemStatus = e.Item.FindControl("ltlItemStatus") as Literal;
                Literal ltlItemEditUrl = e.Item.FindControl("ltlItemEditUrl") as Literal;
                Literal ltlCommandItemRows = e.Item.FindControl("ltlCommandItemRows") as Literal;
                Literal ltlSelect = e.Item.FindControl("ltlSelect") as Literal;
                Literal ltlLink = e.Item.FindControl("ltlLink") as Literal;
                Literal ltlHits = e.Item.FindControl("ltlHits") as Literal;

                ContentInfo contentInfo = new ContentInfo(e.Item.DataItem);
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, contentInfo.NodeID);

                ltlItemTitle.Text = WebUtils.GetContentTitle(base.PublishmentSystemInfo, contentInfo, this.PageUrl);
                string nodeName = valueHashtable[contentInfo.NodeID] as string;
                if (nodeName == null)
                {
                    nodeName = NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, contentInfo.NodeID);
                    valueHashtable[contentInfo.NodeID] = nodeName;
                }
                ltlChannel.Text = nodeName;

                //string showPopWinString = Modal.CheckState.GetOpenWindowString(base.PublishmentSystemID, contentInfo, this.PageUrl);
                //ltlItemStatus.Text = string.Format(@"<a href=""javascript:;"" title=""设置内容状态"" onclick=""{0}"">{1}</a>", showPopWinString, LevelManager.GetCheckState(base.PublishmentSystemInfo, contentInfo.IsChecked, contentInfo.CheckedLevel));
                ltlItemStatus.Text = string.Format(@"{0}", LevelManager.GetCheckState(base.PublishmentSystemInfo, contentInfo.IsChecked, contentInfo.CheckedLevel));


                ltlColumnItemRows.Text = ContentUtility.GetColumnItemRowsHtml(this.tableStyleInfoArrayList, this.attributesOfDisplay, this.valueHashtable, this.tableStyle, base.PublishmentSystemInfo, contentInfo);

                string url = PageUtility.GetContentUrl(base.PublishmentSystemInfo, contentInfo, true, PublishmentSystemInfo.Additional.VisualType);
                ltlLink.Text = string.Format("<a href=\"{0}\" target=\"_blank\">{0}</a>", url);

                ltlHits.Text = contentInfo.Hits.ToString();
            }
        }


        public void Search_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        public void btnRetun_OnClick(object sender, EventArgs E)
        {
            string pageUrl = PageUtils.GetCMSUrl(string.Format("background_analysisByDept.aspx?PublishmentSystemID={0}&StartDate={1}&EndDate={2}&DeptID={3}", base.PublishmentSystemID, this.begin, this.end, this.deptID));
            PageUtils.Redirect(pageUrl);
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    this._pageUrl = PageUtils.GetCMSUrl(string.Format("background_contentByAdmin.aspx?PublishmentSystemID={0}&NodeID={1}&State={2}&IsDuplicate={3}&SearchType={4}&Keyword={5}&StartDate={6}&EndDate={7}&AdminName={8}&SelectType={9}", base.PublishmentSystemID, this.NodeIDDropDownList.SelectedValue, this.State.SelectedValue, this.IsDuplicate.Checked, this.SearchType.SelectedValue, this.Keyword.Text, this.DateFrom.Text, this.DateTo.Text, this.adminName, this.selectType));
                }
                return this._pageUrl;
            }
        }
    }
}
