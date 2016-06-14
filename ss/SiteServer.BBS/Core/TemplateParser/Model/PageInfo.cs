using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using BaiRong.Core;
using SiteServer.BBS.Model;

namespace SiteServer.BBS.Core.TemplateParser.Model
{
    public class PageInfo
    {
        private int publishmentSystemID;
        private ETemplateType templateType;
        private string directoryName;
        private string fileName;

        private int forumID;
        private int threadID;
        private int uniqueID;

        private readonly Stack forumItems;
        private readonly Stack threadItems;
        private readonly Stack postItems;
        private readonly Stack sqlItems;

        public PageInfo(int publishmentSystemID, ETemplateType templateType, string directoryName, string fileName, int forumID, int threadID)
        {
            this.publishmentSystemID = publishmentSystemID;
            this.templateType = templateType;
            this.directoryName = directoryName;
            this.fileName = fileName;
            this.forumID = forumID;
            this.threadID = threadID;
            this.uniqueID = 1;

            this.forumItems = new Stack(5);
            this.threadItems = new Stack(5);
            this.postItems = new Stack(5);
            this.sqlItems = new Stack(5);
        }

        public int PublishmentSystemID
        {
            get { return this.publishmentSystemID; }
            set { this.publishmentSystemID = value; }
        }

        public ETemplateType TemplateType
        {
            get { return this.templateType; }
            set { this.templateType = value; }
        }

        public string DirectoryName
        {
            get { return this.directoryName; }
            set { this.directoryName = value; }
        }

        public string FileName
        {
            get { return this.fileName; }
            set { this.fileName = value; }
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

        public int UniqueID
        {
            get
            {
                return uniqueID++;
            }
        }

        public void SetUniqueID(int uniqueID)
        {
            this.uniqueID = uniqueID;
        }

        public Stack ForumItems
        {
            get { return forumItems; }
        }

        public Stack ThreadItems
        {
            get { return threadItems; }
        }

        public Stack PostItems
        {
            get { return postItems; }
        }

        public Stack SqlItems
        {
            get { return sqlItems; }
        }
    }
}
