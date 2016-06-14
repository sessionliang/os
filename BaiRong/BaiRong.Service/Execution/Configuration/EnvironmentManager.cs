using System.Reflection;
using System.Xml;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.IO;
using BaiRong.Model;
using System;

namespace BaiRong.Service.Execution
{
    public class EnvironmentManager
    {
        private static bool isInitialized = false;

        public static bool IsInitialized
        {
            get { return isInitialized; }
        }

        public static string PhysicalApplicationPath
        {
            get
            {
                InitializeEnvironment();
                if (ConfigUtils.Instance == null)
                {
                    return string.Empty;
                }
                else
                {
                    return ConfigUtils.Instance.PhysicalApplicationPath;
                }
            }
        }

        public static string ApplicationPath
        {
            get
            {
                InitializeEnvironment();
                if (ConfigUtils.Instance == null)
                {
                    return "/";
                }
                else
                {
                    return ConfigUtils.Instance.ApplicationPath;
                }
            }
        }

        public static void InitializeEnvironment()
        {
            if (isInitialized == false)
            {
                string binPath = Assembly.GetExecutingAssembly().Location;
                binPath = binPath.Substring(0, binPath.LastIndexOf('\\'));
                string physicalApplicationPath = binPath.Substring(0, binPath.LastIndexOf('\\'));
                string webConfigPath = PathUtils.Combine(physicalApplicationPath, "web.config");

                if (FileUtils.IsFileExists(webConfigPath))
                {
                    WebConfigurationManager webConfigManager = WebConfigurationManager.GetInstance(webConfigPath);
                    ConfigUtils.InitializeManual(webConfigManager.AppSettings, physicalApplicationPath, null);
                    BaiRongDataProvider.InitializeManual(EDatabaseTypeUtils.GetEnumType(webConfigManager.AppSettings[WebConfigurationManager.Key.DatabaseType]), webConfigManager.AppSettings[WebConfigurationManager.Key.ConnectionString]);
                    isInitialized = true;

                    FileWatcherClass webConfigFileWatcher = new FileWatcherClass(webConfigPath);
                    webConfigFileWatcher.OnFileChange += new FileWatcherClass.FileChange(webConfigFileWatcher_OnFileChange);
                }
            }
        }

        static void webConfigFileWatcher_OnFileChange(object sender, EventArgs e)
        {
            isInitialized = false;
        }
    }
}
