using System;
using System.Xml.Serialization;

namespace SiteServer.CMS.Model
{
	[Serializable]
    [XmlRoot("IndependentTemplate")]
	public class IndependentTemplateInfo
	{
        private string independentTemplateName;
        private string publishmentSystemType;
        private string templateTypes;
		private string webSiteUrl;
		private string picFileNames;
		private string description;

		public IndependentTemplateInfo()
		{
            this.independentTemplateName = string.Empty;
            this.publishmentSystemType = string.Empty;
            this.templateTypes = string.Empty;
			this.webSiteUrl = string.Empty;
            this.picFileNames = string.Empty;
			this.description = string.Empty;
		}

        public IndependentTemplateInfo(string independentTemplateName, string publishmentSystemType, string templateTypes, string webSiteUrl, string picFileNames, string description) 
		{
            this.independentTemplateName = independentTemplateName;
            this.publishmentSystemType = publishmentSystemType;
            this.templateTypes = templateTypes;
			this.webSiteUrl = webSiteUrl;
            this.picFileNames = picFileNames;
			this.description = description;
		}

        [XmlElement(ElementName = "IndependentTemplateName")]
        public string IndependentTemplateName
        {
            get { return independentTemplateName; }
            set { independentTemplateName = value; }
        }

        [XmlElement(ElementName = "PublishmentSystemType")]
        public string PublishmentSystemType
        {
            get { return publishmentSystemType; }
            set { publishmentSystemType = value; }
        }

        [XmlElement(ElementName = "TemplateTypes")]
        public string TemplateTypes
        {
            get { return templateTypes; }
            set { templateTypes = value; }
        }

		[XmlElement(ElementName = "WebSiteUrl")]
		public string WebSiteUrl
		{
			get { return webSiteUrl; }
			set { webSiteUrl = value; }
		}

		[XmlElement(ElementName = "PicFileNames")]
		public string PicFileNames
		{
			get { return picFileNames; }
			set { picFileNames = value; }
		}

		[XmlElement(ElementName = "Description")]
		public string Description
		{
			get { return description; }
			set { description = value; }
		}

	}
}
