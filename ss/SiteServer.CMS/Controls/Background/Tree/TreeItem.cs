using System.Text;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Model;
using System.Collections.Specialized;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Core.Security;
using SiteServer.CMS.Controls;

namespace SiteServer.CMS.Core
{
    public class TreeItem
    {
        private string iconFolderUrl;
        private string iconOpenedFolderUrl;
        private readonly string iconEmptyUrl;
        private readonly string iconMinusUrl;
        private readonly string iconPlusUrl;

        private string classifyType;

        private bool enabled = true;
        private TreeBaseItem itemInfo;

        public static TreeItem CreateInstance(TreeBaseItem itemInfo, string classifyType)
        {
            TreeItem item = new TreeItem();
            item.itemInfo = itemInfo;
            item.classifyType = classifyType;
            return item;
        }

        private TreeItem()
        {
            string treeDirectoryUrl = PageUtils.GetIconUrl("tree");
            this.iconFolderUrl = PageUtils.Combine(treeDirectoryUrl, "folder.gif");
            this.iconOpenedFolderUrl = PageUtils.Combine(treeDirectoryUrl, "openedfolder.gif");
            this.iconEmptyUrl = PageUtils.Combine(treeDirectoryUrl, "empty.gif");
            this.iconMinusUrl = PageUtils.Combine(treeDirectoryUrl, "minus.png");
            this.iconPlusUrl = PageUtils.Combine(treeDirectoryUrl, "plus.png");
        }

        public string GetItemHtml(string redirectUrl, NameValueCollection additional, bool isOpen, bool showCount, int showLayer)
        {
            StringBuilder htmlBuilder = new StringBuilder();
            int parentsCount = this.itemInfo.ParentsCount;

            for (int i = 0; i < parentsCount; i++)
            {
                htmlBuilder.AppendFormat(@"<img align=""absmiddle"" src=""{0}"" />", this.iconEmptyUrl);
            }

            if (showLayer == 0)
            {
                //ƒ¨»œ◊¥Ã¨
                if (this.itemInfo.ChildrenCount > 0)
                {
                    if (isOpen)
                    {
                        htmlBuilder.AppendFormat(@"<img align=""absmiddle"" style=""cursor:pointer"" onClick=""displayChildren(this);"" isAjax=""false"" isOpen=""true"" id=""{0}"" src=""{1}"" />", this.itemInfo.ItemID, this.iconMinusUrl);
                    }
                    else
                    {
                        htmlBuilder.AppendFormat(@"<img align=""absmiddle"" style=""cursor:pointer"" onClick=""displayChildren(this);"" isAjax=""true"" isOpen=""false"" id=""{0}"" src=""{1}"" />", this.itemInfo.ItemID, this.iconPlusUrl);
                    }
                }
                else
                {
                    htmlBuilder.AppendFormat(@"<img align=""absmiddle"" src=""{0}"" />", this.iconEmptyUrl);
                }
            }
            else if (this.itemInfo.ChildrenCount > 0 && this.itemInfo.ParentsCount < showLayer - 1)
            {
                if (isOpen)
                {
                    htmlBuilder.AppendFormat(@"<img align=""absmiddle"" style=""cursor:pointer"" onClick=""displayChildren(this);"" isAjax=""false"" isOpen=""true"" id=""{0}"" src=""{1}"" />", this.itemInfo.ItemID, this.iconMinusUrl);
                }
                else
                {
                    htmlBuilder.AppendFormat(@"<img align=""absmiddle"" style=""cursor:pointer"" onClick=""displayChildren(this);"" isAjax=""true"" isOpen=""false"" id=""{0}"" src=""{1}"" />", this.itemInfo.ItemID, this.iconPlusUrl);
                }
            }
            else
            {
                htmlBuilder.AppendFormat(@"<img align=""absmiddle"" src=""{0}"" />", this.iconEmptyUrl);
            }

            if (!string.IsNullOrEmpty(redirectUrl))
            {
                if (this.itemInfo.ItemID > 0)
                {
                    htmlBuilder.AppendFormat(@"<a href=""{0}"" target=""_blank"" title=""‰Ø¿¿“≥√Ê""><img align=""absmiddle"" border=""0"" src=""{1}"" /></a>", redirectUrl, this.iconFolderUrl);
                }
                else
                {
                    htmlBuilder.AppendFormat(@"<img align=""absmiddle"" src=""{0}"" />", this.iconFolderUrl);
                }
            }

            htmlBuilder.Append("&nbsp;");

            if (this.enabled)
            {
                string linkUrl = string.Empty;
                if (additional != null)
                {
                    if (!string.IsNullOrEmpty(additional["linkUrl"]))
                    {
                        linkUrl = additional["linkUrl"];
                    }

                    //additional.Remove("linkUrl");
                    if (!string.IsNullOrEmpty(linkUrl))
                        linkUrl = PageUtils.AddQueryString(linkUrl, additional);
                    else
                        linkUrl = "javascript:;";
                }
                htmlBuilder.AppendFormat("<a href='{0}' isLink='true' onclick='fontWeightLink(this)' target='content'>{1}</a>", linkUrl, this.itemInfo.ItemName);


                if (showCount && this.itemInfo.ContentNum >= 0)
                {
                    htmlBuilder.Append("&nbsp;");
                    htmlBuilder.AppendFormat(@"<span style=""font-size:8pt;font-family:arial"" class=""gray"">({0})</span>", this.itemInfo.ContentNum);
                }
            }
            else
            {
                htmlBuilder.Append(this.itemInfo.ItemName);
            }

            return htmlBuilder.ToString();
        }

        public string GetItemHtml(string redirectUrl, NameValueCollection additional, bool isOpen)
        {
            return GetItemHtml(redirectUrl, additional, isOpen, true, 0);
        }

        public string GetItemHtml(string redirectUrl, NameValueCollection additional, bool isOpen, int showLayer)
        {
            return GetItemHtml(redirectUrl, additional, isOpen, true, showLayer);
        }

        public static string GetScript(PublishmentSystemInfo publishmentSystemInfo, NameValueCollection additional, string classifyType, string actionType)
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
	if (!isNull(element.childItems)){
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

var completedItemID = null;
function displayChildren(img){
	if (isNull(img)) return;

	var tr = getTrElement(img);

    var isToOpen = img.getAttribute('isOpen') == 'false';
    var isByAjax = img.getAttribute('isAjax') == 'true';
    var itemID = img.getAttribute('id');

	if (!isNull(img) && img.getAttribute('isOpen') != null){
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
        div.innerHTML = ""<img align='absmiddle' border='0' src='{iconLoadingUrl}' /> ∑÷¿‡º”‘ÿ÷–£¨«Î…‘∫Ú..."";
        img.parentNode.appendChild(div);
        $(div).addClass('loading');
        loadingClassifys(tr, img, div, itemID);
    }
    else
    {
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
}
";
            string ajaxUrl = PageUtility.GetCMSServiceUrlByPage(JsManager.CMSService.OtherServiceName, "GetLoadingClassify");

            script += string.Format(@"
function loadingClassifys(tr, img, div, itemID){{
    var url = '{0}';
    var pars = 'publishmentSystemID={1}&parentID=' + itemID + '&additional={2}&classifyType={3}&actionType={4}';

    jQuery.post(url, pars, function(data, textStatus)
    {{
        $($.parseHTML(data)).insertAfter($(tr));
        img.setAttribute('isAjax', 'false');
        img.parentNode.removeChild(div);
    }});
    completedItemID = itemID;
}}

function loadingClassifysOnLoad(paths){{
    if (paths && paths.length > 0){{
        var itemIDs = paths.split(',');
        var itemID = itemIDs[0];
        var img = $('#' + itemID);
        if (img.attr('isOpen') == 'false'){{
            displayChildren(img[0]);
            if (completedItemID && completedItemID == itemID){{
                if (paths.indexOf(',') != -1){{
paths = paths.substring(paths.indexOf(',') + 1);
                    setTimeout(""loadingClassifysOnLoad('"" + paths + ""')"", 1000);
                }}
            }} 
        }}
    }}
}}
</script>
", ajaxUrl, publishmentSystemInfo.PublishmentSystemID, RuntimeUtils.EncryptStringByTranslate(TranslateUtils.NameValueCollectionToString(additional)), classifyType, actionType);

            TreeItem item = new TreeItem();
            script = script.Replace("{iconEmptyUrl}", item.iconEmptyUrl);
            script = script.Replace("{iconFolderUrl}", item.iconFolderUrl);
            script = script.Replace("{iconMinusUrl}", item.iconMinusUrl);
            script = script.Replace("{iconOpenedFolderUrl}", item.iconOpenedFolderUrl);
            script = script.Replace("{iconPlusUrl}", item.iconPlusUrl);

            script = script.Replace("{iconLoadingUrl}", PageUtils.GetIconUrl("loading.gif"));
            return script;
        }

    }
}
