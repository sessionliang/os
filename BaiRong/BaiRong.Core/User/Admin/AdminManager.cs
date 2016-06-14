using System.Collections;
using System.Text;
using BaiRong.Core;
using BaiRong.Model;
using System;
using BaiRong.Core.ActiveDirectory;
using BaiRong.Core.Data.Provider;

namespace BaiRong.Core
{
    public class AdminManager
    {
        private AdminManager()
        {
        }

        public static AdministratorInfo Current
        {
            get
            {
                AdministratorInfo adminInfo = null;
                try
                {
                    adminInfo = GetAdminInfo(BaiRongDataProvider.AdministratorDAO.UserName);
                }
                catch { }
                if (adminInfo == null)
                {
                    adminInfo = new AdministratorInfo();
                }
                return adminInfo;
            }
        }

        public static AdministratorInfo GetAdminInfo(string userName)
        {
            return GetAdminInfo(userName, false);
        }

        public static AdministratorInfo GetAdminInfo(string userName, bool flush)
        {
            Hashtable ht = GetActiveAdminInfo();

            AdministratorInfo adminInfo = null;

            if (!flush)
            {
                adminInfo = ht[userName] as AdministratorInfo;
            }

            if (adminInfo == null)
            {
                adminInfo = BaiRongDataProvider.AdministratorDAO.GetAdministratorInfo(userName);

                if (adminInfo != null)
                {
                    UpdateAdminInfoCache(ht, adminInfo, userName);
                }
            }

            return adminInfo;
        }

        public static string GetDisplayName(string userName, bool isDepartment)
        {
            AdministratorInfo adminInfo = AdminManager.GetAdminInfo(userName);
            if (adminInfo != null)
            {
                if (isDepartment)
                {
                    string departmentName = DepartmentManager.GetDepartmentName(adminInfo.DepartmentID);
                    if (!string.IsNullOrEmpty(departmentName))
                    {
                        return string.Format("{0}({1})", adminInfo.DisplayName, departmentName);
                    }
                    else
                    {
                        return adminInfo.DisplayName;
                    }
                }
                else
                {
                    return adminInfo.DisplayName;
                }
            }
            return userName;
        }

        public static string GetFullName(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                AdministratorInfo adminInfo = AdminManager.GetAdminInfo(userName);
                if (adminInfo != null)
                {
                    string retval = string.Format("账号：{0}<br />姓名：{1}", userName, adminInfo.DisplayName);
                    string departmentName = DepartmentManager.GetDepartmentName(adminInfo.DepartmentID);
                    if (!string.IsNullOrEmpty(departmentName))
                    {
                        retval += string.Format("<br />部门：{0}", departmentName);
                    }
                    return retval;
                }
                return userName;
            }
            return string.Empty;
        }

        public static int GetDepartmentID(string userName)
        {
            AdministratorInfo adminInfo = AdminManager.GetAdminInfo(userName);
            if (adminInfo != null)
            {
                return adminInfo.DepartmentID;
            }
            return 0;
        }

        public static string CurrrentDepartmentName
        {
            get
            {
                if (AdminManager.Current.DepartmentID > 0)
                {
                    return DepartmentManager.GetDepartmentName(AdminManager.Current.DepartmentID);
                }
                return string.Empty;
            }
        }

        public static string DisplayName
        {
            get { return (!string.IsNullOrEmpty(AdminManager.Current.DisplayName)) ? AdminManager.Current.DisplayName : AdminManager.Current.UserName; }
        }

        public static string Theme
        {
            get { return AdminManager.Current.Theme; }
        }

        private static void UpdateAdminInfoCache(Hashtable ht, AdministratorInfo adminInfo, string userName)
        {
            lock (ht.SyncRoot)
            {
                ht[userName] = adminInfo;
            }
        }

        public static void RemoveCache(string userName)
        {
            Hashtable ht = GetActiveAdminInfo();

            lock (ht.SyncRoot)
            {
                ht.Remove(userName);
            }
        }

        const string cacheKey = "BaiRong.Core.AdminManager";

        public static void Clear()
        {
            CacheUtils.Remove(cacheKey);
        }

        public static Hashtable GetActiveAdminInfo()
        {
            Hashtable ht = CacheUtils.Get(cacheKey) as Hashtable;
            if (ht == null)
            {
                ht = new Hashtable();
                CacheUtils.Insert(cacheKey, ht, null, CacheUtils.HourFactor * 12);
            }
            return ht;
        }


        ////

        public static bool CreateAdministrator(AdministratorInfo administratorInfo, out string errorMessage)
        {
            try
            {
                administratorInfo.LastActivityDate = DateUtils.SqlMinValue;
                administratorInfo.CreationDate = DateTime.Now;
                administratorInfo.PasswordFormat = EPasswordFormat.Encrypted;
                bool isCreated = BaiRongDataProvider.AdministratorDAO.CreateUser(administratorInfo, out errorMessage);
                if (isCreated == false) return false;

                string[] roles = new string[] { EPredefinedRoleUtils.GetValue(EPredefinedRole.Administrator) };
                RoleManager.AddUserToRoles(administratorInfo.UserName, roles);

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public static bool Authticate(string userName, string password, out string errorMessage)
        {
            bool isValid = BaiRongDataProvider.AdministratorDAO.ValidateUser(userName, password, out errorMessage);

            if (isValid)
            {
                LogUtils.AddLog(userName, "后台管理员登录");
            }

            return isValid;
        }

        //Utils

        public const string AnonymousUserName = "Anonymous";
        public const string SysUserName = "sys";

        public static void VerifyPermissions(params string[] permissionArray)
        {
            if (AdminManager.HasPermissions(permissionArray))
            {
                return;
            }
            AdminManager.LogoutAndRedirect();
        }

        public static bool HasPermissions(params string[] permissionArray)
        {
            if (PermissionsManager.Current.IsSystemAdministrator)
            {
                return true;
            }
            ArrayList permissionArrayList = PermissionsManager.Current.PermissionArrayList;
            foreach (string permission in permissionArray)
            {
                if (permissionArrayList.Contains(permission))
                {
                    return true;
                }
            }

            return false;
        }

        public static void ClearUserCache(string userName)
        {
            ArrayList cacheKeyStartStrings = PermissionsManager.GetCackeKeyStartStringArrayList(userName);
            foreach (string cacheKeyStartString in cacheKeyStartStrings)
            {
                CacheUtils.RemoveByStartString(cacheKeyStartString);
                //CacheUtils.Remove(cacheKey);
            }
        }

        public static void LogoutAndRedirect()
        {
            string redirectUrl = string.Format("~/{0}/logout.aspx", FileConfigManager.Instance.AdminDirectoryName);
            LogoutAndRedirect(redirectUrl);
        }

        public static void LogoutAndRedirect(string redirectUrl)
        {
            AdminManager.Logout();
            PageUtils.Redirect(PageUtils.ParseNavigationUrl(redirectUrl));
        }

        public static void Logout()
        {
            AdminManager.ClearUserCache(AdminManager.Current.UserName);
            BaiRongDataProvider.AdministratorDAO.Logout();
        }

        public static string GetRolesHtml(string userName)
        {
            bool isConsoleAdministrator = false;
            bool isSystemAdministrator = false;
            ArrayList arraylist = new ArrayList();
            string[] roles = RoleManager.GetRolesForUser(userName);
            foreach (string role in roles)
            {
                if (!EPredefinedRoleUtils.IsPredefinedRole(role))
                {
                    arraylist.Add(role);
                }
                else
                {
                    if (EPredefinedRoleUtils.Equals(EPredefinedRole.ConsoleAdministrator, role))
                    {
                        isConsoleAdministrator = true;
                        break;
                    }
                    else if (EPredefinedRoleUtils.Equals(EPredefinedRole.SystemAdministrator, role))
                    {
                        isSystemAdministrator = true;
                        break;
                    }
                }
            }

            string retval = string.Empty;

            if (isConsoleAdministrator)
            {
                retval += EPredefinedRoleUtils.GetText(EPredefinedRole.ConsoleAdministrator);
            }
            else if (isSystemAdministrator)
            {
                retval += EPredefinedRoleUtils.GetText(EPredefinedRole.SystemAdministrator);
            }
            else
            {
                retval += TranslateUtils.ObjectCollectionToString(arraylist);
            }
            return retval;
        }

        #region 投稿管理
        
        public static bool HasChannelPermissionByAdmin(string userName)
        { 
            string[] roles = RoleManager.GetRolesForUser(userName);

            if (EPredefinedRoleUtils.IsConsoleAdministrator(roles))
            {
                return true;
            }
            else if (EPredefinedRoleUtils.IsSystemAdministrator(roles))
            {
                return true;
            }
            else if (EPredefinedRoleUtils.IsAdministrator(roles))
            {
                return false;
            }
            else
                return false;
        }


        public static bool HasChannelPermissionIsConsoleAdministrator(string userName)
        {
            string[] roles = RoleManager.GetRolesForUser(userName);

            if (EPredefinedRoleUtils.IsConsoleAdministrator(roles))
            {
                return true;
            } 
            else
                return false;
        }

        public static bool HasChannelPermissionIsSystemAdministrator(string userName)
        {
            string[] roles = RoleManager.GetRolesForUser(userName);

            if (EPredefinedRoleUtils.IsSystemAdministrator(roles))
            {
                return true;
            }
            else
                return false;
        }
        
        #endregion
    }
}
