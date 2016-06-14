using System;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.StlElement
{
	public class StlInputContent
	{
        private StlInputContent() { }
        public const string ElementName = "stl:inputcontent";               //提交内容值

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
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
				return attributes;
			}
		}


        //对“评论属性”（stl:comment）元素进行解析
        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
		{
			string parsedContent = string.Empty;
            if (contextInfo.ItemContainer == null || contextInfo.ItemContainer.InputItem == null) return string.Empty;
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
                    if (attributeName.Equals(StlInputContent.Attribute_Type))
					{
						type = attr.Value.ToLower();
					}
                    else if (attributeName.Equals(StlInputContent.Attribute_LeftText))
                    {
                        leftText = attr.Value;
                    }
                    else if (attributeName.Equals(StlInputContent.Attribute_RightText))
                    {
                        rightText = attr.Value;
                    }
                    else if (attributeName.Equals(StlInputContent.Attribute_FormatString))
					{
                        formatString = attr.Value;
                    }
                    else if (attributeName.Equals(StlInputContent.Attribute_Separator))
                    {
                        separator = attr.Value;
                    }
                    else if (attributeName.Equals(StlInputContent.Attribute_StartIndex))
                    {
                        startIndex = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlInputContent.Attribute_Length))
                    {
                        length = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlInputContent.Attribute_WordNum))
                    {
                        wordNum = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlInputContent.Attribute_Ellipsis))
                    {
                        ellipsis = attr.Value;
                    }
                    else if (attributeName.Equals(StlInputContent.Attribute_Replace))
                    {
                        replace = attr.Value;
                    }
                    else if (attributeName.Equals(StlInputContent.Attribute_To))
                    {
                        to = attr.Value;
                    }
                    else if (attributeName.Equals(StlInputContent.Attribute_IsClearTags))
                    {
                        isClearTags = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(StlInputContent.Attribute_IsReturnToBR))
                    {
                        isReturnToBR = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(StlInputContent.Attribute_IsLower))
                    {
                        isLower = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(StlInputContent.Attribute_IsUpper))
                    {
                        isUpper = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(StlInputContent.Attribute_IsDynamic))
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

            if (InputContentAttribute.ID.ToLower().Equals(type))
            {
                parsedContent = TranslateUtils.EvalString(contextInfo.ItemContainer.InputItem.DataItem, InputContentAttribute.ID);
            }
            else if (InputContentAttribute.AddDate.ToLower().Equals(type))
            {
                DateTime addDate = TranslateUtils.EvalDateTime(contextInfo.ItemContainer.InputItem.DataItem, InputContentAttribute.AddDate);
                parsedContent = DateUtils.Format(addDate, formatString);
            }
            else if (InputContentAttribute.UserName.ToLower().Equals(type))
            {
                parsedContent = TranslateUtils.EvalString(contextInfo.ItemContainer.InputItem.DataItem, InputContentAttribute.UserName);
            }
            else if (InputContentAttribute.Reply.ToLower().Equals(type))
            {
                string content = TranslateUtils.EvalString(contextInfo.ItemContainer.InputItem.DataItem, InputContentAttribute.Reply);
                parsedContent = TranslateUtils.ParseEmotionHtml(content, false);
                parsedContent = StringUtils.ParseString(EInputType.TextEditor, parsedContent, replace, to, startIndex, length, wordNum, ellipsis, isClearTags, isReturnToBR, isLower, isUpper, formatString);
            }
            else if (StringUtils.StartsWithIgnoreCase(type, StlParserUtility.ItemIndex))
            {
                parsedContent = StlParserUtility.ParseItemIndex(contextInfo.ItemContainer.InputItem.ItemIndex, type, contextInfo).ToString();
            }
            else
            {
                int id = TranslateUtils.EvalInt(contextInfo.ItemContainer.InputItem.DataItem, InputContentAttribute.ID);
                InputContentInfo inputContentInfo = DataProvider.InputContentDAO.GetContentInfo(id);
                if (inputContentInfo != null)
                {
                    parsedContent = inputContentInfo.GetExtendedAttribute(type);
                    if (!string.IsNullOrEmpty(parsedContent))
                    {
                        if (!InputContentAttribute.HiddenAttributes.Contains(type.ToLower()))
                        {
                            TableStyleInfo styleInfo = TableStyleManager.GetTableStyleInfo(ETableStyle.InputContent, DataProvider.InputContentDAO.TableName, type, RelatedIdentities.GetRelatedIdentities(ETableStyle.InputContent, pageInfo.PublishmentSystemID, inputContentInfo.InputID));
                            parsedContent = InputTypeParser.GetContentByTableStyle(parsedContent, separator, pageInfo.PublishmentSystemInfo, ETableStyle.InputContent, styleInfo, formatString, attributes, node.InnerXml, false);
                            parsedContent = StringUtils.ParseString(styleInfo.InputType, parsedContent, replace, to, startIndex, length, wordNum, ellipsis, isClearTags, isReturnToBR, isLower, isUpper, formatString);
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(parsedContent))
            {
                parsedContent = leftText + parsedContent + rightText;
            }

            return parsedContent;
        }
	}
}
