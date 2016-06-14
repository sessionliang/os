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
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlSelect
    {
        private StlSelect() { }
        public const string ElementName = "stl:select";//栏目或内容下拉列表

        public const string Attribute_IsChannel = "ischannel";                  //是否显示栏目下拉列表

        public const string Attribute_ChannelIndex = "channelindex";			//栏目索引
        public const string Attribute_ChannelName = "channelname";				//栏目名称
        public const string Attribute_UpLevel = "uplevel";					    //上级栏目的级别
        public const string Attribute_TopLevel = "toplevel";				    //从首页向下的栏目级别

        public const string Attribute_Scope = "scope";							//范围
        public const string Attribute_GroupChannel = "groupchannel";		    //指定显示的栏目组
        public const string Attribute_GroupChannelNot = "groupchannelnot";	    //指定不显示的栏目组
        public const string Attribute_GroupContent = "groupcontent";		    //指定显示的内容组
        public const string Attribute_GroupContentNot = "groupcontentnot";	    //指定不显示的内容组
        public const string Attribute_Tags = "tags";	                        //指定标签
        public const string Attribute_Order = "order";							//排序
        public const string Attribute_TotalNum = "totalnum";					//显示数目
        public const string Attribute_TitleWordNum = "titlewordnum";			//标题文字数量
        public const string Attribute_Where = "where";                          //获取下拉列表的条件判断
        public const string Attribute_QueryString = "querystring";              //链接参数

        public const string Attribute_IsTop = "istop";                       //仅显示置顶内容
        public const string Attribute_IsRecommend = "isrecommend";           //仅显示推荐内容
        public const string Attribute_IsHot = "ishot";                       //仅显示热点内容
        public const string Attribute_IsColor = "iscolor";                   //仅显示醒目内容

        public const string Attribute_Title = "title";							//下拉列表提示标题
        public const string Attribute_OpenWin = "openwin";						//选择是否新窗口打开链接
        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_IsChannel, "是否显示栏目下拉列表");

                attributes.Add(Attribute_ChannelIndex, "栏目索引");
                attributes.Add(Attribute_ChannelName, "栏目名称");
                attributes.Add(Attribute_UpLevel, "上级栏目的级别");
                attributes.Add(Attribute_TopLevel, "从首页向下的栏目级别");
                attributes.Add(Attribute_Scope, "选择的范围");
                attributes.Add(Attribute_GroupChannel, "指定显示的栏目组");
                attributes.Add(Attribute_GroupChannelNot, "指定不显示的栏目组");
                attributes.Add(Attribute_GroupContent, "指定显示的内容组");
                attributes.Add(Attribute_GroupContentNot, "指定不显示的内容组");
                attributes.Add(Attribute_Tags, "指定标签");
                attributes.Add(Attribute_Order, "排序");
                attributes.Add(Attribute_TotalNum, "显示数目");
                attributes.Add(Attribute_TitleWordNum, "标题文字数量");
                attributes.Add(Attribute_Where, "获取下拉列表的条件判断");
                attributes.Add(Attribute_QueryString, "链接参数");

                attributes.Add(Attribute_IsTop, "仅显示置顶内容");
                attributes.Add(Attribute_IsRecommend, "仅显示推荐内容");
                attributes.Add(Attribute_IsHot, "仅显示热点内容");
                attributes.Add(Attribute_IsColor, "仅显示醒目内容");

                attributes.Add(Attribute_Title, "下拉列表提示标题");
                attributes.Add(Attribute_OpenWin, "选择是否新窗口打开链接");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
                return attributes;
            }
        }


        //对“下拉列表”（stl:select）元素进行解析
        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                System.Web.UI.HtmlControls.HtmlSelect selectControl = new HtmlSelect();
                IEnumerator ie = node.Attributes.GetEnumerator();

                bool isChannel = true;
                string channelIndex = string.Empty;
                string channelName = string.Empty;
                int upLevel = 0;
                int topLevel = -1;
                string scopeTypeString = string.Empty;
                string groupChannel = string.Empty;
                string groupChannelNot = string.Empty;
                string groupContent = string.Empty;
                string groupContentNot = string.Empty;
                string tags = string.Empty;
                string order = string.Empty;
                int totalNum = 0;
                int titleWordNum = 0;
                string where = string.Empty;
                string queryString = string.Empty;

                bool isTop = false;
                bool isTopExists = false;
                bool isRecommend = false;
                bool isRecommendExists = false;
                bool isHot = false;
                bool isHotExists = false;
                bool isColor = false;
                bool isColorExists = false;

                string displayTitle = string.Empty;
                bool openWin = true;
                bool isDynamic = false;

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(Attribute_IsChannel))
                    {
                        isChannel = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_ChannelIndex))
                    {
                        channelIndex = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(Attribute_ChannelName))
                    {
                        channelName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(Attribute_UpLevel))
                    {
                        upLevel = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_TopLevel))
                    {
                        topLevel = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_Scope))
                    {
                        scopeTypeString = attr.Value;
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
                        order = attr.Value;
                    }
                    else if (attributeName.Equals(Attribute_TotalNum))
                    {
                        try
                        {
                            totalNum = int.Parse(attr.Value);
                        }
                        catch { }
                    }
                    else if (attributeName.Equals(Attribute_Where))
                    {
                        where = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(Attribute_QueryString))
                    {
                        queryString = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
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
                    else if (attributeName.Equals(Attribute_TitleWordNum))
                    {
                        try
                        {
                            titleWordNum = int.Parse(attr.Value);
                        }
                        catch { }
                    }
                    else if (attributeName.Equals(Attribute_Title))
                    {
                        displayTitle = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(Attribute_OpenWin))
                    {
                        try
                        {
                            openWin = bool.Parse(attr.Value);
                        }
                        catch { }
                    }
                    else if (attributeName.Equals(Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value);
                    }
                    else
                    {
                        selectControl.Attributes.Remove(attributeName);
                        selectControl.Attributes.Add(attributeName, attr.Value);
                    }
                }

                if (isDynamic)
                {
                    parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(pageInfo, contextInfo, selectControl, isChannel, channelIndex, channelName, upLevel, topLevel, scopeTypeString, groupChannel, groupChannelNot, groupContent, groupContentNot, tags, order, totalNum, titleWordNum, where, queryString, isTop, isTopExists, isRecommend, isRecommendExists, isHot, isHotExists, isColor, isColorExists, displayTitle, openWin);
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, System.Web.UI.HtmlControls.HtmlSelect selectControl, bool isChannel, string channelIndex, string channelName, int upLevel, int topLevel, string scopeTypeString, string groupChannel, string groupChannelNot, string groupContent, string groupContentNot, string tags, string order, int totalNum, int titleWordNum, string where, string queryString, bool isTop, bool isTopExists, bool isRecommend, bool isRecommendExists, bool isHot, bool isHotExists, bool isColor, bool isColorExists, string displayTitle, bool openWin)
        {
            string parsedContent = string.Empty;

            EScopeType scopeType;
            if (!string.IsNullOrEmpty(scopeTypeString))
            {
                scopeType = EScopeTypeUtils.GetEnumType(scopeTypeString);
            }
            else
            {
                if (isChannel)
                {
                    scopeType = EScopeType.Children;
                }
                else
                {
                    scopeType = EScopeType.Self;
                }
            }

            string orderByString;
            if (isChannel)
            {
                orderByString = StlDataUtility.GetOrderByString(pageInfo.PublishmentSystemID, order, ETableStyle.Channel, ETaxisType.OrderByTaxis);
            }
            else
            {
                orderByString = StlDataUtility.GetOrderByString(pageInfo.PublishmentSystemID, order, ETableStyle.BackgroundContent, ETaxisType.OrderByTaxisDesc);
            }

            int channelID = StlDataUtility.GetNodeIDByLevel(pageInfo.PublishmentSystemID, contextInfo.ChannelID, upLevel, topLevel);

            channelID = CreateCacheManager.NodeID.GetNodeIDByChannelIDOrChannelIndexOrChannelName(pageInfo.PublishmentSystemID, channelID, channelIndex, channelName);

            NodeInfo channel = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, channelID);

            string uniqueID = "Select_" + pageInfo.UniqueID;
            selectControl.ID = uniqueID;

            string scriptHtml;
            if (openWin)
            {
                scriptHtml = string.Format(@"
<script language=""JavaScript"" type=""text/JavaScript"">
<!--
function {0}_jumpMenu(targ,selObj)
{1} //v3.0
window.open(selObj.options[selObj.selectedIndex].value);
selObj.selectedIndex=0;
{2}
//-->
</script>", uniqueID, "{", "}");
                selectControl.Attributes.Add("onChange", string.Format("{0}_jumpMenu('parent',this)", uniqueID));
            }
            else
            {
                scriptHtml = string.Format("<script language=\"JavaScript\">function {0}_jumpMenu(targ,selObj,restore){1}eval(targ+\".location='\"+selObj.options[selObj.selectedIndex].value+\"'\");if (restore) selObj.selectedIndex=0;{2}</script>", uniqueID, "{", "}");
                selectControl.Attributes.Add("onChange", string.Format("{0}_jumpMenu('self',this,0)", uniqueID));
            }
            if (!string.IsNullOrEmpty(displayTitle))
            {
                ListItem listitem = new ListItem(displayTitle, PageUtils.UNCLICKED_URL);
                listitem.Selected = true;
                selectControl.Items.Add(listitem);
            }

            if (isChannel)
            {
                ArrayList nodeIDArrayList = StlDataUtility.GetNodeIDArrayList(pageInfo.PublishmentSystemID, channel.NodeID, groupContent, groupContentNot, orderByString, scopeType, groupChannel, groupChannelNot, false, false, totalNum, where);

                if (nodeIDArrayList != null && nodeIDArrayList.Count > 0)
                {
                    foreach (int nodeIDInSelect in nodeIDArrayList)
                    {
                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, nodeIDInSelect);

                        if (nodeInfo != null)
                        {
                            string title = StringUtils.MaxLengthText(nodeInfo.NodeName, titleWordNum);
                            string url = PageUtility.GetChannelUrl(pageInfo.PublishmentSystemInfo, nodeInfo, pageInfo.VisualType);
                            if (!string.IsNullOrEmpty(queryString))
                            {
                                url = PageUtils.AddQueryString(url, queryString);
                            }
                            ListItem listitem = new ListItem(title, url);
                            selectControl.Items.Add(listitem);
                        }
                    }
                }
            }
            else
            {
                IEnumerable dataSource = StlDataUtility.GetContentsDataSource(pageInfo.PublishmentSystemInfo, channelID, contextInfo.ContentID, groupContent, groupContentNot, tags, false, false, false, false, false, false, false, false, 1, totalNum, orderByString, isTopExists, isTop, isRecommendExists, isRecommend, isHotExists, isHot, isColorExists, isColor, where, scopeType, groupChannel, groupChannelNot, null);

                if (dataSource != null)
                {
                    foreach (object dataItem in dataSource)
                    {
                        BackgroundContentInfo contentInfo = new BackgroundContentInfo(dataItem);
                        if (contentInfo != null)
                        {
                            string title = StringUtils.MaxLengthText(contentInfo.Title, titleWordNum);
                            string url = PageUtility.GetContentUrl(pageInfo.PublishmentSystemInfo, contentInfo, pageInfo.VisualType);
                            if (!string.IsNullOrEmpty(queryString))
                            {
                                url = PageUtils.AddQueryString(url, queryString);
                            }
                            ListItem listitem = new ListItem(title, url);
                            selectControl.Items.Add(listitem);
                        }
                    }
                }
            }

            parsedContent = scriptHtml + ControlUtils.GetControlRenderHtml(selectControl);

            return parsedContent;
        }
    }
}
