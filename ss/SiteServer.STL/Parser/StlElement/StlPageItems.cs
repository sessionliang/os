using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using BaiRong.Core;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System;
using SiteServer.STL.StlTemplate;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlPageItems
    {
        private StlPageItems() { }
        public const string ElementName = "stl:pageitems";				//翻页项容器

        public const string Attribute_AutoHide = "autohide";			//无分页时自动隐藏
        public const string Attribute_Context = "context";                  //所处上下文

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_AutoHide, "无分页时自动隐藏");
                attributes.Add(Attribute_Context, "翻页对象");
                return attributes;
            }
        }

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            return Parse(stlElement, pageInfo, 0, 0, 0, 0, 0, contextInfo.ContextType);
        }
        //对“翻页项容器”（stl:pageItems）元素进行解析，此元素在生成页面时单独解析，不包含在ParseStlElement方法中。
        public static string Parse(string stlElement, PageInfo pageInfo, int nodeID, int contentID, int currentPageIndex, int pageCount, int totalNum, EContextType contextType)
        {
            string stlPageItemsString = stlElement;

            pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.A_JQuery);
            string parsedContent = string.Empty;
            try
            {
                XmlDocument xmlDocument = StlParserUtility.GetXmlDocument(stlElement, false);
                XmlNode node = xmlDocument.DocumentElement;
                node = node.FirstChild;

                IEnumerator ie = node.Attributes.GetEnumerator();
                bool autoHide = true;
                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlPageItems.Attribute_AutoHide))
                    {
                        autoHide = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(StlPageItems.Attribute_Context))
                    {
                        contextType = EContextTypeUtils.GetEnumType(attr.Value);
                    }
                }

                if (pageCount <= 1 && autoHide)
                {
                    return string.Empty;
                }

                bool isXmlContent;
                int index = stlElement.IndexOf(">") + 1;
                int length = stlElement.LastIndexOf("<") - index;
                if (index <= 0 || length <= 0)
                {
                    stlElement = node.InnerXml;
                    isXmlContent = true;
                }
                else
                {
                    stlElement = stlElement.Substring(index, length);
                    isXmlContent = false;
                }

                parsedContent = StlPageElementParser.ParseStlPageItems(stlElement, pageInfo, nodeID, contentID, currentPageIndex, pageCount, totalNum, isXmlContent, contextType);

                ContextInfo contextInfo = new ContextInfo(pageInfo);
                parsedContent = ParseDynamicPageItems(parsedContent, stlPageItemsString, pageInfo, contextInfo, pageCount, totalNum);

            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        public static string ParseDynamicPageItems(string parsedContent, string stlPageItemsElement, PageInfo pageInfo, ContextInfo contextInfo, int pageCount, int totalNum)
        {
            if (pageInfo.PublishmentSystemInfo.Additional.CreateStaticMaxPage > 0 && pageCount > pageInfo.PublishmentSystemInfo.Additional.CreateStaticMaxPage)
            {
                string ajaxDivID = StlParserUtility.GetAjaxDivID(pageInfo.UniqueID);
                parsedContent = string.Format(@"<span id=""{0}"">{1}</span>", ajaxDivID, parsedContent);

                string outputPageUrl = StlTemplateManager.Dynamic.GetPageUrlOutput(pageInfo.PublishmentSystemInfo);
                string currentPageUrl = StlUtility.GetStlCurrentUrl(pageInfo, contextInfo.ChannelID, contextInfo.ContentID, contextInfo.ContentInfo);
                currentPageUrl = PageUtils.AddQuestionOrAndToUrl(currentPageUrl);
                string outputParms = StlTemplateManager.Dynamic.GetOutputParameters(contextInfo.ChannelID, contextInfo.ContentID, pageInfo.TemplateInfo.TemplateID, currentPageUrl, ajaxDivID, false, stlPageItemsElement);
                string pageScripts = string.Format(@"
<script type=""text/javascript"" language=""javascript"">
var stlDynamicPageItemNum = {3};
var pageCount = {4};
var totalNum = {5};
function stlDynamicPageItem(pageNum)
{{
    try
    {{
        if(pageNum=='nextPage'){{
            stlDynamicPageItemNum++;
        }}
        else if(pageNum=='previousPage'){{
            stlDynamicPageItemNum--;
        }}
        else{{
            stlDynamicPageItemNum=pageNum;
        }}

        var url = ""{0}&pageCount="" + pageCount + ""&totalNum="" + totalNum;
        if(typeof(pageContentsAjaxDivID)!=""undefined""){{
            url += ""&pageContentsAjaxDivID="" + pageContentsAjaxDivID;
        }}
        else{{
            url += ""&pageContentsAjaxDivID=page"";
        }}
        var pars = ""{1}&pageNum="" + stlDynamicPageItemNum;
        jQuery.post(url, pars, function(data,textStatus){{
            jQuery(""#{2}"").html(data);
        }});
    }}catch(e){{}}
}}
</script>
", outputPageUrl, outputParms, ajaxDivID, pageInfo.PublishmentSystemInfo.Additional.CreateStaticMaxPage, pageCount, totalNum);
                //parsedContent += pageScripts;
                pageInfo.AddPageScriptsIfNotExists(ajaxDivID, pageScripts);
            }
            return parsedContent;
        }

        public static string ParseInSearchPage(string stlElement, PageInfo pageInfo, string ajaxDivID, int nodeID, int currentPageIndex, int pageCount, int totalNum)
        {
            string parsedContent = string.Empty;
            try
            {
                XmlDocument xmlDocument = StlParserUtility.GetXmlDocument(stlElement, false);
                XmlNode node = xmlDocument.DocumentElement;
                node = node.FirstChild;

                IEnumerator ie = node.Attributes.GetEnumerator();
                bool autoHide = true;
                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(Attribute_AutoHide))
                    {
                        autoHide = TranslateUtils.ToBool(attr.Value);
                    }
                }

                if (pageCount <= 1 && autoHide)
                {
                    return string.Empty;
                }

                //bool isXmlContent;
                int index = stlElement.IndexOf(">") + 1;
                int length = stlElement.LastIndexOf("<") - index;
                if (index <= 0 || length <= 0)
                {
                    stlElement = node.InnerXml;
                    //isXmlContent = true;
                }
                else
                {
                    stlElement = stlElement.Substring(index, length);
                    //isXmlContent = false;
                }

                parsedContent = StlPageElementParser.ParseStlPageItemsInSearchPage(stlElement, pageInfo, ajaxDivID, nodeID, currentPageIndex, pageCount, totalNum);
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        public static string ParseInDynamicPage(string stlElement, PageInfo pageInfo, string pageUrl, int nodeID, int currentPageIndex, int pageCount, int totalNum, bool isPageRefresh, string ajaxDivID)
        {
            string parsedContent = string.Empty;
            try
            {
                XmlDocument xmlDocument = StlParserUtility.GetXmlDocument(stlElement, false);
                XmlNode node = xmlDocument.DocumentElement;
                node = node.FirstChild;

                IEnumerator ie = node.Attributes.GetEnumerator();
                bool autoHide = true;
                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(Attribute_AutoHide))
                    {
                        autoHide = TranslateUtils.ToBool(attr.Value);
                    }
                }

                if (pageCount <= 1 && autoHide)
                {
                    return string.Empty;
                }

                int index = stlElement.IndexOf(">") + 1;
                int length = stlElement.LastIndexOf("<") - index;
                if (index <= 0 || length <= 0)
                {
                    stlElement = node.InnerXml;
                }
                else
                {
                    stlElement = stlElement.Substring(index, length);
                }

                parsedContent = StlPageElementParser.ParseStlPageItemsInDynamicPage(stlElement, pageInfo, pageUrl, nodeID, currentPageIndex, pageCount, totalNum, isPageRefresh, ajaxDivID);
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }
    }
}
