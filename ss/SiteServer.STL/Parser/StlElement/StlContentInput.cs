using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System.Text.RegularExpressions;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CMS.Core;
using SiteServer.STL.StlTemplate;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlContentInput
	{
        private StlContentInput() { }
        public const string ElementName = "stl:contentinput";

        public const string Attribute_StyleName = "stylename";		            //样式名称

        public const string Attribute_ChannelIndex = "channelindex";			//栏目索引
        public const string Attribute_ChannelName = "channelname";				//栏目名称
        public const string Attribute_UpLevel = "uplevel";						//上级栏目的级别
        public const string Attribute_TopLevel = "toplevel";					//从首页向下的栏目级别

		public static ListDictionary AttributeList
		{
			get
			{
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_StyleName, "样式名称");
                attributes.Add(Attribute_ChannelIndex, "栏目索引");
                attributes.Add(Attribute_ChannelName, "栏目名称");
                attributes.Add(Attribute_UpLevel, "上级栏目的级别");
                attributes.Add(Attribute_TopLevel, "从首页向下的栏目级别");
                return attributes;
			}
		}

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
		{
			string parsedContent = string.Empty;
            try
            {
                string styleName = string.Empty;
                string channelIndex = string.Empty;
                string channelName = string.Empty;
                int upLevel = 0;
                int topLevel = 0;

                string inputTemplateString = string.Empty;
                string successTemplateString = string.Empty;
                string failureTemplateString = string.Empty;
                StlParserUtility.GetInnerTemplateStringOfInput(node, out inputTemplateString, out successTemplateString, out failureTemplateString, pageInfo, contextInfo);

                IEnumerator ie = node.Attributes.GetEnumerator();

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlContentInput.Attribute_StyleName))
                    {
                        styleName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(StlContentInput.Attribute_ChannelIndex))
                    {
                        channelIndex = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(StlContentInput.Attribute_ChannelName))
                    {
                        channelName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(StlContentInput.Attribute_UpLevel))
                    {
                        upLevel = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlContentInput.Attribute_TopLevel))
                    {
                        topLevel = TranslateUtils.ToInt(attr.Value);
                    }
                }

                parsedContent = ParseImpl(pageInfo, contextInfo, styleName, channelIndex, channelName, upLevel, topLevel, inputTemplateString, successTemplateString, failureTemplateString);
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

			return parsedContent;
		}

        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, string styleName, string channelIndex, string channelName, int upLevel, int topLevel, string inputTemplateString, string successTemplateString, string failureTemplateString)
        {
            string parsedContent = string.Empty;
            pageInfo.AddPageHeadScriptsIfNotExists(PageInfo.JQuery.A_JQuery);

            TagStyleInfo styleInfo = TagStyleManager.GetTagStyleInfo(pageInfo.PublishmentSystemID, ElementName, styleName);

            if (styleInfo == null)
            {
                styleInfo = new TagStyleInfo();
            }

            TagStyleContentInputInfo inputInfo = new TagStyleContentInputInfo(styleInfo.SettingsXML);

            int channelID = 0;
            if (string.IsNullOrEmpty(channelIndex) && string.IsNullOrEmpty(channelName) && upLevel == 0 && topLevel == 0)
            {
                channelID = inputInfo.ChannelID;
            }
            else
            {
                channelID = StlDataUtility.GetNodeIDByLevel(pageInfo.PublishmentSystemID, contextInfo.ChannelID, upLevel, topLevel);
                channelID = CreateCacheManager.NodeID.GetNodeIDByChannelIDOrChannelIndexOrChannelName(pageInfo.PublishmentSystemID, channelID, channelIndex, channelName);
            }

            ContentInputTemplate inputTemplate = new ContentInputTemplate(pageInfo.PublishmentSystemInfo, styleInfo, inputInfo);
            parsedContent = inputTemplate.GetTemplate(styleInfo.IsTemplate, channelID, inputTemplateString, successTemplateString, failureTemplateString);

            StringBuilder innerBuilder = new StringBuilder(parsedContent);
            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
            return innerBuilder.ToString();
        }
	}
}
