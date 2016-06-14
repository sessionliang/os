using System;

namespace SiteServer.BBS.Model
{
    public class BBSLogInfo
    {
        private int id;
        private int publishmentSystemID;
        private int forumID;
        private int threadID;
        private int postID;
        private string userName;
        private string ipAddress;
        private DateTime addDate;
        private string action;
        private string summary;

        //public BBSLogInfo()
        //{
        //    this.id = 0;
        //    this.publishmentSystemID = 0;
        //    this.forumID = 0;
        //    this.threadID = 0;
        //    this.postID = 0;
        //    this.userName = string.Empty;
        //    this.ipAddress = string.Empty;
        //    this.addDate = DateTime.Now;
        //    this.action = string.Empty;
        //    this.summary = string.Empty;
        //}

        public BBSLogInfo(int id, int publishmentSystemID, int forumID, int threadID, int postID, string userName, string ipAddress, DateTime addDate, string action, string summary)
        {
            this.id = id;
            this.publishmentSystemID = publishmentSystemID;
            this.forumID = forumID;
            this.threadID = threadID;
            this.postID = postID;
            this.userName = userName;
            this.ipAddress = ipAddress;
            this.addDate = addDate;
            this.action = action;
            this.summary = summary;
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

        public int ForumID
        {
            get { return forumID; }
            set { forumID = value; }
        }

        public int ThreadID
        {
            get { return threadID; }
            set { threadID = value; }
        }

        public int PostID
        {
            get { return postID; }
            set { postID = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string IPAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        public DateTime AddDate
        {
            get { return addDate; }
            set { addDate = value; }
        }

        public string Action
        {
            get { return action; }
            set { action = value; }
        }

        public string Summary
        {
            get { return summary; }
            set { summary = value; }
        }
    }
}
