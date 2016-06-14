using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BaiRong.Model;
using System.Collections.Specialized;
using System.Data;

namespace SiteServer.B2C.Model
{
    public class OrderAttribute
    {
        protected OrderAttribute()
        {
        }

        //hidden
        public const string ID = "ID";

        //basic
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string OrderSN = "OrderSN";
        public const string UserName = "UserName";
        public const string IPAddress = "IPAddress";
        public const string OrderStatus = "OrderStatus";
        public const string PaymentStatus = "PaymentStatus";
        public const string ShipmentStatus = "ShipmentStatus";
        public const string TimeOrder = "TimeOrder";
        public const string TimePayment = "TimePayment";
        public const string TimeShipment = "TimeShipment";
        public const string ConsigneeID = "ConsigneeID";
        public const string PaymentID = "PaymentID";
        public const string ShipmentID = "ShipmentID";
        public const string InvoiceID = "InvoiceID";
        public const string PriceTotal = "PriceTotal";
        public const string PriceShipment = "PriceShipment";
        public const string PriceReturn = "PriceReturn";
        public const string PriceActual = "PriceActual";
        public const string Summary = "Summary";
        public const string Extended = "Extended";//交易流水

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
                    allAttributes.Add(OrderSN);
                    allAttributes.Add(UserName);
                    allAttributes.Add(IPAddress);
                    allAttributes.Add(OrderStatus);
                    allAttributes.Add(PaymentStatus);
                    allAttributes.Add(ShipmentStatus);
                    allAttributes.Add(TimeOrder);
                    allAttributes.Add(TimePayment);
                    allAttributes.Add(TimeShipment);
                    allAttributes.Add(ConsigneeID);
                    allAttributes.Add(PaymentID);
                    allAttributes.Add(ShipmentID);
                    allAttributes.Add(InvoiceID);
                    allAttributes.Add(PriceTotal);
                    allAttributes.Add(PriceShipment);
                    allAttributes.Add(PriceReturn);
                    allAttributes.Add(PriceActual);
                    allAttributes.Add(Summary);
                    allAttributes.Add(Extended);
                }

                return allAttributes;
            }
        }
    }
    public class OrderInfo : BaseInfo
    {
        public OrderInfo() { }
        public OrderInfo(object dataItem) : base(dataItem) { }
        public OrderInfo(NameValueCollection form) : base(form) { }
        public OrderInfo(IDataReader rdr) : base(rdr) { }
        public int PublishmentSystemID { get; set; }
        public string OrderSN { get; set; }
        public string UserName { get; set; }
        public string IPAddress { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string ShipmentStatus { get; set; }
        public DateTime TimeOrder { get; set; }
        public DateTime TimePayment { get; set; }
        public DateTime TimeShipment { get; set; }
        public int ConsigneeID { get; set; }
        public int PaymentID { get; set; }
        public int ShipmentID { get; set; }
        public int InvoiceID { get; set; }
        public decimal PriceTotal { get; set; }
        public decimal PriceShipment { get; set; }
        public decimal PriceReturn { get; set; }
        public decimal PriceActual { get; set; }
        public string Summary { get; set; }
        public string Extended { get; set; }

        protected override List<string> AllAttributes
        {
            get
            {
                return OrderAttribute.AllAttributes;
            }
        }
    }
}
