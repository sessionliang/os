using BaiRong.Model;
using SiteServer.B2C.Model;
using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SiteServer.API.Model.WX
{
    public class CardParameter
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsPoweredBy { get; set; }
        public string PoweredBy { get; set; }
        public bool IsAnonymous { get; set; }
        public bool IsBinding { get; set; }
        public bool IsExchange { get; set; }
        public bool IsSign { get; set; }
        public decimal ExchangeProportion { get; set; }
        public CardInfo CardInfo { get; set; }
        public CardSNInfo CardSNInfo { get; set; }
        public CardEntitySNInfo CardEntitySNInfo { get; set; }
        public UserInfo UserInfo { get; set; }
        public UserContactInfo UserContactInfo { get; set; }
        public List<string> SignRuleList { get; set; }
        public List<CardSignLogInfo> CardSignLogInfoList { get; set; }
        public List<UserCreditsLogInfo> UserCreditsLogInfoList { get; set; }
        public List<CardCashYearCountInfo> CardCashYearCountInfoList { get; set; }
    }
}