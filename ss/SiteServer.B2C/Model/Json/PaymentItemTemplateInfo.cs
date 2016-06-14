using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BaiRong.Model;

namespace SiteServer.B2C.Model.Json
{
    public class PaymentItemTemplateInfo : CaseExtendedAttributes
    {
        public int PaymentID
        {
            get { return base.GetInt("PaymentID", 0); }
            set { base.SetExtendedAttribute("PaymentID", value); }
        }

        public string PaymentName
        {
            get { return base.GetString("PaymentName", string.Empty); }
            set { base.SetExtendedAttribute("PaymentName", value); }
        }

        public string Description
        {
            get { return base.GetString("Description", string.Empty); }
            set { base.SetExtendedAttribute("Description", value); }
        }

        public JSONObjectInfo JSONObject
        {
            get
            {
                return new JSONObjectInfo(this, null);
            }
        }
    }
}
