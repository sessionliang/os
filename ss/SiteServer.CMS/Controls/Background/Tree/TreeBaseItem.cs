using BaiRong.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteServer.CMS.Controls
{
    public class TreeBaseItem : ExtendedAttributes
    {
        public TreeBaseItem()
        {

        }

        public TreeBaseItem(object dataItem)
            : base(dataItem)
		{
		}


        public TreeBaseItem(int itemID, string itemName, string itemIndexName, int parentID, string parentsPath, int parentsCount, int childrenCount, int contentNum, int publishmentSystemID, bool enabled, bool isLastItem, int taxis)
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
        }


        /// <summary>
        /// ItemID
        /// </summary>
        public int ItemID { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public int ParentID { get; set; }

        /// <summary>
        /// 父级路径
        /// </summary>
        public string ParentsPath { get; set; }

        /// <summary>
        /// 父级数量
        /// </summary>
        public int ParentsCount { get; set; }

        /// <summary>
        /// 子集数量
        /// </summary>
        public int ChildrenCount { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public int PublishmentSystemID { get; set; }

        /// <summary>
        /// 分类内容数量
        /// </summary>
        public int ContentNum { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 分类索引
        /// </summary>
        public string ItemIndexName { get; set; }

        /// <summary>
        /// 是否是最后一个节点
        /// </summary>
        public bool IsLastItem { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public int Taxis { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enabled { get; set; }

    }
}
