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
    public class Navigation
	{
        private Navigation() { }
        public const string ElementName = "bbs:navigation";//显示导航

        public const string Attribute_Type = "type";							//导航的类型
        public const string Attribute_EmptyText = "emptytext";					//当无内容时显示的信息
        public const string Attribute_TipText = "tiptext";					    //导航提示信息
        public const string Attribute_WordNum = "wordnum";					    //显示字数
        public const string Attribute_IsDisplayIfEmpty = "isdisplayifempty";    //当没链接时是否显示
        public const string Attribute_IsDynamic = "isdynamic";                  //是否动态显示

        public const string Type_PreviousForum = "PreviousForum";			//上一栏目链接
        public const string Type_NextForum = "NextForum";					//下一栏目链接
        public const string Type_PreviousThread = "PreviousThread";			//上一内容链接
        public const string Type_NextThread = "NextThread";					//下一内容链接

        public static ListDictionary AttributeList
		{
			get
			{
				ListDictionary attributes = new ListDictionary();
				attributes.Add(Attribute_Type, "显示的类型");
				attributes.Add(Attribute_EmptyText, "当无内容时显示的信息");
                attributes.Add(Attribute_TipText, "导航提示信息");
                attributes.Add(Attribute_WordNum, "显示字数");
                attributes.Add(Attribute_IsDisplayIfEmpty, "当没链接时是否显示");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
				return attributes;
			}
		}

        public sealed class SuccessTemplate
        {
            const string ElementName = "bbs:successtemplate";
        }

        public sealed class FailureTemplate
        {
            const string ElementName = "bbs:failuretemplate";
        }

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfoRef)
		{
			string parsedContent = string.Empty;
            ContextInfo contextInfo = contextInfoRef.Clone();
			try
			{
				HtmlAnchor stlAnchor = new HtmlAnchor();
				IEnumerator ie = node.Attributes.GetEnumerator();
				string type = Type_NextThread;
				string emptyText = string.Empty;
                string tipText = string.Empty;
                int wordNum = 0;
                bool isDisplayIfEmpty = false;
                bool isDynamic = false;

				while (ie.MoveNext())
				{
					XmlAttribute attr = (XmlAttribute)ie.Current;
					string attributeName = attr.Name.ToLower();
					if (attributeName.Equals(Attribute_Type))
					{
						type = attr.Value;
					}
					else if (attributeName.Equals(Attribute_EmptyText))
					{
						emptyText = attr.Value;
					}
                    else if (attributeName.Equals(Attribute_TipText))
                    {
                        tipText = attr.Value;
                    }
                    else if (attributeName.Equals(Attribute_WordNum))
                    {
                        wordNum = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_IsDisplayIfEmpty))
                    {
                        isDisplayIfEmpty = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value);
                    }
					else
					{
						ControlUtils.AddAttributeIfNotExists(stlAnchor, attributeName, attr.Value);
					}
				}

                if (isDynamic)
                {
                    parsedContent = Dynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(node, pageInfo, contextInfo, stlAnchor, type, emptyText, tipText, wordNum, isDisplayIfEmpty);
                }
			}
            catch (Exception ex)
            {
                parsedContent = ParserUtility.GetErrorMessage(ElementName, ex);
            }
			
			return parsedContent;
		}

        private static string ParseImpl(XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, HtmlAnchor stlAnchor, string type, string emptyText, string tipText, int wordNum, bool isDisplayIfEmpty)
        {
            string parsedContent = string.Empty;

            string successTemplateString = string.Empty;
            string failureTemplateString = string.Empty;

            ParserUtility.GetInnerTemplateString(node, out successTemplateString, out failureTemplateString);

            if (string.IsNullOrEmpty(successTemplateString))
            {
                ForumInfo forumInfo = ForumManager.GetForumInfo(pageInfo.PublishmentSystemID, contextInfo.ForumID);

                if (type.ToLower().Equals(Type_PreviousForum.ToLower()) || type.ToLower().Equals(Type_NextForum.ToLower()))
                {
                    int taxis = forumInfo.Taxis;
                    bool isNextForum = true;
                    if (StringUtils.EqualsIgnoreCase(type, Type_PreviousForum))
                    {
                        isNextForum = false;
                    }
                    int siblingForumID = DataProvider.ForumDAO.GetForumIDByParentIDAndTaxis(forumInfo.PublishmentSystemID, forumInfo.ParentID, taxis, isNextForum);
                    if (siblingForumID != 0)
                    {
                        ForumInfo siblingForumInfo = ForumManager.GetForumInfo(pageInfo.PublishmentSystemID, siblingForumID);
                        string url = PageUtilityBBS.GetForumUrl(pageInfo.PublishmentSystemID, siblingForumInfo);
                        if (url.Equals(PageUtils.UNCLICKED_URL))
                        {
                            stlAnchor.Target = string.Empty;
                        }
                        stlAnchor.HRef = url;

                        if (string.IsNullOrEmpty(node.InnerXml))
                        {
                            stlAnchor.InnerHtml = siblingForumInfo.ForumName;
                            if (wordNum > 0)
                            {
                                stlAnchor.InnerHtml = StringUtils.MaxLengthText(stlAnchor.InnerHtml, wordNum);
                            }
                        }
                        else
                        {
                            contextInfo.ForumID = siblingForumID;
                            StringBuilder innerBuilder = new StringBuilder(node.InnerXml);
                            ParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                            stlAnchor.InnerHtml = innerBuilder.ToString();
                        }
                    }
                }
                else if (type.ToLower().Equals(Type_PreviousThread.ToLower()) || type.ToLower().Equals(Type_NextThread.ToLower()))
                {
                    if (contextInfo.ThreadID != 0)
                    {
                        int taxis = contextInfo.ThreadInfo.Taxis;
                        bool isNextThread = true;
                        if (StringUtils.EqualsIgnoreCase(type, Type_PreviousThread))
                        {
                            isNextThread = false;
                        }
                        int siblingThreadID = DataProvider.ThreadDAO.GetThreadID(pageInfo.PublishmentSystemID, contextInfo.ForumID, taxis, isNextThread);
                        if (siblingThreadID != 0)
                        {
                            string url = PageUtilityBBS.GetThreadUrl(pageInfo.PublishmentSystemID, contextInfo.ForumID, siblingThreadID);
                            if (url.Equals(PageUtils.UNCLICKED_URL))
                            {
                                stlAnchor.Target = string.Empty;
                            }
                            stlAnchor.HRef = url;

                            if (string.IsNullOrEmpty(node.InnerXml))
                            {
                                stlAnchor.InnerHtml = DataProvider.ThreadDAO.GetValue(siblingThreadID, ThreadAttribute.Title);
                                if (wordNum > 0)
                                {
                                    stlAnchor.InnerHtml = StringUtils.MaxLengthText(stlAnchor.InnerHtml, wordNum);
                                }
                            }
                            else
                            {
                                StringBuilder innerBuilder = new StringBuilder(node.InnerXml);
                                contextInfo.ThreadID = siblingThreadID;
                                ParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                                stlAnchor.InnerHtml = innerBuilder.ToString();
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(stlAnchor.HRef))
                {
                    if (isDisplayIfEmpty)
                    {
                        if (!string.IsNullOrEmpty(node.InnerXml))
                        {
                            StringBuilder innerBuilder = new StringBuilder(node.InnerXml);
                            ParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                            parsedContent = innerBuilder.ToString();
                        }
                        else
                        {
                            parsedContent = emptyText;
                        }
                    }
                    else
                    {
                        parsedContent = emptyText;
                    }
                }
                else
                {
                    parsedContent = ControlUtils.GetControlRenderHtml(stlAnchor);
                }
            }
            else
            {
                ForumInfo forumInfo = ForumManager.GetForumInfo(pageInfo.PublishmentSystemID, contextInfo.ForumID);

                bool isSuccess = false;
                ContextInfo theContextInfo = contextInfo.Clone();

                if (type.ToLower().Equals(Type_PreviousForum.ToLower()) || type.ToLower().Equals(Type_NextForum.ToLower()))
                {
                    int taxis = forumInfo.Taxis;
                    bool isNextForum = true;
                    if (StringUtils.EqualsIgnoreCase(type, Type_PreviousForum))
                    {
                        isNextForum = false;
                    }
                    int siblingForumID = DataProvider.ForumDAO.GetForumIDByParentIDAndTaxis(pageInfo.PublishmentSystemID, forumInfo.ParentID, taxis, isNextForum);
                    if (siblingForumID != 0)
                    {
                        isSuccess = true;
                        theContextInfo.ContextType = EContextType.Forum;
                        theContextInfo.ForumID = siblingForumID;
                    }
                }
                else if (type.ToLower().Equals(Type_PreviousThread.ToLower()) || type.ToLower().Equals(Type_NextThread.ToLower()))
                {
                    if (contextInfo.ThreadID != 0)
                    {
                        int taxis = contextInfo.ThreadInfo.Taxis;
                        bool isNextThread = true;
                        if (StringUtils.EqualsIgnoreCase(type, Type_PreviousThread))
                        {
                            isNextThread = false;
                        }
                        int siblingThreadID = DataProvider.ThreadDAO.GetThreadID(pageInfo.PublishmentSystemID, contextInfo.ForumID, taxis, isNextThread);
                        if (siblingThreadID != 0)
                        {
                            isSuccess = true;
                            theContextInfo.ContextType = EContextType.Thread;
                            theContextInfo.ThreadID = siblingThreadID;
                            theContextInfo.ThreadInfo = null;
                        }
                    }
                }

                if (isSuccess)
                {
                    parsedContent = successTemplateString;
                }
                else
                {
                    parsedContent = failureTemplateString;
                }

                if (!string.IsNullOrEmpty(parsedContent))
                {
                    StringBuilder innerBuilder = new StringBuilder(parsedContent);
                    ParserManager.ParseInnerContent(innerBuilder, pageInfo, theContextInfo);

                    parsedContent = innerBuilder.ToString();
                }
            }
            
            parsedContent = tipText + parsedContent;

            return parsedContent;
        }
	}
}
