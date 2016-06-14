using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace SiteServer.CRM.Model
{
    public class ContractAttribute
    {
        protected ContractAttribute()
        {
        }

        public const string ID = "ID";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string SN = "SN";
        public const string ContractType = "ContractType";
        public const string AccountID = "AccountID";
        public const string OrderID = "OrderID";
        public const string ChargeUserName = "ChargeUserName";
        public const string Amount = "Amount";
        public const string ContractTitle = "ContractTitle";
        public const string ContractReceiver = "ContractReceiver";
        public const string ContractTel = "ContractTel";
        public const string ContractAddress = "ContractAddress";
        public const string IsContract = "IsContract";
        public const string ContractDate = "ContractDate";
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
                    allAttributes.Add(ContractType);
                    allAttributes.Add(AccountID);
                    allAttributes.Add(OrderID);
                    allAttributes.Add(ChargeUserName);
                    allAttributes.Add(Amount);
                    allAttributes.Add(ContractTitle);
                    allAttributes.Add(ContractReceiver);
                    allAttributes.Add(ContractTel);
                    allAttributes.Add(ContractAddress);
                    allAttributes.Add(IsContract);
                    allAttributes.Add(ContractDate);
                    allAttributes.Add(IsConfirm);
                    allAttributes.Add(ConfirmDate);
                    allAttributes.Add(AddDate);
                    allAttributes.Add(Summary);
                }

                return allAttributes;
            }
        }
    }

    public class ContractInfo : BaseInfo
    {
        public ContractInfo() { }
        public ContractInfo(object dataItem) : base(dataItem) { }
        public ContractInfo(NameValueCollection form) : base(form) { }
        public ContractInfo(IDataReader rdr) : base(rdr) { }

        public int PublishmentSystemID { get; set; }

        public string SN { get; set; }

        public string ContractType { get; set; }

        public int AccountID { get; set; }

        public int OrderID { get; set; }

        public string ChargeUserName { get; set; }

        public decimal Amount { get; set; }

        public string ContractTitle { get; set; }

        public string ContractReceiver { get; set; }

        public string ContractTel { get; set; }

        public string ContractAddress { get; set; }

        public bool IsContract { get; set; }

        public DateTime ContractDate { get; set; }

        public bool IsConfirm { get; set; }

        public DateTime ConfirmDate { get; set; }

        public DateTime AddDate { get; set; }

        public string Summary { get; set; }

        protected override List<string> AllAttributes
        {
            get
            {
                return ContractAttribute.AllAttributes;
            }
        }
    }
}
