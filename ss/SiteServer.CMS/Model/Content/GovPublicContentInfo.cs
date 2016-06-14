using System;
using BaiRong.Model;
using System.Collections;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
	public class GovPublicContentInfo : ContentInfo
	{
		public GovPublicContentInfo() : base()
		{
            this.Identifier = string.Empty;
            this.Description = string.Empty;
            this.PublishDate = DateTime.Now;
            this.EffectDate = DateTime.Now;
            this.IsAbolition = false;
            this.AbolitionDate = DateTime.Now;
            this.DocumentNo = string.Empty;
            this.Publisher = string.Empty;
            this.Keywords = string.Empty;
            this.ImageUrl = string.Empty;
            this.FileUrl = string.Empty;
            this.IsRecommend = false;
            this.IsHot = false;
            this.IsColor = false;
            this.Content = string.Empty;
            this.DepartmentID = 0;
            this.Category1ID = 0;
            this.Category2ID = 0;
            this.Category3ID = 0;
            this.Category4ID = 0;
            this.Category5ID = 0;
            this.Category6ID = 0;
		}

        public GovPublicContentInfo(object dataItem)
            : base(dataItem)
		{
		}

        public string Identifier
		{
            get { return this.GetExtendedAttribute(GovPublicContentAttribute.Identifier); }
            set { base.SetExtendedAttribute(GovPublicContentAttribute.Identifier, value); }
		}

        public string Description
		{
            get { return this.GetExtendedAttribute(GovPublicContentAttribute.Description); }
            set { base.SetExtendedAttribute(GovPublicContentAttribute.Description, value); }
		}

        public DateTime PublishDate
        {
            get { return base.GetDateTime(GovPublicContentAttribute.PublishDate, DateTime.Now); }
            set { base.SetExtendedAttribute(GovPublicContentAttribute.PublishDate, value.ToString()); }
        }

        public DateTime EffectDate
        {
            get { return base.GetDateTime(GovPublicContentAttribute.EffectDate, DateTime.Now); }
            set { base.SetExtendedAttribute(GovPublicContentAttribute.EffectDate, value.ToString()); }
        }

        public bool IsAbolition
        {
            get { return base.GetBool(GovPublicContentAttribute.IsAbolition, false); }
            set { base.SetExtendedAttribute(GovPublicContentAttribute.IsAbolition, value.ToString()); }
        }

        public DateTime AbolitionDate
        {
            get { return base.GetDateTime(GovPublicContentAttribute.AbolitionDate, DateTime.Now); }
            set { base.SetExtendedAttribute(GovPublicContentAttribute.AbolitionDate, value.ToString()); }
        }

        public string DocumentNo
		{
            get { return this.GetExtendedAttribute(GovPublicContentAttribute.DocumentNo); }
            set { base.SetExtendedAttribute(GovPublicContentAttribute.DocumentNo, value); }
		}

        public string Publisher
        {
            get { return this.GetExtendedAttribute(GovPublicContentAttribute.Publisher); }
            set { base.SetExtendedAttribute(GovPublicContentAttribute.Publisher, value); }
        }

        public string Keywords
        {
            get { return this.GetExtendedAttribute(GovPublicContentAttribute.Keywords); }
            set { base.SetExtendedAttribute(GovPublicContentAttribute.Keywords, value); }
        }

        public string ImageUrl
        {
            get { return this.GetExtendedAttribute(GovPublicContentAttribute.ImageUrl); }
            set { base.SetExtendedAttribute(GovPublicContentAttribute.ImageUrl, value); }
        }

        public string FileUrl
        {
            get { return this.GetExtendedAttribute(GovPublicContentAttribute.FileUrl); }
            set { base.SetExtendedAttribute(GovPublicContentAttribute.FileUrl, value); }
        }

        public bool IsRecommend
        {
            get { return base.GetBool(GovPublicContentAttribute.IsRecommend, false); }
            set { base.SetExtendedAttribute(GovPublicContentAttribute.IsRecommend, value.ToString()); }
        }

        public bool IsHot
        {
            get { return base.GetBool(GovPublicContentAttribute.IsHot, false); }
            set { base.SetExtendedAttribute(GovPublicContentAttribute.IsHot, value.ToString()); }
        }

        public bool IsColor
        {
            get { return base.GetBool(GovPublicContentAttribute.IsColor, false); }
            set { base.SetExtendedAttribute(GovPublicContentAttribute.IsColor, value.ToString()); }
        }

        public int DepartmentID
        {
            get { return base.GetInt(GovPublicContentAttribute.DepartmentID, 0); }
            set { base.SetExtendedAttribute(GovPublicContentAttribute.DepartmentID, value.ToString()); }
        }

        public int Category1ID
        {
            get { return base.GetInt(GovPublicContentAttribute.Category1ID, 0); }
            set { base.SetExtendedAttribute(GovPublicContentAttribute.Category1ID, value.ToString()); }
        }

        public int Category2ID
        {
            get { return base.GetInt(GovPublicContentAttribute.Category2ID, 0); }
            set { base.SetExtendedAttribute(GovPublicContentAttribute.Category2ID, value.ToString()); }
        }

        public int Category3ID
        {
            get { return base.GetInt(GovPublicContentAttribute.Category3ID, 0); }
            set { base.SetExtendedAttribute(GovPublicContentAttribute.Category3ID, value.ToString()); }
        }

        public int Category4ID
        {
            get { return base.GetInt(GovPublicContentAttribute.Category4ID, 0); }
            set { base.SetExtendedAttribute(GovPublicContentAttribute.Category4ID, value.ToString()); }
        }

        public int Category5ID
        {
            get { return base.GetInt(GovPublicContentAttribute.Category5ID, 0); }
            set { base.SetExtendedAttribute(GovPublicContentAttribute.Category5ID, value.ToString()); }
        }

        public int Category6ID
        {
            get { return base.GetInt(GovPublicContentAttribute.Category6ID, 0); }
            set { base.SetExtendedAttribute(GovPublicContentAttribute.Category6ID, value.ToString()); }
        }

        public string Content
        {
            get { return this.GetExtendedAttribute(GovPublicContentAttribute.Content); }
            set { base.SetExtendedAttribute(GovPublicContentAttribute.Content, value); }
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return GovPublicContentAttribute.AllAttributes;
        }
	}
}
