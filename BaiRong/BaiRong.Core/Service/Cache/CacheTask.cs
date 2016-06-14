using System.Collections;
using BaiRong.Core.IO;
using System;
using BaiRong.Core.Data.Provider;
using BaiRong.Model.Service;

namespace BaiRong.Core.Service
{
    public class CacheTask
    {
        #region private

        private CacheTask() { }

        private static CacheTask cacheTask;

        private const string CacheFileName = "TaskCache.txt";

        #endregion

        public static CacheTask Instance
        {
            get
            {
                if (cacheTask == null)
                {
                    FileWatcherClass taskFileWatcher = new FileWatcherClass(CacheManager.GetCacheFilePath(CacheFileName));
                    taskFileWatcher.OnFileChange += new FileWatcherClass.FileChange(taskFileWatcher_OnFileChange);

                    cacheTask = new CacheTask();
                }
                return cacheTask;
            }
        }

        private const string cacheKeyTaskInfoSortedList = "TaskInfoSortedList";

        private static void taskFileWatcher_OnFileChange(object sender, EventArgs e)
        {
            CacheManager.RemoveCache(cacheKeyTaskInfoSortedList);
        }

        private SortedList GetTaskInfoSortedList()
        {
            if (CacheManager.GetCache(cacheKeyTaskInfoSortedList) == null)
            {
                SortedList sortedlist = BaiRongDataProvider.TaskDAO.GetTaskInfoSortedList();
                CacheManager.SetCache(cacheKeyTaskInfoSortedList, sortedlist);
                return sortedlist;
            }
            return CacheManager.GetCache(cacheKeyTaskInfoSortedList) as SortedList;
        }

        public ArrayList GetTaskInfoArrayList()
        {
            SortedList sortedlist = this.GetTaskInfoSortedList();
            ArrayList arraylist = new ArrayList();
            foreach (int taskID in sortedlist.Keys)
            {
                TaskInfo taskInfo = sortedlist[taskID] as TaskInfo;
                if (taskInfo != null && taskInfo.IsEnabled)
                {
                    arraylist.Add(taskInfo);
                }
            }
            return arraylist;
        }

        public static void ClearCache()
        {
            CacheManager.RemoveCache(cacheKeyTaskInfoSortedList);
            CacheManager.UpdateTemporaryCacheFile(CacheFileName);
        }
    }
}
