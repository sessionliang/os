using System;
using BaiRong.Model;

namespace SiteServer.CMS.Model
{
	public class GatherDatabaseRuleInfo
	{
		private string gatherRuleName;
		private int publishmentSystemID;
		private EDatabaseType databaseType;
		private string connectionString;
		private string relatedTableName;
		private string relatedIdentity;
		private string relatedOrderBy;
        private string whereString;
		private int tableMatchID;
		private int nodeID;
		private int gatherNum;
		private bool isChecked;
        private bool isAutoCreate;
        private bool isOrderByDesc;
		private DateTime lastGatherDate;

		public GatherDatabaseRuleInfo()
		{
			this.gatherRuleName = string.Empty;
			this.publishmentSystemID = 0;
			this.databaseType = EDatabaseType.SqlServer;
			this.connectionString = string.Empty;
			this.relatedTableName = string.Empty;
			this.relatedIdentity = string.Empty;
			this.relatedOrderBy = string.Empty;
            this.whereString = string.Empty;
			this.tableMatchID = 0;
			this.nodeID = 0;
			this.gatherNum = 0;
			this.isChecked = true;
            this.isAutoCreate = false;
            this.isOrderByDesc = false;
			this.lastGatherDate = DateTime.Now;
		}

        public GatherDatabaseRuleInfo(string gatherRuleName, int publishmentSystemID, EDatabaseType databaseType, string connectionString, string relatedTableName, string relatedIdentity, string relatedOrderBy, string whereString, int tableMatchID, int nodeID, int gatherNum, bool isChecked, bool isOrderByDesc, DateTime lastGatherDate, bool isAutoCreate) 
		{
            this.gatherRuleName = gatherRuleName;
            this.publishmentSystemID = publishmentSystemID;
            this.databaseType = databaseType;
            this.connectionString = connectionString;
            this.relatedTableName = relatedTableName;
            this.relatedIdentity = relatedIdentity;
            this.relatedOrderBy = relatedOrderBy;
            this.whereString = whereString;
            this.tableMatchID = tableMatchID;
            this.nodeID = nodeID;
            this.gatherNum = gatherNum;
            this.isChecked = isChecked;
            this.isAutoCreate = isAutoCreate;
            this.isOrderByDesc = isOrderByDesc;
            this.lastGatherDate = lastGatherDate;
		}

		public string GatherRuleName
		{
			get { return gatherRuleName; }
			set { gatherRuleName = value; }
		}

		public int PublishmentSystemID
		{
			get { return publishmentSystemID; }
			set { publishmentSystemID = value; }
		}

        public EDatabaseType DatabaseType
		{
            get { return databaseType; }
            set { databaseType = value; }
		}

        public string ConnectionString
		{
            get { return connectionString; }
            set { connectionString = value; }
		}

        public string RelatedTableName
		{
            get { return relatedTableName; }
            set { relatedTableName = value; }
		}

        public string RelatedIdentity
		{
            get { return relatedIdentity; }
            set { relatedIdentity = value; }
		}

        public string RelatedOrderBy
		{
            get { return relatedOrderBy; }
            set { relatedOrderBy = value; }
		}

        public string WhereString
        {
            get { return whereString; }
            set { whereString = value; }
        }

        public int TableMatchID
		{
            get { return tableMatchID; }
            set { tableMatchID = value; }
		}

        public int NodeID
		{
            get { return nodeID; }
            set { nodeID = value; }
		}

        public int GatherNum
		{
            get { return gatherNum; }
            set { gatherNum = value; }
		}

        public bool IsChecked
		{
            get { return isChecked; }
            set { isChecked = value; }
		}

        public bool IsAutoCreate
        {
            get { return isAutoCreate; }
            set { isAutoCreate = value; }
        }

        public bool IsOrderByDesc
		{
            get { return isOrderByDesc; }
            set { isOrderByDesc = value; }
		}

		public DateTime LastGatherDate
		{
			get { return lastGatherDate; }
			set { lastGatherDate = value; }
		}
	}
}
