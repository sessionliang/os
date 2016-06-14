using System;
using System.Collections;

namespace BaiRong.Model
{
	public class BackgroundContentAttribute
	{
		protected BackgroundContentAttribute()
		{
		}

        //system
        public const string SubTitle = "SubTitle";
        public const string ImageUrl = "ImageUrl";
        public const string VideoUrl = "VideoUrl";
        public const string FileUrl = "FileUrl";
        public const string LinkUrl = "LinkUrl";
        public const string Author = "Author";
        public const string Source = "Source";
        public const string Summary = "Summary";
        public const string IsRecommend = "IsRecommend";
        public const string IsHot = "IsHot";
        public const string IsColor = "IsColor";
        public const string Content = "Content";
        //不存在
        public const string TitleFormatString = "TitleFormatString";
        public const string StarSetting = "StarSetting";
        public const string Star = "Star";
        public const string Digg = "Digg";
        public const string PageContent = "PageContent";
        public const string NavigationUrl = "NavigationUrl";
        public const string CountOfPhotos = "CountOfPhotos";			//商品图片数

        public static ArrayList AllAttributes
        {
            get
            {
                ArrayList arraylist = new ArrayList(ContentAttribute.AllAttributes);
                arraylist.AddRange(SystemAttributes);
                return arraylist;
            }
        }

        private static ArrayList systemAttributes;
        public static ArrayList SystemAttributes
        {
            get
            {
                if (systemAttributes == null)
                {
                    systemAttributes = new ArrayList();
                    systemAttributes.Add(SubTitle.ToLower());
                    systemAttributes.Add(ImageUrl.ToLower());
                    systemAttributes.Add(VideoUrl.ToLower());
                    systemAttributes.Add(FileUrl.ToLower());
                    systemAttributes.Add(LinkUrl.ToLower());
                    systemAttributes.Add(Content.ToLower());
                    systemAttributes.Add(Author.ToLower());
                    systemAttributes.Add(Source.ToLower());
                    systemAttributes.Add(Summary.ToLower());
                    systemAttributes.Add(IsRecommend.ToLower());
                    systemAttributes.Add(IsHot.ToLower());
                    systemAttributes.Add(IsColor.ToLower());
                }

                return systemAttributes;
            }
        }

        private static ArrayList excludeAttributes;
        public static ArrayList ExcludeAttributes
        {
            get
            {
                if (excludeAttributes == null)
                {
                    excludeAttributes = new ArrayList(ContentAttribute.ExcludeAttributes);
                    excludeAttributes.Add(IsRecommend.ToLower());
                    excludeAttributes.Add(IsHot.ToLower());
                    excludeAttributes.Add(IsColor.ToLower());
                }

                return excludeAttributes;
            }
        }
	}
}
