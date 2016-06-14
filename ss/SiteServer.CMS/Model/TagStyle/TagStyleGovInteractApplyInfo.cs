using System;
using BaiRong.Model;
using BaiRong.Core;
using System.Collections.Specialized;

namespace SiteServer.CMS.Model
{
    public class TagStyleGovInteractApplyInfo : ExtendedAttributes, ITagStyleMailSMSBaseInfo
    {
        public TagStyleGovInteractApplyInfo(string settingsXML)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(settingsXML);
            base.SetExtendedAttribute(nameValueCollection);
        }

        public bool IsAnomynous         //‘ –Ìƒ‰√˚∆¿¬€
        {
            get { return base.GetBool("IsAnomynous", true); }
            set { base.SetExtendedAttribute("IsAnomynous", value.ToString()); }
        }

        public bool IsValidateCode
        {
            get { return base.GetBool("IsValidateCode", true); }
            set { base.SetExtendedAttribute("IsValidateCode", value.ToString()); }
        }

        public bool IsMail
        {
            get { return base.GetBool("IsMail", false); }
            set { base.SetExtendedAttribute("IsMail", value.ToString()); }
        }

        public ETriState MailReceiver
        {
            get { return ETriStateUtils.GetEnumType(base.GetString("MailReceiver", ETriStateUtils.GetValue(ETriState.True))); }
            set { base.SetExtendedAttribute("MailReceiver", ETriStateUtils.GetValue(value)); }
        }

        public string MailTo
        {
            get { return base.GetString("MailTo", string.Empty); }
            set { base.SetExtendedAttribute("MailTo", value); }
        }

        public string MailFiledName
        {
            get { return base.GetString("MailFiledName", string.Empty); }
            set { base.SetExtendedAttribute("MailFiledName", value); }
        }

        public string MailTitle
        {
            get { return base.GetString("MailTitle", string.Empty); }
            set { base.SetExtendedAttribute("MailTitle", value); }
        }

        public bool IsMailTemplate
        {
            get { return base.GetBool("IsMailTemplate", false); }
            set { base.SetExtendedAttribute("IsMailTemplate", value.ToString()); }
        }

        public string MailContent
        {
            get { return base.GetString("MailContent", string.Empty); }
            set { base.SetExtendedAttribute("MailContent", value); }
        }

        public bool IsSMS
        {
            get { return base.GetBool("IsSMS", false); }
            set { base.SetExtendedAttribute("IsSMS", value.ToString()); }
        }

        public ETriState SMSReceiver
        {
            get { return ETriStateUtils.GetEnumType(base.GetString("SMSReceiver", ETriStateUtils.GetValue(ETriState.True))); }
            set { base.SetExtendedAttribute("SMSReceiver", ETriStateUtils.GetValue(value)); }
        }

        public string SMSTo
        {
            get { return base.GetString("SMSTo", string.Empty); }
            set { base.SetExtendedAttribute("SMSTo", value); }
        }

        public string SMSFiledName
        {
            get { return base.GetString("SMSFiledName", string.Empty); }
            set { base.SetExtendedAttribute("SMSFiledName", value); }
        }

        public bool IsSMSTemplate
        {
            get { return base.GetBool("IsSMSTemplate", false); }
            set { base.SetExtendedAttribute("IsSMSTemplate", value.ToString()); }
        }

        public string SMSContent
        {
            get { return base.GetString("SMSContent", string.Empty); }
            set { base.SetExtendedAttribute("SMSContent", value); }
        }

        public override string ToString()
        {
            return TranslateUtils.NameValueCollectionToString(base.Attributes);
        }
    }
}
