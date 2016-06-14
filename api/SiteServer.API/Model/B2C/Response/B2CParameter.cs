using BaiRong.Model;
using SiteServer.B2C.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SiteServer.API.Model.B2C
{
    public class B2CParameter
    {
        public UserInfo User { get; set; }
        public UserSettingInfo Setting { get; set; }
        public bool IsAnonymous { get; set; }
        public IEnumerable<Cart> Carts { get; set; }
        public AmountInfo Amount { get; set; }
    }
}