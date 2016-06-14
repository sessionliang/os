using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Model;
using System.Data;
using System.Collections;
using BaiRong.Core;
using BaiRong.Core.Data;
using SiteServer.CMS.Controls;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class KeywordClassifyDAO : TreeDAO, IKeywordClassifyDAO
    {
        public KeywordClassifyDAO()
            : base(TABLE_NAME)
        {

        }

        public const string TABLE_NAME = "siteserver_KeywordClassify";

        public const string PARM_ADD_DATE = "@AddDate";

        /// <summary>
        /// 使用事务添加节点信息到KeywordClassify表中
        /// </summary>
        /// <param name="parentKeywordClassifyInfo">父节点</param>
        /// <param name="keywordClassifyInfo">需要添加的节点</param>
        /// <param name="trans"></param>
        private void InsertKeywordClassifyInfoWithTrans(KeywordClassifyInfo parentKeywordClassifyInfo, KeywordClassifyInfo keywordClassifyInfo, IDbTransaction trans)
        {
            if (parentKeywordClassifyInfo != null)
            {
                if (parentKeywordClassifyInfo.ParentsPath.Length == 0)
                {
                    keywordClassifyInfo.ParentsPath = parentKeywordClassifyInfo.ItemID.ToString();
                }
                else
                {
                    keywordClassifyInfo.ParentsPath = parentKeywordClassifyInfo.ParentsPath + "," + parentKeywordClassifyInfo.ItemID;
                }
                keywordClassifyInfo.ParentsCount = parentKeywordClassifyInfo.ParentsCount + 1;

                int maxTaxis = base.GetMaxTaxisByParentPath(keywordClassifyInfo.ParentsPath);
                if (maxTaxis == 0)
                {
                    maxTaxis = parentKeywordClassifyInfo.Taxis;
                }
                keywordClassifyInfo.Taxis = maxTaxis + 1;
            }
            else
            {
                keywordClassifyInfo.ParentsPath = string.Empty;
                keywordClassifyInfo.Taxis = 1;
            }

            string SQL_INSERT_ITEM = "INSERT INTO siteserver_KeywordClassify (ItemName, ItemIndexName, ParentID, ParentsPath, ParentsCount, ChildrenCount, ContentNum, PublishmentSystemID, Enabled, IsLastItem, Taxis, AddDate) VALUES (@ItemName, @ItemIndexName, @ParentID, @ParentsPath, @ParentsCount, @ChildrenCount, @ContentNum, @PublishmentSystemID, @Enabled, @IsLastItem, @Taxis, @AddDate)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                SQL_INSERT_ITEM = "INSERT INTO siteserver_KeywordClassify (ItemID, ItemName, ItemIndexName, ParentID, ParentsPath, ParentsCount, ChildrenCount, ContentNum, PublishmentSystemID, Enabled, IsLastItem, Taxis, AddDate) VALUES (siteserver_KeywordClassify_SEQ.NEXTVAL, @ItemName, @ItemIndexName, @ParentID, @ParentsPath, @ParentsCount, @ChildrenCount, @ContentNum, @PublishmentSystemID, @Enabled, @IsLastItem, @Taxis, @AddDate)";
            }

            IDbDataParameter[] insertParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_ITEM_NAME,EDataType.NVarChar,50,keywordClassifyInfo.ItemName),
                this.GetParameter(PARM_ITEM_INDEX_NAME,EDataType.NVarChar,50,keywordClassifyInfo.ItemIndexName),
                this.GetParameter(PARM_PARENT_ID,EDataType.Integer,keywordClassifyInfo.ParentID),
                this.GetParameter(PARM_PARENTS_PATH,EDataType.NVarChar,255,keywordClassifyInfo.ParentsPath),
                this.GetParameter(PARM_PARENTS_COUNT,EDataType.Integer,keywordClassifyInfo.ParentsCount),
                this.GetParameter(PARM_CHILDREN_COUNT,EDataType.Integer,keywordClassifyInfo.ChildrenCount),
                this.GetParameter(PARM_CONTENT_NUM,EDataType.Integer,keywordClassifyInfo.ContentNum),
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID,EDataType.Integer,keywordClassifyInfo.PublishmentSystemID),
                this.GetParameter(PARM_ENABLED,EDataType.VarChar,18,keywordClassifyInfo.Enabled.ToString()),
                this.GetParameter(PARM_IS_LAST_ITEM,EDataType.VarChar,18,keywordClassifyInfo.IsLastItem.ToString()),
                this.GetParameter(PARM_TAXIS,EDataType.Integer,keywordClassifyInfo.Taxis),
                this.GetParameter(PARM_ADD_DATE,EDataType.DateTime,keywordClassifyInfo.AddDate)
            };

            if (keywordClassifyInfo.ParentID > 0)
            {
                string sqlString = string.Format("UPDATE siteserver_KeywordClassify SET Taxis = Taxis + 1 WHERE (Taxis >= {0}) AND (PublishmentSystemID = {1})", keywordClassifyInfo.Taxis, keywordClassifyInfo.PublishmentSystemID);
                this.ExecuteNonQuery(trans, sqlString);
            }
            this.ExecuteNonQuery(trans, SQL_INSERT_ITEM, insertParms);

            keywordClassifyInfo.ItemID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "siteserver_KeywordClassify");

            if (keywordClassifyInfo.ParentsPath != null && keywordClassifyInfo.ParentsPath.Length > 0)
            {
                string sqlString = string.Concat("UPDATE siteserver_KeywordClassify SET ChildrenCount = ChildrenCount + 1 WHERE ItemID in (", keywordClassifyInfo.ParentsPath, ")");

                this.ExecuteNonQuery(trans, sqlString);
            }

            string SQL_UPDATE_IS_LAST_ITEM = "UPDATE siteserver_KeywordClassify SET IsLastItem = @IsLastItem WHERE ParentID = @ParentID";

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_IS_LAST_ITEM, EDataType.VarChar, 18, false.ToString()),
                this.GetParameter(PARM_PARENT_ID, EDataType.Integer, keywordClassifyInfo.ParentID)
            };

            this.ExecuteNonQuery(trans, SQL_UPDATE_IS_LAST_ITEM, parms);

            SQL_UPDATE_IS_LAST_ITEM = string.Format("UPDATE siteserver_KeywordClassify SET IsLastItem = '{0}' WHERE (ItemID IN (SELECT TOP 1 ItemID FROM siteserver_KeywordClassify WHERE ParentID = {1} ORDER BY Taxis DESC))", true.ToString(), keywordClassifyInfo.ParentID);
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                SQL_UPDATE_IS_LAST_ITEM = string.Format(@"UPDATE siteserver_KeywordClassify SET IsLastItem = '{0}' WHERE (ItemID IN (
SELECT ItemID FROM (
    SELECT ItemID FROM siteserver_KeywordClassify WHERE ParentID = {1} ORDER BY Taxis DESC
) WHERE ROWNUM <= 1
))", true.ToString(), keywordClassifyInfo.ParentID);
            }
            this.ExecuteNonQuery(trans, SQL_UPDATE_IS_LAST_ITEM);

        }


        public int InsertKeywordClassifyInfo(int publishmentSystemID, int parentID, string itemName, string itemIndexName)
        {
            if (publishmentSystemID > 0 && parentID == 0) return 0;
            KeywordClassifyInfo keywordClassifyInfo = new KeywordClassifyInfo();
            keywordClassifyInfo.ParentID = parentID;
            keywordClassifyInfo.PublishmentSystemID = publishmentSystemID;
            keywordClassifyInfo.ItemName = itemName;
            keywordClassifyInfo.ItemIndexName = itemIndexName;
            keywordClassifyInfo.AddDate = DateTime.Now;
            keywordClassifyInfo.Enabled = true;

            KeywordClassifyInfo parentKeywordClassifyInfo = this.GetKeywordClassifyInfo(parentID);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.InsertKeywordClassifyInfoWithTrans(parentKeywordClassifyInfo, keywordClassifyInfo, trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return keywordClassifyInfo.ItemID;
        }

        public int InsertKeywordClassifyInfo(KeywordClassifyInfo keywordClassifyInfo)
        {
            if (keywordClassifyInfo.PublishmentSystemID > 0 && keywordClassifyInfo.ParentID == 0) return 0;

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        KeywordClassifyInfo parentKeywordClassifyInfo = this.GetKeywordClassifyInfo(keywordClassifyInfo.ParentID);

                        this.InsertKeywordClassifyInfoWithTrans(parentKeywordClassifyInfo, keywordClassifyInfo, trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return keywordClassifyInfo.ItemID;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="keywordClassifyInfo"></param>
        public void UpdateKeywordClassifyInfo(KeywordClassifyInfo keywordClassifyInfo)
        {
            KeywordClassifyInfo oldInfo = this.GetKeywordClassifyInfo(keywordClassifyInfo.ItemID);

            TreeBaseItem oldParentInfo = this.GetItemInfo(keywordClassifyInfo.PublishmentSystemID, oldInfo.ParentID);
            TreeBaseItem newParentInfo = this.GetItemInfo(keywordClassifyInfo.PublishmentSystemID, keywordClassifyInfo.ParentID);

            #region 根据当前父级的信息，修改自己的排序，父级路径
            if (newParentInfo != null)
            {
                if (newParentInfo.ParentsPath.Length == 0)
                {
                    keywordClassifyInfo.ParentsPath = newParentInfo.ItemID.ToString();
                }
                else
                {
                    keywordClassifyInfo.ParentsPath = newParentInfo.ParentsPath + "," + newParentInfo.ItemID;
                }
                keywordClassifyInfo.ParentsCount = newParentInfo.ParentsCount + 1;

                int maxTaxis = this.GetMaxTaxisByParentPath(keywordClassifyInfo.ParentsPath);
                if (maxTaxis == 0)
                {
                    maxTaxis = newParentInfo.Taxis;
                }
                keywordClassifyInfo.Taxis = maxTaxis + 1;
            }
            else
            {
                keywordClassifyInfo.ParentsPath = string.Empty;
                keywordClassifyInfo.Taxis = 1;
            }
            #endregion


            string SQL_UPDATE_ITEM = "UPDATE siteserver_KeywordClassify SET ItemName=@ItemName, ItemIndexName=@ItemIndexName, ParentID=@ParentID, ParentsPath=@ParentsPath, ParentsCount=@ParentsCount, ChildrenCount=@ChildrenCount, ContentNum=@ContentNum, PublishmentSystemID=@PublishmentSystemID, Enabled=@Enabled, IsLastItem=@IsLastItem, Taxis=@Taxis, AddDate=@AddDate WHERE ItemID=@ItemID";

            IDbDataParameter[] updateParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_ITEM_NAME,EDataType.NVarChar,50,keywordClassifyInfo.ItemName),
                this.GetParameter(PARM_ITEM_INDEX_NAME,EDataType.NVarChar,50,keywordClassifyInfo.ItemIndexName),
                this.GetParameter(PARM_PARENT_ID,EDataType.Integer,keywordClassifyInfo.ParentID),
                this.GetParameter(PARM_PARENTS_PATH,EDataType.NVarChar,255,keywordClassifyInfo.ParentsPath),
                this.GetParameter(PARM_PARENTS_COUNT,EDataType.Integer,keywordClassifyInfo.ParentsCount),
                this.GetParameter(PARM_CHILDREN_COUNT,EDataType.Integer,keywordClassifyInfo.ChildrenCount),
                this.GetParameter(PARM_CONTENT_NUM,EDataType.Integer,keywordClassifyInfo.ContentNum),
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID,EDataType.Integer,keywordClassifyInfo.PublishmentSystemID),
                this.GetParameter(PARM_ENABLED,EDataType.VarChar,18,keywordClassifyInfo.Enabled.ToString()),
                this.GetParameter(PARM_IS_LAST_ITEM,EDataType.VarChar,18,keywordClassifyInfo.IsLastItem.ToString()),
                this.GetParameter(PARM_TAXIS,EDataType.Integer,keywordClassifyInfo.Taxis),
                this.GetParameter(PARM_ADD_DATE,EDataType.DateTime,keywordClassifyInfo.AddDate),
                this.GetParameter(PARM_ITEM_ID,EDataType.Integer,keywordClassifyInfo.ItemID)
            };

            this.ExecuteNonQuery(SQL_UPDATE_ITEM, updateParms);

            if (oldInfo.ParentID != keywordClassifyInfo.ParentID)
            {
                UpdateItemNum(oldParentInfo, newParentInfo);
            }
        }

        public KeywordClassifyInfo GetKeywordClassifyInfo(int itemID)
        {
            string SQL_SELECT_ITEM = string.Format("SELECT ItemID, ItemName, ItemIndexName, ParentID, ParentsPath, ParentsCount, ChildrenCount, ContentNum, PublishmentSystemID, Enabled, IsLastItem, Taxis, AddDate FROM {0} WHERE ItemID = @ItemID", TABLE_NAME);

            KeywordClassifyInfo item = null;

            IDbDataParameter[] itemParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_ITEM_ID, EDataType.Integer, itemID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ITEM, itemParms))
            {
                if (rdr.Read())
                {
                    item = new KeywordClassifyInfo(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(3), rdr.GetString(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetInt32(8), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), TranslateUtils.ToBool(rdr.GetString(10)), rdr.GetInt32(11), TranslateUtils.ToDateTime(rdr.GetValue(12).ToString()));
                }
                rdr.Close();
            }
            return item;
        }

        public void Delete(int itemID)
        {
            KeywordClassifyInfo itemInfo = this.GetKeywordClassifyInfo(itemID);
            if (itemInfo == null)
                return;
            ArrayList itemIDArrayList = new ArrayList();
            if (itemInfo.ChildrenCount > 0)
            {
                itemIDArrayList = this.GetItemIDArrayListForDescendant(itemID);
            }
            itemIDArrayList.Add(itemID);

            string DELETE_CMD = string.Format("DELETE FROM {1} WHERE ItemID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(itemIDArrayList), TABLE_NAME);

            int deletedNum = 0;

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        deletedNum = this.ExecuteNonQuery(trans, DELETE_CMD);

                        string TAXIS_CMD = string.Format("UPDATE {3} SET Taxis = Taxis - {0} WHERE (Taxis > {1}) AND (PublishmentSystemID = {2})", deletedNum, itemInfo.Taxis, itemInfo.PublishmentSystemID, TABLE_NAME);
                        this.ExecuteNonQuery(trans, TAXIS_CMD);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            //删除内容
            DataProvider.KeywordDAO.DeleteByClassifyID(itemID);

            this.UpdateIsLastItem(itemInfo.ParentID);
            this.UpdateSubtractChildrenCount(itemInfo.ParentsPath, deletedNum);
        }

        public int SetDefaultKeywordClassifyInfo(int publishmentSystemID)
        {
            int id = 0;
            if (this.GetItemIDArrayListByParentID(publishmentSystemID, 0).Count == 0)
            {
                id = this.InsertKeywordClassifyInfo(publishmentSystemID, 0, "全部", "全部");
            }
            return id;
        }
    }
}
