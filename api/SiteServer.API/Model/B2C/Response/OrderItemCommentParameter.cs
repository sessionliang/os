using SiteServer.B2C.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteServer.API.Model.B2C
{
    public class OrderItemCommentParameter
    {
        public int ID { get; set; }
        public int OrderItemID { get; set; }
        public int Star { get; set; }
        public string Tags { get; set; }
        public string Comment { get; set; }
        public bool IsAnonymous { get; set; }
        public string OrderUrl { get; set; }
        public DateTime AddDate { get; set; }
        public string AddUser { get; set; }
        public int GoodCount { get; set; }
        public string ImageUrl { get; set; }
    }
}