using System;
using System.Collections;
using System.IO;

namespace BaiRong.Core
{
    /// <summary>
    /// ��װ�����ļ��д������
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
            //�ļ���
            public const string SiteTemplateMetadata = "SiteTemplateMetadata";//�洢Ƶ��ģ��Ԫ���ݵ��ļ�������
            public const string SiteContent = "SiteContent";//Ƶ�����ݵ��뵼����ʱ�ļ�����
            public const string Input = "Input";//�ύ�����뵼����ʱ�ļ�����
            public const string Table = "Table";//�������뵼����ʱ�ļ�����
            public const string RelatedField = "RelatedField";//�����ֶε��뵼����ʱ�ļ�����
            public const string Photo = "Photo";//��ᵼ�뵼����ʱ�ļ�����

            public const string B2CSettings = "B2CSettings";//�̳�����
            public const string Spec = "Spec";//����뵼����ʱ�ļ�����
            public const string Goods = "Goods";//��Ʒ���뵼����ʱ�ļ�����
            public const string Filter = "Filter";//���˵��뵼����ʱ�ļ�����

            public class WeiXin
            {
                //�ļ���
                public const string DirectoryName = "WeiXin";//΢��

                public const string Coupon = "Coupon";//�Ż�ȯ 
                public const string Lottery = "Lottery";//�ιο� ��ת�� �ҽ� ���� ҡҡ��
                public const string Vote = "Vote";//΢ͶƱ
                public const string Message = "Message";//΢����
                public const string Conference = "Conference";//΢����
                public const string Album = "Album";//΢���
                public const string Search = "Search";//΢����
                public const string Appointment = "Appointment";//΢ԤԼ
                public const string Store = "Store";//΢�ŵ�
                public const string Collect = "Collect";//΢����
                public const string Card = "Card";//΢��Ա��
                public const string KeyWord = "KeyWord";//�ؼ���
                public const string Menu = "Menu";//΢�Ų˵�
                public const string WebMenu = "WebMenu";//΢�����˵�

                //�ļ� 

                public const string File_Account = "Account.xml";//�˻���Ϣ
                public const string File_Count = "Count.xml";//ͼ����Ϣ               
                public const string File_Map = "Map.xml";//΢����
                public const string File_View360 = "View360.xml";//΢360ȫ�� 
                public const string File_Wifi = "Wifi.xml";//΢Wifi
                public const string File_WifiNode = "WifiNode.xml";//΢Wifi·����            

            }


            //�ļ�
            public const string File_Template = "Template.xml";//���л�ģ����ļ���
            public const string File_DisplayMode = "DisplayMode.xml";//���л���ʾ��ʽ���ļ���
            public const string File_MenuDisplay = "MenuDisplay.xml";//���л��˵���ʾ��ʽ���ļ���
            public const string File_TagStyle = "TagStyle.xml";//���л�ģ���ǩ��ʽ���ļ���
            public const string File_GatherRule = "GatherRule.xml";//���л��ɼ�������ļ���
            public const string File_Ad = "Ad.xml";//���л��̶������ļ���
            public const string File_Metadata = "Metadata.xml";//Ƶ��ģ��Ԫ�����ļ�
            public const string File_Configuration = "Configuration.xml";
            public const string File_Seo = "Seo.xml";
            public const string File_StlTag = "StlTag.xml";
            public const string File_ContentModel = "ContentModel.xml";//�Զ�����ӵ�����ģ��
        }

        public class IndependentTemplates
        {
            //�ļ���
            public const string IndependentTemplateMetadata = "IndependentTemplateMetadata";//�洢Ƶ��ģ��Ԫ���ݵ��ļ�������
            public const string SiteContent = "SiteContent";//Ƶ�����ݵ��뵼����ʱ�ļ�����

            //SiteServer CMS�����ļ�
            public const string File_Template = "Template.xml";//���л�ģ����ļ���
            public const string File_Metadata = "Metadata.xml";//Ƶ��ģ��Ԫ�����ļ�
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
        /// ��֤���ַ����Ƿ������Ϊ�ļ�������
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
        /// ��ȡ�ļ����ļ���·�������pathΪ�ļ��У���������
        /// </summary>
        /// <param name="path">�ļ�·��</param>
        /// <returns></returns>
        public static string GetDirectoryPath(string path)
        {
            string directoryPath;
            string ext = Path.GetExtension(path);
            if (!string.IsNullOrEmpty(ext))		//pathΪ�ļ�·��
            {
                directoryPath = Path.GetDirectoryName(path);
            }
            else									//pathΪ�ļ���·��
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
            //����ṩ��·���в�����ĩβ�ָ����������ĩβ�ָ�����
            if (!srcDirectoryPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                srcDirectoryPath += Path.DirectorySeparatorChar;
            }
            if (!destDirectoryPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                destDirectoryPath += Path.DirectorySeparatorChar;
            }

            //���Ŀ��Ŀ¼�����ڣ������Դ����� 
            DirectoryUtils.CreateDirectoryIfNotExists(destDirectoryPath);

            //�ӵ�ǰ��Ŀ¼�л�ȡĿ¼�б� 
            foreach (string srcDir in Directory.GetDirectories(srcDirectoryPath))
            {
                string directoryName = PathUtils.GetDirectoryName(srcDir);

                DirectoryInfo srcDirInfo = new DirectoryInfo(srcDir);

                string destDir = destDirectoryPath + directoryName;
                //�����Ŀ¼�����ڣ��򴴽���Ŀ¼�� 
                DirectoryUtils.CreateDirectoryIfNotExists(destDir);
                //�������Ǵ��ڵݹ�ģʽ�£���˻�Ҫ������Ŀ¼
                MoveDirectory(srcDir, destDir, isOverride);
            }

            //�ӵ�ǰ��Ŀ¼�л�ȡ�ļ���
            foreach (string srcFile in Directory.GetFiles(srcDirectoryPath))
            {
                FileInfo srcFileInfo = new FileInfo(srcFile);
                FileInfo destFileInfo = new FileInfo(srcFile.Replace(srcDirectoryPath, destDirectoryPath));
                //����ļ������ڣ�����и��ơ� 
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
        /// ɾ���ļ���
        /// </summary>
        /// <param name="directoryPath">�ļ���·��</param>
        /// <returns>ɾ���������Ƿ����</returns>
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
            //ͨ��GetFiles����,��ȡĿ¼�������ļ��Ĵ�С
            foreach (string filePath in filePaths)
            {
                FileInfo info = new FileInfo(filePath);
                size += info.Length;
            }
            string[] directoryPaths = DirectoryUtils.GetDirectoryPaths(directoryPath);
            //��ȡĿ¼�������ļ��д�С,���浽һ���µĶ���������
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
