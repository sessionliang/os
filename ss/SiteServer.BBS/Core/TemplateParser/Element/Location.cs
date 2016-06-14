using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Xml;
using BaiRong.Core;
using System;
using SiteServer.BBS.Core.TemplateParser.Model;
using SiteServer.BBS.Model;

namespace SiteServer.BBS.Core.TemplateParser.Element
{
    public class Location
	{
        private Location() { }
        public const string ElementName = "bbs:location";//显示位置

        public const string Attribute_Separator = "separator";				//当前位置分隔符
        public const string Attribute_Target = "target";					//打开窗口的目标
        public const string Attribute_LinkClass = "linkclass";				//链接CSS样式
        public const string Attribute_WordNum = "wordnum";                  //链接字数
        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

        public static ListDictionary AttributeList
		{
			get
			{
				ListDictionary attributes = new ListDictionary();
				attributes.Add(Attribute_Separator, "当前位置分隔符");
				attributes.Add(Attribute_Target, "打开窗口的目标");
				attributes.Add(Attribute_LinkClass, "链接CSS样式");
                attributes.Add(Attribute_WordNum, "链接字数");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
				return attributes;
			}
		}


		//对“当前位置”（stl:location）元素进行解析
        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
		{
			string parsedContent = string.Empty;
			try
			{
				IEnumerator ie = node.Attributes.GetEnumerator();
				string separator = " - ";
				string target = string.Empty;
				string linkClass = string.Empty;
                int wordNum = 0;
                bool isDynamic = false;

				StringDictionary attributes = new StringDictionary();

				while (ie.MoveNext())
				{
					XmlAttribute attr = (XmlAttribute)ie.Current;
					string attributeName = attr.Name.ToLower();
					if (attributeName.Equals(Attribute_Separator))
					{
						separator = attr.Value;
					}
					else if (attributeName.Equals(Attribute_Target))
					{
						target = attr.Value;
					}
					else if (attributeName.Equals(Attribute_LinkClass))
					{
						linkClass = attr.Value;
                    }
                    else if (attributeName.Equals(Attribute_WordNum))
                    {
                        wordNum = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value);
                    }
					else
					{
						attributes.Add(attributeName, attr.Value);
					}
				}

                if (isDynamic)
                {
                    parsedContent = Dynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(node, pageInfo, contextInfo, separator, target, linkClass, wordNum, attributes);
                }
			}
            catch (Exception ex)
            {
                parsedContent = ParserUtility.GetErrorMessage(ElementName, ex);
            }
			
			return parsedContent;
		}

        private static string ParseImpl(XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, string separator, string target, string linkClass, int wordNum, StringDictionary attributes)
        {
            if (!string.IsNullOrEmpty(node.InnerXml))
            {
                separator = node.InnerXml;
            }

            ForumInfo forumInfo = ForumManager.GetForumInfo(pageInfo.PublishmentSystemID, contextInfo.ForumID);

            StringBuilder builder = new StringBuilder();

            string parentsPath = forumInfo.ParentsPath;
            int parentsCount = forumInfo.ParentsCount;
            if (parentsPath.Length != 0)
            {
                string forumPath = parentsPath + "," + contextInfo.ForumID;
                ArrayList forumIDArrayList = TranslateUtils.StringCollectionToArrayList(forumPath);

                ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(pageInfo.PublishmentSystemID);

                foreach (string forumIDStr in forumIDArrayList)
                {
                    int currentID = int.Parse(forumIDStr);
                    if (currentID == 0)
                    {
                        HtmlAnchor stlAnchor = new HtmlAnchor();
                        if (!string.IsNullOrEmpty(target))
                        {
                            stlAnchor.Target = target;
                        }
                        if (!string.IsNullOrEmpty(linkClass))
                        {
                            stlAnchor.Attributes.Add("class", linkClass);
                        }
                        string url = PageUtilityBBS.GetBBSUrl(pageInfo.PublishmentSystemID, string.Empty);
                        if (url.Equals(PageUtils.UNCLICKED_URL))
                        {
                            stlAnchor.Target = string.Empty;
                        }
                        stlAnchor.HRef = url;
                        stlAnchor.InnerHtml = StringUtils.MaxLengthText(additional.BBSName, wordNum);

                        ControlUtils.AddAttributesIfNotExists(stlAnchor, attributes);

                        builder.Append(ControlUtils.GetControlRenderHtml(stlAnchor));

                        if (parentsCount > 0)
                        {
                            builder.Append(separator);
                        }
                    }
                    else if (currentID == contextInfo.ForumID)
                    {
                        ForumInfo currentForumInfo = ForumManager.GetForumInfo(pageInfo.PublishmentSystemID, currentID);
                        HtmlAnchor stlAnchor = new HtmlAnchor();
                        if (!string.IsNullOrEmpty(target))
                        {
                            stlAnchor.Target = target;
                        }
                        if (!string.IsNullOrEmpty(linkClass))
                        {
                            stlAnchor.Attributes.Add("class", linkClass);
                        }
                        string url = PageUtilityBBS.GetForumUrl(pageInfo.PublishmentSystemID, currentForumInfo);
                        if (url.Equals(PageUtils.UNCLICKED_URL))
                        {
                            stlAnchor.Target = string.Empty;
                        }
                        stlAnchor.HRef = url;
                        stlAnchor.InnerHtml = StringUtils.MaxLengthText(currentForumInfo.ForumName, wordNum);

                        ControlUtils.AddAttributesIfNotExists(stlAnchor, attributes);

                        builder.Append(ControlUtils.GetControlRenderHtml(stlAnchor));
                    }
                    else
                    {
                        ForumInfo currentForumInfo = ForumManager.GetForumInfo(pageInfo.PublishmentSystemID, currentID);
                        HtmlAnchor stlAnchor = new HtmlAnchor();
                        if (!string.IsNullOrEmpty(target))
                        {
                            stlAnchor.Target = target;
                        }
                        if (!string.IsNullOrEmpty(linkClass))
                        {
                            stlAnchor.Attributes.Add("class", linkClass);
                        }
                        string url = PageUtilityBBS.GetForumUrl(pageInfo.PublishmentSystemID, currentForumInfo);
                        if (url.Equals(PageUtils.UNCLICKED_URL))
                        {
                            stlAnchor.Target = string.Empty;
                        }
                        stlAnchor.HRef = url;
                        stlAnchor.InnerHtml = StringUtils.MaxLengthText(currentForumInfo.ForumName, wordNum);

                        ControlUtils.AddAttributesIfNotExists(stlAnchor, attributes);

                        builder.Append(ControlUtils.GetControlRenderHtml(stlAnchor));

                        if (parentsCount > 0)
                        {
                            builder.Append(separator);
                        }
                    }
                }
            }

            return builder.ToString();
        }
	}
}
