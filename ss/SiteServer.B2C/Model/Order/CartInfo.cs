using System;
using BaiRong.Model;

namespace SiteServer.B2C.Model
{
    public class CartInfo
    {
        private int cartID;
        private int publishmentSystemID;
        private string userName;
        private string sessionID;
        private int channelID;
        private int contentID;
        private int goodsID;
        private int purchaseNum;
        private DateTime addDate;

        public CartInfo()
        {
            this.cartID = 0;
            this.publishmentSystemID = 0;
            this.userName = string.Empty;
            this.sessionID = string.Empty;
            this.channelID = 0;
            this.contentID = 0;
            this.goodsID = 0;
            this.purchaseNum = 0;
            this.addDate = DateTime.Now;
        }

        public CartInfo(int cartID, int publishmentSystemID, string userName, string sessionID, int channelID, int contentID, int goodsID, int purchaseNum, DateTime addDate)
        {
            this.cartID = cartID;
            this.publishmentSystemID = publishmentSystemID;
            this.userName = userName;
            this.sessionID = sessionID;
            this.channelID = channelID;
            this.contentID = contentID;
            this.goodsID = goodsID;
            this.purchaseNum = purchaseNum;
            this.addDate = addDate;
        }

        public int CartID
        {
            get { return cartID; }
            set { cartID = value; }
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

        public string SessionID
        {
            get { return sessionID; }
            set { sessionID = value; }
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

        public int GoodsID
        {
            get { return goodsID; }
            set { goodsID = value; }
        }

        public int PurchaseNum
        {
            get
            {
                if (purchaseNum <= 0)
                {
                    return 1;
                }
                return purchaseNum;
            }
            set { purchaseNum = value; }
        }

        public DateTime AddDate
        {
            get { return addDate; }
            set { addDate = value; }
        }
    }
}
