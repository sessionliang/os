using System;
using BaiRong.Model;
using BaiRong.Core;
using System.Collections.Specialized;

namespace SiteServer.CMS.Model
{
    public class TagStyleGovPublicApplyInfo : ExtendedAttributes
    {
        public TagStyleGovPublicApplyInfo(string settingsXML)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(settingsXML);
            base.SetExtendedAttribute(nameValueCollection);
        }

        public bool IsMail
        {
            get { return base.GetBool("IsMail", false); }
            set { base.SetExtendedAttribute("IsMail", value.ToString()); }
        }

        public string MailTo
        {
            get { return base.GetString("MailTo", string.Empty); }
            set { base.SetExtendedAttribute("MailTo", value); }
        }

        public string MailTitleFormat
        {
            get { return base.GetString("MailTitleFormat", string.Empty); }
            set { base.SetExtendedAttribute("MailTitleFormat", value); }
        }

        public bool IsSMS
        {
            get { return base.GetBool("IsSMS", false); }
            set { base.SetExtendedAttribute("IsSMS", value.ToString()); }
        }

        public string SMSTo
        {
            get { return base.GetString("SMSTo", string.Empty); }
            set { base.SetExtendedAttribute("SMSTo", value); }
        }

        public string SMSTitle
        {
            get { return base.GetString("SMSTitle", string.Empty); }
            set { base.SetExtendedAttribute("SMSTitle", value); }
        }

        public override string ToString()
        {
            return TranslateUtils.NameValueCollectionToString(base.Attributes);
        }
    }
}
