using System;
using System.Collections;
using System.Text;
using System.Data;
using SiteServer.BBS.Model;
using BaiRong.Core;
using BaiRong.Model;

namespace SiteServer.BBS.Core
{
    public class IdentifyManager
    {
        private static readonly object lockObject = new object();

        private static string GetCacheKey(int publishmentSystemID)
        {
            return string.Format("SiteServer.BBS.Core.IdentifyManager.{0}", publishmentSystemID);
        }

        public static void RemoveCache(int publishmentSystemID)
        {
            string cacheKey = GetCacheKey(publishmentSystemID);
            CacheUtils.Remove(cacheKey);
        }

        public static Hashtable GetIdentifyInfoHashtable(int publishmentSystemID)
        {
            lock (lockObject)
            {
                string cacheKey = GetCacheKey(publishmentSystemID);
                if (CacheUtils.Get(cacheKey) == null)
                {
                    Hashtable hashtable = DataProvider.IdentifyDAO.GetIdentifyInfoHashtable(publishmentSystemID);
                    CacheUtils.Max(cacheKey, hashtable);
                    return hashtable;
                }
                return CacheUtils.Get(cacheKey) as Hashtable;
            }
        }

        public static IdentifyInfo GetIdentifyInfo(int publishmentSystemID, int identifyID)
        {
            IdentifyInfo identifyInfo = null;
            Hashtable hashtable = IdentifyManager.GetIdentifyInfoHashtable(publishmentSystemID);
            if (hashtable != null)
            {
                identifyInfo = hashtable[identifyID] as IdentifyInfo;
            }
            return identifyInfo;
        }
    }
}
