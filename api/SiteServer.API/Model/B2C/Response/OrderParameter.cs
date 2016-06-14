using SiteServer.B2C.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteServer.API.Model.B2C
{
    public class OrderParameter
    {
        public bool IsAnonymous { get; set; }
        public ConsigneeInfo Consignee { get; set; }
        public PaymentInfo Payment { get; set; }
        public ShipmentInfo Shipment { get; set; }
        public InvoiceInfo Invoice { get; set; }
        public IEnumerable<ConsigneeInfo> Consignees { get; set; }
        public IEnumerable<PaymentInfo> Payments { get; set; }
        public IEnumerable<ShipmentInfo> Shipments { get; set; }
        public IEnumerable<InvoiceInfo> Invoices { get; set; }
    }
}