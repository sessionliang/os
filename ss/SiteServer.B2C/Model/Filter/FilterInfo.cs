using System.Collections.Generic;
namespace SiteServer.B2C.Model
{
	public class FilterInfo
	{
		private int filterID;
		private int publishmentSystemID;
        private int nodeID;
        private string attributeName;
        private string filterName;
        private bool isDefaultValues;
        private int taxis;

		public FilterInfo()
		{
            this.filterID = 0;
			this.publishmentSystemID = 0;
            this.nodeID = 0;
            this.attributeName = string.Empty;
            this.filterName = string.Empty;
            this.isDefaultValues = true;
            this.taxis = 0;
		}

        public FilterInfo(int filterID, int publishmentSystemID, int nodeID, string attributeName, string filterName, bool isDefaultValues, int taxis) 
		{
            this.filterID = filterID;
            this.publishmentSystemID = publishmentSystemID;
            this.nodeID = nodeID;
            this.attributeName = attributeName;
            this.filterName = filterName;
            this.isDefaultValues = isDefaultValues;
            this.taxis = taxis;
		}

        public int FilterID
		{
            get { return filterID; }
            set { filterID = value; }
		}

		public int PublishmentSystemID
		{
			get{ return publishmentSystemID; }
			set{ publishmentSystemID = value; }
		}

        public int NodeID
        {
            get { return nodeID; }
            set { nodeID = value; }
        }

        public string AttributeName
		{
            get { return attributeName; }
            set { attributeName = value; }
		}

        public string FilterName
        {
            get { return filterName; }
            set { filterName = value; }
        }

        public bool IsDefaultValues
        {
            get { return isDefaultValues; }
            set { isDefaultValues = value; }
        }

        public int Taxis
        {
            get { return taxis; }
            set { taxis = value; }
        }

        public List<FilterItemInfo> Items { get; set; }
	}
}
