using System.Collections;
using System.Text;
using System.Web;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.STL.Parser;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser.StlElement;
using SiteServer.STL.Parser.StlEntity;
using SiteServer.CMS.Model;
using System.Web.UI;
using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using SiteServer.STL.Parser.TemplateDesign;
using System.Collections.Generic;

namespace SiteServer.CMS.Services
{
    public class DynamicPage : Page
    {        
        public void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                VisualInfo visualInfo = VisualInfo.GetInstance();

                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(visualInfo.PublishmentSystemID);

                TemplateInfo templateInfo = TemplateManager.GetTemplateInfo(visualInfo.PublishmentSystemID, visualInfo.ChannelID, visualInfo.TemplateType);
                if (templateInfo == null)
                {
                    templateInfo = TemplateManager.GetTemplateInfo(visualInfo.PublishmentSystemID, visualInfo.FileTemplateID);
                }

                PageInfo pageInfo = new PageInfo(templateInfo, visualInfo.PublishmentSystemID, visualInfo.ChannelID, visualInfo.ContentID, publishmentSystemInfo, visualInfo.IsDesign ? EVisualType.Design : EVisualType.Dynamic);
                ContextInfo contextInfo = new ContextInfo(pageInfo);

                StringBuilder contentBuilder = null;
                if (visualInfo.IsDesign)
                {
                    contentBuilder = new StringBuilder(TemplateDesignUndoRedo.GetTemplateContent(publishmentSystemInfo, templateInfo, visualInfo.IncludeUrl));
                }
                else
                {
                    contentBuilder = new StringBuilder(CreateCacheManager.FileContent.GetTemplateContent(publishmentSystemInfo, templateInfo));
                }
                //需要完善，考虑单页模板、内容正文、翻页及外部链接
                
                if (visualInfo.IsDesign && publishmentSystemInfo.Additional.DesignMode == EDesignMode.Edit)
                {
                    TemplateDesignManager.ChangeTemplateContent(contentBuilder, publishmentSystemInfo, templateInfo, visualInfo.IncludeUrl);
                }

                if (visualInfo.TemplateType == ETemplateType.IndexPageTemplate)             //首页
                {
                    this.WriteResponse(contentBuilder, publishmentSystemInfo, pageInfo, contextInfo, visualInfo, false);
                    return;
                }
                else if (visualInfo.TemplateType == ETemplateType.FileTemplate)           //单页
                {
                    TemplateInfo fileTemplateInfo = TemplateManager.GetTemplateInfo(visualInfo.PublishmentSystemID, visualInfo.FileTemplateID);
                    PageInfo filePageInfo = new PageInfo(fileTemplateInfo, visualInfo.PublishmentSystemID, visualInfo.ChannelID, visualInfo.ContentID, publishmentSystemInfo, pageInfo.VisualType);
                    StringBuilder fileContentBuilder = new StringBuilder(CreateCacheManager.FileContent.GetTemplateContent(publishmentSystemInfo, fileTemplateInfo));
                    this.WriteResponse(fileContentBuilder, publishmentSystemInfo, filePageInfo, contextInfo, visualInfo, false);
                    return;
                }
                else if (visualInfo.TemplateType == ETemplateType.ChannelTemplate)        //栏目页面
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(visualInfo.PublishmentSystemID, visualInfo.ChannelID);
                    if (nodeInfo == null) return;

                    if (nodeInfo.NodeType != ENodeType.BackgroundPublishNode)
                    {
                        if (!string.IsNullOrEmpty(nodeInfo.LinkUrl))
                        {
                            base.Response.Redirect(nodeInfo.LinkUrl);
                            return;
                        }
                    }

                    List<string> stlLabelList = StlParserUtility.GetStlLabelList(contentBuilder.ToString());

                    //如果标签中存在Content
                    string stlContentElement = string.Empty;

                    foreach (string label in stlLabelList)
                    {
                        if (StlParserUtility.IsStlChannelElement(label, NodeAttribute.PageContent))
                        {
                            stlContentElement = label;
                            break;
                        }
                    }

                    if (!string.IsNullOrEmpty(stlContentElement))//内容存在
                    {
                        StringBuilder innerBuilder = new StringBuilder(stlContentElement);
                        StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                        string contentAttributeHtml = innerBuilder.ToString();
                        int pageCount = StringUtils.GetCount(ContentUtility.PagePlaceHolder, contentAttributeHtml) + 1;//一共需要的页数
                        if (pageCount > 1)
                        {
                            for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                            {
                                PageInfo thePageInfo = new PageInfo(pageInfo.TemplateInfo, pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.PageContentID, pageInfo.PublishmentSystemInfo, pageInfo.VisualType);
                                int index = contentAttributeHtml.IndexOf(ContentUtility.PagePlaceHolder);
                                int length = (index == -1) ? contentAttributeHtml.Length : index;

                                if (currentPageIndex == visualInfo.PageIndex)
                                {
                                    string pagedContentAttributeHtml = contentAttributeHtml.Substring(0, length);
                                    StringBuilder pagedBuilder = new StringBuilder(contentBuilder.ToString().Replace(stlContentElement, pagedContentAttributeHtml));
                                    StlParserManager.ReplacePageElementsInChannelPage(pagedBuilder, thePageInfo, stlLabelList, thePageInfo.PageNodeID, currentPageIndex, pageCount, 0);

                                    this.WriteResponse(pagedBuilder, publishmentSystemInfo, thePageInfo, contextInfo, visualInfo, false);
                                    return;
                                }

                                if (index != -1)
                                {
                                    contentAttributeHtml = contentAttributeHtml.Substring(length + ContentUtility.PagePlaceHolder.Length);
                                }
                            }
                            return;
                        }
                        contentBuilder.Replace(stlContentElement, contentAttributeHtml);
                    }

                    if (StlParserUtility.IsStlElementExists(StlPageContents.ElementName, stlLabelList))//如果标签中存在<stl:pageContents>
                    {
                        string stlElement = StlParserUtility.GetStlElement(StlPageContents.ElementName, stlLabelList);
                        string stlPageContentsElement = stlElement;
                        string stlPageContentsElementReplaceString = stlElement;

                        StlPageContents pageContentsElementParser = new StlPageContents(stlPageContentsElement, pageInfo, contextInfo, false);
                        int totalNum = 0;
                        int pageCount = pageContentsElementParser.GetPageCount(out totalNum);

                        for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                        {
                            if (currentPageIndex == visualInfo.PageIndex)
                            {
                                PageInfo thePageInfo = new PageInfo(pageInfo.TemplateInfo, pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.PageContentID, pageInfo.PublishmentSystemInfo, pageInfo.VisualType);
                                string pageHtml = pageContentsElementParser.Parse(totalNum, currentPageIndex, pageCount);
                                StringBuilder pagedBuilder = new StringBuilder(contentBuilder.ToString().Replace(stlPageContentsElementReplaceString, pageHtml));

                                StlParserManager.ReplacePageElementsInChannelPage(pagedBuilder, thePageInfo, stlLabelList, thePageInfo.PageNodeID, currentPageIndex, pageCount, totalNum);

                                this.WriteResponse(pagedBuilder, publishmentSystemInfo, thePageInfo, contextInfo, visualInfo, false);
                                return;
                            }
                        }
                    }
                    else if (StlParserUtility.IsStlElementExists(StlPageIPushContents.ElementName, stlLabelList))//如果标签中存在<stl:StlPageIPushContents>by 20151125 sofuny 增加了分页标签
                    {
                        string stlElement = StlParserUtility.GetStlElement(StlPageIPushContents.ElementName, stlLabelList);
                        string stlPageContentsElement = stlElement;
                        string stlPageContentsElementReplaceString = stlElement;

                        StlPageIPushContents pageContentsElementParser = new StlPageIPushContents(stlPageContentsElement, pageInfo, contextInfo, false);
                        int totalNum = 0;
                        int pageCount = pageContentsElementParser.GetPageCount(out totalNum);

                        for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                        {
                            if (currentPageIndex == visualInfo.PageIndex)
                            {
                                PageInfo thePageInfo = new PageInfo(pageInfo.TemplateInfo, pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.PageContentID, pageInfo.PublishmentSystemInfo, pageInfo.VisualType);
                                string pageHtml = pageContentsElementParser.Parse(totalNum, currentPageIndex, pageCount);
                                StringBuilder pagedBuilder = new StringBuilder(contentBuilder.ToString().Replace(stlPageContentsElementReplaceString, pageHtml));

                                StlParserManager.ReplacePageElementsInChannelPage(pagedBuilder, thePageInfo, stlLabelList, thePageInfo.PageNodeID, currentPageIndex, pageCount, totalNum);

                                this.WriteResponse(pagedBuilder, publishmentSystemInfo, thePageInfo, contextInfo, visualInfo, false);
                                return;
                            }
                        }
                    }
                    else if (StlParserUtility.IsStlElementExists(StlPageChannels.ElementName, stlLabelList))//如果标签中存在<stl:pageChannels>
                    {
                        string stlElement = StlParserUtility.GetStlElement(StlPageChannels.ElementName, stlLabelList);
                        string stlPageChannelsElement = stlElement;
                        string stlPageChannelsElementReplaceString = stlElement;

                        StlPageChannels pageChannelsElementParser = new StlPageChannels(stlPageChannelsElement, pageInfo, contextInfo, false);
                        int totalNum = 0;
                        int pageCount = pageChannelsElementParser.GetPageCount(out totalNum);

                        for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                        {
                            if (currentPageIndex == visualInfo.PageIndex)
                            {
                                PageInfo thePageInfo = new PageInfo(pageInfo.TemplateInfo, pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.PageContentID, pageInfo.PublishmentSystemInfo, pageInfo.VisualType);
                                string pageHtml = pageChannelsElementParser.Parse(currentPageIndex, pageCount);
                                StringBuilder pagedBuilder = new StringBuilder(contentBuilder.ToString().Replace(stlPageChannelsElementReplaceString, pageHtml));

                                StlParserManager.ReplacePageElementsInChannelPage(pagedBuilder, thePageInfo, stlLabelList, thePageInfo.PageNodeID, currentPageIndex, pageCount, totalNum);

                                this.WriteResponse(pagedBuilder, publishmentSystemInfo, thePageInfo, contextInfo, visualInfo, false);
                                return;
                            }
                        }
                    }
                    else if (StlParserUtility.IsStlElementExists(StlPageSqlContents.ElementName, stlLabelList))//如果标签中存在<stl:pageSqlContents>
                    {
                        string stlElement = StlParserUtility.GetStlElement(StlPageSqlContents.ElementName, stlLabelList);
                        string stlPageSqlContentsElement = stlElement;
                        string stlPageSqlContentsElementReplaceString = stlElement;

                        StlPageSqlContents pageSqlContentsElementParser = new StlPageSqlContents(stlPageSqlContentsElement, pageInfo, contextInfo, false);
                        int totalNum = 0;
                        int pageCount = pageSqlContentsElementParser.GetPageCount(out totalNum);

                        for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                        {
                            if (currentPageIndex == visualInfo.PageIndex)
                            {
                                PageInfo thePageInfo = new PageInfo(pageInfo.TemplateInfo, pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.PageContentID, pageInfo.PublishmentSystemInfo, pageInfo.VisualType);
                                string pageHtml = pageSqlContentsElementParser.Parse(currentPageIndex, pageCount);
                                StringBuilder pagedBuilder = new StringBuilder(contentBuilder.ToString().Replace(stlPageSqlContentsElementReplaceString, pageHtml));

                                StlParserManager.ReplacePageElementsInChannelPage(pagedBuilder, thePageInfo, stlLabelList, thePageInfo.PageNodeID, currentPageIndex, pageCount, totalNum);

                                this.WriteResponse(pagedBuilder, publishmentSystemInfo, thePageInfo, contextInfo, visualInfo, false);
                                return;
                            }
                        }
                    }
                    else
                    {
                        this.WriteResponse(contentBuilder, publishmentSystemInfo, pageInfo, contextInfo, visualInfo, false);
                        return;
                    }
                }
                else if (visualInfo.TemplateType == ETemplateType.ContentTemplate)        //内容页面
                {
                    if (contextInfo.ContentInfo == null) return;

                    if (!string.IsNullOrEmpty(contextInfo.ContentInfo.GetExtendedAttribute(BackgroundContentAttribute.LinkUrl)))
                    {
                        base.Response.Redirect(contextInfo.ContentInfo.GetExtendedAttribute(BackgroundContentAttribute.LinkUrl));
                        return;
                    }

                    string filePath = PathUtility.GetContentPageFilePath(publishmentSystemInfo, pageInfo.PageNodeID, pageInfo.PageContentID, 0);

                    List<string> stlLabelList = StlParserUtility.GetStlLabelList(contentBuilder.ToString());

                    //如果标签中存在Content
                    string stlContentElement = string.Empty;

                    foreach (string label in stlLabelList)
                    {
                        if (StlParserUtility.IsStlContentElement(label, BackgroundContentAttribute.PageContent))
                        {
                            stlContentElement = label;
                            break;
                        }
                    }

                    if (!string.IsNullOrEmpty(stlContentElement))//内容存在
                    {
                        StringBuilder innerBuilder = new StringBuilder(stlContentElement);
                        StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                        string contentAttributeHtml = innerBuilder.ToString();
                        int pageCount = StringUtils.GetCount(ContentUtility.PagePlaceHolder, contentAttributeHtml) + 1;//一共需要的页数
                        if (pageCount > 1)
                        {
                            for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                            {
                                int index = contentAttributeHtml.IndexOf(ContentUtility.PagePlaceHolder);
                                int length = (index == -1) ? contentAttributeHtml.Length : index;

                                if (currentPageIndex == visualInfo.PageIndex)
                                {
                                    PageInfo thePageInfo = new PageInfo(pageInfo.TemplateInfo, pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.PageContentID, pageInfo.PublishmentSystemInfo, pageInfo.VisualType);
                                    string pagedContentAttributeHtml = contentAttributeHtml.Substring(0, length);
                                    StringBuilder pagedBuilder = new StringBuilder(contentBuilder.ToString().Replace(stlContentElement, pagedContentAttributeHtml));
                                    StlParserManager.ReplacePageElementsInContentPage(pagedBuilder, thePageInfo, stlLabelList, visualInfo.ChannelID, visualInfo.ContentID, currentPageIndex, pageCount);

                                    this.WriteResponse(pagedBuilder, publishmentSystemInfo, thePageInfo, contextInfo, visualInfo, false);
                                    return;
                                }

                                if (index != -1)
                                {
                                    contentAttributeHtml = contentAttributeHtml.Substring(length + ContentUtility.PagePlaceHolder.Length);
                                }
                            }
                            return;
                        }
                        contentBuilder.Replace(stlContentElement, contentAttributeHtml);
                    }

                    if (StlParserUtility.IsStlElementExists(StlPageContents.ElementName, stlLabelList))//如果标签中存在<stl:pageContents>
                    {
                        string stlElement = StlParserUtility.GetStlElement(StlPageContents.ElementName, stlLabelList);
                        string stlPageContentsElement = stlElement;
                        string stlPageContentsElementReplaceString = stlElement;

                        StlPageContents pageContentsElementParser = new StlPageContents(stlPageContentsElement, pageInfo, contextInfo, false);
                        int totalNum = 0;
                        int pageCount = pageContentsElementParser.GetPageCount(out totalNum);

                        for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                        {
                            if (currentPageIndex == visualInfo.PageIndex)
                            {
                                PageInfo thePageInfo = new PageInfo(pageInfo.TemplateInfo, pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.PageContentID, pageInfo.PublishmentSystemInfo, pageInfo.VisualType);
                                string pageHtml = pageContentsElementParser.Parse(totalNum, currentPageIndex, pageCount);
                                StringBuilder pagedBuilder = new StringBuilder(contentBuilder.ToString().Replace(stlPageContentsElementReplaceString, pageHtml));

                                StlParserManager.ReplacePageElementsInContentPage(pagedBuilder, thePageInfo, stlLabelList, visualInfo.ChannelID, visualInfo.ContentID, currentPageIndex, pageCount);

                                this.WriteResponse(pagedBuilder, publishmentSystemInfo, thePageInfo, contextInfo, visualInfo, true);
                                return;
                            }
                        }
                    }
                        else if (StlParserUtility.IsStlElementExists(StlPageIPushContents.ElementName, stlLabelList))//如果标签中存在<stl:pageContents>by 20151125 sofuny 增加了分页标签
                    {
                        string stlElement = StlParserUtility.GetStlElement(StlPageIPushContents.ElementName, stlLabelList);
                        string stlPageContentsElement = stlElement;
                        string stlPageContentsElementReplaceString = stlElement;

                        StlPageIPushContents pageContentsElementParser = new StlPageIPushContents(stlPageContentsElement, pageInfo, contextInfo, false);
                        int totalNum = 0;
                        int pageCount = pageContentsElementParser.GetPageCount(out totalNum);

                        for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                        {
                            if (currentPageIndex == visualInfo.PageIndex)
                            {
                                PageInfo thePageInfo = new PageInfo(pageInfo.TemplateInfo, pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.PageContentID, pageInfo.PublishmentSystemInfo, pageInfo.VisualType);
                                string pageHtml = pageContentsElementParser.Parse(totalNum, currentPageIndex, pageCount);
                                StringBuilder pagedBuilder = new StringBuilder(contentBuilder.ToString().Replace(stlPageContentsElementReplaceString, pageHtml));

                                StlParserManager.ReplacePageElementsInContentPage(pagedBuilder, thePageInfo, stlLabelList, visualInfo.ChannelID, visualInfo.ContentID, currentPageIndex, pageCount);

                                this.WriteResponse(pagedBuilder, publishmentSystemInfo, thePageInfo, contextInfo, visualInfo, true);
                                return;
                            }
                        }
                    }
                    else if (StlParserUtility.IsStlElementExists(StlPageChannels.ElementName, stlLabelList))//如果标签中存在<stl:pageChannels>
                    {
                        string stlElement = StlParserUtility.GetStlElement(StlPageChannels.ElementName, stlLabelList);
                        string stlPageChannelsElement = stlElement;
                        string stlPageChannelsElementReplaceString = stlElement;

                        StlPageChannels pageChannelsElementParser = new StlPageChannels(stlPageChannelsElement, pageInfo, contextInfo, false);
                        int totalNum = 0;
                        int pageCount = pageChannelsElementParser.GetPageCount(out totalNum);

                        for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                        {
                            if (currentPageIndex == visualInfo.PageIndex)
                            {
                                PageInfo thePageInfo = new PageInfo(pageInfo.TemplateInfo, pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.PageContentID, pageInfo.PublishmentSystemInfo, pageInfo.VisualType);
                                string pageHtml = pageChannelsElementParser.Parse(currentPageIndex, pageCount);
                                StringBuilder pagedBuilder = new StringBuilder(contentBuilder.ToString().Replace(stlPageChannelsElementReplaceString, pageHtml));

                                StlParserManager.ReplacePageElementsInContentPage(pagedBuilder, thePageInfo, stlLabelList, visualInfo.ChannelID, visualInfo.ContentID, currentPageIndex, pageCount);

                                this.WriteResponse(pagedBuilder, publishmentSystemInfo, thePageInfo, contextInfo, visualInfo, true);
                                return;
                            }
                        }
                    }
                    else if (StlParserUtility.IsStlElementExists(StlPageSqlContents.ElementName, stlLabelList))//如果标签中存在<stl:pageSqlContents>
                    {
                        string stlElement = StlParserUtility.GetStlElement(StlPageSqlContents.ElementName, stlLabelList);
                        string stlPageSqlContentsElement = stlElement;
                        string stlPageSqlContentsElementReplaceString = stlElement;

                        StlPageSqlContents pageSqlContentsElementParser = new StlPageSqlContents(stlPageSqlContentsElement, pageInfo, contextInfo, false);
                        int totalNum = 0;
                        int pageCount = pageSqlContentsElementParser.GetPageCount(out totalNum);

                        for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                        {
                            if (currentPageIndex == visualInfo.PageIndex)
                            {
                                PageInfo thePageInfo = new PageInfo(pageInfo.TemplateInfo, pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.PageContentID, pageInfo.PublishmentSystemInfo, pageInfo.VisualType);
                                string pageHtml = pageSqlContentsElementParser.Parse(currentPageIndex, pageCount);
                                StringBuilder pagedBuilder = new StringBuilder(contentBuilder.ToString().Replace(stlPageSqlContentsElementReplaceString, pageHtml));

                                StlParserManager.ReplacePageElementsInContentPage(pagedBuilder, thePageInfo, stlLabelList, visualInfo.ChannelID, visualInfo.ContentID, currentPageIndex, pageCount);

                                this.WriteResponse(pagedBuilder, publishmentSystemInfo, thePageInfo, contextInfo, visualInfo, true);
                                return;
                            }
                        }
                    }
                    else//无翻页
                    {
                        this.WriteResponse(contentBuilder, publishmentSystemInfo, pageInfo, contextInfo, visualInfo, true);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                base.Response.Write(ex.Message);
            }
        }

        private void WriteResponse(StringBuilder contentBuilder, PublishmentSystemInfo publishmentSystemInfo, PageInfo pageInfo, ContextInfo contextInfo, VisualInfo visualInfo, bool isDeletePreviewContent)
        {
            StlUtility.LoadGeneratePageContent(publishmentSystemInfo, pageInfo, contextInfo, contentBuilder, visualInfo.FilePath);//生成页面

            if (visualInfo.IsDesign && publishmentSystemInfo.Additional.DesignMode == EDesignMode.Edit)
            {
                TemplateDesignManager.ChangeResponseHtml(contentBuilder, publishmentSystemInfo);
            }

            base.Response.ContentEncoding = System.Text.Encoding.GetEncoding(publishmentSystemInfo.Additional.Charset);

            base.Response.Write(contentBuilder.ToString());

            if (isDeletePreviewContent && HttpContext.Current.Request.Form != null && HttpContext.Current.Request.Form.Count > 0)
            {
                visualInfo.RemovePreviewContent();
            }
        }
    }
}
