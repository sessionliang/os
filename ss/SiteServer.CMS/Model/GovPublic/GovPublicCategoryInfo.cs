using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
	public class GovPublicCategoryInfo
	{
		private int categoryID;
        private string classCode;
        private int publishmentSystemID;
		private string categoryName;
        private string categoryCode;
		private int parentID;
		private string parentsPath;
		private int parentsCount;
		private int childrenCount;
		private bool isLastNode;
		private int taxis;
		private DateTime addDate;
		private string summary;
		private int contentNum;

		public GovPublicCategoryInfo()
		{
			this.categoryID = 0;
            this.classCode = string.Empty;
            this.publishmentSystemID = 0;
			this.categoryName = string.Empty;
            this.categoryCode = string.Empty;
			this.parentID = 0;
			this.parentsPath = string.Empty;
			this.parentsCount = 0;
			this.childrenCount = 0;
			this.isLastNode = false;
			this.taxis = 0;
			this.addDate = DateTime.Now;
			this.summary = string.Empty;
            this.contentNum = 0;
		}

        public GovPublicCategoryInfo(int categoryID, string classCode, int publishmentSystemID, string categoryName, string categoryCode, int parentID, string parentsPath, int parentsCount, int childrenCount, bool isLastNode, int taxis, DateTime addDate, string summary, int contentNum) 
		{
            this.categoryID = categoryID;
            this.classCode = classCode;
            this.publishmentSystemID = publishmentSystemID;
            this.categoryName = categoryName;
            this.categoryCode = categoryCode;
            this.parentID = parentID;
            this.parentsPath = parentsPath;
            this.parentsCount = parentsCount;
            this.childrenCount = childrenCount;
            this.isLastNode = isLastNode;
            this.taxis = taxis;
            this.addDate = addDate;
            this.summary = summary;
            this.contentNum = contentNum;
		}

        public int CategoryID
		{
            get { return categoryID; }
            set { categoryID = value; }
		}

        public string ClassCode
        {
            get { return classCode; }
            set { classCode = value; }
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public string CategoryName
		{
            get { return categoryName; }
            set { categoryName = value; }
		}

        public string CategoryCode
        {
            get { return categoryCode; }
            set { categoryCode = value; }
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

        public string Summary
		{
            get { return summary; }
            set { summary = value; }
		}

        public int ContentNum
		{
            get { return contentNum; }
            set { contentNum = value; }
		}
	}
}
