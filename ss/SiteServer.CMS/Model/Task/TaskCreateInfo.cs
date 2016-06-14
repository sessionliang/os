using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Model;

namespace SiteServer.CMS.Model
{
	public class TaskCreateInfo : ExtendedAttributes
	{
        public TaskCreateInfo(string serviceParameters)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(serviceParameters);
            base.SetExtendedAttribute(nameValueCollection);
        }

        public bool IsCreateAll
        {
            get { return base.GetBool("IsCreateAll", false); }
            set { base.SetExtendedAttribute("IsCreateAll", value.ToString()); }
        }

        public string ChannelIDCollection
		{
            get { return this.GetExtendedAttribute("ChannelIDCollection"); }
            set { base.SetExtendedAttribute("ChannelIDCollection", value); }
		}

        public string CreateTypes
        {
            get { return this.GetExtendedAttribute("CreateTypes"); }
            set { base.SetExtendedAttribute("CreateTypes", value); }
        }

        public bool IsCreateSiteMap
        {
            get { return base.GetBool("IsCreateSiteMap", false); }
            set { base.SetExtendedAttribute("IsCreateSiteMap", value.ToString()); }
        }
	}
}
