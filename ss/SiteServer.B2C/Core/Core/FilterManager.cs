using System.Web.UI;
using BaiRong.Core;
using System.Web.UI.WebControls;
using BaiRong.Model;
using System.Collections;

using SiteServer.B2C.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;
using System.Text;
using System.Collections.Generic;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.B2C.Core
{
	public class FilterManager
	{
        public static string GetFilterName(string tableName, ArrayList relatedIdentities, NodeInfo nodeInfo, string attributeName, string filterName)
        {
            if (!string.IsNullOrEmpty(filterName))
            {
                return filterName;
            }
            if (StringUtils.EqualsIgnoreCase(attributeName, GoodsContentAttribute.BrandID))
            {
                return "品牌";
            }
            else if (StringUtils.EqualsIgnoreCase(attributeName, GoodsContentAttribute.PriceSale))
            {
                return "价格区间";
            }
            else if (StringUtils.StartsWithIgnoreCase(attributeName, GoodsContentAttribute.PREFIX_Spec))
            {
                if (nodeInfo != null)
                {
                    List<int> list = DataProviderB2C.SpecDAO.GetSpecIDList(nodeInfo.PublishmentSystemID, nodeInfo.NodeID);
                    int index = TranslateUtils.ToInt(StringUtils.ReplaceIgnoreCase(attributeName, GoodsContentAttribute.PREFIX_Spec, string.Empty));
                    if (list.Count > 0 && index > 0 && list.Count >= index)
                    {
                        SpecInfo specInfo = SpecManager.GetSpecInfo(nodeInfo.PublishmentSystemID, list[index - 1]);
                        if (specInfo != null)
                        {
                            return specInfo.SpecName;
                        }
                    }
                }
            }
            else if (GoodsContentAttribute.IsExtendAttribute(attributeName))
            {
                TableStyleInfo tableStyleInfo = TableStyleManager.GetTableStyleInfo(ETableStyle.GoodsContent, tableName, attributeName, relatedIdentities);
                return tableStyleInfo.DisplayName;
            }
            return string.Empty;
        }

        public static string GetFilterFullName(PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo, FilterInfo filterInfo)
        {
            string tableName = NodeManager.GetTableName(publishmentSystemInfo, filterInfo.NodeID);
            ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(publishmentSystemInfo.PublishmentSystemID, filterInfo.NodeID);
            return GetFilterFullName(tableName, relatedIdentities, nodeInfo, filterInfo.AttributeName);
        }

        public static string GetFilterFullName(string tableName, ArrayList relatedIdentities, NodeInfo nodeInfo, string attributeName)
        {
            if (GoodsContentAttribute.IsExtendAttribute(attributeName))
            {
                TableStyleInfo tableStyleInfo = TableStyleManager.GetTableStyleInfo(ETableStyle.GoodsContent, tableName, attributeName, relatedIdentities);
                return string.Format("{0}({1})", tableStyleInfo.DisplayName, attributeName);
            }
            else if (StringUtils.EqualsIgnoreCase(attributeName, GoodsContentAttribute.BrandID))
            {
                return string.Format("{0}({1})", FilterManager.GetFilterName(tableName, relatedIdentities, nodeInfo, GoodsContentAttribute.BrandID, string.Empty), GoodsContentAttribute.BrandID);
            }
            else if (StringUtils.EqualsIgnoreCase(attributeName, GoodsContentAttribute.PriceSale))
            {
                return string.Format("{0}({1})", FilterManager.GetFilterName(tableName, relatedIdentities, nodeInfo, GoodsContentAttribute.PriceSale, string.Empty), GoodsContentAttribute.PriceSale);
            }
            else if (StringUtils.StartsWithIgnoreCase(attributeName, GoodsContentAttribute.PREFIX_Spec))
            {
                if (nodeInfo != null)
                {
                    int index = TranslateUtils.ToInt(StringUtils.ReplaceIgnoreCase(attributeName, GoodsContentAttribute.PREFIX_Spec, string.Empty));
                    List<int> list = DataProviderB2C.SpecDAO.GetSpecIDList(nodeInfo.PublishmentSystemID, nodeInfo.NodeID);
                    if (list.Count > 0 && index > 0 && list.Count >= index)
                    {
                        SpecInfo specInfo = SpecManager.GetSpecInfo(nodeInfo.PublishmentSystemID, list[index - 1]);
                        if (specInfo != null)
                        {
                            return string.Format("{0}({1})", specInfo.SpecName, attributeName);
                        }
                    }
                }
            }
            return attributeName;
        }

        public static string GetFilterTitles(List<FilterItemInfo> arraylist)
        {
            StringBuilder builder = new StringBuilder();
            foreach (FilterItemInfo itemInfo in arraylist)
            {
                builder.Append(itemInfo.Title).Append(",");
            }
            if (builder.Length > 0) builder.Length -= 1;
            return builder.ToString();
        }

        public static string GetFilterValues(List<FilterItemInfo> itemInfoArrayList)
        {
            StringBuilder builder = new StringBuilder();
            foreach (FilterItemInfo itemInfo in itemInfoArrayList)
            {
                if (itemInfo.Title == itemInfo.Value)
                {
                    builder.AppendLine(itemInfo.Title);
                }
                else
                {
                    builder.AppendLine(string.Format("{0}|{1}", itemInfo.Title, itemInfo.Value));
                }
            }
            return builder.ToString();
        }

        public static List<string> GetItemValueList(PublishmentSystemInfo publishmentSystemInfo, FilterInfo filterInfo)
        {
            List<string> list = new List<string>();

            List<FilterItemInfo> itemInfoArrayList = null;

            if (filterInfo.IsDefaultValues)
            {
                itemInfoArrayList = FilterManager.GetDefaultFilterItemInfoList(publishmentSystemInfo, filterInfo.NodeID, filterInfo.FilterID, filterInfo.AttributeName);
            }
            else
            {
                itemInfoArrayList = DataProviderB2C.FilterItemDAO.GetFilterItemInfoList(filterInfo.FilterID);
            }

            StringBuilder builder = new StringBuilder();
            foreach (FilterItemInfo itemInfo in itemInfoArrayList)
            {
                list.Add(itemInfo.Value);
            }
            return list;
        }

        public static string GetItemValue(PublishmentSystemInfo publishmentSystemInfo, FilterInfo filterInfo, string filterValue)
        {
            string retval = string.Empty;

            List<string> valueList = FilterManager.GetItemValueList(publishmentSystemInfo, filterInfo);
            foreach (string value in valueList)
            {
                if (filterValue == value)
                {
                    return value;
                }
            }

            return retval;
        }

        public static List<FilterItemInfo> GetFilterItemInfoList(int filterID, string inputString)
        {
            List<FilterItemInfo> itemInfoArrayList = new List<FilterItemInfo>();
            if (!string.IsNullOrEmpty(inputString))
            {
                StringBuilder retVal = new StringBuilder();
                inputString = inputString.Trim();
                for (int i = 0; i < inputString.Length; i++)
                {
                    switch (inputString[i])
                    {
                        case '\n':
                            retVal.Append(",");
                            break;
                        case '\r':
                            break;
                        default:
                            retVal.Append(inputString[i]);
                            break;
                    }
                }
                ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(retVal.ToString());
                int id = 1;
                foreach (string item in arraylist)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        FilterItemInfo itemInfo = new FilterItemInfo(id++, filterID, string.Empty, string.Empty, 0);
                        if (item.IndexOf("|") != -1)
                        {
                            string[] items = item.Split('|');
                            itemInfo.Title = items[0];
                            itemInfo.Value = items[1];
                        }
                        else
                        {
                            itemInfo.Title = item;
                        }
                        if (string.IsNullOrEmpty(itemInfo.Value))
                        {
                            itemInfo.Value = itemInfo.Title;
                        }

                        itemInfoArrayList.Add(itemInfo);
                    }
                }
            }
            return itemInfoArrayList;
        }

        public static List<FilterItemInfo> GetDefaultFilterItemInfoList(PublishmentSystemInfo publishmentSystemInfo, int nodeID, int filterID, string attributeName)
        {
            List<FilterItemInfo> arraylist = new List<FilterItemInfo>();
            if (StringUtils.EqualsIgnoreCase(attributeName, GoodsContentAttribute.BrandID))
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                if (nodeInfo != null)
                {
                    bool isBrandSpecified = false;
                    if (nodeInfo.Additional.IsBrandSpecified && nodeInfo.Additional.BrandNodeID > 0)
                    {
                        NodeInfo brandNodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeInfo.Additional.BrandNodeID);
                        if (brandNodeInfo != null && EContentModelTypeUtils.Equals(EContentModelType.Brand, brandNodeInfo.ContentModelID))
                        {
                            isBrandSpecified = true;
                            ListItemCollection listItemCollection = DataProviderB2C.BrandContentDAO.GetListItemCollection(publishmentSystemInfo.PublishmentSystemID, nodeInfo.Additional.BrandNodeID, true);
                            foreach (ListItem listItem in listItemCollection)
                            {
                                FilterItemInfo itemInfo = new FilterItemInfo(TranslateUtils.ToInt(listItem.Value), filterID, listItem.Text, listItem.Value, 0);
                                arraylist.Add(itemInfo);
                            }
                        }
                    }
                    if (!isBrandSpecified)
                    {
                        //ArrayList brandInfoArrayList = DataProvider.BrandDAO.GetBrandInfoArrayList(publishmentSystemInfo.PublishmentSystemID);
                        //foreach (BrandInfo brandInfo in brandInfoArrayList)
                        //{
                        //    FilterItemInfo itemInfo = new FilterItemInfo(brandInfo.BrandID, filterID, brandInfo.BrandName, brandInfo.BrandID.ToString(), 0);
                        //    arraylist.Add(itemInfo);
                        //}
                    }
                }
            }
            else if (StringUtils.EqualsIgnoreCase(attributeName, GoodsContentAttribute.PriceSale))
            {
                arraylist = FilterManager.GetFilterItemInfoList(filterID, publishmentSystemInfo.Additional.ListPriceSales);
            }
            else if (StringUtils.StartsWithIgnoreCase(attributeName, GoodsContentAttribute.PREFIX_Spec))
            {
                int index = TranslateUtils.ToInt(StringUtils.ReplaceIgnoreCase(attributeName, GoodsContentAttribute.PREFIX_Spec, string.Empty));
                List<int> specIDList = DataProviderB2C.SpecDAO.GetSpecIDList(publishmentSystemInfo.PublishmentSystemID, nodeID);
                if (specIDList.Count > 0 && index > 0 && specIDList.Count >= index)
                {
                    int specID = specIDList[index - 1];
                    if (specID > 0)
                    {
                        List<SpecItemInfo> specItemInfoList = SpecItemManager.GetSpecItemInfoList(publishmentSystemInfo.PublishmentSystemID, specID);
                        foreach (SpecItemInfo specItemInfo in specItemInfoList)
                        {
                            FilterItemInfo filterItemInfo = new FilterItemInfo(specItemInfo.ItemID, filterID, specItemInfo.Title, specItemInfo.ItemID.ToString(), 0);
                            arraylist.Add(filterItemInfo);
                        }
                    }
                }
            }
            else if (GoodsContentAttribute.IsExtendAttribute(attributeName))
            {
                string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeID);
                ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(publishmentSystemInfo.PublishmentSystemID, nodeID);

                TableStyleInfo tableStyleInfo = TableStyleManager.GetTableStyleInfo(ETableStyle.GoodsContent, tableName, attributeName, relatedIdentities);
                if (tableStyleInfo != null && EInputTypeUtils.IsWithStyleItems(tableStyleInfo.InputType))
                {
                    ArrayList styleItemArrayList = TableStyleManager.GetStyleItemArrayList(tableStyleInfo.TableStyleID);
                    foreach (TableStyleItemInfo itemInfo in styleItemArrayList)
                    {
                        FilterItemInfo filterItemInfo = new FilterItemInfo(itemInfo.TableStyleItemID, filterID, itemInfo.ItemTitle, itemInfo.ItemValue, 0);
                        arraylist.Add(filterItemInfo);
                    }
                }
            }
            return arraylist;
        }

        public static List<FilterItemInfo> GetFilterItemInfoList(PublishmentSystemInfo publishmentSystemInfo, int nodeID, FilterInfo filterInfo)
        {
            List<FilterItemInfo> list = new List<FilterItemInfo>();

            if (filterInfo.IsDefaultValues)
            {
                list = FilterManager.GetDefaultFilterItemInfoList(publishmentSystemInfo, nodeID, filterInfo.FilterID, filterInfo.AttributeName);
            }
            else
            {
                list = DataProviderB2C.FilterItemDAO.GetFilterItemInfoList(filterInfo.FilterID);
            }

            return list;
        }

        public static string GetFilterValue(FilterInfo filterInfo, string filterValue)
        {
            string retval = string.Empty;

            if (filterInfo.IsDefaultValues)
            {

            }

            return retval;
        }

        public static List<FilterInfo> GetFilterInfoList(PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo)
        {
            List<FilterInfo> filterInfoList = DataProviderB2C.FilterDAO.GetFilterInfoList(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID);

            string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
            ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID);

            foreach (FilterInfo filterInfo in filterInfoList)
            {
                filterInfo.FilterName = FilterManager.GetFilterName(tableName, relatedIdentities, nodeInfo, filterInfo.AttributeName, filterInfo.FilterName);

                filterInfo.Items = FilterManager.GetFilterItemInfoList(publishmentSystemInfo, nodeInfo.NodeID, filterInfo);
            }

            return filterInfoList;
        }
	}
}
