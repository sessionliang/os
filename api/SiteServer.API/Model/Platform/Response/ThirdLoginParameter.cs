using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteServer.API.Model
{
    public class ThirdLoginParameter
    {
        public bool IsSuccess { get; set; }
        public string SiteName { get; set; }
        public string ThirdLoginType { get; set; }
        public string IndexPageUrl { get; set; }
        public string ThirdLoginNickName { get; set; }
        public string ThirdLoginPassword { get; set; }
        public string ThirdLoginUserHeadUrl { get; set; }
        public int PublishmentSystemID { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
    }
}