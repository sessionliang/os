using System;
using System.IO;

using BaiRong.Core;
using BaiRong.Core.IO;

namespace BaiRong.Core.IO
{
    public class UploadManager
    {
        private string applicationName;
        private string userName;
        private string relatedPath;

        public UploadManager(string applicationName, string userName, DateTime datetime)
        {
            this.applicationName = applicationName;
            this.userName = userName;
            this.relatedPath = string.Format("{0}_{1}_{2}", datetime.Year, datetime.Month, datetime.Day);
        }

        public string DirectoryPath
        {
            get
            {
                string path = PathUtils.GetClientUserPath(this.applicationName, this.userName, this.relatedPath);
                DirectoryUtils.CreateDirectoryIfNotExists(path);
                return path;
            }
        }

        public string DirectoryUrl
        {
            get
            {
                return PageUtils.GetClientUserUrl(this.applicationName, this.userName, this.relatedPath);
            }
        }
    }
}
