using System;

namespace BaiRong.Model
{
	public class ContentCheckInfo
	{
        private int checkID;
        private string tableName;
		private int publishmentSystemID;
        private int nodeID;
        private int contentID;
        private bool isAdmin;
        private string userName;
        private bool isChecked;
        private int checkedLevel;
        private DateTime checkDate;
        private string reasons;

		public ContentCheckInfo()
		{
            this.checkID = 0;
            this.tableName = string.Empty;
			this.publishmentSystemID = 0;
            this.nodeID = 0;
            this.contentID = 0;
            this.isAdmin = false;
            this.userName = string.Empty;
            this.isChecked = false;
            this.checkedLevel = 0;
            this.checkDate = DateTime.Now;
            this.reasons = string.Empty;
		}

        public ContentCheckInfo(int checkID, string tableName, int publishmentSystemID, int nodeID, int contentID, bool isAdmin, string userName, bool isChecked, int checkedLevel, DateTime checkDate, string reasons) 
		{
            this.checkID = checkID;
            this.tableName = tableName;
            this.publishmentSystemID = publishmentSystemID;
            this.nodeID = nodeID;
            this.contentID = contentID;
            this.isAdmin = isAdmin;
            this.userName = userName;
            this.isChecked = isChecked;
            this.checkedLevel = checkedLevel;
            this.checkDate = checkDate;
            this.reasons = reasons;
		}

        public int CheckID
		{
            get { return checkID; }
            set { checkID = value; }
		}

        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }

		public int PublishmentSystemID
		{
			get{ return publishmentSystemID; }
			set{ publishmentSystemID = value; }
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

        public bool IsAdmin
		{
			get{ return isAdmin; }
            set { isAdmin = value; }
		}

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }

        public int CheckedLevel
        {
            get { return checkedLevel; }
            set { checkedLevel = value; }
        }

        public DateTime CheckDate
        {
            get { return checkDate; }
            set { checkDate = value; }
        }

        public string Reasons
        {
            get { return reasons; }
            set { reasons = value; }
        }
	}
}
