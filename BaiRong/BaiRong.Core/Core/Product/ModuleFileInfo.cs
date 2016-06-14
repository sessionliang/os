using System;
using System.Xml.Serialization;

namespace BaiRong.Model
{
	[Serializable]
	[XmlRoot("Module")]
	public class ModuleFileInfo
	{
        private string moduleID;
        private string moduleName;
		private string fullName;
        private string database;
		private string productUrl;
        private string email;
        private string developer;
		private string logoName;
		private string description;

		public ModuleFileInfo()
		{
            this.moduleID = string.Empty;
            this.moduleName = string.Empty;
            this.fullName = string.Empty;
            this.database = string.Empty;
            this.productUrl = string.Empty;
            this.email = string.Empty;
            this.developer = string.Empty;
            this.logoName = string.Empty;
			this.description = string.Empty;
		}

        public ModuleFileInfo(string moduleID, string moduleName, string fullName, string database, string productUrl, string email, string developer, string logoName, string description) 
		{
            this.moduleID = moduleID;
            this.moduleName = moduleName;
            this.fullName = fullName;
            this.database = database;
            this.productUrl = productUrl;
            this.email = email;
            this.developer = developer;
            this.logoName = logoName;
            this.description = description;
		}

        public string ModuleID
        {
            get { return moduleID; }
            set { moduleID = value; }
        }

        [XmlElement(ElementName = "ModuleName")]
        public string ModuleName
		{
            get { return moduleName; }
            set { moduleName = value; }
		}

        [XmlElement(ElementName = "FullName")]
        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        [XmlElement(ElementName = "Database")]
        public string Database
        {
            get { return database; }
            set { database = value; }
        }

        [XmlElement(ElementName = "ProductUrl")]
        public string ProductUrl
		{
            get { return productUrl; }
            set { productUrl = value; }
		}

        [XmlElement(ElementName = "Email")]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        [XmlElement(ElementName = "Developer")]
        public string Developer
        {
            get { return developer; }
            set { developer = value; }
        }

        [XmlElement(ElementName = "LogoName")]
        public string LogoName
		{
            get { return logoName; }
            set { logoName = value; }
		}

		[XmlElement(ElementName = "Description")]
		public string Description
		{
			get { return description; }
			set { description = value; }
		}
	}
}
