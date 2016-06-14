using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.Project.Model
{
    public class RequestAttribute
    {
        protected RequestAttribute()
        {
        }

        //hidden
        public const string ID = "ID";
        public const string AddDate = "AddDate";

        //basic
        public const string Status = "Status";
        public const string AddUserName = "AddUserName";
        public const string ChargeUserName = "ChargeUserName";
        public const string Satisfaction = "Satisfaction";
        public const string LicenseID = "LicenseID";
        public const string Domain = "Domain";
        public const string AccountID = "AccountID";
        public const string ContactID = "ContactID";
        public const string RequestSN = "RequestSN";
        public const string RequestType = "RequestType";
        public const string Subject = "Subject";
        public const string Website = "Website";
        public const string Email = "Email";
        public const string Mobile = "Mobile";
        public const string QQ = "QQ";

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
                    hiddenAttributes.Add(RequestSN.ToLower());
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
                    basicAttributes.Add(AddUserName.ToLower());
                    basicAttributes.Add(ChargeUserName.ToLower());
                    basicAttributes.Add(Satisfaction.ToLower());
                    basicAttributes.Add(LicenseID.ToLower());
                    basicAttributes.Add(Domain.ToLower());
                    basicAttributes.Add(AccountID.ToLower());
                    basicAttributes.Add(ContactID.ToLower());
                    basicAttributes.Add(RequestType.ToLower());
                    basicAttributes.Add(Subject.ToLower());
                    basicAttributes.Add(Website.ToLower());
                    basicAttributes.Add(Email.ToLower());
                    basicAttributes.Add(Mobile.ToLower());
                    basicAttributes.Add(QQ.ToLower());
                }

                return basicAttributes;
            }
        }
    }

    public class RequestInfo : ExtendedAttributes
    {
        public const string TableName = "crm_Request";

        public RequestInfo() { }

        public RequestInfo(int id, DateTime addDate)
        {
            this.ID = id;
            this.AddDate = addDate;
        }

        public RequestInfo(object dataItem)
            : base(dataItem)
        {
        }

        public int ID
        {
            get { return base.GetInt(RequestAttribute.ID, 0); }
            set { base.SetExtendedAttribute(RequestAttribute.ID, value.ToString()); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(RequestAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(RequestAttribute.AddDate, value.ToString()); }
        }

        public ERequestStatus Status
        {
            get { return ERequestStatusUtils.GetEnumType(base.GetExtendedAttribute(RequestAttribute.Status)); }
            set { base.SetExtendedAttribute(RequestAttribute.Status, ERequestStatusUtils.GetValue(value)); }
        }

        public string AddUserName
        {
            get { return base.GetExtendedAttribute(RequestAttribute.AddUserName); }
            set { base.SetExtendedAttribute(RequestAttribute.AddUserName, value); }
        }

        public string ChargeUserName
        {
            get { return base.GetExtendedAttribute(RequestAttribute.ChargeUserName); }
            set { base.SetExtendedAttribute(RequestAttribute.ChargeUserName, value); }
        }

        public int Satisfaction
        {
            get { return base.GetInt(RequestAttribute.Satisfaction, 0); }
            set { base.SetExtendedAttribute(RequestAttribute.Satisfaction, value.ToString()); }
        }

        public int LicenseID
        {
            get { return base.GetInt(RequestAttribute.LicenseID, 0); }
            set { base.SetExtendedAttribute(RequestAttribute.LicenseID, value.ToString()); }
        }

        public string Domain
        {
            get { return base.GetExtendedAttribute(RequestAttribute.Domain); }
            set { base.SetExtendedAttribute(RequestAttribute.Domain, value); }
        }

        public int AccountID
        {
            get { return base.GetInt(RequestAttribute.AccountID, 0); }
            set { base.SetExtendedAttribute(RequestAttribute.AccountID, value.ToString()); }
        }

        public int ContactID
        {
            get { return base.GetInt(RequestAttribute.ContactID, 0); }
            set { base.SetExtendedAttribute(RequestAttribute.ContactID, value.ToString()); }
        }

        public string RequestSN
        {
            get { return base.GetExtendedAttribute(RequestAttribute.RequestSN); }
            set { base.SetExtendedAttribute(RequestAttribute.RequestSN, value); }
        }

        public string RequestType
        {
            get { return base.GetExtendedAttribute(RequestAttribute.RequestType); }
            set { base.SetExtendedAttribute(RequestAttribute.RequestType, value); }
        }

        public string Subject
        {
            get { return base.GetExtendedAttribute(RequestAttribute.Subject); }
            set { base.SetExtendedAttribute(RequestAttribute.Subject, value); }
        }

        public string Website
        {
            get { return base.GetExtendedAttribute(RequestAttribute.Website); }
            set { base.SetExtendedAttribute(RequestAttribute.Website, value); }
        }

        public string Email
        {
            get { return base.GetExtendedAttribute(RequestAttribute.Email); }
            set { base.SetExtendedAttribute(RequestAttribute.Email, value); }
        }

        public string Mobile
        {
            get { return base.GetExtendedAttribute(RequestAttribute.Mobile); }
            set { base.SetExtendedAttribute(RequestAttribute.Mobile, value); }
        }

        public string QQ
        {
            get { return base.GetExtendedAttribute(RequestAttribute.QQ); }
            set { base.SetExtendedAttribute(RequestAttribute.QQ, value); }
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return RequestAttribute.AllAttributes;
        }
    }
}
