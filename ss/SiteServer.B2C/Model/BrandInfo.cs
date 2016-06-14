using System;
using BaiRong.Model;

namespace SiteServer.B2C.Model
{
    public class BrandInfo
	{
		private int brandID;
		private int publishmentSystemID;
		private string brandName;
		private string brandUrl;
        private string iconUrl;
        private string content;
		private int taxis;

		public BrandInfo()
		{
			this.brandID = 0;
			this.publishmentSystemID = 0;
			this.brandName = string.Empty;
			this.brandUrl = string.Empty;
            this.iconUrl = string.Empty;
            this.content = string.Empty;
			this.taxis = 0;
		}

        public BrandInfo(int brandID, int publishmentSystemID, string brandName, string brandUrl, string iconUrl, string content, int taxis)
		{
            this.brandID = brandID;
            this.publishmentSystemID = publishmentSystemID;
            this.brandName = brandName;
            this.brandUrl = brandUrl;
            this.iconUrl = iconUrl;
            this.content = content;
            this.taxis = taxis;
		}

        public int BrandID
		{
            get { return brandID; }
            set { brandID = value; }
		}

		public int PublishmentSystemID
		{
			get{ return publishmentSystemID; }
			set{ publishmentSystemID = value; }
		}

        public string BrandName
		{
            get { return brandName; }
            set { brandName = value; }
		}

        public string BrandUrl
		{
            get { return brandUrl; }
            set { brandUrl = value; }
		}

        public string IconUrl
        {
            get { return iconUrl; }
            set { iconUrl = value; }
        }

        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        public int Taxis
		{
            get { return taxis; }
            set { taxis = value; }
		}
	}
}
