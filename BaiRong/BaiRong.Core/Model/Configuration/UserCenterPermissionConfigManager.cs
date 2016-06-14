using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.Caching;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Configuration;
using System.Collections.Generic;

namespace BaiRong.Core.Configuration
{
	public class UserCenterPermissionConfigManager
	{

        public const string CacheKey = "BaiRong.Core.Configuration.UserCenterPermissionConfigManager";
        public const string ModuleCacheKey = "BaiRong.Core.Configuration.UserCenterModulePermissions";

        private static string GetPermissionsFilePath(string productID, string appID)
        {
            if (!string.IsNullOrEmpty(appID))
            {
                return PathUtils.MapPath(string.Format("~/SiteFiles/Products/Apps/{0}/UserCenter/Permissions.config", appID));
            }
            else
            {
                return PathUtils.MapPath(string.Format("~/SiteFiles/Products/{0}/UserCenter/Permissions.config", productID));
            }
        }

        ArrayList permissions = new ArrayList();
        public ArrayList Permissions { get { return permissions; } }

        private UserCenterPermissionConfigManager()
		{
		}

        public static UserCenterPermissionConfigManager Instance
		{
			get
			{
                UserCenterPermissionConfigManager permissionManager = CacheUtils.Get(CacheKey) as UserCenterPermissionConfigManager;
                if (permissionManager == null)
				{
                    permissionManager = new UserCenterPermissionConfigManager();

                    ArrayList pathArrayList = new ArrayList();

                    foreach (string productID in ProductManager.GetProductIDList())
                    {
                        if (productID == ProductManager.Apps.ProductID)
                        {
                            List<string> appIDList = AppManager.GetAppIDList();
                            foreach (string appID in appIDList)
                            {
                                LoadValues(permissionManager, pathArrayList, productID, appID);
                            }
                        }
                        else
                        {
                            LoadValues(permissionManager, pathArrayList, productID, string.Empty);
                        }
                    }

                    CacheUtils.Max(CacheKey, permissionManager, new CacheDependency(TranslateUtils.ArrayListToStringArray(pathArrayList)));
				}
                return permissionManager;
			}
		}

        private static void LoadValues(UserCenterPermissionConfigManager permissionManager, ArrayList pathArrayList, string productID, string appID)
        {
            string path = GetPermissionsFilePath(productID, appID);
            if (FileUtils.IsFileExists(path))
            {
                pathArrayList.Add(path);

                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                permissionManager.LoadValuesFromConfigurationXml(doc);
            }
        }

        public static void SavePermissionsToFile(string productID, string appID, ArrayList permissions)
        {
            string path = GetPermissionsFilePath(productID, appID);
            if (FileUtils.IsFileExists(path))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                XmlNode configSettings = doc.SelectSingleNode("Config");
                configSettings.RemoveAll();
                foreach (PermissionConfig config in permissions)
                {
                    XmlElement setting = doc.CreateElement("add");
                    setting.SetAttribute("name", config.Name);
                    setting.SetAttribute("text", config.Text);
                    configSettings.AppendChild(setting);
                }

                System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(path, System.Text.Encoding.UTF8);
                writer.Formatting = System.Xml.Formatting.Indented;
                doc.Save(writer);
                writer.Flush();
                writer.Close();
            }
        }

        public static ArrayList GetPermissionsFromFile(string productID, string appID)
        {
            ArrayList permissions = new ArrayList();
            string path = GetPermissionsFilePath(productID, appID);
            if (FileUtils.IsFileExists(path))
            {
                XmlDocument XmlDoc = new XmlDocument();
                XmlDoc.Load(path);
                XmlNode coreNode = XmlDoc.SelectSingleNode("Config");
                GetPermissions(coreNode, permissions);
            }
            return permissions;
        }

        private void LoadValuesFromConfigurationXml(XmlDocument doc) 
		{
            XmlNode coreNode = doc.SelectSingleNode("Config");
            GetPermissions(coreNode, permissions);
		}

        private static void GetPermissions(XmlNode node, ArrayList list) 
		{
            foreach (XmlNode permission in node.ChildNodes) 
			{
                switch (permission.Name) 
				{
					case "add" :
                        list.Add(new PermissionConfig(permission.Attributes));
						break;
				}
			}
		}

        public string PermissionNameCollection
        {
            get
            {
                ArrayList arraylist = new ArrayList();
                foreach (PermissionConfig permissionConfig in this.permissions)
                {
                    arraylist.Add(permissionConfig.Name);
                }
                return TranslateUtils.ObjectCollectionToString(arraylist);
            }
        }
	}
}
