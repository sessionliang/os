using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using System.Collections.Specialized;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using System.Collections.Generic;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.B2C.BackgroundPages.Modal
{
    public class FilterAdd : BackgroundBasePage
    {
        protected DropDownList ddlAttributeName;
        protected TextBox tbFilterName;

        private int nodeID;
        private int filterID;

        private NodeInfo nodeInfo;
        private string tableName;
        private ArrayList relatedIdentities;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID, int nodeID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            return PageUtilityB2C.GetOpenWindowString("添加筛选属性", "modal_filterAdd.aspx", arguments, 400, 300);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int nodeID, int filterID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            arguments.Add("FilterID", filterID.ToString());
            return PageUtilityB2C.GetOpenWindowString("编辑筛选属性", "modal_filterAdd.aspx", arguments, 400, 300);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.nodeID = base.GetIntQueryString("NodeID");
            this.filterID = base.GetIntQueryString("FilterID");

            this.tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeID);
            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, nodeID);
            this.nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);

            if (!IsPostBack)
            {
                if (this.filterID == 0)
                {
                    List<string> attributeNameList = DataProviderB2C.FilterDAO.GetAttributeNameList(base.PublishmentSystemID, this.nodeID);

                    string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, this.nodeID);
                    ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, this.nodeID);
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);

                    //添加品牌
                    ListItem listItem = new ListItem(FilterManager.GetFilterFullName(tableName, relatedIdentities, nodeInfo, GoodsContentAttribute.BrandID), GoodsContentAttribute.BrandID);
                    if (!attributeNameList.Contains(listItem.Value))
                    {
                        this.ddlAttributeName.Items.Add(listItem);
                    }
                    //添加辅助字段
                    ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.GoodsContent, tableName, relatedIdentities);
                    foreach (TableStyleInfo styleInfo in tableStyleInfoArrayList)
                    {
                        if (styleInfo.IsVisible)
                        {
                            if (GoodsContentAttribute.IsExtendAttribute(styleInfo.AttributeName))
                            {
                                listItem = new ListItem(string.Format("{0}({1})", styleInfo.DisplayName, styleInfo.AttributeName), styleInfo.AttributeName);
                                if (!attributeNameList.Contains(listItem.Value))
                                {
                                    this.ddlAttributeName.Items.Add(listItem);
                                }
                            }
                        }
                    }
                    //添加规格

                    List<int> specList = DataProviderB2C.SpecDAO.GetSpecIDList(nodeInfo.PublishmentSystemID, this.nodeID);
                    int index = 0;
                    foreach (int specID in specList)
                    {
                        SpecInfo specInfo = SpecManager.GetSpecInfo(nodeInfo.PublishmentSystemID, specID);
                        if (specInfo != null)
                        {
                            string attributeName = GoodsContentAttribute.PREFIX_Spec + ++index;
                            listItem = new ListItem(string.Format("{0}({1})", specInfo.SpecName, attributeName), attributeName);
                            if (!attributeNameList.Contains(listItem.Value))
                            {
                                this.ddlAttributeName.Items.Add(listItem);
                            }
                        }
                    }
                    //添加价格
                    listItem = new ListItem(FilterManager.GetFilterFullName(tableName, relatedIdentities, nodeInfo, GoodsContentAttribute.PriceSale), GoodsContentAttribute.PriceSale);
                    if (!attributeNameList.Contains(listItem.Value))
                    {
                        this.ddlAttributeName.Items.Add(listItem);
                    }
                }
                else
                {
                    FilterInfo filterInfo = DataProviderB2C.FilterDAO.GetFilterInfo(this.filterID);
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);
                    string filterFullName = FilterManager.GetFilterFullName(base.PublishmentSystemInfo, nodeInfo, filterInfo);
                    ListItem listItem = new ListItem(filterFullName, filterInfo.AttributeName);
                    this.ddlAttributeName.Items.Add(listItem);
                    this.ddlAttributeName.Enabled = false;

                    this.tbFilterName.Text = filterInfo.FilterName;
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            //选择的帅选属性
            string selectAttr = this.ddlAttributeName.SelectedValue;

            string displayName = this.tbFilterName.Text;
            if (string.IsNullOrEmpty(displayName))
            {
                displayName = FilterManager.GetFilterName(this.tableName, this.relatedIdentities, this.nodeInfo, selectAttr, displayName);
            }

            if (this.filterID == 0)
            {
                List<string> attributeNameList = DataProviderB2C.FilterDAO.GetAttributeNameList(base.PublishmentSystemID, this.nodeID);

                if (attributeNameList.IndexOf(this.ddlAttributeName.SelectedValue) != -1)
                {
                    base.FailMessage("筛选属性添加失败，筛选属性名称已存在！");
                }
                else
                {
                    try
                    {
                        FilterInfo filterInfo = new FilterInfo(0, base.PublishmentSystemID, this.nodeID, this.ddlAttributeName.SelectedValue, displayName, true, 0);

                        DataProviderB2C.FilterDAO.Insert(filterInfo);

                        isChanged = true;
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "筛选属性添加失败！");
                    }
                }
            }
            else
            {
                try
                {
                    FilterInfo filterInfo = DataProviderB2C.FilterDAO.GetFilterInfo(this.filterID);
                    filterInfo.FilterName = displayName;

                    DataProviderB2C.FilterDAO.Update(filterInfo);

                    isChanged = true;
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "筛选属性修改失败！");
                }
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }
    }
}
