using System;
using System.Collections;
using System.Text;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using BaiRong.Core.Service;
using BaiRong.Model.Service;
using BaiRong.Core.Data.Provider;
using BaiRong.Service;
using BaiRong.Service.Execution;
using System.IO;

namespace SiteServer.STL.Execution
{
    public class PublishExecution : ExecutionBase, IExecution
    {
        private static Hashtable JUST_IN_TIME = new Hashtable();

        public bool Execute(TaskInfo taskInfo)
        {
            base.Init();

            TaskPublishInfo taskPublishInfo = new TaskPublishInfo(taskInfo.ServiceParameters);
            if (taskPublishInfo != null && !string.IsNullOrEmpty(taskPublishInfo.PublishTypes))
            {
                if (taskInfo.FrequencyType == EFrequencyType.JustInTime)
                {
                    return this.ExecuteJustInTime(taskInfo);
                }
                PublishmentSystemInfo publishmentSystemInfo = null;
                if (taskInfo.PublishmentSystemID != 0)
                {
                    publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(taskInfo.PublishmentSystemID);
                    if (publishmentSystemInfo == null)
                    {
                        TaskManager.LogError(taskInfo, new Exception("无法找到对应站点"));
                        return false;
                    }
                }
                ArrayList publishTypes = TranslateUtils.StringCollectionToArrayList(taskPublishInfo.PublishTypes);
                Hashtable directoryPathHashtable = new Hashtable();
                Hashtable storagePathHashtable = new Hashtable();
                foreach (string publishType in publishTypes)
                {
                    int storageID = 0;
                    string directoryPath = PathUtility.GetPublishmentSystemPath(publishmentSystemInfo);
                    string storagePath = string.Empty;
                    if (EPublishTypeUtils.Equals(EPublishType.Site, publishType) && publishmentSystemInfo.Additional.IsSiteStorage)
                    {
                        storageID = publishmentSystemInfo.Additional.SiteStorageID;
                        storagePath = publishmentSystemInfo.Additional.SiteStoragePath;
                    }
                    else if (EPublishTypeUtils.Equals(EPublishType.Image, publishType) && publishmentSystemInfo.Additional.IsImageStorage)
                    {
                        storageID = publishmentSystemInfo.Additional.ImageStorageID;
                        directoryPath = PathUtils.Combine(directoryPath, publishmentSystemInfo.Additional.ImageUploadDirectoryName);
                        storagePath = publishmentSystemInfo.Additional.ImageStoragePath;
                    }
                    else if (EPublishTypeUtils.Equals(EPublishType.Video, publishType) && publishmentSystemInfo.Additional.IsVideoStorage)
                    {
                        storageID = publishmentSystemInfo.Additional.VideoStorageID;
                        directoryPath = PathUtils.Combine(directoryPath, publishmentSystemInfo.Additional.VideoUploadDirectoryName);
                        storagePath = publishmentSystemInfo.Additional.VideoStoragePath;
                    }
                    else if (EPublishTypeUtils.Equals(EPublishType.File, publishType) && publishmentSystemInfo.Additional.IsFileStorage)
                    {
                        storageID = publishmentSystemInfo.Additional.FileStorageID;
                        directoryPath = PathUtils.Combine(directoryPath, publishmentSystemInfo.Additional.FileUploadDirectoryName);
                        storagePath = publishmentSystemInfo.Additional.FileStoragePath;
                    }

                    if (storageID > 0)
                    {
                        directoryPathHashtable[storageID] = directoryPath;
                        storagePathHashtable[storageID] = storagePath;
                    }
                }

                foreach (int storageID in directoryPathHashtable.Keys)
                {
                    string directoryPath = (string)directoryPathHashtable[storageID];
                    string storagePath = (string)storagePathHashtable[storageID];

                    StorageInfo storageInfo = BaiRongDataProvider.StorageDAO.GetStorageInfo(storageID);
                    if (storageInfo == null || storageInfo.IsEnabled == false)
                    {
                        continue;
                    }
                    StorageManager storageManager = new StorageManager(taskInfo, storageInfo, storagePath);
                    if (!storageManager.IsEnabled) continue;

                    string[] fileNames = DirectoryUtils.GetFileNames(directoryPath);
                    if (fileNames != null && fileNames.Length > 0)
                    {
                        storageManager.Manager.UploadFiles(fileNames, directoryPath);
                    }

                    string[] directoryNames = DirectoryUtils.GetDirectoryNames(directoryPath);
                    if (directoryNames != null && directoryNames.Length > 0)
                    {
                        foreach (string directoryName in directoryNames)
                        {
                            storageManager.Manager.UploadDirectory(directoryPath, directoryName);
                        }
                    }
                }

                if (taskInfo.ServiceType == EServiceType.Publish && taskInfo.FrequencyType == EFrequencyType.OnlyOnce)
                {
                    BaiRongDataProvider.TaskDAO.Delete(taskInfo.TaskID);
                }
            }

            return true;
        }

        private bool ExecuteJustInTime(TaskInfo taskInfo)
        {
            if (PublishExecution.JUST_IN_TIME[taskInfo.TaskID] != null)
            {
                return false;
            }
            PublishExecution.JUST_IN_TIME[taskInfo.TaskID] = true;

            TaskPublishInfo taskPublishInfo = new TaskPublishInfo(taskInfo.ServiceParameters);
            if (taskPublishInfo != null)
            {
                PublishmentSystemInfo publishmentSystemInfo = null;
                if (taskInfo.PublishmentSystemID != 0)
                {
                    publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(taskInfo.PublishmentSystemID);
                    if (publishmentSystemInfo == null)
                    {
                        TaskManager.LogError(taskInfo, new Exception("无法找到对应站点"));
                        return false;
                    }
                }

                ArrayList publishTypes = TranslateUtils.StringCollectionToArrayList(taskPublishInfo.PublishTypes);
                Hashtable directoryPathHashtable = new Hashtable();
                Hashtable storagePathHashtable = new Hashtable();
                foreach (string publishType in publishTypes)
                {
                    int storageID = 0;
                    string directoryPath = PathUtility.GetPublishmentSystemPath(publishmentSystemInfo);
                    string storagePath = string.Empty;
                    if (EPublishTypeUtils.Equals(EPublishType.Site, publishType) && publishmentSystemInfo.Additional.IsSiteStorage)
                    {
                        storageID = publishmentSystemInfo.Additional.SiteStorageID;
                        storagePath = publishmentSystemInfo.Additional.SiteStoragePath;
                    }
                    else if (EPublishTypeUtils.Equals(EPublishType.Image, publishType) && publishmentSystemInfo.Additional.IsImageStorage)
                    {
                        storageID = publishmentSystemInfo.Additional.ImageStorageID;
                        directoryPath = PathUtils.Combine(directoryPath, publishmentSystemInfo.Additional.ImageUploadDirectoryName);
                        storagePath = publishmentSystemInfo.Additional.ImageStoragePath;
                    }
                    else if (EPublishTypeUtils.Equals(EPublishType.Video, publishType) && publishmentSystemInfo.Additional.IsVideoStorage)
                    {
                        storageID = publishmentSystemInfo.Additional.VideoStorageID;
                        directoryPath = PathUtils.Combine(directoryPath, publishmentSystemInfo.Additional.VideoUploadDirectoryName);
                        storagePath = publishmentSystemInfo.Additional.VideoStoragePath;
                    }
                    else if (EPublishTypeUtils.Equals(EPublishType.File, publishType) && publishmentSystemInfo.Additional.IsFileStorage)
                    {
                        storageID = publishmentSystemInfo.Additional.FileStorageID;
                        directoryPath = PathUtils.Combine(directoryPath, publishmentSystemInfo.Additional.FileUploadDirectoryName);
                        storagePath = publishmentSystemInfo.Additional.FileStoragePath;
                    }

                    if (storageID > 0)
                    {
                        directoryPathHashtable[storageID] = directoryPath;
                        storagePathHashtable[storageID] = storagePath;
                    }
                }

                foreach (int storageID in directoryPathHashtable.Keys)
                {
                    string directoryPath = (string)directoryPathHashtable[storageID];
                    string storagePath = (string)storagePathHashtable[storageID];

                    if (PathUtils.IsDirectoryPath(directoryPath))
                    {
                        DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);
                        WatcherManager watcherManager = new WatcherManager(taskInfo, storageID, directoryPath, storagePath, taskPublishInfo.Filter);
                        watcherManager.Start();
                    }
                }

                if (taskInfo.ServiceType == EServiceType.Publish && taskInfo.FrequencyType == EFrequencyType.OnlyOnce)
                {
                    BaiRongDataProvider.TaskDAO.Delete(taskInfo.TaskID);
                }
            }
            return true;
        }
    }
}
