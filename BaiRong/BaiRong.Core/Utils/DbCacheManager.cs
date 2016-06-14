using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using BaiRong.Core;

namespace BaiRong.Core
{
	public class DbCacheManager
	{
		private DbCacheManager()
		{
		}

		public static void Insert(string cacheKey, string cacheValue)
		{
			if (!string.IsNullOrEmpty(cacheKey))
			{
				BaiRongDataProvider.DbCacheDAO.Insert(cacheKey, cacheValue);
			}
		}

		public static void Remove(string cacheKey)
		{
            BaiRongDataProvider.DbCacheDAO.Remove(cacheKey);
		}

        public static void Clear()
        {
            BaiRongDataProvider.DbCacheDAO.Clear();
        }

		public static bool IsExists(string cacheKey)
		{
            return BaiRongDataProvider.DbCacheDAO.IsExists(cacheKey);
		}

        public static string Get(string cacheKey)
        {
            return BaiRongDataProvider.DbCacheDAO.GetCacheValue(cacheKey);
        }

		public static string GetAndRemove(string cacheKey)
		{
            string retval = BaiRongDataProvider.DbCacheDAO.GetCacheValue(cacheKey);
            BaiRongDataProvider.DbCacheDAO.Remove(cacheKey);
			return retval;
		}

        public static int GetCacheCount()
        {
            int count = 0;
            count = BaiRongDataProvider.DbCacheDAO.GetCount();
            return count;
        }

	}
}
