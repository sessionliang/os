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
        public const string ElementName = "stl:inputcontent";               //�ύ����ֵ

		public const string Attribute_Type = "type";						//��ʾ������
        public const string Attribute_LeftText = "lefttext";                //��ʾ����Ϣǰ������
        public const string Attribute_RightText = "righttext";              //��ʾ����Ϣ�������
        public const string Attribute_FormatString = "formatstring";        //��ʾ�ĸ�ʽ
        public const string Attribute_Separator = "separator";              //��ʾ����ʱ�ķָ��ַ���
        public const string Attribute_StartIndex = "startindex";			//�ַ���ʼλ��
        public const string Attribute_Length = "length";			        //ָ���ַ�����
		public const string Attribute_WordNum = "wordnum";					//��ʾ�ַ�����Ŀ
        public const string Attribute_Ellipsis = "ellipsis";                //���ֳ���������ʾ������
        public const string Attribute_Replace = "replace";                  //��Ҫ�滻�����֣�������������ʽ
        public const string Attribute_To = "to";                            //�滻replace��������Ϣ
        public const string Attribute_IsClearTags = "iscleartags";          //�Ƿ������ǩ��Ϣ
        public const string Attribute_IsReturnToBR = "isreturntobr";        //�Ƿ񽫻س��滻ΪHTML���б�ǩ
        public const string Attribute_IsLower = "islower";			        //ת��ΪСд
        public const string Attribute_IsUpper = "isupper";			        //ת��Ϊ��д
        public const string Attribute_IsDynamic = "isdynamic";              //�Ƿ�̬��ʾ

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
				return attributes;
			}
		}


        //�ԡ��������ԡ���stl:comment��Ԫ�ؽ��н���
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
