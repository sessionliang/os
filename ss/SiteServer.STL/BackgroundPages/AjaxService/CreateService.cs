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
using SiteServer.STL.IO;
using SiteServer.STL.ImportExport;

namespace SiteServer.STL.BackgroundPages.Service
{
    public class CreateService : Page
    {
        public const string CACHE_TOTAL_COUNT = "_TotalCount";
        public const string CACHE_CURRENT_COUNT = "_CurrentCount";
        public const string CACHE_MESSAGE = "_Message";

        public const string CACHE_QUEUING_COUNT = "_QueuingCount";
        public const string CACHE_CREATE_TASK_ID = "_CreateTaskID";

        public void Page_Load(object sender, System.EventArgs e)
        {
            string type = base.Request.QueryString["type"];
            string userKeyPrefix = base.Request["userKeyPrefix"];
            NameValueCollection retval = new NameValueCollection();

            if (type == "GetCountArray")
            {
                retval = GetCountArray(userKeyPrefix);
            }
            else if (type == "GetCountArrayForService")
            {
                retval = GetCountArrayForService(userKeyPrefix);
            }
            else if (type == "CreateChannels")
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request.Form["publishmentSystemID"]);
                retval = CreateChannels(publishmentSystemID, userKeyPrefix);
            }
            else if (type == "CreateChannelsOneByOne")
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request.Form["publishmentSystemID"]);
                bool isIncludeChildren = TranslateUtils.ToBool(base.Request.Form["isIncludeChildren"]);
                bool isCreateContents = TranslateUtils.ToBool(base.Request.Form["isCreateContents"]);
                retval = CreateChannelsOneByOne(publishmentSystemID, userKeyPrefix, isIncludeChildren, isCreateContents);
            }
            else if (type == "CreateContents")
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request.Form["publishmentSystemID"]);
                retval = CreateContents(publishmentSystemID, userKeyPrefix);
            }
            else if (type == "CreateContentsByService")
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request.Form["publishmentSystemID"]);
                int createTaskID = TranslateUtils.ToInt(base.Request.Form["createTaskID"]);
                retval = CreateContentsByService(publishmentSystemID, userKeyPrefix, createTaskID);
            }
            else if (type == "CreateContentsOneByOne")
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request.Form["publishmentSystemID"]);
                int nodeID = TranslateUtils.ToInt(base.Request.Form["nodeID"]);
                retval = CreateContentsOneByOne(publishmentSystemID, nodeID, userKeyPrefix);
            }
            else if (type == "CreateByTemplate")
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request.Form["publishmentSystemID"]);
                int templateID = TranslateUtils.ToInt(base.Request.Form["templateID"]);
                retval = CreateByTemplate(publishmentSystemID, templateID, userKeyPrefix);
            }
            else if (type == "CreateByIDsCollection")
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request.Form["publishmentSystemID"]);
                retval = CreateByIDsCollection(publishmentSystemID, userKeyPrefix);
            }
            else if (type == "CreateFiles")
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request.Form["publishmentSystemID"]);
                retval = CreateFiles(publishmentSystemID, userKeyPrefix);
            }
            else if (type == "CreatePublishmentSystem")
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request.Form["publishmentSystemID"]);
                bool isUseSiteTemplate = TranslateUtils.ToBool(base.Request.Form["isUseSiteTemplate"]);
                bool isImportContents = TranslateUtils.ToBool(base.Request.Form["isImportContents"]);
                bool isImportTableStyles = TranslateUtils.ToBool(base.Request.Form["isImportTableStyles"]);
                string siteTemplateDir = base.Request.Form["siteTemplateDir"];
                bool isUseTables = TranslateUtils.ToBool(base.Request.Form["isUseTables"]);
                string returnUrl = base.Request.Form["returnUrl"];
                bool isTop = TranslateUtils.ToBool(base.Request.Form["isTop"], false);
                bool isCreateAll = TranslateUtils.ToBool(base.Request.Form["isCreateAll"], false);
                retval = CreatePublishmentSystem(publishmentSystemID, isUseSiteTemplate, isImportContents, isImportTableStyles, siteTemplateDir, isUseTables, userKeyPrefix, returnUrl, isTop, isCreateAll);
            }
            else if (type == "CreateAll")
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request.Form["publishmentSystemID"]);
                retval = CreateAll(publishmentSystemID, userKeyPrefix);
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

        public NameValueCollection GetCountArrayForService(string userKeyPrefix)//进度及显示
        {
            NameValueCollection retval = new NameValueCollection();
            if (CacheUtils.Get(userKeyPrefix + CACHE_CREATE_TASK_ID) != null)
            {
                int createTaskID = TranslateUtils.ToInt(CacheUtils.Get(userKeyPrefix + CACHE_CREATE_TASK_ID).ToString());
                int queuingCount = 0;//排队数量
                int totalCount = 0;//生成总数
                int currentCount = 0;//已经处理数量
                int errorCount = 0;//错误数量
                BaiRongDataProvider.CreateTaskDAO.UpdateState();
                BaiRongDataProvider.CreateTaskDAO.GetQueuingTaskCount(createTaskID, out totalCount, out currentCount, out errorCount, out queuingCount);
                if (queuingCount > 0)
                {
                    //排队中
                    retval = JsManager.AjaxService.GetCountArrayNameValueCollection(totalCount, 0, errorCount, queuingCount, string.Format("队列中现在还有<strong> {0} </strong>任务等待处理，请耐性等待。您现在也可以离开处理其他工作。<a href='{1}'>查看进度</a>", queuingCount, BaiRong.BackgroundPages.BackgroundCreateTaskState.GetRedirectString(createTaskID)));
                }
                else if (currentCount < totalCount)
                {
                    //处理中
                    retval = JsManager.AjaxService.GetCountArrayNameValueCollection(totalCount, currentCount, errorCount, queuingCount, string.Format("任务正在进行。您现在也可以离开处理其他工作。<a href='{0}'>查看进度</a>", BaiRong.BackgroundPages.BackgroundCreateTaskState.GetRedirectString(createTaskID)));
                    BaiRongDataProvider.CreateTaskDAO.UpdateState(createTaskID, ECreateTaskType.Processing);
                }
                else
                {
                    //已完成
                    retval = JsManager.AjaxService.GetProgressTaskNameValueCollection(string.Format("任务已经完成。"), string.Empty);
                    BaiRongDataProvider.CreateTaskDAO.UpdateState(createTaskID, ECreateTaskType.Completed);
                    CacheUtils.Remove(userKeyPrefix + CACHE_TOTAL_COUNT);//删除需要的页面总数
                    CacheUtils.Remove(userKeyPrefix + CACHE_QUEUING_COUNT);//删除排队的页面总数
                    CacheUtils.Remove(userKeyPrefix + CACHE_CREATE_TASK_ID);//删除任务ID
                }
            }
            return retval;
        }

        #region 生成页面

        /// <summary>
        /// 生成全部（首页，栏目，内容，单页）
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="userKeyPrefix"></param>
        /// <returns></returns>
        public NameValueCollection CreateAll(int publishmentSystemID, string userKeyPrefix)
        {
            StringBuilder resultBuilder = new StringBuilder();
            StringBuilder errorBuilder = new StringBuilder();

            string cacheTotalCountKey = userKeyPrefix + CACHE_TOTAL_COUNT;
            string cacheCurrentCountKey = userKeyPrefix + CACHE_CURRENT_COUNT;
            string cacheMessageKey = userKeyPrefix + CACHE_MESSAGE;

            CacheUtils.Max(cacheTotalCountKey, "0");//存储需要的页面总数
            CacheUtils.Max(cacheCurrentCountKey, "0");//存储当前的页面总数
            CacheUtils.Max(cacheMessageKey, string.Empty);//存储消息

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            FileSystemObject FSO = new FileSystemObject(publishmentSystemID);

            //返回“运行结果”和“错误信息”的字符串数组
            NameValueCollection retval;

            //当前生成页面数
            int currentCount = 0;

            #region 设置生成页面总数

            //首页
            int totalCount = 1;

            //栏目页数量
            ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(publishmentSystemID);
            totalCount += nodeIDArrayList.Count;

            //内容页数量
            Hashtable nodeIDWithContentIDArrayListMap = new Hashtable();
            foreach (int nodeID in nodeIDArrayList)
            {
                string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeID);
                string orderByString = ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc);
                ArrayList contentIDArrayList = DataProvider.ContentDAO.GetContentIDArrayListChecked(tableName, nodeID, orderByString);
                nodeIDWithContentIDArrayListMap.Add(nodeID, contentIDArrayList);
                totalCount += contentIDArrayList.Count;
            }

            //文件页数量
            ArrayList templateIDArrayList = DataProvider.TemplateDAO.GetTemplateIDArrayListByType(publishmentSystemID, ETemplateType.FileTemplate);
            string directoryPath = PathUtility.MapPath(publishmentSystemInfo, "@/include");
            DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);
            string[] fileNames = DirectoryUtils.GetFileNames(directoryPath);
            ArrayList includeFileArrayList = new ArrayList();
            includeFileArrayList.AddRange(fileNames);
            totalCount += templateIDArrayList.Count + includeFileArrayList.Count;

            #endregion

            CacheUtils.Max(cacheTotalCountKey, totalCount.ToString());//存储需要的页面总数
            CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数


            #region 生成首页
            try
            {
                if (publishmentSystemInfo.Additional.IsCreateRedirectPage)
                {
                    FSO.AddIndexToWaitingCreate();
                }
                else
                {
                    FSO.CreateIndex();
                }
                currentCount++;
                CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
            }
            catch (Exception ex)
            {
                errorBuilder.Append("<br />首页生成失败！");
                errorBuilder.Append("<br />").Append(ex.Message);
                LogUtils.AddErrorLog(ex);
            }
            #endregion

            #region 生成栏目页
            if (nodeIDArrayList.Count > 0)
            {
                foreach (int nodeIDToCreate in nodeIDArrayList)
                {
                    try
                    {
                        FSO.CreateChannel(nodeIDToCreate);
                        currentCount++;
                        CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                    }
                    catch (Exception ex)
                    {
                        errorBuilder.Append("<br />栏目“").Append(NodeManager.GetNodeName(publishmentSystemID, nodeIDToCreate)).Append("”生成失败！");
                        errorBuilder.Append("<br />").Append(ex.Message);
                        LogUtils.AddErrorLog(ex);
                    }
                }
            }
            #endregion

            #region 生成内容页
            if (nodeIDArrayList.Count > 0)
            {
                foreach (int nodeID in nodeIDArrayList)
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
                    ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                    string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);

                    ArrayList contentIDArrayList = nodeIDWithContentIDArrayListMap[nodeID] as ArrayList;
                    if (contentIDArrayList != null)
                    {
                        foreach (int contentID in contentIDArrayList)
                        {
                            try
                            {
                                FSO.CreateContent(tableStyle, tableName, nodeID, contentID);
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
            #endregion

            #region 生成文件页
            if (templateIDArrayList.Count > 0 || includeFileArrayList.Count > 0)
            {

                foreach (int templateIDToCreate in templateIDArrayList)
                {
                    try
                    {
                        FSO.CreateFile(templateIDToCreate);

                        currentCount++;
                        CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                    }
                    catch (Exception ex)
                    {
                        errorBuilder.Append("<br />文件“").Append(TemplateManager.GetCreatedFileFullName(publishmentSystemID, templateIDToCreate)).Append("”生成失败！");
                        errorBuilder.Append("<br />").Append(ex.Message);
                        LogUtils.AddErrorLog(ex);
                    }
                }

                foreach (string includeFileToCreate in includeFileArrayList)
                {
                    try
                    {
                        string fileName = PathUtils.GetFileNameWithoutExtension(includeFileToCreate);
                        if (fileName.ToLower().EndsWith("_parsed"))
                            continue;

                        FSO.CreateIncludeFile("include/" + includeFileToCreate, true);

                        currentCount++;
                        CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                    }
                    catch (Exception ex)
                    {
                        errorBuilder.Append("<br />文件“").Append(includeFileToCreate).Append("”生成失败！");
                        errorBuilder.Append("<br />").Append(ex.Message);
                        LogUtils.AddErrorLog(ex);
                    }
                }
            }
            #endregion

            resultBuilder.AppendFormat("任务完成，共生成文件<strong> {0} </strong>个。", currentCount);

            retval = JsManager.AjaxService.GetProgressTaskNameValueCollection(resultBuilder.ToString(), errorBuilder.ToString());

            CacheUtils.Remove(cacheTotalCountKey);//取消存储需要的页面总数
            CacheUtils.Remove(cacheCurrentCountKey);//取消存储当前的页面总数
            CacheUtils.Remove(cacheMessageKey);//取消存储消息

            return retval;
        }

        public NameValueCollection CreateChannels(int publishmentSystemID, string userKeyPrefix)
        {
            string cacheTotalCountKey = userKeyPrefix + CACHE_TOTAL_COUNT;
            string cacheCurrentCountKey = userKeyPrefix + CACHE_CURRENT_COUNT;
            string cacheMessageKey = userKeyPrefix + CACHE_MESSAGE;

            CacheUtils.Max(cacheTotalCountKey, "0");//存储需要的页面总数
            CacheUtils.Max(cacheCurrentCountKey, "0");//存储当前的页面总数
            CacheUtils.Max(cacheMessageKey, string.Empty);//存储消息

            //返回“运行结果”和“错误信息”的字符串数组
            NameValueCollection retval;

            ArrayList nodeIDArrayList = TranslateUtils.StringCollectionToIntArrayList(DbCacheManager.GetAndRemove(userKeyPrefix + Constants.CACHE_CREATE_CHANNELS_NODE_ID_ARRAYLIST));
            try
            {
                StringBuilder resultBuilder = new StringBuilder();
                StringBuilder errorBuilder = new StringBuilder();

                int totalCount = nodeIDArrayList.Count;
                int currentCount = 0;
                CacheUtils.Max(cacheTotalCountKey, totalCount.ToString());//存储需要的页面总数
                CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数

                if (nodeIDArrayList.Count > 0)
                {
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    FileSystemObject FSO = new FileSystemObject(publishmentSystemID);
                    foreach (int nodeIDToCreate in nodeIDArrayList)
                    {
                        try
                        {
                            FSO.CreateChannel(nodeIDToCreate);
                            currentCount++;
                            CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                        }
                        catch (Exception ex)
                        {
                            errorBuilder.Append("<br />栏目“").Append(NodeManager.GetNodeName(publishmentSystemID, nodeIDToCreate)).Append("”生成失败！");
                            errorBuilder.Append("<br />").Append(ex.Message);
                            LogUtils.AddErrorLog(ex);
                        }
                    }
                }

                resultBuilder.AppendFormat("任务完成，共生成栏目<strong> {0} </strong>页。", currentCount);

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

        public NameValueCollection CreateChannelsOneByOne(int publishmentSystemID, string userKeyPrefix, bool isIncludeChildren, bool isCreateContents)
        {
            string cacheTotalCountKey = userKeyPrefix + CACHE_TOTAL_COUNT;
            string cacheCurrentCountKey = userKeyPrefix + CACHE_CURRENT_COUNT;
            string cacheMessageKey = userKeyPrefix + CACHE_MESSAGE;

            CacheUtils.Max(cacheTotalCountKey, "0");//存储需要的页面总数
            CacheUtils.Max(cacheCurrentCountKey, "0");//存储当前的页面总数
            CacheUtils.Max(cacheMessageKey, string.Empty);//存储消息

            //返回“运行结果”和“错误信息”的字符串数组
            NameValueCollection retval;
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

            ArrayList nodeIDArrayList = TranslateUtils.StringCollectionToIntArrayList(DbCacheManager.GetAndRemove(userKeyPrefix + Constants.CACHE_CREATE_CHANNELS_NODE_ID_ARRAYLIST));
            if (isIncludeChildren)
            {
                ArrayList theNodeIDArrayList = new ArrayList();
                foreach (int nodeID in nodeIDArrayList)
                {
                    theNodeIDArrayList.Add(nodeID);
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
            try
            {
                StringBuilder resultBuilder = new StringBuilder();
                StringBuilder errorBuilder = new StringBuilder();

                int totalCount = nodeIDArrayList.Count;
                int currentCount = 0;

                Hashtable nodeIDWithContentIDArrayListMap = new Hashtable();
                if (isCreateContents)
                {
                    foreach (int nodeID in nodeIDArrayList)
                    {
                        string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeID);
                        string orderByString = ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc);
                        ArrayList contentIDArrayList = DataProvider.ContentDAO.GetContentIDArrayListChecked(tableName, nodeID, orderByString);
                        if (!nodeIDWithContentIDArrayListMap.ContainsKey(nodeID))
                        {
                            nodeIDWithContentIDArrayListMap.Add(nodeID, contentIDArrayList);
                            totalCount += contentIDArrayList.Count;
                        }
                    }
                }

                CacheUtils.Max(cacheTotalCountKey, totalCount.ToString());//存储需要的页面总数
                CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数

                if (nodeIDArrayList.Count > 0)
                {
                    FileSystemObject FSO = new FileSystemObject(publishmentSystemID);
                    foreach (int nodeIDToCreate in nodeIDArrayList)
                    {
                        try
                        {
                            FSO.CreateChannel(nodeIDToCreate);
                            currentCount++;
                            CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                        }
                        catch (Exception ex)
                        {
                            errorBuilder.Append("<br />栏目“").Append(NodeManager.GetNodeName(publishmentSystemID, nodeIDToCreate)).Append("”生成失败！");
                            errorBuilder.Append("<br />").Append(ex.Message);
                            LogUtils.AddErrorLog(ex);
                        }
                        if (isCreateContents)
                        {
                            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeIDToCreate);
                            ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                            string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);

                            ArrayList contentIDArrayList = nodeIDWithContentIDArrayListMap[nodeIDToCreate] as ArrayList;
                            if (contentIDArrayList != null)
                            {
                                foreach (int contentID in contentIDArrayList)
                                {
                                    try
                                    {
                                        FSO.CreateContent(tableStyle, tableName, nodeIDToCreate, contentID);

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

                resultBuilder.AppendFormat("任务完成，共生成栏目/内容<strong> {0} </strong>页。", currentCount);

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

        public NameValueCollection CreateContents(int publishmentSystemID, string userKeyPrefix)
        {
            string cacheTotalCountKey = userKeyPrefix + CACHE_TOTAL_COUNT;
            string cacheCurrentCountKey = userKeyPrefix + CACHE_CURRENT_COUNT;
            string cacheMessageKey = userKeyPrefix + CACHE_MESSAGE;

            CacheUtils.Max(cacheTotalCountKey, "0");//存储需要的页面总数
            CacheUtils.Max(cacheCurrentCountKey, "0");//存储当前的页面总数
            CacheUtils.Max(cacheMessageKey, string.Empty);//存储消息

            //返回“运行结果”和“错误信息”的字符串数组
            NameValueCollection retval;
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

            ArrayList nodeIDArrayList = TranslateUtils.StringCollectionToIntArrayList(DbCacheManager.GetAndRemove(userKeyPrefix + Constants.CACHE_CREATE_CONTENTS_NODE_ID_ARRAYLIST));
            try
            {
                StringBuilder resultBuilder = new StringBuilder();
                StringBuilder errorBuilder = new StringBuilder();

                int totalCount = 0;
                int currentCount = 0;

                Hashtable nodeIDWithContentIDArrayListMap = new Hashtable();
                foreach (int nodeID in nodeIDArrayList)
                {
                    string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeID);
                    string orderByString = ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc);
                    ArrayList contentIDArrayList = DataProvider.ContentDAO.GetContentIDArrayListChecked(tableName, nodeID, orderByString);
                    if (!nodeIDWithContentIDArrayListMap.ContainsKey(nodeID))
                    {
                        nodeIDWithContentIDArrayListMap.Add(nodeID, contentIDArrayList);
                        totalCount += contentIDArrayList.Count;
                    }
                }

                CacheUtils.Max(cacheTotalCountKey, totalCount.ToString());//存储需要的页面总数
                CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数

                if (nodeIDArrayList.Count > 0)
                {
                    FileSystemObject FSO = new FileSystemObject(publishmentSystemID);
                    foreach (int nodeID in nodeIDArrayList)
                    {
                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
                        ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                        string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);

                        ArrayList contentIDArrayList = nodeIDWithContentIDArrayListMap[nodeID] as ArrayList;
                        if (contentIDArrayList != null)
                        {
                            foreach (int contentID in contentIDArrayList)
                            {
                                try
                                {
                                    FSO.CreateContent(tableStyle, tableName, nodeID, contentID);
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

                resultBuilder.AppendFormat("任务完成，共生成内容<strong> {0} </strong>页。", currentCount);

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

        /// <summary>
        /// 服务组件生成内容
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="userKeyPrefix"></param>
        /// <returns></returns>
        public NameValueCollection CreateContentsByService(int publishmentSystemID, string userKeyPrefix, int createTaskID)
        {
            string cacheTotalCountKey = userKeyPrefix + CACHE_TOTAL_COUNT;
            string cacheQueuingCountKey = userKeyPrefix + CACHE_QUEUING_COUNT;
            string cacheCreateTaskIDKey = userKeyPrefix + CACHE_CREATE_TASK_ID;
            CacheUtils.Max(cacheTotalCountKey, "0");//存储需要的页面总数
            CacheUtils.Max(cacheQueuingCountKey, "0");//存储排队的页面总数
            CacheUtils.Max(cacheCreateTaskIDKey, "0");//存储任务ID
            NameValueCollection retval;
            //服务组件任务
            CreateTaskInfo createTaskInfo = BaiRongDataProvider.CreateTaskDAO.GetCreateTaskInfo(createTaskID);
            if (createTaskInfo != null)
            {
                int queuingCount = 0;//排队数量
                int totalCount = 0;//生成总数
                int currentCount = 0;//已经处理数量
                int errorCount = 0;//错误数量
                BaiRongDataProvider.CreateTaskDAO.UpdateState();
                BaiRongDataProvider.CreateTaskDAO.GetQueuingTaskCount(createTaskInfo.ID, out totalCount, out currentCount, out errorCount, out queuingCount);
                CacheUtils.Max(cacheTotalCountKey, createTaskInfo.TotalCount.ToString());//存储需要的页面总数
                CacheUtils.Max(cacheQueuingCountKey, queuingCount.ToString());//存储当前的页面总数
                CacheUtils.Max(cacheCreateTaskIDKey, createTaskID.ToString());//存储任务ID
                if (queuingCount > 0)
                {
                    //排队中
                    retval = JsManager.AjaxService.GetCountArrayNameValueCollection(totalCount, 0, errorCount, queuingCount, string.Format("队列中现在还有<strong> {0} </strong>任务等待处理，请耐性等待。您现在也可以离开处理其他工作。<a href='{1}'>查看进度</a>", queuingCount, BaiRong.BackgroundPages.BackgroundCreateTaskState.GetRedirectString(createTaskID)));
                }
                else if (currentCount < totalCount)
                {
                    //处理中
                    retval = JsManager.AjaxService.GetCountArrayNameValueCollection(totalCount, currentCount, errorCount, queuingCount, string.Format("任务正在进行。您现在也可以离开处理其他工作。<a href='{0}'>查看进度</a>", BaiRong.BackgroundPages.BackgroundCreateTaskState.GetRedirectString(createTaskID)));
                    BaiRongDataProvider.CreateTaskDAO.UpdateState(createTaskID, ECreateTaskType.Processing);
                }
                else
                {
                    //已完成
                    retval = JsManager.AjaxService.GetProgressTaskNameValueCollection(string.Format("任务已经完成。"), string.Empty);
                    BaiRongDataProvider.CreateTaskDAO.UpdateState(createTaskID, ECreateTaskType.Completed);
                    CacheUtils.Remove(userKeyPrefix + CACHE_TOTAL_COUNT);//删除需要的页面总数
                    CacheUtils.Remove(userKeyPrefix + CACHE_QUEUING_COUNT);//删除排队的页面总数
                    CacheUtils.Remove(userKeyPrefix + CACHE_CREATE_TASK_ID);//删除任务ID
                }
            }
            else
            {
                retval = JsManager.AjaxService.GetProgressTaskNameValueCollection(string.Format("任务不存在。"), string.Empty);
            }
            return retval;
        }

        public NameValueCollection CreateContentsOneByOne(int publishmentSystemID, int nodeID, string userKeyPrefix)
        {
            string cacheTotalCountKey = userKeyPrefix + CACHE_TOTAL_COUNT;
            string cacheCurrentCountKey = userKeyPrefix + CACHE_CURRENT_COUNT;
            string cacheMessageKey = userKeyPrefix + CACHE_MESSAGE;

            CacheUtils.Max(cacheTotalCountKey, "0");//存储需要的页面总数
            CacheUtils.Max(cacheCurrentCountKey, "0");//存储当前的页面总数
            CacheUtils.Max(cacheMessageKey, string.Empty);//存储消息

            //返回“运行结果”和“错误信息”的字符串数组
            NameValueCollection retval;

            ArrayList contentIDArrayList = TranslateUtils.StringCollectionToIntArrayList(DbCacheManager.GetAndRemove(userKeyPrefix + Constants.CACHE_CREATE_CONTENTS_CONTENT_ID_ARRAYLIST));

            StringBuilder resultBuilder = new StringBuilder();
            StringBuilder errorBuilder = new StringBuilder();

            try
            {
                int totalCount = contentIDArrayList.Count;
                int currentCount = 0;

                CacheUtils.Max(cacheTotalCountKey, totalCount.ToString());//存储需要的页面总数
                CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数

                if (contentIDArrayList.Count > 0)
                {
                    FileSystemObject FSO = new FileSystemObject(publishmentSystemID);
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
                    ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                    string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);

                    foreach (int contentID in contentIDArrayList)
                    {
                        try
                        {
                            FSO.CreateContent(tableStyle, tableName, nodeID, contentID);

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

                resultBuilder.AppendFormat("任务完成，共生成内容<strong> {0} </strong>页。", currentCount);

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

        public NameValueCollection CreateByTemplate(int publishmentSystemID, int templateID, string userKeyPrefix)
        {
            string cacheTotalCountKey = userKeyPrefix + CACHE_TOTAL_COUNT;
            string cacheCurrentCountKey = userKeyPrefix + CACHE_CURRENT_COUNT;
            string cacheMessageKey = userKeyPrefix + CACHE_MESSAGE;

            CacheUtils.Max(cacheTotalCountKey, "0");//存储需要的页面总数
            CacheUtils.Max(cacheCurrentCountKey, "0");//存储当前的页面总数
            CacheUtils.Max(cacheMessageKey, string.Empty);//存储消息

            //返回“运行结果”和“错误信息”的字符串数组
            NameValueCollection retval;

            StringBuilder resultBuilder = new StringBuilder();
            StringBuilder errorBuilder = new StringBuilder();

            try
            {
                int totalCount = 0;
                int currentCount = 0;

                TemplateInfo templateInfo = TemplateManager.GetTemplateInfo(publishmentSystemID, templateID);
                ArrayList nodeIDArrayList = new ArrayList();

                if (templateInfo.TemplateType == ETemplateType.IndexPageTemplate || templateInfo.TemplateType == ETemplateType.FileTemplate)
                {
                    totalCount = 1;
                }
                else
                {
                    nodeIDArrayList = DataProvider.TemplateDAO.GetNodeIDArrayList(templateInfo);
                    if (templateInfo.TemplateType == ETemplateType.ChannelTemplate)
                    {
                        totalCount = nodeIDArrayList.Count;
                    }
                    else if (templateInfo.TemplateType == ETemplateType.ContentTemplate)
                    {
                        foreach (int nodeID in nodeIDArrayList)
                        {
                            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
                            if (nodeInfo != null)
                            {
                                totalCount += nodeInfo.ContentNum;
                            }
                        }
                    }
                }

                CacheUtils.Max(cacheTotalCountKey, totalCount.ToString());//存储需要的页面总数
                CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数

                if (totalCount > 0)
                {
                    FileSystemObject FSO = new FileSystemObject(publishmentSystemID);
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

                    if (templateInfo.TemplateType == ETemplateType.IndexPageTemplate)
                    {
                        try
                        {
                            FSO.CreateIndex();

                            currentCount++;
                            CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                        }
                        catch (Exception ex)
                        {
                            errorBuilder.Append("<br />首页生成失败！");
                            errorBuilder.Append("<br />").Append(ex.Message);
                            LogUtils.AddErrorLog(ex);
                        }
                    }
                    else if (templateInfo.TemplateType == ETemplateType.FileTemplate)
                    {
                        try
                        {
                            FSO.CreateFile(templateID);

                            currentCount++;
                            CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                        }
                        catch (Exception ex)
                        {
                            errorBuilder.Append("<br />单页生成失败！");
                            errorBuilder.Append("<br />").Append(ex.Message);
                            LogUtils.AddErrorLog(ex);
                        }
                    }
                    else if (templateInfo.TemplateType == ETemplateType.ChannelTemplate)
                    {
                        foreach (int nodeID in nodeIDArrayList)
                        {
                            try
                            {
                                FSO.CreateChannel(nodeID);
                                currentCount++;
                                CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                            }
                            catch (Exception ex)
                            {
                                errorBuilder.Append("<br />栏目“").Append(NodeManager.GetNodeName(publishmentSystemID, nodeID)).Append("”生成失败！");
                                errorBuilder.Append("<br />").Append(ex.Message);
                                LogUtils.AddErrorLog(ex);
                            }
                        }
                    }
                    else if (templateInfo.TemplateType == ETemplateType.ContentTemplate)
                    {
                        string orderByString = ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc);

                        foreach (int nodeID in nodeIDArrayList)
                        {
                            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
                            if (nodeInfo != null)
                            {
                                ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                                string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
                                ArrayList contentIDArrayList = DataProvider.ContentDAO.GetContentIDArrayListChecked(tableName, nodeID, orderByString);
                                if (contentIDArrayList != null)
                                {
                                    foreach (int contentID in contentIDArrayList)
                                    {
                                        try
                                        {
                                            FSO.CreateContent(tableStyle, tableName, nodeID, contentID);

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
                }

                resultBuilder.AppendFormat("任务完成，共生成内容<strong> {0} </strong>页。", currentCount);

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

        public NameValueCollection CreateByIDsCollection(int publishmentSystemID, string userKeyPrefix)
        {
            string cacheTotalCountKey = userKeyPrefix + CACHE_TOTAL_COUNT;
            string cacheCurrentCountKey = userKeyPrefix + CACHE_CURRENT_COUNT;
            string cacheMessageKey = userKeyPrefix + CACHE_MESSAGE;

            CacheUtils.Max(cacheTotalCountKey, "0");//存储需要的页面总数
            CacheUtils.Max(cacheCurrentCountKey, "0");//存储当前的页面总数
            CacheUtils.Max(cacheMessageKey, string.Empty);//存储消息

            //返回“运行结果”和“错误信息”的字符串数组
            NameValueCollection retval;

            ArrayList idsArrayList = TranslateUtils.StringCollectionToArrayList(DbCacheManager.GetAndRemove(userKeyPrefix + Constants.CACHE_CREATE_IDS_COLLECTION));

            StringBuilder resultBuilder = new StringBuilder();
            StringBuilder errorBuilder = new StringBuilder();

            try
            {
                int totalCount = 0;

                Hashtable idsHashtable = new Hashtable();
                foreach (string ids in idsArrayList)
                {
                    int nodeID = TranslateUtils.ToInt(ids.Split('_')[0]);
                    int contentID = TranslateUtils.ToInt(ids.Split('_')[1]);
                    ArrayList contentIDArrayList = idsHashtable[nodeID] as ArrayList;
                    if (contentIDArrayList == null)
                    {
                        contentIDArrayList = new ArrayList();
                    }
                    contentIDArrayList.Add(contentID);
                    idsHashtable[nodeID] = contentIDArrayList;
                    //totalCount++;
                }

                totalCount += idsHashtable.Count;

                int currentCount = 0;

                CacheUtils.Max(cacheTotalCountKey, totalCount.ToString());//存储需要的页面总数
                CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数

                if (idsHashtable.Count > 0)
                {
                    FileSystemObject FSO = new FileSystemObject(publishmentSystemID);
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

                    foreach (int nodeID in idsHashtable.Keys)
                    {
                        ArrayList contentIDArrayList = idsHashtable[nodeID] as ArrayList;

                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
                        ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                        string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);

                        foreach (int contentID in contentIDArrayList)
                        {
                            try
                            {
                                FSO.CreateContent(tableStyle, tableName, nodeID, contentID);

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

                        try
                        {
                            FSO.CreateChannel(nodeID);

                            currentCount++;
                            CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                        }
                        catch (Exception ex)
                        {
                            errorBuilder.Append("<br />栏目“").Append(nodeInfo.NodeName).Append("”生成失败！");
                            errorBuilder.Append("<br />").Append(ex.Message);
                            LogUtils.AddErrorLog(ex);
                        }
                    }
                }

                resultBuilder.AppendFormat("任务完成，共生成<strong> {0} </strong>页。", currentCount);

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

        public NameValueCollection CreateFiles(int publishmentSystemID, string userKeyPrefix)
        {
            string cacheTotalCountKey = userKeyPrefix + CACHE_TOTAL_COUNT;
            string cacheCurrentCountKey = userKeyPrefix + CACHE_CURRENT_COUNT;
            string cacheMessageKey = userKeyPrefix + CACHE_MESSAGE;

            CacheUtils.Max(cacheTotalCountKey, "0");//存储需要的页面总数
            CacheUtils.Max(cacheCurrentCountKey, "0");//存储当前的页面总数
            CacheUtils.Max(cacheMessageKey, string.Empty);//存储消息

            //返回“运行结果”和“错误信息”的字符串数组
            NameValueCollection retval;

            ArrayList templateIDArrayList = TranslateUtils.StringCollectionToIntArrayList(DbCacheManager.GetAndRemove(userKeyPrefix + Constants.CACHE_CREATE_FILES_TEMPLATE_ID_ARRAYLIST));
            ArrayList includeFileArrayList = TranslateUtils.StringCollectionToArrayList(DbCacheManager.GetAndRemove(userKeyPrefix + Constants.CACHE_CREATE_FILES_INCLUDE_FILE_ARRAYLIST));

            try
            {
                StringBuilder resultBuilder = new StringBuilder();
                StringBuilder errorBuilder = new StringBuilder();

                int totalCount = templateIDArrayList.Count + includeFileArrayList.Count;
                int currentCount = 0;

                CacheUtils.Max(cacheTotalCountKey, totalCount.ToString());//存储需要的页面总数
                CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数

                if (templateIDArrayList.Count > 0 || includeFileArrayList.Count > 0)
                {
                    FileSystemObject FSO = new FileSystemObject(publishmentSystemID);
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

                    foreach (int templateIDToCreate in templateIDArrayList)
                    {
                        try
                        {
                            FSO.CreateFile(templateIDToCreate);

                            currentCount++;
                            CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                        }
                        catch (Exception ex)
                        {
                            errorBuilder.Append("<br />文件“").Append(TemplateManager.GetCreatedFileFullName(publishmentSystemID, templateIDToCreate)).Append("”生成失败！");
                            errorBuilder.Append("<br />").Append(ex.Message);
                            LogUtils.AddErrorLog(ex);
                        }
                    }

                    foreach (string includeFileToCreate in includeFileArrayList)
                    {
                        try
                        {
                            FSO.CreateIncludeFile("include/" + includeFileToCreate, true);

                            currentCount++;
                            CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                        }
                        catch (Exception ex)
                        {
                            errorBuilder.Append("<br />文件“").Append(includeFileToCreate).Append("”生成失败！");
                            errorBuilder.Append("<br />").Append(ex.Message);
                            LogUtils.AddErrorLog(ex);
                        }
                    }
                }

                resultBuilder.AppendFormat("任务完成，共生成文件<strong> {0} </strong>个。", currentCount);

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

        #region 创建应用

        public NameValueCollection CreatePublishmentSystem(int publishmentSystemID, bool isUseSiteTemplate, bool isImportContents, bool isImportTableStyles, string siteTemplateDir, bool isUseTables, string userKeyPrefix, string returnUrl)
        {
            string cacheTotalCountKey = userKeyPrefix + CACHE_TOTAL_COUNT;
            string cacheCurrentCountKey = userKeyPrefix + CACHE_CURRENT_COUNT;
            string cacheMessageKey = userKeyPrefix + CACHE_MESSAGE;

            CacheUtils.Max(cacheTotalCountKey, "3");//存储需要的页面总数
            CacheUtils.Max(cacheCurrentCountKey, "0");//存储当前的页面总数
            CacheUtils.Max(cacheMessageKey, string.Empty);//存储消息

            //返回“运行结果”、“错误信息”及“执行JS脚本”的字符串数组
            NameValueCollection retval;

            try
            {

                CacheUtils.Max(cacheCurrentCountKey, "1");//存储当前的页面总数
                CacheUtils.Max(cacheMessageKey, "正在创建应用...");//存储消息
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);


                CacheUtils.Max(cacheCurrentCountKey, "2");//存储当前的页面总数
                CacheUtils.Max(cacheMessageKey, "正在导入数据...");//存储消息
                if (isUseSiteTemplate && !string.IsNullOrEmpty(siteTemplateDir))
                {
                    SiteTemplateManager.Instance.ImportSiteTemplateToEmptyPublishmentSystem(publishmentSystemID, siteTemplateDir, isUseTables, isImportContents, isImportTableStyles);
                }


                CacheUtils.Max(cacheCurrentCountKey, "3");//存储当前的页面总数
                CacheUtils.Max(cacheMessageKey, "创建成功！");//存储消息
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    returnUrl = PageUtils.AddQueryString(StringUtils.ValueFromUrl(returnUrl), "PublishmentSystemID", publishmentSystemID.ToString());
                    retval = JsManager.AjaxService.GetWaitingTaskNameValueCollection(string.Format("应用 <strong>{0}<strong> 创建成功!", publishmentSystemInfo.PublishmentSystemName), string.Empty, string.Format("location.href='{0}';", returnUrl));
                }
                else
                {
                    string initUrl = PageUtils.GetCMSUrl("background_initialization.aspx");
                    retval = JsManager.AjaxService.GetWaitingTaskNameValueCollection(string.Format("应用 <strong>{0}<strong> 创建成功!", publishmentSystemInfo.PublishmentSystemName), string.Empty, string.Format("top.location.href='{0}';", initUrl));
                }
            }
            catch (Exception ex)
            {
                retval = JsManager.AjaxService.GetWaitingTaskNameValueCollection(string.Empty, ex.Message, string.Empty);
                LogUtils.AddErrorLog(ex);
            }

            CacheUtils.Remove(cacheTotalCountKey);//取消存储需要的页面总数
            CacheUtils.Remove(cacheCurrentCountKey);//取消存储当前的页面总数
            CacheUtils.Remove(cacheMessageKey);//取消存储消息
            CacheUtils.Clear();

            return retval;
        }

        public NameValueCollection CreatePublishmentSystem(int publishmentSystemID, bool isUseSiteTemplate, bool isImportContents, bool isImportTableStyles, string siteTemplateDir, bool isUseTables, string userKeyPrefix, string returnUrl, bool isTop)
        {
            string cacheTotalCountKey = userKeyPrefix + CACHE_TOTAL_COUNT;
            string cacheCurrentCountKey = userKeyPrefix + CACHE_CURRENT_COUNT;
            string cacheMessageKey = userKeyPrefix + CACHE_MESSAGE;

            CacheUtils.Max(cacheTotalCountKey, "3");//存储需要的页面总数
            CacheUtils.Max(cacheCurrentCountKey, "0");//存储当前的页面总数
            CacheUtils.Max(cacheMessageKey, string.Empty);//存储消息

            //返回“运行结果”、“错误信息”及“执行JS脚本”的字符串数组
            NameValueCollection retval;

            try
            {
                CacheUtils.Max(cacheCurrentCountKey, "1");//存储当前的页面总数
                CacheUtils.Max(cacheMessageKey, "正在创建应用...");//存储消息

                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);




                CacheUtils.Max(cacheCurrentCountKey, "2");//存储当前的页面总数
                CacheUtils.Max(cacheMessageKey, "正在导入数据...");//存储消息
                if (isUseSiteTemplate && !string.IsNullOrEmpty(siteTemplateDir))
                {
                    SiteTemplateManager.Instance.ImportSiteTemplateToEmptyPublishmentSystem(publishmentSystemID, siteTemplateDir, isUseTables, isImportContents, isImportTableStyles);
                }


                CacheUtils.Max(cacheCurrentCountKey, "3");//存储当前的页面总数
                CacheUtils.Max(cacheMessageKey, "创建成功！");//存储消息
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    returnUrl = PageUtils.AddQueryString(StringUtils.ValueFromUrl(returnUrl), "PublishmentSystemID", publishmentSystemID.ToString());
                    retval = JsManager.AjaxService.GetWaitingTaskNameValueCollection(string.Format("应用 <strong>{0}<strong> 创建成功!", publishmentSystemInfo.PublishmentSystemName), string.Empty, isTop ? string.Format("top.location.href='{0}';", returnUrl) : string.Format("location.href='{0}';", returnUrl));
                }
                else
                {
                    string initUrl = PageUtils.GetCMSUrl("background_initialization.aspx");
                    retval = JsManager.AjaxService.GetWaitingTaskNameValueCollection(string.Format("应用 <strong>{0}<strong> 创建成功!", publishmentSystemInfo.PublishmentSystemName), string.Empty, isTop ? string.Format("location.href='{0}';", initUrl) : string.Format("top.location.href='{0}';", initUrl));
                }
            }
            catch (Exception ex)
            {
                throw ex;
                retval = JsManager.AjaxService.GetWaitingTaskNameValueCollection(string.Empty, ex.Message, string.Empty);
                LogUtils.AddErrorLog(ex);
            }

            CacheUtils.Remove(cacheTotalCountKey);//取消存储需要的页面总数
            CacheUtils.Remove(cacheCurrentCountKey);//取消存储当前的页面总数
            CacheUtils.Remove(cacheMessageKey);//取消存储消息
            CacheUtils.Clear();

            return retval;
        }

        /// <summary>
        /// 创建站点之后，自动生成
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="isUseSiteTemplate"></param>
        /// <param name="isImportContents"></param>
        /// <param name="isImportTableStyles"></param>
        /// <param name="siteTemplateDir"></param>
        /// <param name="isUseTables"></param>
        /// <param name="userKeyPrefix"></param>
        /// <param name="returnUrl"></param>
        /// <param name="isTop"></param>
        /// <param name="isCreateAll"></param>
        /// <returns></returns>
        public NameValueCollection CreatePublishmentSystem(int publishmentSystemID, bool isUseSiteTemplate, bool isImportContents, bool isImportTableStyles, string siteTemplateDir, bool isUseTables, string userKeyPrefix, string returnUrl, bool isTop, bool isCreateAll)
        {
            //返回“运行结果”、“错误信息”及“执行JS脚本”的字符串数组
            NameValueCollection retval;
            retval = CreatePublishmentSystem(publishmentSystemID, isUseSiteTemplate, isImportContents, isImportTableStyles, siteTemplateDir, isUseTables, userKeyPrefix, returnUrl, isTop);
            if (isCreateAll)
                new SiteServer.STL.BackgroundPages.Service.CreateService().CreateAll(publishmentSystemID, string.Empty);
            return retval;
        }

        #endregion
    }
}
