using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core.Advertisement;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser.StlElement;
using SiteServer.STL.Parser.StlEntity;
using SiteServer.CMS.Model;
using SiteServer.CMS.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using SiteServer.CMS.Core;
using SiteServer.WeiXin.Model;
using SiteServer.WeiXin.Core;
using SiteServer.B2C.Core;

namespace SiteServer.STL.Parser
{
    public class StlUtility
    {
        public static string GetStlCurrentUrl(PageInfo pageInfo, int nodeID, int contentID, ContentInfo contentInfo)
        {
            string currentUrl = string.Empty;
            if (pageInfo.TemplateInfo.TemplateType == ETemplateType.IndexPageTemplate)
            {
                currentUrl = pageInfo.PublishmentSystemInfo.PublishmentSystemUrl;
            }
            else if (pageInfo.TemplateInfo.TemplateType == ETemplateType.ContentTemplate)
            {
                if (contentInfo == null)
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, nodeID);
                    currentUrl = PageUtility.GetContentUrl(pageInfo.PublishmentSystemInfo, nodeInfo, contentID, pageInfo.VisualType);
                }
                else
                {
                    currentUrl = PageUtility.GetContentUrl(pageInfo.PublishmentSystemInfo, contentInfo, pageInfo.VisualType);
                }
            }
            else if (pageInfo.TemplateInfo.TemplateType == ETemplateType.ChannelTemplate)
            {
                currentUrl = PageUtility.GetChannelUrl(pageInfo.PublishmentSystemInfo, NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, nodeID), pageInfo.VisualType);
            }
            else if (pageInfo.TemplateInfo.TemplateType == ETemplateType.FileTemplate)
            {
                currentUrl = PageUtility.GetFileUrl(pageInfo.PublishmentSystemInfo, pageInfo.TemplateInfo.TemplateID, pageInfo.VisualType);
            }
            //currentUrl是当前页面的地址，前后台分离的时候，不允许带上protocol
            //return PageUtils.AddProtocolToUrl(currentUrl);
            return currentUrl;
        }

        public static bool IsAdvertisementExists(PageInfo pageInfo)
        {
            ArrayList[] arraylists = AdvertisementManager.GetAdvertisementArrayLists(pageInfo.PublishmentSystemID);
            if (pageInfo.TemplateInfo.TemplateType == ETemplateType.IndexPageTemplate || pageInfo.TemplateInfo.TemplateType == ETemplateType.ChannelTemplate)
            {
                ArrayList arraylist = arraylists[0];
                return arraylist.Contains(pageInfo.PageNodeID);
            }
            else if (pageInfo.TemplateInfo.TemplateType == ETemplateType.ContentTemplate)
            {
                ArrayList arraylist = arraylists[1];
                return arraylist.Contains(pageInfo.PageContentID);
            }
            else if (pageInfo.TemplateInfo.TemplateType == ETemplateType.FileTemplate)
            {
                ArrayList arraylist = arraylists[2];
                return arraylist.Contains(pageInfo.TemplateInfo.TemplateID);
            }
            return false;
        }

        public static bool IsSeoMetaExists(PageInfo pageInfo)
        {
            ArrayList[] arraylists = SeoManager.GetSeoMetaArrayLists(pageInfo.PublishmentSystemID);
            if (pageInfo.PageContentID != 0)
            {
                ArrayList arraylist = arraylists[1];
                return arraylist.Contains(pageInfo.PageNodeID);
            }
            else
            {
                ArrayList arraylist = arraylists[0];
                return arraylist.Contains(pageInfo.PageNodeID);
            }
        }

        private static void AddScriptsByPublishmentSystemType(PageInfo pageInfo)
        {
            if (pageInfo.PublishmentSystemInfo.PublishmentSystemType == EPublishmentSystemType.Weixin || pageInfo.PublishmentSystemInfo.PublishmentSystemType == EPublishmentSystemType.WeixinB2C)
            {
                string webMenuHtml = NavigationUtils.ParseWebMenu(pageInfo.PublishmentSystemInfo);
                if (!string.IsNullOrEmpty(webMenuHtml))
                {
                    pageInfo.AddPageScriptsIfNotExists("Weixin_WebMenu", webMenuHtml);
                }
            }

            if (EPublishmentSystemTypeUtils.IsB2C(pageInfo.PublishmentSystemInfo.PublishmentSystemType))
            {
                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.A_Platform_BASIC);
                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_BASIC);
            }
        }

        public static void LoadGeneratePageContent(PublishmentSystemInfo publishmentSystemInfo, PageInfo pageInfo, ContextInfo contextInfo, StringBuilder contentBuilder, string filePath)
        {
            if (contentBuilder.Length > 0)
            {
                StlParserManager.ParseTemplateContent(contentBuilder, pageInfo, contextInfo);
            }

            if (EFileSystemTypeUtils.IsHtml(PathUtils.GetExtension(filePath)))
            {
                if (pageInfo.TemplateInfo.TemplateType != ETemplateType.FileTemplate)
                {
                    AddSeoMetaToContent(pageInfo, contentBuilder);
                }

                StlUtility.AddAdvertisementsToContent(pageInfo);

                if (pageInfo.VisualType != EVisualType.Static)
                {
                    string pageUrl = PageUtils.AddProtocolToUrl(PageUtility.GetPublishmentSystemUrlByPhysicalPath(publishmentSystemInfo, filePath));
                    string templateString = string.Format(@"
<base href=""{0}"" />", pageUrl);
                    StringUtils.InsertAfter(new string[] { "<head>", "<HEAD>" }, contentBuilder, templateString);
                }

                if (pageInfo.PublishmentSystemInfo.Additional.IsCreateBrowserNoCache)
                {
                    string templateString = @"
<META HTTP-EQUIV=""Pragma"" CONTENT=""no-cache"">
<META HTTP-EQUIV=""Expires"" CONTENT=""-1"">";
                    StringUtils.InsertAfter(new string[] { "<head>", "<HEAD>" }, contentBuilder, templateString);
                }

                if (pageInfo.PublishmentSystemInfo.Additional.IsCreateIE8Compatible)
                {
                    string templateString = @"
<META HTTP-EQUIV=""x-ua-compatible"" CONTENT=""ie=7"" />";
                    StringUtils.InsertAfter(new string[] { "<head>", "<HEAD>" }, contentBuilder, templateString);
                }

                if (pageInfo.PublishmentSystemInfo.Additional.IsCreateJsIgnoreError)
                {
                    string templateString = @"
<script type=""text/javascript"">window.onerror=function(){return true;}</script>";
                    StringUtils.InsertAfter(new string[] { "<head>", "<HEAD>" }, contentBuilder, templateString);
                }

                if (pageInfo.PageContentID > 0 && pageInfo.PublishmentSystemInfo.Additional.IsCountHits && !pageInfo.IsPageScriptsExists(PageInfo.Js_Ad_StlCountHits))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.Js_Ad_StlCountHits, string.Format(@"
<script src=""{0}"" type=""text/javascript""></script>", PageService.GetAddCountHitsUrl(pageInfo.PublishmentSystemInfo, pageInfo.PageNodeID, pageInfo.PageContentID)));
                }

                /**
                 * by 20151125 sofuny
                 * 培生智能推送--会员浏览内容统计内容的上一级栏目
                 * */
                if (pageInfo.PageContentID > 0 && pageInfo.PublishmentSystemInfo.Additional.IsIntelligentPushCount && !pageInfo.IsPageScriptsExists(PageInfo.Js_Ad_IntelligentPushCount))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.Js_Ad_IntelligentPushCount, string.Format(@"
<script src=""{0}"" type=""text/javascript""></script>", PageService.GetAddIntelligentPushCountUrl(pageInfo.PublishmentSystemInfo, pageInfo.PageNodeID, pageInfo.PageContentID)));
                }

                if (pageInfo.PublishmentSystemInfo.Additional.IsTracker && !pageInfo.IsPageScriptsExists(PageInfo.Js_Ad_AddTracker))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.Js_Ad_AddTracker, string.Format(@"
<script src=""{0}"" type=""text/javascript""></script>
<script type=""text/javascript"">AddTrackerCount('{1}', {2});</script>", PageUtility.GetSiteFilesUrl(pageInfo.PublishmentSystemInfo, SiteFiles.Tracker.Js), PageService.GetAddTrackerCountUrl(pageInfo.PublishmentSystemInfo, pageInfo.PageNodeID, pageInfo.PageContentID), pageInfo.PublishmentSystemID));
                }

                StlUtility.AddScriptsByPublishmentSystemType(pageInfo);

                string headScripts = StlParserManager.GetPageInfoHeadScript(pageInfo, contextInfo);
                if (!string.IsNullOrEmpty(headScripts))
                {
                    if (contentBuilder.ToString().IndexOf("</head>") != -1 || contentBuilder.ToString().IndexOf("</HEAD>") != -1)
                    {
                        StringUtils.InsertBefore(new string[] { "</head>", "</HEAD>" }, contentBuilder, headScripts);
                    }
                    else
                    {
                        contentBuilder.Insert(0, headScripts);
                    }
                }

                string afterBodyScripts = StlParserManager.GetPageInfoScript(pageInfo, true);

                if (!string.IsNullOrEmpty(afterBodyScripts))
                {
                    if (contentBuilder.ToString().IndexOf("<body") != -1 || contentBuilder.ToString().IndexOf("<BODY") != -1)
                    {
                        int index = contentBuilder.ToString().IndexOf("<body");
                        if (index == -1)
                        {
                            index = contentBuilder.ToString().IndexOf("<BODY");
                        }
                        index = contentBuilder.ToString().IndexOf(">", index);
                        contentBuilder.Insert(index + 1, StringUtils.Constants.ReturnAndNewline + afterBodyScripts + StringUtils.Constants.ReturnAndNewline);
                    }
                    else
                    {
                        contentBuilder.Insert(0, afterBodyScripts);
                    }
                }

                string beforeBodyScripts = StlParserManager.GetPageInfoScript(pageInfo, false);

                if (EPublishmentSystemTypeUtils.IsMobile(pageInfo.PublishmentSystemInfo.PublishmentSystemType))
                {
                    string poweredBy = string.Empty;
                    bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);
                    if (!isPoweredBy)
                    {
                        beforeBodyScripts += @"
<div style=""height: 34px;line-height: 34px;font-size: 12px;background: #e3e2dd;text-align: center;color: #999;"">
  <p>技术支持：阁下 <a href=""http://m.gexia.com"" target=""_blank"" style=""color: #666666;text-decoration: none;"">GEXIA.COM</a></p>
</div>";
                    }
                    else if (!string.IsNullOrEmpty(poweredBy))
                    {
                        beforeBodyScripts += string.Format(@"
<div style=""height: 34px;line-height: 34px;font-size: 12px;background: #e3e2dd;text-align: center;color: #999;"">
  <p>{0}</p>
</div>", poweredBy);
                    }

                    if (pageInfo.PublishmentSystemInfo.Additional.ZDH_IsEnabled)
                    {
                        int dataAppid = pageInfo.PublishmentSystemInfo.Additional.ZDH_DataAppID;

                        beforeBodyScripts += @"
<script type=""text/javascript"">
(function(){
    var script = document.createElement(""script"");
    script.type = ""text/javascript"";
    script.charset = ""utf-8"";
    var date = new Date();
    var version = date.getFullYear()+""""+date.getMonth()+""""+date.getDate()+""""+date.getHours();
    script.src = ""http://m.baidu.com/static/search/siteapp/lego/seed.js?t=""+version;
    script.setAttribute(""data-appid""," + dataAppid;
                        beforeBodyScripts += @");
    document.head.appendChild(script);
})();
</script>";
                    }
                }

                if (!string.IsNullOrEmpty(beforeBodyScripts))
                {
                    if (contentBuilder.ToString().IndexOf("</body>") != -1 || contentBuilder.ToString().IndexOf("</BODY>") != -1)
                    {
                        int index = contentBuilder.ToString().IndexOf("</body>");
                        if (index == -1)
                        {
                            index = contentBuilder.ToString().IndexOf("</BODY>");
                        }
                        contentBuilder.Insert(index, StringUtils.Constants.ReturnAndNewline + beforeBodyScripts + StringUtils.Constants.ReturnAndNewline);
                    }
                    else
                    {
                        contentBuilder.Append(beforeBodyScripts);
                    }
                }

                if (pageInfo.PublishmentSystemInfo.Additional.IsCreateDoubleClick && pageInfo.PublishmentSystemInfo.Additional.VisualType == EVisualType.Static)
                {
                    int fileTemplateID = 0;
                    if (pageInfo.TemplateInfo.TemplateType == ETemplateType.FileTemplate)
                    {
                        fileTemplateID = pageInfo.TemplateInfo.TemplateID;
                    }
                    string ajaxUrl = PageUtility.ServiceSTL.Utils.GetStlTriggerUrl(pageInfo.PublishmentSystemID, contextInfo.ChannelID, contextInfo.ContentID, fileTemplateID, true);
                    pageInfo.AddPageEndScriptsIfNotExists("CreateDoubleClick", string.Format(@"
<script type=""text/javascript"" language=""javascript"">document.ondblclick=function(x){{location.href = '{0}';}}</script>", ajaxUrl));
                }

                if (pageInfo.PageEndScriptKeys.Count > 0)
                {
                    StringBuilder endScriptBuilder = new StringBuilder();
                    foreach (string scriptKey in pageInfo.PageEndScriptKeys)
                    {
                        endScriptBuilder.Append(pageInfo.GetPageEndScripts(scriptKey));
                    }
                    endScriptBuilder.Append(StringUtils.Constants.ReturnAndNewline);

                    //contentBuilder.Append(endScriptBuilder.ToString());
                    //StringUtils.InsertBeforeOrAppend(new string[] { "</body>", "</BODY>" }, contentBuilder, endScriptBuilder.ToString());
                    StringUtils.InsertAfterOrAppend(new string[] { "</html>", "</html>" }, contentBuilder, endScriptBuilder.ToString());
                }

                if (pageInfo.PublishmentSystemInfo.Additional.IsCreateShowPageInfo)
                {
                    contentBuilder.AppendFormat(@"
<!-- {0}({1}) -->", pageInfo.TemplateInfo.RelatedFileName, ETemplateTypeUtils.GetText(pageInfo.TemplateInfo.TemplateType));
                }

                if (pageInfo.PublishmentSystemInfo.Additional.IsRestriction)
                {
                    StlUtility.AddRestrictionToContent(pageInfo, contentBuilder);
                }
            }
        }

        private static void AddSeoMetaToContent(PageInfo pageInfo, StringBuilder contentBuilder)
        {
            if (StlUtility.IsSeoMetaExists(pageInfo))
            {
                string metaContent = string.Empty;
                int seoMetaID;
                if (pageInfo.PageContentID != 0)
                {
                    seoMetaID = DataProvider.SeoMetaDAO.GetSeoMetaIDByNodeID(pageInfo.PageNodeID, false);
                    if (seoMetaID == 0)
                    {
                        seoMetaID = DataProvider.SeoMetaDAO.GetDefaultSeoMetaID(pageInfo.PublishmentSystemID);
                    }
                }
                else
                {
                    seoMetaID = DataProvider.SeoMetaDAO.GetSeoMetaIDByNodeID(pageInfo.PageNodeID, true);
                    if (seoMetaID == 0)
                    {
                        seoMetaID = DataProvider.SeoMetaDAO.GetDefaultSeoMetaID(pageInfo.PublishmentSystemID);
                    }
                }
                if (seoMetaID != 0)
                {
                    SeoMetaInfo seoMetaInfo = DataProvider.SeoMetaDAO.GetSeoMetaInfo(seoMetaID);
                    SeoMetaInfo seoMetaInfoFromTemplate = SeoManager.GetSeoMetaInfo(contentBuilder.ToString());
                    if (!string.IsNullOrEmpty(seoMetaInfoFromTemplate.PageTitle)) seoMetaInfo.PageTitle = string.Empty;
                    if (!string.IsNullOrEmpty(seoMetaInfoFromTemplate.Keywords)) seoMetaInfo.Keywords = string.Empty;
                    if (!string.IsNullOrEmpty(seoMetaInfoFromTemplate.Description)) seoMetaInfo.Description = string.Empty;
                    if (!string.IsNullOrEmpty(seoMetaInfoFromTemplate.Copyright)) seoMetaInfo.Copyright = string.Empty;
                    if (!string.IsNullOrEmpty(seoMetaInfoFromTemplate.Author)) seoMetaInfo.Author = string.Empty;
                    if (!string.IsNullOrEmpty(seoMetaInfoFromTemplate.Email)) seoMetaInfo.Email = string.Empty;
                    if (!string.IsNullOrEmpty(seoMetaInfoFromTemplate.Language)) seoMetaInfo.Language = string.Empty;
                    if (!string.IsNullOrEmpty(seoMetaInfoFromTemplate.Charset)) seoMetaInfo.Charset = string.Empty;
                    if (!string.IsNullOrEmpty(seoMetaInfoFromTemplate.Distribution)) seoMetaInfo.Distribution = string.Empty;
                    if (!string.IsNullOrEmpty(seoMetaInfoFromTemplate.Rating)) seoMetaInfo.Rating = string.Empty;
                    if (!string.IsNullOrEmpty(seoMetaInfoFromTemplate.Robots)) seoMetaInfo.Robots = string.Empty;
                    if (!string.IsNullOrEmpty(seoMetaInfoFromTemplate.RevisitAfter)) seoMetaInfo.RevisitAfter = string.Empty;
                    if (!string.IsNullOrEmpty(seoMetaInfoFromTemplate.Expires)) seoMetaInfo.Expires = string.Empty;

                    metaContent = SeoManager.GetMetaContent(seoMetaInfo);

                    if (!string.IsNullOrEmpty(metaContent))
                    {
                        StringUtils.InsertBefore("</head>", contentBuilder, metaContent);
                    }
                }
            }
        }

        private static void AddAdvertisementsToContent(PageInfo pageInfo)
        {
            if (StlUtility.IsAdvertisementExists(pageInfo))
            {
                ArrayList advertisementNameArrayList = DataProvider.AdvertisementDAO.GetAdvertisementNameArrayList(pageInfo.PublishmentSystemID);

                foreach (string advertisementName in advertisementNameArrayList)
                {
                    AdvertisementInfo adInfo = DataProvider.AdvertisementDAO.GetAdvertisementInfo(advertisementName, pageInfo.PublishmentSystemID);
                    if (adInfo.IsDateLimited)
                    {
                        if (DateTime.Now < adInfo.StartDate || DateTime.Now > adInfo.EndDate)
                        {
                            continue;
                        }
                    }
                    bool isToDo = false;
                    if (pageInfo.TemplateInfo.TemplateType == ETemplateType.IndexPageTemplate || pageInfo.TemplateInfo.TemplateType == ETemplateType.ChannelTemplate)
                    {
                        if (!string.IsNullOrEmpty(adInfo.NodeIDCollectionToChannel))
                        {
                            ArrayList nodeIDArrayList = TranslateUtils.StringCollectionToIntArrayList(adInfo.NodeIDCollectionToChannel);
                            if (nodeIDArrayList.Contains(pageInfo.PageNodeID))
                            {
                                isToDo = true;
                            }
                        }
                    }
                    else if (pageInfo.TemplateInfo.TemplateType == ETemplateType.ContentTemplate)
                    {
                        if (!string.IsNullOrEmpty(adInfo.NodeIDCollectionToContent))
                        {
                            ArrayList nodeIDArrayList = TranslateUtils.StringCollectionToIntArrayList(adInfo.NodeIDCollectionToContent);
                            if (nodeIDArrayList.Contains(pageInfo.PageContentID))
                            {
                                isToDo = true;
                            }
                        }
                    }
                    else if (pageInfo.TemplateInfo.TemplateType == ETemplateType.FileTemplate)
                    {
                        if (!string.IsNullOrEmpty(adInfo.FileTemplateIDCollection))
                        {
                            ArrayList fileTemplateIDArrayList = TranslateUtils.StringCollectionToIntArrayList(adInfo.FileTemplateIDCollection);
                            if (fileTemplateIDArrayList.Contains(pageInfo.TemplateInfo.TemplateID))
                            {
                                isToDo = true;
                            }
                        }
                    }

                    if (isToDo)
                    {
                        string scripts = string.Empty;
                        if (adInfo.AdvertisementType == EAdvertisementType.FloatImage)
                        {
                            pageInfo.AddPageScriptsIfNotExists(PageInfo.Js_Static_AdFloating);

                            FloatingScript floatScript = new FloatingScript(pageInfo.PublishmentSystemInfo, pageInfo.UniqueID, adInfo);
                            scripts = floatScript.GetScript();
                        }
                        else if (adInfo.AdvertisementType == EAdvertisementType.ScreenDown)
                        {
                            pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.A_JQuery);

                            ScreenDownScript screenDownScript = new ScreenDownScript(pageInfo.PublishmentSystemInfo, pageInfo.UniqueID, adInfo);
                            scripts = screenDownScript.GetScript();
                        }
                        else if (adInfo.AdvertisementType == EAdvertisementType.OpenWindow)
                        {
                            OpenWindowScript openWindowScript = new OpenWindowScript(pageInfo.PublishmentSystemInfo, pageInfo.UniqueID, adInfo);
                            scripts = openWindowScript.GetScript();
                        }

                        pageInfo.AddPageEndScriptsIfNotExists(EAdvertisementTypeUtils.GetValue(adInfo.AdvertisementType) + "_" + adInfo.AdvertisementName, scripts);
                    }
                }
            }
        }

        private static void AddRestrictionToContent(PageInfo pageInfo, StringBuilder contentBuilder)
        {
            if (pageInfo.TemplateInfo.TemplateType != ETemplateType.FileTemplate)
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, pageInfo.PageNodeID);
                if (nodeInfo.Additional.RestrictionTypeOfChannel != ERestrictionType.NoRestriction)
                {
                    bool isChannel = true;
                    if (pageInfo.TemplateInfo.TemplateType == ETemplateType.ContentTemplate)
                    {
                        isChannel = false;
                    }

                    string ajaxHtml = PageService.RegisterIsVisitAllowed(contentBuilder.ToString(), pageInfo.PublishmentSystemInfo, pageInfo.PageNodeID, isChannel);

                    contentBuilder.Remove(0, contentBuilder.Length);

                    contentBuilder.AppendFormat(@"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
<meta http-equiv=""Content-Type"" content=""text/html; charset={0}"" />
{1}
</head>

<body>
{2}
</body>
</html>
", ECharsetUtils.GetValue(pageInfo.TemplateInfo.Charset), string.Format(@"<script language=""javascript"" type=""text/javascript"" src=""{0}""></script>", PageUtility.GetSiteFilesUrl(pageInfo.PublishmentSystemInfo, SiteFiles.JQuery.Js)), ajaxHtml);
                }
            }
        }

        public static string ParseDynamicContent(int publishmentSystemID, int channelID, int contentID, int templateID, bool isPageRefresh, string templateContent, string pageUrl, int pageIndex, string ajaxDivID, NameValueCollection queryString)
        {
            TemplateInfo templateInfo = TemplateManager.GetTemplateInfo(publishmentSystemID, templateID);
            //TemplateManager.GetTemplateInfo(publishmentSystemID, channelID, templateType);
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            PageInfo pageInfo = new PageInfo(templateInfo, publishmentSystemID, channelID, contentID, publishmentSystemInfo, publishmentSystemInfo.Additional.VisualType);
            pageInfo.SetUniqueID(1000);
            ContextInfo contextInfo = new ContextInfo(pageInfo);

            templateContent = StlRequestEntities.ParseRequestEntities(queryString, templateContent, templateInfo.Charset);
            StringBuilder contentBuilder = new StringBuilder(templateContent);
            List<string> stlElementList = StlParserUtility.GetStlElementList(contentBuilder.ToString());

            //如果标签中存在<stl:pageContents>
            if (StlParserUtility.IsStlElementExists(StlPageContents.ElementName, stlElementList))
            {
                string stlElement = StlParserUtility.GetStlElement(StlPageContents.ElementName, stlElementList);
                string stlPageContentsElement = stlElement;
                string stlPageContentsElementReplaceString = stlElement;

                StlPageContents pageContentsElementParser = new StlPageContents(stlPageContentsElement, pageInfo, contextInfo, true);
                int totalNum;
                int pageCount = pageContentsElementParser.GetPageCount(out totalNum);

                for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                {
                    if (currentPageIndex == pageIndex)
                    {
                        string pageHtml = pageContentsElementParser.Parse(totalNum, currentPageIndex, pageCount, ajaxDivID);
                        contentBuilder.Replace(stlPageContentsElementReplaceString, pageHtml);

                        StlParserManager.ReplacePageElementsInDynamicPage(contentBuilder, pageInfo, stlElementList, pageUrl, pageInfo.PageNodeID, currentPageIndex, pageCount, totalNum, isPageRefresh, ajaxDivID);

                        break;
                    }
                }
            }
            //by 20151125 sofuny 增加了分页标签
            //如果标签中存在<stl:pageIPushContents>
            if (StlParserUtility.IsStlElementExists(StlPageIPushContents.ElementName, stlElementList))
            {
                string stlElement = StlParserUtility.GetStlElement(StlPageIPushContents.ElementName, stlElementList);
                string stlPageContentsElement = stlElement;
                string stlPageContentsElementReplaceString = stlElement;

                StlPageIPushContents pageContentsElementParser = new StlPageIPushContents(stlPageContentsElement, pageInfo, contextInfo, true);
                int totalNum;
                int pageCount = pageContentsElementParser.GetPageCount(out totalNum);

                for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                {
                    if (currentPageIndex == pageIndex)
                    {
                        string pageHtml = pageContentsElementParser.Parse(totalNum, currentPageIndex, pageCount);
                        contentBuilder.Replace(stlPageContentsElementReplaceString, pageHtml);

                        StlParserManager.ReplacePageElementsInDynamicPage(contentBuilder, pageInfo, stlElementList, pageUrl, pageInfo.PageNodeID, currentPageIndex, pageCount, totalNum, isPageRefresh, ajaxDivID);

                        break;
                    }
                }
            }
            //如果标签中存在<stl:pageChannels>
            else if (StlParserUtility.IsStlElementExists(StlPageChannels.ElementName, stlElementList))
            {
                string stlElement = StlParserUtility.GetStlElement(StlPageChannels.ElementName, stlElementList);
                string stlPageChannelsElement = stlElement;
                string stlPageChannelsElementReplaceString = stlElement;

                StlPageChannels pageChannelsElementParser = new StlPageChannels(stlPageChannelsElement, pageInfo, contextInfo, true);
                int totalNum;
                int pageCount = pageChannelsElementParser.GetPageCount(out totalNum);

                for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                {
                    if (currentPageIndex == pageIndex)
                    {
                        string pageHtml = pageChannelsElementParser.Parse(currentPageIndex, pageCount);
                        contentBuilder.Replace(stlPageChannelsElementReplaceString, pageHtml);

                        StlParserManager.ReplacePageElementsInDynamicPage(contentBuilder, pageInfo, stlElementList, pageUrl, pageInfo.PageNodeID, currentPageIndex, pageCount, totalNum, isPageRefresh, ajaxDivID);

                        break;
                    }
                }
            }
            //如果标签中存在<stl:pageComments>
            else if (StlParserUtility.IsStlElementExists(StlPageComments.ElementName, stlElementList))
            {
                string stlElement = StlParserUtility.GetStlElement(StlPageComments.ElementName, stlElementList);
                string stlPageCommentsElement = stlElement;
                string stlPageCommentsElementReplaceString = stlElement;

                StlPageComments pageCommentsElementParser = new StlPageComments(stlPageCommentsElement, pageInfo, contextInfo, true);
                int totalNum;
                int pageCount = pageCommentsElementParser.GetPageCount(out totalNum);

                for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                {
                    if (currentPageIndex == pageIndex)
                    {
                        string pageHtml = pageCommentsElementParser.Parse(currentPageIndex, pageCount);
                        contentBuilder.Replace(stlPageCommentsElementReplaceString, pageHtml);

                        StlParserManager.ReplacePageElementsInDynamicPage(contentBuilder, pageInfo, stlElementList, pageUrl, pageInfo.PageNodeID, currentPageIndex, pageCount, totalNum, isPageRefresh, ajaxDivID);

                        break;
                    }
                }
            }
            //如果标签中存在<stl:pageinputContents>
            else if (StlParserUtility.IsStlElementExists(StlPageInputContents.ElementName, stlElementList))
            {
                string stlElement = StlParserUtility.GetStlElement(StlPageInputContents.ElementName, stlElementList);
                string stlPageInputContentsElement = stlElement;
                string stlPageInputContentsElementReplaceString = stlElement;

                StlPageInputContents pageInputContentsElementParser = new StlPageInputContents(stlPageInputContentsElement, pageInfo, contextInfo, true);
                int totalNum;
                int pageCount = pageInputContentsElementParser.GetPageCount(out totalNum);

                for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                {
                    if (currentPageIndex == pageIndex)
                    {
                        string pageHtml = pageInputContentsElementParser.Parse(currentPageIndex, pageCount);
                        contentBuilder.Replace(stlPageInputContentsElementReplaceString, pageHtml);

                        StlParserManager.ReplacePageElementsInDynamicPage(contentBuilder, pageInfo, stlElementList, pageUrl, pageInfo.PageNodeID, currentPageIndex, pageCount, totalNum, isPageRefresh, ajaxDivID);

                        break;
                    }
                }
            }
            //如果标签中存在<stl:pagewebsiteMessageContents>
            else if (StlParserUtility.IsStlElementExists(StlPageWebsiteMessageContents.ElementName, stlElementList))
            {
                string stlElement = StlParserUtility.GetStlElement(StlPageWebsiteMessageContents.ElementName, stlElementList);
                string stlPageWebsiteMessageContentsElement = stlElement;
                string stlPageWebsiteMessageContentsElementReplaceString = stlElement;

                StlPageWebsiteMessageContents pageWebsiteMessageContentsElementParser = new StlPageWebsiteMessageContents(stlPageWebsiteMessageContentsElement, pageInfo, contextInfo, true);
                int totalNum;
                int pageCount = pageWebsiteMessageContentsElementParser.GetPageCount(out totalNum);

                for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                {
                    if (currentPageIndex == pageIndex)
                    {
                        string pageHtml = pageWebsiteMessageContentsElementParser.Parse(currentPageIndex, pageCount);
                        contentBuilder.Replace(stlPageWebsiteMessageContentsElementReplaceString, pageHtml);

                        StlParserManager.ReplacePageElementsInDynamicPage(contentBuilder, pageInfo, stlElementList, pageUrl, pageInfo.PageNodeID, currentPageIndex, pageCount, totalNum, isPageRefresh, ajaxDivID);

                        break;
                    }
                }
            }
            //如果标签中存在<stl:pageSqlContents>
            else if (StlParserUtility.IsStlElementExists(StlPageSqlContents.ElementName, stlElementList))
            {
                string stlElement = StlParserUtility.GetStlElement(StlPageSqlContents.ElementName, stlElementList);
                string stlPageSqlContentsElement = stlElement;
                string stlPageSqlContentsElementReplaceString = stlElement;

                StlPageSqlContents pageSqlContentsElementParser = new StlPageSqlContents(stlPageSqlContentsElement, pageInfo, contextInfo, true);
                int totalNum;
                int pageCount = pageSqlContentsElementParser.GetPageCount(out totalNum);

                for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                {
                    if (currentPageIndex == pageIndex)
                    {
                        string pageHtml = pageSqlContentsElementParser.Parse(currentPageIndex, pageCount);
                        contentBuilder.Replace(stlPageSqlContentsElementReplaceString, pageHtml);

                        StlParserManager.ReplacePageElementsInDynamicPage(contentBuilder, pageInfo, stlElementList, pageUrl, pageInfo.PageNodeID, currentPageIndex, pageCount, totalNum, isPageRefresh, ajaxDivID);

                        break;
                    }
                }
            }

            else if (StlParserUtility.IsStlElementExists(StlPageItems.ElementName, stlElementList))
            {
                int pageCount = TranslateUtils.ToInt(queryString["pageCount"]);
                int totalNum = TranslateUtils.ToInt(queryString["totalNum"]);
                string pageContentsAjaxDivID = queryString["pageContentsAjaxDivID"];

                //生成静态页面，超过设置页面数量之后，动态获取pageItem
                if (pageInfo.PublishmentSystemInfo.Additional.CreateStaticMaxPage > 0 && pageCount > pageInfo.PublishmentSystemInfo.Additional.CreateStaticMaxPage)
                {
                    for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                    {
                        if (currentPageIndex == pageIndex)
                        {
                            StlParserManager.ReplacePageElementsInDynamicPage(contentBuilder, pageInfo, stlElementList, pageUrl, pageInfo.PageNodeID, currentPageIndex, pageCount, totalNum, isPageRefresh, pageContentsAjaxDivID);

                            break;
                        }
                    }
                }
            }

            StlParserManager.ParseInnerContent(contentBuilder, pageInfo, contextInfo);

            //string afterBodyScript = StlParserManager.GetPageInfoScript(pageInfo, true);
            //string beforBodyScript = StlParserManager.GetPageInfoScript(pageInfo, false);

            //return afterBodyScript + StlParserUtility.GetBackHtml(contentBuilder.ToString(), pageInfo) + beforBodyScript;

            return StlParserUtility.GetBackHtml(contentBuilder.ToString(), pageInfo);

            //return contentBuilder.ToString();
        }
    }
}
