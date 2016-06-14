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
    public class OrganizationClassifyDAO : TreeDAO, IOrganizationClassifyDAO
    {
        public OrganizationClassifyDAO()
            : base(TABLE_NAME)
        {

        }

        public const string TABLE_NAME = OrganizationClassifyInfo.TableName;

        public const string PARM_ADD_DATE = "@" + OrganizationClassifyAttribute.AddDate;

        /// <summary>
        /// 使用事务添加节点信息到OrganizationClassify表中
        /// </summary>
        /// <param name="parentInfo">父节点</param>
        /// <param name="info">需要添加的节点</param>
        /// <param name="trans"></param>
        private void Insert(OrganizationClassifyInfo parentInfo, OrganizationClassifyInfo info, IDbTransaction trans)
        {
            if (parentInfo != null)
            {
                if (parentInfo.ParentsPath.Length == 0)
                {
                    info.ParentsPath = parentInfo.ItemID.ToString();
                }
                else
                {
                    info.ParentsPath = parentInfo.ParentsPath + "," + parentInfo.ItemID;
                }
                info.ParentsCount = parentInfo.ParentsCount + 1;

                int maxTaxis = base.GetMaxTaxisByParentPath(info.ParentsPath);
                if (maxTaxis == 0)
                {
                    maxTaxis = parentInfo.Taxis;
                }
                info.Taxis = maxTaxis + 1;
            }
            else
            {
                info.ParentsPath = "0";
                info.Taxis = 1;
            }

            info.BeforeExecuteNonQuery();
            IDbDataParameter[] insertParms = null;
            string SQL_INSERT_ITEM = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, TABLE_NAME, out insertParms); ;
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                SQL_INSERT_ITEM = "INSERT INTO " + TABLE_NAME + " (ItemID, ItemName, ItemIndexName,IntemCord, ParentID, ParentsPath, ParentsCount, ChildrenCount, ContentNum, PublishmentSystemID, Enabled, IsLastItem, Taxis, AddDate,UserName) VALUES (" + TABLE_NAME + "_SEQ.NEXTVAL, @ItemName, @ItemIndexName,@IntemCord, @ParentID, @ParentsPath, @ParentsCount, @ChildrenCount, @ContentNum, @PublishmentSystemID, @Enabled, @IsLastItem, @Taxis, @AddDate,@UserName)";
            }


            if (info.ParentID > 0)
            {
                string sqlString = string.Format("UPDATE {2} SET Taxis = Taxis + 1 WHERE (Taxis >= {0}) AND (PublishmentSystemID = {1}  )", info.Taxis, info.PublishmentSystemID, TABLE_NAME);
                this.ExecuteNonQuery(trans, sqlString);
            }
            this.ExecuteNonQuery(trans, SQL_INSERT_ITEM, insertParms);

            info.ItemID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, TABLE_NAME);

            if (info.ParentsPath != null && info.ParentsPath.Length > 0)
            {
                string sqlString = string.Concat("UPDATE " + TABLE_NAME + " SET ChildrenCount = ChildrenCount + 1 WHERE ItemID in (", info.ParentsPath, ")");

                this.ExecuteNonQuery(trans, sqlString);
            }

            string SQL_UPDATE_IS_LAST_ITEM = "UPDATE " + TABLE_NAME + " SET IsLastItem = @IsLastItem WHERE ParentID = @ParentID";

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_IS_LAST_ITEM, EDataType.VarChar, 18, false.ToString()),
                this.GetParameter(PARM_PARENT_ID, EDataType.Integer, info.ParentID)
            };

            this.ExecuteNonQuery(trans, SQL_UPDATE_IS_LAST_ITEM, parms);

            SQL_UPDATE_IS_LAST_ITEM = string.Format("UPDATE {2} SET IsLastItem = '{0}' WHERE (ItemID IN (SELECT TOP 1 ItemID FROM {2} WHERE ParentID = {1} ORDER BY Taxis DESC))", true.ToString(), info.ParentID, TABLE_NAME);
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                SQL_UPDATE_IS_LAST_ITEM = string.Format(@"UPDATE {2} SET IsLastItem = '{0}' WHERE (ItemID IN (
SELECT ItemID FROM (
    SELECT ItemID FROM {2} WHERE ParentID = {1} ORDER BY Taxis DESC
) WHERE ROWNUM <= 1
))", true.ToString(), info.ParentID, TABLE_NAME);
            }
            this.ExecuteNonQuery(trans, SQL_UPDATE_IS_LAST_ITEM);

        }


        public int Insert(int publishmentSystemID, int parentID, string itemName, string itemIndexName)
        {
            OrganizationClassifyInfo info = new OrganizationClassifyInfo();
            info.ParentID = parentID;
            info.PublishmentSystemID = publishmentSystemID;
            info.ItemName = itemName;
            info.ItemIndexName = itemIndexName;
            info.AddDate = DateTime.Now;
            info.Enabled = true;
            info.ParentsPath = "0";

            OrganizationClassifyInfo parentInfo = this.GetInfo(parentID);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.Insert(parentInfo, info, trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return info.ItemID;
        }

        public int Insert(OrganizationClassifyInfo info)
        {
            if (info.PublishmentSystemID > 0 && info.ParentID == 0) return 0;

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        OrganizationClassifyInfo parentInfo = this.GetInfo(info.ParentID);

                        this.Insert(parentInfo, info, trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return info.ItemID;
        }

        public void Update(OrganizationClassifyInfo info)
        {
            OrganizationClassifyInfo oldInfo = this.GetInfo(info.ItemID);


            TreeBaseItem oldParentInfo = this.GetItemInfo(info.PublishmentSystemID, oldInfo.ParentID);
            TreeBaseItem newParentInfo = this.GetItemInfo(info.PublishmentSystemID, info.ParentID);

            #region 根据当前父级的信息，修改自己的排序，父级路径
            if (newParentInfo != null)
            {
                if (newParentInfo.ParentsPath.Length == 0)
                {
                    info.ParentsPath = newParentInfo.ItemID.ToString();
                }
                else
                {
                    info.ParentsPath = newParentInfo.ParentsPath + "," + newParentInfo.ItemID;
                }
                info.ParentsCount = newParentInfo.ParentsCount + 1;
                if (info.Taxis == 0)
                {
                    int maxTaxis = this.GetMaxTaxisByParentPath(info.ParentsPath);
                    if (maxTaxis == 0)
                    {
                        maxTaxis = newParentInfo.Taxis + 1;
                    }
                    info.Taxis = maxTaxis + 1;
                }
            }
            else
            {
                info.ParentsPath = string.Empty;
                info.Taxis = 1;
            }
            #endregion

            info.BeforeExecuteNonQuery();
            IDbDataParameter[] updateParms = null;
            string SQL_UPDATE_ITEM = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(info.Attributes, TABLE_NAME, out updateParms);


            this.ExecuteNonQuery(SQL_UPDATE_ITEM, updateParms);

            if (oldInfo.ParentID != info.ParentID)
            {
                UpdateItemNum(oldParentInfo, newParentInfo);
            }

        }

        public OrganizationClassifyInfo GetInfo(int itemID)
        {
            string SQL_WHERE = string.Format("WHERE ItemID = {0}", itemID);
            string SQL_SELECT_ITEM = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, SqlUtils.Asterisk, SQL_WHERE);

            OrganizationClassifyInfo item = null;


            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ITEM))
            {
                if (rdr.Read())
                {
                    item = new OrganizationClassifyInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, item);
                }
                rdr.Close();
            }
            return item;
        }

        public OrganizationClassifyInfo GetInfoByNew(int itemID)
        {
            string SQL_WHERE = string.Format("WHERE ItemID = {0}", itemID);
            string SQL_SELECT_ITEM = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, SqlUtils.Asterisk, SQL_WHERE);

            OrganizationClassifyInfo item = null;


            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ITEM))
            {
                if (rdr.Read())
                {
                    item = new OrganizationClassifyInfo(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(3), rdr.GetString(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetInt32(8), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), TranslateUtils.ToBool(rdr.GetString(10)), rdr.GetInt32(11), TranslateUtils.ToDateTime(rdr.GetValue(12).ToString()), rdr.GetValue(13).ToString());
                }
                rdr.Close();
            }
            return item;
        }
        public void Delete(int itemID)
        {
            OrganizationClassifyInfo itemInfo = this.GetInfo(itemID);

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
            this.UpdateIsLastItem(itemInfo.ParentID);
            this.UpdateSubtractChildrenCount(itemInfo.ParentsPath, deletedNum);

            //删除区域
            DataProvider.OrganizationAreaDAO.DeleteByClassifyID(itemID);
            //删除学习中心
            DataProvider.OrganizationInfoDAO.DeleteByClassifyID(itemID);

        }

        public int SetDefaultInfo(int publishmentSystemID)
        {
            int id = 0;
            if (this.GetItemIDArrayListByParentID(publishmentSystemID, 0).Count == 0)
            {
                id = this.Insert(publishmentSystemID, 0, "全部分类", "全部分类");
            }
            return id;
        }


        public OrganizationClassifyInfo GetFirstInfo()
        {
            string SQL_WHERE = string.Format("WHERE ParentID = 0");
            string SQL_SELECT_ITEM = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, SqlUtils.Asterisk, SQL_WHERE);

            OrganizationClassifyInfo item = null;


            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ITEM))
            {
                if (rdr.Read())
                {
                    item = new OrganizationClassifyInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, item);
                }
                rdr.Close();
            }
            return item;
        }


        #region 前台方法

        public ArrayList GetInfoList(int parentID)
        {
            string SQL_WHERE = string.Format("WHERE 1=1 and ParentID = {0} order by Taxis", parentID);
            string SQL_SELECT_ITEM = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, SqlUtils.Asterisk, SQL_WHERE);

            ArrayList list = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ITEM))
            {
                while (rdr.Read())
                {
                    OrganizationClassifyInfo item = new OrganizationClassifyInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, item);
                    list.Add(item);
                }
                rdr.Close();
            }
            return list;
        }

        public ArrayList GetInfoList(string whereStr)
        {
            string SQL_WHERE = string.Format("WHERE 1=1 {0} order by Taxis", whereStr);
            string SQL_SELECT_ITEM = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, SqlUtils.Asterisk, SQL_WHERE);

            ArrayList list = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ITEM))
            {
                while (rdr.Read())
                {
                    OrganizationClassifyInfo item = new OrganizationClassifyInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, item);
                    list.Add(item);
                }
                rdr.Close();
            }
            return list;
        }

        #endregion
    }
}
