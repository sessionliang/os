using System;
using System.Collections.Generic;
using System.Text;

namespace SiteServer.BBS.Model
{
    public class AnnouncementInfo
    {
        private int id;
        private int publishmentSystemID;
        private int taxis;
        private DateTime addDate;
        private string title;
        private string formatString;
        private string linkUrl;
        private bool isBlank;

        //public AnnouncementInfo()
        //{
        //}

        public AnnouncementInfo(int id, int publishmentSystemID, int taxis, DateTime addDate, string title, string formatString, string linkUrl, bool isBlank)
        {
            this.id = id;
            this.publishmentSystemID = publishmentSystemID;
            this.taxis = taxis;
            this.addDate = addDate;
            this.title = title;
            this.formatString = formatString;
            this.linkUrl = linkUrl;
            this.isBlank = isBlank;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public int Taxis
        {
            get { return taxis; }
            set { taxis = value; }
        }

        public DateTime AddDate
        {
            get { return addDate; }
            set { addDate = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string FormatString
        {
            get { return formatString; }
            set { formatString = value; }
        }

        public string LinkUrl
        {
            get { return linkUrl; }
            set { linkUrl = value; }
        }

        public bool IsBlank
        {
            get { return isBlank; }
            set { isBlank = value; }
        }
    }
}