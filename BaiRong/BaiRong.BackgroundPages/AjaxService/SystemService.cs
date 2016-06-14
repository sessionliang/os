using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Collections;
using System.Web;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System.Web.UI;

using BaiRong.Core;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core.Net;
using BaiRong.Core.IO;
using BaiRong.Core.Data.Provider;


using BaiRong.BackgroundPages;

namespace BaiRong.BackgroundPages.Service
{
    public class SystemService : Page
    {
        public const string CACHE_TOTAL_COUNT = "_TotalCount";
        public const string CACHE_CURRENT_COUNT = "_CurrentCount";
        public const string CACHE_MESSAGE = "_Message";

        public void Page_Load(object sender, System.EventArgs e)
        {
            string type = base.Request.QueryString["type"];
            string userKeyPrefix = base.Request["userKeyPrefix"];
            NameValueCollection retval = new NameValueCollection();
            string retString = string.Empty;

            if (type == "GetCountArray")
            {
                retval = GetCountArray(userKeyPrefix);
            }
            else if (type == "Hotfix")
            {
                bool isDownload = TranslateUtils.ToBool(base.Request.Form["isDownload"]);
                string hotfixID = base.Request.Form["hotfixID"];
                retval = Hotfix(isDownload, hotfixID, userKeyPrefix);
            }
            else if (type == "UpdateSiteYunState")
            {
                int orderID = TranslateUtils.ToInt(base.Request.Form["orderID"]);
                string redirectUrl = base.Request.Form["redirectUrl"];
                UpdateSiteYunState(orderID, redirectUrl);
            }
            else if (type == "GetLoadingDepartments")
            {
                int parentID = TranslateUtils.ToInt(base.Request["parentID"]);
                string loadingType = base.Request["loadingType"];
                string additional = base.Request["additional"];
                retString = GetLoadingDepartments(parentID, loadingType, additional);
            }
            else if (type == "GetLoadingAreas")
            {
                int parentID = TranslateUtils.ToInt(base.Request["parentID"]);
                string loadingType = base.Request["loadingType"];
                string additional = base.Request["additional"];
                retString = GetLoadingAreas(parentID, loadingType, additional);
            }

            if (!string.IsNullOrEmpty(retString))
            {
                Page.Response.Write(retString);
                Page.Response.End();
            }
            else
            {
                string jsonString = TranslateUtils.NameValueCollectionToJsonString(retval);
                Page.Response.Write(jsonString);
                Page.Response.End();
            }
        }

        public NameValueCollection GetCountArray(string userKeyPrefix)//进度及显示
        {
            NameValueCollection retval = new NameValueCollection();
            if (CacheUtils.Get(userKeyPrefix + CACHE_TOTAL_COUNT) != null && CacheUtils.Get(userKeyPrefix + CACHE_CURRENT_COUNT) != null && CacheUtils.Get(userKeyPrefix + CACHE_MESSAGE) != null)
            {
                int totalCount = TranslateUtils.ToInt((string)CacheUtils.Get(userKeyPrefix + CACHE_TOTAL_COUNT));
                int currentCount = TranslateUtils.ToInt((string)CacheUtils.Get(userKeyPrefix + CACHE_CURRENT_COUNT));
                string message = (string)CacheUtils.Get(userKeyPrefix + CACHE_MESSAGE);

                retval.Add("totalCount", totalCount.ToString());
                retval.Add("currentCount", currentCount.ToString());
                retval.Add("message", message);
            }
            return retval;
        }

        public void UpdateSiteYunState(int orderID, string redirectUrl)
        {
            bool isUpdate = false;
            if (ConfigManager.Additional.SiteYun_OrderID != orderID)
            {
                ConfigManager.Additional.SiteYun_OrderID = orderID;
                isUpdate = true;
            }
            if (ConfigManager.Additional.SiteYun_RedirectUrl != redirectUrl)
            {
                ConfigManager.Additional.SiteYun_RedirectUrl = redirectUrl;
                isUpdate = true;
            }
            if (isUpdate)
            {
                BaiRongDataProvider.ConfigDAO.Update(ConfigManager.Instance);
            }
        }

        #region 升级修复

        public NameValueCollection Hotfix(bool isDownload, string hotfixID, string userKeyPrefix)
        {
            string cacheTotalCountKey = userKeyPrefix + CACHE_TOTAL_COUNT;
            string cacheCurrentCountKey = userKeyPrefix + CACHE_CURRENT_COUNT;
            string cacheMessageKey = userKeyPrefix + CACHE_MESSAGE;

            CacheUtils.Max(cacheTotalCountKey, "0");//存储需要的页面总数
            CacheUtils.Max(cacheCurrentCountKey, "0");//存储当前的页面总数
            CacheUtils.Max(cacheMessageKey, string.Empty);//存储消息

            //返回“运行结果”和“错误信息”的字符串数组
            NameValueCollection retval = new NameValueCollection();

            try
            {
                int totalCount = 5;
                int currentCount = 0;
                CacheUtils.Max(cacheTotalCountKey, totalCount.ToString());//存储需要的页面总数
                CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数

                string filePath = PathUtils.GetTemporaryFilesPath(StringUtils.Constants.Hotfix_FileName);

                //下载升级包
                if (!isDownload)
                {
                    currentCount++;
                    CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());
                    CacheUtils.Max(cacheMessageKey, "正在下载升级包...");

                    FileUtils.DeleteFileIfExists(filePath);
                    string fileUrl = StringUtils.Constants.GetHotfixUrl(hotfixID);
                    WebClientUtils.SaveRemoteFileToLocal(fileUrl, filePath);
                }

                //解压升级包
                currentCount++;
                CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());
                CacheUtils.Max(cacheMessageKey, "正在解压升级包...");

                string directoryPath = PathUtils.GetTemporaryFilesPath(StringUtils.Constants.Hotfix_DirectoryName);
                DirectoryUtils.DeleteDirectoryIfExists(directoryPath);
                ZipUtils.UnpackFiles(filePath, directoryPath);

                //补丁升级无法操作数据库，版本升级能够操作数据库
                //版本升级时需要在升级包根目录添加version.txt文件，内容为版本号，升级程序将与SiteFiles/Module/[moduleID]/Upgrade/[databaseType]文件夹进行对比执行SQL命令
                string versionFilePath = PathUtils.Combine(directoryPath, "version.txt");
                string version = string.Empty;
                try
                {
                    if (FileUtils.IsFileExists(versionFilePath))
                    {
                        version = FileUtils.ReadText(versionFilePath, ECharset.utf_8);
                        version = version.Trim();

                        FileUtils.DeleteFileIfExists(versionFilePath);
                    }
                }
                catch { }

                //升级数据库并从Apps的Files文件夹中覆盖文件
                currentCount++;
                CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());
                CacheUtils.Max(cacheMessageKey, "正在升级数据库...");
                string errorMessage = string.Empty;
                AppManager.Upgrade(true, version, out errorMessage);

                //覆盖文件
                currentCount++;
                CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());
                CacheUtils.Max(cacheMessageKey, "正在覆盖升级文件...");

                string rootDirectoryPath = PathUtils.MapPath("~/");
                DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
                if (directoryInfo.GetFileSystemInfos() != null)
                {
                    foreach (FileSystemInfo fileSystemInfo in directoryInfo.GetFileSystemInfos())
                    {
                        string destPath = Path.Combine(rootDirectoryPath, fileSystemInfo.Name);
                        if (fileSystemInfo is FileInfo)
                        {
                            //web.config需要单独更新
                            if (!StringUtils.EqualsIgnoreCase(fileSystemInfo.Name, "Web.config"))
                            {
                                FileUtils.CopyFile(fileSystemInfo.FullName, destPath, true);
                            }
                        }
                        else if (fileSystemInfo is DirectoryInfo)
                        {
                            if (!StringUtils.EqualsIgnoreCase(fileSystemInfo.Name, "bin"))
                            {
                                DirectoryUtils.Copy(fileSystemInfo.FullName, destPath, true);
                            }
                        }
                    }
                }

                //覆盖bin文件夹，重启系统
                string binDirectoryPath = PathUtils.Combine(directoryPath, "bin");
                if (DirectoryUtils.IsDirectoryExists(binDirectoryPath))
                {
                    DirectoryUtils.Copy(binDirectoryPath, Path.Combine(rootDirectoryPath, "bin"), true);
                }

                //配置Web.config
                string configFilePath = PathUtils.Combine(directoryPath, "Web.config");
                string oldConfigFilePath = Path.Combine(rootDirectoryPath, "Web.config");
                if (FileUtils.IsFileExists(configFilePath))
                {
                    if (FileUtils.IsFileExists(oldConfigFilePath))
                    {
                        //首先备份原来web.config
                        FileUtils.CopyFile(oldConfigFilePath, oldConfigFilePath + "_bak");

                        //原来的ConnectionString
                        string connectionString = ConfigUtils.GetAppSettingValue("ConnectionString");

                        //覆盖web.config
                        FileUtils.CopyFile(configFilePath, oldConfigFilePath, true);

                        //修改ConnectionString
                        ConfigUtils.SetAppSettingValue("ConnectionString", connectionString);

                    }
                }

                //修改IIS设置
                //IISManager

                currentCount++;
                CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数

                //删除升级包以及解压文件，add at 2015-05-11
                FileUtils.DeleteFileIfExists(filePath);
                DirectoryUtils.DeleteDirectoryIfExists(directoryPath);

                retval.Add("isUpgrade", "true");
                retval.Add("resultMessage", string.Empty);
                retval.Add("errorMessage", string.Empty);

            }
            catch (Exception ex)
            {
                retval.Add("resultMessage", string.Empty);
                retval.Add("errorMessage", ex.Message);

                LogUtils.AddErrorLog(ex);
            }

            CacheUtils.Remove(cacheTotalCountKey);//取消存储需要的页面总数
            CacheUtils.Remove(cacheCurrentCountKey);//取消存储当前的页面总数
            CacheUtils.Remove(cacheMessageKey);//取消存储消息

            return retval;
        }

        #endregion

        public string GetLoadingDepartments(int parentID, string loadingType, string additional)
        {
            ArrayList arraylist = new ArrayList();

            EDepartmentLoadingType eLoadingType = EDepartmentLoadingTypeUtils.GetEnumType(loadingType);

            ArrayList departmentIDArrayList = BaiRongDataProvider.DepartmentDAO.GetDepartmentIDArrayListByParentID(parentID);
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(RuntimeUtils.DecryptStringByTranslate(additional));
            ArrayList allDepartmentIDArrayList = new ArrayList();
            if (!string.IsNullOrEmpty(nameValueCollection["DepartmentIDCollection"]))
            {
                allDepartmentIDArrayList = TranslateUtils.StringCollectionToIntArrayList(nameValueCollection["DepartmentIDCollection"]);
                nameValueCollection.Remove("DepartmentIDCollection");
                foreach (int departmentID in departmentIDArrayList)
                {
                    DepartmentInfo departmentInfo = DepartmentManager.GetDepartmentInfo(departmentID);
                    if (departmentInfo.ParentID != 0 || allDepartmentIDArrayList.Contains(departmentID))
                    {
                        arraylist.Add(BackgroundDepartment.GetDepartmentRowHtml(departmentInfo, eLoadingType, nameValueCollection));
                    }
                }
            }
            else
            {
                foreach (int departmentID in departmentIDArrayList)
                {
                    DepartmentInfo departmentInfo = DepartmentManager.GetDepartmentInfo(departmentID);
                    arraylist.Add(BackgroundDepartment.GetDepartmentRowHtml(departmentInfo, eLoadingType, nameValueCollection));
                }
            }

            StringBuilder builder = new StringBuilder();
            foreach (string html in arraylist)
            {
                builder.Append(html);
            }
            return builder.ToString();
        }

        public string GetLoadingAreas(int parentID, string loadingType, string additional)
        {
            ArrayList arraylist = new ArrayList();

            EAreaLoadingType eLoadingType = EAreaLoadingTypeUtils.GetEnumType(loadingType);

            ArrayList areaIDArrayList = BaiRongDataProvider.AreaDAO.GetAreaIDArrayListByParentID(parentID);
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(RuntimeUtils.DecryptStringByTranslate(additional));
            ArrayList allAreaIDArrayList = new ArrayList();
            if (!string.IsNullOrEmpty(nameValueCollection["AreaIDCollection"]))
            {
                allAreaIDArrayList = TranslateUtils.StringCollectionToIntArrayList(nameValueCollection["AreaIDCollection"]);
                nameValueCollection.Remove("AreaIDCollection");
                foreach (int areaID in areaIDArrayList)
                {
                    AreaInfo areaInfo = AreaManager.GetAreaInfo(areaID);
                    if (areaInfo.ParentID != 0 || allAreaIDArrayList.Contains(areaID))
                    {
                        arraylist.Add(BackgroundArea.GetAreaRowHtml(areaInfo, eLoadingType, nameValueCollection));
                    }
                }
            }
            else
            {
                foreach (int areaID in areaIDArrayList)
                {
                    AreaInfo areaInfo = AreaManager.GetAreaInfo(areaID);
                    arraylist.Add(BackgroundArea.GetAreaRowHtml(areaInfo, eLoadingType, nameValueCollection));
                }
            }

            StringBuilder builder = new StringBuilder();
            foreach (string html in arraylist)
            {
                builder.Append(html);
            }
            return builder.ToString();
        }
    }
}
