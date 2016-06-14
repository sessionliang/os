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
    public class GovPublicChannelDAO : DataProviderBase, IGovPublicChannelDAO
	{
        private const string SQL_INSERT = "INSERT INTO siteserver_GovPublicChannel (NodeID, PublishmentSystemID, Code, Summary) VALUES (@NodeID, @PublishmentSystemID, @Code, @Summary)";

        private const string SQL_DELETE = "DELETE FROM siteserver_GovPublicChannel WHERE NodeID = @NodeID";

        private const string SQL_SELECT = "SELECT NodeID, PublishmentSystemID, Code, Summary FROM siteserver_GovPublicChannel WHERE NodeID = @NodeID";

        private const string SQL_SELECT_CODE = "SELECT Code FROM siteserver_GovPublicChannel WHERE NodeID = @NodeID";

        private const string SQL_SELECT_ID = "SELECT NodeID FROM siteserver_GovPublicChannel WHERE NodeID = @NodeID";

        private const string SQL_UPDATE = "UPDATE siteserver_GovPublicChannel SET Code = @Code, Summary = @Summary WHERE NodeID = @NodeID";

        private const string PARM_NODE_ID = "@NodeID";
        private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_CODE = "@Code";
        private const string PARM_SUMMARY = "@Summary";

        public void Insert(GovPublicChannelInfo channelInfo)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, channelInfo.NodeID),
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, channelInfo.PublishmentSystemID),
                this.GetParameter(PARM_CODE, EDataType.VarChar, 50, channelInfo.Code),
				this.GetParameter(PARM_SUMMARY, EDataType.NVarChar, 255, channelInfo.Summary)
			};

            this.ExecuteNonQuery(SQL_INSERT, parms);
        }

        public void Update(GovPublicChannelInfo channelInfo)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_CODE, EDataType.VarChar, 50, channelInfo.Code),
				this.GetParameter(PARM_SUMMARY, EDataType.NVarChar, 255, channelInfo.Summary),
				this.GetParameter(PARM_NODE_ID, EDataType.Integer, channelInfo.NodeID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(int nodeID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID)
			};

            this.ExecuteNonQuery(SQL_DELETE, parms);
        }

        public GovPublicChannelInfo GetChannelInfo(int nodeID)
		{
            GovPublicChannelInfo channelInfo = null;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms)) 
			{
				if (rdr.Read()) 
				{
                    channelInfo = new GovPublicChannelInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString());
				}
				rdr.Close();
			}
            return channelInfo;
		}

        public string GetCode(int nodeID)
        {
            string code = string.Empty;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_CODE, parms))
            {
                if (rdr.Read())
                {
                    code = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }
            return code;
        }

		public bool IsExists(int nodeID)
		{
			bool exists = false;

			IDbDataParameter[] nodeParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID)
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
	}
}
