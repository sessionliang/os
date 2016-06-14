using System;
using System.Collections;
using System.Text;
using System.Data;
using SiteServer.BBS.Model;
using BaiRong.Core;
using BaiRong.Model;

using SiteServer.CMS.Core;

namespace SiteServer.BBS.Core
{
    public class PermissionManager
    {
        private static readonly object lockObject = new object();

        private static string GetCacheKey(int publishmentSystemID)
        {
            return string.Format("SiteServer.BBS.Core.PermissionManager.{0}", publishmentSystemID);
        }

        public static void RemoveCache(int publishmentSystemID)
        {
            string cacheKey = GetCacheKey(publishmentSystemID);
            CacheUtils.Remove(cacheKey);
        }

        public static Hashtable GetForbiddenHashtable(int publishmentSystemID)
        {
            lock (lockObject)
            {
                string cacheKey = GetCacheKey(publishmentSystemID);
                if (CacheUtils.Get(cacheKey) == null)
                {
                    Hashtable hashtable = DataProvider.PermissionsDAO.GetForbiddenHashtable(publishmentSystemID);
                    CacheUtils.Max(cacheKey, hashtable);
                    return hashtable;
                }
                return CacheUtils.Get(cacheKey) as Hashtable;
            }
        }

        public static string GetForbidden(int publishmentSystemID, int groupID, int forumID)
        {
            string forbidden = null;
            Hashtable hashtable = GetForbiddenHashtable(publishmentSystemID);
            if (hashtable != null)
            {
                forbidden = hashtable[groupID + "_" + forumID] as string;
                if (forbidden == null)
                {
                    forbidden = TranslateUtils.ObjectCollectionToString(EPermissionUtils.GetForbiddenArrayList(UserGroupManager.GetGroupType(PublishmentSystemManager.GetGroupSN(publishmentSystemID), groupID)));
                }
            }
            return forbidden;
        }
    }
}
