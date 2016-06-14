using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using SiteServer.STL.Parser.StlEntity;
using SiteServer.CMS.Services;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Core;
namespace SiteServer.STL.Parser.StlElement
{
    public class StlAction
    {
        private StlAction() { }
        public const string ElementName = "stl:action";     //执行动作

        public const string Attribute_Type = "type";                        //动作类型
        public const string Attribute_IsLoginFirst = "isloginfirst";        //是否执行动作前需登录
        public const string Attribute_RedirectUrl = "redirecturl";          //转向地址
        public const string Attribute_IsOpenWin = "isopenwin";              //是否新窗口打开转向地址
        public const string Attribute_PageTitle = "pagetitle";              //窗口标题
        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

        public const string Type_Login = "Login";			            //登录
        public const string Type_Register = "Register";			        //注册
        public const string Type_Logout = "Logout";			            //退出
        public const string Type_AddFavorite = "AddFavorite";			//将页面添加至收藏夹
        public const string Type_SetHomePage = "SetHomePage";			//将页面设置为首页
        public const string Type_Translate = "Translate";			    //繁体/简体转换
        public const string Type_Close = "Close";			            //关闭页面
        public const string Type_Comments = "Comments";			        //链接到评论页面
        public const string Type_Share = "Share";                       //bShare分享

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_Type, "动作类型");
                attributes.Add(Attribute_IsLoginFirst, "是否执行动作前需登录");
                attributes.Add(Attribute_RedirectUrl, "转向地址");
                attributes.Add(Attribute_IsOpenWin, "是否新窗口打开转向地址");
                attributes.Add(Attribute_PageTitle, "窗口标题");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
                return attributes;
            }
        }


        //循环解析型标签
        internal static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfoRef)
        {
            string parsedContent = string.Empty;
            ContextInfo contextInfo = contextInfoRef.Clone();

            try
            {
                NameValueCollection attributes = new NameValueCollection();
                IEnumerator ie = node.Attributes.GetEnumerator();
                string type = string.Empty;
                bool isLoginFirst = false;
                string redirectUrl = string.Empty;
                bool isOpenWin = false;
                string pageTitle = string.Empty;
                bool isDynamic = false;

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();

                    if (attributeName.Equals(StlAction.Attribute_Type))
                    {
                        type = attr.Value;
                    }
                    else if (attributeName.Equals(StlAction.Attribute_IsLoginFirst))
                    {
                        isLoginFirst = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(StlAction.Attribute_RedirectUrl))
                    {
                        string value = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                        redirectUrl = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, value);
                    }
                    else if (attributeName.Equals(StlAction.Attribute_IsOpenWin))
                    {
                        isOpenWin = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(StlAction.Attribute_PageTitle))
                    {
                        pageTitle = attr.Value;
                    }
                    else if (attributeName.Equals(StlAction.Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else
                    {
                        attributes.Add(attributeName, attr.Value);
                    }
                }

                if (isDynamic)
                {
                    parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(pageInfo, contextInfo, node, attributes, type, isLoginFirst, redirectUrl, isOpenWin, pageTitle);
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, XmlNode node, NameValueCollection attributes, string type, bool isLoginFirst, string redirectUrl, bool isOpenWin, string pageTitle)
        {
            HtmlAnchor stlAnchor = new HtmlAnchor();

            foreach (string attributeName in attributes.Keys)
            {
                stlAnchor.Attributes.Add(attributeName, attributes[attributeName]);
            }

            string parsedContent = string.Empty;

            string url = PageUtils.UNCLICKED_URL;
            string onclick = string.Empty;

            StringBuilder innerBuilder = new StringBuilder(node.InnerXml);
            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
            stlAnchor.InnerHtml = innerBuilder.ToString();

            //计算动作开始
            if (!string.IsNullOrEmpty(type))
            {
                if (StringUtils.EqualsIgnoreCase(type, StlAction.Type_AddFavorite))
                {
                    pageInfo.SetPageScripts(StlAction.Type_AddFavorite, string.Format(@"
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
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlAction.Type_Login))
                {
                    pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.A_JQuery_1_11_0);
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.A_Platform_BASIC_BUT_JQUERY);

                    string returnUrl = stlAnchor.Attributes["returnUrl"];
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        returnUrl = StlUtility.GetStlCurrentUrl(pageInfo, contextInfo.ChannelID, contextInfo.ContentID, contextInfo.ContentInfo);
                    }
                    string siteUrl = pageInfo.PublishmentSystemInfo.PublishmentSystemUrl.TrimEnd('/');
                    string iframeUrl = PageUtility.GetSiteFilesUrl(pageInfo.PublishmentSystemInfo, string.Format("services/platform/iframe/login.html?publishmentSystemID={0}&siteUrl={1}&returnUrl={2}", pageInfo.PublishmentSystemID, siteUrl, returnUrl));

                    string clickString = string.Format(@"$.layer({{type: 2, title: '', maxmin: false, shadeClose: true, area :['480px', '470px'], offset : ['100px', ''],closeBtn: [0, false], iframe: {{src: '{0}'}} }});", iframeUrl);
                    stlAnchor.HRef = PageUtils.UNCLICKED_URL;
                    stlAnchor.Attributes["onclick"] = clickString;

                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.A_Platform_BASIC + ".LoginPage", @"
<script>
window.onresize = resize;
function resize(){     
var clintW=document.body.clientWidth;
var clintH=document.body.clientHeight;
if(clintW<480){ 
    $('.xubox_layer').width(clintW-13+'px');
    $('.xubox_layer').css('left','245px');
    $('.xubox_layer').css('top','20px');
    $('.xubox_iframe').width(clintW-13+'px');
    $('.xubox_border').width(clintW+'px');
}
else{ 
    $('.xubox_layer').width(480+'px');
    $('.xubox_layer').css('left','50%');
    $('.xubox_layer').css('top','106px');
    $('.xubox_iframe').width(480+'px');
    $('.xubox_border').width(493+'px');
}} </script>");
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlAction.Type_Register))
                {
                    pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.A_JQuery_1_11_0);
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.A_Platform_BASIC_BUT_JQUERY);

                    string returnUrl = stlAnchor.Attributes["returnUrl"];
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        returnUrl = StlUtility.GetStlCurrentUrl(pageInfo, contextInfo.ChannelID, contextInfo.ContentID, contextInfo.ContentInfo);
                    }
                    string siteUrl = pageInfo.PublishmentSystemInfo.PublishmentSystemUrl.TrimEnd('/');
                    string iframeUrl = PageUtility.GetSiteFilesUrl(pageInfo.PublishmentSystemInfo, string.Format("services/platform/iframe/register.html?publishmentSystemID={0}&siteUrl={1}&returnUrl={2}", pageInfo.PublishmentSystemID, siteUrl, returnUrl));

                    string clickString = string.Format(@"$.layer({{type: 2, title: '', maxmin: false, shadeClose: true, area : ['480px' , '490px'], offset : ['100px', ''],closeBtn: [0, false], iframe: {{src: '{0}'}} }});", iframeUrl);
                    stlAnchor.HRef = PageUtils.UNCLICKED_URL;
                    stlAnchor.Attributes["onclick"] = clickString;
                }

                else if (StringUtils.EqualsIgnoreCase(type, StlAction.Type_Logout))
                {
                    pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.A_JQuery_1_11_0);
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.A_Platform_BASIC_BUT_JQUERY);
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.A_Platform_USER);

                    string returnUrl = stlAnchor.Attributes["returnUrl"];
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        returnUrl = StlUtility.GetStlCurrentUrl(pageInfo, contextInfo.ChannelID, contextInfo.ContentID, contextInfo.ContentInfo);
                    }

                    string clickString = string.Format(@"userController.logout('{0}');return false;", returnUrl);
                    stlAnchor.HRef = PageUtils.UNCLICKED_URL;
                    stlAnchor.Attributes["onclick"] = clickString;
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlAction.Type_SetHomePage))
                {
                    url = pageInfo.PublishmentSystemInfo.PublishmentSystemUrl;
                    pageInfo.AddPageEndScriptsIfNotExists(StlAction.Type_AddFavorite, string.Format(@"
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
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlAction.Type_Translate))
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

                    pageInfo.SetPageEndScripts(StlAction.Type_Translate, string.Format(@"
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
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlAction.Type_Close))
                {
                    url = "javascript:window.close()";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlAction.Type_Comments))
                {
                    url = stlAnchor.Attributes["href"];
                    if (string.IsNullOrEmpty(url))
                    {
                        url = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, "@/utils/comments.html");
                    }
                    if (contextInfo.ContextType == EContextType.Content)
                    {
                        NameValueCollection parameters = new NameValueCollection();
                        parameters.Add("channelID", contextInfo.ChannelID.ToString());
                        parameters.Add("contentID", contextInfo.ContentID.ToString());
                        url = PageUtils.AddQueryString(url, parameters);
                    }
                    else if (contextInfo.ContextType == EContextType.Channel)
                    {
                        url = PageUtils.AddQueryString(url, "channelID", contextInfo.ChannelID.ToString());
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlAction.Type_Share))
                {
                    stlAnchor.InnerHtml = contextInfo.PublishmentSystemInfo.Additional.BshareJs;
                }
            }
            //计算动作结束
            stlAnchor.HRef = url;

            if (!string.IsNullOrEmpty(onclick))
            {
                stlAnchor.Attributes.Add("onclick", onclick);
            }

            parsedContent = ControlUtils.GetControlRenderHtml(stlAnchor);

            return parsedContent;
        }
    }
}
