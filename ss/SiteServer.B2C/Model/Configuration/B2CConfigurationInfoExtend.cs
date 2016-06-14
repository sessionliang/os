using System;
using BaiRong.Model;
using BaiRong.Core;
using System.Collections.Specialized;

using System.Collections.Generic;

namespace SiteServer.B2C.Model
{
    public class B2CConfigurationInfoExtend : ExtendedAttributes
    {
        public B2CConfigurationInfoExtend(string settingsXML)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(settingsXML);
            base.SetExtendedAttribute(nameValueCollection);
        }

        public override string ToString()
        {
            return TranslateUtils.NameValueCollectionToString(base.Attributes);
        }

        //π§µ•…Ë÷√

        public List<string> RequestTypeCollection
        {
            get { return TranslateUtils.StringCollectionToStringList(base.GetString("RequestTypeCollection", string.Empty)); }
            set { base.SetExtendedAttribute("RequestTypeCollection", TranslateUtils.ObjectCollectionToString(value)); }
        }
    }
}
