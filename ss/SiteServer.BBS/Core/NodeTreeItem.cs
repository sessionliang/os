using System.Text;
using BaiRong.Core;
using System.Collections.Specialized;
using SiteServer.BBS.Model;

namespace SiteServer.BBS.Core
{
    internal class NodeTreeItem
    {
        private string iconFolderUrl;
        private string iconOpenedFolderUrl;
        private readonly string iconEmptyUrl;
        private readonly string iconMinusUrl;
        private readonly string iconPlusUrl;

        private readonly string iconBackgroundPublishNodeUrl;
        private readonly string iconBackgroundPublishNodeHQUrl;
        private readonly string iconBackgroundImageNodeUrl;

        private int publishmentSystemID;
        private bool enabled = true;
        private ForumInfo forumInfo;

        public static NodeTreeItem CreateInstance(int publishmentSystemID, ForumInfo forumInfo, bool enabled)
        {
            NodeTreeItem item = new NodeTreeItem();
            item.publishmentSystemID = publishmentSystemID;
            item.enabled = enabled;
            item.forumInfo = forumInfo;

            return item;
        }

        private NodeTreeItem()
        {
            string treeDirectoryUrl = PageUtils.GetIconUrl("tree");
            this.iconFolderUrl = PageUtils.Combine(treeDirectoryUrl, "folder.gif");
            this.iconOpenedFolderUrl = PageUtils.Combine(treeDirectoryUrl, "openedfolder.gif");
            this.iconEmptyUrl = PageUtils.Combine(treeDirectoryUrl, "empty.gif");
            this.iconMinusUrl = PageUtils.Combine(treeDirectoryUrl, "minus.gif");
            this.iconPlusUrl = PageUtils.Combine(treeDirectoryUrl, "plus.gif");

            this.iconBackgroundPublishNodeUrl = PageUtils.Combine(treeDirectoryUrl, "site.gif");
            this.iconBackgroundPublishNodeHQUrl = PageUtils.Combine(treeDirectoryUrl, "siteHQ.gif");
            this.iconBackgroundImageNodeUrl = PageUtils.Combine(treeDirectoryUrl, "image.gif");
        }

        public string GetItemHtml()
        {
            StringBuilder htmlBuilder = new StringBuilder();
            for (int i = 0; i < this.forumInfo.ParentsCount; i++)
            {
                htmlBuilder.AppendFormat(@"<img align=""absmiddle"" src=""{0}"" />", this.iconEmptyUrl);
            }

            if (this.forumInfo.ChildrenCount > 0)
            {
                htmlBuilder.AppendFormat(@"<img align=""absmiddle"" style=""cursor:pointer"" onClick=""displayChildren(this);"" isAjax=""false"" isOpen=""true"" id=""{0}"" src=""{1}"" />", this.forumInfo.ForumID, this.iconMinusUrl);
            }
            else
            {
                htmlBuilder.AppendFormat(@"<img align=""absmiddle"" src=""{0}"" />", this.iconEmptyUrl);
            }

            if (!string.IsNullOrEmpty(this.iconFolderUrl))
            {
                if (this.forumInfo.ForumID > 0)
                {
                    htmlBuilder.AppendFormat(@"<a href=""{0}"" target=""_blank"" title=""浏览""><img align=""absmiddle"" border=""0"" src=""{1}"" /></a>", PageUtilityBBS.GetForumUrl(this.publishmentSystemID, this.forumInfo), this.iconFolderUrl);
                }
                else
                {
                    htmlBuilder.AppendFormat(@"<img align=""absmiddle"" src=""{0}"" />", this.iconFolderUrl);
                }
            }

            htmlBuilder.Append("&nbsp;");

            if (this.enabled)
            {
                string onClickUrl = string.Empty;
                htmlBuilder.AppendFormat(@"<a href=""javascript:;"" onClick=""{0}"" title=""快速编辑板块"">{1}</a>", onClickUrl, this.forumInfo.ForumName);
            }
            else
            {
                htmlBuilder.Append(this.forumInfo.ForumName);
            }

            htmlBuilder.Append("&nbsp;");

            if (this.forumInfo.ParentID == 0)
            {
                htmlBuilder.Append("&nbsp;");
                htmlBuilder.Append(@"<span style=""font-size:8pt;font-family:arial;color:#f26c4f"">(分区)</span>");
            }
            else
            {
                htmlBuilder.Append("&nbsp;");
                if (string.IsNullOrEmpty(this.forumInfo.LinkUrl))
                {
                    htmlBuilder.AppendFormat(@"<span style=""font-size:8pt;font-family:arial;color:gray"">({0})</span>", this.forumInfo.ThreadCount);
                }
                else
                {
                    htmlBuilder.Append(@"<span style=""font-size:8pt;font-family:arial;color:gray"">(链接外部地址)</span>");
                }
            }
            
            return htmlBuilder.ToString();
        }

        public static string GetScript(NameValueCollection additional)
        {
            string script = @"
<script language=""JavaScript"">
function getTreeLevel(e) {
	var length = 0;
	if (!isNull(e)){
		if (e.tagName == 'TR') {
			length = parseInt(e.getAttribute('treeItemLevel'));
		}
	}
	return length;
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
function displayChildren(img){
	if (isNull(img)) return;

	var tr = getTrElement(img);

    var isToOpen = img.getAttribute('isOpen') == 'false';
    var nodeID = img.getAttribute('id');

	if (!isNull(img) && img.getAttribute('isOpen') != null){
		if (img.getAttribute('isOpen') == 'false'){
			img.setAttribute('isOpen', 'true');
            img.setAttribute('src', '{iconMinusUrl}');
		}else{
            img.setAttribute('isOpen', 'false');
            img.setAttribute('src', '{iconPlusUrl}');
		}
	}

    var level = getTreeLevel(tr);
    	
	var collection = new Array();
	var index = 0;

	for ( var e = tr.nextSibling; !isNull(e) ; e = e.nextSibling) {
		if (!isNull(e) && !isNull(e.tagName) && e.tagName == 'TR'){
		    var currentLevel = getTreeLevel(e);
		    if (currentLevel <= level) break;
		    if(e.style.display == '') {
			    e.style.display = 'none';
		    }else{
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
</script>
";

            NodeTreeItem item = new NodeTreeItem();
            script = script.Replace("{iconEmptyUrl}", item.iconEmptyUrl);
            script = script.Replace("{iconFolderUrl}", item.iconFolderUrl);
            script = script.Replace("{iconMinusUrl}", item.iconMinusUrl);
            script = script.Replace("{iconOpenedFolderUrl}", item.iconOpenedFolderUrl);
            script = script.Replace("{iconPlusUrl}", item.iconPlusUrl);

            script = script.Replace("{iconLoadingUrl}", PageUtils.GetIconUrl("loading.gif"));
            return script;
        }

        public static string GetScriptOnLoad(string path)
        {
            return string.Format(@"
<script language=""JavaScript"">
Event.observe(window,'load', function(){{
    loadingChannelsOnLoad('{0}');
}});
</script>
", path);
        }

    }
}
