using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace BaiRong.Model
{
    public class UserContactAttribute
    {
        protected UserContactAttribute()
        {
        }

        //hidden
        public const string ID = "ID";
        public const string RelatedUserName = "RelatedUserName";
        public const string CreateUserName = "CreateUserName";
        public const string Taxis = "Taxis";

        //basic
        public const string FullName = "FullName";
        public const string AvatarUrl = "AvatarUrl";
        public const string Summary = "Summary";
        public const string Tel = "Tel";
        public const string Mobile = "Mobile";
        public const string Email = "Email";
        public const string QQ = "QQ";
        public const string Birthday = "Birthday";
        public const string Organization = "Organization";
        public const string Department = "Department";
        public const string Position = "Position";
        public const string Address = "Address";
        public const string Website = "Website";
        public const string Gender = "Gender";

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
                    hiddenAttributes.Add(RelatedUserName.ToLower());
                    hiddenAttributes.Add(CreateUserName.ToLower());
                    hiddenAttributes.Add(Taxis.ToLower());
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
                    basicAttributes.Add(FullName.ToLower());
                    basicAttributes.Add(AvatarUrl.ToLower());
                    basicAttributes.Add(Summary.ToLower());
                    basicAttributes.Add(Tel.ToLower());
                    basicAttributes.Add(Mobile.ToLower());
                    basicAttributes.Add(Email.ToLower());
                    basicAttributes.Add(QQ.ToLower());
                    basicAttributes.Add(Birthday.ToLower());
                    basicAttributes.Add(Organization.ToLower());
                    basicAttributes.Add(Department.ToLower());
                    basicAttributes.Add(Position.ToLower());
                    basicAttributes.Add(Address.ToLower());
                    basicAttributes.Add(Website.ToLower());
                    basicAttributes.Add(Gender.ToLower());
                }

                return basicAttributes;
            }
        }
    }

    public class UserContactInfo : ExtendedAttributes
    {
        public const string TableName = "bairong_UserContact";

        public UserContactInfo()
        {
            this.ID = 0;
            this.RelatedUserName = string.Empty;
            this.CreateUserName = string.Empty;
            this.Taxis = 0;
            this.FullName = string.Empty;
            this.AvatarUrl = string.Empty;
            this.Summary = string.Empty;
            this.Tel = string.Empty;
            this.Mobile = string.Empty;
            this.Email = string.Empty;
            this.QQ = string.Empty;
            this.Birthday = string.Empty;
            this.Organization = string.Empty;
            this.Department = string.Empty;
            this.Position = string.Empty;
            this.Address = string.Empty;
            this.Website = string.Empty;
            this.Gender = string.Empty;

        }

        public UserContactInfo(object dataItem)
            : base(dataItem)
		{
		}

        public UserContactInfo(int id, string relatedUserName, string createUserName, int taxis)
        {
            this.ID = id;
            this.RelatedUserName = relatedUserName;
            this.CreateUserName = createUserName;
            this.Taxis = taxis;
        }

        public int ID
        {
            get { return base.GetInt(UserContactAttribute.ID, 0); }
            set { base.SetExtendedAttribute(UserContactAttribute.ID, value.ToString()); }
        }

        public string RelatedUserName
        {
            get { return base.GetExtendedAttribute(UserContactAttribute.RelatedUserName); }
            set { base.SetExtendedAttribute(UserContactAttribute.RelatedUserName, value); }
        }

        public string CreateUserName
        {
            get { return base.GetExtendedAttribute(UserContactAttribute.CreateUserName); }
            set { base.SetExtendedAttribute(UserContactAttribute.CreateUserName, value); }
        }

        public int Taxis
        {
            get { return base.GetInt(UserContactAttribute.Taxis, 0); }
            set { base.SetExtendedAttribute(UserContactAttribute.Taxis, value.ToString()); }
        }

        public string FullName
        {
            get { return base.GetExtendedAttribute(UserContactAttribute.FullName); }
            set { base.SetExtendedAttribute(UserContactAttribute.FullName, value); }
        }

        public string AvatarUrl
        {
            get { return base.GetExtendedAttribute(UserContactAttribute.AvatarUrl); }
            set { base.SetExtendedAttribute(UserContactAttribute.AvatarUrl, value); }
        }

        public string Summary
        {
            get { return base.GetExtendedAttribute(UserContactAttribute.Summary); }
            set { base.SetExtendedAttribute(UserContactAttribute.Summary, value); }
        }

        public string Tel
        {
            get { return base.GetExtendedAttribute(UserContactAttribute.Tel); }
            set { base.SetExtendedAttribute(UserContactAttribute.Tel, value); }
        }

        public string Mobile
        {
            get { return base.GetExtendedAttribute(UserContactAttribute.Mobile); }
            set { base.SetExtendedAttribute(UserContactAttribute.Mobile, value); }
        }

        public string Email
        {
            get { return base.GetExtendedAttribute(UserContactAttribute.Email); }
            set { base.SetExtendedAttribute(UserContactAttribute.Email, value); }
        }

        public string QQ
        {
            get { return base.GetExtendedAttribute(UserContactAttribute.QQ); }
            set { base.SetExtendedAttribute(UserContactAttribute.QQ, value); }
        }

        public string Birthday
        {
            get { return base.GetExtendedAttribute(UserContactAttribute.Birthday); }
            set { base.SetExtendedAttribute(UserContactAttribute.Birthday, value); }
        }

        public string Organization
        {
            get { return base.GetExtendedAttribute(UserContactAttribute.Organization); }
            set { base.SetExtendedAttribute(UserContactAttribute.Organization, value); }
        }

        public string Department
        {
            get { return base.GetExtendedAttribute(UserContactAttribute.Department); }
            set { base.SetExtendedAttribute(UserContactAttribute.Department, value); }
        }

        public string Position
        {
            get { return base.GetExtendedAttribute(UserContactAttribute.Position); }
            set { base.SetExtendedAttribute(UserContactAttribute.Position, value); }
        }

        public string Address
        {
            get { return base.GetExtendedAttribute(UserContactAttribute.Address); }
            set { base.SetExtendedAttribute(UserContactAttribute.Address, value); }
        }

        public string Website
        {
            get { return base.GetExtendedAttribute(UserContactAttribute.Website); }
            set { base.SetExtendedAttribute(UserContactAttribute.Website, value); }
        }

        public string Gender
        {
            get { return base.GetExtendedAttribute(UserContactAttribute.Gender); }
            set { base.SetExtendedAttribute(UserContactAttribute.Gender, value); }
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return UserContactAttribute.AllAttributes;
        }
    }
}
