using System;
using BaiRong.Core;
using BaiRong.Core.IO;
using SiteServer.CMS.Model;
using System.Collections;
using BaiRong.Core.Data.Provider;
using System.IO;

namespace SiteServer.CMS.Core
{
    public class DirectoryUtility
    {
        public static string GetIndexesDirectoryPath(string siteFilesDirectoryPath)
        {
            return PathUtils.Combine(siteFilesDirectoryPath, "Indexes");
        }

        public static void ChangePublishmentSystemDir(string parentPSPath, string oldPSDir, string newPSDir)
        {
            string oldPSPath = PathUtils.Combine(parentPSPath, oldPSDir);
            string newPSPath = PathUtils.Combine(parentPSPath, newPSDir);
            if (DirectoryUtils.IsDirectoryExists(newPSPath))
            {
                throw new ArgumentException("发布系统修改失败，发布路径文件夹已存在！");
            }
            if (DirectoryUtils.IsDirectoryExists(oldPSPath))
            {
                DirectoryUtils.MoveDirectory(oldPSPath, newPSPath, false);
            }
            else
            {
                DirectoryUtils.CreateDirectoryIfNotExists(newPSPath);
            }
        }

        public static void DeletePublishmentSystemFiles(PublishmentSystemInfo publishmentSystemInfo)
        {
            string publishmentSystemPath = PathUtility.GetPublishmentSystemPath(publishmentSystemInfo);

            if (publishmentSystemInfo.IsHeadquarters)
            {
                string[] filePaths = DirectoryUtils.GetFilePaths(publishmentSystemPath);
                foreach (string filePath in filePaths)
                {
                    string fileName = PathUtils.GetFileName(filePath);
                    if (!PathUtility.IsSystemFile(fileName))
                    {
                        FileUtils.DeleteFileIfExists(filePath);
                    }
                }

                ArrayList publishmentSystemDirArrayList = DataProvider.PublishmentSystemDAO.GetLowerPublishmentSystemDirArrayListThatNotIsHeadquarters();

                string[] directoryPaths = DirectoryUtils.GetDirectoryPaths(publishmentSystemPath);
                foreach (string subDirectoryPath in directoryPaths)
                {
                    string directoryName = PathUtils.GetDirectoryName(subDirectoryPath);
                    if (!DirectoryUtils.IsSystemDirectory(directoryName) && !publishmentSystemDirArrayList.Contains(directoryName.ToLower()))
                    {
                        DirectoryUtils.DeleteDirectoryIfExists(subDirectoryPath);
                    }
                }
            }
            else
            {
                string direcotryPath = publishmentSystemPath;
                DirectoryUtils.DeleteDirectoryIfExists(direcotryPath);
            }
        }

        public static void ImportPublishmentSystemFiles(PublishmentSystemInfo publishmentSystemInfo, string siteTemplatePath, bool isOverride)
        {
            string publishmentSystemPath = PathUtility.GetPublishmentSystemPath(publishmentSystemInfo);

            if (publishmentSystemInfo.IsHeadquarters)
            {
                string[] filePaths = DirectoryUtils.GetFilePaths(siteTemplatePath);
                foreach (string filePath in filePaths)
                {
                    string fileName = PathUtils.GetFileName(filePath);
                    if (!PathUtility.IsSystemFile(fileName))
                    {
                        string destFilePath = PathUtils.Combine(publishmentSystemPath, fileName);
                        FileUtils.MoveFile(filePath, destFilePath, isOverride);
                    }
                }

                ArrayList publishmentSystemDirArrayList = DataProvider.PublishmentSystemDAO.GetLowerPublishmentSystemDirArrayListThatNotIsHeadquarters();

                string[] directoryPaths = DirectoryUtils.GetDirectoryPaths(siteTemplatePath);
                foreach (string subDirectoryPath in directoryPaths)
                {
                    string directoryName = PathUtils.GetDirectoryName(subDirectoryPath);
                    if (!DirectoryUtils.IsSystemDirectory(directoryName) && !publishmentSystemDirArrayList.Contains(directoryName.ToLower()))
                    {
                        string destDirectoryPath = PathUtils.Combine(publishmentSystemPath, directoryName);
                        DirectoryUtils.MoveDirectory(subDirectoryPath, destDirectoryPath, isOverride);
                    }
                }
            }
            else
            {
                DirectoryUtils.MoveDirectory(siteTemplatePath, publishmentSystemPath, isOverride);
            }
            string siteTemplateMetadataPath = PathUtils.Combine(publishmentSystemPath, DirectoryUtils.SiteTemplates.SiteTemplateMetadata);
            DirectoryUtils.DeleteDirectoryIfExists(siteTemplateMetadataPath);
        }

        public static void ChangeParentPublishmentSystem(int oldParentPublishmentSystemID, int newParentPublishmentSystemID, int publishmentSystemID, string publishmentSystemDir)
        {
            if (oldParentPublishmentSystemID == newParentPublishmentSystemID) return;

            string oldPSPath = string.Empty;
            if (oldParentPublishmentSystemID != 0)
            {
                PublishmentSystemInfo oldPublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(oldParentPublishmentSystemID);

                oldPSPath = PathUtils.Combine(PathUtility.GetPublishmentSystemPath(oldPublishmentSystemInfo), publishmentSystemDir);
            }
            else
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                oldPSPath = PathUtility.GetPublishmentSystemPath(publishmentSystemInfo);
            }

            string newPSPath = string.Empty;
            if (newParentPublishmentSystemID != 0)
            {
                PublishmentSystemInfo newPublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(newParentPublishmentSystemID);

                newPSPath = PathUtils.Combine(PathUtility.GetPublishmentSystemPath(newPublishmentSystemInfo), publishmentSystemDir);
            }
            else
            {
                newPSPath = PathUtils.Combine(ConfigUtils.Instance.PhysicalApplicationPath, publishmentSystemDir);
            }

            if (DirectoryUtils.IsDirectoryExists(newPSPath))
            {
                throw new ArgumentException("发布系统修改失败，发布路径文件夹已存在！");
            }
            if (DirectoryUtils.IsDirectoryExists(oldPSPath))
            {
                DirectoryUtils.MoveDirectory(oldPSPath, newPSPath, false);
            }
            else
            {
                DirectoryUtils.CreateDirectoryIfNotExists(newPSPath);
            }
        }

        public static void DeleteContentsByPage(PublishmentSystemInfo publishmentSystemInfo, ArrayList nodeIDArrayList)
        {
            foreach (int nodeID in nodeIDArrayList)
            {
                string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeID);
                ArrayList contentIDArrayList = BaiRongDataProvider.ContentDAO.GetContentIDArrayList(tableName, nodeID);
                if (contentIDArrayList.Count > 0)
                {
                    foreach (int contentID in contentIDArrayList)
                    {
                        string filePath = PathUtility.GetContentPageFilePath(publishmentSystemInfo, nodeID, contentID, 0);
                        FileUtils.DeleteFileIfExists(filePath);
                        DeletePagingFiles(filePath);
                        DirectoryUtils.DeleteEmptyDirectory(DirectoryUtils.GetDirectoryPath(filePath));
                    }
                }
            }
        }

        public static void DeleteContents(PublishmentSystemInfo publishmentSystemInfo, int nodeID, ArrayList contentIDArrayList)
        {
            foreach (int contentID in contentIDArrayList)
            {
                string filePath = PathUtility.GetContentPageFilePath(publishmentSystemInfo, nodeID, contentID, 0);
                FileUtils.DeleteFileIfExists(filePath);
            }
        }

        public static void DeleteChannels(PublishmentSystemInfo publishmentSystemInfo, ArrayList nodeIDArrayList)
        {
            foreach (int nodeID in nodeIDArrayList)
            {
                string filePath = PathUtility.GetChannelPageFilePath(publishmentSystemInfo, nodeID, 0);

                FileUtils.DeleteFileIfExists(filePath);

                string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeID);
                ArrayList contentIDArrayList = BaiRongDataProvider.ContentDAO.GetContentIDArrayList(tableName, nodeID);
                if (contentIDArrayList.Count > 0)
                {
                    DeleteContents(publishmentSystemInfo, nodeID, contentIDArrayList);
                }
            }
        }

        public static void DeleteChannelsByPage(PublishmentSystemInfo publishmentSystemInfo, ArrayList nodeIDArrayList)
        {
            foreach (int nodeID in nodeIDArrayList)
            {
                if (nodeID != publishmentSystemInfo.PublishmentSystemID)//不删除首页
                {
                    string filePath = PathUtility.GetChannelPageFilePath(publishmentSystemInfo, nodeID, 0);
                    FileUtils.DeleteFileIfExists(filePath);
                    DeletePagingFiles(filePath);
                    DirectoryUtils.DeleteEmptyDirectory(DirectoryUtils.GetDirectoryPath(filePath));
                }
            }
        }

        //删除目标文件的分页文件
        public static void DeletePagingFiles(string filePath)
        {
            string fileName = (new FileInfo(filePath)).Name;
            fileName = fileName.Substring(0, fileName.IndexOf('.'));
            string[] filesPath = DirectoryUtils.GetFilePaths(DirectoryUtils.GetDirectoryPath(filePath));
            foreach (string otherFilePath in filesPath)
            {
                string otherFileName = (new FileInfo(otherFilePath)).Name;
                otherFileName = otherFileName.Substring(0, otherFileName.IndexOf('.'));
                if (otherFileName.Contains(fileName + "_"))
                {
                    string isNum = otherFileName.Replace(fileName + "_", string.Empty);
                    if (ConvertHelper.GetInteger(isNum) > 0)
                    {
                        FileUtils.DeleteFileIfExists(otherFilePath);
                    }
                }
            }
        }

        public static void DeleteFiles(PublishmentSystemInfo publishmentSystemInfo, ArrayList templateIDArrayList)
        {
            foreach (int templateID in templateIDArrayList)
            {
                TemplateInfo templateInfo = TemplateManager.GetTemplateInfo(publishmentSystemInfo.PublishmentSystemID, templateID);
                if (templateInfo == null || templateInfo.TemplateType != ETemplateType.FileTemplate)
                {
                    return;
                }

                string filePath = PathUtility.MapPath(publishmentSystemInfo, templateInfo.CreatedFileFullName);

                FileUtils.DeleteFileIfExists(filePath);
            }
        }

        public static void ChangeToHeadquarters(PublishmentSystemInfo publishmentSystemInfo, bool isMoveFiles)
        {
            if (publishmentSystemInfo.IsHeadquarters == false)
            {
                string publishmentSystemPath = PathUtility.GetPublishmentSystemPath(publishmentSystemInfo);

                DataProvider.PublishmentSystemDAO.UpdateParentPublishmentSystemIDToZero(publishmentSystemInfo.PublishmentSystemID);

                publishmentSystemInfo.IsHeadquarters = true;
                publishmentSystemInfo.PublishmentSystemDir = string.Empty;
                publishmentSystemInfo.PublishmentSystemUrl = ConfigUtils.Instance.ApplicationPath;

                DataProvider.PublishmentSystemDAO.Update(publishmentSystemInfo);
                if (isMoveFiles)
                {
                    DirectoryUtils.MoveDirectory(publishmentSystemPath, ConfigUtils.Instance.PhysicalApplicationPath, false);
                    DirectoryUtils.DeleteDirectoryIfExists(publishmentSystemPath);
                }
            }
        }

        public static void ChangeToSubSite(PublishmentSystemInfo publishmentSystemInfo, string psDir, ArrayList fileSystemNameArrayList)
        {
            if (publishmentSystemInfo.IsHeadquarters)
            {
                publishmentSystemInfo.IsHeadquarters = false;
                publishmentSystemInfo.PublishmentSystemDir = psDir.Trim();
                publishmentSystemInfo.PublishmentSystemUrl = PageUtils.Combine(ConfigUtils.Instance.ApplicationPath, psDir.Trim());

                DataProvider.PublishmentSystemDAO.Update(publishmentSystemInfo);

                string psPath = PathUtils.Combine(ConfigUtils.Instance.PhysicalApplicationPath, psDir);
                DirectoryUtils.CreateDirectoryIfNotExists(psPath);
                if (fileSystemNameArrayList != null && fileSystemNameArrayList.Count > 0)
                {
                    foreach (string fileSystemName in fileSystemNameArrayList)
                    {
                        string srcPath = PathUtils.Combine(ConfigUtils.Instance.PhysicalApplicationPath, fileSystemName);
                        if (DirectoryUtils.IsDirectoryExists(srcPath))
                        {
                            string destDirectoryPath = PathUtils.Combine(psPath, fileSystemName);
                            DirectoryUtils.CreateDirectoryIfNotExists(destDirectoryPath);
                            DirectoryUtils.MoveDirectory(srcPath, destDirectoryPath, false);
                            DirectoryUtils.DeleteDirectoryIfExists(srcPath);
                        }
                        else if (FileUtils.IsFileExists(srcPath))
                        {
                            FileUtils.CopyFile(srcPath, PathUtils.Combine(psPath, fileSystemName));
                            FileUtils.DeleteFileIfExists(srcPath);
                        }
                    }
                }
            }
        }

        public static string GetBlogSystemPath(PublishmentSystemInfo publishmentSystemInfo, string blogSystemDir)
        {
            string publishmentSystemPath = PathUtility.GetPublishmentSystemPath(publishmentSystemInfo);

            return PathUtils.Combine(publishmentSystemPath, blogSystemDir);
        }
    }
}
