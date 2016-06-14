using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
    public class AdvertisementFloatImageInfo : ExtendedAttributes
	{
        public AdvertisementFloatImageInfo(string settings)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(settings);
            base.SetExtendedAttribute(nameValueCollection);
        }

        public AdvertisementFloatImageInfo(bool isCloseable, EPositionType positionType, int positionX, int positionY, ERollingType rollingType, string navigationUrl, string imageUrl, int height, int width)
        {
            this.IsCloseable = isCloseable;
            this.PositionType = positionType;
            this.PositionX = positionX;
            this.PositionY = positionY;
            this.RollingType = rollingType;
            this.NavigationUrl = navigationUrl;
            this.ImageUrl = imageUrl;
            this.Height = height;
            this.Width = width;
        }

		public bool IsCloseable
		{
            get { return this.GetBool("IsCloseable", false); }
            set { base.SetExtendedAttribute("IsCloseable", value.ToString()); }
		}

        public EPositionType PositionType
		{
            get { return EPositionTypeUtils.GetEnumType(this.GetExtendedAttribute("PositionType")); }
            set { base.SetExtendedAttribute("PositionType", EPositionTypeUtils.GetValue(value)); }
		}

        public int PositionX
		{
            get { return this.GetInt("PositionX", 0); }
            set { base.SetExtendedAttribute("PositionX", value.ToString()); }
		}

        public int PositionY
		{
            get { return this.GetInt("PositionY", 0); }
            set { base.SetExtendedAttribute("PositionY", value.ToString()); }
		}

		public ERollingType RollingType
		{
            get { return ERollingTypeUtils.GetEnumType(this.GetExtendedAttribute("RollingType")); }
            set { base.SetExtendedAttribute("RollingType", ERollingTypeUtils.GetValue(value)); }
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
