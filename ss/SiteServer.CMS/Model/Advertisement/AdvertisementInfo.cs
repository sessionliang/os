using System;
using BaiRong.Model;

namespace SiteServer.CMS.Model
{
	public class AdvertisementInfo
	{
		private string advertisementName;
		private int publishmentSystemID;
		private EAdvertisementType advertisementType;
		private bool isDateLimited;
		private DateTime startDate;
		private DateTime endDate;
		private DateTime addDate;
        private string nodeIDCollectionToChannel;
        private string nodeIDCollectionToContent;
        private string fileTemplateIDCollection;
        private string settings;

		public AdvertisementInfo()
		{
			this.advertisementName = string.Empty;
			this.publishmentSystemID = 0;
			this.advertisementType = EAdvertisementType.FloatImage;
			this.isDateLimited = false;
			this.startDate = DateTime.Now;
			this.endDate = DateTime.Now;
			this.addDate = DateTime.Now;
			this.nodeIDCollectionToChannel = string.Empty;
            this.nodeIDCollectionToContent = string.Empty;
            this.fileTemplateIDCollection = string.Empty;
            this.settings = string.Empty;
		}

        public AdvertisementInfo(string advertisementName, int publishmentSystemID, EAdvertisementType advertisementType, bool isDateLimited, DateTime startDate, DateTime endDate, DateTime addDate, string nodeIDCollectionToChannel, string nodeIDCollectionToContent, string fileTemplateIDCollection, string settings) 
		{
			this.advertisementName = advertisementName;
			this.publishmentSystemID = publishmentSystemID;
			this.advertisementType = advertisementType;
			this.isDateLimited = isDateLimited;
			this.startDate = startDate;
			this.endDate = endDate;
			this.addDate = addDate;
            this.nodeIDCollectionToChannel = nodeIDCollectionToChannel;
            this.nodeIDCollectionToContent = nodeIDCollectionToContent;
            this.fileTemplateIDCollection = fileTemplateIDCollection;
            this.settings = settings;
		}

		public string AdvertisementName
		{
			get{ return advertisementName; }
			set{ advertisementName = value; }
		}

		public int PublishmentSystemID
		{
			get{ return publishmentSystemID; }
			set{ publishmentSystemID = value; }
		}

		public EAdvertisementType AdvertisementType
		{
			get{ return advertisementType; }
			set{ advertisementType = value; }
		}

		public bool IsDateLimited
		{
			get{ return isDateLimited; }
			set{ isDateLimited = value; }
		}

		public DateTime StartDate
		{
			get{ return startDate; }
			set{ startDate = value; }
		}

		public DateTime EndDate
		{
			get{ return endDate; }
			set{ endDate = value; }
		}

		public DateTime AddDate
		{
			get{ return addDate; }
			set{ addDate = value; }
		}

        public string NodeIDCollectionToChannel
		{
            get { return nodeIDCollectionToChannel; }
            set { nodeIDCollectionToChannel = value; }
		}

        public string NodeIDCollectionToContent
        {
            get { return nodeIDCollectionToContent; }
            set { nodeIDCollectionToContent = value; }
        }

        public string FileTemplateIDCollection
        {
            get { return fileTemplateIDCollection; }
            set { fileTemplateIDCollection = value; }
        }

        public string Settings
        {
            get { return settings; }
            set { settings = value; }
        }
	}
}
