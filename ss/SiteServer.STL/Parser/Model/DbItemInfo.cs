namespace SiteServer.STL.Parser.Model
{
	public class DbItemInfo
	{
        private object dataItem;
        private int itemIndex;

        public DbItemInfo(object dataItem, int itemIndex)
		{
            this.dataItem = dataItem;
            this.itemIndex = itemIndex;
		}

        public object DataItem
		{
            get { return dataItem; }
		}

        public int ItemIndex
        {
            get { return itemIndex; }
        }

        public void Reload(object theDataItem, int theItemIndex)
        {
            this.dataItem = theDataItem;
            this.itemIndex = theItemIndex;
        }
	}
}
