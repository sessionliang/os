using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BaiRong.Model;

namespace SiteServer.B2C.Model.Json
{
    public class ShipmentItemTemplateInfo : CaseExtendedAttributes
    {
        public int ShipmentID
        {
            get { return base.GetInt("ShipmentID", 0); }
            set { base.SetExtendedAttribute("ShipmentID", value); }
        }

        public string ShipmentName
        {
            get { return base.GetString("ShipmentName", string.Empty); }
            set { base.SetExtendedAttribute("ShipmentName", value); }
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
