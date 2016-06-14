using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Model;
using System.Collections.Specialized;
using SiteServer.CMS.Core.Security;
using System;

using BaiRong.Core.Service;
using BaiRong.Model.Service;
using BaiRong.Core.Data.Provider;
using System.Collections.Generic;
using System.Text;

namespace SiteServer.CMS.Core
{
    public sealed class PublishmentSystemManager
    {
        private PublishmentSystemManager()
        {

        }

        private const string CacheFileName = "PublishmentSystemCache.txt";

        static PublishmentSystemManager()
        {
            FileWatcherClass fileWatcher = new FileWatcherClass(PathUtility.GetCacheFilePath(CacheFileName));
            fileWatcher.OnFileChange += new FileWatcherClass.FileChange(fileWatcher_OnFileChange);
        }

        private static void fileWatcher_OnFileChange(object sender, EventArgs e)
        {
            CacheUtils.Remove(cacheKey);
        }

        public static PublishmentSystemInfo GetPublishmentSystemInfo(int publishmentSystemID)
        {
            DictionaryEntryArrayList dictionaryEntryArrayList = GetPublishmentSystemInfoDictionaryEntryArrayList();

            foreach (DictionaryEntry entry in dictionaryEntryArrayList)
            {
                int thePublishmentSystemID = (int)entry.Key;
                if (thePublishmentSystemID == publishmentSystemID)
                {
                    PublishmentSystemInfo publishmentSystemInfo = entry.Value as PublishmentSystemInfo;
                    return publishmentSystemInfo;
                }
            }
            return null;
        }

        public static PublishmentSystemInfo GetPublishmentSystemInfoBySiteName(string publishmentSystemName)
        {
            DictionaryEntryArrayList dictionaryEntryArrayList = GetPublishmentSystemInfoDictionaryEntryArrayList();

            foreach (DictionaryEntry entry in dictionaryEntryArrayList)
            {
                PublishmentSystemInfo publishmentSystemInfo = entry.Value as PublishmentSystemInfo;
                if (StringUtils.EqualsIgnoreCase(publishmentSystemInfo.PublishmentSystemName, publishmentSystemName))
                {
                    return publishmentSystemInfo;
                }
            }
            return null;
        }

        public static PublishmentSystemInfo GetPublishmentSystemInfoByDirectory(string publishmentSystemDir)
        {
            DictionaryEntryArrayList dictionaryEntryArrayList = GetPublishmentSystemInfoDictionaryEntryArrayList();

            foreach (DictionaryEntry entry in dictionaryEntryArrayList)
            {
                PublishmentSystemInfo publishmentSystemInfo = entry.Value as PublishmentSystemInfo;
                if (StringUtils.EqualsIgnoreCase(publishmentSystemInfo.PublishmentSystemDir, publishmentSystemDir))
                {
                    return publishmentSystemInfo;
                }
            }
            return null;
        }

        public static List<EPublishmentSystemType> GetPublishmentSystemTypeList()
        {
            List<EPublishmentSystemType> list = new List<EPublishmentSystemType>();

            DictionaryEntryArrayList dictionaryEntryArrayList = GetPublishmentSystemInfoDictionaryEntryArrayList();

            foreach (DictionaryEntry entry in dictionaryEntryArrayList)
            {
                PublishmentSystemInfo publishmentSystemInfo = entry.Value as PublishmentSystemInfo;
                if (!list.Contains(publishmentSystemInfo.PublishmentSystemType))
                {
                    list.Add(publishmentSystemInfo.PublishmentSystemType);
                }
            }
            return list;
        }

        public static ArrayList GetPublishmentSystemIDArrayList()
        {
            ICollection collection = GetPublishmentSystemInfoDictionaryEntryArrayList().Keys;
            ArrayList arraylist = new ArrayList();
            arraylist.AddRange(collection);
            return arraylist;
        }

        public static ArrayList GetPublishmentSystemIDWithoutUserCenterArrayList()
        {
            ICollection collection = GetPublishmentSystemInfoDictionaryEntryArrayList().Keys;
            ArrayList arraylist = new ArrayList();
            foreach (int publishmentSystemID in collection)
            {
                PublishmentSystemInfo publishmentSystemInfo = GetPublishmentSystemInfo(publishmentSystemID);
                if (EPublishmentSystemTypeUtils.IsUserCenter(publishmentSystemInfo.PublishmentSystemType))
                    continue;
                arraylist.Add(publishmentSystemID);
            }
            return arraylist;
        }

        public static List<int> GetPublishmentSystemIDList()
        {
            ICollection collection = GetPublishmentSystemInfoDictionaryEntryArrayList().Keys;
            List<int> list = new List<int>();
            foreach (int publishmentSystemID in collection)
            {
                list.Add(publishmentSystemID);
            }
            return list;
        }

        public static List<int> GetPublishmentSystemIDWithoutUserCenterList()
        {
            ICollection collection = GetPublishmentSystemInfoDictionaryEntryArrayList().Keys;
            List<int> list = new List<int>();
            foreach (int publishmentSystemID in collection)
            {
                PublishmentSystemInfo publishmentSystemInfo = GetPublishmentSystemInfo(publishmentSystemID);
                if (EPublishmentSystemTypeUtils.Equals(publishmentSystemInfo.PublishmentSystemType, AppManager.UserCenter.AppID))
                    continue;
                list.Add(publishmentSystemID);
            }
            return list;
        }

        public static ArrayList GetPublishmentSystemIDArrayList(EPublishmentSystemType publishmentSystemType)
        {
            ArrayList arraylist = new ArrayList();

            ArrayList publishmentSystemIDArrayList = PublishmentSystemManager.GetPublishmentSystemIDArrayList();
            foreach (int publishmentSystemID in publishmentSystemIDArrayList)
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                if (publishmentSystemInfo.PublishmentSystemType == publishmentSystemType)
                {
                    arraylist.Add(publishmentSystemInfo.PublishmentSystemID);
                }
            }

            return arraylist;
        }

        public static ArrayList GetPublishmentSystemIDArrayListOrderByLevel()
        {
            ArrayList retval = new ArrayList();

            ArrayList publishmentSystemIDArrayList = PublishmentSystemManager.GetPublishmentSystemIDArrayList();
            ArrayList publishmentSystemInfoArrayList = new ArrayList();
            Hashtable parentWithChildren = new Hashtable();
            int hqPublishmentSystemID = 0;
            foreach (int publishmentSystemID in publishmentSystemIDArrayList)
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                if (publishmentSystemInfo.IsHeadquarters)
                {
                    hqPublishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
                }
                else
                {
                    if (publishmentSystemInfo.ParentPublishmentSystemID == 0)
                    {
                        publishmentSystemInfoArrayList.Add(publishmentSystemInfo);
                    }
                    else
                    {
                        ArrayList children = new ArrayList();
                        if (parentWithChildren.Contains(publishmentSystemInfo.ParentPublishmentSystemID))
                        {
                            children = (ArrayList)parentWithChildren[publishmentSystemInfo.ParentPublishmentSystemID];
                        }
                        children.Add(publishmentSystemInfo);
                        parentWithChildren[publishmentSystemInfo.ParentPublishmentSystemID] = children;
                    }
                }
            }

            if (hqPublishmentSystemID > 0)
            {
                retval.Add(hqPublishmentSystemID);
            }
            foreach (PublishmentSystemInfo publishmentSystemInfo in publishmentSystemInfoArrayList)
            {
                AddPublishmentSystemIDArrayList(retval, publishmentSystemInfo, parentWithChildren, 0);
            }
            return retval;
        }

        private static void AddPublishmentSystemIDArrayList(ArrayList dataSource, PublishmentSystemInfo publishmentSystemInfo, Hashtable parentWithChildren, int level)
        {
            dataSource.Add(publishmentSystemInfo.PublishmentSystemID);

            if (parentWithChildren[publishmentSystemInfo.PublishmentSystemID] != null)
            {
                ArrayList children = (ArrayList)parentWithChildren[publishmentSystemInfo.PublishmentSystemID];
                level++;
                foreach (PublishmentSystemInfo subSiteInfo in children)
                {
                    AddPublishmentSystemIDArrayList(dataSource, subSiteInfo, parentWithChildren, level);
                }
            }
        }

        public static void GetAllParentPublishmentSystemIDArrayList(ArrayList parentPublishmentSystemIDs, ICollection publishmentSystemIDCollection, int publishmentSystemID)
        {
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            int parentPublishmentSystemID = -1;
            foreach (int psID in publishmentSystemIDCollection)
            {
                if (psID == publishmentSystemInfo.ParentPublishmentSystemID)
                {
                    parentPublishmentSystemID = psID;
                    break;
                }
            }
            if (parentPublishmentSystemID != -1)
            {
                parentPublishmentSystemIDs.Add(parentPublishmentSystemID);
                GetAllParentPublishmentSystemIDArrayList(parentPublishmentSystemIDs, publishmentSystemIDCollection, parentPublishmentSystemID);
            }
        }

        public static bool IsExists(int publishmentSystemID)
        {
            if (publishmentSystemID == 0) return false;
            return (PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID) != null);
        }

        public static string GetGroupSN(int publishmentSystemID)
        {
            if (publishmentSystemID > 0)
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                if (publishmentSystemID != null)
                {
                    return publishmentSystemInfo.GroupSN;
                }
            }
            return string.Empty;
        }

        public static List<string> GetAuxiliaryTableNameList(PublishmentSystemInfo publishmentSystemInfo)
        {
            List<string> list = new List<string>();

            list.Add(publishmentSystemInfo.AuxiliaryTableForContent);
            list.Add(publishmentSystemInfo.AuxiliaryTableForGovInteract);
            list.Add(publishmentSystemInfo.AuxiliaryTableForGovPublic);
            list.Add(publishmentSystemInfo.AuxiliaryTableForJob);
            list.Add(publishmentSystemInfo.AuxiliaryTableForVote);
            if (publishmentSystemInfo.PublishmentSystemType == EPublishmentSystemType.B2C || publishmentSystemInfo.PublishmentSystemType == EPublishmentSystemType.WeixinB2C)
            {
                list.Add(publishmentSystemInfo.AuxiliaryTableForGoods);
            }
            return list;
        }

        public static ETableStyle GetTableStyle(PublishmentSystemInfo publishmentSystemInfo, string tableName)
        {
            ETableStyle tableStyle = ETableStyle.UserDefined;

            if (StringUtils.EqualsIgnoreCase(tableName, publishmentSystemInfo.AuxiliaryTableForContent))
            {
                tableStyle = ETableStyle.BackgroundContent;
            }
            else if (StringUtils.EqualsIgnoreCase(tableName, publishmentSystemInfo.AuxiliaryTableForGovInteract))
            {
                tableStyle = ETableStyle.GovInteractContent;
            }
            else if (StringUtils.EqualsIgnoreCase(tableName, publishmentSystemInfo.AuxiliaryTableForGovPublic))
            {
                tableStyle = ETableStyle.GovPublicContent;
            }
            else if (StringUtils.EqualsIgnoreCase(tableName, publishmentSystemInfo.AuxiliaryTableForJob))
            {
                tableStyle = ETableStyle.JobContent;
            }
            else if (StringUtils.EqualsIgnoreCase(tableName, publishmentSystemInfo.AuxiliaryTableForVote))
            {
                tableStyle = ETableStyle.VoteContent;
            }
            else if (StringUtils.EqualsIgnoreCase(tableName, DataProvider.PublishmentSystemDAO.TableName))
            {
                tableStyle = ETableStyle.Site;
            }
            else if (StringUtils.EqualsIgnoreCase(tableName, DataProvider.NodeDAO.TableName))
            {
                tableStyle = ETableStyle.Channel;
            }
            else if (StringUtils.EqualsIgnoreCase(tableName, DataProvider.InputContentDAO.TableName))
            {
                tableStyle = ETableStyle.InputContent;
            }
            return tableStyle;
        }

        public static int GetPublishmentSystemLevel(int publishmentSystemID)
        {
            int level = 0;
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            if (publishmentSystemInfo.ParentPublishmentSystemID != 0)
            {
                level++;
                level += GetPublishmentSystemLevel(publishmentSystemInfo.ParentPublishmentSystemID);
            }
            return level;
        }

        public static ArrayList GetPublishmentSystemInfoByLevel(int level)
        {
            ArrayList arraylist = new ArrayList();
            if (level == 0)
            {
                foreach (int publishmentSystemID in PublishmentSystemManager.GetPublishmentSystemIDArrayList())
                {
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    if (publishmentSystemInfo.ParentPublishmentSystemID == 0 && publishmentSystemInfo.IsHeadquarters == false)
                    {
                        arraylist.Add(publishmentSystemInfo);
                    }
                }
            }
            else
            {
                foreach (int publishmentSystemID in PublishmentSystemManager.GetPublishmentSystemIDArrayList())
                {
                    int theLevel = PublishmentSystemManager.GetPublishmentSystemLevel(publishmentSystemID);
                    if (theLevel == level)
                    {
                        PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                        arraylist.Add(publishmentSystemInfo);
                    }
                }
            }
            return arraylist;
        }

        public static void AddListItems(ListControl listControl)
        {
            ArrayList publishmentSystemIDArrayList = PublishmentSystemManager.GetPublishmentSystemIDArrayList();
            ArrayList mySystemInfoArrayList = new ArrayList();
            Hashtable parentWithChildren = new Hashtable();
            PublishmentSystemInfo hqPublishmentSystemInfo = null;
            foreach (int publishmentSystemID in publishmentSystemIDArrayList)
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                if (publishmentSystemInfo.IsHeadquarters)
                {
                    hqPublishmentSystemInfo = publishmentSystemInfo;
                }
                else
                {
                    if (publishmentSystemInfo.ParentPublishmentSystemID == 0)
                    {
                        mySystemInfoArrayList.Add(publishmentSystemInfo);
                    }
                    else
                    {
                        ArrayList children = new ArrayList();
                        if (parentWithChildren.Contains(publishmentSystemInfo.ParentPublishmentSystemID))
                        {
                            children = (ArrayList)parentWithChildren[publishmentSystemInfo.ParentPublishmentSystemID];
                        }
                        children.Add(publishmentSystemInfo);
                        parentWithChildren[publishmentSystemInfo.ParentPublishmentSystemID] = children;
                    }
                }
            }
            if (hqPublishmentSystemInfo != null)
            {
                AddListItem(listControl, hqPublishmentSystemInfo, parentWithChildren, 0);
            }
            foreach (PublishmentSystemInfo publishmentSystemInfo in mySystemInfoArrayList)
            {
                AddListItem(listControl, publishmentSystemInfo, parentWithChildren, 0);
            }
        }

        private static void AddListItem(ListControl listControl, PublishmentSystemInfo publishmentSystemInfo, Hashtable parentWithChildren, int level)
        {
            string padding = string.Empty;
            for (int i = 0; i < level; i++)
            {
                padding += "　";
            }
            if (level > 0)
            {
                padding += "└ ";
            }

            if (parentWithChildren[publishmentSystemInfo.PublishmentSystemID] != null)
            {
                ArrayList children = (ArrayList)parentWithChildren[publishmentSystemInfo.PublishmentSystemID];
                listControl.Items.Add(new ListItem(padding + publishmentSystemInfo.PublishmentSystemName + string.Format("({0})", children.Count), publishmentSystemInfo.PublishmentSystemID.ToString()));
                level++;
                foreach (PublishmentSystemInfo subSiteInfo in children)
                {
                    AddListItem(listControl, subSiteInfo, parentWithChildren, level);
                }
            }
            else
            {
                listControl.Items.Add(new ListItem(padding + publishmentSystemInfo.PublishmentSystemName, publishmentSystemInfo.PublishmentSystemID.ToString()));
            }
        }

        public static int GetParentPublishmentSystemID(int publishmentSystemID)
        {
            int parentPublishmentSystemID = 0;
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            if (publishmentSystemInfo != null && publishmentSystemInfo.IsHeadquarters == false)
            {
                parentPublishmentSystemID = publishmentSystemInfo.ParentPublishmentSystemID;
                if (parentPublishmentSystemID == 0)
                {
                    parentPublishmentSystemID = DataProvider.PublishmentSystemDAO.GetPublishmentSystemIDByIsHeadquarters();
                }
            }
            return parentPublishmentSystemID;
        }

        public static void ClearCache(bool isAddAjaxUrl)
        {
            CacheUtils.Remove(cacheKey);
            CacheManager.UpdateTemporaryCacheFile(CacheFileName);
            if (isAddAjaxUrl)
            {
                AjaxUrlManager.AddAjaxUrl(PageUtils.API.GetPublishmentSystemClearCacheUrl());
            }
        }

        public static DictionaryEntryArrayList GetPublishmentSystemInfoDictionaryEntryArrayList()
        {

            if (CacheUtils.Get(cacheKey) == null)
            {
                DictionaryEntryArrayList dictionaryFormDB = DataProvider.PublishmentSystemDAO.GetPublishmentSystemInfoDictionaryEntryArrayList();
                DictionaryEntryArrayList sl = new DictionaryEntryArrayList();
                foreach (DictionaryEntry entry in dictionaryFormDB)
                {
                    PublishmentSystemInfo publishmentSystemInfo = entry.Value as PublishmentSystemInfo;
                    //设置路径
                    if (publishmentSystemInfo != null)
                    {
                        publishmentSystemInfo.PublishmentSystemDir = GetPublishmentSystemDir(dictionaryFormDB, publishmentSystemInfo);
                        sl.Add(entry);
                    }
                }
                CacheUtils.Max(cacheKey, sl);
                return sl;
            }
            return CacheUtils.Get(cacheKey) as DictionaryEntryArrayList;

        }

        private static string GetPublishmentSystemDir(DictionaryEntryArrayList dictionaryFormDB, PublishmentSystemInfo publishmentSystemInfo)
        {
            if (publishmentSystemInfo == null || publishmentSystemInfo.IsHeadquarters) return string.Empty;
            if (publishmentSystemInfo.ParentPublishmentSystemID != 0)
            {
                PublishmentSystemInfo parent = null;
                foreach (DictionaryEntry entry in dictionaryFormDB)
                {
                    int thePublishmentSystemID = (int)entry.Key;
                    if (thePublishmentSystemID == publishmentSystemInfo.ParentPublishmentSystemID)
                    {
                        parent = entry.Value as PublishmentSystemInfo;
                        break;
                    }
                }
                return PathUtils.Combine(GetPublishmentSystemDir(dictionaryFormDB, parent), PathUtils.GetDirectoryName(publishmentSystemInfo.PublishmentSystemDir));
            }
            else
            {
                return PathUtils.GetDirectoryName(publishmentSystemInfo.PublishmentSystemDir);
            }
        }

        public static void AddSystemPublishTaskIfNotExists(int publishmentSystemID)
        {
            if (!BaiRongDataProvider.TaskDAO.IsSystemTaskExists(AppManager.CMS.AppID, publishmentSystemID, EServiceType.Publish))
            {
                TaskPublishInfo taskPublishInfo = new TaskPublishInfo(string.Empty);
                taskPublishInfo.PublishTypes = EPublishTypeUtils.GetAllPublishTypes();

                TaskInfo taskInfo = new TaskInfo(AppManager.CMS.AppID);
                taskInfo.TaskName = "应用默认发布任务";
                taskInfo.PublishmentSystemID = publishmentSystemID;
                taskInfo.ServiceType = EServiceType.Publish;
                taskInfo.FrequencyType = EFrequencyType.JustInTime;
                taskInfo.Description = "系统默认生成的应用定时发布任务";

                taskInfo.ServiceParameters = taskPublishInfo.ToString();

                taskInfo.IsEnabled = true;
                taskInfo.IsSystemTask = true;
                taskInfo.AddDate = DateTime.Now;
                taskInfo.OnlyOnceDate = DateUtils.SqlMinValue;
                taskInfo.LastExecuteDate = DateUtils.SqlMinValue;

                BaiRongDataProvider.TaskDAO.Insert(taskInfo);
            }
        }

        public static void RemoveSystemPublishTaskIfNecessary(PublishmentSystemInfo publishmentSystemInfo)
        {
            if (!publishmentSystemInfo.Additional.IsSiteStorage && !publishmentSystemInfo.Additional.IsImageStorage && !publishmentSystemInfo.Additional.IsVideoStorage && !publishmentSystemInfo.Additional.IsFileStorage)
            {
                BaiRongDataProvider.TaskDAO.DeleteSystemTask(AppManager.CMS.AppID, publishmentSystemInfo.PublishmentSystemID, EServiceType.Publish);
            }
        }


        /******************FCKEditor Cache Start*********************/

        public static int GetPublishmentSystemIDByCache()
        {
            if (CacheManager.GetCache("PublishmentSystemManager.PublishmentSystemID." + AdminManager.Current.UserName) != null)
            {
                return (int)CacheManager.GetCache("PublishmentSystemManager.PublishmentSystemID." + AdminManager.Current.UserName);
            }
            return 0;
        }

        public static void SetPublishmentSystemIDByCache(int publishmentSystemID)
        {
            CacheManager.SetCache("PublishmentSystemManager.PublishmentSystemID." + AdminManager.Current.UserName, publishmentSystemID);
        }

        /****************** Cache *********************/

        private static readonly object lockObject = new object();
        private const string cacheKey = "SiteServer.CMS.Core.PublishmentSystemManager";

        public static void UpdateUrlRewriteFile()
        {
            string configFilePath = PathUtils.MapPath("~/SiteFiles/Configuration/UrlRewrite.config");

            ArrayList publishmentSystemIDArrayList = PublishmentSystemManager.GetPublishmentSystemIDArrayList(EPublishmentSystemType.BBS);
            if (publishmentSystemIDArrayList.Count > 0)
            {
                string filePath = PathUtils.GetSiteFilesPath("Products/Apps/BBS/UrlRewrite.config");
                string fileContent = FileUtils.ReadText(filePath, ECharset.utf_8);

                StringBuilder builder = new StringBuilder();
                foreach (int publishmentSystemID in publishmentSystemIDArrayList)
                {
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    string siteUrl = PageUtility.GetPublishmentSystemUrl(publishmentSystemInfo, string.Empty).TrimEnd('/');
                    if (siteUrl == "/")
                    {
                        siteUrl = string.Empty;
                    }
                    builder.AppendFormat(fileContent.Replace("{siteUrl}", siteUrl)).AppendLine();
                }

                string xml = string.Format(@"<?xml version=""1.0"" encoding=""utf-8"" ?> 
<RewriterConfig><Rules>

{0}

</Rules></RewriterConfig>", builder.ToString());

                FileUtils.WriteText(configFilePath, ECharset.utf_8, xml);
            }
            else
            {
                FileUtils.DeleteFileIfExists(configFilePath);
            }
        }

        #region 会员中心
        //private static readonly object userCenterLockObject = new object();
        //private const string userCenterCacheKey = "SiteServer.CMS.Core.PublishmentSystemManager.UserCenter";

        //public static DictionaryEntryArrayList GetUserCenterInfoDictionaryEntryArrayList()
        //{
        //    lock (userCenterLockObject)
        //    {
        //        if (CacheUtils.Get(userCenterCacheKey) == null)
        //        {
        //            DictionaryEntryArrayList dictionaryFromDB = DataProvider.PublishmentSystemDAO.GetPublishmentSystemInfoDictionaryEntryArrayList();
        //            DictionaryEntryArrayList sl = new DictionaryEntryArrayList();
        //            foreach (DictionaryEntry entry in dictionaryFromDB)
        //            {
        //                PublishmentSystemInfo userCenterInfo = entry.Value as PublishmentSystemInfo;
        //                if (userCenterInfo != null)
        //                {
        //                    userCenterInfo.PublishmentSystemDir = GetPublishmentSystemDir(dictionaryFromDB, userCenterInfo);
        //                    if (EPublishmentSystemTypeUtils.IsUserCenter(userCenterInfo.PublishmentSystemType))
        //                        sl.Add(entry);
        //                }
        //            }
        //            CacheUtils.Max(userCenterCacheKey,sl);
        //        }
        //        return CacheUtils.Get(userCenterCacheKey) as DictionaryEntryArrayList;
        //    }
        //}

        //public static ArrayList GetUserCenterIDArrayList()
        //{
        //    ICollection collection = GetUserCenterInfoDictionaryEntryArrayList().Keys;
        //    ArrayList arraylist = new ArrayList();
        //    arraylist.AddRange(collection);
        //    return arraylist;
        //}

        //public static PublishmentSystemInfo GetUserCenterInfo(int publishmentSystemID)
        //{
        //    DictionaryEntryArrayList dictionaryEntryArrayList = GetUserCenterInfoDictionaryEntryArrayList();

        //    foreach (DictionaryEntry entry in dictionaryEntryArrayList)
        //    {
        //        int thePublishmentSystemID = (int)entry.Key;
        //        if (thePublishmentSystemID == publishmentSystemID)
        //        {
        //            PublishmentSystemInfo publishmentSystemInfo = entry.Value as PublishmentSystemInfo;
        //            return publishmentSystemInfo;
        //        }
        //    }
        //    return null;
        //}

        //private static void AddUserCenterIDArrayList(ArrayList dataSource, PublishmentSystemInfo userCenterInfo, Hashtable parentWithChildren, int level)
        //{
        //    dataSource.Add(userCenterInfo.PublishmentSystemID);

        //    if (parentWithChildren[userCenterInfo.PublishmentSystemID] != null)
        //    {
        //        ArrayList children = (ArrayList)parentWithChildren[userCenterInfo.PublishmentSystemID];
        //        level++;
        //        foreach (PublishmentSystemInfo subSiteInfo in children)
        //        {
        //            AddUserCenterIDArrayList(dataSource, subSiteInfo, parentWithChildren, level);
        //        }
        //    }
        //}


        /// <summary>
        /// 获取唯一会员中心
        /// </summary>
        /// <returns></returns>
        public static PublishmentSystemInfo GetUniqueUserCenter()
        {
            DictionaryEntryArrayList dictionaryEntryArrayList = GetPublishmentSystemInfoDictionaryEntryArrayList();

            foreach (DictionaryEntry entry in dictionaryEntryArrayList)
            {
                PublishmentSystemInfo publishmentSystemInfo = entry.Value as PublishmentSystemInfo;
                if (EPublishmentSystemTypeUtils.Equals(publishmentSystemInfo.PublishmentSystemType, AppManager.UserCenter.AppID))
                {
                    return publishmentSystemInfo;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取唯一稿件库
        /// </summary>
        /// <returns></returns>
        public static PublishmentSystemInfo GetUniqueMLib()
        {
            DictionaryEntryArrayList dictionaryEntryArrayList = GetPublishmentSystemInfoDictionaryEntryArrayList();

            foreach (DictionaryEntry entry in dictionaryEntryArrayList)
            {
                PublishmentSystemInfo publishmentSystemInfo = entry.Value as PublishmentSystemInfo;
                if (EPublishmentSystemTypeUtils.Equals(publishmentSystemInfo.PublishmentSystemType, AppManager.MLib.AppID))
                {
                    return publishmentSystemInfo;
                }
            }
            return null;
        }


        /// <summary>
        /// 获取默认会员中心
        /// </summary>
        /// <returns></returns>
        public static PublishmentSystemInfo GetDefaultUserCenter()
        {
            DictionaryEntryArrayList dictionaryEntryArrayList = GetPublishmentSystemInfoDictionaryEntryArrayList();

            foreach (DictionaryEntry entry in dictionaryEntryArrayList)
            {
                PublishmentSystemInfo publishmentSystemInfo = entry.Value as PublishmentSystemInfo;
                if (EPublishmentSystemTypeUtils.Equals(publishmentSystemInfo.PublishmentSystemType, AppManager.UserCenter.AppID) && publishmentSystemInfo.Additional.IsDefaultUserCenter)
                {
                    return publishmentSystemInfo;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取会员中心ID集合
        /// </summary>
        /// <returns></returns>
        public static ArrayList GetUserCenterIDArrayListOrderByLevel()
        {
            ArrayList retval = new ArrayList();

            ArrayList publishmentSystemIDArrayList = PublishmentSystemManager.GetPublishmentSystemIDArrayList();
            ArrayList publishmentSystemInfoArrayList = new ArrayList();
            Hashtable parentWithChildren = new Hashtable();
            int hqPublishmentSystemID = 0;
            foreach (int publishmentSystemID in publishmentSystemIDArrayList)
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                if (!EPublishmentSystemTypeUtils.Equals(publishmentSystemInfo.PublishmentSystemType, AppManager.UserCenter.AppID))
                    continue;
                if (publishmentSystemInfo.IsHeadquarters)
                {
                    hqPublishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
                }
                else
                {
                    if (publishmentSystemInfo.ParentPublishmentSystemID == 0)
                    {
                        publishmentSystemInfoArrayList.Add(publishmentSystemInfo);
                    }
                    else
                    {
                        ArrayList children = new ArrayList();
                        if (parentWithChildren.Contains(publishmentSystemInfo.ParentPublishmentSystemID))
                        {
                            children = (ArrayList)parentWithChildren[publishmentSystemInfo.ParentPublishmentSystemID];
                        }
                        children.Add(publishmentSystemInfo);
                        parentWithChildren[publishmentSystemInfo.ParentPublishmentSystemID] = children;
                    }
                }
            }

            if (hqPublishmentSystemID > 0)
            {
                retval.Add(hqPublishmentSystemID);
            }
            foreach (PublishmentSystemInfo publishmentSystemInfo in publishmentSystemInfoArrayList)
            {
                AddPublishmentSystemIDArrayList(retval, publishmentSystemInfo, parentWithChildren, 0);
            }
            return retval;
        }

        /// <summary>
        /// 获取应用ID集合，排除会员中心
        /// </summary>
        /// <returns></returns>
        public static ArrayList GetPublishmentSystemIDWithoutUserCenterArrayListOrderByLevel()
        {
            ArrayList retval = new ArrayList();

            ArrayList publishmentSystemIDArrayList = PublishmentSystemManager.GetPublishmentSystemIDArrayList();
            ArrayList publishmentSystemInfoArrayList = new ArrayList();
            Hashtable parentWithChildren = new Hashtable();
            int hqPublishmentSystemID = 0;
            foreach (int publishmentSystemID in publishmentSystemIDArrayList)
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                if (EPublishmentSystemTypeUtils.Equals(publishmentSystemInfo.PublishmentSystemType, AppManager.UserCenter.AppID))
                    continue;
                if (publishmentSystemInfo.IsHeadquarters)
                {
                    hqPublishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
                }
                else
                {
                    if (publishmentSystemInfo.ParentPublishmentSystemID == 0)
                    {
                        publishmentSystemInfoArrayList.Add(publishmentSystemInfo);
                    }
                    else
                    {
                        ArrayList children = new ArrayList();
                        if (parentWithChildren.Contains(publishmentSystemInfo.ParentPublishmentSystemID))
                        {
                            children = (ArrayList)parentWithChildren[publishmentSystemInfo.ParentPublishmentSystemID];
                        }
                        children.Add(publishmentSystemInfo);
                        parentWithChildren[publishmentSystemInfo.ParentPublishmentSystemID] = children;
                    }
                }
            }

            if (hqPublishmentSystemID > 0)
            {
                retval.Add(hqPublishmentSystemID);
            }
            foreach (PublishmentSystemInfo publishmentSystemInfo in publishmentSystemInfoArrayList)
            {
                AddPublishmentSystemIDArrayList(retval, publishmentSystemInfo, parentWithChildren, 0);
            }
            return retval;
        }

        /// <summary>
        /// 应用集合，排除会员中心
        /// </summary>
        /// <param name="publishmentSystemIDList"></param>
        /// <returns></returns>
        public static List<int> RemoveUserCenter(List<int> publishmentSystemIDList)
        {
            List<int> result = new List<int>();
            PublishmentSystemInfo publishmentSystemInfo;
            for (int i = 0; i < publishmentSystemIDList.Count; i++)
            {
                publishmentSystemInfo = GetPublishmentSystemInfo(publishmentSystemIDList[i]);
                if (!EPublishmentSystemTypeUtils.Equals(publishmentSystemInfo.PublishmentSystemType, AppManager.UserCenter.AppID))
                {
                    result.Add(publishmentSystemIDList[i]);
                }
            }
            return result;
        }

        /// <summary>
        /// 应用集合，排除会员中心
        /// </summary>
        /// <param name="publishmentSystemIDList"></param>
        /// <returns></returns>
        public static List<int> RemoveMLib(List<int> publishmentSystemIDList)
        {
            List<int> result = new List<int>();
            PublishmentSystemInfo publishmentSystemInfo;
            for (int i = 0; i < publishmentSystemIDList.Count; i++)
            {
                publishmentSystemInfo = GetPublishmentSystemInfo(publishmentSystemIDList[i]);
                if (!EPublishmentSystemTypeUtils.Equals(publishmentSystemInfo.PublishmentSystemType, AppManager.MLib.AppID))
                {
                    result.Add(publishmentSystemIDList[i]);
                }
            }
            return result;
        }

        /// <summary>
        /// 应用集合，排除会员中心
        /// </summary>
        /// <param name="publishmentSystemIDList"></param>
        /// <returns></returns>
        public static ArrayList RemoveUserCenter(ArrayList publishmentSystemIDArrayList)
        {
            ArrayList result = new ArrayList();
            PublishmentSystemInfo publishmentSystemInfo;
            for (int i = 0; i < publishmentSystemIDArrayList.Count; i++)
            {
                publishmentSystemInfo = GetPublishmentSystemInfo(TranslateUtils.ToInt(publishmentSystemIDArrayList[i].ToString()));
                if (!EPublishmentSystemTypeUtils.Equals(publishmentSystemInfo.PublishmentSystemType, AppManager.UserCenter.AppID))
                {
                    result.Add(publishmentSystemIDArrayList[i]);
                }
            }
            return result;
        }
        #endregion


        #region 投稿管理


        public static ArrayList GetPublishmentSystem(string userName)
        {
            string[] roles = RoleManager.GetRolesForUser(userName);


            ArrayList allPublishmentSystemIDArrayList = new ArrayList();
            ArrayList allPublishmentSystemArrayList = new ArrayList();

            if (AdminManager.HasChannelPermissionIsConsoleAdministrator(userName) || AdminManager.HasChannelPermissionIsSystemAdministrator(userName))//如果是超级管理员或站点管理员
            {
                ProductAdministratorWithPermissions ps = new ProductAdministratorWithPermissions(userName, true);
                foreach (int itemForPSID in ps.WebsitePermissionSortedList.Keys)
                {
                    if (!allPublishmentSystemIDArrayList.Contains(itemForPSID))
                    {
                        PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(itemForPSID);
                        if ((EPublishmentSystemTypeUtils.Equals(EPublishmentSystemType.CMS, publishmentSystemInfo.PublishmentSystemType) || EPublishmentSystemTypeUtils.Equals(EPublishmentSystemType.WCM, publishmentSystemInfo.PublishmentSystemType)))
                        {
                            allPublishmentSystemArrayList.Add(publishmentSystemInfo);
                            allPublishmentSystemIDArrayList.Add(itemForPSID);
                        }
                    }
                }
            }
            else
            {
                ProductAdministratorWithPermissions ps = new ProductAdministratorWithPermissions(userName, true);
                foreach (int itemForPSID in ps.WebsitePermissionSortedList.Keys)
                {
                    if (!allPublishmentSystemIDArrayList.Contains(itemForPSID))
                    {
                        ArrayList nodeIDCollection = DataProvider.SystemPermissionsDAO.GetAllPermissionArrayList(roles, itemForPSID, true);
                        PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(itemForPSID);
                        if ((EPublishmentSystemTypeUtils.Equals(EPublishmentSystemType.CMS, publishmentSystemInfo.PublishmentSystemType) || EPublishmentSystemTypeUtils.Equals(EPublishmentSystemType.WCM, publishmentSystemInfo.PublishmentSystemType)) && nodeIDCollection.Count > 0)
                        {
                            allPublishmentSystemArrayList.Add(publishmentSystemInfo);
                            allPublishmentSystemIDArrayList.Add(itemForPSID);
                        }
                    }
                }
            }
            return allPublishmentSystemArrayList;
        }


        public static ArrayList GetPublishmentSystem(ArrayList userNames)
        {
            ArrayList allPublishmentSystemIDArrayList = new ArrayList();
            ArrayList allPublishmentSystemArrayList = new ArrayList();
            if (AdminManager.HasChannelPermissionIsConsoleAdministrator(userNames[0].ToString()))//如果是超级管理员
            {
                ProductAdministratorWithPermissions ps = new ProductAdministratorWithPermissions(userNames[0].ToString(), true);

                foreach (int itemForPSID in ps.WebsitePermissionSortedList.Keys)
                {
                    if (!allPublishmentSystemIDArrayList.Contains(itemForPSID))
                    {
                        PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(itemForPSID);
                        if ((EPublishmentSystemTypeUtils.Equals(EPublishmentSystemType.CMS, publishmentSystemInfo.PublishmentSystemType) || EPublishmentSystemTypeUtils.Equals(EPublishmentSystemType.WCM, publishmentSystemInfo.PublishmentSystemType)))
                        {
                            allPublishmentSystemArrayList.Add(publishmentSystemInfo);
                            allPublishmentSystemIDArrayList.Add(itemForPSID);
                        }
                    }
                }
            }
            else
            {
                foreach (string userName in userNames)
                {
                    if (AdminManager.HasChannelPermissionIsSystemAdministrator(userName))//如果是站点管理员
                    {
                        AdministratorInfo ainfo = AdminManager.GetAdminInfo(userName);
                        ArrayList pids = TranslateUtils.StringCollectionToArrayList(ainfo.PublishmentSystemIDCollection);
                        foreach (int itemForPSID in pids)
                        {
                            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(itemForPSID);
                            if ((EPublishmentSystemTypeUtils.Equals(EPublishmentSystemType.CMS, publishmentSystemInfo.PublishmentSystemType) || EPublishmentSystemTypeUtils.Equals(EPublishmentSystemType.WCM, publishmentSystemInfo.PublishmentSystemType)))
                            {
                                allPublishmentSystemArrayList.Add(publishmentSystemInfo);
                                allPublishmentSystemIDArrayList.Add(itemForPSID);
                            }
                        }
                    }
                    else
                    {
                        string[] roles = RoleManager.GetRolesForUser(userName);

                        ProductAdministratorWithPermissions ps = new ProductAdministratorWithPermissions(userName, true);

                        foreach (int itemForPSID in ps.WebsitePermissionSortedList.Keys)
                        {
                            if (!allPublishmentSystemIDArrayList.Contains(itemForPSID))
                            {
                                ArrayList nodeIDCollection = DataProvider.SystemPermissionsDAO.GetAllPermissionArrayList(roles, itemForPSID, true);
                                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(itemForPSID);
                                if ((EPublishmentSystemTypeUtils.Equals(EPublishmentSystemType.CMS, publishmentSystemInfo.PublishmentSystemType) || EPublishmentSystemTypeUtils.Equals(EPublishmentSystemType.WCM, publishmentSystemInfo.PublishmentSystemType)) && nodeIDCollection.Count > 0)
                                {
                                    allPublishmentSystemArrayList.Add(publishmentSystemInfo);
                                    allPublishmentSystemIDArrayList.Add(itemForPSID);
                                }
                            }
                        }
                    }
                }
            }
            return allPublishmentSystemArrayList;
        }


        public static ArrayList GetNode(string userName, int publishmentSystemID)
        {
            ArrayList allNodeIDArrayList = new ArrayList();
            if (AdminManager.HasChannelPermissionIsConsoleAdministrator(userName) || AdminManager.HasChannelPermissionIsSystemAdministrator(userName))//如果是超级管理员或站点管理员
            {
                ArrayList nodeList = DataProvider.NodeDAO.GetNodeInfoArrayListByPublishmentSystemID(publishmentSystemID, string.Empty);
                foreach (NodeInfo nodeInfo in nodeList)
                {
                    if (nodeInfo != null && EContentModelTypeUtils.Equals(nodeInfo.ContentModelID, EContentModelType.Content))
                        allNodeIDArrayList.Add(nodeInfo);
                }
            }
            else
            {
                ProductAdministratorWithPermissions ps = new ProductAdministratorWithPermissions(userName, true);
                ICollection nodeIDCollection = ps.ChannelPermissionSortedList.Keys;
                foreach (int nodeID in nodeIDCollection)
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
                    if (nodeInfo != null && EContentModelTypeUtils.Equals(nodeInfo.ContentModelID, EContentModelType.Content))
                        allNodeIDArrayList.Add(nodeInfo);
                }
            }
            return allNodeIDArrayList;
        }


        public static ArrayList GetNode(int publishmentSystemID)
        {
            ArrayList allNodeIDArrayList = new ArrayList();

            ArrayList adminList = TranslateUtils.StringCollectionToArrayList(UserManager.CurrentNewGroupMLibAddUser);
            if (AdminManager.HasChannelPermissionIsConsoleAdministrator(adminList[0].ToString()))//如果是超级管理员或站点管理员
            {
                ArrayList nodeList = DataProvider.NodeDAO.GetNodeInfoArrayListByPublishmentSystemID(publishmentSystemID, string.Empty);
                foreach (NodeInfo nodeInfo in nodeList)
                {
                    if (nodeInfo != null && EContentModelTypeUtils.Equals(nodeInfo.ContentModelID, EContentModelType.Content))
                        allNodeIDArrayList.Add(nodeInfo);
                }
            }
            else
            {
                //ProductAdministratorWithPermissions ps = new ProductAdministratorWithPermissions(userName, true);
                //ICollection nodeIDCollection = ps.ChannelPermissionSortedList.Keys;
                //foreach (int nodeID in nodeIDCollection)
                //{
                //    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
                //    if (nodeInfo != null && EContentModelTypeUtils.Equals(nodeInfo.ContentModelID, EContentModelType.Content))
                //        allNodeIDArrayList.Add(nodeInfo);
                //}
            }
            return allNodeIDArrayList;
        }

        #endregion

    }
}
