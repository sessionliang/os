using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;
using ECountType = SiteServer.WeiXin.Model.ECountType;
using ECountTypeUtils = SiteServer.WeiXin.Model.ECountTypeUtils;
using BaiRong.Model;
using System.Text;

namespace SiteServer.WeiXin.Provider.Data.SqlServer
{
    public class StoreCategoryDAO : DataProviderBase, IStoreCategoryDAO
    {
        private const string TABLE_NAME = "wx_StoreCategory";

        private const string SQL_SELECT = "SELECT ID, PublishmentSystemID, CategoryName, ParentID, Taxis, ChildCount, ParentsCount, ParentsPath, StoreCount, IsLastNode FROM wx_StoreCategory WHERE ID = @ID";

        private const string SQL_SELECT_ALL = "SELECT ID, PublishmentSystemID, CategoryName, ParentID, Taxis, ChildCount, ParentsCount, ParentsPath, StoreCount, IsLastNode FROM wx_StoreCategory WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY TAXIS";

        private const string SQL_SELECT_NAME = "SELECT CategoryName FROM wx_StoreCategory WHERE ID = @ID";

        private const string SQL_SELECT_ID = "SELECT ID FROM wx_StoreCategory WHERE ID = @ID";

        private const string SQL_SELECT_PARENT_ID = "SELECT ParentID FROM wx_StoreCategory WHERE ID = @ID";

        private const string SQL_SELECT_COUNT = "SELECT COUNT(*) FROM wx_StoreCategory WHERE PublishmentSystemID = @PublishmentSystemID AND ParentID = @ParentID";

        private const string SQL_UPDATE = "UPDATE wx_StoreCategory SET CategoryName = @CategoryName, ParentsPath = @ParentsPath, ParentsCount = @ParentsCount, ChildCount = @ChildCount, IsLastNode = @IsLastNode WHERE ID = @ID";

        private const string PARM_ID = "@ID";
        private const string PARM_PUBLISHIMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_NAME = "@CategoryName";
        private const string PARM_PARENT_ID = "@ParentID";
        private const string PARM_PARENTS_PATH = "@ParentsPath";
        private const string PARM_PARENTS_COUNT = "@ParentsCount";
        private const string PARM_CHILDREN_COUNT = "@ChildCount";
        private const string PARM_IS_LAST_NODE = "@IsLastNode";
        private const string PARM_TAXIS = "@Taxis";


        public int Insert(StoreCategoryInfo storeCategoryInfo)
        {
            int storeCategoryID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(storeCategoryInfo.ToNameValueCollection(), this.ConnectionString, StoreCategoryDAO.TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        storeCategoryID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, StoreCategoryDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return storeCategoryID;
        }

        private void InsertWithTrans(int publishmentSystemID, StoreCategoryInfo parentInfo, StoreCategoryInfo categoryInfo, IDbTransaction trans)
        {
            if (parentInfo != null)
            {
                categoryInfo.ParentsPath = parentInfo.ParentsPath + "," + parentInfo.ID;
                categoryInfo.ParentsCount = parentInfo.ParentsCount + 1;

                int maxTaxis = this.GetMaxTaxisByParentPath(publishmentSystemID, categoryInfo.ParentsPath);
                if (maxTaxis == 0)
                {
                    maxTaxis = parentInfo.Taxis;
                }
                categoryInfo.Taxis = maxTaxis + 1;
            }
            else
            {
                categoryInfo.ParentsPath = "0";
                categoryInfo.ParentsCount = 0;
                int maxTaxis = this.GetMaxTaxisByParentPath(publishmentSystemID, "0");
                categoryInfo.Taxis = maxTaxis + 1;
            }

            string SQL_INSERT = "INSERT INTO wx_StoreCategory (PublishmentSystemID, CategoryName, ParentID, ParentsPath, ParentsCount, ChildCount, IsLastNode, Taxis) VALUES (@PublishmentSystemID, @CategoryName, @ParentID, @ParentsPath, @ParentsCount, @ChildCount, @IsLastNode, @Taxis)";

            IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHIMENTSYSTEMID, EDataType.Integer, categoryInfo.PublishmentSystemID),
				this.GetParameter(PARM_NAME, EDataType.NVarChar, 255, categoryInfo.CategoryName),
				this.GetParameter(PARM_PARENT_ID, EDataType.Integer, categoryInfo.ParentID),
				this.GetParameter(PARM_PARENTS_PATH, EDataType.NVarChar, 255, categoryInfo.ParentsPath),
				this.GetParameter(PARM_PARENTS_COUNT, EDataType.Integer, categoryInfo.ParentsCount),
				this.GetParameter(PARM_CHILDREN_COUNT, EDataType.Integer, 0),
				this.GetParameter(PARM_IS_LAST_NODE, EDataType.VarChar, 18, true.ToString()),
				this.GetParameter(PARM_TAXIS, EDataType.Integer, categoryInfo.Taxis),
			};

            string sqlString = string.Format("UPDATE wx_StoreCategory SET Taxis = Taxis + 1 WHERE (Taxis >= {0})", categoryInfo.Taxis);
            this.ExecuteNonQuery(trans, sqlString);

            this.ExecuteNonQuery(trans, SQL_INSERT, insertParms);

            categoryInfo.ID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "wx_StoreCategory");

            if (!string.IsNullOrEmpty(categoryInfo.ParentsPath))
            {
                sqlString = string.Concat("UPDATE wx_StoreCategory SET ChildCount = ChildCount + 1 WHERE ID in (", categoryInfo.ParentsPath, ")");

                this.ExecuteNonQuery(trans, sqlString);
            }

            sqlString = string.Format("UPDATE wx_StoreCategory SET IsLastNode = 'False' WHERE ParentID = {0}", categoryInfo.ParentID);

            this.ExecuteNonQuery(trans, sqlString);

            sqlString = string.Format("UPDATE wx_StoreCategory SET IsLastNode = 'True' WHERE (ID IN (SELECT TOP 1 ID FROM wx_StoreCategory WHERE ParentID = {0} ORDER BY Taxis DESC))", categoryInfo.ParentID);
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = string.Format(@"UPDATE wx_StoreCategory SET IsLastNode = 'True' WHERE (ID IN (
SELECT ID FROM (
    SELECT ID FROM wx_StoreCategory WHERE ParentID = {0} ORDER BY Taxis DESC
) WHERE ROWNUM <= 1
))", categoryInfo.ParentID);
            }
            this.ExecuteNonQuery(trans, sqlString);
        }

        private void UpdateSubtractChildCount(int publishmentSystemID, string parentsPath, int subtractNum)
        {
            if (!string.IsNullOrEmpty(parentsPath))
            {
                string sqlString = string.Concat("UPDATE wx_StoreCategory SET ChildCount = ChildCount - ", subtractNum, " WHERE ID IN (", parentsPath, ")");
                this.ExecuteNonQuery(sqlString);
            }
        }

        public StoreCategoryInfo GetStoreCategoryInfo(int storeID)
        {
            StoreCategoryInfo storeCategoryInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", storeID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, StoreCategoryDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    storeCategoryInfo = new StoreCategoryInfo(rdr);
                }
                rdr.Close();
            }

            return storeCategoryInfo;
        }

        public StoreCategoryInfo GetStoreCategoryInfoByParentID(int publishmentSystemID, int parentID)
        {
            StoreCategoryInfo storeCategoryInfo = null;

            string SQL_WHERE = string.Format("WHERE PublishmentSystemID = {0} AND ParentID = {1}", publishmentSystemID, parentID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, StoreCategoryDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    storeCategoryInfo = new StoreCategoryInfo(rdr);
                }
                rdr.Close();
            }

            return storeCategoryInfo;
        }

        public string GetSelectString(int publishmentSystemID)
        {
            string whereString = string.Format("WHERE {0} = {1}", StoreCategoryAttribute.PublishmentSystemID, publishmentSystemID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(StoreCategoryDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public Dictionary<string, int> GetStoreCategoryDictionary(int publishmentSystemID)
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();

            string SQL_WHERE = string.Format(" WHERE {0} = {1}", StoreCategoryAttribute.PublishmentSystemID, publishmentSystemID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, StoreCategoryAttribute.CategoryName + "," + StoreCategoryAttribute.Taxis, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    dictionary.Add(rdr.GetValue(0).ToString(), rdr.GetInt32(1));
                }
                rdr.Close();
            }

            return dictionary;
        }

        public List<StoreCategoryInfo> GetStoreCategoryInfoList(int publishmentSystemID, int parentID)
        {
            List<StoreCategoryInfo> list = new List<StoreCategoryInfo>();

            StringBuilder builder = new StringBuilder(string.Format("WHERE {0} = {1} AND {2} = {3} ", StoreCategoryAttribute.PublishmentSystemID, publishmentSystemID, StoreCategoryAttribute.ParentID, parentID));
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, StoreCategoryDAO.TABLE_NAME, 0, SqlUtils.Asterisk, builder.ToString(), "ORDER BY Taxis");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    StoreCategoryInfo categoryInfo = new StoreCategoryInfo(rdr);
                    list.Add(categoryInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public List<StoreCategoryInfo> GetAllStoreCategoryInfoList(int publishmentSystemID)
        {
            List<StoreCategoryInfo> list = new List<StoreCategoryInfo>();

            StringBuilder builder = new StringBuilder(string.Format("WHERE {0} = {1}", StoreCategoryAttribute.PublishmentSystemID, publishmentSystemID));
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, StoreCategoryDAO.TABLE_NAME, 0, SqlUtils.Asterisk, builder.ToString(), "ORDER BY ID");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    StoreCategoryInfo StoreCategoryInfo = new StoreCategoryInfo(rdr);
                    list.Add(StoreCategoryInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public string GetCategoryName(int storeID)
        {
            string title = string.Empty;

            string SQL_WHERE = string.Format("WHERE ID = {0}", storeID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, StoreCategoryDAO.TABLE_NAME, 0, StoreCategoryAttribute.CategoryName, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    title = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }

            return title;
        }

        private void TaxisSubtract(int publishmentSystemID, int selectedID)
        {
            StoreCategoryInfo categoryInfo = this.GetCategoryInfo(selectedID);
            if (categoryInfo == null) return;
            //Get Lower Taxis and ID
            int lowerID = 0;
            int lowerChildCount = 0;
            string lowerParentsPath = "";
            string sqlString = @"SELECT TOP 1 ID, ChildCount, ParentsPath
FROM wx_StoreCategory
WHERE PublishmentSystemID = @PublishmentSystemID AND ParentID = @ParentID AND ID <> @ID AND Taxis < @Taxis
ORDER BY Taxis DESC";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHIMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_PARENT_ID, EDataType.Integer, categoryInfo.ParentID),
				this.GetParameter(PARM_ID, EDataType.Integer, categoryInfo.ID),
				this.GetParameter(PARM_TAXIS, EDataType.Integer, categoryInfo.Taxis),
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    lowerID = Convert.ToInt32(rdr[0]);
                    lowerChildCount = Convert.ToInt32(rdr[1]);
                    lowerParentsPath = rdr.GetValue(2).ToString();
                }
                else
                {
                    return;
                }
                rdr.Close();
            }

            string lowerNodePath = String.Concat(lowerParentsPath, ",", lowerID);
            string selectedNodePath = String.Concat(categoryInfo.ParentsPath, ",", categoryInfo.ID);

            this.SetTaxisSubtract(publishmentSystemID, selectedID, selectedNodePath, lowerChildCount + 1);
            this.SetTaxisAdd(publishmentSystemID, lowerID, lowerNodePath, categoryInfo.ChildCount + 1);

            this.UpdateIsLastNode(publishmentSystemID, categoryInfo.ParentID);
        }

        private void TaxisAdd(int publishmentSystemID, int selectedID)
        {
            StoreCategoryInfo categoryInfo = this.GetCategoryInfo(selectedID);
            if (categoryInfo == null) return;
            //Get Higher Taxis and ID
            int higherID = 0;
            int higherChildCount = 0;
            string higherParentsPath = "";
            string sqlString = @"SELECT TOP 1 ID, ChildCount, ParentsPath
FROM wx_StoreCategory
WHERE PublishmentSystemID = @PublishmentSystemID AND ParentID = @ParentID AND ID <> @ID AND Taxis > @Taxis
ORDER BY Taxis";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHIMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_PARENT_ID, EDataType.Integer, categoryInfo.ParentID),
				this.GetParameter(PARM_ID, EDataType.Integer, categoryInfo.ID),
				this.GetParameter(PARM_TAXIS, EDataType.Integer, categoryInfo.Taxis)
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    higherID = Convert.ToInt32(rdr[0]);
                    higherChildCount = Convert.ToInt32(rdr[1]);
                    higherParentsPath = rdr.GetValue(2).ToString();
                }
                else
                {
                    return;
                }
                rdr.Close();
            }

            string higherNodePath = String.Concat(higherParentsPath, ",", higherID);
            string selectedNodePath = String.Concat(categoryInfo.ParentsPath, ",", categoryInfo.ID);

            this.SetTaxisAdd(publishmentSystemID, selectedID, selectedNodePath, higherChildCount + 1);
            this.SetTaxisSubtract(publishmentSystemID, higherID, higherNodePath, categoryInfo.ChildCount + 1);

            this.UpdateIsLastNode(publishmentSystemID, categoryInfo.ParentID);
        }

        private void SetTaxisAdd(int publishmentSystemID, int id, string parentsPath, int addNum)
        {
            string sqlString = string.Format("UPDATE wx_StoreCategory SET Taxis = Taxis + {0} WHERE PublishmentSystemID = {1} AND (ID = {2} OR ParentsPath = '{3}' OR ParentsPath like '{3},%')", addNum, publishmentSystemID, id, parentsPath);

            this.ExecuteNonQuery(sqlString);
        }

        private void SetTaxisSubtract(int publishmentSystemID, int id, string parentsPath, int subtractNum)
        {
            string sqlString = string.Format("UPDATE wx_StoreCategory SET Taxis = Taxis - {0} WHERE PublishmentSystemID = {1} AND (ID = {2} OR ParentsPath = '{3}' OR ParentsPath like '{3},%')", subtractNum, publishmentSystemID, id, parentsPath);

            this.ExecuteNonQuery(sqlString);
        }

        private void UpdateIsLastNode(int publishmentSystemID, int parentID)
        {
            if (parentID > 0)
            {
                string sqlString = "UPDATE wx_StoreCategory SET IsLastNode = @IsLastNode WHERE PublishmentSystemID = @PublishmentSystemID AND ParentID = @ParentID";

                IDbDataParameter[] parms = new IDbDataParameter[]
			    {
				    this.GetParameter(PARM_IS_LAST_NODE, EDataType.VarChar, 18, false.ToString()),
                    this.GetParameter(PARM_PUBLISHIMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
				    this.GetParameter(PARM_PARENT_ID, EDataType.Integer, parentID)
			    };

                this.ExecuteNonQuery(sqlString, parms);

                sqlString = string.Format("UPDATE wx_StoreCategory SET IsLastNode = '{0}' WHERE (ID IN (SELECT TOP 1 ID FROM wx_StoreCategory WHERE ParentID = {1} ORDER BY Taxis DESC))", true.ToString(), parentID);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    sqlString = string.Format(@"UPDATE wx_StoreCategory SET IsLastNode = '{0}' WHERE (ID IN (
SELECT ID FROM (
    SELECT ID FROM wx_StoreCategory WHERE ParentID = {1} ORDER BY Taxis DESC
) WHERE ROWNUM <= 1
))", true.ToString(), parentID);
                }

                this.ExecuteNonQuery(sqlString);
            }
        }

        private int GetMaxTaxisByParentPath(int publishmentSystemID, string parentPath)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) AS MaxTaxis FROM wx_StoreCategory WHERE PublishmentSystemID = {0} AND (ParentsPath = '{1}' OR ParentsPath LIKE '{1},%')", publishmentSystemID, parentPath);
            int maxTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        maxTaxis = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return maxTaxis;
        }

        private int GetParentID(int id)
        {
            int parentID = 0;

            IDbDataParameter[] nodeParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ID, EDataType.Integer, id)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_PARENT_ID, nodeParms))
            {
                if (rdr.Read())
                {
                    parentID = Convert.ToInt32(rdr[0]);
                }
                rdr.Close();
            }
            return parentID;
        }

        private int GetIDByParentIDAndOrder(int publishmentSystemID, int parentID, int order)
        {
            int id = parentID;

            string sqlString = string.Format("SELECT ID FROM wx_StoreCategory WHERE PublishmentSystemID = {0} AND ParentID = {1} ORDER BY Taxis", publishmentSystemID, parentID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    id = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return id;
        }

        public int Insert(int publishmentSystemID, StoreCategoryInfo categoryInfo)
        {
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StoreCategoryInfo parentCategoryInfo = this.GetCategoryInfo(categoryInfo.ParentID);

                        this.InsertWithTrans(publishmentSystemID, parentCategoryInfo, categoryInfo, trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return categoryInfo.ID;
        }

        public void Update(int publishmentSystemID, StoreCategoryInfo categoryInfo)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_NAME, EDataType.NVarChar, 255, categoryInfo.CategoryName),
				this.GetParameter(PARM_PARENTS_PATH, EDataType.NVarChar, 255, categoryInfo.ParentsPath),
				this.GetParameter(PARM_PARENTS_COUNT, EDataType.Integer, categoryInfo.ParentsCount),
				this.GetParameter(PARM_CHILDREN_COUNT, EDataType.Integer, categoryInfo.ChildCount),
				this.GetParameter(PARM_IS_LAST_NODE, EDataType.VarChar, 18, categoryInfo.IsLastNode.ToString()),

				this.GetParameter(PARM_ID, EDataType.Integer, categoryInfo.ID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, updateParms);
        }

        public void UpdateTaxis(int publishmentSystemID, int selectedID, bool isSubtract)
        {
            if (isSubtract)
            {
                this.TaxisSubtract(publishmentSystemID, selectedID);
            }
            else
            {
                this.TaxisAdd(publishmentSystemID, selectedID);
            }
        }

        public void Delete(int publishmentSystemID, int id)
        {
            StoreCategoryInfo categoryInfo = this.GetCategoryInfo(id);
            if (categoryInfo != null)
            {
                List<int> idList = new List<int>();
                if (categoryInfo.ChildCount > 0)
                {
                    idList = this.GetCategoryIDListForDescendant(publishmentSystemID, id);
                }
                idList.Add(id);

                string sqlString = string.Format("DELETE FROM wx_StoreCategory WHERE ID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idList));

                int deletedNum = 0;

                using (IDbConnection conn = this.GetConnection())
                {
                    conn.Open();
                    using (IDbTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            deletedNum = this.ExecuteNonQuery(trans, sqlString);

                            if (deletedNum > 0)
                            {
                                string sqlStringTaxis = string.Format("UPDATE wx_StoreCategory SET Taxis = Taxis - {0} WHERE (Taxis > {1})", deletedNum, categoryInfo.Taxis);
                                this.ExecuteNonQuery(trans, sqlStringTaxis);
                            }

                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
                this.UpdateIsLastNode(publishmentSystemID, categoryInfo.ParentID);
                this.UpdateSubtractChildCount(publishmentSystemID, categoryInfo.ParentsPath, deletedNum);
            }
        }

        public StoreCategoryInfo GetCategoryInfo(int categoryID)
        {
            StoreCategoryInfo categoryInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ID, EDataType.Integer, categoryID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    categoryInfo = new StoreCategoryInfo(rdr);
                }
                rdr.Close();
            }
            return categoryInfo;
        }

        private ArrayList GetCategoryInfoArrayList(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHIMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL, parms))
            {
                while (rdr.Read())
                {
                    StoreCategoryInfo categoryInfo = new StoreCategoryInfo(rdr);
                    arraylist.Add(categoryInfo);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public int GetNodeCount(int publishmentSystemID, int id)
        {
            int nodeCount = 0;

            IDbDataParameter[] nodeParms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHIMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_PARENT_ID, EDataType.Integer, id)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_COUNT, nodeParms))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    nodeCount = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return nodeCount;
        }

        public List<int> GetCategoryIDListByParentID(int publishmentSystemID, int parentID)
        {
            List<int> list = new List<int>();

            string sqlString = string.Format(@"SELECT ID FROM wx_StoreCategory WHERE PublishmentSystemID = {0} AND ParentID = {1} ORDER BY Taxis", publishmentSystemID, parentID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read() && !rdr.IsDBNull(0))
                {
                    list.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return list;
        }

        public List<int> GetCategoryIDListForDescendant(int publishmentSystemID, int categoryID)
        {
            List<int> list = new List<int>();

            string sqlString = string.Format(@"SELECT ID
FROM wx_StoreCategory
WHERE PublishmentSystemID = {0} AND (
    (ParentsPath LIKE '{1},%') OR
    (ParentsPath LIKE '%,{1},%') OR
    (ParentsPath LIKE '%,{1}') OR
    (ParentID = {1})
)
", publishmentSystemID, categoryID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read() && !rdr.IsDBNull(0))
                {
                    list.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return list;
        }

        public List<int> GetAllCategoryIDList(int publishmentSystemID)
        {
            List<int> list = new List<int>();

            string sqlString = string.Format(@"SELECT ID FROM wx_StoreCategory WHERE PublishmentSystemID = {0} ORDER BY Taxis", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read() && !rdr.IsDBNull(0))
                {
                    list.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return list;
        }

        public List<int> GetCategoryIDListForLastNode(int publishmentSystemID, int categoryID)
        {
            List<int> list = new List<int>();

            string sqlString = string.Format(@"SELECT ID
FROM wx_StoreCategory
WHERE PublishmentSystemID = {0} AND ChildCount = 0 AND (
    (ParentsPath LIKE '{1},%') OR
    (ParentsPath LIKE '%,{1},%') OR
    (ParentsPath LIKE '%,{1}') OR
    (ParentID = {1}) OR
    (ID = {1})
)
", publishmentSystemID, categoryID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read() && !rdr.IsDBNull(0))
                {
                    list.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return list;
        }

        public List<int> GetCategoryIDListByCategoryIDCollection(int publishmentSystemID, string idCollection)
        {
            List<int> list = new List<int>();

            if (!string.IsNullOrEmpty(idCollection))
            {
                string sqlString = string.Format(@"SELECT ID FROM wx_StoreCategory WHERE PublishmentSystemID = {0} AND ID IN ({1})", publishmentSystemID, idCollection);

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    while (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        list.Add(rdr.GetInt32(0));
                    }
                    rdr.Close();
                }
            }

            return list;
        }

        public List<int> GetCategoryIDListByFirstCategoryIDArrayList(int publishmentSystemID, ArrayList firstIDArrayList)
        {
            List<int> list = new List<int>();

            if (firstIDArrayList.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                foreach (int id in firstIDArrayList)
                {
                    builder.AppendFormat("ID = {0} OR ParentID = {0} OR ParentsPath LIKE '{0},%' OR ", id);
                }
                builder.Length -= 3;

                string sqlString = string.Format("SELECT ID FROM wx_StoreCategory WHERE PublishmentSystemID = {0} AND ({1}) ORDER BY Taxis", publishmentSystemID, builder.ToString());

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    while (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        list.Add(rdr.GetInt32(0));
                    }
                    rdr.Close();
                }
            }

            return list;
        }

        public void UpdateStoreItemCount(int publishmentSystemID)
        {
            List<int> categoryIDList = this.GetAllCategoryIDList(publishmentSystemID);

            foreach (int categoryID in categoryIDList)
            {
                int count = DataProviderWX.StoreItemDAO.GetCount(publishmentSystemID, categoryID);
                string sqlString = string.Format(@"UPDATE wx_StoreCategory SET StoreCount = {0} WHERE PublishmentSystemID = {1} AND ID = {2}", count, publishmentSystemID, categoryID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public List<StoreCategoryInfo> GetStoreCategoryInfoList(int publishmentSystemID)
        {
            List<StoreCategoryInfo> list = new List<StoreCategoryInfo>();

            StringBuilder builder = new StringBuilder(string.Format("WHERE {0} = {1}", StoreCategoryAttribute.PublishmentSystemID, publishmentSystemID));
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, StoreCategoryDAO.TABLE_NAME, 0, SqlUtils.Asterisk, builder.ToString(), "ORDER BY Taxis");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    StoreCategoryInfo categoryInfo = new StoreCategoryInfo(rdr);
                    list.Add(categoryInfo);
                }
                rdr.Close();
            }

            return list;
        }
    }
}
