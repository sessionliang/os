using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.Project.Model
{
    public class AccountAttribute
    {
        protected AccountAttribute()
        {
        }

        //hidden
        public const string ID = "ID";

        //basic
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
                    basicAttributes.Add(SN.ToLower());
                    basicAttributes.Add(AccountType.ToLower());
                    basicAttributes.Add(AddUserName.ToLower());
                    basicAttributes.Add(AddDate.ToLower());
                    basicAttributes.Add(Status.ToLower());
                    basicAttributes.Add(ChargeUserName.ToLower());
                    basicAttributes.Add(AccountName.ToLower());
                    basicAttributes.Add(BusinessType.ToLower());
                    basicAttributes.Add(Classification.ToLower());
                    basicAttributes.Add(Priority.ToLower());
                    basicAttributes.Add(Website.ToLower());
                    basicAttributes.Add(Telephone.ToLower());
                    basicAttributes.Add(Province.ToLower());
                    basicAttributes.Add(City.ToLower());
                    basicAttributes.Add(Area.ToLower());
                    basicAttributes.Add(Address.ToLower());
                    basicAttributes.Add(Description.ToLower());
                    basicAttributes.Add(ChatOrNote.ToLower());
                }

                return basicAttributes;
            }
        }
    }

    public class AccountInfo : ExtendedAttributes
    {
        public const string TableName = "crm_Account";

        public AccountInfo(object dataItem)
            : base(dataItem)
        {
        }

        public AccountInfo()
        {
            this.ID = 0;
        }

        public AccountInfo(int id)
        {
            this.ID = id;
        }

        public int ID
        {
            get { return base.GetInt(AccountAttribute.ID, 0); }
            set { base.SetExtendedAttribute(AccountAttribute.ID, value.ToString()); }
        }

        public string SN
        {
            get { return base.GetExtendedAttribute(AccountAttribute.SN); }
            set { base.SetExtendedAttribute(AccountAttribute.SN, value); }
        }

        public EAccountType AccountType
        {
            get { return EAccountTypeUtils.GetEnumType(base.GetExtendedAttribute(AccountAttribute.AccountType)); }
            set { base.SetExtendedAttribute(AccountAttribute.AccountType, EAccountTypeUtils.GetValue(value)); }
        }

        public string AddUserName
        {
            get { return base.GetExtendedAttribute(AccountAttribute.AddUserName); }
            set { base.SetExtendedAttribute(AccountAttribute.AddUserName, value); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(AccountAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(AccountAttribute.AddDate, value.ToString()); }
        }

        public EAccountStatus Status
        {
            get { return EAccountStatusUtils.GetEnumType(base.GetExtendedAttribute(AccountAttribute.Status)); }
            set { base.SetExtendedAttribute(AccountAttribute.Status, EAccountStatusUtils.GetValue(value)); }
        }

        public string ChargeUserName
        {
            get { return base.GetExtendedAttribute(AccountAttribute.ChargeUserName); }
            set { base.SetExtendedAttribute(AccountAttribute.ChargeUserName, value); }
        }

        public string AccountName
        {
            get { return base.GetExtendedAttribute(AccountAttribute.AccountName); }
            set { base.SetExtendedAttribute(AccountAttribute.AccountName, value); }
        }

        public string BusinessType
        {
            get { return base.GetExtendedAttribute(AccountAttribute.BusinessType); }
            set { base.SetExtendedAttribute(AccountAttribute.BusinessType, value); }
        }

        public string Classification
        {
            get { return base.GetExtendedAttribute(AccountAttribute.Classification); }
            set { base.SetExtendedAttribute(AccountAttribute.Classification, value); }
        }

        public int Priority
        {
            get { return base.GetInt(AccountAttribute.Priority, 0); }
            set { base.SetExtendedAttribute(AccountAttribute.Priority, value.ToString()); }
        }

        public string Website
        {
            get { return base.GetExtendedAttribute(AccountAttribute.Website); }
            set { base.SetExtendedAttribute(AccountAttribute.Website, value); }
        }

        public string Telephone
        {
            get { return base.GetExtendedAttribute(AccountAttribute.Telephone); }
            set { base.SetExtendedAttribute(AccountAttribute.Telephone, value); }
        }

        public string Province
        {
            get { return base.GetExtendedAttribute(AccountAttribute.Province); }
            set { base.SetExtendedAttribute(AccountAttribute.Province, value); }
        }

        public string City
        {
            get { return base.GetExtendedAttribute(AccountAttribute.City); }
            set { base.SetExtendedAttribute(AccountAttribute.City, value); }
        }

        public string Area
        {
            get { return base.GetExtendedAttribute(AccountAttribute.Area); }
            set { base.SetExtendedAttribute(AccountAttribute.Area, value); }
        }

        public string Address
        {
            get { return base.GetExtendedAttribute(AccountAttribute.Address); }
            set { base.SetExtendedAttribute(AccountAttribute.Address, value); }
        }

        public string Description
        {
            get { return base.GetExtendedAttribute(AccountAttribute.Description); }
            set { base.SetExtendedAttribute(AccountAttribute.Description, value); }
        }

        public string ChatOrNote
        {
            get { return base.GetExtendedAttribute(AccountAttribute.ChatOrNote); }
            set { base.SetExtendedAttribute(AccountAttribute.ChatOrNote, value); }
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return AccountAttribute.AllAttributes;
        }
    }
}
