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
	public class StlComment
	{
        private StlComment() { }
        public const string ElementName = "stl:comment";//��������

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
        public static string Parse(string stlElement, XmlNode node, SiteServer.STL.Parser.Model.PageInfo pageInfo, ContextInfo contextInfo)
		{
			string parsedContent = string.Empty;
            if (contextInfo.ItemContainer == null || contextInfo.ItemContainer.CommentItem == null) return string.Empty;
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
                string type = CommentAttribute.Content.ToLower();
                bool isDynamic = false;

				while (ie.MoveNext())
				{
					XmlAttribute attr = (XmlAttribute)ie.Current;
					string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlComment.Attribute_Type))
					{
						type = attr.Value.ToLower();
					}
                    else if (attributeName.Equals(StlComment.Attribute_LeftText))
                    {
                        leftText = attr.Value;
                    }
                    else if (attributeName.Equals(StlComment.Attribute_RightText))
                    {
                        rightText = attr.Value;
                    }
                    else if (attributeName.Equals(StlComment.Attribute_FormatString))
					{
                        formatString = attr.Value;
                    }
                    else if (attributeName.Equals(StlComment.Attribute_Separator))
                    {
                        separator = attr.Value;
                    }
                    else if (attributeName.Equals(StlComment.Attribute_StartIndex))
                    {
                        startIndex = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlComment.Attribute_Length))
                    {
                        length = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlComment.Attribute_WordNum))
                    {
                        wordNum = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlComment.Attribute_Ellipsis))
                    {
                        ellipsis = attr.Value;
                    }
                    else if (attributeName.Equals(StlComment.Attribute_Replace))
                    {
                        replace = attr.Value;
                    }
                    else if (attributeName.Equals(StlComment.Attribute_To))
                    {
                        to = attr.Value;
                    }
                    else if (attributeName.Equals(StlComment.Attribute_IsClearTags))
                    {
                        isClearTags = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(StlComment.Attribute_IsReturnToBR))
                    {
                        isReturnToBR = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(StlComment.Attribute_IsLower))
                    {
                        isLower = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(StlComment.Attribute_IsUpper))
                    {
                        isUpper = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(StlComment.Attribute_IsDynamic))
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

            int commentID = TranslateUtils.EvalInt(contextInfo.ItemContainer.CommentItem, CommentAttribute.CommentID);
            int nodeID = TranslateUtils.EvalInt(contextInfo.ItemContainer.CommentItem, CommentAttribute.NodeID);
            int contentID = TranslateUtils.EvalInt(contextInfo.ItemContainer.CommentItem, CommentAttribute.ContentID);
            int good = TranslateUtils.EvalInt(contextInfo.ItemContainer.CommentItem, CommentAttribute.Good);
            string userName = TranslateUtils.EvalString(contextInfo.ItemContainer.CommentItem, CommentAttribute.UserName);
            bool isChecked = TranslateUtils.ToBool(TranslateUtils.EvalString(contextInfo.ItemContainer.CommentItem, CommentAttribute.IsChecked));
            bool isRecommend = TranslateUtils.ToBool(TranslateUtils.EvalString(contextInfo.ItemContainer.CommentItem, CommentAttribute.IsRecommend));
            string ipAddress = TranslateUtils.EvalString(contextInfo.ItemContainer.CommentItem, CommentAttribute.IPAddress);
            DateTime addDate = TranslateUtils.EvalDateTime(contextInfo.ItemContainer.CommentItem, CommentAttribute.AddDate);
            string content = TranslateUtils.EvalString(contextInfo.ItemContainer.CommentItem, CommentAttribute.Content);

            if (CommentAttribute.CommentID.ToLower().Equals(type))
            {
                parsedContent = commentID.ToString();
            }
            else if (CommentAttribute.AddDate.ToLower().Equals(type))
            {
                parsedContent = DateUtils.Format(addDate, formatString);
            }
            else if (CommentAttribute.UserName.ToLower().Equals(type))
            {
                parsedContent = string.IsNullOrEmpty(userName) ? "����" : userName;
            }
            else if (CommentAttribute.IPAddress.ToLower().Equals(type))
            {
                parsedContent = ipAddress;
                if (!string.IsNullOrEmpty(parsedContent))
                {
                    if (parsedContent.LastIndexOf('.') != -1)
                    {
                        parsedContent = parsedContent.Substring(0, parsedContent.LastIndexOf('.') + 1) + "*";
                    }
                }
            }
            else if (CommentAttribute.Good.ToLower().Equals(type))
            {
                parsedContent = string.Format("<span id='commentsDigg_{0}_{1}'>{2}</span>", commentID, true.ToString(), good);
            }
            else if (CommentAttribute.Content.ToLower().Equals(type))
            {
                parsedContent = StringUtils.ParseString(parsedContent, replace, to, startIndex, length, wordNum, ellipsis, isClearTags, isReturnToBR, isLower, isUpper, formatString);
                parsedContent = TranslateUtils.ParseCommentContent(content);
            }
            else if (StringUtils.StartsWithIgnoreCase(type, StlParserUtility.ItemIndex))
            {
                parsedContent = StlParserUtility.ParseItemIndex(contextInfo.ItemContainer.CommentItem.ItemIndex, type, contextInfo).ToString();
            }

            if (!string.IsNullOrEmpty(parsedContent))
            {
                parsedContent = leftText + parsedContent + rightText;
            }

            return parsedContent;
        }
	}
}
