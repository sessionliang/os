using System;
using System.Collections;
using System.Web;
using System.Web.Security;

using BaiRong.Core;
using BaiRong.Model;


using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core.Security
{
    public class AdminUtility
    {
        public static bool HasWebsitePermissions(int publishmentSystemID, params string[] websitePermissionArray)
        {
            if (PermissionsManager.Current.IsSystemAdministrator)
            {
                return true;
            }
            ArrayList websitePermissionArrayList = ProductPermissionsManager.Current.WebsitePermissionSortedList[publishmentSystemID] as ArrayList;
            if (websitePermissionArrayList != null && websitePermissionArrayList.Count > 0)
            {
                foreach (string websitePermission in websitePermissionArray)
                {
                    if (websitePermissionArrayList.Contains(websitePermission))
                    {
                        return true;
                    }
                }
            }
            #region 用户中心不对菜单做权限验证
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            if (publishmentSystemInfo != null && publishmentSystemInfo.PublishmentSystemType == EPublishmentSystemType.UserCenter)
                return true; 
            #endregion
            return false;
        }

        public static void VerifyWebsitePermissions(int publishmentSystemID, params string[] websitePermissionArray)
        {
            if (HasWebsitePermissions(publishmentSystemID, websitePermissionArray))
            {
                return;
            }
            AdminManager.LogoutAndRedirect();
        }

        public static bool HasChannelPermissions(ArrayList channelPermissionArrayList, params string[] channelPermissionArray)
        {
            if (PermissionsManager.Current.IsSystemAdministrator)
            {
                return true;
            }
            foreach (string channelPermission in channelPermissionArray)
            {
                if (channelPermissionArrayList.Contains(channelPermission))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool HasChannelPermissions(int publishmentSystemID, int nodeID, params string[] channelPermissionArray)
        {
            if (nodeID == 0) return false;
            if (PermissionsManager.Current.IsSystemAdministrator)
            {
                return true;
            }
            ArrayList channelPermissionArrayList = (ArrayList)ProductPermissionsManager.Current.ChannelPermissionSortedList[nodeID];
            if (channelPermissionArrayList != null && HasChannelPermissions(channelPermissionArrayList, channelPermissionArray))

            {
                return true;
            }
            #region 用户中心不对菜单做权限验证
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            if (publishmentSystemInfo != null && publishmentSystemInfo.PublishmentSystemType == EPublishmentSystemType.UserCenter)
                return true;
            #endregion
            int parentNodeID = NodeManager.GetParentID(publishmentSystemID, nodeID);
            return HasChannelPermissions(publishmentSystemID, parentNodeID, channelPermissionArray);
        }

        public static bool HasChannelPermissionsIgnoreNodeID(params string[] channelPermissionArray)
        {
            if (PermissionsManager.Current.IsSystemAdministrator)
            {
                return true;
            }
            if (HasChannelPermissions(ProductPermissionsManager.Current.ChannelPermissionArrayListIgnoreNodeID, channelPermissionArray))
            {
                return true;
            }
            return false;
        }

        public static void VerifyChannelPermissions(int publishmentSystemID, int nodeID, params string[] channelPermissionArray)
        {
            if (AdminUtility.HasChannelPermissions(publishmentSystemID, nodeID, channelPermissionArray))
            {
                return;
            }
            AdminManager.LogoutAndRedirect();
        }

        public static bool IsOwningNodeID(int nodeID)
        {
            if (PermissionsManager.Current.IsSystemAdministrator)
            {
                return true;
            }
            if (ProductPermissionsManager.Current.OwningNodeIDArrayList.Contains(nodeID))
            {
                return true;
            }
            return false;
        }

        public static bool IsHasChildOwningNodeID(int nodeID)
        {
            if (PermissionsManager.Current.IsSystemAdministrator)
            {
                return true;
            }
            ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListForDescendant(nodeID);
            foreach (int theNodeID in nodeIDArrayList)
            {
                if (IsOwningNodeID(theNodeID))
                {
                    return true;
                }
            }
            return false;
        }

        //public static void ClearUserCache(string userName)
        //{
        //    // Roles cache key
        //    CacheUtils.Remove("User_Roles:" + userName);
        //    // GeneralPermissionArrayList cache key
        //    CacheUtils.Remove("User_GeneralPermissionArrayList:" + userName);
        //    // WebsitePermissionSortedList cache key
        //    CacheUtils.Remove("User_WebsitePermissionSortedList:" + userName);
        //    // ChannelPermissionSortedList cache key
        //    CacheUtils.Remove("User_ChannelPermissionSortedList:" + userName);
        //    // ChannelPermissionArrayListIgnoreNodeID cache key
        //    CacheUtils.Remove("User_ChannelPermissionArrayListIgnoreNodeID:" + userName);
        //    // LastException cache key
        //    CacheUtils.Remove("User_LastException:" + userName);

        //    //OwningNodeID cahce
        //    OwningNodeIDCache.Clear(userName);
        //}

        public static bool IsViewContentOnlySelf(int publishmentSystemID, int nodeID)
        {
            if (PermissionsManager.Current.IsConsoleAdministrator || PermissionsManager.Current.IsSystemAdministrator)
                return false;
            if (AdminUtility.HasChannelPermissions(publishmentSystemID, nodeID, AppManager.CMS.Permission.Channel.ContentCheck))
                return false;
            return ConfigManager.Additional.IsViewContentOnlySelf;
        }
    }
}
