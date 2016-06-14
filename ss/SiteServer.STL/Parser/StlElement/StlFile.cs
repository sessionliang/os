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
        public const string ElementName = "stl:file";       //文件下载链接

        public const string Attribute_NO = "no";                            //显示字段的顺序
		public const string Attribute_Src = "src";		                        //需要下载的文件地址
        public const string Attribute_IsFilesize = "isfilesize";                //显示文件大小
        public const string Attribute_IsCount = "iscount";                      //是否记录文件下载次数
        public const string Attribute_Type = "type";							//指定存储附件的字段

        public const string Attribute_LeftText = "lefttext";                //显示在信息前的文字
        public const string Attribute_RightText = "righttext";              //显示在信息后的文字
        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

		public static ListDictionary AttributeList
		{
			get
			{
				ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_NO, "显示字段的顺序");
                attributes.Add(Attribute_Src, "需要下载的文件地址");
                attributes.Add(Attribute_IsFilesize, "显示文件大小");
                attributes.Add(Attribute_IsCount, "是否记录文件下载次数");
                attributes.Add(Attribute_Type, "指定存储附件的字段");

                attributes.Add(Attribute_LeftText, "显示在信息前的文字");
                attributes.Add(Attribute_RightText, "显示在信息后的文字");
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
