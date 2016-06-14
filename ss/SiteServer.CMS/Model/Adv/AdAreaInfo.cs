using System;
using BaiRong.Model;

namespace SiteServer.CMS.Model
{
	public class AdAreaInfo
	{
        private int adAreaID;
	    private int publishmentSystemID;
        private string adAreaName;
        private int width;
        private int height;
        private string summary;
        private bool isEnabled;
        private DateTime addDate;

		public AdAreaInfo()
		{
            this.adAreaID = 0;
			this.publishmentSystemID = 0;
            this.adAreaName = string.Empty;
            this.width = 0;
            this.height = 0;
            this.summary = string.Empty;
            this.isEnabled = true;
            this.addDate = DateTime.Now;
			 
		}

        public AdAreaInfo(int adAreaID, int publishmentSystemID, string adAreaName, int width, int height, string summary, bool isEnabled, DateTime addDate) 
		{
            this.adAreaID = adAreaID;
            this.publishmentSystemID = publishmentSystemID;
            this.adAreaName = adAreaName;
            this.width = width;
            this.height = height;
            this.summary = summary;
            this.isEnabled = isEnabled;
            this.addDate = addDate;
		}

        public int AdAreaID
		{
            get { return adAreaID; }
            set { adAreaID = value; }
		}

		public int PublishmentSystemID
		{
			get{ return publishmentSystemID; }
			set{ publishmentSystemID = value; }
		}

        public string AdAreaName
        {
            get { return adAreaName; }
            set { adAreaName = value; }
        }
        public int  Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public string Summary
        {
            get { return summary; }
            set { summary = value; }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }
          
		public DateTime AddDate
		{
			get{ return addDate ; }
            set { addDate = value; }
		}
	}
}
