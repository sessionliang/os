using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.CRM.Model
{
    public class OrderFormSEMAttribute
    {
        protected OrderFormSEMAttribute()
        {
        }

        //hidden
        public static string ID = "ID";
        public static string OrderID = "OrderID";
        public static string Domain = "Domain";
        public static string Title = "Title";
        public static string Keywords = "Keywords";
        public static string Description = "Description";
        public static string AddDate = "AddDate";

        public static ArrayList AllAttributes
        {
            get
            {
                ArrayList arraylist = new ArrayList(HiddenAttributes);
                arraylist.AddRange(BasicAttributes);
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
                }

                return hiddenAttributes;
            }
        }

        private static ArrayList basicAttributes;
        public static ArrayList BasicAttributes
        {
            get
            {
                if (basicAttributes == null)
                {
                    basicAttributes = new ArrayList();
                    basicAttributes.Add(Domain.ToLower());
                    basicAttributes.Add(Title.ToLower());
                    basicAttributes.Add(Keywords.ToLower());
                    basicAttributes.Add(Description.ToLower());
                    basicAttributes.Add(AddDate.ToLower());
                }

                return basicAttributes;
            }
        }
    }

    public class OrderFormSEMInfo : ExtendedAttributes
    {
        public const string TableName = "crm_OrderFormSEM";

        public OrderFormSEMInfo()
        {
            this.ID = 0;
            this.OrderID = 0;
        }

        public OrderFormSEMInfo(object dataItem)
            : base(dataItem)
        {
        }

        public OrderFormSEMInfo(int id, int orderID)
        {
            this.ID = id;
            this.OrderID = orderID;
        }

        public int ID
        {
            get { return base.GetInt(OrderFormSEMAttribute.ID, 0); }
            set { base.SetExtendedAttribute(OrderFormSEMAttribute.ID, value.ToString()); }
        }

        public int OrderID
        {
            get { return base.GetInt(OrderFormSEMAttribute.OrderID, 0); }
            set { base.SetExtendedAttribute(OrderFormSEMAttribute.OrderID, value.ToString()); }
        }

        public string Domain
        {
            get { return base.GetExtendedAttribute(OrderFormSEMAttribute.Domain); }
            set { base.SetExtendedAttribute(OrderFormSEMAttribute.Domain, value); }
        }

        public string Title
        {
            get { return base.GetExtendedAttribute(OrderFormSEMAttribute.Title); }
            set { base.SetExtendedAttribute(OrderFormSEMAttribute.Title, value); }
        }

        public string Keywords
        {
            get { return base.GetExtendedAttribute(OrderFormSEMAttribute.Keywords); }
            set { base.SetExtendedAttribute(OrderFormSEMAttribute.Keywords, value); }
        }

        public string Description
        {
            get { return base.GetExtendedAttribute(OrderFormSEMAttribute.Description); }
            set { base.SetExtendedAttribute(OrderFormSEMAttribute.Description, value); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(OrderFormSEMAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(OrderFormSEMAttribute.AddDate, value.ToString()); }
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return OrderFormSEMAttribute.AllAttributes;
        }
    }
}
