using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BaiRong.Model;

namespace SiteServer.B2C.Model.Json
{
    public class OrderTemplateInfo : CaseExtendedAttributes
    {
        public string InvoiceContentCollection
        {
            get { return base.GetString("InvoiceContentCollection", string.Empty); }
            set { base.SetExtendedAttribute("InvoiceContentCollection", value); }
        }

        private List<JSONObjectInfo> paymentList = new List<JSONObjectInfo>();
        public List<JSONObjectInfo> PaymentList
        {
            get { return paymentList; }
        }

        private List<JSONObjectInfo> shipmentList = new List<JSONObjectInfo>();
        public List<JSONObjectInfo> ShipmentList
        {
            get { return shipmentList; }
        }

        public JSONObjectInfo JSONObject
        {
            get
            {
                return new JSONObjectInfo(this, "PaymentList", this.paymentList, "ShipmentList", this.shipmentList);
            }
        }
    }
}
