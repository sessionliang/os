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
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;
using BaiRong.Model.Service;
using BaiRong.Core.Service;
using SiteServer.STL.IO;

namespace SiteServer.STL.BackgroundPages.Service
{
    public class PublishService : Page
    {
        public void Page_Load(object sender, System.EventArgs e)
        {
            string type = base.Request.QueryString["type"];
            string userKeyPrefix = base.Request["userKeyPrefix"];
            NameValueCollection retval = new NameValueCollection();

            if (type == "GetCountArray")
            {
                retval = GetCountArray(userKeyPrefix);
            }
            else if (type == "PublishChannelsOneByOne")
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request.Form["publishmentSystemID"]);
                bool isCreate = TranslateUtils.ToBool(base.Request.Form["isCreate"]);
                bool isIncludeChildren = TranslateUtils.ToBool(base.Request.Form["isIncludeChildren"]);
                bool isIncludeContents = TranslateUtils.ToBool(base.Request.Form["isIncludeContents"]);
                retval = PublishChannelsOneByOne(publishmentSystemID, isCreate, isIncludeChildren, isIncludeContents, userKeyPrefix);
            }
            else if (type == "PublishContentsOneByOne")
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request.Form["publishmentSystemID"]);
                int nodeID = TranslateUtils.ToInt(base.Request.Form["nodeID"]);
                bool isCreate = TranslateUtils.ToBool(base.Request.Form["isCreate"]);
                retval = PublishContentsOneByOne(publishmentSystemID, nodeID, isCreate, userKeyPrefix);
            }

            string jsonString = TranslateUtils.NameValueCollectionToJsonString(retval);
            Page.Response.Write(jsonString);
            Page.Response.End();
        }

        public NameValueCollection GetCountArray(string userKeyPrefix)//进度及显示
        {
            NameValueCollection retval = new NameValueCollection();
            if (CacheUtils.Get(userKeyPrefix + CACHE_TOTAL_COUNT) != null && CacheUtils.Get(userKeyPrefix + CACHE_CURRENT_COUNT) != null && CacheUtils.Get(userKeyPrefix + CACHE_MESSAGE) != null)
            {
                int totalCount = TranslateUtils.ToInt((string)CacheUtils.Get(userKeyPrefix + CACHE_TOTAL_COUNT));
                int currentCount = TranslateUtils.ToInt((string)CacheUtils.Get(userKeyPrefix + CACHE_CURRENT_COUNT));
                string message = (string)CacheUtils.Get(userKeyPrefix + CACHE_MESSAGE);
                retval = JsManager.AjaxService.GetCountArrayNameValueCollection(totalCount, currentCount, message);
            }
            return retval;
        }

        public const string CACHE_TOTAL_COUNT = "_TotalCount";
        public const string CACHE_CURRENT_COUNT = "_CurrentCount";
        public const string CACHE_MESSAGE = "_Message";

        #region 发布页面

        public NameValueCollection PublishChannelsOneByOne(int publishmentSystemID, bool isCreate, bool isIncludeChildren, bool isIncludeContents, string userKeyPrefix)
        {
            string cacheTotalCountKey = userKeyPrefix + CACHE_TOTAL_COUNT;
            string cacheCurrentCountKey = userKeyPrefix + CACHE_CURRENT_COUNT;
            string cacheMessageKey = userKeyPrefix + CACHE_MESSAGE;

            CacheUtils.Max(cacheTotalCountKey, "0");//存储需要的页面总数
            CacheUtils.Max(cacheCurrentCountKey, "0");//存储当前的页面总数
            CacheUtils.Max(cacheMessageKey, string.Empty);//存储消息

            //返回“运行结果”和“错误信息”的字符串数组
            NameValueCollection retval;

            ArrayList nodeIDArrayList = TranslateUtils.StringCollectionToIntArrayList(DbCacheManager.GetAndRemove(userKeyPrefix + Constants.CACHE_PUBLISH_CHANNELS_NODE_ID_ARRAYLIST));

            try
            {
                StringBuilder resultBuilder = new StringBuilder();
                StringBuilder errorBuilder = new StringBuilder();

                int publishCount = 0;
                int totalCount = 0;
                int currentCount = 0;

                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

                Hashtable nodeIDWithContentIDArrayListMap = new Hashtable();

                if (isIncludeChildren)
                {
                    ArrayList theNodeIDArrayList = new ArrayList();
                    foreach (int nodeID in nodeIDArrayList)
                    {
                        if (!theNodeIDArrayList.Contains(nodeID))
                        {
                            theNodeIDArrayList.Add(nodeID);
                        }
                        ArrayList children = DataProvider.NodeDAO.GetNodeIDArrayListForDescendant(nodeID);
                        if (children != null && children.Count > 0)
                        {
                            foreach (int childrenNodeID in children)
                            {
                                if (!theNodeIDArrayList.Contains(childrenNodeID))
                                {
                                    theNodeIDArrayList.Add(childrenNodeID);
                                }
                            }
                        }
                    }
                    nodeIDArrayList = theNodeIDArrayList;
                }

                publishCount = nodeIDArrayList.Count;

                if (isIncludeContents)
                {
                    foreach (int nodeID in nodeIDArrayList)
                    {
                        string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeID);
                        string orderByString = ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc);
                        ArrayList contentIDArrayList = DataProvider.ContentDAO.GetContentIDArrayListChecked(tableName, nodeID, orderByString);
                        nodeIDWithContentIDArrayListMap.Add(nodeID, contentIDArrayList);
                        publishCount += contentIDArrayList.Count;
                    }
                }

                if (isCreate)
                {
                    totalCount = publishCount;
                }

                totalCount += publishCount;

                CacheUtils.Max(cacheTotalCountKey, totalCount.ToString());//存储需要的页面总数
                CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数

                if (isCreate)
                {
                    FileSystemObject FSO = new FileSystemObject(publishmentSystemID);
                    foreach (int nodeIDToCreate in nodeIDArrayList)
                    {
                        try
                        {
                            if (publishmentSystemInfo.Additional.IsCreateRedirectPage)
                            {
                                FSO.AddChannelToWaitingCreate(nodeIDToCreate);
                            }
                            else
                            {
                                FSO.CreateChannel(nodeIDToCreate);
                            }
                            currentCount++;
                            CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                        }
                        catch (Exception ex)
                        {
                            errorBuilder.Append("<br />栏目“").Append(NodeManager.GetNodeName(publishmentSystemID, nodeIDToCreate)).Append("”生成失败！");
                            errorBuilder.Append("<br />").Append(ex.Message);
                            LogUtils.AddErrorLog(ex);
                        }
                        if (isIncludeContents)
                        {
                            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeIDToCreate);
                            ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                            string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);

                            ArrayList contentIDArrayList = nodeIDWithContentIDArrayListMap[nodeIDToCreate] as ArrayList;
                            if (contentIDArrayList != null && contentIDArrayList.Count > 0)
                            {
                                foreach (int contentID in contentIDArrayList)
                                {
                                    try
                                    {
                                        if (publishmentSystemInfo.Additional.IsCreateRedirectPage)
                                        {
                                            FSO.AddContentToWaitingCreate(nodeIDToCreate, contentID);
                                        }
                                        else
                                        {
                                            FSO.CreateContent(tableStyle, tableName, nodeIDToCreate, contentID);
                                        }
                                        currentCount++;
                                        CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                                    }
                                    catch (Exception ex)
                                    {
                                        errorBuilder.Append("<br />内容“").Append(BaiRongDataProvider.ContentDAO.GetValue(tableName, contentID, ContentAttribute.Title)).Append("”生成失败！");
                                        errorBuilder.Append("<br />").Append(ex.Message);
                                        LogUtils.AddErrorLog(ex);
                                    }
                                }
                            }
                        }
                    }
                }

                if (publishmentSystemInfo.Additional.IsSiteStorage)
                {
                    if (publishmentSystemInfo.Additional.SiteStorageID > 0)
                    {
                        StorageInfo storageInfo = BaiRongDataProvider.StorageDAO.GetStorageInfo(publishmentSystemInfo.Additional.SiteStorageID);
                        if (storageInfo != null)
                        {
                            StorageManager storageManager = new StorageManager(storageInfo, publishmentSystemInfo.Additional.SiteStoragePath);

                            if (storageManager.IsEnabled)
                            {
                                string localDirectoryPath = PathUtility.GetPublishmentSystemPath(publishmentSystemInfo);

                                foreach (int nodeIDToCreate in nodeIDArrayList)
                                {
                                    try
                                    {
                                        string channelFilePath = PathUtility.GetChannelPageFilePath(publishmentSystemInfo, nodeIDToCreate, 0);
                                        string directoryDifference = PathUtils.GetDirectoryDifference(localDirectoryPath, channelFilePath);
                                        storageManager.Manager.ChangeDirDownByRelatedPath(directoryDifference);
                                        storageManager.Manager.UploadFile(channelFilePath);

                                        string channelFilePath2 = PathUtility.GetChannelPageFilePath(publishmentSystemInfo, nodeIDToCreate, 1);
                                        if (FileUtils.IsFileExists(channelFilePath2))
                                        {
                                            string fileNameWithoutExtension = PathUtils.GetFileNameWithoutExtension(channelFilePath);
                                            string extension = PathUtils.GetExtension(channelFilePath);
                                            string searchPattern = fileNameWithoutExtension + "_*" + extension;
                                            string[] filePaths = System.IO.Directory.GetFiles(DirectoryUtils.GetDirectoryPath(channelFilePath), searchPattern, System.IO.SearchOption.TopDirectoryOnly);
                                            if (filePaths != null && filePaths.Length > 0)
                                            {
                                                foreach (string filePath in filePaths)
                                                {
                                                    storageManager.Manager.UploadFile(filePath);
                                                }
                                            }
                                        }

                                        storageManager.Manager.ChangeDirUpByRelatedPath(directoryDifference);

                                        currentCount++;
                                        CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                                    }
                                    catch (Exception ex)
                                    {
                                        errorBuilder.Append("<br />栏目“").Append(NodeManager.GetNodeName(publishmentSystemID, nodeIDToCreate)).Append("”发布失败！");
                                        errorBuilder.Append("<br />").Append(ex.Message);
                                        LogUtils.AddErrorLog(ex);
                                    }
                                    if (isIncludeContents)
                                    {
                                        ArrayList contentIDArrayList = nodeIDWithContentIDArrayListMap[nodeIDToCreate] as ArrayList;
                                        if (contentIDArrayList != null && contentIDArrayList.Count > 0)
                                        {
                                            foreach (int contentID in contentIDArrayList)
                                            {
                                                try
                                                {
                                                    string contentFilePath = PathUtility.GetContentPageFilePath(publishmentSystemInfo, nodeIDToCreate, contentID, 0);
                                                    string directoryDifference = PathUtils.GetDirectoryDifference(localDirectoryPath, contentFilePath);
                                                    storageManager.Manager.ChangeDirDownByRelatedPath(directoryDifference);
                                                    storageManager.Manager.UploadFile(contentFilePath);

                                                    string contentFilePath2 = PathUtility.GetContentPageFilePath(publishmentSystemInfo, nodeIDToCreate, contentID, 1);
                                                    if (FileUtils.IsFileExists(contentFilePath2))
                                                    {
                                                        string fileNameWithoutExtension = PathUtils.GetFileNameWithoutExtension(contentFilePath);
                                                        string extension = PathUtils.GetExtension(contentFilePath);
                                                        string searchPattern = fileNameWithoutExtension + "_*" + extension;
                                                        string[] filePaths = System.IO.Directory.GetFiles(DirectoryUtils.GetDirectoryPath(contentFilePath), searchPattern, System.IO.SearchOption.TopDirectoryOnly);
                                                        if (filePaths != null && filePaths.Length > 0)
                                                        {
                                                            foreach (string filePath in filePaths)
                                                            {
                                                                storageManager.Manager.UploadFile(filePath);
                                                            }
                                                        }
                                                    }

                                                    storageManager.Manager.ChangeDirUpByRelatedPath(directoryDifference);
                                                    currentCount++;
                                                    CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                                                }
                                                catch (Exception ex)
                                                {
                                                    string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeIDToCreate);
                                                    errorBuilder.Append("<br />内容“").Append(BaiRongDataProvider.ContentDAO.GetValue(tableName, contentID, ContentAttribute.Title)).Append("”发布失败！");
                                                    errorBuilder.Append("<br />").Append(ex.Message);
                                                    LogUtils.AddErrorLog(ex);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //if (machineNameArrayList.Count > 0)
                //{
                //    foreach (string machineName in machineNameArrayList)
                //    {
                //        MachineInfo machineInfo = CacheMachine.Instance.GetMachineInfo(machineName);
                //        if (!MachineManager.IsValid(machineInfo, EServiceType.Publish))
                //        {
                //            continue;
                //        }
                //        MachineManager machineManager = new MachineManager(null, machineInfo);
                //        if (!machineManager.IsEnabled) continue;
                //        string localDirectoryPath;
                //        if (machineInfo.PublishmentSystemID == publishmentSystemID)
                //        {
                //            localDirectoryPath = PathUtility.GetPublishmentSystemPath(publishmentSystemInfo);
                //        }
                //        else
                //        {
                //            localDirectoryPath = ConfigUtils.Instance.PhysicalApplicationPath;
                //        }
                        
                //    }
                //}

                resultBuilder.AppendFormat("任务完成，共发布栏目/内容<strong> {0} </strong>页。", currentCount);
                retval = JsManager.AjaxService.GetProgressTaskNameValueCollection(resultBuilder.ToString(), errorBuilder.ToString());
            }
            catch (Exception ex)
            {
                retval = JsManager.AjaxService.GetProgressTaskNameValueCollection(string.Empty, ex.Message);
                LogUtils.AddErrorLog(ex);
            }

            CacheUtils.Remove(cacheTotalCountKey);//取消存储需要的页面总数
            CacheUtils.Remove(cacheCurrentCountKey);//取消存储当前的页面总数
            CacheUtils.Remove(cacheMessageKey);//取消存储消息

            return retval;
        }

        public NameValueCollection PublishContentsOneByOne(int publishmentSystemID, int nodeID, bool isCreate, string userKeyPrefix)
        {
            string cacheTotalCountKey = userKeyPrefix + CACHE_TOTAL_COUNT;
            string cacheCurrentCountKey = userKeyPrefix + CACHE_CURRENT_COUNT;
            string cacheMessageKey = userKeyPrefix + CACHE_MESSAGE;

            CacheUtils.Max(cacheTotalCountKey, "0");//存储需要的页面总数
            CacheUtils.Max(cacheCurrentCountKey, "0");//存储当前的页面总数
            CacheUtils.Max(cacheMessageKey, string.Empty);//存储消息

            //返回“运行结果”和“错误信息”的字符串数组
            NameValueCollection retval;

            ArrayList contentIDArrayList = TranslateUtils.StringCollectionToIntArrayList(DbCacheManager.GetAndRemove(userKeyPrefix + Constants.CACHE_PUBLISH_CONTENTS_CONTENT_ID_ARRAYLIST));

            StringBuilder resultBuilder = new StringBuilder();
            StringBuilder errorBuilder = new StringBuilder();

            int totalCount = 0;
            int currentCount = 0;

            try
            {
                if (contentIDArrayList != null && contentIDArrayList.Count > 0)
                {
                    if (isCreate)
                    {
                        totalCount = contentIDArrayList.Count;
                    }

                    totalCount += contentIDArrayList.Count;

                    CacheUtils.Max(cacheTotalCountKey, totalCount.ToString());//存储需要的页面总数
                    CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数

                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    if (isCreate)
                    {
                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
                        ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                        string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);

                        FileSystemObject FSO = new FileSystemObject(publishmentSystemID);
                        foreach (int contentID in contentIDArrayList)
                        {
                            try
                            {
                                if (publishmentSystemInfo.Additional.IsCreateRedirectPage)
                                {
                                    FSO.AddContentToWaitingCreate(nodeID, contentID);
                                }
                                else
                                {
                                    FSO.CreateContent(tableStyle, tableName, nodeID, contentID);
                                }
                                currentCount++;
                                CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                            }
                            catch (Exception ex)
                            {
                                errorBuilder.Append("<br />内容“").Append(BaiRongDataProvider.ContentDAO.GetValue(tableName, contentID, ContentAttribute.Title)).Append("”生成失败！");
                                errorBuilder.Append("<br />").Append(ex.Message);
                                LogUtils.AddErrorLog(ex);
                            }
                        }
                    }

                    if (publishmentSystemInfo.Additional.IsSiteStorage)
                    {
                        if (publishmentSystemInfo.Additional.SiteStorageID > 0)
                        {
                            StorageInfo storageInfo = BaiRongDataProvider.StorageDAO.GetStorageInfo(publishmentSystemInfo.Additional.SiteStorageID);
                            if (storageInfo != null)
                            {
                                StorageManager storageManager = new StorageManager(storageInfo, publishmentSystemInfo.Additional.SiteStoragePath);

                                if (storageManager.IsEnabled)
                                {
                                    string localDirectoryPath = PathUtility.GetPublishmentSystemPath(publishmentSystemInfo);

                                    foreach (int contentID in contentIDArrayList)
                                    {
                                        try
                                        {
                                            string contentFilePath = PathUtility.GetContentPageFilePath(publishmentSystemInfo, nodeID, contentID, 0);
                                            string directoryDifference = PathUtils.GetDirectoryDifference(localDirectoryPath, contentFilePath);
                                            storageManager.Manager.ChangeDirDownByRelatedPath(directoryDifference);
                                            storageManager.Manager.UploadFile(contentFilePath);

                                            string contentFilePath2 = PathUtility.GetContentPageFilePath(publishmentSystemInfo, nodeID, contentID, 1);
                                            if (FileUtils.IsFileExists(contentFilePath2))
                                            {
                                                string fileNameWithoutExtension = PathUtils.GetFileNameWithoutExtension(contentFilePath);
                                                string extension = PathUtils.GetExtension(contentFilePath);
                                                string searchPattern = fileNameWithoutExtension + "_*" + extension;
                                                string[] filePaths = System.IO.Directory.GetFiles(DirectoryUtils.GetDirectoryPath(contentFilePath), searchPattern, System.IO.SearchOption.TopDirectoryOnly);
                                                if (filePaths != null && filePaths.Length > 0)
                                                {
                                                    foreach (string filePath in filePaths)
                                                    {
                                                        storageManager.Manager.UploadFile(filePath);
                                                    }
                                                }
                                            }

                                            storageManager.Manager.ChangeDirUpByRelatedPath(directoryDifference);
                                            currentCount++;
                                            CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                                        }
                                        catch (Exception ex)
                                        {
                                            string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeID);
                                            errorBuilder.Append("<br />内容“").Append(BaiRongDataProvider.ContentDAO.GetValue(tableName, contentID, ContentAttribute.Title)).Append("”发布失败！");
                                            errorBuilder.Append("<br />").Append(ex.Message);
                                            LogUtils.AddErrorLog(ex);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //if (machineNameArrayList.Count > 0)
                    //{
                    //    foreach (string machineName in machineNameArrayList)
                    //    {
                    //        MachineInfo machineInfo = CacheMachine.Instance.GetMachineInfo(machineName);
                    //        if (!MachineManager.IsValid(machineInfo, EServiceType.Publish))
                    //        {
                    //            continue;
                    //        }
                    //        MachineManager machineManager = new MachineManager(null, machineInfo);
                    //        if (!machineManager.IsEnabled) continue;
                    //        string localDirectoryPath;
                    //        if (machineInfo.PublishmentSystemID == publishmentSystemID)
                    //        {
                    //            localDirectoryPath = PathUtility.GetPublishmentSystemPath(publishmentSystemInfo);
                    //        }
                    //        else
                    //        {
                    //            localDirectoryPath = ConfigUtils.Instance.PhysicalApplicationPath;
                    //        }
                            
                    //    }
                    //}
                }

                resultBuilder.AppendFormat("任务完成，共发布内容<strong> {0} </strong>页。", currentCount);
                retval = JsManager.AjaxService.GetProgressTaskNameValueCollection(resultBuilder.ToString(), errorBuilder.ToString());
            }
            catch (Exception ex)
            {
                retval = JsManager.AjaxService.GetProgressTaskNameValueCollection(string.Empty, ex.Message);

                LogUtils.AddErrorLog(ex);
            }

            CacheUtils.Remove(cacheTotalCountKey);//取消存储需要的页面总数
            CacheUtils.Remove(cacheCurrentCountKey);//取消存储当前的页面总数
            CacheUtils.Remove(cacheMessageKey);//取消存储消息

            return retval;
        }

        #endregion
    }
}
