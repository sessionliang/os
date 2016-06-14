using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;


namespace BaiRong.Provider.Data.SqlServer
{
    public class PermissionsInRolesDAO : DataProviderBase, IPermissionsInRolesDAO
	{
		private const string SQL_SELECT = "SELECT RoleName, GeneralPermissions FROM bairong_PermissionsInRoles WHERE RoleName = @RoleName";

		private const string SQL_INSERT = "INSERT INTO bairong_PermissionsInRoles (RoleName, GeneralPermissions) VALUES (@RoleName, @GeneralPermissions)";
		private const string SQL_DELETE = "DELETE FROM bairong_PermissionsInRoles WHERE RoleName = @RoleName";

		private const string PARM_ROLE_ROLE_NAME = "@RoleName";
		private const string PARM_GENERAL_PERMISSIONS = "@GeneralPermissions";

        public void InsertRoleAndPermissions(string roleName, ArrayList modules, string creatorUserName, string description, ArrayList generalPermissionArrayList)
        {
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (generalPermissionArrayList != null && generalPermissionArrayList.Count > 0)
                        {
                            PermissionsInRolesInfo permissionsInRolesInfo = new PermissionsInRolesInfo(roleName, TranslateUtils.ObjectCollectionToString(generalPermissionArrayList));
                            BaiRongDataProvider.PermissionsInRolesDAO.InsertWithTrans(permissionsInRolesInfo, trans);
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
            RoleManager.InsertRole(roleName, modules, creatorUserName, description);
        }

		public void InsertWithTrans(PermissionsInRolesInfo info, IDbTransaction trans) 
		{
			IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ROLE_ROLE_NAME, EDataType.NVarChar, 255, info.RoleName),
				this.GetParameter(PARM_GENERAL_PERMISSIONS, EDataType.Text, info.GeneralPermissions)
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

        public void Delete(string roleName)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ROLE_ROLE_NAME, EDataType.NVarChar, 255, roleName)
			};

            this.ExecuteNonQuery(SQL_DELETE, parms);
        }

        public void UpdateRoleAndGeneralPermissions(string roleName, ArrayList modules, string description, ArrayList generalPermissionArrayList)
        {
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        BaiRongDataProvider.PermissionsInRolesDAO.DeleteWithTrans(roleName, trans);
                        if (generalPermissionArrayList != null && generalPermissionArrayList.Count > 0)
                        {
                            PermissionsInRolesInfo permissionsInRolesInfo = new PermissionsInRolesInfo(roleName, TranslateUtils.ObjectCollectionToString(generalPermissionArrayList));
                            BaiRongDataProvider.PermissionsInRolesDAO.InsertWithTrans(permissionsInRolesInfo, trans);
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
            RoleManager.UpdateRole(roleName, modules, description);
        }

		private PermissionsInRolesInfo GetPermissionsInRolesInfo(string roleName)
		{
            PermissionsInRolesInfo info = null;
			
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ROLE_ROLE_NAME, EDataType.NVarChar, 255, roleName)
			};
			
			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms)) 
			{
				if (rdr.Read()) 
				{
                    info = new PermissionsInRolesInfo(rdr.GetValue(0).ToString(), rdr.GetValue(1).ToString()); 					
				}
				rdr.Close();
			}
			return info;
		}


		public ArrayList GetGeneralPermissionArrayList(string[] roles)
		{
			ArrayList arraylist = new ArrayList();
			ArrayList roleNameCollection = new ArrayList(roles);
			foreach (string roleName in roleNameCollection)
			{
                PermissionsInRolesInfo permissionsInRolesInfo = this.GetPermissionsInRolesInfo(roleName);
                if (permissionsInRolesInfo != null)
				{
                    ArrayList permissionArrayList = TranslateUtils.StringCollectionToArrayList(permissionsInRolesInfo.GeneralPermissions);
                    foreach (string permission in permissionArrayList)
					{
                        if (!arraylist.Contains(permission)) arraylist.Add(permission);
					}
				}
			}

			return arraylist;
		}
	}
}
