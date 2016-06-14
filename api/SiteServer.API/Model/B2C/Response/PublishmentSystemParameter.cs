using SiteServer.B2C.Model;
using SiteServer.CMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteServer.API.Model.B2C
{
    public class PublishmentSystemParameter
    {
        public int PublishmentSystemID { get; set; }
        public string PublishmentSystemName { get; set; }
        public string PublishmentSystemUrl { get; set; }
    }
}