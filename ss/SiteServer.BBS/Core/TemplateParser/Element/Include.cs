using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Xml;
using System.Collections;
using BaiRong.Core;
using SiteServer.BBS.Core.TemplateParser.Model;

namespace SiteServer.BBS.Core.TemplateParser.Element
{
    public class Include
    {
        private Include() { }
        public const string ElementName = "bbs:include";//包含文件

        public const string Attribute_File = "file";	//文件路径
        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_File, "文件路径");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
                return attributes;
            }
        }

        public static string Parse(string element, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                IEnumerator ie = node.Attributes.GetEnumerator();
                string file = string.Empty;
                bool isDynamic = false;

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(Include.Attribute_File))
                    {
                        file = attr.Value;
                    }
                    else if (attributeName.Equals(Include.Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value);
                    }
                }

                if (isDynamic)
                {
                    parsedContent = Dynamic.ParseDynamicElement(ElementName, element, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(pageInfo, contextInfo, file);
                }
            }
            catch (Exception ex)
            {
                parsedContent = ParserUtility.GetErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, string file)
        {
            string parsedContent = string.Empty;

            if (!string.IsNullOrEmpty(file))
            {
                StringBuilder contentBuilder = new StringBuilder(CreateCacheManager.FileContent.GetIncludeContent(pageInfo.PublishmentSystemID, file));
                ParserManager.ParseTemplateContent(contentBuilder, pageInfo, contextInfo);
                parsedContent = contentBuilder.ToString();
            }

            return parsedContent;
        }
    }
}
