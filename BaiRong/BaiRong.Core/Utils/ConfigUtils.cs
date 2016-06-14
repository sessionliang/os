using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using BaiRong.Core;
using System.Xml;

namespace BaiRong.Core
{
    public class ConfigUtils
    {
        private readonly NameValueCollection appSettings;
        private readonly string physicalApplicationPath;  //示例：    D:\\website
        private readonly string applicationPath;         //示例：    /website

        private ConfigUtils(NameValueCollection appSettings, string physicalApplicationPath, string applicationPath)
        {
            this.appSettings = appSettings;
            this.physicalApplicationPath = physicalApplicationPath;
            this.applicationPath = applicationPath;
        }

        private static ConfigUtils configUtils;

        public static ConfigUtils Instance
        {
            get
            {
                if (configUtils == null && HttpContext.Current != null)
                {
                    string applicationPath = HttpContext.Current.Request.ApplicationPath;
                    if (string.IsNullOrEmpty(applicationPath))
                    {
                        applicationPath = "/";
                    }
                    configUtils = new ConfigUtils(null, HttpContext.Current.Request.PhysicalApplicationPath, applicationPath);
                }
                return configUtils;
            }
        }

        public static void InitializeManual(NameValueCollection appSettings, string physicalApplicationPath, string applicationPath)
        {
            if (string.IsNullOrEmpty(applicationPath))
            {
                applicationPath = "/";
            }
            configUtils = new ConfigUtils(appSettings, physicalApplicationPath, applicationPath);
        }

        public string GetAppSettings(string key)
        {
            if (this.appSettings == null)
            {
                return GetAppSettingValue(key);
            }
            else
            {
                return this.appSettings.Get(key);
            }
        }

        public string PhysicalApplicationPath
        {
            get
            {
                return physicalApplicationPath;
            }
        }

        public string ApplicationPath
        {
            get
            {
                return applicationPath;
            }
        }

        public static string GetAppSettingValue(string key)
        {
            string value = string.Empty;

            try
            {
                XmlDocument doc = new XmlDocument();

                string configFile = HttpContext.Current.Server.MapPath("~/web.config");

                doc.Load(configFile);

                XmlNode appSettings = doc.SelectSingleNode("configuration/appSettings");
                foreach (XmlNode setting in appSettings)
                {
                    if (setting.Name == "add")
                    {
                        XmlAttribute attrKey = setting.Attributes["key"];
                        if (attrKey != null)
                        {
                            if (attrKey.Value == key)
                            {
                                XmlAttribute attrValue = setting.Attributes["value"];
                                if (attrValue != null)
                                {
                                    value = attrValue.Value;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch { }

            return value;
        }

        public static void SetAppSettingValue(string key, string value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();

                string configFile = HttpContext.Current.Server.MapPath("~/web.config");

                doc.Load(configFile);

                XmlNode appSettings = doc.SelectSingleNode("configuration/appSettings");
                foreach (XmlNode setting in appSettings)
                {
                    if (setting.Name == "add")
                    {
                        XmlAttribute attrKey = setting.Attributes["key"];
                        if (attrKey != null)
                        {
                            if (attrKey.Value == key)
                            {
                                XmlAttribute attrValue = setting.Attributes["value"];
                                if (attrValue != null)
                                {
                                    attrValue.Value = value;
                                    
                                    break;
                                }
                            }
                        }
                    }
                }

                doc.Save(configFile);
            }
            catch { }
        }

        /// <summary>
        /// 校验IIS是否是经典模式
        /// </summary>
        /// <returns></returns>
        public static bool isClassic()
        {
            try
            {
                XmlDocument doc = new XmlDocument();

                string configFile = HttpContext.Current.Server.MapPath("~/web.config");

                doc.Load(configFile);

                XmlNode appSettings = doc.SelectSingleNode("configuration/system.webServer");
                if (appSettings != null)
                    return false;
                else
                    return true;
            }
            catch { }
            return false;
        }
    }
}