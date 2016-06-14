using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlChannel
    {
        private StlChannel() { }
        public const string ElementName = "stl:channel";//��Ŀֵ

        public const string Attribute_ChannelIndex = "channelindex";            //��Ŀ����
        public const string Attribute_ChannelName = "channelname";              //��Ŀ����
        public const string Attribute_Parent = "parent";                        //��ʾ����Ŀ����
        public const string Attribute_UpLevel = "uplevel";						//�ϼ���Ŀ�ļ���
        public const string Attribute_TopLevel = "toplevel";                    //����ҳ���µ���Ŀ����
        public const string Attribute_Type = "type";							//��ʾ������

        public const string Attribute_LeftText = "lefttext";                    //��ʾ����Ϣǰ������
        public const string Attribute_RightText = "righttext";                  //��ʾ����Ϣ�������
        public const string Attribute_FormatString = "formatstring";            //��ʾ�ĸ�ʽ
        public const string Attribute_Separator = "separator";                  //��ʾ����ʱ�ķָ��ַ���
        public const string Attribute_StartIndex = "startindex";			//�ַ���ʼλ��
        public const string Attribute_Length = "length";                    //ָ���ַ�����
        public const string Attribute_WordNum = "wordnum";						//��ʾ�ַ�����Ŀ
        public const string Attribute_Ellipsis = "ellipsis";                    //���ֳ���������ʾ������
        public const string Attribute_Replace = "replace";                      //��Ҫ�滻�����֣�������������ʽ
        public const string Attribute_To = "to";                                //�滻replace��������Ϣ
        public const string Attribute_IsClearTags = "iscleartags";              //�Ƿ���ʾ���HTML��ǩ�������
        public const string Attribute_IsReturnToBR = "isreturntobr";        //�Ƿ񽫻س��滻ΪHTML���б�ǩ
        public const string Attribute_IsLower = "islower";			        //ת��ΪСд
        public const string Attribute_IsUpper = "isupper";			        //ת��Ϊ��д
        public const string Attribute_IsDynamic = "isdynamic";              //�Ƿ�̬��ʾ

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_ChannelIndex, "��Ŀ����");
                attributes.Add(Attribute_ChannelName, "��Ŀ����");
                attributes.Add(Attribute_Type, "��ʾ������");
                attributes.Add(Attribute_Parent, "��ʾ����Ŀ����");
                attributes.Add(Attribute_UpLevel, "�ϼ���Ŀ�ļ���");
                attributes.Add(Attribute_TopLevel, "����ҳ���µ���Ŀ����");
                attributes.Add(Attribute_LeftText, "��ʾ����Ϣǰ������");
                attributes.Add(Attribute_RightText, "��ʾ����Ϣ�������");
                attributes.Add(Attribute_FormatString, "��ʾ�ĸ�ʽ");
                attributes.Add(Attribute_Separator, "��ʾ����ʱ�ķָ��ַ���");
                attributes.Add(Attribute_StartIndex, "�ַ���ʼλ��");
                attributes.Add(Attribute_Length, "ָ���ַ�����");
                attributes.Add(Attribute_WordNum, "��ʾ�ַ�����Ŀ");
                attributes.Add(Attribute_Ellipsis, "���ֳ���������ʾ������");
                attributes.Add(Attribute_Replace, "��Ҫ�滻�����֣�������������ʽ");
                attributes.Add(Attribute_To, "�滻replace��������Ϣ");
                attributes.Add(Attribute_IsClearTags, "�Ƿ������ǩ��Ϣ");
                attributes.Add(Attribute_IsReturnToBR, "�Ƿ񽫻س��滻ΪHTML���б�ǩ");
                attributes.Add(Attribute_IsLower, "ת��ΪСд");
                attributes.Add(Attribute_IsUpper, "ת��Ϊ��д");
                attributes.Add(Attribute_IsDynamic, "�Ƿ�̬��ʾ");
                return attributes;
            }
        }


        //�ԡ���Ŀ���ԡ���stl:channel��Ԫ�ؽ��н���
        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                IEnumerator ie = node.Attributes.GetEnumerator();
                StringDictionary attributes = new StringDictionary();
                string leftText = string.Empty;
                string rightText = string.Empty;
                string channelIndex = string.Empty;
                string channelName = string.Empty;
                int upLevel = 0;
                int topLevel = -1;
                string type = NodeAttribute.Title;
                string formatString = string.Empty;
                string separator = null;
                int startIndex = 0;
                int length = 0;
                int wordNum = 0;
                string ellipsis = StringUtils.Constants.Ellipsis;
                string replace = string.Empty;
                string to = string.Empty;
                bool isClearTags = false;
                bool isReturnToBR = false;
                bool isLower = false;
                bool isUpper = false;
                bool isDynamic = false;

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlChannel.Attribute_ChannelIndex))
                    {
                        channelIndex = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(StlChannel.Attribute_ChannelName))
                    {
                        channelName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(StlChannel.Attribute_Parent))
                    {
                        if (TranslateUtils.ToBool(attr.Value))
                        {
                            upLevel = 1;
                        }
                    }
                    else if (attributeName.Equals(StlChannel.Attribute_UpLevel))
                    {
                        upLevel = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlChannel.Attribute_TopLevel))
                    {
                        topLevel = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlChannel.Attribute_Type))
                    {
                        type = attr.Value;
                    }
                    else if (attributeName.Equals(StlChannel.Attribute_LeftText))
                    {
                        leftText = attr.Value;
                    }
                    else if (attributeName.Equals(StlChannel.Attribute_RightText))
                    {
                        rightText = attr.Value;
                    }
                    else if (attributeName.Equals(StlChannel.Attribute_FormatString))
                    {
                        formatString = attr.Value;
                    }
                    else if (attributeName.Equals(StlChannel.Attribute_Separator))
                    {
                        separator = attr.Value;
                    }
                    else if (attributeName.Equals(StlChannel.Attribute_StartIndex))
                    {
                        startIndex = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlChannel.Attribute_Length))
                    {
                        length = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlChannel.Attribute_WordNum))
                    {
                        wordNum = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlChannel.Attribute_Ellipsis))
                    {
                        ellipsis = attr.Value;
                    }
                    else if (attributeName.Equals(StlChannel.Attribute_Replace))
                    {
                        replace = attr.Value;
                    }
                    else if (attributeName.Equals(StlChannel.Attribute_To))
                    {
                        to = attr.Value;
                    }
                    else if (attributeName.Equals(StlChannel.Attribute_IsClearTags))
                    {
                        isClearTags = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(StlChannel.Attribute_IsReturnToBR))
                    {
                        isReturnToBR = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(StlChannel.Attribute_IsLower))
                    {
                        isLower = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(StlChannel.Attribute_IsUpper))
                    {
                        isUpper = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(StlChannel.Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else
                    {
                        attributes.Add(attributeName, attr.Value);
                    }
                }

                if (isDynamic)
                {
                    parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(stlElement, node, pageInfo, contextInfo, attributes, leftText, rightText, channelIndex, channelName, upLevel, topLevel, type, formatString, separator, startIndex, length, wordNum, ellipsis, replace, to, isClearTags, isReturnToBR, isLower, isUpper);
                }

            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, StringDictionary attributes, string leftText, string rightText, string channelIndex, string channelName, int upLevel, int topLevel, string type, string formatString, string separator, int startIndex, int length, int wordNum, string ellipsis, string replace, string to, bool isClearTags, bool isReturnToBR, bool isLower, bool isUpper)
        {
            string parsedContent = string.Empty;

            int channelID = StlDataUtility.GetNodeIDByLevel(pageInfo.PublishmentSystemID, contextInfo.ChannelID, upLevel, topLevel);

            channelID = CreateCacheManager.NodeID.GetNodeIDByChannelIDOrChannelIndexOrChannelName(pageInfo.PublishmentSystemID, channelID, channelIndex, channelName);
            NodeInfo channel = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, channelID);

            if (!string.IsNullOrEmpty(formatString))
            {
                formatString = formatString.Trim();
                if (!formatString.StartsWith("{0"))
                {
                    formatString = "{0:" + formatString;
                }
                if (!formatString.EndsWith("}"))
                {
                    formatString = formatString + "}";
                }
            }

            if (!string.IsNullOrEmpty(type))
            {
                if (type.ToLower().Equals(NodeAttribute.Title.ToLower()))
                {
                    parsedContent = channel.NodeName;

                    if (isClearTags)
                    {
                        parsedContent = StringUtils.StripTags(parsedContent);
                    }

                    if (!string.IsNullOrEmpty(replace))
                    {
                        parsedContent = StringUtils.Replace(replace, parsedContent, to);
                    }

                    if (wordNum == 0 && contextInfo.TitleWordNum > 0)
                        wordNum = contextInfo.TitleWordNum;

                    if (!string.IsNullOrEmpty(parsedContent) && wordNum > 0)
                    {
                        parsedContent = StringUtils.MaxLengthText(parsedContent, wordNum, ellipsis);
                    }
                }
                else if (type.ToLower().Equals(NodeAttribute.ChannelIndex.ToLower()))
                {
                    parsedContent = channel.NodeIndexName;

                    if (!string.IsNullOrEmpty(replace))
                    {
                        parsedContent = StringUtils.Replace(replace, parsedContent, to);
                    }

                    if (!string.IsNullOrEmpty(parsedContent) && wordNum > 0)
                    {
                        parsedContent = StringUtils.MaxLengthText(parsedContent, wordNum, ellipsis);
                    }
                }
                else if (type.ToLower().Equals(NodeAttribute.Content.ToLower()))
                {
                    parsedContent = StringUtility.TextEditorContentDecode(channel.Content, pageInfo.PublishmentSystemInfo);

                    if (isClearTags)
                    {
                        parsedContent = StringUtils.StripTags(parsedContent);
                    }

                    if (!string.IsNullOrEmpty(replace))
                    {
                        parsedContent = StringUtils.Replace(replace, parsedContent, to);
                    }

                    if (!string.IsNullOrEmpty(parsedContent) && wordNum > 0)
                    {
                        parsedContent = StringUtils.MaxLengthText(parsedContent, wordNum, ellipsis);
                    }
                }
                else if (type.ToLower().Equals(NodeAttribute.PageContent.ToLower()))
                {
                    if (contextInfo.IsInnerElement || pageInfo.TemplateInfo.TemplateType != ETemplateType.ChannelTemplate)
                    {
                        parsedContent = StringUtility.TextEditorContentDecode(channel.Content, pageInfo.PublishmentSystemInfo);

                        if (isClearTags)
                        {
                            parsedContent = StringUtils.StripTags(parsedContent);
                        }

                        if (!string.IsNullOrEmpty(replace))
                        {
                            parsedContent = StringUtils.Replace(replace, parsedContent, to);
                        }

                        if (!string.IsNullOrEmpty(parsedContent) && wordNum > 0)
                        {
                            parsedContent = StringUtils.MaxLengthText(parsedContent, wordNum, ellipsis);
                        }
                    }
                    else
                    {
                        return stlElement;
                    }
                }
                else if (type.ToLower().Equals(NodeAttribute.AddDate.ToLower()))
                {
                    parsedContent = DateUtils.Format(channel.AddDate, formatString);
                }
                else if (type.ToLower().Equals(NodeAttribute.ImageUrl.ToLower()))
                {
                    parsedContent = InputParserUtility.GetImageOrFlashHtml(pageInfo.PublishmentSystemInfo, channel.ImageUrl, attributes, false);
                }
                else if (type.ToLower().Equals(NodeAttribute.ID.ToLower()))
                {
                    parsedContent = channelID.ToString();
                }
                else if (StringUtils.StartsWithIgnoreCase(type, StlParserUtility.ItemIndex) && contextInfo.ItemContainer != null && contextInfo.ItemContainer.ChannelItem != null)
                {
                    int itemIndex = StlParserUtility.ParseItemIndex(contextInfo.ItemContainer.ChannelItem.ItemIndex, type, contextInfo);
                    if (!string.IsNullOrEmpty(formatString))
                    {
                        parsedContent = string.Format(formatString, itemIndex);
                    }
                    else
                    {
                        parsedContent = itemIndex.ToString();
                    }
                }
                else if (type.ToLower().Equals(NodeAttribute.CountOfChannels.ToLower()))
                {
                    parsedContent = channel.ChildrenCount.ToString();
                }
                else if (type.ToLower().Equals(NodeAttribute.CountOfContents.ToLower()))
                {
                    parsedContent = channel.ContentNum.ToString();
                }
                else if (type.ToLower().Equals(NodeAttribute.CountOfImageContents.ToLower()))
                {
                    int count = DataProvider.BackgroundContentDAO.GetCountCheckedImage(pageInfo.PublishmentSystemID, channel.NodeID);
                    parsedContent = count.ToString();
                }
                else if (type.ToLower().Equals(NodeAttribute.Keywords.ToLower()))
                {
                    parsedContent = channel.Keywords;
                }
                else if (type.ToLower().Equals(NodeAttribute.Description.ToLower()))
                {
                    parsedContent = channel.Description;
                }
                else
                {
                    string attributeName = type;

                    NameValueCollection formCollection = channel.Additional.Attributes;
                    if (formCollection != null && formCollection.Count > 0)
                    {
                        TableStyleInfo styleInfo = TableStyleManager.GetTableStyleInfo(ETableStyle.Channel, DataProvider.NodeDAO.TableName, attributeName, RelatedIdentities.GetChannelRelatedIdentities(pageInfo.PublishmentSystemID, channel.NodeID));
                        parsedContent = GetValue(attributeName, formCollection, false, styleInfo.DefaultValue);
                        if (!string.IsNullOrEmpty(parsedContent))
                        {
                            parsedContent = InputTypeParser.GetContentByTableStyle(parsedContent, separator, pageInfo.PublishmentSystemInfo, ETableStyle.Channel, styleInfo, formatString, attributes, node.InnerXml, false);
                            parsedContent = StringUtils.ParseString(styleInfo.InputType, parsedContent, replace, to, startIndex, length, wordNum, ellipsis, isClearTags, isReturnToBR, isLower, isUpper, formatString);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(parsedContent))
                {
                    parsedContent = leftText + parsedContent + rightText;
                }
            }

            return parsedContent;
        }

        private static string GetValue(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, string defaultValue)
        {
            string value = string.Empty;
            if (formCollection != null && formCollection[attributeName] != null)
            {
                value = formCollection[attributeName];
            }
            if (isAddAndNotPostBack && string.IsNullOrEmpty(value))
            {
                value = defaultValue;
            }
            return value;
        }
    }
}
