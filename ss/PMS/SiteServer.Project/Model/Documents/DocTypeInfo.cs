using System;

namespace SiteServer.Project.Model
{
	public class DocTypeInfo
	{
        private int typeID;
        private int parentID;
        private string typeName;
        private int taxis;
		private string description;
        private DateTime addDate;

		public DocTypeInfo()
		{
            this.typeID = 0;
            this.parentID = 0;
            this.typeName = string.Empty;
            this.taxis = 0;
			this.description = string.Empty;
            this.addDate = DateTime.Now;
		}

        public DocTypeInfo(int typeID, int parentID, string typeName, int taxis, string description, DateTime addDate)
		{
            this.typeID = typeID;
            this.parentID = parentID;
            this.typeName = typeName;
            this.taxis = taxis;
            this.description = description;
            this.addDate = addDate;
		}

        public int TypeID
        {
            get { return typeID; }
            set { typeID = value; }
        }

        public int ParentID
        {
            get { return parentID; }
            set { parentID = value; }
        }

        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }

        public int Taxis
        {
            get { return taxis; }
            set { taxis = value; }
        }

		public string Description
		{
			get{ return description; }
			set{ description = value; }
		}

        public DateTime AddDate
        {
            get { return addDate; }
            set { addDate = value; }
        }
	}
}
