using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.B2C.Model;
using SiteServer.B2C.Core;
using BaiRong.Core.AuxiliaryTable;

using System.Web.UI.WebControls;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Core.Security;

namespace SiteServer.B2C.Provider.Data.SqlServer
{
    public class BrandContentDAO : DataProviderBase, IBrandContentDAO
    {
        public ArrayList GetCountArrayListUnChecked(bool isSystemAdministrator, int publishmentSystemID, ArrayList owningNodeIDArrayList, string tableName)
        {
            ArrayList arraylist = new ArrayList();
            ArrayList publishmentSystemIDArrayList = PublishmentSystemManager.GetPublishmentSystemIDArrayList();
            foreach (int psID in publishmentSystemIDArrayList)
            {
                if (publishmentSystemID > 0)
                {
                    if (publishmentSystemID != psID) continue;
                }
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(psID);
                if (!isSystemAdministrator)
                {
                    //if (!owningNodeIDArrayList.Contains(psID)) continue;
                    //if (!AdminUtility.HasChannelPermissions(psID, psID, PredefinedCMSPermissions.ChannelPermisson.ContentCheck)) continue;

                    bool isContentCheck = false;
                    foreach (int theNodeID in owningNodeIDArrayList)
                    {
                        if (AdminUtility.HasChannelPermissions(psID, theNodeID, AppManager.CMS.Permission.Channel.ContentCheck))
                        {
                            isContentCheck = true;
                        }
                    }
                    if (!isContentCheck)
                    {
                        continue;
                    }
                }
                int checkedLevel = 0;
                bool isChecked = CheckManager.GetUserCheckLevel(publishmentSystemInfo, publishmentSystemInfo.PublishmentSystemID, out checkedLevel);
                ArrayList checkLevelArrayList = LevelManager.LevelInt.GetCheckLevelArrayListOfNeedCheck(publishmentSystemInfo, isChecked, checkedLevel);
                string sqlString = string.Empty;
                if (isSystemAdministrator)
                {
                    sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE (PublishmentSystemID = {1} AND NodeID > 0 AND IsChecked = '{2}' AND CheckedLevel IN ({3}))", tableName, psID, false.ToString(), TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(checkLevelArrayList));
                }
                else
                {
                    sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE (PublishmentSystemID = {1} AND NodeID IN ({2}) AND IsChecked = '{3}' AND CheckedLevel IN ({4}))", tableName, psID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(owningNodeIDArrayList), false.ToString(), TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(checkLevelArrayList));
                }

                int count = BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
                if (count > 0)
                {
                    arraylist.Add(new int[] { psID, count });
                }
            }
            return arraylist;
        }

        public GoodsContentInfo GetContentInfo(string tableName, int contentID)
        {
            GoodsContentInfo info = null;
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
                            info = new GoodsContentInfo();
                            BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                        }
                        rdr.Close();
                    }
                }
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }

        public int GetCountCheckedImage(int publishmentSystemID, int nodeID)
        {
            string tableName = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID).AuxiliaryTableForContent;
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE (NodeID = {1} AND ImageUrl <> '' AND {2} = '{3}')", tableName, nodeID, ContentAttribute.IsChecked, true.ToString());

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetStlWhereString(int publishmentSystemID, string group, string groupNot, string tags, AttributesInfo attributesInfo, string where)
        {
            StringBuilder whereStringBuilder = new StringBuilder();
            if (attributesInfo.IsImageExists)
            {
                if (attributesInfo.IsImage)
                {
                    whereStringBuilder.AppendFormat(" AND {0} <> '' ", GoodsContentAttribute.ImageUrl);
                }
                else
                {
                    whereStringBuilder.AppendFormat(" AND {0} = '' ", GoodsContentAttribute.ImageUrl);
                }
            }

            if (attributesInfo.IsFileExists)
            {
                if (attributesInfo.IsFile)
                {
                    whereStringBuilder.AppendFormat(" AND {0} <> '' ", GoodsContentAttribute.FileUrl);
                }
                else
                {
                    whereStringBuilder.AppendFormat(" AND {0} = '' ", GoodsContentAttribute.FileUrl);
                }
            }

            if (attributesInfo.IsTopExists)
            {
                whereStringBuilder.AppendFormat(" AND IsTop = '{0}' ", attributesInfo.IsTop.ToString());
            }

            if (attributesInfo.IsRecommendExists)
            {
                whereStringBuilder.AppendFormat(" AND {0} = '{1}' ", GoodsContentAttribute.IsRecommend, attributesInfo.IsRecommend.ToString());
            }

            if (attributesInfo.IsNewExists)
            {
                whereStringBuilder.AppendFormat(" AND {0} = '{1}' ", GoodsContentAttribute.IsNew, attributesInfo.IsNew.ToString());
            }

            if (attributesInfo.IsHotExists)
            {
                whereStringBuilder.AppendFormat(" AND {0} = '{1}' ", GoodsContentAttribute.IsHot, attributesInfo.IsHot.ToString());
            }

            if (!string.IsNullOrEmpty(group))
            {
                group = group.Trim().Trim(',');
                string[] groupArr = group.Split(',');
                if (groupArr != null && groupArr.Length > 0)
                {
                    whereStringBuilder.Append(" AND (");
                    foreach (string theGroup in groupArr)
                    {
                        whereStringBuilder.AppendFormat(" ({0} = '{1}' OR {0} LIKE '{1},%' OR {0} LIKE '%,{1},%' OR {0} LIKE '%,{1}') OR ", ContentAttribute.ContentGroupNameCollection, theGroup.Trim());
                    }
                    if (groupArr.Length > 0)
                    {
                        whereStringBuilder.Length = whereStringBuilder.Length - 3;
                    }
                    whereStringBuilder.Append(") ");
                }
            }

            if (!string.IsNullOrEmpty(groupNot))
            {
                groupNot = groupNot.Trim().Trim(',');
                string[] groupNotArr = groupNot.Split(',');
                if (groupNotArr != null && groupNotArr.Length > 0)
                {
                    whereStringBuilder.Append(" AND (");
                    foreach (string theGroupNot in groupNotArr)
                    {
                        whereStringBuilder.AppendFormat(" ({0} <> '{1}' AND {0} NOT LIKE '{1},%' AND {0} NOT LIKE '%,{1},%' AND {0} NOT LIKE '%,{1}') AND ", ContentAttribute.ContentGroupNameCollection, theGroupNot.Trim());
                    }
                    if (groupNotArr.Length > 0)
                    {
                        whereStringBuilder.Length = whereStringBuilder.Length - 4;
                    }
                    whereStringBuilder.Append(") ");
                }
            }

            if (!string.IsNullOrEmpty(tags))
            {
                StringCollection tagCollection = TagUtils.ParseTagsString(tags);
                ArrayList contentIDArrayList = BaiRongDataProvider.TagDAO.GetContentIDArrayListByTagCollection(tagCollection, AppManager.B2C.AppID, publishmentSystemID);
                if (contentIDArrayList.Count > 0)
                {
                    whereStringBuilder.AppendFormat(" AND (ID IN ({0}))", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(contentIDArrayList));
                }
            }

            if (!string.IsNullOrEmpty(where))
            {
                whereStringBuilder.AppendFormat(" AND ({0}) ", where);
            }

            return whereStringBuilder.ToString();
        }

        public string GetSelectCommendByDownloads(string tableName, int publishmentSystemID)
        {
            StringBuilder whereString = new StringBuilder();
            whereString.AppendFormat("WHERE (PublishmentSystemID = {0} AND IsChecked='True' AND FileUrl <> '') ", publishmentSystemID);

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, SqlUtils.Asterisk, whereString.ToString());
        }

        public ListItemCollection GetListItemCollection(int publishmentSystemID, int nodeID, bool isTotal)
        {
            ListItemCollection collection = new ListItemCollection();

            string tableName = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID).AuxiliaryTableForBrand;

            if (!string.IsNullOrEmpty(tableName))
            {
                string sqlString = string.Empty;
                if (isTotal)
                {
                    ArrayList arraylist = DataProvider.NodeDAO.GetNodeIDArrayListForDescendant(nodeID);
                    arraylist.Add(nodeID);
                    sqlString = string.Format("SELECT ID, Title FROM {0} WHERE NodeID IN ({1}) AND IsChecked = 'True' ORDER BY NodeID, Taxis DESC", tableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(arraylist));
                }
                else
                {
                    sqlString = string.Format("SELECT ID, Title FROM {0} WHERE NodeID = {1} AND IsChecked = 'True' ORDER BY Taxis DESC", tableName, nodeID);
                }

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    while (rdr.Read())
                    {
                        ListItem listItem = new ListItem(rdr.GetValue(1).ToString(), rdr.GetInt32(0).ToString());
                        collection.Add(listItem);
                    }
                    rdr.Close();
                }
            }

            return collection;
        }

        public ArrayList GetBrandContentArrayList(int publishmentSystemID, int nodeID, bool isTotal)
        {
            ArrayList array = new ArrayList();
            GoodsContentInfo info;

            string tableName = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID).AuxiliaryTableForBrand;

            if (!string.IsNullOrEmpty(tableName))
            {
                string sqlString = string.Empty;
                if (isTotal)
                {
                    ArrayList arraylist = DataProvider.NodeDAO.GetNodeIDArrayListForDescendant(nodeID);
                    arraylist.Add(nodeID);
                    sqlString = string.Format("SELECT ID, Title FROM {0} WHERE NodeID IN ({1}) AND IsChecked = 'True' ORDER BY NodeID, Taxis DESC", tableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(arraylist));
                }
                else
                {
                    sqlString = string.Format("SELECT ID, Title FROM {0} WHERE NodeID = {1} AND IsChecked = 'True' ORDER BY Taxis DESC", tableName, nodeID);
                }

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    while (rdr.Read())
                    {
                        info = new GoodsContentInfo();
                        BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                        array.Add(info);
                    }
                    rdr.Close();
                }
            }

            return array;
        }

        public int GetNodeID(int publishmentSystemID, int contentID)
        {
            int nodeID = 0;

            string tableName = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID).AuxiliaryTableForBrand;

            if (!string.IsNullOrEmpty(tableName))
            {
                string sqlString = string.Format("SELECT NodeID FROM {0} WHERE ID = {1}", tableName, contentID);

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        nodeID = rdr.GetInt32(0);
                    }
                    rdr.Close();
                }
            }

            return nodeID;
        }
    }
}
