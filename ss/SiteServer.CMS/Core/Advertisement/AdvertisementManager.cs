using System;
using System.Collections;
using System.Text;
using BaiRong.Core;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core.Advertisement
{
    public class AdvertisementManager
    {
        public static ArrayList[] GetAdvertisementArrayLists(int publishmentSystemID)
        {
            string cacheKey = GetCacheKey(publishmentSystemID);
            lock (lockObject)
            {
                if (CacheUtils.Get(cacheKey) == null)
                {
                    ArrayList[] arraylists = DataProvider.AdvertisementDAO.GetAdvertisementArrayLists(publishmentSystemID);
                    CacheUtils.Insert(cacheKey, arraylists, 30);
                    return arraylists;
                }
                return CacheUtils.Get(cacheKey) as ArrayList[];
            }
        }

        public static void RemoveCache(int publishmentSystemID)
        {
            string cacheKey = GetCacheKey(publishmentSystemID);
            CacheUtils.Remove(cacheKey);
        }

        private static string GetCacheKey(int publishmentSystemID)
        {
            return cacheKeyPrefix + publishmentSystemID;
        }

        private static readonly object lockObject = new object();
        private const string cacheKeyPrefix = "SiteServer.CMS.Core.Advertisement.";
    }
}
