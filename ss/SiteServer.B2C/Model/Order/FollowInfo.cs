using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using SiteServer.CMS.Model;

namespace SiteServer.B2C.Model
{
    public class FollowAttribute
    {
        protected FollowAttribute()
        {
        }

        public const string ID = "ID";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string UserName = "UserName";
        public const string ChannelID = "ChannelID";
        public const string ContentID = "ContentID";
        public const string AddDate = "AddDate";

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
                    allAttributes.Add(UserName);
                    allAttributes.Add(ChannelID);
                    allAttributes.Add(ContentID);
                    allAttributes.Add(AddDate);
                }

                return allAttributes;
            }
        }
    }

    public class FollowInfo : BaseInfo
    {
        public FollowInfo() : base() { }
        public FollowInfo(object dataItem) : base(dataItem) { }
        public FollowInfo(IDataReader rdr) : base(rdr) { }
        public FollowInfo(NameValueCollection form) : base(form) { }
        public int PublishmentSystemID { get; set; }
        public string UserName { get; set; }
        public int ChannelID { get; set; }
        public int ContentID { get; set; }
        public DateTime AddDate { get; set; }

        public GoodsContentInfo ContentInfo { get; set; }
        public string NavigationUrl { get; set; }

        protected override List<string> AllAttributes
        {
            get
            {
                return FollowAttribute.AllAttributes;
            }
        }
    }
}
