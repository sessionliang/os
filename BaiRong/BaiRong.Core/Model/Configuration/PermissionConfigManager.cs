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

namespace BaiRong.Core.Configuration
{
	public class PermissionConfigManager
	{

        public const string CacheKey = "BaiRong.Core.Configuration.PermissionManager";
        public const string ModuleCacheKey = "BaiRong.Core.Configuration.ModulePermissions";

        private static string GetPermissionsFilePathOfProduct(string productID)
        {
            return string.Format("~/SiteFiles/Products/{0}/Menu/Permissions.config", productID);
        }

        private static string GetPermissionsFilePathOfApp(string appID)
        {
            return string.Format("~/SiteFiles/Products/{0}/{1}/Menu/Permissions.config", DirectoryUtils.Products.Apps.DirectoryName, appID);
        }

        ArrayList generalPermissions = new ArrayList();
        public ArrayList GeneralPermissions { get { return generalPermissions; } }

        ArrayList websitePermissions = new ArrayList();
        public ArrayList WebsitePermissions { get { return websitePermissions; } }

        ArrayList channelPermissions = new ArrayList();
        public ArrayList ChannelPermissions { get { return channelPermissions; } }

        ArrayList govInteractPermissions = new ArrayList();
        public ArrayList GovInteractPermissions { get { return govInteractPermissions; } }

        private PermissionConfigManager()
		{
		}

        public static PermissionConfigManager Instance
		{
			get
			{
                PermissionConfigManager permissionManager = CacheUtils.Get(CacheKey) as PermissionConfigManager;
                if (permissionManager == null)
				{
                    permissionManager = new PermissionConfigManager();

                    //ArrayList pathArrayList = new ArrayList();
                    //string path = PathUtils.MapPath(PermissionsPathGlobal);
                    //pathArrayList.Add(path);
                    //XmlDocument doc = new XmlDocument();
                    //doc.Load(path);
                    //permissionManager.LoadValuesFromConfigurationXml(doc, false);

                    ArrayList pathArrayList = new ArrayList();

                    foreach (string productID in ProductManager.GetProductIDList())
                    {
                        if (productID == ProductManager.Apps.ProductID)
                        {
                            foreach (string appID in AppManager.GetAppIDList(true))
                            {
                                string path = PathUtils.MapPath(GetPermissionsFilePathOfApp(appID));
                                if (FileUtils.IsFileExists(path))
                                {
                                    pathArrayList.Add(path);

                                    XmlDocument doc = new XmlDocument();
                                    doc.Load(path);
                                    permissionManager.LoadValuesFromConfigurationXml(doc);
                                }
                            }
                        }
                        else
                        {
                            string path = PathUtils.MapPath(GetPermissionsFilePathOfProduct(productID));
                            if (FileUtils.IsFileExists(path))
                            {
                                pathArrayList.Add(path);

                                XmlDocument doc = new XmlDocument();
                                doc.Load(path);
                                permissionManager.LoadValuesFromConfigurationXml(doc);
                            }
                        }
                    }                    

                    CacheUtils.Max(CacheKey, permissionManager, new CacheDependency(TranslateUtils.ArrayListToStringArray(pathArrayList)));
				}
                return permissionManager;
			}
		}

        public static void SaveGeneralPermissionsOfProductxxx(string productID, ArrayList permissions)
        {
            string path = PathUtils.MapPath(GetPermissionsFilePathOfProduct(productID));
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

        public static void SaveGeneralPermissionsOfApp(string appID, ArrayList permissions)
        {
            string path = PathUtils.MapPath(GetPermissionsFilePathOfApp(appID));
            if (FileUtils.IsFileExists(path))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                XmlNode configSettings = doc.SelectSingleNode("Config/generalPermissions");
                if (configSettings == null)
                {
                    configSettings = doc.SelectSingleNode("Config");
                }

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

        public static ArrayList GetGeneralPermissionsOfProduct(string productID)
        {
            ArrayList permissions = new ArrayList();
            if (productID == ProductManager.Apps.ProductID)
            {
                foreach (string appID in AppManager.GetAppIDList(true))
                {
                    string path = PathUtils.MapPath(GetPermissionsFilePathOfApp(appID));
                    if (FileUtils.IsFileExists(path))
                    {
                        XmlDocument XmlDoc = new XmlDocument();
                        XmlDoc.Load(path);
                        XmlNode coreNode = XmlDoc.SelectSingleNode("Config");

                        bool isGet = false;

                        foreach (XmlNode child in coreNode.ChildNodes)
                        {
                            if (child.Name == "generalPermissions")
                            {
                                isGet = true;
                                GetPermissions(child, permissions);
                                break;
                            }
                        }

                        if (isGet == false)
                        {
                            GetPermissions(coreNode, permissions);
                        }
                    }
                }
            }
            else
            {
                string path = PathUtils.MapPath(GetPermissionsFilePathOfProduct(productID));
                if (FileUtils.IsFileExists(path))
                {
                    XmlDocument XmlDoc = new XmlDocument();
                    XmlDoc.Load(path);
                    XmlNode coreNode = XmlDoc.SelectSingleNode("Config");

                    bool isGet = false;

                    foreach (XmlNode child in coreNode.ChildNodes)
                    {
                        if (child.Name == "generalPermissions")
                        {
                            isGet = true;
                            GetPermissions(child, permissions);
                            break;
                        }
                    }

                    if (isGet == false)
                    {
                        GetPermissions(coreNode, permissions);
                    }
                }
            }
            return permissions;
        }

        public static ArrayList GetGeneralPermissionsOfApp(string appID)
        {
            ArrayList permissions = new ArrayList();

            string path = PathUtils.MapPath(GetPermissionsFilePathOfApp(appID));
            if (FileUtils.IsFileExists(path))
            {
                XmlDocument XmlDoc = new XmlDocument();
                XmlDoc.Load(path);
                XmlNode coreNode = XmlDoc.SelectSingleNode("Config");

                bool isGet = false;

                foreach (XmlNode child in coreNode.ChildNodes)
                {
                    if (child.Name == "generalPermissions")
                    {
                        isGet = true;
                        GetPermissions(child, permissions);
                        break;
                    }
                }

                if (isGet == false)
                {
                    GetPermissions(coreNode, permissions);
                }
            }

            return permissions;
        }

        public static ArrayList GetWebsitePermissionsOfApp(string appID)
        {
            ArrayList permissions = new ArrayList();
            if (!string.IsNullOrEmpty(appID))
            {
                string path = PathUtils.MapPath(GetPermissionsFilePathOfApp(appID));
                if (FileUtils.IsFileExists(path))
                {
                    XmlDocument XmlDoc = new XmlDocument();
                    XmlDoc.Load(path);
                    XmlNode coreNode = XmlDoc.SelectSingleNode("Config");
                    foreach (XmlNode child in coreNode.ChildNodes)
                    {
                        if (child.Name == "websitePermissions")
                        {
                            GetPermissions(child, permissions);
                            break;
                        }
                    }
                }
            }
            return permissions;
        }

        public static ArrayList GetChannelPermissionsOfApp(string appID)
        {
            ArrayList permissions = new ArrayList();
            if (!string.IsNullOrEmpty(appID))
            {
                string path = PathUtils.MapPath(GetPermissionsFilePathOfApp(appID));
                if (FileUtils.IsFileExists(path))
                {
                    XmlDocument XmlDoc = new XmlDocument();
                    XmlDoc.Load(path);
                    XmlNode coreNode = XmlDoc.SelectSingleNode("Config");
                    foreach (XmlNode child in coreNode.ChildNodes)
                    {
                        if (child.Name == "channelPermissions")
                        {
                            GetPermissions(child, permissions);
                            break;
                        }
                    }
                }
            }
            return permissions;
        }

        private void LoadValuesFromConfigurationXml(XmlDocument doc) 
		{
            XmlNode coreNode = doc.SelectSingleNode("Config");

            bool isMultiple = true;
            foreach (XmlNode child in coreNode.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Comment) continue;
                if (child.Name == "generalPermissions")
                {
                    GetPermissions(child, generalPermissions);
                }
                else if (child.Name == "websitePermissions")
                {
                    GetPermissions(child, websitePermissions);
                }
                else if (child.Name == "channelPermissions")
                {
                    GetPermissions(child, channelPermissions);
                }
                else if (child.Name == "govInteractPermissions")
                {
                    GetPermissions(child, govInteractPermissions);
                }
                else
                {
                    isMultiple = false;
                    break;
                }
            }
            if (!isMultiple)
            {
                GetPermissions(coreNode, generalPermissions);
            }
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
	}
}
