using SiteServer.B2C.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteServer.API.Model.B2C
{
    public class OrderItemParameter : OrderItemInfo
    {
        public OrderItemParameter() { }
        public OrderItemParameter(OrderItemInfo orderItemInfo)
        {
            foreach (string attr in base.AllAttributes)
            {
                base.SetValue(attr, orderItemInfo.GetValue(attr));
            }
        }

        public int OrderItemID { get; set; }
        public string NavigationUrl { get; set; }
        public string Spec { get; set; }
        public bool IsApplyReturn { get; set; }

        public List<OrderItemCommentParameter> OrderItemCommentList { get; set; }
        public List<string> DefaultTags { get; set; }
    }
}