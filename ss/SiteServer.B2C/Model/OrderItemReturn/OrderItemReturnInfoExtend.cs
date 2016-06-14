using System;
using BaiRong.Model;
using BaiRong.Core;
using System.Collections.Specialized;

using System.Collections.Generic;

namespace SiteServer.B2C.Model
{
    public class OrderItemReturnInfoExtend : ExtendedAttributes
    {
        public OrderItemReturnInfoExtend(string settingsXML)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(settingsXML);
            base.SetExtendedAttribute(nameValueCollection);
        }

        public override string ToString()
        {
            return TranslateUtils.NameValueCollectionToString(base.Attributes);
        }

        /// <summary>
        /// 买家地址
        /// </summary>
        public string UserAddress
        {
            get { return base.GetString("UserAddress", string.Empty); }
            set { base.SetExtendedAttribute("UserAddress", value); }
        }

        /// <summary>
        /// 退款人姓名
        /// </summary>
        public string CardName
        {
            get { return base.GetString("CardName", string.Empty); }
            set { base.SetExtendedAttribute("CardName", value); }
        }

        /// <summary>
        /// 退款人卡号
        /// </summary>
        public string CardNumber
        {
            get { return base.GetString("CardNumber", string.Empty); }
            set { base.SetExtendedAttribute("CardNumber", value); }
        }
    }
}
