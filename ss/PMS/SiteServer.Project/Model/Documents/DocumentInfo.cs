using BaiRong.Core;
using BaiRong.Model;
using System;

namespace SiteServer.Project.Model
{
	public class DocumentInfo
	{
        private int documentID;
        private EDocumentType documentType;
        private int contractID;
        private int typeID;
        private string fileName;
        private string version;
        private string description;
        private string userName;
        private DateTime addDate;

		public DocumentInfo()
		{
            this.documentID = 0;
            this.documentType = EDocumentType.Category;
            this.contractID = 0;
            this.typeID = 0;
            this.fileName = string.Empty;
            this.version = string.Empty;
            this.description = string.Empty;
            this.userName = string.Empty;
            this.addDate = DateTime.Now;
		}

        public DocumentInfo(int documentID, EDocumentType documentType, int contractID, int typeID, string fileName, string version, string description, string userName, DateTime addDate)
		{
            this.documentID = documentID;
            this.documentType = documentType;
            this.contractID = contractID;
            this.typeID = typeID;
            this.fileName = fileName;
            this.version = version;
            this.description = description;
            this.userName = userName;
            this.addDate = addDate;
		}

        public int DocumentID
        {
            get { return documentID; }
            set { documentID = value; }
        }

        public EDocumentType DocumentType
        {
            get { return documentType; }
            set { documentType = value; }
        }

        public int ContractID
        {
            get { return contractID; }
            set { contractID = value; }
        }

        public int TypeID
        {
            get { return typeID; }
            set { typeID = value; }
        }

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public string Version
        {
            get { return version; }
            set { version = value; }
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
