using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using BaiRong.Core;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System.Text.RegularExpressions;
using BaiRong.Model;
using System;
using SiteServer.CMS.Core;
using SiteServer.STL.StlTemplate;

namespace SiteServer.STL.Parser.StlElement
{
	public class StlGovPublicQuery
	{
        private StlGovPublicQuery() { }
        public const string ElementName = "stl:govpublicquery";     //依申请公开查询

        public const string Attribute_StyleName = "stylename";		        //样式名称

		public static ListDictionary AttributeList
		{
			get
			{
				ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_StyleName, "样式名称");

				return attributes;
			}
		}

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                string styleName = string.Empty;

                string inputTemplateString = string.Empty;
                string successTemplateString = string.Empty;
                string failureTemplateString = string.Empty;
                StlParserUtility.GetInnerTemplateStringOfInput(node, out inputTemplateString, out successTemplateString, out failureTemplateString, pageInfo, contextInfo);

                IEnumerator ie = node.Attributes.GetEnumerator();

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlGovPublicQuery.Attribute_StyleName))
                    {
                        styleName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                }

                parsedContent = ParseImpl(pageInfo, contextInfo, styleName, inputTemplateString, successTemplateString, failureTemplateString);
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        public static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, string styleName, string inputTemplateString, string successTemplateString, string failureTemplateString)
        {
            string parsedContent = string.Empty;

            pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.A_JQuery);
            pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.B_ShowLoading);
            pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.B_JTemplates);
            pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.B_Validate);

            TagStyleInfo styleInfo = TagStyleManager.GetTagStyleInfo(pageInfo.PublishmentSystemID, ElementName, styleName);
            if (styleInfo == null)
            {
                styleInfo = new TagStyleInfo();
            }

            GovPublicQueryTemplate queryTemplate = new GovPublicQueryTemplate(pageInfo.PublishmentSystemInfo, styleInfo);
            StringBuilder contentBuilder = new StringBuilder(queryTemplate.GetTemplate(styleInfo.IsTemplate, inputTemplateString, successTemplateString, failureTemplateString));

            StlParserManager.ParseTemplateContent(contentBuilder, pageInfo, contextInfo);
            parsedContent = contentBuilder.ToString();

            return parsedContent;
        }
	}
}
