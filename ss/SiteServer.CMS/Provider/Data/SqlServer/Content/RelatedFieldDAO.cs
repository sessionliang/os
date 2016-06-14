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
    public class RelatedFieldDAO : DataProviderBase, IRelatedFieldDAO
	{
        private const string SQL_UPDATE = "UPDATE siteserver_RelatedField SET RelatedFieldName = @RelatedFieldName, TotalLevel = @TotalLevel, Prefixes = @Prefixes, Suffixes = @Suffixes WHERE RelatedFieldID = @RelatedFieldID";
        private const string SQL_DELETE = "DELETE FROM siteserver_RelatedField WHERE RelatedFieldID = @RelatedFieldID";

        private const string PARM_RELATED_FIELD_ID = "@RelatedFieldID";
        private const string PARM_RELATED_FIELD_NAME = "@RelatedFieldName";
		private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_TOTAL_LEVEL = "@TotalLevel";
        private const string PARM_PREFIXES = "@Prefixes";
        private const string PARM_SUFFIXES = "@Suffixes";

		public int Insert(RelatedFieldInfo relatedFieldInfo) 
		{
            int relatedFieldID = 0;

            string sqlString = "INSERT INTO siteserver_RelatedField (RelatedFieldName, PublishmentSystemID, TotalLevel, Prefixes, Suffixes) VALUES (@RelatedFieldName, @PublishmentSystemID, @TotalLevel, @Prefixes, @Suffixes)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO siteserver_RelatedField (RelatedFieldID, RelatedFieldName, PublishmentSystemID, TotalLevel, Prefixes, Suffixes) VALUES (siteserver_RelatedField_SEQ.NEXTVAL, @RelatedFieldName, @PublishmentSystemID, @TotalLevel, @Prefixes, @Suffixes)";
            }

			IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_RELATED_FIELD_NAME, EDataType.NVarChar, 50, relatedFieldInfo.RelatedFieldName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, relatedFieldInfo.PublishmentSystemID),
                this.GetParameter(PARM_TOTAL_LEVEL, EDataType.Integer, relatedFieldInfo.TotalLevel),
                this.GetParameter(PARM_PREFIXES, EDataType.NVarChar, 255, relatedFieldInfo.Prefixes),
                this.GetParameter(PARM_SUFFIXES, EDataType.NVarChar, 255, relatedFieldInfo.Suffixes),
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, insertParms);
                        relatedFieldID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "siteserver_RelatedField");
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return relatedFieldID;
		}

        public void Update(RelatedFieldInfo relatedFieldInfo) 
		{
			IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_RELATED_FIELD_NAME, EDataType.NVarChar, 50, relatedFieldInfo.RelatedFieldName),
                this.GetParameter(PARM_TOTAL_LEVEL, EDataType.Integer, relatedFieldInfo.TotalLevel),
                this.GetParameter(PARM_PREFIXES, EDataType.NVarChar, 255, relatedFieldInfo.Prefixes),
                this.GetParameter(PARM_SUFFIXES, EDataType.NVarChar, 255, relatedFieldInfo.Suffixes),
				this.GetParameter(PARM_RELATED_FIELD_ID, EDataType.Integer, relatedFieldInfo.RelatedFieldID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, updateParms);
		}

		public void Delete(int relatedFieldID)
		{
			IDbDataParameter[] relatedFieldInfoParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_RELATED_FIELD_ID, EDataType.Integer, relatedFieldID)
			};

            this.ExecuteNonQuery(SQL_DELETE, relatedFieldInfoParms);
		}

        public RelatedFieldInfo GetRelatedFieldInfo(int relatedFieldID)
		{
			RelatedFieldInfo relatedFieldInfo = null;

            if (relatedFieldID > 0)
            {
                string sqlString = string.Format("SELECT RelatedFieldID, RelatedFieldName, PublishmentSystemID, TotalLevel, Prefixes, Suffixes FROM siteserver_RelatedField WHERE RelatedFieldID = {0}", relatedFieldID);

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    if (rdr.Read())
                    {
                        relatedFieldInfo = new RelatedFieldInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString());
                    }
                    rdr.Close();
                }
            }

			return relatedFieldInfo;
		}

        public RelatedFieldInfo GetRelatedFieldInfo(int publishmentSystemID, string relatedFieldName)
        {
            RelatedFieldInfo relatedFieldInfo = null;

            string sqlString = string.Format("SELECT RelatedFieldID, RelatedFieldName, PublishmentSystemID, TotalLevel, Prefixes, Suffixes FROM siteserver_RelatedField WHERE PublishmentSystemID = {0} AND RelatedFieldName = @RelatedFieldName", publishmentSystemID);

            IDbDataParameter[] selectParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_RELATED_FIELD_NAME, EDataType.NVarChar, 255, relatedFieldName)			 
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, selectParms))
            {
                if (rdr.Read())
                {
                    relatedFieldInfo = new RelatedFieldInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString());
                }
                rdr.Close();
            }

            return relatedFieldInfo;
        }

        public string GetRelatedFieldName(int relatedFieldID)
        {
            string relatedFieldName = string.Empty;

            string sqlString = string.Format("SELECT RelatedFieldName FROM siteserver_RelatedField WHERE RelatedFieldID = {0}", relatedFieldID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    relatedFieldName = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }

            return relatedFieldName;
        }

		public IEnumerable GetDataSource(int publishmentSystemID)
		{
            string sqlString = string.Format("SELECT RelatedFieldID, RelatedFieldName, PublishmentSystemID, TotalLevel, Prefixes, Suffixes FROM siteserver_RelatedField WHERE PublishmentSystemID = {0} ORDER BY RelatedFieldID", publishmentSystemID);
			IEnumerable enumerable = (IEnumerable)this.ExecuteReader(sqlString);
			return enumerable;
		}

		public ArrayList GetRelatedFieldInfoArrayList(int publishmentSystemID)
		{
			ArrayList arraylist = new ArrayList();
            string sqlString = string.Format("SELECT RelatedFieldID, RelatedFieldName, PublishmentSystemID, TotalLevel, Prefixes, Suffixes FROM siteserver_RelatedField WHERE PublishmentSystemID = {0} ORDER BY RelatedFieldID", publishmentSystemID);

			using (IDataReader rdr = this.ExecuteReader(sqlString)) 
			{
				while (rdr.Read()) 
				{
                    arraylist.Add(new RelatedFieldInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString()));
				}
				rdr.Close();
			}

			return arraylist;
		}

		public ArrayList GetRelatedFieldNameArrayList(int publishmentSystemID)
		{
			ArrayList arraylist = new ArrayList();
            string sqlString = string.Format("SELECT RelatedFieldName FROM siteserver_RelatedField WHERE PublishmentSystemID = {0} ORDER BY RelatedFieldID", publishmentSystemID);
			
			using (IDataReader rdr = this.ExecuteReader(sqlString)) 
			{
				while (rdr.Read()) 
				{
                    arraylist.Add(rdr.GetValue(0).ToString());
				}
				rdr.Close();
			}

			return arraylist;
		}

        public string GetImportRelatedFieldName(int publishmentSystemID, string relatedFieldName)
        {
            string importName = "";
            if (relatedFieldName.IndexOf("_") != -1)
            {
                int relatedFieldName_Count = 0;
                string lastName = relatedFieldName.Substring(relatedFieldName.LastIndexOf("_") + 1);
                string firstName = relatedFieldName.Substring(0, relatedFieldName.Length - lastName.Length);
                try
                {
                    relatedFieldName_Count = int.Parse(lastName);
                }
                catch { }
                relatedFieldName_Count++;
                importName = firstName + relatedFieldName_Count;
            }
            else
            {
                importName = relatedFieldName + "_1";
            }

            RelatedFieldInfo relatedFieldInfo = this.GetRelatedFieldInfo(publishmentSystemID, relatedFieldName);
            if (relatedFieldInfo != null)
            {
                importName = GetImportRelatedFieldName(publishmentSystemID, importName);
            }

            return importName;
        }
	}
}