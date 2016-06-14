using System;
using BaiRong.Model;

namespace SiteServer.B2C.Model
{
	public class GoodsInfo
	{
        private int goodsID;
		private int publishmentSystemID;
        private int contentID;
        private string comboIDCollection;
        private string specIDCollection;
        private string specItemIDCollection;
        private string goodsSN;
        private int stock;
        private decimal priceMarket;
        private decimal priceSale;
        private bool isOnSale;

		public GoodsInfo()
		{
            this.goodsID = 0;
			this.publishmentSystemID = 0;
            this.contentID = 0;
            this.comboIDCollection = string.Empty;
            this.specIDCollection = string.Empty;
            this.specItemIDCollection = string.Empty;
            this.goodsSN = string.Empty;
            this.stock = 0;
            this.priceMarket = 0;
            this.priceSale = 0;
            this.isOnSale = true;
		}

        public GoodsInfo(int goodsID, int publishmentSystemID, int contentID, string comboIDCollection, string specIDCollection, string specItemIDCollection, string goodsSN, int stock, decimal priceMarket, decimal priceSale, bool isOnSale)
		{
            this.goodsID = goodsID;
            this.publishmentSystemID = publishmentSystemID;
            this.contentID = contentID;
            this.comboIDCollection = comboIDCollection;
            this.specIDCollection = specIDCollection;
            this.specItemIDCollection = specItemIDCollection;
            this.goodsSN = goodsSN;
            this.stock = stock;
            this.priceMarket = priceMarket;
            this.priceSale = priceSale;
            this.isOnSale = isOnSale;
		}

        public int GoodsID
		{
            get { return goodsID; }
            set { goodsID = value; }
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

        public string ComboIDCollection
        {
            get { return comboIDCollection; }
            set { comboIDCollection = value; }
        }

        public string SpecIDCollection
        {
            get { return specIDCollection; }
            set { specIDCollection = value; }
        }

        public string SpecItemIDCollection
        {
            get { return specItemIDCollection; }
            set { specItemIDCollection = value; }
        }

        public string GoodsSN
        {
            get { return goodsSN; }
            set { goodsSN = value; }
        }

        public int Stock
        {
            get { return stock; }
            set { stock = value; }
        }

        public decimal PriceMarket
        {
            get { return priceMarket; }
            set { priceMarket = value; }
        }

        public decimal PriceSale
        {
            get { return priceSale; }
            set { priceSale = value; }
        }

        public bool IsOnSale
        {
            get { return isOnSale; }
            set { isOnSale = value; }
        }
	}
}
