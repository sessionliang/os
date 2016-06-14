using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.Project.Model
{
    public class LeadAttribute
    {
        protected LeadAttribute()
        {
        }

        //hidden
        public const string ID = "ID";
        public const string AddUserName = "AddUserName";        
        public const string AddDate = "AddDate";

        //basic
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
                    hiddenAttributes.Add(AddUserName.ToLower());
                    hiddenAttributes.Add(AddDate.ToLower());
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
                    basicAttributes.Add(Status.ToLower());
                    basicAttributes.Add(ChargeUserName.ToLower());
                    basicAttributes.Add(Subject.ToLower());
                    basicAttributes.Add(Source.ToLower());
                    basicAttributes.Add(Priority.ToLower());
                    basicAttributes.Add(Possibility.ToLower());

                    basicAttributes.Add(IsAccount.ToLower());
                    basicAttributes.Add(AccountID.ToLower());
                    basicAttributes.Add(AccountName.ToLower());
                    basicAttributes.Add(BusinessType.ToLower());
                    basicAttributes.Add(Website.ToLower());
                    basicAttributes.Add(Province.ToLower());
                    basicAttributes.Add(City.ToLower());
                    basicAttributes.Add(Area.ToLower());
                    basicAttributes.Add(Address.ToLower());

                    basicAttributes.Add(IsContact.ToLower());
                    basicAttributes.Add(ContactName.ToLower());
                    basicAttributes.Add(JobTitle.ToLower());
                    basicAttributes.Add(AccountRole.ToLower());
                    basicAttributes.Add(Mobile.ToLower());
                    basicAttributes.Add(Telephone.ToLower());
                    basicAttributes.Add(Email.ToLower());
                    basicAttributes.Add(QQ.ToLower());
                                
                    basicAttributes.Add(BackgroundInfo.ToLower());
                    basicAttributes.Add(ChatOrNote.ToLower());
                }

                return basicAttributes;
            }
        }
    }

    public class LeadInfo : ExtendedAttributes
    {
        public const string TableName = "crm_Lead";

        public LeadInfo()
        {
            this.ID = 0;
            this.AddUserName = string.Empty;
            this.AddDate = DateTime.Now;
        }

        public LeadInfo(object dataItem)
            : base(dataItem)
        {
        }

        public LeadInfo(int id, string addUserName, DateTime addDate)
        {
            this.ID = id;
            this.AddUserName = addUserName;
            this.AddDate = addDate;
        }

        public int ID
        {
            get { return base.GetInt(LeadAttribute.ID, 0); }
            set { base.SetExtendedAttribute(LeadAttribute.ID, value.ToString()); }
        }

        public string AddUserName
        {
            get { return base.GetExtendedAttribute(LeadAttribute.AddUserName); }
            set { base.SetExtendedAttribute(LeadAttribute.AddUserName, value); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(LeadAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(LeadAttribute.AddDate, value.ToString()); }
        }

        public ELeadStatus Status
        {
            get { return ELeadStatusUtils.GetEnumType(base.GetExtendedAttribute(LeadAttribute.Status)); }
            set { base.SetExtendedAttribute(LeadAttribute.Status, ELeadStatusUtils.GetValue(value)); }
        }

        public string ChargeUserName
        {
            get { return base.GetExtendedAttribute(LeadAttribute.ChargeUserName); }
            set { base.SetExtendedAttribute(LeadAttribute.ChargeUserName, value); }
        }

        public string Subject
        {
            get { return base.GetExtendedAttribute(LeadAttribute.Subject); }
            set { base.SetExtendedAttribute(LeadAttribute.Subject, value); }
        }

        public string Source
        {
            get { return base.GetExtendedAttribute(LeadAttribute.Source); }
            set { base.SetExtendedAttribute(LeadAttribute.Source, value); }
        }

        public int Priority
        {
            get { return base.GetInt(LeadAttribute.Priority, 0); }
            set { base.SetExtendedAttribute(LeadAttribute.Priority, value.ToString()); }
        }

        public int Possibility
        {
            get
            {
                int retval = base.GetInt(LeadAttribute.Possibility, 0);
                if (retval > 100) retval = 100;
                return retval;
            }
            set { base.SetExtendedAttribute(LeadAttribute.Possibility, value.ToString()); }
        }

        public bool IsAccount
        {
            get { return base.GetBool(LeadAttribute.IsAccount, false); }
            set { base.SetExtendedAttribute(LeadAttribute.IsAccount, value.ToString()); }
        }

        public int AccountID
        {
            get { return base.GetInt(LeadAttribute.AccountID, 0); }
            set { base.SetExtendedAttribute(LeadAttribute.AccountID, value.ToString()); }
        }

        public string AccountName
        {
            get { return base.GetExtendedAttribute(LeadAttribute.AccountName); }
            set { base.SetExtendedAttribute(LeadAttribute.AccountName, value); }
        }

        public string BusinessType
        {
            get { return base.GetExtendedAttribute(LeadAttribute.BusinessType); }
            set { base.SetExtendedAttribute(LeadAttribute.BusinessType, value); }
        }

        public string Website
        {
            get { return base.GetExtendedAttribute(LeadAttribute.Website); }
            set { base.SetExtendedAttribute(LeadAttribute.Website, value); }
        }

        public string Province
        {
            get { return base.GetExtendedAttribute(LeadAttribute.Province); }
            set { base.SetExtendedAttribute(LeadAttribute.Province, value); }
        }

        public string City
        {
            get { return base.GetExtendedAttribute(LeadAttribute.City); }
            set { base.SetExtendedAttribute(LeadAttribute.City, value); }
        }

        public string Area
        {
            get { return base.GetExtendedAttribute(LeadAttribute.Area); }
            set { base.SetExtendedAttribute(LeadAttribute.Area, value); }
        }

        public string Address
        {
            get { return base.GetExtendedAttribute(LeadAttribute.Address); }
            set { base.SetExtendedAttribute(LeadAttribute.Address, value); }
        }

        public bool IsContact
        {
            get { return base.GetBool(LeadAttribute.IsContact, false); }
            set { base.SetExtendedAttribute(LeadAttribute.IsContact, value.ToString()); }
        }

        public string ContactName
        {
            get { return base.GetExtendedAttribute(LeadAttribute.ContactName); }
            set { base.SetExtendedAttribute(LeadAttribute.ContactName, value); }
        }

        public string JobTitle
        {
            get { return base.GetExtendedAttribute(LeadAttribute.JobTitle); }
            set { base.SetExtendedAttribute(LeadAttribute.JobTitle, value); }
        }

        public string AccountRole
        {
            get { return base.GetExtendedAttribute(LeadAttribute.AccountRole); }
            set { base.SetExtendedAttribute(LeadAttribute.AccountRole, value); }
        }

        public string Mobile
        {
            get { return base.GetExtendedAttribute(LeadAttribute.Mobile); }
            set { base.SetExtendedAttribute(LeadAttribute.Mobile, value); }
        }

        public string Telephone
        {
            get { return base.GetExtendedAttribute(LeadAttribute.Telephone); }
            set { base.SetExtendedAttribute(LeadAttribute.Telephone, value); }
        }

        public string Email
        {
            get { return base.GetExtendedAttribute(LeadAttribute.Email); }
            set { base.SetExtendedAttribute(LeadAttribute.Email, value); }
        }        

        public string QQ
        {
            get { return base.GetExtendedAttribute(LeadAttribute.QQ); }
            set { base.SetExtendedAttribute(LeadAttribute.QQ, value); }
        }
        

        public string BackgroundInfo
        {
            get { return base.GetExtendedAttribute(LeadAttribute.BackgroundInfo); }
            set { base.SetExtendedAttribute(LeadAttribute.BackgroundInfo, value); }
        }

        public string ChatOrNote
        {
            get { return base.GetExtendedAttribute(LeadAttribute.ChatOrNote); }
            set { base.SetExtendedAttribute(LeadAttribute.ChatOrNote, value); }
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return LeadAttribute.AllAttributes;
        }
    }
}
