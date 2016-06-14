using SiteServer.B2C.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SiteServer.API.Model.B2C
{
    public class RequestParameter
    {
        public List<string> RequestTypeList { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string QQ { get; set; }
    }
}