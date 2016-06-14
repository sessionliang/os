using System.Collections;
using System.Text;
using System.Xml;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser.StlElement;
using SiteServer.CMS.Model;
using System;
using BaiRong.Core;
using System.Web;
using SiteServer.STL.Parser.TemplateDesign;
using System.Collections.Generic;

namespace SiteServer.STL.Parser
{
    /// <summary>
    /// Stl元素解析器
    /// </summary>
    public class StlElementParser
    {
        private StlElementParser()
        {
        }

        /// <summary>
        /// 将原始内容中的STL元素替换为实际内容
        /// </summary>
        public static void ReplaceStlElements(StringBuilder parsedBuilder, PageInfo pageInfo, ContextInfo contextInfo)
        {
            List<string> stlElementArrayList = StlParserUtility.GetStlElementList(parsedBuilder.ToString());
            int stlSequence = 0;
            foreach (string stlElement in stlElementArrayList)
            {
                try
                {
                    int startIndex = parsedBuilder.ToString().IndexOf(stlElement);
                    if (startIndex != -1)
                    {
                        //DateTime start = DateTime.Now;
                        bool isDesignVisible = false;
                        string parsedContent = StlElementParser.ParseStlElement(stlElement, pageInfo, contextInfo, out isDesignVisible);

                        if (pageInfo.VisualType == EVisualType.Design && (contextInfo.IsInnerElement == false || isDesignVisible))
                        {
                            string includeUrl = HttpContext.Current.Request.QueryString["includeUrl"];
                            parsedContent = TemplateDesignManager.ParseStlElement(pageInfo, includeUrl, stlElement, parsedContent, stlSequence++, contextInfo.IsInnerElement);
                        }
                        //                        DateTime end = DateTime.Now;
                        //                        int retval = DateDiff(end, start);
                        //                        if (retval > 5)
                        //                        {
                        //                            resultContent += string.Format(@"
                        //<!-- {0} --><!-- {1} -->
                        //", stlElement, DateDiff(end, start));
                        //}
                        parsedBuilder.Replace(stlElement, parsedContent, startIndex, stlElement.Length);
                    }
                }
                catch { }
            }
        }

        //private static int DateDiff(DateTime DateTime1, DateTime DateTime2)
        //{
        //    int retval = 0;
        //    try
        //    {
        //        TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
        //        TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
        //        TimeSpan ts = ts1.Subtract(ts2).Duration();
        //        retval = ts.Milliseconds;                
        //    }
        //    catch
        //    {

        //    }
        //    return retval;
        //}

        readonly static Dictionary<string, Func<string, XmlNode, PageInfo, ContextInfo, string>> ElementDic = InitElementDic();

        private static Dictionary<string, Func<string, XmlNode, PageInfo, ContextInfo, string>> InitElementDic()
        {
            var dic = new Dictionary<string, Func<string, XmlNode, PageInfo, ContextInfo, string>>();
            dic.Add(StlA.ElementName, StlA.Parse);
            dic.Add(StlAction.ElementName, StlAction.Parse);
            dic.Add(StlAd.ElementName, StlAd.Parse);
            dic.Add(StlAnalysis.ElementName, StlAnalysis.Parse);
            dic.Add(StlAudio.ElementName, StlAudio.Parse);
            dic.Add(StlChannel.ElementName, StlChannel.Parse);
            dic.Add(StlChannels.ElementName, StlChannels.Parse);
            dic.Add(StlComment.ElementName, StlComment.Parse);
            dic.Add(StlCommentInput.ElementName, StlCommentInput.Parse);
            dic.Add(StlComments.ElementName, StlComments.Parse);
            dic.Add(StlContainer.ElementName, StlContainer.Parse);
            dic.Add(StlContent.ElementName, StlContent.Parse);
            dic.Add(StlContentInput.ElementName, StlContentInput.Parse);
            dic.Add(StlContents.ElementName, StlContents.Parse);
            dic.Add(StlCount.ElementName, StlCount.Parse);
            dic.Add(StlCompareInput.ElementName, StlCompareInput.Parse);
            dic.Add(StlDigg.ElementName, StlDigg.Parse);
            dic.Add(StlDynamic.ElementName, StlDynamic.Parse);
            dic.Add(StlEach.ElementName, StlEach.Parse);
            dic.Add(StlEvaluationInput.ElementName, StlEvaluationInput.Parse);
            dic.Add(StlFile.ElementName, StlFile.Parse);
            dic.Add(StlFlash.ElementName, StlFlash.Parse);
            dic.Add(StlFocusViewer.ElementName, StlFocusViewer.Parse);
            dic.Add(StlIf.ElementName, StlIf.Parse);
            dic.Add(StlImage.ElementName, StlImage.Parse);
            dic.Add(StlInclude.ElementName, StlInclude.Parse);
            dic.Add(StlInput.ElementName, StlInput.Parse);
            dic.Add(StlInputContent.ElementName, StlInputContent.Parse);
            dic.Add(StlInputContents.ElementName, StlInputContents.Parse);
            dic.Add(StlIPushContents.ElementName, StlIPushContents.Parse);
            dic.Add(StlWebsiteMessage.ElementName, StlWebsiteMessage.Parse);
            dic.Add(StlWebsiteMessage.SimpleElementName, StlWebsiteMessage.Parse);
            dic.Add(StlWebsiteMessageContent.ElementName, StlWebsiteMessageContent.Parse);
            dic.Add(StlWebsiteMessageContent.SimpleElementName, StlWebsiteMessageContent.Parse);
            dic.Add(StlWebsiteMessageContents.ElementName, StlWebsiteMessageContents.Parse);
            dic.Add(StlWebsiteMessageContents.SimpleElementName, StlWebsiteMessageContents.Parse);
            dic.Add(StlLayout.ElementName, StlLayout.Parse);
            dic.Add(StlLocation.ElementName, StlLocation.Parse);
            dic.Add(StlMarquee.ElementName, StlMarquee.Parse);
            dic.Add(StlMenu.ElementName, StlMenu.Parse);
            dic.Add(StlNavigation.ElementName, StlNavigation.Parse);
            dic.Add(StlPageItems.ElementName, StlPageItems.Parse);
            dic.Add(StlPhoto.ElementName, StlPhoto.Parse);
            dic.Add(StlTeleplay.ElementName, StlTeleplay.Parse);
            dic.Add(StlPlayer.ElementName, StlPlayer.Parse);
            dic.Add(StlPrinter.ElementName, StlPrinter.Parse);
            dic.Add(StlResume.ElementName, StlResume.Parse);
            dic.Add(StlRss.ElementName, StlRss.Parse);
            dic.Add(StlSearchInput.ElementName, StlSearchInput.Parse);
            dic.Add(StlSearchOutput.ElementName, StlSearchOutput.Parse);
            dic.Add(StlSearchwordInput.ElementName, StlSearchwordInput.Parse);
            dic.Add(StlSearchwordInput.SimpleElementName, StlSearchwordInput.Parse);
            dic.Add(StlSelect.ElementName, StlSelect.Parse);
            dic.Add(StlSite.ElementName, StlSite.Parse);
            dic.Add(StlSites.ElementName, StlSites.Parse);
            dic.Add(StlSlide.ElementName, StlSlide.Parse);
            dic.Add(StlSqlContent.ElementName, StlSqlContent.Parse);
            dic.Add(StlSqlContents.ElementName, StlSqlContents.Parse);
            dic.Add(StlStar.ElementName, StlStar.Parse);
            dic.Add(StlSurveyInput.ElementName, StlSurveyInput.Parse);
            dic.Add(StlTabs.ElementName, StlTabs.Parse);
            dic.Add(StlTags.ElementName, StlTags.Parse);
            dic.Add(StlTree.ElementName, StlTree.Parse);
            dic.Add(StlTrialApplyInput.ElementName, StlTrialApplyInput.Parse);
            dic.Add(StlUser.ElementName, StlUser.Parse);
            dic.Add(StlValue.ElementName, StlValue.Parse);
            dic.Add(StlVideo.ElementName, StlVideo.Parse);
            dic.Add(StlVisible.ElementName, StlVisible.Parse);
            dic.Add(StlVote.ElementName, StlVote.Parse);
            dic.Add(StlZoom.ElementName, StlZoom.Parse);
            dic.Add(StlSubscribe.ElementName, StlSubscribe.Parse);
            dic.Add(StlSubscribe.SimpleElementName, StlSubscribe.Parse);

            #region B2C
            dic.Add(StlFilter.ElementName, StlFilter.Parse);
            dic.Add(StlFilters.ElementName, StlFilters.Parse);
            dic.Add(StlSpec.ElementName, StlSpec.Parse);
            dic.Add(StlSpecs.ElementName, StlSpecs.Parse);
            #endregion

            #region WCM
            dic.Add(StlGovInteractApply.ElementName, StlGovInteractApply.Parse);
            dic.Add(StlGovInteractQuery.ElementName, StlGovInteractQuery.Parse);
            dic.Add(StlGovPublicApply.ElementName, StlGovPublicApply.Parse);
            dic.Add(StlGovPublicQuery.ElementName, StlGovPublicQuery.Parse);
            #endregion

            #region Home
            dic.Add(StlChangePwd.ElementName, StlChangePwd.Parse);
            #endregion

            return dic;
        }

        internal static string ParseStlElement(string stlElement, PageInfo pageInfo, ContextInfo contextInfo, out bool isDesignVisible)
        {
            string parsedContent;
            XmlDocument xmlDocument = StlParserUtility.GetXmlDocument(stlElement, contextInfo.IsInnerElement);
            XmlNode node = xmlDocument.DocumentElement;
            node = node.FirstChild;
            isDesignVisible = false;

            if (node != null && node.Name != null)
            {
                string elementName = node.Name.ToLower();

                #region Old version，use switch
                //StlScriptUtility.RegisteScript(pageInfo, elementName);

                //switch (elementName)
                //{
                //    case StlA.ElementName:
                //        parsedContent = StlA.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlAction.ElementName:
                //        parsedContent = StlAction.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlAd.ElementName:
                //        parsedContent = StlAd.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlAnalysis.ElementName:
                //        parsedContent = StlAnalysis.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlAudio.ElementName:
                //        parsedContent = StlAudio.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlChannel.ElementName:
                //        parsedContent = StlChannel.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlChannels.ElementName:
                //        parsedContent = StlChannels.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlComment.ElementName:
                //        parsedContent = StlComment.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlCommentInput.ElementName:
                //        parsedContent = StlCommentInput.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlComments.ElementName:
                //        parsedContent = StlComments.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlContainer.ElementName:
                //        parsedContent = StlContainer.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlContent.ElementName:
                //        parsedContent = StlContent.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlContentInput.ElementName:
                //        parsedContent = StlContentInput.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlContents.ElementName:
                //        parsedContent = StlContents.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    /// <summary>
                //    ///  by 20151125 sofuny
                //    /// 培生智能推送
                //    /// 增加智能推送内容列表标签
                //    /// </summary>
                //    case StlIPushContents.ElementName:
                //        parsedContent = StlIPushContents.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlCount.ElementName:
                //        parsedContent = StlCount.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlDigg.ElementName:
                //        parsedContent = StlDigg.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlDynamic.ElementName:
                //        parsedContent = StlDynamic.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlEach.ElementName:
                //        parsedContent = StlEach.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlFile.ElementName:
                //        parsedContent = StlFile.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlFlash.ElementName:
                //        parsedContent = StlFlash.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlFocusViewer.ElementName:
                //        parsedContent = StlFocusViewer.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlIf.ElementName:
                //        parsedContent = StlIf.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlImage.ElementName:
                //        parsedContent = StlImage.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlInclude.ElementName:
                //        parsedContent = StlInclude.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlInput.ElementName:
                //        parsedContent = StlInput.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlInputContent.ElementName:
                //        parsedContent = StlInputContent.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlInputContents.ElementName:
                //        parsedContent = StlInputContents.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlWebsiteMessage.ElementName:
                //        parsedContent = StlWebsiteMessage.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlWebsiteMessage.SimpleElementName:
                //        parsedContent = StlWebsiteMessage.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlWebsiteMessageContent.ElementName:
                //        parsedContent = StlWebsiteMessageContent.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlWebsiteMessageContent.SimpleElementName:
                //        parsedContent = StlWebsiteMessageContent.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlWebsiteMessageContents.ElementName:
                //        parsedContent = StlWebsiteMessageContents.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlWebsiteMessageContents.SimpleElementName:
                //        parsedContent = StlWebsiteMessageContents.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlLayout.ElementName:
                //        parsedContent = StlLayout.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlLocation.ElementName:
                //        parsedContent = StlLocation.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlMarquee.ElementName:
                //        parsedContent = StlMarquee.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlMenu.ElementName:
                //        parsedContent = StlMenu.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlNavigation.ElementName:
                //        parsedContent = StlNavigation.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlPageItems.ElementName:
                //        parsedContent = StlPageItems.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlPhoto.ElementName:
                //        parsedContent = StlPhoto.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlTeleplay.ElementName:
                //        parsedContent = StlTeleplay.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlPlayer.ElementName:
                //        parsedContent = StlPlayer.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlPrinter.ElementName:
                //        parsedContent = StlPrinter.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlResume.ElementName:
                //        parsedContent = StlResume.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlRss.ElementName:
                //        parsedContent = StlRss.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlSearchInput.ElementName:
                //        parsedContent = StlSearchInput.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlSearchOutput.ElementName:
                //        parsedContent = StlSearchOutput.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    #region 站内搜索关键词
                //    case StlSearchwordInput.ElementName:
                //        parsedContent = StlSearchwordInput.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlSearchwordInput.SimpleElementName:
                //        parsedContent = StlSearchwordInput.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    #endregion
                //    case StlSelect.ElementName:
                //        parsedContent = StlSelect.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlSite.ElementName:
                //        parsedContent = StlSite.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlSites.ElementName:
                //        parsedContent = StlSites.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlSlide.ElementName:
                //        parsedContent = StlSlide.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlSqlContent.ElementName:
                //        parsedContent = StlSqlContent.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlSqlContents.ElementName:
                //        parsedContent = StlSqlContents.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlStar.ElementName:
                //        parsedContent = StlStar.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlTabs.ElementName:
                //        parsedContent = StlTabs.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlTags.ElementName:
                //        parsedContent = StlTags.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlTree.ElementName:
                //        parsedContent = StlTree.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlUser.ElementName:
                //        parsedContent = StlUser.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlValue.ElementName:
                //        parsedContent = StlValue.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlVideo.ElementName:
                //        parsedContent = StlVideo.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlVisible.ElementName:
                //        parsedContent = StlVisible.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlVote.ElementName:
                //        parsedContent = StlVote.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlZoom.ElementName:
                //        parsedContent = StlZoom.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlSubscribe.ElementName:
                //        parsedContent = StlSubscribe.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlSubscribe.SimpleElementName:
                //        parsedContent = StlSubscribe.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;

                //    #region B2C
                //    case StlFilter.ElementName:
                //        parsedContent = StlFilter.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlFilters.ElementName:
                //        parsedContent = StlFilters.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlSpec.ElementName:
                //        parsedContent = StlSpec.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    case StlSpecs.ElementName:
                //        parsedContent = StlSpecs.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    #endregion

                //    #region WCM
                //    case StlGovInteractApply.ElementName:
                //        parsedContent = StlGovInteractApply.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlGovInteractQuery.ElementName:
                //        parsedContent = StlGovInteractQuery.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlGovPublicApply.ElementName:
                //        parsedContent = StlGovPublicApply.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    case StlGovPublicQuery.ElementName:
                //        parsedContent = StlGovPublicQuery.Parse(stlElement, node, pageInfo, contextInfo);
                //        isDesignVisible = true;
                //        break;
                //    #endregion

                //    #region Home
                //    case StlChangePwd.ElementName:
                //        parsedContent = StlChangePwd.Parse(stlElement, node, pageInfo, contextInfo);
                //        break;
                //    #endregion

                //    default:
                //        parsedContent = stlElement;
                //        break;
                //} 
                #endregion

                #region New version, use dictionary, exchange time with space, but initional is slower
                Func<string, XmlNode, PageInfo, ContextInfo, string> func;
                if (ElementDic.TryGetValue(elementName, out func))
                {
                    parsedContent = func(stlElement, node, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = stlElement;
                }
                #endregion
            }
            else
            {
                parsedContent = stlElement;
            }

            //return parsedContent;
            if (contextInfo.IsInnerElement)
            {
                return parsedContent;
            }
            else
            {
                return StlParserUtility.GetBackHtml(parsedContent, pageInfo);
            }
        }
    }
}
