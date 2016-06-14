using System;
using System.Web;
using System.Xml.Serialization;
using BaiRong.Model;

namespace SiteServer.Project.Model
{
    public class DBLicenseInfo
    {
        private int id;
        private string domain;
        private string macAddress;
        private string processorID;
        private string columnSerialNumber;
        private int maxSiteNumber;
        private DateTime licenseDate;
        private string siteName;
        private string clientName;
        private int accountID;
        private int orderID;
        private DateTime expireDate;
        private string summary;

        public DBLicenseInfo()
        {
            this.id = 0;
            this.domain = string.Empty;
            this.macAddress = string.Empty;
            this.processorID = string.Empty;
            this.columnSerialNumber = string.Empty;
            this.maxSiteNumber = 0;
            this.licenseDate = DateTime.Now;
            this.siteName = string.Empty;
            this.clientName = string.Empty;
            this.accountID = 0;
            this.orderID = 0;
            this.expireDate = DateTime.Now;
            this.summary = string.Empty;
        }

        public DBLicenseInfo(int id, string domain, string macAddress, string processorID, string columnSerialNumber, int maxSiteNumber, DateTime licenseDate, string siteName, string clientName, int accountID, int orderID, DateTime expireDate, string summary)
        {
            this.id = id;
            this.domain = domain;
            this.macAddress = macAddress;
            this.processorID = processorID;
            this.columnSerialNumber = columnSerialNumber;
            this.maxSiteNumber = maxSiteNumber;
            this.licenseDate = licenseDate;
            this.siteName = siteName;
            this.clientName = clientName;
            this.accountID = accountID;
            this.orderID = orderID;
            this.expireDate = expireDate;
            this.summary = summary;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Domain
        {
            get { return domain; }
            set { domain = value; }
        }

        public string MacAddress
        {
            get { return macAddress; }
            set { macAddress = value; }
        }

        public string ProcessorID
        {
            get { return processorID; }
            set { processorID = value; }
        }

        public string ColumnSerialNumber
        {
            get { return columnSerialNumber; }
            set { columnSerialNumber = value; }
        }

        public int MaxSiteNumber
        {
            get { return maxSiteNumber; }
            set { maxSiteNumber = value; }
        }

        public DateTime LicenseDate
        {
            get { return licenseDate; }
            set { licenseDate = value; }
        }

        public string SiteName
        {
            get { return siteName; }
            set { siteName = value; }
        }

        public string ClientName
        {
            get { return clientName; }
            set { clientName = value; }
        }

        public int AccountID
        {
            get { return accountID; }
            set { accountID = value; }
        }

        public int OrderID
        {
            get { return orderID; }
            set { orderID = value; }
        }

        public DateTime ExpireDate
        {
            get { return expireDate; }
            set { expireDate = value; }
        }

        public string Summary
        {
            get { return summary; }
            set { summary = value; }
        }
    }
}
