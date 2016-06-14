using BaiRong.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteServer.API.Model
{
    public class ThirdLogin
    {
        public List<BaiRongThirdLoginInfo> ThirdLoginList { get; set; }
        public List<int> BindedThirdLoginList { get; set; }
        public bool IsSucess { get; set; }
    }
}