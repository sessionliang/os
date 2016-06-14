using System;
using BaiRong.Model;

namespace BaiRong.Model
{
	public class TagInfo
	{
		private int tagID;
        private string productID;
        private int publishmentSystemID;
        private string contentIDCollection;
        private string tag;
        private int useNum;

		public TagInfo()
		{
            this.tagID = 0;
            this.productID = string.Empty;
            this.publishmentSystemID = 0;
            this.contentIDCollection = string.Empty;
            this.tag = string.Empty;
            this.useNum = 0;
		}

        public TagInfo(int tagID, string productID, int publishmentSystemID, string contentIDCollection, string tag, int useNum) 
		{
            this.tagID = tagID;
            this.productID = productID;
            this.publishmentSystemID = publishmentSystemID;
            this.contentIDCollection = contentIDCollection;
            this.tag = tag;
            this.useNum = useNum;
		}

        public int TagID
		{
            get { return tagID; }
            set { tagID = value; }
		}

        public string ProductID
		{
            get { return productID; }
            set { productID = value; }
		}

        public int PublishmentSystemID
		{
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
		}

        public string ContentIDCollection
        {
            get { return contentIDCollection; }
            set { contentIDCollection = value; }
        }

        public string Tag
		{
            get { return tag; }
            set { tag = value; }
		}

        public int UseNum
		{
            get { return useNum; }
            set { useNum = value; }
		}

        private int level = 0;
        public int Level
        {
            get { return level; }
            set { level = value; }
        }
	}
}
