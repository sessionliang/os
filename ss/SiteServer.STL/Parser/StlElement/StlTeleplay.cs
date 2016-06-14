using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Xml;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlTeleplay
    {
        private StlTeleplay() { }
        public const string ElementName = "stl:teleplay";                      //���ͼƬ

        public const string Attribute_Type = "type";						//��ʾ������
        public const string Attribute_LeftText = "lefttext";                    //��ʾ����Ϣǰ������
        public const string Attribute_RightText = "righttext";                  //��ʾ����Ϣ�������
        public const string Attribute_FormatString = "formatstring";        //��ʾ�ĸ�ʽ
        public const string Attribute_Separator = "separator";              //��ʾ����ʱ�ķָ��ַ���
        public const string Attribute_StartIndex = "startindex";			//�ַ���ʼλ��
        public const string Attribute_Length = "length";			        //ָ���ַ�����
        public const string Attribute_WordNum = "wordnum";					//��ʾ�ַ�����Ŀ
        public const string Attribute_Ellipsis = "ellipsis";                //���ֳ���������ʾ������
        public const string Attribute_Replace = "replace";                      //��Ҫ�滻�����֣�������������ʽ
        public const string Attribute_To = "to";                                //�滻replace��������Ϣ
        public const string Attribute_IsClearTags = "iscleartags";          //�Ƿ������ǩ��Ϣ
        public const string Attribute_IsReturnToBR = "isreturntobr";        //�Ƿ񽫻س��滻ΪHTML���б�ǩ
        public const string Attribute_IsLower = "islower";			        //ת��ΪСд
        public const string Attribute_IsUpper = "isupper";			        //ת��Ϊ��д
        public const string Attribute_IsDynamic = "isdynamic";              //�Ƿ�̬��ʾ

        public const string Attribute_OnlyUrl = "onlyurl"; //ֻ��ʾ����


        public const string Type_StillUrl = "stillurl";    //�缯��ַ���������Ŵ���
        public const string Type_PlayUrl = "playurl";    //�����Ĳ��ŵ�ַ
        public const string Type_Title = "title";    //�缯����
        public const string Type_Description = "description";    //�缯����
        public const string Type_Content = "content";    //�缯����


        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();

                attributes.Add(Attribute_Type, "��ʾ������");
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
                attributes.Add(Attribute_OnlyUrl, "ֻ�������");
                return attributes;
            }
        }

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                IEnumerator ie = node.Attributes.GetEnumerator();
                StringDictionary attributes = new StringDictionary();

                string leftText = string.Empty;
                string rightText = string.Empty;
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
                string type = string.Empty;
                bool isDynamic = false;
                bool onlyUrl = false;

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();

                    if (attributeName.Equals(StlTeleplay.Attribute_Type))
                    {
                        type = attr.Value.ToLower();
                    }
                    else if (attributeName.Equals(StlTeleplay.Attribute_LeftText))
                    {
                        leftText = attr.Value;
                    }
                    else if (attributeName.Equals(StlTeleplay.Attribute_RightText))
                    {
                        rightText = attr.Value;
                    }
                    else if (attributeName.Equals(StlTeleplay.Attribute_FormatString))
                    {
                        formatString = attr.Value;
                    }
                    else if (attributeName.Equals(StlTeleplay.Attribute_Separator))
                    {
                        separator = attr.Value;
                    }
                    else if (attributeName.Equals(StlTeleplay.Attribute_StartIndex))
                    {
                        startIndex = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlTeleplay.Attribute_Length))
                    {
                        length = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlTeleplay.Attribute_WordNum))
                    {
                        wordNum = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlTeleplay.Attribute_Ellipsis))
                    {
                        ellipsis = attr.Value;
                    }
                    else if (attributeName.Equals(StlTeleplay.Attribute_Replace))
                    {
                        replace = attr.Value;
                    }
                    else if (attributeName.Equals(StlTeleplay.Attribute_To))
                    {
                        to = attr.Value;
                    }
                    else if (attributeName.Equals(StlTeleplay.Attribute_IsClearTags))
                    {
                        isClearTags = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(StlTeleplay.Attribute_IsReturnToBR))
                    {
                        isReturnToBR = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(StlTeleplay.Attribute_IsLower))
                    {
                        isLower = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(StlTeleplay.Attribute_IsUpper))
                    {
                        isUpper = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(StlTeleplay.Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(StlTeleplay.Attribute_OnlyUrl))
                    {
                        onlyUrl = TranslateUtils.ToBool(attr.Value);
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
                    parsedContent = ParseImpl(node, pageInfo, contextInfo, attributes, leftText, rightText, formatString, separator, startIndex, length, wordNum, ellipsis, replace, to, isClearTags, isReturnToBR, isLower, isUpper, type, onlyUrl);
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, StringDictionary attributes, string leftText, string rightText, string formatString, string separator, int startIndex, int length, int wordNum, string ellipsis, string replace, string to, bool isClearTags, bool isReturnToBR, bool isLower, bool isUpper, string type, bool onlyUrl)
        {
            string parsedContent = string.Empty;

            if (!string.IsNullOrEmpty(type) && contextInfo.ItemContainer != null && contextInfo.ItemContainer.TeleplayItem != null)
            {
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
                else
                {
                    formatString = "{0}";
                }

                if (StringUtils.StartsWithIgnoreCase(type, StlParserUtility.ItemIndex))
                {
                    int itemIndex = StlParserUtility.ParseItemIndex(contextInfo.ItemContainer.TeleplayItem.ItemIndex, type, contextInfo);

                    if (!string.IsNullOrEmpty(formatString))
                    {
                        parsedContent = string.Format(formatString, itemIndex);
                    }
                    else
                    {
                        parsedContent = itemIndex.ToString();
                    }
                }
                else if (StringUtils.StartsWithIgnoreCase(type, StlTeleplay.Type_StillUrl))
                {
                    parsedContent = DataBinder.Eval(contextInfo.ItemContainer.TeleplayItem.DataItem, type, formatString);

                    parsedContent = InputParserUtility.GetVideoHtml(pageInfo.PublishmentSystemInfo, parsedContent, attributes, false);
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlTeleplay.Type_PlayUrl))
                {
                    parsedContent = DataBinder.Eval(contextInfo.ItemContainer.TeleplayItem.DataItem, StlTeleplay.Type_StillUrl, formatString);
                    if (!onlyUrl)
                    {
                        string title = DataBinder.Eval(contextInfo.ItemContainer.TeleplayItem.DataItem, StlTeleplay.Type_Title, formatString);
                        parsedContent = InputParserUtility.GetFileHtmlWithoutCount(pageInfo.PublishmentSystemInfo, parsedContent, attributes, title, false);
                    }
                    parsedContent = StringUtils.ReplaceNewlineToBR(parsedContent);

                }
                else if (StringUtils.StartsWithIgnoreCase(type, StlTeleplay.Type_Description) || StringUtils.StartsWithIgnoreCase(type, StlTeleplay.Type_Content))
                {
                    parsedContent = DataBinder.Eval(contextInfo.ItemContainer.TeleplayItem.DataItem, type, formatString);

                    parsedContent = StringUtils.ReplaceNewlineToBR(parsedContent);
                }
                else if (StringUtils.StartsWithIgnoreCase(type, StlTeleplay.Type_Title))
                {
                    parsedContent = DataBinder.Eval(contextInfo.ItemContainer.TeleplayItem.DataItem, type, formatString);

                    parsedContent = StringUtils.ReplaceNewlineToBR(parsedContent);
                }
                else
                {
                    parsedContent = DataBinder.Eval(contextInfo.ItemContainer.TeleplayItem.DataItem, type, formatString);
                }
            }

            if (!string.IsNullOrEmpty(parsedContent))
            {
                parsedContent = StringUtils.ParseString(parsedContent, replace, to, startIndex, length, wordNum, ellipsis, isClearTags, isReturnToBR, isLower, isUpper, formatString);

                if (!string.IsNullOrEmpty(parsedContent))
                {
                    parsedContent = leftText + parsedContent + rightText;
                }
            }

            return parsedContent;
        }
    }
}
