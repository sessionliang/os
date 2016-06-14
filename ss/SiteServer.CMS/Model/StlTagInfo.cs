namespace SiteServer.CMS.Model
{
	public class StlTagInfo
	{
        private string tagName;
		private int publishmentSystemID;
        private string tagDescription;
        private string tagContent;

		public StlTagInfo()
		{
            this.tagName = string.Empty;
			this.publishmentSystemID = 0;
            this.tagDescription = string.Empty;
            this.tagContent = string.Empty;
		}

        public StlTagInfo(string tagName, int publishmentSystemID, string tagDescription, string tagContent) 
		{
            this.tagName = tagName;
			this.publishmentSystemID = publishmentSystemID;
            this.tagDescription = tagDescription;
            this.tagContent = tagContent;
		}

        public string TagName
		{
            get { return tagName; }
            set { tagName = value; }
		}

		public int PublishmentSystemID
		{
			get{ return publishmentSystemID; }
			set{ publishmentSystemID = value; }
		}

        public string TagDescription
		{
            get { return tagDescription; }
            set { tagDescription = value; }
		}

        public string TagContent
		{
            get { return tagContent; }
            set { tagContent = value; }
		}
	}
}
