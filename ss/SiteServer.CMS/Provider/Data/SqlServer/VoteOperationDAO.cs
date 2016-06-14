using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Data;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class VoteOperationDAO : DataProviderBase, IVoteOperationDAO
	{
        private const string SQL_SELECT_COUNT = "SELECT Count(OperationID) AS TotalNum FROM siteserver_VoteOperation WHERE PublishmentSystemID = @PublishmentSystemID AND NodeID = @NodeID AND ContentID = @ContentID";

        private const string SQL_SELECT_ALL = "SELECT OperationID, PublishmentSystemID, NodeID, ContentID, IPAddress, UserName, AddDate FROM siteserver_VoteOperation WHERE PublishmentSystemID = @PublishmentSystemID AND NodeID = @NodeID AND ContentID = @ContentID ORDER BY AddDate DESC";

        private const string SQL_SELECT_BY_USER_NAME = "SELECT OperationID FROM siteserver_VoteOperation WHERE PublishmentSystemID = @PublishmentSystemID AND NodeID = @NodeID AND ContentID = @ContentID AND UserName = @UserName";

        private const string SQL_SELECT_BY_IPADDRESS = "SELECT OperationID FROM siteserver_VoteOperation WHERE PublishmentSystemID = @PublishmentSystemID AND NodeID = @NodeID AND ContentID = @ContentID AND IPAddress = @IPAddress";

        private const string PARM_OPERATION_ID = "@OperationID";
        private const string PARM_PUBLISHMENTSYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_NODE_ID = "@NodeID";
        private const string PARM_CONTENT_ID = "@ContentID";
		private const string PARM_IP_ADDRESS = "@IPAddress";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_ADD_DATE = "@AddDate";

		public void Insert(VoteOperationInfo operationInfo)
		{
            string sqlString = "INSERT INTO siteserver_VoteOperation (PublishmentSystemID, NodeID, ContentID, IPAddress, UserName, AddDate) VALUES (@PublishmentSystemID, @NodeID, @ContentID, @IPAddress, @UserName, @AddDate)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO siteserver_VoteOperation (OperationID, PublishmentSystemID, NodeID, ContentID, IPAddress, UserName, AddDate) VALUES (siteserver_VoteOperation_SEQ.NEXTVAL, @PublishmentSystemID, @NodeID, @ContentID, @IPAddress, @UserName, @AddDate)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEM_ID, EDataType.Integer, operationInfo.PublishmentSystemID),
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, operationInfo.NodeID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, operationInfo.ContentID),
				this.GetParameter(PARM_IP_ADDRESS, EDataType.VarChar, 50, operationInfo.IPAddress),
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, operationInfo.UserName),
				this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, operationInfo.AddDate)
			};

            this.ExecuteNonQuery(sqlString, parms);
		}

		public int GetCount(int publishmentSystemID, int nodeID, int contentID)
		{
			int count = 0;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, contentID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_COUNT, parms))
			{
				if (rdr.Read())
				{
					if (!rdr.IsDBNull(0))
					{
                        count = Convert.ToInt32(rdr[0]);
					}
				}
				rdr.Close();
			}
            return count;
		}

        public bool IsUserExists(int publishmentSystemID, int nodeID, int contentID, string userName)
        {
            bool isExists = false;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, contentID),
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_USER_NAME, parms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        isExists = true;
                    }
                }
                rdr.Close();
            }
            return isExists;
        }

        public bool IsIPAddressExists(int publishmentSystemID, int nodeID, int contentID, string ipAddress)
        {
            bool isExists = false;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, contentID),
                this.GetParameter(PARM_IP_ADDRESS, EDataType.VarChar, 50, ipAddress)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_IPADDRESS, parms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        isExists = true;
                    }
                }
                rdr.Close();
            }
            return isExists;
        }

        public DataSet GetDataSet(int publishmentSystemID, int nodeID, int contentID)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, contentID)
			};

			DataSet dataset = this.ExecuteDataset(SQL_SELECT_ALL, parms);
			return dataset;
		}

        public string GetCookieName(int publishmentSystemID, int nodeID, int contentID)
        {
            return string.Format("SiteServer_CMS_Vote_{0}_{1}_{2}", publishmentSystemID, nodeID, contentID);
        }
	}
}
