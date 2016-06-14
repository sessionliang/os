using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System;
using SiteServer.STL.IO;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlContainer
    {
        private StlContainer() { }
        public const string ElementName = "stl:container";                  //容器

        public const string Attribute_Type = "type";
        public const string Attribute_Context = "context";                  //所处上下文

        //public const string TYPE_B2C = "b2c";
        //public const string TYPE_B2C_FILTER = "filter";
        //public const string TYPE_B2C_ORDER = "order";
        //public const string TYPE_B2C_ORDER_SUCCESS = "orderSuccess";
        //public const string TYPE_B2C_ORDER_RETURN = "orderReturn";

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_Context, "所处上下文");
                return attributes;
            }
        }

        public static string GetContainer(string content)
        {
            return string.Format(@"
<stl:container>
{0}
</stl:container>
", content);
        }

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfoRef)
        {
            string parsedContent = string.Empty;

            string type = string.Empty;
            ContextInfo contextInfo = contextInfoRef.Clone();
            try
            {
                IEnumerator ie = node.Attributes.GetEnumerator();

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlContainer.Attribute_Context))
                    {
                        contextInfo.ContextType = EContextTypeUtils.GetEnumType(attr.Value);
                    }
                    else if (attributeName.Equals(StlContainer.Attribute_Type))
                    {
                        type = attr.Value;
                    }
                }

//                if (!string.IsNullOrEmpty(type))
//                {
//                    string innerHtml = node.InnerXml;

//                    if (StringUtils.EqualsIgnoreCase(type, StlContainer.TYPE_B2C))
//                    {
//                        type = StlContainer.TYPE_B2C;
//                    }
//                    else if (StringUtils.EqualsIgnoreCase(type, StlContainer.TYPE_B2C_FILTER))
//                    {
//                        type = StlContainer.TYPE_B2C_FILTER;
//                        pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_FILTER);
//                    }
//                    else if (StringUtils.EqualsIgnoreCase(type, StlContainer.TYPE_B2C_ORDER))
//                    {
//                        type = StlContainer.TYPE_B2C_ORDER;
//                        pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER);
//                        if (string.IsNullOrEmpty(innerHtml))
//                        {
//                            string filePath = PathUtils.GetSiteFilesPath("services/b2c/components/template/order.html");
//                            innerHtml = FileUtils.ReadText(filePath, ECharset.utf_8);
//                            innerHtml = StlParserUtility.HtmlToXml(innerHtml);
//                        }
//                    }
//                    else if (StringUtils.EqualsIgnoreCase(type, StlContainer.TYPE_B2C_ORDER_SUCCESS))
//                    {
//                        type = StlContainer.TYPE_B2C_ORDER_SUCCESS;
//                        pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER_SUCCESS);
//                    }
//                    else if (StringUtils.EqualsIgnoreCase(type, StlContainer.TYPE_B2C_ORDER_RETURN))
//                    {
//                        type = StlContainer.TYPE_B2C_ORDER_RETURN;
//                        pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER_RETURN);
//                    }
//                    else
//                    {
//                        type = string.Empty;
//                    }

//                    StringBuilder builder = new StringBuilder(innerHtml);
//                    StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

//                    if (!string.IsNullOrEmpty(type))
//                    {
//                        parsedContent = string.Format(@"
//<script type=""text/html"" class=""{0}Controller"">
//    {1}
//</script>
//", type, builder);
//                    }
//                    else
//                    {
//                        parsedContent = builder.ToString();
//                    }
//                }
//                else
//                {
                    if (!string.IsNullOrEmpty(node.InnerXml))
                    {
                        string innerHtml = RegexUtils.GetInnerContent(StlContainer.ElementName, stlElement);

                        StringBuilder builder = new StringBuilder(innerHtml);
                        StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

                        parsedContent = builder.ToString();
                    }
                //}
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }
    }
}
