using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using BaiRong.Core;
using BaiRong.Core.Net;
using BaiRong.Model;
using BaiRong.Core.IO;
using SiteServer.CMS.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.IO.FileManagement;
using BaiRong.Core.AuxiliaryTable;
using System.Text.RegularExpressions;

namespace SiteServer.CMS.Core
{
    public class PathUtility
    {
        private PathUtility()
        {
        }

        public static string GetPublishmentSystemPath(PublishmentSystemInfo publishmentSystemInfo)
        {
            return PathUtils.Combine(ConfigUtils.Instance.PhysicalApplicationPath, publishmentSystemInfo.PublishmentSystemDir);
        }

        public static string GetPublishmentSystemPath(int publishmentSystemID, params string[] paths)
        {
            return PathUtility.GetPublishmentSystemPath(PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID), paths);
        }

        public static string GetPublishmentSystemPath(PublishmentSystemInfo publishmentSystemInfo, params string[] paths)
        {
            string retval = PathUtility.GetPublishmentSystemPath(publishmentSystemInfo);
            if (paths != null && paths.Length > 0)
            {
                for (int i = 0; i < paths.Length; i++)
                {
                    string path = (paths[i] != null) ? paths[i].Replace(PageUtils.SeparatorChar, PathUtils.SeparatorChar).Trim(PathUtils.SeparatorChar) : string.Empty;
                    retval = PathUtils.Combine(retval, path);
                }
            }
            return retval;
        }

        public static string GetIndexPageFilePath(PublishmentSystemInfo publishmentSystemInfo, string createFileFullName, bool isHeadquarters)
        {
            string filePath;

            if (isHeadquarters)
            {
                if (createFileFullName.StartsWith("@"))
                {
                    createFileFullName = "~" + createFileFullName.Substring(1);
                }
                else if (!createFileFullName.StartsWith("~"))
                {
                    createFileFullName = "~" + createFileFullName;
                }
            }
            else
            {
                if (!createFileFullName.StartsWith("~") && !createFileFullName.StartsWith("@"))
                {
                    createFileFullName = "@" + createFileFullName;
                }
            }
            filePath = PathUtility.MapPath(publishmentSystemInfo, createFileFullName);
            return filePath;
        }

        public static string GetBackupFilePath(PublishmentSystemInfo publishmentSystemInfo, EBackupType backupType)
        {
            string extention = ".zip";
            string siteName = publishmentSystemInfo.PublishmentSystemDir;
            if (!string.IsNullOrEmpty(siteName))
            {
                siteName += "_";
            }
            if (backupType == EBackupType.Templates)
            {
                extention = ".xml";
            }
            return PathUtils.Combine(PathUtils.PhysicalSiteFilesPath, DirectoryUtils.SiteFiles.BackupFiles, publishmentSystemInfo.PublishmentSystemDir, DateTime.Now.ToString("yyyy-MM"), EBackupTypeUtils.GetValue(backupType) + "_" + siteName + DateTime.Now.ToString("yyyy-MM-dd-HH-mm") + extention);
        }

        public static string GetUploadDirectoryPath(PublishmentSystemInfo publishmentSystemInfo, string fileExtension)
        {
            return GetUploadDirectoryPath(publishmentSystemInfo, DateTime.Now, fileExtension);
        }

        public static string GetUploadDirectoryPath(PublishmentSystemInfo publishmentSystemInfo, DateTime datetime, string fileExtension)
        {
            string uploadDateFormatString = publishmentSystemInfo.Additional.FileUploadDateFormatString;
            string uploadDirectoryName = publishmentSystemInfo.Additional.FileUploadDirectoryName;

            if (PathUtility.IsImageExtenstionAllowed(publishmentSystemInfo, fileExtension))
            {
                uploadDateFormatString = publishmentSystemInfo.Additional.ImageUploadDateFormatString;
                uploadDirectoryName = publishmentSystemInfo.Additional.ImageUploadDirectoryName;
            }
            else if (PathUtility.IsVideoExtenstionAllowed(publishmentSystemInfo, fileExtension))
            {
                uploadDateFormatString = publishmentSystemInfo.Additional.VideoUploadDateFormatString;
                uploadDirectoryName = publishmentSystemInfo.Additional.VideoUploadDirectoryName;
            }

            string directoryPath;
            EDateFormatType dateFormatType = EDateFormatTypeUtils.GetEnumType(uploadDateFormatString);
            string publishmentSystemPath = PathUtility.GetPublishmentSystemPath(publishmentSystemInfo);
            if (dateFormatType == EDateFormatType.Year)
            {
                directoryPath = PathUtils.Combine(publishmentSystemPath, uploadDirectoryName, datetime.Year.ToString());
            }
            else if (dateFormatType == EDateFormatType.Day)
            {
                directoryPath = PathUtils.Combine(publishmentSystemPath, uploadDirectoryName, datetime.Year.ToString(), datetime.Month.ToString(), datetime.Day.ToString());
            }
            else
            {
                directoryPath = PathUtils.Combine(publishmentSystemPath, uploadDirectoryName, datetime.Year.ToString(), datetime.Month.ToString());
            }
            DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);
            return directoryPath;
        }

        public static string GetUploadDirectoryPath(PublishmentSystemInfo publishmentSystemInfo, EUploadType uploadType)
        {
            return GetUploadDirectoryPath(publishmentSystemInfo, DateTime.Now, uploadType);
        }

        public static string GetUploadDirectoryPath(PublishmentSystemInfo publishmentSystemInfo, DateTime datetime, EUploadType uploadType)
        {
            string uploadDateFormatString = string.Empty;
            string uploadDirectoryName = string.Empty;

            if (uploadType == EUploadType.Image)
            {
                uploadDateFormatString = publishmentSystemInfo.Additional.ImageUploadDateFormatString;
                uploadDirectoryName = publishmentSystemInfo.Additional.ImageUploadDirectoryName;
            }
            else if (uploadType == EUploadType.Video)
            {
                uploadDateFormatString = publishmentSystemInfo.Additional.VideoUploadDateFormatString;
                uploadDirectoryName = publishmentSystemInfo.Additional.VideoUploadDirectoryName;
            }
            else if (uploadType == EUploadType.File)
            {
                uploadDateFormatString = publishmentSystemInfo.Additional.FileUploadDateFormatString;
                uploadDirectoryName = publishmentSystemInfo.Additional.FileUploadDirectoryName;
            }
            else if (uploadType == EUploadType.Special)
            {
                uploadDateFormatString = publishmentSystemInfo.Additional.FileUploadDateFormatString;
                uploadDirectoryName = "/Special";
            }
            else if (uploadType == EUploadType.AdvImage)
            {
                uploadDateFormatString = publishmentSystemInfo.Additional.FileUploadDateFormatString;
                uploadDirectoryName = "/AdvImage";
            }

            string directoryPath;
            EDateFormatType dateFormatType = EDateFormatTypeUtils.GetEnumType(uploadDateFormatString);
            string publishmentSystemPath = PathUtility.GetPublishmentSystemPath(publishmentSystemInfo);
            if (dateFormatType == EDateFormatType.Year)
            {
                directoryPath = PathUtils.Combine(publishmentSystemPath, uploadDirectoryName, datetime.Year.ToString());
            }
            else if (dateFormatType == EDateFormatType.Day)
            {
                directoryPath = PathUtils.Combine(publishmentSystemPath, uploadDirectoryName, datetime.Year.ToString(), datetime.Month.ToString(), datetime.Day.ToString());
            }
            else
            {
                directoryPath = PathUtils.Combine(publishmentSystemPath, uploadDirectoryName, datetime.Year.ToString(), datetime.Month.ToString());
            }
            DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);
            return directoryPath;
        }

        public static string GetUploadFileName(PublishmentSystemInfo publishmentSystemInfo, string filePath)
        {
            return GetUploadFileName(publishmentSystemInfo, filePath, DateTime.Now);
        }

        public static string GetUploadFileName(PublishmentSystemInfo publishmentSystemInfo, string filePath, DateTime now)
        {
            string fileExtension = PathUtils.GetExtension(filePath);

            bool isUploadChangeFileName = publishmentSystemInfo.Additional.IsFileUploadChangeFileName;
            if (PathUtility.IsImageExtenstionAllowed(publishmentSystemInfo, fileExtension))
            {
                isUploadChangeFileName = publishmentSystemInfo.Additional.IsImageUploadChangeFileName;
            }
            else if (PathUtility.IsVideoExtenstionAllowed(publishmentSystemInfo, fileExtension))
            {
                isUploadChangeFileName = publishmentSystemInfo.Additional.IsVideoUploadChangeFileName;
            }

            return GetUploadFileName(publishmentSystemInfo, filePath, now, isUploadChangeFileName);
        }

        public static string GetUploadFileName(PublishmentSystemInfo publishmentSystemInfo, string filePath, DateTime now, bool isUploadChangeFileName)
        {
            string retval = string.Empty;

            if (isUploadChangeFileName)
            {
                string strDateTime = string.Format("{0}{1}{2}{3}{4}", now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);
                retval = string.Format("{0}{1}", strDateTime, PathUtils.GetExtension(filePath));
            }
            else
            {
                retval = PathUtils.GetFileName(filePath);
            }

            retval = StringUtils.ReplaceIgnoreCase(retval, "as", string.Empty);
            retval = StringUtils.ReplaceIgnoreCase(retval, ";", string.Empty);
            return retval;
        }

        public static string GetUploadSpecialName(PublishmentSystemInfo publishmentSystemInfo, string filePath, DateTime now, bool isUploadChangeFileName)
        {
            string retval = string.Empty;

            if (isUploadChangeFileName)
            {
                string strDateTime = string.Format("{0}{1}{2}{3}{4}", now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);
                retval = string.Format("{0}{1}", strDateTime, PathUtils.GetExtension(filePath));
            }
            else
            {
                retval = PathUtils.GetFileName(filePath);
            }

            retval = StringUtils.ReplaceIgnoreCase(retval, "as", string.Empty);
            retval = StringUtils.ReplaceIgnoreCase(retval, ";", string.Empty);
            return retval;
        }

        public static string GetUploadAdvImageName(PublishmentSystemInfo publishmentSystemInfo, string filePath, DateTime now, bool isUploadChangeFileName)
        {
            string retval = string.Empty;

            if (isUploadChangeFileName)
            {
                string strDateTime = string.Format("{0}{1}{2}{3}{4}", now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);
                retval = string.Format("{0}{1}", strDateTime, PathUtils.GetExtension(filePath));
            }
            else
            {
                retval = PathUtils.GetFileName(filePath);
            }

            retval = StringUtils.ReplaceIgnoreCase(retval, "as", string.Empty);
            retval = StringUtils.ReplaceIgnoreCase(retval, ";", string.Empty);
            return retval;
        }

        public static string GetSiteDir(string path)
        {
            string siteDir = string.Empty;
            string directoryPath = DirectoryUtils.GetDirectoryPath(path).ToLower().Trim(' ', '/', '\\');
            string applicationPath = ConfigUtils.Instance.PhysicalApplicationPath.ToLower().Trim(' ', '/', '\\');
            string directoryDir = StringUtils.ReplaceStartsWith(directoryPath, applicationPath, string.Empty).Trim(' ', '/', '\\');
            if (directoryDir == string.Empty)
            {
                return string.Empty;
            }

            ArrayList publishmentSystemArrayList = PublishmentSystemManager.GetPublishmentSystemInfoDictionaryEntryArrayList();
            foreach (DictionaryEntry entry in publishmentSystemArrayList)
            {
                PublishmentSystemInfo publishmentSystemInfo = entry.Value as PublishmentSystemInfo;
                if (publishmentSystemInfo != null)
                {
                    if (publishmentSystemInfo.IsHeadquarters == false)
                    {
                        if (StringUtils.Contains(directoryDir, publishmentSystemInfo.PublishmentSystemDir.ToLower()))
                        {
                            siteDir = publishmentSystemInfo.PublishmentSystemDir;
                        }
                    }
                }
            }

            //string[] directoryPaths = DirectoryUtils.GetDirectoryPaths(ConfigUtils.Instance.PhysicalApplicationPath);

            //foreach (string siteDirectoryPath in directoryPaths)
            //{
            //    if (DirectoryUtils.IsInDirectory(siteDirectoryPath, directoryPath))
            //    {
            //        siteDir = PathUtils.GetDirectoryName(siteDirectoryPath);
            //        break;
            //    }
            //}

            return PathUtils.GetDirectoryName(siteDir);
        }

        public static string GetCurrentSiteDir()
        {
            return GetSiteDir(PathUtils.GetCurrentPagePath());
        }

        public static int GetCurrentPublishmentSystemID()
        {
            int publishmentSystemID;
            ArrayList publishmentSystemIDArrayList = PublishmentSystemManager.GetPublishmentSystemIDArrayList();
            if (publishmentSystemIDArrayList.Count == 1)
            {
                publishmentSystemID = (int)publishmentSystemIDArrayList[0];
            }
            else
            {
                string publishmentSystemDir = PathUtility.GetCurrentSiteDir();
                if (!string.IsNullOrEmpty(publishmentSystemDir))
                {
                    publishmentSystemID = DataProvider.PublishmentSystemDAO.GetPublishmentSystemIDByPublishmentSystemDir(publishmentSystemDir);
                }
                else
                {
                    publishmentSystemID = DataProvider.PublishmentSystemDAO.GetPublishmentSystemIDByIsHeadquarters();
                }

                if (publishmentSystemID == 0)
                {
                    publishmentSystemID = DataProvider.PublishmentSystemDAO.GetPublishmentSystemIDByIsHeadquarters();
                }
            }
            return publishmentSystemID;
        }

        public static string AddVirtualToPath(string path)
        {
            string resolvedPath = path;
            if (!string.IsNullOrEmpty(path))
            {
                path.Replace("../", string.Empty);
                if (!path.StartsWith("@") && !path.StartsWith("~"))
                {
                    resolvedPath = "@" + path;
                }
            }
            return resolvedPath;
        }

        public static string GetAdminDirectoryPath(string relatedPath)
        {
            relatedPath = PathUtils.RemoveParentPath(relatedPath);
            return PathUtils.Combine(ConfigUtils.Instance.PhysicalApplicationPath, FileConfigManager.Instance.AdminDirectoryName, relatedPath);
        }

        public static string MapPath(PublishmentSystemInfo publishmentSystemInfo, string virtualPath)
        {
            string resolvedPath = virtualPath;
            if (string.IsNullOrEmpty(virtualPath))
            {
                virtualPath = "@";
            }
            if (!virtualPath.StartsWith("@") && !virtualPath.StartsWith("~"))
            {
                virtualPath = "@" + virtualPath;
            }
            if (virtualPath.StartsWith("@"))
            {
                if (publishmentSystemInfo != null)
                {
                    if (publishmentSystemInfo.IsHeadquarters)
                    {
                        resolvedPath = string.Concat("~", virtualPath.Substring(1));
                    }
                    else
                    {
                        resolvedPath = PageUtils.Combine(publishmentSystemInfo.PublishmentSystemDir, virtualPath.Substring(1));
                    }
                }
            }
            return PathUtils.MapPath(resolvedPath);
        }

        public static string MapPath(PublishmentSystemInfo publishmentSystemInfo, string virtualPath, bool isCopyToSite)
        {
            if (isCopyToSite)
            {
                string resolvedPath = virtualPath;
                if (string.IsNullOrEmpty(virtualPath))
                {
                    virtualPath = "@";
                }
                if (!virtualPath.StartsWith("@") && !virtualPath.StartsWith("~"))
                {
                    virtualPath = "@" + virtualPath;
                }
                if (virtualPath.StartsWith("@"))
                {
                    if (publishmentSystemInfo != null)
                    {
                        if (publishmentSystemInfo.IsHeadquarters)
                        {
                            resolvedPath = string.Concat("~", virtualPath.Substring(1));
                        }
                        else
                        {
                            if (publishmentSystemInfo.Additional.FuncFilesType == EFuncFilesType.Direct
                                || publishmentSystemInfo.Additional.FuncFilesType == EFuncFilesType.CrossDomain)
                            {
                                resolvedPath = PageUtils.Combine(publishmentSystemInfo.PublishmentSystemDir, virtualPath.Substring(1));
                            }
                            else if (publishmentSystemInfo.Additional.FuncFilesType == EFuncFilesType.CopyToSite)
                            {
                                //如果是赋值到站内，那么根目录就是子站点的目录
                                resolvedPath = virtualPath.Substring(1);
                            }

                        }
                    }
                }
                return PathUtils.MapPath(resolvedPath);
            }
            else
            {
                return MapPath(publishmentSystemInfo, virtualPath);
            }
        }

        public static string MapPath(string directoryPath, string virtualPath)
        {
            string resolvedPath = virtualPath;
            if (string.IsNullOrEmpty(virtualPath))
            {
                virtualPath = "@";
            }
            if (!virtualPath.StartsWith("@") && !virtualPath.StartsWith("~"))
            {
                virtualPath = "@" + virtualPath;
            }
            if (virtualPath.StartsWith("@"))
            {
                if (string.IsNullOrEmpty(directoryPath))
                {
                    resolvedPath = string.Concat("~", virtualPath.Substring(1));
                }
                else
                {
                    return PageUtils.Combine(directoryPath, virtualPath.Substring(1));
                }
            }
            return PathUtils.MapPath(resolvedPath);
        }

        //public static string MapPath(string virtualPath)
        //{
        //    string retval;
        //    if (!string.IsNullOrEmpty(virtualPath))
        //    {
        //        if (virtualPath.StartsWith("~"))
        //        {
        //            virtualPath = virtualPath.Substring(1);
        //        }
        //        virtualPath = PageUtils.Combine("~", virtualPath);
        //    }
        //    else
        //    {
        //        virtualPath = "~/";
        //    }
        //    if (HttpContext.Current != null)
        //    {
        //        retval = HttpContext.Current.Server.MapPath(virtualPath);
        //    }
        //    else
        //    {
        //        string rootPath = ConfigUtils.Instance.PhysicalApplicationPath;

        //        if (!string.IsNullOrEmpty(virtualPath))
        //        {
        //            virtualPath = virtualPath.Substring(2);
        //        }
        //        else
        //        {
        //            virtualPath = string.Empty;
        //        }
        //        retval = PathUtils.Combine(rootPath, virtualPath);
        //    }

        //    if (retval == null) retval = string.Empty;
        //    return retval.Replace("/", "\\");
        //}

        //将编辑器中图片上传至本机
        public static string SaveImage(PublishmentSystemInfo publishmentSystemInfo, string content)
        {
            ArrayList originalImageSrcs = RegexUtils.GetOriginalImageSrcs(content);
            for (int i = 0; i < originalImageSrcs.Count; i++)
            {
                string originalImageSrc = (string)originalImageSrcs[i];
                if (PageUtils.IsProtocolUrl(originalImageSrc) && !StringUtils.StartsWithIgnoreCase(originalImageSrc, ConfigUtils.Instance.ApplicationPath) && !StringUtils.StartsWithIgnoreCase(originalImageSrc, publishmentSystemInfo.PublishmentSystemUrl))
                {
                    string fileExtName = PageUtils.GetExtensionFromUrl(originalImageSrc);
                    if (EFileSystemTypeUtils.IsImageOrFlashOrPlayer(fileExtName))
                    {
                        string fileName = string.Empty;
                        string directoryPath = string.Empty;
                        string filePath = string.Empty;

                        fileName = PathUtility.GetUploadFileName(publishmentSystemInfo, originalImageSrc);
                        directoryPath = PathUtility.GetUploadDirectoryPath(publishmentSystemInfo, fileExtName);
                        filePath = PathUtils.Combine(directoryPath, fileName);

                        try
                        {
                            if (!FileUtils.IsFileExists(filePath))
                            {
                                WebClientUtils.SaveRemoteFileToLocal(originalImageSrc, filePath);
                                if (EFileSystemTypeUtils.IsImage(PathUtils.GetExtension(fileName)))
                                {
                                    FileUtility.AddWaterMark(publishmentSystemInfo, filePath);
                                }
                            }
                            string fileUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(publishmentSystemInfo, filePath);
                            content = content.Replace(originalImageSrc, fileUrl);
                        }
                        catch { }
                    }
                }
            }
            return content;
        }

        public static string GetSiteTemplatesPath(string relatedPath)
        {
            relatedPath = PathUtils.RemoveParentPath(relatedPath);
            return PathUtils.Combine(ConfigUtils.Instance.PhysicalApplicationPath, DirectoryUtils.SiteFiles.DirectoryName, DirectoryUtils.SiteFiles.SiteTemplates, relatedPath);
        }

        public static string GetSiteTemplateMetadataPath(string siteTemplatePath, string relatedPath)
        {
            relatedPath = PathUtils.RemoveParentPath(relatedPath);
            return PathUtils.Combine(siteTemplatePath, DirectoryUtils.SiteTemplates.SiteTemplateMetadata, relatedPath);
        }

        public static string GetIndependentTemplatesPath(string relatedPath)
        {
            relatedPath = PathUtils.RemoveParentPath(relatedPath);
            return PathUtils.Combine(ConfigUtils.Instance.PhysicalApplicationPath, DirectoryUtils.SiteFiles.DirectoryName, DirectoryUtils.SiteFiles.IndependentTemplates, relatedPath);
        }

        public static string GetIndependentTemplateMetadataPath(string siteTemplatePath, string relatedPath)
        {
            relatedPath = PathUtils.RemoveParentPath(relatedPath);
            return PathUtils.Combine(siteTemplatePath, DirectoryUtils.IndependentTemplates.IndependentTemplateMetadata, relatedPath);
        }

        public static string GetCacheFilePath(string cacheFileName)
        {
            cacheFileName = PathUtils.RemoveParentPath(cacheFileName);
            return PathUtils.Combine(ConfigUtils.Instance.PhysicalApplicationPath, DirectoryUtils.SiteFiles.DirectoryName, DirectoryUtils.SiteFiles.TemporaryFiles, cacheFileName);
        }

        public static bool IsSystemFile(string fileName)
        {
            if (StringUtils.EqualsIgnoreCase(fileName, "Web.config")
                || StringUtils.EqualsIgnoreCase(fileName, "Global.asax")
                || StringUtils.EqualsIgnoreCase(fileName, "robots.txt"))
            {
                return true;
            }
            return false;
        }


        public static bool IsSystemFileForChangePublishmentSystemType(string fileName)
        {
            if (StringUtils.EqualsIgnoreCase(fileName, "Web.config")
                || StringUtils.EqualsIgnoreCase(fileName, "Global.asax")
                || StringUtils.EqualsIgnoreCase(fileName, "robots.txt")
                || StringUtils.EqualsIgnoreCase(fileName, "aliyun_login.aspx")
                || StringUtils.EqualsIgnoreCase(fileName, "config.xml")
                || StringUtils.EqualsIgnoreCase(fileName, "packages.config")
                || StringUtils.EqualsIgnoreCase(fileName, "setup.aspx")
                || StringUtils.EqualsIgnoreCase(fileName, "version.txt")
                || StringUtils.EqualsIgnoreCase(fileName, "Web（Aliyun）.config"))
            {
                return true;
            }
            return false;
        }


        public static bool IsWebSiteFile(string fileName)
        {
            if (StringUtils.EqualsIgnoreCase(fileName, "T_系统首页模板.htm")
               || StringUtils.EqualsIgnoreCase(fileName, "index.htm"))
            {
                return true;
            }
            return false;
        }

        #region PathRule

        public class ChannelFilePathRules
        {
            private ChannelFilePathRules()
            {
            }

            private static string ChannelID = "{@ChannelID}";
            private static string ChannelIndex = "{@ChannelIndex}";
            private static string Year = "{@Year}";
            private static string Month = "{@Month}";
            private static string Day = "{@Day}";
            private static string Hour = "{@Hour}";
            private static string Minute = "{@Minute}";
            private static string Second = "{@Second}";
            private static string Sequence = "{@Sequence}";

            //继承父级设置 20151113 sessionliang
            private static string ParentRule = "{@ParentRule}";
            private static string ChannelName = "{@ChannelName}";

            public static string DefaultRule = "/channels/{@ChannelID}.html";
            public static string DefaultDirectoryName = "/channels/";
            public static string DefaultRegexString = "/channels/(?<channelID>[^_]*)_?(?<pageIndex>[^_]*)";

            public static IDictionary GetDictionary(PublishmentSystemInfo publishmentSystemInfo, int nodeID)
            {
                ListDictionary dictionary = new ListDictionary();

                dictionary.Add(ChannelID, "栏目ID");
                dictionary.Add(ChannelIndex, "栏目索引");
                dictionary.Add(Year, "年份");
                dictionary.Add(Month, "月份");
                dictionary.Add(Day, "日期");
                dictionary.Add(Hour, "小时");
                dictionary.Add(Minute, "分钟");
                dictionary.Add(Second, "秒钟");
                dictionary.Add(Sequence, "顺序数");

                //继承父级设置 20151113 sessionliang
                dictionary.Add(ParentRule, "父级命名规则");
                dictionary.Add(ChannelName, "栏目名称");

                ETableStyle tableStyle = ETableStyle.Channel;
                string tableName = DataProvider.NodeDAO.TableName;
                ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(publishmentSystemInfo.PublishmentSystemID, nodeID);

                ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, relatedIdentities);
                foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                {
                    if (styleInfo.InputType == EInputType.Text)
                    {
                        dictionary.Add(string.Format(@"{{@{0}}}", styleInfo.AttributeName), styleInfo.DisplayName);
                    }
                }

                return dictionary;
            }

            public static string Parse(PublishmentSystemInfo publishmentSystemInfo, int nodeID)
            {
                string channelFilePathRule = PathUtility.GetChannelFilePathRule(publishmentSystemInfo, nodeID);
                string filePath = ParseChannelPath(publishmentSystemInfo, nodeID, channelFilePathRule);
                return filePath;
            }

            //递归处理
            private static string ParseChannelPath(PublishmentSystemInfo publishmentSystemInfo, int nodeID, string channelFilePathRule)
            {

                string filePath = channelFilePathRule.Trim();
                string regex = "(?<element>{@[^}]+})";
                ArrayList elements = RegexUtils.GetContents("element", regex, filePath);
                NodeInfo nodeInfo = null;

                foreach (string element in elements)
                {
                    string value = string.Empty;
                    if (StringUtils.EqualsIgnoreCase(element, ChannelFilePathRules.ChannelID))
                    {
                        value = nodeID.ToString();
                    }
                    else if (StringUtils.EqualsIgnoreCase(element, ChannelFilePathRules.ChannelIndex))
                    {
                        if (nodeInfo == null) nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                        value = nodeInfo.NodeIndexName;
                    }
                    else if (StringUtils.EqualsIgnoreCase(element, ChannelFilePathRules.Year))
                    {
                        if (nodeInfo == null) nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                        value = nodeInfo.AddDate.Year.ToString();
                    }
                    else if (StringUtils.EqualsIgnoreCase(element, ChannelFilePathRules.Month))
                    {
                        if (nodeInfo == null) nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                        value = nodeInfo.AddDate.Month.ToString();
                    }
                    else if (StringUtils.EqualsIgnoreCase(element, ChannelFilePathRules.Day))
                    {
                        if (nodeInfo == null) nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                        value = nodeInfo.AddDate.Day.ToString();
                    }
                    else if (StringUtils.EqualsIgnoreCase(element, ChannelFilePathRules.Hour))
                    {
                        if (nodeInfo == null) nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                        value = nodeInfo.AddDate.Hour.ToString();
                    }
                    else if (StringUtils.EqualsIgnoreCase(element, ChannelFilePathRules.Minute))
                    {
                        if (nodeInfo == null) nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                        value = nodeInfo.AddDate.Minute.ToString();
                    }
                    else if (StringUtils.EqualsIgnoreCase(element, ChannelFilePathRules.Second))
                    {
                        if (nodeInfo == null) nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                        value = nodeInfo.AddDate.Second.ToString();
                    }
                    else if (StringUtils.EqualsIgnoreCase(element, ChannelFilePathRules.Sequence))
                    {
                        value = DataProvider.NodeDAO.GetSequence(publishmentSystemInfo.PublishmentSystemID, nodeID).ToString();
                    }
                    else if (StringUtils.EqualsIgnoreCase(element, ChannelFilePathRules.ParentRule))//继承父级设置 20151113 sessionliang
                    {
                        if (nodeInfo == null) nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                        NodeInfo parentInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeInfo.ParentID);
                        if (parentInfo != null)
                        {
                            string parentRule = PathUtility.GetChannelFilePathRule(publishmentSystemInfo, parentInfo.NodeID);
                            value = DirectoryUtils.GetDirectoryPath(ParseChannelPath(publishmentSystemInfo, parentInfo.NodeID, parentRule)).Replace("\\", "/");
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(element, ChannelFilePathRules.ChannelName))//栏目名称 20151113 sessionliang
                    {
                        if (nodeInfo == null) nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                        value = nodeInfo.NodeName;
                    }
                    else
                    {
                        if (nodeInfo == null) nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                        string attributeName = element.Replace("{@", string.Empty).Replace("}", string.Empty);
                        value = nodeInfo.Additional.GetExtendedAttribute(attributeName);
                    }

                    filePath = filePath.Replace(element, value);
                }

                if (filePath.Contains("//"))
                {
                    filePath = Regex.Replace(filePath, @"(/)\1{2,}", "/");
                    filePath = Regex.Replace(filePath, @"//", "/");
                }
                return filePath;
            }
        }

        public class ContentFilePathRules
        {
            private ContentFilePathRules()
            {
            }

            private static string ChannelID = "{@ChannelID}";
            private static string ChannelIndex = "{@ChannelIndex}";
            private static string ContentID = "{@ContentID}";
            private static string Year = "{@Year}";
            private static string Month = "{@Month}";
            private static string Day = "{@Day}";
            private static string Hour = "{@Hour}";
            private static string Minute = "{@Minute}";
            private static string Second = "{@Second}";
            private static string Sequence = "{@Sequence}";

            //继承父级设置 20151113 sessionliang
            private static string ParentRule = "{@ParentRule}";
            private static string ChannelName = "{@ChannelName}";

            public static string DefaultRule = "/contents/{@ChannelID}/{@ContentID}.html";
            public static string DefaultDirectoryName = "/contents/";
            public static string DefaultRegexString = "/contents/(?<channelID>[^/]*)/(?<contentID>[^/]*)_?(?<pageIndex>[^_]*)";

            public static IDictionary GetDictionary(PublishmentSystemInfo publishmentSystemInfo, int nodeID)
            {
                ListDictionary dictionary = new ListDictionary();

                dictionary.Add(ChannelID, "栏目ID");
                dictionary.Add(ChannelIndex, "栏目索引");
                dictionary.Add(ContentID, "内容ID");
                dictionary.Add(Year, "年份");
                dictionary.Add(Month, "月份");
                dictionary.Add(Day, "日期");
                dictionary.Add(Hour, "小时");
                dictionary.Add(Minute, "分钟");
                dictionary.Add(Second, "秒钟");
                dictionary.Add(Sequence, "顺序数");

                //继承父级设置 20151113 sessionliang
                dictionary.Add(ParentRule, "父级命名规则");
                dictionary.Add(ChannelName, "栏目名称");

                ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeID);
                string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeID);
                ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(publishmentSystemInfo.PublishmentSystemID, nodeID);

                ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, relatedIdentities);
                foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                {
                    if (styleInfo.InputType == EInputType.Text)
                    {
                        dictionary.Add(string.Format(@"{{@{0}}}", styleInfo.AttributeName), styleInfo.DisplayName);
                    }
                }

                return dictionary;
            }

            public static string Parse(PublishmentSystemInfo publishmentSystemInfo, int nodeID, int contentID)
            {
                string contentFilePathRule = PathUtility.GetContentFilePathRule(publishmentSystemInfo, nodeID);
                ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeID);
                string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeID);
                ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);
                string filePath = ParseContentPath(publishmentSystemInfo, nodeID, contentInfo, contentFilePathRule);
                return filePath;
            }

            public static string Parse(PublishmentSystemInfo publishmentSystemInfo, int nodeID, ContentInfo contentInfo)
            {
                string contentFilePathRule = PathUtility.GetContentFilePathRule(publishmentSystemInfo, nodeID);
                string filePath = ParseContentPath(publishmentSystemInfo, nodeID, contentInfo, contentFilePathRule);
                return filePath;
            }

            private static string ParseContentPath(PublishmentSystemInfo publishmentSystemInfo, int nodeID, ContentInfo contentInfo, string contentFilePathRule)
            {
                string filePath = contentFilePathRule.Trim();
                string regex = "(?<element>{@[^}]+})";
                ArrayList elements = RegexUtils.GetContents("element", regex, filePath);
                DateTime addDate = DateTime.MinValue;
                //ContentInfo contentInfo = null;
                int contentID = contentInfo.ID;
                foreach (string element in elements)
                {
                    string value = string.Empty;
                    if (StringUtils.EqualsIgnoreCase(element, ContentFilePathRules.ChannelID))
                    {
                        value = nodeID.ToString();
                    }
                    else if (StringUtils.EqualsIgnoreCase(element, ContentFilePathRules.ChannelIndex))
                    {
                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                        if (nodeInfo != null)
                        {
                            value = nodeInfo.NodeIndexName;
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(element, ContentFilePathRules.ContentID))
                    {
                        value = contentID.ToString();
                    }
                    else if (StringUtils.EqualsIgnoreCase(element, ContentFilePathRules.Sequence))
                    {
                        string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeID);
                        value = BaiRongDataProvider.ContentDAO.GetSequence(tableName, nodeID, contentID).ToString();
                    }
                    else if (StringUtils.EqualsIgnoreCase(element, ContentFilePathRules.ParentRule))//继承父级设置 20151113 sessionliang
                    {
                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                        NodeInfo parentInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeInfo.ParentID);
                        if (parentInfo != null)
                        {
                            string parentRule = PathUtility.GetContentFilePathRule(publishmentSystemInfo, parentInfo.NodeID);
                            value = DirectoryUtils.GetDirectoryPath(ParseContentPath(publishmentSystemInfo, parentInfo.NodeID, contentInfo, parentRule)).Replace("\\", "/");
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(element, ContentFilePathRules.ChannelName))//栏目名称 20151113 sessionliang
                    {
                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                        value = nodeInfo.NodeName;
                    }
                    else if (StringUtils.EqualsIgnoreCase(element, ContentFilePathRules.Year) || StringUtils.EqualsIgnoreCase(element, ContentFilePathRules.Month) || StringUtils.EqualsIgnoreCase(element, ContentFilePathRules.Day) || StringUtils.EqualsIgnoreCase(element, ContentFilePathRules.Hour) || StringUtils.EqualsIgnoreCase(element, ContentFilePathRules.Minute) || StringUtils.EqualsIgnoreCase(element, ContentFilePathRules.Second))
                    {
                        if (addDate == DateTime.MinValue)
                        {
                            string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeID);
                            addDate = BaiRongDataProvider.ContentDAO.GetAddDate(tableName, contentID);
                        }

                        if (StringUtils.EqualsIgnoreCase(element, ContentFilePathRules.Year))
                        {
                            value = addDate.Year.ToString();
                        }
                        else if (StringUtils.EqualsIgnoreCase(element, ContentFilePathRules.Month))
                        {
                            value = addDate.Month.ToString("D2");
                            //value = addDate.ToString("MM");
                        }
                        else if (StringUtils.EqualsIgnoreCase(element, ContentFilePathRules.Day))
                        {
                            value = addDate.Day.ToString("D2");
                            //value = addDate.ToString("dd");
                        }
                        else if (StringUtils.EqualsIgnoreCase(element, ContentFilePathRules.Hour))
                        {
                            value = addDate.Hour.ToString();
                        }
                        else if (StringUtils.EqualsIgnoreCase(element, ContentFilePathRules.Minute))
                        {
                            value = addDate.Minute.ToString();
                        }
                        else if (StringUtils.EqualsIgnoreCase(element, ContentFilePathRules.Second))
                        {
                            value = addDate.Second.ToString();
                        }
                    }
                    else
                    {
                        string attributeName = element.Replace("{@", string.Empty).Replace("}", string.Empty);
                        if (contentInfo == null)
                        {
                            ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeID);
                            string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeID);
                            contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);
                        }
                        if (contentInfo != null)
                        {
                            value = contentInfo.GetExtendedAttribute(attributeName);
                        }
                    }

                    filePath = filePath.Replace(element, value);
                }

                if (filePath.Contains("//"))
                {
                    filePath = Regex.Replace(filePath, @"(/)\1{2,}", "/");
                    filePath = filePath.Replace("//", "/");
                }

                if (filePath.Contains("("))
                {
                    regex = @"(?<element>\([^\)]+\))";
                    elements = RegexUtils.GetContents("element", regex, filePath);
                    foreach (string element in elements)
                    {
                        if (element.Contains("|"))
                        {
                            string value = element.Replace("(", string.Empty).Replace(")", string.Empty);
                            string value1 = value.Split('|')[0];
                            string value2 = value.Split('|')[1];
                            value = value1 + value2;

                            if (!string.IsNullOrEmpty(value1) && !string.IsNullOrEmpty(value1))
                            {
                                value = value1;
                            }

                            filePath = filePath.Replace(element, value);
                        }
                    }
                }
                return filePath;
            }
        }

        public static string GetChannelFilePathRule(PublishmentSystemInfo publishmentSystemInfo, int nodeID)
        {
            string channelFilePathRule = PathUtility.GetChannelFilePathRule(publishmentSystemInfo.PublishmentSystemID, nodeID);
            if (string.IsNullOrEmpty(channelFilePathRule))
            {
                channelFilePathRule = publishmentSystemInfo.Additional.ChannelFilePathRule;

                if (string.IsNullOrEmpty(channelFilePathRule))
                {
                    channelFilePathRule = PathUtility.ChannelFilePathRules.DefaultRule;
                }
            }
            return channelFilePathRule;
        }

        private static string GetChannelFilePathRule(int publishmentSystemID, int nodeID)
        {
            if (nodeID == 0) return string.Empty;
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
            if (nodeInfo == null) return string.Empty;

            string filePathRule = nodeInfo.ChannelFilePathRule;
            if (string.IsNullOrEmpty(filePathRule) && nodeInfo.ParentID != 0)
            {
                filePathRule = GetChannelFilePathRule(publishmentSystemID, nodeInfo.ParentID);
            }

            return filePathRule;
        }

        public static string GetContentFilePathRule(PublishmentSystemInfo publishmentSystemInfo, int nodeID)
        {
            string contentFilePathRule = PathUtility.GetContentFilePathRule(publishmentSystemInfo.PublishmentSystemID, nodeID);
            if (string.IsNullOrEmpty(contentFilePathRule))
            {
                contentFilePathRule = publishmentSystemInfo.Additional.ContentFilePathRule;

                if (string.IsNullOrEmpty(contentFilePathRule))
                {
                    contentFilePathRule = PathUtility.ContentFilePathRules.DefaultRule;
                }
            }
            return contentFilePathRule;
        }

        private static string GetContentFilePathRule(int publishmentSystemID, int nodeID)
        {
            if (nodeID == 0) return string.Empty;
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
            if (nodeInfo == null) return string.Empty;

            string filePathRule = nodeInfo.ContentFilePathRule;
            if (string.IsNullOrEmpty(filePathRule) && nodeInfo.ParentID != 0)
            {
                filePathRule = GetContentFilePathRule(publishmentSystemID, nodeInfo.ParentID);
            }

            return filePathRule;
        }

        public static string GetChannelPageFilePath(PublishmentSystemInfo publishmentSystemInfo, int nodeID, int currentPageIndex)
        {
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
            if (nodeInfo.NodeType == ENodeType.BackgroundPublishNode)
            {
                TemplateInfo templateInfo = TemplateManager.GetDefaultTemplateInfo(publishmentSystemInfo.PublishmentSystemID, ETemplateType.IndexPageTemplate);
                return PathUtility.GetIndexPageFilePath(publishmentSystemInfo, templateInfo.CreatedFileFullName, publishmentSystemInfo.IsHeadquarters);
            }
            string filePath = nodeInfo.FilePath;

            if (string.IsNullOrEmpty(filePath))
            {
                filePath = PathUtility.ChannelFilePathRules.Parse(publishmentSystemInfo, nodeID);
            }

            filePath = PathUtility.MapPath(publishmentSystemInfo, filePath);// PathUtils.Combine(publishmentSystemPath, filePath);
            if (PathUtils.IsDirectoryPath(filePath))
            {
                filePath = PathUtils.Combine(filePath, nodeID + ".html");
            }
            DirectoryUtils.CreateDirectoryIfNotExists(filePath);

            if (currentPageIndex != 0)
            {
                string appendix = string.Format("_{0}", (currentPageIndex + 1));
                string fileName = PathUtils.GetFileNameWithoutExtension(filePath) + appendix + PathUtils.GetExtension(filePath);
                filePath = PathUtils.Combine(DirectoryUtils.GetDirectoryPath(filePath), fileName);
            }

            return filePath;
        }

        public static string GetContentPageFilePath(PublishmentSystemInfo publishmentSystemInfo, int nodeID, int contentID, int currentPageIndex)
        {
            ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeID);
            string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeID);
            ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);
            return GetContentPageFilePath(publishmentSystemInfo, nodeID, contentInfo, currentPageIndex);
        }

        public static string GetContentPageFilePath(PublishmentSystemInfo publishmentSystemInfo, int nodeID, ContentInfo contentInfo, int currentPageIndex)
        {
            string filePath = PathUtility.ContentFilePathRules.Parse(publishmentSystemInfo, nodeID, contentInfo);

            filePath = PathUtility.MapPath(publishmentSystemInfo, filePath);
            if (PathUtils.IsDirectoryPath(filePath))
            {
                filePath = PathUtils.Combine(filePath, contentInfo.ID + ".html");
            }
            DirectoryUtils.CreateDirectoryIfNotExists(filePath);

            if (currentPageIndex != 0)
            {
                string appendix = string.Format("_{0}", (currentPageIndex + 1));
                string fileName = PathUtils.GetFileNameWithoutExtension(filePath) + appendix + PathUtils.GetExtension(filePath);
                filePath = PathUtils.Combine(DirectoryUtils.GetDirectoryPath(filePath), fileName);
            }

            return filePath;
        }

        #endregion

        #region Image/Video/File Upload

        public static bool IsImageExtenstionAllowed(PublishmentSystemInfo publishmentSystemInfo, string fileExtention)
        {
            return PathUtils.IsFileExtenstionAllowed(publishmentSystemInfo.Additional.ImageUploadTypeCollection, fileExtention);
        }

        public static bool IsImageSizeAllowed(PublishmentSystemInfo publishmentSystemInfo, int contentLength)
        {
            return contentLength <= publishmentSystemInfo.Additional.ImageUploadTypeMaxSize * 1024;
        }

        public static bool IsVideoExtenstionAllowed(PublishmentSystemInfo publishmentSystemInfo, string fileExtention)
        {
            return PathUtils.IsFileExtenstionAllowed(publishmentSystemInfo.Additional.VideoUploadTypeCollection, fileExtention);
        }

        public static bool IsVideoSizeAllowed(PublishmentSystemInfo publishmentSystemInfo, int contentLength)
        {
            return contentLength <= publishmentSystemInfo.Additional.VideoUploadTypeMaxSize * 1024;
        }

        public static bool IsFileExtenstionAllowed(PublishmentSystemInfo publishmentSystemInfo, string fileExtention)
        {
            string typeCollection = publishmentSystemInfo.Additional.FileUploadTypeCollection + "," + publishmentSystemInfo.Additional.ImageUploadTypeCollection + "," + publishmentSystemInfo.Additional.VideoUploadTypeCollection;
            return PathUtils.IsFileExtenstionAllowed(typeCollection, fileExtention);
        }

        public static bool IsFileSizeAllowed(PublishmentSystemInfo publishmentSystemInfo, int contentLength)
        {
            return contentLength <= publishmentSystemInfo.Additional.FileUploadTypeMaxSize * 1024;
        }

        public static bool IsUploadExtenstionAllowed(EStorageClassify classify, PublishmentSystemInfo publishmentSystemInfo, string fileExtention)
        {
            if (classify == EStorageClassify.File || classify == EStorageClassify.Site)
            {
                return PathUtility.IsFileExtenstionAllowed(publishmentSystemInfo, fileExtention);
            }
            else if (classify == EStorageClassify.Image)
            {
                return PathUtility.IsImageExtenstionAllowed(publishmentSystemInfo, fileExtention);
            }
            else if (classify == EStorageClassify.Video)
            {
                return PathUtility.IsVideoExtenstionAllowed(publishmentSystemInfo, fileExtention);
            }
            return false;
        }

        public static bool IsUploadSizeAllowed(EStorageClassify classify, PublishmentSystemInfo publishmentSystemInfo, int contentLength)
        {
            if (classify == EStorageClassify.File || classify == EStorageClassify.Site)
            {
                return PathUtility.IsFileSizeAllowed(publishmentSystemInfo, contentLength);
            }
            else if (classify == EStorageClassify.Image)
            {
                return PathUtility.IsImageSizeAllowed(publishmentSystemInfo, contentLength);
            }
            else if (classify == EStorageClassify.Video)
            {
                return PathUtility.IsVideoSizeAllowed(publishmentSystemInfo, contentLength);
            }
            return false;
        }

        public static bool IsUploadExtenstionAllowed(EUploadType uploadType, PublishmentSystemInfo publishmentSystemInfo, string fileExtention)
        {
            if (uploadType == EUploadType.Image)
            {
                return PathUtility.IsImageExtenstionAllowed(publishmentSystemInfo, fileExtention);
            }
            else if (uploadType == EUploadType.Video)
            {
                return PathUtility.IsVideoExtenstionAllowed(publishmentSystemInfo, fileExtention);
            }
            else if (uploadType == EUploadType.File)
            {
                return PathUtility.IsFileExtenstionAllowed(publishmentSystemInfo, fileExtention);
            }
            return false;
        }

        public static bool IsUploadSizeAllowed(EUploadType uploadType, PublishmentSystemInfo publishmentSystemInfo, int contentLength)
        {
            if (uploadType == EUploadType.Image)
            {
                return PathUtility.IsImageSizeAllowed(publishmentSystemInfo, contentLength);
            }
            else if (uploadType == EUploadType.Video)
            {
                return PathUtility.IsVideoSizeAllowed(publishmentSystemInfo, contentLength);
            }
            else if (uploadType == EUploadType.File)
            {
                return PathUtility.IsFileSizeAllowed(publishmentSystemInfo, contentLength);
            }
            return false;
        }

        public static string GetFileSystemIconUrl(PublishmentSystemInfo publishmentSystemInfo, FileSystemInfoExtend fileInfo, bool isLargeIcon)
        {
            EFileSystemType fileSystemType = EFileSystemType.Unknown;
            if (PathUtility.IsVideoExtenstionAllowed(publishmentSystemInfo, fileInfo.Type))
            {
                fileSystemType = EFileSystemType.Video;
            }
            else if (PathUtility.IsImageExtenstionAllowed(publishmentSystemInfo, fileInfo.Type))
            {
                fileSystemType = EFileSystemType.Image;
            }
            else
            {
                fileSystemType = EFileSystemTypeUtils.GetEnumType(fileInfo.Type);
            }
            return PageUtils.GetClientFileSystemIconUrl(fileSystemType, isLargeIcon);
        }

        #endregion
    }
}
