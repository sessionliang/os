using System;

namespace SiteServer.CMS.Model
{
	public class MailSubscribeInfo
	{
        private int id;
        private int publishmentSystemID;
        private string receiver;
        private string mail;
        private string ipAddress;
        private DateTime addDate;

		public MailSubscribeInfo()
		{
            this.id = 0;
            this.publishmentSystemID = 0;
            this.receiver = string.Empty;
            this.mail = string.Empty;
            this.ipAddress = string.Empty;
            this.addDate = DateTime.Now;
		}

        public MailSubscribeInfo(int id, int publishmentSystemID, string receiver, string mail, string ipAddress, DateTime addDate) 
		{
            this.id = id;
            this.publishmentSystemID = publishmentSystemID;
            this.receiver = receiver;
            this.mail = mail;
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
