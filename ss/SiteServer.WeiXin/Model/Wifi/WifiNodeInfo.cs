using BaiRong.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteServer.WeiXin.Model
{
    public class WifiNodeAttribute
    {
        protected WifiNodeAttribute()
        {
        }

        public const string ID = "ID";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string BusinessID = "BusinessID";
        public const string NodeID = "NodeID";
        public const string CallBackString = "CallBackString";

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
                    allAttributes.Add(BusinessID);
                    allAttributes.Add(NodeID);
                    allAttributes.Add(CallBackString);
                }

                return allAttributes;
            }
        }
    }

    public class WifiNodeInfo : BaseInfo
    {
        public WifiNodeInfo() { }
        public WifiNodeInfo(object dataItem) : base(dataItem) { }
        public WifiNodeInfo(NameValueCollection form) : base(form) { }
        public WifiNodeInfo(IDataReader rdr) : base(rdr) { }
        public int PublishmentSystemID { get; set; }
        public string BusinessID { get; set; }
        public string NodeID { get; set; }
        public string CallBackString { get; set; }
        protected override List<string> AllAttributes
        {
            get
            {
                return WifiNodeAttribute.AllAttributes;
            }
        }
    }

}
