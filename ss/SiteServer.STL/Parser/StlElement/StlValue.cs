using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using BaiRong.Core;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System;
using BaiRong.Model;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.StlElement
{
	public class StlValue
	{
		private StlValue(){}
		public const string ElementName = "stl:value";//��ȡֵ

		public const string Attribute_Type = "type";		//��Ҫ��ȡֵ������
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

		public const string Type_SiteName = "SiteName";		        //Ƶ������
		public const string Type_SiteUrl = "SiteUrl";			    //Ƶ����������ַ
        public const string Type_Date = "Date";			        //��ǰ����
        public const string Type_DateOfTraditional = "DateOfTraditional";		//��ũ���ĵ�ǰ����

		public static ListDictionary AttributeList
		{
			get
			{
				ListDictionary attributes = new ListDictionary();
				attributes.Add(Attribute_Type, "��Ҫ��ȡֵ������");
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


		//�ԡ�ֵ����stl:value��Ԫ�ؽ��н���
        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
		{
			string parsedContent = string.Empty;
			try
			{
				IEnumerator ie = node.Attributes.GetEnumerator();
                StringDictionary attributes = new StringDictionary();

				string type = string.Empty;
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
					if (attributeName.Equals(StlValue.Attribute_Type))
					{
						type = attr.Value;
                    }
                    else if (attributeName.Equals(StlValue.Attribute_FormatString))
                    {
                        formatString = attr.Value;
                    }
                    else if (attributeName.Equals(StlValue.Attribute_Separator))
                    {
                        separator = attr.Value;
                    }
                    else if (attributeName.Equals(StlValue.Attribute_StartIndex))
                    {
                        startIndex = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlValue.Attribute_Length))
                    {
                        length = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlValue.Attribute_WordNum))
                    {
                        wordNum = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlValue.Attribute_Ellipsis))
                    {
                        ellipsis = attr.Value;
                    }
                    else if (attributeName.Equals(StlValue.Attribute_Replace))
                    {
                        replace = attr.Value;
                    }
                    else if (attributeName.Equals(StlValue.Attribute_To))
                    {
                        to = attr.Value;
                    }
                    else if (attributeName.Equals(StlValue.Attribute_IsClearTags))
                    {
                        isClearTags = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(StlValue.Attribute_IsReturnToBR))
                    {
                        isReturnToBR = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(StlValue.Attribute_IsLower))
                    {
                        isLower = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(StlValue.Attribute_IsUpper))
                    {
                        isUpper = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(StlValue.Attribute_IsDynamic))
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
                    parsedContent = ParseImpl(node, pageInfo, contextInfo, type, attributes, formatString, separator, startIndex, length, wordNum, ellipsis, replace, to, isClearTags, isReturnToBR, isLower, isUpper);
                }
			}
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

			return parsedContent;
		}

        private static string ParseImpl(XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, string type, StringDictionary attributes, string formatString, string separator, int startIndex, int length, int wordNum, string ellipsis, string replace, string to, bool isClearTags, bool isReturnToBR, bool isLower, bool isUpper)
        {
            string parsedContent = string.Empty;

            if (type.Length > 0)
            {
                if (type.ToLower().Equals(StlValue.Type_SiteName.ToLower()))
                {
                    parsedContent = pageInfo.PublishmentSystemInfo.PublishmentSystemName;
                }
                else if (type.ToLower().Equals(StlValue.Type_SiteUrl.ToLower()))
                {
                    parsedContent = pageInfo.PublishmentSystemInfo.PublishmentSystemUrl;
                }
                else if (type.ToLower().Equals(StlValue.Type_Date.ToLower()))
                {
                    pageInfo.AddPageScriptsIfNotExists("datestring.js", string.Format(@"<script charset=""{0}"" src=""{1}"" type=""text/javascript""></script>", SiteFiles.DateString.Charset, PageUtility.GetSiteFilesUrl(pageInfo.PublishmentSystemInfo, SiteFiles.DateString.Js)));

                    parsedContent = string.Format(@"<script language=""javascript"" type=""text/javascript"">RunGLNL(false);</script>");
                }
                else if (type.ToLower().Equals(StlValue.Type_DateOfTraditional.ToLower()))
                {
                    pageInfo.AddPageScriptsIfNotExists("datestring", string.Format(@"<script charset=""{0}"" src=""{1}"" type=""text/javascript""></script>", SiteFiles.DateString.Charset, PageUtility.GetSiteFilesUrl(pageInfo.PublishmentSystemInfo, SiteFiles.DateString.Js)));

                    parsedContent = string.Format(@"<script language=""javascript"" type=""text/javascript"">RunGLNL(true);</script>");
                }
                else
                {
                    if (pageInfo.PublishmentSystemInfo.Additional.Attributes.Get(type) == null)
                    {
                        StlTagInfo stlTagInfo = DataProvider.StlTagDAO.GetStlTagInfo(pageInfo.PublishmentSystemID, type);
                        if (stlTagInfo == null)
                        {
                            stlTagInfo = DataProvider.StlTagDAO.GetStlTagInfo(0, type);
                        }
                        if (stlTagInfo != null && !string.IsNullOrEmpty(stlTagInfo.TagContent))
                        {
                            StringBuilder innerBuilder = new StringBuilder(stlTagInfo.TagContent);
                            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                            parsedContent = innerBuilder.ToString();
                        }
                    }
                    else
                    {
                        parsedContent = pageInfo.PublishmentSystemInfo.Additional.Attributes[type];
                        if (!string.IsNullOrEmpty(parsedContent))
                        {
                            TableStyleInfo styleInfo = TableStyleManager.GetTableStyleInfo(ETableStyle.Site, DataProvider.PublishmentSystemDAO.TableName, type, RelatedIdentities.GetRelatedIdentities(ETableStyle.Site, pageInfo.PublishmentSystemID, pageInfo.PublishmentSystemID));

                            if (isClearTags && (styleInfo.InputType == EInputType.Image || styleInfo.InputType == EInputType.File))
                            {
                                parsedContent = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, parsedContent);
                            }
                            else
                            {
                                parsedContent = InputTypeParser.GetContentByTableStyle(parsedContent, separator, pageInfo.PublishmentSystemInfo, ETableStyle.Site, styleInfo, formatString, attributes, node.InnerXml, false);
                                parsedContent = StringUtils.ParseString(styleInfo.InputType, parsedContent, replace, to, startIndex, length, wordNum, ellipsis, isClearTags, isReturnToBR, isLower, isUpper, formatString);
                            }
                        }
                    }                    
                }
            }

            return parsedContent;
        }
	}
}
