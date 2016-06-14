using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Service;
using BaiRong.Model.Service;
using BaiRong.Service.Execution;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;

namespace SiteServer.STL.Execution
{
    public class WatcherManager
    {
        BackgroundWorker backgroundWorker = new BackgroundWorker();
        FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
        Dictionary<string, int> toPublish = new Dictionary<string, int>();
        Queue<string> publishQueue = new Queue<string>();
        private int publishDelay = 1000;

        private TaskInfo taskInfo;
        private StorageInfo storageInfo;
        private string directoryPath;
        private string storagePath;
        private string filter;
        private const string cachekey = "SiteServer.CMS.PublishmentSystemDir";

        public WatcherManager(TaskInfo taskInfo, int storageID, string directoryPath, string storagePath, string filter)
        {
            this.taskInfo = taskInfo;
            this.storageInfo = BaiRongDataProvider.StorageDAO.GetStorageInfo(storageID);
            this.directoryPath = directoryPath;
            this.storagePath = storagePath;
            this.filter = filter;
        }

        /// <summary>
        /// Start watching the selected files; start backgroundWorker/
        /// </summary>
        public void Start()
        {
            if (this.storageInfo == null || this.storageInfo.IsEnabled == false)
            {
                return;
            }
            InitializeWatcher();
        }

        /// <summary>
        /// Enqueue file paths for publish by backGroundWorker.
        /// </summary>
        private void EnqueueForPublish()
        {
            lock (this.toPublish)
            {
                foreach (var item in this.toPublish)
                {
                    if (!this.publishQueue.Contains(item.Key))
                    {
                        this.publishQueue.Enqueue(item.Key);
                    }
                }
                this.toPublish.Clear();
            }
        }

        /// <summary>
        /// Initialize fileSystemWatcher.
        /// </summary>
        private void InitializeWatcher()
        {
            fileSystemWatcher.Path = this.directoryPath;
            if (!string.IsNullOrEmpty(this.filter))
            {
                fileSystemWatcher.Filter = this.filter;
            }
            fileSystemWatcher.EnableRaisingEvents = true;
            fileSystemWatcher.IncludeSubdirectories = true;
            fileSystemWatcher.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastWrite;

            fileSystemWatcher.Changed += fileSystemWatcher_Detected;
            fileSystemWatcher.Created += fileSystemWatcher_Detected;
            fileSystemWatcher.Renamed += fileSystemWatcher_Detected;
            backgroundWorker.DoWork += backgroundWorker_DoWork;

            if (!backgroundWorker.IsBusy)
            {
                backgroundWorker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// filesystemWatcher detected a change in one of the files.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileSystemWatcher_Detected(object sender, FileSystemEventArgs e)
        {
            if (this.toPublish.ContainsKey(e.FullPath))
            {
                this.toPublish[e.FullPath] = 0;
            }
            else
            {
                this.toPublish.Add(e.FullPath, 0);
            }

            Thread.Sleep(this.publishDelay);

            EnqueueForPublish();
        }

        public ArrayList GetPublishmentSystemDirs()
        {
            ArrayList publishmentSystemDirArrayList = new ArrayList();
            DictionaryEntryArrayList sl = PublishmentSystemManager.GetPublishmentSystemInfoDictionaryEntryArrayList();
            foreach (DictionaryEntry entry in sl)
            {
                PublishmentSystemInfo publishmentSystemInfo = entry.Value as PublishmentSystemInfo;
                if (!publishmentSystemInfo.IsHeadquarters)
                {
                    publishmentSystemDirArrayList.Add(publishmentSystemInfo.PublishmentSystemDir);
                }
            }
            return publishmentSystemDirArrayList;
        }

        /// <summary>
        /// backgroundWorkder queue processing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (this.backgroundWorker.CancellationPending)
                {
                    return;
                }
                if (this.publishQueue.Count > 0)
                {
                    string path = this.publishQueue.Dequeue();
                  
                    try
                    {
                        StorageManager storageManager = null;
                        if (storageInfo.StorageType == EStorageType.Ftp)
                        {
                            if (storageManager == null)
                            {
                                storageManager = new StorageManager(taskInfo, storageInfo, this.storagePath);
                            }
                        }
                        else
                        {
                            storageManager = CacheUtils.Get("SiteServer.CMS.Execution.WatcherManager.StorageManager") as StorageManager;
                            if (storageManager == null)
                            {
                                storageManager = new StorageManager(taskInfo, storageInfo, this.storagePath);
                                CacheUtils.Insert("SiteServer.CMS.Execution.WatcherManager.StorageManager", storageManager, 60);//60
                            }
                        }
                         
                        if (!storageManager.IsEnabled) return;
                         
                        ArrayList publishmentSystemDirArrayList = this.GetPublishmentSystemDirs();
                        if (string.IsNullOrEmpty(DbCacheManager.Get(cachekey)))
                        {
                            DbCacheManager.Insert(cachekey, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(publishmentSystemDirArrayList));
                        }
                         
                        string dirPath = PathUtils.IsDirectoryPath(path) ? path : DirectoryUtils.GetDirectoryPath(path);
                        string pathDifference = PathUtils.GetPathDifference(this.directoryPath, dirPath);
                        string [] dirs = pathDifference.Split('\\'); 

                        if (!string.IsNullOrEmpty(pathDifference))
                        {
                            storageManager.Manager.ChangeDirDownByRelatedPath(pathDifference);
                        }

                        if (PathUtils.IsDirectoryPath(path))
                        {
                            string[] fileNames = DirectoryUtils.GetFileNames(path);
                            if (fileNames != null && fileNames.Length > 0)
                            {
                                if (!publishmentSystemDirArrayList.Contains(dirs[0].ToString()))
                                {
                                    storageManager.Manager.UploadFiles(fileNames, path);
                                }
                            }

                            string[] directoryNames = DirectoryUtils.GetDirectoryNames(path);
                            if (directoryNames != null && directoryNames.Length > 0)
                            {
                                foreach (string directoryName in directoryNames)
                                {
                                    if (publishmentSystemDirArrayList.Contains(directoryName)) continue;
                                    storageManager.Manager.UploadDirectory(path, directoryName);
                                }
                            }
                        }
                        else
                        {
                            if (!publishmentSystemDirArrayList.Contains(dirs[0].ToString()))
                            {
                                storageManager.Manager.UploadFile(path);
                            }
                        }

                        if (!string.IsNullOrEmpty(pathDifference))
                        {
                            storageManager.Manager.ChangeDirUpByRelatedPath(pathDifference);
                        }
                    }
                    catch (Exception ex)
                    {
                        TaskManager.LogError(this.taskInfo, ex);
                    }
                }
                Thread.Sleep(50);//50
            }
        }
    }
}
