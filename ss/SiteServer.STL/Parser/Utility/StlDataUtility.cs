using System.Collections;
using System.Data;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.STL.Parser.Model;
using System.Collections.Specialized;
using SiteServer.STL.Parser.StlElement;
using SiteServer.CMS.Core;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using System.Xml;
using System.Collections.Generic;
using System;

namespace SiteServer.STL.Parser
{
    public class StlDataUtility
    {
        public static int GetNodeIDByLevel(int publishmentSystemID, int nodeID, int upLevel, int topLevel)
        {
            int theNodeID = nodeID;
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
            if (nodeInfo != null)
            {
                if (topLevel >= 0)
                {
                    if (topLevel > 0)
                    {
                        if (topLevel < nodeInfo.ParentsCount)
                        {
                            ArrayList parentIDStrArrayList = TranslateUtils.StringCollectionToArrayList(nodeInfo.ParentsPath);
                            if (parentIDStrArrayList[topLevel] != null)
                            {
                                string parentIDStr = (string)parentIDStrArrayList[topLevel];
                                theNodeID = int.Parse(parentIDStr);
                            }
                        }
                    }
                    else
                    {
                        theNodeID = publishmentSystemID;
                    }
                }
                else if (upLevel > 0)
                {
                    if (upLevel < nodeInfo.ParentsCount)
                    {
                        ArrayList parentIDStrArrayList = TranslateUtils.StringCollectionToArrayList(nodeInfo.ParentsPath);
                        if (parentIDStrArrayList[upLevel] != null)
                        {
                            string parentIDStr = (string)parentIDStrArrayList[nodeInfo.ParentsCount - upLevel];
                            theNodeID = int.Parse(parentIDStr);
                        }
                    }
                    else
                    {
                        theNodeID = publishmentSystemID;
                    }
                }
            }
            return theNodeID;
        }

        public static ArrayList GetNodeIDArrayList(int publishmentSystemID, int channelID, string groupContent, string groupContentNot, string orderByString, EScopeType scopeType, string groupChannel, string groupChannelNot, bool isImageExists, bool isImage, int totalNum, string where)
        {
            string whereString = DataProvider.NodeDAO.GetWhereString(publishmentSystemID, groupContent, groupContentNot, isImageExists, isImage, where);
            return DataProvider.NodeDAO.GetNodeIDArrayList(channelID, totalNum, orderByString, whereString, scopeType, groupChannel, groupChannelNot);
        }

        //public static int GetNodeIDByChannelIDOrChannelIndexOrChannelName(int publishmentSystemID, int channelID, string channelIndex, string channelName)
        //{
        //    int retval = channelID;
        //    if (!string.IsNullOrEmpty(channelIndex))
        //    {
        //        int theNodeID = DataProvider.NodeDAO.GetNodeIDByNodeIndexName(publishmentSystemID, channelIndex);
        //        if (theNodeID != 0)
        //        {
        //            retval = theNodeID;
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(channelName))
        //    {
        //        int theNodeID = DataProvider.NodeDAO.GetNodeIDByParentIDAndNodeName(retval, channelName, true);
        //        if (theNodeID == 0)
        //        {
        //            theNodeID = DataProvider.NodeDAO.GetNodeIDByParentIDAndNodeName(publishmentSystemID, channelName, true);
        //        }
        //        if (theNodeID != 0)
        //        {
        //            retval = theNodeID;
        //        }
        //    }
        //    return retval;
        //}

        public static ETaxisType GetETaxisTypeByOrder(string order, bool isChannel, ETaxisType defaultType)
        {
            ETaxisType taxisType = defaultType;
            if (!string.IsNullOrEmpty(order))
            {
                if (isChannel)
                {
                    if (order.ToLower().Equals(StlParserUtility.Order_Default.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByTaxis;
                    }
                    else if (order.ToLower().Equals(StlParserUtility.Order_Back.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByTaxisDesc;
                    }
                    else if (order.ToLower().Equals(StlParserUtility.Order_AddDate.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByAddDate;
                    }
                    else if (order.ToLower().Equals(StlParserUtility.Order_AddDateBack.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByAddDateDesc;
                    }
                    else if (order.ToLower().Equals(StlParserUtility.Order_Hits.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByHits;
                    }
                }
                else
                {
                    if (order.ToLower().Equals(StlParserUtility.Order_Default.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByTaxisDesc;
                    }
                    else if (order.ToLower().Equals(StlParserUtility.Order_Back.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByTaxis;
                    }
                    else if (order.ToLower().Equals(StlParserUtility.Order_AddDate.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByAddDate;
                    }
                    else if (order.ToLower().Equals(StlParserUtility.Order_AddDateBack.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByAddDateDesc;
                    }
                    else if (order.ToLower().Equals(StlParserUtility.Order_LastEditDate.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByLastEditDate;
                    }
                    else if (order.ToLower().Equals(StlParserUtility.Order_AddDateBack.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByLastEditDateDesc;
                    }
                    else if (order.ToLower().Equals(StlParserUtility.Order_Hits.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByHits;
                    }
                    else if (order.ToLower().Equals(StlParserUtility.Order_HitsByDay.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByHitsByDay;
                    }
                    else if (order.ToLower().Equals(StlParserUtility.Order_HitsByWeek.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByHitsByWeek;
                    }
                    else if (order.ToLower().Equals(StlParserUtility.Order_HitsByMonth.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByHitsByMonth;
                    }
                }
            }
            return taxisType;
        }

        public static IEnumerable GetPropertysDataSource(PublishmentSystemInfo publishmentSystemInfo, ContentInfo contentInfo, EContextType contextType, string property, int startNum, int totalNum)
        {
            ArrayList arrayList = new ArrayList();
            List<string> propertyList = new List<string>();
            if (contextType == EContextType.Content)
            {
                //第一条
                propertyList.Add(contentInfo.GetExtendedAttribute(property));
                //第n条
                string extentAttributeName = ContentAttribute.GetExtendAttributeName(property);
                propertyList.AddRange(TranslateUtils.StringCollectionToStringList(contentInfo.GetExtendedAttribute(extentAttributeName)));

            }
            if (startNum > propertyList.Count)
            {
                startNum = propertyList.Count;
                totalNum = 1;
            }
            if (startNum + totalNum > propertyList.Count)
            {
                totalNum = propertyList.Count - startNum + 1;
            }
            if (totalNum == 0)
            {
                totalNum = propertyList.Count - startNum + 1;
            }
            for (int i = startNum; i < startNum + totalNum; i++)
            {
                contentInfo.SetExtendedAttribute(property, propertyList[i - 1]);
                ContentInfo item = contentInfo.Copy() as ContentInfo;
                arrayList.Add(item);
            }
            return arrayList;
        }

        public static string GetOrderByString(int publishmentSystemID, string orderValue, ETableStyle tableStyle, ETaxisType defaultType)
        {
            ETaxisType taxisType = defaultType;
            string orderByString = string.Empty;
            if (!string.IsNullOrEmpty(orderValue))
            {
                if (tableStyle == ETableStyle.Channel)
                {
                    if (orderValue.ToLower().Equals(StlParserUtility.Order_Default.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByTaxis;
                    }
                    else if (orderValue.ToLower().Equals(StlParserUtility.Order_Back.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByTaxisDesc;
                    }
                    else if (orderValue.ToLower().Equals(StlParserUtility.Order_AddDate.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByAddDateDesc;
                    }
                    else if (orderValue.ToLower().Equals(StlParserUtility.Order_AddDateBack.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByAddDate;
                    }
                    else if (orderValue.ToLower().Equals(StlParserUtility.Order_Hits.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByHits;
                    }
                    else if (orderValue.ToLower().Equals(StlParserUtility.Order_Random.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByRandom;
                    }
                    else
                    {
                        orderByString = orderValue;
                    }
                }
                else if (tableStyle == ETableStyle.BackgroundContent)
                {
                    if (orderValue.ToLower().Equals(StlParserUtility.Order_Default.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByTaxisDesc;
                    }
                    else if (orderValue.ToLower().Equals(StlParserUtility.Order_Back.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByTaxis;
                    }
                    else if (orderValue.ToLower().Equals(StlParserUtility.Order_AddDate.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByAddDateDesc;
                    }
                    else if (orderValue.ToLower().Equals(StlParserUtility.Order_AddDateBack.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByAddDate;
                    }
                    else if (orderValue.ToLower().Equals(StlParserUtility.Order_LastEditDate.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByLastEditDateDesc;
                    }
                    else if (orderValue.ToLower().Equals(StlParserUtility.Order_LastEditDateBack.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByLastEditDate;
                    }
                    else if (orderValue.ToLower().Equals(StlParserUtility.Order_Hits.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByHits;
                    }
                    else if (orderValue.ToLower().Equals(StlParserUtility.Order_HitsByDay.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByHitsByDay;
                    }
                    else if (orderValue.ToLower().Equals(StlParserUtility.Order_HitsByWeek.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByHitsByWeek;
                    }
                    else if (orderValue.ToLower().Equals(StlParserUtility.Order_HitsByMonth.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByHitsByMonth;
                    }
                    else if (orderValue.ToLower().Equals(StlParserUtility.Order_Stars.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByStars;
                    }
                    else if (orderValue.ToLower().Equals(StlParserUtility.Order_Digg.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByDigg;
                    }
                    else if (orderValue.ToLower().Equals(StlParserUtility.Order_Comments.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByComments;
                    }
                    else if (orderValue.ToLower().Equals(StlParserUtility.Order_Random.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByRandom;
                    }
                    else
                    {
                        orderByString = orderValue;
                    }
                }
                else if (tableStyle == ETableStyle.Comment)
                {
                    if (orderValue.ToLower().Equals(StlParserUtility.Order_Default.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByAddDateDesc;
                    }
                    else if (orderValue.ToLower().Equals(StlParserUtility.Order_Back.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByAddDate;
                    }
                    else if (orderValue.ToLower().Equals(StlParserUtility.Order_AddDate.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByAddDateDesc;
                    }
                    else if (orderValue.ToLower().Equals(StlParserUtility.Order_AddDateBack.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByAddDate;
                    }
                    else
                    {
                        orderByString = orderValue;
                    }
                }
                else if (tableStyle == ETableStyle.InputContent)
                {
                    if (orderValue.ToLower().Equals(StlParserUtility.Order_Default.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByTaxisDesc;
                    }
                    else if (orderValue.ToLower().Equals(StlParserUtility.Order_Back.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByTaxis;
                    }
                    else if (orderValue.ToLower().Equals(StlParserUtility.Order_AddDate.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByAddDateDesc;
                    }
                    else if (orderValue.ToLower().Equals(StlParserUtility.Order_AddDateBack.ToLower()))
                    {
                        taxisType = ETaxisType.OrderByAddDate;
                    }
                    else
                    {
                        orderByString = orderValue;
                    }
                }
            }
            ArrayList orderedContentIDArrayList = null;
            if (taxisType == ETaxisType.OrderByHits)
            {
                if (tableStyle == ETableStyle.Channel)
                {
                    orderedContentIDArrayList = DataProvider.TrackingDAO.GetPageNodeIDArrayListByAccessNum(publishmentSystemID);
                }
            }
            else if (taxisType == ETaxisType.OrderByStars)
            {
                orderedContentIDArrayList = DataProvider.StarDAO.GetContentIDArrayListByPoint(publishmentSystemID);
            }
            else if (taxisType == ETaxisType.OrderByDigg)
            {
                orderedContentIDArrayList = BaiRongDataProvider.DiggDAO.GetRelatedIdentityArrayListByTotal(publishmentSystemID);
            }
            else if (taxisType == ETaxisType.OrderByComments)
            {
                orderedContentIDArrayList = DataProvider.CommentDAO.GetContentIDArrayListByCount(publishmentSystemID);
            }
            return ETaxisTypeUtils.GetOrderByString(tableStyle, taxisType, orderByString, orderedContentIDArrayList);
        }

        public static string GetPageContentsSqlString(PublishmentSystemInfo publishmentSystemInfo, int channelID, string groupContent, string groupContentNot, string tags, bool isImageExists, bool isImage, bool isVideoExists, bool isVideo, bool isFileExists, bool isFile, bool isNoDup, int startNum, int totalNum, string orderByString, bool isTopExists, bool isTop, bool isRecommendExists, bool isRecommend, bool isHotExists, bool isHot, bool isColorExists, bool isColor, string where, EScopeType scopeType, string groupChannel, string groupChannelNot)
        {
            string sqlString = string.Empty;

            if (DataProvider.NodeDAO.IsExists(channelID))
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, channelID);
                ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);

                string sqlWhereString = string.Empty;
                if (tableStyle == ETableStyle.BackgroundContent)
                {
                    sqlWhereString = DataProvider.BackgroundContentDAO.GetStlWhereString(publishmentSystemInfo, tableName, groupContent, groupContentNot, tags, isImageExists, isImage, isVideoExists, isVideo, isFileExists, isFile, isTopExists, isTop, isRecommendExists, isRecommend, isHotExists, isHot, isColorExists, isColor, where);
                }
                else
                {
                    sqlWhereString = BaiRongDataProvider.ContentDAO.GetStlWhereString(AppManager.CMS.AppID, publishmentSystemInfo.PublishmentSystemID, groupContent, groupContentNot, tags, isTopExists, isTop, where);
                }
                sqlString = DataProvider.ContentDAO.GetStlSqlStringChecked(tableName, publishmentSystemInfo.PublishmentSystemID, channelID, startNum, totalNum, orderByString, sqlWhereString, scopeType, groupChannel, groupChannelNot, isNoDup);
            }

            return sqlString;
        }

        public static IEnumerable GetContentsDataSource(PublishmentSystemInfo publishmentSystemInfo, int channelID, int contentID, string groupContent, string groupContentNot, string tags, bool isImageExists, bool isImage, bool isVideoExists, bool isVideo, bool isFileExists, bool isFile, bool isNoDup, bool isRelatedContents, int startNum, int totalNum, string orderByString, bool isTopExists, bool isTop, bool isRecommendExists, bool isRecommend, bool isHotExists, bool isHot, bool isColorExists, bool isColor, string where, EScopeType scopeType, string groupChannel, string groupChannelNot, NameValueCollection otherAttributes)
        {
            IEnumerable ie = null;

            if (DataProvider.NodeDAO.IsExists(channelID))
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, channelID);
                ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);

                if (isRelatedContents && contentID > 0)
                {
                    bool isTags = false;
                    string tagCollection = BaiRongDataProvider.ContentDAO.GetValue(tableName, contentID, ContentAttribute.Tags);
                    if (!string.IsNullOrEmpty(tagCollection))
                    {
                        ArrayList contentIDArrayList = BaiRongDataProvider.TagDAO.GetContentIDArrayListByTagCollection(TranslateUtils.StringCollectionToStringCollection(tagCollection), AppManager.CMS.AppID, publishmentSystemInfo.PublishmentSystemID);
                        contentIDArrayList.Remove(contentID);
                        if (contentIDArrayList.Count > 0)
                        {
                            isTags = true;
                            if (string.IsNullOrEmpty(where))
                            {
                                where = string.Format("ID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(contentIDArrayList));
                            }
                            else
                            {
                                where += string.Format(" AND (ID IN ({0}))", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(contentIDArrayList));
                            }
                        }
                    }

                    if (!isTags)
                    {
                        if (string.IsNullOrEmpty(where))
                        {
                            where = string.Format("ID <> {0}", contentID);
                        }
                        else
                        {
                            where += string.Format(" AND (ID <> {0})", contentID);
                        }
                    }
                }

                string sqlWhereString = string.Empty;
                if (tableStyle == ETableStyle.BackgroundContent || tableStyle == ETableStyle.GoodsContent || tableStyle == ETableStyle.GovPublicContent)
                {
                    sqlWhereString = DataProvider.BackgroundContentDAO.GetStlWhereString(publishmentSystemInfo, tableName, groupContent, groupContentNot, tags, isImageExists, isImage, isVideoExists, isVideo, isFileExists, isFile, isTopExists, isTop, isRecommendExists, isRecommend, isHotExists, isHot, isColorExists, isColor, where);
                }
                else
                {
                    sqlWhereString = BaiRongDataProvider.ContentDAO.GetStlWhereString(AppManager.CMS.AppID, publishmentSystemInfo.PublishmentSystemID, groupContent, groupContentNot, tags, isTopExists, isTop, where);
                }
                ie = DataProvider.ContentDAO.GetStlDataSourceChecked(tableName, channelID, startNum, totalNum, orderByString, sqlWhereString, scopeType, groupChannel, groupChannelNot, isNoDup, otherAttributes);
            }

            return ie;
        }

        public static IEnumerable GetCommentsDataSource(int publishmentSystemID, int channelID, int contentID, DbItemContainer itemContainer, int startNum, int totalNum, bool isRecommend, string orderByString, string where)
        {
            string sqlString = DataProvider.CommentDAO.GetSelectSqlStringWithChecked(publishmentSystemID, channelID, contentID, startNum, totalNum, isRecommend, where, orderByString);
            IEnumerable ie = BaiRongDataProvider.DatabaseDAO.GetDataSource(sqlString);

            return ie;
        }

        public static DataSet GetPageCommentsDataSet(int publishmentSystemID, int channelID, int contentID, DbItemContainer itemContainer, int startNum, int totalNum, bool isRecommend, string orderByString, string where)
        {
            string sqlString = DataProvider.CommentDAO.GetSelectSqlStringWithChecked(publishmentSystemID, channelID, contentID, startNum, totalNum, isRecommend, where, orderByString);
            DataSet dataSet = BaiRongDataProvider.DatabaseDAO.GetDataSet(sqlString);

            return dataSet;
        }

        public static IEnumerable GetInputContentsDataSource(int publishmentSystemID, int inputID, ContentsDisplayInfo displayInfo)
        {
            bool isReplyExists = displayInfo.OtherAttributes[StlInputContents.Attribute_IsReply] != null;
            bool isReply = TranslateUtils.ToBool(displayInfo.OtherAttributes[StlInputContents.Attribute_IsReply]);
            string sqlString = DataProvider.InputContentDAO.GetSelectSqlStringWithChecked(publishmentSystemID, inputID, isReplyExists, isReply, displayInfo.StartNum, displayInfo.TotalNum, displayInfo.Where, displayInfo.OrderByString, displayInfo.OtherAttributes);
            return BaiRongDataProvider.DatabaseDAO.GetDataSource(sqlString);
        }

        public static IEnumerable GetWebsiteMessageContentsDataSource(int publishmentSystemID, int websiteMessageID, ContentsDisplayInfo displayInfo)
        {
            bool isReplyExists = displayInfo.OtherAttributes[StlWebsiteMessageContents.Attribute_IsReply] != null;
            bool isReply = TranslateUtils.ToBool(displayInfo.OtherAttributes[StlWebsiteMessageContents.Attribute_IsReply]);
            string sqlString = DataProvider.WebsiteMessageContentDAO.GetSelectSqlStringWithChecked(publishmentSystemID, websiteMessageID, isReplyExists, isReply, displayInfo.StartNum, displayInfo.TotalNum, displayInfo.Where, displayInfo.OrderByString, displayInfo.OtherAttributes);
            return BaiRongDataProvider.DatabaseDAO.GetDataSource(sqlString);
        }

        public static DataSet GetPageInputContentsDataSet(int publishmentSystemID, int inputID, ContentsDisplayInfo displayInfo)
        {
            bool isReplyExists = displayInfo.OtherAttributes[StlInputContents.Attribute_IsReply] != null;
            bool isReply = TranslateUtils.ToBool(displayInfo.OtherAttributes[StlInputContents.Attribute_IsReply]);
            string sqlString = DataProvider.InputContentDAO.GetSelectSqlStringWithChecked(publishmentSystemID, inputID, isReplyExists, isReply, displayInfo.StartNum, displayInfo.TotalNum, displayInfo.Where, displayInfo.OrderByString, displayInfo.OtherAttributes);
            return BaiRongDataProvider.DatabaseDAO.GetDataSet(sqlString);
        }

        public static IEnumerable GetChannelsDataSource(int publishmentSystemID, int channelID, string group, string groupNot, bool isImageExists, bool isImage, int startNum, int totalNum, string orderByString, EScopeType scopeType, bool isTotal, string where)
        {
            IEnumerable ie = null;

            if (isTotal)//从所有栏目中选择
            {
                string sqlWhereString = DataProvider.NodeDAO.GetWhereString(publishmentSystemID, group, groupNot, isImageExists, isImage, where);
                ie = DataProvider.NodeDAO.GetStlDataSourceByPublishmentSystemID(publishmentSystemID, startNum, totalNum, sqlWhereString, orderByString);
            }
            else
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, channelID);
                if (nodeInfo != null)
                {
                    string sqlWhereString = DataProvider.NodeDAO.GetWhereString(publishmentSystemID, group, groupNot, isImageExists, isImage, where);
                    ie = DataProvider.NodeDAO.GetStlDataSource(nodeInfo, startNum, totalNum, sqlWhereString, scopeType, orderByString);
                }
            }

            return ie;
        }

        public static DataSet GetPageChannelsDataSet(int publishmentSystemID, int channelID, string group, string groupNot, bool isImageExists, bool isImage, int startNum, int totalNum, string orderByString, EScopeType scopeType, bool isTotal, string where)
        {
            DataSet dataSet = null;

            if (isTotal)//从所有栏目中选择
            {
                string sqlWhereString = DataProvider.NodeDAO.GetWhereString(publishmentSystemID, group, groupNot, isImageExists, isImage, where);
                dataSet = DataProvider.NodeDAO.GetStlDataSetByPublishmentSystemID(publishmentSystemID, startNum, totalNum, sqlWhereString, orderByString);
            }
            else
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, channelID);
                if (nodeInfo != null)
                {
                    string sqlWhereString = DataProvider.NodeDAO.GetWhereString(publishmentSystemID, group, groupNot, isImageExists, isImage, where);
                    dataSet = DataProvider.NodeDAO.GetStlDataSet(nodeInfo, startNum, totalNum, sqlWhereString, scopeType, orderByString);
                }
            }
            return dataSet;
        }

        public static IEnumerable GetSqlContentsDataSource(string connectionString, string queryString, int startNum, int totalNum, string orderByString)
        {
            string sqlString = BaiRongDataProvider.TableStructureDAO.GetSelectSqlStringByQueryString(connectionString, queryString, startNum, totalNum, orderByString);
            return BaiRongDataProvider.DatabaseDAO.GetDataSource(connectionString, sqlString);
        }

        public static DataSet GetPageSqlContentsDataSet(string connectionString, string queryString, int startNum, int totalNum, string orderByString)
        {
            string sqlString = BaiRongDataProvider.TableStructureDAO.GetSelectSqlStringByQueryString(connectionString, queryString, startNum, totalNum, orderByString);
            return BaiRongDataProvider.DatabaseDAO.GetDataSet(connectionString, sqlString);
        }

        public static IEnumerable GetSitesDataSource(string siteName, string directory, int startNum, int totalNum, string whereString, EScopeType scopeType, string orderByString, string since)
        {
            return DataProvider.PublishmentSystemDAO.GetStlDataSource(siteName, directory, startNum, totalNum, whereString, scopeType, orderByString, since);
        }

        public static IEnumerable GetPhotosDataSource(PublishmentSystemInfo publishmentSystemInfo, int contentID, int startNum, int totalNum)
        {
            return DataProvider.PhotoDAO.GetStlDataSource(publishmentSystemInfo.PublishmentSystemID, contentID, startNum, totalNum);
        }

        public static IEnumerable GetTeleplaysDataSource(PublishmentSystemInfo publishmentSystemInfo, int contentID, int startNum, int totalNum)
        {
            return DataProvider.TeleplayDAO.GetStlDataSource(publishmentSystemInfo.PublishmentSystemID, contentID, startNum, totalNum);
        }

        public static IEnumerable GetSpecsDataSource(PublishmentSystemInfo publishmentSystemInfo, int nodeID, int contentID, int specID, int startNum, int totalNum)
        {
            if (specID > 0)
            {
                return DataProviderB2C.SpecComboDAO.GetStlDataSource(publishmentSystemInfo, nodeID, contentID, specID, startNum, totalNum);
            }
            else
            {
                return DataProviderB2C.SpecDAO.GetStlDataSource(publishmentSystemInfo, nodeID, contentID, startNum, totalNum);
            }

        }

        public static IEnumerable GetFiltersDataSource(PublishmentSystemInfo publishmentSystemInfo, int nodeID, int filterID, int startNum, int totalNum)
        {
            if (filterID > 0)
            {
                FilterInfo filterInfo = DataProviderB2C.FilterDAO.GetFilterInfo(filterID);
                if (filterInfo.IsDefaultValues)
                {
                    //获取默认值
                    List<FilterItemInfo> brandContentList = FilterManager.GetDefaultFilterItemInfoList(publishmentSystemInfo, nodeID, filterID, filterInfo.AttributeName);
                    return brandContentList;
                }
                else
                {
                    //自定义值
                    return TranslateUtils.GetList(FilterManager.GetFilterItemInfoList(publishmentSystemInfo, nodeID, filterInfo), startNum, totalNum);
                }
            }
            else
            {
                return DataProviderB2C.FilterDAO.GetStlDataSource(publishmentSystemInfo.PublishmentSystemID, nodeID, startNum, totalNum);
            }
        }

        public static IEnumerable GetDataSourceByStlElement(PublishmentSystemInfo publishmentSystemInfo, int templateID, string elementName, string stlElement)
        {
            XmlDocument xmlDocument = StlParserUtility.GetXmlDocument(stlElement, false);
            XmlNode node = xmlDocument.DocumentElement;
            node = node.FirstChild;

            TemplateInfo templateInfo = TemplateManager.GetTemplateInfo(publishmentSystemInfo.PublishmentSystemID, templateID);
            PageInfo pageInfo = new PageInfo(templateInfo, publishmentSystemInfo.PublishmentSystemID, publishmentSystemInfo.PublishmentSystemID, 0, publishmentSystemInfo, publishmentSystemInfo.Additional.VisualType);
            ContextInfo contextInfo = new ContextInfo(pageInfo);

            if (node != null && node.Name != null)
            {
                if (elementName == StlChannels.ElementName)
                {
                    ContentsDisplayInfo displayInfo = ContentsDisplayInfo.GetContentsDisplayInfoByXmlNode(node, pageInfo, contextInfo, EContextType.Channel);

                    return StlChannels.GetDataSource(pageInfo, contextInfo, displayInfo);
                }
                else if (elementName == StlContents.ElementName)
                {
                    ContentsDisplayInfo displayInfo = ContentsDisplayInfo.GetContentsDisplayInfoByXmlNode(node, pageInfo, contextInfo, EContextType.Content);

                    return StlContents.GetDataSource(pageInfo, contextInfo, displayInfo);
                }
            }

            return null;
        }

        internal static DataSet GetPageWebsiteMessageContentsDataSet(int publishmentSystemID, int websiteMessageID, ContentsDisplayInfo displayInfo)
        {
            bool isReplyExists = displayInfo.OtherAttributes[StlWebsiteMessageContents.Attribute_IsReply] != null;
            bool isReply = TranslateUtils.ToBool(displayInfo.OtherAttributes[StlWebsiteMessageContents.Attribute_IsReply]);
            string sqlString = DataProvider.WebsiteMessageContentDAO.GetSelectSqlStringWithChecked(publishmentSystemID, websiteMessageID, isReplyExists, isReply, displayInfo.StartNum, displayInfo.TotalNum, displayInfo.Where, displayInfo.OrderByString, displayInfo.OtherAttributes);
            return BaiRongDataProvider.DatabaseDAO.GetDataSet(sqlString);
        }
    }
}
