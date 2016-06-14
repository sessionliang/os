using System;
using System.Collections.Generic;
using BaiRong.Model;

namespace SiteServer.B2C.Model
{
	public class SpecInfo
	{
		private int specID;
		private int publishmentSystemID;
        private int channelID;
		private string specName;
		private bool isIcon;
        private bool isMultiple;
        private bool isRequired;
        private string description;
        private int taxis;

		public SpecInfo()
		{
            this.specID = 0;
			this.publishmentSystemID = 0;
            this.channelID = 0;
            this.specName = string.Empty;
            this.isIcon = false;
            this.isMultiple = false;
            this.isRequired = true;
            this.description = string.Empty;
            this.taxis = 0;
		}

        public SpecInfo(int specID, int publishmentSystemID, int channelID, string specName, bool isIcon, bool isMultiple, bool isRequired, string description, int taxis)
		{
            this.specID = specID;
            this.publishmentSystemID = publishmentSystemID;
            this.channelID = channelID;
            this.specName = specName;
            this.isIcon = isIcon;
            this.isMultiple = isMultiple;
            this.isRequired = isRequired;
            this.description = description;
            this.taxis = taxis;
		}

        public int SpecID
		{
            get { return specID; }
            set { specID = value; }
		}

		public int PublishmentSystemID
		{
			get{ return publishmentSystemID; }
			set{ publishmentSystemID = value; }
		}

        public int ChannelID
        {
            get { return channelID; }
            set { channelID = value; }
        }

        public string SpecName
		{
            get { return specName; }
            set { specName = value; }
		}

        public bool IsIcon
		{
            get { return isIcon; }
            set { isIcon = value; }
		}

        public bool IsMultiple
        {
            get { return isMultiple; }
            set { isMultiple = value; }
        }

        public bool IsRequired
        {
            get { return isRequired; }
            set { isRequired = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public int Taxis
        {
            get { return taxis; }
            set { taxis = value; }
        }
	}
}
