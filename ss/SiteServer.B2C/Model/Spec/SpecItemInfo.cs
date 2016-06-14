using System;
using BaiRong.Model;

namespace SiteServer.B2C.Model
{
	public class SpecItemInfo
	{
		private int itemID;
        private int publishmentSystemID;
		private int specID;
		private string title;
		private string iconUrl;
        private bool isDefault;
        private int taxis;

		public SpecItemInfo()
		{
            this.itemID = 0;
            this.publishmentSystemID = 0;
            this.specID = 0;
            this.title = string.Empty;
            this.iconUrl = string.Empty;
            this.isDefault = false;
            this.taxis = 0;
		}

        public SpecItemInfo(int itemID, int publishmentSystemID, int specID, string title, string iconUrl, bool isDefault, int taxis)
		{
            this.itemID = itemID;
            this.publishmentSystemID = publishmentSystemID;
            this.specID = specID;
            this.title = title;
            this.iconUrl = iconUrl;
            this.isDefault = isDefault;
            this.taxis = taxis;
		}

        public int ItemID
        {
            get { return itemID; }
            set { itemID = value; }
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public int SpecID
		{
            get { return specID; }
            set { specID = value; }
		}

        public string Title
		{
            get { return title; }
            set { title = value; }
		}

        public string IconUrl
        {
            get { return iconUrl; }
            set { iconUrl = value; }
        }

        public bool IsDefault
        {
            get { return isDefault; }
            set { isDefault = value; }
        }

        public int Taxis
        {
            get { return taxis; }
            set { taxis = value; }
        }
	}
}
