using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BaiRong.Model;

namespace SiteServer.B2C.Model.Json
{
    public class CartItemTemplateInfo : CaseExtendedAttributes
    {
        public string navigationUrl
        {
            get { return base.GetString("navigationUrl", string.Empty); }
            set { base.SetExtendedAttribute("navigationUrl", value); }
        }

        public decimal price
        {
            get { return base.GetDecimal("price", 0); }
            set { base.SetExtendedAttribute("price", value); }
        }

        public decimal priceDiscount
        {
            get { return base.GetDecimal("priceDiscount", 0); }
            set { base.SetExtendedAttribute("priceDiscount", value); }
        }

        public int purchaseNum
        {
            get { return base.GetInt("purchaseNum", 0); }
            set { base.SetExtendedAttribute("purchaseNum", value); }
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
