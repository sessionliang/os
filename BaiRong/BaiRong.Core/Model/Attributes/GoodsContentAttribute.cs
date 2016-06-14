using System;
using System.Collections;
using BaiRong.Core;

namespace BaiRong.Model
{
    public class GoodsContentAttribute
    {
        protected GoodsContentAttribute()
        {
        }

        //隐藏字段
        public const string BrandID = "BrandID";
        public const string BrandValue = "BrandValue";
        public const string Sales = "Sales";
        public const string SpecIDCollection = "SpecIDCollection";
        public const string SpecItemIDCollection = "SpecItemIDCollection";
        public const string SKUCount = "SKUCount";

        //显示字段
        public const string SN = "SN";
        public const string Keywords = "Keywords";
        public const string Summary = "Summary";
        public const string ImageUrl = "ImageUrl";
        public const string ThumbUrl = "ThumbUrl";
        public const string LinkUrl = "LinkUrl";
        public const string Content = "Content";
        public const string PriceCost = "PriceCost";
        public const string PriceMarket = "PriceMarket";
        public const string PriceSale = "PriceSale";
        public const string Stock = "Stock";
        public const string FileUrl = "FileUrl";
        public const string IsRecommend = "IsRecommend";
        public const string IsNew = "IsNew";
        public const string IsHot = "IsHot";
        public const string IsOnSale = "IsOnSale";
        public const string F1 = "F1";
        public const string F2 = "F2";
        public const string F3 = "F3";
        public const string F4 = "F4";
        public const string F5 = "F5";
        public const string F6 = "F6";
        public const string F7 = "F7";
        public const string F8 = "F8";
        public const string F9 = "F9";

        //不存在
        public const string PriceSaved = "PriceSaved";  //节省价格
        public const string Brand = "Brand";            //品牌
        public const string PREFIX_Spec = "Spec";

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
                    systemAttributes.Add(BrandID.ToLower());
                    systemAttributes.Add(BrandValue.ToLower());
                    systemAttributes.Add(Sales.ToLower());
                    systemAttributes.Add(SpecIDCollection.ToLower());
                    systemAttributes.Add(SpecItemIDCollection.ToLower());
                    systemAttributes.Add(SKUCount.ToLower());

                    systemAttributes.Add(SN.ToLower());
                    systemAttributes.Add(Keywords.ToLower());
                    systemAttributes.Add(Summary.ToLower());
                    systemAttributes.Add(ImageUrl.ToLower());
                    systemAttributes.Add(ThumbUrl.ToLower());
                    systemAttributes.Add(LinkUrl.ToLower());
                    systemAttributes.Add(Content.ToLower());
                    systemAttributes.Add(PriceCost.ToLower());
                    systemAttributes.Add(PriceMarket.ToLower());
                    systemAttributes.Add(PriceSale.ToLower());
                    systemAttributes.Add(Stock.ToLower());
                    systemAttributes.Add(FileUrl.ToLower());
                    systemAttributes.Add(IsRecommend.ToLower());
                    systemAttributes.Add(IsNew.ToLower());
                    systemAttributes.Add(IsHot.ToLower());
                    systemAttributes.Add(IsOnSale.ToLower());

                    systemAttributes.Add(F1.ToLower());
                    systemAttributes.Add(F2.ToLower());
                    systemAttributes.Add(F3.ToLower());
                    systemAttributes.Add(F4.ToLower());
                    systemAttributes.Add(F5.ToLower());
                    systemAttributes.Add(F6.ToLower());
                    systemAttributes.Add(F7.ToLower());
                    systemAttributes.Add(F8.ToLower());
                    systemAttributes.Add(F9.ToLower());
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
                    excludeAttributes.Add(IsNew.ToLower());
                    excludeAttributes.Add(IsHot.ToLower());
                    excludeAttributes.Add(IsOnSale.ToLower());
                }

                return excludeAttributes;
            }
        }

        public static bool IsExtendAttribute(string attributeName)
        {
            if (StringUtils.EqualsIgnoreCase(attributeName, GoodsContentAttribute.F1) ||
                StringUtils.EqualsIgnoreCase(attributeName, GoodsContentAttribute.F2) ||
                StringUtils.EqualsIgnoreCase(attributeName, GoodsContentAttribute.F3) ||
                StringUtils.EqualsIgnoreCase(attributeName, GoodsContentAttribute.F4) ||
                StringUtils.EqualsIgnoreCase(attributeName, GoodsContentAttribute.F5) ||
                StringUtils.EqualsIgnoreCase(attributeName, GoodsContentAttribute.F6) ||
                StringUtils.EqualsIgnoreCase(attributeName, GoodsContentAttribute.F7) ||
                StringUtils.EqualsIgnoreCase(attributeName, GoodsContentAttribute.F8) ||
                StringUtils.EqualsIgnoreCase(attributeName, GoodsContentAttribute.F9))
            {
                return true;
            }
            return false;
        }
    }
}
