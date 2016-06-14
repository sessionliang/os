using System;
using BaiRong.Model;

namespace SiteServer.B2C.Model
{
	public class FilterItemInfo
	{
		private int itemID;
		private int filterID;
		private string title;
		private string _value;
        private int taxis;

		public FilterItemInfo()
		{
            this.itemID = 0;
            this.filterID = 0;
            this.title = string.Empty;
            this._value = string.Empty;
            this.taxis = 0;
		}

        public FilterItemInfo(int itemID, int filterID, string title, string value, int taxis)
		{
            this.itemID = itemID;
            this.filterID = filterID;
            this.title = title;
            this._value = value;
            this.taxis = taxis;
		}

        public int ItemID
        {
            get { return itemID; }
            set { itemID = value; }
        }

        public int FilterID
		{
            get { return filterID; }
            set { filterID = value; }
		}

        public string Title
		{
            get { return title; }
            set { title = value; }
		}

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public int Taxis
        {
            get { return taxis; }
            set { taxis = value; }
        }
	}
}
