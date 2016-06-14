namespace SiteServer.CMS.Model
{
	public class ContentGroupInfo
	{
		private string contentGroupName;
		private int publishmentSystemID;
        private int taxis;
		private string description;

		public ContentGroupInfo()
		{
			this.contentGroupName = string.Empty;
			this.publishmentSystemID = 0;
            this.taxis = 0;
			this.description = string.Empty;
		}

		public ContentGroupInfo(string contentGroupName, int publishmentSystemID, int taxis, string description) 
		{
			this.contentGroupName = contentGroupName;
			this.publishmentSystemID = publishmentSystemID;
            this.taxis = taxis;
			this.description = description;
		}

		public string ContentGroupName
		{
			get{ return contentGroupName; }
			set{ contentGroupName = value; }
		}

		public int PublishmentSystemID
		{
			get{ return publishmentSystemID; }
			set{ publishmentSystemID = value; }
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
