using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using System.Collections;
using BaiRong.Model;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;

using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Core.Security;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class ContentMultipleSelect : BackgroundBasePage
    {
        public DropDownList NodeIDDropDownList;
        public CheckBox IsDuplicate;
        public DropDownList SearchType;
        public TextBox Keyword;
        public DateTimeTextBox DateFrom;
        public DateTimeTextBox DateTo;

        public Repeater rptContents;
        public SqlPager spContents;

        private NodeInfo nodeInfo;
        private ETableStyle tableStyle;
        private string tableName;
        private StringCollection attributesOfDisplay;
        private ArrayList relatedIdentities;
        private ArrayList tableStyleInfoArrayList;

        private string jsMethod;

        public static string GetOpenWindowString(int publishmentSystemID, string jsMethod)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("jsMethod", jsMethod);
            return PageUtility.GetOpenWindowString("选择内容", "modal_contentMultipleSelect.aspx", arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.jsMethod = base.GetQueryString("jsMethod");

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            int nodeID = TranslateUtils.ToInt(base.GetQueryString("NodeID"));
            if (nodeID == 0)
            {
                nodeID = base.PublishmentSystemID;
            }
            this.nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            this.tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeInfo);
            this.tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);
            this.attributesOfDisplay = TranslateUtils.StringCollectionToStringCollection(NodeManager.GetContentAttributesOfDisplay(base.PublishmentSystemID, nodeID));
            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, this.nodeInfo.NodeID);
            this.tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, this.relatedIdentities);

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            if (string.IsNullOrEmpty(base.GetQueryString("NodeID")))
            {
                this.spContents.SelectCommand = DataProvider.ContentDAO.GetSelectCommend(tableStyle, tableName, base.PublishmentSystemID, this.nodeInfo.NodeID, PermissionsManager.Current.IsSystemAdministrator, ProductPermissionsManager.Current.OwningNodeIDArrayList, this.SearchType.SelectedValue, this.Keyword.Text, this.DateFrom.Text, this.DateTo.Text, true, ETriState.True, !this.IsDuplicate.Checked, false);
            }
            else
            {
                this.spContents.SelectCommand = DataProvider.ContentDAO.GetSelectCommend(tableStyle, tableName, base.PublishmentSystemID, this.nodeInfo.NodeID, PermissionsManager.Current.IsSystemAdministrator, ProductPermissionsManager.Current.OwningNodeIDArrayList, base.GetQueryString("SearchType"), base.GetQueryString("Keyword"), base.GetQueryString("DateFrom"), base.GetQueryString("DateTo"), true, ETriState.True, !TranslateUtils.ToBool(base.GetQueryString("IsDuplicate")), true);
            }
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.SortField = ContentAttribute.ID;
            this.spContents.SortMode = SortMode.DESC;
            this.spContents.OrderByString = ETaxisTypeUtils.GetOrderByString(this.tableStyle, ETaxisType.OrderByIDDesc);
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                NodeManager.AddListItems(this.NodeIDDropDownList.Items, base.PublishmentSystemInfo, false, true, true, EContentModelType.Goods);

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

                if (!string.IsNullOrEmpty(base.GetQueryString("NodeID")))
                {
                    if (base.PublishmentSystemID != this.nodeInfo.NodeID)
                    {
                        ControlUtils.SelectListItems(this.NodeIDDropDownList, this.nodeInfo.NodeID.ToString());
                    }
                    this.IsDuplicate.Checked = TranslateUtils.ToBool(base.GetQueryString("IsDuplicate"));
                    ControlUtils.SelectListItems(this.SearchType, base.GetQueryString("SearchType"));
                    this.Keyword.Text = base.GetQueryString("Keyword");
                    this.DateFrom.Text = base.GetQueryString("DateFrom");
                    this.DateTo.Text = base.GetQueryString("DateTo");
                }

                this.spContents.DataBind();
            }
        }

        private readonly Hashtable valueHashtable = new Hashtable();
        
        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlChannel = e.Item.FindControl("ltlChannel") as Literal;
                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                Literal ltlSelect = e.Item.FindControl("ltlSelect") as Literal;

                GoodsContentInfo contentInfo = new GoodsContentInfo(e.Item.DataItem);

                string nodeName = valueHashtable[contentInfo.NodeID] as string;
                if (nodeName == null)
                {
                    nodeName = NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, contentInfo.NodeID);
                    valueHashtable[contentInfo.NodeID] = nodeName;
                }
                ltlChannel.Text = nodeName;

                ltlTitle.Text = WebUtils.GetContentTitle(base.PublishmentSystemInfo, contentInfo, this.PageUrl);

                ltlSelect.Text = string.Format(@"<input type=""checkbox"" name=""IDsCollection"" value=""{0}_{1}"" />", contentInfo.NodeID, contentInfo.ID);
            }
        }

        public void AddContent_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(WebUtils.GetContentAddAddUrl(base.PublishmentSystemID, this.nodeInfo, this.PageUrl));
        }

        public void Search_OnClick(object sender, EventArgs E)
        {
            base.Response.Redirect(this.PageUrl, true);
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request.Form["IDsCollection"]))
            {
                StringBuilder builder = new StringBuilder();
                foreach (string pair in TranslateUtils.StringCollectionToStringList(base.Request.Form["IDsCollection"]))
                {
                    int channelID = TranslateUtils.ToInt(pair.Split('_')[0]);
                    int contentID = TranslateUtils.ToInt(pair.Split('_')[1]);

                    string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, channelID);
                    string title = BaiRongDataProvider.ContentDAO.GetValue(tableName, contentID, ContentAttribute.Title);
                    builder.AppendFormat(@"parent.{0}('{1}', '{2}');", this.jsMethod, title, pair);
                }
                JsUtils.OpenWindow.CloseModalPageWithoutRefresh(base.Page, builder.ToString());
            }
            else
            {
                JsUtils.OpenWindow.CloseModalPageWithoutRefresh(base.Page);
            }
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    this._pageUrl = string.Format("modal_contentMultipleSelect.aspx?PublishmentSystemID={0}&NodeID={1}&IsDuplicate={2}&SearchType={3}&Keyword={4}&DateFrom={5}&DateTo={6}", base.PublishmentSystemID, this.NodeIDDropDownList.SelectedValue, this.IsDuplicate.Checked, this.SearchType.SelectedValue, this.Keyword.Text, this.DateFrom.Text, this.DateTo.Text);
                }
                return this._pageUrl;
            }
        }
    }
}
