using System;
using System.Collections;
using System.Text;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.STL.Parser;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser.StlElement;
using SiteServer.STL.Parser.StlEntity;
using SiteServer.CMS.Model;
using SiteServer.CMS.BackgroundPages;
using BaiRong.Core.Data.Provider;
using System.IO;
using SiteServer.CMS.Core;
using SiteServer.CMS.Services;
using System.Collections.Generic;
using BaiRong.Model.Service;
using System.Text.RegularExpressions;

namespace SiteServer.STL.IO
{
    public class FileSystemObject
    {
        public FileSystemObject(int publishmentSystemID)
        {
            this.publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            if (this.publishmentSystemInfo == null)
            {
                throw new ArgumentException(publishmentSystemID + " 不是正确的发布系统ID！");
            }
            this.publishmentSystemID = this.publishmentSystemInfo.PublishmentSystemID;
            this.publishmentSystemDir = this.publishmentSystemInfo.PublishmentSystemDir;
            this.publishmentSystemPath = PathUtils.Combine(ConfigUtils.Instance.PhysicalApplicationPath, this.publishmentSystemInfo.PublishmentSystemDir);
            this.isHeadquarters = this.publishmentSystemInfo.IsHeadquarters;
            DirectoryUtils.CreateDirectoryIfNotExists(this.PublishmentSystemPath);
        }


        private readonly PublishmentSystemInfo publishmentSystemInfo;
        private readonly int publishmentSystemID;
        private readonly string publishmentSystemDir;
        private readonly string publishmentSystemPath;
        private readonly bool isHeadquarters;

        public PublishmentSystemInfo PublishmentSystemInfo
        {
            get { return this.publishmentSystemInfo; }
        }

        public int PublishmentSystemID
        {
            get { return this.publishmentSystemID; }
        }

        public string PublishmentSystemDir
        {
            get { return this.publishmentSystemDir; }
        }

        public string PublishmentSystemPath
        {
            get { return this.publishmentSystemPath; }
        }

        public bool IsHeadquarters
        {
            get { return this.isHeadquarters; }
        }

        public void CreateImmediately(EChangedType changedType, ETemplateType templateType, int channelID, int contentID, int fileTemplateID)
        {
            try
            {
                if (templateType == ETemplateType.ContentTemplate)//内容改变
                {
                    //生成内容页
                    if (contentID != 0)
                    {
                        if (changedType == EChangedType.Add)
                        {
                            if (this.publishmentSystemInfo.Additional.IsCreateRedirectPage)
                            {
                                this.AddContentToWaitingCreate(channelID, contentID);
                            }
                            else
                            {
                                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, channelID);
                                ETableStyle tableStyle = NodeManager.GetTableStyle(this.publishmentSystemInfo, nodeInfo);
                                string tableName = NodeManager.GetTableName(this.publishmentSystemInfo, nodeInfo);
                                this.CreateContent(tableStyle, tableName, channelID, contentID);
                            }
                        }
                        else if (changedType == EChangedType.Edit)
                        {
                            if (this.PublishmentSystemInfo.Additional.IsCreateContentIfContentChanged)
                            {
                                if (this.publishmentSystemInfo.Additional.IsCreateRedirectPage)
                                {
                                    this.AddContentToWaitingCreate(channelID, contentID);
                                }
                                else
                                {
                                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, channelID);
                                    ETableStyle tableStyle = NodeManager.GetTableStyle(this.publishmentSystemInfo, nodeInfo);
                                    string tableName = NodeManager.GetTableName(this.publishmentSystemInfo, nodeInfo);
                                    this.CreateContent(tableStyle, tableName, channelID, contentID);
                                }
                            }
                        }
                        else if (changedType == EChangedType.Check)
                        {
                            if (this.PublishmentSystemInfo.Additional.IsCreateContentIfContentChanged)
                            {
                                if (this.publishmentSystemInfo.Additional.IsCreateRedirectPage)
                                {
                                    this.AddContentToWaitingCreate(channelID, contentID);
                                }
                                else
                                {
                                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, channelID);
                                    ETableStyle tableStyle = NodeManager.GetTableStyle(this.publishmentSystemInfo, nodeInfo);
                                    string tableName = NodeManager.GetTableName(this.publishmentSystemInfo, nodeInfo);
                                    this.CreateContent(tableStyle, tableName, channelID, contentID);
                                }
                            }
                        }
                        else if (changedType == EChangedType.Taxis)
                        {
                            if (this.PublishmentSystemInfo.Additional.IsCreateContentIfContentChanged)
                            {
                                if (this.publishmentSystemInfo.Additional.IsCreateRedirectPage)
                                {
                                    this.AddContentToWaitingCreate(channelID, contentID);
                                }
                                else
                                {
                                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, channelID);
                                    ETableStyle tableStyle = NodeManager.GetTableStyle(this.publishmentSystemInfo, nodeInfo);
                                    string tableName = NodeManager.GetTableName(this.publishmentSystemInfo, nodeInfo);
                                    this.CreateContent(tableStyle, tableName, channelID, contentID);
                                }
                            }
                        }
                    }
                    if (channelID != 0)
                    {
                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(this.PublishmentSystemID, channelID);
                        List<int> nodeIDList = TranslateUtils.StringCollectionToIntList(nodeInfo.Additional.CreateChannelIDsIfContentChanged);
                        if (nodeInfo.Additional.IsCreateChannelIfContentChanged && !nodeIDList.Contains(channelID))
                        {
                            nodeIDList.Add(channelID);
                        }
                        if (this.publishmentSystemInfo.Additional.IsCreateRedirectPage)
                        {
                            foreach (int theNodeID in nodeIDList)
                            {
                                this.AddChannelToWaitingCreate(theNodeID);
                            }
                        }
                        else
                        {
                            foreach (int theNodeID in nodeIDList)
                            {
                                this.CreateChannel(theNodeID);
                            }
                        }

                        List<string> includeFileList = TranslateUtils.StringCollectionToStringList(nodeInfo.Additional.CreateIncludeFilesIfContentChanged);
                        foreach (string includeFile in includeFileList)
                        {
                            this.CreateIncludeFile(includeFile, true);
                        }
                    }
                }
                else if (templateType == ETemplateType.ChannelTemplate || templateType == ETemplateType.IndexPageTemplate)//栏目改变
                {
                    if (channelID != 0)
                    {
                        if (changedType == EChangedType.Add)
                        {
                            if (this.publishmentSystemInfo.Additional.IsCreateRedirectPage)
                            {
                                this.AddChannelToWaitingCreate(channelID);
                            }
                            else
                            {
                                this.CreateChannel(channelID);
                            }
                        }
                        else if (changedType == EChangedType.Edit)
                        {
                            if (this.PublishmentSystemInfo.Additional.IsCreateChannelIfChannelChanged)
                            {
                                if (this.publishmentSystemInfo.Additional.IsCreateRedirectPage)
                                {
                                    this.AddChannelToWaitingCreate(channelID);
                                }
                                else
                                {
                                    this.CreateChannel(channelID);
                                }
                            }
                        }
                    }
                }
                else if (templateType == ETemplateType.FileTemplate)//文件改变
                {
                    this.CreateFile(fileTemplateID);
                }
            }
            catch (Exception ex)
            {
                LogUtils.AddErrorLog(ex);
            }
        }

        //public void CreateRedirectIndex()
        //{
        //    TemplateInfo templateInfo = TemplateManager.GetDefaultTemplateInfo(this.PublishmentSystemID, ETemplateType.IndexPageTemplate);

        //    if (templateInfo != null)
        //    {
        //        string filePath = PathUtility.GetIndexPageFilePath(this.publishmentSystemInfo, templateInfo.CreatedFileFullName, this.IsHeadquarters);

        //        string content = StringUtility.GetRedirectPageHtml(PageUtility.ServiceSTL.Utils.GetStlTriggerUrl(this.PublishmentSystemID, 0, 0));
        //        this.GenerateFile(filePath, templateInfo.Charset, content);
        //    }
        //}

        public void CreateAll()
        {
            this.CreateIndex();
            ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(this.publishmentSystemID);
            foreach (int nodeID in nodeIDArrayList)
            {
                this.CreateChannel(nodeID);
                this.CreateContents(nodeID);
            }
            ArrayList templateIDArrayList = DataProvider.TemplateDAO.GetTemplateIDArrayListByType(this.publishmentSystemID, ETemplateType.FileTemplate);
            foreach (int templateID in templateIDArrayList)
            {
                this.CreateFile(templateID);
            }
        }

        public void CreateIndex()
        {
            TemplateInfo templateInfo = TemplateManager.GetTemplateInfo(this.PublishmentSystemID, 0, ETemplateType.IndexPageTemplate);
            if (templateInfo != null)
            {
                PageInfo pageInfo = new PageInfo(templateInfo, this.PublishmentSystemID, this.PublishmentSystemID, 0, this.publishmentSystemInfo, this.publishmentSystemInfo.Additional.VisualType);
                ContextInfo contextInfo = new ContextInfo(pageInfo);
                string filePath = PathUtility.GetIndexPageFilePath(this.publishmentSystemInfo, templateInfo.CreatedFileFullName, this.IsHeadquarters);

                //if (publishmentSystemInfo.Additional.VisualType == EVisualType.Dynamic)
                //{
                //    string pageUrl = PageUtility.GetIndexPageUrl(publishmentSystemInfo, EVisualType.Dynamic);
                //    string content = StringUtility.GetRedirectPageHtml(pageUrl);
                //    this.GenerateFile(filePath, pageInfo.TemplateInfo.Charset, content);
                //    return;
                //}

                StringBuilder contentBuilder = new StringBuilder(CreateCacheManager.FileContent.GetTemplateContent(this.publishmentSystemInfo, templateInfo));
                this.GeneratePage(filePath, contentBuilder, pageInfo, contextInfo);//生成页面
            }
        }

        //public void CreateRedirectChannel(int nodeID)
        //{
        //    if (nodeID == this.publishmentSystemID)
        //    {
        //        this.CreateRedirectIndex();
        //        return;
        //    }
        //    string filePath = PathUtility.GetChannelPageFilePath(this.publishmentSystemInfo, nodeID, 0);

        //    string content = StringUtility.GetRedirectPageHtml(PageUtility.ServiceSTL.Utils.GetStlTriggerUrl(this.PublishmentSystemID, nodeID, 0));

        //    this.GenerateFile(filePath, ECharsetUtils.GetEnumType(this.publishmentSystemInfo.Additional.Charset), content);

        //}

        public void CreateChannel(int nodeID)
        {
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(this.PublishmentSystemID, nodeID);
            if (nodeInfo == null) return;

            if (nodeID == this.publishmentSystemID)
            {
                this.CreateIndex();
                return;
            }
            else
            {
                if (!string.IsNullOrEmpty(nodeInfo.LinkUrl))
                {
                    return;
                }
                else if (!ELinkTypeUtils.IsCreatable(nodeInfo))
                {
                    return;
                }
            }

            TemplateInfo templateInfo = TemplateManager.GetTemplateInfo(this.PublishmentSystemID, nodeID, ETemplateType.ChannelTemplate);

            PageInfo pageInfo = new PageInfo(templateInfo, this.PublishmentSystemID, nodeID, 0, this.publishmentSystemInfo, this.publishmentSystemInfo.Additional.VisualType);
            ContextInfo contextInfo = new ContextInfo(pageInfo);

            string filePath = PathUtility.GetChannelPageFilePath(this.publishmentSystemInfo, pageInfo.PageNodeID, 0);

            //if (publishmentSystemInfo.Additional.VisualType == EVisualType.Dynamic)
            //{
            //    string pageUrl = PageUtility.GetChannelUrl(publishmentSystemInfo, nodeInfo, EVisualType.Dynamic);
            //    string content = StringUtility.GetRedirectPageHtml(pageUrl);
            //    this.GenerateFile(filePath, pageInfo.TemplateInfo.Charset, content);
            //    return;
            //}

            StringBuilder contentBuilder = new StringBuilder(CreateCacheManager.FileContent.GetTemplateContent(this.publishmentSystemInfo, templateInfo));
            List<string> stlLabelList = StlParserUtility.GetStlLabelList(contentBuilder.ToString());

            //如果标签中存在<stl:channel type="PageContent"></stl:channel>
            string stlPageContentElement = string.Empty;

            foreach (string label in stlLabelList)
            {
                if (StlParserUtility.IsStlChannelElement(label, NodeAttribute.PageContent))
                {
                    stlPageContentElement = label;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(stlPageContentElement))//内容存在
            {
                StringBuilder innerBuilder = new StringBuilder(stlPageContentElement);
                StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                string contentAttributeHtml = innerBuilder.ToString();
                int pageCount = StringUtils.GetCount(ContentUtility.PagePlaceHolder, contentAttributeHtml) + 1;//一共需要的页数
                if (pageCount > 1)
                {
                    for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                    {
                        PageInfo thePageInfo = new PageInfo(pageInfo.TemplateInfo, pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.PageContentID, pageInfo.PublishmentSystemInfo, pageInfo.VisualType);
                        contextInfo = new ContextInfo(thePageInfo);
                        int index = contentAttributeHtml.IndexOf(ContentUtility.PagePlaceHolder);
                        int length = (index == -1) ? contentAttributeHtml.Length : index;
                        string pagedContentAttributeHtml = contentAttributeHtml.Substring(0, length);
                        StringBuilder pagedBuilder = new StringBuilder(contentBuilder.ToString().Replace(stlPageContentElement, pagedContentAttributeHtml));
                        StlParserManager.ReplacePageElementsInChannelPage(pagedBuilder, thePageInfo, stlLabelList, thePageInfo.PageNodeID, currentPageIndex, pageCount, 0);
                        filePath = PathUtility.GetChannelPageFilePath(this.publishmentSystemInfo, thePageInfo.PageNodeID, currentPageIndex);
                        this.GeneratePage(filePath, pagedBuilder, thePageInfo, contextInfo);//生成页面

                        if (index != -1)
                        {
                            contentAttributeHtml = contentAttributeHtml.Substring(length + ContentUtility.PagePlaceHolder.Length);
                        }
                    }
                    return;
                }
                contentBuilder.Replace(stlPageContentElement, contentAttributeHtml);
            }

            //如果标签中存在<stl:pageContents>
            if (StlParserUtility.IsStlElementExists(StlPageContents.ElementName, stlLabelList))
            {
                string stlElement = StlParserUtility.GetStlElement(StlPageContents.ElementName, stlLabelList);
                string stlPageContentsElement = stlElement;
                string stlPageContentsElementReplaceString = stlElement;

                string sltPageItemsElement = StlParserUtility.GetStlElement(StlPageItems.ElementName, stlLabelList);

                StlPageContents pageContentsElementParser = new StlPageContents(stlPageContentsElement, pageInfo, contextInfo, false);
                int totalNum;
                int pageCount = pageContentsElementParser.GetPageCount(out totalNum);

                for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                {
                    if (this.publishmentSystemInfo.Additional.CreateStaticMaxPage > 0 && currentPageIndex >= this.publishmentSystemInfo.Additional.CreateStaticMaxPage) break;
                    PageInfo thePageInfo = new PageInfo(pageInfo.TemplateInfo, pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.PageContentID, pageInfo.PublishmentSystemInfo, pageInfo.VisualType);
                    string pageHtml = pageContentsElementParser.Parse(totalNum, currentPageIndex, pageCount);
                    StringBuilder pagedBuilder = new StringBuilder(contentBuilder.ToString().Replace(stlPageContentsElementReplaceString, pageHtml));

                    StlParserManager.ReplacePageElementsInChannelPage(pagedBuilder, thePageInfo, stlLabelList, thePageInfo.PageNodeID, currentPageIndex, pageCount, totalNum);

                    filePath = PathUtility.GetChannelPageFilePath(this.publishmentSystemInfo, thePageInfo.PageNodeID, currentPageIndex);
                    thePageInfo.AddLastPageScript(pageInfo);
                    this.GeneratePage(filePath, pagedBuilder, thePageInfo, contextInfo);//生成页面
                    thePageInfo.ClearLastPageScript(pageInfo);
                    pageInfo.ClearLastPageScript();
                }
            }
            //by 20151125 sofuny 增加了分页标签
            //如果标签中存在<stl:pageIPushContents>
            else if (StlParserUtility.IsStlElementExists(StlPageIPushContents.ElementName, stlLabelList))
            {
                string stlElement = StlParserUtility.GetStlElement(StlPageIPushContents.ElementName, stlLabelList);
                string stlPageContentsElement = stlElement;
                string stlPageContentsElementReplaceString = stlElement;

                StlPageIPushContents pageContentsElementParser = new StlPageIPushContents(stlPageContentsElement, pageInfo, contextInfo, false);
                int totalNum;
                int pageCount = pageContentsElementParser.GetPageCount(out totalNum);

                for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                {
                    if (this.publishmentSystemInfo.Additional.CreateStaticMaxPage > 0 && currentPageIndex >= this.publishmentSystemInfo.Additional.CreateStaticMaxPage) break;
                    PageInfo thePageInfo = new PageInfo(pageInfo.TemplateInfo, pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.PageContentID, pageInfo.PublishmentSystemInfo, pageInfo.VisualType);
                    string pageHtml = pageContentsElementParser.Parse(totalNum, currentPageIndex, pageCount);
                    StringBuilder pagedBuilder = new StringBuilder(contentBuilder.ToString().Replace(stlPageContentsElementReplaceString, pageHtml));
                    StlParserManager.ReplacePageElementsInChannelPage(pagedBuilder, thePageInfo, stlLabelList, thePageInfo.PageNodeID, currentPageIndex, pageCount, totalNum);
                    filePath = PathUtility.GetChannelPageFilePath(this.publishmentSystemInfo, thePageInfo.PageNodeID, currentPageIndex);
                    this.GeneratePage(filePath, pagedBuilder, thePageInfo, contextInfo);//生成页面
                }
            }
            //如果标签中存在<stl:pageChannels>
            else if (StlParserUtility.IsStlElementExists(StlPageChannels.ElementName, stlLabelList))
            {
                string stlElement = StlParserUtility.GetStlElement(StlPageChannels.ElementName, stlLabelList);
                string stlPageChannelsElement = stlElement;
                string stlPageChannelsElementReplaceString = stlElement;

                StlPageChannels pageChannelsElementParser = new StlPageChannels(stlPageChannelsElement, pageInfo, contextInfo, false);
                int totalNum;
                int pageCount = pageChannelsElementParser.GetPageCount(out totalNum);

                for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                {
                    PageInfo thePageInfo = new PageInfo(pageInfo.TemplateInfo, pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.PageContentID, pageInfo.PublishmentSystemInfo, pageInfo.VisualType);
                    string pageHtml = pageChannelsElementParser.Parse(currentPageIndex, pageCount);
                    StringBuilder pagedBuilder = new StringBuilder(contentBuilder.ToString().Replace(stlPageChannelsElementReplaceString, pageHtml));

                    StlParserManager.ReplacePageElementsInChannelPage(pagedBuilder, thePageInfo, stlLabelList, thePageInfo.PageNodeID, currentPageIndex, pageCount, totalNum);

                    filePath = PathUtility.GetChannelPageFilePath(this.publishmentSystemInfo, thePageInfo.PageNodeID, currentPageIndex);
                    this.GeneratePage(filePath, pagedBuilder, thePageInfo, contextInfo);//生成页面
                }
            }
            //如果标签中存在<stl:pageSqlContents>
            else if (StlParserUtility.IsStlElementExists(StlPageSqlContents.ElementName, stlLabelList))
            {
                string stlElement = StlParserUtility.GetStlElement(StlPageSqlContents.ElementName, stlLabelList);
                string stlPageSqlContentsElement = stlElement;
                string stlPageSqlContentsElementReplaceString = stlElement;

                StlPageSqlContents pageSqlContentsElementParser = new StlPageSqlContents(stlPageSqlContentsElement, pageInfo, contextInfo, false);
                int totalNum;
                int pageCount = pageSqlContentsElementParser.GetPageCount(out totalNum);

                for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                {
                    PageInfo thePageInfo = new PageInfo(pageInfo.TemplateInfo, pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.PageContentID, pageInfo.PublishmentSystemInfo, pageInfo.VisualType);
                    string pageHtml = pageSqlContentsElementParser.Parse(currentPageIndex, pageCount);
                    StringBuilder pagedBuilder = new StringBuilder(contentBuilder.ToString().Replace(stlPageSqlContentsElementReplaceString, pageHtml));

                    StlParserManager.ReplacePageElementsInChannelPage(pagedBuilder, thePageInfo, stlLabelList, thePageInfo.PageNodeID, currentPageIndex, pageCount, totalNum);

                    filePath = PathUtility.GetChannelPageFilePath(this.publishmentSystemInfo, thePageInfo.PageNodeID, currentPageIndex);
                    this.GeneratePage(filePath, pagedBuilder, thePageInfo, contextInfo);//生成页面
                }
            }

            //如果标签中存在<stl:StlPageInputContents>
            else if (StlParserUtility.IsStlElementExists(StlPageInputContents.ElementName, stlLabelList))
            {
                string stlElement = StlParserUtility.GetStlElement(StlPageInputContents.ElementName, stlLabelList);
                string stlPageInputContentsElement = stlElement;
                string stlPageInputContentsElementReplaceString = stlElement;

                StlPageInputContents pageInputContentsElementParser = new StlPageInputContents(stlPageInputContentsElement, pageInfo, contextInfo, true);
                int totalNum;
                int pageCount = pageInputContentsElementParser.GetPageCount(out totalNum);

                for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                {
                    PageInfo thePageInfo = new PageInfo(pageInfo.TemplateInfo, pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.PageContentID, pageInfo.PublishmentSystemInfo, pageInfo.VisualType);
                    string pageHtml = pageInputContentsElementParser.Parse(currentPageIndex, pageCount);
                    StringBuilder pagedBuilder = new StringBuilder(contentBuilder.ToString().Replace(stlPageInputContentsElementReplaceString, pageHtml));

                    StlParserManager.ReplacePageElementsInChannelPage(pagedBuilder, thePageInfo, stlLabelList, thePageInfo.PageNodeID, currentPageIndex, pageCount, totalNum);

                    filePath = PathUtility.GetChannelPageFilePath(this.publishmentSystemInfo, thePageInfo.PageNodeID, currentPageIndex);
                    this.GeneratePage(filePath, pagedBuilder, thePageInfo, contextInfo);//生成页面
                }
            }
            else if (StlParserUtility.IsStlElementExists(StlPageWebsiteMessageContents.ElementName, stlLabelList))
            {
                string stlElement = StlParserUtility.GetStlElement(StlPageWebsiteMessageContents.ElementName, stlLabelList);
                string stlPageWebsiteMessageContentsElement = stlElement;
                string stlPageWebsiteMessageContentsElementReplaceString = stlElement;

                StlPageWebsiteMessageContents pageWebsiteMessageContentsElementParser = new StlPageWebsiteMessageContents(stlPageWebsiteMessageContentsElement, pageInfo, contextInfo, true);
                int totalNum;
                int pageCount = pageWebsiteMessageContentsElementParser.GetPageCount(out totalNum);

                for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                {
                    PageInfo thePageInfo = new PageInfo(pageInfo.TemplateInfo, pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.PageContentID, pageInfo.PublishmentSystemInfo, pageInfo.VisualType);
                    string pageHtml = pageWebsiteMessageContentsElementParser.Parse(currentPageIndex, pageCount);
                    StringBuilder pagedBuilder = new StringBuilder(contentBuilder.ToString().Replace(stlPageWebsiteMessageContentsElementReplaceString, pageHtml));

                    StlParserManager.ReplacePageElementsInChannelPage(pagedBuilder, thePageInfo, stlLabelList, thePageInfo.PageNodeID, currentPageIndex, pageCount, totalNum);

                    filePath = PathUtility.GetChannelPageFilePath(this.publishmentSystemInfo, thePageInfo.PageNodeID, currentPageIndex);
                    this.GeneratePage(filePath, pagedBuilder, thePageInfo, contextInfo);//生成页面
                }
            }
            else
            {
                this.GeneratePage(filePath, contentBuilder, pageInfo, contextInfo);//生成页面
            }
        }

        //public void CreateRedirectContents(int nodeID)
        //{
        //    string tableName = NodeManager.GetTableName(this.publishmentSystemInfo, nodeID);
        //    string orderByString = ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc);
        //    ArrayList ContentIDArrayList = DataProvider.ContentDAO.GetContentIDArrayListChecked(tableName, nodeID, orderByString);
        //    foreach (int contentID in ContentIDArrayList)
        //    {
        //        this.CreateRedirectContent(nodeID, contentID);
        //    }
        //}

        //public void CreateRedirectContent(int nodeID, int contentID)
        //{
        //    string filePath = PathUtility.GetContentPageFilePath(this.publishmentSystemInfo, nodeID, contentID, 0);

        //    string content = StringUtility.GetRedirectPageHtml(PageUtility.ServiceSTL.Utils.GetStlTriggerUrl(this.PublishmentSystemID, nodeID, contentID));

        //    this.GenerateFile(filePath, ECharsetUtils.GetEnumType(this.publishmentSystemInfo.Additional.Charset), content);
        //}

        public void AddIndexToWaitingCreate()
        {
            string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateImmediately(this.publishmentSystemID, EChangedType.Add, ETemplateType.IndexPageTemplate, this.publishmentSystemID, 0, 0);
            AjaxUrlManager.AddAjaxUrl(ajaxUrl);
        }

        public void AddChannelToWaitingCreate(int nodeID)
        {
            string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateImmediately(this.publishmentSystemID, EChangedType.Add, ETemplateType.ChannelTemplate, nodeID, 0, 0);
            AjaxUrlManager.AddAjaxUrl(ajaxUrl);
        }

        public void AddFileToWaitingCreate(int nodeID, int fileTemplateID)
        {
            string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateImmediately(this.publishmentSystemID, EChangedType.Add, ETemplateType.FileTemplate, 0, 0, fileTemplateID);
            AjaxUrlManager.AddAjaxUrl(ajaxUrl);
        }

        public void AddContentToWaitingCreate(int nodeID, int contentID)
        {
            string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateImmediately(this.publishmentSystemID, EChangedType.Add, ETemplateType.ContentTemplate, nodeID, contentID, 0);
            AjaxUrlManager.AddAjaxUrl(ajaxUrl);
        }

        public void AddContentsToWaitingCreate(int nodeID)
        {
            string tableName = NodeManager.GetTableName(this.publishmentSystemInfo, nodeID);
            string orderByString = ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc);
            ArrayList contentIDArrayList = DataProvider.ContentDAO.GetContentIDArrayListChecked(tableName, nodeID, orderByString);
            foreach (int contentID in contentIDArrayList)
            {
                this.AddContentToWaitingCreate(nodeID, contentID);
            }
        }

        public void CreateContents(int nodeID)
        {
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
            ETableStyle tableStyle = NodeManager.GetTableStyle(this.publishmentSystemInfo, nodeInfo);
            string tableName = NodeManager.GetTableName(this.publishmentSystemInfo, nodeInfo);
            string orderByString = ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc);
            ArrayList ContentIDArrayList = DataProvider.ContentDAO.GetContentIDArrayListChecked(tableName, nodeID, orderByString);
            foreach (int contentID in ContentIDArrayList)
            {
                this.CreateContent(tableStyle, tableName, nodeID, contentID);
            }
        }

        public void CreateContent(ETableStyle tableStyle, string tableName, int nodeID, int contentID)
        {
            ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);
            if (contentInfo == null) return;
            //引用链接，不需要生成内容页；引用内容，需要生成内容页；
            if (contentInfo.ReferenceID > 0 && contentInfo.GetExtendedAttribute(ContentAttribute.TranslateContentType) != ETranslateContentType.ReferenceContent.ToString())
            {
                return;
            }
            if (tableStyle == ETableStyle.BackgroundContent && !string.IsNullOrEmpty(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.LinkUrl)))
            {
                return;
            }
            if (contentInfo.IsChecked == false)
            {
                return;
            }
            //int contentTemplateID = DataProvider.BackgroundNodeDAO.GetContentTemplateID(nodeID);
            //TemplateInfo templateInfo = DataProvider.TemplateDAO.GetTemplateInfo(this.PublishmentSystemID, contentTemplateID);
            //if (templateInfo == null)
            //{
            //    templateInfo = DataProvider.TemplateDAO.GetDefaultTemplateInfo(this.PublishmentSystemID, ETemplateType.ContentTemplate);
            //}
            TemplateInfo templateInfo = TemplateManager.GetTemplateInfo(this.PublishmentSystemID, nodeID, ETemplateType.ContentTemplate);

            PageInfo pageInfo = new PageInfo(templateInfo, this.PublishmentSystemID, nodeID, contentID, this.publishmentSystemInfo, this.publishmentSystemInfo.Additional.VisualType);
            ContextInfo contextInfo = new ContextInfo(pageInfo);
            contextInfo.ContentInfo = contentInfo;

            string filePath = PathUtility.GetContentPageFilePath(this.publishmentSystemInfo, pageInfo.PageNodeID, contentInfo, 0);

            //if (publishmentSystemInfo.Additional.VisualType == EVisualType.Dynamic)
            //{
            //    string pageUrl = PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo, EVisualType.Dynamic);
            //    string content = StringUtility.GetRedirectPageHtml(pageUrl);
            //    this.GenerateFile(filePath, pageInfo.TemplateInfo.Charset, content);
            //    return;
            //}

            StringBuilder contentBuilder = new StringBuilder(CreateCacheManager.FileContent.GetTemplateContent(this.publishmentSystemInfo, templateInfo));

            List<string> stlLabelList = StlParserUtility.GetStlLabelList(contentBuilder.ToString());

            //如果标签中存在<stl:content type="PageContent"></stl:content>
            string stlPageContentElement = string.Empty;

            foreach (string label in stlLabelList)
            {
                if (StlParserUtility.IsStlContentElement(label, BackgroundContentAttribute.PageContent))
                {
                    stlPageContentElement = label;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(stlPageContentElement))//内容存在
            {
                StringBuilder innerBuilder = new StringBuilder(stlPageContentElement);
                StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                string contentAttributeHtml = innerBuilder.ToString();
                int pageCount = StringUtils.GetCount(ContentUtility.PagePlaceHolder, contentAttributeHtml) + 1;//一共需要的页数
                if (pageCount > 1)
                {
                    for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                    {
                        PageInfo thePageInfo = new PageInfo(pageInfo.TemplateInfo, pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.PageContentID, pageInfo.PublishmentSystemInfo, pageInfo.VisualType);
                        contextInfo = new ContextInfo(thePageInfo);
                        contextInfo.ContentInfo = contentInfo;

                        int index = contentAttributeHtml.IndexOf(ContentUtility.PagePlaceHolder);
                        int length = (index == -1) ? contentAttributeHtml.Length : index;
                        string pagedContentAttributeHtml = contentAttributeHtml.Substring(0, length);
                        StringBuilder pagedBuilder = new StringBuilder(contentBuilder.ToString().Replace(stlPageContentElement, pagedContentAttributeHtml));
                        StlParserManager.ReplacePageElementsInContentPage(pagedBuilder, thePageInfo, stlLabelList, nodeID, contentID, currentPageIndex, pageCount);

                        filePath = PathUtility.GetContentPageFilePath(this.publishmentSystemInfo, thePageInfo.PageNodeID, contentInfo, currentPageIndex);
                        this.GeneratePage(filePath, pagedBuilder, thePageInfo, contextInfo);//生成页面

                        if (index != -1)
                        {
                            contentAttributeHtml = contentAttributeHtml.Substring(length + ContentUtility.PagePlaceHolder.Length);
                        }
                    }
                    return;
                }
                contentBuilder.Replace(stlPageContentElement, contentAttributeHtml);
            }
            //by 20151125 sofuny 增加了分页标签
            //如果标签中存在<stl:pageIPushContents>
            else if (StlParserUtility.IsStlElementExists(StlPageIPushContents.ElementName, stlLabelList))
            {
                string stlElement = StlParserUtility.GetStlElement(StlPageIPushContents.ElementName, stlLabelList);
                string stlPageContentsElement = stlElement;
                string stlPageContentsElementReplaceString = stlElement;

                StlPageIPushContents pageContentsElementParser = new StlPageIPushContents(stlPageContentsElement, pageInfo, contextInfo, false);
                int totalNum;
                int pageCount = pageContentsElementParser.GetPageCount(out totalNum);

                for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                {
                    PageInfo thePageInfo = new PageInfo(pageInfo.TemplateInfo, pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.PageContentID, pageInfo.PublishmentSystemInfo, pageInfo.VisualType);
                    string pageHtml = pageContentsElementParser.Parse(totalNum, currentPageIndex, pageCount);
                    StringBuilder pagedBuilder = new StringBuilder(contentBuilder.ToString().Replace(stlPageContentsElementReplaceString, pageHtml));

                    StlParserManager.ReplacePageElementsInContentPage(pagedBuilder, thePageInfo, stlLabelList, nodeID, contentID, currentPageIndex, pageCount);

                    filePath = PathUtility.GetContentPageFilePath(this.publishmentSystemInfo, thePageInfo.PageNodeID, contentInfo, currentPageIndex);
                    this.GeneratePage(filePath, pagedBuilder, thePageInfo, contextInfo);//生成页面
                }
            }
            //如果标签中存在<stl:pageContents>
            if (StlParserUtility.IsStlElementExists(StlPageContents.ElementName, stlLabelList))
            {
                string stlElement = StlParserUtility.GetStlElement(StlPageContents.ElementName, stlLabelList);
                string stlPageContentsElement = stlElement;
                string stlPageContentsElementReplaceString = stlElement;

                StlPageContents pageContentsElementParser = new StlPageContents(stlPageContentsElement, pageInfo, contextInfo, false);
                int totalNum;
                int pageCount = pageContentsElementParser.GetPageCount(out totalNum);

                for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                {
                    PageInfo thePageInfo = new PageInfo(pageInfo.TemplateInfo, pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.PageContentID, pageInfo.PublishmentSystemInfo, pageInfo.VisualType);
                    string pageHtml = pageContentsElementParser.Parse(totalNum, currentPageIndex, pageCount);
                    StringBuilder pagedBuilder = new StringBuilder(contentBuilder.ToString().Replace(stlPageContentsElementReplaceString, pageHtml));

                    StlParserManager.ReplacePageElementsInContentPage(pagedBuilder, thePageInfo, stlLabelList, nodeID, contentID, currentPageIndex, pageCount);

                    filePath = PathUtility.GetContentPageFilePath(this.publishmentSystemInfo, thePageInfo.PageNodeID, contentInfo, currentPageIndex);
                    this.GeneratePage(filePath, pagedBuilder, thePageInfo, contextInfo);//生成页面
                }
            }
            //by 20151125 sofuny 增加了分页标签
            //如果标签中存在<stl:pageIPushContents>
            else if (StlParserUtility.IsStlElementExists(StlPageIPushContents.ElementName, stlLabelList))
            {
                string stlElement = StlParserUtility.GetStlElement(StlPageIPushContents.ElementName, stlLabelList);
                string stlPageContentsElement = stlElement;
                string stlPageContentsElementReplaceString = stlElement;

                StlPageIPushContents pageContentsElementParser = new StlPageIPushContents(stlPageContentsElement, pageInfo, contextInfo, false);
                int totalNum;
                int pageCount = pageContentsElementParser.GetPageCount(out totalNum);

                for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                {
                    PageInfo thePageInfo = new PageInfo(pageInfo.TemplateInfo, pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.PageContentID, pageInfo.PublishmentSystemInfo, pageInfo.VisualType);
                    string pageHtml = pageContentsElementParser.Parse(totalNum, currentPageIndex, pageCount);
                    StringBuilder pagedBuilder = new StringBuilder(contentBuilder.ToString().Replace(stlPageContentsElementReplaceString, pageHtml));

                    StlParserManager.ReplacePageElementsInContentPage(pagedBuilder, thePageInfo, stlLabelList, nodeID, contentID, currentPageIndex, pageCount);

                    filePath = PathUtility.GetContentPageFilePath(this.publishmentSystemInfo, thePageInfo.PageNodeID, contentInfo, currentPageIndex);
                    this.GeneratePage(filePath, pagedBuilder, thePageInfo, contextInfo);//生成页面
                }
            }
            //如果标签中存在<stl:pageChannels>
            else if (StlParserUtility.IsStlElementExists(StlPageChannels.ElementName, stlLabelList))
            {
                string stlElement = StlParserUtility.GetStlElement(StlPageChannels.ElementName, stlLabelList);
                string stlPageChannelsElement = stlElement;
                string stlPageChannelsElementReplaceString = stlElement;

                StlPageChannels pageChannelsElementParser = new StlPageChannels(stlPageChannelsElement, pageInfo, contextInfo, false);
                int totalNum;
                int pageCount = pageChannelsElementParser.GetPageCount(out totalNum);

                for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                {
                    PageInfo thePageInfo = new PageInfo(pageInfo.TemplateInfo, pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.PageContentID, pageInfo.PublishmentSystemInfo, pageInfo.VisualType);
                    string pageHtml = pageChannelsElementParser.Parse(currentPageIndex, pageCount);
                    StringBuilder pagedBuilder = new StringBuilder(contentBuilder.ToString().Replace(stlPageChannelsElementReplaceString, pageHtml));

                    StlParserManager.ReplacePageElementsInContentPage(pagedBuilder, thePageInfo, stlLabelList, nodeID, contentID, currentPageIndex, pageCount);

                    filePath = PathUtility.GetContentPageFilePath(this.publishmentSystemInfo, thePageInfo.PageNodeID, contentInfo, currentPageIndex);
                    this.GeneratePage(filePath, pagedBuilder, thePageInfo, contextInfo);//生成页面
                }
            }
            //如果标签中存在<stl:pageSqlContents>
            else if (StlParserUtility.IsStlElementExists(StlPageSqlContents.ElementName, stlLabelList))
            {
                string stlElement = StlParserUtility.GetStlElement(StlPageSqlContents.ElementName, stlLabelList);
                string stlPageSqlContentsElement = stlElement;
                string stlPageSqlContentsElementReplaceString = stlElement;

                StlPageSqlContents pageSqlContentsElementParser = new StlPageSqlContents(stlPageSqlContentsElement, pageInfo, contextInfo, false);
                int totalNum;
                int pageCount = pageSqlContentsElementParser.GetPageCount(out totalNum);

                for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                {
                    PageInfo thePageInfo = new PageInfo(pageInfo.TemplateInfo, pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.PageContentID, pageInfo.PublishmentSystemInfo, pageInfo.VisualType);
                    string pageHtml = pageSqlContentsElementParser.Parse(currentPageIndex, pageCount);
                    StringBuilder pagedBuilder = new StringBuilder(contentBuilder.ToString().Replace(stlPageSqlContentsElementReplaceString, pageHtml));

                    StlParserManager.ReplacePageElementsInContentPage(pagedBuilder, thePageInfo, stlLabelList, nodeID, contentID, currentPageIndex, pageCount);

                    filePath = PathUtility.GetContentPageFilePath(this.publishmentSystemInfo, thePageInfo.PageNodeID, contentInfo, currentPageIndex);
                    this.GeneratePage(filePath, pagedBuilder, thePageInfo, contextInfo);//生成页面
                }
            }
            else//无翻页
            {
                this.GeneratePage(filePath, contentBuilder, pageInfo, contextInfo);//生成页面
            }
        }

        public void CreateFile(int templateID)
        {
            TemplateInfo templateInfo = TemplateManager.GetTemplateInfo(this.PublishmentSystemID, templateID);
            if (templateInfo == null || templateInfo.TemplateType != ETemplateType.FileTemplate)
            {
                return;
            }
            PageInfo pageInfo = new PageInfo(templateInfo, this.PublishmentSystemID, this.PublishmentSystemID, 0, this.publishmentSystemInfo, this.publishmentSystemInfo.Additional.VisualType);
            ContextInfo contextInfo = new ContextInfo(pageInfo);
            string filePath = PathUtility.MapPath(this.publishmentSystemInfo, templateInfo.CreatedFileFullName);

            //if (publishmentSystemInfo.Additional.VisualType == EVisualType.Dynamic)
            //{
            //    string pageUrl = PageUtility.GetFileUrl(publishmentSystemInfo, templateID, EVisualType.Dynamic);
            //    string content = StringUtility.GetRedirectPageHtml(pageUrl);
            //    this.GenerateFile(filePath, pageInfo.TemplateInfo.Charset, content);
            //    return;
            //}

            StringBuilder contentBuilder = new StringBuilder(CreateCacheManager.FileContent.GetTemplateContent(this.publishmentSystemInfo, templateInfo));
            this.GeneratePage(filePath, contentBuilder, pageInfo, contextInfo);//生成页面
        }

        public string CreateIncludeFile(string virtualUrl, bool isCreateIfExists)
        {
            TemplateInfo templateInfo = new TemplateInfo(0, this.publishmentSystemID, string.Empty, ETemplateType.FileTemplate, string.Empty, string.Empty, string.Empty, ECharsetUtils.GetEnumType(this.publishmentSystemInfo.Additional.Charset), false);
            PageInfo pageInfo = new PageInfo(templateInfo, this.PublishmentSystemID, this.PublishmentSystemID, 0, this.publishmentSystemInfo, EVisualType.Static);
            ContextInfo contextInfo = new ContextInfo(pageInfo);

            string parsedVirtualUrl = virtualUrl.Substring(0, virtualUrl.LastIndexOf('.')) + "_parsed" + virtualUrl.Substring(virtualUrl.LastIndexOf('.'));
            string filePath = PathUtility.MapPath(this.publishmentSystemInfo, parsedVirtualUrl);
            if (isCreateIfExists || !FileUtils.IsFileExists(filePath))
            {
                StringBuilder contentBuilder = new StringBuilder(CreateCacheManager.FileContent.GetIncludeContent(this.publishmentSystemInfo, virtualUrl, pageInfo.TemplateInfo.Charset));
                StlParserManager.ParseTemplateContent(contentBuilder, pageInfo, contextInfo);
                string pageAfterBodyScripts = StlParserManager.GetPageInfoScript(pageInfo, true);
                string pageBeforeBodyScripts = StlParserManager.GetPageInfoScript(pageInfo, false);
                this.GenerateFile(filePath, pageInfo.TemplateInfo.Charset, pageAfterBodyScripts + contentBuilder.ToString() + pageBeforeBodyScripts);
            }
            return parsedVirtualUrl;
        }

        /// <summary>
        /// 在操作系统中创建文件，如果文件存在，重新创建此文件
        /// </summary>
        /// <param name="filePath">需要创建文件的绝对路径，必须是完整的路径</param>
        /// <param name="charset">编码</param>
        /// <param name="content">需要创建文件的内容</param>
        public void GenerateFile(string filePath, ECharset charset, string content)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                try
                {
                    FileUtils.WriteText(filePath, charset, content);
                    //此方法职责：
                    //1. 分离部署
                    //2. 子站单独部署
                    //3. 图片，视频，附件单独部署
                    FileUtility.ParseOutServerFiles(this.PublishmentSystemInfo, filePath, content);
                }
                catch
                {
                    FileUtils.RemoveReadOnlyAndHiddenIfExists(filePath);
                    FileUtils.WriteText(filePath, charset, content);
                }
            }
        }

        private void GeneratePage(string filePath, StringBuilder contentBuilder, PageInfo pageInfo, ContextInfo contextInfo)
        {
            StlUtility.LoadGeneratePageContent(this.publishmentSystemInfo, pageInfo, contextInfo, contentBuilder, filePath);
            this.GenerateFile(filePath, pageInfo.TemplateInfo.Charset, contentBuilder.ToString());
        }

        public void CreateChannelsTaskByChildNodeID(int publishmentSystemID, string channelIDCollection)
        {
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            if (publishmentSystemInfo != null)
            {
                try
                {
                    if (publishmentSystemInfo.Additional.IsCreateChannelsByChildNodeID)
                    {
                        ExtendedAttributes serviceParamters = new ExtendedAttributes();
                        TaskCreateInfo taskCreateInfo = new TaskCreateInfo(string.Empty);
                        taskCreateInfo.IsCreateAll = false;
                        taskCreateInfo.ChannelIDCollection = channelIDCollection;
                        taskCreateInfo.CreateTypes = ECreateType.Channel.ToString();
                        taskCreateInfo.IsCreateSiteMap = false;
                        serviceParamters = taskCreateInfo;

                        TaskInfo taskInfo = new TaskInfo(AppManager.CMS.AppID);
                        taskInfo.TaskName = string.Format(@"递归更新父栏目页_{0}", DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString());
                        taskInfo.PublishmentSystemID = publishmentSystemID;
                        taskInfo.ServiceType = EServiceType.Create;
                        taskInfo.FrequencyType = EFrequencyType.OnlyOnce;
                        taskInfo.OnlyOnceDate = DateTime.Now.AddMinutes(publishmentSystemInfo.Additional.CreateScopeByChildNodeID);
                        taskInfo.Description = string.Format(@"递归更新父栏目页"); ;

                        taskInfo.ServiceParameters = serviceParamters.ToString();

                        taskInfo.IsEnabled = true;
                        taskInfo.AddDate = DateTime.Now;
                        taskInfo.LastExecuteDate = DateUtils.SqlMinValue;

                        BaiRongDataProvider.TaskDAO.Insert(taskInfo);
                    }
                }

                catch (Exception ex)
                {
                    LogUtils.AddErrorLog(ex);
                }
            }

            /*
            try
            {  
                ArrayList arrayList = new ArrayList();
                ArrayList ruleIDArrayList = TemplateManager.GetRuleIDArrayList(publishmentSystemID);
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByChildNodeID(publishmentSystemID, childNodeID, arrayList);

                if (nodeIDArrayList.Count > 0)
                {
                    if (publishmentSystemInfo != null)
                    {
                        if (publishmentSystemInfo.Additional.IsCreateChannelsByChildNodeID)
                        {
                            foreach (int ruleID in ruleIDArrayList)
                            {
                                foreach (int nodeIDToCreate in nodeIDArrayList)
                                {
                                    this.CreateChannel(ruleID, nodeIDToCreate);
                                }
                            }
                        }

                    }   
                }
            }
            catch (Exception ex)
            {
                LogUtils.SystemErrorLogWrite(ex);
            }
            */
        }
    }
}