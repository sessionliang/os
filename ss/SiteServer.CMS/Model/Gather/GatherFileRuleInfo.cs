using System;
using BaiRong.Model;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
	public class GatherFileRuleInfo
	{
		private string gatherRuleName;
		private int publishmentSystemID;
		private string gatherUrl;
        private ECharset charset;
        private DateTime lastGatherDate;
        private bool isToFile;

        private string filePath;
        private bool isSaveRelatedFiles;
        private bool isRemoveScripts;
        private string styleDirectoryPath;
        private string scriptDirectoryPath;
        private string imageDirectoryPath;

        private int nodeID;
        private bool isSaveImage;
        private bool isChecked;
        private bool isAutoCreate;
        private string contentExclude;      
		private string contentHtmlClearCollection;
        private string contentHtmlClearTagCollection;
		private string contentTitleStart;
		private string contentTitleEnd;
		private string contentContentStart;
		private string contentContentEnd;
        private string contentAttributes;
        private string contentAttributesXML;

		public GatherFileRuleInfo()
		{
			this.gatherRuleName = string.Empty;
			this.publishmentSystemID = 0;
			this.gatherUrl = string.Empty;
            this.charset = ECharset.utf_8;
            this.lastGatherDate = DateTime.Now;
            this.isToFile = true;

            this.filePath = string.Empty;
            this.isSaveRelatedFiles = false;
            this.isRemoveScripts = false;
            this.styleDirectoryPath = string.Empty;
            this.scriptDirectoryPath = string.Empty;
            this.imageDirectoryPath = string.Empty;

            this.nodeID = 0;
            this.isSaveImage = true;
            this.isChecked = true;
            this.isAutoCreate = false;
            this.contentExclude = string.Empty;
			this.contentHtmlClearCollection = string.Empty;
            this.contentHtmlClearTagCollection = string.Empty;
			this.contentTitleStart = string.Empty;
			this.contentTitleEnd = string.Empty;
			this.contentContentStart = string.Empty;
			this.contentContentEnd = string.Empty;
            this.contentAttributes = string.Empty;
            this.contentAttributesXML = string.Empty;
		}

        public GatherFileRuleInfo(string gatherRuleName, int publishmentSystemID, string gatherUrl, ECharset charset, DateTime lastGatherDate, bool isToFile, string filePath, bool isSaveRelatedFiles, bool isRemoveScripts, string styleDirectoryPath, string scriptDirectoryPath, string imageDirectoryPath, int nodeID, bool isSaveImage, bool isChecked, string contentExclude, string contentHtmlClearCollection, string contentHtmlClearTagCollection, string contentTitleStart, string contentTitleEnd, string contentContentStart, string contentContentEnd, string contentAttributes, string contentAttributesXML, bool isAutoCreate)
		{
            this.gatherRuleName = gatherRuleName;
            this.publishmentSystemID = publishmentSystemID;
            this.gatherUrl = gatherUrl;
            this.charset = charset;
            this.lastGatherDate = lastGatherDate;
            this.isToFile = isToFile;

            this.filePath = filePath;
            this.isSaveRelatedFiles = isSaveRelatedFiles;
            this.isRemoveScripts = isRemoveScripts;
            this.styleDirectoryPath = styleDirectoryPath;
            this.scriptDirectoryPath = scriptDirectoryPath;
            this.imageDirectoryPath = imageDirectoryPath;

            this.nodeID = nodeID;
            this.isSaveImage = isSaveImage;
            this.isChecked = isChecked;
            this.isAutoCreate = isAutoCreate;
            this.contentExclude = contentExclude;
            this.contentHtmlClearCollection = contentHtmlClearCollection;
            this.contentHtmlClearTagCollection = contentHtmlClearTagCollection;
            this.contentTitleStart = contentTitleStart;
            this.contentTitleEnd = contentTitleEnd;
            this.contentContentStart = contentContentStart;
            this.contentContentEnd = contentContentEnd;
            this.contentAttributes = contentAttributes;
            this.contentAttributesXML = contentAttributesXML;
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

        public string GatherUrl
		{
            get { return gatherUrl; }
            set { gatherUrl = value; }
		}

        public ECharset Charset
        {
            get { return charset; }
            set { charset = value; }
        }

        public DateTime LastGatherDate
        {
            get { return lastGatherDate; }
            set { lastGatherDate = value; }
        }

        public bool IsToFile
        {
            get { return isToFile; }
            set { isToFile = value; }
        }

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        public bool IsSaveRelatedFiles
        {
            get { return isSaveRelatedFiles; }
            set { isSaveRelatedFiles = value; }
        }

        public bool IsRemoveScripts
        {
            get { return isRemoveScripts; }
            set { isRemoveScripts = value; }
        }

        public string StyleDirectoryPath
        {
            get { return styleDirectoryPath; }
            set { styleDirectoryPath = value; }
        }

        public string ScriptDirectoryPath
        {
            get { return scriptDirectoryPath; }
            set { scriptDirectoryPath = value; }
        }

        public string ImageDirectoryPath
        {
            get { return imageDirectoryPath; }
            set { imageDirectoryPath = value; }
        }

        public int NodeID
        {
            get { return nodeID; }
            set { nodeID = value; }
        }

        public bool IsSaveImage
        {
            get { return isSaveImage; }
            set { isSaveImage = value; }
        }

        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }

        public bool IsAutoCreate
        {
            get { return isAutoCreate; }
            set { isAutoCreate = value; }
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
	}
}
