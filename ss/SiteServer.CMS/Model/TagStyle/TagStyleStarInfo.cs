using System;
using BaiRong.Model;
using BaiRong.Core;
using System.Collections.Specialized;

namespace SiteServer.CMS.Model
{
    public class TagStyleStarInfo : ExtendedAttributes
    {
        public TagStyleStarInfo(string settingsXML)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(settingsXML);
            base.SetExtendedAttribute(nameValueCollection);
        }

        public string Type
        {
            get { return base.GetString("Type", "All"); }
            set { base.SetExtendedAttribute("Type", value.ToString()); }
        }

        public string GoodText
        {
            get { return base.GetString("GoodText", "顶一下"); }
            set { base.SetExtendedAttribute("GoodText", value.ToString()); }
        }

        public string BadText
        {
            get { return base.GetString("BadText", "踩一下"); }
            set { base.SetExtendedAttribute("BadText", value.ToString()); }
        }

        public string Theme
        {
            get { return base.GetString("Theme", "Style1"); }
            set { base.SetExtendedAttribute("Theme", value.ToString()); }
        }

        public override string ToString()
        {
            return TranslateUtils.NameValueCollectionToString(base.Attributes);
        }
    }
}
