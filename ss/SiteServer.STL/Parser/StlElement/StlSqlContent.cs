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

namespace SiteServer.STL.Parser.StlElement
{
	public class StlSqlContent
	{
        private StlSqlContent() { }
        public const string ElementName = "stl:sqlcontent";//��ȡ����

        public const string Attribute_ConnectionStringName = "connectionstringname";        //���ݿ������ַ�������
        public const string Attribute_ConnectionString = "connectionstring";	            //���ݿ������ַ���
        public const string Attribute_QueryString = "querystring";

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

		public static ListDictionary AttributeList
		{
			get
			{
				ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_ConnectionStringName, "���ݿ������ַ�������");
                attributes.Add(Attribute_ConnectionString, "���ݿ������ַ���");
                attributes.Add(Attribute_QueryString, "���ݿ��ѯ���");

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


        //�ԡ���ȡ���ݡ���stl:sqlcontent��Ԫ�ؽ��н���
        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
		{
			string parsedContent = string.Empty;
			try
			{
                IEnumerator ie = node.Attributes.GetEnumerator();
				StringDictionary attributes = new StringDictionary();
                string connectionString = string.Empty;
                string queryString = string.Empty;

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

                    if (attributeName.Equals(StlSqlContent.Attribute_ConnectionString))
                    {
                        connectionString = attr.Value;
                    }
                    else if (attributeName.Equals(StlSqlContent.Attribute_ConnectionStringName))
                    {
                        if (string.IsNullOrEmpty(connectionString))
                        {
                            connectionString = ConfigUtils.Instance.GetAppSettings(attr.Value);
                        }
                    }
                    else if (attributeName.Equals(StlSqlContent.Attribute_QueryString))
                    {
                        queryString = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(StlSqlContent.Attribute_Type))
					{
						type = attr.Value.ToLower();
					}
                    else if (attributeName.Equals(StlSqlContent.Attribute_LeftText))
                    {
                        leftText = attr.Value;
                    }
                    else if (attributeName.Equals(StlSqlContent.Attribute_RightText))
                    {
                        rightText = attr.Value;
                    }
                    else if (attributeName.Equals(StlSqlContent.Attribute_FormatString))
					{
                        formatString = attr.Value;
                    }
                    else if (attributeName.Equals(StlSqlContent.Attribute_Separator))
                    {
                        separator = attr.Value;
                    }
                    else if (attributeName.Equals(StlSqlContent.Attribute_StartIndex))
                    {
                        startIndex = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlSqlContent.Attribute_Length))
                    {
                        length = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlSqlContent.Attribute_WordNum))
                    {
                        wordNum = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlSqlContent.Attribute_Ellipsis))
                    {
                        ellipsis = attr.Value;
                    }
                    else if (attributeName.Equals(StlSqlContent.Attribute_Replace))
                    {
                        replace = attr.Value;
                    }
                    else if (attributeName.Equals(StlSqlContent.Attribute_To))
                    {
                        to = attr.Value;
                    }
                    else if (attributeName.Equals(StlSqlContent.Attribute_IsClearTags))
                    {
                        isClearTags = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(StlSqlContent.Attribute_IsReturnToBR))
                    {
                        isReturnToBR = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(StlSqlContent.Attribute_IsLower))
                    {
                        isLower = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(StlSqlContent.Attribute_IsUpper))
                    {
                        isUpper = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(StlSqlContent.Attribute_IsDynamic))
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
                    parsedContent = ParseImpl(node, pageInfo, contextInfo, attributes, connectionString, queryString, leftText, rightText, formatString, separator, startIndex, length, wordNum, ellipsis, replace, to, isClearTags, isReturnToBR, isLower, isUpper, type);
                }
			}
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

			return parsedContent;
		}

        private static string ParseImpl(XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, StringDictionary attributes, string connectionString, string queryString, string leftText, string rightText, string formatString, string separator, int startIndex, int length, int wordNum, string ellipsis, string replace, string to, bool isClearTags, bool isReturnToBR, bool isLower, bool isUpper, string type)
        {
            string parsedContent = string.Empty;

            if (!string.IsNullOrEmpty(type) && contextInfo.ItemContainer != null && contextInfo.ItemContainer.SqlItem != null)
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
                    int itemIndex = StlParserUtility.ParseItemIndex(contextInfo.ItemContainer.SqlItem.ItemIndex, type, contextInfo);

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
                    parsedContent = DataBinder.Eval(contextInfo.ItemContainer.SqlItem.DataItem, type, formatString);
                }
            }
            else if (!string.IsNullOrEmpty(queryString))
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = BaiRongDataProvider.ConnectionString;
                }

                parsedContent = BaiRongDataProvider.DatabaseDAO.GetString(connectionString, queryString);
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
