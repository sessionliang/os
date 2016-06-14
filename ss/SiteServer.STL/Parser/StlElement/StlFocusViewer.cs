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
using System.Collections.Generic;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlFocusViewer
    {
        private StlFocusViewer() { }
        public const string ElementName = "stl:focusviewer";//显示滚动焦点图

        public const string Attribute_ChannelIndex = "channelindex";			//栏目索引
        public const string Attribute_ChannelName = "channelname";				//栏目名称
        public const string Attribute_Scope = "scope";							//范围
        public const string Attribute_Group = "group";		                    //指定显示的内容组
        public const string Attribute_GroupNot = "groupnot";	                //指定不显示的内容组
        public const string Attribute_GroupChannel = "groupchannel";		    //指定显示的栏目组
        public const string Attribute_GroupChannelNot = "groupchannelnot";	    //指定不显示的栏目组
        public const string Attribute_GroupContent = "groupcontent";		    //指定显示的内容组
        public const string Attribute_GroupContentNot = "groupcontentnot";	    //指定不显示的内容组
        public const string Attribute_Tags = "tags";	                        //指定标签
        public const string Attribute_Order = "order";							//排序
        public const string Attribute_StartNum = "startnum";					//从第几条信息开始显示
        public const string Attribute_TotalNum = "totalnum";					//显示图片数目
        public const string Attribute_TitleWordNum = "titlewordnum";			//标题文字数量
        public const string Attribute_Where = "where";                          //获取滚动焦点图的条件判断

        public const string Attribute_IsTop = "istop";                       //仅显示置顶内容
        public const string Attribute_IsRecommend = "isrecommend";           //仅显示推荐内容
        public const string Attribute_IsHot = "ishot";                       //仅显示热点内容
        public const string Attribute_IsColor = "iscolor";                   //仅显示醒目内容

        public const string Attribute_Theme = "theme";                      //主题样式
        public const string Attribute_Width = "width";						//图片宽度
        public const string Attribute_Height = "height";					//图片高度
        public const string Attribute_BgColor = "bgcolor";					//背景色
        public const string Attribute_IsShowText = "isshowtext";			//是否显示文字标题
        public const string Attribute_IsTopText = "istoptext";				//是否文字显示在顶端
        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

        public const string Theme_Style1 = "Style1";
        public const string Theme_Style2 = "Style2";
        public const string Theme_Style3 = "Style3";
        public const string Theme_Style4 = "Style4";

        public static List<string> AttributeValuesTheme
        {
            get
            {
                List<string> list = new List<string>();

                list.Add(Theme_Style1);
                list.Add(Theme_Style2);
                list.Add(Theme_Style3);
                list.Add(Theme_Style4);

                return list;
            }
        }

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_ChannelIndex, "栏目索引");
                attributes.Add(Attribute_ChannelName, "栏目名称");
                attributes.Add(Attribute_Scope, "范围");
                attributes.Add(Attribute_GroupChannel, "指定显示的栏目组");
                attributes.Add(Attribute_GroupChannelNot, "指定不显示的栏目组");
                attributes.Add(Attribute_GroupContent, "指定显示的内容组");
                attributes.Add(Attribute_GroupContentNot, "指定不显示的内容组");
                attributes.Add(Attribute_Tags, "指定标签");
                attributes.Add(Attribute_Order, "排序");
                attributes.Add(Attribute_StartNum, "从第几条信息开始显示");
                attributes.Add(Attribute_TotalNum, "标题文字数量");
                attributes.Add(Attribute_TitleWordNum, "标题文字数量");
                attributes.Add(Attribute_Where, "获取滚动焦点图的条件判断");

                attributes.Add(Attribute_IsTop, "仅显示置顶内容");
                attributes.Add(Attribute_IsRecommend, "仅显示推荐内容");
                attributes.Add(Attribute_IsHot, "仅显示热点内容");
                attributes.Add(Attribute_IsColor, "仅显示醒目内容");

                attributes.Add(Attribute_Theme, "主题样式");
                attributes.Add(Attribute_Width, "图片宽度");
                attributes.Add(Attribute_Height, "图片高度");
                attributes.Add(Attribute_BgColor, "背景色");
                attributes.Add(Attribute_IsShowText, "是否显示文字标题");
                attributes.Add(Attribute_IsTopText, "是否文字显示在顶端");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
                return attributes;
            }
        }

        public static Dictionary<string, string> BooleanAttributeList
        {
            get
            {
                Dictionary<string, string> attributes = new Dictionary<string, string>();

                attributes.Add(Attribute_IsTop, "仅显示置顶内容");
                attributes.Add(Attribute_IsRecommend, "仅显示推荐内容");
                attributes.Add(Attribute_IsHot, "仅显示热点内容");
                attributes.Add(Attribute_IsColor, "仅显示醒目内容");

                attributes.Add(Attribute_IsDynamic, "是否动态显示");

                return attributes;
            }
        }

        //对“flash滚动焦点图”（stl:focusViewer）元素进行解析
        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                HtmlGenericControl genericControl = new HtmlGenericControl("div");
                IEnumerator ie = node.Attributes.GetEnumerator();

                string channelIndex = string.Empty;
                string channelName = string.Empty;
                EScopeType scopeType = EScopeType.Self;
                string groupChannel = string.Empty;
                string groupChannelNot = string.Empty;
                string groupContent = string.Empty;
                string groupContentNot = string.Empty;
                string tags = string.Empty;
                string orderByString = ETaxisTypeUtils.GetOrderByString(ETableStyle.BackgroundContent, ETaxisType.OrderByTaxisDesc, string.Empty, null);
                int startNum = 1;
                int totalNum = 0;
                bool isShowText = true;
                string isTopText = string.Empty;
                int titleWordNum = 0;
                string where = string.Empty;

                bool isTop = false;
                bool isTopExists = false;
                bool isRecommend = false;
                bool isRecommendExists = false;
                bool isHot = false;
                bool isHotExists = false;
                bool isColor = false;
                bool isColorExists = false;

                string theme = string.Empty;
                int imageWidth = 260;
                int imageHeight = 182;
                int textHeight = 25;
                string bgColor = string.Empty;
                bool isDynamic = false;

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(Attribute_ChannelIndex))
                    {
                        channelIndex = attr.Value;
                    }
                    else if (attributeName.Equals(Attribute_ChannelName))
                    {
                        channelName = attr.Value;
                    }
                    else if (attributeName.Equals(Attribute_Scope))
                    {
                        scopeType = EScopeTypeUtils.GetEnumType(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_GroupChannel))
                    {
                        groupChannel = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(Attribute_GroupChannelNot))
                    {
                        groupChannelNot = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(Attribute_GroupContent))
                    {
                        groupContent = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(Attribute_GroupContentNot))
                    {
                        groupContentNot = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(Attribute_Tags))
                    {
                        tags = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(Attribute_Order))
                    {
                        orderByString = StlDataUtility.GetOrderByString(pageInfo.PublishmentSystemID, attr.Value, ETableStyle.BackgroundContent, ETaxisType.OrderByTaxisDesc);
                    }
                    else if (attributeName.Equals(Attribute_StartNum))
                    {
                        startNum = TranslateUtils.ToInt(attr.Value, 1);
                    }
                    else if (attributeName.Equals(Attribute_TotalNum))
                    {
                        totalNum = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_TitleWordNum))
                    {
                        titleWordNum = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_Where))
                    {
                        where = attr.Value;
                    }
                    else if (attributeName.Equals(Attribute_IsTop))
                    {
                        isTopExists = true;
                        isTop = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_IsRecommend))
                    {
                        isRecommendExists = true;
                        isRecommend = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_IsHot))
                    {
                        isHotExists = true;
                        isHot = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_IsColor))
                    {
                        isColorExists = true;
                        isColor = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_Theme))
                    {
                        theme = attr.Value;
                    }
                    else if (attributeName.Equals(Attribute_Width))
                    {
                        if (StringUtils.EndsWithIgnoreCase(attr.Value, "px"))
                        {
                            attr.Value = attr.Value.Substring(0, attr.Value.Length - 2);
                        }
                        imageWidth = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_Height))
                    {
                        if (StringUtils.EndsWithIgnoreCase(attr.Value, "px"))
                        {
                            attr.Value = attr.Value.Substring(0, attr.Value.Length - 2);
                        }
                        imageHeight = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_BgColor))
                    {
                        bgColor = attr.Value;
                    }
                    else if (attributeName.Equals(Attribute_IsShowText))
                    {
                        isShowText = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(Attribute_IsTopText))
                    {
                        isTopText = attr.Value;
                    }
                    else if (attributeName.Equals(Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value);
                    }
                    else
                    {
                        genericControl.Attributes.Remove(attributeName);
                        genericControl.Attributes.Add(attributeName, attr.Value);
                    }
                }

                if (isDynamic)
                {
                    parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(pageInfo, contextInfo, genericControl, channelIndex, channelName, scopeType, groupChannel, groupChannelNot, groupContent, groupContentNot, tags, orderByString, startNum, totalNum, isShowText, isTopText, titleWordNum, where, isTop, isTopExists, isRecommend, isRecommendExists, isHot, isHotExists, isColor, isColorExists, theme, imageWidth, imageHeight, textHeight, bgColor);
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, HtmlGenericControl genericControl, string channelIndex, string channelName, EScopeType scopeType, string groupChannel, string groupChannelNot, string groupContent, string groupContentNot, string tags, string orderByString, int startNum, int totalNum, bool isShowText, string isTopText, int titleWordNum, string where, bool isTop, bool isTopExists, bool isRecommend, bool isRecommendExists, bool isHot, bool isHotExists, bool isColor, bool isColorExists, string theme, int imageWidth, int imageHeight, int textHeight, string bgColor)
        {
            string parsedContent = string.Empty;

            int channelID = CreateCacheManager.NodeID.GetNodeIDByChannelIDOrChannelIndexOrChannelName(pageInfo.PublishmentSystemID, contextInfo.ChannelID, channelIndex, channelName);

            IEnumerable dataSource = StlDataUtility.GetContentsDataSource(pageInfo.PublishmentSystemInfo, channelID, 0, groupContent, groupContentNot, tags, true, true, false, false, false, false, false, false, startNum, totalNum, orderByString, isTopExists, isTop, isRecommendExists, isRecommend, isHotExists, isHot, isColorExists, isColor, where, scopeType, groupChannel, groupChannelNot, null);

            if (dataSource != null)
            {
                if (StringUtils.EqualsIgnoreCase(theme, Theme_Style2))
                {
                    pageInfo.AddPageScriptsIfNotExists(PageInfo.Js_Ac_SWFObject);

                    StringCollection imageUrls = new StringCollection();
                    StringCollection navigationUrls = new StringCollection();
                    StringCollection titleCollection = new StringCollection();

                    foreach (object dataItem in dataSource)
                    {
                        BackgroundContentInfo contentInfo = new BackgroundContentInfo(dataItem);
                        if (contentInfo != null && !string.IsNullOrEmpty(contentInfo.ImageUrl))
                        {
                            if (contentInfo.ImageUrl.ToLower().EndsWith(".jpg") || contentInfo.ImageUrl.ToLower().EndsWith(".jpeg") || contentInfo.ImageUrl.ToLower().EndsWith(".png") || contentInfo.ImageUrl.ToLower().EndsWith(".pneg"))
                            {
                                titleCollection.Add(StringUtils.ToJsString(PageUtils.UrlEncode(StringUtils.MaxLengthText(StringUtils.StripTags(contentInfo.Title), titleWordNum))));
                                navigationUrls.Add(PageUtils.UrlEncode(PageUtility.GetContentUrl(pageInfo.PublishmentSystemInfo, contentInfo, pageInfo.VisualType)));
                                imageUrls.Add(PageUtils.UrlEncode(PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, contentInfo.ImageUrl)));
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(bgColor))
                    {
                        bgColor = "0xDADADA";
                    }
                    else
                    {
                        bgColor = bgColor.TrimStart('#');
                        if (!bgColor.StartsWith("0x"))
                        {
                            bgColor = "0x" + bgColor;
                        }
                    }

                    if (string.IsNullOrEmpty(isTopText))
                    {
                        isTopText = "0";
                    }
                    else
                    {
                        isTopText = (TranslateUtils.ToBool(isTopText)) ? "0" : "1";
                    }

                    string uniqueID = "FocusViewer_" + pageInfo.UniqueID;
                    StringBuilder paramBuilder = new StringBuilder();
                    paramBuilder.AppendFormat(@"so_{0}.addParam(""quality"", ""high"");", uniqueID).Append(StringUtils.Constants.ReturnAndNewline);
                    paramBuilder.AppendFormat(@"so_{0}.addParam(""wmode"", ""transparent"");", uniqueID).Append(StringUtils.Constants.ReturnAndNewline);
                    paramBuilder.AppendFormat(@"so_{0}.addParam(""menu"", ""false"");", uniqueID).Append(StringUtils.Constants.ReturnAndNewline);
                    paramBuilder.AppendFormat(@"so_{0}.addParam(""FlashVars"", ""bcastr_file=""+files_uniqueID+""&bcastr_link=""+links_uniqueID+""&bcastr_title=""+texts_uniqueID+""&AutoPlayTime=5&TitleBgPosition={1}&TitleBgColor={2}&BtnDefaultColor={2}"");", uniqueID, isTopText, bgColor).Append(StringUtils.Constants.ReturnAndNewline);

                    string scriptHtml = string.Format(@"
<div id=""flashcontent_{0}""></div>
<script type=""text/javascript"">
var files_uniqueID='{1}';
var links_uniqueID='{2}';
var texts_uniqueID='{3}';

var so_{0} = new SWFObject(""{4}"", ""flash_{0}"", ""{5}"", ""{6}"", ""7"", """");
{7}
so_{0}.write(""flashcontent_{0}"");
</script>
", uniqueID, TranslateUtils.ObjectCollectionToString(imageUrls, "|"), TranslateUtils.ObjectCollectionToString(navigationUrls, "|"), TranslateUtils.ObjectCollectionToString(titleCollection, "|"), PageUtility.GetSiteFilesUrl(pageInfo.PublishmentSystemInfo, SiteFiles.Flashes.Bcastr), imageWidth, imageHeight, paramBuilder);
                    scriptHtml = scriptHtml.Replace("uniqueID", uniqueID);

                    parsedContent = scriptHtml;
                }
                else if (StringUtils.EqualsIgnoreCase(theme, Theme_Style3))
                {
                    pageInfo.AddPageScriptsIfNotExists(PageInfo.Js_Ac_SWFObject);

                    StringCollection imageUrls = new StringCollection();
                    StringCollection navigationUrls = new StringCollection();
                    StringCollection titleCollection = new StringCollection();

                    foreach (object dataItem in dataSource)
                    {
                        BackgroundContentInfo contentInfo = new BackgroundContentInfo(dataItem);
                        if (contentInfo != null && !string.IsNullOrEmpty(contentInfo.ImageUrl))
                        {
                            if (contentInfo.ImageUrl.ToLower().EndsWith(".jpg") || contentInfo.ImageUrl.ToLower().EndsWith(".jpeg") || contentInfo.ImageUrl.ToLower().EndsWith(".png") || contentInfo.ImageUrl.ToLower().EndsWith(".pneg"))
                            {
                                titleCollection.Add(StringUtils.ToJsString(PageUtils.UrlEncode(StringUtils.MaxLengthText(StringUtils.StripTags(contentInfo.Title), titleWordNum))));
                                navigationUrls.Add(PageUtils.UrlEncode(PageUtility.GetContentUrl(pageInfo.PublishmentSystemInfo, contentInfo, pageInfo.VisualType)));
                                imageUrls.Add(PageUtils.UrlEncode(PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, contentInfo.ImageUrl)));
                            }
                        }
                    }

                    string uniqueID = "FocusViewer_" + pageInfo.UniqueID;
                    StringBuilder paramBuilder = new StringBuilder();
                    paramBuilder.AppendFormat(@"so_{0}.addParam(""quality"", ""high"");", uniqueID).Append(StringUtils.Constants.ReturnAndNewline);
                    paramBuilder.AppendFormat(@"so_{0}.addParam(""wmode"", ""transparent"");", uniqueID).Append(StringUtils.Constants.ReturnAndNewline);
                    paramBuilder.AppendFormat(@"so_{0}.addParam(""allowFullScreen"", ""true"");", uniqueID).Append(StringUtils.Constants.ReturnAndNewline);
                    paramBuilder.AppendFormat(@"so_{0}.addParam(""allowScriptAccess"", ""always"");", uniqueID).Append(StringUtils.Constants.ReturnAndNewline);
                    paramBuilder.AppendFormat(@"so_{0}.addParam(""menu"", ""false"");", uniqueID).Append(StringUtils.Constants.ReturnAndNewline);
                    paramBuilder.AppendFormat(@"so_{0}.addParam(""flashvars"", ""pw={1}&ph={2}&Times=4000&sizes=14&umcolor=16777215&btnbg=12189697&txtcolor=16777215&urls=""+urls_uniqueID+""&imgs=""+imgs_uniqueID+""&titles=""+titles_uniqueID);", uniqueID, imageWidth, imageHeight).Append(StringUtils.Constants.ReturnAndNewline);

                    string scriptHtml = string.Format(@"
<div id=""flashcontent_{0}""></div>
<script type=""text/javascript"">
var urls_uniqueID='{1}';
var imgs_uniqueID='{2}';
var titles_uniqueID='{3}';

var so_{0} = new SWFObject(""{4}"", ""flash_{0}"", ""{5}"", ""{6}"", ""7"", """");
{7}
so_{0}.write(""flashcontent_{0}"");
</script>
", uniqueID, TranslateUtils.ObjectCollectionToString(navigationUrls, "|"), TranslateUtils.ObjectCollectionToString(imageUrls, "|"), TranslateUtils.ObjectCollectionToString(titleCollection, "|"), PageUtility.GetSiteFilesUrl(pageInfo.PublishmentSystemInfo, SiteFiles.Flashes.Ali), imageWidth, imageHeight, paramBuilder);
                    scriptHtml = scriptHtml.Replace("uniqueID", uniqueID);

                    parsedContent = scriptHtml;
                }
                else if (StringUtils.EqualsIgnoreCase(theme, Theme_Style4))
                {
                    StringCollection imageUrls = new StringCollection();
                    StringCollection navigationUrls = new StringCollection();

                    foreach (object dataItem in dataSource)
                    {
                        BackgroundContentInfo contentInfo = new BackgroundContentInfo(dataItem);
                        if (contentInfo != null && !string.IsNullOrEmpty(contentInfo.ImageUrl))
                        {
                            navigationUrls.Add(PageUtility.GetContentUrl(pageInfo.PublishmentSystemInfo, contentInfo, pageInfo.VisualType));
                            imageUrls.Add(PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, contentInfo.ImageUrl));
                        }
                    }

                    StringBuilder imageBuilder = new StringBuilder();
                    StringBuilder numBuilder = new StringBuilder();

                    imageBuilder.AppendFormat(@"
<div style=""display:block""><a href=""{0}"" target=""_blank""><img src=""{1}"" width=""{2}"" height=""{3}"" border=""0"" onMouseOver=""fv_clearAuto();"" onMouseOut=""fv_setAuto()"" /></a></div>
", navigationUrls[0], imageUrls[0], imageWidth, imageHeight);

                    if (navigationUrls.Count > 1)
                    {
                        for (int i = 1; i < navigationUrls.Count; i++)
                        {
                            imageBuilder.AppendFormat(@"
<div style=""display:none""><a href=""{0}"" target=""_blank""><img src=""{1}"" width=""{2}"" height=""{3}"" border=""0"" onMouseOver=""fv_clearAuto();"" onMouseOut=""fv_setAuto()"" /></a></div>
", navigationUrls[i], imageUrls[i], imageWidth, imageHeight);
                        }
                    }

                    numBuilder.AppendFormat(@"
<td width=""18"" height=""16"" class=""fv_bigon"" onmouseover=""fv_clearAuto();fv_Mea(0);"" onmouseout=""fv_setAuto()"">1</td>
");

                    if (navigationUrls.Count > 1)
                    {
                        for (int i = 1; i < navigationUrls.Count; i++)
                        {
                            numBuilder.AppendFormat(@"
<td width=""18"" class=""fv_bigoff"" onmouseover=""fv_clearAuto();fv_Mea({0});"" onmouseout=""fv_setAuto()"">{1}</td>
", i, i + 1);
                        }
                    }

                    string scriptHtml = string.Format(@"
<style type=""text/css"">
.fv_adnum{{ position:absolute; z-index:1005; width:{0}px; height:24px; padding:5px 3px 0 0; left:21px; top:{1}px; }}
.fv_bigon{{ background:#E59948; font-family:Arial; color:#fff; font-size:12px; text-align:center; cursor:pointer}}
.fv_bigoff{{ background:#7DCABD; font-family:Arial; color:#fff; font-size:12px; text-align:center; cursor:pointer}}
</style>
<div style=""position:relative; left:0; top:0; width:{2}px; height:{3}px; background:#000"">
	<div id=""fv_filbox"" style=""position:absolute; z-index:999; left:0; top:0; width:{2}px; height:{3}px; filter:progid:DXImageTransform.Microsoft.Fade( duration=0.5,overlap=1.0 );"">
		{4}
    </div>
    <div class=""fv_adnum"" style=""background:url({5}) no-repeat !important; background:none ;filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(enabled=true, src='{5}')""></div>
    <div class=""fv_adnum"">
        <table border=""0"" cellspacing=""1"" cellpadding=""0"" align=""right"" id=""fv_num"">
          <tr>
            {6}
          </tr>
        </table>
    </div>
</div>
<script language=""javascript""> 
    var fv_n=0;
    var fv_nums={7};
    var fv_showNum = document.getElementById(""fv_num"");
    var is_IE=(navigator.appName==""Microsoft Internet Explorer"");
    function fv_Mea(value){{
        fv_n=value;
        for(var i=0;i<fv_nums;i++)
            if(value==i){{
                fv_showNum.getElementsByTagName(""td"")[i].setAttribute('className', 'fv_bigon');
                fv_showNum.getElementsByTagName(""td"")[i].setAttribute('class', 'fv_bigon');
            }}
            else{{	
                fv_showNum.getElementsByTagName(""td"")[i].setAttribute('className', 'fv_bigoff');
                fv_showNum.getElementsByTagName(""td"")[i].setAttribute('class', 'fv_bigoff');
            }}
        var divs = document.getElementById(""fv_filbox"").getElementsByTagName(""div""); 
		if (is_IE){{
        	document.getElementById(""fv_filbox"").filters[0].Apply();
		}}
		for(i=0;i<fv_nums;i++)i==value?divs[i].style.display=""block"":divs[i].style.display=""none"";
		if (is_IE){{
			document.getElementById(""fv_filbox"").filters[0].play();
		}}
    }}
    function fv_clearAuto(){{clearInterval(autoStart)}}
    function fv_setAuto(){{autoStart=setInterval(""auto(fv_n)"", 5000)}}
    function auto(){{
        fv_n++;
        if(fv_n>(fv_nums-1))fv_n=0;
        fv_Mea(fv_n);
    }}
    fv_setAuto(); 
</script>
", imageWidth - 24, imageHeight - 29, imageWidth, imageHeight, imageBuilder.ToString(), PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, "@/images/focusviewerbg.png"), numBuilder.ToString(), navigationUrls.Count);

                    parsedContent = scriptHtml.Replace("fv_", string.Format("fv{0}_", pageInfo.UniqueID));
                }
                else
                {
                    StringCollection imageUrls = new StringCollection();
                    StringCollection navigationUrls = new StringCollection();
                    StringCollection titleCollection = new StringCollection();

                    foreach (object dataItem in dataSource)
                    {
                        BackgroundContentInfo contentInfo = new BackgroundContentInfo(dataItem);
                        if (contentInfo != null && !string.IsNullOrEmpty(contentInfo.ImageUrl))
                        {
                            //这里使用png图片不管用
                            //||contentInfo.ImageUrl.ToLower().EndsWith(".png")||contentInfo.ImageUrl.ToLower().EndsWith(".pneg")
                            if (contentInfo.ImageUrl.ToLower().EndsWith(".jpg") || contentInfo.ImageUrl.ToLower().EndsWith(".jpeg"))
                            {
                                titleCollection.Add(StringUtils.ToJsString(PageUtils.UrlEncode(StringUtils.MaxLengthText(StringUtils.StripTags(contentInfo.Title), titleWordNum))));
                                navigationUrls.Add(PageUtils.UrlEncode(PageUtility.GetContentUrl(pageInfo.PublishmentSystemInfo, contentInfo, pageInfo.VisualType)));
                                imageUrls.Add(PageUtils.UrlEncode(PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, contentInfo.ImageUrl)));
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(bgColor))
                    {
                        bgColor = "#DADADA";
                    }
                    string titles = string.Empty;
                    if (isShowText == false)
                    {
                        textHeight = 0;
                    }
                    else
                    {
                        titles = TranslateUtils.ObjectCollectionToString(titleCollection, "|");
                    }
                    string uniqueID = "FocusViewer_" + pageInfo.UniqueID;
                    genericControl.ID = uniqueID;
                    genericControl.InnerHtml = "&nbsp;";
                    string divHtml = ControlUtils.GetControlRenderHtml(genericControl);
                    string scriptHtml = string.Format(@"
<script type=""text/javascript"" src=""{0}""></script>
<SCRIPT language=javascript type=""text/javascript"">
	<!--
	
	var uniqueID_focus_width={1}
	var uniqueID_focus_height={2}
	var uniqueID_text_height={3}
	var uniqueID_swf_height = uniqueID_focus_height + uniqueID_text_height
	
	var uniqueID_pics='{4}'
	var uniqueID_links='{5}'
	var uniqueID_texts='{6}'
	
	var uniqueID_FocusFlash = new bairongFlash(""{7}"", ""focusflash"", uniqueID_focus_width, uniqueID_swf_height, ""7"", ""{8}"", false, ""High"");
	uniqueID_FocusFlash.addParam(""allowScriptAccess"", ""sameDomain"");
	uniqueID_FocusFlash.addParam(""menu"", ""false"");
	uniqueID_FocusFlash.addParam(""wmode"", ""transparent"");

	uniqueID_FocusFlash.addVariable(""pics"", uniqueID_pics);
	uniqueID_FocusFlash.addVariable(""links"", uniqueID_links);
	uniqueID_FocusFlash.addVariable(""texts"", uniqueID_texts);
	uniqueID_FocusFlash.addVariable(""borderwidth"", uniqueID_focus_width);
	uniqueID_FocusFlash.addVariable(""borderheight"", uniqueID_focus_height);
	uniqueID_FocusFlash.addVariable(""textheight"", uniqueID_text_height);
	uniqueID_FocusFlash.write(""uniqueID"");
	
	//-->
</SCRIPT>
", PageUtility.GetSiteFilesUrl(pageInfo.PublishmentSystemInfo, SiteFiles.BaiRongFlash.Js), imageWidth, imageHeight, textHeight, TranslateUtils.ObjectCollectionToString(imageUrls, "|"), TranslateUtils.ObjectCollectionToString(navigationUrls, "|"), titles, PageUtility.GetSiteFilesUrl(pageInfo.PublishmentSystemInfo, SiteFiles.Flashes.FocusViewer), bgColor);
                    scriptHtml = scriptHtml.Replace("uniqueID", uniqueID);

                    parsedContent = divHtml + scriptHtml;
                }
            }

            return parsedContent;
        }
    }
}
