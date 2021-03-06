﻿using BaiRong.Core;
using BaiRong.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace SiteServer.WeiXin.Model
{
    public class CouponSNAttribute
    {
        protected CouponSNAttribute()
        {
        }

        public const string ID = "ID";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string CouponID = "CouponID";
        public const string SN = "SN";
        public const string Status = "Status";
        public const string HoldDate = "HoldDate";
        public const string HoldMobile = "HoldMobile";
        public const string HoldEmail = "HoldEmail";
        public const string HoldWXCode = "HoldWXCode";
        public const string CashDate = "CashDate";
        public const string CashUserName = "CashUserName";

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
                    allAttributes.Add(CouponID);
                    allAttributes.Add(SN);
                    allAttributes.Add(Status);
                    allAttributes.Add(HoldDate);
                    allAttributes.Add(HoldMobile);
                    allAttributes.Add(HoldEmail);
                    allAttributes.Add(HoldWXCode);
                    allAttributes.Add(CashDate);
                    allAttributes.Add(CashUserName);
                }

                return allAttributes;
            }
        }
    }
    public class CouponSNInfo : BaseInfo
    {
        public CouponSNInfo()
        {

        }
        public CouponSNInfo(object dataItem)
            : base(dataItem)
        {

        }
        public int PublishmentSystemID { get; set; }
        public int CouponID { get; set; }
        public string SN { get; set; }
        public string Status { get; set; }
        public DateTime HoldDate { get; set; }
        public string HoldMobile { get; set; }
        public string HoldEmail { get; set; }
        public string HoldWXCode { get; set; }
        public DateTime CashDate { get; set; }
        public string CashUserName { get; set; }

        protected override List<string> AllAttributes
        {
            get
            {
                return CouponSNAttribute.AllAttributes;
            }
        }
    }
}
