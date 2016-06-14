using System;
using System.Collections;
using System.Text;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using BaiRong.Model.Service;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Net;
using System.Net;

namespace BaiRong.Core.Service
{
    public class StorageManager
    {
        private IStorageManager manager;
        private string errorMessage;
        private StorageInfo storageInfo;

        public StorageManager(StorageInfo storageInfo, string relatedPaths)
            : this(null, storageInfo, relatedPaths)
        {
        }

        public StorageManager(TaskInfo taskInfo, StorageInfo storageInfo, string relatedPaths)
        {
            this.storageInfo = storageInfo;

            try
            {
                if (this.storageInfo.StorageType == EStorageType.Local)
                {
                    string directoryPath = BaiRongDataProvider.LocalStorageDAO.GetDirectoryPath(storageInfo.StorageID);
                    DirectoryUtils.CreateDirectoryIfNotExists(PathUtils.Combine(directoryPath, relatedPaths));

                    this.manager = new LocalManager(taskInfo, directoryPath, relatedPaths);
                }
                else if (this.storageInfo.StorageType == EStorageType.Ftp)
                {
                    FTPStorageInfo ftpStorageInfo = BaiRongDataProvider.FTPStorageDAO.GetFTPStorageInfo(storageInfo.StorageID);
                    this.manager = new FTPManager(taskInfo, ftpStorageInfo, relatedPaths);
                }
            }
            catch (Exception ex)
            {
                if (taskInfo != null)
                {
                    TaskLogInfo logInfo = new TaskLogInfo(0, taskInfo.TaskID, false, ex.Message, ex.StackTrace, string.Empty, DateTime.Now);
                    BaiRongDataProvider.TaskLogDAO.Insert(logInfo);
                }
                this.errorMessage = ex.Message;
            }
        }

        public IStorageManager Manager
        {
            get
            {
                return this.manager;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return this.errorMessage;
            }
        }

        public EStorageType StorageType
        {
            get
            {
                return this.storageInfo.StorageType;
            }
        }

        #region Utils

        public const string TEST_FILE_NAME = "storage_test.txt";

        public bool TestWrite()
        {
            if (!this.IsEnabled) return false;
            try
            {
                return this.Manager.WriteText(TEST_FILE_NAME, ECharset.utf_8, string.Format(@"this is storage connection test file!
last test time:{0}
", DateTime.Now.ToString()));
            }
            catch (Exception ex)
            {
                this.errorMessage = ex.Message;
                return false;
            }
        }

        public bool TestRead()
        {
            if (!this.IsEnabled) return false;
            try
            {
                string fileUrl = PageUtils.Combine(this.storageInfo.StorageUrl, TEST_FILE_NAME);
                HttpStatusCode statusCode = WebClientUtils.GetRemoteUrlStatusCode(fileUrl);
                if (statusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                this.errorMessage = statusCode.ToString();
                return false;
            }
            catch(Exception ex)
            {
                this.errorMessage = ex.Message;
                return false;
            }
        }

        public bool IsEnabled
        {
            get
            {
                if (this.manager == null)
                {
                    return false;
                }
                return this.manager.IsEnabled;
            }
        }

        public static string GetCurrentPath(string userName, bool isRemote)
        {
            string cacheKey = string.Format("StorageManager.CurrentPath.{0}.{1}", userName, isRemote);
            return DbCacheManager.Get(cacheKey);
        }

        public static void SetCurrentPath(string userName, string currentDirectory, bool isRemote)
        {
            string cacheKey = string.Format("StorageManager.CurrentPath.{0}.{1}", userName, isRemote);
            DbCacheManager.Insert(cacheKey, currentDirectory);
        }

        #endregion
    }
}
