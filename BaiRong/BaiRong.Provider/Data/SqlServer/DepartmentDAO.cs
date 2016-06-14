using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using BaiRong.Core;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;

namespace BaiRong.Provider.Data.SqlServer
{
    public class DepartmentDAO : DataProviderBase, IDepartmentDAO
	{
        private const string SQL_SELECT = "SELECT DepartmentID, DepartmentName, Code, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, Taxis, AddDate, Summary, CountOfAdmin FROM bairong_Department WHERE DepartmentID = @DepartmentID";

        private const string SQL_SELECT_ALL = "SELECT DepartmentID, DepartmentName, Code, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, Taxis, AddDate, Summary, CountOfAdmin FROM bairong_Department ORDER BY TAXIS";

        private const string SQL_SELECT_NAME = "SELECT DepartmentName FROM bairong_Department WHERE DepartmentID = @DepartmentID";

        private const string SQL_SELECT_CODE = "SELECT Code FROM bairong_Department WHERE DepartmentID = @DepartmentID";

        private const string SQL_SELECT_ID = "SELECT DepartmentID FROM bairong_Department WHERE DepartmentID = @DepartmentID";

        private const string SQL_SELECT_PARENT_ID = "SELECT ParentID FROM bairong_Department WHERE DepartmentID = @DepartmentID";

        private const string SQL_SELECT_COUNT = "SELECT COUNT(*) FROM bairong_Department WHERE ParentID = @ParentID";

        private const string SQL_UPDATE = "UPDATE bairong_Department SET DepartmentName = @DepartmentName, Code = @Code, ParentsPath = @ParentsPath, ParentsCount = @ParentsCount, ChildrenCount = @ChildrenCount, IsLastNode = @IsLastNode, Summary = @Summary, CountOfAdmin = @CountOfAdmin WHERE DepartmentID = @DepartmentID";

        private const string PARM_ID = "@DepartmentID";
        private const string PARM_NAME = "@DepartmentName";
        private const string PARM_CODE = "@Code";
        private const string PARM_PARENT_ID = "@ParentID";
        private const string PARM_PARENTS_PATH = "@ParentsPath";
        private const string PARM_PARENTS_COUNT = "@ParentsCount";
        private const string PARM_CHILDREN_COUNT = "@ChildrenCount";
        private const string PARM_IS_LAST_NODE = "@IsLastNode";
        private const string PARM_TAXIS = "@Taxis";
        private const string PARM_ADD_DATE = "@AddDate";
        private const string PARM_SUMMARY = "@Summary";
        private const string PARM_COUNT_OF_ADMIN = "@CountOfAdmin";

        private void InsertWithTrans(DepartmentInfo parentInfo, DepartmentInfo departmentInfo, IDbTransaction trans)
        {
            if (parentInfo != null)
            {
                departmentInfo.ParentsPath = parentInfo.ParentsPath + "," + parentInfo.DepartmentID;
                departmentInfo.ParentsCount = parentInfo.ParentsCount + 1;

                int maxTaxis = this.GetMaxTaxisByParentPath(departmentInfo.ParentsPath);
                if (maxTaxis == 0)
                {
                    maxTaxis = parentInfo.Taxis;
                }
                departmentInfo.Taxis = maxTaxis + 1;
            }
            else
            {
                departmentInfo.ParentsPath = "0";
                departmentInfo.ParentsCount = 0;
                int maxTaxis = this.GetMaxTaxisByParentPath("0");
                departmentInfo.Taxis = maxTaxis + 1;
            }

            string SQL_INSERT = "INSERT INTO bairong_Department (DepartmentName, Code, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, Taxis, AddDate, Summary, CountOfAdmin) VALUES (@DepartmentName, @Code, @ParentID, @ParentsPath, @ParentsCount, @ChildrenCount, @IsLastNode, @Taxis, @AddDate, @Summary, @CountOfAdmin)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                SQL_INSERT = "INSERT INTO bairong_Department (DepartmentID, DepartmentName, Code, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, Taxis, AddDate, Summary, CountOfAdmin) VALUES (bairong_Department_SEQ.NEXTVAL, @DepartmentName, @Code, @ParentID, @ParentsPath, @ParentsCount, @ChildrenCount, @IsLastNode, @Taxis, @AddDate, @Summary, @CountOfAdmin)";
            }

            IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_NAME, EDataType.NVarChar, 255, departmentInfo.DepartmentName),
                this.GetParameter(PARM_CODE, EDataType.VarChar, 50, departmentInfo.Code),
				this.GetParameter(PARM_PARENT_ID, EDataType.Integer, departmentInfo.ParentID),
				this.GetParameter(PARM_PARENTS_PATH, EDataType.NVarChar, 255, departmentInfo.ParentsPath),
				this.GetParameter(PARM_PARENTS_COUNT, EDataType.Integer, departmentInfo.ParentsCount),
				this.GetParameter(PARM_CHILDREN_COUNT, EDataType.Integer, 0),
				this.GetParameter(PARM_IS_LAST_NODE, EDataType.VarChar, 18, true.ToString()),
				this.GetParameter(PARM_TAXIS, EDataType.Integer, departmentInfo.Taxis),
				this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, departmentInfo.AddDate),
				this.GetParameter(PARM_SUMMARY, EDataType.NVarChar, 255, departmentInfo.Summary),
				this.GetParameter(PARM_COUNT_OF_ADMIN, EDataType.Integer, departmentInfo.CountOfAdmin)
			};

            string sqlString = string.Format("UPDATE bairong_Department SET Taxis = Taxis + 1 WHERE (Taxis >= {0})", departmentInfo.Taxis);
            this.ExecuteNonQuery(trans, sqlString);

            this.ExecuteNonQuery(trans, SQL_INSERT, insertParms);

            departmentInfo.DepartmentID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "bairong_Department");

            if (!string.IsNullOrEmpty(departmentInfo.ParentsPath))
            {
                sqlString = string.Concat("UPDATE bairong_Department SET ChildrenCount = ChildrenCount + 1 WHERE DepartmentID in (", PageUtils.FilterSql(departmentInfo.ParentsPath), ")");

                this.ExecuteNonQuery(trans, sqlString);
            }

            sqlString = string.Format("UPDATE bairong_Department SET IsLastNode = 'False' WHERE ParentID = {0}", departmentInfo.ParentID);

            this.ExecuteNonQuery(trans, sqlString);

            sqlString = string.Format("UPDATE bairong_Department SET IsLastNode = 'True' WHERE (DepartmentID IN (SELECT TOP 1 DepartmentID FROM bairong_Department WHERE ParentID = {0} ORDER BY Taxis DESC))", departmentInfo.ParentID);
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = string.Format(@"UPDATE bairong_Department SET IsLastNode = 'True' WHERE (DepartmentID IN (
SELECT DepartmentID FROM (
    SELECT DepartmentID FROM bairong_Department WHERE ParentID = {0} ORDER BY Taxis DESC
) WHERE ROWNUM <= 1
))", departmentInfo.ParentID);
            }
            this.ExecuteNonQuery(trans, sqlString);

            DepartmentManager.ClearCache();
        }

        private void UpdateSubtractChildrenCount(string parentsPath, int subtractNum)
        {
            if (!string.IsNullOrEmpty(parentsPath))
            {
                string sqlString = string.Concat("UPDATE bairong_Department SET ChildrenCount = ChildrenCount - ", subtractNum, " WHERE DepartmentID IN (", PageUtils.FilterSql(parentsPath) , ")");
                this.ExecuteNonQuery(sqlString);

                DepartmentManager.ClearCache();
            }
        }

        private void TaxisSubtract(int selectedDepartmentID)
        {
            DepartmentInfo departmentInfo = this.GetDepartmentInfo(selectedDepartmentID);
            if (departmentInfo == null) return;
            //Get Lower Taxis and DepartmentID
            int lowerDepartmentID = 0;
            int lowerChildrenCount = 0;
            string lowerParentsPath = "";
            string sqlString = @"SELECT TOP 1 DepartmentID, ChildrenCount, ParentsPath
FROM bairong_Department
WHERE (ParentID = @ParentID) AND (DepartmentID <> @DepartmentID) AND (Taxis < @Taxis)
ORDER BY Taxis DESC";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PARENT_ID, EDataType.Integer, departmentInfo.ParentID),
				this.GetParameter(PARM_ID, EDataType.Integer, departmentInfo.DepartmentID),
				this.GetParameter(PARM_TAXIS, EDataType.Integer, departmentInfo.Taxis),
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    lowerDepartmentID = Convert.ToInt32(rdr[0]);
                    lowerChildrenCount = Convert.ToInt32(rdr[1]);
                    lowerParentsPath = rdr.GetValue(2).ToString();
                }
                else
                {
                    return;
                }
                rdr.Close();
            }


            string lowerNodePath = String.Concat(lowerParentsPath, ",", lowerDepartmentID);
            string selectedNodePath = String.Concat(departmentInfo.ParentsPath, ",", departmentInfo.DepartmentID);

            this.SetTaxisSubtract(selectedDepartmentID, selectedNodePath, lowerChildrenCount + 1);
            this.SetTaxisAdd(lowerDepartmentID, lowerNodePath, departmentInfo.ChildrenCount + 1);

            this.UpdateIsLastNode(departmentInfo.ParentID);
        }

        private void TaxisAdd(int selectedDepartmentID)
        {
            DepartmentInfo departmentInfo = this.GetDepartmentInfo(selectedDepartmentID);
            if (departmentInfo == null) return;
            //Get Higher Taxis and DepartmentID
            int higherDepartmentID = 0;
            int higherChildrenCount = 0;
            string higherParentsPath = "";
            string sqlString = @"SELECT TOP 1 DepartmentID, ChildrenCount, ParentsPath
FROM bairong_Department
WHERE (ParentID = @ParentID) AND (DepartmentID <> @DepartmentID) AND (Taxis > @Taxis)
ORDER BY Taxis";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PARENT_ID, EDataType.Integer, departmentInfo.ParentID),
				this.GetParameter(PARM_ID, EDataType.Integer, departmentInfo.DepartmentID),
				this.GetParameter(PARM_TAXIS, EDataType.Integer, departmentInfo.Taxis)
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    higherDepartmentID = Convert.ToInt32(rdr[0]);
                    higherChildrenCount = Convert.ToInt32(rdr[1]);
                    higherParentsPath = rdr.GetValue(2).ToString();
                }
                else
                {
                    return;
                }
                rdr.Close();
            }


            string higherNodePath = String.Concat(higherParentsPath, ",", higherDepartmentID);
            string selectedNodePath = String.Concat(departmentInfo.ParentsPath, ",", departmentInfo.DepartmentID);

            this.SetTaxisAdd(selectedDepartmentID, selectedNodePath, higherChildrenCount + 1);
            this.SetTaxisSubtract(higherDepartmentID, higherNodePath, departmentInfo.ChildrenCount + 1);

            this.UpdateIsLastNode(departmentInfo.ParentID);
        }

        private void SetTaxisAdd(int departmentID, string parentsPath, int addNum)
        {
            string sqlString = string.Format("UPDATE bairong_Department SET Taxis = Taxis + {0} WHERE DepartmentID = {1} OR ParentsPath = '{2}' OR ParentsPath LIKE '{2},%'", addNum, departmentID, PageUtils.FilterSql(parentsPath));

            this.ExecuteNonQuery(sqlString);

            DepartmentManager.ClearCache();
        }

        private void SetTaxisSubtract(int departmentID, string parentsPath, int subtractNum)
        {
            string sqlString = string.Format("UPDATE bairong_Department SET Taxis = Taxis - {0} WHERE  DepartmentID = {1} OR ParentsPath = '{2}' OR ParentsPath LIKE '{2},%'", subtractNum, departmentID, PageUtils.FilterSql(parentsPath));

            this.ExecuteNonQuery(sqlString);

            DepartmentManager.ClearCache();
        }

        private void UpdateIsLastNode(int parentID)
        {
            if (parentID > 0)
            {
                string sqlString = "UPDATE bairong_Department SET IsLastNode = @IsLastNode WHERE  ParentID = @ParentID";

                IDbDataParameter[] parms = new IDbDataParameter[]
			    {
				    this.GetParameter(PARM_IS_LAST_NODE, EDataType.VarChar, 18, false.ToString()),
				    this.GetParameter(PARM_PARENT_ID, EDataType.Integer, parentID)
			    };

                this.ExecuteNonQuery(sqlString, parms);

                sqlString = string.Format("UPDATE bairong_Department SET IsLastNode = '{0}' WHERE (DepartmentID IN (SELECT TOP 1 DepartmentID FROM bairong_Department WHERE ParentID = {1} ORDER BY Taxis DESC))", true.ToString(), parentID);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    sqlString = string.Format(@"UPDATE bairong_Department SET IsLastNode = '{0}' WHERE (DepartmentID IN (
SELECT DepartmentID FROM (
    SELECT DepartmentID FROM bairong_Department WHERE ParentID = {1} ORDER BY Taxis DESC
) WHERE ROWNUM <= 1
))", true.ToString(), parentID);
                }

                this.ExecuteNonQuery(sqlString);
            }
        }

        private int GetMaxTaxisByParentPath(string parentPath)
        {
            string sqlString = string.Concat("SELECT MAX(Taxis) AS MaxTaxis FROM bairong_Department WHERE (ParentsPath = '", PageUtils.FilterSql(parentPath), "') OR (ParentsPath LIKE '", PageUtils.FilterSql(parentPath), ",%')");
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

        private int GetParentID(int departmentID)
        {
            int parentID = 0;

            IDbDataParameter[] nodeParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ID, EDataType.Integer, departmentID)
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

        private int GetDepartmentIDByParentIDAndOrder(int parentID, int order)
        {
            int DepartmentID = parentID;

            string CMD = string.Format("SELECT DepartmentID FROM bairong_Department WHERE (ParentID = {0}) ORDER BY Taxis", parentID);

            using (IDataReader rdr = this.ExecuteReader(CMD))
            {
                int index = 1;
                while (rdr.Read())
                {
                    DepartmentID = Convert.ToInt32(rdr[0]);
                    if (index == order)
                        break;
                    index++;
                }
                rdr.Close();
            }
            return DepartmentID;
        }

        public int Insert(DepartmentInfo departmentInfo)
        {
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        DepartmentInfo parentDepartmentInfo = this.GetDepartmentInfo(departmentInfo.ParentID);

                        this.InsertWithTrans(parentDepartmentInfo, departmentInfo, trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            DepartmentManager.ClearCache();

            return departmentInfo.DepartmentID;
        }

        public void Update(DepartmentInfo departmentInfo)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_NAME, EDataType.NVarChar, 255, departmentInfo.DepartmentName),
                this.GetParameter(PARM_CODE, EDataType.VarChar, 50, departmentInfo.Code),
				this.GetParameter(PARM_PARENTS_PATH, EDataType.NVarChar, 255, departmentInfo.ParentsPath),
				this.GetParameter(PARM_PARENTS_COUNT, EDataType.Integer, departmentInfo.ParentsCount),
				this.GetParameter(PARM_CHILDREN_COUNT, EDataType.Integer, departmentInfo.ChildrenCount),
				this.GetParameter(PARM_IS_LAST_NODE, EDataType.VarChar, 18, departmentInfo.IsLastNode.ToString()),
				this.GetParameter(PARM_SUMMARY, EDataType.NVarChar, 255, departmentInfo.Summary),
				this.GetParameter(PARM_COUNT_OF_ADMIN, EDataType.Integer, departmentInfo.CountOfAdmin),
				this.GetParameter(PARM_ID, EDataType.Integer, departmentInfo.DepartmentID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, updateParms);

            DepartmentManager.ClearCache();
        }

        public void UpdateTaxis(int selectedDepartmentID, bool isSubtract)
        {
            if (isSubtract)
            {
                this.TaxisSubtract(selectedDepartmentID);
            }
            else
            {
                this.TaxisAdd(selectedDepartmentID);
            }
        }

        public void UpdateCountOfAdmin()
        {
            ArrayList departmentIDArrayList = DepartmentManager.GetDepartmentIDArrayList();
            foreach (int departmentID in departmentIDArrayList)
            {
                string sqlString = string.Format("UPDATE bairong_Department SET CountOfAdmin = (SELECT COUNT(*) AS CountOfAdmin FROM bairong_Administrator WHERE DepartmentID = {0}) WHERE DepartmentID = {0}", departmentID);
                this.ExecuteNonQuery(sqlString);
            }
            DepartmentManager.ClearCache();
        }

        public void Delete(int departmentID)
        {
            DepartmentInfo departmentInfo = this.GetDepartmentInfo(departmentID);
            if (departmentInfo != null)
            {
                ArrayList departmentIDArrayList = new ArrayList();
                if (departmentInfo.ChildrenCount > 0)
                {
                    departmentIDArrayList = this.GetDepartmentIDArrayListForDescendant(departmentID);
                }
                departmentIDArrayList.Add(departmentID);

                string sqlString = string.Format("DELETE FROM bairong_Department WHERE DepartmentID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(departmentIDArrayList));

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
                                string sqlStringTaxis = string.Format("UPDATE bairong_Department SET Taxis = Taxis - {0} WHERE (Taxis > {1})", deletedNum, departmentInfo.Taxis);
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
                this.UpdateIsLastNode(departmentInfo.ParentID);
                this.UpdateSubtractChildrenCount(departmentInfo.ParentsPath, deletedNum);
            }

            DepartmentManager.ClearCache();
        }

        private DepartmentInfo GetDepartmentInfo(int departmentID)
		{
            DepartmentInfo departmentInfo = null;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ID, EDataType.Integer, departmentID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms)) 
			{
				if (rdr.Read()) 
				{
                    departmentInfo = new DepartmentInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), rdr.GetInt32(3), rdr.GetValue(4).ToString(), rdr.GetInt32(5), rdr.GetInt32(6), TranslateUtils.ToBool(rdr.GetValue(7).ToString()), rdr.GetInt32(8), rdr.GetDateTime(9), rdr.GetValue(10).ToString(), rdr.GetInt32(11));
				}
				rdr.Close();
			}
            return departmentInfo;
		}

        private ArrayList GetDepartmentInfoArrayList()
        {
            ArrayList arraylist = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL))
            {
                while (rdr.Read())
                {
                    DepartmentInfo departmentInfo = new DepartmentInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), rdr.GetInt32(3), rdr.GetValue(4).ToString(), rdr.GetInt32(5), rdr.GetInt32(6), TranslateUtils.ToBool(rdr.GetValue(7).ToString()), rdr.GetInt32(8), rdr.GetDateTime(9), rdr.GetValue(10).ToString(), rdr.GetInt32(11));
                    arraylist.Add(departmentInfo);
                }
                rdr.Close();
            }
            return arraylist;
        }

		public int GetNodeCount(int departmentID)
		{
			int nodeCount = 0;

			IDbDataParameter[] nodeParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PARENT_ID, EDataType.Integer, departmentID)
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

        public ArrayList GetDepartmentIDArrayListByParentID(int parentID)
        {
            string sqlString = string.Format(@"SELECT DepartmentID FROM bairong_Department WHERE ParentID = '{0}' ORDER BY Taxis", parentID);
            ArrayList list = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int theDepartmentID = Convert.ToInt32(rdr[0]);
                    list.Add(theDepartmentID);
                }
                rdr.Close();
            }

            return list;
        }

		public ArrayList GetDepartmentIDArrayListForDescendant(int departmentID)
		{
            string sqlString = string.Format(@"SELECT DepartmentID
FROM bairong_Department
WHERE (ParentsPath LIKE '{0},%') OR
      (ParentsPath LIKE '%,{0},%') OR
      (ParentsPath LIKE '%,{0}') OR
      (ParentID = {0})
", departmentID);
			ArrayList list = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int theDepartmentID = Convert.ToInt32(rdr[0]);
                    list.Add(theDepartmentID);
                }
                rdr.Close();
            }

			return list;
		}

        public ArrayList GetDepartmentIDArrayListByDepartmentIDCollection(string departmentIDCollection)
        {
            ArrayList arrayList = new ArrayList();

            if (!string.IsNullOrEmpty(departmentIDCollection))
            {
                string sqlString = string.Format(@"SELECT DepartmentID FROM bairong_Department WHERE DepartmentID IN ({0})", departmentIDCollection);

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    while (rdr.Read())
                    {
                        int theDepartmentID = Convert.ToInt32(rdr[0]);
                        arrayList.Add(theDepartmentID);
                    }
                    rdr.Close();
                }
            }

            return arrayList;
        }

        public ArrayList GetDepartmentIDArrayListByFirstDepartmentIDArrayList(ArrayList firstIDArrayList)
        {
            ArrayList arraylist = new ArrayList();

            if (firstIDArrayList.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                foreach (int departmentID in firstIDArrayList)
                {
                    builder.AppendFormat("DepartmentID = {0} OR ParentID = {0} OR ParentsPath LIKE '{0},%' OR ", departmentID);
                }
                builder.Length -= 3;

                string sqlString = string.Format("SELECT DepartmentID FROM bairong_Department WHERE {0} ORDER BY Taxis", builder.ToString());

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    while (rdr.Read())
                    {
                        int departmentID = Convert.ToInt32(rdr[0]);
                        arraylist.Add(departmentID);
                    }
                    rdr.Close();
                }
            }

            return arraylist;
        }

        public DictionaryEntryArrayList GetDepartmentInfoDictionaryEntryArrayList()
        {
            DictionaryEntryArrayList dictionary = new DictionaryEntryArrayList();

            ArrayList departmentInfoArrayList = this.GetDepartmentInfoArrayList();
            foreach (DepartmentInfo departmentInfo in departmentInfoArrayList)
            {
                DictionaryEntry entry = new DictionaryEntry(departmentInfo.DepartmentID, departmentInfo);
                dictionary.Add(entry);
            }

            return dictionary;
        }
	}
}
