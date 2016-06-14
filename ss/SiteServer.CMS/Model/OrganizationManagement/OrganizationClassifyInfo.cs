using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;
using SiteServer.CMS.Controls;

namespace SiteServer.CMS.Model
{
    public class OrganizationClassifyAttribute
    {
        protected OrganizationClassifyAttribute()
        {
        }

        public const string ItemID = "ItemID";
        public const string ItemName = "ItemName";
        public const string ItemIndexName = "ItemIndexName"; 
        public const string ParentID = "ParentID";
        public const string ParentsPath = "ParentsPath";
        public const string ParentsCount = "ParentsCount";
        public const string ChildrenCount = "ChildrenCount";
        public const string ContentNum = "ContentNum";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string Enabled = "Enabled";
        public const string IsLastItem = "IsLastItem";
        public const string Taxis = "Taxis";
        public const string AddDate = "AddDate";
        public const string UserName = "UserName";

        private static ArrayList allAttributes;
        public static ArrayList AllAttributes
        {
            get
            {
                if (allAttributes == null)
                {
                    allAttributes = new ArrayList(otherAttributes);
                    allAttributes.Add(ItemID.ToLower());
                }
                return allAttributes;
            }
        }


        private static ArrayList otherAttributes;
        public static ArrayList OtherAttributes
        {
            get
            {
                if (otherAttributes == null)
                {
                    otherAttributes = new ArrayList();
                    otherAttributes.Add(ItemName.ToLower()); 
                    otherAttributes.Add(Taxis.ToLower());
                    otherAttributes.Add(AddDate.ToLower());
                    otherAttributes.Add(PublishmentSystemID.ToLower());
                    otherAttributes.Add(UserName.ToLower());
                    otherAttributes.Add(ItemIndexName.ToLower());
                    otherAttributes.Add(ParentID.ToLower());
                    otherAttributes.Add(ParentsPath.ToLower());
                    otherAttributes.Add(ParentsCount.ToLower());
                    otherAttributes.Add(ChildrenCount.ToLower());
                    otherAttributes.Add(ContentNum.ToLower());
                    otherAttributes.Add(Enabled.ToLower());
                    otherAttributes.Add(IsLastItem.ToLower());
                }
                return otherAttributes;
            }
        }

    }

    public class OrganizationClassifyInfo : TreeBaseItem
    {
        public const string TableName = "siteserver_OrganizationClassify";

        public OrganizationClassifyInfo()
        {
            this.ItemID = 0;
            this.ItemName = string.Empty; 
            this.Taxis = 0;
            this.AddDate = DateTime.Now;
            this.PublishmentSystemID = 0;
            this.UserName = string.Empty;
            this.ItemIndexName = string.Empty;
            this.ParentID = 0;
            this.ParentsPath = string.Empty;
            this.ParentsCount = 0;
            this.ChildrenCount = 0;
            this.ContentNum = 0;
            this.Enabled = true;
            this.IsLastItem = false;
        }

        public OrganizationClassifyInfo(object dataItem)
            : base(dataItem)
        {
        }


        public OrganizationClassifyInfo(int itemID, string itemName, string itemIndexName, int parentID, string parentsPath, int parentsCount, int childrenCount, int contentNum, int publishmentSystemID, bool enabled, bool isLastItem, int taxis, DateTime addDate, string userName)
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
            this.UserName = userName;
        }

        public int ItemID
        {
            get { return base.GetInt(OrganizationClassifyAttribute.ItemID, 0); }
            set { base.SetExtendedAttribute(OrganizationClassifyAttribute.ItemID, value.ToString()); }
        }

        public string ItemName
        {
            get { return base.GetExtendedAttribute(OrganizationClassifyAttribute.ItemName); }
            set { base.SetExtendedAttribute(OrganizationClassifyAttribute.ItemName, value); }
        }
         

        public new int Taxis
        {
            get { return base.GetInt(OrganizationClassifyAttribute.Taxis, 0); }
            set { base.SetExtendedAttribute(OrganizationClassifyAttribute.Taxis, value.ToString()); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(OrganizationClassifyAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(OrganizationClassifyAttribute.AddDate, value.ToString()); }
        }

        public int PublishmentSystemID
        {
            get { return base.GetInt(OrganizationClassifyAttribute.PublishmentSystemID, 0); }
            set { base.SetExtendedAttribute(OrganizationClassifyAttribute.PublishmentSystemID, value.ToString()); }
        }

        public string UserName
        {
            get { return base.GetExtendedAttribute(OrganizationClassifyAttribute.UserName); }
            set { base.SetExtendedAttribute(OrganizationClassifyAttribute.UserName, value); }
        }

        public string ItemIndexName
        {
            get { return base.GetExtendedAttribute(OrganizationClassifyAttribute.ItemIndexName); }
            set { base.SetExtendedAttribute(OrganizationClassifyAttribute.ItemIndexName, value); }
        }

        public int ParentID
        {
            get { return base.GetInt(OrganizationClassifyAttribute.ParentID, 0); }
            set { base.SetExtendedAttribute(OrganizationClassifyAttribute.ParentID, value.ToString()); }
        }

        public string ParentsPath
        {
            get { return base.GetExtendedAttribute(OrganizationClassifyAttribute.ParentsPath); }
            set { base.SetExtendedAttribute(OrganizationClassifyAttribute.ParentsPath, value); }
        }

        public int ParentsCount
        {
            get { return base.GetInt(OrganizationClassifyAttribute.ParentsCount, 0); }
            set { base.SetExtendedAttribute(OrganizationClassifyAttribute.ParentsCount, value.ToString()); }
        }


        public int ChildrenCount
        {
            get { return base.GetInt(OrganizationClassifyAttribute.ChildrenCount, 0); }
            set { base.SetExtendedAttribute(OrganizationClassifyAttribute.ChildrenCount, value.ToString()); }
        }


        public int ContentNum
        {
            get { return base.GetInt(OrganizationClassifyAttribute.ContentNum, 0); }
            set { base.SetExtendedAttribute(OrganizationClassifyAttribute.ContentNum, value.ToString()); }
        }

        public bool Enabled
        {
            get { return base.GetBool(OrganizationClassifyAttribute.Enabled, false); }
            set { base.SetExtendedAttribute(OrganizationClassifyAttribute.Enabled, value.ToString()); }
        }

        public bool IsLastItem
        {
            get { return base.GetBool(OrganizationClassifyAttribute.IsLastItem, false); }
            set { base.SetExtendedAttribute(OrganizationClassifyAttribute.IsLastItem, value.ToString()); }
        }


        protected override ArrayList GetDefaultAttributesNames()
        {
            return OrganizationClassifyAttribute.OtherAttributes;
        }
    }
}
