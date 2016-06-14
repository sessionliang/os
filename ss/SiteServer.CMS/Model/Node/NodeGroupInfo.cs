namespace SiteServer.CMS.Model
{
	public class NodeGroupInfo
	{
		private string nodeGroupName;
		private int publishmentSystemID;
        private int taxis;
		private string description;

		public NodeGroupInfo()
		{
			this.nodeGroupName = string.Empty;
			this.publishmentSystemID = 0;
            this.taxis = 0;
			this.description = string.Empty;
		}

		public NodeGroupInfo(string nodeGroupName, int publishmentSystemID, int taxis, string description) 
		{
			this.nodeGroupName = nodeGroupName;
			this.publishmentSystemID = publishmentSystemID;
            this.taxis = taxis;
			this.description = description;
		}

		public string NodeGroupName
		{
			get{ return nodeGroupName; }
			set{ nodeGroupName = value; }
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
