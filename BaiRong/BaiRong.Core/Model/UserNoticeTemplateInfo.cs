using System;

namespace BaiRong.Model
{
    public class UserNoticeTemplateInfo
    {
        private int id;
        private EUserNoticeType relatedIdentity;
        private string name;
        private string title;
        private string content;
        private EUserNoticeTemplateType type;
        private bool isSystem;
        private bool isEnable;
        private ESMSServerType remoteType;
        private string remoteTemplateID;


        public UserNoticeTemplateInfo()
        {
            this.id = 0;
            this.relatedIdentity = EUserNoticeType.ValidateForRegiste;
            this.name = string.Empty;
            this.title = string.Empty;
            this.content = string.Empty;
            this.type = EUserNoticeTemplateType.Message;
            this.isEnable = true;
            this.remoteTemplateID = string.Empty;
            this.remoteType = ESMSServerType.YunPian;

        }

        public UserNoticeTemplateInfo(int id, EUserNoticeType userNoticeType, string name, string title, string content, EUserNoticeTemplateType userNoticeTemplateType, bool isEnable, bool isSystem, string remoteTemplateID,ESMSServerType remoteType)
        {
            this.id = id;
            this.relatedIdentity = userNoticeType;
            this.name = name;
            this.title = title;
            this.content = content;
            this.type = userNoticeTemplateType;
            this.isEnable = isEnable;
            this.isSystem = isSystem;
            this.remoteTemplateID = remoteTemplateID;
            this.remoteType = remoteType;
        }


        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public EUserNoticeType RelatedIdentity
        {
            get { return relatedIdentity; }
            set { relatedIdentity = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        public EUserNoticeTemplateType Type
        {
            get { return type; }
            set { type = value; }
        }

        public bool IsEnable
        {
            get { return isEnable; }
            set { isEnable = value; }
        }

        public bool IsSystem
        {
            get { return isSystem; }
            set { isSystem = value; }
        }

        public string RemoteTemplateID
        {
            get { return remoteTemplateID; }
            set { remoteTemplateID = value; }
        }

        public ESMSServerType RemoteType {
            get { return remoteType; }
            set { remoteType = value; }
        }
    }
}
