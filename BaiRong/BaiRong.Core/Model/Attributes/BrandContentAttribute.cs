using System;
using System.Collections;
using BaiRong.Core;

namespace BaiRong.Model
{
    public class BrandContentAttribute
	{
        protected BrandContentAttribute()
		{
		}

        //ÏÔÊ¾×Ö¶Î
        public const string BrandUrl = "BrandUrl";
        public const string ImageUrl = "ImageUrl";
        public const string LinkUrl = "LinkUrl";
        public const string Content = "Content";
        public const string IsRecommend = "IsRecommend";
        public const string F1 = "F1";
        public const string F2 = "F2";
        public const string F3 = "F3";

        //²»´æÔÚ
        public const string CountOfBrandGoods = "CountOfBrandGoods";

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
                    systemAttributes.Add(BrandUrl.ToLower());
                    systemAttributes.Add(ImageUrl.ToLower());
                    systemAttributes.Add(LinkUrl.ToLower());
                    systemAttributes.Add(Content.ToLower());
                    systemAttributes.Add(IsRecommend.ToLower());

                    systemAttributes.Add(F1.ToLower());
                    systemAttributes.Add(F2.ToLower());
                    systemAttributes.Add(F3.ToLower());
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
                }

                return excludeAttributes;
            }
        }

        public static bool IsExtendAttribute(string attributeName)
        {
            if (StringUtils.EqualsIgnoreCase(attributeName, BrandContentAttribute.F1) ||
                StringUtils.EqualsIgnoreCase(attributeName, BrandContentAttribute.F2) ||
                StringUtils.EqualsIgnoreCase(attributeName, BrandContentAttribute.F3))
            {
                return true;
            }
            return false;
        }
	}
}
