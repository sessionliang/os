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
    public class PermissionsDAO : DataProviderBase, IPermissionsDAO
	{
        public void InsertRoleAndPermissions(string roleName, ArrayList modules, string creatorUserName, string description, ArrayList generalPermissionArrayList, ArrayList systemPermissionsInfoArrayList)
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

                        if (systemPermissionsInfoArrayList != null && systemPermissionsInfoArrayList.Count > 0)
						{
                            foreach (SystemPermissionsInfo systemPermissionsInfo in systemPermissionsInfoArrayList)
							{
                                systemPermissionsInfo.RoleName = roleName;
                                DataProvider.SystemPermissionsDAO.InsertWithTrans(systemPermissionsInfo, trans);
							}
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

        public void UpdatePublishmentPermissions(string roleName, ArrayList systemPermissionsInfoArrayList)
        {
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        DataProvider.SystemPermissionsDAO.DeleteWithTrans(roleName, trans);
                        if (systemPermissionsInfoArrayList != null && systemPermissionsInfoArrayList.Count > 0)
                        {
                            foreach (SystemPermissionsInfo systemPermissionsInfo in systemPermissionsInfoArrayList)
                            {
                                systemPermissionsInfo.RoleName = roleName;
                                DataProvider.SystemPermissionsDAO.InsertWithTrans(systemPermissionsInfo, trans);
                            }
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
        }

		public void DeleteRoleAndPermissions(string roleName)
		{
			using (IDbConnection conn = this.GetConnection()) 
			{
				conn.Open();
				using (IDbTransaction trans = conn.BeginTransaction()) 
				{
					try 
					{
                        DataProvider.SystemPermissionsDAO.DeleteWithTrans(roleName, trans);

                        BaiRongDataProvider.PermissionsInRolesDAO.DeleteWithTrans(roleName, trans);

						trans.Commit();
					}
					catch
					{
						trans.Rollback();
						throw;
					}
				}
			}

            RoleManager.DeleteRole(roleName);
        }
    }
}
