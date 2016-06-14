using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;
using SiteServer.CMS.Controls;

namespace SiteServer.CMS.Model
{
    public class SubscribeAttribute
    {
        protected SubscribeAttribute()
        {
        }
         
        public const string ItemID = "ItemID";
        public const string ItemName = "ItemName";
        public const string SubscribeValue = "SubscribeValue";
        public const string Taxis = "Taxis"; 
        public const string ContentType = "ContentType";
        public const string SubscribeNum = "SubscribeNum";
        public const string IPAddress = "IPAddress";
        public const string AddDate = "AddDate";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string UserName = "UserName";
        public const string ItemIndexName = "ItemIndexName";
        public const string ParentID = "ParentID";
        public const string ParentsPath = "ParentsPath";
        public const string ParentsCount = "ParentsCount";
        public const string ChildrenCount = "ChildrenCount";
        public const string ContentNum = "ContentNum";
        public const string Enabled = "Enabled";
        public const string IsLastItem = "IsLastItem";

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
                    otherAttributes.Add(SubscribeValue.ToLower());
                    otherAttributes.Add(Taxis.ToLower());
                    otherAttributes.Add(ContentType.ToLower());
                    otherAttributes.Add(SubscribeNum.ToLower());
                    otherAttributes.Add(IPAddress.ToLower());
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

    public class SubscribeInfo : TreeBaseItem
    {
        public const string TableName = "siteserver_Subscribe";

        public SubscribeInfo()
        {
            this.ItemID = 0;
            this.ItemName = string.Empty;
            this.SubscribeValue = string.Empty;
            this.Taxis = 0;
            this.SubscribeNum = 0;
            this.ContentType = ESubscribeContentTypeUtils.GetValue( ESubscribeContentType.Label);
            this.IPAddress = string.Empty;
            this.AddDate = DateTime.Now;
            this.PublishmentSystemID = 0;
            this.UserName = string.Empty;
            this.ItemIndexName = string.Empty;
            this.ParentID = 0;
            this.ParentsPath = string.Empty;
            this.ParentsCount = 0;
            this.ChildrenCount = 0;
            this.ContentNum = 0;
            this.Enabled = EBoolean.True.ToString();
            this.IsLastItem = string.Empty;
        }

        public SubscribeInfo(object dataItem)
            : base(dataItem)
		{
		}


        public SubscribeInfo(int itemID, string itemName, string subscribeValue, int taxis, int subscribeNum, ESubscribeContentType contentType, string iPAddress, DateTime addDate, int publishmentSystemID, string userName ) 
        {
            this.ItemID = itemID;
            this.ItemName = itemName;
            this.SubscribeValue = subscribeValue;
            this.Taxis = taxis;
            this.SubscribeNum = subscribeNum;
            this.ContentType = ESubscribeContentTypeUtils.GetValue(contentType);
            this.IPAddress = iPAddress;
            this.AddDate = addDate;
            this.PublishmentSystemID = publishmentSystemID;
            this.UserName = userName;
        }

        public SubscribeInfo(int itemID, string itemName, string subscribeValue, int taxis,int subscribeNum, ESubscribeContentType contentType,string iPAddress,DateTime addDate,int publishmentSystemID,string userName, string itemIndexName, int parentID, string parentsPath, int parentsCount, int childrenCount, int contentNum,  bool enabled, bool isLastItem):   base(itemID, itemName, itemIndexName, parentID, parentsPath, parentsCount, childrenCount, contentNum, publishmentSystemID, enabled, isLastItem, taxis)
        {
            this.ItemID = itemID;
            this.ItemName = itemName;
            this.SubscribeValue = subscribeValue;
            this.Taxis = taxis;
            this.SubscribeNum = subscribeNum;
            this.ContentType = ESubscribeContentTypeUtils.GetValue( contentType);
            this.IPAddress = iPAddress;
            this.AddDate = addDate;
            this.PublishmentSystemID = publishmentSystemID;
            this.UserName = userName;
        }

        public int ItemID
        {
            get { return base.GetInt(SubscribeAttribute.ItemID, 0); }
            set { base.SetExtendedAttribute(SubscribeAttribute.ItemID, value.ToString()); }
        }

        public string ItemName
        {
            get { return base.GetExtendedAttribute(SubscribeAttribute.ItemName); }
            set { base.SetExtendedAttribute(SubscribeAttribute.ItemName, value); }
        }

        public string SubscribeValue
        {
            get { return base.GetExtendedAttribute(SubscribeAttribute.SubscribeValue); }
            set { base.SetExtendedAttribute(SubscribeAttribute.SubscribeValue, value); }
        }

        public new int Taxis
        {
            get { return base.GetInt(SubscribeAttribute.Taxis, 0); }
            set { base.SetExtendedAttribute(SubscribeAttribute.Taxis, value.ToString()); }
        }

        public string ContentType
        {
            get { return base.GetExtendedAttribute(SubscribeAttribute.ContentType); }
            set { base.SetExtendedAttribute(SubscribeAttribute.ContentType, value); }
        }

        public int SubscribeNum
        {
            get { return base.GetInt(SubscribeAttribute.SubscribeNum,0); }
            set { base.SetExtendedAttribute(SubscribeAttribute.SubscribeNum, value.ToString()); }
        }

        public string IPAddress
        {
            get { return base.GetExtendedAttribute(SubscribeAttribute.IPAddress); }
            set { base.SetExtendedAttribute(SubscribeAttribute.IPAddress, value); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(SubscribeSetAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(SubscribeSetAttribute.AddDate, value.ToString()); }
        }

        public int PublishmentSystemID
        {
            get { return base.GetInt(SubscribeAttribute.PublishmentSystemID, 0); }
            set { base.SetExtendedAttribute(SubscribeAttribute.PublishmentSystemID, value.ToString()); }
        }

        public string UserName
        {
            get { return base.GetExtendedAttribute(SubscribeAttribute.UserName); }
            set { base.SetExtendedAttribute(SubscribeAttribute.UserName, value); }
        }

        public string ItemIndexName
        {
            get { return base.GetExtendedAttribute(SubscribeAttribute.ItemIndexName); }
            set { base.SetExtendedAttribute(SubscribeAttribute.ItemIndexName, value); }
        }

        public int ParentID
        {
            get { return base.GetInt(SubscribeAttribute.ParentID, 0); }
            set { base.SetExtendedAttribute(SubscribeAttribute.ParentID, value.ToString()); }
        }

        public string ParentsPath
        {
            get { return base.GetExtendedAttribute(SubscribeAttribute.ParentsPath); }
            set { base.SetExtendedAttribute(SubscribeAttribute.ParentsPath, value); }
        }

        public int ParentsCount
        {
            get { return base.GetInt(SubscribeAttribute.ParentsCount, 0); }
            set { base.SetExtendedAttribute(SubscribeAttribute.ParentsCount, value.ToString()); }
        }


        public int ChildrenCount
        {
            get { return base.GetInt(SubscribeAttribute.ChildrenCount, 0); }
            set { base.SetExtendedAttribute(SubscribeAttribute.ChildrenCount, value.ToString()); }
        }


        public int ContentNum
        {
            get { return base.GetInt(SubscribeAttribute.ContentNum, 0); }
            set { base.SetExtendedAttribute(SubscribeAttribute.ContentNum, value.ToString()); }
        }

        public string Enabled
        {
            get { return base.GetExtendedAttribute(SubscribeAttribute.Enabled); }
            set { base.SetExtendedAttribute(SubscribeAttribute.Enabled, value); }
        }

        public string IsLastItem
        {
            get { return base.GetExtendedAttribute(SubscribeAttribute.IsLastItem); }
            set { base.SetExtendedAttribute(SubscribeAttribute.IsLastItem, value); }
        }


        protected override ArrayList GetDefaultAttributesNames()
        {
            return SubscribeAttribute.OtherAttributes;
        }
    }
}
