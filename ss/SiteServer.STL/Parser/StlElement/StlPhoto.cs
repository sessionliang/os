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
	public class StlPhoto
	{
        private StlPhoto() { }
        public const string ElementName = "stl:photo";                      //相册图片

		public const string Attribute_Type = "type";						//显示的类型
        public const string Attribute_LeftText = "lefttext";                    //显示在信息前的文字
        public const string Attribute_RightText = "righttext";                  //显示在信息后的文字
        public const string Attribute_FormatString = "formatstring";        //显示的格式
        public const string Attribute_Separator = "separator";              //显示多项时的分割字符串
        public const string Attribute_StartIndex = "startindex";			//字符开始位置
        public const string Attribute_Length = "length";			        //指定字符长度
		public const string Attribute_WordNum = "wordnum";					//显示字符的数目
        public const string Attribute_Ellipsis = "ellipsis";                //文字超出部分显示的文字
        public const string Attribute_Replace = "replace";                      //需要替换的文字，可以是正则表达式
        public const string Attribute_To = "to";                                //替换replace的文字信息
        public const string Attribute_IsClearTags = "iscleartags";          //是否清除标签信息
        public const string Attribute_IsReturnToBR = "isreturntobr";        //是否将回车替换为HTML换行标签
        public const string Attribute_IsLower = "islower";			        //转换为小写
        public const string Attribute_IsUpper = "isupper";			        //转换为大写
        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

        public const string Type_SmallUrl = "smallUrl";
        public const string Type_MiddleUrl = "middleUrl";
        public const string Type_LargeUrl = "largeUrl";

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
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
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

				while (ie.MoveNext())
				{
					XmlAttribute attr = (XmlAttribute)ie.Current;
					string attributeName = attr.Name.ToLower();

                    if (attributeName.Equals(StlPhoto.Attribute_Type))
					{
						type = attr.Value.ToLower();
					}
                    else if (attributeName.Equals(StlPhoto.Attribute_LeftText))
                    {
                        leftText = attr.Value;
                    }
                    else if (attributeName.Equals(StlPhoto.Attribute_RightText))
                    {
                        rightText = attr.Value;
                    }
                    else if (attributeName.Equals(StlPhoto.Attribute_FormatString))
					{
                        formatString = attr.Value;
                    }
                    else if (attributeName.Equals(StlPhoto.Attribute_Separator))
                    {
                        separator = attr.Value;
                    }
                    else if (attributeName.Equals(StlPhoto.Attribute_StartIndex))
                    {
                        startIndex = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlPhoto.Attribute_Length))
                    {
                        length = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlPhoto.Attribute_WordNum))
                    {
                        wordNum = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlPhoto.Attribute_Ellipsis))
                    {
                        ellipsis = attr.Value;
                    }
                    else if (attributeName.Equals(StlPhoto.Attribute_Replace))
                    {
                        replace = attr.Value;
                    }
                    else if (attributeName.Equals(StlPhoto.Attribute_To))
                    {
                        to = attr.Value;
                    }
                    else if (attributeName.Equals(StlPhoto.Attribute_IsClearTags))
                    {
                        isClearTags = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(StlPhoto.Attribute_IsReturnToBR))
                    {
                        isReturnToBR = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(StlPhoto.Attribute_IsLower))
                    {
                        isLower = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(StlPhoto.Attribute_IsUpper))
                    {
                        isUpper = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(StlPhoto.Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value);
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
                    parsedContent = ParseImpl(node, pageInfo, contextInfo, attributes, leftText, rightText, formatString, separator, startIndex, length, wordNum, ellipsis, replace, to, isClearTags, isReturnToBR, isLower, isUpper, type);
                }
			}
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

			return parsedContent;
		}

        private static string ParseImpl(XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, StringDictionary attributes, string leftText, string rightText, string formatString, string separator, int startIndex, int length, int wordNum, string ellipsis, string replace, string to, bool isClearTags, bool isReturnToBR, bool isLower, bool isUpper, string type)
        {
            string parsedContent = string.Empty;

            if (!string.IsNullOrEmpty(type) && contextInfo.ItemContainer != null && contextInfo.ItemContainer.PhotoItem != null)
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

                if (string.IsNullOrEmpty(type) || StringUtils.EqualsIgnoreCase(type, "imageUrl"))
                {
                    type = StlPhoto.Type_LargeUrl;
                }

                if (StringUtils.StartsWithIgnoreCase(type, StlParserUtility.ItemIndex))
                {
                    int itemIndex = StlParserUtility.ParseItemIndex(contextInfo.ItemContainer.PhotoItem.ItemIndex, type, contextInfo);

                    if (!string.IsNullOrEmpty(formatString))
                    {
                        parsedContent = string.Format(formatString, itemIndex);
                    }
                    else
                    {
                        parsedContent = itemIndex.ToString();
                    }
                }
                else if (StringUtils.StartsWithIgnoreCase(type, StlPhoto.Type_SmallUrl) || StringUtils.StartsWithIgnoreCase(type, StlPhoto.Type_MiddleUrl) || StringUtils.StartsWithIgnoreCase(type, StlPhoto.Type_LargeUrl))
                {
                    parsedContent = DataBinder.Eval(contextInfo.ItemContainer.PhotoItem.DataItem, type, formatString);

                    parsedContent = InputParserUtility.GetImageOrFlashHtml(pageInfo.PublishmentSystemInfo, parsedContent, attributes, false);
                }
                else if (StringUtils.StartsWithIgnoreCase(type, "description") || StringUtils.StartsWithIgnoreCase(type, "content"))
                {
                    parsedContent = DataBinder.Eval(contextInfo.ItemContainer.PhotoItem.DataItem, "description", formatString);

                    parsedContent = StringUtils.ReplaceNewlineToBR(parsedContent);
                }
                else
                {
                    parsedContent = DataBinder.Eval(contextInfo.ItemContainer.PhotoItem.DataItem, type, formatString);
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
