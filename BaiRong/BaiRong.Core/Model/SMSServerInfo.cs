using System;
using BaiRong.Model;
using System.Collections.Specialized;
using System.Net;

namespace BaiRong.Model
{
    public class SMSServerInfo
    {
        private int smsServerID;
        private string smsServerName;
        private string smsServerEName;
        private NameValueCollection paramCollection;
        private bool isEnable;
        private SMSServerInfoExtend additional;
        private string extendValues;



        public SMSServerInfo()
        {
            this.SMSServerID = 0;
            this.SMSServerName = string.Empty;
            this.SMSServerEName = string.Empty;
            this.ParamCollection = new NameValueCollection();
            this.IsEnable = isEnable;
            this.additional = new SMSServerInfoExtend(string.Empty);
        }

        public SMSServerInfo(int SMSServerID, string smsName, string smsEName, NameValueCollection paramCollection, bool isEnable, string extendValues)
        {
            this.SMSServerID = SMSServerID;
            this.SMSServerName = smsName;
            this.SMSServerEName = smsEName;
            this.ParamCollection = paramCollection;
            this.IsEnable = isEnable;
            this.ExtendValues = extendValues;
        }

        public int SMSServerID
        {
            get { return smsServerID; }
            set { smsServerID = value; }
        }

        public string SMSServerName
        {
            get { return this.smsServerName; }
            set { this.smsServerName = value; }
        }

        public string SMSServerEName
        {
            get { return this.smsServerEName; }
            set { this.smsServerEName = value; }
        }


        public NameValueCollection ParamCollection
        {
            get { return this.paramCollection; }
            set { this.paramCollection = value; }
        }

        public bool IsEnable
        {
            get { return this.isEnable; }
            set { this.isEnable = value; }
        }

        public string ExtendValues
        {
            get
            {
                return extendValues;
            }
            set
            {
                extendValues = value;
            }
        }

        public SMSServerInfoExtend Additional
        {
            get
            {
                if (this.additional == null)
                {
                    this.additional = new SMSServerInfoExtend(this.extendValues);
                }
                return this.additional;
            }
            set
            {
                this.additional = value;
            }
        }
    }
}
