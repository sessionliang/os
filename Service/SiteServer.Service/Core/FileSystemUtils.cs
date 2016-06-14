using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteServer.Service.Core
{
    public class FileSystemUtils
    {
        public static char DirectorySeparatorChar = Path.DirectorySeparatorChar;

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

        public static bool IsFileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public static void WriteText(string filePath, string content)
        {
            StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8);
            sw.Write(content);
            sw.Flush();
            sw.Close();
        }

        public static bool IsDirectoryExists(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

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

        public static void CopyDirectory(string sourcePath, string targetPath)
        {
            CopyDirectory(sourcePath, targetPath, true);
        }

        public static void CopyDirectory(string sourcePath, string targetPath, bool isOverride)
        {
            if (Directory.Exists(sourcePath))
            {
                FileSystemUtils.CreateDirectoryIfNotExists(targetPath);
                DirectoryInfo directoryInfo = new DirectoryInfo(sourcePath);
                if (directoryInfo.GetFileSystemInfos() != null)
                {
                    foreach (FileSystemInfo fileSystemInfo in directoryInfo.GetFileSystemInfos())
                    {
                        string destPath = Path.Combine(targetPath, fileSystemInfo.Name);
                        if (fileSystemInfo is FileInfo)
                        {
                            FileSystemUtils.CopyFile(fileSystemInfo.FullName, destPath, isOverride);
                        }
                        else if (fileSystemInfo is DirectoryInfo)
                        {
                            CopyDirectory(fileSystemInfo.FullName, destPath, isOverride);
                        }
                    }
                }
            }
        }

        public static bool CopyFile(string sourceFilePath, string destFilePath)
        {
            return CopyFile(sourceFilePath, destFilePath, true);
        }

        public static bool CopyFile(string sourceFilePath, string destFilePath, bool isOverride)
        {
            bool retval = true;
            try
            {
                FileSystemUtils.CreateDirectoryIfNotExists(destFilePath);

                File.Copy(sourceFilePath, destFilePath, isOverride);

            }
            catch
            {
                retval = false;
            }
            return retval;
        }
    }
}
