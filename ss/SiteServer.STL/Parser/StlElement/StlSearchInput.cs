using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System;
using SiteServer.CMS.Core;
using SiteServer.STL.StlTemplate;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlSearchInput
    {
        private StlSearchInput() { }
        public const string ElementName = "stl:searchinput";

        public const string Attribute_StyleName = "stylename";		        //样式名称

        public const string Attribute_ChannelIndex = "channelindex";		//栏目索引
        public const string Attribute_ChannelName = "channelname";			//栏目名称
        public const string Attribute_Parent = "parent";					//显示父栏目
        public const string Attribute_UpLevel = "uplevel";					//上级栏目的级别
        public const string Attribute_TopLevel = "toplevel";				//从首页向下的栏目级别

        public const string Attribute_SearchUrl = "searchurl";			    //搜索页地址
        public const string Attribute_Type = "type";			            //搜索类型
        public const string Attribute_IsLoadValues = "isloadvalues";		//是否载入搜索条件
        public const string Attribute_OpenWin = "openwin";                  //是否新窗口打开搜索页

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_StyleName, "样式名称");
                attributes.Add(Attribute_ChannelIndex, "栏目索引");
                attributes.Add(Attribute_ChannelName, "栏目名称");
                attributes.Add(Attribute_Parent, "显示父栏目");
                attributes.Add(Attribute_UpLevel, "上级栏目的级别");
                attributes.Add(Attribute_TopLevel, "从首页向下的栏目级别");
                attributes.Add(Attribute_SearchUrl, "搜索页地址");
                attributes.Add(Attribute_Type, "搜索类型");
                attributes.Add(Attribute_IsLoadValues, "是否载入搜索条件");
                attributes.Add(Attribute_OpenWin, "是否新窗口打开搜索页");
                return attributes;
            }
        }

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                string styleName = string.Empty;
                TagStyleSearchInputInfo inputInfo = new TagStyleSearchInputInfo(string.Empty);
                string channelIndex = string.Empty;
                string channelName = string.Empty;
                int upLevel = 0;
                int topLevel = -1;
                bool isChannelDefined = false;
                bool isSearchUrlExists = false;
                string searchUrl = inputInfo.SearchUrl;
                bool isOpenWinExists = false;
                bool openWin = inputInfo.OpenWin;
                bool isLoadValues = false;
                string type = string.Empty;
                NameValueCollection formAttributes = new NameValueCollection();

                IEnumerator ie = node.Attributes.GetEnumerator();

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlSearchInput.Attribute_StyleName))
                    {
                        styleName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(StlSearchInput.Attribute_ChannelIndex))
                    {
                        channelIndex = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                        isChannelDefined = true;
                    }
                    else if (attributeName.Equals(StlSearchInput.Attribute_ChannelName))
                    {
                        channelName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                        isChannelDefined = true;
                    }
                    else if (attributeName.Equals(StlSearchInput.Attribute_Parent))
                    {
                        if (TranslateUtils.ToBool(attr.Value))
                        {
                            upLevel = 1;
                        }
                        isChannelDefined = true;
                    }
                    else if (attributeName.Equals(StlSearchInput.Attribute_UpLevel))
                    {
                        upLevel = TranslateUtils.ToInt(attr.Value);
                        isChannelDefined = true;
                    }
                    else if (attributeName.Equals(StlSearchInput.Attribute_TopLevel))
                    {
                        topLevel = TranslateUtils.ToInt(attr.Value);
                        isChannelDefined = true;
                    }
                    else if (attributeName.Equals(StlSearchInput.Attribute_SearchUrl))
                    {
                        isSearchUrlExists = true;
                        searchUrl = attr.Value;
                    }
                    else if (attributeName.Equals(StlSearchInput.Attribute_OpenWin))
                    {
                        isOpenWinExists = true;
                        openWin = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(StlSearchInput.Attribute_IsLoadValues))
                    {
                        isLoadValues = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(StlSearchInput.Attribute_Type))
                    {
                        type = attr.Value;
                    }
                    else
                    {
                        formAttributes.Add(attr.Name, attr.Value);
                    }
                }

                searchUrl = StlEntityParser.ReplaceStlEntitiesForAttributeValue(searchUrl, pageInfo, contextInfo);
                searchUrl = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, searchUrl);

                StringBuilder attributeBuilder = new StringBuilder();
                foreach (string key in formAttributes.Keys)
                {
                    attributeBuilder.AppendFormat(@" {0}=""{1}""", StringUtils.ValueToUrl(key), StringUtils.ValueToUrl(formAttributes[key]));
                }

                string formID = string.Empty;

                int channelID = pageInfo.PublishmentSystemID;
                if (isChannelDefined)
                {
                    channelID = StlDataUtility.GetNodeIDByLevel(pageInfo.PublishmentSystemID, contextInfo.ChannelID, upLevel, topLevel);
                    channelID = CreateCacheManager.NodeID.GetNodeIDByChannelIDOrChannelIndexOrChannelName(pageInfo.PublishmentSystemID, channelID, channelIndex, channelName);
                }

                if (string.IsNullOrEmpty(node.InnerXml))
                {
                    parsedContent = ParseImpl(pageInfo, contextInfo, styleName, isSearchUrlExists, searchUrl, isOpenWinExists, openWin, isLoadValues, type, attributeBuilder.ToString(), out formID);
                }
                else if (StringUtils.EqualsIgnoreCase(type, "filter"))
                {
                    //过滤搜索，搜索结果输出在栏目页面内
                    StringBuilder filterBuilder = new StringBuilder();
                    string innerXml = StringUtils.StripTags(node.InnerXml, "stl:inputTemplate");//内部html
                    StringBuilder filterInnneBuilder = new StringBuilder(innerXml);
                    StlParserManager.ParseInnerContent(filterInnneBuilder, pageInfo, contextInfo);
                    filterInnneBuilder.Replace("{", "{{");
                    filterInnneBuilder.Replace("}", "}}");
                    filterBuilder.AppendFormat(filterInnneBuilder.ToString());

                    //动态加载容器模板 {filterSearchTemplate.channelName} {filterSearchTemplate.navigationUrl} {filterSearchTemplate.contentCount}  stl:filterSearchTemplate
                    string filterSearchDataTemplate = string.Empty;
                    List<string> filterSearchElementList = StlParserUtility.GetStlElementList(node.InnerXml);
                    foreach (string filterSearchElement in filterSearchElementList)
                    {
                        if (StlParserUtility.IsSpecifiedStlElement(filterSearchElement, "stl:filtersearchtemplate"))
                        {
                            string filterSearchDataInnerXml = StlParserUtility.GetInnerXml(filterSearchElement, true);
                            StringBuilder filterSearchDataInnneBuilder = new StringBuilder(filterSearchDataInnerXml);
                            StlParserManager.ParseInnerContent(filterSearchDataInnneBuilder, pageInfo, contextInfo);
                            filterSearchDataInnneBuilder = filterSearchDataInnneBuilder.Replace("{filterSearchTemplate.channelName}", "{0}");
                            filterSearchDataInnneBuilder = filterSearchDataInnneBuilder.Replace("{filterSearchTemplate.navigationUrl}", "{1}");
                            filterSearchDataInnneBuilder = filterSearchDataInnneBuilder.Replace("{filterSearchTemplate.contentCount}", "{2}");
                            filterSearchDataTemplate = filterSearchDataInnneBuilder.ToString();
                            filterBuilder.Replace(filterSearchDataInnerXml, "");
                            filterBuilder = new StringBuilder(StringUtils.StripTags(filterBuilder.ToString(), "stl:filtersearchtemplate"));
                            break;
                        }
                    }
                    string apiString = "/api/filter/PostResults";
                    string clickString = "systemFilterSearch(true);";
                    string keyUpString = "systemFilterSearch(false);";
                    string functionString = string.Format(@"<script type='text/javascript'>
                                                String.prototype.format = function(){{
                                                    var args = arguments;
                                                    return this.replace(/\{{(\d+)\}}/g, function(m,i){{
                                                        return args[i];
                                                    }});
                                                }}
                                                function systemFilterSearch(isRedirect){{
                                                    var filterSearchWords = $('#filterSearchWords').val();
                                                    var filterSearchDataTemplate = '{2}';
                                                    if(!filterSearchWords){{
                                                       $('#filterSearchPannel').html('未找到相关商品');
                                                       $('#filterSearchPannel').css('display','none');
                                                       //notifyService.error('未找到相关商品');
                                                    }}
                                                    else{{
                                                        if(!isRedirect){{
                                                            filterService.getResults({{filterSearchWords:filterSearchWords,isRedirect:isRedirect }},function(data){{
                                                                if(data.length == 0){{                                                                                                                                     
                                                                    $('#filterSearchPannel').html('未找到相关商品');
                                                                    $('#filterSearchPannel').css('display','none');
                                                                    $('#filterSearchPannel').append('未找到相关商品');
                                                                }}
                                                                else{{
                                                                $('#filterSearchPannel').html('');
                                                                for(var i=0;i<data.length;i++){{
                                                                    $('#filterSearchPannel').append(filterSearchDataTemplate.format(data[i].nodeName,'{3}?isRedirect=true&filterSearchWords='+encodeURIComponent(data[i].nodeName)+'&channelId='+data[i].nodeID,data[i].contentNum));
                                                                }}
                                                                $('#filterSearchPannel').css('display','block');
                                                                }}
                                                            }});
                                                        }}
                                                        else{{
                                                            top.location.href = '{3}?isRedirect=true&filterSearchWords='+encodeURIComponent(filterSearchWords);
                                                        }}
                                                    }}
                                                }}
                                                </script>", "defaulturl", apiString, filterSearchDataTemplate.Trim(), searchUrl);

                    ArrayList filterStlFormElements = StlHtmlUtility.GetStlFormElementsArrayList(filterInnneBuilder.ToString());
                    if (filterStlFormElements != null && filterStlFormElements.Count > 0)
                    {
                        foreach (string filterStlFormEletment in filterStlFormElements)
                        {
                            XmlNode filterElementNode;
                            NameValueCollection filterAttributes;
                            StlHtmlUtility.ReWriteFormElements(filterStlFormEletment, out filterElementNode, out filterAttributes);
                            if (StringUtils.EqualsIgnoreCase(filterElementNode.Name, "input") && filterAttributes["type"] == "text")
                            {
                                //关键字
                                filterAttributes.Remove("type");
                                filterAttributes.Remove("id");
                                HtmlInputText htmlInputText = new HtmlInputText("text");
                                htmlInputText.ID = "filterSearchWords";
                                htmlInputText.Attributes.Add("onkeyup", keyUpString);
                                StringDictionary sdInputText = new StringDictionary();
                                foreach (string filterAttributeKey in filterAttributes.AllKeys)
                                {
                                    sdInputText.Add(filterAttributeKey, filterAttributes[filterAttributeKey]);
                                }
                                ControlUtils.AddAttributesIfNotExists(htmlInputText, sdInputText);
                                filterBuilder.Replace(filterStlFormEletment, ControlUtils.GetControlRenderHtml(htmlInputText));
                            }
                            else if (StringUtils.EqualsIgnoreCase(filterElementNode.Name, "input") && (filterAttributes["type"] == "button" || filterAttributes["type"] == "submit"))
                            {
                                //提交按钮
                                filterAttributes.Remove("type");
                                filterAttributes.Remove("id");

                                HtmlInputText htmlInputButton = new HtmlInputText("button");
                                htmlInputButton.ID = "filterSearchBtn";
                                htmlInputButton.Attributes.Add("onclick", clickString);
                                StringDictionary sdInputButton = new StringDictionary();
                                foreach (string filterAttributeKey in filterAttributes.AllKeys)
                                {
                                    sdInputButton.Add(filterAttributeKey, filterAttributes[filterAttributeKey]);
                                }
                                ControlUtils.AddAttributesIfNotExists(htmlInputButton, sdInputButton);
                                filterBuilder.Replace(filterStlFormEletment, ControlUtils.GetControlRenderHtml(htmlInputButton));
                            }
                        }
                    }
                    filterBuilder.Append(functionString);
                    return filterBuilder.ToString();
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    string ajaxDivID = StlParserUtility.GetAjaxDivID(pageInfo.UniqueID);
                    formID = ajaxDivID;
                    string clickString = string.Format("document.getElementById('{0}').submit();return false;", ajaxDivID);

                    if (isLoadValues)
                    {
                        openWin = false;
                    }

                    builder.AppendFormat(@"<form id=""{0}"" action=""{1}"" target=""{2}""{3}>", ajaxDivID, searchUrl, openWin ? "_blank" : "_self", attributeBuilder.ToString());

                    if (!string.IsNullOrEmpty(type))
                    {
                        builder.AppendFormat(@"<input type=""hidden"" id=""type"" name=""type"" value=""{0}"" />", type);
                    }

                    string innerXml = StringUtils.StripTags(node.InnerXml, "stl:inputTemplate");
                    StringBuilder innerBuilder = new StringBuilder(innerXml);
                    StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                    builder.Append(innerBuilder.ToString());

                    StlHtmlUtility.ReWriteSubmitButton(builder, clickString);

                    ArrayList stlFormElements = StlHtmlUtility.GetStlFormElementsArrayList(builder.ToString());
                    bool isChannelIDExists = false;
                    if (stlFormElements != null && stlFormElements.Count > 0)
                    {
                        foreach (string stlFormElement in stlFormElements)
                        {
                            XmlNode elementNode;
                            NameValueCollection attributes;
                            StlHtmlUtility.ReWriteFormElements(stlFormElement, out elementNode, out attributes);
                            if (StringUtils.EqualsIgnoreCase(attributes["id"], "channelID"))
                            {
                                isChannelIDExists = true;
                            }

                            if (StringUtils.EqualsIgnoreCase(elementNode.Name, "select"))
                            {
                                if (StringUtils.EqualsIgnoreCase(attributes["id"], "channelID"))
                                {
                                    if (string.IsNullOrEmpty(elementNode.InnerXml))
                                    {
                                        System.Web.UI.WebControls.DropDownList ddl = new System.Web.UI.WebControls.DropDownList();
                                        ddl.ID = "channelID";
                                        NodeManager.AddListItems(ddl.Items, pageInfo.PublishmentSystemInfo, false, false);
                                        if (ddl.Items.Count > 0)
                                        {
                                            ddl.Items[0].Text = "└所有栏目";
                                        }
                                        builder.Replace(stlFormElement, ControlUtils.GetControlRenderHtml(ddl));
                                        continue;
                                    }
                                }
                                else if (StringUtils.EqualsIgnoreCase(attributes["id"], "type"))
                                {
                                    if (string.IsNullOrEmpty(elementNode.InnerXml))
                                    {
                                        elementNode.InnerXml = string.Format(@"
                    <option value=""Title"">标题</option>
                    <option value=""Content"">内容</option>
                    ");
                                    }
                                }
                                else if (StringUtils.EqualsIgnoreCase(attributes["id"], "date"))
                                {
                                    if (string.IsNullOrEmpty(elementNode.InnerXml))
                                    {
                                        elementNode.InnerXml = string.Format(@"
                    <option value=""0"">全部时间</option>
                    <option value=""1"">1天内</option>
                    <option value=""7"">1周内</option>
                    <option value=""30"">1个月内</option>
                    <option value=""365"">1年内</option>
                    ");
                                    }
                                }
                            }
                            else if (StringUtils.EqualsIgnoreCase(elementNode.Name, "input"))
                            {
                                if (StringUtils.EqualsIgnoreCase(attributes["id"], "dateFrom") || StringUtils.EqualsIgnoreCase(attributes["id"], "dateTo"))
                                {
                                    if (string.IsNullOrEmpty(attributes["onfocus"]))
                                    {
                                        pageInfo.AddPageScriptsIfNotExists(PageInfo.Js_Inner_Calendar);
                                        attributes["onfocus"] = "CalendarWebControl.show(this,false,this.value);";
                                    }
                                }
                                else if (StringUtils.EqualsIgnoreCase(attributes["id"], "channelID"))
                                {
                                    if (StringUtils.EqualsIgnoreCase(attributes["type"], "radio"))
                                    {
                                        if (!string.IsNullOrEmpty(attributes["value"]) && !StringUtils.StartsWithIgnoreCase(attributes["value"], "{"))
                                        {
                                            NodeInfo nodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, DataProvider.NodeDAO.GetNodeIDByNodeIndexName(pageInfo.PublishmentSystemID, attributes["value"]));
                                            if (nodeInfo != null)
                                            {
                                                attributes["value"] = nodeInfo.NodeID.ToString();
                                            }
                                            else
                                            {
                                                attributes["value"] = pageInfo.PublishmentSystemID.ToString();
                                            }
                                        }
                                        attributes["id"] = "channelID_" + attributes["value"];
                                        attributes["name"] = "channelID";
                                    }
                                }
                            }

                            if (StringUtils.EqualsIgnoreCase(elementNode.Name, "input"))
                            {
                                builder.Replace(stlFormElement, string.Format(@"<{0} {1}/>", elementNode.Name, TranslateUtils.ToAttributesString(attributes)));
                            }
                            else
                            {
                                builder.Replace(stlFormElement, string.Format(@"<{0} {1}>{2}</{0}>", elementNode.Name, TranslateUtils.ToAttributesString(attributes), elementNode.InnerXml));
                            }
                        }
                    }

                    if (!isChannelIDExists)
                    {
                        builder.AppendFormat(@"<input type=""hidden"" id=""channelID"" name=""channelID"" value=""{0}"" />", channelID);
                    }

                    builder.Append("</form>");

                    parsedContent = builder.ToString();
                }

                if (isLoadValues)
                {
                    pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.A_JQuery);

                    pageInfo.AddPageScriptsIfNotExists(StlSearchInput.ElementName, string.Format(@"
<script language=""vbscript""> 
Function str2asc(strstr) 
 str2asc = hex(asc(strstr)) 
End Function 
Function asc2str(ascasc) 
 asc2str = chr(ascasc) 
End Function 
</script>
<script type=""text/javascript"" src=""{0}""></script>", StlTemplateManager.Search.ScriptUrl));

                    bool isUtf8 = false;
                    if (ECharsetUtils.Equals(pageInfo.PublishmentSystemInfo.Additional.Charset, ECharset.utf_8))
                    {
                        isUtf8 = true;
                    }
                    parsedContent += string.Format(@"
<script type=""text/javascript"">
jQuery(document).ready(function() {{
	stlSearchLoadValues('{0}', {1});
}});
</script>
", formID, isUtf8.ToString().ToLower());
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        public static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, string styleName, bool isSearchUrlExists, string searchUrl, bool isOpenWinExists, bool openWin, bool isLoadValues, string type, string formAttributes, out string formID)
        {
            string parsedContent = string.Empty;

            TagStyleInfo styleInfo = TagStyleManager.GetTagStyleInfo(pageInfo.PublishmentSystemID, ElementName, styleName);
            if (styleInfo == null)
            {
                styleInfo = new TagStyleInfo();
            }
            TagStyleSearchInputInfo inputInfo = new TagStyleSearchInputInfo(styleInfo.SettingsXML);

            if (isSearchUrlExists)
            {
                inputInfo.SearchUrl = searchUrl;
            }
            if (isOpenWinExists)
            {
                inputInfo.OpenWin = openWin;
            }

            SearchInputTemplate inputTemplate = new SearchInputTemplate(pageInfo.PublishmentSystemInfo, styleInfo, inputInfo, type, formAttributes);
            parsedContent = inputTemplate.GetTemplate(styleInfo.IsTemplate);

            formID = inputTemplate.FormID;

            return parsedContent;
        }
    }
}
