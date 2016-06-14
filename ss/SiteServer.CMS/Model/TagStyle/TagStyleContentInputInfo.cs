using System;
using BaiRong.Model;
using BaiRong.Core;
using System.Collections.Specialized;

namespace SiteServer.CMS.Model
{
    public class TagStyleContentInputInfo : ExtendedAttributes
    {
        public TagStyleContentInputInfo(string settingsXML)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(settingsXML);
            base.SetExtendedAttribute(nameValueCollection);
        }

        public int ChannelID
        {
            get { return base.GetInt("ChannelID", 0); }
            set { base.SetExtendedAttribute("ChannelID", value.ToString()); }
        }

        public bool IsChecked
        {
            get { return base.GetBool("IsChecked", true); }
            set { base.SetExtendedAttribute("IsChecked", value.ToString()); }
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

        public bool IsChannel         //显示提交栏目
        {
            get { return base.GetBool("IsChannel", false); }
            set { base.SetExtendedAttribute("IsChannel", value.ToString()); }
        }

        public bool IsAnomynous         //允许匿名评论
        {
            get { return base.GetBool("IsAnomynous", true); }
            set { base.SetExtendedAttribute("IsAnomynous", value.ToString()); }
        }

        public bool IsValidateCode
        {
            get { return base.GetBool("IsValidateCode", true); }
            set { base.SetExtendedAttribute("IsValidateCode", value.ToString()); }
        }

        public bool IsSuccessHide
        {
            get { return base.GetBool("IsSuccessHide", true); }
            set { base.SetExtendedAttribute("IsSuccessHide", value.ToString()); }
        }

        public bool IsSuccessReload
        {
            get { return base.GetBool("IsSuccessReload", false); }
            set { base.SetExtendedAttribute("IsSuccessReload", value.ToString()); }
        }

        public bool IsCtrlEnter
        {
            get { return base.GetBool("IsCtrlEnter", true); }
            set { base.SetExtendedAttribute("IsCtrlEnter", value.ToString()); }
        }

        public override string ToString()
        {
            return TranslateUtils.NameValueCollectionToString(base.Attributes);
        }
    }
}
