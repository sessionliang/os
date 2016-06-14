using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace SiteServer.CRM.Model
{
    public class OrderMessageAttribute
    {
        protected OrderMessageAttribute()
        {
        }

        public const string ID = "ID";
        public const string MessageName = "MessageName";
        public const string IsSMS = "IsSMS";
        public const string IsEmail = "IsEmail";
        public const string TemplateSMS = "TemplateSMS";
        public const string TemplateEmail = "TemplateEmail";

        private static List<string> allAttributes;
        public static List<string> AllAttributes
        {
            get
            {
                if (allAttributes == null)
                {
                    allAttributes = new List<string>();
                    allAttributes.Add(ID);
                    allAttributes.Add(MessageName);
                    allAttributes.Add(IsSMS);
                    allAttributes.Add(IsEmail);
                    allAttributes.Add(TemplateSMS);
                    allAttributes.Add(TemplateEmail);
                }

                return allAttributes;
            }
        }
    }
    public class OrderMessageInfo : BaseInfo
    {
        public OrderMessageInfo(object dataItem) : base(dataItem) { }
        public OrderMessageInfo(IDataReader rdr) : base(rdr) { }
        public OrderMessageInfo(NameValueCollection form) : base(form) { }
        public string MessageName { get; set; }
        public bool IsSMS { get; set; }
        public bool IsEmail { get; set; }
        public string TemplateSMS { get; set; }
        public string TemplateEmail { get; set; }

        protected override List<string> AllAttributes
        {
            get
            {
                return OrderMessageAttribute.AllAttributes;
            }
        }
    }
}
