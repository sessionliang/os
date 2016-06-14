using System;
namespace SiteServer.CMS.Model
{
	public class GovInteractReplyInfo
	{
        private int replyID;
        private int publishmentSystemID;
        private int nodeID;
        private int contentID;
        private string reply;
        private string fileUrl;
        private int departmentID;
        private string userName;
        private DateTime addDate;

		public GovInteractReplyInfo()
		{
            this.replyID = 0;
            this.publishmentSystemID = 0;
            this.nodeID = 0;
            this.contentID = 0;
            this.reply = string.Empty;
            this.fileUrl = string.Empty;
            this.departmentID = 0;
            this.userName = string.Empty;
            this.addDate = DateTime.Now;
		}

        public GovInteractReplyInfo(int replyID, int publishmentSystemID, int nodeID, int contentID, string reply, string fileUrl, int departmentID, string userName, DateTime addDate)
		{
            this.replyID = replyID;
            this.publishmentSystemID = publishmentSystemID;
            this.nodeID = nodeID;
            this.contentID = contentID;
            this.reply = reply;
            this.fileUrl = fileUrl;
            this.departmentID = departmentID;
            this.userName = userName;
            this.addDate = addDate;
		}

        public int AcceptID
        {
            get { return replyID; }
            set { replyID = value; }
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public int NodeID
        {
            get { return nodeID; }
            set { nodeID = value; }
        }

        public int ContentID
        {
            get { return contentID; }
            set { contentID = value; }
        }

        public string Reply
        {
            get { return reply; }
            set { reply = value; }
        }

        public string FileUrl
        {
            get { return fileUrl; }
            set { fileUrl = value; }
        }
        
        public int DepartmentID
        {
            get { return departmentID; }
            set { departmentID = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public DateTime AddDate
        {
            get { return addDate; }
            set { addDate = value; }
        }
	}
}
