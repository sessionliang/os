using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.Project.Model
{
    public class OrderRefundAttribute
    {
        protected OrderRefundAttribute()
        {
        }

        //hidden
        public const string ID = "ID";
        public const string OrderID = "OrderID";
        public const string OrderSN = "OrderSN";
        public const string LoginName = "LoginName";

        //basic
        public const string Amount = "Amount";
        public const string IsAliyunRefund = "IsAliyunRefund";
        public const string AliyunFileUrl = "AliyunFileUrl";
        public const string AccountRealName = "AccountRealName";
        public const string IsAlipayAccount = "IsAlipayAccount";
        public const string AccountAlipayNo = "AccountAlipayNo";
        public const string AccountBankName = "AccountBankName";
        public const string AccountBankCardNo = "AccountBankCardNo";
        public const string IsRefund = "IsRefund";
        public const string RefundDate = "RefundDate";
        public const string IsConfirm = "IsConfirm";
        public const string ConfirmDate = "ConfirmDate";
        public const string AddDate = "AddDate";
        public const string Reason = "Reason";

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
                    basicAttributes.Add(OrderID.ToLower());
                    basicAttributes.Add(OrderSN.ToLower());
                    basicAttributes.Add(LoginName.ToLower());
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
                    basicAttributes.Add(Amount.ToLower());
                    basicAttributes.Add(IsAliyunRefund.ToLower());
                    basicAttributes.Add(AliyunFileUrl.ToLower());
                    basicAttributes.Add(AccountRealName.ToLower());
                    basicAttributes.Add(IsAlipayAccount.ToLower());
                    basicAttributes.Add(AccountAlipayNo.ToLower());
                    basicAttributes.Add(AccountBankName.ToLower());
                    basicAttributes.Add(AccountBankCardNo.ToLower());
                    basicAttributes.Add(IsRefund.ToLower());
                    basicAttributes.Add(RefundDate.ToLower());
                    basicAttributes.Add(IsConfirm.ToLower());
                    basicAttributes.Add(ConfirmDate.ToLower());
                    basicAttributes.Add(AddDate.ToLower());
                    basicAttributes.Add(Reason.ToLower());
                }

                return basicAttributes;
            }
        }
    }

    public class OrderRefundInfo : ExtendedAttributes
    {
        public const string TableName = "crm_OrderRefund";

        public OrderRefundInfo()
        {
            this.ID = 0;
            this.OrderID = 0;
        }

        public OrderRefundInfo(object dataItem)
            : base(dataItem)
        {
        }

        public OrderRefundInfo(int id, int orderID)
        {
            this.ID = id;
            this.OrderID = orderID;
        }

        public int ID
        {
            get { return base.GetInt(OrderRefundAttribute.ID, 0); }
            set { base.SetExtendedAttribute(OrderRefundAttribute.ID, value.ToString()); }
        }

        public int OrderID
        {
            get { return base.GetInt(OrderRefundAttribute.OrderID, 0); }
            set { base.SetExtendedAttribute(OrderRefundAttribute.OrderID, value.ToString()); }
        }

        public string OrderSN
        {
            get { return base.GetExtendedAttribute(OrderRefundAttribute.OrderSN); }
            set { base.SetExtendedAttribute(OrderRefundAttribute.OrderSN, value); }
        }

        public string LoginName
        {
            get { return base.GetExtendedAttribute(OrderRefundAttribute.LoginName); }
            set { base.SetExtendedAttribute(OrderRefundAttribute.LoginName, value); }
        }

        public decimal Amount
        {
            get { return base.GetDecimal(OrderRefundAttribute.Amount, 0); }
            set { base.SetExtendedAttribute(OrderRefundAttribute.Amount, value.ToString()); }
        }

        public bool IsAliyunRefund
        {
            get { return base.GetBool(OrderRefundAttribute.IsAliyunRefund, false); }
            set { base.SetExtendedAttribute(OrderRefundAttribute.IsAliyunRefund, value.ToString()); }
        }

        public string AliyunFileUrl
        {
            get { return base.GetExtendedAttribute(OrderRefundAttribute.AliyunFileUrl); }
            set { base.SetExtendedAttribute(OrderRefundAttribute.AliyunFileUrl, value); }
        }

        public string AccountRealName
        {
            get { return base.GetExtendedAttribute(OrderRefundAttribute.AccountRealName); }
            set { base.SetExtendedAttribute(OrderRefundAttribute.AccountRealName, value); }
        }

        public bool IsAlipayAccount
        {
            get { return base.GetBool(OrderRefundAttribute.IsAlipayAccount, false); }
            set { base.SetExtendedAttribute(OrderRefundAttribute.IsAlipayAccount, value.ToString()); }
        }

        public string AccountAlipayNo
        {
            get { return base.GetExtendedAttribute(OrderRefundAttribute.AccountAlipayNo); }
            set { base.SetExtendedAttribute(OrderRefundAttribute.AccountAlipayNo, value); }
        }

        public string AccountBankName
        {
            get { return base.GetExtendedAttribute(OrderRefundAttribute.AccountBankName); }
            set { base.SetExtendedAttribute(OrderRefundAttribute.AccountBankName, value); }
        }

        public string AccountBankCardNo
        {
            get { return base.GetExtendedAttribute(OrderRefundAttribute.AccountBankCardNo); }
            set { base.SetExtendedAttribute(OrderRefundAttribute.AccountBankCardNo, value); }
        }

        public bool IsRefund
        {
            get { return base.GetBool(OrderRefundAttribute.IsRefund, false); }
            set { base.SetExtendedAttribute(OrderRefundAttribute.IsRefund, value.ToString()); }
        }

        public DateTime RefundDate
        {
            get { return base.GetDateTime(OrderRefundAttribute.RefundDate, DateTime.Now); }
            set { base.SetExtendedAttribute(OrderRefundAttribute.RefundDate, value.ToString()); }
        }

        public bool IsConfirm
        {
            get { return base.GetBool(OrderRefundAttribute.IsConfirm, false); }
            set { base.SetExtendedAttribute(OrderRefundAttribute.IsConfirm, value.ToString()); }
        }

        public DateTime ConfirmDate
        {
            get { return base.GetDateTime(OrderRefundAttribute.ConfirmDate, DateTime.Now); }
            set { base.SetExtendedAttribute(OrderRefundAttribute.ConfirmDate, value.ToString()); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(OrderRefundAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(OrderRefundAttribute.AddDate, value.ToString()); }
        }

        public string Reason
        {
            get { return base.GetExtendedAttribute(OrderRefundAttribute.Reason); }
            set { base.SetExtendedAttribute(OrderRefundAttribute.Reason, value); }
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return OrderRefundAttribute.AllAttributes;
        }
    }
}
