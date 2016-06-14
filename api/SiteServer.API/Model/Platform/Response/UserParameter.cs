using BaiRong.Model;
using SiteServer.B2C.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SiteServer.API.Model
{
    public class UserParameter
    {
        public User User { get; set; }
        public bool IsAnonymous { get; set; }

        public bool HasNewMsg { get; set; }

        public int NewMsgCount { get; set; }
    }
}