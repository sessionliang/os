using System;
using System.Collections;
using System.Collections.Specialized;
using BaiRong.Model;

namespace BaiRong.Core
{
    public class LicenseInfo
	{
		private string domain;
		private string macAddress;
		private string processorID;
		private string columnSerialNumber;
        private int maxSiteNumber;
        private DateTime expireDate;

		public LicenseInfo()
		{
			this.domain = string.Empty;
			this.macAddress = string.Empty;
			this.processorID = string.Empty;
			this.columnSerialNumber = string.Empty;
            this.maxSiteNumber = 0;
            this.expireDate = DateTime.Now;
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

        public DateTime ExpireDate
        {
            get { return expireDate; }
            set { expireDate = value; }
        }
	}
}
