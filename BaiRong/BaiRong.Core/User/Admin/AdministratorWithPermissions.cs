using System.Collections;
using System.Web;
using System.Web.Caching;
using BaiRong.Core;
using BaiRong.Core.Configuration;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;

namespace BaiRong.Core
{
    public class AdministratorWithPermissions
    {
        protected string userName;
        protected bool isAnonymous = true;
        protected string[] roles = null;
        protected ArrayList permissionArrayList = null;
        protected string rolesKey;
        protected string permissionArrayListKey;

        public AdministratorWithPermissions(string userName, bool isAnonymous)
        {
            this.userName = userName;
            this.isAnonymous = isAnonymous;

            this.rolesKey = PermissionsManager.GetRolesKey(userName);
            this.permissionArrayListKey = PermissionsManager.GetPermissionArrayListKey(userName);
        }

        public bool IsAnonymous
        {
            get
            {
                return this.isAnonymous;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                if (this.isAnonymous)
                {
                    return false;
                }
                return HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }

        public bool IsConsoleAdministrator
        {
            get
            {
                return EPredefinedRoleUtils.IsConsoleAdministrator(this.Roles);
            }
        }

        public bool IsSystemAdministrator
        {
            get
            {
                return EPredefinedRoleUtils.IsSystemAdministrator(this.Roles);
            }
        }

        public bool IsAdministrator
        {
            get
            {
                return EPredefinedRoleUtils.IsAdministrator(this.Roles);
            }
        }

        public ArrayList PermissionArrayList
        {
            get
            {
                if (this.permissionArrayList == null)
                {
                    if (!string.IsNullOrEmpty(userName) && !string.Equals(userName, AdminManager.AnonymousUserName))
                    {
                        if (CacheUtils.Get(permissionArrayListKey) != null)
                        {
                            this.permissionArrayList = (ArrayList)CacheUtils.Get(permissionArrayListKey);
                        }
                        else
                        {
                            if (EPredefinedRoleUtils.IsConsoleAdministrator(this.Roles))
                            {
                                //this.generalPermissionArrayList = new ArrayList();
                                //ArrayList moduleIDArrayList = ProductManager.Instance.GetProductIDList();
                                //foreach (string moduleID in moduleIDArrayList)
                                //{
                                //    if (StringUtils.EqualsIgnoreCase(moduleID, ProductManager.CMS.ModuleID))
                                //    {
                                //        foreach (PermissionConfig permission in PermissionConfigManager.Instance.GeneralPermissions)
                                //        {
                                //            this.generalPermissionArrayList.Add(permission.Name);
                                //        }
                                //    }
                                //    else
                                //    {
                                //        foreach (PermissionConfig permission in PermissionConfigManager.GetModulePermissions(moduleID))
                                //        {
                                //            this.generalPermissionArrayList.Add(permission.Name);
                                //        }
                                //    }
                                //}

                                this.permissionArrayList = new ArrayList();
                                foreach (PermissionConfig permission in PermissionConfigManager.Instance.GeneralPermissions)
                                {
                                    this.permissionArrayList.Add(permission.Name);
                                }

                                #region 用户中心的权限
                                foreach (PermissionConfig permission in PermissionConfigManager.Instance.WebsitePermissions)
                                {
                                    if (permission.Name.StartsWith("usercenter_"))
                                        this.permissionArrayList.Add(permission.Name);
                                } 
                                #endregion
                            }
                            else if (EPredefinedRoleUtils.IsSystemAdministrator(this.Roles))
                            {
                                this.permissionArrayList = new ArrayList();
                                this.permissionArrayList.Add(AppManager.Platform.Permission.Platform_Administrator);
                            }
                            else
                            {
                                this.permissionArrayList = BaiRongDataProvider.PermissionsInRolesDAO.GetGeneralPermissionArrayList(this.Roles);
                            }

                            CacheUtils.Insert(permissionArrayListKey, this.permissionArrayList, 30 * CacheUtils.MinuteFactor, CacheItemPriority.Normal);
                        }
                    }
                }
                if (this.permissionArrayList == null)
                {
                    this.permissionArrayList = new ArrayList();
                }
                return permissionArrayList;
            }
        }

        public string[] Roles
        {
            get
            {
                if (this.roles == null)
                {
                    if (!string.IsNullOrEmpty(userName) && !string.Equals(userName, AdminManager.AnonymousUserName))
                    {
                        if (CacheUtils.Get(rolesKey) != null)
                        {
                            this.roles = (string[])CacheUtils.Get(rolesKey);
                        }
                        else
                        {
                            this.roles = RoleManager.GetRolesForUser(userName);
                            CacheUtils.Insert(rolesKey, this.roles, 30 * CacheUtils.MinuteFactor, CacheItemPriority.Normal);
                        }
                    }
                }
                if (roles != null && roles.Length > 0)
                {
                    return roles;
                }
                else
                {
                    return new string[] { EPredefinedRoleUtils.GetValue(EPredefinedRole.Administrator) };
                }
            }
        }

        public bool IsInRole(string role)
        {
            foreach (string r in this.Roles)
            {
                if (role == r) return true;
            }
            return false;
        }

        public static AdministratorWithPermissions GetAnonymousUserWithPermissions()
        {
            AdministratorWithPermissions userWithPermissions = new AdministratorWithPermissions(AdminManager.AnonymousUserName, true);
            return userWithPermissions;
        }
    }
}
