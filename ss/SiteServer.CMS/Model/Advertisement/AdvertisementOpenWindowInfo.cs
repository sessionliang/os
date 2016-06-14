using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
    public class AdvertisementOpenWindowInfo : ExtendedAttributes
	{
        public AdvertisementOpenWindowInfo(string settings)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(settings);
            base.SetExtendedAttribute(nameValueCollection);
        }

        public AdvertisementOpenWindowInfo(string fileUrl, int height, int width)
        {
            this.FileUrl = fileUrl;
            this.Height = height;
            this.Width = width;
        }

        public string FileUrl
		{
            get { return this.GetExtendedAttribute("FileUrl"); }
            set { base.SetExtendedAttribute("FileUrl", value); }
		}

		public int Height
		{
            get { return this.GetInt("Height", 0); }
            set { base.SetExtendedAttribute("Height", value.ToString()); }
		}

		public int Width
		{
            get { return this.GetInt("Width", 0); }
            set { base.SetExtendedAttribute("Width", value.ToString()); }
		}
	}
}
