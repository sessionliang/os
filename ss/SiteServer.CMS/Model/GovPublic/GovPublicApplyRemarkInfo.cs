using System;
namespace SiteServer.CMS.Model
{
	public class GovPublicApplyRemarkInfo
	{
        private int remarkID;
        private int publishmentSystemID;
        private int applyID;
        private EGovPublicApplyRemarkType remarkType;
        private string remark;
        private int departmentID;
        private string userName;
        private DateTime addDate;

		public GovPublicApplyRemarkInfo()
		{
            this.remarkID = 0;
            this.publishmentSystemID = 0;
            this.applyID = 0;
            this.remarkType = EGovPublicApplyRemarkType.Accept;
            this.remark = string.Empty;
            this.departmentID = 0;
            this.userName = string.Empty;
            this.addDate = DateTime.Now;
		}

        public GovPublicApplyRemarkInfo(int remarkID, int publishmentSystemID, int applyID, EGovPublicApplyRemarkType remarkType, string remark, int departmentID, string userName, DateTime addDate)
		{
            this.remarkID = remarkID;
            this.publishmentSystemID = publishmentSystemID;
            this.applyID = applyID;
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

        public int ApplyID
        {
            get { return applyID; }
            set { applyID = value; }
        }

        public EGovPublicApplyRemarkType RemarkType
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
