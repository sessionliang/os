using BaiRong.Model;
using SiteServer.B2C.Model;
using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SiteServer.API.Model.WX
{
    public class SearchParameter
    { 
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsPoweredBy { get; set; }
        public string PoweredBy { get; set; }
        public SearchInfo SearchInfo { get; set; }
        public List<SearchNavigationInfo> SearchNavigationInfoList { get; set; }
        public List<SearchContentInfo> ImageContentInfoList { get; set; }
        public List<SearchContentInfo> TextContentInfoList { get; set; }
        public List<SearchContentInfo> ContentInfoList { get; set; }
         
    }
}