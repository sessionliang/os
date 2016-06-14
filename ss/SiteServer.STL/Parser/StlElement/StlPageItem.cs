using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BaiRong.Core;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using System.Collections.Generic;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlPageItem
    {
        private StlPageItem() { }
        public const string ElementName = "stl:pageitem";//��ҳ��

        public const string Attribute_Type = "type";					                //��ʾ����
        public const string Attribute_Text = "text";					                //��ʾ������
        public const string Attribute_LinkClass = "linkclass";
        public const string Attribute_TextClass = "textclass";
        public const string Attribute_ListNum = "listnum";                              //ҳ������ҳ��ת��ʾ������
        public const string Attribute_ListEllipsis = "listellipsis";                    //ҳ������ҳ��ת����̫��ʱ��ʾ��ʡ�Ժ�

        public const string Attribute_HasLR = "haslr";                                  //ҳ�뵼���Ƿ���������ַ�
        //public const string Attribute_LRStr = "lrstr";                                  //ҳ�������ַ�
        public const string Attribute_LStr = "lstr";                                    //ҳ�����ַ�
        public const string Attribute_RStr = "rstr";                                    //ҳ�����ַ�
        public const string Attribute_AlwaysA = "alwaysa";                              //ҳ�����ǳ����ӣ�����������ʱ

        public const string Type_PreviousPage = "PreviousPage";				            //��һҳ
        public const string Type_NextPage = "NextPage";						            //��һҳ
        public const string Type_FirstPage = "FirstPage";						        //��ҳ
        public const string Type_LastPage = "LastPage";						            //ĩҳ
        public const string Type_CurrentPageIndex = "CurrentPageIndex";		            //��ǰҳ����
        public const string Type_TotalPageNum = "TotalPageNum";		                    //��ҳ��
        public const string Type_TotalNum = "TotalNum";		                            //��������
        public const string Type_PageNavigation = "PageNavigation";			            //ҳ����
        public const string Type_PageSelect = "PageSelect";			                    //ҳ��ת

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_Type, "��ʾ����");
                attributes.Add(Attribute_Text, "��ʾ������");
                attributes.Add(Attribute_LinkClass, "����CSS��ʽ");
                attributes.Add(Attribute_TextClass, "����CSS��ʽ");
                attributes.Add(Attribute_ListNum, "ҳ������ҳ��ת��ʾ������");
                attributes.Add(Attribute_ListEllipsis, "ҳ������ҳ��ת����̫��ʱ��ʾ��ʡ�Ժ�");
                return attributes;
            }
        }


        //�ԡ���ҳ���pageItem��Ԫ�ؽ��н�������Ԫ��������ҳ��ʱ������������������ParseStlElement�����С�
        public static string ParseElement(string stlElement, PageInfo pageInfo, int nodeID, int contentID, int currentPageIndex, int pageCount, int totalNum, bool isXmlContent, EContextType contextType)
        {
            string parsedContent = string.Empty;
            try
            {
                XmlDocument xmlDocument = StlParserUtility.GetXmlDocument(stlElement, isXmlContent);
                XmlNode node = xmlDocument.DocumentElement;
                node = node.FirstChild;
                string label = node.Name;
                if (!label.ToLower().Equals(StlPageItem.ElementName)) return string.Empty;

                IEnumerator ie = node.Attributes.GetEnumerator();
                string text = string.Empty;
                string type = string.Empty;
                string linkClass = string.Empty;
                string textClass = string.Empty;
                int listNum = 9;
                string listEllipsis = "...";
                bool hasLR = true;
                //string lrStr = string.Empty;
                string lStr = string.Empty;
                string rStr = string.Empty;
                bool alwaysA = true;
                StringDictionary attributes = new StringDictionary();
                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlPageItem.Attribute_Type))
                    {
                        type = attr.Value;
                    }
                    else if (attributeName.Equals(StlPageItem.Attribute_Text))
                    {
                        text = attr.Value;
                    }
                    else if (attributeName.Equals(StlPageItem.Attribute_ListNum))
                    {
                        listNum = TranslateUtils.ToInt(attr.Value, 9);
                    }
                    else if (attributeName.Equals(StlPageItem.Attribute_ListEllipsis))
                    {
                        listEllipsis = attr.Value;
                    }
                    else if (attributeName.Equals(StlPageItem.Attribute_LinkClass))
                    {
                        linkClass = attr.Value;
                    }
                    else if (attributeName.Equals(StlPageItem.Attribute_TextClass))
                    {
                        textClass = attr.Value;
                    }
                    else if (attributeName.Equals(StlPageItem.Attribute_HasLR))
                    {
                        hasLR = TranslateUtils.ToBool(attr.Value);
                    }
                    //else if (attributeName.Equals(StlPageItem.Attribute_LRStr))
                    //{
                    //    lrStr = attr.Value;
                    //}
                    else if (attributeName.Equals(StlPageItem.Attribute_LStr))
                    {
                        lStr = attr.Value;
                    }
                    else if (attributeName.Equals(StlPageItem.Attribute_RStr))
                    {
                        rStr = attr.Value;
                    }
                    else if (attributeName.Equals(StlPageItem.Attribute_AlwaysA))
                    {
                        alwaysA = TranslateUtils.ToBool(attr.Value);
                    }
                    else
                    {
                        attributes[attributeName] = attr.Value;
                    }
                }

                string successTemplateString = string.Empty;
                string failureTemplateString = string.Empty;

                StlParserUtility.GetInnerTemplateString(node, pageInfo, out successTemplateString, out failureTemplateString);
                if (!string.IsNullOrEmpty(node.InnerXml) && string.IsNullOrEmpty(failureTemplateString))
                {
                    failureTemplateString = successTemplateString;
                }

                //�������������isChannelPage=trueʱ��Ҫ
                NodeInfo nodeInfo = null;

                string pageUrl = string.Empty;
                if (contextType == EContextType.Channel)
                {
                    nodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, nodeID);
                    pageUrl = StlParserUtility.GetUrlInChannelPage(type, pageInfo.PublishmentSystemInfo, nodeInfo, 0, currentPageIndex, pageCount, pageInfo.VisualType);
                }
                else
                {
                    pageUrl = StlParserUtility.GetUrlInContentPage(type, pageInfo.PublishmentSystemInfo, nodeID, contentID, 0, currentPageIndex, pageCount, pageInfo.VisualType);
                }

                bool isActive = false;
                bool isAddSpan = false;

                if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_FirstPage) || StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_LastPage) || StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_PreviousPage) || StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_NextPage))
                {
                    if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_FirstPage))
                    {
                        if (string.IsNullOrEmpty(text))
                        {
                            text = "��ҳ";
                        }
                        if (currentPageIndex != 0)//��ǰҳ��Ϊ��ҳ
                        {
                            isActive = true;
                        }
                        else
                        {
                            pageUrl = PageUtils.UNCLICKED_URL;
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_LastPage))
                    {
                        if (string.IsNullOrEmpty(text))
                        {
                            text = "ĩҳ";
                        }
                        if (currentPageIndex + 1 != pageCount)//��ǰҳ��Ϊĩҳ
                        {
                            isActive = true;
                        }
                        else
                        {
                            pageUrl = PageUtils.UNCLICKED_URL;
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_PreviousPage))
                    {
                        if (string.IsNullOrEmpty(text))
                        {
                            text = "��һҳ";
                        }
                        if (currentPageIndex != 0)//��ǰҳ��Ϊ��ҳ
                        {
                            isActive = true;
                        }
                        else
                        {
                            pageUrl = PageUtils.UNCLICKED_URL;
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_NextPage))
                    {
                        if (text.Equals(string.Empty))
                        {
                            text = "��һҳ";
                        }
                        if (currentPageIndex + 1 != pageCount)//��ǰҳ��Ϊĩҳ
                        {
                            isActive = true;
                        }
                        else
                        {
                            pageUrl = PageUtils.UNCLICKED_URL;
                        }
                    }

                    if (isActive)
                    {
                        if (!string.IsNullOrEmpty(successTemplateString))
                        {
                            parsedContent = StlPageItem.GetParsedContent(successTemplateString, pageUrl, Convert.ToString(currentPageIndex + 1), pageInfo);
                        }
                        else
                        {
                            HyperLink pageHyperLink = new HyperLink();
                            ControlUtils.AddAttributesIfNotExists(pageHyperLink, attributes);
                            pageHyperLink.Text = text;
                            if (!string.IsNullOrEmpty(linkClass))
                            {
                                pageHyperLink.CssClass = linkClass;
                            }
                            pageHyperLink.NavigateUrl = pageUrl;
                            parsedContent = ControlUtils.GetControlRenderHtml(pageHyperLink);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(failureTemplateString))
                        {
                            parsedContent = StlPageItem.GetParsedContent(failureTemplateString, pageUrl, Convert.ToString(currentPageIndex + 1), pageInfo);
                        }
                        else
                        {
                            isAddSpan = true;
                            parsedContent = text;
                        }
                    }
                }

                else if (type.ToLower().Equals(StlPageItem.Type_CurrentPageIndex.ToLower()))//��ǰҳ����
                {
                    string currentPageHtml = text + Convert.ToString(currentPageIndex + 1);
                    isAddSpan = true;
                    parsedContent = currentPageHtml;
                }
                else if (type.ToLower().Equals(StlPageItem.Type_TotalPageNum.ToLower()))//��ҳ��
                {
                    string currentPageHtml = text + Convert.ToString(pageCount);
                    isAddSpan = true;
                    parsedContent = currentPageHtml;
                }
                else if (type.ToLower().Equals(StlPageItem.Type_TotalNum.ToLower()))//��������
                {
                    isAddSpan = true;
                    parsedContent = text + Convert.ToString(totalNum);
                }
                else if (type.ToLower().Equals(StlPageItem.Type_PageNavigation.ToLower()))//ҳ����
                {
                    string leftText = "[";
                    string rightText = "]";
                    if (hasLR)
                    {
                        if (!string.IsNullOrEmpty(lStr) && !string.IsNullOrEmpty(rStr))
                        {
                            leftText = lStr;
                            rightText = rStr;
                        }
                        else if (!string.IsNullOrEmpty(lStr))
                        {
                            leftText = rightText = lStr;
                        }
                        else if (!string.IsNullOrEmpty(rStr))
                        {
                            leftText = rightText = rStr;
                        }
                    }
                    else if (!hasLR)
                    {
                        leftText = rightText = string.Empty;
                    }

                    StringBuilder pageBuilder = new StringBuilder();

                    int pageLength = listNum;
                    int pageHalf = Convert.ToInt32(listNum / 2);

                    int index = currentPageIndex + 1;
                    int totalPage = currentPageIndex + pageLength;
                    if (totalPage > pageCount)
                    {
                        if (index + pageHalf < pageCount)
                        {
                            index = (currentPageIndex + 1) - pageHalf;
                            if (index <= 0)
                            {
                                index = 1;
                                totalPage = pageCount;
                            }
                            else
                            {
                                totalPage = (currentPageIndex + 1) + pageHalf;
                            }
                        }
                        else
                        {
                            index = (pageCount - pageLength) > 0 ? (pageCount - pageLength + 1) : 1;
                            totalPage = pageCount;
                        }
                    }
                    else
                    {
                        index = (currentPageIndex + 1) - pageHalf;
                        if (index <= 0)
                        {
                            index = 1;
                            totalPage = pageLength;
                        }
                        else
                        {
                            totalPage = index + pageLength - 1;
                        }
                    }

                    //pre ellipsis
                    if (index + pageLength < currentPageIndex + 1 && !string.IsNullOrEmpty(listEllipsis))
                    {
                        if (contextType == EContextType.Channel)
                        {
                            pageUrl = StlParserUtility.GetUrlInChannelPage(type, pageInfo.PublishmentSystemInfo, nodeInfo, index, currentPageIndex, pageCount, pageInfo.VisualType);
                        }
                        else
                        {
                            pageUrl = StlParserUtility.GetUrlInContentPage(type, pageInfo.PublishmentSystemInfo, nodeID, contentID, index, currentPageIndex, pageCount, pageInfo.VisualType);
                        }

                        if (!string.IsNullOrEmpty(successTemplateString))
                        {
                            pageBuilder.Append(StlPageItem.GetParsedContent(successTemplateString, pageUrl, listEllipsis, pageInfo));
                        }
                        else
                        {
                            pageBuilder.AppendFormat(@"<a href=""{0}"" {1}>{2}</a>", pageUrl, TranslateUtils.ToAttributesString(attributes), listEllipsis);
                        }
                    }

                    for (; index <= totalPage; index++)
                    {
                        if (currentPageIndex + 1 != index)
                        {
                            if (contextType == EContextType.Channel)
                            {
                                pageUrl = StlParserUtility.GetUrlInChannelPage(type, pageInfo.PublishmentSystemInfo, nodeInfo, index, currentPageIndex, pageCount, pageInfo.VisualType);
                            }
                            else
                            {
                                pageUrl = StlParserUtility.GetUrlInContentPage(type, pageInfo.PublishmentSystemInfo, nodeID, contentID, index, currentPageIndex, pageCount, pageInfo.VisualType);
                            }

                            if (!string.IsNullOrEmpty(successTemplateString))
                            {
                                pageBuilder.Append(StlPageItem.GetParsedContent(successTemplateString, pageUrl, index.ToString(), pageInfo));
                            }
                            else
                            {
                                HyperLink pageHyperLink = new HyperLink();
                                pageHyperLink.NavigateUrl = pageUrl;
                                pageHyperLink.Text = string.Format("{0}{1}{2}", leftText, index, rightText);
                                if (!string.IsNullOrEmpty(linkClass))
                                {
                                    pageHyperLink.CssClass = linkClass;
                                }
                                pageBuilder.Append(ControlUtils.GetControlRenderHtml(pageHyperLink) + "&nbsp;");
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(failureTemplateString))
                            {
                                pageBuilder.Append(StlPageItem.GetParsedContent(failureTemplateString, pageUrl, index.ToString(), pageInfo));
                            }
                            else
                            {
                                isAddSpan = true;
                                if (!alwaysA)
                                    pageBuilder.AppendFormat("{0}{1}{2}&nbsp;", leftText, index, rightText);
                                else
                                    pageBuilder.AppendFormat("<a href='javascript:void(0);'>{0}{1}{2}</a>&nbsp;", leftText, index, rightText);
                            }
                        }
                    }

                    //pre ellipsis
                    if (index < pageCount && !string.IsNullOrEmpty(listEllipsis))
                    {
                        if (contextType == EContextType.Channel)
                        {
                            pageUrl = StlParserUtility.GetUrlInChannelPage(type, pageInfo.PublishmentSystemInfo, nodeInfo, index, currentPageIndex, pageCount, pageInfo.VisualType);
                        }
                        else
                        {
                            pageUrl = StlParserUtility.GetUrlInContentPage(type, pageInfo.PublishmentSystemInfo, nodeID, contentID, index, currentPageIndex, pageCount, pageInfo.VisualType);
                        }

                        if (!string.IsNullOrEmpty(successTemplateString))
                        {
                            pageBuilder.Append(StlPageItem.GetParsedContent(successTemplateString, pageUrl, listEllipsis, pageInfo));
                        }
                        else
                        {
                            pageBuilder.AppendFormat(@"<a href=""{0}"" {1}>{2}</a>", pageUrl, TranslateUtils.ToAttributesString(attributes), listEllipsis);
                        }
                    }

                    parsedContent = text + pageBuilder;
                }
                else if (type.ToLower().Equals(StlPageItem.Type_PageSelect.ToLower()))//ҳ��ת
                {
                    System.Web.UI.HtmlControls.HtmlSelect selectControl = new HtmlSelect();
                    if (!string.IsNullOrEmpty(textClass))
                    {
                        selectControl.Attributes.Add("class", textClass);
                    }
                    foreach (string key in attributes.Keys)
                    {
                        selectControl.Attributes[key] = attributes[key];
                    }

                    string uniqueID = "PageSelect_" + pageInfo.UniqueID;
                    selectControl.ID = uniqueID;

                    string scriptHtml = string.Format("<script language=\"JavaScript\">function {0}_jumpMenu(targ,selObj,restore){1}eval(targ+\".location='\"+selObj.options[selObj.selectedIndex].value+\"'\");if (restore) selObj.selectedIndex=0;{2}</script>", uniqueID, "{", "}");
                    selectControl.Attributes.Add("onChange", string.Format("{0}_jumpMenu('self',this,0)", uniqueID));

                    for (int index = 1; index <= pageCount; index++)
                    {
                        if (currentPageIndex + 1 != index)
                        {
                            if (contextType == EContextType.Channel)
                            {
                                pageUrl = StlParserUtility.GetUrlInChannelPage(type, pageInfo.PublishmentSystemInfo, nodeInfo, index, currentPageIndex, pageCount, pageInfo.VisualType);
                            }
                            else
                            {
                                pageUrl = StlParserUtility.GetUrlInContentPage(type, pageInfo.PublishmentSystemInfo, nodeID, contentID, index, currentPageIndex, pageCount, pageInfo.VisualType);
                            }

                            ListItem listitem = new ListItem(index.ToString(), pageUrl);
                            selectControl.Items.Add(listitem);
                        }
                        else
                        {
                            ListItem listitem = new ListItem(index.ToString(), string.Empty);
                            listitem.Selected = true;
                            selectControl.Items.Add(listitem);
                        }
                    }

                    parsedContent = scriptHtml + ControlUtils.GetControlRenderHtml(selectControl);
                }

                if (isAddSpan && !string.IsNullOrEmpty(textClass))
                {
                    parsedContent = string.Format(@"<span class=""{0}"">{1}</span>", textClass, parsedContent);
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return StlParserUtility.GetBackHtml(parsedContent, pageInfo);

            //return parsedContent;
        }

        public static string ParseEntity(string stlEntity, PageInfo pageInfo, int nodeID, int contentID, int currentPageIndex, int pageCount, int totalNum, bool isXmlContent, EContextType contextType)
        {
            string parsedContent = string.Empty;
            try
            {
                string type = stlEntity.Substring(stlEntity.IndexOf(".") + 1);
                if (!string.IsNullOrEmpty(type))
                {
                    type = type.TrimEnd('}').Trim();
                }
                bool isHyperlink = false;

                //�������������isChannelPage=trueʱ��Ҫ
                NodeInfo nodeInfo = null;

                string pageUrl = string.Empty;

                if (contextType == EContextType.Channel)
                {
                    nodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, nodeID);
                    pageUrl = StlParserUtility.GetUrlInChannelPage(type, pageInfo.PublishmentSystemInfo, nodeInfo, 0, currentPageIndex, pageCount, pageInfo.VisualType);
                }
                else
                {
                    pageUrl = StlParserUtility.GetUrlInContentPage(type, pageInfo.PublishmentSystemInfo, nodeID, contentID, 0, currentPageIndex, pageCount, pageInfo.VisualType);
                }

                if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_FirstPage) || StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_LastPage) || StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_PreviousPage) || StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_NextPage))
                {
                    if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_FirstPage))
                    {
                        if (currentPageIndex != 0)//��ǰҳ��Ϊ��ҳ
                        {
                            isHyperlink = true;
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_LastPage))
                    {
                        if (currentPageIndex + 1 != pageCount)//��ǰҳ��Ϊĩҳ
                        {
                            isHyperlink = true;
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_PreviousPage))
                    {
                        if (currentPageIndex != 0)//��ǰҳ��Ϊ��ҳ
                        {
                            isHyperlink = true;
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_NextPage))
                    {
                        if (currentPageIndex + 1 != pageCount)//��ǰҳ��Ϊĩҳ
                        {
                            isHyperlink = true;
                        }
                    }

                    if (isHyperlink)//��ǰҳ��Ϊ��ҳ
                    {
                        parsedContent = pageUrl;
                    }
                    else
                    {
                        parsedContent = PageUtils.UNCLICKED_URL;
                    }

                }
                else if (type.ToLower().Equals(StlPageItem.Type_CurrentPageIndex.ToLower()))//��ǰҳ����
                {
                    parsedContent = Convert.ToString(currentPageIndex + 1);
                }
                else if (type.ToLower().Equals(StlPageItem.Type_TotalPageNum.ToLower()))//��ҳ��
                {
                    parsedContent = Convert.ToString(pageCount);
                }
                else if (type.ToLower().Equals(StlPageItem.Type_TotalNum.ToLower()))//��������
                {
                    parsedContent = Convert.ToString(totalNum);
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        public static string ParseElementInSearchPage(string stlElement, PageInfo pageInfo, string ajaxDivID, int nodeID, int currentPageIndex, int pageCount, int totalNum)
        {
            string parsedContent = string.Empty;
            try
            {
                XmlDocument xmlDocument = StlParserUtility.GetXmlDocument(stlElement, true);
                XmlNode node = xmlDocument.DocumentElement;
                node = node.FirstChild;
                string label = node.Name;
                if (!label.ToLower().Equals(StlPageItem.ElementName)) return string.Empty;

                IEnumerator ie = node.Attributes.GetEnumerator();
                string text = string.Empty;
                string type = string.Empty;
                string linkClass = string.Empty;
                string textClass = string.Empty;
                int listNum = 9;
                string listEllipsis = "...";
                bool hasLR = true;
                //string lrStr = string.Empty;
                string lStr = string.Empty;
                string rStr = string.Empty;
                bool alwaysA = true;
                StringDictionary attributes = new StringDictionary();
                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlPageItem.Attribute_Type))
                    {
                        type = attr.Value;
                    }
                    else if (attributeName.Equals(StlPageItem.Attribute_Text))
                    {
                        text = attr.Value;
                    }
                    else if (attributeName.Equals(StlPageItem.Attribute_ListNum))
                    {
                        listNum = TranslateUtils.ToInt(attr.Value, 9);
                    }
                    else if (attributeName.Equals(StlPageItem.Attribute_ListEllipsis))
                    {
                        listEllipsis = attr.Value;
                    }
                    else if (attributeName.Equals(StlPageItem.Attribute_LinkClass))
                    {
                        linkClass = attr.Value;
                    }
                    else if (attributeName.Equals(StlPageItem.Attribute_TextClass))
                    {
                        textClass = attr.Value;
                    }
                    else if (attributeName.Equals(StlPageItem.Attribute_HasLR))
                    {
                        hasLR = TranslateUtils.ToBool(attr.Value);
                    }
                    //else if (attributeName.Equals(StlPageItem.Attribute_LRStr))
                    //{
                    //    lrStr = attr.Value;
                    //}
                    else if (attributeName.Equals(StlPageItem.Attribute_LStr))
                    {
                        lStr = attr.Value;
                    }
                    else if (attributeName.Equals(StlPageItem.Attribute_RStr))
                    {
                        rStr = attr.Value;
                    }
                    else if (attributeName.Equals(StlPageItem.Attribute_AlwaysA))
                    {
                        alwaysA = TranslateUtils.ToBool(attr.Value);
                    }
                    else
                    {
                        attributes[attributeName] = attr.Value;
                    }
                }

                string successTemplateString = string.Empty;
                string failureTemplateString = string.Empty;

                if (!string.IsNullOrEmpty(node.InnerXml))
                {
                    List<string> stlElementList = StlParserUtility.GetStlElementList(node.InnerXml);
                    if (stlElementList.Count > 0)
                    {
                        foreach (string theStlElement in stlElementList)
                        {
                            if (StlParserUtility.IsSpecifiedStlElement(theStlElement, StlYes.ElementName) || StlParserUtility.IsSpecifiedStlElement(theStlElement, StlYes.ElementName2))
                            {
                                successTemplateString = StlParserUtility.GetInnerXml(theStlElement, true);
                            }
                            else if (StlParserUtility.IsSpecifiedStlElement(theStlElement, StlNo.ElementName) || StlParserUtility.IsSpecifiedStlElement(theStlElement, StlNo.ElementName2))
                            {
                                failureTemplateString = StlParserUtility.GetInnerXml(theStlElement, true);
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(successTemplateString) && string.IsNullOrEmpty(failureTemplateString))
                    {
                        successTemplateString = failureTemplateString = node.InnerXml;
                    }
                }

                string clickString = StlParserUtility.GetClickStringInSearchPage(type, ajaxDivID, 0, currentPageIndex, pageCount);

                bool isActive = false;
                bool isAddSpan = false;

                if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_FirstPage) || StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_LastPage) || StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_PreviousPage) || StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_NextPage))
                {
                    if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_FirstPage))
                    {
                        if (string.IsNullOrEmpty(text))
                        {
                            text = "��ҳ";
                        }
                        if (currentPageIndex != 0)//��ǰҳ��Ϊ��ҳ
                        {
                            isActive = true;
                        }
                        else
                        {
                            clickString = string.Empty;
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_LastPage))
                    {
                        if (string.IsNullOrEmpty(text))
                        {
                            text = "ĩҳ";
                        }
                        if (currentPageIndex + 1 != pageCount)//��ǰҳ��Ϊĩҳ
                        {
                            isActive = true;
                        }
                        else
                        {
                            clickString = string.Empty;
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_PreviousPage))
                    {
                        if (string.IsNullOrEmpty(text))
                        {
                            text = "��һҳ";
                        }
                        if (currentPageIndex != 0)//��ǰҳ��Ϊ��ҳ
                        {
                            isActive = true;
                        }
                        else
                        {
                            clickString = string.Empty;
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_NextPage))
                    {
                        if (text.Equals(string.Empty))
                        {
                            text = "��һҳ";
                        }
                        if (currentPageIndex + 1 != pageCount)//��ǰҳ��Ϊĩҳ
                        {
                            isActive = true;
                        }
                        else
                        {
                            clickString = string.Empty;
                        }
                    }

                    if (isActive)//��ǰҳ��Ϊ��ҳ
                    {
                        if (!string.IsNullOrEmpty(successTemplateString))
                        {
                            string pageUrl = string.Format("javascript:{0}", clickString);
                            parsedContent = StlPageItem.GetParsedContent(successTemplateString, pageUrl, Convert.ToString(currentPageIndex + 1), pageInfo);
                        }
                        else
                        {
                            HyperLink pageHyperLink = new HyperLink();
                            ControlUtils.AddAttributesIfNotExists(pageHyperLink, attributes);
                            pageHyperLink.NavigateUrl = PageUtils.UNCLICKED_URL;
                            pageHyperLink.Attributes.Add("onclick", clickString);
                            pageHyperLink.Text = text;
                            if (!string.IsNullOrEmpty(linkClass))
                            {
                                pageHyperLink.CssClass = linkClass;
                            }
                            parsedContent = ControlUtils.GetControlRenderHtml(pageHyperLink);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(failureTemplateString))
                        {
                            parsedContent = StlPageItem.GetParsedContent(failureTemplateString, PageUtils.UNCLICKED_URL, Convert.ToString(currentPageIndex + 1), pageInfo);
                        }
                        else
                        {
                            isAddSpan = true;
                            parsedContent = text;
                        }
                    }
                }

                else if (type.ToLower().Equals(StlPageItem.Type_CurrentPageIndex.ToLower()))//��ǰҳ����
                {
                    string currentPageHtml = text + Convert.ToString(currentPageIndex + 1);
                    isAddSpan = true;
                    parsedContent = currentPageHtml;
                }
                else if (type.ToLower().Equals(StlPageItem.Type_TotalPageNum.ToLower()))//��ҳ��
                {
                    string currentPageHtml = text + Convert.ToString(pageCount);
                    isAddSpan = true;
                    parsedContent = currentPageHtml;
                }
                else if (type.ToLower().Equals(StlPageItem.Type_TotalNum.ToLower()))//��������
                {
                    isAddSpan = true;
                    parsedContent = text + Convert.ToString(totalNum);
                }
                else if (type.ToLower().Equals(StlPageItem.Type_PageNavigation.ToLower()))//ҳ����
                {
                    string leftText = "[";
                    string rightText = "]";
                    if (hasLR)
                    {
                        if (!string.IsNullOrEmpty(lStr) && !string.IsNullOrEmpty(rStr))
                        {
                            leftText = lStr;
                            rightText = rStr;
                        }
                        else if (!string.IsNullOrEmpty(lStr))
                        {
                            leftText = rightText = lStr;
                        }
                        else if (!string.IsNullOrEmpty(rStr))
                        {
                            leftText = rightText = rStr;
                        }
                    }
                    else if (!hasLR)
                    {
                        leftText = rightText = string.Empty;
                    }
                    StringBuilder pageBuilder = new StringBuilder();

                    int pageLength = listNum;
                    int pageHalf = Convert.ToInt32(listNum / 2);

                    int index = currentPageIndex + 1;
                    int totalPage = currentPageIndex + pageLength;
                    if (totalPage > pageCount)
                    {
                        if (index + pageHalf < pageCount)
                        {
                            index = (currentPageIndex + 1) - pageHalf;
                            if (index <= 0)
                            {
                                index = 1;
                                totalPage = pageCount;
                            }
                            else
                            {
                                totalPage = (currentPageIndex + 1) + pageHalf;
                            }
                        }
                        else
                        {
                            index = (pageCount - pageLength) > 0 ? (pageCount - pageLength + 1) : 1;
                            totalPage = pageCount;
                        }
                    }
                    else
                    {
                        index = (currentPageIndex + 1) - pageHalf;
                        if (index <= 0)
                        {
                            index = 1;
                            totalPage = pageLength;
                        }
                        else
                        {
                            totalPage = index + pageLength - 1;
                        }
                    }

                    //pre ellipsis
                    if (index + pageLength < currentPageIndex + 1 && !string.IsNullOrEmpty(listEllipsis))
                    {
                        clickString = StlParserUtility.GetClickStringInSearchPage(type, ajaxDivID, index, currentPageIndex, pageCount);

                        if (!string.IsNullOrEmpty(successTemplateString))
                        {
                            string pageUrl = string.Format("javascript:{0}", clickString);
                            pageBuilder.Append(StlPageItem.GetParsedContent(successTemplateString, pageUrl, listEllipsis, pageInfo));
                        }
                        else
                        {
                            pageBuilder.AppendFormat(@"<a href=""{0}"" onclick=""{1}"" {2}>{3}</a>", PageUtils.UNCLICKED_URL, clickString, TranslateUtils.ToAttributesString(attributes), listEllipsis);
                        }
                    }

                    for (; index <= totalPage; index++)
                    {
                        if (currentPageIndex + 1 != index)
                        {
                            clickString = StlParserUtility.GetClickStringInSearchPage(type, ajaxDivID, index, currentPageIndex, pageCount);
                            if (!string.IsNullOrEmpty(successTemplateString))
                            {
                                string pageUrl = string.Format("javascript:{0}", clickString);
                                pageBuilder.Append(StlPageItem.GetParsedContent(successTemplateString, pageUrl, index.ToString(), pageInfo));
                            }
                            else
                            {
                                HyperLink pageHyperLink = new HyperLink();
                                pageHyperLink.NavigateUrl = PageUtils.UNCLICKED_URL;
                                pageHyperLink.Attributes.Add("onclick", clickString);
                                pageHyperLink.Text = string.Format("{0}{1}{2}", leftText, index, rightText);
                                if (!string.IsNullOrEmpty(linkClass))
                                {
                                    pageHyperLink.CssClass = linkClass;
                                }
                                pageBuilder.Append(ControlUtils.GetControlRenderHtml(pageHyperLink) + "&nbsp;");
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(failureTemplateString))
                            {
                                pageBuilder.Append(StlPageItem.GetParsedContent(failureTemplateString, PageUtils.UNCLICKED_URL, index.ToString(), pageInfo));
                            }
                            else
                            {
                                isAddSpan = true;
                                if (!alwaysA)
                                    pageBuilder.AppendFormat("{0}{1}{2}&nbsp;", leftText, index, rightText);
                                else
                                    pageBuilder.AppendFormat("<a href='javascript:void(0);'>{0}{1}{2}</a>&nbsp;", leftText, index, rightText);
                            }
                        }
                    }

                    //pre ellipsis
                    if (index < pageCount && !string.IsNullOrEmpty(listEllipsis))
                    {
                        clickString = StlParserUtility.GetClickStringInSearchPage(type, ajaxDivID, index, currentPageIndex, pageCount);

                        if (!string.IsNullOrEmpty(successTemplateString))
                        {
                            string pageUrl = string.Format("javascript:{0}", clickString);
                            pageBuilder.Append(StlPageItem.GetParsedContent(successTemplateString, pageUrl, listEllipsis, pageInfo));
                        }
                        else
                        {
                            pageBuilder.AppendFormat(@"<a href=""{0}"" onclick=""{1}"" {2}>{3}</a>", PageUtils.UNCLICKED_URL, clickString, TranslateUtils.ToAttributesString(attributes), listEllipsis);
                        }
                    }

                    parsedContent = text + pageBuilder;
                }
                else if (type.ToLower().Equals(StlPageItem.Type_PageSelect.ToLower()))//ҳ��ת
                {
                    HtmlSelect selectControl = new HtmlSelect();
                    if (!string.IsNullOrEmpty(textClass))
                    {
                        selectControl.Attributes.Add("class", textClass);
                    }
                    foreach (string key in attributes.Keys)
                    {
                        selectControl.Attributes[key] = attributes[key];
                    }

                    //selectControl.Attributes.Add("onChange", string.Format("stlJump{0}(this)", ajaxDivID));
                    selectControl.Attributes.Add("onChange", clickString);

                    for (int index = 1; index <= pageCount; index++)
                    {
                        if (currentPageIndex + 1 != index)
                        {
                            ListItem listitem = new ListItem(index.ToString(), string.Format("{0}", (index - 1)));
                            selectControl.Items.Add(listitem);
                        }
                        else
                        {
                            ListItem listitem = new ListItem(index.ToString(), string.Empty);
                            listitem.Selected = true;
                            selectControl.Items.Add(listitem);
                        }
                    }

                    parsedContent = ControlUtils.GetControlRenderHtml(selectControl);
                }

                if (isAddSpan && !string.IsNullOrEmpty(textClass))
                {
                    parsedContent = string.Format(@"<span class=""{0}"">{1}</span>", textClass, parsedContent);
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        public static string ParseEntityInSearchPage(string stlEntity, PageInfo pageInfo, string ajaxDivID, int nodeID, int currentPageIndex, int pageCount, int totalNum)
        {
            string parsedContent = string.Empty;
            try
            {
                string type = stlEntity.Substring(stlEntity.IndexOf(".") + 1);
                if (!string.IsNullOrEmpty(type))
                {
                    type = type.TrimEnd('}').Trim();
                }
                bool isHyperlink = false;

                string clickString = StlParserUtility.GetClickStringInSearchPage(type, ajaxDivID, 0, currentPageIndex, pageCount);

                if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_FirstPage) || StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_LastPage) || StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_PreviousPage) || StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_NextPage))
                {
                    if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_FirstPage))
                    {
                        if (currentPageIndex != 0)//��ǰҳ��Ϊ��ҳ
                        {
                            isHyperlink = true;
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_LastPage))
                    {
                        if (currentPageIndex + 1 != pageCount)//��ǰҳ��Ϊĩҳ
                        {
                            isHyperlink = true;
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_PreviousPage))
                    {
                        if (currentPageIndex != 0)//��ǰҳ��Ϊ��ҳ
                        {
                            isHyperlink = true;
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_NextPage))
                    {
                        if (currentPageIndex + 1 != pageCount)//��ǰҳ��Ϊĩҳ
                        {
                            isHyperlink = true;
                        }
                    }

                    if (isHyperlink)//��ǰҳ��Ϊ��ҳ
                    {
                        parsedContent = string.Format("javascript:{0}", clickString);
                    }
                    else
                    {
                        parsedContent = PageUtils.UNCLICKED_URL;
                    }
                }
                else if (type.ToLower().Equals(StlPageItem.Type_CurrentPageIndex.ToLower()))//��ǰҳ����
                {
                    parsedContent = Convert.ToString(currentPageIndex + 1);
                }
                else if (type.ToLower().Equals(StlPageItem.Type_TotalPageNum.ToLower()))//��ҳ��
                {
                    parsedContent = Convert.ToString(pageCount);
                }
                else if (type.ToLower().Equals(StlPageItem.Type_TotalNum.ToLower()))//��������
                {
                    parsedContent = Convert.ToString(totalNum);
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }


        public static string ParseElementInDynamicPage(string stlElement, PageInfo pageInfo, string pageUrl, int nodeID, int currentPageIndex, int pageCount, int totalNum, bool isPageRefresh, string ajaxDivID)
        {
            string parsedContent = string.Empty;
            try
            {
                ContextInfo contextInfo = new ContextInfo(pageInfo);

                XmlDocument xmlDocument = StlParserUtility.GetXmlDocument(stlElement, true);
                XmlNode node = xmlDocument.DocumentElement;
                node = node.FirstChild;
                string label = node.Name;
                if (!label.ToLower().Equals(StlPageItem.ElementName)) return string.Empty;

                IEnumerator ie = node.Attributes.GetEnumerator();
                string text = string.Empty;
                string type = string.Empty;
                string linkClass = string.Empty;
                string textClass = string.Empty;
                int listNum = 9;
                string listEllipsis = "...";
                bool hasLR = true;
                //string lrStr = string.Empty;
                string lStr = string.Empty;
                string rStr = string.Empty;
                bool alwaysA = true;
                StringDictionary attributes = new StringDictionary();
                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlPageItem.Attribute_Type))
                    {
                        type = attr.Value;
                    }
                    else if (attributeName.Equals(StlPageItem.Attribute_Text))
                    {
                        text = attr.Value;
                    }
                    else if (attributeName.Equals(StlPageItem.Attribute_ListNum))
                    {
                        listNum = TranslateUtils.ToInt(attr.Value, 9);
                    }
                    else if (attributeName.Equals(StlPageItem.Attribute_ListEllipsis))
                    {
                        listEllipsis = attr.Value;
                    }
                    else if (attributeName.Equals(StlPageItem.Attribute_LinkClass))
                    {
                        linkClass = attr.Value;
                    }
                    else if (attributeName.Equals(StlPageItem.Attribute_TextClass))
                    {
                        textClass = attr.Value;
                    }
                    else if (attributeName.Equals(StlPageItem.Attribute_HasLR))
                    {
                        hasLR = TranslateUtils.ToBool(attr.Value);
                    }
                    //else if (attributeName.Equals(StlPageItem.Attribute_LRStr))
                    //{
                    //    lrStr = attr.Value;
                    //}
                    else if (attributeName.Equals(StlPageItem.Attribute_LStr))
                    {
                        lStr = attr.Value;
                    }
                    else if (attributeName.Equals(StlPageItem.Attribute_RStr))
                    {
                        rStr = attr.Value;
                    }
                    else if (attributeName.Equals(StlPageItem.Attribute_AlwaysA))
                    {
                        alwaysA = TranslateUtils.ToBool(attr.Value);
                    }
                    else
                    {
                        attributes[attributeName] = attr.Value;
                    }
                }

                string successTemplateString = string.Empty;
                string failureTemplateString = string.Empty;

                if (!string.IsNullOrEmpty(node.InnerXml))
                {
                    List<string> stlElementList = StlParserUtility.GetStlElementList(node.InnerXml);
                    if (stlElementList.Count > 0)
                    {
                        foreach (string theStlElement in stlElementList)
                        {
                            if (StlParserUtility.IsSpecifiedStlElement(theStlElement, StlYes.ElementName) || StlParserUtility.IsSpecifiedStlElement(theStlElement, StlYes.ElementName2))
                            {
                                successTemplateString = StlParserUtility.GetInnerXml(theStlElement, true);
                            }
                            else if (StlParserUtility.IsSpecifiedStlElement(theStlElement, StlNo.ElementName) || StlParserUtility.IsSpecifiedStlElement(theStlElement, StlNo.ElementName2))
                            {
                                failureTemplateString = StlParserUtility.GetInnerXml(theStlElement, true);
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(successTemplateString) && string.IsNullOrEmpty(failureTemplateString))
                    {
                        successTemplateString = failureTemplateString = node.InnerXml;
                    }
                }

                string jsMethod = StlParserUtility.GetJsMethodInDynamicPage(type, pageInfo.PublishmentSystemInfo, contextInfo.ChannelID, contextInfo.ContentID, pageUrl, 0, currentPageIndex, pageCount, isPageRefresh, ajaxDivID);

                bool isActive = false;
                bool isAddSpan = false;

                if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_FirstPage) || StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_LastPage) || StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_PreviousPage) || StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_NextPage))
                {
                    if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_FirstPage))
                    {
                        if (string.IsNullOrEmpty(text))
                        {
                            text = "��ҳ";
                        }
                        if (currentPageIndex != 0)//��ǰҳ��Ϊ��ҳ
                        {
                            isActive = true;
                        }
                        else
                        {
                            jsMethod = string.Empty;
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_LastPage))
                    {
                        if (string.IsNullOrEmpty(text))
                        {
                            text = "ĩҳ";
                        }
                        if (currentPageIndex + 1 != pageCount)//��ǰҳ��Ϊĩҳ
                        {
                            isActive = true;
                        }
                        else
                        {
                            jsMethod = string.Empty;
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_PreviousPage))
                    {
                        if (string.IsNullOrEmpty(text))
                        {
                            text = "��һҳ";
                        }
                        if (currentPageIndex != 0)//��ǰҳ��Ϊ��ҳ
                        {
                            isActive = true;
                        }
                        else
                        {
                            jsMethod = string.Empty;
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_NextPage))
                    {
                        if (text.Equals(string.Empty))
                        {
                            text = "��һҳ";
                        }
                        if (currentPageIndex + 1 != pageCount)//��ǰҳ��Ϊĩҳ
                        {
                            isActive = true;
                        }
                        else
                        {
                            jsMethod = string.Empty;
                        }
                    }

                    if (isActive)//��ǰҳ��Ϊ��ҳ
                    {
                        if (!string.IsNullOrEmpty(successTemplateString))
                        {
                            parsedContent = StlPageItem.GetParsedContent(successTemplateString, string.Format("javascript:{0}", jsMethod), Convert.ToString(currentPageIndex + 1), pageInfo);
                        }
                        else
                        {
                            HyperLink pageHyperLink = new HyperLink();
                            ControlUtils.AddAttributesIfNotExists(pageHyperLink, attributes);
                            pageHyperLink.NavigateUrl = PageUtils.UNCLICKED_URL;
                            pageHyperLink.Attributes.Add("onclick", jsMethod + ";return false;");
                            pageHyperLink.Text = text;
                            if (!string.IsNullOrEmpty(linkClass))
                            {
                                pageHyperLink.CssClass = linkClass;
                            }
                            parsedContent = ControlUtils.GetControlRenderHtml(pageHyperLink);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(failureTemplateString))
                        {
                            parsedContent = StlPageItem.GetParsedContent(failureTemplateString, PageUtils.UNCLICKED_URL, Convert.ToString(currentPageIndex + 1), pageInfo);
                        }
                        else
                        {
                            isAddSpan = true;
                            parsedContent = text;
                        }
                    }
                }
                else if (type.ToLower().Equals(StlPageItem.Type_CurrentPageIndex.ToLower()))//��ǰҳ����
                {
                    string currentPageHtml = text + Convert.ToString(currentPageIndex + 1);
                    isAddSpan = true;
                    parsedContent = currentPageHtml;
                }
                else if (type.ToLower().Equals(StlPageItem.Type_TotalPageNum.ToLower()))//��ҳ��
                {
                    string currentPageHtml = text + Convert.ToString(pageCount);
                    isAddSpan = true;
                    parsedContent = currentPageHtml;
                }
                else if (type.ToLower().Equals(StlPageItem.Type_TotalNum.ToLower()))//��������
                {
                    isAddSpan = true;
                    parsedContent = text + Convert.ToString(totalNum);
                }
                else if (type.ToLower().Equals(StlPageItem.Type_PageNavigation.ToLower()))//ҳ����
                {
                    string leftText = "[";
                    string rightText = "]";
                    if (hasLR)
                    {
                        if (!string.IsNullOrEmpty(lStr) && !string.IsNullOrEmpty(rStr))
                        {
                            leftText = lStr;
                            rightText = rStr;
                        }
                        else if (!string.IsNullOrEmpty(lStr))
                        {
                            leftText = rightText = lStr;
                        }
                        else if (!string.IsNullOrEmpty(rStr))
                        {
                            leftText = rightText = rStr;
                        }
                    }
                    else if (!hasLR)
                    {
                        leftText = rightText = string.Empty;
                    }
                    StringBuilder pageBuilder = new StringBuilder();

                    int pageLength = listNum;
                    int pageHalf = Convert.ToInt32(listNum / 2);

                    int index = currentPageIndex + 1;
                    int totalPage = currentPageIndex + pageLength;
                    if (totalPage > pageCount)
                    {
                        if (index + pageHalf < pageCount)
                        {
                            index = (currentPageIndex + 1) - pageHalf;
                            if (index <= 0)
                            {
                                index = 1;
                                totalPage = pageCount;
                            }
                            else
                            {
                                totalPage = (currentPageIndex + 1) + pageHalf;
                            }
                        }
                        else
                        {
                            index = (pageCount - pageLength) > 0 ? (pageCount - pageLength + 1) : 1;
                            totalPage = pageCount;
                        }
                    }
                    else
                    {
                        index = (currentPageIndex + 1) - pageHalf;
                        if (index <= 0)
                        {
                            index = 1;
                            totalPage = pageLength;
                        }
                        else
                        {
                            totalPage = index + pageLength - 1;
                        }
                    }

                    //pre ellipsis
                    if (index + pageLength < currentPageIndex + 1 && !string.IsNullOrEmpty(listEllipsis))
                    {
                        jsMethod = StlParserUtility.GetJsMethodInDynamicPage(type, pageInfo.PublishmentSystemInfo, contextInfo.ChannelID, contextInfo.ContentID, pageUrl, index, currentPageIndex, pageCount, isPageRefresh, ajaxDivID);

                        if (!string.IsNullOrEmpty(successTemplateString))
                        {
                            parsedContent = StlPageItem.GetParsedContent(successTemplateString, string.Format("javascript:{0}", jsMethod), listEllipsis, pageInfo);
                        }
                        else
                        {
                            pageBuilder.AppendFormat(@"<a href=""{0}"" onclick=""{1};return false;"" {2}>{3}</a>", PageUtils.UNCLICKED_URL, jsMethod, TranslateUtils.ToAttributesString(attributes), listEllipsis);
                        }
                    }

                    for (; index <= totalPage; index++)
                    {
                        if (currentPageIndex + 1 != index)
                        {
                            jsMethod = StlParserUtility.GetJsMethodInDynamicPage(type, pageInfo.PublishmentSystemInfo, contextInfo.ChannelID, contextInfo.ContentID, pageUrl, index, currentPageIndex, pageCount, isPageRefresh, ajaxDivID);

                            if (!string.IsNullOrEmpty(successTemplateString))
                            {
                                pageBuilder.Append(StlPageItem.GetParsedContent(successTemplateString, string.Format("javascript:{0}", jsMethod), Convert.ToString(index), pageInfo));
                            }
                            else
                            {
                                HyperLink pageHyperLink = new HyperLink();
                                pageHyperLink.NavigateUrl = PageUtils.UNCLICKED_URL;
                                pageHyperLink.Attributes.Add("onclick", jsMethod + ";return false;");
                                pageHyperLink.Text = string.Format("{0}{1}{2}", leftText, index, rightText);
                                if (!string.IsNullOrEmpty(linkClass))
                                {
                                    pageHyperLink.CssClass = linkClass;
                                }
                                pageBuilder.Append(ControlUtils.GetControlRenderHtml(pageHyperLink) + "&nbsp;");
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(failureTemplateString))
                            {
                                pageBuilder.Append(StlPageItem.GetParsedContent(failureTemplateString, PageUtils.UNCLICKED_URL, Convert.ToString(currentPageIndex + 1), pageInfo));
                            }
                            else
                            {
                                isAddSpan = true;
                                if (!alwaysA)
                                    pageBuilder.AppendFormat("{0}{1}{2}&nbsp;", leftText, index, rightText);
                                else
                                    pageBuilder.AppendFormat("<a href='javascript:void(0);'>{0}{1}{2}</a>&nbsp;", leftText, index, rightText);
                            }
                        }
                    }

                    //pre ellipsis
                    if (index < pageCount && !string.IsNullOrEmpty(listEllipsis))
                    {
                        jsMethod = StlParserUtility.GetJsMethodInDynamicPage(type, pageInfo.PublishmentSystemInfo, contextInfo.ChannelID, contextInfo.ContentID, pageUrl, index, currentPageIndex, pageCount, isPageRefresh, ajaxDivID);

                        if (!string.IsNullOrEmpty(successTemplateString))
                        {
                            parsedContent = StlPageItem.GetParsedContent(successTemplateString, string.Format("javascript:{0}", jsMethod), Convert.ToString(currentPageIndex + 1), pageInfo);
                        }
                        else
                        {
                            pageBuilder.AppendFormat(@"<a href=""{0}"" onclick=""{1};return false;"" {2}>{3}</a>", PageUtils.UNCLICKED_URL, jsMethod, TranslateUtils.ToAttributesString(attributes), listEllipsis);
                        }
                    }

                    parsedContent = text + pageBuilder;
                }
                else if (type.ToLower().Equals(StlPageItem.Type_PageSelect.ToLower()))//ҳ��ת
                {
                    HtmlSelect selectControl = new HtmlSelect();
                    if (!string.IsNullOrEmpty(textClass))
                    {
                        selectControl.Attributes.Add("class", textClass);
                    }
                    foreach (string key in attributes.Keys)
                    {
                        selectControl.Attributes[key] = attributes[key];
                    }

                    selectControl.Attributes.Add("onChange", jsMethod + ";return false;");

                    for (int index = 1; index <= pageCount; index++)
                    {
                        ListItem listitem = new ListItem(index.ToString(), index.ToString());
                        if (currentPageIndex + 1 == index)
                        {
                            listitem.Selected = true;
                        }
                        selectControl.Items.Add(listitem);
                    }

                    parsedContent = ControlUtils.GetControlRenderHtml(selectControl);
                }

                if (isAddSpan && !string.IsNullOrEmpty(textClass))
                {
                    parsedContent = string.Format(@"<span class=""{0}"">{1}</span>", textClass, parsedContent);
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        public static string ParseEntityInDynamicPage(string stlEntity, PageInfo pageInfo, string pageUrl, int nodeID, int currentPageIndex, int pageCount, int totalNum, bool isPageRefresh, string ajaxDivID)
        {
            string parsedContent = string.Empty;
            try
            {
                ContextInfo contextInfo = new ContextInfo(pageInfo);

                string type = stlEntity.Substring(stlEntity.IndexOf(".") + 1);
                if (!string.IsNullOrEmpty(type))
                {
                    type = type.TrimEnd('}').Trim();
                }
                bool isHyperlink = false;

                string jsMethod = StlParserUtility.GetJsMethodInDynamicPage(type, pageInfo.PublishmentSystemInfo, contextInfo.ChannelID, contextInfo.ContentID, pageUrl, 0, currentPageIndex, pageCount, isPageRefresh, ajaxDivID);

                if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_FirstPage) || StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_LastPage) || StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_PreviousPage) || StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_NextPage))
                {
                    if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_FirstPage))
                    {
                        if (currentPageIndex != 0)//��ǰҳ��Ϊ��ҳ
                        {
                            isHyperlink = true;
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_LastPage))
                    {
                        if (currentPageIndex + 1 != pageCount)//��ǰҳ��Ϊĩҳ
                        {
                            isHyperlink = true;
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_PreviousPage))
                    {
                        if (currentPageIndex != 0)//��ǰҳ��Ϊ��ҳ
                        {
                            isHyperlink = true;
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(type, StlPageItem.Type_NextPage))
                    {
                        if (currentPageIndex + 1 != pageCount)//��ǰҳ��Ϊĩҳ
                        {
                            isHyperlink = true;
                        }
                    }

                    if (isHyperlink)//��ǰҳ��Ϊ��ҳ
                    {
                        parsedContent = string.Format("javascript:{0}", jsMethod);
                    }
                    else
                    {
                        parsedContent = PageUtils.UNCLICKED_URL;
                    }
                }
                else if (type.ToLower().Equals(StlPageItem.Type_CurrentPageIndex.ToLower()))//��ǰҳ����
                {
                    parsedContent = Convert.ToString(currentPageIndex + 1);
                }
                else if (type.ToLower().Equals(StlPageItem.Type_TotalPageNum.ToLower()))//��ҳ��
                {
                    parsedContent = Convert.ToString(pageCount);
                }
                else if (type.ToLower().Equals(StlPageItem.Type_TotalNum.ToLower()))//��������
                {
                    parsedContent = Convert.ToString(totalNum);
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string GetParsedContent(string content, string pageUrl, string pageNum, PageInfo pageInfo)
        {
            string parsedContent = StringUtils.ReplaceIgnoreCase(content, "{Current.Url}", pageUrl);
            parsedContent = StringUtils.ReplaceIgnoreCase(parsedContent, "{Current.Num}", pageNum);

            StringBuilder innerBuilder = new StringBuilder(parsedContent);
            ContextInfo contextInfo = new ContextInfo(pageInfo);
            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
            return innerBuilder.ToString();
        }
    }
}
