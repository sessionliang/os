using SiteServer.B2C.Model;
using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteServer.API.Model.WX
{
    public class CouponParameter
    {
        public bool IsSuccess { get; set; }
        public bool IsPoweredBy { get; set; }
        public string PoweredBy { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string SN { get; set; }
        public string Status { get; set; }
        public string HoldDate { get; set; }
        public string Description { get; set; }
        public string Usage { get; set; }
        public string AwardCode { get; set; }
        public int SnID { get; set; }
        public List<Coupon> CouponList { get; set; }
        public CouponActInfo ConponActInfo { get; set; }
        public CouponSNInfo CouponSNInfo { get; set; }
        public string ErrorMessage { get; set; }
    }
}