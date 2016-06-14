using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SiteServer.Service.Core
{
    public class ConfigurationManager
    {
        public const string BIN_NAME = "Bin";
        public const string FILE_NAME = "BaiRong.Service.dll";
        public const string NAME_SPACE = "BaiRong.Service.Execution";
        public const string CLASS_NAME = "TaskManager";
        public const string METHOD_NAME = "Execute";

        public const string METHOD_NAME_CREATETASK = "ExecuteCreateTask";
        public const string METHOD_NAME_BACKGROUNDCREATETASK = "ExecuteBackgroundCreateTask";

        private static string DIRECTORY_PATH;

        public static string DirectoryPath
        {
            get
            {
                if (ConfigurationManager.DIRECTORY_PATH == null)
                {
                    string directoryPath = string.Empty;

                    string configPath = Assembly.GetExecutingAssembly().Location;
                    configPath = Path.Combine(configPath.Substring(0, configPath.LastIndexOf("\\")), "Configuration.xml");
                    if (FileSystemUtils.IsFileExists(configPath))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(configPath);
                        XmlNode pathNode = doc.SelectSingleNode("configuration/directoryPath");
                        directoryPath = pathNode.InnerText;
                        directoryPath = directoryPath.Trim(new char[] { '/', '\\' });
                    }

                    ConfigurationManager.DIRECTORY_PATH = directoryPath;

                    return directoryPath;
                }
                return ConfigurationManager.DIRECTORY_PATH;                
            }
        }

        public static string GetDDLFilePath(string directoryPath)
        {
            return Path.Combine(directoryPath, BIN_NAME, FILE_NAME);
        }

        public static bool SaveDirectoryPath(string directoryPath)
        {
            bool success = false;

            if (!string.IsNullOrEmpty(directoryPath))
            {
                if (FileSystemUtils.IsDirectoryExists(directoryPath))
                {
                    string dllPath = ConfigurationManager.GetDDLFilePath(directoryPath);
                    if (FileSystemUtils.IsFileExists(dllPath))
                    {
                        success = SaveConfiguration(directoryPath);
                    }
                }
            }

            return success;
        }

        private static bool SaveConfiguration(string directoryPath)
        {
            bool success = false;

            try
            {
                string configPath = Assembly.GetExecutingAssembly().Location;
                configPath = Path.Combine(configPath.Substring(0, configPath.LastIndexOf("\\")), "Configuration.xml");
                if (!FileSystemUtils.IsFileExists(configPath))
                {
                    string content = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <directoryPath>{0}</directoryPath>
</configuration>", directoryPath);
                    FileSystemUtils.WriteText(configPath, content);
                }
                else
                {
                    XmlDocument doc = new XmlDocument();
                    doc.PreserveWhitespace = true;
                    doc.Load(configPath);

                    XmlNode pathNode = doc.SelectSingleNode("configuration/directoryPath");
                    pathNode.InnerText = directoryPath;

                    XmlTextWriter writer = new XmlTextWriter(configPath, System.Text.Encoding.UTF8);
                    writer.Formatting = Formatting.Indented;
                    doc.Save(writer);
                    writer.Flush();
                    writer.Close();
                }
                ConfigurationManager.DIRECTORY_PATH = null;
                success = true;
            }
            catch { }

            return success;
        }
    }
}
