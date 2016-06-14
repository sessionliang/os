using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System.Text.RegularExpressions;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CMS.Core;
using SiteServer.STL.StlTemplate;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlCommentInput
    {
        private StlCommentInput() { }
        public const string ElementName = "stl:commentinput";

        public const string Attribute_IsLoginFirst = "isloginfirst";                //评论是否需要登录

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_IsLoginFirst, "评论是否需要登录");
                return attributes;
            }
        }

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                IEnumerator ie = node.Attributes.GetEnumerator();

                bool isLoginFirst = true;

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(Attribute_IsLoginFirst))
                    {
                        isLoginFirst = TranslateUtils.ToBool(attr.Value);
                    }
                }

                parsedContent = ParseImpl(pageInfo, contextInfo, node, isLoginFirst);
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, XmlNode node, bool isLoginFirst)
        {
            string parsedContent = string.Empty;

            pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.A_Platform_BASIC);
            pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.A_Platform_USER);
            pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.B_CMS_COMMENT);

            string innerHtml = node.InnerXml;
            if (string.IsNullOrEmpty(innerHtml))
            {
                string filePath = PathUtils.GetSiteFilesPath("services/cms/components/comment/commentInputTemplate.html");
                innerHtml = FileUtils.ReadText(filePath, ECharset.utf_8);
                innerHtml = StlParserUtility.HtmlToXml(innerHtml);
            }

            StringBuilder builder = new StringBuilder(innerHtml);
            StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

            parsedContent = string.Format(@"
<script type=""text/html"" class=""commentController"">
    {0}
</script>
<script>function isLoginFirst(){{return {1};}}</script>
", builder, isLoginFirst ? "true" : "false");

            return parsedContent;
        }
    }
}
