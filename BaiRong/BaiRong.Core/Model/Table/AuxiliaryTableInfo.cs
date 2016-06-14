using System;
using BaiRong.Model;

namespace BaiRong.Model
{
    public class AuxiliaryTableInfo
	{
		private string tableENName;
		private string tableCNName;
		private int attributeNum;
		private EAuxiliaryTableType auxiliaryTableType;
		private bool isCreatedInDB;
        private bool isChangedAfterCreatedInDB;
        private bool isDefault;
		private string description;

		public AuxiliaryTableInfo()
		{
			this.tableENName = string.Empty;
			this.tableCNName = string.Empty;
			this.attributeNum = 0;
			this.auxiliaryTableType = EAuxiliaryTableType.BackgroundContent;
			this.isCreatedInDB = false;
			this.isChangedAfterCreatedInDB = false;
            this.isDefault = false;
			this.description = string.Empty;
		}

        public AuxiliaryTableInfo(string tableENName, string tableCNName, int attributeNum, EAuxiliaryTableType auxiliaryTableType, bool isCreatedInDB, bool isChangedAfterCreatedInDB, bool isDefault, string description) 
		{
			this.tableENName = tableENName;
			this.tableCNName = tableCNName;
			this.attributeNum = attributeNum;
			this.auxiliaryTableType = auxiliaryTableType;
			this.isCreatedInDB = isCreatedInDB;
			this.isChangedAfterCreatedInDB = isChangedAfterCreatedInDB;
            this.isDefault = isDefault;
			this.description = description;
		}

		public string TableENName
		{
			get{ return tableENName; }
			set{ tableENName = value; }
		}

		public string TableCNName
		{
			get{ return tableCNName; }
			set{ tableCNName = value; }
		}

		public int AttributeNum
		{
			get{ return attributeNum; }
			set{ attributeNum = value; }
		}

		public EAuxiliaryTableType AuxiliaryTableType
		{
			get{ return auxiliaryTableType; }
			set{ auxiliaryTableType = value; }
		}

        public bool IsCreatedInDB
		{
			get{ return isCreatedInDB; }
			set{ isCreatedInDB = value; }
		}

        public bool IsChangedAfterCreatedInDB
		{
			get{ return isChangedAfterCreatedInDB; }
			set{ isChangedAfterCreatedInDB = value; }
		}

        public bool IsDefault
        {
            get { return isDefault; }
            set { isDefault = value; }
        }

		public string Description
		{
			get{ return description; }
			set{ description = value; }
		}

	}
}
