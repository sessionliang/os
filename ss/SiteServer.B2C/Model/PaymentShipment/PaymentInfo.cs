using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Model;

namespace SiteServer.B2C.Model
{
    public class PaymentInfo
    {
        private int id;
        private int publishmentSystemID;
        private EPaymentType paymentType;
        private string paymentName;
        private bool isEnabled;
        private bool isOnline;
        private int taxis;
        private string description;
        private string settingsXML;

        public PaymentInfo()
        {
            this.id = 0;
            this.publishmentSystemID = 0;
            this.paymentType = EPaymentType.COD;
            this.paymentName = string.Empty;
            this.isEnabled = true;
            this.isOnline = true;
            this.taxis = 0;
            this.description = string.Empty;
            this.settingsXML = string.Empty;
        }

        public PaymentInfo(int id, int publishmentSystemID, EPaymentType paymentType, string paymentName, bool isEnabled, bool isOnline, int taxis, string description, string settingsXML)
        {
            this.id = id;
            this.publishmentSystemID = publishmentSystemID;
            this.paymentType = paymentType;
            this.paymentName = paymentName;
            this.isEnabled = isEnabled;
            this.isOnline = isOnline;
            this.taxis = taxis;
            this.description = description;
            this.settingsXML = settingsXML;
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

        public EPaymentType PaymentType
        {
            get { return paymentType; }
            set { paymentType = value; }
        }

        public string PaymentName
        {
            get { return paymentName; }
            set { paymentName = value; }
        }        

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        public bool IsOnline
        {
            get { return isOnline; }
            set { isOnline = value; }
        }

        public int Taxis
        {
            get { return taxis; }
            set { taxis = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string SettingsXML
        {
            get { return settingsXML; }
            set { settingsXML = value; }
        }
    }
}