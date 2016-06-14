namespace SiteServer.Project.Model
{
	public class TypeInfo
	{
        private int typeID;
        private string typeName;
        private int projectID;
        private int taxis;

		public TypeInfo()
		{
            this.typeID = 0;
            this.typeName = string.Empty;
            this.projectID = 0;
            this.taxis = 0;
		}

        public TypeInfo(int typeID, string typeName, int projectID, int taxis)
		{
            this.typeID = typeID;
            this.typeName = typeName;
            this.projectID = projectID;
            this.taxis = taxis;
		}

        public int TypeID
        {
            get { return typeID; }
            set { typeID = value; }
        }

        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }

        public int ProjectID
        {
            get { return projectID; }
            set { projectID = value; }
        }

        public int Taxis
        {
            get { return taxis; }
            set { taxis = value; }
        }
	}
}
