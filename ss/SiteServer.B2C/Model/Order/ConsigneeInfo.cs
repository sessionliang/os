using BaiRong.Core;
using BaiRong.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;

namespace SiteServer.B2C.Model
{
    public class ConsigneeAttribute
    {
        protected ConsigneeAttribute()
        {
        }

        public const string ID = "ID";
        public const string GroupSN = "GroupSN";
        public const string UserName = "UserName";
        public const string IsOrder = "IsOrder";
        public const string IPAddress = "IPAddress";
        public const string IsDefault = "IsDefault";
        public const string Consignee = "Consignee";
        public const string Country = "Country";
        public const string Province = "Province";
        public const string City = "City";
        public const string Area = "Area";
        public const string Address = "Address";
        public const string Zipcode = "Zipcode";
        public const string Mobile = "Mobile";
        public const string Tel = "Tel";
        public const string Email = "Email";

        private static List<string> allAttributes;
        public static List<string> AllAttributes
        {
            get
            {
                if (allAttributes == null)
                {
                    allAttributes = new List<string>();
                    allAttributes.Add(ID);
                    allAttributes.Add(GroupSN);
                    allAttributes.Add(UserName);
                    allAttributes.Add(IsOrder);
                    allAttributes.Add(IPAddress);
                    allAttributes.Add(IsDefault);
                    allAttributes.Add(Consignee);
                    allAttributes.Add(Country);
                    allAttributes.Add(Province);
                    allAttributes.Add(City);
                    allAttributes.Add(Area);
                    allAttributes.Add(Address);
                    allAttributes.Add(Zipcode);
                    allAttributes.Add(Mobile);
                    allAttributes.Add(Tel);
                    allAttributes.Add(Email);
                }

                return allAttributes;
            }
        }
    }
    public class ConsigneeInfo : BaseInfo
    {
        public ConsigneeInfo() { }
        public ConsigneeInfo(object dataItem) : base(dataItem) { }
        public ConsigneeInfo(NameValueCollection form) : base(form) { }
        public ConsigneeInfo(IDataReader rdr) : base(rdr) { }
        public string GroupSN { get; set; }
        public string UserName { get; set; }
        public bool IsOrder { get; set; }
        public string IPAddress { get; set; }
        public bool IsDefault { get; set; }
        public string Consignee { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Address { get; set; }
        public string Zipcode { get; set; }
        public string Mobile { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }

        protected override List<string> AllAttributes
        {
            get
            {
                return ConsigneeAttribute.AllAttributes;
            }
        }
    }
}
