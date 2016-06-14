using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using BaiRong.Core;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System;
using BaiRong.Model;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.StlElement
{
	public class StlFile
	{
		private StlFile(){}
        public const string ElementName = "stl:file";       //�ļ���������

        public const string Attribute_NO = "no";                            //��ʾ�ֶε�˳��
		public const string Attribute_Src = "src";		                        //��Ҫ���ص��ļ���ַ
        public const string Attribute_IsFilesize = "isfilesize";                //��ʾ�ļ���С
        public const string Attribute_IsCount = "iscount";                      //�Ƿ��¼�ļ����ش���
        public const string Attribute_Type = "type";							//ָ���洢�������ֶ�

        public const string Attribute_LeftText = "lefttext";                //��ʾ����Ϣǰ������
        public const string Attribute_RightText = "righttext";              //��ʾ����Ϣ�������
        public const string Attribute_IsDynamic = "isdynamic";              //�Ƿ�̬��ʾ

		public static ListDictionary AttributeList
		{
			get
			{
				ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_NO, "��ʾ�ֶε�˳��");
                attributes.Add(Attribute_Src, "��Ҫ���ص��ļ���ַ");
                attributes.Add(Attribute_IsFilesize, "��ʾ�ļ���С");
                attributes.Add(Attribute_IsCount, "�Ƿ��¼�ļ����ش���");
                attributes.Add(Attribute_Type, "ָ���洢�������ֶ�");

                attributes.Add(Attribute_LeftText, "��ʾ����Ϣǰ������");
                attributes.Add(Attribute_RightText, "��ʾ����Ϣ�������");
                attributes.Add(Attribute_IsDynamic, "�Ƿ�̬��ʾ");
				return attributes;
			}
		}

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                IEnumerator ie = node.Attributes.GetEnumerator();

                int no = 0;
                string src = string.Empty;
                bool isFilesize = false;
                bool isCount = true;
                string type = BackgroundContentAttribute.FileUrl;
                string leftText = string.Empty;
                string rightText = string.Empty;
                bool isDynamic = false;

                StringDictionary attributes = new StringDictionary();

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlFile.Attribute_NO))
                    {
                        no = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlFile.Attribute_Src))
                    {
                        src = attr.Value;
                    }
                    else if (attributeName.Equals(StlFile.Attribute_IsFilesize))
                    {
                        isFilesize = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(StlFile.Attribute_IsCount))
                    {
                        isCount = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(StlFile.Attribute_Type))
                    {
                        type = attr.Value;
                    }
                    else if (attributeName.Equals(StlFile.Attribute_LeftText))
                    {
                        leftText = attr.Value;
                    }
                    else if (attributeName.Equals(StlFile.Attribute_RightText))
                    {
                        rightText = attr.Value;
                    }
                    else if (attributeName.Equals(StlFile.Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value);
                    }
                    else
                    {
                        attributes.Remove(attributeName);
                        attributes.Add(attributeName, attr.Value);
                    }
                }

                if (isDynamic)
                {
                    parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(node, pageInfo, contextInfo, type, no, src, isFilesize, isCount, leftText, rightText, attributes);
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, string type, int no, string src, bool isFilesize, bool isCount, string leftText, string rightText, StringDictionary attributes)
        {
            string parsedContent = string.Empty;

            if (!string.IsNullOrEmpty(node.InnerXml))
            {
                StringBuilder innerBuilder = new StringBuilder(node.InnerXml);
                StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                node.InnerXml = innerBuilder.ToString();
            }

            string fileUrl = string.Empty;
            if (!string.IsNullOrEmpty(src))
            {
                fileUrl = src;
            }
            else
            {
                if (contextInfo.ContextType == EContextType.Undefined)
                    contextInfo.ContextType = EContextType.Content;
                if (contextInfo.ContextType == EContextType.Content)
                {
                    if (contextInfo.ContentID != 0)
                    {
                        ContentInfo contentInfo = contextInfo.ContentInfo;

                        if (contentInfo != null && !string.IsNullOrEmpty(contentInfo.GetExtendedAttribute(type)))
                        {
                            if (no <= 1)
                            {
                                if (StringUtils.EqualsIgnoreCase(type, BackgroundContentAttribute.FileUrl))
                                {
                                    fileUrl = contentInfo.GetExtendedAttribute(BackgroundContentAttribute.FileUrl);
                                }
                                else
                                {
                                    fileUrl = contentInfo.GetExtendedAttribute(type);
                                }
                            }
                            else
                            {
                                string extendAttributeName = ContentAttribute.GetExtendAttributeName(type);
                                string extendValues = contentInfo.GetExtendedAttribute(extendAttributeName);
                                if (!string.IsNullOrEmpty(extendValues))
                                {
                                    int index = 2;
                                    foreach (string extendValue in TranslateUtils.StringCollectionToArrayList(extendValues))
                                    {
                                        if (index == no)
                                        {
                                            fileUrl = extendValue;

                                            break;
                                        }
                                        index++;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (contextInfo.ContextType == EContextType.Each)
                {
                    fileUrl = contextInfo.ItemContainer.EachItem.DataItem as string;
                }
            }

            parsedContent = InputParserUtility.GetFileHtmlWithoutCount(pageInfo.PublishmentSystemInfo, fileUrl, attributes, node.InnerXml, false);

            if (isFilesize)
            {
                string filePath = PathUtility.MapPath(pageInfo.PublishmentSystemInfo, fileUrl);
                parsedContent += " (" + FileUtils.GetFileSizeByFilePath(filePath) + ")";
            }
            else
            {
                if (isCount && contextInfo.ContentInfo != null)
                {
                    parsedContent = InputParserUtility.GetFileHtmlWithCount(pageInfo.PublishmentSystemInfo, contextInfo.ContentInfo.NodeID, contextInfo.ContentInfo.ID, fileUrl, attributes, node.InnerXml, false);
                }
                else
                {
                    parsedContent = InputParserUtility.GetFileHtmlWithoutCount(pageInfo.PublishmentSystemInfo, fileUrl, attributes, node.InnerXml, false);
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
