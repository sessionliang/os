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
    public class StlSearchwordInput
    {
        private StlSearchwordInput() { }
        public const string ElementName = "stl:searchwordinput";
        public const string SimpleElementName = "stl:swinput";

        public const string Attribute_StyleName = "stylename";		        //��ʽ����

        public const string Attribute_ChannelIndex = "channelindex";		//��Ŀ����
        public const string Attribute_ChannelName = "channelname";			//��Ŀ����
        public const string Attribute_Parent = "parent";					//��ʾ����Ŀ
        public const string Attribute_UpLevel = "uplevel";					//�ϼ���Ŀ�ļ���
        public const string Attribute_TopLevel = "toplevel";				//����ҳ���µ���Ŀ����

        public const string Attribute_SearchwordUrl = "searchwordurl";			    //����ҳ��ַ
        public const string Attribute_Type = "type";			            //��������
        public const string Attribute_IsLoadValues = "isloadvalues";		//�Ƿ�������������
        public const string Attribute_OpenWin = "openwin";                  //�Ƿ��´��ڴ�����ҳ

        public const string Attribute_IsRelated = "isrelated";//�Ƿ����������

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_StyleName, "��ʽ����");
                attributes.Add(Attribute_ChannelIndex, "��Ŀ����");
                attributes.Add(Attribute_ChannelName, "��Ŀ����");
                attributes.Add(Attribute_Parent, "��ʾ����Ŀ");
                attributes.Add(Attribute_UpLevel, "�ϼ���Ŀ�ļ���");
                attributes.Add(Attribute_TopLevel, "����ҳ���µ���Ŀ����");
                attributes.Add(Attribute_SearchwordUrl, "����ҳ��ַ");
                attributes.Add(Attribute_Type, "��������");
                attributes.Add(Attribute_IsLoadValues, "�Ƿ�������������");
                attributes.Add(Attribute_OpenWin, "�Ƿ��´��ڴ�����ҳ");
                attributes.Add(Attribute_IsRelated, "�Ƿ����������");
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
                bool isSearchwordUrlExists = false;
                string searchwordUrl = inputInfo.SearchUrl;
                bool isOpenWinExists = false;
                bool openWin = inputInfo.OpenWin;
                bool isLoadValues = false;
                string type = string.Empty;
                bool isRelated = true;
                NameValueCollection formAttributes = new NameValueCollection();

                IEnumerator ie = node.Attributes.GetEnumerator();

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlSearchwordInput.Attribute_StyleName))
                    {
                        styleName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(StlSearchwordInput.Attribute_ChannelIndex))
                    {
                        channelIndex = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                        isChannelDefined = true;
                    }
                    else if (attributeName.Equals(StlSearchwordInput.Attribute_ChannelName))
                    {
                        channelName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                        isChannelDefined = true;
                    }
                    else if (attributeName.Equals(StlSearchwordInput.Attribute_Parent))
                    {
                        if (TranslateUtils.ToBool(attr.Value))
                        {
                            upLevel = 1;
                        }
                        isChannelDefined = true;
                    }
                    else if (attributeName.Equals(StlSearchwordInput.Attribute_UpLevel))
                    {
                        upLevel = TranslateUtils.ToInt(attr.Value);
                        isChannelDefined = true;
                    }
                    else if (attributeName.Equals(StlSearchwordInput.Attribute_TopLevel))
                    {
                        topLevel = TranslateUtils.ToInt(attr.Value);
                        isChannelDefined = true;
                    }
                    else if (attributeName.Equals(StlSearchwordInput.Attribute_SearchwordUrl))
                    {
                        isSearchwordUrlExists = true;
                        searchwordUrl = attr.Value;
                    }
                    else if (attributeName.Equals(StlSearchwordInput.Attribute_OpenWin))
                    {
                        isOpenWinExists = true;
                        openWin = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(StlSearchwordInput.Attribute_IsLoadValues))
                    {
                        isLoadValues = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(StlSearchwordInput.Attribute_Type))
                    {
                        type = attr.Value;
                    }
                    else if (attributeName.Equals(StlSearchwordInput.Attribute_IsRelated))
                    {
                        isRelated = TranslateUtils.ToBool(attr.Value);
                    }
                    else
                    {
                        formAttributes.Add(attr.Name, attr.Value);
                    }
                }

                string searchwordInputTemplateString = string.Empty;
                string successTemplateString = string.Empty;
                string failureTemplateString = string.Empty;
                StlParserUtility.GetInnerTemplateStringOfSearchwordInput(node, out searchwordInputTemplateString, out successTemplateString, out failureTemplateString, pageInfo, contextInfo);

                searchwordUrl = StlEntityParser.ReplaceStlEntitiesForAttributeValue(searchwordUrl, pageInfo, contextInfo);
                searchwordUrl = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, searchwordUrl);

                StringBuilder attributeBuilder = new StringBuilder();
                foreach (string key in formAttributes.Keys)
                {
                    attributeBuilder.AppendFormat(@" {0}=""{1}""", StringUtils.ValueToUrl(key), StringUtils.ValueToUrl(formAttributes[key]));
                }

                int channelID = pageInfo.PublishmentSystemID;
                if (isChannelDefined)
                {
                    channelID = StlDataUtility.GetNodeIDByLevel(pageInfo.PublishmentSystemID, contextInfo.ChannelID, upLevel, topLevel);
                    channelID = CreateCacheManager.NodeID.GetNodeIDByChannelIDOrChannelIndexOrChannelName(pageInfo.PublishmentSystemID, channelID, channelIndex, channelName);
                }

                parsedContent = ParseImpl(pageInfo, contextInfo, searchwordInputTemplateString, styleName, isSearchwordUrlExists, searchwordUrl, isOpenWinExists, openWin, isLoadValues, type, isRelated, attributeBuilder);
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        /// <summary>
        /// ת���û��Զ����ģ������
        /// </summary>
        /// <param name="node"></param>
        /// <param name="pageInfo"></param>
        /// <param name="contextInfo"></param>
        /// <param name="parsedContent"></param>
        /// <param name="searchwordUrl"></param>
        /// <param name="openWin"></param>
        /// <param name="isLoadValues"></param>
        /// <param name="type"></param>
        /// <param name="attributeBuilder"></param>
        /// <param name="formID"></param>
        /// <param name="channelID"></param>
        private static string ParseTemplateToHtml(PageInfo pageInfo, ContextInfo contextInfo, string parsedContent, string searchwordUrl, bool openWin, bool isLoadValues, string type, bool isRelated, StringBuilder attributeBuilder, string formID, int channelID)
        {
            StringBuilder builder = new StringBuilder();
            if (string.IsNullOrEmpty(formID))
            {
                string ajaxDivID = StlParserUtility.GetAjaxDivID(pageInfo.UniqueID);
                formID = ajaxDivID;
            }
            string clickString = string.Format("document.getElementById('{0}').submit();return false;", formID);

            if (isLoadValues)
            {
                openWin = false;
            }
            string innerXml = StringUtils.StripTags(parsedContent, "stl:inputTemplate");

            StringBuilder innerBuilder = new StringBuilder(innerXml);
            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
            builder.Append(innerBuilder.ToString());

            StlHtmlUtility.ReWriteSubmitButton(builder, clickString);
            //�ж��Ƿ��Ѿ�����form
            if (builder.ToString().IndexOf("<form") < 0 && builder.ToString().IndexOf("< form") < 0)
                builder.Insert(0, string.Format(@"<form id=""{0}"" action=""{1}"" target=""{2}""{3}>", formID, searchwordUrl, openWin ? "_blank" : "_self", attributeBuilder.ToString()));


            if (!string.IsNullOrEmpty(type))
            {
                if (builder.ToString().IndexOf("</form>") < 0 && builder.ToString().IndexOf("</form >") < 0)
                    builder.AppendFormat(@"<input type=""hidden"" id=""type"" name=""type"" value=""{0}"" />", type);
                else
                    builder.Replace("</form>", string.Format(@"<input type=""hidden"" id=""type"" name=""type"" value=""{0}"" /></from", type))
                        .Replace("</form >", string.Format(@"<input type=""hidden"" id=""type"" name=""type"" value=""{0}"" /></from", type));
            }

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
                                    ddl.Items[0].Text = "��������Ŀ";
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
                    <option value=""Title"">����</option>
                    <option value=""Content"">����</option>
                    ");
                            }
                        }
                        else if (StringUtils.EqualsIgnoreCase(attributes["id"], "date"))
                        {
                            if (string.IsNullOrEmpty(elementNode.InnerXml))
                            {
                                elementNode.InnerXml = string.Format(@"
                    <option value=""0"">ȫ��ʱ��</option>
                    <option value=""1"">1����</option>
                    <option value=""7"">1����</option>
                    <option value=""30"">1������</option>
                    <option value=""365"">1����</option>
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
                                attributes["onfocus"] = SiteFiles.DatePicker.OnFocus;//"CalendarWebControl.show(this,false,this.value);";
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
                        if (elementNode.Attributes["type"] != null && elementNode.Attributes["type"].Value == "text")
                        {
                            attributes["autocomplete"] = "off";
                        }
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
            //�ж��Ƿ��Ѿ�����form
            if (builder.ToString().IndexOf("</form>") < 0 && builder.ToString().IndexOf("</form >") < 0)
                builder.Append("</form>");

            return builder.ToString();
        }

        public static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, string searchwordInputTemplateString, string styleName, bool isSearchwordUrlExists, string searchwordUrl, bool isOpenWinExists, bool openWin, bool isLoadValues, string type, bool isRelated, StringBuilder formAttributes)
        {
            string parsedContent = string.Empty;

            SearchwordSettingInfo settingInfo = DataProvider.SearchwordSettingDAO.GetSearchwordSettingInfo(pageInfo.PublishmentSystemID);
            if (settingInfo == null)
            {
                settingInfo = new SearchwordSettingInfo();
            }
            TagStyleSearchInputInfo inputInfo = new TagStyleSearchInputInfo(string.Empty);

            if (isSearchwordUrlExists)
            {
                inputInfo.SearchUrl = searchwordUrl;
            }
            if (isOpenWinExists)
            {
                inputInfo.OpenWin = openWin;
            }

            SearchwordInputTemplate inputTemplate = new SearchwordInputTemplate(pageInfo.PublishmentSystemInfo, settingInfo, inputInfo, type, formAttributes.ToString());
            parsedContent = inputTemplate.GetTemplate(settingInfo.IsTemplate, isRelated, searchwordInputTemplateString);

            parsedContent = ParseTemplateToHtml(pageInfo, contextInfo, parsedContent, searchwordUrl, openWin, isLoadValues, type, isRelated, formAttributes, inputTemplate.FormID, contextInfo.ChannelID);

            StringBuilder innerBuilder = new StringBuilder(parsedContent);
            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
            parsedContent = innerBuilder.ToString();

            if (isLoadValues)
            {
                pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.A_JQuery);

                pageInfo.AddPageScriptsIfNotExists(StlSearchwordInput.ElementName, string.Format(@"
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
", inputTemplate.FormID, isUtf8.ToString().ToLower());
            }

            return parsedContent;
        }
    }
}
