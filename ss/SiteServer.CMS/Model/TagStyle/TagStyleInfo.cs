using System;
using System.Xml.Serialization;
using BaiRong.Model;

namespace SiteServer.CMS.Model
{
	public class TagStyleInfo
	{
        private int styleID;
        private string styleName;
        private string elementName;
		private int publishmentSystemID;
        private bool isTemplate;
        private string styleTemplate;
        private string scriptTemplate;
        private string contentTemplate;
        private string successTemplate;
        private string failureTemplate;
        private string settingsXML;

		public TagStyleInfo()
		{
            this.styleID = 0;
            this.styleName = string.Empty;
            this.elementName = string.Empty;
			this.publishmentSystemID = 0;
            this.isTemplate = false;
            this.styleTemplate = string.Empty;
            this.scriptTemplate = string.Empty;
            this.contentTemplate = string.Empty;
            this.successTemplate = string.Empty;
            this.failureTemplate = string.Empty;
            this.settingsXML = string.Empty;
		}

        public TagStyleInfo(int styleID, string styleName, string elementName, int publishmentSystemID, bool isTemplate, string styleTemplate, string scriptTemplate, string contentTemplate, string successTemplate, string failureTemplate, string settingsXML) 
		{
            this.styleID = styleID;
            this.styleName = styleName;
            this.elementName = elementName;
            this.publishmentSystemID = publishmentSystemID;
            this.isTemplate = isTemplate;
            this.styleTemplate = styleTemplate;
            this.scriptTemplate = scriptTemplate;
            this.contentTemplate = contentTemplate;
            this.successTemplate = successTemplate;
            this.failureTemplate = failureTemplate;
            this.settingsXML = settingsXML;
		}

        public int StyleID
        {
            get { return styleID; }
            set { styleID = value; }
        }

        public string StyleName
		{
            get { return styleName; }
            set { styleName = value; }
		}

        public string ElementName
        {
            get { return elementName; }
            set { elementName = value; }
        }

		public int PublishmentSystemID
		{
			get{ return publishmentSystemID; }
			set{ publishmentSystemID = value; }
		}

        public bool IsTemplate
        {
            get { return isTemplate; }
            set { isTemplate = value; }
        }

        public string StyleTemplate
        {
            get { return styleTemplate; }
            set { styleTemplate = value; }
        }

        public string ScriptTemplate
        {
            get { return scriptTemplate; }
            set { scriptTemplate = value; }
        }

        public string ContentTemplate
        {
            get { return contentTemplate; }
            set { contentTemplate = value; }
        }

        public string SuccessTemplate
        {
            get { return successTemplate; }
            set { successTemplate = value; }
        }

        public string FailureTemplate
        {
            get { return failureTemplate; }
            set { failureTemplate = value; }
        }

        public string SettingsXML
        {
            get { return settingsXML; }
            set { settingsXML = value; }
        }
	}
}
