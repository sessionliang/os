using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.Project.Model
{
    public class InvoiceAttribute
    {
        protected InvoiceAttribute()
        {
        }

        //hidden
        public const string ID = "ID";

        //basic
        public const string SN = "SN";
        public const string InvoiceType = "InvoiceType";
        public const string AccountID = "AccountID";
        public const string OrderID = "OrderID";
        public const string Amount = "Amount";
        public const string IsVAT = "IsVAT";
        public const string InvoiceTitle = "InvoiceTitle";
        public const string InvoiceReceiver = "InvoiceReceiver";
        public const string InvoiceTel = "InvoiceTel";
        public const string InvoiceAddress = "InvoiceAddress";
        public const string VATTaxpayerID = "VATTaxpayerID";
        public const string VATBankName = "VATBankName";
        public const string VATBankCardNo = "VATBankCardNo";
        public const string IsInvoice = "IsInvoice";
        public const string InvoiceDate = "InvoiceDate";
        public const string IsConfirm = "IsConfirm";
        public const string ConfirmDate = "ConfirmDate";
        public const string AddDate = "AddDate";
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
                    basicAttributes.Add(InvoiceType.ToLower());
                    basicAttributes.Add(AccountID.ToLower());
                    basicAttributes.Add(OrderID.ToLower());
                    basicAttributes.Add(Amount.ToLower());
                    basicAttributes.Add(IsVAT.ToLower());
                    basicAttributes.Add(InvoiceTitle.ToLower());
                    basicAttributes.Add(InvoiceReceiver.ToLower());
                    basicAttributes.Add(InvoiceTel.ToLower());
                    basicAttributes.Add(InvoiceAddress.ToLower());
                    basicAttributes.Add(VATTaxpayerID.ToLower());
                    basicAttributes.Add(VATBankName.ToLower());
                    basicAttributes.Add(VATBankCardNo.ToLower());
                    basicAttributes.Add(IsInvoice.ToLower());
                    basicAttributes.Add(InvoiceDate.ToLower());
                    basicAttributes.Add(IsConfirm.ToLower());
                    basicAttributes.Add(ConfirmDate.ToLower());
                    basicAttributes.Add(AddDate.ToLower());
                    basicAttributes.Add(Summary.ToLower());
                }

                return basicAttributes;
            }
        }
    }

    public class InvoiceInfo : ExtendedAttributes
    {
        public const string TableName = "crm_Invoice";

        public InvoiceInfo()
        {
            this.ID = 0;
        }

        public InvoiceInfo(object dataItem)
            : base(dataItem)
        {
        }

        public InvoiceInfo(int id)
        {
            this.ID = id;
        }

        public int ID
        {
            get { return base.GetInt(InvoiceAttribute.ID, 0); }
            set { base.SetExtendedAttribute(InvoiceAttribute.ID, value.ToString()); }
        }

        public string SN
        {
            get { return base.GetExtendedAttribute(InvoiceAttribute.SN); }
            set { base.SetExtendedAttribute(InvoiceAttribute.SN, value); }
        }

        public EInvoiceType InvoiceType
        {
            get { return EInvoiceTypeUtils.GetEnumType(base.GetExtendedAttribute(InvoiceAttribute.InvoiceType)); }
            set { base.SetExtendedAttribute(InvoiceAttribute.InvoiceType, EInvoiceTypeUtils.GetValue(value)); }
        }

        public int AccountID
        {
            get { return base.GetInt(InvoiceAttribute.AccountID, 0); }
            set { base.SetExtendedAttribute(InvoiceAttribute.AccountID, value.ToString()); }
        }

        public int OrderID
        {
            get { return base.GetInt(InvoiceAttribute.OrderID, 0); }
            set { base.SetExtendedAttribute(InvoiceAttribute.OrderID, value.ToString()); }
        }

        public decimal Amount
        {
            get { return base.GetDecimal(InvoiceAttribute.Amount, 0); }
            set { base.SetExtendedAttribute(InvoiceAttribute.Amount, value.ToString()); }
        }

        public bool IsVAT
        {
            get { return base.GetBool(InvoiceAttribute.IsVAT, false); }
            set { base.SetExtendedAttribute(InvoiceAttribute.IsVAT, value.ToString()); }
        }

        public string InvoiceTitle
        {
            get { return base.GetExtendedAttribute(InvoiceAttribute.InvoiceTitle); }
            set { base.SetExtendedAttribute(InvoiceAttribute.InvoiceTitle, value); }
        }

        public string InvoiceReceiver
        {
            get { return base.GetExtendedAttribute(InvoiceAttribute.InvoiceReceiver); }
            set { base.SetExtendedAttribute(InvoiceAttribute.InvoiceReceiver, value); }
        }

        public string InvoiceTel
        {
            get { return base.GetExtendedAttribute(InvoiceAttribute.InvoiceTel); }
            set { base.SetExtendedAttribute(InvoiceAttribute.InvoiceTel, value); }
        }

        public string InvoiceAddress
        {
            get { return base.GetExtendedAttribute(InvoiceAttribute.InvoiceAddress); }
            set { base.SetExtendedAttribute(InvoiceAttribute.InvoiceAddress, value); }
        }

        public string VATTaxpayerID
        {
            get { return base.GetExtendedAttribute(InvoiceAttribute.VATTaxpayerID); }
            set { base.SetExtendedAttribute(InvoiceAttribute.VATTaxpayerID, value); }
        }

        public string VATBankName
        {
            get { return base.GetExtendedAttribute(InvoiceAttribute.VATBankName); }
            set { base.SetExtendedAttribute(InvoiceAttribute.VATBankName, value); }
        }

        public string VATBankCardNo
        {
            get { return base.GetExtendedAttribute(InvoiceAttribute.VATBankCardNo); }
            set { base.SetExtendedAttribute(InvoiceAttribute.VATBankCardNo, value); }
        }

        public bool IsInvoice
        {
            get { return base.GetBool(InvoiceAttribute.IsInvoice, false); }
            set { base.SetExtendedAttribute(InvoiceAttribute.IsInvoice, value.ToString()); }
        }

        public DateTime InvoiceDate
        {
            get { return base.GetDateTime(InvoiceAttribute.InvoiceDate, DateTime.Now); }
            set { base.SetExtendedAttribute(InvoiceAttribute.InvoiceDate, value.ToString()); }
        }

        public bool IsConfirm
        {
            get { return base.GetBool(InvoiceAttribute.IsConfirm, false); }
            set { base.SetExtendedAttribute(InvoiceAttribute.IsConfirm, value.ToString()); }
        }

        public DateTime ConfirmDate
        {
            get { return base.GetDateTime(InvoiceAttribute.ConfirmDate, DateTime.Now); }
            set { base.SetExtendedAttribute(InvoiceAttribute.ConfirmDate, value.ToString()); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(InvoiceAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(InvoiceAttribute.AddDate, value.ToString()); }
        }

        public string Summary
        {
            get { return base.GetExtendedAttribute(InvoiceAttribute.Summary); }
            set { base.SetExtendedAttribute(InvoiceAttribute.Summary, value); }
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return InvoiceAttribute.AllAttributes;
        }
    }
}
