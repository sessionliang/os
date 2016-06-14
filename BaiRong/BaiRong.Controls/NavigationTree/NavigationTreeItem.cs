using System.Text;
using BaiRong.Core;
using BaiRong.Model;

namespace BaiRong.Controls
{
	public class NavigationTreeItem
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
        private bool openWindow = false;
		private string text = string.Empty;
		private string linkUrl = string.Empty;
		private string onClickUrl = string.Empty;
		private string target = string.Empty;
		private bool enabled = true;
		private bool isClickChange = false;
		private int contentNum = 0;

        public static NavigationTreeItem CreateNodeTreeItem(bool isDisplay, bool selected, int parentsCount, bool hasChildren, bool openWindow, string text, string linkUrl, string onClickUrl, string target, bool enabled, bool isClickChange, int contentNum)
		{
			NavigationTreeItem item = new NavigationTreeItem();
			item.isDisplay = isDisplay;
			item.selected = selected;
			item.parentsCount = parentsCount;
			item.hasChildren = hasChildren;
            item.openWindow = openWindow;
			item.text = text;
			item.linkUrl = linkUrl;
			item.onClickUrl = onClickUrl;
			item.target = target;
			item.enabled = enabled;
			item.isClickChange = isClickChange;
			item.contentNum = contentNum;
			return item;
		}

        public static NavigationTreeItem CreateNavigationBarItem(bool isDisplay, bool selected, int parentsCount, bool hasChildren, bool openWindow, string text, string linkUrl, string target, bool enabled, string iconUrl)
		{
			NavigationTreeItem item = new NavigationTreeItem();
			item.isDisplay = isDisplay;
			item.selected = selected;
			item.parentsCount = parentsCount;
			item.hasChildren = hasChildren;
            item.openWindow = openWindow;
			item.text = text;
			item.linkUrl = linkUrl;
			item.target = target;
			item.enabled = enabled;
			item.isClickChange = true;
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

        private NavigationTreeItem()
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
            string displayHtml = (this.isDisplay) ? StringUtils.Constants.SHOW_ELEMENT_STYLE : StringUtils.Constants.HIDE_ELEMENT_STYLE;
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
                htmlBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}\"/>", this.iconFolderUrl);
			}

			htmlBuilder.Append("&nbsp;");

            if (this.enabled)
            {
                if (!string.IsNullOrEmpty(this.linkUrl))
                {
                    string targetHtml = (string.IsNullOrEmpty(this.target)) ? string.Empty : string.Format("target='{0}'", this.target);
                    string clickChangeHtml = (this.isClickChange) ? "onclick='openFolderByA(this);'" : string.Empty;

                    htmlBuilder.AppendFormat("<a href='{0}' {1} {2} isTreeLink='true'><lan>{3}</lan>", this.linkUrl, targetHtml, clickChangeHtml, this.text);

                    htmlBuilder.Append("</a>");

                    if (openWindow)
                    {
                        htmlBuilder.AppendFormat(@"&nbsp;<a href='{0}' target='_blank' title='�´��ڴ�'><img src=""{1}"" /></a>", this.linkUrl, PageUtils.ParseNavigationUrl("~/sitefiles/bairong/icons/tree/open.png"));
                    }
                }
                else if (!string.IsNullOrEmpty(this.onClickUrl))
                {
                    htmlBuilder.AppendFormat(@"<a href=""javascript:;"" onClick=""{0}"" title='���ٱ༭��Ŀ' isTreeLink='true'>{1}</a>", this.onClickUrl, this.text);
                }
                else
                {
                    htmlBuilder.AppendFormat("<lan>{0}</lan>", this.text);
                }
            }
            else
            {
                htmlBuilder.AppendFormat("<lan>{0}</lan>", this.text);
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
//ȡ��Tree�ļ���1Ϊ��һ��
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

//��ʾ�������¼�Ŀ¼
function displayChildren(element){
	if (isNull(element)) return;

	var tr = getTrElement(element);

	var img = getImgClickableElementByTr(tr);//��Ҫ�任�ļӼ�ͼ��

	if (!isNull(img) && img.getAttribute('isOpen') != null){
		if (img.getAttribute('isOpen') == 'false'){
			img.setAttribute('isOpen', 'true');
            img.setAttribute('src', '{iconMinusUrl}');
		}else{
            img.setAttribute('isOpen', 'false');
            img.setAttribute('src', '{iconPlusUrl}');
		}
	}

	var level = getTreeLevel(tr);//�����˵��ļ���
	
	var collection = new Array();
	var index = 0;

	for ( var e = tr.nextSibling; !isNull(e) ; e = e.nextSibling) {
		if (!isNull(e) && !isNull(e.tagName) && e.tagName == 'TR'){
		    var currentLevel = getTreeLevel(e);
		    if (currentLevel <= level) break;
		    if(e.style.display == '') {
			    e.style.display = 'none';
		    }else{//չ��
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
			NavigationTreeItem item = new NavigationTreeItem();
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
