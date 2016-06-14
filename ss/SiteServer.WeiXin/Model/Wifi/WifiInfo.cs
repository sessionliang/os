using BaiRong.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteServer.WeiXin.Model
{
    public class WifiAttribute
    {
        protected WifiAttribute()
        {
        }

        public const string ID = "ID";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string KeywordID = "KeywordID";
        public const string PVCount = "PVCount";
        public const string IsDisabled = "IsDisabled";
        public const string Title = "Title";
        public const string ImageUrl = "ImageUrl";
        public const string Summary = "Summary";
        public const string BusinessID = "BusinessID";
        public const string CallBackString = "CallBackString";

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
                    allAttributes.Add(IsDisabled);
                    allAttributes.Add(PVCount);
                    allAttributes.Add(Title);
                    allAttributes.Add(ImageUrl);
                    allAttributes.Add(Summary);
                    allAttributes.Add(BusinessID);
                    allAttributes.Add(CallBackString);
                }

                return allAttributes;
            }
        }
    }

    public class WifiInfo : BaseInfo
    {
        public WifiInfo() { }
        public WifiInfo(object dataItem) : base(dataItem) { }
        public WifiInfo(NameValueCollection form) : base(form) { }
        public WifiInfo(IDataReader rdr) : base(rdr) { }
        public int PublishmentSystemID { get; set; }
        public int KeywordID { get; set; }
        public bool IsDisabled { get; set; }
        public int PVCount { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Summary { get; set; }
        public string BusinessID { get; set; }
        public string CallBackString { get; set; }
        protected override List<string> AllAttributes
        {
            get
            {
                return WifiAttribute.AllAttributes;
            }
        }
    }

}
