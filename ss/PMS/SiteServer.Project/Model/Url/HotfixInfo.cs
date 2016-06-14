using System;
using System.Web;
using System.Xml.Serialization;

namespace SiteServer.Project.Model
{
    public class HotfixInfo
    {
        private int id;
        private string version;
        private bool isBeta;
        private string fileUrl;
        private string pageUrl;
        private DateTime pubDate;
        private string message;
        private bool isEnabled;
        private bool isRestrict;
        private string restrictDomain;
        private string restrictProductIDCollection;
        private string restrictDatabase;
        private string restrictVersion;
        private bool restrictIsBeta;
        private int downloadCount;

        public HotfixInfo()
        {
            this.id = 0;
            this.version = string.Empty;
            this.isBeta = false;
            this.fileUrl = string.Empty;
            this.pageUrl = string.Empty;
            this.pubDate = DateTime.Now;
            this.message = string.Empty;
            this.isEnabled = false;
            this.isRestrict = false;
            this.restrictDomain = string.Empty;
            this.restrictProductIDCollection = string.Empty;
            this.restrictDatabase = string.Empty;
            this.restrictVersion = string.Empty;
            this.restrictIsBeta = false;
            this.downloadCount = 0;
        }

        public HotfixInfo(int id, string version, bool isBeta, string fileUrl, string pageUrl, DateTime pubDate, string message, bool isEnabled, bool isRestrict, string restrictDomain, string restrictProductIDCollection, string restrictDatabase, string restrictVersion, bool restrictIsBeta, int downloadCount)
        {
            this.id = id;
            this.version = version;
            this.isBeta = isBeta;
            this.fileUrl = fileUrl;
            this.pageUrl = pageUrl;
            this.pubDate = pubDate;
            this.message = message;
            this.isEnabled = isEnabled;
            this.isRestrict = isRestrict;
            this.restrictDomain = restrictDomain;
            this.restrictProductIDCollection = restrictProductIDCollection;
            this.restrictDatabase = restrictDatabase;
            this.restrictVersion = restrictVersion;
            this.restrictIsBeta = restrictIsBeta;
            this.downloadCount = downloadCount;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        public bool IsBeta
        {
            get { return isBeta; }
            set { isBeta = value; }
        }

        public string FileUrl
        {
            get { return fileUrl; }
            set { fileUrl = value; }
        }

        public string PageUrl
        {
            get { return pageUrl; }
            set { pageUrl = value; }
        }

        public DateTime PubDate
        {
            get { return pubDate; }
            set { pubDate = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        public bool IsRestrict
        {
            get { return isRestrict; }
            set { isRestrict = value; }
        }

        public string RestrictDomain
        {
            get { return restrictDomain; }
            set { restrictDomain = value; }
        }

        public string RestrictProductIDCollection
        {
            get { return restrictProductIDCollection; }
            set { restrictProductIDCollection = value; }
        }

        public string RestrictDatabase
        {
            get { return restrictDatabase; }
            set { restrictDatabase = value; }
        }

        public string RestrictVersion
        {
            get { return restrictVersion; }
            set { restrictVersion = value; }
        }

        public bool RestrictIsBeta
        {
            get { return restrictIsBeta; }
            set { restrictIsBeta = value; }
        }

        public int DownloadCount
        {
            get { return downloadCount; }
            set { downloadCount = value; }
        }
    }
}
