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
    public class WebsiteMessageClassifyDAO : TreeDAO, IWebsiteMessageClassifyDAO
    {
        public WebsiteMessageClassifyDAO()
            : base(TABLE_NAME)
        {

        }

        public const string TABLE_NAME = "siteserver_WebsiteMessageClassify";

        public const string PARM_ADD_DATE = "@AddDate";

        /// <summary>
        /// 使用事务添加节点信息到WebsiteMessageClassify表中
        /// </summary>
        /// <param name="parentWebsiteMessageClassifyInfo">父节点</param>
        /// <param name="websiteMessageClassifyInfo">需要添加的节点</param>
        /// <param name="trans"></param>
        private void InsertWebsiteMessageClassifyInfoWithTrans(WebsiteMessageClassifyInfo parentWebsiteMessageClassifyInfo, WebsiteMessageClassifyInfo websiteMessageClassifyInfo, IDbTransaction trans)
        {
            if (parentWebsiteMessageClassifyInfo != null)
            {
                if (parentWebsiteMessageClassifyInfo.ParentsPath.Length == 0)
                {
                    websiteMessageClassifyInfo.ParentsPath = parentWebsiteMessageClassifyInfo.ItemID.ToString();
                }
                else
                {
                    websiteMessageClassifyInfo.ParentsPath = parentWebsiteMessageClassifyInfo.ParentsPath + "," + parentWebsiteMessageClassifyInfo.ItemID;
                }
                websiteMessageClassifyInfo.ParentsCount = parentWebsiteMessageClassifyInfo.ParentsCount + 1;

                int maxTaxis = base.GetMaxTaxisByParentPath(websiteMessageClassifyInfo.ParentsPath);
                if (maxTaxis == 0)
                {
                    maxTaxis = parentWebsiteMessageClassifyInfo.Taxis;
                }
                websiteMessageClassifyInfo.Taxis = maxTaxis + 1;
            }
            else
            {
                websiteMessageClassifyInfo.ParentsPath = string.Empty;
                websiteMessageClassifyInfo.Taxis = 1;
            }

            string SQL_INSERT_ITEM = "INSERT INTO siteserver_WebsiteMessageClassify (ItemName, ItemIndexName, ParentID, ParentsPath, ParentsCount, ChildrenCount, ContentNum, PublishmentSystemID, Enabled, IsLastItem, Taxis, AddDate) VALUES (@ItemName, @ItemIndexName, @ParentID, @ParentsPath, @ParentsCount, @ChildrenCount, @ContentNum, @PublishmentSystemID, @Enabled, @IsLastItem, @Taxis, @AddDate)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                SQL_INSERT_ITEM = "INSERT INTO siteserver_WebsiteMessageClassify (ItemID, ItemName, ItemIndexName, ParentID, ParentsPath, ParentsCount, ChildrenCount, ContentNum, PublishmentSystemID, Enabled, IsLastItem, Taxis, AddDate) VALUES (siteserver_WebsiteMessageClassify_SEQ.NEXTVAL, @ItemName, @ItemIndexName, @ParentID, @ParentsPath, @ParentsCount, @ChildrenCount, @ContentNum, @PublishmentSystemID, @Enabled, @IsLastItem, @Taxis, @AddDate)";
            }

            IDbDataParameter[] insertParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_ITEM_NAME,EDataType.NVarChar,50,websiteMessageClassifyInfo.ItemName),
                this.GetParameter(PARM_ITEM_INDEX_NAME,EDataType.NVarChar,50,websiteMessageClassifyInfo.ItemIndexName),
                this.GetParameter(PARM_PARENT_ID,EDataType.Integer,websiteMessageClassifyInfo.ParentID),
                this.GetParameter(PARM_PARENTS_PATH,EDataType.NVarChar,255,websiteMessageClassifyInfo.ParentsPath),
                this.GetParameter(PARM_PARENTS_COUNT,EDataType.Integer,websiteMessageClassifyInfo.ParentsCount),
                this.GetParameter(PARM_CHILDREN_COUNT,EDataType.Integer,websiteMessageClassifyInfo.ChildrenCount),
                this.GetParameter(PARM_CONTENT_NUM,EDataType.Integer,websiteMessageClassifyInfo.ContentNum),
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID,EDataType.Integer,websiteMessageClassifyInfo.PublishmentSystemID),
                this.GetParameter(PARM_ENABLED,EDataType.VarChar,18,websiteMessageClassifyInfo.Enabled.ToString()),
                this.GetParameter(PARM_IS_LAST_ITEM,EDataType.VarChar,18,websiteMessageClassifyInfo.IsLastItem.ToString()),
                this.GetParameter(PARM_TAXIS,EDataType.Integer,websiteMessageClassifyInfo.Taxis),
                this.GetParameter(PARM_ADD_DATE,EDataType.DateTime,websiteMessageClassifyInfo.AddDate)
            };

            if (websiteMessageClassifyInfo.ParentID > 0)
            {
                string sqlString = string.Format("UPDATE siteserver_WebsiteMessageClassify SET Taxis = Taxis + 1 WHERE (Taxis >= {0}) AND (PublishmentSystemID = {1})", websiteMessageClassifyInfo.Taxis, websiteMessageClassifyInfo.PublishmentSystemID);
                this.ExecuteNonQuery(trans, sqlString);
            }
            this.ExecuteNonQuery(trans, SQL_INSERT_ITEM, insertParms);

            websiteMessageClassifyInfo.ItemID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "siteserver_WebsiteMessageClassify");

            if (websiteMessageClassifyInfo.ParentsPath != null && websiteMessageClassifyInfo.ParentsPath.Length > 0)
            {
                string sqlString = string.Concat("UPDATE siteserver_WebsiteMessageClassify SET ChildrenCount = ChildrenCount + 1 WHERE ItemID in (", websiteMessageClassifyInfo.ParentsPath, ")");

                this.ExecuteNonQuery(trans, sqlString);
            }

            string SQL_UPDATE_IS_LAST_ITEM = "UPDATE siteserver_WebsiteMessageClassify SET IsLastItem = @IsLastItem WHERE ParentID = @ParentID";

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_IS_LAST_ITEM, EDataType.VarChar, 18, false.ToString()),
                this.GetParameter(PARM_PARENT_ID, EDataType.Integer, websiteMessageClassifyInfo.ParentID)
            };

            this.ExecuteNonQuery(trans, SQL_UPDATE_IS_LAST_ITEM, parms);

            SQL_UPDATE_IS_LAST_ITEM = string.Format("UPDATE siteserver_WebsiteMessageClassify SET IsLastItem = '{0}' WHERE (ItemID IN (SELECT TOP 1 ItemID FROM siteserver_WebsiteMessageClassify WHERE ParentID = {1} ORDER BY Taxis DESC))", true.ToString(), websiteMessageClassifyInfo.ParentID);
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                SQL_UPDATE_IS_LAST_ITEM = string.Format(@"UPDATE siteserver_WebsiteMessageClassify SET IsLastItem = '{0}' WHERE (ItemID IN (
SELECT ItemID FROM (
    SELECT ItemID FROM siteserver_WebsiteMessageClassify WHERE ParentID = {1} ORDER BY Taxis DESC
) WHERE ROWNUM <= 1
))", true.ToString(), websiteMessageClassifyInfo.ParentID);
            }
            this.ExecuteNonQuery(trans, SQL_UPDATE_IS_LAST_ITEM);

        }


        public int InsertWebsiteMessageClassifyInfo(int publishmentSystemID, int parentID, string itemName, string itemIndexName)
        {
            //if (publishmentSystemID > 0 && parentID == 0) return 0;
            WebsiteMessageClassifyInfo websiteMessageClassifyInfo = new WebsiteMessageClassifyInfo();
            websiteMessageClassifyInfo.ParentID = parentID;
            websiteMessageClassifyInfo.PublishmentSystemID = publishmentSystemID;
            websiteMessageClassifyInfo.ItemName = itemName;
            websiteMessageClassifyInfo.ItemIndexName = itemIndexName;
            websiteMessageClassifyInfo.AddDate = DateTime.Now;
            websiteMessageClassifyInfo.Enabled = true;

            WebsiteMessageClassifyInfo parentWebsiteMessageClassifyInfo = this.GetWebsiteMessageClassifyInfo(parentID);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.InsertWebsiteMessageClassifyInfoWithTrans(parentWebsiteMessageClassifyInfo, websiteMessageClassifyInfo, trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return websiteMessageClassifyInfo.ItemID;
        }

        public int InsertWebsiteMessageClassifyInfo(WebsiteMessageClassifyInfo websiteMessageClassifyInfo)
        {
            if (websiteMessageClassifyInfo.PublishmentSystemID > 0 && websiteMessageClassifyInfo.ParentID == 0) return 0;

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        WebsiteMessageClassifyInfo parentWebsiteMessageClassifyInfo = this.GetWebsiteMessageClassifyInfo(websiteMessageClassifyInfo.ParentID);

                        this.InsertWebsiteMessageClassifyInfoWithTrans(parentWebsiteMessageClassifyInfo, websiteMessageClassifyInfo, trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return websiteMessageClassifyInfo.ItemID;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="websiteMessageClassifyInfo"></param>
        public void UpdateWebsiteMessageClassifyInfo(WebsiteMessageClassifyInfo websiteMessageClassifyInfo)
        {
            WebsiteMessageClassifyInfo oldInfo = this.GetWebsiteMessageClassifyInfo(websiteMessageClassifyInfo.ItemID);


            TreeBaseItem oldParentInfo = this.GetItemInfo(websiteMessageClassifyInfo.PublishmentSystemID, oldInfo.ParentID);
            TreeBaseItem newParentInfo = this.GetItemInfo(websiteMessageClassifyInfo.PublishmentSystemID, websiteMessageClassifyInfo.ParentID);

            #region 根据当前父级的信息，修改自己的排序，父级路径
            if (newParentInfo != null)
            {
                if (newParentInfo.ParentsPath.Length == 0)
                {
                    websiteMessageClassifyInfo.ParentsPath = newParentInfo.ItemID.ToString();
                }
                else
                {
                    websiteMessageClassifyInfo.ParentsPath = newParentInfo.ParentsPath + "," + newParentInfo.ItemID;
                }
                websiteMessageClassifyInfo.ParentsCount = newParentInfo.ParentsCount + 1;

                int maxTaxis = this.GetMaxTaxisByParentPath(websiteMessageClassifyInfo.ParentsPath);
                if (maxTaxis == 0)
                {
                    maxTaxis = newParentInfo.Taxis;
                }
                websiteMessageClassifyInfo.Taxis = maxTaxis + 1;
            }
            else
            {
                websiteMessageClassifyInfo.ParentsPath = string.Empty;
                websiteMessageClassifyInfo.Taxis = 1;
            }
            #endregion

            string SQL_UPDATE_ITEM = "UPDATE siteserver_WebsiteMessageClassify SET ItemName=@ItemName, ItemIndexName=@ItemIndexName, ParentID=@ParentID, ParentsPath=@ParentsPath, ParentsCount=@ParentsCount, ChildrenCount=@ChildrenCount, ContentNum=@ContentNum, PublishmentSystemID=@PublishmentSystemID, Enabled=@Enabled, IsLastItem=@IsLastItem, Taxis=@Taxis, AddDate=@AddDate WHERE ItemID=@ItemID";

            IDbDataParameter[] updateParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_ITEM_NAME,EDataType.NVarChar,50,websiteMessageClassifyInfo.ItemName),
                this.GetParameter(PARM_ITEM_INDEX_NAME,EDataType.NVarChar,50,websiteMessageClassifyInfo.ItemIndexName),
                this.GetParameter(PARM_PARENT_ID,EDataType.Integer,websiteMessageClassifyInfo.ParentID),
                this.GetParameter(PARM_PARENTS_PATH,EDataType.NVarChar,255,websiteMessageClassifyInfo.ParentsPath),
                this.GetParameter(PARM_PARENTS_COUNT,EDataType.Integer,websiteMessageClassifyInfo.ParentsCount),
                this.GetParameter(PARM_CHILDREN_COUNT,EDataType.Integer,websiteMessageClassifyInfo.ChildrenCount),
                this.GetParameter(PARM_CONTENT_NUM,EDataType.Integer,websiteMessageClassifyInfo.ContentNum),
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID,EDataType.Integer,websiteMessageClassifyInfo.PublishmentSystemID),
                this.GetParameter(PARM_ENABLED,EDataType.VarChar,18,websiteMessageClassifyInfo.Enabled.ToString()),
                this.GetParameter(PARM_IS_LAST_ITEM,EDataType.VarChar,18,websiteMessageClassifyInfo.IsLastItem.ToString()),
                this.GetParameter(PARM_TAXIS,EDataType.Integer,websiteMessageClassifyInfo.Taxis),
                this.GetParameter(PARM_ADD_DATE,EDataType.DateTime,websiteMessageClassifyInfo.AddDate),
                this.GetParameter(PARM_ITEM_ID,EDataType.Integer,websiteMessageClassifyInfo.ItemID)
            };

            this.ExecuteNonQuery(SQL_UPDATE_ITEM, updateParms);

            if (oldInfo.ParentID != websiteMessageClassifyInfo.ParentID)
            {
                UpdateItemNum(oldParentInfo, newParentInfo);
            }

        }

        public WebsiteMessageClassifyInfo GetWebsiteMessageClassifyInfo(int itemID)
        {
            string SQL_SELECT_ITEM = string.Format("SELECT ItemID, ItemName, ItemIndexName, ParentID, ParentsPath, ParentsCount, ChildrenCount, ContentNum, PublishmentSystemID, Enabled, IsLastItem, Taxis, AddDate FROM {0} WHERE ItemID = @ItemID", TABLE_NAME);

            WebsiteMessageClassifyInfo item = null;

            IDbDataParameter[] itemParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_ITEM_ID, EDataType.Integer, itemID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ITEM, itemParms))
            {
                if (rdr.Read())
                {
                    item = new WebsiteMessageClassifyInfo(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(3), rdr.GetString(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetInt32(8), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), TranslateUtils.ToBool(rdr.GetString(10)), rdr.GetInt32(11), TranslateUtils.ToDateTime(rdr.GetValue(12).ToString()));
                }
                rdr.Close();
            }
            return item;
        }

        public void Delete(int itemID)
        {
            WebsiteMessageClassifyInfo itemInfo = this.GetWebsiteMessageClassifyInfo(itemID);
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
            //DataProvider.WebsiteMessageDAO.DeleteByClassifyID(itemID);

            this.UpdateIsLastItem(itemInfo.ParentID);
            this.UpdateSubtractChildrenCount(itemInfo.ParentsPath, deletedNum);
        }

        public int SetDefaultWebsiteMessageClassifyInfo(int publishmentSystemID)
        {
            int id = 0;
            if (this.GetItemIDArrayListByParentID(publishmentSystemID, 0).Count == 0)
            {
                id = this.InsertWebsiteMessageClassifyInfo(publishmentSystemID, 0, "全部留言", "全部留言");
                //添加一条默认分类
                this.InsertWebsiteMessageClassifyInfo(publishmentSystemID, id, "默认分类", "Default");
            }
            return id;
        }

        /// <summary>
        /// 获取默认分类ID
        /// </summary>
        /// <returns></returns>
        public int GetDefaultClassifyID()
        {
            int defaultID = 0;
            string sql = string.Format("SELECT ItemID FROM {0} WHERE ItemIndexName='Default'", TABLE_NAME);
            object result = base.ExecuteScalar(sql);
            defaultID = TranslateUtils.ToInt(result.ToString());
            return defaultID;
        }
    }
}
