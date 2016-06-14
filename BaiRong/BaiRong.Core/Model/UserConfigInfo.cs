using System;
using BaiRong.Model;
using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core;

namespace BaiRong.Model
{
	public class UserConfigInfo
	{
        private string settingsXML;

        public UserConfigInfo()
		{
            this.settingsXML = string.Empty;
		}

        public UserConfigInfo(string settingsXML) 
		{
            this.settingsXML = settingsXML;
		}

        public string SettingsXML
        {
            get { return settingsXML; }
            set { settingsXML = value; }
        }

        UserConfigInfoExtend additional;
        public UserConfigInfoExtend Additional
        {
            get
            {
                if (this.additional == null)
                {
                    this.additional = new UserConfigInfoExtend(this.settingsXML);
                }
                return this.additional;
            }
        }
	}
}
