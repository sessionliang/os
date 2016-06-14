using BaiRong.Model;
using SiteServer.B2C.Model;
using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SiteServer.API.Model.WX
{
    public class SearchContentInfo
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string NavigationUrl { get; set; }
        public string Summary { get; set; }
        public DateTime AddDate { get; set; }
         
    }
}