using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteServer.API.Model.WX
{
    public class StoreItemParameter
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsPoweredBy { get; set; }
        public string PoweredBy { get; set; }
        public string Title { get; set; }
        public StoreItemInfo ContentInfo { get; set; }
        public List<StoreItemInfo> ContentInfoList { get; set; }
    }
}