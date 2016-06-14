using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Model; 
 
namespace SiteServer.BBS.Model
{
    public class AdInfo
    {
        private int id;
        private int publishmentSystemID;
        private string adName;
        private EAdType adType;
        private EAdLocation adLocation;
        private string code;
        private string textWord;
        private string textLink; 
        private string textColor;
        private int textFontSize;
        private string imageUrl;
        private string imageLink;
        private int imageWidth;
        private int imageHeight;
        private string imageAlt;
        private bool isEnabled;
        private bool isDateLimited;
        private DateTime startDate;
        private DateTime endDate;

        //public AdInfo()
        //{
        //    this.id = 0;
        //    this.publishmentSystemID = 0;
        //    this.adName = string.Empty;
        //    this.adType = EAdType.HtmlCode;
        //    this.adLocation = EAdLocation.NavBanner;
        //    this.code = string.Empty;
        //    this.textWord = string.Empty;
        //    this.textLink = string.Empty;
        //    this.textColor = string.Empty;
        //    this.textFontSize = 0;
        //    this.imageUrl = string.Empty;
        //    this.imageLink = string.Empty;
        //    this.imageWidth = 0;
        //    this.imageHeight = 0;
        //    this.imageAlt = string.Empty;
        //    this.isEnabled = true;
        //    this.isDateLimited = false;
        //    this.startDate = DateTime.Now;
        //    this.endDate = DateTime.Now.AddMonths(1);
        //}

        public AdInfo(int id, int publishmentSystemID, string adName, EAdType adType, EAdLocation adLocation, string code, string textWord, string textLink, string textColor, int textFontSize, string imageUrl, string imageLink, int imageWidth, int imageHeight, string imageAlt, bool isEnabled, bool isDateLimited, DateTime startDate, DateTime endDate)
        {
            this.id = id;
            this.publishmentSystemID = publishmentSystemID;
            this.adName = adName;
            this.adType = adType;
            this.adLocation = adLocation;
            this.code = code;
            this.textWord = textWord;
            this.textLink = textLink;
            this.textColor = textColor;
            this.textFontSize = textFontSize;
            this.imageUrl = imageUrl;
            this.imageLink = imageLink;
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
            this.imageAlt = imageAlt;
            this.isEnabled = isEnabled;
            this.isDateLimited = isDateLimited;
            this.startDate = startDate;
            this.endDate = endDate;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public string AdName
        {
            get { return adName; }
            set { adName = value; }
        }

        public EAdType AdType
        {
            get { return adType; }
            set { adType = value; }
        }

        public EAdLocation AdLocation
        {
            get { return adLocation; }
            set { adLocation = value; }
        }

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public string TextWord
        {
            get { return textWord; }
            set { textWord = value; }
        }

        public string TextLink
        {
            get { return textLink; }
            set { textLink = value; }
        }

        public string TextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }

        public int TextFontSize
        {
            get { return textFontSize; }
            set { textFontSize = value; }
        }

        public string ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = value; }
        }

        public string ImageLink
        {
            get { return imageLink; }
            set { imageLink = value; }
        }

        public int ImageWidth
        {
            get { return imageWidth; }
            set { imageWidth = value; }
        }

        public int ImageHeight
        {
            get { return imageHeight; }
            set { imageHeight = value; }
        }

        public string ImageAlt
        {
            get { return imageAlt; }
            set { imageAlt = value; }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        public bool IsDateLimited
        {
            get { return isDateLimited; }
            set { isDateLimited = value; }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }
    }
}
