using SiteServer.B2C.Model;
using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SiteServer.API.Model.WX
{
    public class VoteParameter
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsPoweredBy { get; set; }
        public string PoweredBy { get; set; }
        public bool IsVoted { get; set; }
        public VoteInfo VoteInfo { get; set; }
        public List<VoteItemInfo> ItemList { get; set; }
        public bool IsEnd { get; set; }
    }
}