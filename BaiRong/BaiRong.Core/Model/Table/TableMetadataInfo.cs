using System;
using System.Text;

namespace BaiRong.Model
{
	[Serializable]
	public class TableMetadataInfo
	{
		private int tableMetadataID;
		private string auxiliaryTableENName;
		private string attributeName;
		private EDataType dataType;
		private int dataLength;
		private bool canBeNull;
		private string dBDefaultValue;
		private int taxis;
        private bool isSystem;

		public TableMetadataInfo()
		{
			this.tableMetadataID = 0;
			this.auxiliaryTableENName = string.Empty;
			this.attributeName = string.Empty;
			this.dataType = EDataType.VarChar;
			this.dataLength = 50;
			this.canBeNull = true;
			this.dBDefaultValue = string.Empty;
			this.taxis = 0;
			this.isSystem = false;
		}

        public TableMetadataInfo(int tableMetadataID, string auxiliaryTableENName, string attributeName, EDataType dataType, int dataLength, bool canBeNull, string dBDefaultValue, int taxis, bool isSystem) 
		{
			this.tableMetadataID = tableMetadataID;
			this.auxiliaryTableENName = auxiliaryTableENName;
			this.attributeName = attributeName;
			this.dataType = dataType;
			this.dataLength = dataLength;
			this.canBeNull = canBeNull;
			this.dBDefaultValue = dBDefaultValue;
			this.taxis = taxis;
			this.isSystem = isSystem;
		}

		public int TableMetadataID
		{
			get{ return tableMetadataID; }
			set{ tableMetadataID = value; }
		}

		public string AuxiliaryTableENName
		{
			get{ return auxiliaryTableENName; }
			set{ auxiliaryTableENName = value; }
		}

		public string AttributeName
		{
			get{ return attributeName; }
			set{ attributeName = value; }
		}

		public EDataType DataType
		{
			get{ return dataType; }
			set{ dataType = value; }
		}

		public int DataLength
		{
			get{ return dataLength; }
			set{ dataLength = value; }
		}

        public bool CanBeNull
		{
			get{ return canBeNull; }
			set{ canBeNull = value; }
		}

		public string DBDefaultValue
		{
			get{ return dBDefaultValue; }
			set{ dBDefaultValue = value; }
		}

		public int Taxis
		{
			get{ return taxis; }
			set{ taxis = value; }
		}

        public bool IsSystem
		{
			get{ return isSystem; }
			set{ isSystem = value; }
		}

        private TableStyleInfo styleInfo;
        public TableStyleInfo StyleInfo
        {
            get { return styleInfo; }
            set { styleInfo = value; }
        }


	}
}
