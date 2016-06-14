using System;
using System.Collections.Generic;

namespace BaiRong.Model
{
    public class SMSMessageInfo
    {
        private int id;
        private string mobilesList;
        private string smsContent;
        private DateTime sendDate;
        private string smsUserName;

        public SMSMessageInfo()
        {
            this.id = 0;
            this.mobilesList = string.Empty;
            this.smsContent = string.Empty;
            this.sendDate = System.DateTime.Now;
            this.smsUserName = string.Empty;
        }

        public SMSMessageInfo(int id, string mobilesList, string smsContent, DateTime sendDate, string smsUserName)
        {
            this.id = id;
            this.mobilesList = mobilesList;
            this.smsContent = smsContent;
            this.sendDate = sendDate;
            this.smsUserName = smsUserName;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string MobilesList
        {
            get { return mobilesList; }
            set { mobilesList = value; }
        }

        public string SMSContent
        {
            get { return smsContent; }
            set { smsContent = value; }
        }

        public DateTime SendDate
        {
            get { return sendDate; }
            set { sendDate = value; }
        }

        public string SMSUserName
        {
            get { return smsUserName; }
            set { smsUserName = value; }
        }
    }
}
