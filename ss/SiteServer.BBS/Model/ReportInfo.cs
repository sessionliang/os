using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Model;

namespace SiteServer.BBS.Model
{
    public class ReportInfo : ExtendedAttributes
    {
        private int id;
        private int publishmentSystemID;
        private int forumID;
        private int threadID;
        private int postID;
        private string userName;
        private string ipAddress;
        private DateTime addDate;
        private string content;

        //public ReportInfo()
        //{
        //    this.id = 0;
        //    this.publishmentSystemID = 0;
        //    this.forumID = 0;
        //    this.threadID = 0;
        //    this.postID = 0;
        //    this.userName = string.Empty;
        //    this.ipAddress = string.Empty;
        //    this.addDate = DateTime.Now;
        //    this.content = string.Empty;
        //}

        public ReportInfo(object dataItem)
            : base(dataItem)
		{
		}

        public ReportInfo(int id, int publishmentSystemID, int forumID, int threadID, int postID, string userName, string ipAddress, DateTime addDate, string content)
        {
            this.id = id;
            this.publishmentSystemID = publishmentSystemID;
            this.forumID = forumID;
            this.threadID = threadID;
            this.postID = postID;
            this.userName = userName;
            this.ipAddress = ipAddress;
            this.addDate = addDate;
            this.content = content;
          
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

        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        
    }
}
