using System;
using BaiRong.Model;
using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core;

namespace SiteServer.B2C.Model
{
	public class B2CConfigurationInfo
	{
        private int nodeID;
        private bool isVirtualGoods;
        private string settingsXML;

        public B2CConfigurationInfo(int nodeID, bool isVirtualGoods, string settingsXML) 
		{
            this.nodeID = nodeID;
            this.IsVirtualGoods = isVirtualGoods;
            this.settingsXML = settingsXML;
		}

        public int NodeID
        {
            get { return nodeID; }
            set { nodeID = value; }
        }

        public bool IsVirtualGoods
        {
            get { return isVirtualGoods; }
            set { isVirtualGoods = value; }
        }

        public string SettingsXML
        {
            get { return settingsXML; }
            set { settingsXML = value; }
        }

        B2CConfigurationInfoExtend additional;
        public B2CConfigurationInfoExtend Additional
        {
            get
            {
                if (this.additional == null)
                {
                    this.additional = new B2CConfigurationInfoExtend(this.settingsXML);
                }
                return this.additional;
            }
        }
	}
}
