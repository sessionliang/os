using System.Collections;
using System.Web.Caching;
using System.Xml;
using BaiRong.Core.Configuration;
using BaiRong.Core;
using BaiRong.Model;
using System;

namespace BaiRong.Core
{
    public class FileConfigManager
    {
        public static readonly string CacheKey = "BaiRong.Core.FileConfigManager";

        private string adminDirectoryName = "siteserver";
        public string AdminDirectoryName { get { return adminDirectoryName; } }

        private string apiUrl = "/api";
        public string APIUrl { get { return apiUrl; } }

        private bool cors = false;
        public bool Cors { get { return cors; } }

        private string apiSecretKey = "vEnfkn16t8aeaZKG3a4Gl9UUlzf4vgqU9xwh8ZV5";
        public string APISecretKey { get { return apiSecretKey; } }

        private bool isSaas = false;
        public bool IsSaas { get { return isSaas; } }

        public bool IsSaasQCloud
        {
            get
            {
                return IsSaas && SSOConfig.IntegrationType == EIntegrationType.QCloud;
            }
        }

        private bool isCollection = true;
        public bool IsCollection { get { return isCollection; } }

        private bool isValidateCode = true;
        public bool IsValidateCode { get { return isValidateCode; } }

        private bool isForgetPassword = false;
        public bool IsForgetPassword { get { return isForgetPassword; } }

        private SSOConfigInfo ssoConfigInfo = new SSOConfigInfo();
        public SSOConfigInfo SSOConfig { get { return ssoConfigInfo; } }

        private OEMConfigInfo oemConfigInfo = new OEMConfigInfo();
        public OEMConfigInfo OEMConfig { get { return oemConfigInfo; } }

        private string checkCode = string.Empty;
        public bool IsCheckCode(string inputCode)
        {
            if (!string.IsNullOrEmpty(this.checkCode) && !string.IsNullOrEmpty(inputCode))
            {
                return inputCode == this.checkCode;
            }
            return false;
        }

        public static FileConfigManager Instance
        {
            get
            {
                FileConfigManager configManager = CacheUtils.Get(CacheKey) as FileConfigManager;
                if (configManager == null)
                {
                    try
                    {
                        string path = PathUtils.MapPath("~/SiteFiles/Configuration/Configuration.config");

                        XmlDocument doc = new XmlDocument();
                        doc.Load(path);
                        configManager = new FileConfigManager(doc);
                        CacheUtils.Max(CacheKey, configManager, new CacheDependency(path));
                    }
                    catch
                    {
                        XmlDocument doc = new XmlDocument();
                        configManager = new FileConfigManager(doc);
                    }
                }
                return configManager;
            }
        }

        private FileConfigManager(XmlDocument doc)
        {
            XmlNode coreNode = doc.SelectSingleNode("config/core");

            if (coreNode != null)
            {
                XmlAttributeCollection attributeCollection = coreNode.Attributes;

                XmlAttribute xmlAttribute = attributeCollection["adminDirectory"];
                if (xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.Value))
                {
                    adminDirectoryName = xmlAttribute.Value.ToLower();
                }

                xmlAttribute = attributeCollection["apiUrl"];
                if (xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.Value))
                {
                    apiUrl = xmlAttribute.Value.ToLower();
                }

                xmlAttribute = attributeCollection["cors"];
                if (xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.Value))
                {
                    cors = TranslateUtils.ToBool(xmlAttribute.Value);
                }

                xmlAttribute = attributeCollection["apiSecretKey"];
                if (xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.Value))
                {
                    apiSecretKey = xmlAttribute.Value;
                }

                xmlAttribute = attributeCollection["isSaas"];
                if (xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.Value))
                {
                    isSaas = TranslateUtils.ToBool(xmlAttribute.Value);
                }

                xmlAttribute = attributeCollection["isCollection"];
                if (xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.Value))
                {
                    isCollection = TranslateUtils.ToBool(xmlAttribute.Value);
                }

                xmlAttribute = attributeCollection["isValidateCode"];
                if (xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.Value))
                {
                    isValidateCode = TranslateUtils.ToBool(xmlAttribute.Value);
                }

                xmlAttribute = attributeCollection["checkCode"];
                if (xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.Value))
                {
                    checkCode = xmlAttribute.Value;
                }

                xmlAttribute = attributeCollection["isForgetPassword"];
                if (xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.Value))
                {
                    isForgetPassword = TranslateUtils.ToBool(xmlAttribute.Value);
                }

                XmlNode ssoNode = doc.SelectSingleNode("config/sso");

                if (ssoNode != null && ssoNode.Attributes != null)
                {
                    attributeCollection = ssoNode.Attributes;

                    xmlAttribute = attributeCollection["integrationType"];
                    if (xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.Value))
                    {
                        this.ssoConfigInfo.IntegrationType = EIntegrationTypeUtils.GetEnumType(xmlAttribute.Value);
                    }

                    xmlAttribute = attributeCollection["isvKey"];
                    if (xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.Value))
                    {
                        this.ssoConfigInfo.ISVKey = xmlAttribute.Value;
                    }

                    xmlAttribute = attributeCollection["isUser"];
                    if (xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.Value))
                    {
                        this.ssoConfigInfo.IsUser = TranslateUtils.ToBool(xmlAttribute.Value);
                    }

                    xmlAttribute = attributeCollection["userPrefix"];
                    if (xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.Value))
                    {
                        this.ssoConfigInfo.UserPrefix = xmlAttribute.Value;
                    }

                    xmlAttribute = attributeCollection["loginUrl"];
                    if (xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.Value))
                    {
                        this.ssoConfigInfo.LoginUrl = xmlAttribute.Value;
                    }

                    xmlAttribute = attributeCollection["callbackUrl"];
                    if (xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.Value))
                    {
                        this.ssoConfigInfo.CallbackUrl = xmlAttribute.Value;
                    }
                }

                XmlNode oemNode = doc.SelectSingleNode("config/oem");

                if (oemNode != null && oemNode.Attributes != null)
                {
                    attributeCollection = oemNode.Attributes;

                    xmlAttribute = attributeCollection["isOEM"];
                    if (xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.Value))
                    {
                        this.oemConfigInfo.IsOEM = TranslateUtils.ToBool(xmlAttribute.Value);
                    }

                    xmlAttribute = attributeCollection["companyName"];
                    if (xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.Value))
                    {
                        this.oemConfigInfo.CompanyName = xmlAttribute.Value;
                    }

                    xmlAttribute = attributeCollection["companyUrl"];
                    if (xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.Value))
                    {
                        this.oemConfigInfo.CompanyUrl = xmlAttribute.Value;
                    }

                    xmlAttribute = attributeCollection["productName"];
                    if (xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.Value))
                    {
                        this.oemConfigInfo.ProductName = xmlAttribute.Value;
                    }

                    xmlAttribute = attributeCollection["productUrl"];
                    if (xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.Value))
                    {
                        this.oemConfigInfo.ProductUrl = xmlAttribute.Value;
                    }
                }
            }
        }
    }
}
