using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;
using SiteServer.CMS.Controls;

namespace SiteServer.CMS.Model
{
    public class UserNewGroupAttribute
    {
        protected UserNewGroupAttribute()
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
        public const string GroupType = "GroupType";
        public const string Enabled = "Enabled";
        public const string IsLastItem = "IsLastItem";
        public const string Taxis = "Taxis";
        public const string AddDate = "AddDate";
        public const string UserName = "UserName";
        public const string Description = "Description";
        public const string SetXML = "SetXML";

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
                    otherAttributes.Add(GroupType.ToLower());
                    otherAttributes.Add(UserName.ToLower());
                    otherAttributes.Add(ClassifyID.ToLower());
                    otherAttributes.Add(ParentID.ToLower());
                    otherAttributes.Add(ParentsPath.ToLower());
                    otherAttributes.Add(ParentsCount.ToLower());
                    otherAttributes.Add(ChildrenCount.ToLower());
                    otherAttributes.Add(ContentNum.ToLower());
                    otherAttributes.Add(Enabled.ToLower());
                    otherAttributes.Add(IsLastItem.ToLower());
                    otherAttributes.Add(Description.ToLower());
                    otherAttributes.Add(SetXML.ToLower());
                }
                return otherAttributes;
            }
        }

    }

    public class UserNewGroupInfo : TreeBaseItem
    {
        public const string TableName = "bairong_UserNewGroup";

        public UserNewGroupInfo()
        {
            this.ItemID = 0;
            this.ItemName = string.Empty;
            this.Taxis = 0;
            this.AddDate = DateTime.Now;
            this.ClassifyID = 0;
            this.GroupType = string.Empty;
            this.UserName = string.Empty;
            this.ItemIndexName = string.Empty;
            this.ParentID = 0;
            this.ParentsPath = string.Empty;
            this.ParentsCount = 0;
            this.ChildrenCount = 0;
            this.ContentNum = 0;
            this.Enabled = true;
            this.IsLastItem = false;
            this.Description = string.Empty;
            this.SetXML = string.Empty;
        }

        public UserNewGroupInfo(object dataItem)
            : base(dataItem)
        {
        }


        public UserNewGroupInfo(int itemID, string itemName, string itemIndexName, int parentID, string parentsPath, int parentsCount, int childrenCount, int contentNum, int classifyID, string groupType, int publishmentSystemID, bool enabled, bool isLastItem, int taxis, DateTime addDate, string userName, string description, string setXML)
            : base(itemID, itemName, itemIndexName, parentID, parentsPath, parentsCount, childrenCount, contentNum, publishmentSystemID, enabled, isLastItem, taxis)
        {
            this.ItemID = itemID;
            this.ParentID = parentID;
            this.ParentsPath = parentsPath;
            this.ParentsCount = parentsCount;
            this.ChildrenCount = childrenCount;
            this.GroupType = groupType;
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
            this.Description = description;
            this.SetXML = setXML;
        }

        public int ItemID
        {
            get { return base.GetInt(UserNewGroupAttribute.ItemID, 0); }
            set { base.SetExtendedAttribute(UserNewGroupAttribute.ItemID, value.ToString()); }
        }

        public string ItemName
        {
            get { return base.GetExtendedAttribute(UserNewGroupAttribute.ItemName); }
            set { base.SetExtendedAttribute(UserNewGroupAttribute.ItemName, value); }
        }

        public int ClassifyID
        {
            get { return base.GetInt(UserNewGroupAttribute.ClassifyID, 0); }
            set { base.SetExtendedAttribute(UserNewGroupAttribute.ClassifyID, value.ToString()); }
        }

        public new int Taxis
        {
            get { return base.GetInt(UserNewGroupAttribute.Taxis, 0); }
            set { base.SetExtendedAttribute(UserNewGroupAttribute.Taxis, value.ToString()); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(UserNewGroupAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(UserNewGroupAttribute.AddDate, value.ToString()); }
        }

        public string GroupType
        {
            get { return base.GetExtendedAttribute(UserNewGroupAttribute.GroupType); }
            set { base.SetExtendedAttribute(UserNewGroupAttribute.GroupType, value); }
        }

        public string UserName
        {
            get { return base.GetExtendedAttribute(UserNewGroupAttribute.UserName); }
            set { base.SetExtendedAttribute(UserNewGroupAttribute.UserName, value); }
        }

        public string ItemIndexName
        {
            get { return base.GetExtendedAttribute(UserNewGroupAttribute.ItemIndexName); }
            set { base.SetExtendedAttribute(UserNewGroupAttribute.ItemIndexName, value); }
        }

        public int ParentID
        {
            get { return base.GetInt(UserNewGroupAttribute.ParentID, 0); }
            set { base.SetExtendedAttribute(UserNewGroupAttribute.ParentID, value.ToString()); }
        }

        public string ParentsPath
        {
            get { return base.GetExtendedAttribute(UserNewGroupAttribute.ParentsPath); }
            set { base.SetExtendedAttribute(UserNewGroupAttribute.ParentsPath, value); }
        }

        public int ParentsCount
        {
            get { return base.GetInt(UserNewGroupAttribute.ParentsCount, 0); }
            set { base.SetExtendedAttribute(UserNewGroupAttribute.ParentsCount, value.ToString()); }
        }


        public int ChildrenCount
        {
            get { return base.GetInt(UserNewGroupAttribute.ChildrenCount, 0); }
            set { base.SetExtendedAttribute(UserNewGroupAttribute.ChildrenCount, value.ToString()); }
        }


        public int ContentNum
        {
            get { return base.GetInt(UserNewGroupAttribute.ContentNum, 0); }
            set { base.SetExtendedAttribute(UserNewGroupAttribute.ContentNum, value.ToString()); }
        }

        public bool Enabled
        {
            get { return base.GetBool(UserNewGroupAttribute.Enabled, false); }
            set { base.SetExtendedAttribute(UserNewGroupAttribute.Enabled, value.ToString()); }
        }

        public bool IsLastItem
        {
            get { return base.GetBool(UserNewGroupAttribute.IsLastItem, false); }
            set { base.SetExtendedAttribute(UserNewGroupAttribute.IsLastItem, value.ToString()); }
        }

        public string Description
        {
            get { return base.GetExtendedAttribute(UserNewGroupAttribute.Description); }
            set { base.SetExtendedAttribute(UserNewGroupAttribute.Description, value); }
        }
        public string SetXML
        {
            get { return base.GetExtendedAttribute(UserNewGroupAttribute.SetXML); }
            set { base.SetExtendedAttribute(UserNewGroupAttribute.SetXML, value); }
        }


        protected override ArrayList GetDefaultAttributesNames()
        {
            return UserNewGroupAttribute.OtherAttributes;
        }


        UserNewGroupInfoExtend additional;
        public UserNewGroupInfoExtend Additional
        {
            get
            {
                if (this.additional == null)
                {
                    this.additional = new UserNewGroupInfoExtend(base.GetExtendedAttribute(UserNewGroupAttribute.SetXML));
                }
                return this.additional;
            }
        }
    }
}
