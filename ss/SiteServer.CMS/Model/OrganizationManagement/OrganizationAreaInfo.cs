using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;
using SiteServer.CMS.Controls;

namespace SiteServer.CMS.Model
{
    public class OrganizationAreaAttribute
    {
        protected OrganizationAreaAttribute()
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
        public const string ClassifyID = "ClassifyID";
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
                    otherAttributes.Add(ItemIndexName.ToLower());
                    otherAttributes.Add(Taxis.ToLower()); 
                    otherAttributes.Add(AddDate.ToLower());
                    otherAttributes.Add(PublishmentSystemID.ToLower());
                    otherAttributes.Add(UserName.ToLower());
                    otherAttributes.Add(ClassifyID.ToLower());
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

    public class OrganizationAreaInfo : TreeBaseItem
    {
        public const string TableName = "siteserver_OrganizationArea";

        public OrganizationAreaInfo()
        {
            this.ItemID = 0;
            this.ItemName = string.Empty;
            this.Taxis = 0; 
            this.AddDate = DateTime.Now;
            this.ClassifyID = 0;
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

        public OrganizationAreaInfo(object dataItem)
            : base(dataItem)
		{
		}


        public OrganizationAreaInfo(int itemID, string itemName, string itemIndexName, int parentID, string parentsPath, int parentsCount, int childrenCount, int contentNum, int classifyID, int publishmentSystemID, bool enabled, bool isLastItem, int taxis, DateTime addDate, string userName)
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
            this.ClassifyID = classifyID;
            this.AddDate = addDate;
            this.UserName = userName;
        }

        public int ItemID
        {
            get { return base.GetInt(OrganizationAreaAttribute.ItemID, 0); }
            set { base.SetExtendedAttribute(OrganizationAreaAttribute.ItemID, value.ToString()); }
        }

        public string ItemName
        {
            get { return base.GetExtendedAttribute(OrganizationAreaAttribute.ItemName); }
            set { base.SetExtendedAttribute(OrganizationAreaAttribute.ItemName, value); }
        }

        public int  ClassifyID
        {
            get { return base.GetInt(OrganizationAreaAttribute.ClassifyID,0); }
            set { base.SetExtendedAttribute(OrganizationAreaAttribute.ClassifyID, value.ToString()); }
        }

        public new int Taxis
        {
            get { return base.GetInt(OrganizationAreaAttribute.Taxis, 0); }
            set { base.SetExtendedAttribute(OrganizationAreaAttribute.Taxis, value.ToString()); }
        }
          
        public DateTime AddDate
        {
            get { return base.GetDateTime(OrganizationAreaAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(OrganizationAreaAttribute.AddDate, value.ToString()); }
        }

        public int PublishmentSystemID
        {
            get { return base.GetInt(OrganizationAreaAttribute.PublishmentSystemID, 0); }
            set { base.SetExtendedAttribute(OrganizationAreaAttribute.PublishmentSystemID, value.ToString()); }
        }

        public string UserName
        {
            get { return base.GetExtendedAttribute(OrganizationAreaAttribute.UserName); }
            set { base.SetExtendedAttribute(OrganizationAreaAttribute.UserName, value); }
        }

        public string ItemIndexName
        {
            get { return base.GetExtendedAttribute(OrganizationAreaAttribute.ItemIndexName); }
            set { base.SetExtendedAttribute(OrganizationAreaAttribute.ItemIndexName, value); }
        }

        public int ParentID
        {
            get { return base.GetInt(OrganizationAreaAttribute.ParentID, 0); }
            set { base.SetExtendedAttribute(OrganizationAreaAttribute.ParentID, value.ToString()); }
        }

        public string ParentsPath
        {
            get { return base.GetExtendedAttribute(OrganizationAreaAttribute.ParentsPath); }
            set { base.SetExtendedAttribute(OrganizationAreaAttribute.ParentsPath, value); }
        }

        public int ParentsCount
        {
            get { return base.GetInt(OrganizationAreaAttribute.ParentsCount, 0); }
            set { base.SetExtendedAttribute(OrganizationAreaAttribute.ParentsCount, value.ToString()); }
        }


        public int ChildrenCount
        {
            get { return base.GetInt(OrganizationAreaAttribute.ChildrenCount, 0); }
            set { base.SetExtendedAttribute(OrganizationAreaAttribute.ChildrenCount, value.ToString()); }
        }


        public int ContentNum
        {
            get { return base.GetInt(OrganizationAreaAttribute.ContentNum, 0); }
            set { base.SetExtendedAttribute(OrganizationAreaAttribute.ContentNum, value.ToString()); }
        }

        public bool Enabled
        {
            get { return base.GetBool(OrganizationAreaAttribute.Enabled,false); }
            set { base.SetExtendedAttribute(OrganizationAreaAttribute.Enabled, value.ToString()); }
        }

        public bool IsLastItem
        {
            get { return base.GetBool(OrganizationAreaAttribute.IsLastItem, false); }
            set { base.SetExtendedAttribute(OrganizationAreaAttribute.IsLastItem, value.ToString()); }
        }


        protected override ArrayList GetDefaultAttributesNames()
        {
            return OrganizationAreaAttribute.OtherAttributes;
        }
    }
}
