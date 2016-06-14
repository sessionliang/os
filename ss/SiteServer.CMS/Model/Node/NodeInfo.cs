using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using System.Collections;

namespace SiteServer.CMS.Model
{
    public class NodeAttribute
    {
        protected NodeAttribute()
        {
        }

        public const string NodeID = "NodeID";
        public const string NodeName = "NodeName";
        public const string NodeType = "NodeType";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string ContentModelID = "ContentModelID";
        public const string ParentID = "ParentID";
        public const string ParentsPath = "ParentsPath";
        public const string ParentsCount = "ParentsCount";
        public const string ChildrenCount = "ChildrenCount";
        public const string IsLastNode = "IsLastNode";
        public const string NodeIndexName = "NodeIndexName";
        public const string NodeGroupNameCollection = "NodeGroupNameCollection";
        public const string Taxis = "Taxis";
        public const string AddDate = "AddDate";
        public const string ImageUrl = "ImageUrl";
        public const string Content = "Content";
        public const string ContentNum = "ContentNum";
        public const string FilePath = "FilePath";
        public const string ChannelFilePathRule = "ChannelFilePathRule";
        public const string ContentFilePathRule = "ContentFilePathRule";
        public const string LinkUrl = "LinkUrl";
        public const string LinkType = "LinkType";
        public const string ChannelTemplateID = "ChannelTemplateID";
        public const string ContentTemplateID = "ContentTemplateID";
        public const string Keywords = "Keywords";
        public const string Description = "Description";
        public const string ExtendValues = "ExtendValues";

        //不存在
        public static string ID = "ID";
        public static string Title = "Title";
        public static string ChannelName = "ChannelName";
        public static string ChannelIndex = "ChannelIndex";
        public static string ChannelGroupNameCollection = "ChannelGroupNameCollection";
        public const string PageContent = "PageContent";
        public const string CountOfChannels = "CountOfChannels";			//子栏目数
        public const string CountOfContents = "CountOfContents";			//内容数
        public const string CountOfImageContents = "CountOfImageContents";	//图片内容数

        private static ArrayList lowerDefaultAttributes;
        public static ArrayList LowerDefaultAttributes
        {
            get
            {
                if (lowerDefaultAttributes == null)
                {
                    lowerDefaultAttributes = new ArrayList();
                    lowerDefaultAttributes.Add(NodeID.ToLower());
                    lowerDefaultAttributes.Add(NodeName.ToLower());
                    lowerDefaultAttributes.Add(NodeType.ToLower());
                    lowerDefaultAttributes.Add(PublishmentSystemID.ToLower());
                    lowerDefaultAttributes.Add(ParentID.ToLower());
                    lowerDefaultAttributes.Add(ParentsPath.ToLower());
                    lowerDefaultAttributes.Add(ParentsCount.ToLower());
                    lowerDefaultAttributes.Add(ChildrenCount.ToLower());
                    lowerDefaultAttributes.Add(IsLastNode.ToLower());
                    lowerDefaultAttributes.Add(NodeIndexName.ToLower());
                    lowerDefaultAttributes.Add(NodeGroupNameCollection.ToLower());
                    lowerDefaultAttributes.Add(Taxis.ToLower());
                    lowerDefaultAttributes.Add(AddDate.ToLower());
                    lowerDefaultAttributes.Add(ImageUrl.ToLower());
                    lowerDefaultAttributes.Add(Content.ToLower());
                    lowerDefaultAttributes.Add(ContentNum.ToLower());
                    lowerDefaultAttributes.Add(FilePath.ToLower());
                    lowerDefaultAttributes.Add(ChannelFilePathRule.ToLower());
                    lowerDefaultAttributes.Add(ContentFilePathRule.ToLower());

                    lowerDefaultAttributes.Add(LinkUrl.ToLower());
                    lowerDefaultAttributes.Add(LinkType.ToLower());
                    lowerDefaultAttributes.Add(ChannelTemplateID.ToLower());
                    lowerDefaultAttributes.Add(ContentTemplateID.ToLower());

                    lowerDefaultAttributes.Add(ExtendValues.ToLower());
                }

                return lowerDefaultAttributes;
            }
        }
    }

	public class NodeInfo
	{
		private int nodeID;
		private string nodeName;
		private ENodeType nodeType;
		private int publishmentSystemID;
        private string contentModelID;
		private int parentID;
		private string parentsPath;
		private int parentsCount;
		private int childrenCount;
		private bool isLastNode;
        private string nodeIndexName;
        private string nodeGroupNameCollection;
		private int taxis;
		private DateTime addDate;
		private string imageUrl;
		private string content;
		private int contentNum;
        private int commentNum;
        private string filePath;
        private string channelFilePathRule;
        private string contentFilePathRule;
        private string linkUrl;
        private ELinkType linkType;
        private int channelTemplateID;
        private int contentTemplateID;
        private string keywords;
        private string description;
        private string extendValues;

		public NodeInfo()
		{
			this.nodeID = 0;
			this.nodeName = string.Empty;
			this.nodeType = ENodeType.BackgroundNormalNode;
			this.publishmentSystemID = 0;
            this.contentModelID = EContentModelTypeUtils.GetValue(EContentModelType.Content);
			this.parentID = 0;
			this.parentsPath = string.Empty;
			this.parentsCount = 0;
			this.childrenCount = 0;
			this.isLastNode = false;
			this.nodeIndexName = string.Empty;
			this.nodeGroupNameCollection = string.Empty;
			this.taxis = 0;
			this.addDate = DateTime.Now;
			this.imageUrl = string.Empty;
			this.content = string.Empty;
			this.contentNum = 0;
            this.commentNum = 0;
            this.filePath = string.Empty;
            this.channelFilePathRule = string.Empty;
            this.contentFilePathRule = string.Empty;
            this.linkUrl = string.Empty;
            this.linkType = ELinkType.LinkNoRelatedToChannelAndContent;
            this.channelTemplateID = 0;
            this.contentTemplateID = 0;
            this.keywords = string.Empty;
            this.description = string.Empty;
            this.extendValues = string.Empty;
		}

        public NodeInfo(int nodeID, string nodeName, ENodeType nodeType, int publishmentSystemID, string contentModelID, int parentID, string parentsPath, int parentsCount, int childrenCount, bool isLastNode, string nodeIndexName, string nodeGroupNameCollection, int taxis, DateTime addDate, string imageUrl, string content, int contentNum, int commentNum, string filePath, string channelFilePathRule, string contentFilePathRule, string linkUrl, ELinkType linkType, int channelTemplateID, int contentTemplateID, string keywords, string description, string extendValues) 
		{
			this.nodeID = nodeID;
			this.nodeName = nodeName;
			this.nodeType = nodeType;
			this.publishmentSystemID = publishmentSystemID;
            this.contentModelID = contentModelID;
			this.parentID = parentID;
			this.parentsPath = parentsPath;
			this.parentsCount = parentsCount;
			this.childrenCount = childrenCount;
			this.isLastNode = isLastNode;
			this.nodeIndexName = nodeIndexName;
			this.nodeGroupNameCollection = nodeGroupNameCollection;
			this.taxis = taxis;
			this.addDate = addDate;
			this.imageUrl = imageUrl;
			this.content = content;
			this.contentNum = contentNum;
            this.commentNum = commentNum;
            this.filePath = filePath;
            this.channelFilePathRule = channelFilePathRule;
            this.contentFilePathRule = contentFilePathRule;
            this.linkUrl = linkUrl;
            this.linkType = linkType;
            this.channelTemplateID = channelTemplateID;
            this.contentTemplateID = contentTemplateID;
            this.keywords = keywords;
            this.description = description;
            this.extendValues = extendValues;
		}

        public NodeInfo(NodeInfo nodeInfo)
        {
            this.nodeID = nodeInfo.nodeID;
            this.nodeName = nodeInfo.nodeName;
            this.nodeType = nodeInfo.nodeType;
            this.publishmentSystemID = nodeInfo.publishmentSystemID;
            this.contentModelID = nodeInfo.contentModelID;
            this.parentID = nodeInfo.parentID;
            this.parentsPath = nodeInfo.parentsPath;
            this.parentsCount = nodeInfo.parentsCount;
            this.childrenCount = nodeInfo.childrenCount;
            this.isLastNode = nodeInfo.isLastNode;
            this.nodeIndexName = nodeInfo.nodeIndexName;
            this.nodeGroupNameCollection = nodeInfo.nodeGroupNameCollection;
            this.taxis = nodeInfo.taxis;
            this.addDate = nodeInfo.addDate;
            this.imageUrl = nodeInfo.imageUrl;
            this.content = nodeInfo.content;
            this.contentNum = nodeInfo.contentNum;
            this.commentNum = nodeInfo.commentNum;
            this.filePath = nodeInfo.filePath;
            this.channelFilePathRule = nodeInfo.channelFilePathRule;
            this.contentFilePathRule = nodeInfo.contentFilePathRule;
            this.linkUrl = nodeInfo.LinkUrl;
            this.linkType = nodeInfo.LinkType;
            this.channelTemplateID = nodeInfo.ChannelTemplateID;
            this.contentTemplateID = nodeInfo.ContentTemplateID;
            this.keywords = nodeInfo.keywords;
            this.description = nodeInfo.description;
            this.extendValues = nodeInfo.extendValues;
        }

		public int NodeID
		{
			get{ return nodeID; }
            set { nodeID = value; }
		}

		public string NodeName
		{
			get{ return nodeName; }
			set{ nodeName = value; }
		}

		public ENodeType NodeType
		{
			get{ return nodeType; }
			set{ nodeType = value; }
		}

		public int PublishmentSystemID
		{
			get{ return publishmentSystemID; }
			set{ publishmentSystemID = value; }
		}

        public string ContentModelID
        {
            get
            {
                if (string.IsNullOrEmpty(contentModelID))
                {
                    contentModelID = EContentModelTypeUtils.GetValue(EContentModelType.Content);
                }
                return contentModelID;
            }
            set { contentModelID = value; }
        }

		public int ParentID
		{
			get{ return parentID; }
			set{ parentID = value; }
		}

		public string ParentsPath
		{
			get{ return parentsPath; }
			set{ parentsPath = value; }
		}

		public int ParentsCount
		{
			get{ return parentsCount; }
			set{ parentsCount = value; }
		}

		public int ChildrenCount
		{
			get{ return childrenCount; }
			set{ childrenCount = value; }
		}

        public bool IsLastNode
		{
			get{ return isLastNode; }
			set{ isLastNode = value; }
		}

		public string NodeIndexName
		{
			get{ return nodeIndexName; }
			set{ nodeIndexName = value; }
		}

		public string NodeGroupNameCollection
		{
			get{ return nodeGroupNameCollection; }
			set{ nodeGroupNameCollection = value; }
		}

		public int Taxis
		{
			get{ return taxis; }
			set{ taxis = value; }
		}

		public DateTime AddDate
		{
			get{ return addDate; }
			set{ addDate = value; }
		}

		public string ImageUrl
		{
			get{ return imageUrl; }
			set{ imageUrl = value; }
		}

		public string Content
		{
			get{ return content; }
			set{ content = value; }
		}

		public int ContentNum
		{
			get{ return contentNum; }
			set{ contentNum = value; }
		}

        public int CommentNum
        {
            get { return commentNum; }
            set { commentNum = value; }
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

        public string LinkUrl
        {
            get { return linkUrl; }
            set { linkUrl = value; }
        }

        public ELinkType LinkType
        {
            get { return linkType; }
            set { linkType = value; }
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

        public string Keywords
        {
            get { return keywords; }
            set { keywords = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        //public string ExtendValues
        //{
        //    get { return extendValues; }
        //    set { extendValues = value; }
        //}

        public void SetExtendValues(string extendValues)
        {
            this.extendValues = extendValues;
        }

        NodeInfoExtend additional;
        public NodeInfoExtend Additional
        {
            get
            {
                if (this.additional == null)
                {
                    this.additional = new NodeInfoExtend(this.extendValues);
                }
                return this.additional;
            }
        }
	}
}
