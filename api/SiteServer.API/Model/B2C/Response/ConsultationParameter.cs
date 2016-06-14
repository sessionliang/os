using SiteServer.B2C.Model;
using SiteServer.CMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteServer.API.Model.B2C
{
    public class ConsultationParameter
    {
        public int ContentID { get; set; }
        public int GoodsID { get; set; }
        public string Title { get; set; }
        public string ThumbUrl { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Type { get; set; }
        public string AddUser { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime ReplyDate { get; set; }
        public bool IsReply { get; set; }
        public string NavigationUrl { get; set; }
        public PublishmentSystemParameter PublishmentSystemInfo { get; set; }
    }
}