using BaiRong.Core;
using BaiRong.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;

namespace SiteServer.B2C.Model
{
    public class InvoiceAttribute
    {
        protected InvoiceAttribute()
        {
        }

        public const string ID = "ID";
        public const string GroupSN = "GroupSN";
        public const string UserName = "UserName";
        public const string IsOrder = "IsOrder";
        public const string IsDefault = "IsDefault";
        public const string IsVat = "IsVat";
        public const string IsCompany = "IsCompany";
        public const string CompanyName = "CompanyName";
        public const string VatCompanyName = "VatCompanyName";
        public const string VatCode = "VatCode";
        public const string VatAddress = "VatAddress";
        public const string VatPhone = "VatPhone";
        public const string VatBankName = "VatBankName";
        public const string VatBankAccount = "VatBankAccount";
        public const string ConsigneeName = "ConsigneeName";
        public const string ConsigneeMobile = "ConsigneeMobile";
        public const string ConsigneeAddress = "ConsigneeAddress";

        private static List<string> allAttributes;
        public static List<string> AllAttributes
        {
            get
            {
                if (allAttributes == null)
                {
                    allAttributes = new List<string>();
                    allAttributes.Add(ID);
                    allAttributes.Add(GroupSN);
                    allAttributes.Add(UserName);
                    allAttributes.Add(IsOrder);
                    allAttributes.Add(IsDefault);
                    allAttributes.Add(IsVat);
                    allAttributes.Add(IsCompany);
                    allAttributes.Add(CompanyName);
                    allAttributes.Add(VatCompanyName);
                    allAttributes.Add(VatCode);
                    allAttributes.Add(VatAddress);
                    allAttributes.Add(VatPhone);
                    allAttributes.Add(VatBankName);
                    allAttributes.Add(VatBankAccount);
                    allAttributes.Add(ConsigneeName);
                    allAttributes.Add(ConsigneeMobile);
                    allAttributes.Add(ConsigneeAddress);
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
        public string GroupSN { get; set; }
        public string UserName { get; set; }
        public bool IsOrder { get; set; }
        public bool IsDefault { get; set; }
        public bool IsVat { get; set; }
        public bool IsCompany { get; set; }
        public string CompanyName { get; set; }
        public string VatCompanyName { get; set; }
        public string VatCode { get; set; }
        public string VatAddress { get; set; }
        public string VatPhone { get; set; }
        public string VatBankName { get; set; }
        public string VatBankAccount { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeMobile { get; set; }
        public string ConsigneeAddress { get; set; }

        protected override List<string> AllAttributes
        {
            get
            {
                return InvoiceAttribute.AllAttributes;
            }
        }
    }
}
