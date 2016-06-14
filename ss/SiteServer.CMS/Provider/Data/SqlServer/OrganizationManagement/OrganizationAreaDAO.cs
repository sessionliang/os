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
    public class OrganizationAreaDAO : TreeDAO, IOrganizationAreaDAO
    {
        public OrganizationAreaDAO()
            : base(TABLE_NAME)
        {

        }

        public const string TABLE_NAME = OrganizationAreaInfo.TableName;


        public const string PARM_CLASSIFY_ID = "@" + OrganizationAreaAttribute.ClassifyID;

        /// <summary>
        /// 使用事务添加节点信息到OrganizationArea表中
        /// </summary>
        /// <param name="parentInfo">父节点</param>
        /// <param name="info">需要添加的节点</param>
        /// <param name="trans"></param>
        private void Insert(OrganizationAreaInfo parentInfo, OrganizationAreaInfo info, IDbTransaction trans)
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
                info.Taxis = GetMaxTaxis(info.ParentID, info.ClassifyID) + 1;
            }

            info.BeforeExecuteNonQuery();
            IDbDataParameter[] insertParms = null;
            string SQL_INSERT_ITEM = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, TABLE_NAME, out insertParms);  
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                SQL_INSERT_ITEM = "INSERT INTO " + TABLE_NAME + " (ItemID, ItemName, ItemIndexName, ParentID, ParentsPath, ParentsCount, ChildrenCount, ContentNum, ClassifyID,PublishmentSystemID, Enabled, IsLastItem, Taxis, AddDate,UserName) VALUES (" + TABLE_NAME + "_SEQ.NEXTVAL, @ItemName, @ItemIndexName, @ParentID, @ParentsPath, @ParentsCount, @ChildrenCount, @ContentNum, @ClassifyID,@PublishmentSystemID, @Enabled, @IsLastItem, @Taxis, @AddDate,@UserName)";
            }


            if (info.ParentID > 0)
            {
                string sqlString = string.Format("UPDATE {2} SET Taxis = Taxis + 1 WHERE (Taxis >= {0}) AND (PublishmentSystemID = {1} and ClassifyID={3} )", info.Taxis, info.PublishmentSystemID, TABLE_NAME, info.ClassifyID);
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
            OrganizationAreaInfo info = new OrganizationAreaInfo();
            info.ParentID = parentID;
            info.PublishmentSystemID = publishmentSystemID;
            info.ItemName = itemName;
            info.ItemIndexName = itemIndexName;
            info.AddDate = DateTime.Now;
            info.Enabled = true;
            info.ParentsPath = "0";

            OrganizationAreaInfo parentInfo = this.GetInfo(parentID);

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

        public int Insert(OrganizationAreaInfo info)
        {
            if (info.PublishmentSystemID > 0 && info.ParentID == 0) return 0;

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        OrganizationAreaInfo parentInfo = this.GetInfo(info.ParentID);

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

        public void Update(OrganizationAreaInfo info)
        {
            OrganizationAreaInfo oldInfo = this.GetInfo(info.ItemID);


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
                        maxTaxis = newParentInfo.Taxis;
                    }
                    info.Taxis = maxTaxis + 1;
                }
            }
            //else
            //{
            //    info.ParentsPath = string.Empty; 
            //    info.Taxis = GetMaxTaxis(info.ParentID, info.ClassifyID) + 1;
            //}
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

        /// <summary>
        /// 获取分类下某个级最在的排序号
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="classifyID"></param>
        /// <returns></returns>
        private int GetMaxTaxis(int parentID, int classifyID)
        {
            string sqlString = string.Empty;
            if (classifyID > 0)
                sqlString = string.Format("SELECT MAX(Taxis) FROM {0} WHERE  ClassifyID = {1} and ParentID={2} ", TABLE_NAME, classifyID, parentID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public OrganizationAreaInfo GetInfo(int itemID)
        {
            string SQL_WHERE = string.Format("WHERE ItemID = {0}", itemID);
            string SQL_SELECT_ITEM = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, SqlUtils.Asterisk, SQL_WHERE);

            OrganizationAreaInfo item = null;


            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ITEM))
            {
                if (rdr.Read())
                {
                    item = new OrganizationAreaInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, item);
                }
                rdr.Close();
            }
            return item;
        }

        public OrganizationAreaInfo GetInfoByNew(int itemID)
        {
            string SQL_WHERE = string.Format("WHERE ItemID = {0}", itemID);
            string SQL_SELECT_ITEM = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, SqlUtils.Asterisk, SQL_WHERE);

            OrganizationAreaInfo item = null;


            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ITEM))
            {
                if (rdr.Read())
                {
                    item = new OrganizationAreaInfo(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(3), rdr.GetString(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetInt32(8), rdr.GetInt32(9), TranslateUtils.ToBool(rdr.GetValue(10).ToString()), TranslateUtils.ToBool(rdr.GetString(11)), rdr.GetInt32(12), TranslateUtils.ToDateTime(rdr.GetValue(13).ToString()), rdr.GetValue(14).ToString());
                }
                rdr.Close();
            }
            return item;
        }
        public void Delete(int itemID)
        {
            OrganizationAreaInfo itemInfo = this.GetInfo(itemID);

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

        public void DeleteByClassifyID(int classifyID)
        {
            string DELETE_CMD = string.Format("DELETE FROM {1} WHERE ClassifyID = ({0})", classifyID, TABLE_NAME);
            this.ExecuteNonQuery(DELETE_CMD);
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


        public OrganizationAreaInfo GetFirstInfo()
        {
            string SQL_WHERE = string.Format("WHERE ParentID = 0");
            string SQL_SELECT_ITEM = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, SqlUtils.Asterisk, SQL_WHERE);

            OrganizationAreaInfo item = null;


            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ITEM))
            {
                if (rdr.Read())
                {
                    item = new OrganizationAreaInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, item);
                }
                rdr.Close();
            }
            return item;
        }


        public bool IsExists(string itemName, int classifyID)
        {
            bool isExists = false;

            string SQL_WHERE = string.Format("WHERE ItemName = '{0}' and ClassifyID={1}", itemName, classifyID);
            string SQL_SELECT_ITEM = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ITEM))
            {
                if (rdr.Read())
                {
                    isExists = true;
                }
                rdr.Close();
            }
            return isExists;
        }

        public ArrayList GetItemIDArrayListByParentID(int publishmentSystemID, int parentID, int classifyID)
        {
            ArrayList list = new ArrayList();
            string sqlString = string.Empty;

            sqlString = string.Format(@"SELECT ItemID
FROM {2}
WHERE (ParentID = {1}) AND PublishmentSystemID = {0} and ClassifyID={3}
ORDER BY Taxis", publishmentSystemID, parentID, TABLE_NAME, classifyID);

            ArrayList arraylist = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int itemID = Convert.ToInt32(rdr[0]);
                    arraylist.Add(itemID);
                }
                rdr.Close();
            }

            return arraylist;
        }


        public string getArea(int publishmentSystemID, int itemID)
        {
            ArrayList list = new ArrayList();
            string sqlString = string.Empty;

            sqlString = string.Format(@"SELECT * FROM {0} WHERE   PublishmentSystemID = {1} and ItemID={2} ORDER BY Taxis", TABLE_NAME, publishmentSystemID, itemID);

            ArrayList arraylist = new ArrayList();
            string areaName = "";
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int parentID = Convert.ToInt32(rdr[3]);
                    areaName = rdr[1].ToString();
                    if (parentID != 0)
                        areaName = getArea(publishmentSystemID, parentID) + "-" + areaName;
                }
                rdr.Close();
            }
            return areaName;
        }



        public void UpdateTaxis(int publishmentSystemID, int classifyID, int itemID, bool isSubtract)
        {
            if (isSubtract)
            {
                this.TaxisSubtract(publishmentSystemID, classifyID, itemID);
            }
            else
            {
                this.TaxisAdd(publishmentSystemID, classifyID, itemID);
            }
        }


        /// <summary>
        /// Change The Texis To Lowerer Level
        /// </summary>
        public void TaxisSubtract(int publishmentSystemID, int classifyID, int selectedItemID)
        {
            OrganizationAreaInfo itemInfo = this.GetInfo(selectedItemID);
            if (itemInfo == null) return;
            this.UpdateWholeTaxisByPublishmentSystemID(itemInfo.PublishmentSystemID);
            //Get Lower Taxis and ItemID
            int lowerItemID = 0;
            int lowerChildrenCount = 0;
            string lowerParentsPath = "";
            string sqlString = string.Format(@"SELECT TOP 1 ItemID, ChildrenCount, ParentsPath
FROM {0}
WHERE (ParentID = @ParentID) AND (ItemID <> @ItemID) AND (Taxis < @Taxis) AND (PublishmentSystemID = @PublishmentSystemID and ClassifyID=@ClassifyID)
ORDER BY Taxis DESC", TABLE_NAME);

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(TreeDAO.PARM_PARENT_ID, EDataType.Integer, itemInfo.ParentID),
                this.GetParameter(TreeDAO.PARM_ITEM_ID, EDataType.Integer, itemInfo.ItemID),
                this.GetParameter(TreeDAO.PARM_TAXIS, EDataType.Integer, itemInfo.Taxis),
                this.GetParameter(TreeDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, itemInfo.PublishmentSystemID),
                this.GetParameter("@ClassifyID", EDataType.Integer, classifyID)
            };

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    lowerItemID = Convert.ToInt32(rdr[0]);
                    lowerChildrenCount = Convert.ToInt32(rdr[1]);
                    lowerParentsPath = rdr.GetValue(2).ToString();
                }
                else
                {
                    return;
                }
                rdr.Close();
            }


            string lowerItemPath = "";
            if (lowerParentsPath == "")
            {
                lowerItemPath = lowerItemID.ToString();
            }
            else
            {
                lowerItemPath = String.Concat(lowerParentsPath, ",", lowerItemID);
            }
            string selectedItemPath = "";
            if (itemInfo.ParentsPath == "")
            {
                selectedItemPath = itemInfo.ItemID.ToString();
            }
            else
            {
                selectedItemPath = String.Concat(itemInfo.ParentsPath, ",", itemInfo.ItemID);
            }

            this.SetTaxisSubtract(selectedItemID, selectedItemPath, lowerChildrenCount + 1);
            this.SetTaxisAdd(lowerItemID, lowerItemPath, itemInfo.ChildrenCount + 1);

            this.UpdateIsLastItem(itemInfo.ParentID);

        }

        /// <summary>
        /// Change The Texis To Higher Level
        /// </summary>
        public void TaxisAdd(int publishmentSystemID, int classifyID, int selectedItemID)
        {
            OrganizationAreaInfo itemInfo = this.GetInfo(selectedItemID);
            if (itemInfo == null) return;
            this.UpdateWholeTaxisByPublishmentSystemID(itemInfo.PublishmentSystemID);
            //Get Higher Taxis and ItemID
            int higherItemID = 0;
            int higherChildrenCount = 0;
            string higherParentsPath = "";
            string sqlString = string.Format(@"SELECT TOP 1 ItemID, ChildrenCount, ParentsPath
FROM {0}
WHERE (ParentID = @ParentID) AND (ItemID <> @ItemID) AND (Taxis > @Taxis) AND (PublishmentSystemID = @PublishmentSystemID and ClassifyID=@ClassifyID)
ORDER BY Taxis", TABLE_NAME);

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(TreeDAO.PARM_PARENT_ID, EDataType.Integer, itemInfo.ParentID),
                this.GetParameter(TreeDAO.PARM_ITEM_ID, EDataType.Integer, itemInfo.ItemID),
                this.GetParameter(TreeDAO.PARM_TAXIS, EDataType.Integer, itemInfo.Taxis),
                this.GetParameter(TreeDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, itemInfo.PublishmentSystemID),
                this.GetParameter("@ClassifyID", EDataType.Integer, classifyID)
            };

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    higherItemID = Convert.ToInt32(rdr[0]);
                    higherChildrenCount = Convert.ToInt32(rdr[1]);
                    higherParentsPath = rdr.GetValue(2).ToString();
                }
                else
                {
                    return;
                }
                rdr.Close();
            }


            string higherItemPath = string.Empty;
            if (higherParentsPath == string.Empty)
            {
                higherItemPath = higherItemID.ToString();
            }
            else
            {
                higherItemPath = String.Concat(higherParentsPath, ",", higherItemID);
            }
            string selectedItemPath = string.Empty;
            if (itemInfo.ParentsPath == string.Empty)
            {
                selectedItemPath = itemInfo.ItemID.ToString();
            }
            else
            {
                selectedItemPath = String.Concat(itemInfo.ParentsPath, ",", itemInfo.ItemID);
            }

            this.SetTaxisAdd(selectedItemID, selectedItemPath, higherChildrenCount + 1);
            this.SetTaxisSubtract(higherItemID, higherItemPath, itemInfo.ChildrenCount + 1);

            this.UpdateIsLastItem(itemInfo.ParentID);
        }

        #region 前台方法

        /// <summary>
        /// 获取上级区域
        /// </summary>
        /// <param name="classifyID"></param>
        /// <returns></returns>
        public ArrayList getParentArea(int classifyID)
        {
            ArrayList list = new ArrayList();
            string sqlString = string.Empty;

            string whereStr = "";
            if (classifyID > 0)
                whereStr = " and ClassifyID = " + classifyID;

            sqlString = string.Format(@"SELECT * FROM {0} WHERE ParentID=0 {1} ORDER BY Taxis", TABLE_NAME, whereStr);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    OrganizationAreaInfo info = new OrganizationAreaInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    list.Add(info);
                }
                rdr.Close();
            }
            return list;
        }

        /// <summary>
        /// 获取子级区域
        /// </summary>
        /// <param name="parenID"></param>
        /// <returns></returns>
        public ArrayList getChildArea(int parenID)
        {
            ArrayList list = new ArrayList();
            string sqlString = string.Empty;

            sqlString = string.Format(@"SELECT * FROM {0} WHERE ParentID = {1} ORDER BY Taxis", TABLE_NAME, parenID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    OrganizationAreaInfo info = new OrganizationAreaInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    list.Add(info);
                }
                rdr.Close();
            }
            return list;
        }
        #endregion
    }
}
