using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using SiteServer.BBS.Model;
using BaiRong.Model;
using BaiRong.Core.Net;
using SiteServer.CMS.Core;

namespace SiteServer.BBS.Core
{
    public class PathUtilityBBS
    {
        public static string GetTemplateDirectoryPath(int publishmentSystemID)
        {
            ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(publishmentSystemID);

            return GetTemplateDirectoryPath(publishmentSystemID, additional.TemplateDir);
        }

        public static string GetTemplateDirectoryPathOfDefault(int publishmentSystemID)
        {
            return GetTemplateDirectoryPath(publishmentSystemID, "default");
        }

        public static string GetTemplateDirectoryPath(int publishmentSystemID, string templateDir)
        {
            return PathUtility.GetPublishmentSystemPath(publishmentSystemID, "templates", templateDir);
        }

        public static string GetTemplatesPath(int publishmentSystemID)
        {
            return PathUtility.GetPublishmentSystemPath(publishmentSystemID, "templates");
        }

        public static string GetTemplateXmlPath(string templatePath)
        {
            return PathUtils.Combine(templatePath, "template.xml");
        }

        public static string GetUploadDirectoryPath(int publishmentSystemID)
        {
            return GetUploadDirectoryPath(publishmentSystemID, DateTime.Now);
        }

        public static string GetUploadDirectoryPath(int publishmentSystemID, DateTime datetime)
        {
            string directoryPath = string.Empty;

            ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(publishmentSystemID);

            EDateFormatType dateFormatType = EDateFormatTypeUtils.GetEnumType(additional.UploadDateFormatString);
            string uploadDirectoryPath = PathUtility.GetPublishmentSystemPath(publishmentSystemID, additional.UploadDirectoryName);
            
            if (dateFormatType == EDateFormatType.Year)
            {
                directoryPath = PathUtils.Combine(uploadDirectoryPath, datetime.Year.ToString());
            }
            else if (dateFormatType == EDateFormatType.Day)
            {
                directoryPath = PathUtils.Combine(uploadDirectoryPath, datetime.Year.ToString(), datetime.Month.ToString(), datetime.Day.ToString());
            }
            else
            {
                directoryPath = PathUtils.Combine(uploadDirectoryPath, datetime.Year.ToString(), datetime.Month.ToString());
            }
            DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);
            return directoryPath;
        }

        public static string GetUploadFileName(int publishmentSystemID, string filePath)
        {
            return GetUploadFileName(publishmentSystemID, filePath, DateTime.Now);
        }

        public static string GetUploadFileName(int publishmentSystemID, string filePath, DateTime now)
        {
            string retval = string.Empty;

            ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(publishmentSystemID);

            if (additional.IsUploadChangeFileName)
            {
                string strDateTime = string.Format("{0}{1}{2}{3}{4}", now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);
                retval = string.Format("{0}{1}", strDateTime, PathUtils.GetExtension(filePath));
            }
            else
            {
                retval = PathUtils.GetFileName(filePath);
            }
            return retval;
        }

        //将编辑器中图片上传至本机
        public static string SaveImage(int publishmentSystemID, string content)
        {
            string bbsUrl = PageUtils.AddProtocolToUrl(PageUtilityBBS.GetBBSUrl(publishmentSystemID, string.Empty));
            string directoryPath = PathUtilityBBS.GetUploadDirectoryPath(publishmentSystemID);
            ArrayList originalImageSrcs = RegexUtils.GetOriginalImageSrcs(content);
            for (int i = 0; i < originalImageSrcs.Count; i++)
            {
                string originalImageSrc = (string)originalImageSrcs[i];
                if (PageUtils.IsProtocolUrl(originalImageSrc) && !StringUtils.StartsWithIgnoreCase(originalImageSrc, ConfigUtils.Instance.ApplicationPath) && !StringUtils.StartsWithIgnoreCase(originalImageSrc, bbsUrl))
                {
                    string fileName = PathUtilityBBS.GetUploadFileName(publishmentSystemID, originalImageSrc);
                    string filePath = PathUtils.Combine(directoryPath, fileName);

                    try
                    {
                        if (!FileUtils.IsFileExists(filePath))
                        {
                            WebClientUtils.SaveRemoteFileToLocal(originalImageSrc, filePath);
                            if (EFileSystemTypeUtils.IsImage(PathUtils.GetExtension(fileName)))
                            {
                                FileUtilityBBS.AddWaterMark(publishmentSystemID, filePath);
                            }
                        }
                        string fileUrl = PageUtilityBBS.GetBBSUrlByPhysicalPath(publishmentSystemID, filePath);
                        content = content.Replace(originalImageSrc, fileUrl);
                    }
                    catch { }
                }
            }
            return content;
        }

        #region PathRule

        public class FilePathRulesForum
        {
            private FilePathRulesForum()
            {
            }

            public static string ForumID = "{@ForumID}";
            public static string ForumIndex = "{@ForumIndex}";
            public static string Year = "{@Year}";
            public static string Month = "{@Month}";
            public static string Day = "{@Day}";
            public static string Hour = "{@Hour}";
            public static string Minute = "{@Minute}";
            public static string Second = "{@Second}";

            public static string DefaultRule = "/forums/{@ForumID}.aspx";
            public static string DefaultDirectoryName = "/forums/";
            public static string DefaultRegexString = "/forums/(?<forumID>[^_]*)_?(?<pageIndex>[^_]*)";

            public static IDictionary GetDictionary()
            {
                ListDictionary dictionary = new ListDictionary();

                dictionary.Add(ForumID, "板块ID");
                dictionary.Add(ForumIndex, "板块索引");
                dictionary.Add(Year, "年份");
                dictionary.Add(Month, "月份");
                dictionary.Add(Day, "日期");
                dictionary.Add(Hour, "小时");
                dictionary.Add(Minute, "分钟");
                dictionary.Add(Second, "秒钟");

                return dictionary;
            }
        }

        public static string GetFilePathRule(int publishmentSystemID, int forumID)
        {
            if (forumID == 0) return string.Empty;
            ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, forumID);
            if (forumInfo == null) return string.Empty;

            string filePathRule = string.Empty;

            filePathRule = forumInfo.FilePathRule;
            if (string.IsNullOrEmpty(filePathRule) && forumInfo.ParentID != 0)
            {
                filePathRule = GetFilePathRule(publishmentSystemID, forumInfo.ParentID);
            }

            if (string.IsNullOrEmpty(filePathRule))
            {
                filePathRule = ConfigurationManager.GetAdditional(publishmentSystemID).FilePathRule;
                if (string.IsNullOrEmpty(filePathRule))
                {
                    filePathRule = PathUtilityBBS.FilePathRulesForum.DefaultRule;
                }
            }

            return filePathRule;
        }

        public static string ParseFilePathRule(int publishmentSystemID, int forumID)
        {
            string filePathRule = PathUtilityBBS.GetFilePathRule(publishmentSystemID, forumID);
            string filePath = filePathRule.ToLower().Trim();
            filePath = filePath.Replace("{@forumid}", forumID.ToString());
            if (StringUtils.Contains(filePath, "{@forumindex}"))
            {
                ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, forumID);
                filePath = filePath.Replace("{@forumindex}", forumInfo.IndexName);
            }
            if (StringUtils.Contains(filePath, "{@year}") || StringUtils.Contains(filePath, "{@month}") || StringUtils.Contains(filePath, "{@day}") || StringUtils.Contains(filePath, "{@hour}") || StringUtils.Contains(filePath, "{@minute}") || StringUtils.Contains(filePath, "{@second}"))
            {
                ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, forumID);
                filePath = filePath.Replace("{@year}", forumInfo.AddDate.Year.ToString());
                filePath = filePath.Replace("{@month}", forumInfo.AddDate.Month.ToString());
                filePath = filePath.Replace("{@day}", forumInfo.AddDate.Day.ToString());
                filePath = filePath.Replace("{@hour}", forumInfo.AddDate.Hour.ToString());
                filePath = filePath.Replace("{@minute}", forumInfo.AddDate.Minute.ToString());
                filePath = filePath.Replace("{@second}", forumInfo.AddDate.Second.ToString());
            }
            return filePath;
        }

        public static string GetForumFilePath(int publishmentSystemID, int forumID)
        {
            ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, forumID);

            string filePath = forumInfo.FilePath;

            if (string.IsNullOrEmpty(filePath))
            {
                filePath = ParseFilePathRule(publishmentSystemID, forumID);
            }

            filePath = PathUtility.GetPublishmentSystemPath(publishmentSystemID, filePath);
            if (PathUtils.IsDirectoryPath(filePath))
            {
                filePath = PathUtils.Combine(filePath, forumID + ".aspx");
            }
            DirectoryUtils.CreateDirectoryIfNotExists(filePath);

            return filePath;
        }

        //新加
        public static string MapPath(string virtualPath)
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
                    
                        resolvedPath = PageUtils.Combine("bbs", virtualPath.Substring(1));
            }
            return PathUtils.MapPath(resolvedPath);
        }
        #endregion
    }
}
