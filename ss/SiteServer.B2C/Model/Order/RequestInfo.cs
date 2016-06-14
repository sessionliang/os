using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace SiteServer.B2C.Model
{
    public class RequestAttribute
    {
        protected RequestAttribute()
        {
        }

        public const string ID = "ID";
        public const string SN = "SN";
        public const string Status = "Status";
        public const string UserName = "UserName";
        public const string AdminUserName = "AdminUserName";
        public const string AddDate = "AddDate";
        public const string RequestType = "RequestType";
        public const string Subject = "Subject";
        public const string Website = "Website";
        public const string Email = "Email";
        public const string Mobile = "Mobile";
        public const string QQ = "QQ";
        public const string IsEstimate = "IsEstimate";
        public const string EstimateValue = "EstimateValue";
        public const string EstimateComment = "EstimateComment";
        public const string EstimateDate = "EstimateDate";

        private static List<string> allAttributes;
        public static List<string> AllAttributes
        {
            get
            {
                if (allAttributes == null)
                {
                    allAttributes = new List<string>();
                    allAttributes.Add(ID);
                    allAttributes.Add(SN);
                    allAttributes.Add(Status);
                    allAttributes.Add(UserName);
                    allAttributes.Add(AdminUserName);
                    allAttributes.Add(AddDate);
                    allAttributes.Add(RequestType);
                    allAttributes.Add(Subject);
                    allAttributes.Add(Website);
                    allAttributes.Add(Email);
                    allAttributes.Add(Mobile);
                    allAttributes.Add(QQ);
                    allAttributes.Add(IsEstimate);
                    allAttributes.Add(EstimateValue);
                    allAttributes.Add(EstimateComment);
                    allAttributes.Add(EstimateDate);
                }

                return allAttributes;
            }
        }
    }

    public class RequestInfo : BaseInfo
    {
        public RequestInfo(object dataItem) : base(dataItem) { }
        public RequestInfo(IDataReader rdr) : base(rdr) { }
        public RequestInfo(NameValueCollection form) : base(form) { }
        public string SN { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
        public string AdminUserName { get; set; }
        public DateTime AddDate { get; set; }
        public string RequestType { get; set; }
        public string Subject { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string QQ { get; set; }
        public bool IsEstimate { get; set; }
        public string EstimateValue { get; set; }
        public string EstimateComment { get; set; }
        public DateTime EstimateDate { get; set; }

        protected override List<string> AllAttributes
        {
            get
            {
                return RequestAttribute.AllAttributes;
            }
        }
    }
}
