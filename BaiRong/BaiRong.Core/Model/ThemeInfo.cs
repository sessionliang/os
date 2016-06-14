using System;
using System.Xml.Serialization;

namespace BaiRong.Model
{
	[Serializable]
	[XmlRoot("Theme")]
	public class ThemeInfo
	{
		private string themeName;
		private string description;

		public ThemeInfo()
		{
            this.themeName = string.Empty;
			this.description = string.Empty;
		}

        public ThemeInfo(string themeName, string description) 
		{
            this.themeName = themeName;
			this.description = description;
		}

        [XmlElement(ElementName = "ThemeName")]
        public string ThemeName
		{
            get { return themeName; }
            set { themeName = value; }
		}

		[XmlElement(ElementName = "Description")]
		public string Description
		{
			get { return description; }
			set { description = value; }
		}

	}
}
