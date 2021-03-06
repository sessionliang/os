﻿using BaiRong.Core;
using BaiRong.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace SiteServer.WeiXin.Model
{
    public class CouponAttribute
    {
        protected CouponAttribute()
        {
        }

        public const string ID = "ID";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string ActID = "ActID";
        public const string Title = "Title";
        public const string TotalNum = "TotalNum";
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
                    allAttributes.Add(ActID);
                    allAttributes.Add(Title);
                    allAttributes.Add(TotalNum);
                    allAttributes.Add(AddDate);
                }

                return allAttributes;
            }
        }
    }
    public class CouponInfo : BaseInfo
    {
        public CouponInfo()
        {

        }
        public CouponInfo(object dataItem)
            : base(dataItem)
        {

        }
        public int PublishmentSystemID { get; set; }
        public int ActID { get; set; }
        public string Title { get; set; }
        public int TotalNum { get; set; }
        public DateTime AddDate { get; set; }

        protected override List<string> AllAttributes
        {
            get
            {
                return CouponAttribute.AllAttributes;
            }
        }
    }
}
