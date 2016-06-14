using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using BaiRong.Core;
using BaiRong.Core.IO;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System;
using SiteServer.STL.IO;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.StlElement
{
	public class StlInclude
	{
		private StlInclude(){}
		public const string ElementName = "stl:include";//�����ļ�

		public const string Attribute_File = "file";	//�ļ�·��
        public const string Attribute_IsContext = "iscontext";            //�Ƿ�STL��ǩ���������
        public const string Attribute_IsDynamic = "isdynamic";              //�Ƿ�̬��ʾ

		public static ListDictionary AttributeList
		{
			get
			{
				ListDictionary attributes = new ListDictionary();
				attributes.Add(Attribute_File, "�ļ�·��");
                attributes.Add(Attribute_IsContext, "�Ƿ�STL�����뵱ǰҳ�����");
                attributes.Add(Attribute_IsDynamic, "�Ƿ�̬��ʾ");
				return attributes;
			}
		}

		//�ԡ������ļ�����stl:include��Ԫ�ؽ��н���
        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
		{
            string parsedContent = string.Empty;
			try
			{
				IEnumerator ie = node.Attributes.GetEnumerator();
				string file = string.Empty;
                bool isContext = !pageInfo.PublishmentSystemInfo.Additional.IsCreateIncludeToSSI;
                bool isDynamic = false;

				while (ie.MoveNext())
				{
					XmlAttribute attr = (XmlAttribute)ie.Current;
					string attributeName = attr.Name.ToLower();
					if (attributeName.Equals(StlInclude.Attribute_File))
					{
                        file = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                        file = PageUtility.AddVirtualToUrl(file);
                    }
                    else if (attributeName.Equals(StlInclude.Attribute_IsContext))
                    {
                        isContext = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(StlInclude.Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value);
                    }
				}

                if (isDynamic)
                {
                    parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(pageInfo, contextInfo, file, isContext);
                }
			}
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
		}

        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, string file, bool isContext)
        {
            string parsedContent = string.Empty;

            if (!string.IsNullOrEmpty(file))
            {
                if (pageInfo.VisualType == EVisualType.Static && !isContext)
                {
                    FileSystemObject FSO = new FileSystemObject(pageInfo.PublishmentSystemID);
                    string parsedFile = FSO.CreateIncludeFile(file, false);

                    if (pageInfo.PublishmentSystemInfo.Additional.IsCreateIncludeToSSI)
                    {
                        string pathDifference = PathUtils.GetPathDifference(PathUtils.MapPath("~/"), PathUtility.MapPath(pageInfo.PublishmentSystemInfo, parsedFile));
                        string virtualUrl = pathDifference.Replace("\\", "/").Trim('/');
                        parsedContent = string.Format(@"<!--#include virtual=""{0}""-->", virtualUrl);
                    }
                    else
                    {
                        string filePath = PathUtility.MapPath(pageInfo.PublishmentSystemInfo, parsedFile);
                        parsedContent = FileUtils.ReadText(filePath, pageInfo.TemplateInfo.Charset);
                    }
                }
                else
                {
                    string content = CreateCacheManager.FileContent.GetIncludeContent(pageInfo.PublishmentSystemInfo, file, pageInfo.TemplateInfo.Charset);
                    content = StlParserUtility.Amp(content);
                    StringBuilder contentBuilder = new StringBuilder(content);
                    StlParserManager.ParseTemplateContent(contentBuilder, pageInfo, contextInfo);
                    parsedContent = contentBuilder.ToString();
                }
            }

            return parsedContent;
        }
	}
}
