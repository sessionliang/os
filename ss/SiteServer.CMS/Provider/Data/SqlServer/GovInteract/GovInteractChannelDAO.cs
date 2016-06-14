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
    public class GovInteractChannelDAO : DataProviderBase, IGovInteractChannelDAO
    {
        private const string SQL_INSERT = "INSERT INTO siteserver_GovInteractChannel (NodeID, PublishmentSystemID, ApplyStyleID, QueryStyleID, DepartmentIDCollection, Summary) VALUES (@NodeID, @PublishmentSystemID, @ApplyStyleID, @QueryStyleID, @DepartmentIDCollection, @Summary)";

        private const string SQL_DELETE = "DELETE FROM siteserver_GovInteractChannel WHERE NodeID = @NodeID";

        private const string SQL_SELECT = "SELECT NodeID, PublishmentSystemID, ApplyStyleID, QueryStyleID, DepartmentIDCollection, Summary FROM siteserver_GovInteractChannel WHERE NodeID = @NodeID";

        private const string SQL_SELECT_ID = "SELECT NodeID FROM siteserver_GovInteractChannel WHERE NodeID = @NodeID";

        private const string SQL_UPDATE = "UPDATE siteserver_GovInteractChannel SET DepartmentIDCollection = @DepartmentIDCollection, Summary = @Summary WHERE NodeID = @NodeID";

        private const string PARM_NODE_ID = "@NodeID";
        private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_APPLY_STYLE_ID = "@ApplyStyleID";
        private const string PARM_QUERY_STYLE_ID = "@QueryStyleID";
        private const string PARM_DEPARTMENTID_COLLECTION = "@DepartmentIDCollection";
        private const string PARM_SUMMARY = "@Summary";

        public void Insert(GovInteractChannelInfo channelInfo)
        {
            channelInfo.ApplyStyleID = DataProvider.TagStyleDAO.Insert(new TagStyleInfo(0, channelInfo.NodeID.ToString(), Constants.STLElementName.StlGovInteractApply, channelInfo.PublishmentSystemID, false, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty));
            channelInfo.QueryStyleID = DataProvider.TagStyleDAO.Insert(new TagStyleInfo(0, channelInfo.NodeID.ToString(), Constants.STLElementName.StlGovInteractQuery, channelInfo.PublishmentSystemID, false, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty));

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, channelInfo.NodeID),
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, channelInfo.PublishmentSystemID),
                this.GetParameter(PARM_APPLY_STYLE_ID, EDataType.Integer, channelInfo.ApplyStyleID),
                this.GetParameter(PARM_QUERY_STYLE_ID, EDataType.Integer, channelInfo.QueryStyleID),
                this.GetParameter(PARM_DEPARTMENTID_COLLECTION, EDataType.NVarChar, 255, channelInfo.DepartmentIDCollection),
				this.GetParameter(PARM_SUMMARY, EDataType.NVarChar, 255, channelInfo.Summary)
			};

            this.ExecuteNonQuery(SQL_INSERT, parms);

            GovInteractManager.AddDefaultTypeInfos(channelInfo.PublishmentSystemID, channelInfo.NodeID);
        }

        public void Update(GovInteractChannelInfo channelInfo)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_DEPARTMENTID_COLLECTION, EDataType.NVarChar, 255, channelInfo.DepartmentIDCollection),
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

        public GovInteractChannelInfo GetChannelInfo(int publishmentSystemID, int nodeID)
        {
            GovInteractChannelInfo channelInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    channelInfo = new GovInteractChannelInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString());
                }
                rdr.Close();
            }

            if (channelInfo == null)
            {
                GovInteractChannelInfo theChannelInfo = new GovInteractChannelInfo(nodeID, publishmentSystemID, 0, 0, string.Empty, string.Empty);
                DataProvider.GovInteractChannelDAO.Insert(theChannelInfo);

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
                {
                    if (rdr.Read())
                    {
                        channelInfo = new GovInteractChannelInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString());
                    }
                    rdr.Close();
                }
            }

            return channelInfo;
        }

        public string GetSummary(int nodeID)
        {
            string sqlString = string.Format("SELECT Summary FROM siteserver_GovInteractChannel WHERE NodeID = {0}", nodeID);
            return BaiRongDataProvider.DatabaseDAO.GetString(sqlString);
        }

        public int GetApplyStyleID(int publishmentSystemID, int nodeID)
        {
            string sqlString = string.Format("SELECT ApplyStyleID FROM siteserver_GovInteractChannel WHERE NodeID = {0}", nodeID);
            int styleID = BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);

            if (styleID == 0)
            {
                GovInteractChannelInfo theChannelInfo = new GovInteractChannelInfo(nodeID, publishmentSystemID, 0, 0, string.Empty, string.Empty);
                DataProvider.GovInteractChannelDAO.Insert(theChannelInfo);

                styleID = BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
            }

            return styleID;
        }

        public int GetQueryStyleID(int publishmentSystemID, int nodeID)
        {
            string sqlString = string.Format("SELECT QueryStyleID FROM siteserver_GovInteractChannel WHERE NodeID = {0}", nodeID);
            int styleID = BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
            if (styleID == 0)
            {
                GovInteractChannelInfo theChannelInfo = new GovInteractChannelInfo(nodeID, publishmentSystemID, 0, 0, string.Empty, string.Empty);
                DataProvider.GovInteractChannelDAO.Insert(theChannelInfo);

                styleID = BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
            }
            return styleID;
        }

        public int GetNodeIDByInteractName(int publishmentSystemID, string interactName)
        {
            string sqlString = string.Format("SELECT siteserver_GovInteractChannel.NodeID FROM siteserver_GovInteractChannel INNER JOIN siteserver_Node ON siteserver_GovInteractChannel.NodeID = siteserver_Node.NodeID and siteserver_Node.PublishmentSystemID = {0} AND siteserver_Node.ContentModelID = '{1}' AND siteserver_Node.NodeName = @NodeName", publishmentSystemID, EContentModelTypeUtils.GetValue(EContentModelType.GovInteract));

            IDbDataParameter[] selectParms = new IDbDataParameter[]
			{
				this.GetParameter("@NodeName", EDataType.NVarChar,255, interactName)
			};

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString, selectParms);
        }

        public int GetNodeIDByApplyStyleID(int applyStyleID)
        {
            string sqlString = string.Format("SELECT NodeID FROM siteserver_GovInteractChannel WHERE ApplyStyleID = {0}", applyStyleID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetNodeIDByQueryStyleID(int queryStyleID)
        {
            string sqlString = string.Format("SELECT NodeID FROM siteserver_GovInteractChannel WHERE QueryStyleID = {0}", queryStyleID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
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
