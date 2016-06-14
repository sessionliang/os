using System;
using System.Collections.Generic;
using System.Text;

namespace SiteServer.BBS.Model
{
    public class NavigationInfo
    {
        private int id;
        private int publishmentSystemID;
        private int taxis;
        private ENavType navType;
        private string title;
        private string formatString;
        private string linkUrl;
        private bool isBlank;

        //public NavigationInfo()
        //{
        //}

        public NavigationInfo(int id, int publishmentSystemID, int taxis, ENavType navType, string title, string formatString, string linkUrl, bool isBlank)
        {
            this.id = id;
            this.publishmentSystemID = publishmentSystemID;
            this.taxis = taxis;
            this.navType = navType;
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

        public ENavType NavType
        {
            get { return navType; }
            set { navType = value; }
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