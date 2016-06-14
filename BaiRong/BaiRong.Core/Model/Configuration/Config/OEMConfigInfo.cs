using System;
using System.Collections;
using System.Text;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Xml;
using BaiRong.Model;

namespace BaiRong.Core.Configuration
{
    public class OEMConfigInfo
    {
        private bool isOEM;
        private string companyName;
        private string companyUrl;
        private string productName;
        private string productUrl;

        public OEMConfigInfo() { }

        public bool IsOEM
        {
            get { return this.isOEM; }
            set { this.isOEM = value; }
        }

        public string CompanyName
        {
            get
            {
                if (!this.isOEM)
                {
                    return StringUtils.Constants.CompanyName;
                }
                return this.companyName;
            }
            set { this.companyName = value; }
        }

        public string CompanyUrl
        {
            get
            {
                if (!this.isOEM)
                {
                    return StringUtils.Constants.CompanyUrl;
                }
                return this.companyUrl;
            }
            set { this.companyUrl = value; }
        }

        public string ProductName
        {
            get
            {
                if (!this.isOEM)
                {
                    return StringUtils.Constants.ProductName;
                }
                return this.productName;
            }
            set { this.productName = value; }
        }

        public string ProductUrl
        {
            get
            {
                if (!this.isOEM)
                {
                    return StringUtils.Constants.ProductUrl;
                }
                return this.productUrl;
            }
            set { this.productUrl = value; }
        }
    }
}
