using System;
using BaiRong.Model;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
	[Serializable]
	public class TemplateInfo
	{
		private int templateID;
		private int publishmentSystemID;
		private string templateName;
		private ETemplateType templateType;
		private string relatedFileName;
        private string createdFileFullName;
        private string createdFileExtName;
		private ECharset charset;
		private bool isDefault;

		public TemplateInfo()
		{
			this.templateID = 0;
			this.publishmentSystemID = 0;
			this.templateName = string.Empty;
			this.templateType = ETemplateType.ContentTemplate;
			this.relatedFileName = string.Empty;
            this.createdFileFullName = string.Empty;
            this.createdFileExtName = string.Empty;
			this.charset = ECharset.utf_8;
			this.isDefault = false;
		}

        public TemplateInfo(int templateID, int publishmentSystemID, string templateName, ETemplateType templateType, string relatedFileName, string createdFileFullName, string createdFileExtName, ECharset charset, bool isDefault) 
		{
			this.templateID = templateID;
			this.publishmentSystemID = publishmentSystemID;
			this.templateName = templateName;
			this.templateType = templateType;
            this.relatedFileName = relatedFileName;
            this.createdFileFullName = createdFileFullName;
            this.createdFileExtName = createdFileExtName;
			this.charset = charset;
			this.isDefault = isDefault;
		}

		public int TemplateID
		{
			get{ return templateID; }
			set{ templateID = value; }
		}

		public int PublishmentSystemID
		{
			get{ return publishmentSystemID; }
			set{ publishmentSystemID = value; }
		}

		public string TemplateName
		{
			get{ return templateName; }
			set{ templateName = value; }
		}

		public ETemplateType TemplateType
		{
			get{ return templateType; }
			set{ templateType = value; }
		}

        public string RelatedFileName
		{
            get { return relatedFileName; }
            set { relatedFileName = value; }
		}

        public string CreatedFileFullName
        {
            get { return createdFileFullName; }
            set { createdFileFullName = value; }
        }

        public string CreatedFileExtName
        {
            get { return createdFileExtName; }
            set { createdFileExtName = value; }
        }

		public ECharset Charset
		{
			get { return charset; }
			set { charset = value; }
		}

		public bool IsDefault
		{
			get{ return isDefault; }
			set{ isDefault = value; }
		}

        //ÐéÄâ×Ö¶Î
        private string content;
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
	}
}
