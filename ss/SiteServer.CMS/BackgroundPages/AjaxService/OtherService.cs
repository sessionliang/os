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
using SiteServer.CMS.Core.Security;
using BaiRong.Core.Net;
using BaiRong.Core.IO;
using SiteServer.CMS.Controls;

namespace SiteServer.CMS.BackgroundPages.Service
{
    public class OtherService : Page
    {
        public void Page_Load(object sender, System.EventArgs e)
        {
            string type = base.Request["type"];
            NameValueCollection retval = new NameValueCollection();
            string retString = string.Empty;

            if (type == "GetCountArray")
            {
                string userKeyPrefix = base.Request["userKeyPrefix"];
                retval = GetCountArray(userKeyPrefix);
            }
            else if (type == "SiteTemplateDownload")
            {
                string userKeyPrefix = base.Request["userKeyPrefix"];
                string downloadUrl = RuntimeUtils.DecryptStringByTranslate(base.Request["downloadUrl"]);
                string directoryName = base.Request["directoryName"];
                retval = this.SiteTemplateDownload(downloadUrl, directoryName, userKeyPrefix);
            }
            else if (type == "SiteTemplateZip")
            {
                string userKeyPrefix = base.Request["userKeyPrefix"];
                string directoryName = base.Request["directoryName"];
                retval = this.SiteTemplateZip(directoryName, userKeyPrefix);
            }
            else if (type == "IndependentTemplateDownload")
            {
                string userKeyPrefix = base.Request["userKeyPrefix"];
                string downloadUrl = RuntimeUtils.DecryptStringByTranslate(base.Request["downloadUrl"]);
                string directoryName = base.Request["directoryName"];
                retval = this.IndependentTemplateDownload(downloadUrl, directoryName, userKeyPrefix);
            }
            else if (type == "IndependentTemplateZip")
            {
                string userKeyPrefix = base.Request["userKeyPrefix"];
                string directoryName = base.Request["directoryName"];
                retval = this.IndependentTemplateZip(directoryName, userKeyPrefix);
            }
            else if (type == "GetLoadingChannels")
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
                int parentID = TranslateUtils.ToInt(base.Request["parentID"]);
                string loadingType = base.Request["loadingType"];
                string additional = base.Request["additional"];
                retString = GetLoadingChannels(publishmentSystemID, parentID, loadingType, additional);
            }
            else if (type == "GetLoadingClassify")
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
                int parentID = TranslateUtils.ToInt(base.Request["parentID"]);
                string additional = base.Request["additional"];
                string classifyType = base.Request["classifyType"];
                string actionType = base.Request["actionType"];
                retString = GetLoadingClassify(publishmentSystemID, parentID, classifyType, actionType, additional);
            }
            //else if (type == "GetLoadingGovPublicCategories")
            //{
            //    string classCode = base.Request["classCode"];
            //    int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
            //    int parentID = TranslateUtils.ToInt(base.Request["parentID"]);
            //    string loadingType = base.Request["loadingType"];
            //    string additional = base.Request["additional"];
            //    retString = GetLoadingGovPublicCategories(classCode, publishmentSystemID, parentID, loadingType, additional);
            //}
            //else if (type == "GetLoadingTemplates")
            //{
            //    int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
            //    string templateType = base.Request["templateType"];
            //    retString = GetLoadingTemplates(publishmentSystemID, templateType);
            //}
            //else if (type == "StlTemplate")
            //{
            //    int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
            //    int templateID = TranslateUtils.ToInt(base.Request["templateID"]);
            //    string includeUrl = base.Request["includeUrl"];
            //    string operation = base.Request["operation"];
            //    retval = TemplateDesignOperation.Operate(publishmentSystemID, templateID, includeUrl, operation, base.Request.Form);
            //}

            if (!string.IsNullOrEmpty(retString))
            {
                Page.Response.Write(retString);
                Page.Response.End();
            }
            else
            {
                string jsonString = TranslateUtils.NameValueCollectionToJsonString(retval);
                if (jsonString == "{}")
                    jsonString = "";
                Page.Response.Write(jsonString);
                Page.Response.End();
            }
        }

        public const string CACHE_TOTAL_COUNT = "_TotalCount";
        public const string CACHE_CURRENT_COUNT = "_CurrentCount";
        public const string CACHE_MESSAGE = "_Message";

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

        public NameValueCollection SiteTemplateDownload(string downloadUrl, string directoryName, string userKeyPrefix)
        {
            string cacheTotalCountKey = userKeyPrefix + CACHE_TOTAL_COUNT;
            string cacheCurrentCountKey = userKeyPrefix + CACHE_CURRENT_COUNT;
            string cacheMessageKey = userKeyPrefix + CACHE_MESSAGE;

            CacheUtils.Max(cacheTotalCountKey, "5");//存储需要的页面总数
            CacheUtils.Max(cacheCurrentCountKey, "0");//存储当前的页面总数
            CacheUtils.Max(cacheMessageKey, string.Empty);//存储消息

            //返回“运行结果”和“错误信息”的字符串数组
            NameValueCollection retval;

            try
            {
                CacheUtils.Max(cacheCurrentCountKey, "1");
                CacheUtils.Max(cacheMessageKey, "开始下载模板压缩包，可能需要10到30分钟，请耐心等待");

                string filePath = PathUtility.GetSiteTemplatesPath(directoryName + ".zip");
                FileUtils.DeleteFileIfExists(filePath);
                WebClientUtils.SaveRemoteFileToLocal(downloadUrl, filePath);

                CacheUtils.Max(cacheCurrentCountKey, "4");
                CacheUtils.Max(cacheMessageKey, "模板压缩包下载成功，开始解压缩");

                string directoryPath = PathUtility.GetSiteTemplatesPath(directoryName);
                if (!DirectoryUtils.IsDirectoryExists(directoryPath))
                {
                    ZipUtils.UnpackFiles(filePath, directoryPath);
                }

                CacheUtils.Max(cacheCurrentCountKey, "5");
                CacheUtils.Max(cacheMessageKey, string.Empty);

                retval = JsManager.AjaxService.GetProgressTaskNameValueCollection("应用模板下载成功，请到应用模板管理中查看。", string.Empty);
            }
            catch (Exception ex)
            {
                retval = JsManager.AjaxService.GetProgressTaskNameValueCollection(string.Empty, string.Format(@"<br />下载失败！<br />{0}", ex.Message));
            }

            CacheUtils.Remove(cacheTotalCountKey);//取消存储需要的页面总数
            CacheUtils.Remove(cacheCurrentCountKey);//取消存储当前的页面总数
            CacheUtils.Remove(cacheMessageKey);//取消存储消息

            return retval;
        }

        public NameValueCollection SiteTemplateZip(string directoryName, string userKeyPrefix)
        {
            string cacheTotalCountKey = userKeyPrefix + CACHE_TOTAL_COUNT;
            string cacheCurrentCountKey = userKeyPrefix + CACHE_CURRENT_COUNT;
            string cacheMessageKey = userKeyPrefix + CACHE_MESSAGE;

            CacheUtils.Max(cacheTotalCountKey, "1");//存储需要的页面总数
            CacheUtils.Max(cacheCurrentCountKey, "0");//存储当前的页面总数
            CacheUtils.Max(cacheMessageKey, string.Empty);//存储消息

            //返回“运行结果”和“错误信息”的字符串数组
            NameValueCollection retval;

            try
            {
                directoryName = PathUtils.RemoveParentPath(directoryName);
                string fileName = directoryName + ".zip";
                string filePath = PathUtility.GetSiteTemplatesPath(fileName);
                string directoryPath = PathUtility.GetSiteTemplatesPath(directoryName);

                FileUtils.DeleteFileIfExists(filePath);

                ZipUtils.PackFiles(filePath, directoryPath);

                CacheUtils.Max(cacheCurrentCountKey, "1");//存储当前的页面总数

                retval = JsManager.AjaxService.GetProgressTaskNameValueCollection(string.Format("应用模板压缩成功，<a href='{0}' target=_blank>点击下载</a>。", PageUtility.GetSiteTemplatesUrl(fileName)), string.Empty);
            }
            catch (Exception ex)
            {
                retval = JsManager.AjaxService.GetProgressTaskNameValueCollection(string.Empty, string.Format(@"<br />应用模板压缩失败！<br />{0}", ex.Message));
            }

            CacheUtils.Remove(cacheTotalCountKey);//取消存储需要的页面总数
            CacheUtils.Remove(cacheCurrentCountKey);//取消存储当前的页面总数
            CacheUtils.Remove(cacheMessageKey);//取消存储消息

            return retval;
        }

        public NameValueCollection IndependentTemplateDownload(string downloadUrl, string directoryName, string userKeyPrefix)
        {
            string cacheTotalCountKey = userKeyPrefix + CACHE_TOTAL_COUNT;
            string cacheCurrentCountKey = userKeyPrefix + CACHE_CURRENT_COUNT;
            string cacheMessageKey = userKeyPrefix + CACHE_MESSAGE;

            CacheUtils.Max(cacheTotalCountKey, "5");//存储需要的页面总数
            CacheUtils.Max(cacheCurrentCountKey, "0");//存储当前的页面总数
            CacheUtils.Max(cacheMessageKey, string.Empty);//存储消息

            //返回“运行结果”和“错误信息”的字符串数组
            NameValueCollection retval;

            try
            {
                CacheUtils.Max(cacheCurrentCountKey, "1");
                CacheUtils.Max(cacheMessageKey, "开始下载模板压缩包，可能需要10到30分钟，请耐心等待");

                string filePath = PathUtility.GetIndependentTemplatesPath(directoryName + ".zip");
                FileUtils.DeleteFileIfExists(filePath);
                WebClientUtils.SaveRemoteFileToLocal(downloadUrl, filePath);

                CacheUtils.Max(cacheCurrentCountKey, "4");
                CacheUtils.Max(cacheMessageKey, "模板压缩包下载成功，开始解压缩");

                string directoryPath = PathUtility.GetIndependentTemplatesPath(directoryName);
                if (!DirectoryUtils.IsDirectoryExists(directoryPath))
                {
                    ZipUtils.UnpackFiles(filePath, directoryPath);
                }

                CacheUtils.Max(cacheCurrentCountKey, "5");
                CacheUtils.Max(cacheMessageKey, string.Empty);

                retval = JsManager.AjaxService.GetProgressTaskNameValueCollection("独立模板下载成功，请到独立模板管理中查看。", string.Empty);
            }
            catch (Exception ex)
            {
                retval = JsManager.AjaxService.GetProgressTaskNameValueCollection(string.Empty, string.Format(@"<br />下载失败！<br />{0}", ex.Message));
            }

            CacheUtils.Remove(cacheTotalCountKey);//取消存储需要的页面总数
            CacheUtils.Remove(cacheCurrentCountKey);//取消存储当前的页面总数
            CacheUtils.Remove(cacheMessageKey);//取消存储消息

            return retval;
        }

        public NameValueCollection IndependentTemplateZip(string directoryName, string userKeyPrefix)
        {
            string cacheTotalCountKey = userKeyPrefix + CACHE_TOTAL_COUNT;
            string cacheCurrentCountKey = userKeyPrefix + CACHE_CURRENT_COUNT;
            string cacheMessageKey = userKeyPrefix + CACHE_MESSAGE;

            CacheUtils.Max(cacheTotalCountKey, "1");//存储需要的页面总数
            CacheUtils.Max(cacheCurrentCountKey, "0");//存储当前的页面总数
            CacheUtils.Max(cacheMessageKey, string.Empty);//存储消息

            //返回“运行结果”和“错误信息”的字符串数组
            NameValueCollection retval;

            try
            {
                directoryName = PathUtils.RemoveParentPath(directoryName);
                string fileName = directoryName + ".zip";
                string filePath = PathUtility.GetIndependentTemplatesPath(fileName);
                string directoryPath = PathUtility.GetIndependentTemplatesPath(directoryName);
                FileUtils.DeleteFileIfExists(filePath);

                ZipUtils.PackFiles(filePath, directoryPath);

                CacheUtils.Max(cacheCurrentCountKey, "1");//存储当前的页面总数

                retval = JsManager.AjaxService.GetProgressTaskNameValueCollection(string.Format("独立模板压缩成功，<a href='{0}' target=_blank>点击下载</a>。", PageUtility.GetIndependentTemplatesUrl(fileName)), string.Empty);
            }
            catch (Exception ex)
            {
                retval = JsManager.AjaxService.GetProgressTaskNameValueCollection(string.Empty, string.Format(@"<br />独立模板压缩失败！<br />{0}", ex.Message));
            }

            CacheUtils.Remove(cacheTotalCountKey);//取消存储需要的页面总数
            CacheUtils.Remove(cacheCurrentCountKey);//取消存储当前的页面总数
            CacheUtils.Remove(cacheMessageKey);//取消存储消息

            return retval;
        }

        private ArrayList nodeList = new ArrayList();
        public string GetLoadingChannels(int publishmentSystemID, int parentID, string loadingType, string additional)
        {
            ArrayList arraylist = new ArrayList();

            ELoadingType eLoadingType = ELoadingTypeUtils.GetEnumType(loadingType);

            ArrayList nodeIDArrayList = new ArrayList();

            if (ELoadingTypeUtils.Equals(ELoadingType.EvaluationNodeTree, loadingType))
            {
                nodeList = DataProvider.NodeDAO.GetNodeIDByFunction(publishmentSystemID, "IsUseEvaluation".ToLower());
                nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(publishmentSystemID, parentID, nodeList);
            }
            else if (ELoadingTypeUtils.Equals(ELoadingType.TrialApplyNodeTree, loadingType) || ELoadingTypeUtils.Equals(ELoadingType.TrialReportNodeTree, loadingType) || ELoadingTypeUtils.Equals(ELoadingType.TrialAnalysisNodeTree, loadingType))
            {
                nodeList = DataProvider.NodeDAO.GetNodeIDByFunction(publishmentSystemID, "IsUseTrial".ToLower());
                nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(publishmentSystemID, parentID, nodeList);
            }
            else if (ELoadingTypeUtils.Equals(ELoadingType.SurveyNodeTree, loadingType) || ELoadingTypeUtils.Equals(ELoadingType.SurveyAnalysisNodeTree, loadingType))
            {
                nodeList = DataProvider.NodeDAO.GetNodeIDByFunction(publishmentSystemID, "IsUseSurvey".ToLower());
                nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(publishmentSystemID, parentID, nodeList);
            }
            else if (ELoadingTypeUtils.Equals(ELoadingType.CompareNodeTree, loadingType) )
            {
                nodeList = DataProvider.NodeDAO.GetNodeIDByFunction(publishmentSystemID, "IsUseCompare".ToLower());
                nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(publishmentSystemID, parentID, nodeList);
            }
            else 
            {
                nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(publishmentSystemID, parentID);
            }


            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(RuntimeUtils.DecryptStringByTranslate(additional));

            foreach (int nodeID in nodeIDArrayList)
            {
                bool enabled = true;
                enabled = AdminUtility.IsOwningNodeID(nodeID);
                if (!enabled)
                {
                    if (!AdminUtility.IsHasChildOwningNodeID(nodeID))
                    {
                        continue;
                    }
                }
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);

                arraylist.Add(ChannelLoading.GetChannelRowHtml(publishmentSystemInfo, nodeInfo, enabled, eLoadingType, nameValueCollection));
            }

            //arraylist.Reverse();

            StringBuilder builder = new StringBuilder();
            foreach (string html in arraylist)
            {
                builder.Append(html);
            }
            return builder.ToString();
        }

        /// <summary>
        /// 树形菜单加载子集数据
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="parentID"></param>
        /// <param name="loadingType"></param>
        /// <param name="additional"></param>
        /// <returns></returns>
        public string GetLoadingClassify(int publishmentSystemID, int parentID, string classifyType, string actionType, string additional)
        {
            ArrayList arraylist = new ArrayList();

            ArrayList itemIDArrayList = TreeManager.GetItemIDArrayListByParentID(publishmentSystemID, parentID, classifyType);
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(RuntimeUtils.DecryptStringByTranslate(additional));
            string redirectUrl = nameValueCollection["redirectUrl"];
            string linkUrl = nameValueCollection["linkUrl"];
            bool showCount = TranslateUtils.ToBool(nameValueCollection["showCount"], true);
            int showLayer = TranslateUtils.ToInt(nameValueCollection["showLayer"]);
            foreach (int itemID in itemIDArrayList)
            {
                nameValueCollection["itemID"] = itemID.ToString();
                TreeBaseItem itemInfo = TreeManager.GetItemInfo(publishmentSystemID, itemID, classifyType);
                if (actionType == "ClassifyTree")
                    arraylist.Add(Tree.GetItemRowHtml(publishmentSystemInfo, itemInfo, classifyType, nameValueCollection, showCount, showLayer));
                else if (actionType == "ClassifyManage")
                    arraylist.Add(Tree.GetItemRowHtmlForManage(publishmentSystemInfo, itemInfo, classifyType, nameValueCollection, showLayer));
            }

            //arraylist.Reverse();

            StringBuilder builder = new StringBuilder();
            foreach (string html in arraylist)
            {
                builder.Append(html);
            }
            return builder.ToString();
        }

        #region Helper

        private void ResponseText(string text)
        {
            base.Response.Clear();
            base.Response.Write(text);
            base.Response.End();
        }

        /// <summary>
        /// 向页面输出xml内容
        /// </summary>
        /// <param name="xmlnode">xml内容</param>
        private void ResponseXML(StringBuilder xmlnode)
        {
            base.Response.Clear();
            base.Response.ContentType = "Text/XML";
            base.Response.Expires = 0;

            base.Response.Cache.SetNoStore();
            base.Response.Write(xmlnode.ToString());
            base.Response.End();
        }

        /// <summary>
        /// 输出json内容
        /// </summary>
        /// <param name="json"></param>
        private void ResponseJSON(string json)
        {
            base.Response.Clear();
            base.Response.ContentType = "application/json";
            base.Response.Expires = 0;

            base.Response.Cache.SetNoStore();
            base.Response.Write(json);
            base.Response.End();
        }
        #endregion

    }
}
