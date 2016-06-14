using System;
using System.Collections;
using System.Text;
using System.Data;
using SiteServer.BBS.Model;
using BaiRong.Core;
using BaiRong.Model;
using System.Web.Caching;

namespace SiteServer.BBS.Core
{
    public class CreateCacheManager
    {
        private CreateCacheManager()
        {
        }

        public class FileContent
        {
            private FileContent()
            {
            }

            public static string GetTemplateContent(int publishmentSystemID, string directoryName, string fileName)
            {
                try
                {
                    string filePath = PathUtils.Combine(PathUtilityBBS.GetTemplateDirectoryPath(publishmentSystemID), directoryName, fileName);
                    if (!FileUtils.IsFileExists(filePath))
                    {
                        filePath = PathUtils.Combine(PathUtilityBBS.GetTemplateDirectoryPathOfDefault(publishmentSystemID), directoryName, fileName);
                    }
                    if (CacheUtils.Get(filePath) == null)
                    {
                        string templateContent = FileUtils.ReadText(filePath, ECharset.utf_8);
                        CacheUtils.Insert(filePath, templateContent, new CacheDependency(filePath), CacheUtils.HourFactor);
                        return templateContent;
                    }
                    return CacheUtils.Get(filePath) as string;
                }
                catch
                {
                    return string.Empty;
                }
            }

            public static string GetIncludeContent(int publishmentSystemID, string file)
            {
                try
                {
                    string filePath = PathUtils.Combine(PathUtilityBBS.GetTemplateDirectoryPath(publishmentSystemID), file);
                    if (!FileUtils.IsFileExists(filePath))
                    {
                        filePath = PathUtils.Combine(PathUtilityBBS.GetTemplateDirectoryPathOfDefault(publishmentSystemID), file);
                    }
                    if (CacheUtils.Get(filePath) == null)
                    {
                        string includeContent = FileUtils.ReadText(filePath, ECharset.utf_8);

                        CacheUtils.Insert(filePath, includeContent, new CacheDependency(filePath), CacheUtils.HourFactor);
                        return includeContent;
                    }
                    return CacheUtils.Get(filePath) as string;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
    }
}
