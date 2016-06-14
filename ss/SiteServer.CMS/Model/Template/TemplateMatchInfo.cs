using System;
using BaiRong.Model;
using BaiRong.Core;
using System.Collections.Specialized;

namespace SiteServer.CMS.Model
{
	public class TemplateMatchInfo
	{
        private int nodeID;
		private int publishmentSystemID;
		private int channelTemplateID;
        private int contentTemplateID;
        private string filePath;
        private string channelFilePathRule;
        private string contentFilePathRule;

		public TemplateMatchInfo()
		{
            this.nodeID = 0;
			this.publishmentSystemID = 0;
			this.channelTemplateID = 0;
			this.contentTemplateID = 0;
            this.filePath = string.Empty;
            this.channelFilePathRule = string.Empty;
            this.contentFilePathRule = string.Empty;
		}

        public TemplateMatchInfo(int nodeID, int publishmentSystemID, int channelTemplateID, int contentTemplateID, string filePath, string channelFilePathRule, string contentFilePathRule) 
		{
            this.nodeID = nodeID;
            this.publishmentSystemID = publishmentSystemID;
            this.channelTemplateID = channelTemplateID;
            this.contentTemplateID = contentTemplateID;
            this.filePath = filePath;
            this.channelFilePathRule = channelFilePathRule;
            this.contentFilePathRule = contentFilePathRule;
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

        public int ChannelTemplateID
		{
            get { return channelTemplateID; }
            set { channelTemplateID = value; }
		}

        public int ContentTemplateID
		{
            get { return contentTemplateID; }
            set { contentTemplateID = value; }
		}

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        public string ChannelFilePathRule
        {
            get { return channelFilePathRule; }
            set { channelFilePathRule = value; }
        }

        public string ContentFilePathRule
        {
            get { return contentFilePathRule; }
            set { contentFilePathRule = value; }
        }
	}
}
