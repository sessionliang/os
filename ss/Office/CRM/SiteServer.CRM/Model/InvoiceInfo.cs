using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;
using System.Data;
using System.Collections.Generic;

namespace SiteServer.CRM.Model
{
    public class InvoiceAttribute
    {
        protected InvoiceAttribute()
        {
        }

        public const string ID = "ID";
        public const string PublishmentSystemID = "PublishmentSystemID";
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
                    allAttributes.Add(SN);
                    allAttributes.Add(InvoiceType);
                    allAttributes.Add(AccountID);
                    allAttributes.Add(OrderID);
                    allAttributes.Add(Amount);
                    allAttributes.Add(IsVAT);
                    allAttributes.Add(InvoiceTitle);
                    allAttributes.Add(InvoiceReceiver);
                    allAttributes.Add(InvoiceTel);
                    allAttributes.Add(InvoiceAddress);
                    allAttributes.Add(VATTaxpayerID);
                    allAttributes.Add(VATBankName);
                    allAttributes.Add(VATBankCardNo);
                    allAttributes.Add(IsInvoice);
                    allAttributes.Add(InvoiceDate);
                    allAttributes.Add(IsConfirm);
                    allAttributes.Add(ConfirmDate);
                    allAttributes.Add(AddDate);
                    allAttributes.Add(Summary);
                }

                return allAttributes;
            }
        }
    }

    public class InvoiceInfo : BaseInfo
    {

        public InvoiceInfo() { }
        public InvoiceInfo(object dataItem) : base(dataItem) { }
        public InvoiceInfo(NameValueCollection form) : base(form) { }
        public InvoiceInfo(IDataReader rdr) : base(rdr) { }

        public int PublishmentSystemID { get; set; }

        public string SN { get; set; }

        public string InvoiceType { get; set; }

        public int AccountID { get; set; }

        public int OrderID { get; set; }

        public decimal Amount { get; set; }

        public bool IsVAT { get; set; }

        public string InvoiceTitle { get; set; }

        public string InvoiceReceiver { get; set; }

        public string InvoiceTel { get; set; }

        public string InvoiceAddress { get; set; }

        public string VATTaxpayerID { get; set; }

        public string VATBankName { get; set; }

        public string VATBankCardNo { get; set; }

        public bool IsInvoice { get; set; }

        public DateTime InvoiceDate { get; set; }

        public bool IsConfirm { get; set; }

        public DateTime ConfirmDate { get; set; }

        public DateTime AddDate { get; set; }

        public string Summary { get; set; }

        protected override List<string> AllAttributes
        {
            get
            {
                return InvoiceAttribute.AllAttributes;
            }
        }
    }
}
