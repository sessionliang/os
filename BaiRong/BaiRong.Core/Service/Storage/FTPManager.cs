using System;
using System.Collections;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Core.IO.FileManagement;
using BaiRong.Core.Net;
using BaiRong.Model;
using BaiRong.Model.Service;
using BaiRong.Core.Data.Provider;

namespace BaiRong.Core.Service
{
    public class FTPManager : IStorageManager
	{
        private readonly FTPStorageInfo ftpStorageInfo;
        private FTP ftplib;
        private bool isEnabled;
        private TaskInfo taskInfo;

        public FTPManager(TaskInfo taskInfo, FTPStorageInfo ftpStorageInfo)
            : this(taskInfo, ftpStorageInfo, string.Empty)
        {
        }

        public FTPManager(TaskInfo taskInfo, FTPStorageInfo ftpStorageInfo, string relatedPaths)
		{
            this.ftpStorageInfo = ftpStorageInfo;
            this.isEnabled = false;
            this.taskInfo = taskInfo;
            try
            {
                LoadFtpLib(relatedPaths);
            }
            catch
            {
                LoadFtpLib(relatedPaths);
            }
            this.isEnabled = true;
		}

        public FTPStorageInfo FTPStorageInfo
        {
            get { return this.ftpStorageInfo; }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
        }

        private void LoadFtpLib(string relatedPath)
        {
            this.ftplib = new FTP(this.ftpStorageInfo.Server, this.ftpStorageInfo.Port, this.ftpStorageInfo.UserName, this.ftpStorageInfo.Password, this.ftpStorageInfo.IsPassiveMode);

            this.ftplib.Connect();
            if (!string.IsNullOrEmpty(relatedPath))
            {
                relatedPath = relatedPath.Trim('\\', '/');
                relatedPath = relatedPath.Replace('\\', '/');
                string[] arr = relatedPath.Split('/');
                foreach (string directoryName in arr)
                {
                    bool isExists = this.IsExists(directoryName);
                    if (!isExists)
                    {
                        this.ftplib.MakeDir(directoryName);
                    }
                    this.ftplib.ChangeDir(directoryName);
                }
            }
        }

        public void ChangeDirDownByRelatedPath(string relatedPath)
        {
            if (!this.IsEnabled) return;
            try
            {
                if (!string.IsNullOrEmpty(relatedPath))
                {
                    relatedPath = relatedPath.Trim(new char[] { '@', '~', '/', '\\' });
                    if (!string.IsNullOrEmpty(relatedPath))
                    {
                        relatedPath = relatedPath.Replace('\\', '/');
                        string[] directoryNames = relatedPath.Split('/');
                        foreach (string directoryName in directoryNames)
                        {
                            this.ChangeDir(directoryName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.AddErrorLog(string.Format("转到目录:{0}", relatedPath), ex);
            }
        }

        public void ChangeDirUpByRelatedPath(string relatedPath)
        {
            if (!this.IsEnabled) return;
            try
            {
                if (!string.IsNullOrEmpty(relatedPath))
                {
                    relatedPath = relatedPath.Trim(new char[] { '@', '~', '/', '\\' });
                    if (!string.IsNullOrEmpty(relatedPath))
                    {
                        relatedPath = relatedPath.Replace('\\', '/');
                        string[] directoryNames = relatedPath.Split('/');
                        for (int i = 0; i < directoryNames.Length; i++)
                        {
                            this.ChangeDirToParent();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.AddErrorLog(string.Format("转到目录:{0}", relatedPath), ex);
            }
        }

        public void ChangeDir(string directoryName)
        {
            if (!this.IsEnabled) return;
            try
            {
                bool isExists = this.IsExists(directoryName);
                if (!isExists)
                {
                    this.ftplib.MakeDir(directoryName);
                }
                this.ftplib.ChangeDir(directoryName);
            }
            catch (Exception ex)
            {
                this.AddErrorLog(string.Format("转到目录:{0}", directoryName), ex);
            }
        }

        private bool IsExists(string directoryName)
        {
            bool isExists = false;

            ArrayList directoryArrayList = this.ftplib.ListDirectories();
            foreach (string directoryString in directoryArrayList)
            {
                FTPFileSystemInfo fileSystemInfo = new FTPFileSystemInfo(directoryString);
                if (fileSystemInfo.IsDirectory)
                {
                    if (StringUtils.EqualsIgnoreCase(directoryName, fileSystemInfo.Name))
                    {
                        isExists = true;
                        break;
                    }
                }
            }
            return isExists;
        }

        public void ChangeDirToParent()
        {
            if (!this.IsEnabled) return;
            try
            {
                this.ftplib.ChangeDir("..");
            }
            catch (Exception ex)
            {
                this.AddErrorLog("转到上级目录", ex);
            }
        }

        public void CreateDirectory(string directoryName)
        {
            if (!this.IsEnabled) return;
            try
            {
                this.ftplib.MakeDir(directoryName);
            }
            catch (Exception ex)
            {
                this.AddErrorLog(string.Format("创建目录:{0}", directoryName), ex);
            }
            
        }

        /// <summary>
        /// 上传指定路径下的指定文件夹
        /// </summary>
        /// <param name="directoryPath">指定的路径</param>
        /// <param name="directoryName">指定的文件夹</param>
        public void UploadDirectory(string directoryPath, string directoryName)
        {
            if (!this.IsEnabled) return;
            this.ChangeDir(directoryName);
            string currentDirectoryPath = PathUtils.Combine(directoryPath, directoryName);
            string[] fileNames = DirectoryUtils.GetFileNames(currentDirectoryPath);
            this.UploadFiles(fileNames, currentDirectoryPath);

            string[] childDirectoryNames = DirectoryUtils.GetDirectoryNames(currentDirectoryPath);
            foreach (string childDirectoryName in childDirectoryNames)
            {
                this.UploadDirectory(currentDirectoryPath, childDirectoryName);
            }
            this.ChangeDirToParent();
        }

        public void UploadDirectory(string currentDirectoryPath)
        {
            if (!this.IsEnabled) return;
            string directoryName = PathUtils.GetDirectoryName(currentDirectoryPath);
            this.ChangeDir(directoryName);
            string[] fileNames = DirectoryUtils.GetFileNames(currentDirectoryPath);
            this.UploadFiles(fileNames, currentDirectoryPath);

            string[] childDirectoryNames = DirectoryUtils.GetDirectoryNames(currentDirectoryPath);
            foreach (string childDirectoryName in childDirectoryNames)
            {
                this.UploadDirectory(currentDirectoryPath, childDirectoryName);
            }
            this.ChangeDirToParent();
        }

        public void UploadFiles(string[] fileNames, string currentDirectoryPath)
        {
            if (!this.IsEnabled) return;
            if (fileNames != null && fileNames.Length > 0)
            {
                foreach (string fileName in fileNames)
                {
                    string filePath = PathUtils.Combine(currentDirectoryPath, fileName);
                    try
                    {
                        //int perc = 0;

                        // open the file with resume support if it already exists, the last 
                        // peram should be false for no resume
                        this.ftplib.OpenUpload(filePath, fileName);
                        while (this.ftplib.DoUpload() > 0)
                        {
                            //perc = (int)((this.ftplib.BytesTotal * 100) / this.ftplib.FileSize);
                            //Console.Write("\rDownloading: {0}/{1} {2}%",
                            //  this.ftplib.BytesTotal, this.ftplib.FileSize, perc);
                            //Console.Out.Flush();
                        }
                        //Console.WriteLine("");
                    }
                    catch (Exception ex)
                    {
                        this.AddErrorLog(string.Format("上传文件:{0}", filePath), ex);
                    }
                }
            }
        }

        public bool UploadFile(string filePath)
        {
            if (!this.IsEnabled) return false;
            try
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    // open the file with resume support if it already exists, the last 
                    // peram should be false for no resume
                    string fileName = PathUtils.GetFileName(filePath);
                    this.ftplib.OpenUpload(filePath, fileName);
                    while (this.ftplib.DoUpload() > 0)
                    {
                        //perc = (int)((this.ftplib.BytesTotal * 100) / this.ftplib.FileSize);
                        //Console.Write("\rDownloading: {0}/{1} {2}%",
                        //  this.ftplib.BytesTotal, this.ftplib.FileSize, perc);
                        //Console.Out.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                this.AddErrorLog(string.Format("上传文件:{0}", filePath), ex);
                return false;
            }
            return true;
        }

        public void DownloadFiles(string[] fileNames, string currentDirectoryPath)
        {
            if (!this.IsEnabled) return;
            
            if (fileNames != null && fileNames.Length > 0)
            {
                foreach (string fileName in fileNames)
                {
                    string filePath = PathUtils.Combine(currentDirectoryPath, fileName);
                    try
                    {
                        //int perc = 0;

                        // open the file with resume support if it already exists, the last 
                        // peram should be false for no resume
                        this.ftplib.OpenDownload(fileName, filePath);
                        while (this.ftplib.DoDownload() > 0)
                        {
                            //perc = (int)((this.ftplib.BytesTotal * 100) / this.ftplib.FileSize);
                            //Console.Write("\rDownloading: {0}/{1} {2}%",
                            //  this.ftplib.BytesTotal, this.ftplib.FileSize, perc);
                            //Console.Out.Flush();
                        }
                        //Console.WriteLine("");
                    }
                    catch (Exception ex)
                    {
                        this.AddErrorLog(string.Format("下载文件:{0}", filePath), ex);
                    }
                }
            }
            
        }

        public void DownloadDirectory(string directoryPath, string directoryName)
        {
            if (!this.IsEnabled) return;
            this.ftplib.ChangeDir(directoryName);
            string currentDirectoryPath = PathUtils.Combine(directoryPath, directoryName);
            DirectoryUtils.CreateDirectoryIfNotExists(currentDirectoryPath);

            ArrayList fileStrings = this.ftplib.ListFiles();
            string[] fileNames = new string[fileStrings.Count];
            int i = 0;
            foreach (string fileString in fileStrings)
            {
                FTPFileSystemInfo fileSystemInfo = new FTPFileSystemInfo(fileString);
                fileNames[i++] = fileSystemInfo.Name;
            }
            this.DownloadFiles(fileNames, currentDirectoryPath);

            ArrayList directoryStrings = this.ftplib.ListDirectories();
            foreach (string directoryString in directoryStrings)
            {
                FTPFileSystemInfo fileSystemInfo = new FTPFileSystemInfo(directoryString);
                if (fileSystemInfo.Name == "." || fileSystemInfo.Name == "..") continue;
                this.DownloadDirectory(currentDirectoryPath, fileSystemInfo.Name);
            }
            this.ChangeDirToParent();
        }

        public void RemoveDirs(string dir)
        {
            this.ftplib.RemoveDirs(dir);
        }

        public void RemoveFile(string filename)
        {
            this.ftplib.RemoveFile(filename);
        }

        public string GetWorkingDirectory()
        {
            return this.ftplib.GetWorkingDirectory();
        }

        public ArrayList ListDirectories()
        {
            return this.ftplib.ListDirectories();
        }

        public ArrayList ListFiles()
        {
            return this.ftplib.ListFiles();
        }

        public bool CopyFrom(string sourcePath)
        {
            if (!this.IsEnabled) return false;
            try
            {
                if (PathUtils.IsDirectoryPath(sourcePath))
                {
                    this.UploadDirectory(sourcePath);
                }
                else
                {
                    return this.UploadFile(sourcePath);
                }
                return true;
            }
            catch (Exception ex)
            {
                this.AddErrorLog(string.Format("复制文件:{0}", sourcePath), ex);
            }
            return false;
        }

        public bool WriteText(string fileName, ECharset charset, string content)
        {
            if (!this.IsEnabled) return false;
            try
            {
                string filePath = PathUtils.GetTemporaryFilesPath(fileName);
                FileUtils.WriteText(filePath, charset, content);
                return this.UploadFile(filePath);
            }
            catch (Exception ex)
            {
                this.AddErrorLog(string.Format("新建文件:{0}", fileName), ex);
            }
            return false;
        }

        public void AddErrorLog(string action, Exception ex)
        {
            if (this.taskInfo != null && ex != null)
            {
                TaskLogInfo logInfo = new TaskLogInfo(0, taskInfo.TaskID, false, action + " " + ex.Message, ex.StackTrace, string.Empty, DateTime.Now);
                BaiRongDataProvider.TaskLogDAO.Insert(logInfo);
            }
        }
	}
}
