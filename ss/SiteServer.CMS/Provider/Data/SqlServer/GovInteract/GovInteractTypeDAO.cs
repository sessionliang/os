using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class GovInteractTypeDAO : DataProviderBase, IGovInteractTypeDAO 
	{
        private const string SQL_UPDATE = "UPDATE siteserver_GovInteractType SET TypeName = @TypeName WHERE TypeID = @TypeID";

        private const string SQL_DELETE = "DELETE FROM siteserver_GovInteractType WHERE TypeID = @TypeID";

        private const string SQL_SELECT = "SELECT TypeID, TypeName, NodeID, PublishmentSystemID, Taxis FROM siteserver_GovInteractType WHERE TypeID = @TypeID";

        private const string SQL_SELECT_NAME = "SELECT TypeName FROM siteserver_GovInteractType WHERE TypeID = @TypeID";

        private const string SQL_SELECT_ALL = "SELECT TypeID, TypeName, NodeID, PublishmentSystemID, Taxis FROM siteserver_GovInteractType WHERE NodeID = @NodeID ORDER BY Taxis";

        private const string SQL_SELECT_INTERACT_NAME = "SELECT TypeName FROM siteserver_GovInteractType WHERE NodeID = @NodeID ORDER BY Taxis";

        private const string PARM_TYPE_ID = "@TypeID";
        private const string PARM_INTERACT_NAME = "@TypeName";
        private const string PARM_NODE_ID = "@NodeID";
        private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_TAXIS = "@Taxis";

		public void Insert(GovInteractTypeInfo typeInfo) 
		{
            string sqlString = "INSERT INTO siteserver_GovInteractType (TypeName, NodeID, PublishmentSystemID, Taxis) VALUES (@TypeName, @NodeID, @PublishmentSystemID, @Taxis)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO siteserver_GovInteractType(TypeID, TypeName, NodeID, PublishmentSystemID, Taxis) VALUES (siteserver_GovInteractType_SEQ.NEXTVAL, @TypeName, @NodeID, @PublishmentSystemID, @Taxis)";
            }

            int taxis = this.GetMaxTaxis(typeInfo.NodeID) + 1;
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_INTERACT_NAME, EDataType.NVarChar, 50, typeInfo.TypeName),
				this.GetParameter(PARM_NODE_ID, EDataType.Integer, typeInfo.NodeID),
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, typeInfo.PublishmentSystemID),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, taxis)
			};

            this.ExecuteNonQuery(sqlString, parms);
		}

        public void Update(GovInteractTypeInfo typeInfo) 
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

        public GovInteractTypeInfo GetTypeInfo(int typeID)
		{
            GovInteractTypeInfo typeInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TYPE_ID, EDataType.Integer, typeID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    typeInfo = new GovInteractTypeInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetInt32(4));
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

        public IEnumerable GetDataSource(int nodeID)
		{
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID)
			};
            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL, parms);
			return enumerable;
		}

        public ArrayList GetTypeInfoArrayList(int nodeID)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL, parms))
            {
                while (rdr.Read())
                {
                    GovInteractTypeInfo typeInfo = new GovInteractTypeInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetInt32(4));
                    arraylist.Add(typeInfo);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public ArrayList GetTypeNameArrayList(int nodeID)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] selectParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID)
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

        public bool UpdateTaxisToUp(int typeID, int nodeID)
        {
            string sqlString = string.Format("SELECT TOP 1 TypeID, Taxis FROM siteserver_GovInteractType WHERE ((Taxis > (SELECT Taxis FROM siteserver_GovInteractType WHERE TypeID = {0})) AND NodeID ={1}) ORDER BY Taxis", typeID, nodeID);
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
                SetTaxis(typeID, nodeID, higherTaxis);
                SetTaxis(higherID, nodeID, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToDown(int typeID, int nodeID)
        {
            string sqlString = string.Format("SELECT TOP 1 TypeID, Taxis FROM siteserver_GovInteractType WHERE ((Taxis < (SELECT Taxis FROM siteserver_GovInteractType WHERE TypeID = {0})) AND NodeID = {1}) ORDER BY Taxis DESC", typeID, nodeID);
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
                SetTaxis(typeID, nodeID, lowerTaxis);
                SetTaxis(lowerID, nodeID, selectedTaxis);
                return true;
            }
            return false;
        }

        private int GetMaxTaxis(int nodeID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM siteserver_GovInteractType WHERE NodeID = {0}", nodeID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        private int GetTaxis(int typeID)
        {
            string sqlString = string.Format("SELECT Taxis FROM siteserver_GovInteractType WHERE TypeID = {0}", typeID);
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

        private void SetTaxis(int typeID, int nodeID, int taxis)
        {
            string sqlString = string.Format("UPDATE siteserver_GovInteractType SET Taxis = {0} WHERE TypeID = {1} AND NodeID = {2}", taxis, typeID, nodeID);
            this.ExecuteNonQuery(sqlString);
        }
	}
}