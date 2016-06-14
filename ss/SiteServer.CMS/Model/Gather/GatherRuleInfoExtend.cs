using System;
using BaiRong.Model;
using BaiRong.Core;
using System.Collections.Specialized;

namespace SiteServer.CMS.Model
{
    public class GatherRuleInfoExtend : ExtendedAttributes
    {
        public GatherRuleInfoExtend(string extendValues)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(extendValues);
            base.SetExtendedAttribute(nameValueCollection);
        }

        public int GatherNum
        {
            get { return base.GetInt("GatherNum", 0); }
            set { base.SetExtendedAttribute("GatherNum", value.ToString()); }
        }

        public bool IsSaveImage
        {
            get { return base.GetBool("IsSaveImage", true); }
            set { base.SetExtendedAttribute("IsSaveImage", value.ToString()); }
        }

        public bool IsSetFirstImageAsImageUrl
        {
            get { return base.GetBool("IsSetFirstImageAsImageUrl", true); }
            set { base.SetExtendedAttribute("IsSetFirstImageAsImageUrl", value.ToString()); }
        }

        public bool IsEmptyContentAllowed
        {
            get { return base.GetBool("IsEmptyContentAllowed", false); }
            set { base.SetExtendedAttribute("IsEmptyContentAllowed", value.ToString()); }
        }

        public bool IsSameTitleAllowed
        {
            get { return base.GetBool("IsSameTitleAllowed", false); }
            set { base.SetExtendedAttribute("IsSameTitleAllowed", value.ToString()); }
        }

        public bool IsChecked
        {
            get { return base.GetBool("IsChecked", true); }
            set { base.SetExtendedAttribute("IsChecked", value.ToString()); }
        }

        public bool IsAutoCreate
        {
            get { return base.GetBool("IsAutoCreate", false); }
            set { base.SetExtendedAttribute("IsAutoCreate", value.ToString()); }
        }

        public bool IsOrderByDesc
        {
            get { return base.GetBool("IsOrderByDesc", true); }
            set { base.SetExtendedAttribute("IsOrderByDesc", value.ToString()); }
        }

        public string ContentContentStart2
        {
            get { return base.GetString("ContentContentStart2", string.Empty); }
            set { base.SetExtendedAttribute("ContentContentStart2", value); }
        }

        public string ContentContentEnd2
        {
            get { return base.GetString("ContentContentEnd2", string.Empty); }
            set { base.SetExtendedAttribute("ContentContentEnd2", value); }
        }

        public string ContentContentStart3
        {
            get { return base.GetString("ContentContentStart3", string.Empty); }
            set { base.SetExtendedAttribute("ContentContentStart3", value); }
        }

        public string ContentContentEnd3
        {
            get { return base.GetString("ContentContentEnd3", string.Empty); }
            set { base.SetExtendedAttribute("ContentContentEnd3", value); }
        }

        public string ContentReplaceFrom
        {
            get { return base.GetString("ContentReplaceFrom", string.Empty); }
            set { base.SetExtendedAttribute("ContentReplaceFrom", value); }
        }

        public string ContentReplaceTo
        {
            get { return base.GetString("ContentReplaceTo", string.Empty); }
            set { base.SetExtendedAttribute("ContentReplaceTo", value); }
        }

        public override string ToString()
        {
            return TranslateUtils.NameValueCollectionToString(base.Attributes);
        }
    }
}
