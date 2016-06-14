using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using BaiRong.Core.IO;
using System;
using SiteServer.CMS.Core;
using SiteServer.STL.StlTemplate;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlSearchOutput
    {
        private StlSearchOutput() { }
        public const string ElementName = "stl:searchoutput";

        public const string Attribute_PageNum = "pagenum";			                //ÿҳ��ʾ��������Ŀ
        public const string Attribute_Width = "width";				                //���
        public const string Attribute_IsHighlight = "ishighlight";			        //�Ƿ�ؼ��ָ���
        public const string Attribute_IsRedirectSingle = "isredirectsingle";        //���������һ��ʱ�Ƿ���ת
        public const string Attribute_IsDefaultDisplay = "isdefaultdisplay";        //�Ƿ�Ĭ����ʾ
        public const string Attribute_DateAttribute = "dateattribute";              //���������ֶ�

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_PageNum, "ÿҳ��ʾ��������Ŀ");
                attributes.Add(Attribute_Width, "���");
                attributes.Add(Attribute_IsHighlight, "�Ƿ�ؼ��ָ���");
                attributes.Add(Attribute_IsRedirectSingle, "���������һ��ʱ�Ƿ���ת");
                attributes.Add(Attribute_IsDefaultDisplay, "�Ƿ�Ĭ����ʾ");
                attributes.Add(Attribute_DateAttribute, "���������ֶ�");
                return attributes;
            }
        }

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                IEnumerator ie = node.Attributes.GetEnumerator();
                int pageNum = 0;
                string width = string.Empty;
                bool isHighlight = false;
                bool isRedirectSingle = false;
                bool isDefaultDisplay = false;
                string dateAttribute = ContentAttribute.AddDate;

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlSearchOutput.Attribute_PageNum))
                    {
                        pageNum = TranslateUtils.ToInt(attr.Value, 0);
                    }
                    else if (attributeName.Equals(StlSearchOutput.Attribute_Width))
                    {
                        width = attr.Value;
                    }
                    else if (attributeName.Equals(StlSearchOutput.Attribute_IsHighlight))
                    {
                        isHighlight = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(StlSearchOutput.Attribute_IsRedirectSingle))
                    {
                        isRedirectSingle = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(StlSearchOutput.Attribute_IsDefaultDisplay))
                    {
                        isDefaultDisplay = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(StlSearchOutput.Attribute_DateAttribute))
                    {
                        dateAttribute = attr.Value;
                    }
                }

                string successTemplateString = string.Empty;
                string failureTemplateString = string.Empty;
                StlParserUtility.GetInnerTemplateString(node, pageInfo, out successTemplateString, out failureTemplateString);

                if (string.IsNullOrEmpty(successTemplateString))
                {
                    successTemplateString = FileUtils.ReadText(StlTemplateManager.Search.SuccessTemplatePath, ECharset.utf_8);
                }
                else
                {
                    successTemplateString = StlParserUtility.XmlToHtml(successTemplateString);
                }

                if (string.IsNullOrEmpty(failureTemplateString))
                {
                    failureTemplateString = FileUtils.ReadText(StlTemplateManager.Search.FailureTemplatePath, ECharset.utf_8);
                }
                else
                {
                    failureTemplateString = StlParserUtility.XmlToHtml(failureTemplateString);
                }

                pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.A_JQuery);

                string ajaxDivID = StlParserUtility.GetAjaxDivID(pageInfo.UniqueID);

                string innerPageUrl = StlTemplateManager.Search.GetPageUrlOutput(pageInfo.PublishmentSystemInfo);

                string innerPageParms = StlTemplateManager.Search.GetOutputParameters(ajaxDivID, pageNum, isHighlight, isRedirectSingle, isDefaultDisplay, dateAttribute, pageInfo.TemplateInfo.Charset, successTemplateString, failureTemplateString);

                StringBuilder builder = new StringBuilder();
                if (string.IsNullOrEmpty(width))
                {
                    builder.AppendFormat(@"<div id=""{0}"">", ajaxDivID);
                }
                else
                {
                    builder.AppendFormat(@"<div id=""{0}"" style=""width:{1}"">", ajaxDivID, width);
                }
                builder.AppendFormat(@"<img src=""{0}"" />", PageUtility.GetIconUrl(pageInfo.PublishmentSystemInfo, "loading.gif"));
                builder.Append("</div>");

                string updateScript = string.Format(@"
jQuery.post(url, pars, 
function(data, textStatus){{
   if({1})
       data = mapPath(data);
   jQuery(""#{0}"").html(data);
}});", ajaxDivID, pageInfo.PublishmentSystemInfo.Additional.IsSonSiteAlone.ToString().ToLower());

                builder.AppendFormat(@"
<script type=""text/javascript"" language=""javascript"">
function stlUpdate{0}(pageIndex)
{{
    var url = '{1}';

    var queryString = document.location.search;
    var queryString2 = '';
    
	if (queryString == null || queryString.length <= 1){{queryString = '';}}else{{
        queryString = queryString.substring(1);
        var arr = queryString.split('&');
        for(var i=0; i < arr.length; i++) {{
            var item = arr[i];
            var arr2 = item.split('=');
            if (arr2 && arr2.length == 2){{
			    queryString2 += '&' + arr2[0] + '=' + encodeURIComponent(arr2[1]);
            }}else{{queryString2 += '&' + item}}
		}}
        queryString = queryString2;
    }}

    var pars = ""{2}&pageIndex="" + pageIndex + queryString;
    {3}
}}
function stlJump{0}(selObj)
{{
    stlUpdate{0}(selObj.options[selObj.selectedIndex].value);
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
stlUpdate{0}(0);
</script>
", ajaxDivID, innerPageUrl, innerPageParms, updateScript, pageInfo.PublishmentSystemInfo.PublishmentSystemUrl.TrimStart('/'));

                parsedContent = builder.ToString();
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }


    }
}
