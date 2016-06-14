using System;

namespace SiteServer.CMS.Model
{
	public class VoteOptionInfo
	{
        private int optionID;
        private int publishmentSystemID;
        private int nodeID;
        private int contentID;
        private string title;
        private string imageUrl;
		private string navigationUrl;
		private int voteNum;

		public VoteOptionInfo()
		{
            this.optionID = 0;
            this.publishmentSystemID = 0;
            this.nodeID = 0;
            this.contentID = 0;
            this.title = string.Empty;
            this.imageUrl = string.Empty;
			this.navigationUrl = string.Empty;
			this.voteNum = 0;
		}

        public VoteOptionInfo(int optionID, int publishmentSystemID, int nodeID, int contentID, string title, string imageUrl, string navigationUrl, int voteNum) 
		{
            this.optionID = optionID;
            this.publishmentSystemID = publishmentSystemID;
            this.nodeID = nodeID;
            this.contentID = contentID;
            this.title = title;
            this.imageUrl = imageUrl;
			this.navigationUrl = navigationUrl;
			this.voteNum = voteNum;
		}

        public int OptionID
		{
            get { return optionID; }
            set { optionID = value; }
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

        public string Title
		{
			get{ return title; }
            set { title = value; }
		}

		public string ImageUrl
		{
			get{ return imageUrl; }
			set{ imageUrl = value; }
		}

		public string NavigationUrl
		{
			get{ return navigationUrl; }
			set{ navigationUrl = value; }
		}

		public int VoteNum
		{
			get{ return voteNum; }
			set{ voteNum = value; }
		}
	}
}
