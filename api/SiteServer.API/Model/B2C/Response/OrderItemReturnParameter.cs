using SiteServer.B2C.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteServer.API.Model.B2C
{
    public class OrderItemReturnParameter : OrderItemReturnInfo
    {
        public OrderItemReturnParameter() { }
        public OrderItemReturnParameter(OrderItemReturnInfo orderItemReturnInfo)
        {
            foreach (string attr in base.AllAttributes)
            {
                base.SetValue(attr, orderItemReturnInfo.GetValue(attr));
            }
        }

        public string DetailStatus { get; set; }
        public string NavigationUrl { get; set; }

    }
}