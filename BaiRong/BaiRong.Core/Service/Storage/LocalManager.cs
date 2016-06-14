using System;
using System.Collections;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Core.IO.FileManagement;
using BaiRong.Core.Net;
using System.IO;
using BaiRong.Model;
using BaiRong.Model.Service;
using BaiRong.Core.Data.Provider;

namespace BaiRong.Core.Service
{
    public class LocalManager : IStorageManager
	{
        private readonly TaskInfo taskInfo;
        private readonly string homeDirectoryPath;
        private string directoryPath;
        private bool isEnabled;

        public LocalManager(TaskInfo taskInfo, string homeDirectoryPath)
            : this(taskInfo, homeDirectoryPath, string.Empty)
        {
        }

        public LocalManager(TaskInfo taskInfo, string homeDirectoryPath, string relatedPaths)
		{
            this.taskInfo = taskInfo;
            this.homeDirectoryPath = homeDirectoryPath;
            this.isEnabled = false;

            this.directoryPath = PathUtils.Combine(this.homeDirectoryPath, relatedPaths);
            if (!Directory.Exists(this.directoryPath))
            {
                Directory.CreateDirectory(this.directoryPath);
            }
            this.isEnabled = true;
		}

        public bool IsEnabled
        {
            get { return this.isEnabled; }
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
                this.directoryPath = PathUtils.Combine(this.directoryPath, directoryName);
                DirectoryUtils.CreateDirectoryIfNotExists(this.directoryPath);
            }
            catch (Exception ex)
            {
                this.AddErrorLog(string.Format("转到目录:{0}", PathUtils.Combine(this.directoryPath, directoryName)), ex);
            }
        }

        public void ChangeDirToParent()
        {
            if (!this.IsEnabled) return;
            try
            {
                string[] paths = this.directoryPath.Split(Path.DirectorySeparatorChar);
                if (paths.Length > 1)
                {
                    int count = 0;
                    string dirPath = string.Empty;
                    foreach (string path in paths)
                    {
                        count++;
                        if (paths.Length == count) break;
                        dirPath = PathUtils.Combine(dirPath, path);
                    }
                    this.directoryPath = dirPath;
                }
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
                DirectoryUtils.CreateDirectoryIfNotExists(PathUtils.Combine(this.directoryPath, directoryName));
            }
            catch (Exception ex)
            {
                this.AddErrorLog(string.Format("创建目录:{0}", PathUtils.Combine(this.directoryPath, directoryName)), ex);
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
                        string destFilePath = PathUtils.Combine(this.directoryPath, fileName);
                        FileUtils.CopyFile(filePath, destFilePath, true);
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

                    string destFilePath = PathUtils.Combine(this.directoryPath, fileName);
                    FileUtils.CopyFile(filePath, destFilePath, true);
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
                        string sourFilePath = PathUtils.Combine(this.directoryPath, fileName);
                        FileUtils.CopyFile(sourFilePath, filePath);
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
            this.ChangeDir(directoryName);
            string currentDirectoryPath = PathUtils.Combine(directoryPath, directoryName);
            DirectoryUtils.CreateDirectoryIfNotExists(currentDirectoryPath);

            ArrayList fileStrings = this.ListFiles();
            string[] fileNames = new string[fileStrings.Count];
            int i = 0;
            foreach (string fileString in fileStrings)
            {
                fileNames[i++] = PathUtils.GetFileName(fileString);
            }
            this.DownloadFiles(fileNames, currentDirectoryPath);

            ArrayList directoryStrings = this.ListDirectories();
            foreach (string directoryString in directoryStrings)
            {
                string d = PathUtils.GetDirectoryName(directoryString);
                if (d == "." || d == "..") continue;
                this.DownloadDirectory(currentDirectoryPath, d);
            }
            this.ChangeDirToParent();
        }

        public void RemoveDirs(string dir)
        {
            DirectoryUtils.DeleteDirectoryIfExists(PathUtils.Combine(this.directoryPath, dir));
        }

        public void RemoveFile(string filename)
        {
            FileUtils.DeleteFileIfExists(PathUtils.Combine(this.directoryPath, filename));
        }

        public string GetWorkingDirectory()
        {
            return this.directoryPath;
        }

        public ArrayList ListDirectories()
        {
            return TranslateUtils.StringArrayToArrayList(DirectoryUtils.GetDirectoryPaths(this.directoryPath));
        }

        public ArrayList ListFiles()
        {
            return TranslateUtils.StringArrayToArrayList(DirectoryUtils.GetFilePaths(this.directoryPath));
        }

        public bool CopyFrom(string sourcePath)
        {
            if (!this.IsEnabled) return false;
            try
            {
                if (PathUtils.IsDirectoryPath(sourcePath))
                {
                    DirectoryUtils.Copy(sourcePath, this.directoryPath);
                }
                else
                {
                    string fileName = PathUtils.GetFileName(sourcePath);
                    FileUtils.CopyFile(sourcePath, PathUtils.Combine(this.directoryPath, fileName));
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
                FileUtils.WriteText(PathUtils.Combine(this.directoryPath, fileName), charset, content);
                return true;
            }
            catch (Exception ex)
            {
                this.AddErrorLog(string.Format("新建文件:{0}", PathUtils.Combine(this.directoryPath, fileName)), ex);
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
