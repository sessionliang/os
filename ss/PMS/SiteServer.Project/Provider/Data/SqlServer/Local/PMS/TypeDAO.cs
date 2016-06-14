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
    public class TypeDAO : DataProviderBase, ITypeDAO 
	{
        private const string SQL_UPDATE = "UPDATE pms_Type SET TypeName = @TypeName WHERE TypeID = @TypeID";

        private const string SQL_DELETE = "DELETE FROM pms_Type WHERE TypeID = @TypeID";

        private const string SQL_SELECT = "SELECT TypeID, TypeName, ProjectID, Taxis FROM pms_Type WHERE TypeID = @TypeID";

        private const string SQL_SELECT_NAME = "SELECT TypeName FROM pms_Type WHERE TypeID = @TypeID";

        private const string SQL_SELECT_ALL = "SELECT TypeID, TypeName, ProjectID, Taxis FROM pms_Type WHERE ProjectID = @ProjectID ORDER BY Taxis";

        private const string SQL_SELECT_INTERACT_NAME = "SELECT TypeName FROM pms_Type WHERE ProjectID = @ProjectID ORDER BY Taxis";

        private const string PARM_TYPE_ID = "@TypeID";
        private const string PARM_INTERACT_NAME = "@TypeName";
        private const string PARM_INTERACT_ID = "@ProjectID";
        private const string PARM_TAXIS = "@Taxis";

		public void Insert(TypeInfo typeInfo) 
		{
            string sqlString = "INSERT INTO pms_Type (TypeName, ProjectID, Taxis) VALUES (@TypeName, @ProjectID, @Taxis)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO pms_Type(TypeID, TypeName, ProjectID, Taxis) VALUES (pms_Type_SEQ.NEXTVAL, @TypeName, @ProjectID, @Taxis)";
            }

            int taxis = this.GetMaxTaxis(typeInfo.ProjectID) + 1;
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_INTERACT_NAME, EDataType.NVarChar, 50, typeInfo.TypeName),
				this.GetParameter(PARM_INTERACT_ID, EDataType.Integer, typeInfo.ProjectID),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, taxis)
			};

            this.ExecuteNonQuery(sqlString, parms);
		}

        public void Update(TypeInfo typeInfo) 
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_INTERACT_NAME, EDataType.NVarChar, 50, typeInfo.TypeName),
				this.GetParameter(PARM_TYPE_ID, EDataType.Integer, typeInfo.TypeID),
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

        public TypeInfo GetTypeInfo(int typeID)
		{
            TypeInfo typeInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TYPE_ID, EDataType.Integer, typeID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    typeInfo = new TypeInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetInt32(3));
                }
                rdr.Close();
            }

            return typeInfo;
		}

        public string GetTypeName(int typeID)
        {
            string typeName = string.Empty;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TYPE_ID, EDataType.Integer, typeID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_NAME, parms))
            {
                if (rdr.Read())
                {
                    typeName = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }

            return typeName;
        }

        public IEnumerable GetDataSource(int projectID)
		{
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_INTERACT_ID, EDataType.Integer, projectID)
			};
            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL, parms);
			return enumerable;
		}

        public ArrayList GetTypeInfoArrayList(int projectID)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_INTERACT_ID, EDataType.Integer, projectID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL, parms))
            {
                while (rdr.Read())
                {
                    TypeInfo typeInfo = new TypeInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetInt32(3));
                    arraylist.Add(typeInfo);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public ArrayList GetTypeNameArrayList(int projectID)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] selectParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_INTERACT_ID, EDataType.Integer, projectID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_INTERACT_NAME, selectParms))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }

            return arraylist;
        }

        public bool UpdateTaxisToUp(int typeID, int projectID)
        {
            string sqlString = string.Format("SELECT TOP 1 TypeID, Taxis FROM pms_Type WHERE ((Taxis > (SELECT Taxis FROM pms_Type WHERE TypeID = {0})) AND ProjectID ={1}) ORDER BY Taxis", typeID, projectID);
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
                SetTaxis(typeID, projectID, higherTaxis);
                SetTaxis(higherID, projectID, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToDown(int typeID, int projectID)
        {
            string sqlString = string.Format("SELECT TOP 1 TypeID, Taxis FROM pms_Type WHERE ((Taxis < (SELECT Taxis FROM pms_Type WHERE TypeID = {0})) AND ProjectID = {1}) ORDER BY Taxis DESC", typeID, projectID);
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
                SetTaxis(typeID, projectID, lowerTaxis);
                SetTaxis(lowerID, projectID, selectedTaxis);
                return true;
            }
            return false;
        }

        private int GetMaxTaxis(int projectID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM pms_Type WHERE ProjectID = {0}", projectID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        private int GetTaxis(int typeID)
        {
            string sqlString = string.Format("SELECT Taxis FROM pms_Type WHERE TypeID = {0}", typeID);
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

        private void SetTaxis(int typeID, int projectID, int taxis)
        {
            string sqlString = string.Format("UPDATE pms_Type SET Taxis = {0} WHERE TypeID = {1} AND ProjectID = {2}", taxis, typeID, projectID);
            this.ExecuteNonQuery(sqlString);
        }
	}
}