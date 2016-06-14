using System;
namespace SiteServer.CRM.Model
{
	public class ReplyInfo
	{
        private int replyID;
        private int applyID;
        private string reply;
        private string fileUrl;
        private int departmentID;
        private string userName;
        private DateTime addDate;

		public ReplyInfo()
		{
            this.replyID = 0;
            this.applyID = 0;
            this.reply = string.Empty;
            this.fileUrl = string.Empty;
            this.departmentID = 0;
            this.userName = string.Empty;
            this.addDate = DateTime.Now;
		}

        public ReplyInfo(int replyID, int applyID, string reply, string fileUrl, int departmentID, string userName, DateTime addDate)
		{
            this.replyID = replyID;
            this.applyID = applyID;
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

        public int ApplyID
        {
            get { return applyID; }
            set { applyID = value; }
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
