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
    public class StlCompareInput
    {
        private StlCompareInput() { }
        public const string ElementName = "stl:compareinput";

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
            //判断当前内容的栏目是否比较反馈
            NodeInfo nodeinfo = NodeManager.GetNodeInfo(contextInfo.PublishmentSystemInfo.PublishmentSystemID, contextInfo.ChannelID);
            if (!nodeinfo.Additional.IsUseCompare)
            {
                return StlParserUtility.GetStlErrorMessage(ElementName, new Exception("当前内容所属栏目未开启比较反馈，比较反馈标签无效！"));
            }
            string parsedContent = string.Empty;
            try
            {
                IEnumerator ie = node.Attributes.GetEnumerator();

                bool isLoginFirst = true;

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
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
            pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.B_CMS_COMPARE);

            string innerHtml = node.InnerXml;
            if (string.IsNullOrEmpty(innerHtml))
            {
                string filePath = PathUtils.GetSiteFilesPath("services/cms/components/compare/compareInputTemplate.html");
                innerHtml = FileUtils.ReadText(filePath, ECharset.utf_8);
                innerHtml = StlParserUtility.HtmlToXml(innerHtml);
            }

            StringBuilder builder = new StringBuilder(innerHtml);



            StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

            parsedContent = string.Format(@"
<script type=""text/html"" class=""compareController"">
    {0}
</script>
<script>function isLoginFirst(){{return {1};}}</script>
", builder, isLoginFirst ? "true" : "false");

            return parsedContent;
        }
    }
}
