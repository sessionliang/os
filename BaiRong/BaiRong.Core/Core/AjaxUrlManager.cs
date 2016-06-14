using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.DirectoryServices;
using System.Text.RegularExpressions;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using System.Collections.Generic;

namespace BaiRong.Core
{
    public class AjaxUrlManager
    {
        public AjaxUrlManager() { }

        public static void AddAjaxUrl(string ajaxUrl, string parameters)
        {
            AjaxUrlInfo ajaxUrlInfo = new AjaxUrlInfo(StringUtils.GUID(), ajaxUrl, parameters);
            BaiRongDataProvider.AjaxUrlDAO.Insert(ajaxUrlInfo);
            OpenAjaxChange();
        }

        //public static bool AddCreateUrl(int publishmentSystemID, int nodeID, int contentID, int templateID, int createTaskID, out int existsID)
        //{
        //    AjaxUrlInfo ajaxUrlInfo = new AjaxUrlInfo(StringUtils.GUID(), string.Empty, string.Empty, publishmentSystemID, contentID, nodeID, templateID, createTaskID);
        //    return BaiRongDataProvider.AjaxUrlDAO.InsertForCreate(ajaxUrlInfo, out existsID);
        //}

        public static void AddQueueCreateUrl(int publishmentSystemID, int nodeID, int contentID, int templateID, int createTaskID)
        {
            AjaxUrlInfo ajaxUrlInfo = new AjaxUrlInfo(StringUtils.GUID(), string.Empty, string.Empty, publishmentSystemID, contentID, nodeID, templateID, createTaskID);
            ajaxUrlInfo.ActionType = AjaxUrlInfo.ACTION_QUEUE_CREATE;
            BaiRongDataProvider.AjaxUrlDAO.Insert(ajaxUrlInfo);
        }

        public static void AddAjaxUrl(string ajaxUrl)
        {
            //判断是否启用了异步生成
            if (ConfigManager.Instance.Additional.IsUseAjaxCreatePage)
                AddAjaxUrl(ajaxUrl, string.Empty);
        }

        public static AjaxUrlInfo GetAjaxUrlInfo(string sn)
        {
            DictionaryEntryArrayList dictionaryEntryArrayList = GetAjaxUrlInfoDictionaryEntryArrayList();

            foreach (DictionaryEntry entry in dictionaryEntryArrayList)
            {
                string theSN = (string)entry.Key;
                if (theSN == sn)
                {
                    AjaxUrlInfo ajaxUrlInfo = entry.Value as AjaxUrlInfo;
                    return ajaxUrlInfo;
                }
            }
            return null;
        }


        public static AjaxUrlInfo FetchQueueCreateUrlInfo()
        {
            DictionaryEntryArrayList dictionary = GetAjaxUrlInfoDictionaryEntryArrayListForQueueCreateTask();
            if (dictionary.Count > 0)
            {
                DictionaryEntry entry = (DictionaryEntry)dictionary[0];
                AjaxUrlInfo ajaxUrlInfo = entry.Value as AjaxUrlInfo;
                if (ajaxUrlInfo != null && string.IsNullOrEmpty(ajaxUrlInfo.AjaxUrl))
                {
                    BaiRongDataProvider.AjaxUrlDAO.DeleteSameQueue(ajaxUrlInfo);
                    dictionary.Remove(entry);
                    return ajaxUrlInfo;
                }
            }
            return null;
        }


        public static AjaxUrlInfo FetchAjaxUrlInfo()
        {
            DictionaryEntryArrayList dictionary = GetAjaxUrlInfoDictionaryEntryArrayList();
            if (dictionary != null && dictionary.Count > 0)
            {
                DictionaryEntry entry = (DictionaryEntry)dictionary[0];
                AjaxUrlInfo ajaxUrlInfo = entry.Value as AjaxUrlInfo;
                if (ajaxUrlInfo != null && !string.IsNullOrEmpty(ajaxUrlInfo.AjaxUrl))
                {
                    BaiRongDataProvider.AjaxUrlDAO.Delete(ajaxUrlInfo.SN);
                    dictionary.Remove(entry);
                    return ajaxUrlInfo;
                }
            }
            return null;
        }

        public static DictionaryEntryArrayList GetAjaxUrlInfoDictionaryEntryArrayList()
        {
            lock (lockObject)
            {
                if (CacheUtils.Get(cacheKey) == null || IsChage(cacheKey))
                {
                    DictionaryEntryArrayList sl = BaiRongDataProvider.AjaxUrlDAO.GetAjaxUrlInfoDictionaryEntryArrayList();
                    CloseChange(cacheKey);
                    CacheUtils.Max(cacheKey, sl);
                    return sl;
                }
                return CacheUtils.Get(cacheKey) as DictionaryEntryArrayList;
            }
        }

        public static DictionaryEntryArrayList GetAjaxUrlInfoDictionaryEntryArrayListForQueueCreateTask()
        {
            lock (lockObject)
            {
                if (CacheUtils.Get(queueCreateCacheKey) == null || IsChage(queueCreateCacheKey))
                {
                    DictionaryEntryArrayList sl = BaiRongDataProvider.AjaxUrlDAO.GetAjaxUrlInfoDictionaryEntryArrayListForQueueCreate();
                    CloseChange(queueCreateCacheKey);
                    CacheUtils.Max(queueCreateCacheKey, sl);
                    return sl;
                }
                return CacheUtils.Get(queueCreateCacheKey) as DictionaryEntryArrayList;
            }
        }

        public static void OpenCreateChange()
        {
            OpenChange(createCacheKey);
        }

        public static void OpenQueueCreateChange()
        {
            OpenChange(queueCreateCacheKey);
        }

        public static void OpenAjaxChange()
        {
            OpenChange(cacheKey);
        }

        private static bool IsChage(string cache)
        {
            if (StringUtils.Equals(cache, cacheKey))
                return TranslateUtils.ToBool(DbCacheManager.Get(isChangeKey));
            else if (StringUtils.Equals(cache, queueCreateCacheKey))
                return TranslateUtils.ToBool(DbCacheManager.Get(isQueueChangeCreateKey));
            else if (StringUtils.Equals(cache, createCacheKey))
                return TranslateUtils.ToBool(DbCacheManager.Get(isChangeCreateKey));
            else
                return false;
        }

        private static void OpenChange(string cache)
        {
            if (StringUtils.Equals(cache, cacheKey))
            {
                DbCacheManager.Remove(isChangeKey);
                DbCacheManager.Insert(isChangeKey, true.ToString());
            }
            else if (StringUtils.Equals(cache, queueCreateCacheKey))
            {
                DbCacheManager.Remove(isQueueChangeCreateKey);
                DbCacheManager.Insert(isQueueChangeCreateKey, true.ToString());
            }
            else if (StringUtils.Equals(cache, createCacheKey))
            {
                DbCacheManager.Remove(isChangeCreateKey);
                DbCacheManager.Insert(isChangeCreateKey, true.ToString());
            }
        }

        private static void CloseChange(string cache)
        {
            if (StringUtils.Equals(cache, cacheKey))
            {
                DbCacheManager.Remove(isChangeKey);
                DbCacheManager.Insert(isChangeKey, false.ToString());
            }
            else if (StringUtils.Equals(cache, queueCreateCacheKey))
            {
                DbCacheManager.Remove(isQueueChangeCreateKey);
                DbCacheManager.Insert(isQueueChangeCreateKey, false.ToString());
            }
            else if (StringUtils.Equals(cache, createCacheKey))
            {
                DbCacheManager.Remove(isChangeCreateKey);
                DbCacheManager.Insert(isChangeCreateKey, false.ToString());
            }
        }


        #region TouGao And Background Pages Auto Create Html Page
        /// <summary>
        /// 添加生成任务
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="nodeID"></param>
        /// <param name="contentID"></param>
        /// <param name="templateID"></param>
        /// <param name="createTaskID"></param>
        public static void AddCreateUrl(int publishmentSystemID, int nodeID, int contentID, int templateID)
        {
            AjaxUrlInfo ajaxUrlInfo = new AjaxUrlInfo(StringUtils.GUID(), string.Empty, string.Empty, publishmentSystemID, contentID, nodeID, templateID, 0);
            ajaxUrlInfo.ActionType = AjaxUrlInfo.ACTION_BACKGROUND_CREATE;
            BaiRongDataProvider.AjaxUrlDAO.Insert(ajaxUrlInfo);
        }
        /// <summary>
        /// 获取一条任务
        /// </summary>
        /// <returns></returns>
        public static AjaxUrlInfo FetchCreateUrlInfo()
        {
            DictionaryEntryArrayList dictionary = GetAjaxUrlInfoDictionaryEntryArrayListForCreateTask();
            if (dictionary.Count > 0)
            {
                DictionaryEntry entry = (DictionaryEntry)dictionary[0];
                AjaxUrlInfo ajaxUrlInfo = entry.Value as AjaxUrlInfo;
                if (ajaxUrlInfo != null && string.IsNullOrEmpty(ajaxUrlInfo.AjaxUrl))
                {
                    BaiRongDataProvider.AjaxUrlDAO.Delete(ajaxUrlInfo.SN);
                    dictionary.Remove(entry);
                    return ajaxUrlInfo;
                }
            }
            return null;
        }
        public static DictionaryEntryArrayList GetAjaxUrlInfoDictionaryEntryArrayListForCreateTask()
        {
            lock (lockObject)
            {
                if (CacheUtils.Get(createCacheKey) == null || IsChage(createCacheKey))
                {
                    DictionaryEntryArrayList sl = BaiRongDataProvider.AjaxUrlDAO.GetAjaxUrlInfoDictionaryEntryArrayListForCreate();
                    CloseChange(createCacheKey);
                    CacheUtils.Max(createCacheKey, sl);
                    return sl;
                }
                return CacheUtils.Get(createCacheKey) as DictionaryEntryArrayList;
            }
        }


        private const string createCacheKey = "BaiRong.Core.CreateAjaxUrlManager";
        private const string isChangeCreateKey = "BaiRong.Core.AjaxUrlManager.IsChangeCreate";
        #endregion


        /****************** Cache *********************/

        private static readonly object lockObject = new object();
        private const string cacheKey = "BaiRong.Core.AjaxUrlManager";
        private const string isChangeKey = "BaiRong.Core.AjaxUrlManager.IsChange";

        private const string queueCreateCacheKey = "BaiRong.Core.QueueCreateAjaxUrlManager";
        private const string isQueueChangeCreateKey = "BaiRong.Core.AjaxUrlManager.IsQueueChangeCreate";
    }
}