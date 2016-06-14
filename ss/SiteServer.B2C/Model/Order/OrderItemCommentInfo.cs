using System;
using BaiRong.Model;
using System.Collections.Generic;

namespace SiteServer.B2C.Model
{
    public class OrderItemCommentAttribute
    {
        protected OrderItemCommentAttribute()
        {
        }

        //hidden
        public const string ID = "ID";

        //basic
        public const string OrderItemID = "OrderItemID";
        public const string Star = "Star";
        public const string Tags = "Tags";
        public const string Comment = "Comment";
        public const string IsAnonymous = "IsAnonymous";
        public const string OrderUrl = "OrderUrl";
        public const string AddDate = "AddDate";
        public const string AddUser = "AddUser";
        public const string GoodCount = "GoodCount";
        public const string ImageUrl = "ImageUrl";

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
                    allAttributes.Add(Star);
                    allAttributes.Add(Tags);
                    allAttributes.Add(Comment);
                    allAttributes.Add(IsAnonymous);
                    allAttributes.Add(OrderUrl);
                    allAttributes.Add(AddDate);
                    allAttributes.Add(AddUser);
                    allAttributes.Add(GoodCount);
                    allAttributes.Add(ImageUrl);
                }

                return allAttributes;
            }
        }
    }
    public class OrderItemCommentInfo : BaseInfo
    {
        public int OrderItemID { get; set; }
        public int Star { get; set; }
        public string Tags { get; set; }
        public string Comment { get; set; }
        public bool IsAnonymous { get; set; }
        public string OrderUrl { get; set; }
        public DateTime AddDate { get; set; }
        public string AddUser { get; set; }
        public int GoodCount { get; set; }
        public string ImageUrl { get; set; }

        protected override List<string> AllAttributes
        {
            get
            {
                return OrderItemCommentAttribute.AllAttributes;
            }
        }
    }
}
