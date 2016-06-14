using System;

namespace SiteServer.WeiXin.Model
{
	public class KeywordResourceInfo
	{
        private int resourceID;
        private int publishmentSystemID;
        private int keywordID;
        private string title;
        private string imageUrl;
        private string summary;
        private EResourceType resourceType;
        private bool isShowCoverPic;
        private string content;
        private string navigationUrl;
        private int channelID;
        private int contentID;
        private int taxis;

		public KeywordResourceInfo()
		{
            this.resourceID = 0;
            this.publishmentSystemID = 0;
            this.keywordID = 0;
            this.title = string.Empty;
            this.imageUrl = string.Empty;
            this.summary = string.Empty;
            this.resourceType = EResourceType.Content;
            this.isShowCoverPic = true;
            this.content = string.Empty;
            this.navigationUrl = string.Empty;
            this.channelID = 0;
            this.contentID = 0;
            this.taxis = 0;
		}

        public KeywordResourceInfo(int resourceID, int publishmentSystemID, int keywordID, string title, string imageUrl, string summary, EResourceType resourceType, bool isShowCoverPic, string content, string navigationUrl, int channelID, int contentID, int taxis)
		{
            this.resourceID = resourceID;
            this.publishmentSystemID = publishmentSystemID;
            this.keywordID = keywordID;
            this.title = title;
            this.imageUrl = imageUrl;
            this.summary = summary;
            this.resourceType = resourceType;
            this.isShowCoverPic = isShowCoverPic;
            this.content = content;
            this.navigationUrl = navigationUrl;
            this.channelID = channelID;
            this.contentID = contentID;
            this.taxis = taxis;
		}

        public int ResourceID
        {
            get { return resourceID; }
            set { resourceID = value; }
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public int KeywordID
        {
            get { return keywordID; }
            set { keywordID = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = value; }
        }

        public string Summary
        {
            get { return summary; }
            set { summary = value; }
        }

        public EResourceType ResourceType
        {
            get { return resourceType; }
            set { resourceType = value; }
        }

        public bool IsShowCoverPic
        {
            get { return isShowCoverPic; }
            set { isShowCoverPic = value; }
        }

        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        public string NavigationUrl
        {
            get { return navigationUrl; }
            set { navigationUrl = value; }
        }

        public int ChannelID
        {
            get { return channelID; }
            set { channelID = value; }
        }

        public int ContentID
        {
            get { return contentID; }
            set { contentID = value; }
        }

        public int Taxis
        {
            get { return taxis; }
            set { taxis = value; }
        }
	}
}
