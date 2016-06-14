using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using BaiRong.Core;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System;
using BaiRong.Model;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlMenu
    {
        private StlMenu() { }
        public const string ElementName = "stl:menu";//获取链接

        public const string Attribute_StyleName = "stylename";				    //样式名称
        public const string Attribute_ChannelIndex = "channelindex";			//栏目索引
        public const string Attribute_ChannelName = "channelname";				//栏目名称
        public const string Attribute_GroupChannel = "groupchannel";		    //指定显示的栏目组
        public const string Attribute_GroupChannelNot = "groupchannelnot";	    //指定不显示的栏目组
        public const string Attribute_IsShowChildren = "isshowchildren";		//是否显示二级菜单
        public const string Attribute_MenuWidth = "menuwidth";					//菜单宽度
        public const string Attribute_MenuHeight = "menuheight";				//菜单高度
        public const string Attribute_XPosition = "xposition";					//菜单水平位置
        public const string Attribute_YPosition = "yposition";					//菜单垂直位置
        public const string Attribute_ChildMenuDisplay = "childmenudisplay";	//二级菜单显示方式
        public const string Attribute_ChildMenuWidth = "childmenuwidth";		//二级菜单宽度
        public const string Attribute_ChildMenuHeight = "childmenuheight";		//二级菜单高度
        public const string Attribute_Target = "target";						//打开窗口目标
        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_StyleName, "样式名称");
                attributes.Add(Attribute_ChannelIndex, "栏目索引");
                attributes.Add(Attribute_ChannelName, "栏目名称");
                attributes.Add(Attribute_GroupChannel, "指定显示的栏目组");
                attributes.Add(Attribute_GroupChannelNot, "指定不显示的栏目组");
                attributes.Add(Attribute_IsShowChildren, "是否显示二级菜单");
                attributes.Add(Attribute_MenuWidth, "菜单宽度");
                attributes.Add(Attribute_MenuHeight, "菜单高度");
                attributes.Add(Attribute_XPosition, "菜单水平位置");
                attributes.Add(Attribute_YPosition, "菜单垂直位置");
                attributes.Add(Attribute_ChildMenuDisplay, "二级菜单显示方式");
                attributes.Add(Attribute_ChildMenuWidth, "二级菜单宽度");
                attributes.Add(Attribute_ChildMenuHeight, "二级菜单高度");
                attributes.Add(Attribute_Target, "打开窗口目标");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
                return attributes;
            }
        }


        //对“栏目链接”（stl:menu）元素进行解析
        internal static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                IEnumerator ie = node.Attributes.GetEnumerator();
                string channelIndex = string.Empty;
                string channelName = string.Empty;
                string groupChannel = string.Empty;
                string groupChannelNot = string.Empty;
                bool isShowChildren = false;
                string styleName = string.Empty;
                string menuWidth = string.Empty;
                string menuHeight = string.Empty;
                string xPosition = string.Empty;
                string yPosition = string.Empty;
                string childMenuDisplay = string.Empty;
                string childMenuWidth = string.Empty;
                string childMenuHeight = string.Empty;
                string target = string.Empty;
                bool isDynamic = false;

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlMenu.Attribute_StyleName))
                    {
                        styleName = attr.Value;
                    }
                    else if (attributeName.Equals(StlMenu.Attribute_ChannelIndex))
                    {
                        channelIndex = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(StlMenu.Attribute_ChannelName))
                    {
                        channelName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(StlMenu.Attribute_GroupChannel))
                    {
                        groupChannel = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(StlMenu.Attribute_GroupChannelNot))
                    {
                        groupChannelNot = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(StlMenu.Attribute_IsShowChildren))
                    {
                        isShowChildren = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(StlMenu.Attribute_MenuWidth))
                    {
                        menuWidth = attr.Value;
                    }
                    else if (attributeName.Equals(StlMenu.Attribute_MenuHeight))
                    {
                        menuHeight = attr.Value;
                    }
                    else if (attributeName.Equals(StlMenu.Attribute_XPosition))
                    {
                        xPosition = attr.Value;
                    }
                    else if (attributeName.Equals(StlMenu.Attribute_YPosition))
                    {
                        yPosition = attr.Value;
                    }
                    else if (attributeName.Equals(StlMenu.Attribute_ChildMenuDisplay))
                    {
                        childMenuDisplay = attr.Value;
                    }
                    else if (attributeName.Equals(StlMenu.Attribute_ChildMenuWidth))
                    {
                        childMenuWidth = attr.Value;
                    }
                    else if (attributeName.Equals(StlMenu.Attribute_ChildMenuHeight))
                    {
                        childMenuHeight = attr.Value;
                    }
                    else if (attributeName.Equals(StlMenu.Attribute_Target))
                    {
                        target = attr.Value;
                    }
                    else if (attributeName.Equals(StlMenu.Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value);
                    }
                }

                if (isDynamic)
                {
                    parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(node, pageInfo, contextInfo, channelIndex, channelName, groupChannel, groupChannelNot, isShowChildren, styleName, menuWidth, menuHeight, xPosition, yPosition, childMenuDisplay, childMenuWidth, childMenuHeight, target);
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, string channelIndex, string channelName, string groupChannel, string groupChannelNot, bool isShowChildren, string styleName, string menuWidth, string menuHeight, string xPosition, string yPosition, string childMenuDisplay, string childMenuWidth, string childMenuHeight, string target)
        {
            string parsedContent = string.Empty;

            int channelID = CreateCacheManager.NodeID.GetNodeIDByChannelIDOrChannelIndexOrChannelName(pageInfo.PublishmentSystemID, contextInfo.ChannelID, channelIndex, channelName);
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, channelID);

            string innerHtml = nodeInfo.NodeName.Trim();
            if (!string.IsNullOrEmpty(node.InnerXml))
            {
                StringBuilder innerBuilder = new StringBuilder(node.InnerXml);
                StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                innerHtml = innerBuilder.ToString();
            }

            ArrayList nodeInfoArrayList = new ArrayList();//需要显示的栏目列表

            ArrayList childNodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(nodeInfo, EScopeType.Children, groupChannel, groupChannelNot);
            if (childNodeIDArrayList != null && childNodeIDArrayList.Count > 0)
            {
                foreach (int childNodeID in childNodeIDArrayList)
                {
                    NodeInfo theNodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, childNodeID);
                    nodeInfoArrayList.Add(theNodeInfo);
                }
            }

            if (nodeInfoArrayList.Count == 0)
            {
                parsedContent = innerHtml;
            }
            else
            {
                MenuDisplayInfo menuDisplayInfo = null;
                int menuDisplayID = DataProvider.MenuDisplayDAO.GetMenuDisplayIDByName(pageInfo.PublishmentSystemID, styleName);
                if (menuDisplayID == 0)
                {
                    menuDisplayInfo = DataProvider.MenuDisplayDAO.GetDefaultMenuDisplayInfo(pageInfo.PublishmentSystemID);
                }
                else
                {
                    menuDisplayInfo = DataProvider.MenuDisplayDAO.GetMenuDisplayInfo(menuDisplayID);
                }
                MenuDisplayInfo level2MenuDisplayInfo = menuDisplayInfo;
                if (isShowChildren && !string.IsNullOrEmpty(childMenuDisplay))
                {
                    int childMenuDisplayID = DataProvider.MenuDisplayDAO.GetMenuDisplayIDByName(pageInfo.PublishmentSystemID, childMenuDisplay);
                    if (childMenuDisplayID == 0)
                    {
                        level2MenuDisplayInfo = DataProvider.MenuDisplayDAO.GetDefaultMenuDisplayInfo(pageInfo.PublishmentSystemID);
                    }
                    else
                    {
                        level2MenuDisplayInfo = DataProvider.MenuDisplayDAO.GetMenuDisplayInfo(childMenuDisplayID);
                    }
                }

                if (string.IsNullOrEmpty(menuWidth)) menuWidth = menuDisplayInfo.MenuWidth.ToString();
                if (string.IsNullOrEmpty(menuHeight)) menuHeight = menuDisplayInfo.MenuItemHeight.ToString();
                if (string.IsNullOrEmpty(xPosition)) xPosition = menuDisplayInfo.XPosition;
                if (string.IsNullOrEmpty(yPosition)) yPosition = menuDisplayInfo.YPosition;
                if (string.IsNullOrEmpty(childMenuWidth)) childMenuWidth = level2MenuDisplayInfo.MenuWidth.ToString();
                if (string.IsNullOrEmpty(childMenuHeight)) childMenuHeight = level2MenuDisplayInfo.MenuItemHeight.ToString();

                string createMenuString = string.Empty;
                string writeMenuString = string.Empty;

                int menuID = pageInfo.UniqueID;

                parsedContent = string.Format(@"<span name=""mm_link_{0}"" id=""mm_link_{0}"" onmouseover=""MM_showMenu(window.mm_menu_{0},{1},{2},null,'mm_link_{0}');"" onmouseout=""MM_startTimeout();"">{3}</span>", menuID, xPosition, yPosition, innerHtml);

                StringBuilder menuBuilder = new StringBuilder();
                StringBuilder level2MenuBuilder = new StringBuilder();


                foreach (NodeInfo theNodeInfo in nodeInfoArrayList)
                {
                    bool isLevel2Exist = false;
                    int level2MenuID = 0;

                    if (isShowChildren)
                    {
                        ArrayList level2NodeInfoArrayList = new ArrayList();

                        ArrayList level2NodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(theNodeInfo, EScopeType.Children, groupChannel, groupChannelNot);
                        if (level2NodeIDArrayList != null && level2NodeIDArrayList.Count > 0)
                        {
                            foreach (int level2NodeID in level2NodeIDArrayList)
                            {
                                NodeInfo level2NodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, level2NodeID);
                                level2NodeInfoArrayList.Add(level2NodeInfo);
                            }

                            if (level2NodeInfoArrayList.Count > 0)
                            {
                                isLevel2Exist = true;
                                StringBuilder level2ChildMenuBuilder = new StringBuilder();
                                level2MenuID = pageInfo.UniqueID;

                                foreach (NodeInfo level2NodeInfo in level2NodeInfoArrayList)
                                {
                                    string level2NodeUrl = PageUtility.GetChannelUrl(pageInfo.PublishmentSystemInfo, level2NodeInfo, pageInfo.VisualType);
                                    if (PageUtils.UNCLICKED_URL.Equals(level2NodeUrl))
                                    {
                                        level2ChildMenuBuilder.Append(string.Format(@"  mm_menu_{0}.addMenuItem(""{1}"", ""location='{2}'"");", level2MenuID, level2NodeInfo.NodeName.Trim(), level2NodeUrl));
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(target))
                                        {
                                            if (target.ToLower().Equals("_blank"))
                                            {
                                                level2ChildMenuBuilder.Append(string.Format(@"  mm_menu_{0}.addMenuItem(""{1}"", ""window.open('{2}', '_blank');"");", level2MenuID, level2NodeInfo.NodeName.Trim(), level2NodeUrl));
                                            }
                                            else
                                            {
                                                level2ChildMenuBuilder.Append(string.Format(@"  mm_menu_{0}.addMenuItem(""{1}"", ""location='{2}', '{3}'"");", level2MenuID, level2NodeInfo.NodeName.Trim(), level2NodeUrl, target));
                                            }
                                        }
                                        else
                                        {
                                            level2ChildMenuBuilder.Append(string.Format(@"  mm_menu_{0}.addMenuItem(""{1}"", ""location='{2}'"");", level2MenuID, level2NodeInfo.NodeName.Trim(), level2NodeUrl));
                                        }
                                    }
                                }

                                string level2CreateMenuString = string.Format(@"
  window.mm_menu_{0} = new Menu('{1}',{2},{3},'{4}',{5},'{6}','{7}','{8}','{9}','{10}','{11}',{12},{13},{14},-5,7,true,{15},{16},{17},true,true);
  {18}
  mm_menu_{0}.fontWeight='{19}';
  mm_menu_{0}.fontStyle='{20}';
  mm_menu_{0}.hideOnMouseOut={21};
  mm_menu_{0}.bgColor='{22}';
  mm_menu_{0}.menuBorder={23};
  mm_menu_{0}.menuLiteBgColor='{24}';
  mm_menu_{0}.menuBorderBgColor='{25}';
", level2MenuID, theNodeInfo.NodeName.Trim(), childMenuWidth, childMenuHeight, level2MenuDisplayInfo.FontFamily, level2MenuDisplayInfo.FontSize, level2MenuDisplayInfo.FontColor, level2MenuDisplayInfo.FontColorHilite, level2MenuDisplayInfo.MenuItemBgColor, level2MenuDisplayInfo.MenuHiliteBgColor, level2MenuDisplayInfo.MenuItemHAlign, level2MenuDisplayInfo.MenuItemVAlign, level2MenuDisplayInfo.MenuItemPadding, level2MenuDisplayInfo.MenuItemSpacing, level2MenuDisplayInfo.HideTimeout, level2MenuDisplayInfo.MenuBgOpaque, level2MenuDisplayInfo.Vertical, level2MenuDisplayInfo.MenuItemIndent, level2ChildMenuBuilder, level2MenuDisplayInfo.FontWeight, level2MenuDisplayInfo.FontStyle, level2MenuDisplayInfo.HideOnMouseOut, level2MenuDisplayInfo.BGColor, level2MenuDisplayInfo.MenuBorder, level2MenuDisplayInfo.MenuLiteBgColor, level2MenuDisplayInfo.MenuBorderBgColor);
                                level2MenuBuilder.Append(level2CreateMenuString);
                            }
                        }
                    }

                    string menuName = string.Empty;
                    if (isLevel2Exist)
                    {
                        menuName = "mm_menu_" + level2MenuID;
                    }
                    else
                    {
                        menuName = "\"" + theNodeInfo.NodeName.Trim() + "\"";
                    }

                    string nodeUrl = PageUtility.GetChannelUrl(pageInfo.PublishmentSystemInfo, theNodeInfo, pageInfo.VisualType);
                    if (PageUtils.UNCLICKED_URL.Equals(nodeUrl))
                    {
                        menuBuilder.Append(string.Format(@"  mm_menu_{0}.addMenuItem({1}, ""location='{2}'"");", menuID, menuName, nodeUrl));
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(target))
                        {
                            if (target.ToLower().Equals("_blank"))
                            {
                                menuBuilder.Append(string.Format(@"  mm_menu_{0}.addMenuItem({1}, ""window.open('{2}', '_blank');"");", menuID, menuName, nodeUrl));
                            }
                            else
                            {
                                menuBuilder.Append(string.Format(@"  mm_menu_{0}.addMenuItem({1}, ""location='{2}', '{3}'"");", menuID, menuName, nodeUrl, target));
                            }
                        }
                        else
                        {
                            menuBuilder.Append(string.Format(@"  mm_menu_{0}.addMenuItem({1}, ""location='{2}'"");", menuID, menuName, nodeUrl));
                        }
                    }

                }

                string childMenuIcon = string.Empty;
                if (!string.IsNullOrEmpty(menuDisplayInfo.ChildMenuIcon))
                {
                    childMenuIcon = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, menuDisplayInfo.ChildMenuIcon);
                }
                createMenuString += string.Format(@"
  if (window.mm_menu_{0}) return;
  {1}
  window.mm_menu_{0} = new Menu('root',{2},{3},'{4}',{5},'{6}','{7}','{8}','{9}','{10}','{11}',{12},{13},{14},-5,7,true,{15},{16},{17},true,true);
  {18}
  mm_menu_{0}.fontWeight='{19}';
  mm_menu_{0}.fontStyle='{20}';
  mm_menu_{0}.hideOnMouseOut={21};
  mm_menu_{0}.bgColor='{22}';
  mm_menu_{0}.menuBorder={23};
  mm_menu_{0}.menuLiteBgColor='{24}';
  mm_menu_{0}.menuBorderBgColor='{25}';
  mm_menu_{0}.childMenuIcon = ""{26}"";

//NEXT
", menuID, level2MenuBuilder, menuWidth, menuHeight, menuDisplayInfo.FontFamily, menuDisplayInfo.FontSize, menuDisplayInfo.FontColor, menuDisplayInfo.FontColorHilite, menuDisplayInfo.MenuItemBgColor, menuDisplayInfo.MenuHiliteBgColor, menuDisplayInfo.MenuItemHAlign, menuDisplayInfo.MenuItemVAlign, menuDisplayInfo.MenuItemPadding, menuDisplayInfo.MenuItemSpacing, menuDisplayInfo.HideTimeout, menuDisplayInfo.MenuBgOpaque, menuDisplayInfo.Vertical, menuDisplayInfo.MenuItemIndent, menuBuilder, menuDisplayInfo.FontWeight, menuDisplayInfo.FontStyle, menuDisplayInfo.HideOnMouseOut, menuDisplayInfo.BGColor, menuDisplayInfo.MenuBorder, menuDisplayInfo.MenuLiteBgColor, menuDisplayInfo.MenuBorderBgColor, childMenuIcon);

                //writeMenuString += string.Format("mm_menu_{0}.writeMenus();", menuID);

                StringBuilder scriptBuilder = new StringBuilder();

                string functionHead = string.Format(@"<script language=""JavaScript"" src=""{0}""></script>
<script language=""JavaScript"">
//HEAD
function siteserverLoadMenus() {{", PageUtility.GetSiteFilesUrl(pageInfo.PublishmentSystemInfo, SiteFiles.MM_Menu.Js), menuID);
                string functionFoot = string.Format(@"
//FOOT
}}
</script>
<script language=""JavaScript1.2"">siteserverLoadMenus();writeMenus();</script>", menuID);
                //取得已经保存的js
                string existScript = string.Empty;
                if (pageInfo.IsPageScriptsExists(PageInfo.Js_Ac_MenuScripts, true))
                { 
                    existScript = pageInfo.GetPageScripts(PageInfo.Js_Ac_MenuScripts, true);
                }
                if (string.IsNullOrEmpty(existScript) || existScript.IndexOf("//HEAD") < 0)
                { 
                    scriptBuilder.Append(functionHead);
                }
                scriptBuilder.Append(createMenuString);
                //scriptBuilder.Append(writeMenuString);
                if (string.IsNullOrEmpty(existScript) || existScript.IndexOf("//FOOT") < 0)
                { 
                    scriptBuilder.Append(functionFoot);
                }
                if (!string.IsNullOrEmpty(existScript) && existScript.IndexOf("//NEXT") >= 0)
                {
                    existScript = existScript.Replace("//NEXT", scriptBuilder.ToString());
                    pageInfo.SetPageScripts(PageInfo.Js_Ac_MenuScripts, existScript, true);
                }
                else
                {
                    pageInfo.SetPageScripts(PageInfo.Js_Ac_MenuScripts, scriptBuilder.ToString(), true);
                }
            }

            return parsedContent;
        }
    }
}
