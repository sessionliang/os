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
    public class GovInteractPermissionsDAO : DataProviderBase, IGovInteractPermissionsDAO
	{
        private const string SQL_SELECT = "SELECT UserName, NodeID, Permissions FROM siteserver_GovInteractPermissions WHERE UserName = @UserName AND NodeID = @NodeID";
        private const string SQL_SELECT_ALL = "SELECT UserName, NodeID, Permissions FROM siteserver_GovInteractPermissions WHERE UserName = @UserName";
        private const string SQL_INSERT = "INSERT INTO siteserver_GovInteractPermissions (UserName, NodeID, Permissions) VALUES (@UserName, @NodeID, @Permissions)";
        private const string SQL_UPDATE = "UPDATE siteserver_GovInteractPermissions SET Permissions = @Permissions WHERE UserName = @UserName AND NodeID = @NodeID";
        private const string SQL_DELETE = "DELETE FROM siteserver_GovInteractPermissions WHERE UserName = @UserName AND NodeID = @NodeID";

        private const string PARM_USERNAME = "@UserName";
        private const string PARM_NODE_ID = "@NodeID";
        private const string PARM_PERMISSIONS = "@Permissions";

		public void Insert(int publishmentSystemID, GovInteractPermissionsInfo permissionsInfo) 
		{
            if (!DataProvider.GovInteractChannelDAO.IsExists(permissionsInfo.NodeID))
            {
                GovInteractChannelInfo channelInfo = new GovInteractChannelInfo(permissionsInfo.NodeID, publishmentSystemID, 0, 0, string.Empty, string.Empty);
                DataProvider.GovInteractChannelDAO.Insert(channelInfo);
            }
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 50, permissionsInfo.UserName),
				this.GetParameter(PARM_NODE_ID, EDataType.Integer, permissionsInfo.NodeID),
                this.GetParameter(PARM_PERMISSIONS, EDataType.Text, permissionsInfo.Permissions)
			};

            this.ExecuteNonQuery(SQL_INSERT, parms);
		}

		public void Delete(string userName, int nodeID)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 50, userName),
				this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID)
			};

			this.ExecuteNonQuery(SQL_DELETE, parms);
		}

        public void Update(GovInteractPermissionsInfo permissionsInfo)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PERMISSIONS, EDataType.Text, permissionsInfo.Permissions),
                this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 50, permissionsInfo.UserName),
				this.GetParameter(PARM_NODE_ID, EDataType.Integer, permissionsInfo.NodeID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public GovInteractPermissionsInfo GetPermissionsInfo(string userName, int nodeID)
		{
            GovInteractPermissionsInfo permissionsInfo = null;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 50, userName),
				this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
			{
				if (rdr.Read()) 
				{
                    permissionsInfo = new GovInteractPermissionsInfo(rdr.GetValue(0).ToString(), rdr.GetInt32(1), rdr.GetValue(2).ToString());
				}
				rdr.Close();
			}

            return permissionsInfo;
		}

        public ArrayList GetPermissionsInfoArrayList(string userName)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 50, userName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL, parms))
            {
                while (rdr.Read())
                {
                    GovInteractPermissionsInfo permissionsInfo = new GovInteractPermissionsInfo(rdr.GetValue(0).ToString(), rdr.GetInt32(1), rdr.GetValue(2).ToString());
                    arraylist.Add(permissionsInfo);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public SortedList GetPermissionSortedList(string userName)
        {
            SortedList sortedlist = new SortedList();

            ArrayList permissionsInfoArrayList = GetPermissionsInfoArrayList(userName);
            foreach (GovInteractPermissionsInfo permissionsInfo in permissionsInfoArrayList)
            {
                ArrayList arraylist = new ArrayList();
                if (sortedlist[permissionsInfo.NodeID] != null)
                {
                    arraylist = (ArrayList)sortedlist[permissionsInfo.NodeID];
                }

                ArrayList permissionArrayList = TranslateUtils.StringCollectionToArrayList(permissionsInfo.Permissions);
                foreach (string permission in permissionArrayList)
                {
                    if (!arraylist.Contains(permission)) arraylist.Add(permission);
                }
                sortedlist[permissionsInfo.NodeID] = arraylist;
            }

            return sortedlist;
        }
	}
}
