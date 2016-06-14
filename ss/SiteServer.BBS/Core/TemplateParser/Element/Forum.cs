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

namespace SiteServer.BBS.Core.TemplateParser.Element
{
    public class Forum
    {
        private Forum() { }
        public const string ElementName = "bbs:forum";//板块值

        public const string Attribute_ForumIndex = "forumindex";			//栏目索引
        public const string Attribute_ForumName = "forumname";				//栏目名称
        public const string Attribute_Parent = "parent";						//显示父栏目属性
        public const string Attribute_UpLevel = "uplevel";						//上级栏目的级别
        public const string Attribute_TopLevel = "toplevel";                    //从首页向下的栏目级别
        public const string Attribute_Type = "type";							//显示的类型

        public const string Attribute_LeftText = "lefttext";                    //显示在信息前的文字
        public const string Attribute_RightText = "righttext";                  //显示在信息后的文字
        public const string Attribute_FormatString = "formatstring";            //显示的格式
        public const string Attribute_Separator = "separator";                  //显示多项时的分割字符串
        public const string Attribute_StartIndex = "startindex";			//字符开始位置
        public const string Attribute_Length = "length";			        //指定字符长度
        public const string Attribute_WordNum = "wordnum";						//显示字符的数目
        public const string Attribute_Ellipsis = "ellipsis";                    //文字超出部分显示的文字
        public const string Attribute_Replace = "replace";                      //需要替换的文字，可以是正则表达式
        public const string Attribute_To = "to";                                //替换replace的文字信息
        public const string Attribute_IsClearTags = "iscleartags";              //是否显示清除HTML标签后的文字
        public const string Attribute_IsReturnToBR = "isreturntobr";        //是否将回车替换为HTML换行标签
        public const string Attribute_IsLower = "islower";			        //转换为小写
        public const string Attribute_IsUpper = "isupper";			        //转换为大写
        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_ForumIndex, "板块索引");
                attributes.Add(Attribute_ForumName, "板块名称");
                attributes.Add(Attribute_Type, "显示的类型");
                attributes.Add(Attribute_Parent, "显示父栏目属性");
                attributes.Add(Attribute_UpLevel, "上级栏目的级别");
                attributes.Add(Attribute_TopLevel, "从首页向下的栏目级别");
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
                string channelIndex = string.Empty;
                string channelName = string.Empty;
                int upLevel = 0;
                int topLevel = -1;
                string type = "Title";
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
                    if (attributeName.Equals(Forum.Attribute_ForumIndex))
                    {
                        channelIndex = attr.Value;
                    }
                    else if (attributeName.Equals(Forum.Attribute_ForumName))
                    {
                        channelName = attr.Value;
                    }
                    else if (attributeName.Equals(Forum.Attribute_Parent))
                    {
                        if (TranslateUtils.ToBool(attr.Value))
                        {
                            upLevel = 1;
                        }
                    }
                    else if (attributeName.Equals(Forum.Attribute_UpLevel))
                    {
                        upLevel = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(Forum.Attribute_TopLevel))
                    {
                        topLevel = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(Forum.Attribute_Type))
                    {
                        type = attr.Value;
                    }
                    else if (attributeName.Equals(Forum.Attribute_LeftText))
                    {
                        leftText = attr.Value;
                    }
                    else if (attributeName.Equals(Forum.Attribute_RightText))
                    {
                        rightText = attr.Value;
                    }
                    else if (attributeName.Equals(Forum.Attribute_FormatString))
                    {
                        formatString = attr.Value;
                    }
                    else if (attributeName.Equals(Forum.Attribute_Separator))
                    {
                        separator = attr.Value;
                    }
                    else if (attributeName.Equals(Forum.Attribute_StartIndex))
                    {
                        startIndex = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(Forum.Attribute_Length))
                    {
                        length = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(Forum.Attribute_WordNum))
                    {
                        wordNum = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(Forum.Attribute_Ellipsis))
                    {
                        ellipsis = attr.Value;
                    }
                    else if (attributeName.Equals(Forum.Attribute_Replace))
                    {
                        replace = attr.Value;
                    }
                    else if (attributeName.Equals(Forum.Attribute_To))
                    {
                        to = attr.Value;
                    }
                    else if (attributeName.Equals(Forum.Attribute_IsClearTags))
                    {
                        isClearTags = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(Forum.Attribute_IsReturnToBR))
                    {
                        isReturnToBR = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(Forum.Attribute_IsLower))
                    {
                        isLower = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(Forum.Attribute_IsUpper))
                    {
                        isUpper = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(Forum.Attribute_IsDynamic))
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
                    parsedContent = Dynamic.ParseDynamicElement(ElementName, element, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(node, pageInfo, contextInfo, attributes, leftText, rightText, channelIndex, channelName, upLevel, topLevel, type, formatString, separator, startIndex, length, wordNum, ellipsis, replace, to, isClearTags, isReturnToBR, isLower, isUpper);
                }

            }
            catch (Exception ex)
            {
                parsedContent = ParserUtility.GetErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, StringDictionary attributes, string leftText, string rightText, string channelIndex, string channelName, int upLevel, int topLevel, string type, string formatString, string separator, int startIndex, int length, int wordNum, string ellipsis, string replace, string to, bool isClearTags, bool isReturnToBR, bool isLower, bool isUpper)
        {
            string parsedContent = string.Empty;

            int forumID = DataUtility.GetForumIDByLevel(pageInfo.PublishmentSystemID, contextInfo.ForumID, upLevel, topLevel);

            //channelID = CreateCacheManager.NodeID.GetNodeIDByChannelIDOrChannelIndexOrChannelName(pageInfo.PublishmentSystemID, channelID, channelIndex, channelName);
            ForumInfo forumInfo = ForumManager.GetForumInfo(pageInfo.PublishmentSystemID, forumID);

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
                if (StringUtils.EqualsIgnoreCase(ForumAttribute.Title, type))
                {
                    parsedContent = StringUtils.ParseString(forumInfo.ForumName, replace, to, startIndex, length, wordNum, ellipsis, isClearTags, isReturnToBR, isLower, isUpper, formatString);
                }
                else if (StringUtils.EqualsIgnoreCase(ForumAttribute.IndexName, type))
                {
                    parsedContent = StringUtils.ParseString(forumInfo.IndexName, replace, to, startIndex, length, wordNum, ellipsis, isClearTags, isReturnToBR, isLower, isUpper, formatString);
                }
                else if (StringUtils.EqualsIgnoreCase(ForumAttribute.Content, type))
                {
                    parsedContent = StringUtils.ParseString(forumInfo.Summary, replace, to, startIndex, length, wordNum, ellipsis, isClearTags, isReturnToBR, isLower, isUpper, formatString);
                }
                else if (StringUtils.EqualsIgnoreCase(ForumAttribute.AddDate, type))
                {
                    parsedContent = DateUtils.Format(forumInfo.AddDate, formatString);
                }
                else if (StringUtils.EqualsIgnoreCase(ForumAttribute.ForumID, type))
                {
                    parsedContent = forumID.ToString();
                }
                else if (StringUtils.EqualsIgnoreCase("moderators", type))
                {
                    StringBuilder builder = new StringBuilder();
                    if (!string.IsNullOrEmpty(forumInfo.Additional.Moderators))
                    {
                        ArrayList userNameArrayList = TranslateUtils.StringCollectionToArrayList(forumInfo.Additional.Moderators);
                        bool isEllipsis = false;
                        if (userNameArrayList.Count > 10)
                        {
                            isEllipsis = true;
                            userNameArrayList = userNameArrayList.GetRange(0, 10);
                        }
                        foreach (string userName in userNameArrayList)
                        {
                            builder.AppendFormat(@"<a href=""{0}"" target=""_blank"">{1}</a>", UserUtils.GetInstance(pageInfo.PublishmentSystemID).GetUserUrl(userName), userName);
                            builder.Append(separator);
                        }

                        builder.Length -= separator.Length;

                        if (isEllipsis)
                        {
                            builder.Append(" ...");
                        }
                    }

                    parsedContent = builder.ToString();
                }
                else if (StringUtils.StartsWithIgnoreCase(type, ParserUtility.ItemIndex) && contextInfo.ItemContainer != null && contextInfo.ItemContainer.ForumItem != null)
                {
                    int itemIndex = ParserUtility.ParseItemIndex(contextInfo.ItemContainer.ForumItem.ItemIndex, type, contextInfo);
                    if (!string.IsNullOrEmpty(formatString))
                    {
                        parsedContent = string.Format(formatString, itemIndex);
                    }
                    else
                    {
                        parsedContent = itemIndex.ToString();
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(ForumAttribute.ChildrenCount, type))
                {
                    parsedContent = forumInfo.ChildrenCount.ToString();
                }
                else if (StringUtils.EqualsIgnoreCase(ForumAttribute.ThreadCount, type))
                {
                    parsedContent = forumInfo.ThreadCount.ToString();
                }
                else if (StringUtils.EqualsIgnoreCase(ForumAttribute.MetaKeywords, type))
                {
                    parsedContent = StringUtils.ParseString(forumInfo.MetaKeywords, replace, to, startIndex, length, wordNum, ellipsis, isClearTags, isReturnToBR, isLower, isUpper, formatString);
                    if (!string.IsNullOrEmpty(parsedContent))
                    {
                        parsedContent = string.Format(@"
<meta name=""keywords"" content=""{0}"" />", parsedContent);
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(ForumAttribute.MetaDescription, type))
                {
                    parsedContent = StringUtils.ParseString(forumInfo.MetaDescription, replace, to, startIndex, length, wordNum, ellipsis, isClearTags, isReturnToBR, isLower, isUpper, formatString);
                    if (!string.IsNullOrEmpty(parsedContent))
                    {
                        parsedContent = string.Format(@"
<meta name=""description"" content=""{0}"" />", parsedContent);
                    }
                }
                else
                {
                    string attributeName = type;                    
                }

                if (!string.IsNullOrEmpty(parsedContent))
                {
                    parsedContent = leftText + parsedContent + rightText;
                }
            }

            return parsedContent;
        }
    }
}
