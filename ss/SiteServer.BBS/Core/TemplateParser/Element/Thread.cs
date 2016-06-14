using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Xml;
using System.Collections;
using BaiRong.Core;
using SiteServer.BBS.Core.TemplateParser.Model;
using System.Web.UI.WebControls;
using SiteServer.BBS.Core.TemplateParser.ListTemplate;
using SiteServer.BBS.Model;
using System.Web.UI;

namespace SiteServer.BBS.Core.TemplateParser.Element
{
    public class Thread
    {
        private Thread() { }
        public const string ElementName = "bbs:thread";//内容值

        public const string Attribute_Type = "type";						//显示的类型
        public const string Attribute_LeftText = "lefttext";                //显示在信息前的文字
        public const string Attribute_RightText = "righttext";              //显示在信息后的文字
        public const string Attribute_FormatString = "formatstring";        //显示的格式
        public const string Attribute_Separator = "separator";              //显示多项时的分割字符串
        public const string Attribute_StartIndex = "startindex";			//字符开始位置
        public const string Attribute_Length = "length";			        //指定字符长度
        public const string Attribute_WordNum = "wordnum";					//显示字符的数目
        public const string Attribute_Ellipsis = "ellipsis";                //文字超出部分显示的文字
        public const string Attribute_Replace = "replace";                  //需要替换的文字，可以是正则表达式
        public const string Attribute_To = "to";                            //替换replace的文字信息
        public const string Attribute_IsClearTags = "iscleartags";          //是否清除标签信息
        public const string Attribute_IsReturnToBR = "isreturntobr";        //是否将回车替换为HTML换行标签
        public const string Attribute_IsLower = "islower";			        //转换为小写
        public const string Attribute_IsUpper = "isupper";			        //转换为大写
        public const string Attribute_IsEditable = "iseditable";            //是否可在前台编辑
        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_Type, "显示的类型");
                attributes.Add(Attribute_LeftText, "显示在信息前的文字");
                attributes.Add(Attribute_RightText, "显示在信息后的文字");
                attributes.Add(Attribute_FormatString, "显示的格式");
                attributes.Add(Attribute_Separator, "显示多项时的分割字符串");
                attributes.Add(Attribute_StartIndex, "字符开始位置");
                attributes.Add(Attribute_Length, "指定字符长度");
                attributes.Add(Attribute_WordNum, "显示字符的数目");
                attributes.Add(Attribute_Ellipsis, "文字超出部分显示的文字");
                attributes.Add(Attribute_Replace, "需要替换的文字，可以是正则表达式");
                attributes.Add(Attribute_To, "替换replace的文字信息");
                attributes.Add(Attribute_IsClearTags, "是否清除标签信息");
                attributes.Add(Attribute_IsReturnToBR, "是否将回车替换为HTML换行标签");
                attributes.Add(Attribute_IsLower, "转换为小写");
                attributes.Add(Attribute_IsUpper, "转换为大写");
                attributes.Add(Attribute_IsEditable, "是否可在前台编辑");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
                return attributes;
            }
        }

        public static string Parse(string element, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
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
                bool isEditable = true;
                bool isDynamic = false;
                string type = ThreadAttribute.Title.ToLower();

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(Thread.Attribute_Type))
                    {
                        type = attr.Value.ToLower();
                    }
                    else if (attributeName.Equals(Thread.Attribute_LeftText))
                    {
                        leftText = attr.Value;
                    }
                    else if (attributeName.Equals(Thread.Attribute_RightText))
                    {
                        rightText = attr.Value;
                    }
                    else if (attributeName.Equals(Thread.Attribute_FormatString))
                    {
                        formatString = attr.Value;
                    }
                    else if (attributeName.Equals(Thread.Attribute_Separator))
                    {
                        separator = attr.Value;
                    }
                    else if (attributeName.Equals(Thread.Attribute_StartIndex))
                    {
                        startIndex = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(Thread.Attribute_Length))
                    {
                        length = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(Thread.Attribute_WordNum))
                    {
                        wordNum = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(Thread.Attribute_Ellipsis))
                    {
                        ellipsis = attr.Value;
                    }
                    else if (attributeName.Equals(Thread.Attribute_Replace))
                    {
                        replace = attr.Value;
                    }
                    else if (attributeName.Equals(Thread.Attribute_To))
                    {
                        to = attr.Value;
                    }
                    else if (attributeName.Equals(Thread.Attribute_IsClearTags))
                    {
                        isClearTags = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(Thread.Attribute_IsReturnToBR))
                    {
                        isReturnToBR = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(Thread.Attribute_IsLower))
                    {
                        isLower = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(Thread.Attribute_IsUpper))
                    {
                        isUpper = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(Thread.Attribute_IsEditable))
                    {
                        isEditable = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(Thread.Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else
                    {
                        attributes.Add(attributeName, attr.Value);
                    }
                }

                if (isDynamic)
                {
                    parsedContent = Dynamic.ParseDynamicElement(ElementName, element, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(element, node, pageInfo, contextInfo, leftText, rightText, formatString, separator, startIndex, length, wordNum, ellipsis, replace, to, isClearTags, isReturnToBR, isLower, isUpper, isEditable, type, attributes);
                }
            }
            catch (Exception ex)
            {
                parsedContent = ParserUtility.GetErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(string element, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, string leftText, string rightText, string formatString, string separator, int startIndex, int length, int wordNum, string ellipsis, string replace, string to, bool isClearTags, bool isReturnToBR, bool isLower, bool isUpper, bool isEditable, string type, StringDictionary attributes)
        {
            string parsedContent = string.Empty;

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

            if (contextInfo.ThreadInfo != null)
            {
                if (ThreadAttribute.Title.ToLower().Equals(type))
                {
                    parsedContent = StringUtils.ParseString(contextInfo.ThreadInfo.Title, replace, to, startIndex, length, wordNum, ellipsis, isClearTags, isReturnToBR, isLower, isUpper, formatString);

                    if (!contextInfo.IsInnerElement)
                    {
                        parsedContent = parsedContent.Replace("&", "&amp;");
                    }
                }
                else if (ThreadAttribute.AddDate.ToLower().Equals(type))
                {
                    parsedContent = DateUtils.Format(contextInfo.ThreadInfo.AddDate, formatString);
                }
                else if (ThreadAttribute.NavigationUrl.ToLower().Equals(type))
                {
                    parsedContent = PageUtilityBBS.GetThreadUrl(pageInfo.PublishmentSystemID, contextInfo.ForumID, contextInfo.ThreadID);
                }
                else if (ThreadAttribute.Content.ToLower().Equals(type))
                {
                    parsedContent = DataProvider.PostDAO.GetThreadValue(pageInfo.PublishmentSystemID, contextInfo.ThreadID, PostAttribute.Content);

                    parsedContent = StringUtilityBBS.TextEditorContentDecode(pageInfo.PublishmentSystemID, parsedContent);

                    parsedContent = StringUtils.ParseString(parsedContent, replace, to, startIndex, length, wordNum, ellipsis, isClearTags, isReturnToBR, isLower, isUpper, formatString);

                    if (!contextInfo.IsInnerElement)
                    {
                        parsedContent = parsedContent.Replace("&", "&amp;");
                    }
                }
                else if (StringUtils.StartsWithIgnoreCase(type, ParserUtility.ItemIndex) && contextInfo.ItemContainer != null && contextInfo.ItemContainer.ThreadItem != null)
                {
                    int itemIndex = ParserUtility.ParseItemIndex(contextInfo.ItemContainer.ThreadItem.ItemIndex, type, contextInfo);
                    if (!string.IsNullOrEmpty(formatString))
                    {
                        parsedContent = string.Format(formatString, itemIndex);
                    }
                    else
                    {
                        parsedContent = itemIndex.ToString();
                    }
                }
                else
                {
                    if (contextInfo.ThreadInfo.ContainsKey(type))
                    {
                        parsedContent = contextInfo.ThreadInfo.GetExtendedAttribute(type);

                        parsedContent = StringUtils.ParseString(parsedContent, replace, to, startIndex, length, wordNum, ellipsis, isClearTags, isReturnToBR, isLower, isUpper, formatString);

                        if (!contextInfo.IsInnerElement)
                        {
                            parsedContent = parsedContent.Replace("&", "&amp;");
                        }
                    }
                }

                if (!string.IsNullOrEmpty(parsedContent))
                {
                    parsedContent = leftText + parsedContent + rightText;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(type) && contextInfo.ItemContainer != null && contextInfo.ItemContainer.ThreadItem != null)
                {
                    parsedContent = DataBinder.Eval(contextInfo.ItemContainer.ThreadItem.DataItem, type, "{0}");

                    parsedContent = StringUtils.ParseString(parsedContent, replace, to, startIndex, length, wordNum, ellipsis, isClearTags, isReturnToBR, isLower, isUpper, formatString);

                    if (!string.IsNullOrEmpty(parsedContent))
                    {
                        parsedContent = leftText + parsedContent + rightText;
                    }
                }
            }

            return parsedContent;
        }
    }
}
