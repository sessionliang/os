using System;

namespace SiteServer.Project.Model
{
	public class ProjectInfo
	{
        private int projectID;
        private string projectName;
        private string projectType;
        private bool isContract;
        private int amountTotal;
        private int amountCashBack;
        private string contractNO;
		private string description;
        private DateTime addDate;
        private bool isClosed;
        private DateTime closeDate;
        private string userNameAM;
        private string userNamePM;
        private string userNameCollection;
        private string settingsXML;

		public ProjectInfo()
		{
            this.projectID = 0;
            this.projectName = string.Empty;
            this.projectType = string.Empty;
            this.isContract = false;
            this.amountTotal = 0;
            this.amountCashBack = 0;
            this.contractNO = string.Empty;
			this.description = string.Empty;
            this.addDate = DateTime.Now;
            this.isClosed = false;
            this.closeDate = DateTime.Now;
            this.userNameAM = string.Empty;
            this.userNamePM = string.Empty;
            this.userNameCollection = string.Empty;
            this.settingsXML = string.Empty;
		}

        public ProjectInfo(int projectID, string projectName, string projectType, bool isContract, int amountTotal, int amountCashBack, string contractNO, string description, DateTime addDate, bool isClosed, DateTime closeDate, string userNameAM, string userNamePM, string userNameCollection, string settingsXML)
		{
            this.projectID = projectID;
            this.projectName = projectName;
            this.projectType = projectType;
            this.isContract = isContract;
            this.amountTotal = amountTotal;
            this.amountCashBack = amountCashBack;
            this.contractNO = contractNO;
            this.description = description;
            this.addDate = addDate;
            this.isClosed = isClosed;
            this.closeDate = closeDate;
            this.userNameAM = userNameAM;
            this.userNamePM = userNamePM;
            this.userNameCollection = userNameCollection;
            this.settingsXML = settingsXML;
		}

        public int ProjectID
        {
            get { return projectID; }
            set { projectID = value; }
        }

        public string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }

        public string ProjectType
        {
            get { return projectType; }
            set { projectType = value; }
        }

        public bool IsContract
        {
            get { return isContract; }
            set { isContract = value; }
        }

        public int AmountTotal
        {
            get { return amountTotal; }
            set { amountTotal = value; }
        }

        public int AmountCashBack
        {
            get { return amountCashBack; }
            set { amountCashBack = value; }
        }

        public string ContractNO
        {
            get { return contractNO; }
            set { contractNO = value; }
        }

		public string Description
		{
			get{ return description; }
			set{ description = value; }
		}

        public DateTime AddDate
        {
            get { return addDate; }
            set { addDate = value; }
        }

        public bool IsClosed
        {
            get { return isClosed; }
            set { isClosed = value; }
        }

        public DateTime CloseDate
        {
            get { return closeDate; }
            set { closeDate = value; }
        }

        public string UserNameAM
        {
            get { return userNameAM; }
            set { userNameAM = value; }
        }

        public string UserNamePM
        {
            get { return userNamePM; }
            set { userNamePM = value; }
        }

        public string UserNameCollection
        {
            get { return userNameCollection; }
            set { userNameCollection = value; }
        }

        public string SettingsXML
        {
            get { return settingsXML; }
            set
            {
                this.additional = null;
                settingsXML = value;
            }
        }

        ProjectInfoExtend additional;
        public ProjectInfoExtend Additional
        {
            get
            {
                if (this.additional == null)
                {
                    this.additional = new ProjectInfoExtend(this.settingsXML);
                }
                return this.additional;
            }
        }
	}
}
