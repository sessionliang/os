using System.Collections.Specialized;
using System.Xml;
using BaiRong.Core;
using BaiRong.Core.IO;

namespace BaiRong.Service.Execution
{
    public class WebConfigurationManager
    {
        public class Key
        {
            private Key() { }
            public const string DatabaseType = "DatabaseType";
            public const string ConnectionString = "ConnectionString";
            public const string DirectoryPath = "DirectoryPath";
        }

        private readonly XmlDocument XmlDoc = null;

        readonly NameValueCollection appSettings = new NameValueCollection();
        public NameValueCollection AppSettings { get { return appSettings; } }

        private WebConfigurationManager(XmlDocument doc)
        {
            XmlDoc = doc;
            LoadValuesFromConfigurationXml();
        }

        public XmlNode GetConfigSection(string nodePath)
        {
            return XmlDoc.SelectSingleNode(nodePath);
        }

        private static WebConfigurationManager configManager;

        public static WebConfigurationManager GetInstance(string webConfigPath)
        {
            if (configManager == null)
            {
                if (!string.IsNullOrEmpty(webConfigPath) && FileUtils.IsFileExists(webConfigPath))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(webConfigPath);
                    configManager = new WebConfigurationManager(doc);
                }
                else
                {
                    return new WebConfigurationManager(null);
                }
            }
            return configManager;
        }

        private void LoadValuesFromConfigurationXml()
        {
            if (this.XmlDoc != null)
            {
                XmlNode settingsNode = GetConfigSection("configuration/appSettings");

                foreach (XmlNode node in settingsNode.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "add":
                            this.AppSettings.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
                            break;

                        case "remove":
                            this.AppSettings.Remove(node.Attributes["key"].Value);
                            break;

                        case "clear":
                            this.AppSettings.Clear();
                            break;
                    }
                }
            }
        }
    }
}
