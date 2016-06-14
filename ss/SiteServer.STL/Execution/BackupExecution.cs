using System;
using System.Collections;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using BaiRong.Model.Service;
using BaiRong.Core.Service;
using BaiRong.Core.Data.Provider;
using BaiRong.Service;
using BaiRong.Service.Execution;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using SiteServer.STL.IO;

namespace SiteServer.STL.Execution
{
    public class BackupExecution : ExecutionBase, IExecution
    {
        public bool Execute(TaskInfo taskInfo)
        {
            base.Init();

            TaskBackupInfo taskBackupInfo = new TaskBackupInfo(taskInfo.ServiceParameters);

            if (taskInfo.PublishmentSystemID != 0)
            {
                return BackupByPublishmentSystemID(taskInfo, taskInfo.PublishmentSystemID, taskBackupInfo.BackupType);
            }
            else
            {
                ArrayList publishmentSystemIDArrayList = null;
                if (taskBackupInfo.IsBackupAll)
                {
                    publishmentSystemIDArrayList = PublishmentSystemManager.GetPublishmentSystemIDArrayList();
                }
                else
                {
                    publishmentSystemIDArrayList = TranslateUtils.StringCollectionToIntArrayList(taskBackupInfo.PublishmentSystemIDCollection);
                }
                foreach (int publishmentSystemID in publishmentSystemIDArrayList)
                {
                    BackupByPublishmentSystemID(taskInfo, publishmentSystemID, taskBackupInfo.BackupType);
                }
            }

            return true;
        }

        private static bool BackupByPublishmentSystemID(TaskInfo taskInfo, int publishmentSystemID, EBackupType backupType)
        {
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            if (publishmentSystemInfo == null)
            {
                TaskManager.LogError(taskInfo, new Exception("无法找到对应站点"));
                return false;
            }

            string filePath = PathUtility.GetBackupFilePath(publishmentSystemInfo, backupType);
            DirectoryUtils.CreateDirectoryIfNotExists(filePath);
            FileUtils.DeleteFileIfExists(filePath);

            if (backupType == EBackupType.Templates)
            {
                BackupUtility.BackupTemplates(publishmentSystemInfo.PublishmentSystemID, filePath);
            }
            else if (backupType == EBackupType.ChannelsAndContents)
            {
                BackupUtility.BackupChannelsAndContents(publishmentSystemInfo.PublishmentSystemID, filePath);
            }
            else if (backupType == EBackupType.Files)
            {
                BackupUtility.BackupFiles(publishmentSystemInfo.PublishmentSystemID, filePath);
            }
            else if (backupType == EBackupType.Site)
            {
                BackupUtility.BackupSite(publishmentSystemInfo.PublishmentSystemID, filePath);
            }

            if (publishmentSystemInfo.Additional.IsBackupStorage)
            {
                int storageID = publishmentSystemInfo.Additional.BackupStorageID;
                string storagePath = publishmentSystemInfo.Additional.BackupStoragePath;

                StorageInfo storageInfo = BaiRongDataProvider.StorageDAO.GetStorageInfo(storageID);
                if (storageInfo != null && storageInfo.IsEnabled)
                {
                    StorageManager storageManager = new StorageManager(taskInfo, storageInfo, storagePath);
                    if (storageManager.IsEnabled)
                    {
                        storageManager.Manager.CopyFrom(filePath);
                    }
                }
            }

            return true;
        }
    }
}
