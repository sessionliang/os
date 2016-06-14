using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;

namespace SiteServer.B2C.Model
{
    public class PaymentAlipayInfo : ExtendedAttributes
    {
        public PaymentAlipayInfo(string settingsXML)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(settingsXML);
            base.SetExtendedAttribute(nameValueCollection);
        }

        public string SellerEmail     //支付宝账号
        {
            get { return base.GetString("SellerEmail", string.Empty); }
            set { base.SetExtendedAttribute("SellerEmail", value); }
        }

        public string Partner     //合作者身份
        {
            get { return base.GetString("Partner", string.Empty); }
            set { base.SetExtendedAttribute("Partner", value); }
        }

        public string Key     //安全校验码
        {
            get { return base.GetString("Key", string.Empty); }
            set { base.SetExtendedAttribute("Key", value); }
        }

        public EPaymentAlipay AlipayType     //安全校验码
        {
            get { return EPaymentAlipayUtils.GetEnumType(base.GetString("AlipayType", string.Empty)); }
            set { base.SetExtendedAttribute("AlipayType", EPaymentAlipayUtils.GetValue(value)); }
        }
    }
}
