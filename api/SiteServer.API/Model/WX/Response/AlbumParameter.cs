using SiteServer.B2C.Model;
using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SiteServer.API.Model.WX
{
    public class AlbumParameter
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsPoweredBy { get; set; }
        public string PoweredBy { get; set; }
        public string Title { get; set; }
        public List<AlbumContentInfo> ContentInfoList { get; set; }
    }
}