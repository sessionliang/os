using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.Project.Model;
using SiteServer.Project.Core;

namespace SiteServer.Project.Provider.Data.SqlServer
{
    public class DocTypeDAO : DataProviderBase, IDocTypeDAO
	{
        private const string SQL_UPDATE = "UPDATE pms_DocType SET TypeName = @TypeName, Description = @Description, AddDate = @AddDate WHERE TypeID = @TypeID";

        private const string SQL_DELETE = "DELETE FROM pms_DocType WHERE TypeID = @TypeID";

        private const string SQL_SELECT = "SELECT TypeID, ParentID, TypeName, Taxis, Description, AddDate FROM pms_DocType WHERE TypeID = @TypeID";

        private const string SQL_SELECT_BY_NAME = "SELECT TypeID, ParentID, TypeName, Taxis, Description, AddDate FROM pms_DocType WHERE TypeName = @TypeName";

        private const string SQL_SELECT_ALL = "SELECT TypeID, ParentID, TypeName, Taxis, Description, AddDate FROM pms_DocType WHERE ParentID = @ParentID ORDER BY Taxis";

        private const string SQL_SELECT_TYPE_NAME = "SELECT TypeName FROM pms_DocType WHERE ParentID = @ParentID ORDER BY Taxis";

        private const string SQL_SELECT_TYPE_NAME_BY_ID = "SELECT TypeName FROM pms_DocType WHERE TypeID = @TypeID";

        private const string PARM_TYPE_ID = "@TypeID";
        private const string PARM_PARENT_ID = "@ParentID";
        private const string PARM_TYPE_NAME = "@TypeName";
        private const string PARM_TAXIS = "@Taxis";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_ADD_DATE = "@AddDate";

		public int Insert(DocTypeInfo typeInfo) 
		{
            int typeID = 0;

            string sqlString = "INSERT INTO pms_DocType (ParentID, TypeName, Taxis, Description, AddDate) VALUES (@ParentID, @TypeName, @Taxis, @Description, @AddDate)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO pms_DocType(TypeID, ParentID, TypeName, Taxis, Description, AddDate) VALUES (pms_DocType_SEQ.NEXTVAL, @ParentID, @TypeName, @Taxis, @Description, @AddDate)";
            }

            int taxis = this.GetMaxTaxis(typeInfo.ParentID) + 1;
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PARENT_ID, EDataType.Integer, typeInfo.ParentID),
				this.GetParameter(PARM_TYPE_NAME, EDataType.NVarChar, 50, typeInfo.TypeName),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, taxis),
                this.GetParameter(PARM_DESCRIPTION, EDataType.NVarChar, 255, typeInfo.Description),
                this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, typeInfo.AddDate)
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, parms);
                        typeID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "pms_DocType");
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return typeID;
		}

        public void Update(DocTypeInfo typeInfo) 
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_TYPE_NAME, EDataType.NVarChar, 50, typeInfo.TypeName),
                this.GetParameter(PARM_DESCRIPTION, EDataType.NVarChar, 255, typeInfo.Description),
                this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, typeInfo.AddDate),
                this.GetParameter(PARM_TYPE_ID, EDataType.Integer, typeInfo.TypeID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, parms);
		}

		public void Delete(int typeID)
		{
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TYPE_ID, EDataType.Integer, typeID)
			};

            this.ExecuteNonQuery(SQL_DELETE, parms);
		}

        public DocTypeInfo GetTypeInfo(int typeID)
		{
            DocTypeInfo typeInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TYPE_ID, EDataType.Integer, typeID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    typeInfo = new DocTypeInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetInt32(3), rdr.GetValue(4).ToString(), rdr.GetDateTime(5));
                }
                rdr.Close();
            }

            return typeInfo;
		}

        public string GetTypeName(int typeID)
        {
            string typeName = string.Empty;

            IDbDataParameter[] selectParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TYPE_ID, EDataType.Integer, typeID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TYPE_NAME_BY_ID, selectParms))
            {
                if (rdr.Read())
                {
                    typeName = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }

            return typeName;
        }

		public IEnumerable GetDataSource(int parentID)
		{
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PARENT_ID, EDataType.Integer, parentID)
			};

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL, parms);
			return enumerable;
		}

        public int GetCount(int parentID)
        {
            string sqlString = "SELECT COUNT(*) FROM pms_DocType WHERE ParentID = " + parentID;
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public ArrayList GetTypeInfoArrayList(int parentID)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PARENT_ID, EDataType.Integer, parentID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL, parms))
            {
                while (rdr.Read())
                {
                    DocTypeInfo typeInfo = new DocTypeInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetInt32(3), rdr.GetValue(4).ToString(), rdr.GetDateTime(5));
                    arraylist.Add(typeInfo);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public ArrayList GetTypeNameArrayList(int parentID)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PARENT_ID, EDataType.Integer, parentID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TYPE_NAME, parms))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }

            return arraylist;
        }

        public bool UpdateTaxisToUp(int parentID, int typeID)
        {
            string sqlString = string.Format("SELECT TOP 1 TypeID, Taxis FROM pms_DocType WHERE (Taxis > (SELECT Taxis FROM pms_DocType WHERE TypeID = {0} AND ParentID = {1})) AND ParentID = {1} ORDER BY Taxis", typeID, parentID);
            int higherID = 0;
            int higherTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    higherID = rdr.GetInt32(0);
                    higherTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(typeID);

            if (higherID > 0)
            {
                SetTaxis(typeID, higherTaxis);
                SetTaxis(higherID, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToDown(int parentID, int typeID)
        {
            string sqlString = string.Format("SELECT TOP 1 TypeID, Taxis FROM pms_DocType WHERE (Taxis < (SELECT Taxis FROM pms_DocType WHERE TypeID = {0} AND ParentID = {1})) AND ParentID = {1} ORDER BY Taxis DESC", typeID, parentID);
            int lowerID = 0;
            int lowerTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    lowerID = rdr.GetInt32(0);
                    lowerTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(typeID);

            if (lowerID > 0)
            {
                SetTaxis(typeID, lowerTaxis);
                SetTaxis(lowerID, selectedTaxis);
                return true;
            }
            return false;
        }

        private int GetMaxTaxis(int parentID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM pms_DocType WHERE ParentID = {0}", parentID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        private int GetTaxis(int typeID)
        {
            string sqlString = string.Format("SELECT Taxis FROM pms_DocType WHERE TypeID = {0}", typeID);
            int taxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    taxis = Convert.ToInt32(rdr[0]);
                }
                rdr.Close();
            }

            return taxis;
        }

        private void SetTaxis(int typeID, int taxis)
        {
            string sqlString = string.Format("UPDATE pms_DocType SET Taxis = {0} WHERE TypeID = {1}", taxis, typeID);
            this.ExecuteNonQuery(sqlString);
        }
	}
}