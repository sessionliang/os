using System;
using System.Collections.Generic;
using System.Text;

namespace SiteServer.BBS.Model
{
    [Serializable]
    public class ConfigurationInfo
    {
        private int publishmentSystemID;
        private string settingsXML;

        public ConfigurationInfo()
        {
            this.publishmentSystemID = 0;
            this.settingsXML = string.Empty;
        }

        public ConfigurationInfo(int publishmentSystemID, string settingsXML)
        {
            this.publishmentSystemID = publishmentSystemID;
            this.settingsXML = settingsXML;
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
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