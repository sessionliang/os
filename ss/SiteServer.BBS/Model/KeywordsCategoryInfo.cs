using System;
using System.Collections.Generic;
using System.Text;

namespace SiteServer.BBS.Model
{
    public class KeywordsCategoryInfo
    {
        private int categoryID;
        private int publishmentSystemID;
        private string categoryName;
        private bool isOpen;
        private int taxis;

        //public KeywordsCategoryInfo()
        //{
 
        //}

        public KeywordsCategoryInfo(int categoryID, int publishmentSystemID, string categoryName, bool isOpen, int taxis)
        {
            this.categoryID = categoryID;
            this.publishmentSystemID = publishmentSystemID;
            this.categoryName = categoryName;
            this.isOpen = isOpen;
            this.taxis = taxis;
        }

        public int CategoryID
        {
            get { return categoryID; }
            set { categoryID = value; }
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }

        public bool IsOpen
        {
            get { return isOpen; }
            set { isOpen = value; }
        }

        public int Taxis
        {
            get { return taxis; }
            set { taxis = value; }
        }
    }
}
