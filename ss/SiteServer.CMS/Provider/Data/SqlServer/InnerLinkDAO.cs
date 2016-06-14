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
	public class InnerLinkDAO : DataProviderBase, IInnerLinkDAO
	{
        public InnerLinkDAO()
		{
		}

		private const string SQL_INSERT_INNER_LINK = "INSERT INTO siteserver_InnerLink VALUES (@InnerLinkName, @PublishmentSystemID, @LinkUrl)";
		private const string SQL_UPDATE_INNER_LINK = "UPDATE siteserver_InnerLink SET LinkUrl = @LinkUrl WHERE InnerLinkName = @InnerLinkName AND PublishmentSystemID = @PublishmentSystemID";
		private const string SQL_DELETE_INNER_LINK = "DELETE FROM siteserver_InnerLink WHERE InnerLinkName = @InnerLinkName AND PublishmentSystemID = @PublishmentSystemID";

		private const string PARM_INNER_LINK_NAME = "@InnerLinkName";
		private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
		private const string PARM_LINK_URL = "@LinkUrl";


		public void Insert(InnerLinkInfo innerLinkInfo) 
		{
			IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_INNER_LINK_NAME, EDataType.NVarChar, 255, innerLinkInfo.InnerLinkName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, innerLinkInfo.PublishmentSystemID),
				this.GetParameter(PARM_LINK_URL, EDataType.VarChar, 200, innerLinkInfo.LinkUrl)
			};

            base.ExecuteNonQuery(SQL_INSERT_INNER_LINK, insertParms);
		}

        public void Update(InnerLinkInfo innerLinkInfo) 
		{
			IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_LINK_URL, EDataType.VarChar, 200, innerLinkInfo.LinkUrl),
				this.GetParameter(PARM_INNER_LINK_NAME, EDataType.NVarChar, 255, innerLinkInfo.InnerLinkName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, innerLinkInfo.PublishmentSystemID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_INNER_LINK, updateParms);
		}

		public void Delete(string innerLinkName, int publishmentSystemID)
		{
			IDbDataParameter[] innerLinkParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_INNER_LINK_NAME, EDataType.NVarChar, 255, innerLinkName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

            this.ExecuteNonQuery(SQL_DELETE_INNER_LINK, innerLinkParms);
		}

		public InnerLinkInfo GetInnerLinkInfo(string innerLinkName, int publishmentSystemID)
		{
			InnerLinkInfo innerLinkInfo = null;

            string sqlString = string.Format("SELECT InnerLinkName, PublishmentSystemID, LinkUrl FROM siteserver_InnerLink WHERE InnerLinkName = @InnerLinkName AND PublishmentSystemID = 0");
			if (publishmentSystemID != 0)
			{
                sqlString = string.Format("SELECT InnerLinkName, PublishmentSystemID, LinkUrl FROM siteserver_InnerLink WHERE InnerLinkName = @InnerLinkName AND (PublishmentSystemID = 0 OR PublishmentSystemID = @PublishmentSystemID)");
			}
            IDbDataParameter[] selectParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_INNER_LINK_NAME, EDataType.NVarChar, 255, innerLinkName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)				 
			};
            using (IDataReader rdr = this.ExecuteReader(sqlString, selectParms)) 
			{
				if (rdr.Read()) 
				{
                    innerLinkInfo = new InnerLinkInfo(rdr.GetValue(0).ToString(), rdr.GetInt32(1), rdr.GetValue(2).ToString());
				}
				rdr.Close();
			}

            return innerLinkInfo;
		}

        public bool IsExists(string innerLinkName, int publishmentSystemID)
        {
            bool exists = false;

            string sqlString = string.Format("SELECT InnerLinkName, PublishmentSystemID, LinkUrl FROM siteserver_InnerLink WHERE InnerLinkName = @InnerLinkName AND PublishmentSystemID = 0");
            if (publishmentSystemID != 0)
            {
                sqlString = string.Format("SELECT InnerLinkName, PublishmentSystemID, LinkUrl FROM siteserver_InnerLink WHERE InnerLinkName = @InnerLinkName AND (PublishmentSystemID = 0 OR PublishmentSystemID = @PublishmentSystemID)");
            }
            IDbDataParameter[] selectParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_INNER_LINK_NAME, EDataType.NVarChar, 255, innerLinkName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)				 
			};
            using (IDataReader rdr = this.ExecuteReader(sqlString, selectParms))
            {
                if (rdr.Read())
                {
                    exists = true;
                }
                rdr.Close();
            }

            return exists;
        }

		public bool IsExactExists(string innerLinkName, int publishmentSystemID)
		{
			bool exists = false;

            string sqlString = string.Format("SELECT InnerLinkName, PublishmentSystemID, LinkUrl FROM siteserver_InnerLink WHERE InnerLinkName = @InnerLinkName AND PublishmentSystemID = @PublishmentSystemID");
            IDbDataParameter[] selectParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_INNER_LINK_NAME, EDataType.NVarChar, 255, innerLinkName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)				 
			};
			using (IDataReader rdr = this.ExecuteReader(sqlString,selectParms)) 
			{
				if (rdr.Read()) 
				{					
					exists = true;
				}
				rdr.Close();
			}

			return exists;
		}

		public IEnumerable GetDataSource(int publishmentSystemID)
		{
			string sqlString = "SELECT InnerLinkName, PublishmentSystemID, LinkUrl FROM siteserver_InnerLink WHERE PublishmentSystemID = 0 ORDER BY PublishmentSystemID, InnerLinkName";
			if (publishmentSystemID != 0)
			{
				sqlString = string.Format("SELECT InnerLinkName, PublishmentSystemID, LinkUrl FROM siteserver_InnerLink WHERE  PublishmentSystemID = 0 OR PublishmentSystemID = {0} ORDER BY PublishmentSystemID, InnerLinkName", publishmentSystemID);
			}
			IEnumerable enumerable = (IEnumerable)this.ExecuteReader(sqlString);
			return enumerable;
		}

		public ArrayList GetInnerLinkInfoArrayList(int publishmentSystemID)
		{
			ArrayList arraylist = new ArrayList();
			string sqlString = "SELECT InnerLinkName, PublishmentSystemID, LinkUrl FROM siteserver_InnerLink WHERE PublishmentSystemID = 0 ORDER BY PublishmentSystemID, InnerLinkName";
			if (publishmentSystemID != 0)
			{
				sqlString = string.Format("SELECT InnerLinkName, PublishmentSystemID, LinkUrl FROM siteserver_InnerLink WHERE PublishmentSystemID = 0 OR PublishmentSystemID = {0} ORDER BY PublishmentSystemID, InnerLinkName", publishmentSystemID);
			}

			using (IDataReader rdr = this.ExecuteReader(sqlString)) 
			{
				while (rdr.Read()) 
				{
                    arraylist.Add(new InnerLinkInfo(rdr.GetValue(0).ToString(), rdr.GetInt32(1), rdr.GetValue(2).ToString()));
				}
				rdr.Close();
			}

			return arraylist;
		}

		public ArrayList GetInnerLinkNameArrayList(int publishmentSystemID)
		{
			ArrayList arraylist = new ArrayList();
			string sqlString = "SELECT InnerLinkName FROM siteserver_InnerLink WHERE PublishmentSystemID = 0";
			if (publishmentSystemID != 0)
			{
				sqlString = string.Format("SELECT InnerLinkName FROM siteserver_InnerLink WHERE PublishmentSystemID = 0 OR PublishmentSystemID = {0}", publishmentSystemID);
			}
			
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
	}
}
