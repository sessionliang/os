using System;
using BaiRong.Model;

namespace SiteServer.CMS.Model
{
	[Serializable]
	public class SeoMetaInfo
	{
		private int seoMetaID;
		private int publishmentSystemID;
		private string seoMetaName;
        private bool isDefault;
		private string pageTitle;
        private string keywords;
		private string description;
        private string copyright;
		private string author;
		private string email;
		private string language;
		private string charset;
		private string distribution;
		private string rating;
		private string robots;
		private string revisitAfter;
		private string expires;

		public SeoMetaInfo()
		{
			this.seoMetaID = 0;
			this.publishmentSystemID = 0;
			this.seoMetaName = string.Empty;
			this.isDefault = false;
			this.pageTitle = string.Empty;
            this.keywords = string.Empty;
			this.description = string.Empty;
            this.copyright = string.Empty;
			this.author = string.Empty;
			this.email = string.Empty;
			this.language = string.Empty;
			this.charset = string.Empty;
			this.distribution = string.Empty;
			this.rating = string.Empty;
			this.robots = string.Empty;
			this.revisitAfter = string.Empty;
			this.expires = string.Empty;
		}

        public SeoMetaInfo(int seoMetaID, int publishmentSystemID, string seoMetaName, bool isDefault, string pageTitle, string keywords, string description, string copyright, string author, string email, string language, string charset, string distribution, string rating, string robots, string revisitAfter, string expires)
		{
			this.seoMetaID = seoMetaID;
			this.publishmentSystemID = publishmentSystemID;
			this.seoMetaName = seoMetaName;
			this.isDefault = isDefault;
            this.pageTitle = pageTitle;
            this.keywords = keywords;
			this.description = description;
            this.copyright = copyright;
			this.author = author;
			this.email = email;
			this.language = language;
			this.charset = charset;
			this.distribution = distribution;
			this.rating = rating;
			this.robots = robots;
			this.revisitAfter = revisitAfter;
			this.expires = expires;
		}

		public int SeoMetaID
		{
			get{ return seoMetaID; }
			set{ seoMetaID = value; }
		}

		public int PublishmentSystemID
		{
			get{ return publishmentSystemID; }
			set{ publishmentSystemID = value; }
		}

		public string SeoMetaName
		{
			get{ return seoMetaName; }
			set{ seoMetaName = value; }
		}

        public bool IsDefault
		{
			get{ return isDefault; }
			set{ isDefault = value; }
		}

        public string PageTitle
		{
            get { return pageTitle; }
            set { pageTitle = value; }
		}

        public string Keywords
        {
            get { return keywords; }
            set { keywords = value; }
        }

		public string Description
		{
			get{ return description; }
			set{ description = value; }
		}

        public string Copyright
        {
            get { return copyright; }
            set { copyright = value; }
        }

		public string Author
		{
			get { return author; }
			set { author = value; }
		}

		public string Email
		{
			get{ return email; }
			set{ email = value; }
		}

		public string Language
		{
			get{ return language; }
			set{ language = value; }
		}

		public string Charset
		{
			get{ return charset; }
			set{ charset = value; }
		}

		public string Distribution
		{
			get{ return distribution; }
			set{ distribution = value; }
		}

		public string Rating
		{
			get{ return rating; }
			set{ rating = value; }
		}

		public string Robots
		{
			get{ return robots; }
			set{ robots = value; }
		}

		public string RevisitAfter
		{
			get{ return revisitAfter; }
			set{ revisitAfter = value; }
		}

		public string Expires
		{
			get{ return expires; }
			set{ expires = value; }
		}
	}
}
