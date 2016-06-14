using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;
using System.Collections.Generic;

namespace SiteServer.Project.Model
{
    public class TouchAttribute
    {
        protected TouchAttribute()
        {
        }

        public const string ID = "ID";
        public const string AddUserName = "AddUserName";
        public const string AddDate = "AddDate";
        public const string LeadID = "LeadID";
        public const string OrderID = "OrderID";
        public const string MessageID = "MessageID";
        public const string TouchBy = "TouchBy";
        public const string ContactName = "ContactName";
        public const string Summary = "Summary";

        private static List<string> allAttributes;
        public static List<string> AllAttributes
        {
            get
            {
                if (allAttributes == null)
                {
                    allAttributes = new List<string>();
                    allAttributes.Add(ID);
                    allAttributes.Add(AddUserName);
                    allAttributes.Add(AddDate);
                    allAttributes.Add(LeadID);
                    allAttributes.Add(OrderID);
                    allAttributes.Add(MessageID);
                    allAttributes.Add(TouchBy);
                    allAttributes.Add(ContactName);
                    allAttributes.Add(Summary);
                }

                return allAttributes;
            }
        }
    }
    public class TouchInfo : BaseInfo
    {
        public string AddUserName { get; set; }
        public DateTime AddDate { get; set; }
        public int LeadID { get; set; }
        public int OrderID { get; set; }
        public int MessageID { get; set; }
        public string TouchBy { get; set; }
        public string ContactName { get; set; }
        public string Summary { get; set; }

        protected override List<string> AllAttributes
        {
            get
            {
                return TouchAttribute.AllAttributes;
            }
        }
    }
}
