using System;
using BaiRong.Model;
using System.Collections.Generic;

namespace SiteServer.B2C.Model
{
    public class OrderItemAttribute
    {
        protected OrderItemAttribute()
        {
        }

        //hidden
        public const string ID = "ID";

        //basic
        public const string OrderID = "OrderID";
        public const string ChannelID = "ChannelID";
        public const string ContentID = "ContentID";
        public const string GoodsID = "GoodsID";
        public const string GoodsSN = "GoodsSN";
        public const string Title = "Title";
        public const string ThumbUrl = "ThumbUrl";
        public const string PriceSale = "PriceSale";
        public const string PurchaseNum = "PurchaseNum";
        public const string IsShipment = "IsShipment";

        private static List<string> allAttributes;
        public static List<string> AllAttributes
        {
            get
            {
                if (allAttributes == null)
                {
                    allAttributes = new List<string>();
                    allAttributes.Add(ID);
                    allAttributes.Add(OrderID);
                    allAttributes.Add(ChannelID);
                    allAttributes.Add(ContentID);
                    allAttributes.Add(GoodsID);
                    allAttributes.Add(GoodsSN);
                    allAttributes.Add(Title);
                    allAttributes.Add(ThumbUrl);
                    allAttributes.Add(PriceSale);
                    allAttributes.Add(PurchaseNum);
                    allAttributes.Add(IsShipment);
                }

                return allAttributes;
            }
        }
    }
    public class OrderItemInfo : BaseInfo
    {
        public int OrderID { get; set; }
        public int ChannelID { get; set; }
        public int ContentID { get; set; }
        public int GoodsID { get; set; }
        public string GoodsSN { get; set; }
        public string Title { get; set; }
        public string ThumbUrl { get; set; }
        public decimal PriceSale { get; set; }
        public int PurchaseNum { get; set; }
        public bool IsShipment { get; set; }

        protected override List<string> AllAttributes
        {
            get
            {
                return OrderItemAttribute.AllAttributes;
            }
        }
    }
}
