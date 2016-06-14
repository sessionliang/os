using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SiteServer.BBS.Model
{
    [Serializable]
    [XmlRoot("Template")]
    public class TemplateInfo
    {
        private string templateName;
        private string webSiteUrl;
        private string picFileName;
        private string description;

        public TemplateInfo()
        {
            this.templateName = string.Empty;
            this.webSiteUrl = string.Empty;
            this.picFileName = string.Empty;
            this.description = string.Empty;
        }

        public TemplateInfo(string templateName, string webSiteUrl, string picFileName, string description)
        {
            this.templateName = templateName;
            this.webSiteUrl = webSiteUrl;
            this.picFileName = picFileName;
            this.description = description;
        }

        [XmlElement(ElementName = "TemplateName")]
        public string TemplateName
        {
            get { return templateName; }
            set { templateName = value; }
        }

        [XmlElement(ElementName = "WebSiteUrl")]
        public string WebSiteUrl
        {
            get { return webSiteUrl; }
            set { webSiteUrl = value; }
        }

        [XmlElement(ElementName = "PicFileName")]
        public string PicFileName
        {
            get { return picFileName; }
            set { picFileName = value; }
        }

        [XmlElement(ElementName = "Description")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

    }
}