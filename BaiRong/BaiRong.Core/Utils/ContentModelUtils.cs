using System;
using System.Text;
using System.Xml;
using System.Collections;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.IO;
using BaiRong.Core.Net;
using BaiRong.Core.Data.Provider;

namespace BaiRong.Core
{
	public class ContentModelUtils
	{
        private ContentModelUtils()
		{
		}

        public static ArrayList GetContentModelArrayList(string productID, int siteID)
        {
            string cacheKey = GetCacheKey(productID, siteID);
            lock (lockObject)
            {
                if (CacheUtils.Get(cacheKey) == null)
                {
                    ArrayList arraylist = BaiRongDataProvider.ContentModelDAO.GetContentModelInfoArrayList(productID, siteID);

                    CacheUtils.Insert(cacheKey, arraylist, CacheUtils.HourFactor);
                    return arraylist;
                }
                return CacheUtils.Get(cacheKey) as ArrayList;
            }
        }

        public static void RemoveCache(string productID, int siteID)
        {
            string cacheKey = GetCacheKey(productID, siteID);
            CacheUtils.Remove(cacheKey);
        }

        private static string GetCacheKey(string productID, int siteID)
        {
            return cacheKeyPrefix + productID + siteID;
        }

        private static readonly object lockObject = new object();
        private const string cacheKeyPrefix = "BaiRong.Core.ContentModelUtils.";
	}
}
