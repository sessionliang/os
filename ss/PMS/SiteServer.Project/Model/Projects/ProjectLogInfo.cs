using System;
namespace SiteServer.Project.Model
{
	public class ProjectLogInfo
	{
        private int logID;
        private int applyID;
        private int departmentID;
        private string userName;
        private EProjectLogType logType;
        private string ipAddress;
        private DateTime addDate;
        private string summary;

		public ProjectLogInfo()
		{
            this.logID = 0;
            this.applyID = 0;
            this.departmentID = 0;
            this.userName = string.Empty;
            this.logType = EProjectLogType.New;
            this.ipAddress = string.Empty;
            this.addDate = DateTime.Now;
            this.summary = string.Empty;
		}

        public ProjectLogInfo(int logID, int applyID, int departmentID, string userName, EProjectLogType logType, string ipAddress, DateTime addDate, string summary)
		{
            this.logID = logID;
            this.applyID = applyID;
            this.departmentID = departmentID;
            this.userName = userName;
            this.logType = logType;
            this.ipAddress = ipAddress;
            this.addDate = addDate;
            this.summary = summary;
		}

        public int LogID
        {
            get { return logID; }
            set { logID = value; }
        }

        public int ApplyID
        {
            get { return applyID; }
            set { applyID = value; }
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

        public EProjectLogType LogType
        {
            get { return logType; }
            set { logType = value; }
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

        public string Summary
        {
            get { return summary; }
            set { summary = value; }
        }
	}
}
