using System;
using System.Collections.Generic;
using System.Text;

namespace SiteServer.BBS.Model
{
    public class IdentifyInfo
    {
        private int id;
        private int publishmentSystemID;
        private string title;
        private string iconUrl;
        private string stampUrl;
        private int taxis;

        //public IdentifyInfo()
        //{
 
        //}

        public IdentifyInfo(int id, int publishmentSystemID, string title, string iconUrl, string stampUrl, int taxis)
        {
            this.id = id;
            this.publishmentSystemID = publishmentSystemID;
            this.title = title;
            this.iconUrl = iconUrl;
            this.stampUrl = stampUrl;
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

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string IconUrl
        {
            get { return iconUrl; }
            set { iconUrl = value; }
        }

        public string StampUrl
        {
            get { return stampUrl; }
            set { stampUrl = value; }
        }

        public int Taxis
        {
            get { return taxis; }
            set { taxis = value; }
        }
    }
}
