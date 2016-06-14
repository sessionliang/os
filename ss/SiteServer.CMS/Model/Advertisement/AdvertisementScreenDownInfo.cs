using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
    public class AdvertisementScreenDownInfo : ExtendedAttributes
	{
        public AdvertisementScreenDownInfo(string settings)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(settings);
            base.SetExtendedAttribute(nameValueCollection);
        }

        public AdvertisementScreenDownInfo(string navigationUrl, string imageUrl, int delay, int height, int width)
        {
            this.NavigationUrl = navigationUrl;
            this.ImageUrl = imageUrl;
            this.Delay = delay;
            this.Height = height;
            this.Width = width;
        }

        public string NavigationUrl
        {
            get { return this.GetExtendedAttribute("NavigationUrl"); }
            set { base.SetExtendedAttribute("NavigationUrl", value); }
        }

        public string ImageUrl
		{
            get { return this.GetExtendedAttribute("ImageUrl"); }
            set { base.SetExtendedAttribute("ImageUrl", value); }
		}

        public int Delay
        {
            get { return this.GetInt("Delay", 0); }
            set { base.SetExtendedAttribute("Delay", value.ToString()); }
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
