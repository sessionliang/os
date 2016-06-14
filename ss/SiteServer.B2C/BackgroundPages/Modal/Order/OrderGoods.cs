using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using System.Collections;
using SiteServer.B2C.Model;
using SiteServer.B2C.Core;
using BaiRong.Model;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;

using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Core.Security;

namespace SiteServer.B2C.BackgroundPages.Modal
{
    public class OrderGoods : BackgroundBasePage
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

        public static string GetOpenWindowString(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            return PageUtilityB2C.GetOpenWindowString("选择订单商品", "modal_orderGoods.aspx", arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            int nodeID = base.GetIntQueryString("NodeID");
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
                this.spContents.SelectCommand = DataProvider.ContentDAO.GetSelectCommend(tableStyle, tableName, base.PublishmentSystemID, this.nodeInfo.NodeID, PermissionsManager.Current.IsSystemAdministrator, ProductPermissionsManager.Current.OwningNodeIDArrayList, base.GetQueryString("SearchType"), base.GetQueryString("Keyword"), base.GetQueryString("DateFrom"), base.GetQueryString("DateTo"), true, ETriState.True, !base.GetBoolQueryString("IsDuplicate"), true);
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
                    this.IsDuplicate.Checked = base.GetBoolQueryString("IsDuplicate");
                    ControlUtils.SelectListItems(this.SearchType, base.GetQueryString("SearchType"));
                    this.Keyword.Text = base.GetQueryString("Keyword");
                    this.DateFrom.Text = base.GetQueryString("DateFrom");
                    this.DateTo.Text = base.GetQueryString("DateTo");
                }

                this.spContents.DataBind();
            }
        }

        private readonly Hashtable valueHashtable = new Hashtable();
        private Dictionary<int, SpecComboInfo> comboDictionary = null;
        private List<int> specIDList = null;
        private GoodsContentInfo contentInfo = null;

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlChannel = e.Item.FindControl("ltlChannel") as Literal;
                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                Repeater rptGoods = e.Item.FindControl("rptGoods") as Repeater;

                this.contentInfo = new GoodsContentInfo(e.Item.DataItem);

                string nodeName = valueHashtable[contentInfo.NodeID] as string;
                if (nodeName == null)
                {
                    nodeName = NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, contentInfo.NodeID);
                    valueHashtable[contentInfo.NodeID] = nodeName;
                }
                ltlChannel.Text = nodeName;

                ltlTitle.Text = WebUtils.GetContentTitle(base.PublishmentSystemInfo, contentInfo, this.PageUrl);

                
                rptGoods.DataSource = SpecManager.GetGoodsInfoList(base.PublishmentSystemID, NodeManager.GetNodeInfo(base.PublishmentSystemID, contentInfo.NodeID), contentInfo.ID, contentInfo.GetExtendedAttribute(GoodsContentAttribute.SN), out this.comboDictionary, out this.specIDList);
                rptGoods.ItemDataBound += rptGoods_ItemDataBound;
                rptGoods.DataBind();

                //

                //rptGoods.DataSource = DataProvider.GoodsDAO.GetItemIDCollectionHashtable(
            }
        }

        void rptGoods_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlGoodsSN = e.Item.FindControl("ltlGoodsSN") as Literal;
                Literal ltlGoodsSpec = e.Item.FindControl("ltlGoodsSpec") as Literal;
                Literal ltlPrice = e.Item.FindControl("ltlPrice") as Literal;
                Literal ltlSelect = e.Item.FindControl("ltlSelect") as Literal;

                GoodsInfo goodsInfo = e.Item.DataItem as GoodsInfo;
                if (!GoodsManager.IsOnSale(this.contentInfo, goodsInfo))
                {
                    e.Item.Visible = false;
                    return;
                }
                StringBuilder builder = new StringBuilder();
                List<int> comboIDList = TranslateUtils.StringCollectionToIntList(goodsInfo.ComboIDCollection);
                foreach (int specID in this.specIDList)
                {
                    foreach (int comboID in comboIDList)
                    {
                        SpecComboInfo comboInfo = this.comboDictionary[comboID] as SpecComboInfo;
                        if (comboInfo != null && comboInfo.SpecID == specID)
                        {
                            builder.Append(SpecManager.GetSpecValue(base.PublishmentSystemInfo, comboInfo));
                        }
                    }
                }

                ltlGoodsSN.Text = goodsInfo.GoodsSN;
                ltlGoodsSpec.Text = builder.ToString();
                ltlPrice.Text = PriceManager.GetDisplayMoney(PriceManager.GetPrice(this.contentInfo, goodsInfo));
                ltlSelect.Text = string.Format(@"<input type=""checkbox"" name=""IDCollection"" value=""{0}"" />", goodsInfo.GoodsID);
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
            if (!string.IsNullOrEmpty(base.Request.Form["IDCollection"]))
            {
                List<int> goodsIDList = TranslateUtils.StringCollectionToIntList(base.Request.Form["IDCollection"]);
                if (goodsIDList.Count > 0)
                {
                    List<GoodsInfo> goodsInfoList = new List<GoodsInfo>();
                    StringBuilder builder = new StringBuilder();
                    foreach (int goodsID in goodsIDList)
                    {
                        GoodsInfo goodsInfo = DataProviderB2C.GoodsDAO.GetGoodsInfo(goodsID);
                        if (goodsInfo != null)
                        {
                            GoodsContentInfo contentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(this.tableName, goodsInfo.ContentID);
                            if (contentInfo != null)
                            {
                                string spec = SpecManager.GetSpecValues(base.PublishmentSystemInfo, goodsInfo);
                                builder.AppendFormat(@"{{goodsID: {0}, channelID : {1}, contentID : {2}, title: '{3}', price: {4}, purchaseNum: 1, goodsSN: '{5}', spec: '{6}'}},", goodsInfo.GoodsID, contentInfo.NodeID, contentInfo.ID, StringUtils.ToJsString(contentInfo.Title), PriceManager.GetPrice(contentInfo, goodsInfo), StringUtils.ToJsString(goodsInfo.GoodsSN), spec);
                            }
                        }
                    }
                    string scripts = string.Empty;
                    if (builder.Length > 0)
                    {
                        builder.Length -= 1;
                        scripts = string.Format(@"parent.addGoods([{0}]);", builder.ToString());
                    }
                    JsUtils.OpenWindow.CloseModalPageWithoutRefresh(base.Page, scripts);
                }
                else
                {
                    JsUtils.OpenWindow.CloseModalPageWithoutRefresh(base.Page);
                }
            }
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    this._pageUrl = string.Format("orderGoods.aspx?PublishmentSystemID={0}&NodeID={1}&IsDuplicate={2}&SearchType={3}&Keyword={4}&DateFrom={5}&DateTo={6}", base.PublishmentSystemID, this.NodeIDDropDownList.SelectedValue, this.IsDuplicate.Checked, this.SearchType.SelectedValue, this.Keyword.Text, this.DateFrom.Text, this.DateTo.Text);
                }
                return this._pageUrl;
            }
        }
    }
}
