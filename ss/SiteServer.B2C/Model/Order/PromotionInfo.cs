using BaiRong.Core;
using BaiRong.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;

namespace SiteServer.B2C.Model
{
    public class PromotionAttribute
    {
        protected PromotionAttribute()
        {
        }

        //hidden
        public const string ID = "ID";

        //basic
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string PromotionName = "PromotionName";
        public const string PromotionType = "PromotionType";
        public const string StartDate = "StartDate";
        public const string EndDate = "EndDate";
        public const string Tags = "Tags";
        public const string Target = "Target";
        public const string ChannelIDCollection = "ChannelIDCollection";
        public const string IDsCollection = "IDsCollection";
        public const string ExcludeChannelIDCollection = "ExcludeChannelIDCollection";
        public const string ExcludeIDsCollection = "ExcludeIDsCollection";
        public const string IfAmount = "IfAmount";
        public const string IfCount = "IfCount";
        public const string Discount = "Discount";
        public const string ReturnAmount = "ReturnAmount";
        public const string IsReturnMultiply = "IsReturnMultiply";
        public const string IsShipmentFree = "IsShipmentFree";
        public const string IsGift = "IsGift";
        public const string GiftName = "GiftName";
        public const string GiftUrl = "GiftUrl";
        public const string IsEnabled = "IsEnabled";
        public const string Taxis = "Taxis";
        public const string AddDate = "AddDate";
        public const string Description = "Description";

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
                    allAttributes.Add(PromotionName);
                    allAttributes.Add(StartDate);
                    allAttributes.Add(EndDate);
                    allAttributes.Add(Tags);
                    allAttributes.Add(Target);
                    allAttributes.Add(ChannelIDCollection);
                    allAttributes.Add(IDsCollection);
                    allAttributes.Add(ExcludeChannelIDCollection);
                    allAttributes.Add(ExcludeIDsCollection);
                    allAttributes.Add(IfAmount);
                    allAttributes.Add(IfCount);
                    allAttributes.Add(Discount);
                    allAttributes.Add(ReturnAmount);
                    allAttributes.Add(IsReturnMultiply);
                    allAttributes.Add(IsShipmentFree);
                    allAttributes.Add(IsGift);
                    allAttributes.Add(GiftName);
                    allAttributes.Add(GiftUrl);
                    allAttributes.Add(IsEnabled);
                    allAttributes.Add(Taxis);
                    allAttributes.Add(AddDate);
                    allAttributes.Add(Description);
                }

                return allAttributes;
            }
        }
    }
   public class PromotionInfo : BaseInfo
    {
        public PromotionInfo() { }
        public PromotionInfo(object dataItem) : base(dataItem) { }
        public PromotionInfo(NameValueCollection form) : base(form) { }
        public PromotionInfo(IDataReader rdr) : base(rdr) { }
        public int PublishmentSystemID { get; set; }
        public string PromotionName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Tags { get; set; }
        public string Target { get; set; }
        public string ChannelIDCollection { get; set; }
        public string IDsCollection { get; set; }
        public string ExcludeChannelIDCollection { get; set; }
        public string ExcludeIDsCollection { get; set; }
        public decimal IfAmount { get; set; }
        public int IfCount { get; set; }
        public decimal Discount { get; set; }
        public decimal ReturnAmount { get; set; }
        public bool IsReturnMultiply { get; set; }
        public bool IsShipmentFree { get; set; }
        public bool IsGift { get; set; }
        public string GiftName { get; set; }
        public string GiftUrl { get; set; }
        public bool IsEnabled { get; set; }
        public int Taxis { get; set; }
        public DateTime AddDate { get; set; }
        public string Description { get; set; }

        protected override List<string> AllAttributes
        {
            get
            {
                return PromotionAttribute.AllAttributes;
            }
        }
    }
}
