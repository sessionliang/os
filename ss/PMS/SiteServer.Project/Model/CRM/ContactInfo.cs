using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.Project.Model
{
    public class ContactAttribute
    {
        protected ContactAttribute()
        {
        }

        //hidden
        public const string ID = "ID";
        public const string LoginName = "LoginName";
        public const string AddUserName = "AddUserName";
        public const string AddDate = "AddDate";
        public const string AccountID = "AccountID";
        public const string LeadID = "LeadID";

        //basic
        public const string ContactName = "ContactName";
        public const string JobTitle = "JobTitle";
        public const string AccountRole = "AccountRole";
        public const string Mobile = "Mobile";
        public const string Telephone = "Telephone";
        public const string Email = "Email";
        public const string QQ = "QQ";
        public const string ChatOrNote = "ChatOrNote";

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
                    hiddenAttributes.Add(LoginName.ToLower());
                    hiddenAttributes.Add(AddUserName.ToLower());
                    hiddenAttributes.Add(AddDate.ToLower());
                    hiddenAttributes.Add(AccountID.ToLower());
                    hiddenAttributes.Add(LeadID.ToLower());
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
                    basicAttributes.Add(ContactName.ToLower());
                    basicAttributes.Add(JobTitle.ToLower());
                    basicAttributes.Add(AccountRole.ToLower());
                    basicAttributes.Add(Mobile.ToLower());
                    basicAttributes.Add(Telephone.ToLower());
                    basicAttributes.Add(Email.ToLower());
                    basicAttributes.Add(QQ.ToLower());
                    basicAttributes.Add(ChatOrNote.ToLower());
                }

                return basicAttributes;
            }
        }
    }

    public class ContactInfo : ExtendedAttributes
    {
        public const string TableName = "crm_Contact";

        public ContactInfo()
        {
            this.ID = 0;
            this.LoginName = string.Empty;
            this.AddUserName = string.Empty;
            this.AddDate = DateTime.Now;
            this.AccountID = 0;
            this.LeadID = 0;
        }

        public ContactInfo(object dataItem)
            : base(dataItem)
        {
        }

        public ContactInfo(int id, string loginName, string addUserName, DateTime addDate, int accountID, int leadID)
        {
            this.ID = id;
            this.LoginName = loginName;
            this.AddUserName = addUserName;
            this.AddDate = DateTime.Now;
            this.AccountID = accountID;
            this.LeadID = leadID;
        }

        public int ID
        {
            get { return base.GetInt(ContactAttribute.ID, 0); }
            set { base.SetExtendedAttribute(ContactAttribute.ID, value.ToString()); }
        }

        public string LoginName
        {
            get { return base.GetExtendedAttribute(ContactAttribute.LoginName); }
            set { base.SetExtendedAttribute(ContactAttribute.LoginName, value); }
        }

        public string AddUserName
        {
            get { return base.GetExtendedAttribute(ContactAttribute.AddUserName); }
            set { base.SetExtendedAttribute(ContactAttribute.AddUserName, value); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(ContactAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(ContactAttribute.AddDate, value.ToString()); }
        }

        public int AccountID
        {
            get { return base.GetInt(ContactAttribute.AccountID, 0); }
            set { base.SetExtendedAttribute(ContactAttribute.AccountID, value.ToString()); }
        }

        public int LeadID
        {
            get { return base.GetInt(ContactAttribute.LeadID, 0); }
            set { base.SetExtendedAttribute(ContactAttribute.LeadID, value.ToString()); }
        }

        public string ContactName
        {
            get { return base.GetExtendedAttribute(ContactAttribute.ContactName); }
            set { base.SetExtendedAttribute(ContactAttribute.ContactName, value); }
        }

        public string JobTitle
        {
            get { return base.GetExtendedAttribute(ContactAttribute.JobTitle); }
            set { base.SetExtendedAttribute(ContactAttribute.JobTitle, value); }
        }

        public string AccountRole
        {
            get { return base.GetExtendedAttribute(ContactAttribute.AccountRole); }
            set { base.SetExtendedAttribute(ContactAttribute.AccountRole, value); }
        }

        public string Mobile
        {
            get { return base.GetExtendedAttribute(ContactAttribute.Mobile); }
            set { base.SetExtendedAttribute(ContactAttribute.Mobile, value); }
        }

        public string Telephone
        {
            get { return base.GetExtendedAttribute(ContactAttribute.Telephone); }
            set { base.SetExtendedAttribute(ContactAttribute.Telephone, value); }
        }

        public string Email
        {
            get { return base.GetExtendedAttribute(ContactAttribute.Email); }
            set { base.SetExtendedAttribute(ContactAttribute.Email, value); }
        }

        public string QQ
        {
            get { return base.GetExtendedAttribute(ContactAttribute.QQ); }
            set { base.SetExtendedAttribute(ContactAttribute.QQ, value); }
        }

        public string ChatOrNote
        {
            get { return base.GetExtendedAttribute(ContactAttribute.ChatOrNote); }
            set { base.SetExtendedAttribute(ContactAttribute.ChatOrNote, value); }
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return ContactAttribute.AllAttributes;
        }
    }
}
