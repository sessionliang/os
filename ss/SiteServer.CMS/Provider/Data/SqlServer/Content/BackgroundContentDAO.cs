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
using System.Collections.Generic;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
	public class BackgroundContentDAO : DataProviderBase, IBackgroundContentDAO
	{
        public ArrayList GetCountArrayListUnChecked(bool isSystemAdministrator, List<int> publishmentSystemIDList, ArrayList owningNodeIDArrayList, string tableName)
        {
            ArrayList arraylist = new ArrayList();
            ArrayList publishmentSystemIDArrayList = PublishmentSystemManager.GetPublishmentSystemIDArrayList();
            foreach (int publishmentSystemID in publishmentSystemIDArrayList)
            {
                if (!publishmentSystemIDList.Contains(publishmentSystemID)) continue;

                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                if (!isSystemAdministrator)
                {
                    //if (!owningNodeIDArrayList.Contains(psID)) continue;
                    //if (!AdminUtility.HasChannelPermissions(psID, psID, AppManager.CMS.Permission.Channel.ContentCheck)) continue;

                    bool isContentCheck = false;
                    foreach (int theNodeID in owningNodeIDArrayList)
                    {
                        if (AdminUtility.HasChannelPermissions(publishmentSystemID, theNodeID, AppManager.CMS.Permission.Channel.ContentCheck))
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
                    sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE (PublishmentSystemID = {1} AND NodeID > 0 AND IsChecked = '{2}' AND CheckedLevel IN ({3}))", tableName, publishmentSystemID, false.ToString(), TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(checkLevelArrayList));
                }
                else
                {
                    sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE (PublishmentSystemID = {1} AND NodeID IN ({2}) AND IsChecked = '{3}' AND CheckedLevel IN ({4}))", tableName, publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(owningNodeIDArrayList), false.ToString(), TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(checkLevelArrayList));
                }

                int count = BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
                if (count > 0)
                {
                    arraylist.Add(new int[] { publishmentSystemID, count });
                }
            }
            return arraylist;
        }

		public BackgroundContentInfo GetContentInfo(string tableName, int contentID)
		{
			BackgroundContentInfo info = null;
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
                            info = new BackgroundContentInfo();
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

        public string GetStlWhereString(PublishmentSystemInfo publishmentSystemInfo, string tableName, string group, string groupNot, string tags, bool isImageExists, bool isImage, bool isVideoExists, bool isVideo, bool isFileExists, bool isFile, bool isTopExists, bool isTop, bool isRecommendExists, bool isRecommend, bool isHotExists, bool isHot, bool isColorExists, bool isColor, string where)
        {
            StringBuilder whereBuilder = new StringBuilder();
            whereBuilder.AppendFormat(" AND PublishmentSystemID = {0} ", publishmentSystemInfo.PublishmentSystemID);

            if (isImageExists)
            {
                if (isImage)
                {
                    whereBuilder.AppendFormat(" AND {0} <> '' ", BackgroundContentAttribute.ImageUrl);
                }
                else
                {
                    whereBuilder.AppendFormat(" AND {0} = '' ", BackgroundContentAttribute.ImageUrl);
                }
            }

            if (isVideoExists)
            {
                if (isVideo)
                {
                    whereBuilder.AppendFormat(" AND {0} <> '' ", BackgroundContentAttribute.VideoUrl);
                }
                else
                {
                    whereBuilder.AppendFormat(" AND {0} = '' ", BackgroundContentAttribute.VideoUrl);
                }
            }

            if (isFileExists)
            {
                if (isFile)
                {
                    whereBuilder.AppendFormat(" AND {0} <> '' ", BackgroundContentAttribute.FileUrl);
                }
                else
                {
                    whereBuilder.AppendFormat(" AND {0} = '' ", BackgroundContentAttribute.FileUrl);
                }
            }

            if (isTopExists)
            {
                whereBuilder.AppendFormat(" AND IsTop = '{0}' ", isTop.ToString());
            }

            if (isRecommendExists)
            {
                whereBuilder.AppendFormat(" AND {0} = '{1}' ", BackgroundContentAttribute.IsRecommend, isRecommend.ToString());
            }

            if (isHotExists)
            {
                whereBuilder.AppendFormat(" AND {0} = '{1}' ", BackgroundContentAttribute.IsHot, isHot.ToString());
            }

            if (isColorExists)
            {
                whereBuilder.AppendFormat(" AND {0} = '{1}' ", BackgroundContentAttribute.IsColor, isColor.ToString());
            }

            if (!string.IsNullOrEmpty(group))
            {
                group = group.Trim().Trim(',');
                string[] groupArr = group.Split(',');
                if (groupArr != null && groupArr.Length > 0)
                {
                    whereBuilder.Append(" AND (");
                    foreach (string theGroup in groupArr)
                    {
                        if (this.DataBaseType == EDatabaseType.SqlServer)
                        {
                            whereBuilder.AppendFormat(" ({0} = '{1}' OR CHARINDEX('{1},',{0}) > 0 OR CHARINDEX(',{1},',{0}) > 0 OR CHARINDEX(',{1}',{0}) > 0) OR ", ContentAttribute.ContentGroupNameCollection, theGroup.Trim());
                        }
                        else if (this.DataBaseType == EDatabaseType.Oracle)
                        {
                            whereBuilder.AppendFormat(" ({0} = '{1}' OR instr({0}, '{1},') > 0 OR instr({0}, ',{1},') > 0 OR instr({0}, ',{1}') > 0) OR ", ContentAttribute.ContentGroupNameCollection, theGroup.Trim());
                        }
                        else
                        {
                            whereBuilder.AppendFormat(" ({0} = '{1}' OR {0} LIKE '{1},%' OR {0} LIKE '%,{1},%' OR {0} LIKE '%,{1}') OR ", ContentAttribute.ContentGroupNameCollection, theGroup.Trim());
                        }
                    }
                    if (groupArr.Length > 0)
                    {
                        whereBuilder.Length = whereBuilder.Length - 3;
                    }
                    whereBuilder.Append(") ");
                }
            }

            if (!string.IsNullOrEmpty(groupNot))
            {
                groupNot = groupNot.Trim().Trim(',');
                string[] groupNotArr = groupNot.Split(',');
                if (groupNotArr != null && groupNotArr.Length > 0)
                {
                    whereBuilder.Append(" AND (");
                    foreach (string theGroupNot in groupNotArr)
                    {
                        if (this.DataBaseType == EDatabaseType.SqlServer)
                        {
                            whereBuilder.AppendFormat(" ({0} <> '{1}' AND CHARINDEX('{1},',{0}) = 0 AND CHARINDEX(',{1},',{0}) = 0 AND CHARINDEX(',{1}',{0}) = 0) AND ", ContentAttribute.ContentGroupNameCollection, theGroupNot.Trim());
                        }
                        else if (this.DataBaseType == EDatabaseType.Oracle)
                        {
                            whereBuilder.AppendFormat(" ({0} <> '{1}' AND instr({0}, '{1},') = 0 AND instr({0}, ',{1},') = 0 AND instr({0}, ',{1}') = 0) AND ", ContentAttribute.ContentGroupNameCollection, theGroupNot.Trim());
                        }
                        else
                        {
                            whereBuilder.AppendFormat(" ({0} <> '{1}' AND {0} NOT LIKE '{1},%' AND {0} NOT LIKE '%,{1},%' AND {0} NOT LIKE '%,{1}') AND ", ContentAttribute.ContentGroupNameCollection, theGroupNot.Trim());
                        }
                    }
                    if (groupNotArr.Length > 0)
                    {
                        whereBuilder.Length = whereBuilder.Length - 4;
                    }
                    whereBuilder.Append(") ");
                }
            }

            if (!string.IsNullOrEmpty(tags))
            {
                StringCollection tagCollection = TagUtils.ParseTagsString(tags);
                ArrayList contentIDArrayList = BaiRongDataProvider.TagDAO.GetContentIDArrayListByTagCollection(tagCollection, AppManager.CMS.AppID, publishmentSystemInfo.PublishmentSystemID);
                if (contentIDArrayList.Count > 0)
                {
                    whereBuilder.AppendFormat(" AND (ID IN ({0}))", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(contentIDArrayList));
                }
            }

            if (!string.IsNullOrEmpty(where))
            {
                whereBuilder.AppendFormat(" AND ({0}) ", where);
            }

            if (!publishmentSystemInfo.Additional.IsCreateSearchDuplicate)
            {
                string sqlString = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, "MIN(ID)", whereBuilder.ToString() + " GROUP BY Title");
                whereBuilder.AppendFormat(" AND ID IN ({0}) ", sqlString);
            }

            return whereBuilder.ToString();
        }

        public string GetSelectCommendByDownloads(string tableName, int publishmentSystemID)
        {
            StringBuilder whereString = new StringBuilder();
            whereString.AppendFormat("WHERE (PublishmentSystemID = {0} AND IsChecked='True' AND FileUrl <> '') ", publishmentSystemID);

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, SqlUtils.Asterisk, whereString.ToString());
        }
	}
}
