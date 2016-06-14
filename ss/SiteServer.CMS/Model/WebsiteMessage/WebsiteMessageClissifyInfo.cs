using SiteServer.CMS.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiteServer.CMS.Model
{
    public class WebsiteMessageClassifyInfo : TreeBaseItem
    {

        public WebsiteMessageClassifyInfo() { }

        public WebsiteMessageClassifyInfo(int itemID, string itemName, string itemIndexName, int parentID, string parentsPath, int parentsCount, int childrenCount, int contentNum, int publishmentSystemID, bool enabled, bool isLastItem, int taxis, DateTime addDate)
            : base(itemID, itemName, itemIndexName, parentID, parentsPath, parentsCount, childrenCount, contentNum, publishmentSystemID, enabled, isLastItem, taxis)
        {
            this.AddDate = addDate;
        }

        #region self
        public DateTime AddDate
        {
            get; set;
        }
        #endregion
    }
}
