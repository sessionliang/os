using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Model;

namespace SiteServer.CMS.Model
{
	public class TaskGatherInfo : ExtendedAttributes
	{
        public TaskGatherInfo(string serviceParameters)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(serviceParameters);
            base.SetExtendedAttribute(nameValueCollection);
        }

        public string PublishmentSystemIDCollection
        {
            get { return this.GetExtendedAttribute("PublishmentSystemIDCollection"); }
            set { base.SetExtendedAttribute("PublishmentSystemIDCollection", value); }
        }

        public string WebGatherNames
		{
            get { return this.GetExtendedAttribute("WebGatherNames"); }
            set { base.SetExtendedAttribute("WebGatherNames", value); }
		}

        public string DatabaseGatherNames
        {
            get { return this.GetExtendedAttribute("DatabaseGatherNames"); }
            set { base.SetExtendedAttribute("DatabaseGatherNames", value); }
        }

        public string FileGatherNames
        {
            get { return this.GetExtendedAttribute("FileGatherNames"); }
            set { base.SetExtendedAttribute("FileGatherNames", value); }
        }
	}
}
