using System;
using System.Web;
using System.Xml.Serialization;

namespace SiteServer.Project.Model
{
    public class UrlInfo
    {
        private int id;
        private string guid;
        private int site;
        private string version;
        private bool isBeta;
        private string databaseType;
        private string dotnet;
        private string domain;
        private DateTime addDate;
        private DateTime lastActivityDate;
        private int countOfActivity;
        private bool isExpire;
        private DateTime expireDate;
        private string expireReason;
        private string summary;

        public UrlInfo()
        {
            this.id = 0;
            this.guid = string.Empty;
            this.site = 0;
            this.version = string.Empty;
            this.isBeta = false;
            this.databaseType = string.Empty;
            this.dotnet = string.Empty;
            this.domain = string.Empty;
            this.addDate = DateTime.Now;
            this.lastActivityDate = DateTime.Now;
            this.countOfActivity = 0;
            this.isExpire = false;
            this.expireDate = DateTime.Now;
            this.expireReason = string.Empty;
            this.summary = string.Empty;
        }

        public UrlInfo(int id, string guid, int site, string version, bool isBeta, string databaseType, string dotnet, string domain, DateTime addDate, DateTime lastActivityDate, int countOfActivity, bool isExpire, DateTime expireDate, string expireReason, string summary)
        {
            this.id = id;
            this.guid = guid;
            this.site = site;
            this.version = version;
            this.isBeta = isBeta;
            this.databaseType = databaseType;
            this.dotnet = dotnet;
            this.domain = domain;
            this.addDate = addDate;
            this.lastActivityDate = lastActivityDate;
            this.countOfActivity = countOfActivity;
            this.isExpire = isExpire;
            this.expireDate = expireDate;
            this.expireReason = expireReason;
            this.summary = summary;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string GUID
        {
            get { return guid; }
            set { guid = value; }
        }

        public int Site
        {
            get { return site; }
            set { site = value; }
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

        public string DatabaseType
        {
            get { return databaseType; }
            set { databaseType = value; }
        }

        public string Dotnet
        {
            get { return dotnet; }
            set { dotnet = value; }
        }

        public string Domain
        {
            get { return domain; }
            set { domain = value; }
        }

        public DateTime AddDate
        {
            get { return addDate; }
            set { addDate = value; }
        }

        public DateTime LastActivityDate
        {
            get { return lastActivityDate; }
            set { lastActivityDate = value; }
        }

        public int CountOfActivity
        {
            get { return countOfActivity; }
            set { countOfActivity = value; }
        }

        public bool IsExpire
        {
            get { return isExpire; }
            set { isExpire = value; }
        }

        public DateTime ExpireDate
        {
            get { return expireDate; }
            set { expireDate = value; }
        }

        public string ExpireReason
        {
            get { return expireReason; }
            set { expireReason = value; }
        }

        public string Summary
        {
            get { return summary; }
            set { summary = value; }
        }
    }
}
