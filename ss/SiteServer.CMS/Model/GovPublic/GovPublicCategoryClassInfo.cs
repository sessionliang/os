namespace SiteServer.CMS.Model
{
	public class GovPublicCategoryClassInfo
	{
        private string classCode;
        private int publishmentSystemID;
        private string className;
        private bool isSystem;
        private bool isEnabled;
        private string contentAttributeName;
        private int taxis;
		private string description;

		public GovPublicCategoryClassInfo()
		{
            this.classCode = string.Empty;
            this.publishmentSystemID = 0;
            this.className = string.Empty;
            this.isSystem = false;
            this.isEnabled = true;
            this.contentAttributeName = string.Empty;
            this.taxis = 0;
			this.description = string.Empty;
		}

        public GovPublicCategoryClassInfo(string classCode, int publishmentSystemID, string className, bool isSystem, bool isEnabled, string contentAttributeName, int taxis, string description)
		{
            this.classCode = classCode;
            this.publishmentSystemID = publishmentSystemID;
            this.className = className;
            this.isSystem = isSystem;
            this.isEnabled = isEnabled;
            this.contentAttributeName = contentAttributeName;
            this.taxis = taxis;
            this.description = description;
		}

        public string ClassCode
        {
            get { return classCode; }
            set { classCode = value; }
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

        public bool IsSystem
        {
            get { return isSystem; }
            set { isSystem = value; }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        public string ContentAttributeName
        {
            get { return contentAttributeName; }
            set { contentAttributeName = value; }
        }

        public int Taxis
        {
            get { return taxis; }
            set { taxis = value; }
        }

		public string Description
		{
			get{ return description; }
			set{ description = value; }
		}

	}
}
