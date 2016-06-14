using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.Project.Model
{
    public class EmailAttribute
    {
        protected EmailAttribute()
        {
        }

        //hidden
        public const string ID = "ID";
        public const string AddDate = "AddDate";
        public const string AccountID = "AccountID";
        public const string ContactID = "ContactID";

        //basic
        public const string Email = "Email";
        public const string IsUnsubscribe = "IsUnsubscribe";

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
                    hiddenAttributes.Add(AddDate.ToLower());
                    hiddenAttributes.Add(AccountID.ToLower());
                    hiddenAttributes.Add(ContactID.ToLower());
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
                    basicAttributes.Add(Email.ToLower());
                    basicAttributes.Add(IsUnsubscribe.ToLower());
                }

                return basicAttributes;
            }
        }
    }

    public class EmailInfo : ExtendedAttributes
    {
        public const string TableName = "crm_Email";

        public EmailInfo()
        {
            this.ID = 0;
            this.AddDate = DateTime.Now;
            this.AccountID = 0;
            this.ContactID = 0;
        }

        public EmailInfo(object dataItem)
            : base(dataItem)
        {
        }

        public EmailInfo(int id, DateTime addDate, int accountID, int contactID)
        {
            this.ID = id;
            this.AddDate = DateTime.Now;
            this.AccountID = accountID;
            this.ContactID = contactID;
        }

        public int ID
        {
            get { return base.GetInt(EmailAttribute.ID, 0); }
            set { base.SetExtendedAttribute(EmailAttribute.ID, value.ToString()); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(EmailAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(EmailAttribute.AddDate, value.ToString()); }
        }

        public int AccountID
        {
            get { return base.GetInt(EmailAttribute.AccountID, 0); }
            set { base.SetExtendedAttribute(EmailAttribute.AccountID, value.ToString()); }
        }

        public int ContactID
        {
            get { return base.GetInt(EmailAttribute.ContactID, 0); }
            set { base.SetExtendedAttribute(EmailAttribute.ContactID, value.ToString()); }
        }

        public string Email
        {
            get { return base.GetExtendedAttribute(EmailAttribute.Email); }
            set { base.SetExtendedAttribute(EmailAttribute.Email, value); }
        }

        public bool IsUnsubscribe
        {
            get { return TranslateUtils.ToBool(base.GetExtendedAttribute(EmailAttribute.IsUnsubscribe)); }
            set { base.SetExtendedAttribute(EmailAttribute.IsUnsubscribe, value.ToString()); }
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return EmailAttribute.AllAttributes;
        }
    }
}
