﻿using BaiRong.Core;
using BaiRong.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace SiteServer.WeiXin.Model
{
    public class VoteLogAttribute
    {
        protected VoteLogAttribute()
        {
        }

        public const string ID = "ID";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string VoteID = "VoteID";
        public const string ItemIDCollection = "ItemIDCollection";
        public const string IPAddress = "IPAddress";
        public const string UserName = "UserName";
        public const string WXOpenID = "WXOpenID";
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
                    allAttributes.Add(VoteID);
                    allAttributes.Add(ItemIDCollection);
                    allAttributes.Add(IPAddress);
                    allAttributes.Add(UserName);
                    allAttributes.Add(WXOpenID);
                    allAttributes.Add(AddDate);
                }

                return allAttributes;
            }
        }
    }
    public class VoteLogInfo : BaseInfo
    {
        public VoteLogInfo() { }
        public VoteLogInfo(object dataItem) : base(dataItem) { }
        public VoteLogInfo(NameValueCollection form) : base(form) { }
        public int PublishmentSystemID { get; set; }
        public int VoteID { get; set; }
        public string ItemIDCollection { get; set; }
        public string IPAddress { get; set; }
        public string UserName { get; set; }
        public string WXOpenID { get; set; }
        public DateTime AddDate { get; set; }

        protected override List<string> AllAttributes
        {
            get
            {
                return VoteLogAttribute.AllAttributes;
            }
        }
    }
}
