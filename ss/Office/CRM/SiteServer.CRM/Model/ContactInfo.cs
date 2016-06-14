using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace SiteServer.CRM.Model
{
    public class ContactAttribute
    {
        protected ContactAttribute()
        {
        }

        public const string ID = "ID";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string LoginName = "LoginName";
        public const string AddUserName = "AddUserName";
        public const string AddDate = "AddDate";
        public const string AccountID = "AccountID";
        public const string LeadID = "LeadID";

        public const string ContactName = "ContactName";
        public const string JobTitle = "JobTitle";
        public const string AccountRole = "AccountRole";
        public const string Mobile = "Mobile";
        public const string Telephone = "Telephone";
        public const string Email = "Email";
        public const string QQ = "QQ";
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
                    allAttributes.Add(LoginName);
                    allAttributes.Add(AddUserName);
                    allAttributes.Add(AddUserName);
                    allAttributes.Add(AddDate);
                    allAttributes.Add(AccountID);
                    allAttributes.Add(LeadID);
                    allAttributes.Add(ContactName);
                    allAttributes.Add(JobTitle);
                    allAttributes.Add(AccountRole);
                    allAttributes.Add(Mobile);
                    allAttributes.Add(Telephone);
                    allAttributes.Add(Email);
                    allAttributes.Add(QQ);
                    allAttributes.Add(ChatOrNote);
                }

                return allAttributes;
            }
        }
    }

    public class ContactInfo : BaseInfo
    {
        public ContactInfo() { }
        public ContactInfo(object dataItem) : base(dataItem) { }
        public ContactInfo(NameValueCollection form) : base(form) { }
        public ContactInfo(IDataReader rdr) : base(rdr) { }

        public int PublishmentSystemID { get; set; }

        public string LoginName { get; set; }

        public string AddUserName { get; set; }

        public DateTime AddDate { get; set; }

        public int AccountID { get; set; }

        public int LeadID { get; set; }

        public string ContactName { get; set; }

        public string JobTitle { get; set; }

        public string AccountRole { get; set; }

        public string Mobile { get; set; }

        public string Telephone { get; set; }

        public string Email { get; set; }

        public string QQ { get; set; }

        public string ChatOrNote { get; set; }

        protected override List<string> AllAttributes
        {
            get
            {
                return ContactAttribute.AllAttributes;
            }
        }
    }
}
