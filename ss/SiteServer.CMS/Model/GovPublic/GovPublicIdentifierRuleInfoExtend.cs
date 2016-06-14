using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Model;

namespace SiteServer.CMS.Model
{
	public class GovPublicIdentifierRuleInfoExtend : ExtendedAttributes
	{
        public GovPublicIdentifierRuleInfoExtend(string settingsXML)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(settingsXML);
            base.SetExtendedAttribute(nameValueCollection);
        }

        //！！！！！！！！！！Sequence！！！！！！！！！！/

        public bool IsSequenceChannelZero
        {
            get { return base.GetBool("IsSequenceChannelZero", true); }
            set { base.SetExtendedAttribute("IsSequenceChannelZero", value.ToString()); }
        }

        public bool IsSequenceDepartmentZero
        {
            get { return base.GetBool("IsSequenceDepartmentZero", false); }
            set { base.SetExtendedAttribute("IsSequenceDepartmentZero", value.ToString()); }
        }

        public bool IsSequenceYearZero
        {
            get { return base.GetBool("IsSequenceYearZero", true); }
            set { base.SetExtendedAttribute("IsSequenceYearZero", value.ToString()); }
        }

        public override string ToString()
        {
            return TranslateUtils.NameValueCollectionToString(base.Attributes);
        }
	}
}
