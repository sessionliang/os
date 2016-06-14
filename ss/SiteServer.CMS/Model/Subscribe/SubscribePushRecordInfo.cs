using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.CMS.Model
{
    public class SubscribePushRecordAttribute
    {
        protected SubscribePushRecordAttribute()
        {
        }

        public const string RecordID = "RecordID";
        public const string Email = "Email";
        public const string UserID = "UserID";
        public const string SubscriptionTemplate = "SubscriptionTemplate";
        public const string Mobile = "Mobile";
        public const string SubscribeName = "SubscribeName";
        public const string SubscribeSendRecordID = "SubscribeSendRecordID";
        public const string PushType = "PushType";
        public const string PushStatu = "PushStatu";
        public const string TaskID = "TaskID";
        public const string AddDate = "AddDate";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string UserName = "UserName";

        private static ArrayList allAttributes;
        public static ArrayList AllAttributes
        {
            get
            {
                if (allAttributes == null)
                {
                    allAttributes = new ArrayList();
                    allAttributes.Add(RecordID.ToLower());
                    allAttributes.Add(Email.ToLower());
                    allAttributes.Add(UserID.ToLower());
                    allAttributes.Add(SubscriptionTemplate.ToLower());
                    allAttributes.Add(Mobile.ToLower());
                    allAttributes.Add(SubscribeName.ToLower());
                    allAttributes.Add(PushType.ToLower());
                    allAttributes.Add(PushStatu.ToLower());
                    allAttributes.Add(TaskID.ToLower());
                    allAttributes.Add(SubscribeSendRecordID.ToLower());
                    allAttributes.Add(AddDate.ToLower());
                    allAttributes.Add(PublishmentSystemID.ToLower());
                    allAttributes.Add(UserName.ToLower()); 
                }
                return allAttributes;
            }
        } 
         
    }

    public class SubscribePushRecordInfo : ExtendedAttributes
    {
        public const string TableName = "siteserver_SubscribePushRecord";

        public SubscribePushRecordInfo()
        {
            this.RecordID = 0;
            this.Email = string.Empty;
            this.UserID = 0;
            this.SubscribeSendRecordID = 0;
            this.Mobile = string.Empty;
            this.SubscriptionTemplate = string.Empty;
            this.TaskID = 0;
            this.SubscribeName = string.Empty;
            this.PushStatu = EBoolean.True;
            this.SubscriptionTemplate = string.Empty;
            this.AddDate = DateTime.Now;
            this.PublishmentSystemID = 0;
            this.PushType = ESubscribePushType.TimedPush;
        }

        public SubscribePushRecordInfo(object dataItem)
            : base(dataItem)
		{
		}

        public SubscribePushRecordInfo(int id, string email, int userID, string mobile, int subscribeSendRecordID, ESubscribePushType pushType, string subscribeName, string subscriptionTemplate, DateTime addDate, int publishmentSystemID, bool pushStatu, int taskID, string userName)
        {
            this.RecordID = id;
            this.Email = email;
            this.UserID = userID;
            this.SubscribeSendRecordID = subscribeSendRecordID;
            this.PushType =pushType;
            this.Mobile = mobile;
            this.SubscribeName = subscribeName;
            this.SubscriptionTemplate = subscriptionTemplate;
            this.AddDate = addDate;
            this.PublishmentSystemID = publishmentSystemID;
            this.PushStatu = EBoolean.True;
            this.TaskID = taskID;
            this.UserName = userName;
        }

        public int RecordID
        {
            get { return base.GetInt(SubscribePushRecordAttribute.RecordID, 0); }
            set { base.SetExtendedAttribute(SubscribePushRecordAttribute.RecordID, value.ToString()); }
        }

        public string Email
        {
            get { return base.GetExtendedAttribute(SubscribePushRecordAttribute.Email); }
            set { base.SetExtendedAttribute(SubscribePushRecordAttribute.Email, value); }
        }

        public int UserID
        {
            get { return base.GetInt(SubscribePushRecordAttribute.UserID,0); }
            set { base.SetExtendedAttribute(SubscribePushRecordAttribute.UserID, value.ToString()); }
        }

        public int SubscribeSendRecordID
        {
            get { return base.GetInt(SubscribePushRecordAttribute.SubscribeSendRecordID, 0); }
            set { base.SetExtendedAttribute(SubscribePushRecordAttribute.SubscribeSendRecordID, value.ToString()); }
        }

        public string Mobile
        {
            get { return base.GetExtendedAttribute(SubscribePushRecordAttribute.Mobile); }
            set { base.SetExtendedAttribute(SubscribePushRecordAttribute.Mobile, value); }
        }

        public string SubscribeName
        {
            get { return base.GetExtendedAttribute(SubscribePushRecordAttribute.SubscribeName); }
            set { base.SetExtendedAttribute(SubscribePushRecordAttribute.SubscribeName, value); }
        }

        public ESubscribePushType PushType
        {
            get { return ESubscribePushTypeUtils.GetEnumType(SubscribePushRecordAttribute.PushType); }
            set { base.SetExtendedAttribute(SubscribePushRecordAttribute.PushType, value.ToString()); }
        }
        public EBoolean PushStatu
        {
            get { return EBooleanUtils.GetEnumType(SubscribePushRecordAttribute.PushStatu); }
            set { base.SetExtendedAttribute(SubscribePushRecordAttribute.PushStatu, value.ToString()); }
        }

        public string SubscriptionTemplate
        {
            get { return base.GetExtendedAttribute(SubscribePushRecordAttribute.SubscriptionTemplate); }
            set { base.SetExtendedAttribute(SubscribePushRecordAttribute.SubscriptionTemplate, value); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(SubscribeSetAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(SubscribeSetAttribute.AddDate, value.ToString()); }
        }

        public int PublishmentSystemID
        {
            get { return base.GetInt(SubscribePushRecordAttribute.PublishmentSystemID, 0); }
            set { base.SetExtendedAttribute(SubscribePushRecordAttribute.PublishmentSystemID, value.ToString()); }
        }

        public int TaskID
        {
            get { return base.GetInt(SubscribePushRecordAttribute.TaskID, 0); }
            set { base.SetExtendedAttribute(SubscribePushRecordAttribute.TaskID, value.ToString()); }
        } 

        public string UserName
        {
            get { return base.GetExtendedAttribute(SubscribePushRecordAttribute.UserName); }
            set { base.SetExtendedAttribute(SubscribePushRecordAttribute.UserName, value); }
        }
        protected override ArrayList GetDefaultAttributesNames()
        {
            return SubscribePushRecordAttribute.AllAttributes;
        }
    }
}
