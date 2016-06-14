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
	public class NodeGroupDAO : DataProviderBase, INodeGroupDAO
	{
        private const string SQL_INSERT_NODEGROUP = "INSERT INTO siteserver_NodeGroup (NodeGroupName, PublishmentSystemID, Taxis, Description) VALUES (@NodeGroupName, @PublishmentSystemID, @Taxis, @Description)";
		private const string SQL_UPDATE_NODEGROUP = "UPDATE siteserver_NodeGroup SET Description = @Description WHERE NodeGroupName = @NodeGroupName AND PublishmentSystemID = @PublishmentSystemID";
		private const string SQL_DELETE_NODEGROUP = "DELETE FROM siteserver_NodeGroup WHERE NodeGroupName = @NodeGroupName AND PublishmentSystemID = @PublishmentSystemID";

		private const string PARM_GROUP_NAME = "@NodeGroupName";
		private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_TAXIS = "@Taxis";
		private const string PARM_DESCRIPTION = "@Description";


		public void Insert(NodeGroupInfo nodeGroup) 
		{
            int maxTaxis = this.GetMaxTaxis(nodeGroup.PublishmentSystemID);
            nodeGroup.Taxis = maxTaxis + 1;

			IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_GROUP_NAME, EDataType.NVarChar, 255, nodeGroup.NodeGroupName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, nodeGroup.PublishmentSystemID),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, nodeGroup.Taxis),
				this.GetParameter(PARM_DESCRIPTION, EDataType.NText, nodeGroup.Description)
			};

            this.ExecuteNonQuery(SQL_INSERT_NODEGROUP, insertParms);
		}

		public void Update(NodeGroupInfo nodeGroup) 
		{
			IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_DESCRIPTION, EDataType.NText, nodeGroup.Description),
				this.GetParameter(PARM_GROUP_NAME, EDataType.NVarChar, 255, nodeGroup.NodeGroupName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, nodeGroup.PublishmentSystemID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_NODEGROUP, updateParms);
		}

        public void Delete(int publishmentSystemID, string groupName)
		{
			IDbDataParameter[] nodeGroupParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_GROUP_NAME, EDataType.NVarChar, 255, groupName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

            this.ExecuteNonQuery(SQL_DELETE_NODEGROUP, nodeGroupParms);
		}

        public NodeGroupInfo GetNodeGroupInfo(int publishmentSystemID, string groupName)
		{
			NodeGroupInfo nodeGroup = null;

            string sqlString = "SELECT NodeGroupName, PublishmentSystemID, Taxis, Description FROM siteserver_NodeGroup WHERE NodeGroupName = @NodeGroupName AND PublishmentSystemID = @PublishmentSystemID";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_GROUP_NAME, EDataType.NVarChar, 255, groupName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};
			
			using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
			{
				if (rdr.Read()) 
				{
                    nodeGroup = new NodeGroupInfo(rdr.GetValue(0).ToString(), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString());
				}
				rdr.Close();
			}

			return nodeGroup;
		}

        public bool IsExists(int publishmentSystemID, string groupName)
		{
			bool exists = false;

            string sqlString = "SELECT NodeGroupName FROM siteserver_NodeGroup WHERE NodeGroupName = @NodeGroupName AND PublishmentSystemID = @PublishmentSystemID";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_GROUP_NAME, EDataType.NVarChar, 255, groupName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};
			
			using (IDataReader rdr = this.ExecuteReader(sqlString, parms)) 
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
            string sqlString = string.Format("SELECT NodeGroupName, PublishmentSystemID, Taxis, Description FROM siteserver_NodeGroup WHERE PublishmentSystemID = {0} ORDER BY Taxis DESC, NodeGroupName", publishmentSystemID);
			IEnumerable enumerable = (IEnumerable)this.ExecuteReader(sqlString);
			return enumerable;
		}

		public ArrayList GetNodeGroupInfoArrayList(int publishmentSystemID)
		{
			ArrayList arraylist = new ArrayList();
            string sqlString = string.Format("SELECT NodeGroupName, PublishmentSystemID, Taxis, Description FROM siteserver_NodeGroup WHERE PublishmentSystemID = {0} ORDER BY Taxis DESC, NodeGroupName", publishmentSystemID);

			using (IDataReader rdr = this.ExecuteReader(sqlString)) 
			{
				while (rdr.Read()) 
				{
                    arraylist.Add(new NodeGroupInfo(rdr.GetValue(0).ToString(), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString()));
				}
				rdr.Close();
			}

			return arraylist;
		}

		public ArrayList GetNodeGroupNameArrayList(int publishmentSystemID)
		{
			ArrayList arraylist = new ArrayList();
            string sqlString = string.Format("SELECT NodeGroupName FROM siteserver_NodeGroup WHERE PublishmentSystemID = {0} ORDER BY Taxis DESC, NodeGroupName", publishmentSystemID);
			
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

		//得到属于此发布系统和栏目组的所有栏目
		public ArrayList GetNodeInfoArrayListChecked(int publishmentSystemID, string nodeGroupName)
		{			
			string whereString = string.Format(" AND (siteserver_Node.NodeGroupNameCollection = '{0}' OR siteserver_Node.NodeGroupNameCollection LIKE '{0},%' OR siteserver_Node.NodeGroupNameCollection LIKE '%,{0},%' OR siteserver_Node.NodeGroupNameCollection LIKE '%,{0}')",PageUtils.FilterSql(nodeGroupName));
			return DataProvider.NodeDAO.GetNodeInfoArrayListByPublishmentSystemID(publishmentSystemID, whereString);
		}

        private int GetTaxis(int publishmentSystemID, string groupName)
        {
            string sqlString = string.Format("SELECT Taxis FROM siteserver_NodeGroup WHERE (NodeGroupName = @NodeGroupName AND PublishmentSystemID = @PublishmentSystemID)");
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_GROUP_NAME, EDataType.NVarChar, 255, groupName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString, parms);
        }

        private void SetTaxis(int publishmentSystemID, string groupName, int taxis)
        {
            string sqlString = string.Format("UPDATE siteserver_NodeGroup SET Taxis = {0} WHERE (NodeGroupName = @NodeGroupName AND PublishmentSystemID = @PublishmentSystemID)", taxis);
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_GROUP_NAME, EDataType.NVarChar, 255, groupName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};
            this.ExecuteNonQuery(sqlString, parms);
        }

        private int GetMaxTaxis(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM siteserver_NodeGroup WHERE (PublishmentSystemID = {0})", publishmentSystemID);
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

        public bool UpdateTaxisToUp(int publishmentSystemID, string groupName)
        {
            //Get Higher Taxis and ID
            string sqlString = string.Format("SELECT TOP 1 NodeGroupName, Taxis FROM siteserver_NodeGroup WHERE (Taxis > (SELECT Taxis FROM siteserver_NodeGroup WHERE NodeGroupName = @NodeGroupName AND PublishmentSystemID = @PublishmentSystemID) AND PublishmentSystemID = @PublishmentSystemID) ORDER BY Taxis");

            string higherGroupName = string.Empty;
            int higherTaxis = 0;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_GROUP_NAME, EDataType.NVarChar, 255, groupName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    higherGroupName = rdr.GetValue(0).ToString();
                    higherTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            if (!string.IsNullOrEmpty(higherGroupName))
            {
                //Get Taxis Of Selected ID
                int selectedTaxis = GetTaxis(publishmentSystemID, groupName);

                //Set The Selected Class Taxis To Higher Level
                SetTaxis(publishmentSystemID, groupName, higherTaxis);
                //Set The Higher Class Taxis To Lower Level
                SetTaxis(publishmentSystemID, higherGroupName, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToDown(int publishmentSystemID, string groupName)
        {
            //Get Lower Taxis and ID
            string sqlString = string.Format("SELECT TOP 1 NodeGroupName, Taxis FROM siteserver_NodeGroup WHERE (Taxis < (SELECT Taxis FROM siteserver_NodeGroup WHERE NodeGroupName = @NodeGroupName AND PublishmentSystemID = @PublishmentSystemID) AND PublishmentSystemID = @PublishmentSystemID) ORDER BY Taxis DESC");

            string lowerGroupName = string.Empty;
            int lowerTaxis = 0;
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_GROUP_NAME, EDataType.NVarChar, 255, groupName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};
            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    lowerGroupName = rdr.GetValue(0).ToString();
                    lowerTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            if (!string.IsNullOrEmpty(lowerGroupName))
            {
                //Get Taxis Of Selected Class
                int selectedTaxis = GetTaxis(publishmentSystemID, groupName);

                //Set The Selected Class Taxis To Lower Level
                SetTaxis(publishmentSystemID, groupName, lowerTaxis);
                //Set The Lower Class Taxis To Higher Level
                SetTaxis(publishmentSystemID, lowerGroupName, selectedTaxis);
                return true;
            }
            return false;
        }
	}
}
