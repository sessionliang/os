using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using System;
using SiteServer.BBS.Core.TemplateParser.Model;
using SiteServer.BBS.Model;
using SiteServer.BBS.BackgroundPages;

namespace SiteServer.BBS.Core.TemplateParser.Element
{
    public class Dynamic
	{
        private Dynamic() { }
        public const string ElementName = "bbs:dynamic";//显示动态内容

        public const string Attribute_ID = "id";				            //唯一标识符
        public const string Attribute_Context = "context";                  //所处上下文
        public const string Attribute_IsPageRefresh = "ispagerefresh";      //翻页时是否刷新页面

        public static ListDictionary AttributeList
		{
			get
			{
				ListDictionary attributes = new ListDictionary();
				attributes.Add(Attribute_ID, "唯一标识符");
                attributes.Add(Attribute_Context, "所处上下文");
				return attributes;
			}
		}

        internal static string Parse(XmlNode node, PageInfo pageInfo, ContextInfo contextInfoRef)
		{
			string parsedContent = string.Empty;
            ContextInfo contextInfo = contextInfoRef.Clone();
			try
			{
				IEnumerator ie = node.Attributes.GetEnumerator();

				string htmlID = string.Empty;
                bool isPageRefresh = false;

				while (ie.MoveNext())
				{
					XmlAttribute attr = (XmlAttribute)ie.Current;
					string attributeName = attr.Name.ToLower();
					if (attributeName.Equals(Dynamic.Attribute_ID))
					{
						htmlID = attr.Value;
					}
                    else if (attributeName.Equals(Dynamic.Attribute_Context))
                    {
                        contextInfo.ContextType = EContextTypeUtils.GetEnumType(attr.Value);
                    }
                    else if (attributeName.Equals(Dynamic.Attribute_IsPageRefresh))
                    {
                        isPageRefresh = TranslateUtils.ToBool(attr.Value);
                    }
				}

                parsedContent = ParseImpl(pageInfo, contextInfo, ElementName, node.InnerXml, isPageRefresh);
			}
            catch (Exception ex)
            {
                parsedContent = ParserUtility.GetErrorMessage(ElementName, ex);
            }

			return parsedContent;
		}

        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, string elementName, string templateContent, bool isPageRefresh)
        {
            string ajaxDivID = "ajaxElement_" + pageInfo.UniqueID;

            string functionName = string.Format("dynamic_{0}", ajaxDivID);
            
            if (string.IsNullOrEmpty(templateContent))
            {
                return string.Empty;
            }

            string innerPageUrl = BackgroundUtils.GetDynamicUrl(pageInfo.PublishmentSystemID);
            string currentPageUrl = PageUtilityBBS.GetParserCurrentUrl(pageInfo, contextInfo.ForumID, contextInfo.ThreadID);
            currentPageUrl = PageUtils.AddQuestionOrAndToUrl(currentPageUrl);

            string innerPageParms = BackgroundUtils.GetDynamicParms(pageInfo.DirectoryName, pageInfo.FileName, contextInfo.ForumID, contextInfo.ThreadID, pageInfo.TemplateType, isPageRefresh, currentPageUrl, ajaxDivID, templateContent);

            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(@"<span id=""{0}""></span>", ajaxDivID);

            builder.AppendFormat(@"
<script type=""text/javascript"" language=""javascript"">
function getQueryString()
{{
	var retval = '';
	var queryString = document.location.search;
	if (queryString == null || queryString.length <= 1) return '';
	return decodeURI(decodeURI(queryString.substring(1)));
}}
function {0}(pageNum)
{{
    try
    {{
        var url = ""{1}&"" + getQueryString();
        var pars = ""{2}"";
        if (pageNum && pageNum > 0)
        {{
            pars += ""&pageNum="" + pageNum;
        }}
        jQuery.post(url, pars, function(data, textStatus){{jQuery(""#{3}"").html(data);}});
    }}catch(e){{}}
}}
{0}(0);
</script>
", functionName, innerPageUrl, innerPageParms, ajaxDivID);

            return builder.ToString();
        }

        internal static string ParseDynamicElement(string elementName, string element, PageInfo pageInfo, ContextInfo contextInfo)
        {
            element = StringUtils.ReplaceIgnoreCase(element, "isdynamic=\"true\"", string.Empty);
            return ParseImpl(pageInfo, contextInfo, elementName, element, false);
        }
	}
}
