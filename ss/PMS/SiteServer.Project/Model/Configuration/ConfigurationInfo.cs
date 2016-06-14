using System;
using BaiRong.Model;
using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core;

namespace SiteServer.Project.Model
{
	public class ConfigurationInfo
	{
        private string settingsXML;

        public ConfigurationInfo()
		{
            this.settingsXML = string.Empty;
		}

        public ConfigurationInfo(string settingsXML) 
		{
            this.settingsXML = settingsXML;
		}

        public string SettingsXML
        {
            get { return settingsXML; }
            set { settingsXML = value; }
        }

        ConfigurationInfoExtend additional;
        public ConfigurationInfoExtend Additional
        {
            get
            {
                if (this.additional == null)
                {
                    this.additional = new ConfigurationInfoExtend(this.settingsXML);
                }
                return this.additional;
            }
        }
	}
}
