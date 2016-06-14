using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteServer.B2C.Model
{
    public class OrderItemReturnStatus
    {
        public string AuditStatus { get; set; }
        public string AuditUser { get; set; }
        public DateTime? AuditDate { get; set; }

        public string ReturnOrderStatus { get; set; }
        public string ReturnOrderUser { get; set; }
        public DateTime? ReturnOrderDate { get; set; }

        public string ReturnMoneyStatus { get; set; }
        public string ReturnMoneyUser { get; set; }
        public DateTime? ReturnMoneyDate { get; set; }

        public string Status { get; set; }
    }
}
