using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.CRM.Model
{
    public class OrderFormAttribute
    {
        protected OrderFormAttribute()
        {
        }

        //hidden
        public static string ID = "ID";
        public static string OrderID = "OrderID";
        public static string MobanID = "MobanID";
        public static string IsCompleted = "IsCompleted";
        public static string CurrentPageID = "CurrentPageID";
        public static string AddDate = "AddDate";
        public static string SettingsXML = "SettingsXML";

        public static ArrayList AllAttributes
        {
            get
            {
                ArrayList arraylist = new ArrayList(HiddenAttributes);
                return arraylist;
            }
        }

        private static ArrayList hiddenAttributes;
        public static ArrayList HiddenAttributes
        {
            get
            {
                if (hiddenAttributes == null)
                {
                    hiddenAttributes = new ArrayList();
                    hiddenAttributes.Add(ID.ToLower());
                    hiddenAttributes.Add(OrderID.ToLower());
                    hiddenAttributes.Add(MobanID.ToLower());
                    hiddenAttributes.Add(IsCompleted.ToLower());
                    hiddenAttributes.Add(CurrentPageID.ToLower());
                    hiddenAttributes.Add(AddDate.ToLower());
                    hiddenAttributes.Add(SettingsXML.ToLower());
                }

                return hiddenAttributes;
            }
        }
    }

    public class OrderFormInfo : ExtendedAttributes
    {
        public const string TableName = "crm_OrderForm";

        public OrderFormInfo()
        {
            this.ID = 0;
            this.OrderID = 0;
            this.MobanID = 0;
            this.IsCompleted = false;
            this.CurrentPageID = 0;
            this.AddDate = DateTime.Now;
        }

        public OrderFormInfo(int id, int orderID, int mobanID, bool isCompleted, int currentPageID, DateTime addDate)
        {
            this.ID = id;
            this.OrderID = orderID;
            this.MobanID = mobanID;
            this.IsCompleted = isCompleted;
            this.CurrentPageID = currentPageID;
            this.AddDate = addDate;
        }

        public int ID
        {
            get { return base.GetInt(OrderFormAttribute.ID, 0); }
            set { base.SetExtendedAttribute(OrderFormAttribute.ID, value.ToString()); }
        }

        public int OrderID
        {
            get { return base.GetInt(OrderFormAttribute.OrderID, 0); }
            set { base.SetExtendedAttribute(OrderFormAttribute.OrderID, value.ToString()); }
        }

        public int MobanID
        {
            get { return base.GetInt(OrderFormAttribute.MobanID, 0); }
            set { base.SetExtendedAttribute(OrderFormAttribute.MobanID, value.ToString()); }
        }

        public bool IsCompleted
        {
            get { return base.GetBool(OrderFormAttribute.IsCompleted, false); }
            set { base.SetExtendedAttribute(OrderFormAttribute.IsCompleted, value.ToString()); }
        }

        public int CurrentPageID
        {
            get { return base.GetInt(OrderFormAttribute.CurrentPageID, 0); }
            set { base.SetExtendedAttribute(OrderFormAttribute.CurrentPageID, value.ToString()); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(OrderFormAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(OrderFormAttribute.AddDate, value.ToString()); }
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return OrderFormAttribute.AllAttributes;
        }
    }
}
