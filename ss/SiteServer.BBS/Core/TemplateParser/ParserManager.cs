using System;
using System.Collections.Generic;
using System.Text;
using SiteServer.BBS.Core.TemplateParser.Model;
using SiteServer.BBS.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.BBS.Core.TemplateParser
{
    public class ParserManager
    {
        private ParserManager()
        {
        }

        public static void ParseTemplateContent(StringBuilder parsedBuilder, PageInfo pageInfo, ContextInfo contextInfo)
        {
            contextInfo.IsInnerElement = false;
            contextInfo.ContainerClientID = string.Empty;
            ElementParser.ReplaceElements(parsedBuilder, pageInfo , contextInfo);
            EntityParser.ReplaceEntities(parsedBuilder, pageInfo, contextInfo);
        }

        public static void ParseInnerContent(StringBuilder parsedBuilder, PageInfo pageInfo, ContextInfo contextInfo)
        {
            bool isInnerElement = contextInfo.IsInnerElement;
            contextInfo.IsInnerElement = true;
            ElementParser.ReplaceElements(parsedBuilder, pageInfo, contextInfo);
            EntityParser.ReplaceEntities(parsedBuilder, pageInfo, contextInfo);
            contextInfo.IsInnerElement = isInnerElement;
        }

        public static string ParseDynamicContent(int publishmentSystemID, string directoryName, string fileName, int forumID, int threadID, ETemplateType templateType, bool isPageRefresh, string templateContent, string pageUrl, int pageIndex, string ajaxDivID, NameValueCollection queryString)
        {
            if (queryString.Count > 0)
            {
                foreach (string key in queryString.Keys)
                {
                    templateContent = StringUtils.ReplaceIgnoreCase(templateContent, string.Format("{{bbs.request.{0}}}", key), PageUtils.UrlDecode(queryString[key]));
                }
            }
            templateContent = RegexUtils.Replace("{bbs.request.[^}]+}", templateContent, string.Empty);
            StringBuilder contentBuilder = new StringBuilder(templateContent);


            PageInfo pageInfo = new PageInfo(publishmentSystemID, templateType, directoryName, fileName, forumID, threadID);
            pageInfo.SetUniqueID(1000);
            ContextInfo contextInfo = new ContextInfo(pageInfo);
            ArrayList elementArrayList = ParserUtility.GetElementArrayList(contentBuilder.ToString());

            ParserManager.ParseInnerContent(contentBuilder, pageInfo, contextInfo);

            return ParserUtility.GetBackHtml(contentBuilder.ToString());
        }
    }
}
