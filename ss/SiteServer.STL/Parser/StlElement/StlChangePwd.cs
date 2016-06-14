using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using System.Text;
using System.Collections.Generic;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlChangePwd
    {
        private StlChangePwd() { }
        public const string ElementName = "stl:changepwd";//ÐÞ¸ÄÃÜÂë

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                return attributes;
            }
        }

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                IEnumerator ie = node.Attributes.GetEnumerator();
                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();

                }

                parsedContent = ParseImpl(pageInfo, node, contextInfo);
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        public static string ParseImpl(PageInfo pageInfo, XmlNode node, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;

            pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.A_Platform_BASIC);
            pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.D_Home_CHANGEPWD);

            string filePath = PathUtils.GetSiteFilesPath("services/cms/home/template/changePwdTemplate.html");
            string innerHtml = FileUtils.ReadText(filePath, ECharset.utf_8);
            innerHtml = StlParserUtility.HtmlToXml(innerHtml);

            StringBuilder builder = new StringBuilder(innerHtml);
            StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);
            string unique = pageInfo.UniqueID.ToString();

            parsedContent = string.Format(@"
<script type=""text/html"" class=""changePwdController"">
        {0}
</script>
", builder);

            return parsedContent;
        }
    }
}
