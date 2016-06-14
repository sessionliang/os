using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System;
using SiteServer.STL.Parser.StlEntity;
using System.Web;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlA
    {
        private StlA() { }
        public const string ElementName = "stl:a";//获取链接

        public const string Attribute_ID = "id";							//唯一标识符
        public const string Attribute_ChannelIndex = "channelindex";		//栏目索引
        public const string Attribute_ChannelName = "channelname";			//栏目名称
        public const string Attribute_Parent = "parent";					//显示父栏目
        public const string Attribute_UpLevel = "uplevel";					//上级栏目的级别
        public const string Attribute_TopLevel = "toplevel";				//从首页向下的栏目级别
        public const string Attribute_Context = "context";                  //所处上下文
        public const string Attribute_Href = "href";						//链接地址
        public const string Attribute_QueryString = "querystring";          //链接参数
        public const string Attribute_Action = "action";                    //点击链接的动作
        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

        public const string Action_AddFavorite = "AddFavorite";			//将页面添加至收藏夹
        public const string Action_SetHomePage = "SetHomePage";			//将页面设置为首页
        public const string Action_Translate = "Translate";			    //繁体/简体转换
        public const string Action_Close = "Close";			            //关闭页面
        public const string Action_Comments = "Comments";			    //链接到评论页面

        public const string Attribute_Host = "host";  //链接域名

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_ID, "唯一标识符");
                attributes.Add(Attribute_ChannelIndex, "栏目索引");
                attributes.Add(Attribute_ChannelName, "栏目名称");
                attributes.Add(Attribute_Parent, "显示父栏目");
                attributes.Add(Attribute_UpLevel, "上级栏目的级别");
                attributes.Add(Attribute_TopLevel, "从首页向下的栏目级别");
                attributes.Add(Attribute_Context, "所处上下文");
                attributes.Add(Attribute_Href, "链接地址");
                attributes.Add(Attribute_QueryString, "链接参数");
                attributes.Add(Attribute_Action, "点击链接的动作");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
                return attributes;
            }
        }

        internal static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfoRef)
        {
            string parsedContent = string.Empty;
            ContextInfo contextInfo = contextInfoRef.Clone();

            try
            {
                HtmlAnchor stlAnchor = new HtmlAnchor();
                IEnumerator ie = node.Attributes.GetEnumerator();
                string htmlID = string.Empty;
                string channelIndex = string.Empty;
                string channelName = string.Empty;
                int upLevel = 0;
                int topLevel = -1;
                bool removeTarget = false;
                string href = string.Empty;
                string queryString = string.Empty;
                string action = string.Empty;
                bool isDynamic = false;
                string host = string.Empty;

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlA.Attribute_ID))
                    {
                        htmlID = attr.Value;
                    }
                    else if (attributeName.Equals(StlA.Attribute_ChannelIndex))
                    {
                        channelIndex = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                        if (!string.IsNullOrEmpty(channelIndex))
                        {
                            contextInfo.ContextType = EContextType.Channel;
                        }
                    }
                    else if (attributeName.Equals(StlA.Attribute_ChannelName))
                    {
                        channelName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                        if (!string.IsNullOrEmpty(channelName))
                        {
                            contextInfo.ContextType = EContextType.Channel;
                        }
                    }
                    else if (attributeName.Equals(StlA.Attribute_Parent))
                    {
                        if (TranslateUtils.ToBool(attr.Value))
                        {
                            upLevel = 1;
                            contextInfo.ContextType = EContextType.Channel;
                        }
                    }
                    else if (attributeName.Equals(StlA.Attribute_UpLevel))
                    {
                        upLevel = TranslateUtils.ToInt(attr.Value);
                        if (upLevel > 0)
                        {
                            contextInfo.ContextType = EContextType.Channel;
                        }
                    }
                    else if (attributeName.Equals(StlA.Attribute_TopLevel))
                    {
                        topLevel = TranslateUtils.ToInt(attr.Value);
                        if (topLevel >= 0)
                        {
                            contextInfo.ContextType = EContextType.Channel;
                        }
                    }
                    else if (attributeName.Equals(StlA.Attribute_Context))
                    {
                        contextInfo.ContextType = EContextTypeUtils.GetEnumType(attr.Value);
                    }
                    else if (attributeName.Equals(StlA.Attribute_Href))
                    {
                        href = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(StlA.Attribute_QueryString))
                    {
                        queryString = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(StlA.Attribute_Action))
                    {
                        action = attr.Value;
                    }
                    else if (attributeName.Equals(StlA.Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(StlA.Attribute_Host))
                    {
                        host = attr.Value;
                    }
                    else
                    {
                        ControlUtils.AddAttributeIfNotExists(stlAnchor, attributeName, attr.Value);
                    }
                }

                if (isDynamic)
                {
                    parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(pageInfo, contextInfo, node, stlAnchor, htmlID, channelIndex, channelName, upLevel, topLevel, removeTarget, href, queryString, action, host);
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, XmlNode node, HtmlAnchor stlAnchor, string htmlID, string channelIndex, string channelName, int upLevel, int topLevel, bool removeTarget, string href, string queryString, string action, string host)
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
                url = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, href);

                StringBuilder innerBuilder = new StringBuilder(node.InnerXml);
                StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                stlAnchor.InnerHtml = innerBuilder.ToString();
            }
            else
            {
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
                if (contextInfo.ContextType == EContextType.Content)//获取内容Url
                {
                    if (contextInfo.ContentInfo != null)
                    {
                        url = PageUtility.GetContentUrl(pageInfo.PublishmentSystemInfo, contextInfo.ContentInfo, pageInfo.VisualType);
                    }
                    else
                    {
                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contextInfo.ChannelID);
                        url = PageUtility.GetContentUrl(pageInfo.PublishmentSystemInfo, nodeInfo, contextInfo.ContentID, pageInfo.VisualType);
                    }
                    if (string.IsNullOrEmpty(node.InnerXml))
                    {
                        string title = StringUtils.MaxLengthText(contextInfo.ContentInfo.Title, contextInfo.TitleWordNum);
                        title = ContentUtility.FormatTitle(contextInfo.ContentInfo.Attributes[BackgroundContentAttribute.TitleFormatString], title);

                        if (pageInfo.PublishmentSystemInfo.Additional.IsContentTitleBreakLine)
                        {
                            title = title.Replace("  ", string.Empty);
                        }

                        stlAnchor.InnerHtml = title;
                    }
                    else
                    {
                        StringBuilder innerBuilder = new StringBuilder(node.InnerXml);
                        StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                        stlAnchor.InnerHtml = innerBuilder.ToString();
                    }
                }
                else if (contextInfo.ContextType == EContextType.Channel)//获取栏目Url
                {
                    contextInfo.ChannelID = StlDataUtility.GetNodeIDByLevel(pageInfo.PublishmentSystemID, contextInfo.ChannelID, upLevel, topLevel);
                    contextInfo.ChannelID = CreateCacheManager.NodeID.GetNodeIDByChannelIDOrChannelIndexOrChannelName(pageInfo.PublishmentSystemID, contextInfo.ChannelID, channelIndex, channelName);
                    NodeInfo channel = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contextInfo.ChannelID);

                    url = PageUtility.GetChannelUrl(pageInfo.PublishmentSystemInfo, channel, pageInfo.VisualType);
                    if (node.InnerXml.Trim().Length == 0)
                    {
                        stlAnchor.InnerHtml = channel.NodeName;
                    }
                    else
                    {
                        StringBuilder innerBuilder = new StringBuilder(node.InnerXml);
                        StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                        stlAnchor.InnerHtml = innerBuilder.ToString();
                    }
                }
            }

            //计算动作开始
            if (!string.IsNullOrEmpty(action))
            {
                if (StringUtils.EqualsIgnoreCase(action, StlA.Action_AddFavorite))
                {
                    //string title = "document.title";
                    //if (!string.IsNullOrEmpty(stlAnchor.Attributes["title"]))
                    //{
                    //    title = string.Format("'{0}'", stlAnchor.Attributes["title"]);
                    //}
                    //url = string.Format("javascript:window.external.AddFavorite('{0}','{1}')", url, title);

                    pageInfo.SetPageScripts(StlA.Action_AddFavorite, string.Format(@"
<script type=""text/javascript""> 
    function AddFavorite(){{  
        if (document.all) {{
            window.external.addFavorite(window.location.href, document.title);
        }} 
        else if (window.sidebar) {{
            window.sidebar.addPanel(document.title, window.location.href, """");
        }}
    }}
</script>
"), true);
                    stlAnchor.Attributes["onclick"] = string.Format("AddFavorite();");
                    url = PageUtils.UNCLICKED_URL;
                    removeTarget = true;
                }
                else if (StringUtils.EqualsIgnoreCase(action, StlA.Action_SetHomePage))
                {
                    //if (pageInfo.PublishmentSystemInfo.IsRelatedUrl)
                    //{
                    //    //stlAnchor.Attributes["onclick"] = string.Format("this.style.behavior='url(#default#homepage)';this.setHomePage('{0}')", url);
                    //    url = PageUtility.GetIndexPageUrl(pageInfo.PublishmentSystemInfo, 0, pageInfo.PublishmentSystemInfo.Additional.VisualType);
                    //    url = System.Uri.UriSchemeHttp + System.Uri.SchemeDelimiter + HttpContext.Current.Request.Url.Authority + url;
                    //}
                    //else
                    //{
                    url = pageInfo.PublishmentSystemInfo.PublishmentSystemUrl;
                    //}
                    pageInfo.AddPageEndScriptsIfNotExists(StlA.Action_AddFavorite, string.Format(@"
<script type=""text/javascript""> 
    function SetHomepage(){{   
        if (document.all) {{
            document.body.style.behavior = 'url(#default#homepage)';
            document.body.setHomePage(""{0}"");
        }}
        else if (window.sidebar) {{
            if (window.netscape) {{
                try {{
                    netscape.security.PrivilegeManager.enablePrivilege(""UniversalXPConnect"");
                 }}
                catch(e) {{
                    alert(""该操作被浏览器拒绝，如果想启用该功能，请在地址栏内输入 about:config,然后将项 signed.applets.codebase_principal_support 值该为true"");
                }}
             }}
            var prefs = Components.classes['@mozilla.org/preferences-service;1'].getService(Components.interfaces.nsIPrefBranch);
            prefs.setCharPref('browser.startup.homepage', ""{0}"");
        }}
    }}
</script>
", url));
                    stlAnchor.Attributes["onclick"] = string.Format("SetHomepage();");
                    url = PageUtils.UNCLICKED_URL;
                    removeTarget = true;
                }
                else if (StringUtils.EqualsIgnoreCase(action, StlA.Action_Translate))
                {
                    pageInfo.AddPageScriptsIfNotExists(PageInfo.Js_Ah_Translate);

                    string msgToTraditionalChinese = "繁體";
                    string msgToSimplifiedChinese = "简体";
                    if (!string.IsNullOrEmpty(stlAnchor.InnerHtml))
                    {
                        if (stlAnchor.InnerHtml.IndexOf(",") != -1)
                        {
                            msgToTraditionalChinese = stlAnchor.InnerHtml.Substring(0, stlAnchor.InnerHtml.IndexOf(","));
                            msgToSimplifiedChinese = stlAnchor.InnerHtml.Substring(stlAnchor.InnerHtml.IndexOf(",") + 1);
                        }
                        else
                        {
                            msgToTraditionalChinese = stlAnchor.InnerHtml;
                        }
                    }
                    stlAnchor.InnerHtml = msgToTraditionalChinese;

                    if (string.IsNullOrEmpty(stlAnchor.ID))
                    {
                        stlAnchor.ID = "translateLink";
                    }

                    pageInfo.SetPageEndScripts(StlA.Action_Translate, string.Format(@"
<script type=""text/javascript""> 
var defaultEncoding = 0;
var translateDelay = 0;
var cookieDomain = ""/"";
var msgToTraditionalChinese = ""{0}"";
var msgToSimplifiedChinese = ""{1}"";
var translateButtonId = ""{2}"";
translateInitilization();
</script>
", msgToTraditionalChinese, msgToSimplifiedChinese, stlAnchor.ClientID));
                    url = PageUtils.UNCLICKED_URL;
                    removeTarget = true;
                }
                else if (StringUtils.EqualsIgnoreCase(action, StlA.Action_Close))
                {
                    url = "javascript:window.close()";
                    removeTarget = true;
                }
                else if (StringUtils.EqualsIgnoreCase(action, StlA.Action_Comments))
                {
                    url = href;
                    if (string.IsNullOrEmpty(url))
                    {
                        url = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, "@/utils/comments.html");
                    }
                    if (contextInfo.ContextType == EContextType.Content)
                    {
                        NameValueCollection attributes = new NameValueCollection();
                        attributes.Add("channelID", contextInfo.ChannelID.ToString());
                        attributes.Add("contentID", contextInfo.ContentID.ToString());
                        url = PageUtils.AddQueryString(url, attributes);
                    }
                    else if (contextInfo.ContextType == EContextType.Channel)
                    {
                        url = PageUtils.AddQueryString(url, "channelID", contextInfo.ChannelID.ToString());
                    }
                }
            }
            //计算动作结束

            if (url.Equals(PageUtils.UNCLICKED_URL))
            {
                removeTarget = true;
            }
            else if (!string.IsNullOrEmpty(queryString))
            {
                url = PageUtils.AddQueryString(url, queryString);
            }

            //如果host不为空，那么添加host
            if (!string.IsNullOrEmpty(host))
            {
                url = PageUtils.AddProtocolToUrl(url, host);
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
