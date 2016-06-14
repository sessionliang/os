using System;
using System.Collections;
using System.Text;
using System.Web;
using BaiRong.Model;
using BaiRong.Core;
using System.Collections.Generic;
using BaiRong.Core.Data.Provider;

namespace BaiRong.Core
{
    public class RoleManager
    {
        private RoleManager() { }

        #region RoleDAO
        public static void InsertRole(string roleName, ArrayList modules, string creatorUserName, string description)
        {
            BaiRongDataProvider.RoleDAO.InsertRole(roleName, modules, creatorUserName, description);
        }

        public static void UpdateRole(string roleName, ArrayList modules, string description)
        {
            BaiRongDataProvider.RoleDAO.UpdateRole(roleName, modules, description);
        }

        public static List<string> GetProductIDList(string[] roles)
        {
            if (EPredefinedRoleUtils.IsConsoleAdministrator(roles))
            {
                return ProductManager.GetProductIDList();
            }
            else if (EPredefinedRoleUtils.IsSystemAdministrator(roles))
            {
                List<string> productIDList = new List<string>();
                productIDList.Add(ProductManager.Apps.ProductID);
                return productIDList;
            }
            else
            {
                List<string> productIDList = new List<string>();

                ArrayList roleNameArrayList = new ArrayList();
                if (roles != null && roles.Length > 0)
                {
                    foreach (string roleName in roles)
                    {
                        if (!EPredefinedRoleUtils.IsPredefinedRole(roleName))
                        {
                            roleNameArrayList.Add(roleName);
                        }
                    }
                }

                if (roleNameArrayList.Count > 0)
                {
                    foreach (string roleName in roleNameArrayList)
                    {
                        List<string> list = BaiRongDataProvider.RoleDAO.GetProductIDList(roleName);
                        if (list.Count > 0)
                        {
                            foreach (string productID in list)
                            {
                                if (!productIDList.Contains(productID) && ProductManager.IsProductExists(productID))
                                {
                                    productIDList.Add(productID);
                                }
                            }
                        }
                    }
                }

                return productIDList;
            }
        }

        public static bool DeleteRole(string roleName)
        {
            return BaiRongDataProvider.RoleDAO.DeleteRole(roleName);
        }

        public static string GetRoleDescription(string roleName)
        {
            return BaiRongDataProvider.RoleDAO.GetRoleDescription(roleName);
        }

        public static string GetRolesCreatorUserName(string roleName)
        {
            return BaiRongDataProvider.RoleDAO.GetRolesCreatorUserName(roleName);
        }

        public static ArrayList GetRoleNameArrayListByCreatorUserName(string creatorUserName)
        {
            return BaiRongDataProvider.RoleDAO.GetRoleNameArrayListByCreatorUserName(creatorUserName);
        }

        public static string[] GetAllRoles()
        {
            return BaiRongDataProvider.RoleDAO.GetAllRoles();
        }

        public static string[] GetAllRolesByCreatorUserName(string creatorUserName)
        {
            return BaiRongDataProvider.RoleDAO.GetAllRolesByCreatorUserName(creatorUserName);
        }

        public static string[] GetRolesForUser(string userName)
        {
            return BaiRongDataProvider.RoleDAO.GetRolesForUser(userName);
        }

        public static string[] GetUsersInRole(string roleName)
        {
            return BaiRongDataProvider.RoleDAO.GetUsersInRole(roleName);
        }

        public static void RemoveUserFromRole(string userName, string roleName)
        {
            BaiRongDataProvider.RoleDAO.RemoveUserFromRole(userName, roleName);
        }

        public static void RemoveUserFromRoles(string userName, string[] roleNames)
        {
            BaiRongDataProvider.RoleDAO.RemoveUserFromRoles(userName, roleNames);
        }

        public static bool IsRoleExists(string roleName)
        {
            return BaiRongDataProvider.RoleDAO.IsRoleExists(roleName);
        }

        public static void AddUserToRoles(string userName, string[] roleNames)
        {
            BaiRongDataProvider.RoleDAO.AddUserToRoles(userName, roleNames);
        }

        public static void AddUserToRole(string userName, string roleName)
        {
            BaiRongDataProvider.RoleDAO.AddUserToRole(userName, roleName);
        }

        public static string[] FindUsersInRole(string rolename, string usernameToMatch)
        {
            return BaiRongDataProvider.RoleDAO.FindUsersInRole(rolename, usernameToMatch);
        }

        public static bool IsUserInRole(string userName, string roleName)
        {
            return BaiRongDataProvider.RoleDAO.IsUserInRole(userName, roleName);
        }
        #endregion

        public static void CreatePredefinedRoles()
        {
            ArrayList allPredefinedRoles = EPredefinedRoleUtils.GetAllPredefinedRole();
            foreach (EPredefinedRole enumRole in allPredefinedRoles)
            {
                BaiRongDataProvider.RoleDAO.InsertRole(EPredefinedRoleUtils.GetValue(enumRole), null, string.Empty, EPredefinedRoleUtils.GetText(enumRole));
            }
        }

        public static void DeleteCookie()
        {
            HttpContext current = HttpContext.Current;
            if ((current != null) && current.Request.Browser.Cookies)
            {
                string text = string.Empty;
                if (current.Request.Browser["supportsEmptyStringInCookieValue"] == "false")
                {
                    text = "NoCookie";
                }
                HttpCookie cookie = new HttpCookie(CookieName, text);
                cookie.Path = "/";
                cookie.Domain = string.Empty;
                cookie.Expires = new DateTime(0x7cf, 10, 12);
                cookie.Secure = false;
                current.Response.Cookies.Remove(CookieName);
                current.Response.Cookies.Add(cookie);
            }
        }

        public const string CookieName = "BAIRONG.ROLES";
        public const int CookieTimeout = 90;
        public const string CookiePath = "/";
        public const bool CookieSlidingExpiration = true;
        public const int MaxCachedResults = 1000;
        public const string Domain = "";
        public const bool CreatePersistentCookie = true;
        public const bool CookieRequireSSL = false;
        public const bool CacheRolesInCookie = true;
    }
}
