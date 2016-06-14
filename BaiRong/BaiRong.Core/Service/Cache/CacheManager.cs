using System;
using System.Web;
using System.Web.Caching;
using BaiRong.Core.IO;
using BaiRong.Model;
using BaiRong.Core;

namespace BaiRong.Core.Service
{
    public class CacheManager
    {
        private CacheManager() { }

        private static HttpRuntime _httpRuntime;

        private static Cache Cache
        {
            get
            {
                EnsureHttpRuntime();
                return HttpRuntime.Cache;
            }
        }

        private static void EnsureHttpRuntime()
        {
            if (null == _httpRuntime)
            {
                _httpRuntime = new HttpRuntime();
            }
        }

        public static object GetCache(string cacheKey)
        {
            return CacheManager.Cache.Get(cacheKey);
        }

        public static void SetCache(string cacheKey, object obj)
        {
            CacheManager.Cache.Insert(cacheKey, obj, null, DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.AboveNormal, null);
        }

        private static void SetCache(string cacheKey, object obj, DateTime dateTime)
        {
            CacheManager.Cache.Insert(cacheKey, obj, null, dateTime, TimeSpan.Zero, CacheItemPriority.AboveNormal, null);
        }

        public static void RemoveCache(string cacheKey)
        {
            CacheManager.Cache.Remove(cacheKey);
        }

        public static void UpdateTemporaryCacheFile(string cacheFileName)
        {
            string cacheFilePath = GetCacheFilePath(cacheFileName);
            FileUtils.WriteText(cacheFilePath, ECharset.utf_8, "cache chaged");
        }

        public static string GetCacheFilePath(string cacheFileName)
        {
            return PathUtils.Combine(ConfigUtils.Instance.PhysicalApplicationPath, DirectoryUtils.SiteFiles.DirectoryName, DirectoryUtils.SiteFiles.TemporaryFiles, cacheFileName);
        }

        #region Status

        public const string CacheKey_Status = "BaiRong.Core.Service.Status";
        public const int Slide_Minutes_Status = 10;

        public static bool IsServiceOnline(out DateTime dateTime)
        {
            dateTime = DateTime.MinValue;
            string value = DbCacheManager.Get(CacheManager.CacheKey_Status);
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            dateTime = TranslateUtils.ToDateTime(value);
            return true;
        }

        public static void RenewServiceStatus()
        {
            object obj = CacheManager.GetCache(CacheManager.CacheKey_Status);
            if (obj == null)
            {
                DbCacheManager.GetAndRemove(CacheManager.CacheKey_Status);
                DbCacheManager.Insert(CacheManager.CacheKey_Status, DateTime.Now.ToString());
                CacheManager.SetCache(CacheManager.CacheKey_Status, DateTime.Now.ToString(), DateTime.UtcNow.AddMinutes(Slide_Minutes_Status));
            }
        }

        #endregion
    }
}
