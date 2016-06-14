using System;

namespace BaiRong.Model
{
    public class UserMessageInfo
    {
        private int id;
        private string messageFrom;
        private string messageTo;
        private EUserMessageType messageType;
        private int parentID;
        private bool isViewed;
        private DateTime addDate;
        private string content;
        private DateTime lastAddDate;
        private string lastContent;
        private string title;

        public UserMessageInfo()
        {
            this.id = 0;
            this.messageFrom = string.Empty;
            this.messageTo = string.Empty;
            this.messageType = EUserMessageType.Private;
            this.parentID = 0;
            this.isViewed = false;
            this.addDate = DateTime.MinValue;
            this.content = string.Empty;
            this.lastAddDate = DateTime.MinValue;
            this.lastContent = string.Empty;
            this.title = string.Empty;
        }

        public UserMessageInfo(int id, string messageFrom, string messageTo, EUserMessageType messageType, int parentID, bool isViewed, DateTime addDate, string content, DateTime lastAddDate, string lastContent)
        {
            this.id = id;
            this.messageFrom = messageFrom;
            this.messageTo = messageTo;
            this.messageType = messageType;
            this.parentID = parentID;
            this.isViewed = isViewed;
            this.addDate = addDate;
            this.content = content;
            this.lastAddDate = lastAddDate;
            this.lastContent = lastContent;
            this.title = string.Empty;
        }

        public UserMessageInfo(int id, string messageFrom, string messageTo, EUserMessageType messageType, int parentID, bool isViewed, DateTime addDate, string content, DateTime lastAddDate, string lastContent, string title)
        {
            this.id = id;
            this.messageFrom = messageFrom;
            this.messageTo = messageTo;
            this.messageType = messageType;
            this.parentID = parentID;
            this.isViewed = isViewed;
            this.addDate = addDate;
            this.content = content;
            this.lastAddDate = lastAddDate;
            this.lastContent = lastContent;
            this.title = title;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string MessageFrom
        {
            get { return messageFrom; }
            set { messageFrom = value; }
        }

        public string MessageTo
        {
            get { return messageTo; }
            set { messageTo = value; }
        }

        public EUserMessageType MessageType
        {
            get { return messageType; }
            set { messageType = value; }
        }

        public int ParentID
        {
            get { return parentID; }
            set { parentID = value; }
        }

        public bool IsViewed
        {
            get { return isViewed; }
            set { isViewed = value; }
        }

        public DateTime AddDate
        {
            get { return addDate; }
            set { addDate = value; }
        }

        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        public DateTime LastAddDate
        {
            get { return lastAddDate; }
            set { lastAddDate = value; }
        }

        public string LastContent
        {
            get { return lastContent; }
            set { lastContent = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
    }
}
