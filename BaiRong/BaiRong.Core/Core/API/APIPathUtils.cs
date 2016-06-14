using System;
using System.Collections.Generic;

namespace BaiRong.Core
{
    public class APIPathUtils
    {
        public static string GetPath(string relatedPath)
        {
            if (!string.IsNullOrEmpty(relatedPath))
            {
                return PathUtils.Combine(ConfigUtils.Instance.PhysicalApplicationPath, relatedPath);
            }
            else
            {
                return ConfigUtils.Instance.PhysicalApplicationPath;
            }
        }

        public static string GetUploadDirectoryPath()
        {
            DateTime now = DateTime.Now;
            string directoryPath = PathUtils.Combine(APIPathUtils.GetPath("upload"), now.Year.ToString(), now.Month.ToString());

            DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);
            return directoryPath;
        }

        public static string GetUploadFileName(string filePath)
        {
            DateTime now = DateTime.Now;
            string strDateTime = string.Format("{0}{1}{2}{3}{4}", now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);
            return string.Format("{0}{1}", strDateTime, PathUtils.GetExtension(filePath));
        }
    }
}