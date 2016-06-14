using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using System;
using SiteServer.BBS.Core.TemplateParser.Model;
using SiteServer.BBS.Model;

namespace SiteServer.BBS.Core.TemplateParser.Element
{
    public class A
	{
		private A(){}
        public const string ElementName = "bbs:a";//获取链接

        public const string Attribute_ID = "id";							//唯一标识符
        public const string Attribute_ForumIndex = "forumindex";		//板块索引
        public const string Attribute_ForumName = "forumname";			//板块名称
        public const string Attribute_Parent = "parent";					//显示父板块
        public const string Attribute_UpLevel = "uplevel";					//上级板块的级别
        public const string Attribute_TopLevel = "toplevel";				//从首页向下的板块级别
        public const string Attribute_IsIndex = "isindex";				    //链接到首页
        public const string Attribute_Context = "context";                  //所处上下文
        public const string Attribute_Href = "href";						//链接地址
        public const string Attribute_QueryString = "querystring";          //链接参数
        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

        public static ListDictionary AttributeList
		{
			get
			{
				ListDictionary attributes = new ListDictionary();
				attributes.Add(Attribute_ID, "唯一标识符");
				attributes.Add(Attribute_ForumIndex, "板块索引");
				attributes.Add(Attribute_ForumName, "板块名称");
				attributes.Add(Attribute_Parent, "显示父板块");
				attributes.Add(Attribute_UpLevel, "上级板块的级别");
                attributes.Add(Attribute_TopLevel, "从首页向下的板块级别");
                attributes.Add(Attribute_IsIndex, "链接到首页");
                attributes.Add(Attribute_Context, "所处上下文");
                attributes.Add(Attribute_Href, "链接地址");
                attributes.Add(Attribute_QueryString, "链接参数");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
				return attributes;
			}
		}

        internal static string Parse(string element, XmlNode node, PageInfo pageInfo, ContextInfo contextInfoRef)
		{
			string parsedContent = string.Empty;
            ContextInfo contextInfo = contextInfoRef.Clone();

			try
			{
                HtmlAnchor stlAnchor = new HtmlAnchor();
				IEnumerator ie = node.Attributes.GetEnumerator();
				string htmlID = string.Empty;
				string forumIndex = string.Empty;
				string forumName = string.Empty;
				int upLevel = 0;
                int topLevel = -1;
                bool isIndex = false;
				bool removeTarget = false;
                string href = string.Empty;
                string queryString = string.Empty;
                bool isDynamic = false;

				while (ie.MoveNext())
				{
					XmlAttribute attr = (XmlAttribute)ie.Current;
					string attributeName = attr.Name.ToLower();
					if (attributeName.Equals(A.Attribute_ID))
					{
						htmlID = attr.Value;
					}
					else if (attributeName.Equals(A.Attribute_ForumIndex))
					{
                        forumIndex = attr.Value;
                        if (!string.IsNullOrEmpty(forumIndex))
						{
                            contextInfo.ContextType = EContextType.Forum;
						}
					}
					else if (attributeName.Equals(A.Attribute_ForumName))
					{
                        forumName = attr.Value;
                        if (!string.IsNullOrEmpty(forumName))
						{
                            contextInfo.ContextType = EContextType.Forum;
						}
					}
					else if (attributeName.Equals(A.Attribute_Parent))
					{
						if (TranslateUtils.ToBool(attr.Value))
						{
							upLevel = 1;
                            contextInfo.ContextType = EContextType.Forum;
						}
					}
					else if (attributeName.Equals(A.Attribute_UpLevel))
					{
						upLevel = TranslateUtils.ToInt(attr.Value);
						if (upLevel > 0)
						{
                            contextInfo.ContextType = EContextType.Forum;
						}
                    }
                    else if (attributeName.Equals(A.Attribute_TopLevel))
                    {
                        topLevel = TranslateUtils.ToInt(attr.Value);
                        if (topLevel >= 0)
                        {
                            contextInfo.ContextType = EContextType.Forum;
                        }
                    }
                    else if (attributeName.Equals(A.Attribute_IsIndex))
                    {
                        isIndex = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(A.Attribute_Context))
                    {
                        contextInfo.ContextType = EContextTypeUtils.GetEnumType(attr.Value);
                    }
                    else if (attributeName.Equals(A.Attribute_Href))
                    {
                        href = attr.Value;
                    }
                    else if (attributeName.Equals(A.Attribute_QueryString))
                    {
                        queryString = attr.Value;
                    }
                    else if (attributeName.Equals(A.Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value, false);
                    }
					else
					{
						ControlUtils.AddAttributeIfNotExists(stlAnchor, attributeName, attr.Value);
					}
				}

                parsedContent = ParseImpl(pageInfo, contextInfo, node, stlAnchor, htmlID, forumIndex, forumName, upLevel, topLevel, isIndex, removeTarget, href, queryString);
			}
            catch (Exception ex)
            {
                parsedContent = ParserUtility.GetErrorMessage(ElementName, ex);
            }

			return parsedContent;
		}

        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, XmlNode node, HtmlAnchor stlAnchor, string htmlID, string forumIndex, string forumName, int upLevel, int topLevel, bool isIndex, bool removeTarget, string href, string queryString)
        {
            string parsedContent = string.Empty;

            if (!string.IsNullOrEmpty(htmlID) && !string.IsNullOrEmpty(contextInfo.ContainerClientID))
            {
                htmlID = contextInfo.ContainerClientID + "_" + htmlID;
            }
            stlAnchor.ID = htmlID;

            string url = string.Empty;
            string onclick = string.Empty;
            if (!string.IsNullOrEmpty(href))
            {
                url = PageUtils.ParseNavigationUrl(href);

                StringBuilder innerBuilder = new StringBuilder(node.InnerXml);
                ParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                stlAnchor.InnerHtml = innerBuilder.ToString();
            }
            else if (isIndex)
            {
                url = PageUtilityBBS.GetIndexPageUrl(pageInfo.PublishmentSystemID);

                if (string.IsNullOrEmpty(node.InnerXml))
                {
                    stlAnchor.InnerHtml = ConfigurationManager.GetAdditional(pageInfo.PublishmentSystemID).BBSName;
                }
                else
                {
                    StringBuilder innerBuilder = new StringBuilder(node.InnerXml);
                    ParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                    stlAnchor.InnerHtml = innerBuilder.ToString();
                }
            }
            else
            {
                if (contextInfo.ContextType == EContextType.Undefined)
                {
                    if (contextInfo.ThreadID != 0)
                    {
                        contextInfo.ContextType = EContextType.Thread;
                    }
                    else
                    {
                        contextInfo.ContextType = EContextType.Forum;
                    }
                }
                if (contextInfo.ContextType == EContextType.Forum)//获取板块Url
                {
                    contextInfo.ForumID = DataUtility.GetForumIDByLevel(pageInfo.PublishmentSystemID, contextInfo.ForumID, upLevel, topLevel);
                    contextInfo.ForumID = DataUtility.GetForumIDByForumIndexOrForumName(pageInfo.PublishmentSystemID, contextInfo.ForumID, forumIndex, forumName);
                    ForumInfo forumInfo = ForumManager.GetForumInfo(pageInfo.PublishmentSystemID, contextInfo.ForumID);

                    url = PageUtilityBBS.GetForumUrl(pageInfo.PublishmentSystemID, forumInfo);
                    if (string.IsNullOrEmpty(node.InnerXml))
                    {
                        stlAnchor.InnerHtml = forumInfo.ForumName;
                    }
                    else
                    {
                        StringBuilder innerBuilder = new StringBuilder(node.InnerXml);
                        ParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                        stlAnchor.InnerHtml = innerBuilder.ToString();
                    }
                }
                else if (contextInfo.ContextType == EContextType.Thread)
                {
                    url = PageUtilityBBS.GetThreadUrl(pageInfo.PublishmentSystemID, contextInfo.ForumID, contextInfo.ThreadID);
                    if (node.InnerXml.Trim().Length == 0 && contextInfo.ThreadInfo != null)
                    {
                        stlAnchor.InnerHtml = contextInfo.ThreadInfo.Title;
                    }
                    else
                    {
                        StringBuilder innerBuilder = new StringBuilder(node.InnerXml);
                        ParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                        stlAnchor.InnerHtml = innerBuilder.ToString();
                    }
                }
            }

            if (url.Equals(PageUtils.UNCLICKED_URL))
            {
                removeTarget = true;
            }
            else if (!string.IsNullOrEmpty(queryString))
            {
                url = PageUtils.AddQueryString(url, queryString);
            }
            stlAnchor.HRef = url;

            if (!string.IsNullOrEmpty(onclick))
            {
                stlAnchor.Attributes.Add("onclick", onclick);
            }

            if (removeTarget)
            {
                stlAnchor.Target = string.Empty;
            }

            parsedContent = ControlUtils.GetControlRenderHtml(stlAnchor);            

            return parsedContent;
        }
	}
}
