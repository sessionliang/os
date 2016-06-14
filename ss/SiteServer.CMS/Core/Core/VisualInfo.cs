using System.Web;
using BaiRong.Core;
using SiteServer.CMS.Model;
using BaiRong.Model;
using System.Collections;
using System;
using System.Collections.Specialized;

namespace SiteServer.CMS.Core
{
	public class VisualInfo
	{
		private int publishmentSystemID;
        private int channelID;
        private int contentID;
        private int fileTemplateID;
        private int pageIndex;
        private ETemplateType templateType;
        private bool isPreview;
        private bool isDesign;
        private string includeUrl;
        private string filePath;

		private VisualInfo()
		{
            this.publishmentSystemID = this.channelID = this.contentID = this.fileTemplateID = this.pageIndex = 0;
            this.templateType = ETemplateType.IndexPageTemplate;
            this.isPreview = false;
            this.isDesign = false;
            this.includeUrl = string.Empty;
            this.filePath = string.Empty;
		}

        public static VisualInfo GetInstance()
        {
            VisualInfo visualInfo = new VisualInfo();

            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["s"]))
            {
                visualInfo.publishmentSystemID = TranslateUtils.ToInt(HttpContext.Current.Request.QueryString["s"]);
            }
            if (visualInfo.publishmentSystemID == 0)
            {
                visualInfo.publishmentSystemID = PathUtility.GetCurrentPublishmentSystemID();
            }
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["n"]))
            {
                visualInfo.channelID = TranslateUtils.ToInt(HttpContext.Current.Request.QueryString["n"]);
            }
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["c"]))
            {
                visualInfo.contentID = TranslateUtils.ToInt(HttpContext.Current.Request.QueryString["c"]);
            }
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["f"]))
            {
                visualInfo.fileTemplateID = TranslateUtils.ToInt(HttpContext.Current.Request.QueryString["f"]);
            }
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["p"]))
            {
                visualInfo.pageIndex = TranslateUtils.ToInt(HttpContext.Current.Request.QueryString["p"]);
            }
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["isPreview"]))
            {
                visualInfo.isPreview = TranslateUtils.ToBool(HttpContext.Current.Request.QueryString["isPreview"]);
            }
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["isDesign"]))
            {
                visualInfo.isDesign = TranslateUtils.ToBool(HttpContext.Current.Request.QueryString["isDesign"]);
                visualInfo.includeUrl = HttpContext.Current.Request.QueryString["includeUrl"];
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["designMode"]))
                {
                    EDesignMode designMode = EDesignModeUtils.GetEnumType(HttpContext.Current.Request.QueryString["designMode"]);
                    PublishmentSystemInfo psInfo = PublishmentSystemManager.GetPublishmentSystemInfo(visualInfo.publishmentSystemID);
                    if (psInfo.Additional.DesignMode != designMode)
                    {
                        psInfo.Additional.DesignMode = designMode;
                        DataProvider.PublishmentSystemDAO.Update(psInfo);
                    }
                }
            }

            //if (visualInfo.channelID == 0)
            //{
            //    string path = HttpContext.Current.Request.Path;
            //    if (visualType == EVisualType.Dynamic)
            //    {
            //        path = path.Substring(0, path.LastIndexOf("_v.aspx"));
            //    }

            //    if (StringUtils.Contains(path, PathUtility.ChannelFilePathRules.DefaultDirectoryName))
            //    {
            //        visualInfo.channelID = TranslateUtils.ToInt(RegexUtils.GetContent("channelID", PathUtility.ChannelFilePathRules.DefaultRegexString, path));
            //        visualInfo.pageIndex = TranslateUtils.ToInt(RegexUtils.GetContent("pageIndex", PathUtility.ChannelFilePathRules.DefaultRegexString, path));
            //        if (visualInfo.pageIndex > 0)
            //        {
            //            visualInfo.pageIndex--;
            //        }
            //    }
            //    else if (StringUtils.Contains(path, PathUtility.ContentFilePathRules.DefaultDirectoryName))
            //    {
            //        visualInfo.channelID = TranslateUtils.ToInt(RegexUtils.GetContent("channelID", PathUtility.ContentFilePathRules.DefaultRegexString, path));
            //        visualInfo.contentID = TranslateUtils.ToInt(RegexUtils.GetContent("contentID", PathUtility.ContentFilePathRules.DefaultRegexString, path));
            //        visualInfo.pageIndex = TranslateUtils.ToInt(RegexUtils.GetContent("pageIndex", PathUtility.ContentFilePathRules.DefaultRegexString, path));
            //        if (visualInfo.pageIndex > 0)
            //        {
            //            visualInfo.pageIndex--;
            //        }
            //    }
            //    else
            //    {
            //        visualInfo.channelID = visualInfo.publishmentSystemID;
            //    }
            //}

            if (visualInfo.channelID > 0)
            {
                visualInfo.templateType = ETemplateType.ChannelTemplate;
            }
            if (visualInfo.contentID > 0 || visualInfo.isPreview)
            {
                visualInfo.templateType = ETemplateType.ContentTemplate;
            }
            if (visualInfo.fileTemplateID > 0)
            {
                visualInfo.templateType = ETemplateType.FileTemplate;
            }

            if (visualInfo.channelID == 0)
            {
                visualInfo.channelID = visualInfo.publishmentSystemID;
            }

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(visualInfo.publishmentSystemID);
            if (visualInfo.templateType == ETemplateType.IndexPageTemplate)
            {
                TemplateInfo templateInfo = TemplateManager.GetTemplateInfo(visualInfo.publishmentSystemID, 0, ETemplateType.IndexPageTemplate);
                bool isHeadquarters = publishmentSystemInfo.IsHeadquarters;
                visualInfo.filePath = PathUtility.GetIndexPageFilePath(publishmentSystemInfo, templateInfo.CreatedFileFullName, isHeadquarters);
            }
            else if (visualInfo.templateType == ETemplateType.ChannelTemplate)
            {
                visualInfo.filePath = PathUtility.GetChannelPageFilePath(publishmentSystemInfo, visualInfo.channelID, visualInfo.pageIndex);
            }
            else if (visualInfo.templateType == ETemplateType.ContentTemplate)
            {
                visualInfo.filePath = PathUtility.GetContentPageFilePath(publishmentSystemInfo, visualInfo.channelID, visualInfo.contentID, visualInfo.pageIndex);
            }
            else if (visualInfo.templateType == ETemplateType.FileTemplate)
            {
                TemplateInfo templateInfo = TemplateManager.GetTemplateInfo(visualInfo.publishmentSystemID, visualInfo.fileTemplateID);
                visualInfo.filePath = PathUtility.MapPath(publishmentSystemInfo, templateInfo.CreatedFileFullName);
            }

            if (visualInfo.isPreview)
            {
                int previewContentID = 0;//添加界面预览
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["previewID"]))
                {
                    previewContentID = TranslateUtils.ToInt(HttpContext.Current.Request.QueryString["previewID"]);//编辑界面预览
                }

                if (HttpContext.Current.Request.Form != null && HttpContext.Current.Request.Form.Count > 0)
                {
                    visualInfo.contentID = AddPreviewContent(visualInfo.publishmentSystemID, visualInfo.ChannelID, previewContentID);
                }
                else
                {
                    visualInfo.contentID = previewContentID;
                }
            }

            return visualInfo;
        }

		public int PublishmentSystemID
		{
			get{ return publishmentSystemID; }
		}

        public int ChannelID
		{
            get { return channelID; }
		}

        public int ContentID
		{
            get { return contentID; }
		}

        public int FileTemplateID
        {
            get { return fileTemplateID; }
        }

        public ETemplateType TemplateType
        {
            get { return templateType; }
        }

        public bool IsPreview
        {
            get { return isPreview; }
        }

        public bool IsDesign
        {
            get { return isDesign; }
        }

        public string IncludeUrl
        {
            get { return includeUrl; }
        }

        public string FilePath
        {
            get { return filePath; }
        }

        public int PageIndex
        {
            get { return pageIndex; }
        }

        public void RemovePreviewContent()
        {
            if (this.isPreview)
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                ArrayList contentIDArrayList = new ArrayList();
                contentIDArrayList.Add(this.contentID);
                string tableName = NodeManager.GetTableName(publishmentSystemInfo, this.channelID);
                DataProvider.ContentDAO.DeleteContents(this.publishmentSystemID, tableName, contentIDArrayList, this.channelID);
            }
        }

        public static void RemovePreviewContent(PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo)
        {
            if (nodeInfo.Additional.IsPreviewContentToDelete)
            {
                string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo.NodeID);
                DataProvider.ContentDAO.DeleteContentsByPreview(publishmentSystemInfo.PublishmentSystemID, tableName, nodeInfo.NodeID);
            }
        }

        private static int AddPreviewContent(int publishmentSystemID, int channelID, int previewContentID)
        {
            int contentID = 0;
            try
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, channelID);
                string tableName = NodeManager.GetTableName(publishmentSystemInfo, channelID);

                ContentInfo contentInfo = new ContentInfo();
                if (previewContentID > 0)
                {
                    contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, previewContentID);
                }
                contentInfo.PublishmentSystemID = publishmentSystemID;
                contentInfo.NodeID = channelID;
                contentInfo.SourceID = SourceManager.Preview;

                ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(publishmentSystemID, channelID);

                InputTypeParser.AddValuesToAttributes(tableStyle, tableName, publishmentSystemInfo, relatedIdentities, HttpContext.Current.Request.Form, contentInfo.Attributes);

                contentInfo.LastEditDate = DateTime.Now;
                contentInfo.AddDate = TranslateUtils.ToDateTime(HttpContext.Current.Request.Form[ContentAttribute.AddDate]);

                contentID = DataProvider.ContentDAO.Insert(tableName, publishmentSystemInfo, contentInfo);
                //设置栏目属性，需要删除预览内容
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, channelID);
                if (nodeInfo != null)
                {
                    nodeInfo.Additional.IsPreviewContentToDelete = true;
                    DataProvider.NodeDAO.UpdateNodeInfo(nodeInfo);
                }

                if (publishmentSystemInfo.Additional.IsRelatedByTags)
                {
                    if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form[ContentAttribute.Tags]))
                    {
                        StringCollection tagCollection = TagUtils.ParseTagsString(HttpContext.Current.Request.Form[ContentAttribute.Tags]);
                        TagUtils.AddTags(tagCollection, AppManager.CMS.AppID, publishmentSystemID, contentID);
                    }
                }
            }
            catch { }

            return contentID;
        }
	}
}
