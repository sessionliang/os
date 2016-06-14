using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Model;

namespace SiteServer.CMS.Model
{
	public class TaskPublishInfo : ExtendedAttributes
	{
        public TaskPublishInfo(string serviceParameters)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(serviceParameters);
            base.SetExtendedAttribute(nameValueCollection);
        }

        public string PublishTypes
        {
            get { return this.GetExtendedAttribute("PublishTypes"); }
            set { base.SetExtendedAttribute("PublishTypes", value); }
        }

        public string Filter
        {
            get { return this.GetExtendedAttribute("Filter"); }
            set { base.SetExtendedAttribute("Filter", value); }
        }
	}
}
