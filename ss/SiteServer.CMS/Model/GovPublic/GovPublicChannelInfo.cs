using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
	public class GovPublicChannelInfo
	{
		private int nodeID;
        private int publishmentSystemID;
        private string code;
		private string summary;

		public GovPublicChannelInfo()
		{
            this.nodeID = 0;
            this.publishmentSystemID = 0;
            this.code = string.Empty;
			this.summary = string.Empty;
		}

        public GovPublicChannelInfo(int nodeID, int publishmentSystemID, string code, string summary) 
		{
            this.nodeID = nodeID;
            this.publishmentSystemID = publishmentSystemID;
            this.code = code;
            this.summary = summary;
		}

        public int NodeID
		{
            get { return nodeID; }
            set { nodeID = value; }
		}

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public string Code
		{
            get { return code; }
            set { code = value; }
		}

        public string Summary
		{
            get { return summary; }
            set { summary = value; }
		}
	}
}
