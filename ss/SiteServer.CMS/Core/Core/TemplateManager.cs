using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.Caching;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Advertisement;
using SiteServer.CMS.BackgroundPages;

using System.Collections.Generic;

namespace SiteServer.CMS.Core
{
	public class TemplateManager
	{
        private TemplateManager()
		{
        }

        #region 缓存

        private const string cacheKey = "SiteServer.CMS.Core.TemplateManager";
        private static object syncRoot = new object();

        public static TemplateInfo GetTemplateInfo(int publishmentSystemID, int templateID)
        {
            TemplateInfo templateInfo = null;
            Dictionary<int, TemplateInfo> templateInfoDictionary = TemplateManager.GetTemplateInfoDictionaryByPublishmentSystemID(publishmentSystemID);

            if (templateInfoDictionary != null && templateInfoDictionary.ContainsKey(templateID))
            {
                templateInfo = templateInfoDictionary[templateID];
            }
            return templateInfo;
        }

        public static string GetCreatedFileFullName(int publishmentSystemID, int templateID)
        {
            string createdFileFullName = string.Empty;

            TemplateInfo templateInfo = TemplateManager.GetTemplateInfo(publishmentSystemID, templateID);
            if (templateInfo != null)
            {
                createdFileFullName = templateInfo.CreatedFileFullName;
            }

            return createdFileFullName;
        }

        public static string GetTemplateName(int publishmentSystemID, int templateID)
        {
            string templateName = string.Empty;

            TemplateInfo templateInfo = TemplateManager.GetTemplateInfo(publishmentSystemID, templateID);
            if (templateInfo != null)
            {
                templateName = templateInfo.TemplateName;
            }

            return templateName;
        }

        public static TemplateInfo GetTemplateInfoByTemplateName(int publishmentSystemID, ETemplateType templateType, string templateName)
        {
            TemplateInfo info = null;

            Dictionary<int, TemplateInfo> templateInfoDictionary = TemplateManager.GetTemplateInfoDictionaryByPublishmentSystemID(publishmentSystemID);
            if (templateInfoDictionary != null)
            {
                foreach (TemplateInfo templateInfo in templateInfoDictionary.Values)
                {
                    if (templateInfo.TemplateType == templateType && templateInfo.TemplateName == templateName)
                    {
                        info = templateInfo;
                        break;
                    }
                }
            }

            return info;
        }

        public static TemplateInfo GetDefaultTemplateInfo(int publishmentSystemID, ETemplateType templateType)
        {
            TemplateInfo info = null;

            Dictionary<int, TemplateInfo> templateInfoDictionary = TemplateManager.GetTemplateInfoDictionaryByPublishmentSystemID(publishmentSystemID);
            if (templateInfoDictionary != null)
            {
                foreach (TemplateInfo templateInfo in templateInfoDictionary.Values)
                {
                    if (templateInfo.TemplateType == templateType && templateInfo.IsDefault)
                    {
                        info = templateInfo;
                        break;
                    }
                }
            }

            return info;
        }

        public static int GetDefaultTemplateID(int publishmentSystemID, ETemplateType templateType)
        {
            int id = 0;

            Dictionary<int, TemplateInfo> templateInfoDictionary = TemplateManager.GetTemplateInfoDictionaryByPublishmentSystemID(publishmentSystemID);
            if (templateInfoDictionary != null)
            {
                foreach (TemplateInfo templateInfo in templateInfoDictionary.Values)
                {
                    if (templateInfo.TemplateType == templateType && templateInfo.IsDefault)
                    {
                        id = templateInfo.TemplateID;
                        break;
                    }
                }
            }

            return id;
        }

        public static int GetTemplateIDByTemplateName(int publishmentSystemID, ETemplateType templateType, string templateName)
        {
            int id = 0;

            Dictionary<int, TemplateInfo> templateInfoDictionary = TemplateManager.GetTemplateInfoDictionaryByPublishmentSystemID(publishmentSystemID);
            if (templateInfoDictionary != null)
            {
                foreach (TemplateInfo templateInfo in templateInfoDictionary.Values)
                {
                    if (templateInfo.TemplateType == templateType && templateInfo.TemplateName == templateName)
                    {
                        id = templateInfo.TemplateID;
                        break;
                    }
                }
            }

            return id;
        }

        private static Dictionary<int, TemplateInfo> GetTemplateInfoDictionaryByPublishmentSystemID(int publishmentSystemID)
        {
            return GetTemplateInfoDictionaryByPublishmentSystemID(publishmentSystemID, false);
        }

        private static Dictionary<int, TemplateInfo> GetTemplateInfoDictionaryByPublishmentSystemID(int publishmentSystemID, bool flush)
        {
            Dictionary<int, Dictionary<int, TemplateInfo>> dictionary = GetCacheDictionary();

            Dictionary<int, TemplateInfo> templateInfoDictionary = null;

            if (!flush && dictionary.ContainsKey(publishmentSystemID))
            {
                templateInfoDictionary = dictionary[publishmentSystemID];
            }

            if (templateInfoDictionary == null)
            {
                templateInfoDictionary = DataProvider.TemplateDAO.GetTemplateInfoDictionaryByPublishmentSystemID(publishmentSystemID);

                if (templateInfoDictionary != null)
                {
                    UpdateCache(dictionary, templateInfoDictionary, publishmentSystemID);
                }
            }
            return templateInfoDictionary;
        }

        private static void UpdateCache(Dictionary<int, Dictionary<int, TemplateInfo>> dictionary, Dictionary<int, TemplateInfo> templateInfoDictionary, int publishmentSystemID)
        {
            lock (syncRoot)
            {
                dictionary[publishmentSystemID] = templateInfoDictionary;
            }
        }

        public static void RemoveCache(int publishmentSystemID)
        {
            Dictionary<int, Dictionary<int, TemplateInfo>> dictionary = GetCacheDictionary();

            lock (syncRoot)
            {
                dictionary.Remove(publishmentSystemID);
            }
        }

        private static Dictionary<int, Dictionary<int, TemplateInfo>> GetCacheDictionary()
        {
            Dictionary<int, Dictionary<int, TemplateInfo>> dictionary = CacheUtils.Get(cacheKey) as Dictionary<int, Dictionary<int, TemplateInfo>>;
            if (dictionary == null)
            {
                dictionary = new Dictionary<int, Dictionary<int, TemplateInfo>>();
                CacheUtils.Insert(cacheKey, dictionary, null, CacheUtils.DayFactor);
            }
            return dictionary;
        }

        #endregion

        #region 模板相关

        public static string GetTemplateFilePath(PublishmentSystemInfo publishmentSystemInfo, TemplateInfo templateInfo)
		{
			string filePath;
			if (templateInfo.TemplateType == ETemplateType.IndexPageTemplate)
			{
                filePath = PathUtils.Combine(ConfigUtils.Instance.PhysicalApplicationPath, publishmentSystemInfo.PublishmentSystemDir, templateInfo.RelatedFileName);
			}
			else if (templateInfo.TemplateType == ETemplateType.ContentTemplate)
			{
                filePath = PathUtils.Combine(ConfigUtils.Instance.PhysicalApplicationPath, publishmentSystemInfo.PublishmentSystemDir, DirectoryUtils.PublishmentSytem.Template, DirectoryUtils.PublishmentSytem.Content, templateInfo.RelatedFileName);
			}
			else
			{
                filePath = PathUtils.Combine(ConfigUtils.Instance.PhysicalApplicationPath, publishmentSystemInfo.PublishmentSystemDir, DirectoryUtils.PublishmentSytem.Template, templateInfo.RelatedFileName);
			}
			return filePath;
		}

        public static TemplateInfo GetTemplateInfo(int publishmentSystemID, int nodeID, ETemplateType templateType)
        {
            int templateID = 0;
            if (templateType == ETemplateType.IndexPageTemplate)
            {
                templateID = TemplateManager.GetDefaultTemplateID(publishmentSystemID, ETemplateType.IndexPageTemplate);
            }
            else if (templateType == ETemplateType.ChannelTemplate)
            {
                ENodeType nodeType = NodeManager.GetNodeType(publishmentSystemID, nodeID);
                if (nodeType == ENodeType.BackgroundPublishNode)
                {
                    templateID = TemplateManager.GetDefaultTemplateID(publishmentSystemID, ETemplateType.IndexPageTemplate);
                }
                else
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
                    if (nodeInfo != null)
                    {
                        templateID = nodeInfo.ChannelTemplateID;
                    }
                }
            }
            else if (templateType == ETemplateType.ContentTemplate)
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
                if (nodeInfo != null)
                {
                    templateID = nodeInfo.ContentTemplateID;
                }
            }

            TemplateInfo templateInfo = null;
            if (templateID != 0)
            {
                templateInfo = TemplateManager.GetTemplateInfo(publishmentSystemID, templateID);
            }
            if (templateInfo == null)
            {
                templateInfo = TemplateManager.GetDefaultTemplateInfo(publishmentSystemID, templateType);
            }
            return templateInfo;
        }

		public static void WriteContentToTemplateFile(PublishmentSystemInfo publishmentSystemInfo, TemplateInfo templateInfo, string content)
		{
            if (content == null) content = string.Empty;
            string filePath = GetTemplateFilePath(publishmentSystemInfo, templateInfo);
            FileUtils.WriteText(filePath, templateInfo.Charset, content);

            if (templateInfo.TemplateID > 0)
            {
                string userName = BaiRongDataProvider.AdministratorDAO.UserName;
                if (userName == null) userName = string.Empty;
                TemplateLogInfo logInfo = new TemplateLogInfo(0, templateInfo.TemplateID, templateInfo.PublishmentSystemID, DateTime.Now, userName, content.Length, content);
                DataProvider.TemplateLogDAO.Insert(logInfo);
            }
        }

        #endregion

        #region 模版匹配

        public static void UpdateChannelTemplateID(int publishmentSystemID, int nodeID, int channelTemplateID)
        {
            DataProvider.NodeDAO.UpdateChannelTemplateID(nodeID, channelTemplateID);
        }

        public static void UpdateContentTemplateID(int publishmentSystemID, int nodeID, int contentTemplateID)
        {
            DataProvider.NodeDAO.UpdateContentTemplateID(nodeID, contentTemplateID);
        }

        public static int GetIndexTempalteID(int publishmentSystemID)
        {
            return TemplateManager.GetDefaultTemplateID(publishmentSystemID, ETemplateType.IndexPageTemplate);
        }

        public static int GetChannelTempalteID(int publishmentSystemID, int nodeID)
        {
            int templateID = 0;

            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
            if (nodeInfo != null)
            {
                templateID = nodeInfo.ChannelTemplateID;
            }

            if (templateID == 0)
            {
                templateID = TemplateManager.GetDefaultTemplateID(publishmentSystemID, ETemplateType.ChannelTemplate);
            }

            return templateID;
        }

        public static int GetContentTempalteID(int publishmentSystemID, int nodeID)
        {
            int templateID = 0;

            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
            if (nodeInfo != null)
            {
                templateID = nodeInfo.ContentTemplateID;
            }

            if (templateID == 0)
            {
                templateID = TemplateManager.GetDefaultTemplateID(publishmentSystemID, ETemplateType.ContentTemplate);
            }

            return templateID;
        }

        #endregion
    }
}
