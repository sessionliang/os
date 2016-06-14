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
    public class RelatedFieldItemDAO : DataProviderBase, IRelatedFieldItemDAO
    {
        private const string SQL_SELECT = "SELECT ID, RelatedFieldID, ItemName, ItemValue, ParentID, Taxis FROM siteserver_RelatedFieldItem WHERE ID = @ID";

        private const string SQL_UPDATE = "UPDATE siteserver_RelatedFieldItem SET ItemName = @ItemName, ItemValue = @ItemValue WHERE ID = @ID";

        private const string PARM_ID = "@ID";
        private const string PARM_RELATED_FIELD_ID = "@RelatedFieldID";
        private const string PARM_ITEM_NAME = "@ItemName";
        private const string PARM_ITEM_VALUE = "@ItemValue";
        private const string PARM_PARENT_ID = "@ParentID";
        private const string PARM_TAXIS = "@Taxis";

        public int Insert(RelatedFieldItemInfo info)
        {
            int id = 0;

            info.Taxis = this.GetMaxTaxis(info.ParentID) + 1;

            string sqlString = "INSERT INTO siteserver_RelatedFieldItem (RelatedFieldID, ItemName, ItemValue, ParentID, Taxis) VALUES (@RelatedFieldID, @ItemName, @ItemValue, @ParentID, @Taxis)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO siteserver_RelatedFieldItem (ID, RelatedFieldID, ItemName, ItemValue, ParentID, Taxis) VALUES (siteserver_RelatedFieldIte_SEQ.NEXTVAL, @RelatedFieldID, @ItemName, @ItemValue, @ParentID, @Taxis)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_RELATED_FIELD_ID, EDataType.Integer, info.RelatedFieldID),
                this.GetParameter(PARM_ITEM_NAME, EDataType.NVarChar, 255, info.ItemName),
                this.GetParameter(PARM_ITEM_VALUE, EDataType.NVarChar, 255, info.ItemValue),
				this.GetParameter(PARM_PARENT_ID, EDataType.Integer, info.ParentID),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, info.Taxis)
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, parms);
                        if (this.DataBaseType == EDatabaseType.Oracle)
                        {
                            id = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "siteserver_RelatedFieldItem");
                        }
                        else
                        {
                            id = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "siteserver_RelatedFieldItem");
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

            return id;

            //RelatedFieldManager.ClearCache();
        }

        public void Update(RelatedFieldItemInfo info)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ITEM_NAME, EDataType.NVarChar, 255, info.ItemName),
                this.GetParameter(PARM_ITEM_VALUE, EDataType.NVarChar, 255, info.ItemValue),
				this.GetParameter(PARM_ID, EDataType.Integer, info.ID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, parms);

            //RelatedFieldManager.ClearCache();
        }

        public void Delete(int id)
        {
            if (id > 0)
            {
                string sqlString = string.Format("DELETE siteserver_RelatedFieldItem WHERE ID = {0} OR ParentID = {0}", id);
                this.ExecuteNonQuery(sqlString);
            }
            //RelatedFieldManager.ClearCache();
        }

        public IEnumerable GetDataSource(int relatedFieldID, int parentID)
        {
            string sqlString = string.Format("SELECT ID, RelatedFieldID, ItemName, ItemValue, ParentID, Taxis FROM siteserver_RelatedFieldItem WHERE RelatedFieldID = {0} AND ParentID = {1} ORDER BY Taxis", relatedFieldID, parentID);
            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(sqlString);
            return enumerable;
        }

        public void UpdateTaxisToUp(int id, int parentID)
        {
            //Get Higher Taxis and ClassID
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM siteserver_RelatedFieldItem WHERE ((Taxis > (SELECT Taxis FROM siteserver_RelatedFieldItem WHERE ID = {0})) AND ParentID = {1}) ORDER BY Taxis", id, parentID);
            int HigherID = 0;
            int HigherTaxis = 0;

            try
            {
                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    if (rdr.Read())
                    {
                        HigherID = rdr.GetInt32(0);
                        HigherTaxis = rdr.GetInt32(1);
                    }
                    rdr.Close();
                }
            }
            catch
            {
                throw;
            }

            //Get Taxis Of Selected Class
            int SelectedTaxis = GetTaxis(id);

            if (HigherID != 0)
            {
                //Set The Selected Class Taxis To Higher Level
                SetTaxis(id, HigherTaxis);
                //Set The Higher Class Taxis To Lower Level
                SetTaxis(HigherID, SelectedTaxis);
            }

            //RelatedFieldManager.ClearCache();
        }

        public void UpdateTaxisToDown(int id, int parentID)
        {
            //Get Lower Taxis and ClassID
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM siteserver_RelatedFieldItem WHERE ((Taxis < (SELECT Taxis FROM siteserver_RelatedFieldItem WHERE (ID = {0}))) AND ParentID = {1}) ORDER BY Taxis DESC", id, parentID);
            int LowerID = 0;
            int LowerTaxis = 0;

            try
            {
                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    if (rdr.Read())
                    {
                        LowerID = rdr.GetInt32(0);
                        LowerTaxis = rdr.GetInt32(1);
                    }
                    rdr.Close();
                }
            }
            catch
            {
                throw;
            }

            //Get Taxis Of Selected Class
            int SelectedTaxis = GetTaxis(id);

            if (LowerID != 0)
            {
                //Set The Selected Class Taxis To Lower Level
                SetTaxis(id, LowerTaxis);
                //Set The Lower Class Taxis To Higher Level
                SetTaxis(LowerID, SelectedTaxis);
            }

            //RelatedFieldManager.ClearCache();
        }

        private int GetTaxis(int id)
        {
            string CMD = string.Format("SELECT Taxis FROM siteserver_RelatedFieldItem WHERE (ID = {0})", id);
            int taxis = 0;

            try
            {
                using (IDataReader rdr = this.ExecuteReader(CMD))
                {
                    if (rdr.Read())
                    {
                        taxis = rdr.GetInt32(0);
                    }
                    rdr.Close();
                }
            }
            catch
            {
                throw;
            }
            return taxis;
        }

        private void SetTaxis(int id, int taxis)
        {
            string CMD = string.Format("UPDATE siteserver_RelatedFieldItem SET Taxis = {0} WHERE ID = {1}", taxis, id);

            this.ExecuteNonQuery(CMD);
        }

        public int GetMaxTaxis(int parentID)
        {
            int maxTaxis = 0;
            string CMD = string.Format("SELECT MAX(Taxis) FROM siteserver_RelatedFieldItem WHERE ParentID = {0} AND Taxis <> {1}", parentID, int.MaxValue);
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                try
                {
                    object o = this.ExecuteScalar(conn, CMD);
                    if (o is System.DBNull)
                        maxTaxis = 0;
                    else
                        maxTaxis = int.Parse(o.ToString());
                }
                catch
                {
                    throw;
                }
            }
            return maxTaxis;
        }

        public int GetMinTaxis(int parentID)
        {
            int minTaxis = 0;
            string CMD = string.Format("SELECT MIN(Taxis) FROM siteserver_RelatedFieldItem WHERE ParentID = {0}", parentID);
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                try
                {
                    object o = this.ExecuteScalar(conn, CMD);
                    if (o is System.DBNull)
                        minTaxis = 0;
                    else
                        minTaxis = int.Parse(o.ToString());
                }
                catch
                {
                    throw;
                }
            }
            return minTaxis;
        }

        public RelatedFieldItemInfo GetRelatedFieldItemInfo(int id)
        {
            RelatedFieldItemInfo info = null;

            string sqlString = string.Format("SELECT ID, RelatedFieldID, ItemName, ItemValue, ParentID, Taxis FROM siteserver_RelatedFieldItem WHERE ID = {0}", id);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    info = new RelatedFieldItemInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetInt32(4), rdr.GetInt32(5));
                }
                rdr.Close();
            }

            return info;
        }

        public ArrayList GetRelatedFieldItemInfoArrayList(int relatedFieldID, int parentID)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = string.Format("SELECT ID, RelatedFieldID, ItemName, ItemValue, ParentID, Taxis FROM siteserver_RelatedFieldItem WHERE RelatedFieldID = {0} AND ParentID = {1} ORDER BY Taxis", relatedFieldID, parentID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    RelatedFieldItemInfo info = new RelatedFieldItemInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetInt32(4), rdr.GetInt32(5));
                    arraylist.Add(info);
                }
                rdr.Close();
            }

            return arraylist;
        }
    }
}