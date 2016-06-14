using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using BaiRong.Core.AuxiliaryTable;

using SiteServer.CMS.Core.Security;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class ContentDAO : DataProviderBase, SiteServer.CMS.Core.IContentDAO
    {
        public int GetTaxisToInsert(string tableName, int nodeID, bool isTop)
        {
            int taxis = 0;

            if (isTop)
            {
                taxis = BaiRongDataProvider.ContentDAO.GetMaxTaxis(tableName, nodeID, true) + 1;
            }
            else
            {
                taxis = BaiRongDataProvider.ContentDAO.GetMaxTaxis(tableName, nodeID, false) + 1;
            }

            return taxis;
        }

        public int Insert(string tableName, PublishmentSystemInfo publishmentSystemInfo, ContentInfo contentInfo)
        {
            int taxis = this.GetTaxisToInsert(tableName, contentInfo.NodeID, contentInfo.IsTop);

            return Insert(tableName, publishmentSystemInfo, contentInfo, true, taxis);
        }

        public int Insert(string tableName, PublishmentSystemInfo publishmentSystemInfo, ContentInfo contentInfo, bool isUpdateContentNum, int taxis)
        {
            int contentID = 0;

            if (!string.IsNullOrEmpty(tableName))
            {
                if (publishmentSystemInfo.Additional.IsAutoPageInTextEditor && contentInfo.ContainsKey(BackgroundContentAttribute.Content))
                {
                    contentInfo.SetExtendedAttribute(BackgroundContentAttribute.Content, ContentUtility.GetAutoPageContent(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.Content), publishmentSystemInfo.Additional.AutoPageWordNum));
                }

                contentInfo.BeforeExecuteNonQuery();

                contentInfo.Taxis = taxis;

                contentID = BaiRongDataProvider.ContentDAO.Insert(tableName, contentInfo);

                if (isUpdateContentNum)
                {
                    new Action(() =>
                    {
                        DataProvider.NodeDAO.UpdateContentNum(PublishmentSystemManager.GetPublishmentSystemInfo(contentInfo.PublishmentSystemID), contentInfo.NodeID, true);
                    }).BeginInvoke(null, null);
                }
            }

            return contentID;
        }

        public void Update(string tableName, PublishmentSystemInfo publishmentSystemInfo, ContentInfo contentInfo)
        {
            if (publishmentSystemInfo.Additional.IsAutoPageInTextEditor && contentInfo.ContainsKey(BackgroundContentAttribute.Content))
            {
                contentInfo.SetExtendedAttribute(BackgroundContentAttribute.Content, ContentUtility.GetAutoPageContent(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.Content), publishmentSystemInfo.Additional.AutoPageWordNum));
            }

            BaiRongDataProvider.ContentDAO.Update(tableName, contentInfo);
        }

        public void UpdateAutoPageContent(string tableName, PublishmentSystemInfo publishmentSystemInfo)
        {
            if (publishmentSystemInfo.Additional.IsAutoPageInTextEditor)
            {
                string sqlString = string.Format("SELECT ID, [{0}] FROM [{1}] WHERE ([PublishmentSystemID] = {2})", BackgroundContentAttribute.Content, tableName, publishmentSystemInfo.PublishmentSystemID);

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    while (rdr.Read())
                    {
                        int contentID = Convert.ToInt32(rdr[0].ToString());
                        string content = rdr[1].ToString();
                        if (!string.IsNullOrEmpty(content))
                        {
                            content = ContentUtility.GetAutoPageContent(content, publishmentSystemInfo.Additional.AutoPageWordNum);
                            string updateString = string.Format("UPDATE [{0}] SET [{1}] = '{2}' WHERE ID = {3}", tableName, BackgroundContentAttribute.Content, content, contentID);
                            try
                            {
                                this.ExecuteNonQuery(updateString);
                            }
                            catch { }
                        }
                    }

                    rdr.Close();
                }
            }
        }

        public ContentInfo GetContentInfoNotTrash(ETableStyle tableStyle, string tableName, int contentID)
        {
            ContentInfo info = null;
            if (contentID > 0)
            {
                if (!string.IsNullOrEmpty(tableName))
                {
                    string SQL_WHERE = string.Format("WHERE NodeID > 0 AND ID = {0}", contentID);
                    string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, SqlUtils.Asterisk, SQL_WHERE);

                    using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
                    {
                        if (rdr.Read())
                        {
                            info = ContentUtility.GetContentInfo(tableStyle);
                            BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                        }
                        rdr.Close();
                    }
                }
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }

        public ContentInfo GetContentInfo(ETableStyle tableStyle, string tableName, int contentID)
        {
            ContentInfo info = null;
            if (contentID > 0)
            {
                if (!string.IsNullOrEmpty(tableName))
                {
                    string SQL_WHERE = string.Format("WHERE ID = {0}", contentID);
                    string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, SqlUtils.Asterisk, SQL_WHERE);

                    using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
                    {
                        if (rdr.Read())
                        {
                            info = ContentUtility.GetContentInfo(tableStyle);
                            BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                        }
                        rdr.Close();
                    }
                }
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }

        public ContentInfo GetContentInfo(ETableStyle tableStyle, string sqlString)
        {
            ContentInfo info = null;
            if (!string.IsNullOrEmpty(sqlString))
            {
                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    if (rdr.Read())
                    {
                        info = ContentUtility.GetContentInfo(tableStyle);
                        BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    }
                    rdr.Close();
                }
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }

        public int GetCountOfContentAdd(string tableName, int publishmentSystemID, int nodeID, DateTime begin, DateTime end, string userName)
        {
            ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(nodeID, EScopeType.All, string.Empty, string.Empty);
            return BaiRongDataProvider.ContentDAO.GetCountOfContentAdd(tableName, publishmentSystemID, nodeIDArrayList, begin, end, userName);
        }

        public int GetCountOfContentUpdate(string tableName, int publishmentSystemID, int nodeID, DateTime begin, DateTime end, string userName)
        {
            ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(nodeID, EScopeType.All, string.Empty, string.Empty);
            return BaiRongDataProvider.ContentDAO.GetCountOfContentUpdate(tableName, publishmentSystemID, nodeIDArrayList, begin, end, userName);
        }

        private string GetSelectCommendByCondition(ETableStyle tableStyle, string tableName, int publishmentSystemID, int nodeID, bool isSystemAdministrator, ArrayList owningNodeIDArrayList, string searchType, string keyword, string dateFrom, string dateTo, bool isSearchChildren, ETriState checkedState, bool isNoDup, bool isTrashContent)
        {

            ArrayList nodeIDArrayList;
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
            if (isSearchChildren)
            {
                nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(nodeInfo, EScopeType.All, string.Empty, string.Empty, nodeInfo.ContentModelID);
            }
            else
            {
                nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(nodeInfo, EScopeType.Self, string.Empty, string.Empty, nodeInfo.ContentModelID);
            }

            ArrayList arraylist = new ArrayList();
            if (isSystemAdministrator)
            {
                arraylist = nodeIDArrayList;
            }
            else
            {
                foreach (int theNodeID in nodeIDArrayList)
                {
                    if (owningNodeIDArrayList.Contains(theNodeID))
                    {
                        arraylist.Add(theNodeID);
                    }
                }
            }

            return BaiRongDataProvider.ContentDAO.GetSelectCommendByCondition(tableStyle, tableName, publishmentSystemID, arraylist, searchType, keyword, dateFrom, dateTo, checkedState, isNoDup, isTrashContent);
        }

        public ArrayList GetContentIDArrayListChecked(string tableName, int nodeID, bool isSystemAdministrator, ArrayList owningNodeIDArrayList, int totalNum, string orderByString, string whereString, EScopeType scopeType)
        {
            ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(nodeID, scopeType, string.Empty, string.Empty);

            ArrayList theArrayList = new ArrayList();
            foreach (int theNodeID in nodeIDArrayList)
            {
                if (isSystemAdministrator || owningNodeIDArrayList.Contains(theNodeID))
                {
                    theArrayList.Add(theNodeID);
                }
            }

            return BaiRongDataProvider.ContentDAO.GetContentIDArrayListChecked(tableName, theArrayList, totalNum, orderByString, whereString);
        }

        public ArrayList GetContentIDArrayListChecked(string tableName, int nodeID, int totalNum, string orderByString, string whereString, EScopeType scopeType)
        {
            ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(nodeID, scopeType, string.Empty, string.Empty);

            return BaiRongDataProvider.ContentDAO.GetContentIDArrayListChecked(tableName, nodeIDArrayList, totalNum, orderByString, whereString);
        }

        public ArrayList GetContentIDArrayListChecked(string tableName, int nodeID, int totalNum, string orderByFormatString, string whereString)
        {
            return GetContentIDArrayListChecked(tableName, nodeID, totalNum, orderByFormatString, whereString, EScopeType.Self);
        }

        public ArrayList GetContentIDArrayListChecked(string tableName, int nodeID, string orderByFormatString, string whereString)
        {
            return GetContentIDArrayListChecked(tableName, nodeID, 0, orderByFormatString, whereString);
        }

        public ArrayList GetContentIDArrayListChecked(string tableName, int nodeID, int totalNum, string orderByFormatString)
        {
            return this.GetContentIDArrayListChecked(tableName, nodeID, totalNum, orderByFormatString, string.Empty);
        }

        public ArrayList GetContentIDArrayListChecked(string tableName, int nodeID, string orderByFormatString)
        {
            return this.GetContentIDArrayListChecked(tableName, nodeID, orderByFormatString, string.Empty);
        }

        public void TrashContents(int publishmentSystemID, string tableName, ArrayList contentIDArrayList, int nodeID)
        {
            if (!string.IsNullOrEmpty(tableName))
            {
                ArrayList referenceIDArrayList = BaiRongDataProvider.ContentDAO.GetReferenceIDArrayList(tableName, contentIDArrayList);
                if (referenceIDArrayList.Count > 0)
                {
                    this.DeleteContents(publishmentSystemID, tableName, referenceIDArrayList);
                }
                int updateNum = BaiRongDataProvider.ContentDAO.TrashContents(publishmentSystemID, tableName, contentIDArrayList);
                if (updateNum > 0)
                {
                    new Action(() =>
                    {
                        DataProvider.NodeDAO.UpdateContentNum(PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID), nodeID, true);
                    }).BeginInvoke(null, null);
                }
            }
        }

        public void TrashContents(int publishmentSystemID, string tableName, ArrayList contentIDArrayList)
        {
            if (!string.IsNullOrEmpty(tableName))
            {
                ArrayList referenceIDArrayList = BaiRongDataProvider.ContentDAO.GetReferenceIDArrayList(tableName, contentIDArrayList);
                if (referenceIDArrayList.Count > 0)
                {
                    this.DeleteContents(publishmentSystemID, tableName, referenceIDArrayList);
                }
                int updateNum = BaiRongDataProvider.ContentDAO.TrashContents(publishmentSystemID, tableName, contentIDArrayList);
                if (updateNum > 0)
                {
                    new Action(() =>
                    {
                        DataProvider.NodeDAO.UpdateContentNum(PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID));
                    }).BeginInvoke(null, null);
                }
            }
        }

        public void TrashContentsByNodeID(int publishmentSystemID, string tableName, int nodeID)
        {
            if (!string.IsNullOrEmpty(tableName))
            {
                ArrayList contentIDArrayList = BaiRongDataProvider.ContentDAO.GetContentIDArrayList(tableName, nodeID);
                ArrayList referenceIDArrayList = BaiRongDataProvider.ContentDAO.GetReferenceIDArrayList(tableName, contentIDArrayList);
                if (referenceIDArrayList.Count > 0)
                {
                    this.DeleteContents(publishmentSystemID, tableName, referenceIDArrayList);
                }
                int updateNum = BaiRongDataProvider.ContentDAO.TrashContentsByNodeID(publishmentSystemID, tableName, nodeID);
                if (updateNum > 0)
                {
                    new Action(() =>
                    {
                        DataProvider.NodeDAO.UpdateContentNum(PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID), nodeID, true);
                    }).BeginInvoke(null, null);
                }
            }
        }

        public void DeleteContents(int publishmentSystemID, string tableName, ArrayList contentIDArrayList, int nodeID)
        {
            if (!string.IsNullOrEmpty(tableName))
            {
                int deleteNum = BaiRongDataProvider.ContentDAO.DeleteContents(AppManager.CMS.AppID, publishmentSystemID, tableName, contentIDArrayList);

                if (nodeID > 0 && deleteNum > 0)
                {
                    new Action(() =>
                    {
                        DataProvider.NodeDAO.UpdateContentNum(PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID), nodeID, true);
                    }).BeginInvoke(null, null);
                }
            }
        }

        private void DeleteContents(int publishmentSystemID, string tableName, ArrayList contentIDArrayList)
        {
            if (!string.IsNullOrEmpty(tableName))
            {
                int deleteNum = BaiRongDataProvider.ContentDAO.DeleteContents(AppManager.CMS.AppID, publishmentSystemID, tableName, contentIDArrayList);
                if (deleteNum > 0)
                {
                    new Action(() =>
                    {
                        DataProvider.NodeDAO.UpdateContentNum(PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID));
                    }).BeginInvoke(null, null);
                }
            }
        }

        public void DeleteContentsByNodeID(int publishmentSystemID, string tableName, int nodeID)
        {
            if (!string.IsNullOrEmpty(tableName))
            {
                ArrayList contentIDArrayList = this.GetContentIDArrayListChecked(tableName, nodeID, string.Empty);
                int deleteNum = BaiRongDataProvider.ContentDAO.DeleteContentsByNodeID(AppManager.CMS.AppID, publishmentSystemID, tableName, nodeID, contentIDArrayList);

                if (nodeID > 0 && deleteNum > 0)
                {
                    new Action(() =>
                    {
                        DataProvider.NodeDAO.UpdateContentNum(PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID), nodeID, true);
                    }).BeginInvoke(null, null);
                }
            }
        }

        public void DeleteContentsByPreview(int publishmentSystemID, string tableName, int nodeID)
        {
            if (!string.IsNullOrEmpty(tableName))
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE PublishmentSystemID = {1} AND NodeID = {2} AND SourceID = {3}", tableName, publishmentSystemID, nodeID, SourceManager.Preview);
                int deleteNum = this.ExecuteNonQuery(sqlString);

                if (nodeID > 0 && deleteNum > 0)
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
                    nodeInfo.Additional.IsPreviewContentToDelete = false;
                    nodeInfo.ContentNum = nodeInfo.ContentNum - deleteNum;
                    if (nodeInfo.ContentNum < 0)
                    {
                        nodeInfo.ContentNum = 0;
                    }
                    DataProvider.NodeDAO.UpdateNodeInfo(nodeInfo);
                }
            }
        }

        public void RestoreContentsByTrash(int publishmentSystemID, string tableName)
        {
            int updateNum = BaiRongDataProvider.ContentDAO.RestoreContentsByTrash(publishmentSystemID, tableName);
            if (updateNum > 0)
            {
                new Action(() =>
                {
                    DataProvider.NodeDAO.UpdateContentNum(PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID));
                }).BeginInvoke(null, null);
            }
        }

        public string GetSelectCommend(ETableStyle tableStyle, string tableName, int publishmentSystemID, int nodeID, bool isSystemAdministrator, ArrayList owningNodeIDArrayList, string searchType, string keyword, string dateFrom, string dateTo, bool isSearchChildren, ETriState checkedState)
        {
            return GetSelectCommend(tableStyle, tableName, publishmentSystemID, nodeID, isSystemAdministrator, owningNodeIDArrayList, searchType, keyword, dateFrom, dateTo, isSearchChildren, checkedState, false, false);
        }

        public string GetSelectCommend(ETableStyle tableStyle, string tableName, int publishmentSystemID, int nodeID, bool isSystemAdministrator, ArrayList owningNodeIDArrayList, string searchType, string keyword, string dateFrom, string dateTo, bool isSearchChildren, ETriState checkedState, bool isNoDup, bool isTrashContent)
        {

            ArrayList nodeIDArrayList;
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
            if (isSearchChildren)
            {
                nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(nodeInfo, EScopeType.All, string.Empty, string.Empty, nodeInfo.ContentModelID);
            }
            else
            {
                nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(nodeInfo, EScopeType.Self, string.Empty, string.Empty, nodeInfo.ContentModelID);
            }

            ArrayList arraylist = new ArrayList();
            if (isSystemAdministrator)
            {
                arraylist = nodeIDArrayList;
            }
            else
            {
                foreach (int theNodeID in nodeIDArrayList)
                {
                    if (owningNodeIDArrayList.Contains(theNodeID))
                    {
                        arraylist.Add(theNodeID);
                    }
                }
            }

            return BaiRongDataProvider.ContentDAO.GetSelectCommendByCondition(tableStyle, tableName, publishmentSystemID, arraylist, searchType, keyword, dateFrom, dateTo, checkedState, isNoDup, isTrashContent);
        }

        public string GetSelectCommend(ETableStyle tableStyle, string tableName, int publishmentSystemID, int nodeID, bool isSystemAdministrator, ArrayList owningNodeIDArrayList, string searchType, string keyword, string dateFrom, string dateTo, bool isSearchChildren, ETriState checkedState, bool isNoDup, bool isTrashContent, bool isViewContentOnlySelf)
        {
            ArrayList nodeIDArrayList;
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
            if (isSearchChildren)
            {
                nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(nodeInfo, EScopeType.All, string.Empty, string.Empty, nodeInfo.ContentModelID);
            }
            else
            {
                nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(nodeInfo, EScopeType.Self, string.Empty, string.Empty, nodeInfo.ContentModelID);
            }

            ArrayList arraylist = new ArrayList();
            if (isSystemAdministrator)
            {
                arraylist = nodeIDArrayList;
            }
            else
            {
                foreach (int theNodeID in nodeIDArrayList)
                {
                    if (owningNodeIDArrayList.Contains(theNodeID))
                    {
                        arraylist.Add(theNodeID);
                    }
                }
            }

            return BaiRongDataProvider.ContentDAO.GetSelectCommendByCondition(tableStyle, tableName, publishmentSystemID, arraylist, searchType, keyword, dateFrom, dateTo, checkedState, isNoDup, isTrashContent, isViewContentOnlySelf);
        }

        public string GetSelectCommendByContentGroup(string tableName, string contentGroupName, int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT * FROM {0} WHERE PublishmentSystemID={1} AND NodeID>0 AND (ContentGroupNameCollection LIKE '{2},%' OR ContentGroupNameCollection LIKE '%,{2}' OR ContentGroupNameCollection  LIKE '%,{2},%'  OR ContentGroupNameCollection='{2}')", tableName, publishmentSystemID, PageUtils.FilterSql(contentGroupName));
            return sqlString;
        }

        public IEnumerable GetStlDataSourceChecked(string tableName, int nodeID, int startNum, int totalNum, string orderByString, string whereString, EScopeType scopeType, string groupChannel, string groupChannelNot, bool isNoDup, NameValueCollection otherAttributes)
        {
            ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(nodeID, scopeType, groupChannel, groupChannelNot);
            return BaiRongDataProvider.ContentDAO.GetStlDataSourceChecked(tableName, nodeIDArrayList, startNum, totalNum, orderByString, whereString, isNoDup, otherAttributes);
        }

        public string GetStlSqlStringChecked(string tableName, int publishmentSystemID, int nodeID, int startNum, int totalNum, string orderByString, string whereString, EScopeType scopeType, string groupChannel, string groupChannelNot, bool isNoDup)
        {
            string sqlWhereString = string.Empty;

            if (publishmentSystemID == nodeID && scopeType == EScopeType.All && string.IsNullOrEmpty(groupChannel) && string.IsNullOrEmpty(groupChannelNot))
            {
                sqlWhereString = string.Format("WHERE (PublishmentSystemId = {0} AND NodeID > 0 AND IsChecked = '{1}' {2})", publishmentSystemID, true.ToString(), whereString);
            }
            else
            {
                ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(nodeID, scopeType, groupChannel, groupChannelNot);
                if (nodeIDArrayList == null || nodeIDArrayList.Count == 0)
                {
                    return string.Empty;
                }
                if (nodeIDArrayList.Count == 1)
                    sqlWhereString = string.Format("WHERE (NodeID = {0} AND IsChecked = '{1}' {2})", nodeIDArrayList[0], true.ToString(), whereString);
                else
                    sqlWhereString = string.Format("WHERE (NodeID IN ({0}) AND IsChecked = '{1}' {2})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), true.ToString(), whereString);
            }

            if (isNoDup)
            {
                string sqlString = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, "MIN(ID)", sqlWhereString.ToString() + " GROUP BY Title");
                sqlWhereString += string.Format(" AND ID IN ({0})", sqlString);
            }

            if (!string.IsNullOrEmpty(tableName))
            {
                return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, startNum, totalNum, SqlUtils.Asterisk, sqlWhereString, orderByString);
            }
            return string.Empty;
        }

        public void TidyUp(string tableName, int nodeID, string attributeName, bool isDESC)
        {
            string taxisDirection = isDESC ? "ASC" : "DESC";//升序,但由于页面排序是按Taxis的Desc排序的，所以这里sql里面的ASC/DESC取反

            string sqlString = string.Format("SELECT ID, IsTop FROM {0} WHERE NodeID = {1} OR NodeID = -{1} ORDER BY {2} {3}", tableName, nodeID, attributeName, taxisDirection);
            ArrayList sqlArrayList = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                int taxis = 1;
                while (rdr.Read())
                {
                    int id = rdr.GetInt32(0);
                    bool isTop = TranslateUtils.ToBool(rdr.GetValue(1).ToString());

                    sqlArrayList.Add(string.Format("UPDATE {0} SET Taxis = {1}, IsTop = '{2}' WHERE ID = {3}", tableName, taxis++, isTop.ToString(), id));
                }
                rdr.Close();
            }

            BaiRongDataProvider.DatabaseDAO.ExecuteSql(sqlArrayList);
        }

        public bool SetWhereStringBySearch(StringBuilder whereBuilder, PublishmentSystemInfo publishmentSystemInfo, int nodeID, ETableStyle tableStyle, string word, StringCollection typeCollection, string channelID, string dateFrom, string dateTo, string date, string dateAttribute, string excludeAttributes, NameValueCollection form, ECharset charset, SearchwordSettingInfo settingInfo)
        {
            bool isSearch = false;
            if (typeCollection.Count == 0)
            {
                typeCollection.Add(ContentAttribute.Title);
                if (tableStyle == ETableStyle.BackgroundContent)
                {
                    typeCollection.Add(BackgroundContentAttribute.Content);
                }
                else if (tableStyle == ETableStyle.JobContent)
                {
                    typeCollection.Add(JobContentAttribute.Location);
                    typeCollection.Add(JobContentAttribute.Department);
                    typeCollection.Add(JobContentAttribute.Requirement);
                    typeCollection.Add(JobContentAttribute.Responsibility);
                }
            }

            whereBuilder.AppendFormat("(PublishmentSystemID = {0}) ", publishmentSystemInfo.PublishmentSystemID);
            if (!string.IsNullOrEmpty(word))
            {
                whereBuilder.Append(" AND (");
                foreach (string attributeName in typeCollection)
                {
                    whereBuilder.AppendFormat("[{0}] like '%{1}%' OR ", attributeName, PageUtils.FilterSql(word));
                }
                whereBuilder.Length = whereBuilder.Length - 3;
                whereBuilder.Append(")");
                isSearch = true;
            }
            if (!string.IsNullOrEmpty(channelID))
            {
                int theChannelID = TranslateUtils.ToInt(channelID);
                if (theChannelID > 0 && theChannelID != publishmentSystemInfo.PublishmentSystemID)
                {
                    whereBuilder.Append(" AND ");
                    ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListForDescendant(theChannelID);
                    nodeIDArrayList.Add(theChannelID);
                    ArrayList nodeIDArrayListFilter = new ArrayList();
                    //update 20151106, 根据站内搜索设置搜索范围查询
                    foreach (int nodeId in nodeIDArrayList)
                    {
                        if (settingInfo.IsAllow)
                        {
                            ArrayList allowNodeID = TranslateUtils.StringCollectionToArrayList(settingInfo.InNode);
                            if (allowNodeID.IndexOf(nodeId) >= 0)
                                nodeIDArrayListFilter.Add(nodeId);
                        }
                        else
                        {
                            ArrayList notAllowNodeID = TranslateUtils.StringCollectionToArrayList(settingInfo.NotInNode);
                            if (notAllowNodeID.IndexOf(nodeId) < 0)
                                nodeIDArrayListFilter.Add(nodeId);
                        }
                    }
                    if (nodeIDArrayList.Count == 1)
                        whereBuilder.AppendFormat("(NodeID = {0}) ", nodeIDArrayList[0]);
                    else
                        whereBuilder.AppendFormat("(NodeID IN ({0})) ", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList));
                }
                isSearch = true;
            }
            if (!string.IsNullOrEmpty(dateFrom))
            {
                whereBuilder.Append(" AND ");
                whereBuilder.AppendFormat(" {0} >= '{1}' ", dateAttribute, dateFrom);
                isSearch = true;
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                whereBuilder.Append(" AND ");
                whereBuilder.AppendFormat(" {0} <= '{1}' ", dateAttribute, dateTo);
                isSearch = true;
            }
            if (!string.IsNullOrEmpty(date))
            {
                int days = TranslateUtils.ToInt(date);
                if (days > 0)
                {
                    whereBuilder.Append(" AND ");
                    whereBuilder.AppendFormat("(DATEDIFF([Day], {0}, getdate()) < {1})", dateAttribute, days);
                }
                isSearch = true;
            }

            ArrayList styleInfoArrayList = RelatedIdentities.GetTableStyleInfoArrayList(publishmentSystemInfo, tableStyle, nodeID);
            string tableName = RelatedIdentities.GetTableName(publishmentSystemInfo, tableStyle, nodeID);

            ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(excludeAttributes);
            foreach (string key in form.Keys)
            {
                if (arraylist.Contains(key.ToLower())) continue;
                if (!string.IsNullOrEmpty(form[key]))
                {
                    string value = StringUtils.Trim(PageUtils.UrlDecode(form[key], charset));
                    if (!string.IsNullOrEmpty(value))
                    {
                        if (TableManager.IsAttributeNameExists(tableStyle, tableName, key))
                        {
                            whereBuilder.Append(" AND ");
                            whereBuilder.AppendFormat("([{0}] LIKE '%{1}%')", key, value);
                            isSearch = true;
                        }
                        else
                        {
                            foreach (TableStyleInfo tableStyleInfo in styleInfoArrayList)
                            {
                                if (StringUtils.EqualsIgnoreCase(tableStyleInfo.AttributeName, key))
                                {
                                    whereBuilder.Append(" AND ");
                                    whereBuilder.AppendFormat("({0} LIKE '%{1}={2}%')", ContentAttribute.SettingsXML, key, value);
                                    isSearch = true;
                                    break;
                                }
                            }
                        }

                        if (tableStyle == ETableStyle.GovPublicContent)
                        {
                            if (StringUtils.EqualsIgnoreCase(key, GovPublicContentAttribute.DepartmentID))
                            {
                                whereBuilder.Append(" AND ");
                                whereBuilder.AppendFormat("([{0}] = {1})", key, TranslateUtils.ToInt(value));
                                isSearch = true;
                            }
                            else if (StringUtils.EqualsIgnoreCase(key, GovPublicContentAttribute.Category1ID))
                            {
                                whereBuilder.Append(" AND ");
                                whereBuilder.AppendFormat("([{0}] = {1})", key, TranslateUtils.ToInt(value));
                                isSearch = true;
                            }
                            else if (StringUtils.EqualsIgnoreCase(key, GovPublicContentAttribute.Category2ID))
                            {
                                whereBuilder.Append(" AND ");
                                whereBuilder.AppendFormat("([{0}] = {1})", key, TranslateUtils.ToInt(value));
                                isSearch = true;
                            }
                            else if (StringUtils.EqualsIgnoreCase(key, GovPublicContentAttribute.Category3ID))
                            {
                                whereBuilder.Append(" AND ");
                                whereBuilder.AppendFormat("([{0}] = {1})", key, TranslateUtils.ToInt(value));
                                isSearch = true;
                            }
                            else if (StringUtils.EqualsIgnoreCase(key, GovPublicContentAttribute.Category4ID))
                            {
                                whereBuilder.Append(" AND ");
                                whereBuilder.AppendFormat("([{0}] = {1})", key, TranslateUtils.ToInt(value));
                                isSearch = true;
                            }
                            else if (StringUtils.EqualsIgnoreCase(key, GovPublicContentAttribute.Category5ID))
                            {
                                whereBuilder.Append(" AND ");
                                whereBuilder.AppendFormat("([{0}] = {1})", key, TranslateUtils.ToInt(value));
                                isSearch = true;
                            }
                            else if (StringUtils.EqualsIgnoreCase(key, GovPublicContentAttribute.Category6ID))
                            {
                                whereBuilder.Append(" AND ");
                                whereBuilder.AppendFormat("([{0}] = {1})", key, TranslateUtils.ToInt(value));
                                isSearch = true;
                            }
                        }
                    }
                }
            }
            return isSearch;
        }

        public ContentInfo GetContentInfoByTitle(ETableStyle tableStyle, string tableName, string title)
        {
            ContentInfo info = null;

            if (!string.IsNullOrEmpty(tableName))
            {
                string SQL_WHERE = string.Format("WHERE Title = {0}", title);
                string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, SqlUtils.Asterisk, SQL_WHERE);

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
                {
                    if (rdr.Read())
                    {
                        info = ContentUtility.GetContentInfo(tableStyle);
                        BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    }
                    rdr.Close();
                }
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }

        /// <summary>
        /// 投稿内容
        /// </summary>
        /// <param name="tableStyle"></param>
        /// <param name="tableName"></param>
        /// <param name="publishmentSystemID"></param>
        /// <param name="nodeID"></param>
        /// <param name="isSystemAdministrator"></param>
        /// <param name="owningNodeIDArrayList"></param>
        /// <param name="searchType"></param>
        /// <param name="keyword"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="isSearchChildren"></param>
        /// <param name="checkedState"></param>
        /// <param name="isNoDup"></param>
        /// <param name="isTrashContent"></param>
        /// <param name="isViewContentOnlySelf"></param>
        /// <param name="isMLib"></param>
        /// <returns></returns>
        public string GetSelectCommend(ETableStyle tableStyle, string tableName, int publishmentSystemID, int nodeID, bool isSystemAdministrator, ArrayList owningNodeIDArrayList, string searchType, string keyword, string dateFrom, string dateTo, bool isSearchChildren, ETriState checkedState, bool isNoDup, bool isTrashContent, bool isViewContentOnlySelf, bool isMLib)
        {
            ArrayList nodeIDArrayList;
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
            if (isSearchChildren)
            {
                nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(nodeInfo, EScopeType.All, string.Empty, string.Empty, nodeInfo.ContentModelID);
            }
            else
            {
                nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(nodeInfo, EScopeType.Self, string.Empty, string.Empty, nodeInfo.ContentModelID);
            }

            ArrayList arraylist = new ArrayList();
            if (isSystemAdministrator)
            {
                arraylist = nodeIDArrayList;
            }
            else
            {
                foreach (int theNodeID in nodeIDArrayList)
                {
                    if (owningNodeIDArrayList.Contains(theNodeID))
                    {
                        arraylist.Add(theNodeID);
                    }
                }
            }

            return BaiRongDataProvider.ContentDAO.GetSelectCommendByMLib(tableStyle, tableName, publishmentSystemID, arraylist, searchType, keyword, dateFrom, dateTo, checkedState, isNoDup, isTrashContent, isViewContentOnlySelf, string.Empty, isMLib);
        }

        public string GetSelectCommend(ETableStyle tableStyle, string tableName, int publishmentSystemID, int nodeID, bool isSystemAdministrator, ArrayList owningNodeIDArrayList, string searchType, string keyword, string dateFrom, string dateTo, bool isSearchChildren, ETriState checkedState, bool isNoDup, bool isTrashContent, bool isViewContentOnlySelf, string memberName, bool isMLib)
        {
            ArrayList nodeIDArrayList;
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
            if (isSearchChildren)
            {
                nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(nodeInfo, EScopeType.All, string.Empty, string.Empty, nodeInfo.ContentModelID);
            }
            else
            {
                nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(nodeInfo, EScopeType.Self, string.Empty, string.Empty, nodeInfo.ContentModelID);
            }

            ArrayList arraylist = new ArrayList();
            if (isSystemAdministrator)
            {
                arraylist = nodeIDArrayList;
            }
            else
            {
                foreach (int theNodeID in nodeIDArrayList)
                {
                    if (owningNodeIDArrayList.Contains(theNodeID))
                    {
                        arraylist.Add(theNodeID);
                    }
                }
            }

            return BaiRongDataProvider.ContentDAO.GetSelectCommendByMLib(tableStyle, tableName, publishmentSystemID, arraylist, searchType, keyword, dateFrom, dateTo, checkedState, isNoDup, isTrashContent, isViewContentOnlySelf, memberName, isMLib);
        }

        public DataSet GetDateSet(string tableName, int publishmentSystemID, string keyword, string dateFrom, string dateTo, bool isChecked, bool isMLib, string userName)
        {
            string orderByString = ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc);

            string dateString = string.Empty;
            if (!string.IsNullOrEmpty(dateFrom))
            {
                dateString = string.Format(" AND AddDate >= '{0}' ", dateFrom);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    dateString = string.Format(" AND to_char(AddDate,'YYYY-MM-DD') >= '{0}' ", dateFrom);
                }
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                dateTo = DateUtils.GetDateString(TranslateUtils.ToDateTime(dateTo).AddDays(1));
                dateString += string.Format(" AND AddDate <= '{0}' ", dateTo);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    dateString += string.Format(" AND to_char(AddDate,'YYYY-MM-DD') <= '{0}' ", dateTo);
                }
            }
            StringBuilder whereString = new StringBuilder("WHERE ");

            whereString.AppendFormat(" PublishmentSystemID = {0} and NodeID>0", publishmentSystemID);

            if (string.IsNullOrEmpty(keyword))
            {
                whereString.Append(dateString);
            }
            //投稿内容
            if (!string.IsNullOrEmpty(keyword))
            {
                whereString.AppendFormat(" AND Title like '%{0}%' ", keyword);
            }
            if (isChecked)
                whereString.Append("AND IsChecked='True' ");
            //投稿内容
            if (isMLib)
            {
                whereString.AppendFormat(" AND MemberName != '' ");
            }
            //用户
            if (!string.IsNullOrEmpty(userName))
            {
                whereString.AppendFormat(" AND MemberName ='{0}' ", userName);
            }
            whereString.Append(" ").Append(orderByString);
            return this.ExecuteDataset(string.Format(" select PublishmentSystemID,NodeID,Title,ID from {0} {1}", tableName, whereString.ToString()));

        }



        /// <summary>
        /// 稿件添加量统计
        /// add by sofuny at 20160201
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="tableName"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="xType"></param>
        /// <returns></returns>
        public Hashtable GetTrackingHashtable(int publishmentSystemID, string tableName, DateTime dateFrom, DateTime dateTo, string xType)
        {
            Hashtable hashtable = new Hashtable();
            if (string.IsNullOrEmpty(xType))
            {
                xType = EStatictisXTypeUtils.GetValue(EStatictisXType.Day);
            }

            StringBuilder builder = new StringBuilder();
            if (dateFrom != null)
            {
                builder.AppendFormat(" AND AddDate >= '{0}'", dateFrom.ToString());
            }
            if (dateTo != null)
            {
                builder.AppendFormat(" AND AddDate < '{0}'", dateTo.ToString());
            }
            builder.AppendFormat(" AND PublishmentSystemID ={0} ", publishmentSystemID);
            builder.AppendFormat(" AND MemberName !='' ");


            string SQL_SELECT_TRACKING_DAY = string.Format(@"
SELECT COUNT(*) AS AddNum, AddYear, AddMonth, AddDay
FROM (SELECT DATEPART([year], AddDate) AS AddYear, DATEPART([Month], 
              AddDate) AS AddMonth, DATEPART([Day], AddDate) 
              AS AddDay
        FROM {2}
        WHERE (DATEDIFF([Day], AddDate, {0}) < 30)  {1}) 
      DERIVEDTBL
GROUP BY AddYear, AddMonth, AddDay
ORDER BY AddYear, AddMonth, AddDay
", SqlUtils.GetDefaultDateString(this.DataBaseType), builder, tableName);//添加日统计

            if (EStatictisXTypeUtils.Equals(xType, EStatictisXType.Month))
            {
                SQL_SELECT_TRACKING_DAY = string.Format(@"
SELECT COUNT(*) AS AddNum, AddYear, AddMonth
FROM (SELECT DATEPART([year], AddDate) AS AddYear, DATEPART([Month], 
              AddDate) AS AddMonth
        FROM {2}
        WHERE (DATEDIFF([Month], AddDate, {0}) < 12) {1}) 
      DERIVEDTBL
GROUP BY AddYear, AddMonth
ORDER BY AddYear, AddMonth
", SqlUtils.GetDefaultDateString(this.DataBaseType), builder, tableName);//添加月统计
            }
            else if (EStatictisXTypeUtils.Equals(xType, EStatictisXType.Year))
            {
                SQL_SELECT_TRACKING_DAY = string.Format(@"
SELECT COUNT(*) AS AddNum, AddYear
FROM (SELECT DATEPART([year], AddDate) AS AddYear
        FROM {2}
        WHERE (DATEDIFF([Year], AddDate, {0}) < 10) {1}) 
      DERIVEDTBL
GROUP BY AddYear
ORDER BY AddYear
", SqlUtils.GetDefaultDateString(this.DataBaseType), builder, tableName);//添加年统计
            }


            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TRACKING_DAY))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        int accessNum = Convert.ToInt32(rdr[0]);
                        if (EStatictisXTypeUtils.Equals(xType, EStatictisXType.Day))
                        {
                            string year = rdr[1].ToString();
                            string month = rdr[2].ToString();
                            string day = rdr[3].ToString();
                            DateTime dateTime = TranslateUtils.ToDateTime(string.Format("{0}-{1}-{2}", year, month, day));
                            hashtable.Add(dateTime, accessNum);
                        }
                        else if (EStatictisXTypeUtils.Equals(xType, EStatictisXType.Month))
                        {
                            string year = rdr[1].ToString();
                            string month = rdr[2].ToString();

                            DateTime dateTime = TranslateUtils.ToDateTime(string.Format("{0}-{1}-1", year, month));
                            hashtable.Add(dateTime, accessNum);
                        }
                        else if (EStatictisXTypeUtils.Equals(xType, EStatictisXType.Year))
                        {
                            string year = rdr[1].ToString();
                            DateTime dateTime = TranslateUtils.ToDateTime(string.Format("{0}-1-1", year));
                            hashtable.Add(dateTime, accessNum);
                        }
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }


        public ArrayList GetIDListBySameTitleInOneNode(string tableName, int nodeID, string title)
        {
            ArrayList array = new ArrayList();
            string sql = string.Format("SELECT ID FROM {0} WHERE NodeID = {1} AND Title = '{2}'", tableName, nodeID, title);
            using (IDataReader reader = this.ExecuteReader(sql))
            {
                while (reader.Read())
                {
                    array.Add(reader.GetInt32(0));
                }
                reader.Close();
            }
            return array;
        }


        public string GetSelectCommendBySelectType(ETableStyle tableStyle, string tableName, int publishmentSystemID, int nodeID, bool isSystemAdministrator, ArrayList owningNodeIDArrayList, string searchType, string keyword, string dateFrom, string dateTo, bool isSearchChildren, ETriState checkedState, bool isNoDup, bool isTrashContent, string selectType, string adminName)
        {
            ArrayList nodeIDArrayList;
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
            if (isSearchChildren)
            {
                nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(nodeInfo, EScopeType.All, string.Empty, string.Empty, nodeInfo.ContentModelID);
            }
            else
            {
                nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(nodeInfo, EScopeType.Self, string.Empty, string.Empty, nodeInfo.ContentModelID);
            }

            ArrayList arraylist = new ArrayList();
            if (isSystemAdministrator)
            {
                arraylist = nodeIDArrayList;
            }
            else
            {
                foreach (int theNodeID in nodeIDArrayList)
                {
                    if (owningNodeIDArrayList.Contains(theNodeID))
                    {
                        arraylist.Add(theNodeID);
                    }
                }
            }

            return BaiRongDataProvider.ContentDAO.GetSelectCommendBySelectType(tableStyle, tableName, publishmentSystemID, arraylist, searchType, keyword, dateFrom, dateTo, checkedState, isNoDup, isTrashContent, selectType, adminName);
        }


        #region  试用管理


        public void UpdateSettingXML(string tableName, int publishmentSystemID, int nodeID, ArrayList contentInfoArrayList)
        {
            string sqlString = string.Empty;
            foreach (ContentInfo info in contentInfoArrayList)
            {
                sqlString += string.Format("UPDATE {0} SET SettingsXML = '{1}' WHERE PublishmentSystemID={2} and NodeID={3} AND ID ={4} ;", tableName, info.GetExtendedAttribute(ContentAttribute.SettingsXML), publishmentSystemID, nodeID, info.ID);
            }

            this.ExecuteNonQuery(sqlString);
        }


        public ArrayList GetContentInfoArrayList(string tableName, ETableStyle tableStyle, int publishmentSystemID, int nodeID, ArrayList contentIDs)
        {
            ArrayList arraylist = new ArrayList();
            string sqlString = string.Empty;
            if (contentIDs.Count > 1)
                sqlString = string.Format("SELECT * FROM {0} WHERE PublishmentSystemID={1} and NodeID={2} and ID IN ({3})", tableName, publishmentSystemID, nodeID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(contentIDs));
            else
                sqlString = string.Format("SELECT * FROM {0} WHERE PublishmentSystemID={1} and NodeID={2} and ID = {3} ", tableName, publishmentSystemID, nodeID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(contentIDs));

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read() && !rdr.IsDBNull(0))
                {
                    ContentInfo info = ContentUtility.GetContentInfo(tableStyle);
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    arraylist.Add(info);
                }
                rdr.Close();
            }

            return arraylist;
        }
        #endregion
    }
}
