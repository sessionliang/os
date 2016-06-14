using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.Project.Model
{
    public class RequestAnswerAttribute
    {
        protected RequestAnswerAttribute()
        {
        }

        //hidden
        public const string ID = "ID";
        public const string RequestID = "RequestID";

        //basic
        public const string IsAnswer = "IsAnswer";
        public const string Content = "Content";
        public const string FileUrl = "FileUrl";
        public const string AddDate = "AddDate";

        public static ArrayList AllAttributes
        {
            get
            {
                ArrayList arraylist = new ArrayList(HiddenAttributes);
                arraylist.AddRange(BasicAttributes);
                return arraylist;
            }
        }

        private static ArrayList hiddenAttributes;
        public static ArrayList HiddenAttributes
        {
            get
            {
                if (hiddenAttributes == null)
                {
                    hiddenAttributes = new ArrayList();
                    hiddenAttributes.Add(ID.ToLower());
                    hiddenAttributes.Add(RequestID.ToLower());
                }

                return hiddenAttributes;
            }
        }

        private static ArrayList basicAttributes;
        public static ArrayList BasicAttributes
        {
            get
            {
                if (basicAttributes == null)
                {
                    basicAttributes = new ArrayList();
                    basicAttributes.Add(IsAnswer.ToLower());
                    basicAttributes.Add(Content.ToLower());
                    basicAttributes.Add(FileUrl.ToLower());
                    basicAttributes.Add(AddDate.ToLower());
                }

                return basicAttributes;
            }
        }
    }

    public class RequestAnswerInfo : ExtendedAttributes
    {
        public const string TableName = "crm_RequestAnswer";

        public RequestAnswerInfo()
        {
        }

        public RequestAnswerInfo(object dataItem)
            : base(dataItem)
        {
        }

        public int ID
        {
            get { return base.GetInt(RequestAnswerAttribute.ID, 0); }
            set { base.SetExtendedAttribute(RequestAnswerAttribute.ID, value.ToString()); }
        }

        public int RequestID
        {
            get { return base.GetInt(RequestAnswerAttribute.RequestID, 0); }
            set { base.SetExtendedAttribute(RequestAnswerAttribute.RequestID, value.ToString()); }
        }

        public bool IsAnswer
        {
            get { return base.GetBool(RequestAnswerAttribute.IsAnswer, false); }
            set { base.SetExtendedAttribute(RequestAnswerAttribute.IsAnswer, value.ToString()); }
        }

        public string Content
        {
            get { return base.GetExtendedAttribute(RequestAnswerAttribute.Content); }
            set { base.SetExtendedAttribute(RequestAnswerAttribute.Content, value); }
        }

        public string FileUrl
        {
            get { return base.GetExtendedAttribute(RequestAnswerAttribute.FileUrl); }
            set { base.SetExtendedAttribute(RequestAnswerAttribute.FileUrl, value); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(RequestAnswerAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(RequestAnswerAttribute.AddDate, value.ToString()); }
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return RequestAnswerAttribute.AllAttributes;
        }
    }
}
