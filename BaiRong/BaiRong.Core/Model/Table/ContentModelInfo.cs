namespace BaiRong.Model
{
	public class ContentModelInfo
	{
        private string modelID;
        private string productID;
		private int siteID;
        private string modelName;
        private bool isSystem;
        private string tableName;
        private EAuxiliaryTableType tableType;
        private string iconUrl;
		private string description;

		public ContentModelInfo()
		{
            this.modelID = string.Empty;
            this.productID = string.Empty;
            this.siteID = 0;
            this.modelName = string.Empty;
            this.isSystem = false;
            this.tableName = string.Empty;
            this.tableType = EAuxiliaryTableType.BackgroundContent;
            this.iconUrl = string.Empty;
			this.description = string.Empty;
		}

        public ContentModelInfo(string modelID, string productID, int siteID, string modelName, bool isSystem, string tableName, EAuxiliaryTableType tableType, string iconUrl, string description)
		{
            this.modelID = modelID;
            this.productID = productID;
            this.siteID = siteID;
            this.modelName = modelName;
            this.isSystem = isSystem;
            this.tableName = tableName;
            this.tableType = tableType;
            this.iconUrl = iconUrl;
            this.description = description;
		}

        public string ModelID
		{
            get { return modelID; }
            set { modelID = value; }
		}

        public string ProductID
        {
            get { return productID; }
            set { productID = value; }
        }

        public int SiteID
		{
			get{ return siteID; }
            set { siteID = value; }
		}

        public string ModelName
        {
            get { return modelName; }
            set { modelName = value; }
        }

        public bool IsSystem
        {
            get { return isSystem; }
            set { isSystem = value; }
        }

        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }

        public EAuxiliaryTableType TableType
        {
            get { return tableType; }
            set { tableType = value; }
        }

        public string IconUrl
        {
            get { return iconUrl; }
            set { iconUrl = value; }
        }

		public string Description
		{
			get{ return description; }
			set{ description = value; }
		}

	}
}
