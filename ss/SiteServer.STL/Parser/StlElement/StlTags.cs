using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System.Text;
using BaiRong.Controls;
using System.Collections.Generic;
using SiteServer.CMS.Core;
using SiteServer.STL.StlTemplate;

namespace SiteServer.STL.Parser.StlElement
{
	public class StlTags
	{
        private StlTags() { }
        public const string ElementName = "stl:tags";//标签

        public const string Attribute_TagLevel = "taglevel";					        //标签级别
        public const string Attribute_TotalNum = "totalnum";					        //显示标签数目
        public const string Attribute_IsOrderByCount = "isorderbycount";				//是否按引用次数排序
        public const string Attribute_Theme = "theme";			            //主题样式
        public const string Attribute_Context = "context";                  //所处上下文

        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

		public static ListDictionary AttributeList
		{
			get
			{
				ListDictionary attributes = new ListDictionary();

                attributes.Add(Attribute_TagLevel, "标签级别");
                attributes.Add(Attribute_TotalNum, "显示标签数目");
                attributes.Add(Attribute_IsOrderByCount, "是否按引用次数排序");
                attributes.Add(Attribute_Theme, "主题样式");
                attributes.Add(Attribute_Context, "所处上下文");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
				return attributes;
			}
		}

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfoRef)
		{
			string parsedContent = string.Empty;
            ContextInfo contextInfo = contextInfoRef.Clone();
			try
			{
                LowerNameValueCollection attributes = new LowerNameValueCollection();
				IEnumerator ie = node.Attributes.GetEnumerator();

                int tagLevel = 1;
                int totalNum = 0;
                bool isOrderByCount = false;
                string theme = "default";

                bool isDynamic = false;
                bool isInnerXml = !string.IsNullOrEmpty(node.InnerXml);

				while (ie.MoveNext())
				{
					XmlAttribute attr = (XmlAttribute)ie.Current;
					string attributeName = attr.Name.ToLower();
                    
                    if (attributeName.Equals(Attribute_TagLevel))
                    {
                        tagLevel = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_TotalNum))
                    {
                        totalNum = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_IsOrderByCount))
                    {
                        isOrderByCount = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_Theme))
                    {
                        theme = attr.Value;
                    }
                    else if (attributeName.Equals(Attribute_Context))
                    {
                        contextInfo.ContextType = EContextTypeUtils.GetEnumType(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value);
                    }
					else
					{
                        attributes[attributeName] = attr.Value;
					}
				}

                if (isDynamic)
                {
                    parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(stlElement, isInnerXml, pageInfo, contextInfo, tagLevel, totalNum, isOrderByCount, theme);
                }
			}
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

			return parsedContent;
		}

        private static string ParseImpl(string stlElement, bool isInnerXml, PageInfo pageInfo, ContextInfo contextInfo, int tagLevel, int totalNum, bool isOrderByCount, string theme)
        {
            string innerHtml = string.Empty;
            if (isInnerXml)
            {
                innerHtml = StringUtils.StripTags(stlElement, StlTags.ElementName);
            }

            StringBuilder tagsBuilder = new StringBuilder();
            if (!isInnerXml)
            {
                tagsBuilder.AppendFormat(@"
<link rel=""stylesheet"" href=""{0}"" type=""text/css"" />
", StlTemplateManager.Tags.GetStyleUrl(theme));
                tagsBuilder.Append(@"<ul class=""tagCloud"">");
            }

            if (contextInfo.ContextType == EContextType.Undefined)
            {
                if (contextInfo.ContentID != 0)
                {
                    contextInfo.ContextType = EContextType.Content;
                }
                else
                {
                    contextInfo.ContextType = EContextType.Channel;
                }
            }
            int contentID = 0;
            if (contextInfo.ContextType == EContextType.Content)
            {
                contentID = contextInfo.ContentID;
            }

            ArrayList tagInfoArrayList = TagUtils.GetTagInfoArrayList(AppManager.CMS.AppID, pageInfo.PublishmentSystemID, contentID, totalNum, isOrderByCount, tagLevel);
            if (contextInfo.ContextType == EContextType.Content && contextInfo.ContentInfo != null)
            {
                ArrayList tagInfoArrayList2 = new ArrayList();
                List<string> tagNameList = TranslateUtils.StringCollectionToStringList(contextInfo.ContentInfo.Tags.Trim().Replace(" ", ","));
                foreach (string tagName in tagNameList)
                {
                    if (!string.IsNullOrEmpty(tagName))
                    {
                        bool isAdd = false;
                        foreach (TagInfo tagInfo in tagInfoArrayList)
                        {
                            if (tagInfo.Tag == tagName)
                            {
                                isAdd = true;
                                tagInfoArrayList2.Add(tagInfo);
                                break;
                            }
                        }
                        if (!isAdd)
                        {
                            TagInfo tagInfo = new TagInfo(0, AppManager.CMS.AppID, pageInfo.PublishmentSystemID, contentID.ToString(), tagName, 1);
                            tagInfoArrayList2.Add(tagInfo);
                        }
                    }
                }
                tagInfoArrayList = tagInfoArrayList2;
            }

            foreach (TagInfo tagInfo in tagInfoArrayList)
            {
                if (isInnerXml)
                {
                    string tagHtml = innerHtml;
                    tagHtml = StringUtils.ReplaceIgnoreCase(tagHtml, "{Tag.Name}", tagInfo.Tag);
                    tagHtml = StringUtils.ReplaceIgnoreCase(tagHtml, "{Tag.Count}", tagInfo.UseNum.ToString());
                    tagHtml = StringUtils.ReplaceIgnoreCase(tagHtml, "{Tag.Level}", tagInfo.Level.ToString());
                    StringBuilder innerBuilder = new StringBuilder(tagHtml);
                    StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                    tagsBuilder.Append(innerBuilder);
                }
                else
                {
                    tagsBuilder.AppendFormat(@"
<li class=""tag_popularity_{0}""><a target=""_blank"" href=""{1}"">{2}</a></li>
", tagInfo.Level, PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, string.Format("@/utils/tags.html?tagName={0}", PageUtils.UrlEncode(tagInfo.Tag))), tagInfo.Tag);
                }
            }

            if (!isInnerXml)
            {
                tagsBuilder.Append("</ul>");
            }
            return tagsBuilder.ToString();
        }
	}
}
