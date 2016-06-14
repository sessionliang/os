using System;
using BaiRong.Model;

namespace SiteServer.B2C.Model
{
	public class SpecComboInfo
	{
        private int comboID;
		private int publishmentSystemID;
        private int contentID;
        private int specID;
        private int itemID;
        private string title;
        private string iconUrl;
        private string photoIDCollection;
        private int taxis;

		public SpecComboInfo()
		{
            this.comboID = 0;
			this.publishmentSystemID = 0;
            this.contentID = 0;
            this.specID = 0;
            this.itemID = 0;
            this.title = string.Empty;
            this.iconUrl = string.Empty;
            this.photoIDCollection = string.Empty;
            this.taxis = 0;
		}

        public SpecComboInfo(int comboID, int publishmentSystemID, int contentID, int specID, int itemID, string title, string iconUrl, string photoIDCollection, int taxis)
		{
            this.comboID = comboID;
            this.publishmentSystemID = publishmentSystemID;
            this.contentID = contentID;
            this.specID = specID;
            this.itemID = itemID;
            this.title = title;
            this.iconUrl = iconUrl;
            this.photoIDCollection = photoIDCollection;
            this.taxis = taxis;
		}

        public int ComboID
		{
            get { return comboID; }
            set { comboID = value; }
		}

		public int PublishmentSystemID
		{
			get{ return publishmentSystemID; }
			set{ publishmentSystemID = value; }
		}

        public int ContentID
        {
            get { return contentID; }
            set { contentID = value; }
        }

        public int SpecID
        {
            get { return specID; }
            set { specID = value; }
        }

        public int ItemID
        {
            get { return itemID; }
            set { itemID = value; }
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

        public string PhotoIDCollection
        {
            get { return photoIDCollection; }
            set { photoIDCollection = value; }
        }

        public int Taxis
        {
            get { return taxis; }
            set { taxis = value; }
        }
	}
}
