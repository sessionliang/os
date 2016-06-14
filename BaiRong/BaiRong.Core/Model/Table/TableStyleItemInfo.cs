using System;
using System.Text;

namespace BaiRong.Model
{
	public class TableStyleItemInfo
	{
        private int tableStyleItemID;
        private int tableStyleID;
        private string itemTitle;
        private string itemValue;
        private bool isSelected;

		public TableStyleItemInfo()
		{
            this.tableStyleItemID = 0;
            this.tableStyleID = 0;
            this.itemTitle = string.Empty;
            this.itemValue = string.Empty;
            this.isSelected = false;
		}

        public TableStyleItemInfo(int tableStyleItemID, int tableStyleID, string itemTitle, string itemValue, bool isSelected) 
		{
            this.tableStyleItemID = tableStyleItemID;
            this.tableStyleID = tableStyleID;
            this.itemTitle = itemTitle;
            this.itemValue = itemValue;
            this.isSelected = isSelected;
		}

        public int TableStyleItemID
		{
            get { return tableStyleItemID; }
            set { tableStyleItemID = value; }
		}

        public int TableStyleID
		{
            get { return tableStyleID; }
            set { tableStyleID = value; }
		}

        public string ItemTitle
        {
            get { return itemTitle; }
            set { itemTitle = value; }
        }

        public string ItemValue
        {
            get { return itemValue; }
            set { itemValue = value; }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }
	}
}
