using System;
using System.Collections.Generic;
using System.Text;

namespace SiteServer.CMS.Model.MLib
{
    public class ReferenceLog
    {
        public int RLID { get; set; }
        public int RTID { get; set; }
        public int PublishmentSystemID { get; set; }
        public int NodeID { get; set; }
        public int ToContentID { get; set; }
        public string Operator { get; set; }
        public DateTime OperateDate { get; set; }
        public int SubmissionID { get; set; }
        
    }
}
