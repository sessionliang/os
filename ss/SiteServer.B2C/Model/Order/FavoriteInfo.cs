using System;
using BaiRong.Model;

namespace SiteServer.B2C.Model
{
	public class FavoriteInfo
	{
        private int favoriteID;
		private int publishmentSystemID;
		private string userName;
        private int goodsID;
        private DateTime addDate;

		public FavoriteInfo()
		{
            this.favoriteID = 0;
			this.publishmentSystemID = 0;
            this.userName = string.Empty;
            this.goodsID = 0;
            this.addDate = DateTime.Now;
		}

        public FavoriteInfo(int favoriteID, int publishmentSystemID, string userName, int goodsID, DateTime addDate)
		{
            this.favoriteID = favoriteID;
            this.publishmentSystemID = publishmentSystemID;
            this.userName = userName;
            this.goodsID = goodsID;
            this.addDate = addDate;
		}

        public int FavoriteID
		{
            get { return favoriteID; }
            set { favoriteID = value; }
		}

		public int PublishmentSystemID
		{
			get{ return publishmentSystemID; }
			set{ publishmentSystemID = value; }
		}

        public string UserName
		{
            get { return userName; }
            set { userName = value; }
		}

        public int GoodsID
        {
            get { return goodsID; }
            set { goodsID = value; }
        }

        public DateTime AddDate
        {
            get { return addDate; }
            set { addDate = value; }
        }
	}
}
