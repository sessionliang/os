using System.Collections;
using System.Text;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser.StlElement;
using SiteServer.CMS.Model;
using BaiRong.Core;
using System.Collections.Generic;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser
{
    /// <summary>
    /// Stl½âÎöÆ÷
    /// </summary>
    public class StlParserManager
    {
        private StlParserManager()
        {
        }

        public static string ParsePreviewContent(PublishmentSystemInfo publishmentSystemInfo, string content)
        {
            TemplateInfo templateInfo = new TemplateInfo();
            PageInfo pageInfo = new PageInfo(templateInfo, publishmentSystemInfo.PublishmentSystemID, publishmentSystemInfo.PublishmentSystemID, 0, publishmentSystemInfo, EVisualType.Static);
            ContextInfo contextInfo = new ContextInfo(pageInfo);

            StringBuilder parsedBuilder = new StringBuilder(content);

            StlElementParser.ReplaceStlElements(parsedBuilder, pageInfo, contextInfo);
            StlEntityParser.ReplaceStlEntities(parsedBuilder, pageInfo, contextInfo);

            string pageAfterBodyScripts = StlParserManager.GetPageInfoScript(pageInfo, true);
            string pageBeforeBodyScripts = StlParserManager.GetPageInfoScript(pageInfo, false);

            return pageAfterBodyScripts + parsedBuilder.ToString() + pageBeforeBodyScripts;
        }

        public static void ParseTemplateContent(StringBuilder parsedBuilder, PageInfo pageInfo, ContextInfo contextInfo)
        {
            bool isInnerElement = contextInfo.IsInnerElement;
            contextInfo.IsInnerElement = false;
            contextInfo.ContainerClientID = string.Empty;
            StlElementParser.ReplaceStlElements(parsedBuilder, pageInfo, contextInfo);
            StlEntityParser.ReplaceStlEntities(parsedBuilder, pageInfo, contextInfo);
            contextInfo.IsInnerElement = isInnerElement;
        }

        public static void ParseInnerContent(StringBuilder parsedBuilder, PageInfo pageInfo, ContextInfo contextInfo)
        {
            bool isInnerElement = contextInfo.IsInnerElement;
            contextInfo.IsInnerElement = true;
            StlElementParser.ReplaceStlElements(parsedBuilder, pageInfo, contextInfo);
            StlEntityParser.ReplaceStlEntities(parsedBuilder, pageInfo, contextInfo);
            contextInfo.IsInnerElement = isInnerElement;
        }

        public static void ReplacePageElementsInContentPage(StringBuilder parsedBuilder, PageInfo pageInfo, List<string> labelList, int nodeID, int contentID, int currentPageIndex, int pageCount)
        {
            //Ìæ»»·ÖÒ³Ä£°å
            foreach (string labelString in labelList)
            {
                if (StlParserUtility.IsSpecifiedStlElement(labelString, StlPageItems.ElementName))
                {
                    string stlElement = labelString;
                    string pageHtml = StlPageElementParser.ParseStlPageInContentPage(stlElement, pageInfo, nodeID, contentID, currentPageIndex, pageCount);
                    parsedBuilder.Replace(stlElement, pageHtml);
                }
                else if (StlParserUtility.IsSpecifiedStlElement(labelString, StlPageItem.ElementName))
                {
                    string stlElement = labelString;
                    string pageHtml = StlPageElementParser.ParseStlPageItemInContentPage(stlElement, pageInfo, nodeID, contentID, currentPageIndex, pageCount, pageCount);
                    parsedBuilder.Replace(stlElement, pageHtml);
                }
            }
        }

        public static void ReplacePageElementsInChannelPage(StringBuilder parsedBuilder, PageInfo pageInfo, List<string> labelList, int nodeID, int currentPageIndex, int pageCount, int totalNum)
        {
            //Ìæ»»·ÖÒ³Ä£°å
            foreach (string labelString in labelList)
            {
                if (StlParserUtility.IsSpecifiedStlElement(labelString, StlPageItems.ElementName))
                {
                    string stlElement = labelString;
                    string pageHtml = StlPageElementParser.ParseStlPageInChannelPage(stlElement, pageInfo, nodeID, currentPageIndex, pageCount, totalNum);
                    parsedBuilder.Replace(stlElement, pageHtml);
                }
                else if (StlParserUtility.IsSpecifiedStlElement(labelString, StlPageItem.ElementName))
                {
                    string stlElement = labelString;
                    string pageHtml = StlPageElementParser.ParseStlPageItemInChannelPage(stlElement, pageInfo, nodeID, currentPageIndex, pageCount, totalNum);
                    parsedBuilder.Replace(stlElement, pageHtml);
                }
            }
        }

        public static void ReplacePageElementsInSearchPage(StringBuilder parsedBuilder, PageInfo pageInfo, List<string> labelList, string ajaxDivID, int nodeID, int currentPageIndex, int pageCount, int totalNum)
        {
            //Ìæ»»·ÖÒ³Ä£°å
            foreach (string labelString in labelList)
            {
                if (StlParserUtility.IsSpecifiedStlElement(labelString, StlPageItems.ElementName))
                {
                    string stlElement = labelString;
                    string pageHtml = StlPageElementParser.ParseStlPageInSearchPage(stlElement, pageInfo, ajaxDivID, nodeID, currentPageIndex, pageCount, totalNum);
                    parsedBuilder.Replace(stlElement, pageHtml);
                }
                else if (StlParserUtility.IsSpecifiedStlElement(labelString, StlPageItem.ElementName))
                {
                    string stlElement = labelString;
                    string pageHtml = StlPageElementParser.ParseStlPageItemInSearchPage(stlElement, pageInfo, ajaxDivID, nodeID, currentPageIndex, pageCount, totalNum);
                    parsedBuilder.Replace(stlElement, pageHtml);
                }
            }
        }

        public static void ReplacePageElementsInDynamicPage(StringBuilder parsedBuilder, PageInfo pageInfo, List<string> labelList, string pageUrl, int nodeID, int currentPageIndex, int pageCount, int totalNum, bool isPageRefresh, string ajaxDivID)
        {
            //Ìæ»»·ÖÒ³Ä£°å
            foreach (string labelString in labelList)
            {
                if (StlParserUtility.IsSpecifiedStlElement(labelString, StlPageItems.ElementName))
                {
                    string stlElement = labelString;
                    string pageHtml = StlPageElementParser.ParseStlPageInDynamicPage(stlElement, pageInfo, pageUrl, nodeID, currentPageIndex, pageCount, totalNum, isPageRefresh, ajaxDivID);
                    parsedBuilder.Replace(stlElement, pageHtml);
                }
                else if (StlParserUtility.IsSpecifiedStlElement(labelString, StlPageItem.ElementName))
                {
                    string stlElement = labelString;
                    string pageHtml = StlPageElementParser.ParseStlPageItemInDynamicPage(stlElement, pageInfo, pageUrl, nodeID, currentPageIndex, pageCount, totalNum, isPageRefresh, ajaxDivID);
                    parsedBuilder.Replace(stlElement, pageHtml);
                }
            }
        }

        public static string GetPageInfoHeadScript(PageInfo pageInfo, ContextInfo contextInfo)
        {
            StringBuilder builder = new StringBuilder();

            //update by liangjian at 20150727, ÉèÖÃAPI·ÃÎÊÂ·¾¶£¬·½±ãAPI·ÖÀë²¿Êð
            builder.AppendFormat(@"<script>var $pageInfo = {{ publishmentSystemID : {0}, channelID : {1}, contentID : {2}, siteUrl : ""{3}"", homeUrl : ""{4}"", currentUrl : ""{5}"", rootUrl : ""{6}"", apiUrl : ""{7}"" }}</script>
", pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.PageContentID, pageInfo.PublishmentSystemInfo.PublishmentSystemUrl.TrimEnd('/'), PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, pageInfo.PublishmentSystemInfo.Additional.HomeUrl).TrimEnd('/'), StlUtility.GetStlCurrentUrl(pageInfo, contextInfo.ChannelID, contextInfo.ContentID, contextInfo.ContentInfo), PageUtils.GetRootUrl(string.Empty).TrimEnd('/'), PageUtility.IsCorsCrossDomain(pageInfo.PublishmentSystemInfo) ? PageUtils.AddProtocolToUrl(pageInfo.PublishmentSystemInfo.Additional.APIUrl) : PageUtils.GetRootUrl("/api").TrimEnd('/'));

            //add by liangjian at 20150817, api¿çÓò·ÃÎÊ xdomain
            //            if (PageUtility.IsCorsCrossDomain(pageInfo.PublishmentSystemInfo))
            //            {
            //                builder.AppendFormat(@"<script src='/{1}/utils/xdomain.min.js'></script>
            //<script>xdomain.slaves({{'{0}':'/{1}/utils/proxy_{2}.html'}}); </script>
            //", PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, pageInfo.PublishmentSystemInfo.Additional.APIUrl)), pageInfo.PublishmentSystemInfo.PublishmentSystemDir, pageInfo.PublishmentSystemInfo.PublishmentSystemID);
            //            }
            foreach (string key in pageInfo.PageHeadScriptKeys)
            {
                string js = pageInfo.GetPageHeadScripts(key) as string;
                if (!string.IsNullOrEmpty(js))
                {
                    builder.Append(js);
                }
            }

            return builder.ToString();
        }

        public static string GetPageInfoScript(PageInfo pageInfo, bool isAfterBody)
        {
            StringBuilder scriptBuilder = new StringBuilder();

            if (isAfterBody)
            {
                foreach (string key in pageInfo.PageAfterBodyScriptKeys)
                {
                    scriptBuilder.Append(pageInfo.GetPageScripts(key, true));
                }
            }
            else
            {
                foreach (string key in pageInfo.PageBeforeBodyScriptKeys)
                {
                    scriptBuilder.Append(pageInfo.GetPageScripts(key, false));
                }
            }

            return scriptBuilder.ToString();
        }
    }
}
