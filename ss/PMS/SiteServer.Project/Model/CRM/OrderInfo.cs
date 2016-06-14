using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.Project.Model
{
    public class OrderAttribute
    {
        protected OrderAttribute()
        {
        }

        //hidden
        public const string ID = "ID";

        //basic
        public const string SN = "SN";
        public const string OrderType = "OrderType";
        public const string BizType = "BizType";
        public const string IsReNew = "IsReNew";
        public const string RelatedOrderID = "RelatedOrderID";

        public const string Status = "Status";
        public const string Amount = "Amount";
        public const string IsLicense = "IsLicense";
        public const string IsContract = "IsContract";
        public const string IsInvoice = "IsInvoice";
        public const string IsRefund = "IsRefund";
        public const string Duration = "Duration";
        public const string MobanID = "MobanID";
        public const string AddDate = "AddDate";
        public const string ExpiredDate = "ExpiredDate";
        public const string DomainTemp = "DomainTemp";
        public const string DomainFormal = "DomainFormal";
        public const string BackgroundUserName = "BackgroundUserName";
        public const string BackgroundPassword = "BackgroundPassword";
        public const string LoginName = "LoginName";
        public const string Email = "Email";
        public const string ContactName = "ContactName";
        public const string Mobile = "Mobile";
        public const string QQ = "QQ";
        public const string AliWangWang = "AliWangWang";
        public const string InvoiceTitle = "InvoiceTitle";
        public const string InvoiceReceiver = "InvoiceReceiver";
        public const string InvoiceTel = "InvoiceTel";
        public const string InvoiceAddress = "InvoiceAddress";
        public const string Summary = "Summary";

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
                    basicAttributes.Add(OrderType.ToLower());
                    basicAttributes.Add(BizType.ToLower());
                    basicAttributes.Add(IsReNew.ToLower());
                    basicAttributes.Add(RelatedOrderID.ToLower());

                    basicAttributes.Add(Status.ToLower());
                    basicAttributes.Add(Amount.ToLower());
                    basicAttributes.Add(IsLicense.ToLower());
                    basicAttributes.Add(IsContract.ToLower());
                    basicAttributes.Add(IsInvoice.ToLower());
                    basicAttributes.Add(IsRefund.ToLower());
                    basicAttributes.Add(Duration.ToLower());
                    basicAttributes.Add(MobanID.ToLower());
                    basicAttributes.Add(AddDate.ToLower());
                    basicAttributes.Add(ExpiredDate.ToLower());
                    basicAttributes.Add(DomainTemp.ToLower());
                    basicAttributes.Add(DomainFormal.ToLower());
                    basicAttributes.Add(BackgroundUserName.ToLower());
                    basicAttributes.Add(BackgroundPassword.ToLower());
                    basicAttributes.Add(LoginName.ToLower());
                    basicAttributes.Add(Email.ToLower());
                    basicAttributes.Add(ContactName.ToLower());
                    basicAttributes.Add(Mobile.ToLower());
                    basicAttributes.Add(QQ.ToLower());
                    basicAttributes.Add(AliWangWang.ToLower());
                    basicAttributes.Add(InvoiceTitle.ToLower());
                    basicAttributes.Add(InvoiceReceiver.ToLower());
                    basicAttributes.Add(InvoiceTel.ToLower());
                    basicAttributes.Add(InvoiceAddress.ToLower());
                    basicAttributes.Add(Summary.ToLower());
                }

                return basicAttributes;
            }
        }
    }

    public class OrderInfo : ExtendedAttributes
    {
        public const string TableName = "crm_Order";

        public OrderInfo()
        {
            this.ID = 0;
        }

        public OrderInfo(object dataItem)
            : base(dataItem)
        {
        }

        public OrderInfo(int id)
        {
            this.ID = id;
        }

        public int ID
        {
            get { return base.GetInt(OrderAttribute.ID, 0); }
            set { base.SetExtendedAttribute(OrderAttribute.ID, value.ToString()); }
        }

        public string SN
        {
            get { return base.GetExtendedAttribute(OrderAttribute.SN); }
            set { base.SetExtendedAttribute(OrderAttribute.SN, value); }
        }

        public EOrderType OrderType
        {
            get { return EOrderTypeUtils.GetEnumType(base.GetExtendedAttribute(OrderAttribute.OrderType)); }
            set { base.SetExtendedAttribute(OrderAttribute.OrderType, EOrderTypeUtils.GetValue(value)); }
        }

        public string BizType
        {
            get { return base.GetExtendedAttribute(OrderAttribute.BizType); }
            set { base.SetExtendedAttribute(OrderAttribute.BizType, value); }
        }

        public bool IsReNew
        {
            get { return base.GetBool(OrderAttribute.IsReNew, false); }
            set { base.SetExtendedAttribute(OrderAttribute.IsReNew, value.ToString()); }
        }

        public int RelatedOrderID
        {
            get { return base.GetInt(OrderAttribute.RelatedOrderID, 0); }
            set { base.SetExtendedAttribute(OrderAttribute.RelatedOrderID, value.ToString()); }
        }

        public EOrderStatus Status
        {
            get { return EOrderStatusUtils.GetEnumType(base.GetExtendedAttribute(OrderAttribute.Status)); }
            set { base.SetExtendedAttribute(OrderAttribute.Status, EOrderStatusUtils.GetValue(value)); }
        }

        public decimal Amount
        {
            get { return base.GetDecimal(OrderAttribute.Amount, 0); }
            set { base.SetExtendedAttribute(OrderAttribute.Amount, value.ToString()); }
        }

        public bool IsLicense
        {
            get { return base.GetBool(OrderAttribute.IsLicense, false); }
            set { base.SetExtendedAttribute(OrderAttribute.IsLicense, value.ToString()); }
        }

        public bool IsContract
        {
            get { return base.GetBool(OrderAttribute.IsContract, false); }
            set { base.SetExtendedAttribute(OrderAttribute.IsContract, value.ToString()); }
        }

        public bool IsInvoice
        {
            get { return base.GetBool(OrderAttribute.IsInvoice, false); }
            set { base.SetExtendedAttribute(OrderAttribute.IsInvoice, value.ToString()); }
        }

        public bool IsRefund
        {
            get { return base.GetBool(OrderAttribute.IsRefund, false); }
            set { base.SetExtendedAttribute(OrderAttribute.IsRefund, value.ToString()); }
        }

        public int Duration
        {
            get { return base.GetInt(OrderAttribute.Duration, 0); }
            set { base.SetExtendedAttribute(OrderAttribute.Duration, value.ToString()); }
        }

        public string MobanID
        {
            get { return base.GetExtendedAttribute(OrderAttribute.MobanID); }
            set { base.SetExtendedAttribute(OrderAttribute.MobanID, value); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(OrderAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(OrderAttribute.AddDate, value.ToString()); }
        }

        public DateTime ExpiredDate
        {
            get { return base.GetDateTime(OrderAttribute.ExpiredDate, DateTime.Now); }
            set { base.SetExtendedAttribute(OrderAttribute.ExpiredDate, value.ToString()); }
        }

        public string DomainTemp
        {
            get { return base.GetExtendedAttribute(OrderAttribute.DomainTemp); }
            set { base.SetExtendedAttribute(OrderAttribute.DomainTemp, value); }
        }

        public string DomainFormal
        {
            get { return base.GetExtendedAttribute(OrderAttribute.DomainFormal); }
            set { base.SetExtendedAttribute(OrderAttribute.DomainFormal, value); }
        }

        public string BackgroundUserName
        {
            get { return base.GetExtendedAttribute(OrderAttribute.BackgroundUserName); }
            set { base.SetExtendedAttribute(OrderAttribute.BackgroundUserName, value); }
        }

        public string BackgroundPassword
        {
            get { return base.GetExtendedAttribute(OrderAttribute.BackgroundPassword); }
            set { base.SetExtendedAttribute(OrderAttribute.BackgroundPassword, value); }
        }

        public string LoginName
        {
            get { return base.GetExtendedAttribute(OrderAttribute.LoginName); }
            set { base.SetExtendedAttribute(OrderAttribute.LoginName, value); }
        }

        public string Email
        {
            get { return base.GetExtendedAttribute(OrderAttribute.Email); }
            set { base.SetExtendedAttribute(OrderAttribute.Email, value); }
        }

        public string ContactName
        {
            get { return base.GetExtendedAttribute(OrderAttribute.ContactName); }
            set { base.SetExtendedAttribute(OrderAttribute.ContactName, value); }
        }

        public string Mobile
        {
            get { return base.GetExtendedAttribute(OrderAttribute.Mobile); }
            set { base.SetExtendedAttribute(OrderAttribute.Mobile, value); }
        }

        public string QQ
        {
            get { return base.GetExtendedAttribute(OrderAttribute.QQ); }
            set { base.SetExtendedAttribute(OrderAttribute.QQ, value); }
        }

        public string AliWangWang
        {
            get { return base.GetExtendedAttribute(OrderAttribute.AliWangWang); }
            set { base.SetExtendedAttribute(OrderAttribute.AliWangWang, value); }
        }

        public string InvoiceTitle
        {
            get { return base.GetExtendedAttribute(OrderAttribute.InvoiceTitle); }
            set { base.SetExtendedAttribute(OrderAttribute.InvoiceTitle, value); }
        }

        public string InvoiceReceiver
        {
            get { return base.GetExtendedAttribute(OrderAttribute.InvoiceReceiver); }
            set { base.SetExtendedAttribute(OrderAttribute.InvoiceReceiver, value); }
        }

        public string InvoiceTel
        {
            get { return base.GetExtendedAttribute(OrderAttribute.InvoiceTel); }
            set { base.SetExtendedAttribute(OrderAttribute.InvoiceTel, value); }
        }

        public string InvoiceAddress
        {
            get { return base.GetExtendedAttribute(OrderAttribute.InvoiceAddress); }
            set { base.SetExtendedAttribute(OrderAttribute.InvoiceAddress, value); }
        }

        public string Summary
        {
            get { return base.GetExtendedAttribute(OrderAttribute.Summary); }
            set { base.SetExtendedAttribute(OrderAttribute.Summary, value); }
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return OrderAttribute.AllAttributes;
        }
    }
}
