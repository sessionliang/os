using System;
using System.Collections;
using System.Collections.Specialized;

namespace BaiRong.Model
{
    [Serializable]
	public class TableMatchInfo
	{

		private int tableMatchID;
		private string connectionString;
		private string tableName;
		private string connectionStringToMatch;
		private string tableNameToMatch;
		private NameValueCollection columnsMap;

		public TableMatchInfo()
		{
			this.tableMatchID = 0;
			this.connectionString = string.Empty;
			this.tableName = string.Empty;
			this.connectionStringToMatch = string.Empty;
			this.tableNameToMatch = string.Empty;
			this.columnsMap = new NameValueCollection();
		}

		public TableMatchInfo(int tableMatchID, string connectionString, string tableName, string connectionStringToMatch, string tableNameToMatch, NameValueCollection columnsMap) 
		{
			this.tableMatchID = tableMatchID;
			this.connectionString = connectionString;
			this.tableName = tableName;
			this.connectionStringToMatch = connectionStringToMatch;
			this.tableNameToMatch = tableNameToMatch;
			this.columnsMap = columnsMap;
		}

		public int TableMatchID
		{
			get{ return tableMatchID; }
			set{ tableMatchID = value; }
		}

		public string ConnectionString
		{
			get{ return connectionString; }
			set{ connectionString = value; }
		}

		public string TableName
		{
			get{ return tableName; }
			set{ tableName = value; }
		}

		public string ConnectionStringToMatch
		{
			get{ return connectionStringToMatch; }
			set{ connectionStringToMatch = value; }
		}

		public string TableNameToMatch
		{
			get{ return tableNameToMatch; }
			set{ tableNameToMatch = value; }
		}

		public NameValueCollection ColumnsMap
		{
			get{ return this.columnsMap; }
			set{ columnsMap = value; }
		}
	}
}
