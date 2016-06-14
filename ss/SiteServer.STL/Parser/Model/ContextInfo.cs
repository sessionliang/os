using System.Collections;
using System.Text;
using SiteServer.CMS.Model;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.Model
{
    public class ContextInfo
    {
        private EContextType contextType = EContextType.Undefined;
        private PublishmentSystemInfo publishmentSystemInfo;
        private int channelID;
        private int contentID;
        private ContentInfo contentInfo;

        private bool isInnerElement;
        private bool isCurlyBrace;
        private int titleWordNum;
        private int totalNum;           //用于缓存列表内容总数
        private int pageItemIndex;
        private DbItemContainer itemContainer;
        private string containerClientID;

        public ContextInfo(PageInfo pageInfo)
        {
            this.publishmentSystemInfo = pageInfo.PublishmentSystemInfo;
            this.channelID = pageInfo.PageNodeID;
            this.contentID = pageInfo.PageContentID;
        }

        public ContextInfo(EContextType contextType, PublishmentSystemInfo publishmentSystemInfo, int channelID, int contentID, ContentInfo contentInfo)
        {
            this.contextType = contextType;
            this.publishmentSystemInfo = publishmentSystemInfo;
            this.channelID = channelID;
            this.contentID = contentID;
            this.contentInfo = contentInfo;
        }

        //用于clone
        private ContextInfo(ContextInfo contextInfo)
        {
            this.contextType = contextInfo.contextType;
            this.publishmentSystemInfo = contextInfo.publishmentSystemInfo;
            this.channelID = contextInfo.channelID;
            this.contentID = contextInfo.contentID;
            this.contentInfo = contextInfo.contentInfo;

            this.isInnerElement = contextInfo.isInnerElement;
            this.isCurlyBrace = contextInfo.isCurlyBrace;
            this.titleWordNum = contextInfo.titleWordNum;
            this.pageItemIndex = contextInfo.pageItemIndex;
            this.totalNum = contextInfo.totalNum;
            this.itemContainer = contextInfo.itemContainer;
            this.containerClientID = contextInfo.containerClientID;
        }

        public ContextInfo Clone()
        {
            ContextInfo contextInfo = new ContextInfo(this);
            return contextInfo;
        }

        public EContextType ContextType
        {
            get { return this.contextType; }
            set { this.contextType = value; }
        }

        public PublishmentSystemInfo PublishmentSystemInfo
        {
            get { return this.publishmentSystemInfo; }
            set { this.publishmentSystemInfo = value; }
        }

        public int ChannelID
        {
            get { return this.channelID; }
            set { this.channelID = value; }
        }

        public int ContentID
        {
            get { return this.contentID; }
            set { this.contentID = value; }
        }

        public ContentInfo ContentInfo
        {
            get
            {
                if (this.contentInfo == null)
                {
                    if (this.contentID > 0)
                    {
                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, this.channelID);
                        ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                        string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
                        this.contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, this.contentID);
                    }
                }
                return this.contentInfo;
            }
            set { this.contentInfo = value; }
        }

        public bool IsInnerElement
        {
            get { return this.isInnerElement; }
            set { this.isInnerElement = value; }
        }

        public bool IsCurlyBrace
        {
            get { return this.isCurlyBrace; }
            set { this.isCurlyBrace = value; }
        }

        public int TitleWordNum
        {
            get { return this.titleWordNum; }
            set { this.titleWordNum = value; }
        }

        public int TotalNum
        {
            get { return this.totalNum; }
            set { this.totalNum = value; }
        }

        public int PageItemIndex
        {
            get { return this.pageItemIndex; }
            set { this.pageItemIndex = value; }
        }

        public DbItemContainer ItemContainer
        {
            get { return this.itemContainer; }
            set { this.itemContainer = value; }
        }

        public string ContainerClientID
        {
            get { return this.containerClientID; }
            set { this.containerClientID = value; }
        }
    }
}
