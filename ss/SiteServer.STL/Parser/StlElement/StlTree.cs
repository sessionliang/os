using System;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Services;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.StlElement
{
	public class StlTree
	{
        private StlTree() { }
        public const string ElementName = "stl:tree";//树状导航

		public const string Attribute_ChannelIndex = "channelindex";			        //栏目索引
		public const string Attribute_ChannelName = "channelname";				        //栏目名称
        public const string Attribute_UpLevel = "uplevel";					            //上级栏目的级别
        public const string Attribute_TopLevel = "toplevel";				            //从首页向下的栏目级别

        public const string Attribute_GroupChannel = "groupchannel";		    //指定显示的栏目组
        public const string Attribute_GroupChannelNot = "groupchannelnot";	    //指定不显示的栏目组

        public const string Attribute_Title = "title";							        //根节点显示名称
        public const string Attribute_IsLoading = "isloading";				            //是否AJAX方式动态载入
        public const string Attribute_IsShowContentNum = "isshowcontentnum";            //是否显示栏目内容数
        public const string Attribute_IsShowTreeLine = "isshowtreeline";                //是否显示树状线
        public const string Attribute_CurrentFormatString = "currentformatstring";      //当前项格式化字符串
        public const string Attribute_Target = "target";							        //打开窗口目标
        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

		public static ListDictionary AttributeList
		{
			get
			{
				ListDictionary attributes = new ListDictionary();

				attributes.Add(Attribute_ChannelIndex, "栏目索引");
				attributes.Add(Attribute_ChannelName, "栏目名称");
                attributes.Add(Attribute_UpLevel, "上级栏目的级别");
                attributes.Add(Attribute_TopLevel, "从首页向下的栏目级别");
                attributes.Add(Attribute_GroupChannel, "指定显示的栏目组");
                attributes.Add(Attribute_GroupChannelNot, "指定不显示的栏目组");

                attributes.Add(Attribute_Title, "根节点显示名称");
                attributes.Add(Attribute_IsLoading, "是否AJAX方式即时载入");
                attributes.Add(Attribute_IsShowContentNum, "是否显示栏目内容数");
                attributes.Add(Attribute_IsShowTreeLine, "是否显示树状线");
                attributes.Add(Attribute_CurrentFormatString, "当前项格式化字符串");
                attributes.Add(Attribute_Target, "打开窗口目标");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
				return attributes;
			}
		}

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
		{
			string parsedContent = string.Empty;
			try
			{
                LowerNameValueCollection attributes = new LowerNameValueCollection();
				IEnumerator ie = node.Attributes.GetEnumerator();

				string channelIndex = string.Empty;
				string channelName = string.Empty;
                int upLevel = 0;
                int topLevel = -1;
                string groupChannel = string.Empty;
                string groupChannelNot = string.Empty;

                string title = string.Empty;
                bool isLoading = false;
                bool isShowContentNum = false;
                bool isShowTreeLine = true;
                string currentFormatString = "<strong>{0}</strong>";
                bool isDynamic = false;

				while (ie.MoveNext())
				{
					XmlAttribute attr = (XmlAttribute)ie.Current;
					string attributeName = attr.Name.ToLower();
                    
                    if (attributeName.Equals(Attribute_ChannelIndex))
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
                    else if (attributeName.Equals(Attribute_GroupChannel))
                    {
                        groupChannel = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(Attribute_GroupChannelNot))
                    {
                        groupChannelNot = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(Attribute_Title))
                    {
                        title = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(Attribute_IsLoading))
                    {
                        isLoading = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(Attribute_IsShowContentNum))
                    {
                        isShowContentNum = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(Attribute_IsShowTreeLine))
                    {
                        isShowTreeLine = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(Attribute_CurrentFormatString))
                    {
                        currentFormatString = attr.Value;
                        if (!StringUtils.Contains(currentFormatString, "{0}"))
                        {
                            currentFormatString += "{0}";
                        }
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
                    if (isLoading)
                    {
                        parsedContent = ParseImplAjax(pageInfo, contextInfo, channelIndex, channelName, upLevel, topLevel, groupChannel, groupChannelNot, title, isShowContentNum, isShowTreeLine, currentFormatString);
                    }
                    else
                    {
                        parsedContent = ParseImplNotAjax(pageInfo, contextInfo, channelIndex, channelName, upLevel, topLevel, groupChannel, groupChannelNot, title, isShowContentNum, isShowTreeLine, currentFormatString);
                    }
                }
			}
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

			return parsedContent;
		}

        private static string ParseImplNotAjax(PageInfo pageInfo, ContextInfo contextInfo, string channelIndex, string channelName, int upLevel, int topLevel, string groupChannel, string groupChannelNot, string title, bool isShowContentNum, bool isShowTreeLine, string currentFormatString)
        {
            string treeDirectoryUrl = PageUtility.GetIconUrl(pageInfo.PublishmentSystemInfo, "tree");

            int channelID = StlDataUtility.GetNodeIDByLevel(pageInfo.PublishmentSystemID, contextInfo.ChannelID, upLevel, topLevel);

            channelID = CreateCacheManager.NodeID.GetNodeIDByChannelIDOrChannelIndexOrChannelName(pageInfo.PublishmentSystemID, channelID, channelIndex, channelName);

            NodeInfo channel = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, channelID);

            string target = "";

            StringBuilder htmlBuilder = new StringBuilder();

            htmlBuilder.Append(@"<table border=""0"" cellpadding=""0"" cellspacing=""0"" style=""width:100%;"">");

            ArrayList theNodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(channel, EScopeType.All, groupChannel, groupChannelNot);
            bool[] isLastNodeArray = new bool[theNodeIDArrayList.Count];
            ArrayList nodeIDArrayList = new ArrayList();

            NodeInfo currentNodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, pageInfo.PageNodeID);
            if (currentNodeInfo != null)
            {
                nodeIDArrayList = TranslateUtils.StringCollectionToIntArrayList(currentNodeInfo.ParentsPath);
                nodeIDArrayList.Add(currentNodeInfo.NodeID);
            }

            foreach (int theNodeID in theNodeIDArrayList)
            {
                NodeInfo theNodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, theNodeID);
                NodeInfo nodeInfo = new NodeInfo(theNodeInfo);
                if (theNodeID == pageInfo.PublishmentSystemID && !string.IsNullOrEmpty(title))
                {
                    nodeInfo.NodeName = title;
                }
                bool isDisplay = nodeIDArrayList.Contains(theNodeID);
                if (!isDisplay)
                {

                    isDisplay = (nodeInfo.ParentID == channelID || nodeIDArrayList.Contains(nodeInfo.ParentID));
                }

                bool selected = (theNodeID == channelID);
                if (!selected && nodeIDArrayList.Contains(nodeInfo.NodeID))
                {
                    selected = true;
                }
                bool hasChildren = (nodeInfo.ChildrenCount != 0);

                string linkUrl = PageUtility.GetChannelUrl(pageInfo.PublishmentSystemInfo, theNodeInfo, pageInfo.VisualType);
                int level = theNodeInfo.ParentsCount - channel.ParentsCount;
                StlTreeItemNotAjax item = new StlTreeItemNotAjax(isDisplay, selected, pageInfo, nodeInfo, hasChildren, linkUrl, target, isShowTreeLine, isShowContentNum, isLastNodeArray, currentFormatString, channelID, level);

                htmlBuilder.Append(item.GetTrHtml());
            }

            htmlBuilder.Append("</table>");

            pageInfo.AddPageScriptsIfNotExists(PageInfo.Js_Ag_StlTreeNotAjax, StlTreeItemNotAjax.GetNodeTreeScript(pageInfo.PublishmentSystemInfo));

            return htmlBuilder.ToString();
        }

        private class StlTreeItemNotAjax
        {
            private string treeDirectoryUrl;
            private string iconFolderUrl;
            private string iconOpenedFolderUrl;
            private readonly string iconEmptyUrl;
            private readonly string iconMinusUrl;
            private readonly string iconPlusUrl;

            private bool isDisplay = false;
            private bool selected = false;
            private PageInfo pageInfo;
            private NodeInfo nodeInfo;
            private bool hasChildren = false;
            private string linkUrl = string.Empty;
            private string target = string.Empty;
            private bool isShowTreeLine = true;
            private bool isShowContentNum = false;
            bool[] isLastNodeArray;
            string currentFormatString;
            private int topNodeID = 0;
            private int level = 0;

            private StlTreeItemNotAjax() { }

            public StlTreeItemNotAjax(bool isDisplay, bool selected, PageInfo pageInfo, NodeInfo nodeInfo, bool hasChildren, string linkUrl, string target, bool isShowTreeLine, bool isShowContentNum, bool[] isLastNodeArray, string currentFormatString, int topNodeID, int level)
            {
                this.isDisplay = isDisplay;
                this.selected = selected;
                this.pageInfo = pageInfo;
                this.nodeInfo = nodeInfo;
                this.hasChildren = hasChildren;
                this.linkUrl = linkUrl;
                this.target = target;
                this.isShowTreeLine = isShowTreeLine;
                this.isShowContentNum = isShowContentNum;
                this.isLastNodeArray = isLastNodeArray;
                this.currentFormatString = currentFormatString;
                this.topNodeID = topNodeID;
                this.level = level;

                this.treeDirectoryUrl = PageUtility.GetIconUrl(pageInfo.PublishmentSystemInfo, "tree");
                this.iconFolderUrl = PageUtils.Combine(treeDirectoryUrl, "folder.gif");
                this.iconOpenedFolderUrl = PageUtils.Combine(treeDirectoryUrl, "openedfolder.gif");
                this.iconEmptyUrl = PageUtils.Combine(treeDirectoryUrl, "empty.gif");
                this.iconMinusUrl = PageUtils.Combine(treeDirectoryUrl, "minus.png");
                this.iconPlusUrl = PageUtils.Combine(treeDirectoryUrl, "plus.png");
            }

            public string GetTrHtml()
            {
                string displayHtml = (this.isDisplay) ? Constants.SHOW_ELEMENT_STYLE : Constants.HIDE_ELEMENT_STYLE;
                string trElementHtml = string.Format(@"
<tr style='{0}' treeItemLevel='{1}'>
	<td nowrap>
		{2}
	</td>
</tr>
", displayHtml, level + 1, GetItemHtml());

                return trElementHtml;
            }

            public string GetItemHtml()
            {
                StringBuilder htmlBuilder = new StringBuilder();
                if (this.isShowTreeLine)
                {
                    if (this.topNodeID == nodeInfo.NodeID)
                    {
                        nodeInfo.IsLastNode = true;
                    }
                    if (nodeInfo.IsLastNode == false)
                    {
                        this.isLastNodeArray[level] = false;
                    }
                    else
                    {
                        this.isLastNodeArray[level] = true;
                    }
                    for (int i = 0; i < level; i++)
                    {
                        if (this.isLastNodeArray[i])
                        {
                            htmlBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}\"/>", this.iconEmptyUrl);
                        }
                        else
                        {
                            htmlBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_line.gif\"/>", this.treeDirectoryUrl);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < level; i++)
                    {
                        htmlBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}\"/>", this.iconEmptyUrl);
                    }
                }

                if (this.isDisplay)
                {
                    if (this.isShowTreeLine)
                    {
                        if (nodeInfo.IsLastNode)
                        {
                            if (this.hasChildren)
                            {
                                if (this.selected)
                                {
                                    htmlBuilder.AppendFormat("<img align=\"absmiddle\" style=\"cursor:pointer;\" onClick=\"stltree_displayChildren(this);\" isOpen=\"true\" src=\"{0}/tree_minusbottom.gif\"/>", treeDirectoryUrl);
                                }
                                else
                                {
                                    htmlBuilder.AppendFormat("<img align=\"absmiddle\" style=\"cursor:pointer;\" onClick=\"stltree_displayChildren(this);\" isOpen=\"false\" src=\"{0}/tree_plusbottom.gif\"/>", treeDirectoryUrl);
                                }
                            }
                            else
                            {
                                htmlBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_bottom.gif\"/>", treeDirectoryUrl);
                            }
                        }
                        else
                        {
                            if (this.hasChildren)
                            {
                                if (this.selected)
                                {
                                    htmlBuilder.AppendFormat("<img align=\"absmiddle\" style=\"cursor:pointer;\" onClick=\"stltree_displayChildren(this);\" isOpen=\"true\" src=\"{0}/tree_minusmiddle.gif\"/>", treeDirectoryUrl);
                                }
                                else
                                {
                                    htmlBuilder.AppendFormat("<img align=\"absmiddle\" style=\"cursor:pointer;\" onClick=\"stltree_displayChildren(this);\" isOpen=\"false\" src=\"{0}/tree_plusmiddle.gif\"/>", treeDirectoryUrl);
                                }
                            }
                            else
                            {
                                htmlBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_middle.gif\"/>", treeDirectoryUrl);
                            }
                        }
                    }
                    else
                    {
                        if (this.hasChildren)
                        {
                            if (this.selected)
                            {
                                htmlBuilder.AppendFormat("<img align=\"absmiddle\" style=\"cursor:pointer;\" onClick=\"stltree_displayChildren(this);\" isOpen=\"true\" src=\"{0}\"/>", this.iconMinusUrl);
                            }
                            else
                            {
                                htmlBuilder.AppendFormat("<img align=\"absmiddle\" style=\"cursor:pointer;\" onClick=\"stltree_displayChildren(this);\" isOpen=\"false\" src=\"{0}\"/>", this.iconPlusUrl);
                            }
                        }
                        else
                        {
                            htmlBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}\"/>", this.iconEmptyUrl);
                        }
                    }
                }
                else
                {
                    if (this.isShowTreeLine)
                    {
                        if (nodeInfo.IsLastNode)
                        {
                            if (this.hasChildren)
                            {
                                htmlBuilder.AppendFormat("<img align=\"absmiddle\" style=\"cursor:pointer;\" onClick=\"stltree_displayChildren(this);\" isOpen=\"false\" src=\"{0}/tree_plusbottom.gif\"/>", treeDirectoryUrl);
                            }
                            else
                            {
                                htmlBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_bottom.gif\"/>", treeDirectoryUrl);
                            }
                        }
                        else
                        {
                            if (this.hasChildren)
                            {
                                htmlBuilder.AppendFormat("<img align=\"absmiddle\" style=\"cursor:pointer;\" onClick=\"stltree_displayChildren(this);\" isOpen=\"false\" src=\"{0}/tree_plusmiddle.gif\"/>", treeDirectoryUrl);
                            }
                            else
                            {
                                htmlBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_middle.gif\"/>", treeDirectoryUrl);
                            }
                        }
                    }
                    else
                    {
                        if (this.hasChildren)
                        {
                            htmlBuilder.AppendFormat("<img align=\"absmiddle\" style=\"cursor:pointer;\" onClick=\"stltree_displayChildren(this);\" isOpen=\"false\" src=\"{0}\"/>", this.iconPlusUrl);
                        }
                        else
                        {
                            htmlBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}\"/>", this.iconEmptyUrl);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(this.iconFolderUrl))
                {
                    htmlBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}\"/>", this.iconFolderUrl);
                }

                htmlBuilder.Append("&nbsp;");

                string nodeName = nodeInfo.NodeName;
                if ((this.pageInfo.TemplateInfo.TemplateType == ETemplateType.ChannelTemplate || this.pageInfo.TemplateInfo.TemplateType == ETemplateType.ContentTemplate) && this.pageInfo.PageNodeID == nodeInfo.NodeID)
                {
                    nodeName = string.Format(this.currentFormatString, nodeName);
                }

                if (!string.IsNullOrEmpty(this.linkUrl))
                {
                    string targetHtml = (string.IsNullOrEmpty(this.target)) ? string.Empty : string.Format("target='{0}'", this.target);
                    string clickChangeHtml = "onclick='stltree_openFolderByA(this);'";

                    htmlBuilder.AppendFormat("<a href='{0}' {1} {2} isTreeLink='true'>{3}</a>", this.linkUrl, targetHtml, clickChangeHtml, nodeName);
                }
                else
                {
                    htmlBuilder.Append(nodeName);
                }

                if (this.isShowContentNum && nodeInfo.ContentNum >= 0)
                {
                    htmlBuilder.Append("&nbsp;");
                    htmlBuilder.AppendFormat("<span style=\"font-size:8pt;font-family:arial\">({0})</span>", nodeInfo.ContentNum);
                }

                return htmlBuilder.ToString();
            }

            public static string GetNodeTreeScript(PublishmentSystemInfo publishmentSystemInfo)
            {
                string script = @"
<script language=""JavaScript"">
function stltree_isNull(obj){
	if (typeof(obj) == 'undefined')
	  return true;
	  
	if (obj == undefined)
	  return true;
	  
	if (obj == null)
	  return true;
	 
	return false;
}

//取得Tree的级别，1为第一级
function stltree_getTreeLevel(e) {
	var length = 0;
	if (!stltree_isNull(e)){
		if (e.tagName == 'TR') {
			length = parseInt(e.getAttribute('treeItemLevel'));
		}
	}
	return length;
}

function stltree_closeAllFolder(){
	if (stltree_isNodeTree){
		var imgCol = document.getElementsByTagName('IMG');
		if (!stltree_isNull(imgCol)){
			for (x=0;x<imgCol.length;x++){
				if (!stltree_isNull(imgCol.item(x).getAttribute('src'))){
					if (imgCol.item(x).getAttribute('src') == '{iconOpenedFolderUrl}'){
						imgCol.item(x).setAttribute('src', '{iconFolderUrl}');
					}
				}
			}
		}
	}

	var aCol = document.getElementsByTagName('A');
	if (!stltree_isNull(aCol)){
		for (x=0;x<aCol.length;x++){
			if (aCol.item(x).getAttribute('isTreeLink') == 'true'){
				aCol.item(x).style.fontWeight = 'normal';
			}
		}
	}
}

function stltree_openFolderByA(element){
	stltree_closeAllFolder();
	if (stltree_isNull(element) || element.tagName != 'A') return;

	element.style.fontWeight = 'bold';

	if (stltree_isNodeTree){
		for (element = element.previousSibling;;){
			if (element != null && element.tagName == 'A'){
				element = element.firstChild;
			}
			if (element != null && element.tagName == 'IMG'){
				if (element.getAttribute('src') == '{iconFolderUrl}') break;
				break;
			}else{
				element = element.previousSibling;
			} 
		}
		if (!stltree_isNull(element)){
			element.setAttribute('src', '{iconOpenedFolderUrl}');
		}
	}
}

function stltree_getTrElement(element){
	if (stltree_isNull(element)) return;
	for (element = element.parentNode;;){
		if (element != null && element.tagName == 'TR'){
			break;
		}else{
			element = element.parentNode;
		} 
	}
	return element;
}

function stltree_getImgClickableElementByTr(element){
	if (stltree_isNull(element) || element.tagName != 'TR') return;
	var img = null;
	if (!stltree_isNull(element.childNodes)){
		var imgCol = element.getElementsByTagName('IMG');
		if (!stltree_isNull(imgCol)){
			for (x=0;x<imgCol.length;x++){
				if (!stltree_isNull(imgCol.item(x).getAttribute('isOpen'))){
					img = imgCol.item(x);
					break;
				}
			}
		}
	}
	return img;
}

//显示、隐藏下级目录
function stltree_displayChildren(element){
	if (stltree_isNull(element)) return;

	var tr = stltree_getTrElement(element);

	var img = stltree_getImgClickableElementByTr(tr);//需要变换的加减图标

	if (!stltree_isNull(img) && img.getAttribute('isOpen') != null){
		if (img.getAttribute('isOpen') == 'false'){
			img.setAttribute('isOpen', 'true');
            var iconMinusUrl = img.getAttribute('src').replace('plus','minus');
            img.setAttribute('src', iconMinusUrl);
		}else{
            img.setAttribute('isOpen', 'false');
            var iconPlusUrl = img.getAttribute('src').replace('minus','plus');
            img.setAttribute('src', iconPlusUrl);
		}
	}

	var level = stltree_getTreeLevel(tr);//点击项菜单的级别
	
	var collection = new Array();
	var index = 0;

	for ( var e = tr.nextSibling; !stltree_isNull(e) ; e = e.nextSibling) {
		if (!stltree_isNull(e) && !stltree_isNull(e.tagName) && e.tagName == 'TR'){
		    var currentLevel = stltree_getTreeLevel(e);
		    if (currentLevel <= level) break;
		    if(e.style.display == '') {
			    e.style.display = 'none';
		    }else{//展开
			    if (currentLevel != level + 1) continue;
			    e.style.display = '';
			    var imgClickable = stltree_getImgClickableElementByTr(e);
			    if (!stltree_isNull(imgClickable)){
				    if (!stltree_isNull(imgClickable.getAttribute('isOpen')) && imgClickable.getAttribute('isOpen') =='true'){
					    imgClickable.setAttribute('isOpen', 'false');
                        var iconPlusUrl = imgClickable.getAttribute('src').replace('minus','plus');
                        imgClickable.setAttribute('src', iconPlusUrl);
					    collection[index] = imgClickable;
					    index++;
				    }
			    }
		    }
        }
	}
	
	if (index > 0){
		for (i=0;i<=index;i++){
			stltree_displayChildren(collection[i]);
		}
	}
}
var stltree_isNodeTree = {isNodeTree};
</script>
";
                string treeDirectoryUrl = PageUtility.GetIconUrl(publishmentSystemInfo, "tree");
                string iconFolderUrl = PageUtils.Combine(treeDirectoryUrl, "folder.gif");
                string iconOpenedFolderUrl = PageUtils.Combine(treeDirectoryUrl, "openedfolder.gif");
                string iconMinusUrl = PageUtils.Combine(treeDirectoryUrl, "minus.png");
                string iconPlusUrl = PageUtils.Combine(treeDirectoryUrl, "plus.png");

                script = script.Replace("{iconFolderUrl}", iconFolderUrl);
                script = script.Replace("{iconMinusUrl}", iconMinusUrl);
                script = script.Replace("{iconOpenedFolderUrl}", iconOpenedFolderUrl);
                script = script.Replace("{iconPlusUrl}", iconPlusUrl);
                script = script.Replace("{isNodeTree}", "true");
                return script;
            }

        }

        private static string ParseImplAjax(PageInfo pageInfo, ContextInfo contextInfo, string channelIndex, string channelName, int upLevel, int topLevel, string groupChannel, string groupChannelNot, string title, bool isShowContentNum, bool isShowTreeLine, string currentFormatString)
        {
            pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.A_JQuery);

            string treeDirectoryUrl = PageUtility.GetIconUrl(pageInfo.PublishmentSystemInfo, "tree");

            int channelID = StlDataUtility.GetNodeIDByLevel(pageInfo.PublishmentSystemID, contextInfo.ChannelID, upLevel, topLevel);

            channelID = CreateCacheManager.NodeID.GetNodeIDByChannelIDOrChannelIndexOrChannelName(pageInfo.PublishmentSystemID, channelID, channelIndex, channelName);

            NodeInfo channel = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, channelID);

            string target = "";

            StringBuilder htmlBuilder = new StringBuilder();

            htmlBuilder.Append(@"<table border=""0"" cellpadding=""0"" cellspacing=""0"" style=""width:100%;"">");

            ArrayList theNodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(channel, EScopeType.SelfAndChildren, groupChannel, groupChannelNot);
            ArrayList nodeIDArrayList = new ArrayList();

            NodeInfo currentNodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, pageInfo.PageNodeID);
            if (currentNodeInfo != null)
            {
                nodeIDArrayList = TranslateUtils.StringCollectionToIntArrayList(currentNodeInfo.ParentsPath);
                nodeIDArrayList.Add(currentNodeInfo.NodeID);
            }

            foreach (int theNodeID in theNodeIDArrayList)
            {
                NodeInfo theNodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, theNodeID);
                NodeInfo nodeInfo = new NodeInfo(theNodeInfo);
                if (theNodeID == pageInfo.PublishmentSystemID && !string.IsNullOrEmpty(title))
                {
                    nodeInfo.NodeName = title;
                }

                string rowHtml = StlTree.GetChannelRowHtml(pageInfo.PublishmentSystemID, nodeInfo, target, isShowTreeLine, isShowContentNum, currentFormatString, channelID, channel.ParentsCount, pageInfo.PageNodeID);

                htmlBuilder.Append(rowHtml);
            }

            htmlBuilder.Append("</table>");

            pageInfo.AddPageScriptsIfNotExists(PageInfo.Js_Ag_StlTreeAjax, StlTreeItemAjax.GetScript(pageInfo, target, isShowTreeLine, isShowContentNum, currentFormatString, channelID, channel.ParentsCount, pageInfo.PageNodeID));

            return htmlBuilder.ToString();
        }

        public static string GetChannelRowHtml(int publishmentSystemID, NodeInfo nodeInfo, string target, bool isShowTreeLine, bool isShowContentNum, string currentFormatString, int topNodeID, int topParantsCount, int currentNodeID)
        {
            StlTreeItemAjax nodeTreeItem = new StlTreeItemAjax(publishmentSystemID, nodeInfo, target, isShowTreeLine, isShowContentNum, currentFormatString, topNodeID, topParantsCount, currentNodeID);
            string title = nodeTreeItem.GetItemHtml();

            string rowHtml = string.Format(@"
<tr treeItemLevel=""{0}"">
	<td nowrap>
		{1}
	</td>
</tr>
", nodeInfo.ParentsCount + 1, title);

            return rowHtml;
        }

        private class StlTreeItemAjax
        {
            private string treeDirectoryUrl;
            private string iconFolderUrl;
            private string iconOpenedFolderUrl;
            private readonly string iconEmptyUrl;
            private readonly string iconMinusUrl;
            private readonly string iconPlusUrl;

            private int publishmentSystemID;
            private NodeInfo nodeInfo;
            private bool hasChildren = false;
            private string linkUrl = string.Empty;
            private string target = string.Empty;
            private bool isShowTreeLine = true;
            private bool isShowContentNum = false;
            string currentFormatString;
            private int topNodeID = 0;
            private int level = 0;
            private int currentNodeID = 0;

            private StlTreeItemAjax() { }

            public StlTreeItemAjax(int publishmentSystemID, NodeInfo nodeInfo, string target, bool isShowTreeLine, bool isShowContentNum, string currentFormatString, int topNodeID, int topParentsCount, int currentNodeID)
            {
                this.publishmentSystemID = publishmentSystemID;
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                this.nodeInfo = nodeInfo;
                this.hasChildren = (nodeInfo.ChildrenCount != 0);
                this.linkUrl = PageUtility.GetChannelUrl(publishmentSystemInfo, nodeInfo, publishmentSystemInfo.Additional.VisualType);
                this.target = target;
                this.isShowTreeLine = isShowTreeLine;
                this.isShowContentNum = isShowContentNum;
                this.currentFormatString = currentFormatString;
                this.topNodeID = topNodeID;
                this.level = nodeInfo.ParentsCount - topParentsCount;
                this.currentNodeID = currentNodeID;

                this.treeDirectoryUrl = PageUtility.GetIconUrl(publishmentSystemInfo, "tree");
                this.iconFolderUrl = PageUtils.Combine(treeDirectoryUrl, "folder.gif");
                this.iconOpenedFolderUrl = PageUtils.Combine(treeDirectoryUrl, "openedfolder.gif");
                this.iconEmptyUrl = PageUtils.Combine(treeDirectoryUrl, "empty.gif");
                this.iconMinusUrl = PageUtils.Combine(treeDirectoryUrl, "minus.png");
                this.iconPlusUrl = PageUtils.Combine(treeDirectoryUrl, "plus.png");
            }

            public string GetItemHtml()
            {
                StringBuilder htmlBuilder = new StringBuilder();

                for (int i = 0; i < level; i++)
                {
                    htmlBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}\"/>", this.iconEmptyUrl);
                }

                if (this.hasChildren)
                {
                    if (this.topNodeID != this.nodeInfo.NodeID)
                    {
                        htmlBuilder.AppendFormat("<img align=\"absmiddle\" style=\"cursor:pointer;\" onClick=\"stltree_displayChildren(this);\" isAjax=\"true\" isOpen=\"false\" id=\"{0}\" src=\"{1}\"/>", nodeInfo.NodeID, this.iconPlusUrl);
                    }
                    else
                    {
                        htmlBuilder.AppendFormat("<img align=\"absmiddle\" style=\"cursor:pointer;\" onClick=\"stltree_displayChildren(this);\" isAjax=\"false\" isOpen=\"true\" id=\"{0}\" src=\"{1}\"/>", nodeInfo.NodeID, this.iconMinusUrl);
                    }
                }
                else
                {
                    htmlBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}\"/>", this.iconEmptyUrl);
                }

                if (!string.IsNullOrEmpty(this.iconFolderUrl))
                {
                    htmlBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}\"/>", this.iconFolderUrl);
                }

                htmlBuilder.Append("&nbsp;");

                //string nodeName = nodeInfo.NodeName;
                //if ((this.pageInfo.TemplateInfo.TemplateType == ETemplateType.ChannelTemplate || this.pageInfo.TemplateInfo.TemplateType == ETemplateType.ContentTemplate) && this.pageInfo.PageNodeID == nodeInfo.NodeID)
                //{
                //    nodeName = string.Format(this.currentFormatString, nodeName);
                //}
                string nodeName = nodeInfo.NodeName;
                if (this.currentNodeID == nodeInfo.NodeID)
                {
                    nodeName = string.Format(this.currentFormatString, nodeName);
                }

                if (!string.IsNullOrEmpty(this.linkUrl))
                {
                    string targetHtml = (string.IsNullOrEmpty(this.target)) ? string.Empty : string.Format("target='{0}'", this.target);

                    htmlBuilder.AppendFormat("<a href='{0}' {1} isTreeLink='true'>{2}</a>", this.linkUrl, targetHtml, nodeName);
                }
                else
                {
                    htmlBuilder.Append(nodeName);
                }

                if (this.isShowContentNum && nodeInfo.ContentNum >= 0)
                {
                    htmlBuilder.Append("&nbsp;");
                    htmlBuilder.AppendFormat("<span style=\"font-size:8pt;font-family:arial\">({0})</span>", nodeInfo.ContentNum);
                }

                return htmlBuilder.ToString();
            }

            public static string GetScript(PageInfo pageInfo, string target, bool isShowTreeLine, bool isShowContentNum, string currentFormatString, int topNodeID, int topParentsCount, int currentNodeID)
            {
                string script = @"
<script language=""JavaScript"">
function stltree_isNull(obj){
	if (typeof(obj) == 'undefined')
	  return true;
	  
	if (obj == undefined)
	  return true;
	  
	if (obj == null)
	  return true;
	 
	return false;
}

function stltree_getTreeLevel(e) {
	var length = 0;
	if (!stltree_isNull(e)){
		if (e.tagName == 'TR') {
			length = parseInt(e.getAttribute('treeItemLevel'));
		}
	}
	return length;
}

function stltree_getTrElement(element){
	if (stltree_isNull(element)) return;
	for (element = element.parentNode;;){
		if (element != null && element.tagName == 'TR'){
			break;
		}else{
			element = element.parentNode;
		} 
	}
	return element;
}

function stltree_getImgClickableElementByTr(element){
	if (stltree_isNull(element) || element.tagName != 'TR') return;
	var img = null;
	if (!stltree_isNull(element.childNodes)){
		var imgCol = element.getElementsByTagName('IMG');
		if (!stltree_isNull(imgCol)){
			for (x=0;x<imgCol.length;x++){
				if (!stltree_isNull(imgCol.item(x).getAttribute('isOpen'))){
					img = imgCol.item(x);
					break;
				}
			}
		}
	}
	return img;
}

var weightedLink = null;

function fontWeightLink(element){
    if (weightedLink != null)
    {
        weightedLink.style.fontWeight = 'normal';
    }
    element.style.fontWeight = 'bold';
    weightedLink = element;
}

var completedNodeID = null;
function stltree_displayChildren(img){
	if (stltree_isNull(img)) return;

	var tr = stltree_getTrElement(img);

    var isToOpen = img.getAttribute('isOpen') == 'false';
    var isByAjax = img.getAttribute('isAjax') == 'true';
    var nodeID = img.getAttribute('id');

	if (!stltree_isNull(img) && img.getAttribute('isOpen') != null){
		if (img.getAttribute('isOpen') == 'false'){
			img.setAttribute('isOpen', 'true');
            img.setAttribute('src', '{iconMinusUrl}');
		}else{
            img.setAttribute('isOpen', 'false');
            img.setAttribute('src', '{iconPlusUrl}');
		}
	}

    if (isToOpen && isByAjax)
    {
        var div = document.createElement('div');
        div.innerHTML = ""<img align='absmiddle' border='0' src='{iconLoadingUrl}' /> 栏目加载中，请稍候..."";
        img.parentNode.appendChild(div);
        //Element.addClassName(div, 'loading');
        loadingChannels(tr, img, div, nodeID);
    }
    else
    {
        var level = stltree_getTreeLevel(tr);
    	
	    var collection = new Array();
	    var index = 0;

	    for ( var e = tr.nextSibling; !stltree_isNull(e) ; e = e.nextSibling) {
		    if (!stltree_isNull(e) && !stltree_isNull(e.tagName) && e.tagName == 'TR'){
		        var currentLevel = stltree_getTreeLevel(e);
		        if (currentLevel <= level) break;
		        if(e.style.display == '') {
			        e.style.display = 'none';
		        }else{
			        if (currentLevel != level + 1) continue;
			        e.style.display = '';
			        var imgClickable = stltree_getImgClickableElementByTr(e);
			        if (!stltree_isNull(imgClickable)){
				        if (!stltree_isNull(imgClickable.getAttribute('isOpen')) && imgClickable.getAttribute('isOpen') =='true'){
					        imgClickable.setAttribute('isOpen', 'false');
                            imgClickable.setAttribute('src', '{iconPlusUrl}');
					        collection[index] = imgClickable;
					        index++;
				        }
			        }
		        }
            }
	    }
    	
	    if (index > 0){
		    for (i=0;i<=index;i++){
			    stltree_displayChildren(collection[i]);
		    }
	    }
    }
}
";
                script += string.Format(@"
function loadingChannels(tr, img, div, nodeID){{
    var url = '{0}';
    var pars = 'publishmentSystemID={1}&parentID=' + nodeID + '&target={2}&isShowTreeLine={3}&isShowContentNum={4}&currentFormatString={5}&topNodeID={6}&topParentsCount={7}&currentNodeID={8}';

    //jQuery.post(url, pars, function(data, textStatus){{
        //$($.parseHTML(data)).insertAfter($(tr));
        //img.setAttribute('isAjax', 'false');
        //img.parentNode.removeChild(div);
        //completedNodeID = nodeID;
    //}});
    $.ajax({{
                url: url,
                type: ""POST"",
                mimeType:""multipart/form-data"",
                contentType: false,
                processData: false,
                cache: false,
                xhrFields: {{   
                    withCredentials: true   
                }},
                data: pars,
                success: function(json, textStatus){{
                    $($.parseHTML(data)).insertAfter($(tr));
                    img.setAttribute('isAjax', 'false');
                    img.parentNode.removeChild(div);
                    completedNodeID = nodeID;
                }}
    }});
}}

function loadingChannelsOnLoad(path){{
    if (path && path.length > 0){{
        var nodeIDs = path.split(',');
        var nodeID = nodeIDs[0];
        var img = $(nodeID);
        if (!img) return;
        if (img.getAttribute('isOpen') == 'false'){{
            stltree_displayChildren(img);
            new PeriodicalExecuter(function(pe){{
                if (completedNodeID && completedNodeID == nodeID){{
                    if (path.indexOf(',') != -1){{
                        var thePath = path.substring(path.indexOf(',') + 1);
                        loadingChannelsOnLoad(thePath);
                    }}
                    pe.stop();
                }} 
            }}, 1);
        }}
    }}
}}
</script>
", PageService.GetLoadingChannelsUrl(pageInfo.PublishmentSystemInfo), pageInfo.PublishmentSystemID, target, isShowTreeLine, isShowContentNum, RuntimeUtils.EncryptStringByTranslate(currentFormatString), topNodeID, topParentsCount, currentNodeID);

                script += GetScriptOnLoad(pageInfo.PublishmentSystemID, topNodeID, pageInfo.PageNodeID);

                string treeDirectoryUrl = PageUtility.GetIconUrl(pageInfo.PublishmentSystemInfo, "tree");
                string iconFolderUrl = PageUtils.Combine(treeDirectoryUrl, "folder.gif");
                string iconOpenedFolderUrl = PageUtils.Combine(treeDirectoryUrl, "openedfolder.gif");
                string iconMinusUrl = PageUtils.Combine(treeDirectoryUrl, "minus.png");
                string iconPlusUrl = PageUtils.Combine(treeDirectoryUrl, "plus.png");

                script = script.Replace("{iconFolderUrl}", iconFolderUrl);
                script = script.Replace("{iconMinusUrl}", iconMinusUrl);
                script = script.Replace("{iconOpenedFolderUrl}", iconOpenedFolderUrl);
                script = script.Replace("{iconPlusUrl}", iconPlusUrl);
                script = script.Replace("{isNodeTree}", "true");

                script = script.Replace("{iconLoadingUrl}", PageUtility.GetIconUrl(pageInfo.PublishmentSystemInfo, "loading.gif"));

                return script;
            }

            public static string GetScriptOnLoad(int publishmentSystemID, int topNodeID, int currentNodeID)
            {
                if (currentNodeID != 0 && currentNodeID != publishmentSystemID && currentNodeID != topNodeID)
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, currentNodeID);
                    if (nodeInfo != null)
                    {
                        string path = string.Empty;
                        if (nodeInfo.ParentID == publishmentSystemID)
                        {
                            path = currentNodeID.ToString();
                        }
                        else
                        {
                            path = nodeInfo.ParentsPath.Substring(nodeInfo.ParentsPath.IndexOf(",") + 1) + "," + currentNodeID.ToString();
                        }
                        return string.Format(@"
<script language=""JavaScript"">
Event.observe(window,'load', function(){{
    loadingChannelsOnLoad('{0}');
}});
</script>
", path);
                    }
                }
                return string.Empty;
            }
        }
	}
}
