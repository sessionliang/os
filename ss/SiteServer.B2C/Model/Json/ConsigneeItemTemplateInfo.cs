using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BaiRong.Model;

namespace SiteServer.B2C.Model.Json
{
    public class ConsigneeItemTemplateInfo : CaseExtendedAttributes
    {
        public int ConsigneeID
        {
            get { return base.GetInt("ConsigneeID", 0); }
            set { base.SetExtendedAttribute("ConsigneeID", value); }
        }

        public string Consignee
        {
            get { return base.GetString("Consignee", string.Empty); }
            set { base.SetExtendedAttribute("Consignee", value); }
        }

        public string Province
        {
            get { return base.GetString("Province", string.Empty); }
            set { base.SetExtendedAttribute("Province", value); }
        }

        public string City
        {
            get { return base.GetString("City", string.Empty); }
            set { base.SetExtendedAttribute("City", value); }
        }

        public string Area
        {
            get { return base.GetString("Area", string.Empty); }
            set { base.SetExtendedAttribute("Area", value); }
        }

        public string Address
        {
            get { return base.GetString("Address", string.Empty); }
            set { base.SetExtendedAttribute("Address", value); }
        }

        public string Zipcode
        {
            get { return base.GetString("Zipcode", string.Empty); }
            set { base.SetExtendedAttribute("Zipcode", value); }
        }

        public string Mobile
        {
            get { return base.GetString("Mobile", string.Empty); }
            set { base.SetExtendedAttribute("Mobile", value); }
        }

        public string Tel
        {
            get { return base.GetString("Tel", string.Empty); }
            set { base.SetExtendedAttribute("Tel", value); }
        }

        public string Email
        {
            get { return base.GetString("Email", string.Empty); }
            set { base.SetExtendedAttribute("Email", value); }
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
