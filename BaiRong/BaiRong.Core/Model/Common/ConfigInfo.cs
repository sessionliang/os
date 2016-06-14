using System;
using BaiRong.Model;
using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core;

namespace BaiRong.Model
{
	public class ConfigInfo
	{
        private bool isInitialized;
		private string databaseVersion;
        private StringCollection restrictionBlackList;
        private StringCollection restrictionWhiteList;
        private bool isRelatedUrl;
        private string rootUrl;
        private DateTime updateDate;
        private string settingsXML;

        public ConfigInfo()
		{
            this.isInitialized = false;
            this.databaseVersion = string.Empty;
            this.restrictionBlackList = new StringCollection();
            this.restrictionWhiteList = new StringCollection();
            this.isRelatedUrl = true;
            this.rootUrl = string.Empty;
            this.updateDate = DateTime.Now;
            this.settingsXML = string.Empty;
		}

        public ConfigInfo(bool isInitialized, string databaseVersion, StringCollection restrictionBlackList, StringCollection restrictionWhiteList, bool isRelatedUrl, string rootUrl, DateTime updateDate, string settingsXML) 
		{
            this.isInitialized = isInitialized;
            this.databaseVersion = databaseVersion;
            this.restrictionBlackList = restrictionBlackList;
            this.restrictionWhiteList = restrictionWhiteList;
            this.isRelatedUrl = isRelatedUrl;
            this.rootUrl = rootUrl;
            this.updateDate = updateDate;
            this.settingsXML = settingsXML;
		}

        public bool IsInitialized
		{
            get { return isInitialized; }
            set { isInitialized = value; }
		}

		public string DatabaseVersion
		{
			get{ return databaseVersion; }
			set{ databaseVersion = value; }
		}

        public StringCollection RestrictionBlackList
        {
            get { return restrictionBlackList; }
            set { restrictionBlackList = value; }
        }

        public StringCollection RestrictionWhiteList
        {
            get { return restrictionWhiteList; }
            set { restrictionWhiteList = value; }
        }

        public bool IsRelatedUrl
        {
            get { return isRelatedUrl; }
            set { isRelatedUrl = value; }
        }

        public string RootUrl
        {
            get { return rootUrl; }
            set { rootUrl = value; }
        }

        public DateTime UpdateDate
        {
            get { return updateDate; }
            set { updateDate = value; }
        }

        public string SettingsXML
        {
            get { return settingsXML; }
            set { settingsXML = value; }
        }

        ConfigInfoExtend additional;
        public ConfigInfoExtend Additional
        {
            get
            {
                if (this.additional == null)
                {
                    this.additional = new ConfigInfoExtend(this.settingsXML);
                }
                return this.additional;
            }
        }
	}
}
