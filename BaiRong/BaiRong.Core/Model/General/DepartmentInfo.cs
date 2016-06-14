using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;

namespace BaiRong.Model
{
	public class DepartmentInfo
	{
		private int departmentID;
		private string departmentName;
        private string code;
		private int parentID;
		private string parentsPath;
		private int parentsCount;
		private int childrenCount;
		private bool isLastNode;
		private int taxis;
		private DateTime addDate;
		private string summary;
		private int countOfAdmin;

		public DepartmentInfo()
		{
			this.departmentID = 0;
			this.departmentName = string.Empty;
            this.code = string.Empty;
			this.parentID = 0;
			this.parentsPath = string.Empty;
			this.parentsCount = 0;
			this.childrenCount = 0;
			this.isLastNode = false;
			this.taxis = 0;
			this.addDate = DateTime.Now;
			this.summary = string.Empty;
            this.countOfAdmin = 0;
		}

        public DepartmentInfo(int departmentID, string departmentName, string code, int parentID, string parentsPath, int parentsCount, int childrenCount, bool isLastNode, int taxis, DateTime addDate, string summary, int countOfAdmin) 
		{
            this.departmentID = departmentID;
            this.departmentName = departmentName;
            this.code = code;
            this.parentID = parentID;
            this.parentsPath = parentsPath;
            this.parentsCount = parentsCount;
            this.childrenCount = childrenCount;
            this.isLastNode = isLastNode;
            this.taxis = taxis;
            this.addDate = addDate;
            this.summary = summary;
            this.countOfAdmin = countOfAdmin;
		}

        public int DepartmentID
		{
            get { return departmentID; }
            set { departmentID = value; }
		}

        public string DepartmentName
		{
            get { return departmentName; }
            set { departmentName = value; }
		}

        public string Code
        {
            get { return code; }
            set { code = value; }
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

        public int CountOfAdmin
		{
            get { return countOfAdmin; }
            set { countOfAdmin = value; }
		}
	}
}
