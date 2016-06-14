using SiteServer.B2C.Model;
using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SiteServer.API.Model.WX
{
    public class AppointmentParameter
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsPoweredBy { get; set; }
        public string PoweredBy { get; set; }
        public AppointmentInfo AppointmentInfo { get; set; }
        public AppointmentItemInfo AppointmentItemInfo { get; set; }
        public AppointmentContentInfo AppointmentContentInfo { get; set; }
        public List<AppointmentItemInfo> AppointmentItemInfoList { get; set; }
        public List<AppointmentContentInfo> AppointmentContentInfoList { get; set; }

        public List<ConfigExtendInfo> ConfigExtendInfoList { get; set; }

        public bool IsEnd { get; set; }
    }
}