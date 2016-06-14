using System;
using BaiRong.Model;
using System.Collections;

namespace SiteServer.CMS.Model
{
	public class BackgroundContentInfo : ContentInfo
	{
		public BackgroundContentInfo() : base()
		{
            this.SubTitle = string.Empty;
            this.ImageUrl = string.Empty;
            this.VideoUrl = string.Empty;
            this.FileUrl = string.Empty;
            this.LinkUrl = string.Empty;
            this.Summary = string.Empty;
            this.Author = string.Empty;
            this.Source = string.Empty;
            this.IsRecommend = false;
            this.IsHot = false;
            this.IsColor = false;
            this.IsTop = false;
            this.Content = string.Empty;          
		}

        public BackgroundContentInfo(object dataItem)
            : base(dataItem)
		{
		}

        public string SubTitle
		{
            get { return this.GetExtendedAttribute(BackgroundContentAttribute.SubTitle); }
            set { base.SetExtendedAttribute(BackgroundContentAttribute.SubTitle, value); }
		}

        public string ImageUrl
		{
            get { return this.GetExtendedAttribute(BackgroundContentAttribute.ImageUrl); }
            set { base.SetExtendedAttribute(BackgroundContentAttribute.ImageUrl, value); }
		}

        public string VideoUrl
        {
            get { return this.GetExtendedAttribute(BackgroundContentAttribute.VideoUrl); }
            set { base.SetExtendedAttribute(BackgroundContentAttribute.VideoUrl, value); }
        }

        public string FileUrl
        {
            get { return this.GetExtendedAttribute(BackgroundContentAttribute.FileUrl); }
            set { base.SetExtendedAttribute(BackgroundContentAttribute.FileUrl, value); }
        }

        public string LinkUrl
		{
            get { return this.GetExtendedAttribute(BackgroundContentAttribute.LinkUrl); }
            set { base.SetExtendedAttribute(BackgroundContentAttribute.LinkUrl, value); }
		}

        public string Summary
        {
            get { return this.GetExtendedAttribute(BackgroundContentAttribute.Summary); }
            set { base.SetExtendedAttribute(BackgroundContentAttribute.Summary, value); }
        }

        public string Author
		{
            get { return this.GetExtendedAttribute(BackgroundContentAttribute.Author); }
            set { base.SetExtendedAttribute(BackgroundContentAttribute.Author, value); }
		}

        public string Source
		{
            get { return this.GetExtendedAttribute(BackgroundContentAttribute.Source); }
            set { base.SetExtendedAttribute(BackgroundContentAttribute.Source, value); }
		}

        public bool IsRecommend
		{
            get { return base.GetBool(BackgroundContentAttribute.IsRecommend, false); }
            set { base.SetExtendedAttribute(BackgroundContentAttribute.IsRecommend, value.ToString()); }
		}

        public bool IsHot
		{
            get { return base.GetBool(BackgroundContentAttribute.IsHot, false); }
            set { base.SetExtendedAttribute(BackgroundContentAttribute.IsHot, value.ToString()); }
		}

        public bool IsColor
		{
            get { return base.GetBool(BackgroundContentAttribute.IsColor, false); }
            set { base.SetExtendedAttribute(BackgroundContentAttribute.IsColor, value.ToString()); }
		}

        public string Content
		{
            get { return this.GetExtendedAttribute(BackgroundContentAttribute.Content); }
            set { base.SetExtendedAttribute(BackgroundContentAttribute.Content, value); }
		}

        protected override ArrayList GetDefaultAttributesNames()
        {
            return BackgroundContentAttribute.AllAttributes;
        }
	}
}
