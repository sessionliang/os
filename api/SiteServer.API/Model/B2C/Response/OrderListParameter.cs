using SiteServer.B2C.Model;
using SiteServer.CMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteServer.API.Model.B2C
{
    public class OrderListParameter
    {
        public PublishmentSystemParameter PublishmentSystemInfo { get; set; }
        public OrderInfo OrderInfo { get; set; }
        public List<OrderItemParameter> Items { get; set; }
        public bool IsPaymentClick { get; set; }
        public string PaymentForm { get; set; }
        public string ClickString { get; set; }
    }
}