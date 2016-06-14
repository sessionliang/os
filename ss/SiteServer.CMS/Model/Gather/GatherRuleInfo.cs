using System;
using BaiRong.Model;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
	public class GatherRuleInfo
	{
		private string gatherRuleName;
		private int publishmentSystemID;
		private string cookieString;
		private bool gatherUrlIsCollection;
		private string gatherUrlCollection;
		private bool gatherUrlIsSerialize;
		private string gatherUrlSerialize;
		private int serializeFrom;
		private int serializeTo;
		private int serializeInterval;
		private bool serializeIsOrderByDesc;
		private bool serializeIsAddZero;
		private int nodeID;
		private ECharset charset;
		private string urlInclude;
		private string titleInclude;
		private string contentExclude;
		private string contentHtmlClearCollection;
        private string contentHtmlClearTagCollection;
		private DateTime lastGatherDate;
		private string listAreaStart;
		private string listAreaEnd;
		private string contentChannelStart;
		private string contentChannelEnd;
		private string contentTitleStart;
		private string contentTitleEnd;
		private string contentContentStart;
		private string contentContentEnd;
		private string contentNextPageStart;
		private string contentNextPageEnd;
        private string contentAttributes;
        private string contentAttributesXML;
        private string extendValues;

		public GatherRuleInfo()
		{
			this.gatherRuleName = string.Empty;
			this.publishmentSystemID = 0;
			this.cookieString = string.Empty;
			this.gatherUrlIsCollection = true;
			this.gatherUrlCollection = string.Empty;
			this.gatherUrlIsSerialize = false;
			this.gatherUrlSerialize = string.Empty;
			this.serializeFrom = 0;
			this.serializeTo = 0;
			this.serializeInterval = 0;
			this.serializeIsOrderByDesc = true;
			this.serializeIsAddZero = false;
			this.nodeID = 0;
			this.charset = ECharset.utf_8;
			this.urlInclude = string.Empty;
			this.titleInclude = string.Empty;
			this.contentExclude = string.Empty;
			this.contentHtmlClearCollection = string.Empty;
            this.contentHtmlClearTagCollection = string.Empty;
			this.lastGatherDate = DateTime.Now;
			this.listAreaStart = string.Empty;
			this.listAreaEnd = string.Empty;
			this.contentChannelStart = string.Empty;
			this.contentChannelEnd = string.Empty;
			this.contentTitleStart = string.Empty;
			this.contentTitleEnd = string.Empty;
			this.contentContentStart = string.Empty;
			this.contentContentEnd = string.Empty;
			this.contentNextPageStart = string.Empty;
			this.contentNextPageEnd = string.Empty;
            this.contentAttributes = string.Empty;
            this.contentAttributesXML = string.Empty;
            this.extendValues = string.Empty;
		}

        public GatherRuleInfo(string gatherRuleName, int publishmentSystemID, string cookieString, bool gatherUrlIsCollection, string gatherUrlCollection, bool gatherUrlIsSerialize, string gatherUrlSerialize, int serializeFrom, int serializeTo, int serializeInterval, bool serializeIsOrderByDesc, bool serializeIsAddZero, int nodeID, ECharset charset, string urlInclude, string titleInclude, string contentExclude, string contentHtmlClearCollection, string contentHtmlClearTagCollection, DateTime lastGatherDate, string listAreaStart, string listAreaEnd, string contentChannelStart, string contentChannelEnd, string contentTitleStart, string contentTitleEnd, string contentContentStart, string contentContentEnd, string contentNextPageStart, string contentNextPageEnd, string contentAttributes, string contentAttributesXML, string extendValues) 
		{
			this.gatherRuleName = gatherRuleName;
			this.publishmentSystemID = publishmentSystemID;
			this.cookieString = cookieString;
			this.gatherUrlIsCollection = gatherUrlIsCollection;
			this.gatherUrlCollection = gatherUrlCollection;
			this.gatherUrlIsSerialize = gatherUrlIsSerialize;
			this.gatherUrlSerialize = gatherUrlSerialize;
			this.serializeFrom = serializeFrom;
			this.serializeTo = serializeTo;
			this.serializeInterval = serializeInterval;
			this.serializeIsOrderByDesc = serializeIsOrderByDesc;
			this.serializeIsAddZero = serializeIsAddZero;
			this.nodeID = nodeID;
			this.charset = charset;
			this.urlInclude = urlInclude;
			this.titleInclude = titleInclude;
			this.contentExclude = contentExclude;
			this.contentHtmlClearCollection = contentHtmlClearCollection;
            this.contentHtmlClearTagCollection = contentHtmlClearTagCollection;
			this.lastGatherDate = lastGatherDate;
			this.listAreaStart = listAreaStart;
			this.listAreaEnd = listAreaEnd;
			this.contentChannelStart = contentChannelStart;
			this.contentChannelEnd = contentChannelEnd;
			this.contentTitleStart = contentTitleStart;
			this.contentTitleEnd = contentTitleEnd;
			this.contentContentStart = contentContentStart;
			this.contentContentEnd = contentContentEnd;
			this.contentNextPageStart = contentNextPageStart;
			this.contentNextPageEnd = contentNextPageEnd;
            this.contentAttributes = contentAttributes;
            this.contentAttributesXML = contentAttributesXML;
            this.extendValues = extendValues;
		}

		public string GatherRuleName
		{
			get { return gatherRuleName; }
			set { gatherRuleName = value; }
		}

		public int PublishmentSystemID
		{
			get { return publishmentSystemID; }
			set { publishmentSystemID = value; }
		}

		public string CookieString
		{
			get { return cookieString; }
			set { cookieString = value; }
		}

		public bool GatherUrlIsCollection
		{
			get { return gatherUrlIsCollection; }
			set { gatherUrlIsCollection = value; }
		}

		public string GatherUrlCollection
		{
			get { return gatherUrlCollection; }
			set { gatherUrlCollection = value; }
		}

		public bool GatherUrlIsSerialize
		{
			get { return gatherUrlIsSerialize; }
			set { gatherUrlIsSerialize = value; }
		}

		public string GatherUrlSerialize
		{
			get { return gatherUrlSerialize; }
			set { gatherUrlSerialize = value; }
		}

		public int SerializeFrom
		{
			get { return serializeFrom; }
			set { serializeFrom = value; }
		}

		public int SerializeTo
		{
			get { return serializeTo; }
			set { serializeTo = value; }
		}

		public int SerializeInterval
		{
			get { return serializeInterval; }
			set { serializeInterval = value; }
		}

		public bool SerializeIsOrderByDesc
		{
			get { return serializeIsOrderByDesc; }
			set { serializeIsOrderByDesc = value; }
		}

		public bool SerializeIsAddZero
		{
			get { return serializeIsAddZero; }
			set { serializeIsAddZero = value; }
		}

		public int NodeID
		{
			get { return nodeID; }
			set { nodeID = value; }
		}

		public ECharset Charset
		{
			get { return charset; }
			set { charset = value; }
		}

		public string UrlInclude
		{
			get { return urlInclude; }
			set { urlInclude = value; }
		}

		public string TitleInclude
		{
			get { return titleInclude; }
			set { titleInclude = value; }
		}

		public string ContentExclude
		{
			get { return contentExclude; }
			set { contentExclude = value; }
		}

		public string ContentHtmlClearCollection
		{
			get { return contentHtmlClearCollection; }
			set { contentHtmlClearCollection = value; }
		}

        public string ContentHtmlClearTagCollection
        {
            get { return contentHtmlClearTagCollection; }
            set { contentHtmlClearTagCollection = value; }
        }

		public DateTime LastGatherDate
		{
			get { return lastGatherDate; }
			set { lastGatherDate = value; }
		}

		public string ListAreaStart
		{
			get { return listAreaStart; }
			set { listAreaStart = value; }
		}

		public string ListAreaEnd
		{
			get { return listAreaEnd; }
			set { listAreaEnd = value; }
		}

		public string ContentChannelStart
		{
			get { return contentChannelStart; }
			set { contentChannelStart = value; }
		}

		public string ContentChannelEnd
		{
			get { return contentChannelEnd; }
			set { contentChannelEnd = value; }
		}

		public string ContentTitleStart
		{
			get { return contentTitleStart; }
			set { contentTitleStart = value; }
		}

		public string ContentTitleEnd
		{
			get { return contentTitleEnd; }
			set { contentTitleEnd = value; }
		}

		public string ContentContentStart
		{
			get { return contentContentStart; }
			set { contentContentStart = value; }
		}

		public string ContentContentEnd
		{
			get { return contentContentEnd; }
			set { contentContentEnd = value; }
		}

		public string ContentNextPageStart
		{
			get { return contentNextPageStart; }
			set { contentNextPageStart = value; }
		}

		public string ContentNextPageEnd
		{
			get { return contentNextPageEnd; }
			set { contentNextPageEnd = value; }
		}

        public string ContentAttributes
        {
            get { return contentAttributes; }
            set { contentAttributes = value; }
        }

        public string ContentAttributesXML
        {
            get { return contentAttributesXML; }
            set { contentAttributesXML = value; }
        }

        public string ExtendValues
        {
            get { return extendValues; }
            set { extendValues = value; }
        }

        GatherRuleInfoExtend additional;
        public GatherRuleInfoExtend Additional
        {
            get
            {
                if (this.additional == null)
                {
                    this.additional = new GatherRuleInfoExtend(this.extendValues);
                }
                return this.additional;
            }
        }
	}
}
