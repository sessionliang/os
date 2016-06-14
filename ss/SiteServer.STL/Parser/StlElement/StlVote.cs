using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using BaiRong.Core.Data.Provider;
using System.Text.RegularExpressions;
using System;

using System.Collections.Generic;
using SiteServer.CMS.Core;
using SiteServer.STL.StlTemplate;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlVote
    {
        private StlVote() { }
        public const string ElementName = "stl:vote";

        public const string Attribute_Theme = "theme";                      //主题样式

        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

        public const string Theme_Style1 = "Style1";
        public const string Theme_Style2 = "Style2";

        public static List<string> AttributeValuesTheme
        {
            get
            {
                List<string> list = new List<string>();
                list.Add(Theme_Style1);
                list.Add(Theme_Style2);
                return list;
            }
        }

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_Theme, "主题样式");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
                return attributes;
            }
        }

        public sealed class InputTemplate
        {
            public const string ElementName = "stl:inputtemplate";
        }

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                string theme = string.Empty;
                bool isDynamic = false;

                string inputTemplateString = string.Empty;
                string successTemplateString = string.Empty;
                string failureTemplateString = string.Empty;
                StlParserUtility.GetInnerTemplateStringOfInput(node, out inputTemplateString, out successTemplateString, out failureTemplateString, pageInfo, contextInfo);

                IEnumerator ie = node.Attributes.GetEnumerator();

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlVote.Attribute_Theme))
                    {
                        theme = attr.Value;
                    }
                    else if (attributeName.Equals(StlVote.Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value);
                    }
                }

                pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.A_JQuery_1_11_0);
                pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.B_JTemplates);
                pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.B_ShowLoading);
                pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.B_Validate);

                pageInfo.AddPageScriptsIfNotExists("SiteServer.STL.Parser.StlElement", string.Format(@"<link href=""{0}"" type=""text/css"" rel=""stylesheet"" />
", StlTemplateManager.Vote.StyleUrl));

                if (isDynamic)
                {
                    parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(pageInfo, contextInfo, theme, inputTemplateString);
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, string theme, string inputTemplateString)
        {
            string parsedContent = string.Empty;


//            pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.A_JQuery_1_11_0);
//            pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.B_JTemplates);
//            pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.B_ShowLoading);
//            pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.B_Validate);

//            pageInfo.AddPageScriptsIfNotExists("SiteServer.STL.Parser.StlElement", string.Format(@"<link href=""{0}"" type=""text/css"" rel=""stylesheet"" />
//", StlTemplateManager.Vote.StyleUrl));

            VoteContentInfo contentInfo = contextInfo.ContentInfo as VoteContentInfo;
            if (contentInfo == null && contextInfo.ContentID > 0)
            {
                contentInfo = DataProvider.VoteContentDAO.GetContentInfo(pageInfo.PublishmentSystemInfo, contextInfo.ContentID);
            }

            if (contentInfo != null)
            {
                TagStyleInfo styleInfo = new TagStyleInfo();
                TagStyleVoteInfo voteInfo = new TagStyleVoteInfo(styleInfo.SettingsXML);

                VoteTemplate voteTemplate = new VoteTemplate(pageInfo.PublishmentSystemInfo, contentInfo.NodeID, contentInfo, styleInfo, voteInfo);
                StringBuilder contentBuilder = new StringBuilder(voteTemplate.GetTemplate(styleInfo.IsTemplate, inputTemplateString));

                StlParserManager.ParseTemplateContent(contentBuilder, pageInfo, contextInfo);
                parsedContent = contentBuilder.ToString();
            }

            return parsedContent;
        }
    }
}
