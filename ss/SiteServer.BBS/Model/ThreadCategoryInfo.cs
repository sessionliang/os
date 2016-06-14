using System;
using System.Collections.Generic;
using System.Text;

namespace SiteServer.BBS.Model
{
    public class ThreadCategoryInfo
    {
        private int categoryID;
        private int publishmentSystemID;
        private int forumID;
        private string categoryName;
        private string summary;
        private int taxis;

        public ThreadCategoryInfo(int publishmentSystemID)
        {
            this.categoryID = 0;
            this.publishmentSystemID = publishmentSystemID;
            this.forumID = 0;
            this.categoryName = string.Empty;
            this.summary = string.Empty;
            this.taxis = 0;
        }

        public ThreadCategoryInfo(int categoryID, int publishmentSystemID, int forumID, string categoryName, string summary, int taxis)
        {

            //this.categoryID = 0;
            //this.publishmentSystemID = 0;
            //this.forumID = 0;
            //this.categoryName = categoryName;
            //this.summary = summary;
            //this.taxis = taxis;

            //by 20151201 sofuny
            this.categoryID = categoryID;
            this.publishmentSystemID = publishmentSystemID;
            this.forumID = forumID;
            this.categoryName = categoryName;
            this.summary = summary;
            this.taxis = taxis;
        }

        public int CategoryID
        {
            get
            {
                return categoryID;
            }
            set
            {
                categoryID = value;
            }
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public int ForumID
        {
            get
            {
                return forumID;
            }
            set
            {
                forumID = value;
            }
        }

        public string CategoryName
        {
            get
            {
                return categoryName;
            }
            set
            {
                categoryName = value;
            }
        }

        public string Summary
        {
            get
            {
                return summary;
            }
            set
            {
                summary = value;
            }
        }
        public int Taxis
        {
            get
            {
                return taxis;
            }
            set
            {
                taxis = value;
            }
        }
    }
}