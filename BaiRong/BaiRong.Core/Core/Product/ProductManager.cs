
using System;
using System.Text;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using System.Collections;
using System.Collections.Generic;
using BaiRong.Model;
using System.Web.Caching;

namespace BaiRong.Core
{
    public class ProductManager
    {
        private ProductManager() { }

        public const string Version = "4.1.1603";
        public const bool IsBeta = false;

        public const bool IsRemoveTabs = true; //是否精简菜单

        private static ArrayList removedTabs;

        /// <summary>
        /// 精简菜单
        /// </summary>
        public static ArrayList RemovedTabs
        {
            get {
                if (removedTabs == null)
                {
                    removedTabs = new ArrayList();
                    removedTabs.Add("stl/background_tagStyleMenu.aspx"); //下拉菜单样式
                    removedTabs.Add("stl/background_stlTag.aspx");       //自定义模板
                    removedTabs.Add("stl/background_tagStyle.aspx?elementName=stl:resume");     //简历管理
                    removedTabs.Add("cms/background_mailSendLog.aspx");  //推荐好友记录
                    removedTabs.Add("cms/background_mailSendFormat.aspx"); //邮件格式设置
                    removedTabs.Add("cms/background_mailSubscribe.aspx");  //邮件订阅列表
                    removedTabs.Add("cms/background_mailSubscribeFormat.aspx");   //邮件格式设置
                    removedTabs.Add("cms/background_restrictionList.aspx");  //黑名单
                    removedTabs.Add("cms/background_restrictionList.aspx");  //白名单
                    removedTabs.Add("cms/background_restrictionOptions.aspx");//访问限制设置
                    removedTabs.Add("cms/background_fileMain.aspx");           //站点文件管理
                    removedTabs.Add("cms/background_shareSet.aspx");           //插件设置
                    removedTabs.Add("cms/background_shareStatistics.aspx");    //统计信息
                    //removedTabs.Add("cms/background_task.aspx?ServiceType=Create");           //定时生成任务
                    //removedTabs.Add("cms/background_task.aspx?ServiceType=Publish");        //定时发布任务
                    //removedTabs.Add("cms/background_task.aspx?ServiceType=Gather");           //定时采集任务
                    //removedTabs.Add("cms/background_task.aspx?ServiceType=Backup");          //定时备份任务
                    //removedTabs.Add("cms/background_task.aspx?ServiceType=Create&PublishmentSystemID=0");           //定时生成任务
                    //removedTabs.Add("cms/background_task.aspx?ServiceType=Publish&PublishmentSystemID=0");        //定时发布任务
                    //removedTabs.Add("cms/background_task.aspx?ServiceType=Gather&PublishmentSystemID=0");           //定时采集任务
                    //removedTabs.Add("cms/background_task.aspx?ServiceType=Backup&PublishmentSystemID=0");          //定时备份任务
                    removedTabs.Add("stl/background_tagStyle.aspx?elementName=stl:login");    //用户登录样式
                    removedTabs.Add("stl/background_tagStyle.aspx?elementName=stl:register");    //用户注册样式  
                }
                return removedTabs;
            }
        }

        public static string GetFullVersion()
        {
            return GetFullVersion(ProductManager.Version, ProductManager.IsBeta);
        }

        public static string GetFullVersion(string version, bool isBeta)
        {
            string retval = version;
            if (isBeta)
            {
                retval += " BETA";
            }
            return retval;
        }

        public static double GetVersionDouble(string version)
        {
            version = version.Replace("_", ".").ToLower().Trim();
            version = version.Replace(" ", string.Empty);
            if (StringUtils.GetCount(".", version) == 2)
            {
                string theVersion = version;
                version = theVersion.Substring(0, theVersion.LastIndexOf("."));
                version += theVersion.Substring(theVersion.LastIndexOf(".") + 1);
            }
            return TranslateUtils.ToDouble(version);
        }

        public static bool IsNeedUpgrade()
        {
            return !StringUtils.EqualsIgnoreCase(ProductManager.Version, BaiRongDataProvider.ConfigDAO.GetDatabaseVersion());
        }

        public static bool IsNeedInstall()
        {
            bool isNeedInstall = true;
            try
            {
                isNeedInstall = !BaiRongDataProvider.ConfigDAO.IsInitialized();
            }
            catch { }
            return isNeedInstall;
        }

        public class Apps
        {
            public const string ProductID = "apps";

            public static bool Exists
            {
                get
                {
                    return ProductManager.IsProductExists(ProductManager.Apps.ProductID);
                }
            }
        }

        public static List<string> GetProductIDList()
        {
            string cacheKey = "BaiRong.Core.ProductManager.ProductIDList";

            List<string> productIDList = CacheUtils.Get(cacheKey) as List<string>;
            if (productIDList == null)
            {
                string directoryPath = PathUtils.Combine(ConfigUtils.Instance.PhysicalApplicationPath, DirectoryUtils.SiteFiles.DirectoryName, DirectoryUtils.SiteFiles.Products);
                string[] array = DirectoryUtils.GetDirectoryNames(directoryPath);

                productIDList = new List<string>();
                foreach (string productID in array)
                {
                    if (!StringUtils.EqualsIgnoreCase(productID, ProductManager.Apps.ProductID))
                    {
                        if (!ProductFileUtils.IsExists(productID))
                        {
                            continue;
                        }
                    }
                    productIDList.Add(productID.ToLower());
                }

                CacheUtils.Max(cacheKey, productIDList);
            }

            return productIDList;
        }

        public static bool IsProductExists(string productID)
        {
            List<string> productIDList = ProductManager.GetProductIDList();
            return productIDList.Contains(productID.ToLower());
        }

        public static List<string> GetExternalInstalledProductIDArrayList()
        {
            List<string> list = new List<string>();

            List<string> productIDList = ProductManager.GetProductIDList();
            foreach (string productID in productIDList)
            {
                if (productID != ProductManager.Apps.ProductID)
                {
                    list.Add(productID);
                }
            }

            return list;
        }

        //public static void InstallProduct(string productID, bool isDatabase, bool isOverride)
        //{
        //    if (isDatabase)
        //    {
        //        string sqlPath = PathUtils.GetInstallerProductSqlFilePath(productID, BaiRongDataProvider.DatabaseType);
        //        if (FileUtils.IsFileExists(sqlPath))
        //        {
        //            StringBuilder errorBuilder = new StringBuilder();
        //            BaiRongDataProvider.DatabaseDAO.ExecuteSqlInFile(sqlPath, errorBuilder);
        //        }
        //        if (productID == ProductManager.Apps.ProductID)
        //        {
        //            BaiRongDataProvider.TableCollectionDAO.CreateAllAuxiliaryTableIfNotExists();
        //        }
        //    }

        //    string sourcePath = PathUtils.Combine(PathUtils.GetProductPath(productID), DirectoryUtils.Products.Directory_Files);
        //    string targetPath = PathUtils.MapPath("~/");

        //    if (DirectoryUtils.IsDirectoryExists(sourcePath))
        //    {
        //        DirectoryUtils.Copy(sourcePath, targetPath, isOverride);
        //    }

        //    BaiRongDataProvider.ProductDAO.Insert(productID);
        //}

        //public static void UnInstallProduct(string productID, bool isDeleteDatabase, bool isDeleteFiles)
        //{
        //    if (isDeleteDatabase)
        //    {
        //        string sqlPath = PathUtils.GetUnInstallerProductSqlFilePath(productID, BaiRongDataProvider.DatabaseType);
        //        if (FileUtils.IsFileExists(sqlPath))
        //        {
        //            StringBuilder errorBuilder = new StringBuilder();
        //            BaiRongDataProvider.DatabaseDAO.ExecuteSqlInFile(sqlPath, errorBuilder);
        //        }
        //    }

        //    BaiRongDataProvider.ProductDAO.Delete(productID);

        //    if (isDeleteFiles)
        //    {
        //        string rootDirectoryPath = PathUtils.MapPath("~/");
        //        string syncDirectoryPath = PathUtils.Combine(PathUtils.GetProductPath(productID), DirectoryUtils.Products.Directory_Files);

        //        DirectoryUtils.DeleteFilesSync(rootDirectoryPath, syncDirectoryPath);

        //        //if (StringUtils.EqualsIgnoreCase(productID, AppManager.Space.AppID))
        //        //{
        //        //    DirectoryUtils.DeleteDirectoryIfExists(PathUtils.MapPath(string.Format("~/{0}", FileConfigManager.Instance.SpaceDirectoryName)));
        //        //}
        //    }
        //}
    }
}
