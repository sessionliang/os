using System;
using System.Collections;
using System.IO;

namespace BaiRong.Core
{
    /// <summary>
    /// 封装操作文件夹代码的类
    /// </summary>
    public class DirectoryUtils
    {
        public class aspnet_client
        {
            public const string DirectoryName = "aspnet_client";
        }

        public class Bin
        {
            public const string DirectoryName = "Bin";
        }

        public class LiveFiles
        {
            public const string DirectoryName = "LiveFiles";
        }

        public class Ask
        {
            public const string DirectoryName = "Ask";
        }
        public class BBS
        {
            public const string DirectoryName = "BBS";
        }

        public class obj
        {
            public const string DirectoryName = "obj";
        }
        public class SiteServer
        {
            public const string DirectoryName = "SiteServer";
        }

        public class Api
        {
            public const string DirectoryName = "Api";
        }

        public class PublishmentSytem
        {
            public const string Include = "Include";
            public const string Template = "Template";
            public const string Content = "Content";
        }
        public class WebConfig
        {
            public const string DirectoryName = "Web.config";
        }

        public class SiteFiles
        {
            public const string Services = "Services";
            public const string DirectoryName = "SiteFiles";
            public const string UserFiles = "UserFiles";
            public const string BackupFiles = "BackupFiles";
            public const string TemporaryFiles = "TemporaryFiles";
            public const string SiteTemplates = "SiteTemplates";
            public const string IndependentTemplates = "IndependentTemplates";
            public const string Module = "Products";
            public const string Products = "Products";
            public const string SSO = "SSO";
        }

        public class SiteTemplates
        {
            //文件夹
            public const string SiteTemplateMetadata = "SiteTemplateMetadata";//存储频道模板元数据的文件夹名称
            public const string SiteContent = "SiteContent";//频道内容导入导出临时文件夹名
            public const string Input = "Input";//提交表单导入导出临时文件夹名
            public const string Table = "Table";//辅助表导入导出临时文件夹名
            public const string RelatedField = "RelatedField";//关联字段导入导出临时文件夹名
            public const string Photo = "Photo";//相册导入导出临时文件夹名

            public const string B2CSettings = "B2CSettings";//商城设置
            public const string Spec = "Spec";//规格导入导出临时文件夹名
            public const string Goods = "Goods";//商品导入导出临时文件夹名
            public const string Filter = "Filter";//过滤导入导出临时文件夹名

            public class WeiXin
            {
                //文件夹
                public const string DirectoryName = "WeiXin";//微信

                public const string Coupon = "Coupon";//优惠券 
                public const string Lottery = "Lottery";//刮刮卡 大转盘 砸金蛋 大翻牌 摇摇乐
                public const string Vote = "Vote";//微投票
                public const string Message = "Message";//微留言
                public const string Conference = "Conference";//微会议
                public const string Album = "Album";//微相册
                public const string Search = "Search";//微搜索
                public const string Appointment = "Appointment";//微预约
                public const string Store = "Store";//微门店
                public const string Collect = "Collect";//微征集
                public const string Card = "Card";//微会员卡
                public const string KeyWord = "KeyWord";//关键字
                public const string Menu = "Menu";//微信菜单
                public const string WebMenu = "WebMenu";//微导航菜单

                //文件 

                public const string File_Account = "Account.xml";//账户信息
                public const string File_Count = "Count.xml";//图表信息               
                public const string File_Map = "Map.xml";//微导航
                public const string File_View360 = "View360.xml";//微360全景 
                public const string File_Wifi = "Wifi.xml";//微Wifi
                public const string File_WifiNode = "WifiNode.xml";//微Wifi路由器            

            }


            //文件
            public const string File_Template = "Template.xml";//序列化模板的文件名
            public const string File_DisplayMode = "DisplayMode.xml";//序列化显示方式的文件名
            public const string File_MenuDisplay = "MenuDisplay.xml";//序列化菜单显示方式的文件名
            public const string File_TagStyle = "TagStyle.xml";//序列化模板标签样式的文件名
            public const string File_GatherRule = "GatherRule.xml";//序列化采集规则的文件名
            public const string File_Ad = "Ad.xml";//序列化固定广告的文件名
            public const string File_Metadata = "Metadata.xml";//频道模板元数据文件
            public const string File_Configuration = "Configuration.xml";
            public const string File_Seo = "Seo.xml";
            public const string File_StlTag = "StlTag.xml";
            public const string File_ContentModel = "ContentModel.xml";//自定义添加的内容模型
        }

        public class IndependentTemplates
        {
            //文件夹
            public const string IndependentTemplateMetadata = "IndependentTemplateMetadata";//存储频道模板元数据的文件夹名称
            public const string SiteContent = "SiteContent";//频道内容导入导出临时文件夹名

            //SiteServer CMS导入文件
            public const string File_Template = "Template.xml";//序列化模板的文件名
            public const string File_Metadata = "Metadata.xml";//频道模板元数据文件
        }

        public class UserCenter
        {
            public const string DirectoryName = "UserCenter";
            public const string Themes = "themes";
        }

        public class Module
        {
            public const string DirectoryName = "Module";

            public const string File_Configuration = "Configuration.config";

            public const string Directory_Filesxxx = "Files";

            public class Menu
            {
                public const string DirectoryName = "Menu";
                //public const string File_Permissions = "Permissions.config";
                public const string File_Top = "Top.config";
            }

            public class UserCenter
            {
                public const string DirectoryName = "UserCenter";
                public const string File_Permissions = "Permissions.config";
                public const string File_Top = "Top.config";
            }
        }

        public class Products
        {
            public const string DirectoryName = "Products";

            public const string File_Configuration = "Configuration.config";

            public class Apps
            {
                public const string DirectoryName = "Apps";
                public const string File_Permissions = "Permissions.config";

                public const string Directory_Files = "Files";
            }

            public class Menu
            {
                public const string DirectoryName = "Menu";
                public const string File_Top = "Top.config";
            }

            public class UserCenter
            {
                public const string DirectoryName = "UserCenter";
                public const string File_Permissions = "Permissions.config";
                public const string File_Top = "Top.config";
            }
        }

        public static char DirectorySeparatorChar = Path.DirectorySeparatorChar;

        public static void CreateDirectoryIfNotExists(string path)
        {
            string directoryPath = GetDirectoryPath(path);

            if (!IsDirectoryExists(directoryPath))
            {
                try
                {
                    Directory.CreateDirectory(directoryPath);
                }
                catch
                {
                    //Scripting.FileSystemObject fso = new Scripting.FileSystemObjectClass();
                    //string[] directoryNames = directoryPath.Split('\\');
                    //string thePath = directoryNames[0];
                    //for (int i = 1; i < directoryNames.Length; i++)
                    //{
                    //    thePath = thePath + "\\" + directoryNames[i];
                    //    if (StringUtils.Contains(thePath.ToLower(), ConfigUtils.Instance.PhysicalApplicationPath.ToLower()) && !IsDirectoryExists(thePath))
                    //    {
                    //        fso.CreateFolder(thePath);
                    //    }
                    //}                    
                }
            }
        }

        public static void Copy(string sourcePath, string targetPath)
        {
            Copy(sourcePath, targetPath, true);
        }

        public static void Copy(string sourcePath, string targetPath, bool isOverride)
        {
            if (Directory.Exists(sourcePath))
            {
                DirectoryUtils.CreateDirectoryIfNotExists(targetPath);
                DirectoryInfo directoryInfo = new DirectoryInfo(sourcePath);
                if (directoryInfo.GetFileSystemInfos() != null)
                {
                    foreach (FileSystemInfo fileSystemInfo in directoryInfo.GetFileSystemInfos())
                    {
                        string destPath = Path.Combine(targetPath, fileSystemInfo.Name);
                        if (fileSystemInfo is FileInfo)
                        {
                            FileUtils.CopyFile(fileSystemInfo.FullName, destPath, isOverride);
                        }
                        else if (fileSystemInfo is DirectoryInfo)
                        {
                            Copy(fileSystemInfo.FullName, destPath, isOverride);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 验证此字符串是否合作作为文件夹名称
        /// </summary>
        public static bool IsDirectoryNameCompliant(string directoryName)
        {
            if (string.IsNullOrEmpty(directoryName)) return false;
            if (-1 != directoryName.IndexOfAny(PathUtils.InvalidPathChars))
            {
                return false;
            }
            //for (int i = 0; i < directoryName.Length; i++)
            //{
            //    if (StringUtils.IsTwoBytesChar(directoryName[i]))
            //    {
            //        return false;
            //    }
            //}
            return true;
        }

        /// <summary>
        /// 获取文件的文件夹路径，如果path为文件夹，返回自身。
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string GetDirectoryPath(string path)
        {
            string directoryPath;
            string ext = Path.GetExtension(path);
            if (!string.IsNullOrEmpty(ext))		//path为文件路径
            {
                directoryPath = Path.GetDirectoryName(path);
            }
            else									//path为文件夹路径
            {
                directoryPath = path;
            }
            return directoryPath;
        }


        public static bool IsDirectoryExists(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        public static bool IsInDirectory(string parentDirectoryPath, string path)
        {
            if (string.IsNullOrEmpty(parentDirectoryPath) || string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException();
            }

            parentDirectoryPath = parentDirectoryPath.Trim().ToLower();
            path = path.Trim().ToLower();

            char ch1 = parentDirectoryPath[parentDirectoryPath.Length - 1];
            if (ch1 == Path.DirectorySeparatorChar)
            {
                parentDirectoryPath = parentDirectoryPath.Substring(0, parentDirectoryPath.Length - 1);
            }

            char ch2 = path[path.Length - 1];
            if (ch2 == Path.DirectorySeparatorChar)
            {
                path = path.Substring(0, path.Length - 1);
            }

            return path.StartsWith(parentDirectoryPath);
        }

        public static void MoveDirectory(string srcDirectoryPath, string destDirectoryPath, bool isOverride)
        {
            //如果提供的路径中不存在末尾分隔符，则添加末尾分隔符。
            if (!srcDirectoryPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                srcDirectoryPath += Path.DirectorySeparatorChar;
            }
            if (!destDirectoryPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                destDirectoryPath += Path.DirectorySeparatorChar;
            }

            //如果目标目录不存在，则予以创建。 
            DirectoryUtils.CreateDirectoryIfNotExists(destDirectoryPath);

            //从当前父目录中获取目录列表。 
            foreach (string srcDir in Directory.GetDirectories(srcDirectoryPath))
            {
                string directoryName = PathUtils.GetDirectoryName(srcDir);

                DirectoryInfo srcDirInfo = new DirectoryInfo(srcDir);

                string destDir = destDirectoryPath + directoryName;
                //如果该目录不存在，则创建该目录。 
                DirectoryUtils.CreateDirectoryIfNotExists(destDir);
                //由于我们处于递归模式下，因此还要复制子目录
                MoveDirectory(srcDir, destDir, isOverride);
            }

            //从当前父目录中获取文件。
            foreach (string srcFile in Directory.GetFiles(srcDirectoryPath))
            {
                FileInfo srcFileInfo = new FileInfo(srcFile);
                FileInfo destFileInfo = new FileInfo(srcFile.Replace(srcDirectoryPath, destDirectoryPath));
                //如果文件不存在，则进行复制。 
                bool isExists = destFileInfo.Exists;
                if (isOverride)
                {
                    if (isExists)
                    {
                        FileUtils.DeleteFileIfExists(destFileInfo.FullName);
                    }
                    FileUtils.CopyFile(srcFileInfo.FullName, destFileInfo.FullName);
                }
                else if (!isExists)
                {
                    FileUtils.CopyFile(srcFileInfo.FullName, destFileInfo.FullName);
                }
            }
        }


        public static string[] GetDirectoryNames(string directoryPath)
        {
            string[] directorys = Directory.GetDirectories(directoryPath);
            string[] retval = new string[directorys.Length];
            int i = 0;
            foreach (string directory in directorys)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                retval[i++] = directoryInfo.Name;
            }
            return retval;
        }

        public static ArrayList GetLowerDirectoryNames(string directoryPath)
        {
            ArrayList arraylist = new ArrayList();
            string[] directorys = Directory.GetDirectories(directoryPath);
            foreach (string directory in directorys)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                arraylist.Add(directoryInfo.Name.ToLower());
            }
            return arraylist;
        }

        public static string[] GetFileNames(string directoryPath)
        {
            string[] filePaths = Directory.GetFiles(directoryPath);
            string[] retval = new string[filePaths.Length];
            int i = 0;
            foreach (string filePath in filePaths)
            {
                FileInfo fileInfo = new FileInfo(filePath);
                retval[i++] = fileInfo.Name;
            }
            return retval;
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="directoryPath">文件夹路径</param>
        /// <returns>删除过程中是否出错</returns>
        public static bool DeleteDirectoryIfExists(string directoryPath)
        {
            bool retval = true;
            try
            {
                if (DirectoryUtils.IsDirectoryExists(directoryPath))
                {
                    Directory.Delete(directoryPath, true);
                }
            }
            catch
            {
                retval = false;
            }
            return retval;
        }

        public static void DeleteFilesSync(string rootDirectoryPath, string syncDirectoryPath)
        {
            if (DirectoryUtils.IsDirectoryExists(syncDirectoryPath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(syncDirectoryPath);
                if (directoryInfo.GetFileSystemInfos() != null)
                {
                    foreach (FileSystemInfo fileSystemInfo in directoryInfo.GetFileSystemInfos())
                    {
                        string fileSystemPath = PathUtils.Combine(rootDirectoryPath, fileSystemInfo.Name);
                        if (fileSystemInfo is FileInfo)
                        {
                            try
                            {
                                FileUtils.DeleteFileIfExists(fileSystemPath);
                            }
                            catch { }
                        }
                        else if (fileSystemInfo is DirectoryInfo)
                        {
                            DeleteFilesSync(fileSystemPath, fileSystemInfo.FullName);
                            DirectoryUtils.DeleteEmptyDirectory(fileSystemPath);
                        }
                    }
                }
            }
        }

        public static void DeleteEmptyDirectory(string directoryPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            if (directoryInfo != null && directoryInfo.Exists)
            {
                if (directoryInfo.GetFileSystemInfos() == null || directoryInfo.GetFileSystemInfos().Length == 0)
                {
                    try
                    {
                        DirectoryUtils.DeleteDirectoryIfExists(directoryPath);
                    }
                    catch { }
                }
            }
        }

        //public static void DeleteChildrens(string directoryPath)
        //{
        //    string[] filePaths = DirectoryUtils.GetFilePaths(directoryPath);
        //    FileUtils.DeleteFilesIfExists(filePaths);

        //    string[] directoryPaths = DirectoryUtils.GetDirectoryPaths(directoryPath);
        //    foreach (string subDirectoryPath in directoryPaths)
        //    {
        //        DirectoryUtils.DeleteDirectoryIfExists(subDirectoryPath);
        //    }
        //}

        public static void CreateUrlRedirectDirectories(string sourceUrlRedirectFilePath, string directoryPath, ArrayList directoryNameArrayList)
        {
            if (directoryNameArrayList != null && directoryNameArrayList.Count > 0)
            {
                foreach (string directoryName in directoryNameArrayList)
                {
                    DirectoryUtils.CreateUrlRedirectDirectory(sourceUrlRedirectFilePath, PathUtils.Combine(directoryPath, directoryName));
                }
            }
        }

        public static void CreateUrlRedirectDirectories(string directoryPath, ArrayList directoryNameArrayList)
        {
            if (directoryNameArrayList != null && directoryNameArrayList.Count > 0)
            {
                foreach (string directoryName in directoryNameArrayList)
                {
                    DirectoryUtils.CreateUrlRedirectDirectory(PathUtils.Combine(directoryPath, directoryName));
                }
            }
        }

        public static void CreateUrlRedirectDirectory(string sourceUrlRedirectFilePath, string directoryPath)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);
            string filePath = PathUtils.Combine(directoryPath, "default.aspx");
            if (!FileUtils.IsFileExists(filePath))
            {
                FileUtils.CopyFile(sourceUrlRedirectFilePath, filePath);
            }
        }

        public static void CreateUrlRedirectDirectory(string directoryPath)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);
            string filePath = PathUtils.Combine(directoryPath, "default.aspx");
            if (!FileUtils.IsFileExists(filePath))
            {
                FileUtils.CopyFile(PathUtils.MapPath(PageUtils.GetAbsoluteSiteFilesUrl(BaiRong.Core.SiteFiles.Default.UrlRedirectPage)), filePath);
            }
        }

        public static string[] GetDirectoryPaths(string directoryPath)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);
            return Directory.GetDirectories(directoryPath);
        }

        public static string[] GetDirectoryPaths(string directoryPath, string searchPattern)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);
            return Directory.GetDirectories(directoryPath, searchPattern);
        }

        public static string[] GetFilePaths(string directoryPath)
        {
            return Directory.GetFiles(directoryPath);
        }

        public static long GetDirectorySize(string directoryPath)
        {
            long size = 0;
            string[] filePaths = DirectoryUtils.GetFilePaths(directoryPath);
            //通过GetFiles方法,获取目录中所有文件的大小
            foreach (string filePath in filePaths)
            {
                FileInfo info = new FileInfo(filePath);
                size += info.Length;
            }
            string[] directoryPaths = DirectoryUtils.GetDirectoryPaths(directoryPath);
            //获取目录下所有文件夹大小,并存到一个新的对象数组中
            foreach (string path in directoryPaths)
            {
                size += GetDirectorySize(path);
            }
            return size;
        }

        public static bool IsSystemDirectory(string directoryName)
        {
            if (StringUtils.EqualsIgnoreCase(directoryName, DirectoryUtils.aspnet_client.DirectoryName)
                || StringUtils.EqualsIgnoreCase(directoryName, DirectoryUtils.Bin.DirectoryName)
                || StringUtils.EqualsIgnoreCase(directoryName, DirectoryUtils.LiveFiles.DirectoryName)
                || StringUtils.EqualsIgnoreCase(directoryName, DirectoryUtils.SiteFiles.DirectoryName)
                || StringUtils.EqualsIgnoreCase(directoryName, FileConfigManager.Instance.AdminDirectoryName)
                || StringUtils.EqualsIgnoreCase(directoryName, DirectoryUtils.Ask.DirectoryName)
                || StringUtils.EqualsIgnoreCase(directoryName, DirectoryUtils.BBS.DirectoryName)
                || StringUtils.EqualsIgnoreCase(directoryName, DirectoryUtils.SiteTemplates.SiteTemplateMetadata)
                || StringUtils.EqualsIgnoreCase(directoryName, DirectoryUtils.Api.DirectoryName)
                || StringUtils.EqualsIgnoreCase(directoryName, "obj")
                || StringUtils.EqualsIgnoreCase(directoryName, "Properties"))
            {
                return true;
            }
            return false;
        }

        public static bool IsWebSiteDirectory(string directoryName)
        {
            if (StringUtils.EqualsIgnoreCase(directoryName, "channels")
                || StringUtils.EqualsIgnoreCase(directoryName, "contents")
                || StringUtils.EqualsIgnoreCase(directoryName, "Template")
                || StringUtils.EqualsIgnoreCase(directoryName, "include")
                || StringUtils.EqualsIgnoreCase(directoryName, "upload"))
            {
                return true;
            }
            return false;
        }

        public static ArrayList GetLowerSystemDirectoryNames()
        {
            ArrayList arraylist = new ArrayList();
            arraylist.Add(DirectoryUtils.aspnet_client.DirectoryName.ToLower());
            arraylist.Add(DirectoryUtils.Bin.DirectoryName.ToLower());
            arraylist.Add(DirectoryUtils.LiveFiles.DirectoryName.ToLower());
            arraylist.Add(DirectoryUtils.SiteFiles.DirectoryName.ToLower());
            arraylist.Add(FileConfigManager.Instance.AdminDirectoryName.ToLower());
            arraylist.Add(DirectoryUtils.SiteTemplates.SiteTemplateMetadata.ToLower());

            return arraylist;
        }
    }
}
