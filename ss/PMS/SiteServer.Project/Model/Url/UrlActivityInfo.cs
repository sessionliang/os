using System;
using System.Web;
using System.Xml.Serialization;

namespace SiteServer.Project.Model
{
    public class UrlActivityInfo
    {
        private int id;
        private int urlID;
        private DateTime activityDate;

        public UrlActivityInfo()
        {
            this.id = 0;
            this.urlID = 0;
            this.activityDate = DateTime.Now;
        }

        public UrlActivityInfo(int id, int urlID, DateTime activityDate)
        {
            this.id = id;
            this.urlID = urlID;
            this.activityDate = activityDate;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public int UrlID
        {
            get { return urlID; }
            set { urlID = value; }
        }

        public DateTime ActivityDate
        {
            get { return activityDate; }
            set { activityDate = value; }
        }
    }
}
