using System;
namespace SiteServer.CMS.Model
{
	public class GovInteractRemarkInfo
	{
        private int remarkID;
        private int publishmentSystemID;
        private int nodeID;
        private int contentID;
        private EGovInteractRemarkType remarkType;
        private string remark;
        private int departmentID;
        private string userName;
        private DateTime addDate;

		public GovInteractRemarkInfo()
		{
            this.remarkID = 0;
            this.publishmentSystemID = 0;
            this.nodeID = 0;
            this.contentID = 0;
            this.remarkType = EGovInteractRemarkType.Accept;
            this.remark = string.Empty;
            this.departmentID = 0;
            this.userName = string.Empty;
            this.addDate = DateTime.Now;
		}

        public GovInteractRemarkInfo(int remarkID, int publishmentSystemID, int nodeID, int contentID, EGovInteractRemarkType remarkType, string remark, int departmentID, string userName, DateTime addDate)
		{
            this.remarkID = remarkID;
            this.publishmentSystemID = publishmentSystemID;
            this.nodeID = nodeID;
            this.contentID = contentID;
            this.remarkType = remarkType;
            this.remark = remark;
            this.departmentID = departmentID;
            this.userName = userName;
            this.addDate = addDate;
		}

        public int RemarkID
        {
            get { return remarkID; }
            set { remarkID = value; }
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

        public EGovInteractRemarkType RemarkType
        {
            get { return remarkType; }
            set { remarkType = value; }
        }

        public string Remark
        {
            get { return remark; }
            set { remark = value; }
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
