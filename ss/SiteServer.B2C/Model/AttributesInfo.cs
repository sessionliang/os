using System;

namespace SiteServer.B2C.Model
{
	public class AttributesInfo
	{
        private bool isFile = false;
        private bool isFileExists = false;
        private bool isImage = false;
        private bool isImageExists = false;

        private bool isTop = false;
        private bool isTopExists = false;
        private bool isRecommend = false;
        private bool isRecommendExists = false;
        private bool isHot = false;
        private bool isHotExists = false;
        private bool isColor = false;
        private bool isColorExists = false;

        private bool isNew = false;
        private bool isNewExists = false;

		public AttributesInfo()
		{
		}

        public bool IsFile
        {
            get { return isFile; }
            set
            {
                isFileExists = true;
                isFile = value;
            }
        }

        public bool IsFileExists
        {
            get { return isFileExists; }
        }

        public bool IsImage
        {
            get { return isImage; }
            set
            {
                isImageExists = true;
                isImage = value;
            }
        }

        public bool IsImageExists
        {
            get { return isImageExists; }
        }

        public bool IsTop
        {
            get { return isTop; }
            set
            {
                isTopExists = true;
                isTop = value;
            }
        }

        public bool IsTopExists
        {
            get { return isTopExists; }
        }

        public bool IsRecommend
        {
            get { return isRecommend; }
            set
            {
                isRecommendExists = true;
                isRecommend = value;
            }
        }

        public bool IsRecommendExists
        {
            get { return isRecommendExists; }
        }

        public bool IsHot
        {
            get { return isHot; }
            set
            {
                isHotExists = true;
                isHot = value;
            }
        }

        public bool IsHotExists
        {
            get { return isHotExists; }
        }

        public bool IsColor
        {
            get { return isColor; }
            set
            {
                isColorExists = true;
                isColor = value;
            }
        }

        public bool IsColorExists
        {
            get { return isColorExists; }
        }

        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNewExists = true;
                isNew = value;
            }
        }

        public bool IsNewExists
        {
            get { return isNewExists; }
        }
	}
}
