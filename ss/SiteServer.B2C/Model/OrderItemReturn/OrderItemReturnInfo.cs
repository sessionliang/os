using System;
using BaiRong.Model;
using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections.Generic;
using System.Data;

namespace SiteServer.B2C.Model
{
    public class OrderItemReturnAttribute
    {
        protected OrderItemReturnAttribute()
        {
        }

        //hidden
        public const string ID = "ID";

        //basic
        public const string OrderItemID = "OrderItemID";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string ApplyDate = "ApplyDate";
        public const string ApplyUser = "ApplyUser";
        public const string AuditStatus = "AuditStatus";
        public const string AuditUser = "AuditUser";
        public const string AuditDate = "AuditDate";
        public const string ReturnOrderStatus = "ReturnOrderStatus";
        public const string ReturnOrderUser = "ReturnOrderUser";
        public const string ReturnOrderDate = "ReturnOrderDate";
        public const string ReturnMoneyStatus = "ReturnMoneyStatus";
        public const string ReturnMoneyUser = "ReturnMoneyUser";
        public const string ReturnMoneyDate = "ReturnMoneyDate";
        public const string Status = "Status";
        public const string Title = "Title";
        public const string GoodsSN = "GoodsSN";

        public const string Type = "Type";
        public const string Description = "Description";
        public const string ImageUrl = "ImageUrl";
        public const string ReturnCount = "ReturnCount";
        public const string InspectReport = "InspectReport";
        public const string ReturnMode = "ReturnMode";
        public const string Contact = "Contact";
        public const string ContactPhone = "ContactPhone";
        public const string SettingXml = "SettingXml";


        private static List<string> allAttributes;
        public static List<string> AllAttributes
        {
            get
            {
                if (allAttributes == null)
                {
                    allAttributes = new List<string>();
                    allAttributes.Add(ID);
                    allAttributes.Add(OrderItemID);
                    allAttributes.Add(PublishmentSystemID);
                    allAttributes.Add(ApplyDate);
                    allAttributes.Add(ApplyUser);
                    allAttributes.Add(AuditStatus);
                    allAttributes.Add(AuditUser);
                    allAttributes.Add(AuditDate);
                    allAttributes.Add(ReturnOrderStatus);
                    allAttributes.Add(ReturnOrderUser);
                    allAttributes.Add(ReturnOrderDate);
                    allAttributes.Add(ReturnMoneyStatus);
                    allAttributes.Add(ReturnMoneyUser);
                    allAttributes.Add(ReturnMoneyDate);
                    allAttributes.Add(Status);
                    allAttributes.Add(Title);
                    allAttributes.Add(GoodsSN);

                    allAttributes.Add(Type);
                    allAttributes.Add(Description);
                    allAttributes.Add(ImageUrl);
                    allAttributes.Add(ReturnCount);
                    allAttributes.Add(InspectReport);
                    allAttributes.Add(ReturnMode);
                    allAttributes.Add(Contact);
                    allAttributes.Add(ContactPhone);
                    allAttributes.Add(SettingXml);
                }

                return allAttributes;
            }
        }
    }

    public class OrderItemReturnInfo : BaseInfo
    {
        public OrderItemReturnInfo() { }
        public OrderItemReturnInfo(object dataItem) : base(dataItem) { }
        public OrderItemReturnInfo(IDataReader rdr) : base(rdr) { }
        public OrderItemReturnInfo(NameValueCollection form, bool isFilterSqlAndXss) : base(form, isFilterSqlAndXss) { }

        public int OrderItemID { get; set; }
        public int PublishmentSystemID { get; set; }
        public DateTime ApplyDate { get; set; }
        public string ApplyUser { get; set; }
        public string AuditStatus { get; set; }
        public string AuditUser { get; set; }
        public DateTime AuditDate { get; set; }
        public string ReturnOrderStatus { get; set; }
        public string ReturnOrderUser { get; set; }
        public DateTime ReturnOrderDate { get; set; }
        public string ReturnMoneyStatus { get; set; }
        public string ReturnMoneyUser { get; set; }
        public DateTime ReturnMoneyDate { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public string GoodsSN { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int ReturnCount { get; set; }
        public bool InspectReport { get; set; }
        public string ReturnMode { get; set; }
        public string Contact { get; set; }
        public string ContactPhone { get; set; }
        public string SettingXml { get; set; }

        protected override List<string> AllAttributes
        {
            get
            {
                return OrderItemReturnAttribute.AllAttributes;
            }
        }

        OrderItemReturnInfoExtend additional;
        public OrderItemReturnInfoExtend Additional
        {
            get
            {
                if (this.additional == null)
                {
                    this.additional = new OrderItemReturnInfoExtend(this.SettingXml);
                }
                return this.additional;
            }
        }
    }
}
