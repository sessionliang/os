using SiteServer.B2C.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteServer.API.Model.B2C
{
    public class OrderSuccessParameter
    {
        public bool IsSuccess { get; set; }
        public string PaymentName { get; set; }
        public OrderInfo Order { get; set; }
        public string PaymentForm { get; set; }
        public string ClickString { get; set; }
    }
}