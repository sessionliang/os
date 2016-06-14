using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core.Data.Provider;
using System.Data;
using BaiRong.Model;
using BaiRong.Core;
using System.Collections;

namespace BaiRong.Provider.Data.SqlServer
{
    public class AreaDAO : DataProviderBase, IAreaDAO
    {
        private const string SQL_SELECT = "SELECT AreaID, AreaName, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, Taxis, CountOfAdmin FROM bairong_Area WHERE AreaID = @AreaID";

        private const string SQL_SELECT_ALL = "SELECT AreaID, AreaName, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, Taxis, CountOfAdmin FROM bairong_Area ORDER BY TAXIS";

        private const string SQL_SELECT_NAME = "SELECT AreaName FROM bairong_Area WHERE AreaID = @AreaID";

        private const string SQL_SELECT_ID = "SELECT AreaID FROM bairong_Area WHERE AreaID = @AreaID";

        private const string SQL_SELECT_PARENT_ID = "SELECT ParentID FROM bairong_Area WHERE AreaID = @AreaID";

        private const string SQL_SELECT_COUNT = "SELECT COUNT(*) FROM bairong_Area WHERE ParentID = @ParentID";

        private const string SQL_UPDATE = "UPDATE bairong_Area SET AreaName = @AreaName, ParentsPath = @ParentsPath, ParentsCount = @ParentsCount, ChildrenCount = @ChildrenCount, IsLastNode = @IsLastNode, CountOfAdmin = @CountOfAdmin WHERE AreaID = @AreaID";

        private const string PARM_ID = "@AreaID";
        private const string PARM_NAME = "@AreaName";
        private const string PARM_PARENT_ID = "@ParentID";
        private const string PARM_PARENTS_PATH = "@ParentsPath";
        private const string PARM_PARENTS_COUNT = "@ParentsCount";
        private const string PARM_CHILDREN_COUNT = "@ChildrenCount";
        private const string PARM_IS_LAST_NODE = "@IsLastNode";
        private const string PARM_TAXIS = "@Taxis";
        private const string PARM_COUNT_OF_ADMIN = "@CountOfAdmin";

        private void InsertWithTrans(AreaInfo parentInfo, AreaInfo areaInfo, IDbTransaction trans)
        {
            if (parentInfo != null)
            {
                areaInfo.ParentsPath = parentInfo.ParentsPath + "," + parentInfo.AreaID;
                areaInfo.ParentsCount = parentInfo.ParentsCount + 1;

                int maxTaxis = this.GetMaxTaxisByParentPath(areaInfo.ParentsPath);
                if (maxTaxis == 0)
                {
                    maxTaxis = parentInfo.Taxis;
                }
                areaInfo.Taxis = maxTaxis + 1;
            }
            else
            {
                areaInfo.ParentsPath = "0";
                areaInfo.ParentsCount = 0;
                int maxTaxis = this.GetMaxTaxisByParentPath("0");
                areaInfo.Taxis = maxTaxis + 1;
            }

            string SQL_INSERT = "INSERT INTO bairong_Area (AreaName, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, Taxis, CountOfAdmin) VALUES (@AreaName, @ParentID, @ParentsPath, @ParentsCount, @ChildrenCount, @IsLastNode, @Taxis, @CountOfAdmin)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                SQL_INSERT = "INSERT INTO bairong_Area (AreaID, AreaName, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, Taxis, CountOfAdmin) VALUES (bairong_Area_SEQ.NEXTVAL, @AreaName, @ParentID, @ParentsPath, @ParentsCount, @ChildrenCount, @IsLastNode, @Taxis, @CountOfAdmin)";
            }

            IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_NAME, EDataType.NVarChar, 255, areaInfo.AreaName),
				this.GetParameter(PARM_PARENT_ID, EDataType.Integer, areaInfo.ParentID),
				this.GetParameter(PARM_PARENTS_PATH, EDataType.NVarChar, 255, areaInfo.ParentsPath),
				this.GetParameter(PARM_PARENTS_COUNT, EDataType.Integer, areaInfo.ParentsCount),
				this.GetParameter(PARM_CHILDREN_COUNT, EDataType.Integer, 0),
				this.GetParameter(PARM_IS_LAST_NODE, EDataType.VarChar, 18, true.ToString()),
				this.GetParameter(PARM_TAXIS, EDataType.Integer, areaInfo.Taxis),
				this.GetParameter(PARM_COUNT_OF_ADMIN, EDataType.Integer, areaInfo.CountOfAdmin)
			};

            string sqlString = string.Format("UPDATE bairong_Area SET Taxis = Taxis + 1 WHERE (Taxis >= {0})", areaInfo.Taxis);
            this.ExecuteNonQuery(trans, sqlString);

            this.ExecuteNonQuery(trans, SQL_INSERT, insertParms);

            areaInfo.AreaID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "bairong_Area");

            if (!string.IsNullOrEmpty(areaInfo.ParentsPath))
            {
                sqlString = string.Concat("UPDATE bairong_Area SET ChildrenCount = ChildrenCount + 1 WHERE AreaID IN (", PageUtils.FilterSql(areaInfo.ParentsPath), ")");

                this.ExecuteNonQuery(trans, sqlString);
            }

            sqlString = string.Format("UPDATE bairong_Area SET IsLastNode = 'False' WHERE ParentID = {0}", areaInfo.ParentID);

            this.ExecuteNonQuery(trans, sqlString);

            sqlString = string.Format("UPDATE bairong_Area SET IsLastNode = 'True' WHERE (AreaID IN (SELECT TOP 1 AreaID FROM bairong_Area WHERE ParentID = {0} ORDER BY Taxis DESC))", areaInfo.ParentID);
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = string.Format(@"UPDATE bairong_Area SET IsLastNode = 'True' WHERE (AreaID IN (
SELECT AreaID FROM (
    SELECT AreaID FROM bairong_Area WHERE ParentID = {0} ORDER BY Taxis DESC
) WHERE ROWNUM <= 1
))", areaInfo.ParentID);
            }
            this.ExecuteNonQuery(trans, sqlString);

            AreaManager.ClearCache();
        }

        private void UpdateSubtractChildrenCount(string parentsPath, int subtractNum)
        {
            if (!string.IsNullOrEmpty(parentsPath))
            {
                string sqlString = string.Concat("UPDATE bairong_Area SET ChildrenCount = ChildrenCount - ", subtractNum, " WHERE AreaID IN (", PageUtils.FilterSql(parentsPath), ")");
                this.ExecuteNonQuery(sqlString);

                AreaManager.ClearCache();
            }
        }

        private void TaxisSubtract(int selectedAreaID)
        {
            AreaInfo areaInfo = this.GetAreaInfo(selectedAreaID);
            if (areaInfo == null) return;
            //Get Lower Taxis and AreaID
            int lowerAreaID = 0;
            int lowerChildrenCount = 0;
            string lowerParentsPath = "";
            string sqlString = @"SELECT TOP 1 AreaID, ChildrenCount, ParentsPath
FROM bairong_Area
WHERE (ParentID = @ParentID) AND (AreaID <> @AreaID) AND (Taxis < @Taxis)
ORDER BY Taxis DESC";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PARENT_ID, EDataType.Integer, areaInfo.ParentID),
				this.GetParameter(PARM_ID, EDataType.Integer, areaInfo.AreaID),
				this.GetParameter(PARM_TAXIS, EDataType.Integer, areaInfo.Taxis),
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    lowerAreaID = Convert.ToInt32(rdr[0]);
                    lowerChildrenCount = Convert.ToInt32(rdr[1]);
                    lowerParentsPath = rdr.GetValue(2).ToString();
                }
                else
                {
                    return;
                }
                rdr.Close();
            }


            string lowerNodePath = String.Concat(lowerParentsPath, ",", lowerAreaID);
            string selectedNodePath = String.Concat(areaInfo.ParentsPath, ",", areaInfo.AreaID);

            this.SetTaxisSubtract(selectedAreaID, selectedNodePath, lowerChildrenCount + 1);
            this.SetTaxisAdd(lowerAreaID, lowerNodePath, areaInfo.ChildrenCount + 1);

            this.UpdateIsLastNode(areaInfo.ParentID);
        }

        private void TaxisAdd(int selectedAreaID)
        {
            AreaInfo areaInfo = this.GetAreaInfo(selectedAreaID);
            if (areaInfo == null) return;
            //Get Higher Taxis and AreaID
            int higherAreaID = 0;
            int higherChildrenCount = 0;
            string higherParentsPath = "";
            string sqlString = @"SELECT TOP 1 AreaID, ChildrenCount, ParentsPath
FROM bairong_Area
WHERE (ParentID = @ParentID) AND (AreaID <> @AreaID) AND (Taxis > @Taxis)
ORDER BY Taxis";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PARENT_ID, EDataType.Integer, areaInfo.ParentID),
				this.GetParameter(PARM_ID, EDataType.Integer, areaInfo.AreaID),
				this.GetParameter(PARM_TAXIS, EDataType.Integer, areaInfo.Taxis)
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    higherAreaID = Convert.ToInt32(rdr[0]);
                    higherChildrenCount = Convert.ToInt32(rdr[1]);
                    higherParentsPath = rdr.GetValue(2).ToString();
                }
                else
                {
                    return;
                }
                rdr.Close();
            }


            string higherNodePath = String.Concat(higherParentsPath, ",", higherAreaID);
            string selectedNodePath = String.Concat(areaInfo.ParentsPath, ",", areaInfo.AreaID);

            this.SetTaxisAdd(selectedAreaID, selectedNodePath, higherChildrenCount + 1);
            this.SetTaxisSubtract(higherAreaID, higherNodePath, areaInfo.ChildrenCount + 1);

            this.UpdateIsLastNode(areaInfo.ParentID);
        }

        private void SetTaxisAdd(int areaID, string parentsPath, int addNum)
        {
            string sqlString = string.Format("UPDATE bairong_Area SET Taxis = Taxis + {0} WHERE AreaID = {1} OR ParentsPath = '{2}' OR ParentsPath LIKE '{2},%'", addNum, areaID, PageUtils.FilterSql(parentsPath));

            this.ExecuteNonQuery(sqlString);

            AreaManager.ClearCache();
        }

        private void SetTaxisSubtract(int areaID, string parentsPath, int subtractNum)
        {
            string sqlString = string.Format("UPDATE bairong_Area SET Taxis = Taxis - {0} WHERE  AreaID = {1} OR ParentsPath = '{2}' OR ParentsPath LIKE '{2},%'", subtractNum, areaID, PageUtils.FilterSql(parentsPath));

            this.ExecuteNonQuery(sqlString);

            AreaManager.ClearCache();
        }

        private void UpdateIsLastNode(int parentID)
        {
            if (parentID > 0)
            {
                string sqlString = "UPDATE bairong_Area SET IsLastNode = @IsLastNode WHERE ParentID = @ParentID";

                IDbDataParameter[] parms = new IDbDataParameter[]
			    {
				    this.GetParameter(PARM_IS_LAST_NODE, EDataType.VarChar, 18, false.ToString()),
				    this.GetParameter(PARM_PARENT_ID, EDataType.Integer, parentID)
			    };

                this.ExecuteNonQuery(sqlString, parms);

                sqlString = string.Format("UPDATE bairong_Area SET IsLastNode = '{0}' WHERE (AreaID IN (SELECT TOP 1 AreaID FROM bairong_Area WHERE ParentID = {1} ORDER BY Taxis DESC))", true.ToString(), parentID);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    sqlString = string.Format(@"UPDATE bairong_Area SET IsLastNode = '{0}' WHERE (AreaID IN (
SELECT AreaID FROM (
    SELECT AreaID FROM bairong_Area WHERE ParentID = {1} ORDER BY Taxis DESC
) WHERE ROWNUM <= 1
))", true.ToString(), parentID);
                }

                this.ExecuteNonQuery(sqlString);
            }
        }

        private int GetMaxTaxisByParentPath(string parentPath)
        {
            string sqlString = string.Concat("SELECT MAX(Taxis) AS MaxTaxis FROM bairong_Area WHERE (ParentsPath = '", PageUtils.FilterSql(parentPath), "') OR (ParentsPath LIKE '", PageUtils.FilterSql(parentPath), ",%')");
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

        private int GetParentID(int areaID)
        {
            int parentID = 0;

            IDbDataParameter[] nodeParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ID, EDataType.Integer, areaID)
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

        private int GetAreaIDByParentIDAndOrder(int parentID, int order)
        {
            int AreaID = parentID;

            string CMD = string.Format("SELECT AreaID FROM bairong_Area WHERE (ParentID = {0}) ORDER BY Taxis", parentID);

            using (IDataReader rdr = this.ExecuteReader(CMD))
            {
                int index = 1;
                while (rdr.Read())
                {
                    AreaID = Convert.ToInt32(rdr[0]);
                    if (index == order)
                        break;
                    index++;
                }
                rdr.Close();
            }
            return AreaID;
        }

        public int Insert(AreaInfo areaInfo)
        {
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        AreaInfo parentAreaInfo = this.GetAreaInfo(areaInfo.ParentID);

                        this.InsertWithTrans(parentAreaInfo, areaInfo, trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            AreaManager.ClearCache();

            return areaInfo.AreaID;
        }

        public void Update(AreaInfo areaInfo)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_NAME, EDataType.NVarChar, 255, areaInfo.AreaName),
				this.GetParameter(PARM_PARENTS_PATH, EDataType.NVarChar, 255, areaInfo.ParentsPath),
				this.GetParameter(PARM_PARENTS_COUNT, EDataType.Integer, areaInfo.ParentsCount),
				this.GetParameter(PARM_CHILDREN_COUNT, EDataType.Integer, areaInfo.ChildrenCount),
				this.GetParameter(PARM_IS_LAST_NODE, EDataType.VarChar, 18, areaInfo.IsLastNode.ToString()),
				this.GetParameter(PARM_COUNT_OF_ADMIN, EDataType.Integer, areaInfo.CountOfAdmin),
				this.GetParameter(PARM_ID, EDataType.Integer, areaInfo.AreaID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, updateParms);

            AreaManager.ClearCache();
        }

        public void UpdateTaxis(int selectedAreaID, bool isSubtract)
        {
            if (isSubtract)
            {
                this.TaxisSubtract(selectedAreaID);
            }
            else
            {
                this.TaxisAdd(selectedAreaID);
            }
        }

        public void UpdateCountOfAdmin()
        {
            ArrayList areaIDArrayList = AreaManager.GetAreaIDArrayList();
            foreach (int areaID in areaIDArrayList)
            {
                string sqlString = string.Format("UPDATE bairong_Area SET CountOfAdmin = (SELECT COUNT(*) AS CountOfAdmin FROM bairong_Administrator WHERE AreaID = {0}) WHERE AreaID = {0}", areaID);
                this.ExecuteNonQuery(sqlString);
            }
            AreaManager.ClearCache();
        }

        public void Delete(int areaID)
        {
            AreaInfo areaInfo = this.GetAreaInfo(areaID);
            if (areaInfo != null)
            {
                ArrayList areaIDArrayList = new ArrayList();
                if (areaInfo.ChildrenCount > 0)
                {
                    areaIDArrayList = this.GetAreaIDArrayListForDescendant(areaID);
                }
                areaIDArrayList.Add(areaID);

                string sqlString = string.Format("DELETE FROM bairong_Area WHERE AreaID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(areaIDArrayList));

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
                                string sqlStringTaxis = string.Format("UPDATE bairong_Area SET Taxis = Taxis - {0} WHERE (Taxis > {1})", deletedNum, areaInfo.Taxis);
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
                this.UpdateIsLastNode(areaInfo.ParentID);
                this.UpdateSubtractChildrenCount(areaInfo.ParentsPath, deletedNum);
            }

            AreaManager.ClearCache();
        }

        private AreaInfo GetAreaInfo(int areaID)
        {
            AreaInfo areaInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ID, EDataType.Integer, areaID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    areaInfo = new AreaInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetInt32(4), rdr.GetInt32(5), TranslateUtils.ToBool(rdr.GetValue(6).ToString()), rdr.GetInt32(7), rdr.GetInt32(8));
                }
                rdr.Close();
            }
            return areaInfo;
        }

        private ArrayList GetAreaInfoArrayList()
        {
            ArrayList arraylist = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL))
            {
                while (rdr.Read())
                {
                    AreaInfo areaInfo = new AreaInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetInt32(4), rdr.GetInt32(5), TranslateUtils.ToBool(rdr.GetValue(6).ToString()), rdr.GetInt32(7), rdr.GetInt32(8));
                    arraylist.Add(areaInfo);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public int GetNodeCount(int areaID)
        {
            int nodeCount = 0;

            IDbDataParameter[] nodeParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PARENT_ID, EDataType.Integer, areaID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_COUNT, nodeParms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        nodeCount = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return nodeCount;
        }

        public ArrayList GetAreaIDArrayListByParentID(int parentID)
        {
            string sqlString = string.Format(@"SELECT AreaID FROM bairong_Area WHERE ParentID = '{0}' ORDER BY Taxis", parentID);
            ArrayList list = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int theAreaID = Convert.ToInt32(rdr[0]);
                    list.Add(theAreaID);
                }
                rdr.Close();
            }

            return list;
        }

        public ArrayList GetAreaIDArrayListForDescendant(int areaID)
        {
            string sqlString = string.Format(@"SELECT AreaID
FROM bairong_Area
WHERE (ParentsPath LIKE '{0},%') OR
      (ParentsPath LIKE '%,{0},%') OR
      (ParentsPath LIKE '%,{0}') OR
      (ParentID = {0})
", areaID);
            ArrayList list = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int theAreaID = Convert.ToInt32(rdr[0]);
                    list.Add(theAreaID);
                }
                rdr.Close();
            }

            return list;
        }

        public ArrayList GetAreaIDArrayListByAreaIDCollection(string areaIDCollection)
        {
            ArrayList arrayList = new ArrayList();

            if (!string.IsNullOrEmpty(areaIDCollection))
            {
                string sqlString = string.Format(@"SELECT AreaID FROM bairong_Area WHERE AreaID IN ({0})", areaIDCollection);

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    while (rdr.Read())
                    {
                        int theAreaID = Convert.ToInt32(rdr[0]);
                        arrayList.Add(theAreaID);
                    }
                    rdr.Close();
                }
            }

            return arrayList;
        }

        public ArrayList GetAreaIDArrayListByFirstAreaIDArrayList(ArrayList firstIDArrayList)
        {
            ArrayList arraylist = new ArrayList();

            if (firstIDArrayList.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                foreach (int areaID in firstIDArrayList)
                {
                    builder.AppendFormat("AreaID = {0} OR ParentID = {0} OR ParentsPath LIKE '{0},%' OR ", areaID);
                }
                builder.Length -= 3;

                string sqlString = string.Format("SELECT AreaID FROM bairong_Area WHERE {0} ORDER BY Taxis", builder.ToString());

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    while (rdr.Read())
                    {
                        int areaID = Convert.ToInt32(rdr[0]);
                        arraylist.Add(areaID);
                    }
                    rdr.Close();
                }
            }

            return arraylist;
        }

        public DictionaryEntryArrayList GetAreaInfoDictionaryEntryArrayList()
        {
            DictionaryEntryArrayList dictionary = new DictionaryEntryArrayList();

            ArrayList areaInfoArrayList = this.GetAreaInfoArrayList();
            foreach (AreaInfo areaInfo in areaInfoArrayList)
            {
                DictionaryEntry entry = new DictionaryEntry(areaInfo.AreaID, areaInfo);
                dictionary.Add(entry);
            }

            return dictionary;
        }
    }
}
