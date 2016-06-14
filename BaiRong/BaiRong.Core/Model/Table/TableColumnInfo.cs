using System;
using System.Text;
using BaiRong.Model;

namespace BaiRong.Model
{
	public class TableColumnInfo
	{
		private string databaseName;
		private string tableID;
		private string columnName;
		private EDataType dataType;
		private int length;
		private int precision;
		private int scale;
		private bool isPrimaryKey;
		private bool isNullable;
		private bool isIdentity;
		private string defaultValue;
		private string foreignColumnName;
		private string foreignTableName;
		private int foreignTableID;

		public TableColumnInfo()
		{
			this.databaseName = string.Empty;
			this.tableID = string.Empty;
			this.columnName = string.Empty;
			this.dataType = EDataType.Unknown;
			this.length = 0;
			this.precision = 0;
			this.scale = 0;
			this.isPrimaryKey = false;
			this.isNullable = false;
			this.isIdentity = false;
			this.defaultValue = string.Empty;
			this.foreignColumnName = string.Empty;
			this.foreignTableName = string.Empty;
			this.foreignTableID = 0;
		}

		public TableColumnInfo(string databaseName, string tableID, string columnName, EDataType dataType, int length, int precision, int scale, bool isPrimaryKey, bool isNullable, bool isIdentity, string defaultValue, string foreignColumnName, string foreignTableName, int foreignTableID) 
		{
			this.databaseName = databaseName;
			this.tableID = tableID;
			this.columnName = columnName;
			this.dataType = dataType;
			this.length = length;
			this.precision = precision;
			this.scale = scale;
			this.isPrimaryKey = isPrimaryKey;
			this.isNullable = isNullable;
			this.isIdentity = isIdentity;
			this.defaultValue = defaultValue;
			this.foreignColumnName = foreignColumnName;
			this.foreignTableName = foreignTableName;
			this.foreignTableID = foreignTableID;
		}

		public string DatabaseName
		{
			get{ return databaseName; }
			set{ databaseName = value; }
		}

		public string TableID
		{
			get{ return tableID; }
			set{ tableID = value; }
		}

		public string ColumnName
		{
			get{ return columnName; }
			set{ columnName = value; }
		}

		public EDataType DataType
		{
			get{ return dataType; }
			set{ dataType = value; }
		}

		public int Length
		{
			get{ return length; }
			set{ length = value; }
		}

		public int Precision
		{
			get{ return precision; }
			set{ precision = value; }
		}

		public int Scale
		{
			get{ return scale; }
			set{ scale = value; }
		}

		public bool IsPrimaryKey
		{
			get{ return isPrimaryKey; }
			set{ isPrimaryKey = value; }
		}

		public bool IsNullable
		{
			get{ return isNullable; }
			set{ isNullable = value; }
		}

		public bool IsIdentity
		{
			get{ return isIdentity; }
			set{ isIdentity = value; }
		}

		public string DefaultValue
		{
			get{ return defaultValue; }
			set{ defaultValue = value; }
		}

		public string ForeignColumnName
		{
			get{ return foreignColumnName; }
			set{ foreignColumnName = value; }
		}

		public string ForeignTableName
		{
			get{ return foreignTableName; }
			set{ foreignTableName = value; }
		}

		public int ForeignTableID
		{
			get{ return foreignTableID; }
			set{ foreignTableID = value; }
		}
	}
}
