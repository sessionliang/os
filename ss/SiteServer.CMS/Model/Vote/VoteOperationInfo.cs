using System;

namespace BaiRong.Model
{
	public class VoteOperationInfo
	{
        private int operationID;
        private int publishmentSystemID;
        private int nodeID;
        private int contentID;
		private string ipAddress;
        private string userName;
		private DateTime addDate;

		public VoteOperationInfo()
		{
            this.operationID = 0;
            this.publishmentSystemID = 0;
            this.nodeID = 0;
            this.contentID = 0;
			this.ipAddress = string.Empty;
            this.userName = string.Empty;
			this.addDate = DateTime.Now;
		}

        public VoteOperationInfo(int operationID, int publishmentSystemID, int nodeID, int contentID, string ipAddress, string userName, DateTime addDate) 
		{
            this.operationID = operationID;
            this.publishmentSystemID = publishmentSystemID;
            this.nodeID = nodeID;
            this.contentID = contentID;
			this.ipAddress = ipAddress;
            this.userName = userName;
			this.addDate = addDate;
		}

        public int OperationID
		{
            get { return operationID; }
            set { operationID = value; }
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

		public string IPAddress
		{
			get{ return ipAddress; }
			set{ ipAddress = value; }
		}

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

		public DateTime AddDate
		{
			get{ return addDate; }
			set{ addDate = value; }
		}
	}
}
