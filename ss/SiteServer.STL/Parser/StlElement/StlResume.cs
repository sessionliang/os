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
    public class StlResume
    {
        private StlResume() { }
        public const string ElementName = "stl:resume";//用户登录及状态显示

        public const string Attribute_StyleName = "stylename";              //样式名称

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
                TagStyleResumeInfo resumeInfo = new TagStyleResumeInfo(string.Empty);

                string successTemplateString = string.Empty;
                string failureTemplateString = string.Empty;
                StlParserUtility.GetInnerTemplateString(node, out successTemplateString, out failureTemplateString, pageInfo, contextInfo);

                IEnumerator ie = node.Attributes.GetEnumerator();

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlResume.Attribute_StyleName))
                    {
                        styleName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                }

                parsedContent = ParseImpl(pageInfo, contextInfo, styleName, successTemplateString, failureTemplateString);
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        public static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, string styleName, string successTemplateString, string failureTemplateString)
        {
            string parsedContent = string.Empty;

            pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.A_JQuery);
            pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.B_AjaxUpload);
            pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.B_QueryString);
            pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.B_Validate);
            pageInfo.AddPageScriptsIfNotExists(PageInfo.Js_Inner_Calendar);

            TagStyleInfo styleInfo = TagStyleManager.GetTagStyleInfo(pageInfo.PublishmentSystemID, ElementName, styleName);
            if (styleInfo == null)
            {
                styleInfo = new TagStyleInfo();
            }
            TagStyleResumeInfo resumeInfo = new TagStyleResumeInfo(styleInfo.SettingsXML);

            ResumeTemplate resumeTemplate = new ResumeTemplate(pageInfo.PublishmentSystemInfo, styleInfo, resumeInfo);
            parsedContent = resumeTemplate.GetTemplate(styleInfo.IsTemplate, successTemplateString, failureTemplateString);

            return parsedContent;
        }
    }
}
