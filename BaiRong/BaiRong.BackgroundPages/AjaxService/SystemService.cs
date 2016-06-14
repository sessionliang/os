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

        public NameValueCollection GetCountArray(string userKeyPrefix)//���ȼ���ʾ
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

        #region �����޸�

        public NameValueCollection Hotfix(bool isDownload, string hotfixID, string userKeyPrefix)
        {
            string cacheTotalCountKey = userKeyPrefix + CACHE_TOTAL_COUNT;
            string cacheCurrentCountKey = userKeyPrefix + CACHE_CURRENT_COUNT;
            string cacheMessageKey = userKeyPrefix + CACHE_MESSAGE;

            CacheUtils.Max(cacheTotalCountKey, "0");//�洢��Ҫ��ҳ������
            CacheUtils.Max(cacheCurrentCountKey, "0");//�洢��ǰ��ҳ������
            CacheUtils.Max(cacheMessageKey, string.Empty);//�洢��Ϣ

            //���ء����н�����͡�������Ϣ�����ַ�������
            NameValueCollection retval = new NameValueCollection();

            try
            {
                int totalCount = 5;
                int currentCount = 0;
                CacheUtils.Max(cacheTotalCountKey, totalCount.ToString());//�洢��Ҫ��ҳ������
                CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//�洢��ǰ��ҳ������

                string filePath = PathUtils.GetTemporaryFilesPath(StringUtils.Constants.Hotfix_FileName);

                //����������
                if (!isDownload)
                {
                    currentCount++;
                    CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());
                    CacheUtils.Max(cacheMessageKey, "��������������...");

                    FileUtils.DeleteFileIfExists(filePath);
                    string fileUrl = StringUtils.Constants.GetHotfixUrl(hotfixID);
                    WebClientUtils.SaveRemoteFileToLocal(fileUrl, filePath);
                }

                //��ѹ������
                currentCount++;
                CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());
                CacheUtils.Max(cacheMessageKey, "���ڽ�ѹ������...");

                string directoryPath = PathUtils.GetTemporaryFilesPath(StringUtils.Constants.Hotfix_DirectoryName);
                DirectoryUtils.DeleteDirectoryIfExists(directoryPath);
                ZipUtils.UnpackFiles(filePath, directoryPath);

                //���������޷��������ݿ⣬�汾�����ܹ��������ݿ�
                //�汾����ʱ��Ҫ����������Ŀ¼���version.txt�ļ�������Ϊ�汾�ţ�����������SiteFiles/Module/[moduleID]/Upgrade/[databaseType]�ļ��н��жԱ�ִ��SQL����
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

                //�������ݿⲢ��Apps��Files�ļ����и����ļ�
                currentCount++;
                CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());
                CacheUtils.Max(cacheMessageKey, "�����������ݿ�...");
                string errorMessage = string.Empty;
                AppManager.Upgrade(true, version, out errorMessage);

                //�����ļ�
                currentCount++;
                CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());
                CacheUtils.Max(cacheMessageKey, "���ڸ��������ļ�...");

                string rootDirectoryPath = PathUtils.MapPath("~/");
                DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
                if (directoryInfo.GetFileSystemInfos() != null)
                {
                    foreach (FileSystemInfo fileSystemInfo in directoryInfo.GetFileSystemInfos())
                    {
                        string destPath = Path.Combine(rootDirectoryPath, fileSystemInfo.Name);
                        if (fileSystemInfo is FileInfo)
                        {
                            //web.config��Ҫ��������
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

                //����bin�ļ��У�����ϵͳ
                string binDirectoryPath = PathUtils.Combine(directoryPath, "bin");
                if (DirectoryUtils.IsDirectoryExists(binDirectoryPath))
                {
                    DirectoryUtils.Copy(binDirectoryPath, Path.Combine(rootDirectoryPath, "bin"), true);
                }

                //����Web.config
                string configFilePath = PathUtils.Combine(directoryPath, "Web.config");
                string oldConfigFilePath = Path.Combine(rootDirectoryPath, "Web.config");
                if (FileUtils.IsFileExists(configFilePath))
                {
                    if (FileUtils.IsFileExists(oldConfigFilePath))
                    {
                        //���ȱ���ԭ��web.config
                        FileUtils.CopyFile(oldConfigFilePath, oldConfigFilePath + "_bak");

                        //ԭ����ConnectionString
                        string connectionString = ConfigUtils.GetAppSettingValue("ConnectionString");

                        //����web.config
                        FileUtils.CopyFile(configFilePath, oldConfigFilePath, true);

                        //�޸�ConnectionString
                        ConfigUtils.SetAppSettingValue("ConnectionString", connectionString);

                    }
                }

                //�޸�IIS����
                //IISManager

                currentCount++;
                CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//�洢��ǰ��ҳ������

                //ɾ���������Լ���ѹ�ļ���add at 2015-05-11
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

            CacheUtils.Remove(cacheTotalCountKey);//ȡ���洢��Ҫ��ҳ������
            CacheUtils.Remove(cacheCurrentCountKey);//ȡ���洢��ǰ��ҳ������
            CacheUtils.Remove(cacheMessageKey);//ȡ���洢��Ϣ

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
