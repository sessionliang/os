using System.Collections;
using BaiRong.Core.IO;
using System;
using BaiRong.Core.Data.Provider;
using BaiRong.Model.Service;

namespace BaiRong.Core.Service
{
    public class CacheStorage
    {
        #region private

        private CacheStorage() { }

        private static CacheStorage cacheStorage;

        public const string CacheFileName = "StorageCache.txt";

        #endregion

        public static CacheStorage Instance
        {
            get
            {
                if (cacheStorage == null)
                {
                    FileWatcherClass storageFileWatcher = new FileWatcherClass(CacheManager.GetCacheFilePath(CacheFileName));
                    storageFileWatcher.OnFileChange += new FileWatcherClass.FileChange(storageFileWatcher_OnFileChange);

                    cacheStorage = new CacheStorage();
                }
                return cacheStorage;
            }
        }

        private const string cacheKeyStorageInfoSortedList = "StorageInfoSortedList";

        private static void storageFileWatcher_OnFileChange(object sender, EventArgs e)
        {
            CacheManager.RemoveCache(cacheKeyStorageInfoSortedList);
        }

        private static SortedList GetStorageInfoSortedList()
        {
            if (CacheManager.GetCache(cacheKeyStorageInfoSortedList) == null)
            {
                SortedList sortedlist = BaiRongDataProvider.StorageDAO.GetStorageInfoSortedList();
                CacheManager.SetCache(cacheKeyStorageInfoSortedList, sortedlist);
                return sortedlist;
            }
            return CacheManager.GetCache(cacheKeyStorageInfoSortedList) as SortedList;
        }

        public StorageInfo GetStorageInfo(int storageID)
        {
            SortedList sortedlist = GetStorageInfoSortedList();
            return sortedlist[storageID] as StorageInfo;
        }

        public static void ClearCache()
        {
            CacheManager.RemoveCache(cacheKeyStorageInfoSortedList);
            CacheManager.UpdateTemporaryCacheFile(CacheFileName);
        }
    }
}
