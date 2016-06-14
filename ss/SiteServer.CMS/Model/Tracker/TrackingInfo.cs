using System;

namespace SiteServer.CMS.Model
{
	public class TrackingInfo
	{
		private int trackingID;
		private int publishmentSystemID;
        private string userName;
        private ETrackerType trackerType;
		private DateTime lastAccessDateTime;
        private string pageUrl;
		private int pageNodeID;
        private int pageContentID;
		private string referrer;
		private string ipAddress;
		private string operatingSystem;
		private string browser;
		private DateTime accessDateTime;

		public TrackingInfo()
		{
			this.trackingID = 0;
            this.publishmentSystemID = 0;
            this.userName = string.Empty;
            this.trackerType = ETrackerType.Page;
			this.lastAccessDateTime = DateTime.MinValue;
            this.pageUrl = string.Empty;
			this.pageNodeID = 0;
            this.pageContentID = 0;
			this.referrer = string.Empty;
			this.ipAddress = string.Empty;
			this.operatingSystem = string.Empty;
			this.browser = string.Empty;
			this.accessDateTime = DateTime.Now;
		}

        public TrackingInfo(int trackingID, int publishmentSystemID, string userName, ETrackerType trackerType, DateTime lastAccessDateTime, string pageUrl, int pageNodeID, int pageContentID, string referrer, string ipAddress, string operatingSystem, string browser, DateTime accessDateTime) 
		{
            this.trackingID = trackingID;
            this.publishmentSystemID = publishmentSystemID;
            this.userName = userName;
            this.trackerType = trackerType;
			this.lastAccessDateTime = lastAccessDateTime;
            this.pageUrl = pageUrl;
            this.pageNodeID = pageNodeID;
            this.pageContentID = pageContentID;
			this.referrer = referrer;
			this.ipAddress = ipAddress;
			this.operatingSystem = operatingSystem;
			this.browser = browser;
			this.accessDateTime = accessDateTime;
		}

        public int TrackingID
		{
            get { return trackingID; }
            set { trackingID = value; }
		}

        public int PublishmentSystemID
		{
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
		}

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public ETrackerType TrackerType
		{
            get { return trackerType; }
            set { trackerType = value; }
		}

		public DateTime LastAccessDateTime
		{
			get{ return lastAccessDateTime; }
			set{ lastAccessDateTime = value; }
		}

        public string PageUrl
        {
            get { return pageUrl; }
            set { pageUrl = value; }
        }

        public int PageNodeID
		{
            get { return pageNodeID; }
            set { pageNodeID = value; }
		}

        public int PageContentID
        {
            get { return pageContentID; }
            set { pageContentID = value; }
        }

		public string Referrer
		{
			get{ return referrer; }
			set{ referrer = value; }
		}

		public string IPAddress
		{
			get{ return ipAddress; }
			set{ ipAddress = value; }
		}

		public string OperatingSystem
		{
			get{ return operatingSystem; }
			set{ operatingSystem = value; }
		}

		public string Browser
		{
			get{ return browser; }
			set{ browser = value; }
		}

		public DateTime AccessDateTime
		{
			get{ return accessDateTime; }
			set{ accessDateTime = value; }
		}
	}
}
