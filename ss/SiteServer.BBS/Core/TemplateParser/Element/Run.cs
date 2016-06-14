using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using BaiRong.Core;
using System.Web.UI;
using System;
using BaiRong.Model;
using SiteServer.BBS.Core.TemplateParser.Model;
using SiteServer.BBS.Model;

namespace SiteServer.BBS.Core.TemplateParser.Element
{
    public class Run
    {
        private Run() { }
        public const string ElementName = "bbs:run";//运行

        public const string Attribute_Class = "class";              //类名
        public const string Attribute_If = "if";			        //测试
        public const string Attribute_Get = "get";			        //获取

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();

                attributes.Add(Attribute_Class, "类名");
                attributes.Add(Attribute_If, "测试");
                attributes.Add(Attribute_Get, "获取");

                return attributes;
            }
        }

        public sealed class SuccessTemplate
        {
            const string ElementName = "bbs:successtemplate";
        }

        public sealed class FailureTemplate
        {
            const string ElementName = "bbs:failuretemplate";
        }

        internal static string Parse(string element, XmlNode node, PageInfo pageInfo, ContextInfo contextInfoRef)
        {
            string parsedContent = string.Empty;
            ContextInfo contextInfo = contextInfoRef.Clone();
            try
            {
                IEnumerator ie = node.Attributes.GetEnumerator();

                string classString = string.Empty;
                string ifString = string.Empty;
                string getString = string.Empty;

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(Run.Attribute_Class))
                    {
                        classString = EntityParser.ReplaceEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(Run.Attribute_If))
                    {
                        ifString = EntityParser.ReplaceEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                        ifString = ifString.Replace('\'', '"');
                    }
                    else if (attributeName.Equals(Run.Attribute_Get))
                    {
                        getString = EntityParser.ReplaceEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                        getString = getString.Replace('\'', '"');
                    }
                }

                parsedContent = ParseImpl(node, pageInfo, contextInfo, classString, ifString, getString);
            }
            catch (Exception ex)
            {
                parsedContent = ParserUtility.GetErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, string classString, string ifString, string getString)
        {
            string parsedContent = string.Empty;

            string successTemplateString = string.Empty;
            string failureTemplateString = string.Empty;

            ParserUtility.GetInnerTemplateString(node, out successTemplateString, out failureTemplateString);

            if (!string.IsNullOrEmpty(classString))
            {
                if (!string.IsNullOrEmpty(ifString))
                {
                    if (ifString.StartsWith("!"))
                    {
                        ifString = "!" + classString + "." + ifString.Substring(1);
                    }
                    else
                    {
                        ifString = classString + "." + ifString;
                    }
                }
                if (!string.IsNullOrEmpty(getString))
                {
                    getString = classString + "." + getString;
                }
            }

            if (!string.IsNullOrEmpty(ifString) && !string.IsNullOrEmpty(successTemplateString) && !string.IsNullOrEmpty(failureTemplateString))
            {
                parsedContent = string.Format(@"
<%if ({0}){{%>{1}<%}}else{{%>{2}<%}}%>
", ifString, successTemplateString, failureTemplateString);
            }
            else if (!string.IsNullOrEmpty(ifString) && !string.IsNullOrEmpty(successTemplateString))
            {
                parsedContent = string.Format(@"
<%if ({0}){{%>{1}<%}}%>
", ifString, successTemplateString);
            }
            else if (!string.IsNullOrEmpty(getString))
            {
                parsedContent = string.Format(@"<%={0}%>", getString);
            }
            else
            {
                parsedContent = string.Format(@"<%{0}%>", node.InnerText);
            }

            if (!string.IsNullOrEmpty(parsedContent))
            {
                StringBuilder innerBuilder = new StringBuilder(parsedContent);
                ParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);

                parsedContent = innerBuilder.ToString();
            }

            return parsedContent;
        }
    }
}
