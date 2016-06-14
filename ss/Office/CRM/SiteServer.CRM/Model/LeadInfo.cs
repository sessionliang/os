using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace SiteServer.CRM.Model
{
    public class LeadAttribute
    {
        protected LeadAttribute()
        {
        }


        public const string ID = "ID";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string AddUserName = "AddUserName";        
        public const string AddDate = "AddDate";
        public const string Status = "Status";
        public const string ChargeUserName = "ChargeUserName";
        public const string Subject = "Subject";
        public const string Source = "Source";
        public const string Priority = "Priority";
        public const string Possibility = "Possibility";
        public const string IsAccount = "IsAccount";
        public const string AccountID = "AccountID";
        public const string AccountName = "AccountName";
        public const string BusinessType = "BusinessType";
        public const string Website = "Website";
        public const string Province = "Province";
        public const string City = "City";
        public const string Area = "Area";
        public const string Address = "Address";
        public const string IsContact = "IsContact";
        public const string ContactName = "ContactName";
        public const string JobTitle = "JobTitle";
        public const string AccountRole = "AccountRole";
        public const string Mobile = "Mobile";
        public const string Telephone = "Telephone";
        public const string Email = "Email";
        public const string QQ = "QQ";
        public const string BackgroundInfo = "BackgroundInfo";
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
                    allAttributes.Add(AddUserName);
                    allAttributes.Add(AddDate);
                    allAttributes.Add(Status);
                    allAttributes.Add(ChargeUserName);
                    allAttributes.Add(Subject);
                    allAttributes.Add(Source);
                    allAttributes.Add(Priority);
                    allAttributes.Add(Possibility);

                    allAttributes.Add(IsAccount);
                    allAttributes.Add(AccountID);
                    allAttributes.Add(AccountName);
                    allAttributes.Add(BusinessType);
                    allAttributes.Add(Website);
                    allAttributes.Add(Province);
                    allAttributes.Add(City);
                    allAttributes.Add(Area);
                    allAttributes.Add(Address);

                    allAttributes.Add(IsContact);
                    allAttributes.Add(ContactName);
                    allAttributes.Add(JobTitle);
                    allAttributes.Add(AccountRole);
                    allAttributes.Add(Mobile);
                    allAttributes.Add(Telephone);
                    allAttributes.Add(Email);
                    allAttributes.Add(QQ);

                    allAttributes.Add(BackgroundInfo);
                    allAttributes.Add(ChatOrNote);
                }

                return allAttributes;
            }
        }
    }

    public class LeadInfo : BaseInfo
    {
        public LeadInfo() { }
        public LeadInfo(object dataItem) : base(dataItem) { }
        public LeadInfo(NameValueCollection form) : base(form) { }
        public LeadInfo(IDataReader rdr) : base(rdr) { }

        public int PublishmentSystemID { get; set; }

        public string AddUserName { get; set; }

        public DateTime AddDate { get; set; }

        public string Status { get; set; }

        public string ChargeUserName { get; set; }

        public string Subject { get; set; }

        public string Source { get; set; }

        public int Priority { get; set; }

        public int Possibility { get; set; }

        public bool IsAccount { get; set; }

        public int AccountID { get; set; }

        public string AccountName { get; set; }

        public string BusinessType { get; set; }

        public string Website { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string Area { get; set; }

        public string Address { get; set; }

        public bool IsContact { get; set; }

        public string ContactName { get; set; }

        public string JobTitle { get; set; }

        public string AccountRole { get; set; }

        public string Mobile { get; set; }

        public string Telephone { get; set; }

        public string Email { get; set; }

        public string QQ { get; set; }

        public string BackgroundInfo { get; set; }

        public string ChatOrNote { get; set; }

        protected override List<string> AllAttributes
        {
            get
            {
                return LeadAttribute.AllAttributes;
            }
        }
    }
}
