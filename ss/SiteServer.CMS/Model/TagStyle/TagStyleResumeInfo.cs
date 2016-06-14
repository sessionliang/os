using System;
using BaiRong.Model;
using BaiRong.Core;
using System.Collections.Specialized;

namespace SiteServer.CMS.Model
{
    public class TagStyleResumeInfo : ExtendedAttributes
    {
        public TagStyleResumeInfo(string settingsXML)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(settingsXML);
            base.SetExtendedAttribute(nameValueCollection);
        }

        public string MessageSuccess
        {
            get { return base.GetString("MessageSuccess", string.Empty); }
            set { base.SetExtendedAttribute("MessageSuccess", value); }
        }

        public string MessageFailure
        {
            get { return base.GetString("MessageFailure", string.Empty); }
            set { base.SetExtendedAttribute("MessageFailure", value); }
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

        public bool IsAnomynous
        {
            get { return base.GetBool("IsAnomynous", true); }
            set { base.SetExtendedAttribute("IsAnomynous", value.ToString()); }
        }

        public override string ToString()
        {
            return TranslateUtils.NameValueCollectionToString(base.Attributes);
        }
    }
}
