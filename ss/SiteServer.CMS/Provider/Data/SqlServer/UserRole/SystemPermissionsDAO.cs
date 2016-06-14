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
    public class SystemPermissionsDAO : DataProviderBase, ISystemPermissionsDAO
    {
        private const string SQL_SELECT_ALL_BY_ROLE_NAME = "SELECT RoleName, PublishmentSystemID, NodeIDCollection, ChannelPermissions, WebsitePermissions FROM siteserver_SystemPermissions WHERE RoleName = @RoleName ORDER BY PublishmentSystemID DESC";

        private const string SQL_INSERT = "INSERT INTO siteserver_SystemPermissions (RoleName, PublishmentSystemID, NodeIDCollection, ChannelPermissions, WebsitePermissions) VALUES (@RoleName, @PublishmentSystemID, @NodeIDCollection, @ChannelPermissions, @WebsitePermissions)";
        private const string SQL_DELETE = "DELETE FROM siteserver_SystemPermissions WHERE RoleName = @RoleName";

        private const string PARM_ROLE_ROLE_NAME = "@RoleName";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_NODE_ID_COLLECTION = "@NodeIDCollection";
        private const string PARM_CHANNEL_PERMISSIONS = "@ChannelPermissions";
        private const string PARM_WEBSITE_PERMISSIONS = "@WebsitePermissions";


        public void InsertWithTrans(SystemPermissionsInfo info, IDbTransaction trans)
        {
            if (this.IsExists(info.RoleName, info.PublishmentSystemID, trans))
            {
                this.DeleteWithTrans(info.RoleName, info.PublishmentSystemID, trans);
            }

            IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ROLE_ROLE_NAME, EDataType.NVarChar, 255, info.RoleName),
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, info.PublishmentSystemID),
				this.GetParameter(PARM_NODE_ID_COLLECTION, EDataType.Text, info.NodeIDCollection),
				this.GetParameter(PARM_CHANNEL_PERMISSIONS, EDataType.Text, info.ChannelPermissions),
				this.GetParameter(PARM_WEBSITE_PERMISSIONS, EDataType.Text, info.WebsitePermissions)
			};

            this.ExecuteNonQuery(trans, SQL_INSERT, insertParms);
        }


        public void DeleteWithTrans(string roleName, IDbTransaction trans)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ROLE_ROLE_NAME, EDataType.NVarChar, 255, roleName)
			};

            this.ExecuteNonQuery(trans, SQL_DELETE, parms);
        }

        private void DeleteWithTrans(string roleName, int publishmentSystemID, IDbTransaction trans)
        {
            string sqlString = "DELETE FROM siteserver_SystemPermissions WHERE RoleName = @RoleName AND PublishmentSystemID = @PublishmentSystemID";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ROLE_ROLE_NAME, EDataType.NVarChar, 255, roleName),
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            this.ExecuteNonQuery(trans, sqlString, parms);
        }

        private bool IsExists(string roleName, int publishmentSystemID, IDbTransaction trans)
        {
            bool isExists = false;

            string sqlString = "SELECT RoleName FROM siteserver_SystemPermissions WHERE RoleName = @RoleName AND PublishmentSystemID = @PublishmentSystemID";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ROLE_ROLE_NAME, EDataType.NVarChar, 255, roleName),
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(trans, sqlString, parms))
            {
                if (rdr.Read())
                {
                    isExists = true;
                }
                rdr.Close();
            }

            return isExists;
        }

        public ArrayList GetSystemPermissionsInfoArrayList(string roleName)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ROLE_ROLE_NAME, EDataType.NVarChar, 255, roleName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_BY_ROLE_NAME, parms))
            {
                while (rdr.Read())
                {
                    SystemPermissionsInfo info = new SystemPermissionsInfo(rdr.GetValue(0).ToString(), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString());
                    arraylist.Add(info);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public SortedList GetWebsitePermissionSortedList(string[] roles)
        {
            SortedList sortedlist = new SortedList();
            ArrayList roleNameCollection = new ArrayList(roles);
            foreach (string roleName in roleNameCollection)
            {
                ArrayList systemPermissionsInfoArrayList = GetSystemPermissionsInfoArrayList(roleName);
                foreach (SystemPermissionsInfo systemPermissionsInfo in systemPermissionsInfoArrayList)
                {
                    ArrayList arraylist = new ArrayList();
                    ArrayList websitePermissionArrayList = TranslateUtils.StringCollectionToArrayList(systemPermissionsInfo.WebsitePermissions);
                    foreach (string websitePermission in websitePermissionArrayList)
                    {
                        if (!arraylist.Contains(websitePermission)) arraylist.Add(websitePermission);
                    }
                    sortedlist[systemPermissionsInfo.PublishmentSystemID] = arraylist;
                }
            }

            return sortedlist;
        }

        public SortedList GetChannelPermissionSortedList(string[] roles)
        {
            SortedList sortedlist = new SortedList();
            ArrayList roleNameCollection = new ArrayList(roles);


            //ArrayList allNodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayList();//所有存在的栏目ID
            foreach (string roleName in roleNameCollection)
            {
                ArrayList systemPermissionsInfoArrayList = GetSystemPermissionsInfoArrayList(roleName);
                foreach (SystemPermissionsInfo systemPermissionsInfo in systemPermissionsInfoArrayList)
                {
                    ArrayList nodeIDArrayList = TranslateUtils.StringCollectionToArrayList(systemPermissionsInfo.NodeIDCollection);
                    foreach (string nodeIDStr in nodeIDArrayList)
                    {
                        int nodeID = TranslateUtils.ToInt(nodeIDStr);
                        //if (!allNodeIDArrayList.Contains(nodeID))
                        //{
                        //    continue;//此角色包含的栏目已被删除
                        //}
                        ArrayList arraylist = new ArrayList();
                        if (sortedlist[nodeID] != null)
                        {
                            arraylist = (ArrayList)sortedlist[nodeID];
                        }

                        ArrayList channelPermissionArrayList = TranslateUtils.StringCollectionToArrayList(systemPermissionsInfo.ChannelPermissions);
                        foreach (string channelPermission in channelPermissionArrayList)
                        {
                            if (!arraylist.Contains(channelPermission)) arraylist.Add(channelPermission);
                        }
                        sortedlist[nodeID] = arraylist;

                    }
                }
            }

            return sortedlist;
        }

        public ArrayList GetChannelPermissionArrayListIgnoreNodeID(string[] roles)
        {
            ArrayList arraylist = new ArrayList();
            ArrayList roleNameCollection = new ArrayList(roles);

            foreach (string roleName in roleNameCollection)
            {
                ArrayList systemPermissionsInfoArrayList = GetSystemPermissionsInfoArrayList(roleName);
                foreach (SystemPermissionsInfo systemPermissionsInfo in systemPermissionsInfoArrayList)
                {
                    ArrayList channelPermissionArrayList = TranslateUtils.StringCollectionToArrayList(systemPermissionsInfo.ChannelPermissions);
                    foreach (string channelPermission in channelPermissionArrayList)
                    {
                        if (!arraylist.Contains(channelPermission)) arraylist.Add(channelPermission);
                    }
                }
            }

            return arraylist;
        }



        #region by 20151206 sofuny  站点权限角色
        public ArrayList GetSystemPermissionsInfoArrayListByPublishmentSystemID(int publishmentSystemID, string whereStr)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer,  publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(string.Format("SELECT RoleName, PublishmentSystemID, NodeIDCollection, ChannelPermissions, WebsitePermissions FROM siteserver_SystemPermissions WHERE PublishmentSystemID = @PublishmentSystemID {0} ", whereStr), parms))
            {
                while (rdr.Read())
                {
                    SystemPermissionsInfo info = new SystemPermissionsInfo(rdr.GetValue(0).ToString(), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString());
                    arraylist.Add(info);
                }
                rdr.Close();
            }

            return arraylist;
        }


        private const string SQL_SELECT_ALL_BY_RP = "SELECT RoleName, PublishmentSystemID, NodeIDCollection, ChannelPermissions, WebsitePermissions FROM siteserver_SystemPermissions WHERE RoleName = @RoleName AND PublishmentSystemID=@PublishmentSystemID ORDER BY PublishmentSystemID DESC";
        /// <summary>
        /// 根据角色名和站点ID获取角色权限信息
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="publishmentSystemID"></param>
        /// <returns></returns>
        public SystemPermissionsInfo GetSystemPermissionsInfoByRP(string roleName, int publishmentSystemID)
        {
            SystemPermissionsInfo info = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ROLE_ROLE_NAME, EDataType.NVarChar, 255, roleName),
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_BY_RP, parms))
            {
                if (rdr.Read())
                {
                    info = new SystemPermissionsInfo(rdr.GetValue(0).ToString(), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString());
                }
                rdr.Close();
            }

            return info;
        }

        /// <summary>
        /// 根据角色名和站点ID获取角色的站点权限信息
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="publishmentSystemID"></param>
        /// <returns></returns>
        public ArrayList GetWebsitePermissionListByRP(string roleName, int publishmentSystemID)
        {
            SystemPermissionsInfo systemPermissionsInfo = GetSystemPermissionsInfoByRP(roleName, publishmentSystemID);

            ArrayList websitePermissionArrayList = TranslateUtils.StringCollectionToArrayList(systemPermissionsInfo.WebsitePermissions);

            return websitePermissionArrayList;
        }

        string SQL_UPDATE = "update siteserver_SystemPermissions set  NodeIDCollection=@NodeIDCollection, ChannelPermissions=@ChannelPermissions, WebsitePermissions=@WebsitePermissions where RoleName =@RoleName and PublishmentSystemID = @PublishmentSystemID";

        public void Update(SystemPermissionsInfo info)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ROLE_ROLE_NAME, EDataType.NVarChar, 255, info.RoleName),
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, info.PublishmentSystemID),
				this.GetParameter(PARM_NODE_ID_COLLECTION, EDataType.Text, info.NodeIDCollection),
				this.GetParameter(PARM_CHANNEL_PERMISSIONS, EDataType.Text, info.ChannelPermissions),
				this.GetParameter(PARM_WEBSITE_PERMISSIONS, EDataType.Text, info.WebsitePermissions)
			};

            this.ExecuteNonQuery(SQL_UPDATE, updateParms);
        }
        #endregion

        #region 投稿管理


        public ArrayList GetAllPermissionArrayList(string[] roles, int publishmentSystemID, bool iscc)
        {
            ArrayList permissionArrayList = new ArrayList();
            ArrayList roleNameCollection = new ArrayList(roles);
            foreach (string roleName in roleNameCollection)
            {
                //string roleNames = roleName + ",";
                //string[] roless = roleNames.Split(',');

                //if (!EPredefinedRoleUtils.IsAdministrator(roless) && !EPredefinedRoleUtils.IsConsoleAdministrator(roless) && !EPredefinedRoleUtils.IsSystemAdministrator(roless))
                //{
                    ArrayList systemPermissionsInfoArrayList = GetSystemPermissionsInfoArrayList(roleName);
                    foreach (SystemPermissionsInfo systemPermissionsInfo in systemPermissionsInfoArrayList)
                    {
                        if (publishmentSystemID != 0)
                        {
                            if (iscc)
                            {
                                if (systemPermissionsInfo.PublishmentSystemID == publishmentSystemID && !string.IsNullOrEmpty(systemPermissionsInfo.NodeIDCollection))
                                    permissionArrayList.Add(systemPermissionsInfo);
                            }
                            else
                            {
                                if (systemPermissionsInfo.PublishmentSystemID == publishmentSystemID)
                                    permissionArrayList.Add(systemPermissionsInfo);
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(systemPermissionsInfo.NodeIDCollection))
                                permissionArrayList.Add(systemPermissionsInfo);
                            else
                                permissionArrayList.Add(systemPermissionsInfo);
                        }
                    }
                }
            //}

            return permissionArrayList;
        }


        #endregion
    }
}
