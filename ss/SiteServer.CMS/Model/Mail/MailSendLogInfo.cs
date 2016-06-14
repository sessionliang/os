using System;

namespace SiteServer.CMS.Model
{
	public class MailSendLogInfo
	{
        private int id;
        private int publishmentSystemID;
        private int channelID;
        private int contentID;
        private string title;
        private string pageUrl;
        private string receiver;
        private string mail;
        private string sender;
        private string ipAddress;
        private DateTime addDate;

		public MailSendLogInfo()
		{
            this.id = 0;
            this.publishmentSystemID = 0;
            this.channelID = 0;
            this.contentID = 0;
            this.title = string.Empty;
            this.pageUrl = string.Empty;
            this.receiver = string.Empty;
            this.mail = string.Empty;
            this.sender = string.Empty;
            this.ipAddress = string.Empty;
            this.addDate = DateTime.Now;
		}

        public MailSendLogInfo(int id, int publishmentSystemID, int channelID, int contentID, string title, string pageUrl, string receiver, string mail, string sender, string ipAddress, DateTime addDate) 
		{
            this.id = id;
            this.publishmentSystemID = publishmentSystemID;
            this.channelID = channelID;
            this.contentID = contentID;
            this.title = title;
            this.pageUrl = pageUrl;
            this.receiver = receiver;
            this.mail = mail;
            this.sender = sender;
            this.ipAddress = ipAddress;
            this.addDate = addDate;
		}

        public int ID
		{
            get { return id; }
            set { id = value; }
		}

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public int ChannelID
        {
            get { return channelID; }
            set { channelID = value; }
        }

        public int ContentID
        {
            get { return contentID; }
            set { contentID = value; }
        }

        public string Title
		{
            get { return title; }
            set { title = value; }
		}

        public string PageUrl
        {
            get { return pageUrl; }
            set { pageUrl = value; }
        }

        public string Receiver
        {
            get { return receiver; }
            set { receiver = value; }
        }

        public string Mail
        {
            get { return mail; }
            set { mail = value; }
        }

        public string Sender
        {
            get { return sender; }
            set { sender = value; }
        }

        public string IPAddress
		{
            get { return ipAddress; }
            set { ipAddress = value; }
		}

        public DateTime AddDate
        {
            get { return addDate; }
            set { addDate = value; }
        }
	}
}
