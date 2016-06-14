using System;
using BaiRong.Model;
using BaiRong.Core;
using System.Collections.Specialized;

namespace SiteServer.CMS.Model
{
    public class TagStyleSearchInputInfo : ExtendedAttributes
    {
        public TagStyleSearchInputInfo(string settingsXML)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(settingsXML);
            base.SetExtendedAttribute(nameValueCollection);
        }

        public string SearchUrl
        {
            get { return base.GetString("SearchUrl", "{Stl.SiteUrl}/utils/search.html"); }
            set { base.SetExtendedAttribute("SearchUrl", value.ToString()); }
        }

        public bool OpenWin
        {
            get { return base.GetBool("OpenWin", true); }
            set { base.SetExtendedAttribute("OpenWin", value.ToString()); }
        }

        public string InputWidth
        {
            get { return base.GetString("InputWidth", "300"); }
            set { base.SetExtendedAttribute("InputWidth", value.ToString()); }
        }

        public bool IsType
        {
            get { return base.GetBool("IsType", false); }
            set { base.SetExtendedAttribute("IsType", value.ToString()); }
        }

        public bool IsChannel
        {
            get { return base.GetBool("IsChannel", false); }
            set { base.SetExtendedAttribute("IsChannel", value.ToString()); }
        }

        public bool IsChannelRadio
        {
            get { return base.GetBool("IsChannelRadio", false); }
            set { base.SetExtendedAttribute("IsChannelRadio", value.ToString()); }
        }

        public bool IsDate
        {
            get { return base.GetBool("IsDate", false); }
            set { base.SetExtendedAttribute("IsDate", value.ToString()); }
        }

        public bool IsDateFrom
        {
            get { return base.GetBool("IsDateFrom", false); }
            set { base.SetExtendedAttribute("IsDateFrom", value.ToString()); }
        }

        public override string ToString()
        {
            return TranslateUtils.NameValueCollectionToString(base.Attributes);
        }
    }
}
