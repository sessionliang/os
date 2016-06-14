using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Text.Sgml;
using System;
using System.Web.UI.WebControls;
using SiteServer.BBS.Core.TemplateParser.Model;
using SiteServer.BBS.Core.TemplateParser.Element;

namespace SiteServer.BBS.Core.TemplateParser
{
    public class ParserUtility
    {
        const string Order_Default = "Default";								//默认排序
        const string Order_Back = "Back";								//默认排序的相反方向
        const string Order_AddDate = "AddDate";							//添加时间
        const string Order_AddDateBack = "AddDateBack";					//添加时间的相反方向
        const string Order_LastEditDate = "LastEditDate";				//最后更改时间（只可用于内容列表）
        const string Order_LastEditDateBack = "LastEditDateBack";	//最后更改时间的相反方向（只可用于内容列表）
        const string Order_Hits = "Hits";	            //点击量
        const string Order_HitsByDay = "HitsByDay";	    //日点击量
        const string Order_HitsByWeek = "HitsByWeek";	//周点击量
        const string Order_HitsByMonth = "HitsByMonth";	//月点击量
        const string Order_Stars = "Stars";	            //评分数
        const string Order_Digg = "Digg";	            //Digg数
        const string Order_Comments = "Comments";        //评论数
        const string Order_Random = "Random";            //随机

        private const string xmlDeclaration = "<?xml version='1.0'?>";

        private const string xmlNamespaceStart = "<bbs:document xmlns=\"http://www.siteserver.cn/bbs\" xmlns:bbs=\"http://www.siteserver.cn/bbs\" xmlns:BBS=\"http://www.siteserver.cn/bbs\" xmlns:bBS=\"http://www.siteserver.cn/bbs\" xmlns:bbS=\"http://www.siteserver.cn/bbs\" xmlns:bBs=\"http://www.siteserver.cn/bbs\" xmlns:Bbs=\"http://www.siteserver.cn/bbs\" xmlns:BbS=\"http://www.siteserver.cn/bbs\" xmlns:asp=\"http://www.siteserver.cn/bbs\" xmlns:ext=\"http://www.siteserver.cn/bbs\">";
        private const string xmlNamespaceEnd = "</bbs:document>";

        public static XmlDocument GetXmlDocument(string element, bool isXmlContent)
        {
            XmlDocument xmlDocument = new XmlDocument();
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
        public static string GetBackHtml(string content)
        {
            if (content != null)
            {
                content = content.Replace("&quot;", "'");
                content = content.Replace("&gt;", ">");
                content = content.Replace("&lt;", "<");
                content = content.Replace("&amp;", "&");
                content = content.Replace(" xmlns=\"http://www.siteserver.cn/bbs\"", string.Empty);
                content = content.Replace(" xmlns:bbs=\"http://www.siteserver.cn/bbs\"", string.Empty);
                content = content.Replace(" xmlns:asp=\"http://www.siteserver.cn/bbs\"", string.Empty);
                content = content.Replace("&amp;#", "&#");
                content = content.Replace("<![CDATA[", string.Empty);
                content = content.Replace("]]>", string.Empty);
            }
            return content;
        }

        /// <summary>
        /// 将html代码转换为xml代码，需要在try-catch块中调用。
        /// </summary>
        public static string HtmlToXml(string strInputHtml)
        {
            strInputHtml = StringUtils.ReplaceIgnoreCase(strInputHtml, "<br>", "<br />");
            strInputHtml = StringUtils.ReplaceIgnoreCase(strInputHtml, "&#", "&amp;#");
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
            return sw.ToString();
            //string xml = sw.ToString();
            //if (xml != null)
            //{
            //    if (xml.ToLower().StartsWith("<html>"))
            //    {
            //        xml = xml.Substring("<html>".Length);
            //    }
            //    if (xml.ToLower().EndsWith("</html>"))
            //    {
            //        xml = xml.Substring(0, xml.Length - "</html>".Length);
            //    }
            //}
            //return xml;
        }

        public static void XmlToHtml(StringBuilder builder)
        {
            builder.Replace("&quot;", "'");
            builder.Replace("&gt;", ">");
            builder.Replace("&lt;", "<");
            builder.Replace("&amp;", "&");
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

        public static ArrayList GetLabelArrayList(string templateContent)
        {
            ArrayList elementArrayList = ParserUtility.GetElementArrayList(templateContent);
            ArrayList entityArrayList = ParserUtility.GetEntityArrayList(templateContent);
            elementArrayList.AddRange(entityArrayList);
            return elementArrayList;
        }


        //需要修改
        public static bool IsStlElementExists(string stlElementName, ArrayList arrayList)
        {
            bool exists = false;
            foreach (string label in arrayList)
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
            string entity = string.Empty;
            ArrayList LabelArrayList = TranslateUtils.StringCollectionToArrayList(insertedLabelCollection);
            foreach (string labelWithDisplayModeENNameAndNodeID in LabelArrayList)
            {
                if (labelWithDisplayModeENNameAndNodeID.StartsWith(stlEntityName.Substring(0, stlEntityName.Length - 1)))
                {
                    entity = labelWithDisplayModeENNameAndNodeID;
                    break;
                }
            }
            return entity;
        }

        public static string GetStlElement(string stlElementName, ArrayList labelArrayList)
        {
            string element = string.Empty;
            foreach (string labelWithDisplayModeENNameAndNodeID in labelArrayList)
            {
                if (labelWithDisplayModeENNameAndNodeID.ToLower().StartsWith(string.Format("<{0} ", stlElementName.ToLower())) || labelWithDisplayModeENNameAndNodeID.ToLower().StartsWith(string.Format("<{0}>", stlElementName.ToLower())))
                {
                    element = labelWithDisplayModeENNameAndNodeID;
                    break;
                }
            }
            return element;
        }

        public static string GetNameFromEntity(string entity)
        {
            string name = entity;
            if (entity.IndexOf("_") != -1)
            {
                name = entity.Substring(0, entity.IndexOf("_"));
                name = name + "}";
            }
            return name;
        }

        public static string GetForumIndexFromEntity(string entity)
        {
            string forumIndex = string.Empty;
            if (entity.IndexOf("_") != -1)
            {
                try
                {
                    int length = entity.LastIndexOf("}") - entity.LastIndexOf("_") - 1;
                    forumIndex = entity.Substring(entity.LastIndexOf("_") + 1, length);
                }
                catch { }
            }
            return forumIndex;
        }

        private static string EntityToElement(string elementName, NameValueCollection attributeMap)
        {
            return EntityToElement(elementName, attributeMap, string.Empty);
        }

        private static string EntityToElement(string elementName, NameValueCollection attributeMap, string innerXml)
        {
            string element = string.Empty;
            StringBuilder attributes = new StringBuilder();
            if (attributeMap != null && attributeMap.Count > 0)
            {
                foreach (string key in attributeMap.Keys)
                {
                    attributes.AppendFormat(" {0}={1}{2}{1}", key, "\"", attributeMap[key]);
                }
            }
            element = string.Format("<{0} {1}>{2}</{0}>", elementName, attributes, innerXml);
            return element;
        }


        /// <summary>
        /// 判断此标签是否为Stl实体
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public static bool IsEntity(string label)
        {
            if (label == null) return false;
            label = label.ToLower();
            if (label.StartsWith("{bbs.") && label.EndsWith("}"))
            {
                return true;
            }
            return false;
        }


        public static bool IsEntityInclude(string content)
        {
            if (content == null) return false;
            content = content.ToLower();
            if (StringUtils.Contains(content, "}") && (StringUtils.Contains(content, "{bbs.")))
            {
                return true;
            }
            return false;
        }


        public static bool IsSpecifiedEntity(string entity)
        {
            if (entity == null) return false;
            if (entity.TrimStart('{').ToLower().StartsWith("bbs"))
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
        static bool IsStlElement(string label)
        {
            if (label == null) return false;
            if (label.ToLower().StartsWith("<bbs:") && label.IndexOf(">") != -1)
            {
                return true;
            }
            return false;
        }


        public static bool IsSpecifiedStlElement(string element, string elementName)
        {
            if (element == null) return false;
            if ((element.ToLower().StartsWith(string.Format("<{0} ", elementName)) || element.ToLower().StartsWith(string.Format("<{0}>", elementName))) && element.ToLower().EndsWith(string.Format("</{0}>", elementName)))
            {
                return true;
            }
            return false;
        }

        //http://weblogs.asp.net/whaggard/archive/2005/02/20/377025.aspx
        public static Regex REGEX_ELEMENT = new Regex(@"
<bbs:(\w+?)[^>]*>
  (?>
      <bbs:\1[^>]*> (?<LEVEL>)
    | 
      </bbs:\1[^>]*> (?<-LEVEL>)
    |
      (?! <bbs:\1[^>]*> | </bbs:\1[^>]*> ).
  )*
  (?(LEVEL)(?!))
</bbs:\1[^>]*>
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
        public static ArrayList GetElementArrayList(string content)
        {
            ArrayList elementArrayList = new ArrayList();

            //Regex regex = new Regex(@"<bbs:(\w+)[^>]*>.*?<\/bbs:\1>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            MatchCollection mc = ParserUtility.REGEX_ELEMENT.Matches(content);
            for (int i = 0; i < mc.Count; i++)
            {
                string element = mc[i].Value;
                elementArrayList.Add(element);
            }

            return elementArrayList;
        }

        /// <summary>
        /// 得到内容中的STL实体列表
        /// </summary>
        /// <param name="content">需要解析的内容</param>
        /// <returns></returns>
        public static ArrayList GetEntityArrayList(string content)
        {
            //首先需要去除<bbs:项
            //content = RegexUtils.Replace(@"<bbs:(\w+)[^>]*>.*?<\/bbs:\1>", content, string.Empty);
            content = ParserUtility.REGEX_ELEMENT.Replace(content, string.Empty);

            ArrayList stlEntityArrayList = new ArrayList();

            //Regex regex = new Regex(@"{[^{}]*}", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Regex regex = new Regex(@"{bbs\.[^{}]*}", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            MatchCollection mc = regex.Matches(content);
            for (int i = 0; i < mc.Count; i++)
            {
                string entity = mc[i].Value;
                stlEntityArrayList.Add(entity);
            }

            return stlEntityArrayList;
        }

        //TODO:测试
        public static string GetInnerXml(string element, bool isInnerElement)
        {
            return GetInnerXml(element, isInnerElement, null);
        }

        //TODO:测试
        public static string GetInnerXml(string element, bool isInnerElement, LowerNameValueCollection attributes)
        {
            string retval = string.Empty;
            try
            {
                XmlDocument xmlDocument = ParserUtility.GetXmlDocument(element, isInnerElement);
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

        public static LowerNameValueCollection GetAttributes(string element, bool isInnerElement)
        {
            LowerNameValueCollection attributes = new LowerNameValueCollection();
            try
            {
                XmlDocument xmlDocument = ParserUtility.GetXmlDocument(element, isInnerElement);
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

        public static string GetErrorMessage(string elementName, Exception ex)
        {
            return string.Format("<!-- {0} error: {1} -->", elementName, ex.Message);
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
            if (contextInfo.ContextType == EContextType.Forum)
            {
                dbItemIndex = contextInfo.ItemContainer.ForumItem.ItemIndex;
            }
            else if (contextInfo.ContextType == EContextType.Thread)
            {
                dbItemIndex = contextInfo.ItemContainer.ThreadItem.ItemIndex;
            }
            else if (contextInfo.ContextType == EContextType.Post)
            {
                dbItemIndex = contextInfo.ItemContainer.PostItem.ItemIndex;
            }

            return contextInfo.PageItemIndex + dbItemIndex + 1;
        }

        public static void GetInnerTemplateString(XmlNode node, out string successTemplateString, out string failureTemplateString)
        {
            successTemplateString = string.Empty;
            failureTemplateString = string.Empty;

            if (!string.IsNullOrEmpty(node.InnerXml))
            {
                ArrayList stlElementArrayList = ParserUtility.GetElementArrayList(node.InnerXml);
                if (stlElementArrayList.Count > 0)
                {
                    foreach (string theStlElement in stlElementArrayList)
                    {
                        if (ParserUtility.IsSpecifiedStlElement(theStlElement, If.SuccessTemplate.ElementName))
                        {
                            successTemplateString = ParserUtility.GetInnerXml(theStlElement, true);
                        }
                        else if (ParserUtility.IsSpecifiedStlElement(theStlElement, If.FailureTemplate.ElementName))
                        {
                            failureTemplateString = ParserUtility.GetInnerXml(theStlElement, true);
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
                ArrayList stlElementArrayList = ParserUtility.GetElementArrayList(node.InnerXml);
                if (stlElementArrayList.Count > 0)
                {
                    foreach (string theStlElement in stlElementArrayList)
                    {
                        if (ParserUtility.IsSpecifiedStlElement(theStlElement, If.SuccessTemplate.ElementName))
                        {
                            StringBuilder innerBuilder = new StringBuilder(ParserUtility.GetInnerXml(theStlElement, true));
                            ParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                            successTemplateString = innerBuilder.ToString();

                        }
                        else if (ParserUtility.IsSpecifiedStlElement(theStlElement, If.FailureTemplate.ElementName))
                        {
                            StringBuilder innerBuilder = new StringBuilder(ParserUtility.GetInnerXml(theStlElement, true));
                            ParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                            failureTemplateString = innerBuilder.ToString();
                        }
                    }
                }
                if (string.IsNullOrEmpty(successTemplateString) && string.IsNullOrEmpty(failureTemplateString))
                {
                    StringBuilder innerBuilder = new StringBuilder(node.InnerXml);
                    ParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                    successTemplateString = innerBuilder.ToString();
                }
            }
        }

        public static HorizontalAlign ToHorizontalAlign(string typeStr)
        {
            return (HorizontalAlign)TranslateUtils.ToEnum(typeof(HorizontalAlign), typeStr, HorizontalAlign.Left);
        }

        public static VerticalAlign ToVerticalAlign(string typeStr)
        {
            return (VerticalAlign)TranslateUtils.ToEnum(typeof(VerticalAlign), typeStr, VerticalAlign.Middle);
        }

        public static GridLines ToGridLines(string typeStr)
        {
            return (GridLines)TranslateUtils.ToEnum(typeof(GridLines), typeStr, GridLines.None);
        }

        public static RepeatDirection ToRepeatDirection(string typeStr)
        {
            return (RepeatDirection)TranslateUtils.ToEnum(typeof(RepeatDirection), typeStr, RepeatDirection.Vertical);
        }

        public static RepeatLayout ToRepeatLayout(string typeStr)
        {
            return (RepeatLayout)TranslateUtils.ToEnum(typeof(RepeatLayout), typeStr, RepeatLayout.Table);
        }
    }
}
