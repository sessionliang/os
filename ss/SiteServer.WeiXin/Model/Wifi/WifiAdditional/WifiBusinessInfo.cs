using BaiRong.Core;
using BaiRong.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteServer.WeiXin.Model
{

    public class Conf
    {
        public string total { get; set; }
        public int page { get; set; }
        public int perpage { get; set; }
        public int totalpage { get; set; }
    }

    public class Data
    {
        public string create_time { get; set; }
        public string update_time { get; set; }
        public string business_id { get; set; }
        public string contact_name { get; set; }
        public string contact_email { get; set; }
        public string contact_phone { get; set; }
        public object area_id { get; set; }
        public string name { get; set; }
        public object business_ext { get; set; }
        public string welcome_url { get; set; }
        public string other_url { get; set; }
        public string status { get; set; }
        public string contact_qq { get; set; }
        public string contact_address { get; set; }
        public string introduction { get; set; }
        public string reply_content { get; set; }
    }

    public class WifiBusinessInfo
    {
        public int ret { get; set; }
        public Data data { get; set; }      
        public string msg { get; set; }

    }
}
