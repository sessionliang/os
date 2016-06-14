using System;
using System.Collections.Generic;
using System.Text;

namespace SiteServer.BBS.Model
{
    public class LinkInfo
    {
        private int id;
        private int publishmentSystemID;
        private string linkName;
        private string linkUrl;
        private string iconUrl;
        private int taxis;

        //public LinkInfo()
        //{
        //    this.id = 0;
        //    this.publishmentSystemID = 0;
        //    this.linkName = string.Empty;
        //    this.linkUrl = string.Empty;
        //    this.iconUrl = string.Empty;
        //    this.taxis = 0;
        //}

        public LinkInfo(int id, int publishmentSystemID, string linkName, string linkUrl, string iconUrl, int taxis)
        {
            this.id = id;
            this.publishmentSystemID = publishmentSystemID;
            this.linkName = linkName;
            this.linkUrl = linkUrl;
            this.iconUrl = iconUrl;
            this.taxis = taxis;
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

        public string LinkName
        {
            get { return linkName; }
            set { linkName = value; }
        }

        public string LinkUrl
        {
            get { return linkUrl; }
            set { linkUrl = value; }
        }

        public string IconUrl
        {
            get { return iconUrl; }
            set { iconUrl = value; }
        }

        public int Taxis
        {
            get { return taxis; }
            set { taxis = value; }
        }
    }
}