using System;
namespace SiteServer.Project.Model
{
	public class RemarkInfo
	{
        private int remarkID;
        private int applyID;
        private ERemarkType remarkType;
        private string remark;
        private int departmentID;
        private string userName;
        private DateTime addDate;

		public RemarkInfo()
		{
            this.remarkID = 0;
            this.applyID = 0;
            this.remarkType = ERemarkType.Accept;
            this.remark = string.Empty;
            this.departmentID = 0;
            this.userName = string.Empty;
            this.addDate = DateTime.Now;
		}

        public RemarkInfo(int remarkID, int applyID, ERemarkType remarkType, string remark, int departmentID, string userName, DateTime addDate)
		{
            this.remarkID = remarkID;
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

        public int ApplyID
        {
            get { return applyID; }
            set { applyID = value; }
        }

        public ERemarkType RemarkType
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
