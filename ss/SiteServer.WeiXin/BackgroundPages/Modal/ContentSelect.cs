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


using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using System.Text;
using System.Collections.Generic;
using SiteServer.WeiXin.Model;

namespace SiteServer.WeiXin.BackgroundPages.Modal
{
    public class ContentSelect : BackgroundBasePage
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

        int nodeID = 0;
        NodeInfo nodeInfo;
        private ETableStyle tableStyle;
        private StringCollection attributesOfDisplay;
        ArrayList relatedIdentities;
        ArrayList tableStyleInfoArrayList;

        private bool isMultiple;
        private string jsMethod;
        private int itemIndex;

        private bool isKeywordAdd;
        private int keywordID;

        public static string GetRedirectUrlByKeywordAddList(int publishmentSystemID, bool isMultiple, int keywordID)
        {
            return PageUtils.GetWXUrl(string.Format("modal_contentSelect.aspx?publishmentSystemID={0}&isMultiple={1}&isKeywordAdd=true&keywordID={2}", publishmentSystemID, isMultiple, keywordID));
        }

        public static string GetOpenWindowString(int publishmentSystemID, bool isMultiple, string jsMethod)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("isMultiple", isMultiple.ToString());
            arguments.Add("jsMethod", jsMethod);
            return PageUtilityWX.GetOpenWindowString("选择微官网内容", "modal_contentSelect.aspx", arguments);
        }

        public static string GetOpenWindowStringByItemIndex(int publishmentSystemID, string jsMethod, string itemIndex)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("jsMethod", jsMethod);
            arguments.Add("itemIndex", itemIndex);
            return PageUtilityWX.GetOpenWindowString("选择微官网内容", "modal_contentSelect.aspx", arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.isMultiple = TranslateUtils.ToBool(Request.QueryString["isMultiple"]);
            this.jsMethod = Request.QueryString["jsMethod"];
            this.itemIndex = TranslateUtils.ToInt(Request.QueryString["itemIndex"]);

            this.isKeywordAdd = TranslateUtils.ToBool(Request.QueryString["isKeywordAdd"]);
            this.keywordID = TranslateUtils.ToInt(Request.QueryString["keywordID"]);

            if (!string.IsNullOrEmpty(Request.QueryString["NodeID"]))
            {
                this.nodeID = int.Parse(Request.QueryString["NodeID"]);
            }
            else
            {
                this.nodeID = base.PublishmentSystemID;
            }
            this.nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            this.tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeInfo);
            string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);
            this.attributesOfDisplay = TranslateUtils.StringCollectionToStringCollection(NodeManager.GetContentAttributesOfDisplay(base.PublishmentSystemID, nodeID));
            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, this.nodeID);
            this.tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, this.relatedIdentities);

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            if (string.IsNullOrEmpty(base.Request.QueryString["NodeID"]))
            {
                ETriState stateType = ETriStateUtils.GetEnumType(this.State.SelectedValue);
                this.spContents.SelectCommand = DataProvider.ContentDAO.GetSelectCommend(tableStyle, tableName, base.PublishmentSystemID, this.nodeID, PermissionsManager.Current.IsSystemAdministrator, ProductPermissionsManager.Current.OwningNodeIDArrayList, this.SearchType.SelectedValue, this.Keyword.Text, this.DateFrom.Text, this.DateTo.Text, true, stateType, !this.IsDuplicate.Checked, false);
            }
            else
            {
                ETriState stateType = ETriStateUtils.GetEnumType(base.Request.QueryString["State"]);
                this.spContents.SelectCommand = DataProvider.ContentDAO.GetSelectCommend(tableStyle, tableName, base.PublishmentSystemID, this.nodeID, PermissionsManager.Current.IsSystemAdministrator, ProductPermissionsManager.Current.OwningNodeIDArrayList, base.Request.QueryString["SearchType"], base.Request.QueryString["Keyword"], base.Request.QueryString["DateFrom"], base.Request.QueryString["DateTo"], true, stateType, !TranslateUtils.ToBool(base.Request.QueryString["IsDuplicate"]), false);
            }
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.SortField = ContentAttribute.ID;
            this.spContents.SortMode = SortMode.DESC;
            this.spContents.OrderByString = ETaxisTypeUtils.GetOrderByString(this.tableStyle, ETaxisType.OrderByIDDesc);
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, "内容搜索", string.Empty);

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

                if (!string.IsNullOrEmpty(base.Request.QueryString["NodeID"]))
                {
                    if (base.PublishmentSystemID != this.nodeID)
                    {
                        ControlUtils.SelectListItems(this.NodeIDDropDownList, this.nodeID.ToString());
                    }
                    ControlUtils.SelectListItems(this.State, base.Request.QueryString["State"]);
                    this.IsDuplicate.Checked = TranslateUtils.ToBool(base.Request.QueryString["IsDuplicate"]);
                    ControlUtils.SelectListItems(this.SearchType, base.Request.QueryString["SearchType"]);
                    this.Keyword.Text = base.Request.QueryString["Keyword"];
                    this.DateFrom.Text = base.Request.QueryString["DateFrom"];
                    this.DateTo.Text = base.Request.QueryString["DateTo"];
                }

                this.spContents.DataBind();
            }
        }

        private readonly Hashtable valueHashtable = new Hashtable();

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlItemTitle = e.Item.FindControl("ltlItemTitle") as Literal;
                Literal ltlChannel = e.Item.FindControl("ltlChannel") as Literal;
                Literal ltlItemImageUrl = e.Item.FindControl("ltlItemImageUrl") as Literal;
                Literal ltlItemSummary = e.Item.FindControl("ltlItemSummary") as Literal;
                Literal ltlSelect = e.Item.FindControl("ltlSelect") as Literal;

                ContentInfo contentInfo = new ContentInfo(e.Item.DataItem);

                ltlItemTitle.Text = WebUtils.GetContentTitle(base.PublishmentSystemInfo, contentInfo, this.PageUrl);
                string nodeName = valueHashtable[contentInfo.NodeID] as string;
                if (nodeName == null)
                {
                    nodeName = NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, contentInfo.NodeID);
                    valueHashtable[contentInfo.NodeID] = nodeName;
                }
                ltlChannel.Text = nodeName;

                string imageUrl = contentInfo.GetExtendedAttribute(BackgroundContentAttribute.ImageUrl);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    ltlItemImageUrl.Text = string.Format(@"<img src=""{0}"" style=""max-width:78px;max-height:78px;"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, imageUrl));
                }

                ltlItemSummary.Text = MPUtils.GetSummary(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.Summary), contentInfo.GetExtendedAttribute(BackgroundContentAttribute.Content));

                if (this.isMultiple)
                {
                    ltlSelect.Text = string.Format(@"<input type=""checkbox"" name=""IDsCollection"" value=""{0}_{1}"" />", contentInfo.NodeID, contentInfo.ID);
                }
                else
                {
                    ltlSelect.Text = string.Format(@"<input type=""radio"" name=""IDsCollection"" value=""{0}_{1}"" />", contentInfo.NodeID, contentInfo.ID);
                }
            }
        }

        public void Search_OnClick(object sender, EventArgs E)
        {
            base.Response.Redirect(this.PageUrl, true);
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    if (base.Request.QueryString["itemIndex"] != null)
                    {
                        this._pageUrl = PageUtils.GetWXUrl(string.Format("modal_contentSelect.aspx?publishmentSystemID={0}&isMultiple={1}&jsMethod={2}&itemIndex={3}&isKeywordAdd={4}&keywordID={5}&", base.PublishmentSystemID, this.isMultiple, this.jsMethod, this.itemIndex, this.isKeywordAdd, this.keywordID)) + string.Format("NodeID={0}&State={1}&IsDuplicate={2}&SearchType={3}&Keyword={4}&DateFrom={5}&DateTo={6}", this.NodeIDDropDownList.SelectedValue, this.State.SelectedValue, this.IsDuplicate.Checked, this.SearchType.SelectedValue, this.Keyword.Text, this.DateFrom.Text, this.DateTo.Text);
                    }
                    else
                    {
                        this._pageUrl = PageUtils.GetWXUrl(string.Format("modal_contentSelect.aspx?publishmentSystemID={0}&isMultiple={1}&jsMethod={2}&isKeywordAdd={3}&keywordID={4}&", base.PublishmentSystemID, this.isMultiple, this.jsMethod, this.isKeywordAdd, this.keywordID)) + string.Format("NodeID={0}&State={1}&IsDuplicate={2}&SearchType={3}&Keyword={4}&DateFrom={5}&DateTo={6}", this.NodeIDDropDownList.SelectedValue, this.State.SelectedValue, this.IsDuplicate.Checked, this.SearchType.SelectedValue, this.Keyword.Text, this.DateFrom.Text, this.DateTo.Text);
                    }
                }
                return this._pageUrl;
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            try
            {
                string idsCollection = base.Request.Form["IDsCollection"];

                if (string.IsNullOrEmpty(idsCollection))
                {
                    base.FailMessage("操作失败，请选择需要显示的内容");
                    return;
                }

                if (this.isKeywordAdd)
                {
                    if (this.keywordID > 0)
                    {
                        List<string> idsList = TranslateUtils.StringCollectionToStringList(idsCollection);
                        int resourceID = 0;
                        foreach (string ids in idsList)
                        {
                            int nodeID = TranslateUtils.ToInt(ids.Split('_')[0]);
                            int contentID = TranslateUtils.ToInt(ids.Split('_')[1]);
                            ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeID);
                            string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeID);

                            ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);

                            KeywordResourceInfo resourceInfo = new KeywordResourceInfo();

                            resourceInfo.ResourceID = 0;
                            resourceInfo.PublishmentSystemID = base.PublishmentSystemID;
                            resourceInfo.KeywordID = this.keywordID;
                            resourceInfo.Title = contentInfo.Title;
                            resourceInfo.ImageUrl = contentInfo.GetExtendedAttribute(BackgroundContentAttribute.ImageUrl);
                            resourceInfo.Summary = MPUtils.GetSummary(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.Summary), contentInfo.GetExtendedAttribute(BackgroundContentAttribute.Content));
                            resourceInfo.ResourceType = EResourceType.Site;
                            resourceInfo.IsShowCoverPic = false;
                            resourceInfo.Content = contentInfo.GetExtendedAttribute(BackgroundContentAttribute.Content);
                            resourceInfo.NavigationUrl = string.Empty;
                            resourceInfo.ChannelID = contentInfo.NodeID;
                            resourceInfo.ContentID = contentInfo.ID;
                            resourceInfo.Taxis = 0;

                            int id = DataProviderWX.KeywordResourceDAO.Insert(resourceInfo);
                            if (resourceID == 0)
                            {
                                resourceID = id;
                            }
                        }

                        string redirectUrl = BackgroundKeywordNewsAdd.GetRedirectUrl(base.PublishmentSystemID, this.keywordID, resourceID, !this.isMultiple);
                        JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, redirectUrl);
                    }
                }
                else
                {
                    string scripts = string.Empty;
                    if (this.isMultiple)
                    {
                        StringBuilder titleBuilder = new StringBuilder();
                        List<string> idsList = TranslateUtils.StringCollectionToStringList(idsCollection);
                        foreach (string ids in idsList)
                        {
                            int nodeID = TranslateUtils.ToInt(ids.Split('_')[0]);
                            int contentID = TranslateUtils.ToInt(ids.Split('_')[1]);
                            ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeID);
                            string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeID);

                            ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);

                            titleBuilder.AppendFormat("{0}&nbsp;<a href='{1}' target='blank'>查看</a><br />", contentInfo.Title, PageUtilityWX.GetContentUrl(base.PublishmentSystemInfo, contentInfo));
                        }
                        scripts = string.Format(@"window.parent.{0}(""{1}"", ""{2}"");", this.jsMethod, idsCollection, titleBuilder.ToString());
                    }
                    else
                    {
                        int nodeID = TranslateUtils.ToInt(idsCollection.Split('_')[0]);
                        int contentID = TranslateUtils.ToInt(idsCollection.Split('_')[1]);
                        ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeID);
                        string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeID);

                        ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);

                        string imageUrl = contentInfo.GetExtendedAttribute(BackgroundContentAttribute.ImageUrl);
                        string imageSrc = PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, imageUrl);
                        string summary = MPUtils.GetSummary(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.Summary), contentInfo.GetExtendedAttribute(BackgroundContentAttribute.Content));

                        string pageUrl = PageUtilityWX.GetContentUrl(base.PublishmentSystemInfo, contentInfo);
                        scripts = string.Format(@"window.parent.{0}(""{1}"", ""{2}"", ""{3}"", ""{4}"", ""{5}"", ""{6}"", ""{7}"");", this.jsMethod, contentInfo.Title, nodeID, contentID, pageUrl, imageUrl, imageSrc, summary);

                        if (base.Request.QueryString["itemIndex"] != null)
                        {
                            scripts = string.Format(@"window.parent.{0}({1}, ""{2}"", {3}, {4});", this.jsMethod, this.itemIndex, contentInfo.Title, nodeID, contentID);
                        }
                    }

                    JsUtils.OpenWindow.CloseModalPageWithoutRefresh(base.Page, scripts);
                }
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "失败：" + ex.Message);
            }
        }
    }
}
