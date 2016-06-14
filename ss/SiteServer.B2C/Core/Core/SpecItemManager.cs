using System.Web.UI;
using BaiRong.Core;
using System.Collections.Specialized;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Text;
using SiteServer.B2C.Model;
using BaiRong.Model;

namespace SiteServer.B2C.Core
{
	public class SpecItemManager
    {
        #region Cache

        private static readonly object lockObject = new object();
        private const string cacheKeyPrefix = "SiteServer.B2C.Core.SpecItemManager";

        public static SpecItemInfo GetSpecItemInfo(int publishmentSystemID, int specItemID)
        {
            Dictionary<int, SpecItemInfo> dictionary = SpecItemManager.GetSpecItemInfoDictionary(publishmentSystemID);

            foreach (int key in dictionary.Keys)
            {
                if (key == specItemID)
                {
                    return dictionary[key];
                }
            }
            return null;
        }

        public static List<int> GetSpecItemIDList(int publishmentSystemID, int specID)
        {
            List<int> list = new List<int>();
            Dictionary<int, SpecItemInfo> dictionary = SpecItemManager.GetSpecItemInfoDictionary(publishmentSystemID);
            foreach (int key in dictionary.Keys)
            {
                SpecItemInfo specItemInfo = dictionary[key];
                if (specItemInfo.SpecID == specID)
                {
                    list.Add(key);
                }
            }
            return list;
        }

        public static List<SpecItemInfo> GetSpecItemInfoList(int publishmentSystemID, int specID)
        {
            List<SpecItemInfo> list = new List<SpecItemInfo>();
            Dictionary<int, SpecItemInfo> dictionary = SpecItemManager.GetSpecItemInfoDictionary(publishmentSystemID);
            foreach (int key in dictionary.Keys)
            {
                SpecItemInfo specItemInfo = dictionary[key];
                if (specItemInfo.SpecID == specID)
                {
                    list.Add(specItemInfo);
                }
            }
            return list;
        }

        public static void ClearCache(int publishmentSystemID)
        {
            string cacheKey = cacheKeyPrefix + "." + publishmentSystemID;
            CacheUtils.Remove(cacheKey);
        }

        public static Dictionary<int, SpecItemInfo> GetSpecItemInfoDictionary(int publishmentSystemID)
        {
            lock (lockObject)
            {
                string cacheKey = cacheKeyPrefix + "." + publishmentSystemID;
                if (CacheUtils.Get(cacheKey) == null)
                {
                    Dictionary<int, SpecItemInfo> dictionary = DataProviderB2C.SpecItemDAO.GetSpecItemInfoDictionary(publishmentSystemID);
                    CacheUtils.Max(cacheKey, dictionary);
                    return dictionary;
                }
                return CacheUtils.Get(cacheKey) as Dictionary<int, SpecItemInfo>;
            }
        }

        #endregion
	}
}
