using System;
using System.Xml;
using BaiRong.Core;
using BaiRong.Core.Drawing;
using BaiRong.Model;
using SiteServer.CMS.Model;
using BaiRong.Core.IO;
using System.Collections;

using BaiRong.Core.Data.Provider;
using System.Text;

namespace SiteServer.CMS.Core
{
    public class FileUtility
    {
        private FileUtility()
        {
        }

        public static void AddWaterMark(PublishmentSystemInfo publishmentSystemInfo, string imagePath)
        {
            try
            {
                string fileExtName = PathUtils.GetExtension(imagePath);
                if (EFileSystemTypeUtils.IsImage(fileExtName))
                {
                    if (publishmentSystemInfo.Additional.IsWaterMark)
                    {
                        if (publishmentSystemInfo.Additional.IsImageWaterMark)
                        {
                            if (!string.IsNullOrEmpty(publishmentSystemInfo.Additional.WaterMarkImagePath))
                            {
                                ImageUtils.AddImageWaterMark(imagePath, PathUtility.MapPath(publishmentSystemInfo, publishmentSystemInfo.Additional.WaterMarkImagePath), publishmentSystemInfo.Additional.WaterMarkPosition, publishmentSystemInfo.Additional.WaterMarkTransparency, publishmentSystemInfo.Additional.WaterMarkMinWidth, publishmentSystemInfo.Additional.WaterMarkMinHeight, publishmentSystemInfo.Additional.Qty);
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(publishmentSystemInfo.Additional.WaterMarkFormatString))
                            {
                                DateTime now = DateTime.Now;
                                ImageUtils.AddTextWaterMark(imagePath, string.Format(publishmentSystemInfo.Additional.WaterMarkFormatString, DateUtils.GetDateString(now), DateUtils.GetTimeString(now)), publishmentSystemInfo.Additional.WaterMarkFontName, publishmentSystemInfo.Additional.WaterMarkFontSize, publishmentSystemInfo.Additional.WaterMarkPosition, publishmentSystemInfo.Additional.WaterMarkTransparency, publishmentSystemInfo.Additional.WaterMarkMinWidth, publishmentSystemInfo.Additional.WaterMarkMinHeight);
                            }
                        }
                    }
                }
            }
            catch { }
        }


        //        public static void CreateWebConfig(string configFilePath, string encoding)
        //        {
        //            string content = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
        //<configuration>
        //
        //<system.web>
        //	<globalization fileEncoding=""{0}"" requestEncoding=""{0}"" responseEncoding=""{0}""/>
        //</system.web>
        // 
        //</configuration>", encoding);
        //            FileUtils.WriteText(configFilePath, ECharset.utf_8, content);
        //        }

        //        public static void UpdateWebConfig(string configFilePath, string encoding)
        //        {
        //            XmlDocument doc = new XmlDocument();

        //            doc.PreserveWhitespace = true;

        //            doc.Load(configFilePath);
        //            bool dirty = false;
        //            System.Xml.XmlNode appSettings = doc.SelectSingleNode("configuration/system.web/globalization");
        //            foreach (XmlAttribute attribute in appSettings.Attributes)
        //            {
        //                if (StringUtils.EqualsIgnoreCase(attribute.Name, "fileEncoding") || StringUtils.EqualsIgnoreCase(attribute.Name, "requestEncoding") || StringUtils.EqualsIgnoreCase(attribute.Name, "responseEncoding"))
        //                {
        //                    if (attribute.Value != encoding)
        //                    {
        //                        attribute.Value = encoding;
        //                        dirty = true;
        //                    }
        //                }
        //            }

        //            if (dirty)
        //            {
        //                // Save the document to a file and auto-indent the output.
        //                XmlTextWriter writer = new XmlTextWriter(configFilePath, System.Text.Encoding.UTF8);
        //                writer.Formatting = System.Xml.Formatting.Indented;
        //                doc.Save(writer);
        //                writer.Flush();
        //                writer.Close();
        //            }
        //        }

        private static void MoveFile(PublishmentSystemInfo sourcePublishmentSystemInfo, PublishmentSystemInfo destPublishmentSystemInfo, string relatedUrl)
        {
            if (!string.IsNullOrEmpty(relatedUrl))
            {
                string sourceFilePath = PathUtility.MapPath(sourcePublishmentSystemInfo, relatedUrl);
                string descFilePath = PathUtility.MapPath(destPublishmentSystemInfo, relatedUrl);
                if (FileUtils.IsFileExists(sourceFilePath))
                {
                    FileUtils.MoveFile(sourceFilePath, descFilePath, false);
                }
            }
        }

        public static void MoveFileByContentInfo(PublishmentSystemInfo sourcePublishmentSystemInfo, PublishmentSystemInfo destPublishmentSystemInfo, ContentInfo contentInfo)
        {
            if (contentInfo == null || sourcePublishmentSystemInfo.PublishmentSystemID == destPublishmentSystemInfo.PublishmentSystemID) return;

            try
            {
                if (contentInfo is BackgroundContentInfo)
                {
                    contentInfo = contentInfo as BackgroundContentInfo;
                    if (PageUtility.IsVirtualUrl(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.ImageUrl)))
                    {
                        FileUtility.MoveFile(sourcePublishmentSystemInfo, destPublishmentSystemInfo, contentInfo.GetExtendedAttribute(BackgroundContentAttribute.ImageUrl));
                    }
                    if (PageUtility.IsVirtualUrl(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.VideoUrl)))
                    {
                        FileUtility.MoveFile(sourcePublishmentSystemInfo, destPublishmentSystemInfo, contentInfo.GetExtendedAttribute(BackgroundContentAttribute.VideoUrl));
                    }
                    if (PageUtility.IsVirtualUrl(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.FileUrl)))
                    {
                        FileUtility.MoveFile(sourcePublishmentSystemInfo, destPublishmentSystemInfo, contentInfo.GetExtendedAttribute(BackgroundContentAttribute.FileUrl));
                    }
                    ArrayList srcArrayList = RegexUtils.GetOriginalImageSrcs(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.Content));
                    foreach (string src in srcArrayList)
                    {
                        if (PageUtility.IsVirtualUrl(src))
                        {
                            FileUtility.MoveFile(sourcePublishmentSystemInfo, destPublishmentSystemInfo, src);
                        }
                    }
                }
            }
            catch { }
        }

        public static void MoveFileByVirtaulUrl(PublishmentSystemInfo sourcePublishmentSystemInfo, PublishmentSystemInfo destPublishmentSystemInfo, string fileVirtaulUrl)
        {
            if (string.IsNullOrEmpty(fileVirtaulUrl) || sourcePublishmentSystemInfo.PublishmentSystemID == destPublishmentSystemInfo.PublishmentSystemID) return;

            try
            {
                if (PageUtility.IsVirtualUrl(fileVirtaulUrl))
                {
                    FileUtility.MoveFile(sourcePublishmentSystemInfo, destPublishmentSystemInfo, fileVirtaulUrl);
                }
            }
            catch { }
        }

        public static void CopyCrossDomainFilesToSite(PublishmentSystemInfo publishmentSystemInfo)
        {
            if (!publishmentSystemInfo.IsHeadquarters)
            {
                string directoryPath = PathUtility.MapPath(publishmentSystemInfo, "utils");
                DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);

                string sourceFilePath = PathUtils.GetSiteFilesPath(SiteFiles.Default.Proxy);
                string fileName = PathUtils.GetFileName(sourceFilePath);
                string destFilePath = PathUtils.Combine(directoryPath, fileName);
                if (!FileUtils.IsFileExists(destFilePath))
                {
                    FileUtils.CopyFile(sourceFilePath, destFilePath);
                }

                sourceFilePath = PathUtils.GetSiteFilesPath(SiteFiles.Default.AjaxProxy);
                fileName = PathUtils.GetFileName(sourceFilePath);
                destFilePath = PathUtils.Combine(directoryPath, fileName);
                if (!FileUtils.IsFileExists(destFilePath))
                {
                    FileUtils.CopyFile(sourceFilePath, destFilePath);
                }
            }
        }

        public static void CopyFuncFilesToSite(PublishmentSystemInfo publishmentSystemInfo)
        {
            if (!publishmentSystemInfo.IsHeadquarters)
            {
                ArrayList virtualPathArrayList = new ArrayList();

                virtualPathArrayList.Add(DirectoryUtils.Bin.DirectoryName);
                virtualPathArrayList.Add(PathUtils.Combine(DirectoryUtils.SiteFiles.DirectoryName, SiteFiles.Directory.BaiRong));
                virtualPathArrayList.Add(PathUtils.Combine(DirectoryUtils.SiteFiles.DirectoryName, SiteFiles.Directory.Configuration));
                virtualPathArrayList.Add(PathUtils.Combine(DirectoryUtils.SiteFiles.DirectoryName, SiteFiles.Directory.Module));
                virtualPathArrayList.Add(PathUtils.Combine(DirectoryUtils.SiteFiles.DirectoryName, SiteFiles.Directory.Services));
                virtualPathArrayList.Add(DirectoryUtils.UserCenter.DirectoryName);

                foreach (string virtualPath in virtualPathArrayList)
                {
                    string sourceDirectoryPath = PathUtils.MapPath(virtualPath);
                    string targetDirectoryPath = PathUtility.MapPath(publishmentSystemInfo, virtualPath);
                    DirectoryUtils.Copy(sourceDirectoryPath, targetDirectoryPath, true);
                }

                string webConfig = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <appSettings>
    <add key=""DatabaseType"" value=""{0}"" />
    <add key=""ConnectionString"" value=""{1}"" />
  </appSettings>
</configuration>", EDatabaseTypeUtils.GetValue(BaiRongDataProvider.DatabaseType), BaiRongDataProvider.ConnectionString);

                string filePath = PathUtility.MapPath(publishmentSystemInfo, "Web.config");
                FileUtils.WriteText(filePath, ECharset.utf_8, webConfig);
            }
        }

        /// <summary>
        /// 设置跨域代理文件
        /// </summary>
        /// <param name="publishmentSystemInfo"></param>
        public static void CopyXDomainFilesToSite(PublishmentSystemInfo publishmentSystemInfo)
        {
            if (!publishmentSystemInfo.IsHeadquarters)
            {
                string sourceDirectoryPath = PathUtils.MapPath(PathUtils.Combine(DirectoryUtils.SiteFiles.DirectoryName, SiteFiles.Directory.BaiRong, "XDomain/xdomain.min.js"));
                string targetDirectoryPath = PathUtility.MapPath(DirectoryUtils.Api.DirectoryName, "utils/xdomain.min.js");
                FileUtils.CopyFile(sourceDirectoryPath, targetDirectoryPath, true);

                Uri uri = new Uri(publishmentSystemInfo.Additional.OuterUrl);
                if (uri != null)
                {
                    string proxy = string.Format(@"<!DOCTYPE html>
<script src='xdomain.min.js'></script>
<script>
  xdomain.masters({{
     '{0}':'/api/*'
     //siteserver xdomain extend
  }});
  xdomain.debug = true;
</script> ", PageUtils.AddProtocolToUrl(uri.Host).TrimEnd('/'), FileUtils.ReadText(PathUtils.MapPath(PathUtils.Combine(DirectoryUtils.SiteFiles.DirectoryName, SiteFiles.Directory.BaiRong, "XDomain/xdomain.min.js")), ECharset.utf_8));

                    string filePath = PathUtility.MapPath(DirectoryUtils.Api.DirectoryName, string.Format("utils/proxy_{0}.html", publishmentSystemInfo.PublishmentSystemID));
                    FileUtils.DeleteFileIfExists(filePath);
                    FileUtils.WriteText(filePath, ECharset.utf_8, proxy);
                }
            }
        }

        public static void ParseOutServerFiles(PublishmentSystemInfo publishmentSystemInfo, string inputPath, string input)
        {
            StringBuilder builder = new StringBuilder(input);

            //只有在前后台分离部署的情况下，需要处理路径规则
            if (publishmentSystemInfo.Additional.IsSiteStorage)
            {
                if (publishmentSystemInfo.Additional.IsSonSiteAlone)
                {
                    //子站单独域名，与主站不同
                    //需要处理页面资源css、js、sitefiles/xxx等文件的路径，去除子站文件夹名称，上传的文件除外。
                }

                //根据图片服务器配置，替换图片网络路径
                if (publishmentSystemInfo.Additional.IsImageStorage)
                {
                    //站点名称替换为host+pre，例如：xxxsite/upload/images 替换为 protocal+host+pre/upload/images
                }
                else
                {
                    if (publishmentSystemInfo.Additional.IsSonSiteAlone)
                    {
                        //站点名称替换为string.empty，xxxsite/upload/images 替换为 /upload/images
                    }
                }
                //根据视频服务器配置，替换视频网络路径
                //根据附件服务器配置，替换附件网络路径
                //根据API服务器配置，替换API网络路径
            }


            #region 子站单独部署 20151116 sessionliang
            if (publishmentSystemInfo.Additional.IsSonSiteAlone)
            {
                //文件的虚拟目录，@/xxxx/xxx.html
                string virtualUrl = PageUtility.GetPublishmentSystemVirtualUrlByPhysicalPath(publishmentSystemInfo, inputPath);
                string outerPath = PathUtils.MapPath(PageUtils.GetSiteFilesUrl(PageUtils.Combine("SonSiteHtml", PageUtility.ParseNavigationUrl(publishmentSystemInfo, virtualUrl))));
                if (!string.IsNullOrEmpty(publishmentSystemInfo.Additional.IISDefaultPage))
                {
                    if (StringUtils.EqualsIgnoreCase(PageUtils.GetFileNameFromUrl(virtualUrl), publishmentSystemInfo.Additional.IISDefaultPage))
                    {
                        outerPath = outerPath + PageUtils.GetFileNameFromUrl(virtualUrl);
                    }
                }

                //替换全部
                builder = builder.Replace(string.Format("\"{0}\"", publishmentSystemInfo.PublishmentSystemUrl), "\"/\"");
                builder = builder.Replace(string.Format("'{0}'", publishmentSystemInfo.PublishmentSystemUrl), "'/'");
                builder = builder.Replace(publishmentSystemInfo.PublishmentSystemUrl, string.Empty);

                if (publishmentSystemInfo.Additional.EditorUploadFilePre != null && publishmentSystemInfo.Additional.EditorUploadFilePre.Length > 0)
                {
                    builder = builder.Replace("/upload/images", string.Format("{0}/upload/images", publishmentSystemInfo.PublishmentSystemUrl));
                    builder = builder.Replace("/upload/files", string.Format("{0}/upload/files", publishmentSystemInfo.PublishmentSystemUrl));
                    builder = builder.Replace("/upload/videos", string.Format("{0}/upload/videos", publishmentSystemInfo.PublishmentSystemUrl));
                }

                //sitefile中的service权并不需要改写到api中
                if (PageUtils.IsProtocolUrl(publishmentSystemInfo.Additional.APIUrl))
                {
                    //替换/sitefiles/xxx为http://api/sitefiles/xxx
                    builder = builder.Replace("/sitefiles", PageUtils.AddProtocolToUrl("/sitefiles", string.Format("{0}://{1}", new Uri(publishmentSystemInfo.Additional.APIUrl).Scheme, new Uri(publishmentSystemInfo.Additional.APIUrl).Host)));
                }
                else if (!StringUtils.EqualsIgnoreCase(publishmentSystemInfo.Additional.APIUrl, "/api"))
                {
                    //替换/sitefiles/xxx为http://api/sitefiles/xxx
                    builder = builder.Replace("/sitefiles", PageUtils.AddProtocolToUrl("/sitefiles", string.Format("{0}://{1}", new Uri(PageUtils.AddProtocolToUrl(publishmentSystemInfo.Additional.APIUrl)).Scheme, new Uri(PageUtils.AddProtocolToUrl(publishmentSystemInfo.Additional.APIUrl)).Host)));
                }
                FileUtils.WriteText(outerPath, ECharset.utf_8, builder.ToString());
            }
            #endregion
        }
    }
}
