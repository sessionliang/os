using BaiRong.Core;
using BaiRong.Model;
using System;

namespace SiteServer.Project.Model
{
	public class ProjectDocumentInfo
	{
        private int documentID;
        private int projectID;
        private string fileName;
        private string description;
        private string userName;
        private DateTime addDate;

		public ProjectDocumentInfo()
		{
            this.documentID = 0;
            this.projectID = 0;
            this.fileName = string.Empty;
            this.description = string.Empty;
            this.userName = string.Empty;
            this.addDate = DateTime.Now;
		}

        public ProjectDocumentInfo(int documentID, int projectID, string fileName, string description, string userName, DateTime addDate)
		{
            this.documentID = documentID;
            this.projectID = projectID;
            this.fileName = fileName;
            this.description = description;
            this.userName = userName;
            this.addDate = addDate;
		}

        public int DocumentID
        {
            get { return documentID; }
            set { documentID = value; }
        }

        public int ProjectID
        {
            get { return projectID; }
            set { projectID = value; }
        }

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public DateTime AddDate
        {
            get { return addDate; }
            set { addDate = value; }
        }
	}
}
