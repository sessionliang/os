using System;
using System.Collections.Generic;
using System.Text;

namespace SiteServer.BBS.Model
{
    public class FaceInfo
    {
        private int id;
        private int publishmentSystemID;
        private string faceName;
        private string title;
        private int taxis;
        private bool isEnabled;

        //public FaceInfo()
        //{

        //}

        public FaceInfo(int id, int publishmentSystemID, string faceName, string title, int taxis, bool isEnabled)
        {
            this.id = id;
            this.publishmentSystemID = publishmentSystemID;
            this.faceName = faceName;
            this.title = title;
            this.taxis = taxis;
            this.isEnabled = isEnabled;
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

        public string FaceName
        {
            get { return faceName; }
            set { faceName = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public int Taxis
        {
            get { return taxis; }
            set { taxis = value; }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }
    }
}