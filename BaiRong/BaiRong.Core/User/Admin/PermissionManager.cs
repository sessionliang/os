using System;
using System.Collections;
using System.Collections.Specialized;
using System.Threading;
using System.Web;
using BaiRong.Core;

namespace BaiRong.Core
{
	public class PermissionsManager
	{
        private string userName;
        private AdministratorWithPermissions permissions = null;

        private PermissionsManager(string userName)
        {
            this.userName = userName;
        }

        public static AdministratorWithPermissions Current
        {
            get
            {
                //PermissionsManager instance = AdminContext.Current.Permissions as PermissionsManager;
                //if (instance == null)
                //{
                //    instance = new PermissionsManager(AdminContext.Current.UserName);
                //    AdminContext.Current.Permissions = instance;
                //}
                //return instance.Permissions;

                PermissionsManager instance = new PermissionsManager(AdminManager.Current.UserName);
                return instance.Permissions;
            }
        }

        public AdministratorWithPermissions Permissions
        {
            get
            {
                if (permissions == null)
                {
                    if (!string.IsNullOrEmpty(this.userName))
                    {
                        permissions = new AdministratorWithPermissions(this.userName, false);
                    }
                    else
                    {
                        permissions = AdministratorWithPermissions.GetAnonymousUserWithPermissions();
                    }
                }
                return permissions;
            }
            set { permissions = value; }
        }

        public static string GetRolesKey(string userName)
        {
            return "User_Roles:" + userName;
        }

        public static string GetPermissionArrayListKey(string userName)
        {
            return "User_PermissionArrayList:" + userName;
        }

        public static string GetWebsitePermissionSortedListKey(string userName, string productID)
        {
            return string.Format("User_WebsitePermissionSortedList_{0}_{1}", userName, productID);
        }

        public static string GetChannelPermissionSortedListKey(string userName, string productID)
        {
            return string.Format("User_ChannelPermissionSortedList_{0}_{1}", userName, productID);
        }

        public static string GetGovInteractPermissionSortedListKey(string userName, string productID)
        {
            return string.Format("User_GovInteractPermissionSortedList_{0}_{1}", userName, productID);
        }

        public static string GetChannelPermissionArrayListIgnoreNodeIDKey(string userName, string productID)
        {
            return string.Format("User_ChannelPermissionArrayListIgnoreNodeID_{0}_{1}", userName, productID);
        }

        public static string GetPublishmentSystemIDKey(string userName, string productID)
        {
            return string.Format("User_PublishmentSystemID_{0}_{1}", userName, productID);
        }

        public static string GetOwningNodeIDArrayListKey(string userName, string productID)
        {
            return string.Format("User_OwningNodeIDArrayList_{0}_{1}", userName, productID);
        }

        public static ArrayList GetCackeKeyStartStringArrayList(string userName)
        {
            ArrayList arraylist = new ArrayList();

            arraylist.Add(PermissionsManager.GetRolesKey(userName));
            arraylist.Add(PermissionsManager.GetPermissionArrayListKey(userName));
            arraylist.Add(PermissionsManager.GetWebsitePermissionSortedListKey(userName, string.Empty));
            arraylist.Add(PermissionsManager.GetChannelPermissionSortedListKey(userName, string.Empty));
            arraylist.Add(PermissionsManager.GetGovInteractPermissionSortedListKey(userName, string.Empty));
            arraylist.Add(PermissionsManager.GetChannelPermissionArrayListIgnoreNodeIDKey(userName, string.Empty));
            arraylist.Add(PermissionsManager.GetPublishmentSystemIDKey(userName, string.Empty));
            arraylist.Add(PermissionsManager.GetOwningNodeIDArrayListKey(userName, string.Empty));

            return arraylist;
        }
	}
}
