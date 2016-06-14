using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Text.Sgml;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser.StlElement;
using SiteServer.STL.Parser.StlEntity;
using SiteServer.CMS.Model;
using System;
using SiteServer.CMS.Services;
using System.Collections.Generic;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser
{
    public class StlParserUtility
    {
        public const string Order_Default = "Default";								//默认排序
        public const string Order_Back = "Back";								//默认排序的相反方向
        public const string Order_AddDate = "AddDate";							//添加时间
        public const string Order_AddDateBack = "AddDateBack";					//添加时间的相反方向
        public const string Order_LastEditDate = "LastEditDate";				//最后更改时间（只可用于内容列表）
        public const string Order_LastEditDateBack = "LastEditDateBack";	//最后更改时间的相反方向（只可用于内容列表）
        public const string Order_Hits = "Hits";	            //点击量
        public const string Order_HitsByDay = "HitsByDay";	    //日点击量
        public const string Order_HitsByWeek = "HitsByWeek";	//周点击量
        public const string Order_HitsByMonth = "HitsByMonth";	//月点击量
        public const string Order_Stars = "Stars";	            //评分数
        public const string Order_Digg = "Digg";	            //Digg数
        public const string Order_Comments = "Comments";        //评论数
        public const string Order_Random = "Random";            //随机

        private const string xmlDeclaration = "<?xml version='1.0'?>";

        private const string xmlNamespaceStart = "<stl:document xmlns=\"http://www.siteserver.cn/stl\" xmlns:stl=\"http://www.siteserver.cn/stl\" xmlns:STL=\"http://www.siteserver.cn/stl\" xmlns:sTL=\"http://www.siteserver.cn/stl\" xmlns:stL=\"http://www.siteserver.cn/stl\" xmlns:sTl=\"http://www.siteserver.cn/stl\" xmlns:Stl=\"http://www.siteserver.cn/stl\" xmlns:StL=\"http://www.siteserver.cn/stl\" xmlns:asp=\"http://www.siteserver.cn/stl\" xmlns:ext=\"http://www.siteserver.cn/stl\">";
        private const string xmlNamespaceEnd = "</stl:document>";

        public static string ReplaceXmlNamespace(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                return content.Replace(@" xmlns=""http://www.siteserver.cn/stl""", string.Empty).Replace(@" xmlns:stl=""http://www.siteserver.cn/stl""", string.Empty);
            }
            return string.Empty;
        }

        public static XmlDocument GetXmlDocument(string element, bool isXmlContent)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.PreserveWhitespace = true;
            try
            {
                if (isXmlContent)
                {
                    xmlDocument.LoadXml(xmlDeclaration + xmlNamespaceStart + element + xmlNamespaceEnd);
                }
                else
                {
                    xmlDocument.LoadXml(xmlDeclaration + xmlNamespaceStart + HtmlToXml(element) + xmlNamespaceEnd);
                }
            }
            catch { }
            //catch(Exception e)
            //{
            //    TraceUtils.Warn(e.ToString());
            //    throw e;
            //}
            return xmlDocument;
        }

        //还原Html转换为Xml时无法保留的特定字符
        public static string GetBackHtml(string content, PageInfo pageInfo)
        {
            if (content != null)
            {
                content = content.Replace(ContentUtility.PagePlaceHolder, string.Empty);
                content = content.Replace("&quot;", "'");
                content = content.Replace("&gt;", ">");
                content = content.Replace("&lt;", "<");
                content = content.Replace("&amp;", "&");
                content = content.Replace(" xmlns=\"http://www.siteserver.cn/stl\"", string.Empty);
                content = content.Replace(" xmlns:stl=\"http://www.siteserver.cn/stl\"", string.Empty);
                content = content.Replace(" xmlns:asp=\"http://www.siteserver.cn/stl\"", string.Empty);
                content = content.Replace("&amp;#", "&#");
                if (pageInfo != null && pageInfo.TemplateInfo != null)//&& pageInfo.TemplateInfo.TemplateType != ETemplateType.FileTemplate
                {
                    content = content.Replace("<![CDATA[", string.Empty);
                    content = content.Replace("]]>", string.Empty);
                }
            }
            return content;
        }

        public static void XmlToHtml(StringBuilder builder)
        {
            builder.Replace("&quot;", "'");
            builder.Replace("&gt;", ">");
            builder.Replace("&lt;", "<");
            builder.Replace("&amp;", "&");
        }

        public static string XmlToHtml(string content)
        {
            if (content != null)
            {
                content = content.Replace("&quot;", "'");
                content = content.Replace("&gt;", ">");
                content = content.Replace("&lt;", "<");
                content = content.Replace("&amp;", "&");
            }
            return content;
        }

        public static string Amp(string content)
        {
            if (content != null)
            {
                content = content.Replace("&", "&amp;");
            }
            return content;
        }

        private const string NEWLINE_REPLACEMENT = @" __Newline__="""" ";

        /// <summary>
        /// 将html代码转换为xml代码，需要在try-catch块中调用。
        /// </summary>
        public static string HtmlToXml(string strInputHtml)
        {
            strInputHtml = StringUtils.ReplaceIgnoreCase(strInputHtml, "<br>", "<br />");
            strInputHtml = StringUtils.ReplaceIgnoreCase(strInputHtml, "&#", "&amp;#");
            //strInputHtml = StringUtils.ReplaceNewline(strInputHtml, NEWLINE_REPLACEMENT);
            SgmlReader reader = new SgmlReader();
            reader.DocType = "HTML";
            System.IO.StringReader sr = new System.IO.StringReader(strInputHtml);
            reader.InputStream = sr;
            System.IO.StringWriter sw = new System.IO.StringWriter();
            XmlTextWriter w = new XmlTextWriter(sw);
            reader.Read();
            while (!reader.EOF)
            {
                w.WriteNode(reader, true);
            }
            w.Flush();
            w.Close();
            string xml = sw.ToString();
            //xml = xml.Replace(NEWLINE_REPLACEMENT, "\r\n");
            return xml;
        }

        public static List<string> GetEditableElementList(string templateContent)
        {
            List<string> editableElementList = new List<string>();

            MatchCollection mc = StlParserUtility.REGEX_EDITABLE_ELEMENT.Matches(templateContent);
            for (int i = 0; i < mc.Count; i++)
            {
                string editableElement = mc[i].Value;
                editableElementList.Add(editableElement);
            }

            return editableElementList;
        }

        public static bool IsStlEntityExists(string stlEntityName, string insertedLabelCollection)
        {
            bool exists = false;
            if (insertedLabelCollection.IndexOf(stlEntityName.Substring(0, stlEntityName.Length - 1)) != -1)
            {
                exists = true;
            }
            return exists;
        }

        public static List<string> GetStlLabelList(string templateContent)
        {
            List<string> stlElementList = StlParserUtility.GetStlElementList(templateContent);
            List<string> stlEntityList = StlParserUtility.GetStlEntityList(templateContent);
            stlElementList.AddRange(stlEntityList);
            return stlElementList;
        }


        //需要修改
        public static bool IsStlElementExists(string stlElementName, List<string> list)
        {
            bool exists = false;
            foreach (string label in list)
            {
                if (label.ToLower().StartsWith(string.Format("<{0} ", stlElementName.ToLower())) || label.ToLower().StartsWith(string.Format("<{0}>", stlElementName.ToLower())))
                {
                    exists = true;
                    break;
                }
            }
            return exists;
        }

        public static string GetStlEntity(string stlEntityName, string insertedLabelCollection)
        {
            string stlEntity = string.Empty;
            List<string> labelList = TranslateUtils.StringCollectionToStringList(insertedLabelCollection);
            foreach (string labelWithDisplayModeENNameAndNodeID in labelList)
            {
                if (labelWithDisplayModeENNameAndNodeID.StartsWith(stlEntityName.Substring(0, stlEntityName.Length - 1)))
                {
                    stlEntity = labelWithDisplayModeENNameAndNodeID;
                    break;
                }
            }
            return stlEntity;
        }

        public static string GetStlElement(string stlElementName, List<string> labelList)
        {
            string stlElement = string.Empty;
            foreach (string labelWithDisplayModeENNameAndNodeID in labelList)
            {
                if (labelWithDisplayModeENNameAndNodeID.ToLower().StartsWith(string.Format("<{0} ", stlElementName.ToLower())) || labelWithDisplayModeENNameAndNodeID.ToLower().StartsWith(string.Format("<{0}>", stlElementName.ToLower())))
                {
                    stlElement = labelWithDisplayModeENNameAndNodeID;
                    break;
                }
            }
            return stlElement;
        }


        public static string GetNameFromEntity(string stlEntity)
        {
            string name = stlEntity;
            if (stlEntity.IndexOf("_") != -1)
            {
                name = stlEntity.Substring(0, stlEntity.IndexOf("_"));
                name = name + "}";
            }
            return name;
        }

        public static string GetValueFromEntity(string stlEntity)
        {
            string value = string.Empty;
            if (stlEntity.IndexOf("_") != -1)
            {
                try
                {
                    int length = stlEntity.LastIndexOf("}") - stlEntity.LastIndexOf("_") - 1;
                    value = stlEntity.Substring(stlEntity.LastIndexOf("_") + 1, length);
                }
                catch { }
            }
            return value;
        }

        private static string EntityToStlElement(string elementName, NameValueCollection attributeMap)
        {
            return EntityToStlElement(elementName, attributeMap, string.Empty);
        }

        private static string EntityToStlElement(string elementName, NameValueCollection attributeMap, string innerXml)
        {
            string stlElement = string.Empty;
            StringBuilder attributes = new StringBuilder();
            if (attributeMap != null && attributeMap.Count > 0)
            {
                foreach (string key in attributeMap.Keys)
                {
                    attributes.AppendFormat(" {0}={1}{2}{1}", key, "\"", attributeMap[key]);
                }
            }
            stlElement = string.Format("<{0} {1}>{2}</{0}>", elementName, attributes, innerXml);
            return stlElement;
        }


        /// <summary>
        /// 判断此标签是否为Stl实体
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public static bool IsStlEntity(string label)
        {
            if (label == null) return false;
            label = label.ToLower();
            if ((label.StartsWith("{stl.") || label.StartsWith("{content.") || label.StartsWith("{channel.")) && label.EndsWith("}"))
            {
                return true;
            }
            return false;
        }


        public static bool IsStlEntityInclude(string content)
        {
            if (content == null) return false;
            content = content.ToLower();
            if (StringUtils.Contains(content, "}") && (StringUtils.Contains(content, "{stl.") || StringUtils.Contains(content, "{content.") || StringUtils.Contains(content, "{channel.")))
            {
                return true;
            }
            return false;
        }


        public static bool IsSpecifiedStlEntity(EStlEntityType entityType, string stlEntity)
        {
            if (stlEntity == null) return false;
            if (stlEntity.TrimStart('{').ToLower().StartsWith(EStlEntityTypeUtils.GetValue(entityType).ToLower()))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 判断此标签是否为Stl元素
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public static bool IsStlElement(string label)
        {
            if (label == null) return false;
            if (label.ToLower().StartsWith("<stl:") && label.IndexOf(">") != -1)
            {
                return true;
            }
            return false;
        }


        public static bool IsSpecifiedStlElement(string stlElement, string elementName)
        {
            if (stlElement == null) return false;
            if ((stlElement.ToLower().StartsWith(string.Format("<{0} ", elementName)) || stlElement.ToLower().StartsWith(string.Format("<{0}>", elementName))) && (stlElement.ToLower().EndsWith(string.Format("</{0}>", elementName)) || stlElement.ToLower().EndsWith("/>")))
            {
                return true;
            }
            return false;
        }

        //http://weblogs.asp.net/whaggard/archive/2005/02/20/377025.aspx
        //        public static Regex REGEX_STL_ELEMENT = new Regex(@"
        //<stl:(\w+?)[^>]*>
        //  (?>
        //      <stl:\1[^>]*> (?<LEVEL>)
        //    | 
        //      </stl:\1[^>]*> (?<-LEVEL>)
        //    |
        //      (?! <stl:\1[^>]*> | </stl:\1[^>]*> ).
        //  )*
        //  (?(LEVEL)(?!))
        //</stl:\1[^>]*>
        //", ((RegexOptions.Singleline | RegexOptions.IgnoreCase) | RegexOptions.IgnorePatternWhitespace) | RegexOptions.Compiled);
        public static Regex REGEX_STL_ELEMENT = new Regex(@"
<stl:(\w+?)[^>]*>
  (?>
      <stl:\1[^>]*> (?<LEVEL>)
    | 
      </stl:\1[^>]*> (?<-LEVEL>)
    |
      (?! <stl:\1[^>]*> | </stl:\1[^>]*> ).
  )*
  (?(LEVEL)(?!))
</stl:\1[^>]*>|<stl:(\w+?)[^>]*/>
", ((RegexOptions.Singleline | RegexOptions.IgnoreCase) | RegexOptions.IgnorePatternWhitespace) | RegexOptions.Compiled);

        public static Regex REGEX_EDITABLE_ELEMENT = new Regex(@"
<([a|p|img|div|footer|header|article|section|aside|nav])[^>]*>
  (?>
      <\1[^>]*> (?<LEVEL>)
    | 
      </\1[^>]*> (?<-LEVEL>)
    |
      (?! <\1[^>]*> | </\1[^>]*> ).
  )*
  (?(LEVEL)(?!))
</\1[^>]*>|<([a|p|img|div|footer|header|article|section|aside|nav])[^>]*/>
", ((RegexOptions.Singleline | RegexOptions.IgnoreCase) | RegexOptions.IgnorePatternWhitespace) | RegexOptions.Compiled);

        public static Regex GetStlEntityRegex(string entityName)
        {
            return new Regex(string.Format(@"{{{0}.[^{{}}]*}}", entityName), (RegexOptions.Singleline | RegexOptions.IgnoreCase) | RegexOptions.Compiled);
        }

        /// <summary>
        /// 得到内容中的STL元素列表
        /// </summary>
        /// <param name="content">需要解析的内容</param>
        /// <returns></returns>
        public static List<string> GetStlElementList(string templateContent)
        {
            List<string> stlElementList = new List<string>();

            MatchCollection mc = StlParserUtility.REGEX_STL_ELEMENT.Matches(templateContent);
            for (int i = 0; i < mc.Count; i++)
            {
                string stlElement = mc[i].Value;
                stlElementList.Add(stlElement);
            }

            return stlElementList;
        }

        /// <summary>
        /// 得到内容中的STL实体列表
        /// </summary>
        /// <param name="content">需要解析的内容</param>
        /// <returns></returns>
        public static List<string> GetStlEntityList(string content)
        {
            //首先需要去除<stl:项
            //content = RegexUtils.Replace(@"<stl:(\w+)[^>]*>.*?<\/stl:\1>", content, string.Empty);
            content = StlParserUtility.REGEX_STL_ELEMENT.Replace(content, string.Empty);

            List<string> stlEntityList = new List<string>();

            //Regex regex = new Regex(@"{[^{}]*}", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            //Regex regex = new Regex(@"{stl\.[^{}]*}|{content\.[^{}]*}|{channel\.[^{}]*}|{comment\.[^{}]*}|{request\.[^{}]*}|{sql\.[^{}]*}|{navigation\.[^{}]*}", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Regex regex = new Regex(EStlEntityTypeUtils.REGEX_STRING_ALL, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            MatchCollection mc = regex.Matches(content);
            for (int i = 0; i < mc.Count; i++)
            {
                string stlEntity = mc[i].Value;
                stlEntityList.Add(stlEntity);
            }

            return stlEntityList;
        }

        public static List<string> GetStlSqlEntityList(string content)
        {
            //首先需要去除<stl:项
            //content = RegexUtils.Replace(@"<stl:(\w+)[^>]*>.*?<\/stl:\1>", content, string.Empty);
            content = StlParserUtility.REGEX_STL_ELEMENT.Replace(content, string.Empty);

            List<string> stlEntityList = new List<string>();

            //Regex regex = new Regex(@"{[^{}]*}", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Regex regex = new Regex(EStlEntityTypeUtils.REGEX_STRING_SQL, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            MatchCollection mc = regex.Matches(content);
            for (int i = 0; i < mc.Count; i++)
            {
                string stlEntity = mc[i].Value;
                stlEntityList.Add(stlEntity);
            }

            return stlEntityList;
        }

        public static List<string> GetStlUserEntityList(string content)
        {
            content = StlParserUtility.REGEX_STL_ELEMENT.Replace(content, string.Empty);

            List<string> stlEntityList = new List<string>();

            Regex regex = new Regex(EStlEntityTypeUtils.REGEX_STRING_USER, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            MatchCollection mc = regex.Matches(content);
            for (int i = 0; i < mc.Count; i++)
            {
                string stlEntity = mc[i].Value;
                stlEntityList.Add(stlEntity);
            }

            return stlEntityList;
        }

        //判断属于某种类型（type）的<stl:content>元素是否存在
        public static bool IsStlContentElement(string labelString, string type)
        {
            return RegexUtils.IsMatch(string.Format(@"<stl:content[^>]+type=""{0}""[^>]*>", type), labelString);
        }

        //判断属于某种类型（type）的<stl:channel>元素是否存在
        public static bool IsStlChannelElement(string labelString, string type)
        {
            //return RegexUtils.IsMatch(string.Format("<stl:channel\\s+type=\"{0}\"\\s*>", type), labelString);
            return RegexUtils.IsMatch(string.Format(@"<stl:channel[^>]+type=""{0}""[^>]*>", type), labelString);
        }

        //TODO:测试
        public static string GetInnerXml(string stlElement, bool isInnerElement)
        {
            return GetInnerXml(stlElement, isInnerElement, null);
        }

        //TODO:测试
        public static string GetInnerXml(string stlElement, bool isInnerElement, LowerNameValueCollection attributes)
        {
            string retval = string.Empty;
            try
            {
                XmlDocument xmlDocument = StlParserUtility.GetXmlDocument(stlElement, isInnerElement);
                XmlNode node = xmlDocument.DocumentElement;
                node = node.FirstChild;
                retval = node.InnerXml;

                if (attributes != null)
                {
                    foreach (XmlAttribute attribute in node.Attributes)
                    {
                        attributes.Set(attribute.Name, attribute.Value);
                    }
                }
            }
            catch { }
            return retval;
        }

        public static string GetAttribute(string stlElement, string attributeName)
        {
            return RegexUtils.GetAttributeContent(attributeName, stlElement);
        }

        public static LowerNameValueCollection GetAttributes(string stlElement, bool isInnerElement)
        {
            LowerNameValueCollection attributes = new LowerNameValueCollection();
            try
            {
                XmlDocument xmlDocument = StlParserUtility.GetXmlDocument(stlElement, isInnerElement);
                XmlNode node = xmlDocument.DocumentElement;
                node = node.FirstChild;

                foreach (XmlAttribute attribute in node.Attributes)
                {
                    attributes.Set(attribute.Name, attribute.Value);
                }
            }
            catch { }
            return attributes;
        }

        public static NameValueCollection GetStlAttributes(string stlElement)
        {
            NameValueCollection attributes = new NameValueCollection();
            try
            {
                XmlDocument xmlDocument = StlParserUtility.GetXmlDocument(stlElement, false);
                XmlNode node = xmlDocument.DocumentElement;
                node = node.FirstChild;

                foreach (XmlAttribute attribute in node.Attributes)
                {
                    attributes.Set(attribute.Name, attribute.Value);
                }
            }
            catch { }
            return attributes;
        }

        public static string GetUrlInChannelPage(string type, PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo, int index, int currentPageIndex, int pageCount, EVisualType visualType)
        {
            int pageIndex = 0;
            if (type.ToLower().Equals(StlPageItem.Type_FirstPage.ToLower()))//首页
            {
                pageIndex = 0;
            }
            else if (type.ToLower().Equals(StlPageItem.Type_LastPage.ToLower()))//末页
            {
                pageIndex = pageCount - 1;
            }
            else if (type.ToLower().Equals(StlPageItem.Type_PreviousPage.ToLower()))//上一页
            {
                pageIndex = currentPageIndex - 1;
            }
            else if (type.ToLower().Equals(StlPageItem.Type_NextPage.ToLower()))//下一页
            {
                pageIndex = currentPageIndex + 1;
            }
            else if (type.ToLower().Equals(StlPageItem.Type_PageNavigation.ToLower()) || type.ToLower().Equals(StlPageItem.Type_PageSelect.ToLower()))
            {
                pageIndex = index - 1;
            }

            if (visualType == EVisualType.Static)
            {
                if (publishmentSystemInfo.Additional.CreateStaticMaxPage > 0 && pageIndex >= publishmentSystemInfo.Additional.CreateStaticMaxPage)
                {
                    string pageUrl = string.Empty;
                    string formatString = "javascript:stlDynamic_page({0});";//return false;

                    if (type.ToLower().Equals(StlPageItem.Type_FirstPage.ToLower()))//首页
                    {
                        pageUrl = string.Format(formatString, 1);
                    }
                    else if (type.ToLower().Equals(StlPageItem.Type_LastPage.ToLower()))//末页
                    {
                        pageUrl = string.Format(formatString, pageCount);
                    }
                    else if (type.ToLower().Equals(StlPageItem.Type_PreviousPage.ToLower()))//上一页
                    {
                        //pageUrl = string.Format(formatString, currentPageIndex);
                        pageUrl = string.Format(formatString, "'previousPage'");
                    }
                    else if (type.ToLower().Equals(StlPageItem.Type_NextPage.ToLower()))//下一页
                    {
                        //pageUrl = string.Format(formatString, currentPageIndex + 2);
                        pageUrl = string.Format(formatString, "'nextPage'");
                    }
                    else if (type.ToLower().Equals(StlPageItem.Type_PageNavigation.ToLower()))
                    {
                        pageUrl = string.Format(formatString, index);
                    }
                    else if (type.ToLower().Equals(StlPageItem.Type_PageSelect.ToLower()))
                    {
                        pageUrl = "javascript:stlDynamic_page(this.options[this.selectedIndex].value);";//return false;
                    }
                    return pageUrl;
                }
                else
                {
                    string physicalPath = PathUtility.GetChannelPageFilePath(publishmentSystemInfo, nodeInfo.NodeID, pageIndex);
                    return PageUtility.GetPublishmentSystemUrlByPhysicalPath(publishmentSystemInfo, physicalPath);
                }
            }
            else
            {
                return PageUtility.DynamicPage.GetRedirectUrl(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, 0, 0, pageIndex);
            }
        }

        public static string GetUrlInContentPage(string type, PublishmentSystemInfo publishmentSystemInfo, int nodeID, int contentID, int index, int currentPageIndex, int pageCount, EVisualType visualType)
        {
            int pageIndex = 0;
            if (type.ToLower().Equals(StlPageItem.Type_FirstPage.ToLower()))//首页
            {
                pageIndex = 0;
            }
            else if (type.ToLower().Equals(StlPageItem.Type_LastPage.ToLower()))//末页
            {
                pageIndex = pageCount - 1;
            }
            else if (type.ToLower().Equals(StlPageItem.Type_PreviousPage.ToLower()))//上一页
            {
                pageIndex = currentPageIndex - 1;
            }
            else if (type.ToLower().Equals(StlPageItem.Type_NextPage.ToLower()))//下一页
            {
                pageIndex = currentPageIndex + 1;
            }
            else if (type.ToLower().Equals(StlPageItem.Type_PageNavigation.ToLower()) || type.ToLower().Equals(StlPageItem.Type_PageSelect.ToLower()))
            {
                pageIndex = index - 1;
            }

            if (visualType == EVisualType.Static)
            {
                string physicalPath = PathUtility.GetContentPageFilePath(publishmentSystemInfo, nodeID, contentID, pageIndex);

                return PageUtility.GetPublishmentSystemUrlByPhysicalPath(publishmentSystemInfo, physicalPath);
            }
            else
            {
                return PageUtility.DynamicPage.GetRedirectUrl(publishmentSystemInfo.PublishmentSystemID, nodeID, contentID, 0, pageIndex);
            }
        }

        public static string GetClickStringInSearchPage(string type, string ajaxDivID, int index, int currentPageIndex, int pageCount)
        {
            string clickString = string.Empty;

            if (type.ToLower().Equals(StlPageItem.Type_FirstPage.ToLower()))//首页
            {
                clickString = string.Format("stlUpdate{0}({1})", ajaxDivID, 0);
            }
            else if (type.ToLower().Equals(StlPageItem.Type_LastPage.ToLower()))//末页
            {
                clickString = string.Format("stlUpdate{0}({1})", ajaxDivID, pageCount - 1);
            }
            else if (type.ToLower().Equals(StlPageItem.Type_PreviousPage.ToLower()))//上一页
            {
                clickString = string.Format("stlUpdate{0}({1})", ajaxDivID, currentPageIndex - 1);
            }
            else if (type.ToLower().Equals(StlPageItem.Type_NextPage.ToLower()))//下一页
            {
                clickString = string.Format("stlUpdate{0}({1})", ajaxDivID, currentPageIndex + 1);
            }
            else if (type.ToLower().Equals(StlPageItem.Type_PageNavigation.ToLower()))
            {
                clickString = string.Format("stlUpdate{0}({1})", ajaxDivID, index - 1);
            }
            else if (type.ToLower().Equals(StlPageItem.Type_PageSelect.ToLower()))
            {
                clickString = string.Format("stlJump{0}(this)", ajaxDivID);
            }

            return clickString;
        }

        public static string GetJsMethodInDynamicPage(string type, PublishmentSystemInfo publishmentSystemInfo, int nodeID, int contentID, string pageUrl, int index, int currentPageIndex, int pageCount, bool isPageRefresh, string ajaxDivID)
        {
            string jsMethod = string.Empty;
            int pageIndex = 0;
            if (type.ToLower().Equals(StlPageItem.Type_FirstPage.ToLower()))//首页
            {
                pageIndex = 0;
            }
            else if (type.ToLower().Equals(StlPageItem.Type_LastPage.ToLower()))//末页
            {
                pageIndex = pageCount - 1;
            }
            else if (type.ToLower().Equals(StlPageItem.Type_PreviousPage.ToLower()))//上一页
            {
                pageIndex = currentPageIndex - 1;
            }
            else if (type.ToLower().Equals(StlPageItem.Type_NextPage.ToLower()))//下一页
            {
                pageIndex = currentPageIndex + 1;
            }
            else if (type.ToLower().Equals(StlPageItem.Type_PageNavigation.ToLower()) || type.ToLower().Equals(StlPageItem.Type_PageSelect.ToLower()))
            {
                pageIndex = index - 1;
            }

            if (isPageRefresh)
            {
                if (type.ToLower().Equals(StlPageItem.Type_FirstPage.ToLower()))//首页
                {
                    jsMethod = string.Format("stlRedirectPage('{0}', {1})", pageUrl, 1);
                }
                else if (type.ToLower().Equals(StlPageItem.Type_LastPage.ToLower()))//末页
                {
                    jsMethod = string.Format("stlRedirectPage('{0}', {1})", pageUrl, pageCount);
                }
                else if (type.ToLower().Equals(StlPageItem.Type_PreviousPage.ToLower()))//上一页
                {
                    jsMethod = string.Format("stlRedirectPage('{0}', {1})", pageUrl, currentPageIndex);
                }
                else if (type.ToLower().Equals(StlPageItem.Type_NextPage.ToLower()))//下一页
                {
                    jsMethod = string.Format("stlRedirectPage('{0}', {1})", pageUrl, currentPageIndex + 2);
                }
                else if (type.ToLower().Equals(StlPageItem.Type_PageNavigation.ToLower()))
                {
                    jsMethod = string.Format("stlRedirectPage('{0}', {1})", pageUrl, index);
                }
                else if (type.ToLower().Equals(StlPageItem.Type_PageSelect.ToLower()))
                {
                    jsMethod = string.Format("stlRedirectPage('{0}', this.options[this.selectedIndex].value)", pageUrl);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(ajaxDivID))
                {
                    if (type.ToLower().Equals(StlPageItem.Type_FirstPage.ToLower()))//首页
                    {
                        jsMethod = string.Format("stlDynamic_{0}({1})", ajaxDivID, 1);
                    }
                    else if (type.ToLower().Equals(StlPageItem.Type_LastPage.ToLower()))//末页
                    {
                        jsMethod = string.Format("stlDynamic_{0}({1})", ajaxDivID, pageCount);
                    }
                    else if (type.ToLower().Equals(StlPageItem.Type_PreviousPage.ToLower()))//上一页
                    {
                        jsMethod = string.Format("stlDynamic_{0}({1})", ajaxDivID, currentPageIndex);
                    }
                    else if (type.ToLower().Equals(StlPageItem.Type_NextPage.ToLower()))//下一页
                    {
                        jsMethod = string.Format("stlDynamic_{0}({1})", ajaxDivID, currentPageIndex + 2);
                    }
                    else if (type.ToLower().Equals(StlPageItem.Type_PageNavigation.ToLower()))
                    {
                        jsMethod = string.Format("stlDynamic_{0}({1})", ajaxDivID, index);
                    }
                    else if (type.ToLower().Equals(StlPageItem.Type_PageSelect.ToLower()))
                    {
                        jsMethod = string.Format("stlDynamic_{0}(this.options[this.selectedIndex].value)", ajaxDivID);
                    }
                }
                else
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                    string redirectUrl = string.Empty;
                    if (contentID > 0)
                    {
                        redirectUrl = PathUtility.GetContentPageFilePath(publishmentSystemInfo, nodeInfo.NodeID, contentID, pageIndex);
                    }
                    else
                    {
                        redirectUrl = PathUtility.GetChannelPageFilePath(publishmentSystemInfo, nodeInfo.NodeID, pageIndex);
                    }
                    redirectUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(publishmentSystemInfo, redirectUrl);
                    jsMethod = string.Format("window.location.href='{0}';", redirectUrl);
                }
            }
            return jsMethod;
        }

        public const string ItemIndex = "ItemIndex";
        public static int ParseItemIndex(int dbItemIndex, string attributeName, ContextInfo contextInfo)
        {
            int itemIndex = contextInfo.PageItemIndex + dbItemIndex + 1;
            if (attributeName.IndexOf('+') != -1 || attributeName.IndexOf('-') != -1)
            {
                string[] array = attributeName.Split('+');
                if (array != null && array.Length == 2)
                {
                    int addNum = TranslateUtils.ToInt(array[1].Trim(), 1);
                    return itemIndex + addNum;
                }

                array = attributeName.Split('-');
                if (array != null && array.Length == 2)
                {
                    int substractNum = TranslateUtils.ToInt(array[1].Trim(), 1);
                    return itemIndex - substractNum;
                }
            }

            return itemIndex;
        }

        public static int GetItemIndex(ContextInfo contextInfo)
        {
            int dbItemIndex = 0;
            if (contextInfo.ContextType == EContextType.Channel)
            {
                dbItemIndex = contextInfo.ItemContainer.ChannelItem.ItemIndex;
            }
            else if (contextInfo.ContextType == EContextType.Comment)
            {
                dbItemIndex = contextInfo.ItemContainer.CommentItem.ItemIndex;
            }
            else if (contextInfo.ContextType == EContextType.Content)
            {
                dbItemIndex = contextInfo.ItemContainer.ContentItem.ItemIndex;
            }
            else if (contextInfo.ContextType == EContextType.InputContent)
            {
                dbItemIndex = contextInfo.ItemContainer.InputItem.ItemIndex;
            }
            else if (contextInfo.ContextType == EContextType.SqlContent)
            {
                dbItemIndex = contextInfo.ItemContainer.SqlItem.ItemIndex;
            }
            else if (contextInfo.ContextType == EContextType.Site)
            {
                dbItemIndex = contextInfo.ItemContainer.SiteItem.ItemIndex;
            }
            else if (contextInfo.ContextType == EContextType.Photo)
            {
                dbItemIndex = contextInfo.ItemContainer.PhotoItem.ItemIndex;
            }
            else if (contextInfo.ContextType == EContextType.Each)
            {
                dbItemIndex = contextInfo.ItemContainer.EachItem.ItemIndex;
            }
            else if (contextInfo.ContextType == EContextType.Spec)
            {
                dbItemIndex = contextInfo.ItemContainer.SpecItem.ItemIndex;
            }
            else if (contextInfo.ContextType == EContextType.Filter)
            {
                dbItemIndex = contextInfo.ItemContainer.FilterItem.ItemIndex;
            }

            return contextInfo.PageItemIndex + dbItemIndex + 1;
        }

        public static void GetInnerTemplateString(XmlNode node, PageInfo pageInfo, out string successTemplateString, out string failureTemplateString)
        {
            successTemplateString = string.Empty;
            failureTemplateString = string.Empty;

            if (!string.IsNullOrEmpty(node.InnerXml))
            {
                List<string> stlElementList = StlParserUtility.GetStlElementList(node.InnerXml);
                if (stlElementList.Count > 0)
                {
                    foreach (string theStlElement in stlElementList)
                    {
                        if (StlParserUtility.IsSpecifiedStlElement(theStlElement, StlYes.ElementName) || StlParserUtility.IsSpecifiedStlElement(theStlElement, StlYes.ElementName2))
                        {
                            successTemplateString = StlParserUtility.GetInnerXml(theStlElement, true);
                        }
                        else if (StlParserUtility.IsSpecifiedStlElement(theStlElement, StlNo.ElementName) || StlParserUtility.IsSpecifiedStlElement(theStlElement, StlNo.ElementName2))
                        {
                            failureTemplateString = StlParserUtility.GetInnerXml(theStlElement, true);
                        }
                    }
                }
                if (string.IsNullOrEmpty(successTemplateString) && string.IsNullOrEmpty(failureTemplateString))
                {
                    successTemplateString = node.InnerXml;
                }
            }
        }

        public static void GetInnerTemplateString(XmlNode node, out string successTemplateString, out string failureTemplateString, PageInfo pageInfo, ContextInfo contextInfo)
        {
            successTemplateString = string.Empty;
            failureTemplateString = string.Empty;

            if (!string.IsNullOrEmpty(node.InnerXml))
            {
                List<string> stlElementList = StlParserUtility.GetStlElementList(node.InnerXml);
                if (stlElementList.Count > 0)
                {
                    foreach (string theStlElement in stlElementList)
                    {
                        if (StlParserUtility.IsSpecifiedStlElement(theStlElement, StlYes.ElementName) || StlParserUtility.IsSpecifiedStlElement(theStlElement, StlYes.ElementName2))
                        {
                            StringBuilder innerBuilder = new StringBuilder(StlParserUtility.GetInnerXml(theStlElement, true));
                            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                            successTemplateString = innerBuilder.ToString();

                        }
                        else if (StlParserUtility.IsSpecifiedStlElement(theStlElement, StlNo.ElementName) || StlParserUtility.IsSpecifiedStlElement(theStlElement, StlNo.ElementName2))
                        {
                            StringBuilder innerBuilder = new StringBuilder(StlParserUtility.GetInnerXml(theStlElement, true));
                            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                            failureTemplateString = innerBuilder.ToString();
                        }
                    }
                }
                if (string.IsNullOrEmpty(successTemplateString) && string.IsNullOrEmpty(failureTemplateString))
                {
                    StringBuilder innerBuilder = new StringBuilder(node.InnerXml);
                    StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                    successTemplateString = innerBuilder.ToString();
                }
            }
        }

        public static void GetInnerTemplateStringOfInput(XmlNode node, out string inputTemplateString, out string successTemplateString, out string failureTemplateString, PageInfo pageInfo, ContextInfo contextInfo)
        {
            inputTemplateString = string.Empty;
            successTemplateString = string.Empty;
            failureTemplateString = string.Empty;

            if (!string.IsNullOrEmpty(node.InnerXml))
            {
                List<string> stlElementList = StlParserUtility.GetStlElementList(node.InnerXml);
                if (stlElementList.Count > 0)
                {
                    foreach (string theStlElement in stlElementList)
                    {
                        if (StlParserUtility.IsSpecifiedStlElement(theStlElement, StlInput.InputTemplate.ElementName))
                        {
                            StringBuilder innerBuilder = new StringBuilder(StlParserUtility.GetInnerXml(theStlElement, true));
                            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                            inputTemplateString = innerBuilder.ToString();
                        }
                        else if (StlParserUtility.IsSpecifiedStlElement(theStlElement, StlYes.ElementName) || StlParserUtility.IsSpecifiedStlElement(theStlElement, StlYes.ElementName2))
                        {
                            StringBuilder innerBuilder = new StringBuilder(StlParserUtility.GetInnerXml(theStlElement, true));
                            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                            successTemplateString = innerBuilder.ToString();
                        }
                        else if (StlParserUtility.IsSpecifiedStlElement(theStlElement, StlNo.ElementName) || StlParserUtility.IsSpecifiedStlElement(theStlElement, StlNo.ElementName2))
                        {
                            StringBuilder innerBuilder = new StringBuilder(StlParserUtility.GetInnerXml(theStlElement, true));
                            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                            failureTemplateString = innerBuilder.ToString();
                        }
                    }
                }
                if (string.IsNullOrEmpty(inputTemplateString) && string.IsNullOrEmpty(successTemplateString) && string.IsNullOrEmpty(failureTemplateString))
                {
                    StringBuilder innerBuilder = new StringBuilder(node.InnerXml);
                    StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                    inputTemplateString = innerBuilder.ToString();
                }
            }
        }

        public static void GetInnerTemplateStringOfSearchwordInput(XmlNode node, out string searchwordInputTemplateString, out string successTemplateString, out string failureTemplateString, PageInfo pageInfo, ContextInfo contextInfo)
        {
            searchwordInputTemplateString = string.Empty;
            successTemplateString = string.Empty;
            failureTemplateString = string.Empty;

            if (!string.IsNullOrEmpty(node.InnerXml))
            {
                List<string> stlElementList = StlParserUtility.GetStlElementList(node.InnerXml);
                if (stlElementList.Count > 0)
                {
                    foreach (string theStlElement in stlElementList)
                    {
                        if (StlParserUtility.IsSpecifiedStlElement(theStlElement, StlSearchwordInput.ElementName))
                        {
                            StringBuilder innerBuilder = new StringBuilder(StlParserUtility.GetInnerXml(theStlElement, true));
                            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                            searchwordInputTemplateString = innerBuilder.ToString();
                        }
                        else if (StlParserUtility.IsSpecifiedStlElement(theStlElement, StlYes.ElementName) || StlParserUtility.IsSpecifiedStlElement(theStlElement, StlYes.ElementName2))
                        {
                            StringBuilder innerBuilder = new StringBuilder(StlParserUtility.GetInnerXml(theStlElement, true));
                            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                            successTemplateString = innerBuilder.ToString();
                        }
                        else if (StlParserUtility.IsSpecifiedStlElement(theStlElement, StlNo.ElementName) || StlParserUtility.IsSpecifiedStlElement(theStlElement, StlNo.ElementName2))
                        {
                            StringBuilder innerBuilder = new StringBuilder(StlParserUtility.GetInnerXml(theStlElement, true));
                            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                            failureTemplateString = innerBuilder.ToString();
                        }
                    }
                }
                if (string.IsNullOrEmpty(searchwordInputTemplateString) && string.IsNullOrEmpty(successTemplateString) && string.IsNullOrEmpty(failureTemplateString))
                {
                    StringBuilder innerBuilder = new StringBuilder(node.InnerXml);
                    StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                    searchwordInputTemplateString = innerBuilder.ToString();
                }
            }
        }

        public static void GetInnerTemplateStringOfWebsiteMessage(XmlNode node, out string websiteMessageTemplateString, out string classifyTemplateString, out string successTemplateString, out string failureTemplateString, PageInfo pageInfo, ContextInfo contextInfo)
        {
            websiteMessageTemplateString = string.Empty;
            classifyTemplateString = string.Empty;
            successTemplateString = string.Empty;
            failureTemplateString = string.Empty;

            if (!string.IsNullOrEmpty(node.InnerXml))
            {
                List<string> stlElementList = StlParserUtility.GetStlElementList(node.InnerXml);
                if (stlElementList.Count > 0)
                {
                    foreach (string theStlElement in stlElementList)
                    {
                        if (StlParserUtility.IsSpecifiedStlElement(theStlElement, StlWebsiteMessage.WebsiteMessageTemplate.ElementName))
                        {
                            StringBuilder innerBuilder = new StringBuilder(StlParserUtility.GetInnerXml(theStlElement, true));
                            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                            websiteMessageTemplateString = innerBuilder.ToString();
                        }
                        else if (StlParserUtility.IsSpecifiedStlElement(theStlElement, StlYes.ElementName) || StlParserUtility.IsSpecifiedStlElement(theStlElement, StlYes.ElementName2))
                        {
                            StringBuilder innerBuilder = new StringBuilder(StlParserUtility.GetInnerXml(theStlElement, true));
                            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                            successTemplateString = innerBuilder.ToString();
                        }
                        else if (StlParserUtility.IsSpecifiedStlElement(theStlElement, StlNo.ElementName) || StlParserUtility.IsSpecifiedStlElement(theStlElement, StlNo.ElementName2))
                        {
                            StringBuilder innerBuilder = new StringBuilder(StlParserUtility.GetInnerXml(theStlElement, true));
                            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                            failureTemplateString = innerBuilder.ToString();
                        }
                        else if (StlParserUtility.IsSpecifiedStlElement(theStlElement, StlWebsiteMessage.WebstieMessageClassifyTemplate.ElementName))
                        {
                            StringBuilder innerBuilder = new StringBuilder(StlParserUtility.GetInnerXml(theStlElement, true));
                            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                            classifyTemplateString = innerBuilder.ToString();
                        }
                    }
                }
                if (string.IsNullOrEmpty(websiteMessageTemplateString) && string.IsNullOrEmpty(successTemplateString) && string.IsNullOrEmpty(failureTemplateString))
                {
                    StringBuilder innerBuilder = new StringBuilder(node.InnerXml);
                    StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                    websiteMessageTemplateString = innerBuilder.ToString();
                }
            }
        }

        public static void GetInnerTemplateStringOfSubscribe(XmlNode node, out string subscribeTemplateString, out string successTemplateString, out string failureTemplateString, PageInfo pageInfo, ContextInfo contextInfo)
        {
            subscribeTemplateString = string.Empty;
            successTemplateString = string.Empty;
            failureTemplateString = string.Empty;

            if (!string.IsNullOrEmpty(node.InnerXml))
            {
                List<string> stlElementList = StlParserUtility.GetStlElementList(node.InnerXml);
                if (stlElementList.Count > 0)
                {
                    foreach (string theStlElement in stlElementList)
                    {
                        if (StlParserUtility.IsSpecifiedStlElement(theStlElement, StlYes.ElementName) || StlParserUtility.IsSpecifiedStlElement(theStlElement, StlYes.ElementName2))
                        {
                            StringBuilder innerBuilder = new StringBuilder(StlParserUtility.GetInnerXml(theStlElement, true));
                            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                            successTemplateString = innerBuilder.ToString();
                        }
                        else if (StlParserUtility.IsSpecifiedStlElement(theStlElement, StlNo.ElementName) || StlParserUtility.IsSpecifiedStlElement(theStlElement, StlNo.ElementName2))
                        {
                            StringBuilder innerBuilder = new StringBuilder(StlParserUtility.GetInnerXml(theStlElement, true));
                            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                            failureTemplateString = innerBuilder.ToString();
                        }
                    }
                }
                if (string.IsNullOrEmpty(subscribeTemplateString) && string.IsNullOrEmpty(successTemplateString) && string.IsNullOrEmpty(failureTemplateString))
                {
                    StringBuilder innerBuilder = new StringBuilder(node.InnerXml);
                    StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                    subscribeTemplateString = innerBuilder.ToString();
                }
            }
        }


        public static string GetStlErrorMessage(string elementName, Exception ex)
        {
            return string.Format("<!-- {0} error: {1} -->", elementName, ex.Message);
        }

        public static string GetAjaxDivID(int updaterID)
        {
            return "ajaxElement_" + updaterID + "_" + StringUtils.GetRandomInt(100, 1000);
        }

        public static string GetStlElement(string stlElementName, NameValueCollection attributes, string innerContent)
        {
            if (string.IsNullOrEmpty(innerContent))
            {
                return string.Format(@"<{0} {1}></{0}>", stlElementName, TranslateUtils.ToAttributesString(attributes));
            }
            else
            {
                return string.Format(@"<{0} {1}>
{2}
</{0}>", stlElementName, TranslateUtils.ToAttributesString(attributes), innerContent);
            }
        }
    }
}
