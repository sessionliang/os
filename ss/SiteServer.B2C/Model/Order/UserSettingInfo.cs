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
    public class UserSettingAttribute
    {
        protected UserSettingAttribute()
        {
        }

        public const string ID = "ID";
        public const string UserName = "UserName";
        public const string SessionID = "SessionID";
        public const string Province = "Province";

        private static List<string> allAttributes;
        public static List<string> AllAttributes
        {
            get
            {
                if (allAttributes == null)
                {
                    allAttributes = new List<string>();
                    allAttributes.Add(ID);
                    allAttributes.Add(UserName);
                    allAttributes.Add(SessionID);
                    allAttributes.Add(Province);
                }

                return allAttributes;
            }
        }
    }

    public class UserSettingInfo : BaseInfo
    {
        public UserSettingInfo() : base() { }
        public UserSettingInfo(object dataItem) : base(dataItem) { }
        public UserSettingInfo(IDataReader rdr) : base(rdr) { }
        public UserSettingInfo(NameValueCollection form) : base(form) { }
        public string UserName { get; set; }
        public string SessionID { get; set; }
        public string Province { get; set; }

        protected override List<string> AllAttributes
        {
            get
            {
                return UserSettingAttribute.AllAttributes;
            }
        }
    }
}
