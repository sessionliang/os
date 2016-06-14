using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteServer.API.Model.B2C
{
    public class Cart
    {
        public int CartID { get; set; }
        public int PublishmentSystemID { get; set; }
        public int ChannelID { get; set; }
        public int ContentID { get; set; }
        public int GoodsID { get; set; }
        public int PurchaseNum { get; set; }
        public DateTime AddDate { get; set; }
        public string SN { get; set; }
        public string NavigationUrl { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }
        public string Spec { get; set; }
        public string ImageUrl { get; set; }
        public string ThumbUrl { get; set; }
        public string Summary { get; set; }
    }
}