using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BaiRong.Model;

namespace SiteServer.B2C.Model.Json
{
    public class InvoiceItemTemplateInfo : CaseExtendedAttributes
    {
        public int InvoiceID
        {
            get { return base.GetInt("InvoiceID", 0); }
            set { base.SetExtendedAttribute("InvoiceID", value); }
        }

        public bool IsCompany
        {
            get { return base.GetBool("IsCompany", false); }
            set { base.SetExtendedAttribute("IsCompany", value); }
        }

        public string InvoiceTitle
        {
            get { return base.GetString("InvoiceTitle", string.Empty); }
            set { base.SetExtendedAttribute("InvoiceTitle", value); }
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
