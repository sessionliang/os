using System;
using BaiRong.Model;
using System.Collections;

namespace SiteServer.CMS.Model
{
	public class GoodsContentInfo : ContentInfo
	{
		public GoodsContentInfo() : base()
		{
            this.BrandID = 0;
            this.Sales = 0;
            this.SKUCount = 0;
		}

        public GoodsContentInfo(object dataItem)
            : base(dataItem)
		{
		}

        public int BrandID
		{
            get { return this.GetInt(GoodsContentAttribute.BrandID, 0); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.BrandID, value.ToString()); }
		}

        public string BrandValue
        {
            get { return this.GetString(GoodsContentAttribute.BrandValue, string.Empty); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.BrandValue, value); }
        }

        public int Sales
        {
            get { return this.GetInt(GoodsContentAttribute.Sales, 0); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.Sales, value.ToString()); }
        }

        public string SpecIDCollection
        {
            get { return this.GetString(GoodsContentAttribute.SpecIDCollection, string.Empty); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.SpecIDCollection, value); }
        }

        public string SpecItemIDCollection
        {
            get { return this.GetString(GoodsContentAttribute.SpecItemIDCollection, string.Empty); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.SpecItemIDCollection, value); }
        }

        public int SKUCount
        {
            get { return this.GetInt(GoodsContentAttribute.SKUCount, 0); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.SKUCount, value.ToString()); }
        }

        public string SN
        {
            get { return this.GetString(GoodsContentAttribute.SN, string.Empty); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.SN, value); }
        }

        public string Keywords
        {
            get { return this.GetString(GoodsContentAttribute.Keywords, string.Empty); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.Keywords, value); }
        }

        public string Summary
        {
            get { return this.GetString(GoodsContentAttribute.Summary, string.Empty); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.Summary, value); }
        }

        public string ImageUrl
        {
            get { return this.GetString(GoodsContentAttribute.ImageUrl, string.Empty); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.ImageUrl, value); }
        }

        public string ThumbUrl
        {
            get { return this.GetString(GoodsContentAttribute.ThumbUrl, string.Empty); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.ThumbUrl, value); }
        }

        public string LinkUrl
        {
            get { return this.GetString(GoodsContentAttribute.LinkUrl, string.Empty); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.LinkUrl, value); }
        }

        public string Content
        {
            get { return this.GetString(GoodsContentAttribute.Content, string.Empty); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.Content, value); }
        }

        public decimal PriceCost
        {
            get { return this.GetDecimal(GoodsContentAttribute.PriceCost, 0); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.PriceCost, value.ToString()); }
        }

        public decimal PriceMarket
        {
            get { return this.GetDecimal(GoodsContentAttribute.PriceMarket, 0); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.PriceMarket, value.ToString()); }
        }

        public decimal PriceSale
        {
            get { return this.GetDecimal(GoodsContentAttribute.PriceSale, 0); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.PriceSale, value.ToString()); }
        }

        public int Stock
        {
            get { return this.GetInt(GoodsContentAttribute.Stock, -1); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.Stock, value.ToString()); }
        }

        public string FileUrl
        {
            get { return this.GetString(GoodsContentAttribute.FileUrl, string.Empty); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.FileUrl, value); }
        }

        public bool IsRecommend
        {
            get { return this.GetBool(GoodsContentAttribute.IsRecommend, false); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.IsRecommend, value.ToString()); }
        }

        public bool IsNew
        {
            get { return this.GetBool(GoodsContentAttribute.IsNew, false); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.IsNew, value.ToString()); }
        }

        public bool IsHot
        {
            get { return this.GetBool(GoodsContentAttribute.IsHot, false); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.IsHot, value.ToString()); }
        }

        public bool IsOnSale
        {
            get { return this.GetBool(GoodsContentAttribute.IsOnSale, false); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.IsOnSale, value.ToString()); }
        }

        public string F1
        {
            get { return this.GetString(GoodsContentAttribute.F1, string.Empty); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.F1, value); }
        }

        public string F2
        {
            get { return this.GetString(GoodsContentAttribute.F2, string.Empty); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.F2, value); }
        }

        public string F3
        {
            get { return this.GetString(GoodsContentAttribute.F3, string.Empty); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.F3, value); }
        }

        public string F4
        {
            get { return this.GetString(GoodsContentAttribute.F4, string.Empty); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.F4, value); }
        }

        public string F5
        {
            get { return this.GetString(GoodsContentAttribute.F5, string.Empty); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.F5, value); }
        }

        public string F6
        {
            get { return this.GetString(GoodsContentAttribute.F6, string.Empty); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.F6, value); }
        }

        public string F7
        {
            get { return this.GetString(GoodsContentAttribute.F7, string.Empty); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.F7, value); }
        }

        public string F8
        {
            get { return this.GetString(GoodsContentAttribute.F8, string.Empty); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.F8, value); }
        }

        public string F9
        {
            get { return this.GetString(GoodsContentAttribute.F9, string.Empty); }
            set { base.SetExtendedAttribute(GoodsContentAttribute.F9, value); }
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return GoodsContentAttribute.AllAttributes;
        }
	}
}
