using System.Text;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.CMS.Core
{
    public class NodeNaviTreeItem
    {
        private string iconFolderUrl;
        private string iconOpenedFolderUrl;
        private readonly string iconEmptyUrl;
        private readonly string iconMinusUrl;
        private readonly string iconPlusUrl;

        private bool isDisplay = false;
        private bool selected = false;
        private int parentsCount = 0;
        private bool hasChildren = false;
        private string text = string.Empty;
        private string linkUrl = string.Empty;
        private string onClickUrl = string.Empty;
        private string target = string.Empty;
        private bool enabled = true;
        private bool isClickChange = false;
        private bool isNodeTree = true;
        private int publishmentSystemID = 0;
        private int nodeID = 0;
        private ENodeType nodeType = ENodeType.BackgroundNormalNode;
        private int contentNum = 0;

        public static NodeNaviTreeItem CreateNodeTreeItem(bool isDisplay, bool selected, int parentsCount, bool hasChildren, string text, string linkUrl, string onClickUrl, string target, bool enabled, bool isClickChange, int publishmentSystemID, int nodeID, ENodeType nodeType, int contentNum)
        {
            NodeNaviTreeItem item = new NodeNaviTreeItem();
            item.isDisplay = isDisplay;
            item.selected = selected;
            item.parentsCount = parentsCount;
            item.hasChildren = hasChildren;
            item.text = text;
            item.linkUrl = linkUrl;
            item.onClickUrl = onClickUrl;
            item.target = target;
            item.enabled = enabled;
            item.isClickChange = isClickChange;
            item.isNodeTree = true;
            item.publishmentSystemID = publishmentSystemID;
            item.nodeID = nodeID;
            item.nodeType = nodeType;
            item.contentNum = contentNum;
            return item;
        }

        public static NodeNaviTreeItem CreateNavigationBarItem(bool isDisplay, bool selected, int parentsCount, bool hasChildren, string text, string linkUrl, string target, bool enabled, string iconUrl)
        {
            NodeNaviTreeItem item = new NodeNaviTreeItem();
            item.isDisplay = isDisplay;
            item.selected = selected;
            item.parentsCount = parentsCount;
            item.hasChildren = hasChildren;
            item.text = text;
            item.linkUrl = linkUrl;
            item.target = target;
            item.enabled = enabled;
            item.isClickChange = true;
            item.isNodeTree = false;
            if (!string.IsNullOrEmpty(iconUrl))
            {
                item.iconFolderUrl = PageUtils.ParseNavigationUrl(iconUrl);
            }
            else
            {
                if (hasChildren)
                {
                    item.iconFolderUrl = PageUtils.ParseNavigationUrl("~/sitefiles/bairong/icons/menu/itemContainer.png");
                }
                else
                {
                    item.iconFolderUrl = PageUtils.ParseNavigationUrl("~/sitefiles/bairong/icons/menu/item.png");
                }
            }
            item.iconOpenedFolderUrl = item.iconFolderUrl;
            return item;
        }

        private NodeNaviTreeItem()
        {
            string treeDirectoryUrl = PageUtils.ParseNavigationUrl("~/sitefiles/bairong/icons/tree");
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
", displayHtml, this.parentsCount + 1, GetItemHtml());

            return trElementHtml;
        }

        public string GetItemHtml()
        {
            StringBuilder htmlBuilder = new StringBuilder();
            for (int i = 0; i < this.parentsCount; i++)
            {
                htmlBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}\"/>", this.iconEmptyUrl);
            }

            if (this.isDisplay)
            {
                if (this.hasChildren)
                {
                    if (this.selected)
                    {
                        htmlBuilder.AppendFormat("<img align=\"absmiddle\" style=\"cursor:pointer;\" onClick=\"displayChildren(this);\" isOpen=\"true\" src=\"{0}\"/>", this.iconMinusUrl);
                    }
                    else
                    {
                        htmlBuilder.AppendFormat("<img align=\"absmiddle\" style=\"cursor:pointer;\" onClick=\"displayChildren(this);\" isOpen=\"false\" src=\"{0}\"/>", this.iconPlusUrl);
                    }
                }
                else
                {
                    htmlBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}\"/>", this.iconEmptyUrl);
                }
            }
            else
            {
                if (this.hasChildren)
                {
                    htmlBuilder.AppendFormat("<img align=\"absmiddle\" style=\"cursor:pointer;\" onClick=\"displayChildren(this);\" isOpen=\"false\" src=\"{0}\"/>", this.iconPlusUrl);
                }
                else
                {
                    htmlBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}\"/>", this.iconEmptyUrl);
                }
            }

            if (!string.IsNullOrEmpty(this.iconFolderUrl))
            {
                if (this.nodeID > 0)
                {
                    htmlBuilder.AppendFormat("<a href=\"{0}\" target=\"_blank\" title='浏览页面'><img align=\"absmiddle\" border=\"0\" src=\"{1}\"/></a>", PageUtility.ServiceSTL.Utils.GetRedirectUrl(this.nodeID, true), this.iconFolderUrl);
                }
                else
                {
                    htmlBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}\"/>", this.iconFolderUrl);
                }
            }

            htmlBuilder.Append("&nbsp;");

            if (this.enabled)
            {
                if (!string.IsNullOrEmpty(this.linkUrl))
                {
                    string targetHtml = (string.IsNullOrEmpty(this.target)) ? string.Empty : string.Format("target='{0}'", this.target);
                    string clickChangeHtml = (this.isClickChange) ? "onclick='openFolderByA(this);'" : string.Empty;

                    htmlBuilder.AppendFormat("<a href='{0}' {1} {2} isTreeLink='true'>{3}</a>", this.linkUrl, targetHtml, clickChangeHtml, this.text);
                }
                else if (!string.IsNullOrEmpty(this.onClickUrl))
                {
                    htmlBuilder.AppendFormat(@"<a href=""javascript:;"" onClick=""{0}"" title='快速编辑栏目' isTreeLink='true'>{1}</a>", this.onClickUrl, this.text);
                }
                else
                {
                    htmlBuilder.Append(this.text);
                }
            }
            else
            {
                htmlBuilder.Append(this.text);
            }

            if (this.isNodeTree && this.publishmentSystemID != 0)
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(this.publishmentSystemID);

                htmlBuilder.Append("&nbsp;");
                htmlBuilder.Append(NodeManager.GetNodeTreeLastImageHtml(publishmentSystemInfo, NodeManager.GetNodeInfo(this.publishmentSystemID, this.nodeID)));

                if (this.contentNum >= 0)
                {
                    htmlBuilder.Append("&nbsp;");
                    htmlBuilder.AppendFormat("<span style=\"font-size:8pt;font-family:arial\" class=\"gray\">({0})</span>", this.contentNum);
                }
            }

            return htmlBuilder.ToString();
        }

        public string GetItemHtml(int parentContentNum)
        {
            StringBuilder htmlBuilder = new StringBuilder();
            for (int i = 0; i < this.parentsCount; i++)
            {
                htmlBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}\"/>", this.iconEmptyUrl);
            }

            if (this.isDisplay)
            {
                if (this.hasChildren)
                {
                    if (this.selected)
                    {
                        htmlBuilder.AppendFormat("<img align=\"absmiddle\" style=\"cursor:pointer;\" onClick=\"displayChildren(this);\" isOpen=\"true\" src=\"{0}\"/>", this.iconMinusUrl);
                    }
                    else
                    {
                        htmlBuilder.AppendFormat("<img align=\"absmiddle\" style=\"cursor:pointer;\" onClick=\"displayChildren(this);\" isOpen=\"false\" src=\"{0}\"/>", this.iconPlusUrl);
                    }
                }
                else
                {
                    htmlBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}\"/>", this.iconEmptyUrl);
                }
            }
            else
            {
                if (this.hasChildren)
                {
                    htmlBuilder.AppendFormat("<img align=\"absmiddle\" style=\"cursor:pointer;\" onClick=\"displayChildren(this);\" isOpen=\"false\" src=\"{0}\"/>", this.iconPlusUrl);
                }
                else
                {
                    htmlBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}\"/>", this.iconEmptyUrl);
                }
            }

            if (!string.IsNullOrEmpty(this.iconFolderUrl))
            {
                if (this.nodeID > 0)
                {
                    htmlBuilder.AppendFormat("<a href=\"{0}\" target=\"_blank\" title='浏览页面'><img align=\"absmiddle\" border=\"0\" src=\"{1}\"/></a>", PageUtility.ServiceSTL.Utils.GetRedirectUrl(this.nodeID, true), this.iconFolderUrl);
                }
                else
                {
                    htmlBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}\"/>", this.iconFolderUrl);
                }
            }

            htmlBuilder.Append("&nbsp;");

            if (this.enabled)
            {
                if (!string.IsNullOrEmpty(this.linkUrl))
                {
                    string targetHtml = (string.IsNullOrEmpty(this.target)) ? string.Empty : string.Format("target='{0}'", this.target);
                    string clickChangeHtml = (this.isClickChange) ? "onclick='openFolderByA(this);'" : string.Empty;

                    htmlBuilder.AppendFormat("<a href='{0}' {1} {2} isTreeLink='true'>{3}</a>", this.linkUrl, targetHtml, clickChangeHtml, this.text);
                }
                else if (!string.IsNullOrEmpty(this.onClickUrl))
                {
                    htmlBuilder.AppendFormat(@"<a href=""javascript:;"" onClick=""{0}"" title='快速编辑栏目' isTreeLink='true'>{1}</a>", this.onClickUrl, this.text);
                }
                else
                {
                    htmlBuilder.Append(this.text);
                }
            }
            else
            {
                htmlBuilder.Append(this.text);
            }

            if (this.isNodeTree && this.publishmentSystemID != 0)
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(this.publishmentSystemID);

                htmlBuilder.Append("&nbsp;");
                htmlBuilder.Append(NodeManager.GetNodeTreeLastImageHtml(publishmentSystemInfo, NodeManager.GetNodeInfo(this.publishmentSystemID, this.nodeID)));

                if (this.contentNum >= 0)
                {
                    htmlBuilder.Append("&nbsp;");
                    htmlBuilder.AppendFormat("<span style=\"font-size:8pt;font-family:arial\" class=\"gray\">(总共：{1},本级：{0})</span>", this.contentNum, parentContentNum);
                }
            }

            return htmlBuilder.ToString();
        }

        public static string GetNavigationBarScript()
        {
            return GetScript(false);
        }

        public static string GetNodeTreeScript()
        {
            return GetScript(true);
        }

        private static string GetScript(bool isNodeTree)
        {
            string script = @"
<script language=""JavaScript"">
//取得Tree的级别，1为第一级
function getTreeLevel(e) {
	var length = 0;
	if (!isNull(e)){
		if (e.tagName == 'TR') {
			length = parseInt(e.getAttribute('treeItemLevel'));
		}
	}
	return length;
}

function closeAllFolder(element){
    $(element).closest('table').find('td').removeClass('active');
}

function openFolderByA(element){
	closeAllFolder(element);
	if (isNull(element) || element.tagName != 'A') return;

	$(element).parent().addClass('active');

	if (isNodeTree){
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
		if (!isNull(element)){
			element.setAttribute('src', '{iconOpenedFolderUrl}');
		}
	}
}

function getTrElement(element){
	if (isNull(element)) return;
	for (element = element.parentNode;;){
		if (element != null && element.tagName == 'TR'){
			break;
		}else{
			element = element.parentNode;
		} 
	}
	return element;
}

function getImgClickableElementByTr(element){
	if (isNull(element) || element.tagName != 'TR') return;
	var img = null;
	if (!isNull(element.childNodes)){
		var imgCol = element.getElementsByTagName('IMG');
		if (!isNull(imgCol)){
			for (x=0;x<imgCol.length;x++){
				if (!isNull(imgCol.item(x).getAttribute('isOpen'))){
					img = imgCol.item(x);
					break;
				}
			}
		}
	}
	return img;
}

//显示、隐藏下级目录
function displayChildren(element){
	if (isNull(element)) return;

	var tr = getTrElement(element);

	var img = getImgClickableElementByTr(tr);//需要变换的加减图标

	if (!isNull(img) && img.getAttribute('isOpen') != null){
		if (img.getAttribute('isOpen') == 'false'){
			img.setAttribute('isOpen', 'true');
            img.setAttribute('src', '{iconMinusUrl}');
		}else{
            img.setAttribute('isOpen', 'false');
            img.setAttribute('src', '{iconPlusUrl}');
		}
	}

	var level = getTreeLevel(tr);//点击项菜单的级别
	
	var collection = new Array();
	var index = 0;

	for ( var e = tr.nextSibling; !isNull(e) ; e = e.nextSibling) {
		if (!isNull(e) && !isNull(e.tagName) && e.tagName == 'TR'){
		    var currentLevel = getTreeLevel(e);
		    if (currentLevel <= level) break;
		    if(e.style.display == '') {
			    e.style.display = 'none';
		    }else{//展开
			    if (currentLevel != level + 1) continue;
			    e.style.display = '';
			    var imgClickable = getImgClickableElementByTr(e);
			    if (!isNull(imgClickable)){
				    if (!isNull(imgClickable.getAttribute('isOpen')) && imgClickable.getAttribute('isOpen') =='true'){
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
			displayChildren(collection[i]);
		}
	}
}
var isNodeTree = {isNodeTree};
</script>
";
            NodeNaviTreeItem item = new NodeNaviTreeItem();
            script = script.Replace("{iconEmptyUrl}", item.iconEmptyUrl);
            script = script.Replace("{iconFolderUrl}", item.iconFolderUrl);
            script = script.Replace("{iconMinusUrl}", item.iconMinusUrl);
            script = script.Replace("{iconOpenedFolderUrl}", item.iconOpenedFolderUrl);
            script = script.Replace("{iconPlusUrl}", item.iconPlusUrl);
            script = script.Replace("{isNodeTree}", (isNodeTree) ? "true" : "false");
            return script;
        }

    }
}
