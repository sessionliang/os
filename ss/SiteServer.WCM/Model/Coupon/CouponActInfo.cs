using BaiRong.Core;
using BaiRong.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace SiteServer.WeiXin.Model
{
    public class CouponActAttribute
    {
        protected CouponActAttribute()
        {
        }

        public const string ID = "ID";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string KeywordID = "KeywordID";
        public const string UserCount = "UserCount";
        public const string PVCount = "PVCount";
        public const string Template = "Template";
        public const string StartDate = "StartDate";
        public const string EndDate = "EndDate";
        public const string Title = "Title";
        public const string ImageUrl = "ImageUrl";
        public const string Summary = "Summary";
        public const string ContentImageUrl = "ContentImageUrl";
        public const string ContentUsage = "ContentUsage";
        public const string ContentDescription = "ContentDescription";
        public const string EndTitle = "EndTitle";
        public const string EndImageUrl = "EndImageUrl";
        public const string EndSummary = "EndSummary";

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
                    allAttributes.Add(KeywordID);
                    allAttributes.Add(UserCount);
                    allAttributes.Add(PVCount);
                    allAttributes.Add(Template);
                    allAttributes.Add(StartDate);
                    allAttributes.Add(EndDate);
                    allAttributes.Add(Title);
                    allAttributes.Add(ImageUrl);
                    allAttributes.Add(Summary);
                    allAttributes.Add(ContentImageUrl);
                    allAttributes.Add(ContentUsage);
                    allAttributes.Add(ContentDescription);
                    allAttributes.Add(EndTitle);
                    allAttributes.Add(EndImageUrl);
                    allAttributes.Add(EndSummary);
                }

                return allAttributes;
            }
        }
    }
    public class CouponActInfo : BaseInfo
    {
        public CouponActInfo()
        {

        }
        public CouponActInfo(object dataItem)
            : base(dataItem)
        {

        }
        public int PublishmentSystemID { get; set; }
        public int KeywordID { get; set; }
        public int UserCount { get; set; }
        public int PVCount { get; set; }
        public string Template { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Summary { get; set; }
        public string ContentImageUrl { get; set; }
        public string ContentUsage { get; set; }
        public string ContentDescription { get; set; }
        public string EndTitle { get; set; }
        public string EndImageUrl { get; set; }
        public string EndSummary { get; set; }

        protected override List<string> AllAttributes
        {
            get
            {
                return CouponActAttribute.AllAttributes;
            }
        }
    }
}
