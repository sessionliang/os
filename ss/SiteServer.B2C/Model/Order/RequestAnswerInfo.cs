using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace SiteServer.B2C.Model
{
    public class RequestAnswerAttribute
    {
        protected RequestAnswerAttribute()
        {
        }

        public const string ID = "ID";
        public const string RequestID = "RequestID";
        public const string IsAnswer = "IsAnswer";
        public const string Content = "Content";
        public const string FileUrl = "FileUrl";
        public const string AddDate = "AddDate";

        private static List<string> allAttributes;
        public static List<string> AllAttributes
        {
            get
            {
                if (allAttributes == null)
                {
                    allAttributes = new List<string>();
                    allAttributes.Add(ID);
                    allAttributes.Add(RequestID);
                    allAttributes.Add(IsAnswer);
                    allAttributes.Add(Content);
                    allAttributes.Add(FileUrl);
                    allAttributes.Add(AddDate);
                }

                return allAttributes;
            }
        }
    }

    public class RequestAnswerInfo : BaseInfo
    {
        public RequestAnswerInfo(object dataItem) : base(dataItem) { }
        public RequestAnswerInfo(IDataReader rdr) : base(rdr) { }
        public RequestAnswerInfo(NameValueCollection form) : base(form) { }
        public int RequestID { get; set; }
        public bool IsAnswer { get; set; }
        public string Content { get; set; }
        public string FileUrl { get; set; }
        public DateTime AddDate { get; set; }

        protected override List<string> AllAttributes
        {
            get
            {
                return RequestAnswerAttribute.AllAttributes;
            }
        }
    }
}
