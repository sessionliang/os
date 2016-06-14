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
using SiteServer.CMS.Controls;



namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundContentChannel : BackgroundBasePage
    {
        public DateTimeTextBox DateFrom;
        public DropDownList SearchType;
        public TextBox Keyword;

        public Repeater rptContents;
        public SqlPager spContents;
        public Literal ltlColumnHeadRows;
        public Literal ltlCommandHeadRows;

        public Repeater rptChannels;

        public Literal ltlContentButtons;
        public Literal ltlChannelButtons;

        private NodeInfo nodeInfo;
        private ETableStyle tableStyle;
        private string tableName;
        private StringCollection attributesOfDisplay;
        private ArrayList relatedIdentities;
        private ArrayList tableStyleInfoArrayList;
        private readonly Hashtable displayNameHashtable = new Hashtable();

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            int nodeID = (!string.IsNullOrEmpty(base.GetQueryString("NodeID"))) ? TranslateUtils.ToInt(base.GetQueryString("NodeID"), base.PublishmentSystemID) : base.PublishmentSystemID;
            this.nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            this.tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, this.nodeInfo);
            this.tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, this.nodeInfo);
            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, nodeID);
            this.tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(this.tableStyle, this.tableName, this.relatedIdentities);
            this.attributesOfDisplay = TranslateUtils.StringCollectionToStringCollection(NodeManager.GetContentAttributesOfDisplay(base.PublishmentSystemID, nodeID));

            new Action(() =>
            {
                VisualInfo.RemovePreviewContent(base.PublishmentSystemInfo, this.nodeInfo);
            }).BeginInvoke(null, null);

            if (base.GetQueryString("TheNodeID") != null && (base.GetQueryString("Subtract") != null || base.GetQueryString("Add") != null))
            {
                int theNodeID = int.Parse(base.GetQueryString("TheNodeID"));
                if (base.PublishmentSystemID != theNodeID)
                {
                    bool isSubtract = (base.GetQueryString("Subtract") != null) ? true : false;
                    DataProvider.NodeDAO.UpdateTaxis(base.PublishmentSystemID, theNodeID, isSubtract);

                    StringUtility.AddLog(base.PublishmentSystemID, theNodeID, 0, "栏目排序" + (isSubtract ? "上升" : "下降"), string.Format("栏目:{0}", NodeManager.GetNodeName(base.PublishmentSystemID, theNodeID)));
                }
            }

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

            //this.spContents.SortField = BaiRongDataProvider.ContentDAO.GetSortFieldName();
            //this.spContents.SortMode = SortMode.DESC;
            if (!string.IsNullOrEmpty(base.GetQueryString("strDirection")))
            {
                this.spContents.SortField = "AddDate";
                if ((base.GetQueryString("strDirection")).Equals("0"))//正序
                {
                    this.spContents.SortMode = SortMode.ASC;
                }
                else//倒序
                {
                    this.spContents.SortMode = SortMode.DESC;
                }
            }
            else
            {
                this.spContents.SortField = BaiRongDataProvider.ContentDAO.GetSortFieldName();
                this.spContents.SortMode = SortMode.DESC;
            }

            //分页的时候，不去查询总条数，直接使用栏目的属性：ContentNum
            this.spContents.IsQueryTotalCount = false;
            this.spContents.TotalCount = this.nodeInfo.ContentNum;

            this.rptChannels.DataSource = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(this.nodeInfo, EScopeType.Children, string.Empty, string.Empty);
            this.rptChannels.ItemDataBound += new RepeaterItemEventHandler(rptChannels_ItemDataBound);

            if (!IsPostBack)
            {
                string nodeName = NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, nodeID).Replace(">", "/");
                base.BreadCrumbWithItemTitle(AppManager.CMS.LeftMenu.ID_Content, "栏目及内容", nodeName, string.Empty);

                this.ltlContentButtons.Text = WebUtils.GetContentCommands(base.PublishmentSystemInfo, this.nodeInfo, this.PageUrl, PageUtils.GetCMSUrl("background_contentChannel.aspx"), false);
                this.ltlChannelButtons.Text = WebUtils.GetChannelCommands(base.PublishmentSystemInfo, this.nodeInfo, this.PageUrl, PageUtils.GetCMSUrl("background_contentChannel.aspx"));
                this.spContents.DataBind();
                this.rptChannels.DataBind();

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
                ltlItemStatus.Text = string.Format(@"<a href=""javascript:;"" title=""设置内容状态"" onclick=""{0}"">{1}</a>", showPopWinString, LevelManager.GetCheckState(base.PublishmentSystemInfo, contentInfo.IsChecked, contentInfo.CheckedLevel));

                if (base.HasChannelPermissions(contentInfo.NodeID, AppManager.CMS.Permission.Channel.ContentEdit) || AdminManager.Current.UserName == contentInfo.AddUserName)
                {
                    ltlItemEditUrl.Text = string.Format("<a href=\"{0}\">编辑</a>", WebUtils.GetContentAddEditUrl(base.PublishmentSystemID, this.nodeInfo, contentInfo.ID, this.PageUrl));
                }

                ltlColumnItemRows.Text = ContentUtility.GetColumnItemRowsHtml(this.tableStyleInfoArrayList, this.attributesOfDisplay, this.displayNameHashtable, this.tableStyle, base.PublishmentSystemInfo, contentInfo);

                ltlCommandItemRows.Text = ContentUtility.GetCommandItemRowsHtml(this.tableStyle, base.PublishmentSystemInfo, this.nodeInfo, contentInfo, this.PageUrl);
            }
        }

        void rptChannels_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int nodeID = (int)e.Item.DataItem;
            bool enabled = (base.IsOwningNodeID(nodeID)) ? true : false;
            if (!enabled)
            {
                if (!base.IsHasChildOwningNodeID(nodeID)) e.Item.Visible = false;
            }
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);

            Literal ltlEditLink = e.Item.FindControl("ltlEditLink") as Literal;
            Literal ltlNodeTitle = e.Item.FindControl("ltlNodeTitle") as Literal;

            Literal ltlNodeIndexName = e.Item.FindControl("ltlNodeIndexName") as Literal;
            Literal ltlUpLink = e.Item.FindControl("ltlUpLink") as Literal;
            Literal ltlDownLink = e.Item.FindControl("ltlDownLink") as Literal;
            Literal ltlCheckBoxHtml = e.Item.FindControl("ltlCheckBoxHtml") as Literal;

            if (enabled && base.HasChannelPermissions(nodeID, AppManager.CMS.Permission.Channel.ChannelEdit))
            {
                string urlEdit = PageUtils.GetCMSUrl(string.Format("background_channelEdit.aspx?NodeID={0}&PublishmentSystemID={1}&ReturnUrl={2}", nodeID, base.PublishmentSystemID, StringUtils.ValueToUrl(this.PageUrl)));
                ltlEditLink.Text = string.Format("<a href=\"{0}\">编辑</a>", urlEdit);

                ltlUpLink.Text = string.Format(@"<a href=""{0}&TheNodeID={1}&Subtract=True""><img src=""../Pic/icon/up.gif"" border=""0"" alt=""上升"" /></a>", this.PageUrl, nodeInfo.NodeID);
                ltlDownLink.Text = string.Format(@"<a href=""{0}&TheNodeID={1}&Add=True""><img src=""../Pic/icon/down.gif"" border=""0"" alt=""下降"" /></a>", this.PageUrl, nodeInfo.NodeID);
            }

            ltlNodeTitle.Text = string.Format(@"<a href=""{0}"" title=""浏览页面"" target=""_blank""><img src=""{1}"" border=""0"" align=""absMiddle"" /></a>&nbsp;<A title=""进入栏目"" href=""{2}"">{3}</A>&nbsp;{4}&nbsp;<SPAN class=""gray"" style=""FONT-SIZE: 8pt; FONT-FAMILY: arial"">({5})</SPAN>", PageUtility.ServiceSTL.Utils.GetRedirectUrl(nodeID, true), PageUtils.GetIconUrl("tree/folder.gif"), PageUtils.GetCMSUrl(string.Format("background_contentChannel.aspx?PublishmentSystemID={0}&NodeID={1}", base.PublishmentSystemID, nodeID)), nodeInfo.NodeName, NodeManager.GetNodeTreeLastImageHtml(base.PublishmentSystemInfo, nodeInfo), nodeInfo.ContentNum);

            ltlNodeIndexName.Text = nodeInfo.NodeIndexName;

            if (enabled)
            {
                ltlCheckBoxHtml.Text = string.Format("<input type='checkbox' name='ChannelIDCollection' value='{0}' />", nodeInfo.NodeID);
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
                    _pageUrl = PageUtils.GetCMSUrl(string.Format("background_contentchannel.aspx?PublishmentSystemID={0}&NodeID={1}&DateFrom={2}&SearchType={3}&Keyword={4}&page={5}", base.PublishmentSystemID, this.nodeInfo.NodeID, this.DateFrom.Text, this.SearchType.SelectedValue, this.Keyword.Text, TranslateUtils.ToInt(base.GetQueryString("page"), 1)));
                }
                return _pageUrl;
            }
        }
    }
}
