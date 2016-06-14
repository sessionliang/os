using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.Project.Model
{
    public class ContractAttribute
    {
        protected ContractAttribute()
        {
        }

        //hidden
        public const string ID = "ID";

        //basic
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
                    basicAttributes.Add(ContractType.ToLower());
                    basicAttributes.Add(AccountID.ToLower());
                    basicAttributes.Add(OrderID.ToLower());
                    basicAttributes.Add(ChargeUserName.ToLower());
                    basicAttributes.Add(Amount.ToLower());
                    basicAttributes.Add(ContractTitle.ToLower());
                    basicAttributes.Add(ContractReceiver.ToLower());
                    basicAttributes.Add(ContractTel.ToLower());
                    basicAttributes.Add(ContractAddress.ToLower());
                    basicAttributes.Add(IsContract.ToLower());
                    basicAttributes.Add(ContractDate.ToLower());
                    basicAttributes.Add(IsConfirm.ToLower());
                    basicAttributes.Add(ConfirmDate.ToLower());
                    basicAttributes.Add(AddDate.ToLower());
                    basicAttributes.Add(Summary.ToLower());
                }

                return basicAttributes;
            }
        }
    }

    public class ContractInfo : ExtendedAttributes
    {
        public const string TableName = "crm_Contract";

        public ContractInfo()
        {
            this.ID = 0;
        }

        public ContractInfo(object dataItem)
            : base(dataItem)
        {
        }

        public ContractInfo(int id)
        {
            this.ID = id;
        }

        public int ID
        {
            get { return base.GetInt(ContractAttribute.ID, 0); }
            set { base.SetExtendedAttribute(ContractAttribute.ID, value.ToString()); }
        }

        public string SN
        {
            get { return base.GetExtendedAttribute(ContractAttribute.SN); }
            set { base.SetExtendedAttribute(ContractAttribute.SN, value); }
        }

        public EContractType ContractType
        {
            get { return EContractTypeUtils.GetEnumType(base.GetExtendedAttribute(ContractAttribute.ContractType)); }
            set { base.SetExtendedAttribute(ContractAttribute.ContractType, EContractTypeUtils.GetValue(value)); }
        }

        public int AccountID
        {
            get { return base.GetInt(ContractAttribute.AccountID, 0); }
            set { base.SetExtendedAttribute(ContractAttribute.AccountID, value.ToString()); }
        }

        public int OrderID
        {
            get { return base.GetInt(ContractAttribute.OrderID, 0); }
            set { base.SetExtendedAttribute(ContractAttribute.OrderID, value.ToString()); }
        }

        public string ChargeUserName
        {
            get { return base.GetExtendedAttribute(ContractAttribute.ChargeUserName); }
            set { base.SetExtendedAttribute(ContractAttribute.ChargeUserName, value); }
        }

        public decimal Amount
        {
            get { return base.GetDecimal(ContractAttribute.Amount, 0); }
            set { base.SetExtendedAttribute(ContractAttribute.Amount, value.ToString()); }
        }

        public string ContractTitle
        {
            get { return base.GetExtendedAttribute(ContractAttribute.ContractTitle); }
            set { base.SetExtendedAttribute(ContractAttribute.ContractTitle, value); }
        }

        public string ContractReceiver
        {
            get { return base.GetExtendedAttribute(ContractAttribute.ContractReceiver); }
            set { base.SetExtendedAttribute(ContractAttribute.ContractReceiver, value); }
        }

        public string ContractTel
        {
            get { return base.GetExtendedAttribute(ContractAttribute.ContractTel); }
            set { base.SetExtendedAttribute(ContractAttribute.ContractTel, value); }
        }

        public string ContractAddress
        {
            get { return base.GetExtendedAttribute(ContractAttribute.ContractAddress); }
            set { base.SetExtendedAttribute(ContractAttribute.ContractAddress, value); }
        }

        public bool IsContract
        {
            get { return base.GetBool(ContractAttribute.IsContract, false); }
            set { base.SetExtendedAttribute(ContractAttribute.IsContract, value.ToString()); }
        }

        public DateTime ContractDate
        {
            get { return base.GetDateTime(ContractAttribute.ContractDate, DateTime.Now); }
            set { base.SetExtendedAttribute(ContractAttribute.ContractDate, value.ToString()); }
        }

        public bool IsConfirm
        {
            get { return base.GetBool(ContractAttribute.IsConfirm, false); }
            set { base.SetExtendedAttribute(ContractAttribute.IsConfirm, value.ToString()); }
        }

        public DateTime ConfirmDate
        {
            get { return base.GetDateTime(ContractAttribute.ConfirmDate, DateTime.Now); }
            set { base.SetExtendedAttribute(ContractAttribute.ConfirmDate, value.ToString()); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(ContractAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(ContractAttribute.AddDate, value.ToString()); }
        }

        public string Summary
        {
            get { return base.GetExtendedAttribute(ContractAttribute.Summary); }
            set { base.SetExtendedAttribute(ContractAttribute.Summary, value); }
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return ContractAttribute.AllAttributes;
        }
    }
}
