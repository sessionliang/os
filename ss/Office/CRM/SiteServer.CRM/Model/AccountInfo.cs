using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;
using System.Data;
using System.Collections.Generic;

namespace SiteServer.CRM.Model
{
    public class AccountAttribute
    {
        protected AccountAttribute()
        {
        }

        public const string ID = "ID";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string SN = "SN";
        public const string AccountType = "AccountType";
        public const string AddUserName = "AddUserName";
        public const string AddDate = "AddDate";
        public const string Status = "Status";
        public const string ChargeUserName = "ChargeUserName";
        public const string AccountName = "AccountName";
        public const string BusinessType = "BusinessType";
        public const string Classification = "Classification";
        public const string Priority = "Priority";
        public const string Website = "Website";
        public const string Telephone = "Telephone";
        public const string Province = "Province";
        public const string City = "City";
        public const string Area = "Area";
        public const string Address = "Address";
        public const string Description = "Description";
        public const string ChatOrNote = "ChatOrNote";

        private static List<string> allAttributes;
        public static List<string> AllAttributes
        {
            get
            {
                if (allAttributes == null)
                {
                    allAttributes = new List<string>();
                    allAttributes.Add(ID);
                    allAttributes.Add(PublishmentSystemID);
                    allAttributes.Add(SN);
                    allAttributes.Add(AccountType);
                    allAttributes.Add(AddUserName);
                    allAttributes.Add(AddDate);
                    allAttributes.Add(Status);
                    allAttributes.Add(ChargeUserName);
                    allAttributes.Add(AccountName);
                    allAttributes.Add(BusinessType);
                    allAttributes.Add(Classification);
                    allAttributes.Add(Priority);
                    allAttributes.Add(Website);
                    allAttributes.Add(Telephone);
                    allAttributes.Add(Province);
                    allAttributes.Add(City);
                    allAttributes.Add(Area);
                    allAttributes.Add(Address);
                    allAttributes.Add(Description);
                    allAttributes.Add(ChatOrNote);
                }

                return allAttributes;
            }
        }
    }

    public class AccountInfo : BaseInfo
    {
        public AccountInfo() { }
        public AccountInfo(object dataItem) : base(dataItem) { }
        public AccountInfo(NameValueCollection form) : base(form) { }
        public AccountInfo(IDataReader rdr) : base(rdr) { }

        public int PublishmentSystemID { get; set; }

        public string SN { get; set; }

        public string AccountType { get; set; }

        public string AddUserName { get; set; }

        public DateTime AddDate { get; set; }

        public string Status { get; set; }

        public string ChargeUserName { get; set; }

        public string AccountName { get; set; }

        public string BusinessType { get; set; }

        public string Classification { get; set; }

        public int Priority { get; set; }

        public string Website { get; set; }

        public string Telephone { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string Area { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public string ChatOrNote { get; set; }

        protected override List<string> AllAttributes
        {
            get
            {
                return AccountAttribute.AllAttributes;
            }
        }
    }
}
