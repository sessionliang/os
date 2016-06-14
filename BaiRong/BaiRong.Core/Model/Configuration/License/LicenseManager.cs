using System;
using System.Collections;
using System.Text;
using System.Web.Caching;
using System.Xml;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Diagnostics;
using BaiRong.Core.IO;
using BaiRong.Model;
using BaiRong.Core.Cryptography;
using BaiRong.Core;
using System.Collections.Specialized;

namespace BaiRong.Core
{
    public class LicenseManager
	{
		private readonly string licenseFilePath;

		private readonly bool isExists = false;
        private readonly bool isLegality = false;
		private readonly LicenseInfo licenseInfo = null;

        public static LicenseManager Instance
        {
            get
            {
                LicenseManager manager;
                string cacheKey = string.Format("License_{0}", PageUtils.GetHost());
                manager = CacheUtils.Get(cacheKey) as LicenseManager;
                if (manager == null)
                {
                    manager = new LicenseManager();
                    if (manager.IsExists)
                    {
                        CacheUtils.Max(cacheKey, manager, new CacheDependency(manager.LicenseFilePath));
                    }
                }
                return manager;
            }
        }

        public static void ClearCache()
        {
            string cacheKey = string.Format("License_{0}", PageUtils.GetHost());
            CacheUtils.Remove(cacheKey);
        }

        private LicenseManager()
		{
            this.licenseFilePath = PathUtils.GetSiteFilesPath("license.lic");
            this.licenseInfo = LicenseManager.GetLicenseInfo(this.licenseFilePath);
            this.isLegality = this.IsValidComplete();
		}

		public string LicenseFilePath
		{
			get{ return this.licenseFilePath;}
		}

		public bool IsExists
		{
			get{ return this.isExists; }
		}

        public bool IsLegality
		{
            get { return this.isLegality; }
		}

        public bool IsMaxSiteNumberLimited
        {
            get{ return (this.licenseInfo.MaxSiteNumber > 0); }
        }

        public int MaxSiteNumber
        {
            get{ return this.licenseInfo.MaxSiteNumber; }
        }

        public bool IsExpireDateLimited
        {
            get
            {
                //if (StringUtils.EqualsIgnoreCase(this.licenseInfo.ProductID, ProductManager.WCM.ModuleID))
                //{
                //    if (!this.isLegality)
                //    {
                //        return true;
                //    }
                //}
                return false;
            }
        }

        public DateTime ExpireDate
        {
            get { return this.licenseInfo.ExpireDate; }
        }

        public string Domain
        {
            get { return this.licenseInfo.Domain; }
        }

        public string MacAddress
        {
            get { return this.licenseInfo.MacAddress; }
        }

        public string ProcessorID
        {
            get { return this.licenseInfo.ProcessorID; }
        }

        public string ColumnSerialNumber
        {
            get { return this.licenseInfo.ColumnSerialNumber; }
        }

		#region LicenseManager Helper

        public static string GetPoweredByHtmlForNotOEM()
        {
            return "Powered by <a href='http://www.siteserver.cn' target='_blank'>SiteServer</a>";
        }

		private static LicenseInfo GetLicenseInfo(string licenseFilePath)
		{
			LicenseInfo licInfo = CacheUtils.Get(licenseFilePath) as LicenseInfo;
            if (licInfo == null)
			{
                licInfo = new LicenseInfo();
                if (FileUtils.IsFileExists(licenseFilePath))
                {
                    XmlDocument xmlDocument = GetXmlDocument(licenseFilePath);

                    //计算机信息
                    licInfo.Domain = XmlUtils.GetXmlNodeInnerText(xmlDocument, "//registration/computer/domain");
                    licInfo.MacAddress = XmlUtils.GetXmlNodeInnerText(xmlDocument, "//registration/computer/macAddress");
                    licInfo.ProcessorID = XmlUtils.GetXmlNodeInnerText(xmlDocument, "//registration/computer/processorID");
                    licInfo.ColumnSerialNumber = XmlUtils.GetXmlNodeInnerText(xmlDocument, "//registration/computer/columnSerialNumber");
                    //应用数量
                    licInfo.MaxSiteNumber = TranslateUtils.ToInt(XmlUtils.GetXmlNodeInnerText(xmlDocument, "//registration/product/maxSiteNumber"));
                    licInfo.ExpireDate = TranslateUtils.ToDateTime(XmlUtils.GetXmlNodeInnerText(xmlDocument, "//registration/product/expireDate"), DateTime.Now);

                    CacheUtils.Max(licenseFilePath, licInfo, new CacheDependency(licenseFilePath));
                }
			}

            return licInfo;
		}

        private bool IsValidComplete()
		{
            //if (!StringUtils.EqualsIgnoreCase(licenseInfo.ProductID, this.productID))
            //{
            //    return false;
            //}

            string host = PageUtils.GetHost();
            if (host.StartsWith("localhost") || host.StartsWith("127.0.0.1")) return true;

            //首先判断域名是否正确
            if (!string.IsNullOrEmpty(licenseInfo.Domain))
            {
                //string[] domains = licenseInfo.Domain.Split('|');
                //foreach (string theDomain in domains)
                //{
                //    if (StringUtils.StartsWithIgnoreCase(host, theDomain))
                //    {
                //        return true;
                //    }
                //}
                if (RegexUtils.IsMatch(licenseInfo.Domain, host))
                {
                    return true;
                }
            }

            //判断网卡是否正确
            if (!string.IsNullOrEmpty(licenseInfo.MacAddress))
            {
                string macAddress = ComputerUtils.GetMacAddress();
                if (StringUtils.EqualsIgnoreCase(macAddress, licenseInfo.MacAddress))
                {
                    return true;
                }
            }

            //判断CPU是否正确
            if (!string.IsNullOrEmpty(licenseInfo.ProcessorID))
            {
                string processorId = ComputerUtils.GetProcessorId();
                if (StringUtils.EqualsIgnoreCase(processorId, licenseInfo.ProcessorID))
                {
                    return true;
                }
            }

            //判断硬盘序列号是否正确
            if (!string.IsNullOrEmpty(licenseInfo.ColumnSerialNumber))
            {
                string columnSerialNumber = ComputerUtils.GetColumnSerialNumber();
                if (StringUtils.EqualsIgnoreCase(columnSerialNumber, licenseInfo.ColumnSerialNumber))
                {
                    return true;
                }
            }

            return false;
		}

		private static XmlDocument GetXmlDocument(string filePath)
		{
			XmlDocument xmlDocument = new System.Xml.XmlDocument();
			try
			{
				DESEncryptor encryptor = new DESEncryptor();
				encryptor.InputString = FileUtils.ReadBase64StringFromFile(filePath);
				encryptor.DecryptKey = "kesgf4zb";
				encryptor.DesDecrypt();
				string xmlContent = encryptor.OutString.Substring(encryptor.OutString.IndexOf("\r\n") + 2);
				xmlDocument.LoadXml(xmlContent);
			}
			catch{}

			return xmlDocument;
		}

		#endregion
	}
}
