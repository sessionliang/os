using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System;
using SiteServer.STL.Parser.StlEntity;
using SiteServer.CMS.Core;
using SiteServer.STL.StlTemplate;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlDynamic
    {
        private StlDynamic() { }
        public const string ElementName = "stl:dynamic";//显示动态内容

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
                attributes.Add(Attribute_IsPageRefresh, "翻页时是否刷新页面");
                return attributes;
            }
        }

        internal static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfoRef)
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
                    if (attributeName.Equals(StlDynamic.Attribute_ID))
                    {
                        htmlID = attr.Value;
                    }
                    else if (attributeName.Equals(StlDynamic.Attribute_Context))
                    {
                        contextInfo.ContextType = EContextTypeUtils.GetEnumType(attr.Value);
                    }
                    else if (attributeName.Equals(StlDynamic.Attribute_IsPageRefresh))
                    {
                        isPageRefresh = TranslateUtils.ToBool(attr.Value);
                    }
                }

                parsedContent = ParseImpl(pageInfo, contextInfo, ElementName, node.InnerXml, isPageRefresh);
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, string elementName, string templateContent, bool isPageRefresh)
        {
            pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.A_JQuery);
            //pageInfo.AddPageScriptsIfNotExists(PageInfo.Js_Aj_JSON);
            //pageInfo.AddPageScriptsIfNotExists(PageInfo.Js_Page_Script);
            //pageInfo.AddPageScriptsIfNotExists(PageInfo.Js_Validate_Script);

            //StlScriptUtility.RegisteScript(pageInfo, elementName);

            string ajaxDivID = StlParserUtility.GetAjaxDivID(pageInfo.UniqueID);

            string functionName = string.Empty;
            if (StringUtils.EqualsIgnoreCase(elementName, StlComments.ElementName))
            {
                functionName = "stlDynamic_Comments";
            }
            else
            {
                functionName = string.Format("stlDynamic_{0}", ajaxDivID);
            }

            if (string.IsNullOrEmpty(templateContent))
            {
                return string.Empty;
            }

            string outputPageUrl = StlTemplateManager.Dynamic.GetPageUrlOutput(pageInfo.PublishmentSystemInfo);
            string currentPageUrl = StlUtility.GetStlCurrentUrl(pageInfo, contextInfo.ChannelID, contextInfo.ContentID, contextInfo.ContentInfo);
            currentPageUrl = PageUtils.AddQuestionOrAndToUrl(currentPageUrl);
            string outputParms = StlTemplateManager.Dynamic.GetOutputParameters(contextInfo.ChannelID, contextInfo.ContentID, pageInfo.TemplateInfo.TemplateID, currentPageUrl, ajaxDivID, isPageRefresh, templateContent);

            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(@"<span id=""{0}""></span>", ajaxDivID);

            builder.AppendFormat(@"
<script type=""text/javascript"" language=""javascript"">
function getQueryString()
{{
  var i, len, params = '', hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
  if(hashes.length == 0) return params;
  for(i = 0, len = hashes.length, len; i < len; i+=1) {{
    hash = hashes[i].split('=');
    if (hash && hash.length == 2){{
      value = encodeURIComponent(decodeURIComponent(decodeURIComponent(hash[1])));
      params += (hash[0] + '=' + value + '&');
    }}
  }}
  if (params && params.length > 0) params = params.substring(0, params.length - 1);
  return params;
}}
function mapPath(input)
{{
    var sitePath = ""{4}"";
    sitePath = ""/"" + sitePath;
    var sitePathRegex = new RegExp(sitePath, ""g"");
    var output = input.replace(""\""""+sitePath+""\"""", """"/"""").replace(""'""+sitePath+""'"", ""'/'"");
    output = input.replace(sitePathRegex, """");
    return output;
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
        //jQuery.post(url, pars, function(data,textStatus){{
            //jQuery(""#{3}"").html(data);
        //}});
        $.ajax({{
                url: url,
                type: ""POST"",
                mimeType:""multipart/form-data"",
                //contentType: false,
                //processData: false,
                cache: false,
                xhrFields: {{   
                    withCredentials: true   
                }},
                data: pars,
                success: function(json, textStatus, jqXHR){{
                    if({5})
                        json = mapPath(json);
                    jQuery(""#{3}"").html(json);
                }}
          }});
    }}catch(e){{}}
}}
{0}(0);
</script>
", functionName, outputPageUrl, outputParms, ajaxDivID, pageInfo.PublishmentSystemInfo.PublishmentSystemUrl.TrimStart('/'), pageInfo.PublishmentSystemInfo.Additional.IsSonSiteAlone.ToString().ToLower());

            return builder.ToString();
        }

        internal static string ParseDynamicElement(string elementName, string stlElement, PageInfo pageInfo, ContextInfo contextInfo)
        {
            stlElement = StringUtils.ReplaceIgnoreCase(stlElement, "isdynamic=\"true\"", string.Empty);
            return ParseImpl(pageInfo, contextInfo, elementName, stlElement, false);
        }
    }
}
