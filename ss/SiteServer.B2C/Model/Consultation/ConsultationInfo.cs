using System;
using BaiRong.Model;
using System.Collections.Generic;
using System.Data;
using System.Collections.Specialized;

namespace SiteServer.B2C.Model
{
    public class ConsultationAttribute
    {
        protected ConsultationAttribute()
        {
        }

        //hidden
        public const string ID = "ID";

        //basic
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string ContentID = "ContentID";
        public const string ChannelID = "ChannelID";
        public const string Title = "Title";
        public const string ThumbUrl = "ThumbUrl";
        public const string Question = "Question";
        public const string Answer = "Answer";
        public const string Type = "Type";
        public const string AddUser = "AddUser";
        public const string AddDate = "AddDate";
        public const string ReplyDate = "ReplyDate";
        public const string IsReply = "IsReply";

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
                    allAttributes.Add(ContentID);
                    allAttributes.Add(ChannelID);
                    allAttributes.Add(Title);
                    allAttributes.Add(ThumbUrl);
                    allAttributes.Add(Question);
                    allAttributes.Add(Answer);
                    allAttributes.Add(Type);
                    allAttributes.Add(AddUser);
                    allAttributes.Add(AddDate);
                    allAttributes.Add(ReplyDate);
                    allAttributes.Add(IsReply);
                }

                return allAttributes;
            }
        }
    }
    public class ConsultationInfo : BaseInfo
    {
        public ConsultationInfo() { }
        public ConsultationInfo(object dataItem) : base(dataItem) { }
        public ConsultationInfo(IDataReader rdr) : base(rdr) { }
        public ConsultationInfo(NameValueCollection form) : base(form) { }
        public int PublishmentSystemID { get; set; }
        public int ContentID { get; set; }
        public int ChannelID { get; set; }
        public string Title { get; set; }
        public string ThumbUrl { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Type { get; set; }
        public string AddUser { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime ReplyDate { get; set; }
        public bool IsReply { get; set; }

        protected override List<string> AllAttributes
        {
            get
            {
                return ConsultationAttribute.AllAttributes;
            }
        }
    }
}
