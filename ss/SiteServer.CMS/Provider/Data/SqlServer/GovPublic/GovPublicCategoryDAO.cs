using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using BaiRong.Core;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;


using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class GovPublicCategoryDAO : DataProviderBase, IGovPublicCategoryDAO
	{
        private const string SQL_SELECT = "SELECT CategoryID, ClassCode, PublishmentSystemID, CategoryName, CategoryCode, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, Taxis, AddDate, Summary, ContentNum FROM siteserver_GovPublicCategory WHERE CategoryID = @CategoryID";

        private const string SQL_SELECT_NAME = "SELECT CategoryName FROM siteserver_GovPublicCategory WHERE CategoryID = @CategoryID";

        private const string SQL_SELECT_ID = "SELECT CategoryID FROM siteserver_GovPublicCategory WHERE CategoryID = @CategoryID";

        private const string SQL_SELECT_PARENT_ID = "SELECT ParentID FROM siteserver_GovPublicCategory WHERE CategoryID = @CategoryID";

        private const string SQL_SELECT_COUNT = "SELECT COUNT(*) FROM siteserver_GovPublicCategory WHERE ParentID = @ParentID";

        private const string SQL_UPDATE = "UPDATE siteserver_GovPublicCategory SET CategoryName = @CategoryName, CategoryCode = @CategoryCode, ParentsPath = @ParentsPath, ParentsCount = @ParentsCount, ChildrenCount = @ChildrenCount, IsLastNode = @IsLastNode, Summary = @Summary, ContentNum = @ContentNum WHERE CategoryID = @CategoryID";

        private const string PARM_CATEGORY_ID = "@CategoryID";
        private const string PARM_CLASS_CODE = "@ClassCode";
        private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_CATEGORY_NAME = "@CategoryName";
        private const string PARM_CATEGORY_CODE = "@CategoryCode";
        private const string PARM_PARENT_ID = "@ParentID";
        private const string PARM_PARENTS_PATH = "@ParentsPath";
        private const string PARM_PARENTS_COUNT = "@ParentsCount";
        private const string PARM_CHILDREN_COUNT = "@ChildrenCount";
        private const string PARM_IS_LAST_NODE = "@IsLastNode";
        private const string PARM_TAXIS = "@Taxis";
        private const string PARM_ADD_DATE = "@AddDate";
        private const string PARM_SUMMARY = "@Summary";
        private const string PARM_CONTENT_NUM = "@ContentNum";

        private void InsertWithTrans(GovPublicCategoryInfo parentInfo, GovPublicCategoryInfo categoryInfo, IDbTransaction trans)
        {
            if (parentInfo != null)
            {
                categoryInfo.ParentsPath = parentInfo.ParentsPath + "," + parentInfo.CategoryID;
                categoryInfo.ParentsCount = parentInfo.ParentsCount + 1;

                int maxTaxis = this.GetMaxTaxisByParentPath(categoryInfo.ClassCode, categoryInfo.PublishmentSystemID, categoryInfo.ParentsPath);
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
                int maxTaxis = this.GetMaxTaxisByParentPath(categoryInfo.ClassCode, categoryInfo.PublishmentSystemID, "0");
                categoryInfo.Taxis = maxTaxis + 1;
            }

            string SQL_INSERT = "INSERT INTO siteserver_GovPublicCategory (ClassCode, PublishmentSystemID, CategoryName, CategoryCode, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, Taxis, AddDate, Summary, ContentNum) VALUES (@ClassCode, @PublishmentSystemID, @CategoryName, @CategoryCode, @ParentID, @ParentsPath, @ParentsCount, @ChildrenCount, @IsLastNode, @Taxis, @AddDate, @Summary, @ContentNum)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                SQL_INSERT = "INSERT INTO siteserver_GovPublicCategory (CategoryID, ClassCode, PublishmentSystemID, CategoryName, CategoryCode, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, Taxis, AddDate, Summary, ContentNum) VALUES (siteserver_GovPublicCategory_SEQ.NEXTVAL, @ClassCode, @PublishmentSystemID, @CategoryName, @CategoryCode, @ParentID, @ParentsPath, @ParentsCount, @ChildrenCount, @IsLastNode, @Taxis, @AddDate, @Summary, @ContentNum)";
            }

            IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_CLASS_CODE, EDataType.NVarChar, 50, categoryInfo.ClassCode),
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, categoryInfo.PublishmentSystemID),
				this.GetParameter(PARM_CATEGORY_NAME, EDataType.NVarChar, 255, categoryInfo.CategoryName),
                this.GetParameter(PARM_CATEGORY_CODE, EDataType.VarChar, 50, categoryInfo.CategoryCode),
				this.GetParameter(PARM_PARENT_ID, EDataType.Integer, categoryInfo.ParentID),
				this.GetParameter(PARM_PARENTS_PATH, EDataType.NVarChar, 255, categoryInfo.ParentsPath),
				this.GetParameter(PARM_PARENTS_COUNT, EDataType.Integer, categoryInfo.ParentsCount),
				this.GetParameter(PARM_CHILDREN_COUNT, EDataType.Integer, 0),
				this.GetParameter(PARM_IS_LAST_NODE, EDataType.VarChar, 18, true.ToString()),
				this.GetParameter(PARM_TAXIS, EDataType.Integer, categoryInfo.Taxis),
				this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, categoryInfo.AddDate),
				this.GetParameter(PARM_SUMMARY, EDataType.NVarChar, 255, categoryInfo.Summary),
				this.GetParameter(PARM_CONTENT_NUM, EDataType.Integer, categoryInfo.ContentNum)
			};

            string sqlString = string.Format("UPDATE siteserver_GovPublicCategory SET Taxis = Taxis + 1 WHERE (ClassCode = '{0}' AND PublishmentSystemID = {1} AND Taxis >= {2})", categoryInfo.ClassCode, categoryInfo.PublishmentSystemID, categoryInfo.Taxis);
            this.ExecuteNonQuery(trans, sqlString);

            this.ExecuteNonQuery(trans, SQL_INSERT, insertParms);

            categoryInfo.CategoryID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "siteserver_GovPublicCategory");

            if (!string.IsNullOrEmpty(categoryInfo.ParentsPath))
            {
                sqlString = string.Concat("UPDATE siteserver_GovPublicCategory SET ChildrenCount = ChildrenCount + 1 WHERE CategoryID in (", categoryInfo.ParentsPath, ")");

                this.ExecuteNonQuery(trans, sqlString);
            }

            sqlString = string.Format("UPDATE siteserver_GovPublicCategory SET IsLastNode = 'False' WHERE ParentID = {0} AND ClassCode = '{1}' AND PublishmentSystemID = {2}", categoryInfo.ParentID, categoryInfo.ClassCode, categoryInfo.PublishmentSystemID);

            this.ExecuteNonQuery(trans, sqlString);

            sqlString = string.Format("UPDATE siteserver_GovPublicCategory SET IsLastNode = 'True' WHERE (CategoryID IN (SELECT TOP 1 CategoryID FROM siteserver_GovPublicCategory WHERE ParentID = {0} ORDER BY Taxis DESC))", categoryInfo.ParentID);
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = string.Format(@"UPDATE siteserver_GovPublicCategory SET IsLastNode = 'True' WHERE (CategoryID IN (
SELECT CategoryID FROM (
    SELECT CategoryID FROM siteserver_GovPublicCategory WHERE ParentID = {0} ORDER BY Taxis DESC
) WHERE ROWNUM <= 1
))", categoryInfo.ParentID);
            }
            this.ExecuteNonQuery(trans, sqlString);
        }

        private void UpdateSubtractChildrenCount(string parentsPath, int subtractNum)
        {
            if (!string.IsNullOrEmpty(parentsPath))
            {
                string sqlString = string.Format("UPDATE siteserver_GovPublicCategory SET ChildrenCount = ChildrenCount - {0} WHERE CategoryID IN ({1})", subtractNum, parentsPath);
                this.ExecuteNonQuery(sqlString);
            }
        }

        private void TaxisSubtract(string classCode, int publishmentSystemID, int selectedCategoryID)
        {
            GovPublicCategoryInfo categoryInfo = this.GetCategoryInfo(selectedCategoryID);
            if (categoryInfo == null) return;
            //Get Lower Taxis and CategoryID
            int lowerCategoryID = 0;
            int lowerChildrenCount = 0;
            string lowerParentsPath = "";
            string sqlString = string.Format(@"SELECT TOP 1 CategoryID, ChildrenCount, ParentsPath
FROM siteserver_GovPublicCategory
WHERE (ClassCode = '{0}' AND PublishmentSystemID = {1}) AND (ParentID = {2}) AND (CategoryID <> {3}) AND (Taxis < {4})
ORDER BY Taxis DESC", classCode, publishmentSystemID, categoryInfo.ParentID, categoryInfo.CategoryID, categoryInfo.Taxis);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    lowerCategoryID = Convert.ToInt32(rdr[0]);
                    lowerChildrenCount = Convert.ToInt32(rdr[1]);
                    lowerParentsPath = rdr.GetValue(2).ToString();
                }
                else
                {
                    return;
                }
                rdr.Close();
            }


            string lowerNodePath = String.Concat(lowerParentsPath, ",", lowerCategoryID);
            string selectedNodePath = String.Concat(categoryInfo.ParentsPath, ",", categoryInfo.CategoryID);

            this.SetTaxisSubtract(classCode, publishmentSystemID, selectedCategoryID, selectedNodePath, lowerChildrenCount + 1);
            this.SetTaxisAdd(classCode, publishmentSystemID, lowerCategoryID, lowerNodePath, categoryInfo.ChildrenCount + 1);

            this.UpdateIsLastNode(classCode, publishmentSystemID, categoryInfo.ParentID);

        }

        private void TaxisAdd(string classCode, int publishmentSystemID, int selectedCategoryID)
        {
            GovPublicCategoryInfo categoryInfo = this.GetCategoryInfo(selectedCategoryID);
            if (categoryInfo == null) return;
            //Get Higher Taxis and CategoryID
            int higherCategoryID = 0;
            int higherChildrenCount = 0;
            string higherParentsPath = "";
            string sqlString = string.Format(@"SELECT TOP 1 CategoryID, ChildrenCount, ParentsPath
FROM siteserver_GovPublicCategory
WHERE (ClassCode = '{0}' AND PublishmentSystemID = {1}) AND (ParentID = {2}) AND (CategoryID <> {3}) AND (Taxis > {4})
ORDER BY Taxis", classCode, publishmentSystemID, categoryInfo.ParentID,categoryInfo.CategoryID, categoryInfo.Taxis);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    higherCategoryID = Convert.ToInt32(rdr[0]);
                    higherChildrenCount = Convert.ToInt32(rdr[1]);
                    higherParentsPath = rdr.GetValue(2).ToString();
                }
                else
                {
                    return;
                }
                rdr.Close();
            }


            string higherNodePath = String.Concat(higherParentsPath, ",", higherCategoryID);
            string selectedNodePath = String.Concat(categoryInfo.ParentsPath, ",", categoryInfo.CategoryID);

            this.SetTaxisAdd(classCode, publishmentSystemID, selectedCategoryID, selectedNodePath, higherChildrenCount + 1);
            this.SetTaxisSubtract(classCode, publishmentSystemID, higherCategoryID, higherNodePath, categoryInfo.ChildrenCount + 1);

            this.UpdateIsLastNode(categoryInfo.ClassCode, categoryInfo.PublishmentSystemID, categoryInfo.ParentID);
        }

        private void SetTaxisAdd(string classCode, int publishmentSystemID, int categoryID, string parentsPath, int addNum)
        {
            string sqlString = string.Format("UPDATE siteserver_GovPublicCategory SET Taxis = Taxis + {0} WHERE (ClassCode = '{1}' AND PublishmentSystemID = {2}) AND (CategoryID = {3} OR ParentsPath = '{4}' OR ParentsPath LIKE '{4},%')", addNum, classCode, publishmentSystemID, categoryID, parentsPath);

            this.ExecuteNonQuery(sqlString);
        }

        private void SetTaxisSubtract(string classCode, int publishmentSystemID, int categoryID, string parentsPath, int subtractNum)
        {
            string sqlString = string.Format("UPDATE siteserver_GovPublicCategory SET Taxis = Taxis - {0} WHERE (ClassCode = '{1}' AND PublishmentSystemID = {2}) AND (CategoryID = {3} OR ParentsPath = '{4}' OR ParentsPath LIKE '{4},%')", subtractNum, classCode, publishmentSystemID, categoryID, parentsPath);

            this.ExecuteNonQuery(sqlString);
        }

        private void UpdateIsLastNode(string classCode, int publishmentSystemID, int parentID)
        {
            if (parentID > 0)
            {
                string sqlString = string.Format("UPDATE siteserver_GovPublicCategory SET IsLastNode = 'False' WHERE (ClassCode = '{0}' AND PublishmentSystemID = {1}) AND (ParentID = {2})", classCode, publishmentSystemID, parentID);

                this.ExecuteNonQuery(sqlString);

                sqlString = string.Format("UPDATE siteserver_GovPublicCategory SET IsLastNode = 'True' WHERE (CategoryID IN (SELECT TOP 1 CategoryID FROM siteserver_GovPublicCategory WHERE (ClassCode = '{0}' AND PublishmentSystemID = {1}) AND (ParentID = {2}) ORDER BY Taxis DESC))", classCode, publishmentSystemID, parentID);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    sqlString = string.Format(@"UPDATE siteserver_GovPublicCategory SET IsLastNode = 'True' WHERE (CategoryID IN (
SELECT CategoryID FROM (
    SELECT CategoryID FROM siteserver_GovPublicCategory WHERE (ClassCode = '{0}' AND PublishmentSystemID = {1}) AND (ParentID = {2}) ORDER BY Taxis DESC
) WHERE ROWNUM <= 1
))", classCode, publishmentSystemID, parentID);
                }

                this.ExecuteNonQuery(sqlString);
            }
        }

        private int GetMaxTaxisByParentPath(string classCode, int publishmentSystemID, string parentPath)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) AS MaxTaxis FROM siteserver_GovPublicCategory WHERE (ClassCode = '{0}' AND PublishmentSystemID = {1}) AND (ParentsPath = '{2}' OR ParentsPath LIKE '{2},%')", classCode, publishmentSystemID, parentPath);
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

        private int GetParentID(int categoryID)
        {
            int parentID = 0;

            IDbDataParameter[] nodeParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_CATEGORY_ID, EDataType.Integer, categoryID)
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

        private int GetCategoryIDByParentIDAndOrder(string classCode, int publishmentSystemID, int parentID, int order)
        {
            int CategoryID = parentID;

            string CMD = string.Format("SELECT CategoryID FROM siteserver_GovPublicCategory WHERE (ClassCode = '{0}' AND PublishmentSystemID = {1}) AND (ParentID = {2}) ORDER BY Taxis", classCode, publishmentSystemID, parentID);

            using (IDataReader rdr = this.ExecuteReader(CMD))
            {
                int index = 1;
                while (rdr.Read())
                {
                    CategoryID = Convert.ToInt32(rdr[0]);
                    if (index == order)
                        break;
                    index++;
                }
                rdr.Close();
            }
            return CategoryID;
        }

        public int Insert(GovPublicCategoryInfo categoryInfo)
        {
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        GovPublicCategoryInfo parentDepartmentInfo = this.GetCategoryInfo(categoryInfo.ParentID);

                        this.InsertWithTrans(parentDepartmentInfo, categoryInfo, trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return categoryInfo.CategoryID;
        }

        public void Update(GovPublicCategoryInfo categoryInfo)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_CATEGORY_NAME, EDataType.NVarChar, 255, categoryInfo.CategoryName),
                this.GetParameter(PARM_CATEGORY_CODE, EDataType.VarChar, 50, categoryInfo.CategoryCode),
				this.GetParameter(PARM_PARENTS_PATH, EDataType.NVarChar, 255, categoryInfo.ParentsPath),
				this.GetParameter(PARM_PARENTS_COUNT, EDataType.Integer, categoryInfo.ParentsCount),
				this.GetParameter(PARM_CHILDREN_COUNT, EDataType.Integer, categoryInfo.ChildrenCount),
				this.GetParameter(PARM_IS_LAST_NODE, EDataType.VarChar, 18, categoryInfo.IsLastNode.ToString()),
				this.GetParameter(PARM_SUMMARY, EDataType.NVarChar, 255, categoryInfo.Summary),
				this.GetParameter(PARM_CONTENT_NUM, EDataType.Integer, categoryInfo.ContentNum),
				this.GetParameter(PARM_CATEGORY_ID, EDataType.Integer, categoryInfo.CategoryID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, updateParms);
        }

        public void UpdateTaxis(string classCode, int publishmentSystemID, int selectedCategoryID, bool isSubtract)
        {
            if (isSubtract)
            {
                this.TaxisSubtract(classCode, publishmentSystemID, selectedCategoryID);
            }
            else
            {
                this.TaxisAdd(classCode, publishmentSystemID, selectedCategoryID);
            }
        }

        public virtual void UpdateContentNum(PublishmentSystemInfo publishmentSystemInfo, string contentAttributeName, int categoryID)
        {
            string sqlString = string.Format("UPDATE siteserver_GovPublicCategory SET ContentNum = (SELECT COUNT(*) AS ContentNum FROM {0} WHERE ({1} = {2})) WHERE (CategoryID = {2})", publishmentSystemInfo.AuxiliaryTableForGovPublic, contentAttributeName, categoryID);

            this.ExecuteNonQuery(sqlString);
        }

        public void Delete(int categoryID)
        {
            GovPublicCategoryInfo categoryInfo = this.GetCategoryInfo(categoryID);
            if (categoryInfo != null)
            {
                ArrayList categoryIDArrayList = new ArrayList();
                if (categoryInfo.ChildrenCount > 0)
                {
                    categoryIDArrayList = this.GetCategoryIDArrayListForDescendant(categoryInfo.ClassCode, categoryInfo.PublishmentSystemID, categoryID);
                }
                categoryIDArrayList.Add(categoryID);

                string sqlString = string.Format("DELETE FROM siteserver_GovPublicCategory WHERE CategoryID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(categoryIDArrayList));

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
                                string sqlStringTaxis = string.Format("UPDATE siteserver_GovPublicCategory SET Taxis = Taxis - {0} WHERE ClassCode = '{1}' AND  PublishmentSystemID = {2} AND Taxis > {3}", deletedNum, categoryInfo.ClassCode, categoryInfo.PublishmentSystemID, categoryInfo.Taxis);
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
                this.UpdateIsLastNode(categoryInfo.ClassCode, categoryInfo.PublishmentSystemID, categoryInfo.ParentID);
                this.UpdateSubtractChildrenCount(categoryInfo.ParentsPath, deletedNum);
            }
        }

        public GovPublicCategoryInfo GetCategoryInfo(int categoryID)
		{
            GovPublicCategoryInfo categoryInfo = null;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_CATEGORY_ID, EDataType.Integer, categoryID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms)) 
			{
				if (rdr.Read()) 
				{
                    categoryInfo = new GovPublicCategoryInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetInt32(5), rdr.GetValue(6).ToString(), rdr.GetInt32(7), rdr.GetInt32(8), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), rdr.GetInt32(10), rdr.GetDateTime(11), rdr.GetValue(12).ToString(), rdr.GetInt32(13));
				}
				rdr.Close();
			}
            return categoryInfo;
		}

        public string GetCategoryName(int categoryID)
        {
            string departmentName = string.Empty;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_CATEGORY_ID, EDataType.Integer, categoryID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_NAME, parms))
            {
                if (rdr.Read())
                {
                    departmentName = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }
            return departmentName;
        }

		public int GetNodeCount(int categoryID)
		{
			int nodeCount = 0;

			IDbDataParameter[] nodeParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PARENT_ID, EDataType.Integer, categoryID)
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

		public bool IsExists(int categoryID)
		{
			bool exists = false;

			IDbDataParameter[] nodeParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_CATEGORY_ID, EDataType.Integer, categoryID)
			};

			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ID, nodeParms))
			{
				if (rdr.Read())
				{
					if (!rdr.IsDBNull(0))
					{
						exists = true;
					}
				}
				rdr.Close();
			}
			return exists;
		}

        public ArrayList GetCategoryIDArrayList(string classCode, int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();
            string sqlString = string.Format("SELECT CategoryID FROM siteserver_GovPublicCategory WHERE ClassCode = '{0}' AND PublishmentSystemID = {1} ORDER BY Taxis", classCode, publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int categoryID = Convert.ToInt32(rdr[0]);
                    arraylist.Add(categoryID);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public ArrayList GetCategoryIDArrayListByParentID(string classCode, int publishmentSystemID, int parentID)
        {
            string sqlString = string.Format(@"SELECT CategoryID FROM siteserver_GovPublicCategory WHERE ClassCode = '{0}' AND  PublishmentSystemID = {1} AND ParentID = {2} ORDER BY Taxis", classCode, publishmentSystemID, parentID);
            ArrayList list = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int theCategoryID = Convert.ToInt32(rdr[0]);
                    list.Add(theCategoryID);
                }
                rdr.Close();
            }

            return list;
        }

		public ArrayList GetCategoryIDArrayListForDescendant(string classCode, int publishmentSystemID, int categoryID)
		{
            string sqlString = string.Format(@"SELECT CategoryID
FROM siteserver_GovPublicCategory
WHERE (ClassCode = '{0}' AND PublishmentSystemID = {1}) AND (
      (ParentsPath LIKE '{2},%') OR
      (ParentsPath LIKE '%,{2},%') OR
      (ParentsPath LIKE '%,{2}') OR
      (ParentID = {2}))
", classCode, publishmentSystemID, categoryID);
			ArrayList list = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int theCategoryID = Convert.ToInt32(rdr[0]);
                    list.Add(theCategoryID);
                }
                rdr.Close();
            }

			return list;
		}
	}
}
