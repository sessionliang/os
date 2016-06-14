using SiteServer.B2C.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SiteServer.API.Model.B2C
{
    public class OrderResponse
    {
        public OrderInfo Order { get; set; }
        public List<OrderItemInfo> ItemList { get; set; }
        public ConsigneeInfo Consignee { get; set; }
        public InvoiceInfo Invoice { get; set; }
    }
}