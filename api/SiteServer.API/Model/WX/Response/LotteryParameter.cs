using SiteServer.B2C.Model;
using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SiteServer.API.Model.WX
{
    public class LotteryParameter
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsPoweredBy { get; set; }
        public string PoweredBy { get; set; }
        public LotteryInfo LotteryInfo { get; set; }
        public List<LotteryAwardInfo> AwardInfoList { get; set; }
        public LotteryAwardInfo AwardInfo { get; set; }
        public LotteryWinnerInfo WinnerInfo { get; set; }
        public decimal Angle { get; set; }
        public bool IsEnd { get; set; }
    }
}