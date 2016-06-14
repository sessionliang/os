using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace BaiRong.Model
{
    public class PayRecordAttribute
    {
        public PayRecordAttribute()
       { }
        public static string RecordID = "RecordID";
        public static string OrderSN = "OrderSN";
        public static string UserName = "UserName";
        public static string Price = "Price";
        public static string PayTime = "PayTime";
        public static string IP = "IP";
        public static string SettingsXML = "SettingsXML";
        public static string ApiType = "ApiType";

        private static ArrayList payRecordInfoAttribute;
        public static ArrayList PayRecordInfoAttribute
        {
            get
            {
                if (payRecordInfoAttribute == null)
                {
                    payRecordInfoAttribute = new ArrayList();
                    payRecordInfoAttribute.Add(RecordID.ToLower());
                    payRecordInfoAttribute.Add(OrderSN.ToLower());
                    payRecordInfoAttribute.Add(UserName.ToLower());
                    payRecordInfoAttribute.Add(Price.ToLower());
                    payRecordInfoAttribute.Add(PayTime.ToLower());
                    payRecordInfoAttribute.Add(IP.ToLower());
                    payRecordInfoAttribute.Add(SettingsXML.ToLower());
                    payRecordInfoAttribute.Add(ApiType.ToLower());
                }

                return payRecordInfoAttribute;
            }
        }
    }
}
