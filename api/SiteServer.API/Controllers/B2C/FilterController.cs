using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.STL.Parser;
using SiteServer.STL.Parser.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Xml;
using SiteServer.API.Model.B2C;
using SiteServer.STL.Parser.StlElement;

namespace SiteServer.API.Controllers.B2C
{
    public class FilterController : ApiController
    {
        [HttpPost]
        [ActionName("PostResults")]
        public IHttpActionResult PostResults(RequestFilterParameter request)
        {
            PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
            if (publishmentSystemInfo != null)
            {
                int channelID = TranslateUtils.ToInt(HttpContext.Current.Request.QueryString["channelID"]);

                #region 全局搜索
                int type = 0;
                if (!string.IsNullOrEmpty(request.IsRedirect) && !TranslateUtils.ToBool(request.IsRedirect, false))
                {
                    //根据关键字，查找到相关 栏目/品牌/内容 信息
                    ArrayList array = GetChannelInfoByFilterSearchWords(request.FilterSearchWords, 10, out type);
                    return Ok(array);
                }
                else if (!string.IsNullOrEmpty(request.FilterSearchWords))
                {
                    //需要根据FilterSearchWords，确定channelID
                    int channelIDByKeywords = GetChannelIDByFilterSearchWords(request.FilterSearchWords, out type);
                    if (type == 1)
                    {
                        request.FilterSearchWords = string.Empty;
                        channelID = channelIDByKeywords;
                    }
                    else if (type == 2)
                    {
                        request.FilterSearchWords = string.Empty;
                        channelID = channelIDByKeywords;
                    }
                    else if (type == 3)
                    {
                        if (channelID == 0)
                            channelID = channelIDByKeywords;
                    }
                }
                #endregion

                NameValueCollection filterMap = TranslateUtils.ToNameValueCollection(request.Filters, '&');
                int page = TranslateUtils.ToInt(request.Page, 1);
                if (page <= 0)
                {
                    page = 1;
                }


                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, channelID);
                if (nodeInfo == null)
                {
                    FilterParameter error = new FilterParameter { Filters = new List<FilterInfo>(), Order = request.Order, HasGoods = TranslateUtils.ToBool(request.HasGoods), IsCOD = TranslateUtils.ToBool(request.IsCOD), Contents = new List<Dictionary<string, string>>(), PageItem = new PageItem { }, ChannelName = string.Empty };
                    return Ok(error);
                }
                string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);

                PageInfo pageInfo = new PageInfo(null, publishmentSystemInfo.PublishmentSystemID, channelID, 0, publishmentSystemInfo, publishmentSystemInfo.Additional.VisualType);
                ContextInfo contextInfo = new ContextInfo(pageInfo);

                XmlDocument xmlDocument = StlParserUtility.GetXmlDocument(request.StlPageContents, false);
                XmlNode node = xmlDocument.DocumentElement;
                node = node.FirstChild;

                ContentsDisplayInfo displayInfo = SiteServer.STL.Parser.Model.ContentsDisplayInfo.GetContentsDisplayInfoByXmlNode(node, pageInfo, contextInfo, EContextType.Content);

                List<FilterInfo> filterInfoList = FilterManager.GetFilterInfoList(publishmentSystemInfo, nodeInfo);

                string orderByString = "ORDER BY Taxis DESC";
                if (StringUtils.EqualsIgnoreCase(request.Order, ContentAttribute.AddDate))
                {
                    orderByString = "ORDER BY AddDate DESC";
                }
                else if (StringUtils.EqualsIgnoreCase(request.Order, GoodsContentAttribute.PriceSale))
                {
                    orderByString = "ORDER BY PriceSale";
                }
                //else if (StringUtils.EqualsIgnoreCase(order, "price desc"))
                //{
                //    orderByString = "ORDER BY PriceSale DESC";
                //}
                else if (StringUtils.EqualsIgnoreCase(request.Order, GoodsContentAttribute.Sales))
                {
                    orderByString = "ORDER BY Sales DESC";
                }
                else if (StringUtils.EqualsIgnoreCase(request.Order, ContentAttribute.Comments))
                {
                    orderByString = "ORDER BY Comments DESC";
                }

                StringBuilder whereBuilder = new StringBuilder();
                foreach (FilterInfo filterInfo in filterInfoList)
                {
                    if (filterMap[filterInfo.FilterID.ToString()] != null)
                    {
                        int itemID = TranslateUtils.ToInt(filterMap[filterInfo.FilterID.ToString()]);
                        if (itemID > 0)
                        {
                            List<FilterItemInfo> itemArrayList = null;
                            if (filterInfo.IsDefaultValues)
                            {
                                itemArrayList = FilterManager.GetDefaultFilterItemInfoList(publishmentSystemInfo, channelID, filterInfo.FilterID, filterInfo.AttributeName);
                            }
                            else
                            {
                                itemArrayList = DataProviderB2C.FilterItemDAO.GetFilterItemInfoList(filterInfo.FilterID);
                            }
                            foreach (FilterItemInfo itemInfo in itemArrayList)
                            {
                                if (itemInfo.ItemID == itemID)
                                {
                                    if (StringUtils.EqualsIgnoreCase(filterInfo.AttributeName, GoodsContentAttribute.BrandID))
                                    {

                                        int brandID = TranslateUtils.ToInt(itemInfo.Value);
                                        if (brandID > 0)
                                        {
                                            whereBuilder.AppendFormat("({0} = {1}) AND ", GoodsContentAttribute.BrandID, brandID);
                                        }
                                        else if (!string.IsNullOrEmpty(itemInfo.Value))
                                        {
                                            whereBuilder.AppendFormat("({0} = '{1}') AND ", GoodsContentAttribute.BrandValue, itemInfo.Value);
                                        }
                                    }
                                    else if (StringUtils.EqualsIgnoreCase(filterInfo.AttributeName, GoodsContentAttribute.PriceSale))
                                    {
                                        string prices = itemInfo.Value;
                                        if (!string.IsNullOrEmpty(prices) && (prices == "0" || prices.IndexOf('-') > -1))
                                        {
                                            if (prices == "0")
                                            {
                                                whereBuilder.AppendFormat("({0} = 0) AND ", GoodsContentAttribute.PriceSale);
                                            }
                                            else
                                            {
                                                string[] priceArr = prices.Split('-');
                                                int start = TranslateUtils.ToInt(priceArr[0]);
                                                int end = TranslateUtils.ToInt(priceArr[1]);
                                                whereBuilder.AppendFormat("({0} BETWEEN {1} AND {2}) AND ", GoodsContentAttribute.PriceSale, start, end);
                                            }
                                        }
                                    }
                                    else if (StringUtils.StartsWithIgnoreCase(filterInfo.AttributeName, GoodsContentAttribute.PREFIX_Spec))
                                    {
                                        int specItemID = TranslateUtils.ToInt(itemInfo.Value);
                                        if (specItemID > 0)
                                        {
                                            whereBuilder.AppendFormat("({0} = '{1}' OR {0} LIKE '{1},%' OR {0} LIKE '%,{1},%' OR {0} LIKE '%,{1}') AND ", GoodsContentAttribute.SpecItemIDCollection, specItemID);
                                        }
                                    }
                                    else if (GoodsContentAttribute.IsExtendAttribute(filterInfo.AttributeName))
                                    {
                                        if (!string.IsNullOrEmpty(itemInfo.Value))
                                        {
                                            whereBuilder.AppendFormat("({0} = '{1}' OR {0} LIKE '{1},%' OR {0} LIKE '%,{1},%' OR {0} LIKE '%,{1}') AND ", filterInfo.AttributeName, itemInfo.Value);
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(request.Keywords))
                {
                    whereBuilder.AppendFormat("({0} LIKE '%{1}%' OR {2} LIKE '%{1}%' OR {3} LIKE '%{1}%' OR {4} LIKE '%{1}%' OR {5} LIKE '%{1}%' OR {6} LIKE '%{1}%') AND ", ContentAttribute.Title, request.Keywords, GoodsContentAttribute.SN, GoodsContentAttribute.F1, GoodsContentAttribute.F2, GoodsContentAttribute.F3, GoodsContentAttribute.Summary, GoodsContentAttribute.Keywords);
                }
                if (!string.IsNullOrEmpty(request.FilterSearchWords))
                {
                    whereBuilder.AppendFormat("({0} LIKE '%{1}%' OR {2} LIKE '%{1}%' OR {3} LIKE '%{1}%' OR {4} LIKE '%{1}%' OR {5} LIKE '%{1}%') AND ", ContentAttribute.Title, request.FilterSearchWords, GoodsContentAttribute.SN, GoodsContentAttribute.F1, GoodsContentAttribute.F2, GoodsContentAttribute.F3, GoodsContentAttribute.Summary);
                }
                if (!string.IsNullOrEmpty(request.HasGoods) && TranslateUtils.ToBool(request.HasGoods))
                {
                    whereBuilder.AppendFormat(" (stock = -1 OR stock > 0) AND");//不限制商品数量或者库存数量大于0
                }
                if (!string.IsNullOrEmpty(request.IsCOD) && TranslateUtils.ToBool(request.IsCOD))
                {
                    //支持货到付款，待开发
                }

                if (whereBuilder.Length > 0)
                {
                    whereBuilder.Length -= 4;
                    whereBuilder.Insert(0, " AND ");
                }

                int pageNum = displayInfo.PageNum;
                int startNum = (page - 1) * pageNum + 1;

                ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(channelID, EScopeType.All, string.Empty, string.Empty);
                IEnumerable dataSource = BaiRongDataProvider.ContentDAO.GetStlDataSourceChecked(tableName, nodeIDArrayList, startNum, pageNum, orderByString, whereBuilder.ToString(), false, null);


                List<Dictionary<string, string>> contents = new List<Dictionary<string, string>>();
                if (dataSource != null)
                {
                    foreach (var item in dataSource)
                    {
                        GoodsContentInfo contentInfo = new GoodsContentInfo(item);
                        contentInfo.SetExtendedAttribute("navigationUrl", APIPageUtils.ParseUrl(PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo, publishmentSystemInfo.Additional.VisualType)));
                        contentInfo.ThumbUrl = APIPageUtils.ParseUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, contentInfo.ThumbUrl));
                        contentInfo.ImageUrl = APIPageUtils.ParseUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, contentInfo.ImageUrl));
                        //选择商品的第一个规格
                        List<GoodsInfo> goodsInfoListInDB = DataProviderB2C.GoodsDAO.GetGoodsInfoList(publishmentSystemInfo.PublishmentSystemID, contentInfo.ID);
                        GoodsInfo firstGoods = new GoodsInfo();
                        if (goodsInfoListInDB.Count > 0)
                            firstGoods = goodsInfoListInDB[0];

                        Dictionary<string, string> content = new Dictionary<string, string>();
                        foreach (string name in contentInfo.Attributes.Keys)
                        {
                            content[name] = contentInfo.GetExtendedAttribute(name);
                        }
                        foreach (string name in GoodsContentAttribute.AllAttributes)
                        {
                            content[name] = contentInfo.GetExtendedAttribute(name);
                        }
                        content["firstGoodID"] = firstGoods.GoodsID.ToString();

                        if (publishmentSystemInfo.PublishmentSystemType == EPublishmentSystemType.B2C)
                        {
                            int goodCount, middelCount, badCount, totalCount = 0;
                            DataProviderB2C.OrderItemCommentDAO.GetOrderItemCommentStatistic(contentInfo.ID, out goodCount, out middelCount, out badCount, out totalCount);
                            content["comments"] = totalCount.ToString();
                            content["goodComments"] = goodCount.ToString();
                            content["middleComments"] = middelCount.ToString();
                            content["badComments"] = badCount.ToString();
                            if (totalCount > 0)
                            {
                                content["goodPercent"] = (goodCount / totalCount * 100).ToString();
                                content["middlePercent"] = (middelCount / totalCount * 100).ToString();
                                content["badPercent"] = (badCount / totalCount * 100).ToString();
                            }
                            else
                            {
                                content["goodPercent"] = "100";
                                content["middlePercent"] = "100";
                                content["badPercent"] = "100";
                            }
                        }

                        contents.Add(content);
                    }
                }

                int currentPageIndex = page;
                int totalNum = BaiRongDataProvider.ContentDAO.GetStlCountChecked(tableName, nodeIDArrayList, whereBuilder.ToString());
                int totalPageNum = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(totalNum) / Convert.ToDouble(pageNum)));
                bool previousPage = page > 1;
                bool nextPage = totalPageNum > page;
                bool firstPage = page > 1;
                bool lastPage = totalPageNum > page;
                int previousPageIndex = currentPageIndex - 1;
                int nextPageIndex = currentPageIndex + 1;
                if (!previousPage)
                    previousPageIndex = 0;
                if (!nextPage)
                    nextPageIndex = 0;
                if (totalPageNum == 0)
                    currentPageIndex = 0;

                //页导航
                int startPageNum = 0;
                int endPageNum = 0;

                if (currentPageIndex > 0 && currentPageIndex <= 5)
                {
                    startPageNum = 1;
                    endPageNum = totalPageNum > 10 ? 10 : totalPageNum;
                }
                else if (currentPageIndex > 5 && currentPageIndex < totalPageNum - 5)
                {
                    startPageNum = currentPageIndex - 5 + 1;
                    endPageNum = currentPageIndex + 5;
                }
                else
                {
                    startPageNum = totalPageNum - 10 + 1;
                    endPageNum = totalPageNum;
                }

                if (startPageNum < 0)
                    startPageNum = 0;
                if (endPageNum < 0)
                    endPageNum = 0;
                List<int> pageNavigation = new List<int>();
                for (int i = startPageNum; i <= endPageNum; i++)
                {
                    pageNavigation.Add(i);
                }

                PageItem pageItem = new PageItem { CurrentPageIndex = currentPageIndex, TotalNum = totalNum, TotalPageNum = totalPageNum, PreviousPage = previousPage, NextPage = nextPage, FirstPage = firstPage, LastPage = lastPage, PreviousPageIndex = previousPageIndex, NextPageIndex = nextPageIndex, PageNavigation = pageNavigation };

                FilterParameter retval = new FilterParameter { Filters = filterInfoList, Order = request.Order, HasGoods = TranslateUtils.ToBool(request.HasGoods), IsCOD = TranslateUtils.ToBool(request.IsCOD), Contents = contents, PageItem = pageItem, ChannelName = nodeInfo.NodeName };

                return Ok(retval);
            }
            return Ok();
        }

        /// <summary>
        /// 通过全局搜索关键字，确定channelID
        /// 1. 首先查找栏目，有，返回channelID
        /// 2. 没有匹配到栏目则去查找品牌，有，返回品牌对应的channelID
        /// 3. 没有匹配到品牌则去查找内容，有，返回内容对应的channelID
        /// </summary>
        /// <param name="filterSearchWords"></param>
        /// <returns></returns>
        private int GetChannelIDByFilterSearchWords(string filterSearchWords, out int type)
        {
            ArrayList array = new ArrayList();
            int channelID = 0;
            type = 0;//栏目，品牌，内容都没有相关内容

            #region 查找栏目
            array = DataProvider.NodeDAO.GetNodeInfoByNodeIndexOrNodeName(RequestUtils.PublishmentSystemID, filterSearchWords, 1);
            if (array.Count > 0)
            {
                channelID = ((NodeInfo)array[0]).NodeID;
                type = 1;//匹配到栏目
            }
            #endregion

            #region 查找品牌
            if (type == 0)
            {
                array = DataProvider.NodeDAO.GetNodeInfoByBrandName(RequestUtils.PublishmentSystemID, filterSearchWords, 1);
                if (array.Count > 0)
                {
                    channelID = ((NodeInfo)array[0]).NodeID;
                    type = 2;//匹配到品牌
                }
            }
            #endregion

            #region 查找内容
            if (type == 0)
            {
                array = DataProvider.NodeDAO.GetNodeInfoByConentName(RequestUtils.PublishmentSystemID, filterSearchWords, 1);
                if (array.Count > 0)
                {
                    channelID = ((NodeInfo)array[0]).NodeID;
                    type = 3;//匹配到内容
                }
            }
            #endregion

            return channelID;
        }

        /// <summary>
        /// 通过全局搜索关键字，确定N条内容
        /// 1. 首先查找栏目，有，返回栏目
        /// 2. 没有匹配到栏目则去查找品牌，有，返回品牌对应的栏目
        /// 3. 没有匹配到品牌则去查找内容，有，返回内容对应的栏目
        /// </summary>
        /// <param name="filterSearchWords"></param>
        /// <returns></returns>
        private ArrayList GetChannelInfoByFilterSearchWords(string filterSearchWords, int count, out int type)
        {
            ArrayList array = new ArrayList();
            type = 0;//栏目，品牌，内容都没有相关内容

            #region 查找栏目
            array = DataProvider.NodeDAO.GetNodeInfoByNodeIndexOrNodeName(RequestUtils.PublishmentSystemID, filterSearchWords, count);
            if (array.Count > 0)
            {
                type = 1;//匹配到栏目
            }
            #endregion

            #region 查找品牌
            if (type == 0)
            {
                array = DataProvider.NodeDAO.GetNodeInfoByBrandName(RequestUtils.PublishmentSystemID, filterSearchWords, count);
                if (array.Count > 0)
                {
                    type = 2;//匹配到品牌
                }
            }
            #endregion

            #region 查找内容
            if (type == 0)
            {
                array = DataProvider.NodeDAO.GetNodeInfoByConentName(RequestUtils.PublishmentSystemID, filterSearchWords, count);
                if (array.Count > 0)
                {
                    type = 3;//匹配到内容
                }
            }
            #endregion

            return array;
        }
    }
}
