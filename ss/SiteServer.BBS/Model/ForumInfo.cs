using System;
using System.Collections.Generic;
using System.Text;

namespace SiteServer.BBS.Model
{
    public class ForumInfo
    {
        private int forumID;
        private int publishmentSystemID;
        private string forumName;
        private string indexName;
        private int parentID;
        private string parentsPath;
        private int parentsCount;
        private int childrenCount;
        private bool isLastNode;
        private int taxis;
        private DateTime addDate;
        private string iconUrl;
        private string color;
        private int columns;
        private string metaKeywords;
        private string metaDescription;
        private string summary;
        private string content;
        private string filePath;
        private string filePathRule;
        private int templateID;
        private string linkUrl;
        private int threadCount;
        private int todayThreadCount;
        private int postCount;
        private int todayPostCount;
        private int lastThreadID;
        private int lastPostID;
        private string lastTitle;
        private string lastUserName;
        private DateTime lastDate;
        private string state;
        private string extendValues;

        public ForumInfo(int publishmentSystemID)
        {
            this.forumID = 0;
            this.publishmentSystemID = publishmentSystemID;
            this.forumName = string.Empty;
            this.indexName = string.Empty;
            this.parentID = 0;
            this.parentsPath = string.Empty;
            this.parentsCount = 0;
            this.childrenCount = 0;
            this.isLastNode = false;
            this.taxis = 0;
            this.addDate = DateTime.Now;
            this.iconUrl = string.Empty;
            this.color = string.Empty;
            this.columns = 0;
            this.metaKeywords = string.Empty;
            this.metaDescription = string.Empty;
            this.summary = string.Empty;
            this.content = string.Empty;
            this.filePath = string.Empty;
            this.filePathRule = string.Empty;
            this.templateID = 0;
            this.linkUrl = string.Empty;
            this.threadCount = 0;
            this.todayThreadCount = 0;
            this.postCount = 0;
            this.todayPostCount = 0;
            this.lastThreadID = 0;
            this.lastPostID = 0;
            this.lastTitle = string.Empty;
            this.lastUserName = string.Empty;
            this.lastDate = DateTime.Now;
            this.state = string.Empty;
            this.extendValues = string.Empty;
        }

        public ForumInfo(int forumID, int publishmentSystemID, string forumName, string indexName, int parentID, string parentsPath, int parentsCount, int childrenCount, bool isLastNode, int taxis, DateTime addDate, string iconUrl, string color, int columns, string metaKeywords, string metaDescription, string summary, string content, string filePath, string filePathRule, int templateID, string linkUrl, int threadCount, int todayThreadCount, int postCount, int todayPostCount, int lastThreadID, int lastPostID, string lastTitle, string lastUserName, DateTime lastDate, string state, string extendValues)
        {
            this.forumID = forumID;
            this.publishmentSystemID = publishmentSystemID;
            this.forumName = forumName;
            this.indexName = indexName;
            this.parentID = parentID;
            this.parentsPath = parentsPath;
            this.parentsCount = parentsCount;
            this.childrenCount = childrenCount;
            this.isLastNode = isLastNode;
            this.taxis = taxis;
            this.addDate = addDate;
            this.iconUrl = iconUrl;
            this.color = color;
            this.columns = columns;
            this.metaKeywords = metaKeywords;
            this.metaDescription = metaDescription;
            this.summary = summary;
            this.content = content;
            this.filePath = filePath;
            this.filePathRule = filePathRule;
            this.templateID = templateID;
            this.linkUrl = linkUrl;
            this.threadCount = threadCount;
            this.todayThreadCount = todayThreadCount;
            this.postCount = postCount;
            this.todayPostCount = todayPostCount;
            this.lastThreadID = lastThreadID;
            this.lastPostID = lastPostID;
            this.lastTitle = lastTitle;
            this.lastUserName = lastUserName;
            this.lastDate = lastDate;
            this.state = state;
            this.extendValues = extendValues;
        }

        public int ForumID
        {
            get
            {
                return forumID;
            }
            set
            {
                forumID = value;
            }
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public string ForumName
        {
            get
            {
                return forumName;
            }
            set
            {
                forumName = value;
            }
        }

        public string IndexName
        {
            get
            {
                return indexName;
            }
            set
            {
                indexName = value;
            }
        }

        public int ParentID
        {
            get
            {
                return parentID;
            }
            set
            {
                parentID = value;
            }
        }

        public string ParentsPath
        {
            get
            {
                return parentsPath;
            }
            set
            {
                parentsPath = value;
            }
        }

        public int ParentsCount
        {
            get
            {
                return parentsCount;
            }
            set
            {
                parentsCount = value;
            }
        }

        public int ChildrenCount
        {
            get
            {
                return childrenCount;
            }
            set
            {
                childrenCount = value;
            }
        }

        public bool IsLastNode
        {
            get
            {
                return isLastNode;
            }
            set
            {
                isLastNode = value;
            }
        }

        public int Taxis
        {
            get
            {
                return taxis;
            }
            set
            {
                taxis = value;
            }
        }

        public DateTime AddDate
        {
            get
            {
                return addDate;
            }
            set
            {
                addDate = value;
            }
        }

        public string IconUrl
        {
            get
            {
                return iconUrl;
            }
            set
            {
                iconUrl = value;
            }
        }

        public string Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        public int Columns
        {
            get
            {
                return columns;
            }
            set
            {
                columns = value;
            }
        }

        public string MetaKeywords
        {
            get
            {
                return metaKeywords;
            }
            set
            {
                metaKeywords = value;
            }
        }

        public string MetaDescription
        {
            get
            {
                return metaDescription;
            }
            set
            {
                metaDescription = value;
            }
        }

        public string Summary
        {
            get
            {
                return summary;
            }
            set
            {
                summary = value;
            }
        }

        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
            }
        }

        public string FilePath
        {
            get
            {
                return filePath;
            }
            set
            {
                filePath = value;
            }
        }

        public string FilePathRule
        {
            get
            {
                return filePathRule;
            }
            set
            {
                filePathRule = value;
            }
        }

        public int TemplateID
        {
            get
            {
                return templateID;
            }
            set
            {
                templateID = value;
            }
        }

        public string LinkUrl
        {
            get
            {
                return linkUrl;
            }
            set
            {
                linkUrl = value;
            }
        }

        public int ThreadCount
        {
            get
            {
                return threadCount;
            }
            set
            {
                threadCount = value;
            }
        }

        public int TodayThreadCount
        {
            get
            {
                return todayThreadCount;
            }
            set
            {
                todayThreadCount = value;
            }
        }

        public int PostCount
        {
            get
            {
                return postCount;
            }
            set
            {
                postCount = value;
            }
        }

        public int TodayPostCount
        {
            get
            {
                return todayPostCount;
            }
            set
            {
                todayPostCount = value;
            }
        }

        public int LastThreadID
        {
            get
            {
                return lastThreadID;
            }
            set
            {
                lastThreadID = value;
            }
        }

        public int LastPostID
        {
            get
            {
                return lastPostID;
            }
            set
            {
                lastPostID = value;
            }
        }

        public string LastTitle
        {
            get
            {
                return lastTitle;
            }
            set
            {
                lastTitle = value;
            }
        }

        public string LastUserName
        {
            get
            {
                return lastUserName;
            }
            set
            {
                lastUserName = value;
            }
        }

        public DateTime LastDate
        {
            get
            {
                return lastDate;
            }
            set
            {
                lastDate = value;
            }
        }

        public string State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }

        public string ExtendValues
        {
            get
            {
                return extendValues;
            }
            set
            {
                extendValues = value;
            }
        }

        ForumInfoExtend additional;
        public ForumInfoExtend Additional
        {
            get
            {
                if (this.additional == null)
                {
                    this.additional = new ForumInfoExtend(this.extendValues);
                }
                return this.additional;
            }
        }
    }
}