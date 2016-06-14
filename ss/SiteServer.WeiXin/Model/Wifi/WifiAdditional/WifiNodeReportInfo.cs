using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteServer.WeiXin.Model
{
    public class ReportList
    {
        public string fans_cnt { get; set; }
         public string visiter_cnt { get; set; }
         public string total_cnt { get; set; }
         public string online_cnt { get; set; }
         public string new_cnt { get; set; }
         public string created_at { get; set; }
    }

    public class ReportData
    {
        public List<ReportList> list { get; set; }
    }

    public class WifiNodeReportInfo
    {
        public int ret { get; set; }
        public ReportData data { get; set; }
        public string msg { get; set; }
    }
}
