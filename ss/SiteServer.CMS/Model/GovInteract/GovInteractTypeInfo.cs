namespace SiteServer.CMS.Model
{
	public class GovInteractTypeInfo
	{
        private int typeID;
        private string typeName;
        private int nodeID;
        private int publishmentSystemID;
        private int taxis;

		public GovInteractTypeInfo()
		{
            this.typeID = 0;
            this.typeName = string.Empty;
            this.nodeID = 0;
            this.publishmentSystemID = 0;
            this.taxis = 0;
		}

        public GovInteractTypeInfo(int typeID, string typeName, int nodeID, int publishmentSystemID, int taxis)
		{
            this.typeID = typeID;
            this.typeName = typeName;
            this.nodeID = nodeID;
            this.publishmentSystemID = publishmentSystemID;
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

        public int NodeID
        {
            get { return nodeID; }
            set { nodeID = value; }
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public int Taxis
        {
            get { return taxis; }
            set { taxis = value; }
        }
	}
}
