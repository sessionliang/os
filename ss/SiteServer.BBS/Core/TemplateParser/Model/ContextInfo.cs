using System;
using System.Collections.Generic;
using System.Text;
using SiteServer.BBS.Model;

namespace SiteServer.BBS.Core.TemplateParser.Model
{
    public class ContextInfo
    {
        private EContextType contextType = EContextType.Undefined;
        private bool isInnerElement;
        private int pageItemIndex;
        private DbItemContainer itemContainer;
        private string containerClientID;

        private int publishmentSystemID;
        private int forumID;
        private int threadID;
        private ThreadInfo threadInfo;

        public ContextInfo(PageInfo pageInfo)
        {
            this.publishmentSystemID = pageInfo.PublishmentSystemID;
            this.forumID = pageInfo.ForumID;
            this.threadID = pageInfo.ThreadID;
        }

        private ContextInfo(ContextInfo contextInfo)
        {
            this.contextType = contextInfo.contextType;
            this.isInnerElement = contextInfo.isInnerElement;
            this.pageItemIndex = contextInfo.pageItemIndex;
            this.itemContainer = contextInfo.itemContainer;
            this.containerClientID = contextInfo.containerClientID;

            this.forumID = contextInfo.forumID;
            this.threadID = contextInfo.threadID;
            this.threadInfo = contextInfo.threadInfo;
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

        public bool IsInnerElement
        {
            get { return this.isInnerElement; }
            set { this.isInnerElement = value; }
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

        public int ForumID
        {
            get { return this.forumID; }
            set { this.forumID = value; }
        }

        public int ThreadID
        {
            get { return this.threadID; }
            set { this.threadID = value; }
        }

        public ThreadInfo ThreadInfo
        {
            get
            {
                if (this.threadInfo == null)
                {
                    if (this.threadID > 0)
                    {
                        this.threadInfo = DataProvider.ThreadDAO.GetThreadInfo(this.publishmentSystemID, this.threadID);
                    }
                }
                return this.threadInfo;
            }
            set { this.threadInfo = value; }
        }
    }
}
