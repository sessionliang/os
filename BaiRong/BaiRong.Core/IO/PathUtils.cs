using System.IO;
using System.Web;
using BaiRong.Model;
using BaiRong.Core;
using System.Text.RegularExpressions;
using System;
using System.Xml;

namespace BaiRong.Core
{
    public class PathUtils
    {
        public const char SeparatorChar = '\\';
        public static readonly char[] InvalidPathChars = Path.InvalidPathChars;

        public static string Combine(params string[] paths)
        {
            string retval = string.Empty;
            if (paths != null && paths.Length > 0)
            {
                retval = (paths[0] != null) ? paths[0].Replace(PageUtils.SeparatorChar, PathUtils.SeparatorChar).TrimEnd(PathUtils.SeparatorChar) : string.Empty;
                for (int i = 1; i < paths.Length; i++)
                {
                    string path = (paths[i] != null) ? paths[i].Replace(PageUtils.SeparatorChar, PathUtils.SeparatorChar).Trim(PathUtils.SeparatorChar) : string.Empty;
                    retval = Path.Combine(retval, path);
                }
            }
            return retval;
        }

        //public static string MapPath(string virtualPath)
        //{
        //    string retval = string.Empty;
        //    if (!string.IsNullOrEmpty(virtualPath))
        //    {
        //        if (virtualPath.StartsWith("~"))
        //        {
        //            virtualPath = virtualPath.Substring(1);
        //        }
        //        virtualPath = PageUtils.Combine("~", virtualPath);
        //    }
        //    HttpContext context = HttpContext.Current;
        //    if(context != null)
        //    {
        //        retval = context.Server.MapPath(virtualPath);
        //    }
        //    else
        //    {
        //        if (virtualPath != null)
        //        {
        //            virtualPath = virtualPath.Substring(2);
        //        }
        //        else
        //        {
        //            virtualPath = string.Empty;
        //        }
        //        retval = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,  virtualPath);
        //    }
        //    if (retval == null) retval = string.Empty;
        //    return retval.Replace("/", "\\");
        //}


        //获取当前页面不带后缀的文件名
        public static string GetCurrentFileNameWithoutExtension()
        {
            if (HttpContext.Current != null)
            {
                return Path.GetFileNameWithoutExtension(HttpContext.Current.Request.PhysicalPath);
            }
            return string.Empty;
        }

        /// <summary>
        /// 根据路径扩展名判断是否为文件夹路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsDirectoryPath(string path)
        {
            bool retval = false;
            if (!string.IsNullOrEmpty(path))
            {
                string ext = Path.GetExtension(path);
                if (!string.IsNullOrEmpty(ext))		//path为文件路径
                {
                    retval = false;
                }
                else									//path为文件夹路径
                {
                    retval = true;
                }
            }
            return retval;
        }

        public static string GetExtension(string path)
        {
            string retval = string.Empty;
            if (!string.IsNullOrEmpty(path))
            {
                path = path.Trim('/', '\\').Trim();
                try
                {
                    retval = Path.GetExtension(path);
                }
                catch { }
            }
            return retval;
        }

        public static string RemoveExtension(string fileName)
        {
            string retval = string.Empty;
            if (!string.IsNullOrEmpty(fileName))
            {
                int index = fileName.LastIndexOf('.');
                if (index != -1)
                {
                    retval = fileName.Substring(0, index);
                }
                else
                {
                    retval = fileName;
                }
            }
            return retval;
        }

        public static string RemoveParentPath(string path)
        {
            string retval = string.Empty;
            if (!string.IsNullOrEmpty(path))
            {
                retval = path.Replace("../", string.Empty);
                retval = retval.Replace("./", string.Empty);
            }
            return retval;
        }

        public static string GetFileName(string filePath)
        {
            return Path.GetFileName(filePath);
        }

        public static string GetFileNameWithoutExtension(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

        public static string GetDirectoryName(string directoryPath)
        {
            if (!string.IsNullOrEmpty(directoryPath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
                return directoryInfo.Name;
            }
            return string.Empty;
        }

        public static string GetDirectoryDifference(string rootDirectoryPath, string path)
        {
            string directoryPath = DirectoryUtils.GetDirectoryPath(path);
            if (!string.IsNullOrEmpty(directoryPath) && StringUtils.StartsWithIgnoreCase(directoryPath, rootDirectoryPath))
            {
                string retval = directoryPath.Substring(rootDirectoryPath.Length, directoryPath.Length - rootDirectoryPath.Length);
                return retval.Trim('/', '\\');
            }
            return string.Empty;
        }

        public static string GetPathDifference(string rootPath, string path)
        {
            if (!string.IsNullOrEmpty(path) && StringUtils.StartsWithIgnoreCase(path, rootPath))
            {
                string retval = path.Substring(rootPath.Length, path.Length - rootPath.Length);
                return retval.Trim('/', '\\');
            }
            return string.Empty;
        }

        public static string AddVirtualToPath(string path)
        {
            string resolvedPath = path;
            if (!string.IsNullOrEmpty(path))
            {
                path.Replace("../", string.Empty);
                if (!path.StartsWith("~"))
                {
                    resolvedPath = "~" + path;
                }
            }
            return resolvedPath;
        }

        public static string GetCurrentPagePath()
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.PhysicalPath;
            }
            return string.Empty;
        }

        public static string GetSiteFilesPath(string relatedPath)
        {
            return PathUtils.MapPath(PageUtils.GetAbsoluteSiteFilesUrl(relatedPath));
        }

        public static string RemovePathInvalidChar(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return filePath;
            string invalidChars = new string(Path.GetInvalidPathChars());
            string invalidReStr = string.Format("[{0}]", Regex.Escape(invalidChars));
            return Regex.Replace(filePath, invalidReStr, "");
        }

        public static string MapPath(string virtualPath)
        {
            virtualPath = PathUtils.RemovePathInvalidChar(virtualPath);
            string retval;
            if (!string.IsNullOrEmpty(virtualPath))
            {
                if (virtualPath.StartsWith("~"))
                {
                    virtualPath = virtualPath.Substring(1);
                }
                virtualPath = PageUtils.Combine("~", virtualPath);
            }
            else
            {
                virtualPath = "~/";
            }
            if (HttpContext.Current != null)
            {
                retval = HttpContext.Current.Server.MapPath(virtualPath);
            }
            else
            {
                string rootPath = ConfigUtils.Instance.PhysicalApplicationPath;

                if (!string.IsNullOrEmpty(virtualPath))
                {
                    virtualPath = virtualPath.Substring(2);
                }
                else
                {
                    virtualPath = string.Empty;
                }
                retval = PathUtils.Combine(rootPath, virtualPath);
            }

            if (retval == null) retval = string.Empty;
            return retval.Replace("/", "\\");
        }

        public static bool IsFileExtenstionAllowed(string sAllowedExt, string sExt)
        {
            bool allow = false;
            if (sExt != null && sExt.StartsWith("."))
            {
                sExt = sExt.Substring(1, sExt.Length - 1);
            }
            sAllowedExt = sAllowedExt.Replace("|", ",");
            string[] aExt = sAllowedExt.Split(',');
            for (int i = 0; i < aExt.Length; i++)
            {
                if (StringUtils.EqualsIgnoreCase(sExt, aExt[i]))
                {
                    allow = true;
                    break;
                }
            }
            return allow;
        }

        public static bool IsFileExtenstionNotAllowed(string sNotAllowedExt, string sExt)
        {
            bool allow = true;
            if (sExt != null && sExt.StartsWith("."))
            {
                sExt = sExt.Substring(1, sExt.Length - 1);
            }
            sNotAllowedExt = sNotAllowedExt.Replace("|", ",");
            string[] aExt = sNotAllowedExt.Split(',');
            for (int i = 0; i < aExt.Length; i++)
            {
                if (StringUtils.EqualsIgnoreCase(sExt, aExt[i]))
                {
                    allow = false;
                    break;
                }
            }
            return allow;
        }

        public static string GetClientUserPath(string applicationName, string userName, string relatedPath)
        {
            string systemName = string.Empty;
            if (!string.IsNullOrEmpty(applicationName) && applicationName.IndexOf("_") != -1)
            {
                systemName = applicationName.Split('_')[1];
            }
            else
            {
                systemName = applicationName;
            }
            return PathUtils.GetSiteFilesPath(string.Format("{0}/{1}/{2}/{3}", SiteFiles.Directory.Users, userName, systemName, relatedPath));
        }

        public static string GetTemporaryFilesPath(string relatedPath)
        {
            return PathUtils.Combine(ConfigUtils.Instance.PhysicalApplicationPath, DirectoryUtils.SiteFiles.DirectoryName, DirectoryUtils.SiteFiles.TemporaryFiles, relatedPath);
        }

        public static string GetModulePath(params string[] paths)
        {
            return GetModulePath(false, paths);
        }

        public static string GetModulePath(bool isHotfix, params string[] paths)
        {
            string directoryPath = ConfigUtils.Instance.PhysicalApplicationPath;
            if (isHotfix)
            {
                directoryPath = PathUtils.GetTemporaryFilesPath(StringUtils.Constants.Hotfix_DirectoryName);
            }
            return PathUtils.Combine(directoryPath, DirectoryUtils.SiteFiles.DirectoryName, DirectoryUtils.SiteFiles.Module, PathUtils.Combine(paths));
        }

        public static string GetProductPath(params string[] paths)
        {
            return GetProductPath(false, paths);
        }

        public static string GetProductPath(bool isHotfix, params string[] paths)
        {
            string directoryPath = ConfigUtils.Instance.PhysicalApplicationPath;
            if (isHotfix)
            {
                directoryPath = PathUtils.GetTemporaryFilesPath(StringUtils.Constants.Hotfix_DirectoryName);
            }
            return PathUtils.Combine(directoryPath, DirectoryUtils.SiteFiles.DirectoryName, DirectoryUtils.SiteFiles.Products, PathUtils.Combine(paths));
        }

        public static string GetAppPath(params string[] paths)
        {
            string directoryPath = ConfigUtils.Instance.PhysicalApplicationPath;
            return PathUtils.Combine(directoryPath, DirectoryUtils.SiteFiles.DirectoryName, DirectoryUtils.SiteFiles.Products, DirectoryUtils.Products.Apps.DirectoryName, PathUtils.Combine(paths));
        }

        public static string GetAppPath(bool isHotfix, params string[] paths)
        {
            string directoryPath = ConfigUtils.Instance.PhysicalApplicationPath;
            if (isHotfix)
            {
                directoryPath = PathUtils.GetTemporaryFilesPath(StringUtils.Constants.Hotfix_DirectoryName);
            }
            return PathUtils.Combine(directoryPath, DirectoryUtils.SiteFiles.DirectoryName, DirectoryUtils.SiteFiles.Products, DirectoryUtils.Products.Apps.DirectoryName, PathUtils.Combine(paths));
        }

        public static string GetUpgradeDirectoryPath(bool isHotfix, EDatabaseType databaseType, string appID)
        {
            string directoryPath = PathUtils.GetAppPath(isHotfix, appID, "Upgrade");

            if (databaseType == EDatabaseType.SqlServer)
            {
                directoryPath = PathUtils.Combine(directoryPath, "SqlServer");
            }
            else if (databaseType == EDatabaseType.Oracle)
            {
                directoryPath = PathUtils.Combine(directoryPath, "Oracle");
            }

            return directoryPath;
        }

        //public static string GetInstallerProductSqlFilePath(string productID, EDatabaseType databaseType)
        //{
        //    string filePath = string.Empty;

        //    if (databaseType == EDatabaseType.SqlServer)
        //    {
        //        filePath = PathUtils.MapPath(string.Format("~/SiteFiles/Products/{0}/Installer/SqlServer/install.sql", productID));
        //    }
        //    else if (databaseType == EDatabaseType.Oracle)
        //    {
        //        filePath = PathUtils.MapPath(string.Format("~/SiteFiles/Products/{0}/Installer/Oracle/install.sql", productID));
        //    }

        //    return filePath;
        //}

        public static string GetInstallerAppSqlFilePath(string appID, EDatabaseType databaseType)
        {
            string filePath = string.Empty;

            if (databaseType == EDatabaseType.SqlServer)
            {
                filePath = PathUtils.MapPath(string.Format("~/SiteFiles/Products/Apps/{0}/Installer/SqlServer/install.sql", appID));
            }
            else if (databaseType == EDatabaseType.Oracle)
            {
                filePath = PathUtils.MapPath(string.Format("~/SiteFiles/Products/Apps/{0}/Installer/Oracle/install.sql", appID));
            }

            return filePath;
        }

        //public static string GetUnInstallerProductSqlFilePath(string productID, EDatabaseType databaseType)
        //{
        //    string filePath = string.Empty;

        //    if (databaseType == EDatabaseType.SqlServer)
        //    {
        //        filePath = PathUtils.MapPath(string.Format("~/SiteFiles/Products/{0}/Installer/SqlServer/unInstall.sql", productID));
        //    }
        //    else if (databaseType == EDatabaseType.Oracle)
        //    {
        //        filePath = PathUtils.MapPath(string.Format("~/SiteFiles/Products/{0}/Installer/Oracle/unInstall.sql", productID));
        //    }

        //    return filePath;
        //}

        public static string GetUserFilesPath(string userName, string relatedPath)
        {
            return PathUtils.Combine(ConfigUtils.Instance.PhysicalApplicationPath, DirectoryUtils.SiteFiles.DirectoryName, DirectoryUtils.SiteFiles.UserFiles, userName, relatedPath);
        }

        public static string GetUserUploadDirectoryPath(string userName)
        {
            string directoryPath;
            EDateFormatType dateFormatType = EDateFormatTypeUtils.GetEnumType(UserConfigManager.Additional.UploadDateFormatString);
            DateTime datetime = DateTime.Now;
            string userFilesPath = PathUtils.GetUserFilesPath(userName, string.Empty);
            if (dateFormatType == EDateFormatType.Year)
            {
                directoryPath = PathUtils.Combine(userFilesPath, datetime.Year.ToString());
            }
            else if (dateFormatType == EDateFormatType.Day)
            {
                directoryPath = PathUtils.Combine(userFilesPath, datetime.Year.ToString(), datetime.Month.ToString(), datetime.Day.ToString());
            }
            else
            {
                directoryPath = PathUtils.Combine(userFilesPath, datetime.Year.ToString(), datetime.Month.ToString());
            }
            DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);
            return directoryPath;
        }

        public static string GetUserUploadFileName(string filePath)
        {
            string retval;
            if (UserConfigManager.Additional.IsUploadChangeFileName)
            {
                DateTime dt = DateTime.Now;
                string strDateTime = string.Format("{0}{1}{2}{3}{4}", dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
                retval = string.Format("{0}{1}", strDateTime, PathUtils.GetExtension(filePath));
            }
            else
            {
                retval = PathUtils.GetFileName(filePath);
            }
            return retval;
        }

        public static string PhysicalSiteServerPath
        {
            get
            {
                return PathUtils.Combine(ConfigUtils.Instance.PhysicalApplicationPath, FileConfigManager.Instance.AdminDirectoryName);
            }
        }

        public static string PhysicalSiteFilesPath
        {
            get
            {
                return PathUtils.Combine(ConfigUtils.Instance.PhysicalApplicationPath, DirectoryUtils.SiteFiles.DirectoryName);
            }
        }

        public static void UpdateWebConfig(string configFilePath, string databaseType, string connectionString)
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(configFilePath);
            bool dirty = false;
            XmlNode appSettings = doc.SelectSingleNode("configuration/appSettings");
            foreach (XmlNode setting in appSettings)
            {
                if (setting.Name == "add")
                {
                    XmlAttribute attrKey = setting.Attributes["key"];
                    if (attrKey != null)
                    {
                        if (attrKey.Value == BaiRongDataProvider.NAME_DATABASE_TYPE)
                        {
                            XmlAttribute attrValue = setting.Attributes["value"];
                            if (attrValue != null)
                            {
                                attrValue.Value = databaseType;
                                dirty = true;
                            }
                        }
                        else if (attrKey.Value == BaiRongDataProvider.NAME_CONNECTION_STRING)
                        {
                            XmlAttribute attrValue = setting.Attributes["value"];
                            if (attrValue != null)
                            {
                                attrValue.Value = connectionString;
                                dirty = true;
                            }
                        }
                        else if (attrKey.Value == BaiRongDataProvider.NAME_DATABASE_OWNER)
                        {
                            XmlAttribute attrValue = setting.Attributes["value"];
                            if (attrValue != null)
                            {
                                attrValue.Value = "dbo";
                                dirty = true;
                            }
                        }
                    }
                }
            }

            if (dirty)
            {
                System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(configFilePath, System.Text.Encoding.UTF8);
                writer.Formatting = System.Xml.Formatting.Indented;
                doc.Save(writer);
                writer.Flush();
                writer.Close();
            }
        }
    }
}
