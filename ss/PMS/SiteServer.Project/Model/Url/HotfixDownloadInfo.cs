using System;
using System.Web;
using System.Xml.Serialization;

namespace SiteServer.Project.Model
{
    public class HotfixDownloadInfo
    {
        private int id;
        private int hotfixID;
        private string version;
        private bool isBeta;
        private string domain;
        private DateTime downloadDate;

        public HotfixDownloadInfo()
        {
            this.id = 0;
            this.hotfixID = 0;
            this.version = string.Empty;
            this.isBeta = false;
            this.domain = string.Empty;
            this.downloadDate = DateTime.Now;
        }

        public HotfixDownloadInfo(int id, int hotfixID, string version, bool isBeta, string domain, DateTime downloadDate)
        {
            this.id = id;
            this.hotfixID = hotfixID;
            this.version = version;
            this.isBeta = isBeta;
            this.domain = domain;
            this.downloadDate = downloadDate;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public int HotfixID
        {
            get { return hotfixID; }
            set { hotfixID = value; }
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

        public string Domain
        {
            get { return domain; }
            set { domain = value; }
        }

        public DateTime DownloadDate
        {
            get { return downloadDate; }
            set { downloadDate = value; }
        }
    }
}
