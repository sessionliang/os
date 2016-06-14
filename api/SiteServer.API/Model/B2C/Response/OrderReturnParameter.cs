using SiteServer.B2C.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteServer.API.Model.B2C
{
    public class OrderReturnParameter
    {
        public bool IsSuccess { get; set; }
        public OrderInfo Order { get; set; }
        public string ErrorMessage { get; set; }
    }
}