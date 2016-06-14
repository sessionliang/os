using System.Collections;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using System.Web.Caching;
using System.Xml;
using System.Collections.Generic;

namespace BaiRong.Core
{
	public class ProductFileUtils
	{
        private ProductFileUtils()
	    {
            
	    }

        //public static void DeleteProduct(string productID)
        //{
        //    string productPath = PathUtils.GetProductPath(productID);
        //    DirectoryUtils.DeleteDirectoryIfExists(productPath);
        //}

        public static List<ModuleFileInfo> GetModuleFileInfoListOfApps()
        {
            List<ModuleFileInfo> list = new List<ModuleFileInfo>();

            foreach (string appID in AppManager.GetAppIDList())
            {
                ModuleFileInfo moduleFileInfo = ProductFileUtils.GetModuleFileInfoOfApp(appID);
                if (moduleFileInfo != null)
                {
                    list.Add(moduleFileInfo);
                }
            }

            return list;
        }

        public static ModuleFileInfo GetModuleFileInfo(string productID)
        {
            string cacheKey = "BaiRong.Core.ProductFileManager." + productID.ToLower();

            ModuleFileInfo moduleFileInfo = CacheUtils.Get(cacheKey) as ModuleFileInfo;
            if (moduleFileInfo == null)
            {
                string filePath = PathUtils.Combine(PathUtils.GetProductPath(productID), DirectoryUtils.Products.File_Configuration);
                if (FileUtils.IsFileExists(filePath))
                {
                    moduleFileInfo = Serializer.ConvertFileToObject(filePath, typeof(ModuleFileInfo)) as ModuleFileInfo;
                    if (moduleFileInfo != null)
                    {
                        moduleFileInfo.ModuleID = productID;
                    }

                    CacheUtils.Max(cacheKey, moduleFileInfo, new CacheDependency(filePath));
                }
            }

            return moduleFileInfo;
        }

        public static ModuleFileInfo GetModuleFileInfoOfApp(string appID)
        {
            string cacheKey = "BaiRong.Core.ProductFileManager." + appID.ToLower();

            ModuleFileInfo moduleFileInfo = CacheUtils.Get(cacheKey) as ModuleFileInfo;
            if (moduleFileInfo == null)
            {
                string filePath = PathUtils.Combine(PathUtils.GetProductPath(ProductManager.Apps.ProductID, appID), DirectoryUtils.Products.File_Configuration);
                if (FileUtils.IsFileExists(filePath))
                {
                    moduleFileInfo = Serializer.ConvertFileToObject(filePath, typeof(ModuleFileInfo)) as ModuleFileInfo;
                    if (moduleFileInfo != null)
                    {
                        moduleFileInfo.ModuleID = appID;
                    }

                    CacheUtils.Max(cacheKey, moduleFileInfo, new CacheDependency(filePath));
                }
            }

            return moduleFileInfo;
        }

        public static string GetModuleName(string productID)
        {
            ModuleFileInfo moduleFileInfo = ProductFileUtils.GetModuleFileInfo(productID);
            if (moduleFileInfo != null)
            {
                return moduleFileInfo.ModuleName;
            }
            return string.Empty;
        }

        public static void SaveProductFileInfo(string productID, ModuleFileInfo moduleFileInfo)
        {
            string path = PathUtils.Combine(PathUtils.GetProductPath(productID), DirectoryUtils.Products.File_Configuration);
            FileUtils.WriteText(path, ECharset.utf_8, string.Empty);
            Serializer.SaveAsXML(moduleFileInfo, path);
        }

        public static ArrayList GetMenuTopArrayList(string productID)
        {
            ArrayList tabArrayList = new ArrayList();

            string menuPath = PathUtils.Combine(PathUtils.GetProductPath(productID), DirectoryUtils.Products.Menu.DirectoryName, DirectoryUtils.Products.Menu.File_Top);

            if (FileUtils.IsFileExists(menuPath))
            {
                try
                {
                    TabCollection tabs = TabCollection.GetTabs(menuPath);
                    foreach (Tab parent in tabs.Tabs)
                    {
                        tabArrayList.Add(parent);
                    }
                }
                catch { }
            }

            if (!StringUtils.EqualsIgnoreCase(productID, ProductManager.Apps.ProductID))
            {
                menuPath = PathUtils.Combine(PathUtils.GetProductPath(ProductManager.Apps.ProductID), DirectoryUtils.Products.Menu.DirectoryName, DirectoryUtils.Products.Menu.File_Top);

                if (FileUtils.IsFileExists(menuPath))
                {
                    try
                    {
                        TabCollection tabs = TabCollection.GetTabs(menuPath);
                        foreach (Tab parent in tabs.Tabs)
                        {
                            parent.IsPlatform = true;
                            tabArrayList.Add(parent);
                        }
                    }
                    catch { }
                }
            }

            return tabArrayList;
        }

        public static ArrayList GetMenuTopArrayList()
        {
            ArrayList tabArrayList = new ArrayList();

            string menuPath = PathUtils.Combine(PathUtils.GetAppPath(AppManager.Platform.AppID), DirectoryUtils.Products.Menu.DirectoryName, DirectoryUtils.Products.Menu.File_Top);

            if (FileUtils.IsFileExists(menuPath))
            {
                try
                {
                    TabCollection tabs = TabCollection.GetTabs(menuPath);
                    foreach (Tab parent in tabs.Tabs)
                    {
                        parent.IsPlatform = true;
                        tabArrayList.Add(parent);
                    }
                }
                catch { }
            }

            return tabArrayList;
        }

        public static bool IsExists(string productID)
        {
            string menuPath = PathUtils.GetProductPath(productID, DirectoryUtils.Products.Menu.DirectoryName, DirectoryUtils.Products.Menu.File_Top);
            return FileUtils.IsFileExists(menuPath);
        }

        public static ArrayList GetMenuTopTabArrayList(string productID)
        {
            ArrayList arraylist = new ArrayList();

            string menuPath = PathUtils.GetProductPath(productID, DirectoryUtils.Products.Menu.DirectoryName, DirectoryUtils.Products.Menu.File_Top);
            if (FileUtils.IsFileExists(menuPath))
            {
                try
                {
                    TabCollection tabs = TabCollection.GetTabs(menuPath);
                    foreach (Tab parent in tabs.Tabs)
                    {
                        arraylist.Add(parent);
                    }
                }
                catch { }
            }

            return arraylist;
        }

        public static ArrayList GetMenuGroupTabArrayListOfApp(string appID, string menuID)
        {
            ArrayList arraylist = new ArrayList();

            string menuPath = PathUtils.GetProductPath(ProductManager.Apps.ProductID, appID, DirectoryUtils.Products.Menu.DirectoryName, menuID + ".config");
            if (FileUtils.IsFileExists(menuPath))
            {
                try
                {
                    TabCollection tabs = TabCollection.GetTabs(menuPath);
                    foreach (Tab parent in tabs.Tabs)
                    {
                        arraylist.Add(parent);
                    }
                }
                catch { }
            }

            return arraylist;
        }

        public static void SaveMenuTabArrayList(string productID, string menuID, ArrayList tabArrayList)
        {
            string menuPath = PathUtils.GetProductPath(productID, DirectoryUtils.Products.Menu.DirectoryName, menuID + ".config");
            if (FileUtils.IsFileExists(menuPath))
            {
                TabCollection tabCollection = new TabCollection();
                tabCollection.Tabs = new Tab[tabArrayList.Count];

                for (int i = 0; i < tabArrayList.Count; i++)
                {
                    tabCollection.Tabs[i] = tabArrayList[i] as Tab;
                }
                Serializer.SaveAsXML(tabCollection, menuPath);
            }
        }

        public static ArrayList GetUCMenuTopTabArrayList(string productID)
        {
            ArrayList arraylist = new ArrayList();

            string menuPath = PathUtils.GetProductPath(productID, DirectoryUtils.Products.UserCenter.DirectoryName, DirectoryUtils.Products.UserCenter.File_Top);
            if (FileUtils.IsFileExists(menuPath))
            {
                try
                {
                    TabCollection tabs = TabCollection.GetTabs(menuPath);
                    foreach (Tab parent in tabs.Tabs)
                    {
                        arraylist.Add(parent);
                    }
                }
                catch { }
            }

            return arraylist;
        }

        public static ArrayList GetUCMenuGroupTabArrayList(string productID, string menuID)
        {
            ArrayList arraylist = new ArrayList();

            string menuPath = PathUtils.GetProductPath(productID, DirectoryUtils.Products.UserCenter.DirectoryName, menuID + ".config");
            if (FileUtils.IsFileExists(menuPath))
            {
                try
                {
                    TabCollection tabs = TabCollection.GetTabs(menuPath);
                    foreach (Tab parent in tabs.Tabs)
                    {
                        arraylist.Add(parent);
                    }
                }
                catch { }
            }

            return arraylist;
        }

        public static void SaveUCMenuTabArrayList(string productID, bool isTop, ArrayList tabArrayList)
        {
            string menuID = isTop ? "Top" : "Menu";
            string menuPath = PathUtils.GetProductPath(productID, DirectoryUtils.Products.UserCenter.DirectoryName, menuID + ".config");
            if (FileUtils.IsFileExists(menuPath))
            {
                TabCollection tabCollection = new TabCollection();
                tabCollection.Tabs = new Tab[tabArrayList.Count];

                for (int i = 0; i < tabArrayList.Count; i++)
                {
                    tabCollection.Tabs[i] = tabArrayList[i] as Tab;
                }
                Serializer.SaveAsXML(tabCollection, menuPath);
            }
        }
	}
}
