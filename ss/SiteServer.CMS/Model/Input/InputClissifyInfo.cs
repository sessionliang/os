using SiteServer.CMS.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiteServer.CMS.Model
{

    /**
    **
    *  by 20151026 sofuny
    表单分类
    */


    public class InputClissifyInfo : TreeBaseItem
    {

        public InputClissifyInfo() { }

        public InputClissifyInfo(int itemID, string itemName, string itemIndexName, int parentID, string parentsPath, int parentsCount, int childrenCount, int contentNum, int publishmentSystemID, bool enabled, bool isLastItem, int taxis, DateTime addDate)
            : base(itemID, itemName, itemIndexName, parentID, parentsPath, parentsCount, childrenCount, contentNum, publishmentSystemID, enabled, isLastItem, taxis)
        {
            this.ItemID = itemID;
            this.ParentID = parentID;
            this.ParentsPath = parentsPath;
            this.ParentsCount = parentsCount;
            this.ChildrenCount = childrenCount;
            this.PublishmentSystemID = publishmentSystemID;
            this.ContentNum = contentNum;
            this.ItemName = itemName;
            this.ItemIndexName = itemIndexName;
            this.IsLastItem = isLastItem;
            this.Taxis = taxis;
            this.Enabled = enabled;
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
