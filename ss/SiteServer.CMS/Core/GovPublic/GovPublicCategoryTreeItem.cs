using System.Text;
using BaiRong.Core;
using BaiRong.Model;
using System.Collections.Specialized;
using System;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
    public enum EGovPublicCategoryLoadingType
    {
        Tree,
        List,
        Select
    }

    public class EGovPublicCategoryLoadingTypeUtils
    {
        public static string GetValue(EGovPublicCategoryLoadingType type)
        {
            if (type == EGovPublicCategoryLoadingType.Tree)
            {
                return "Tree";
            }
            else if (type == EGovPublicCategoryLoadingType.Select)
            {
                return "Select";
            }
            else if (type == EGovPublicCategoryLoadingType.List)
            {
                return "List";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EGovPublicCategoryLoadingType GetEnumType(string typeStr)
        {
            EGovPublicCategoryLoadingType retval = EGovPublicCategoryLoadingType.List;

            if (Equals(EGovPublicCategoryLoadingType.Tree, typeStr))
            {
                retval = EGovPublicCategoryLoadingType.Tree;
            }
            else if (Equals(EGovPublicCategoryLoadingType.Select, typeStr))
            {
                retval = EGovPublicCategoryLoadingType.Select;
            }

            return retval;
        }

        public static bool Equals(EGovPublicCategoryLoadingType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EGovPublicCategoryLoadingType type)
        {
            return Equals(type, typeStr);
        }
    }

    public class GovPublicCategoryTreeItem
    {
        private string iconFolderUrl;
        private readonly string iconEmptyUrl;
        private readonly string iconMinusUrl;
        private readonly string iconPlusUrl;

        private bool enabled = true;
        private GovPublicCategoryInfo categoryInfo;

        public static GovPublicCategoryTreeItem CreateInstance(GovPublicCategoryInfo categoryInfo, bool enabled)
        {
            GovPublicCategoryTreeItem item = new GovPublicCategoryTreeItem();
            item.enabled = enabled;
            item.categoryInfo = categoryInfo;

            return item;
        }

        private GovPublicCategoryTreeItem()
        {
            string treeDirectoryUrl = PageUtils.GetIconUrl("tree");
            this.iconFolderUrl = PageUtils.Combine(treeDirectoryUrl, "folder.gif");
            this.iconEmptyUrl = PageUtils.Combine(treeDirectoryUrl, "empty.gif");
            this.iconMinusUrl = PageUtils.Combine(treeDirectoryUrl, "minus.png");
            this.iconPlusUrl = PageUtils.Combine(treeDirectoryUrl, "plus.png");
        }

        public string GetItemHtml(EGovPublicCategoryLoadingType loadingType)
        {
            StringBuilder htmlBuilder = new StringBuilder();
            int parentsCount = this.categoryInfo.ParentsCount;

            if (loadingType == EGovPublicCategoryLoadingType.Tree || loadingType == EGovPublicCategoryLoadingType.Select)
            {
                parentsCount = parentsCount + 1;
            }
            
            for (int i = 0; i < parentsCount; i++)
            {
                htmlBuilder.AppendFormat(@"<img align=""absmiddle"" src=""{0}"" />", this.iconEmptyUrl);
            }

            if (this.categoryInfo.ChildrenCount > 0)
            {
                htmlBuilder.AppendFormat(@"<img align=""absmiddle"" style=""cursor:pointer"" onClick=""displayChildren(this);"" isAjax=""true"" isOpen=""false"" id=""{0}"" src=""{1}"" />", this.categoryInfo.CategoryID, this.iconPlusUrl);
            }
            else
            {
                htmlBuilder.AppendFormat(@"<img align=""absmiddle"" src=""{0}"" />", this.iconEmptyUrl);
            }

            if (!string.IsNullOrEmpty(this.iconFolderUrl))
            {
                htmlBuilder.AppendFormat(@"<img align=""absmiddle"" src=""{0}"" />", this.iconFolderUrl);
            }

            htmlBuilder.Append("&nbsp;");

            if (this.enabled)
            {
                if (loadingType == EGovPublicCategoryLoadingType.Tree)
                {
                    string linkUrl = PageUtils.GetWCMUrl(string.Format("background_govPublicContent.aspx?PublishmentSystemID={0}&ClassCode={1}&CategoryID={2}", this.categoryInfo.PublishmentSystemID, this.categoryInfo.ClassCode, this.categoryInfo.CategoryID));

                    htmlBuilder.AppendFormat("<a href='{0}' isLink='true' onclick='fontWeightLink(this)' target='content'>{1}</a>", linkUrl, this.categoryInfo.CategoryName);

                }
                else if (loadingType == EGovPublicCategoryLoadingType.Select)
                {
                    string linkUrl = PageUtils.GetWCMUrl(string.Format("modal_govPublicCategorySelect.aspx?PublishmentSystemID={0}&ClassCode={1}&CategoryID={2}", this.categoryInfo.PublishmentSystemID, this.categoryInfo.ClassCode, this.categoryInfo.CategoryID));

                    htmlBuilder.AppendFormat("<a href='{0}'>{1}</a>", linkUrl, this.categoryInfo.CategoryName);
                }
                else
                {
                    htmlBuilder.Append(this.categoryInfo.CategoryName);
                }
            }
            else
            {
                htmlBuilder.Append(this.categoryInfo.CategoryName);
            }

            if (this.categoryInfo.ContentNum >= 0)
            {
                htmlBuilder.Append("&nbsp;");
                htmlBuilder.AppendFormat(@"<span style=""font-size:8pt;font-family:arial"" class=""gray"">({0})</span>", this.categoryInfo.ContentNum);
            }

            htmlBuilder.Replace("displayChildren", string.Format("displayChildren_{0}", this.categoryInfo.ClassCode));

            return htmlBuilder.ToString();
        }

        public static string GetScript(string classCode, int publishmentSystemID, EGovPublicCategoryLoadingType loadingType, NameValueCollection additional)
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

var completedClassID = null;
function displayChildren(img){
	if (isNull(img)) return;

	var tr = getTrElement(img);

    var isToOpen = img.getAttribute('isOpen') == 'false';
    var isByAjax = img.getAttribute('isAjax') == 'true';
    var classID = img.getAttribute('id');

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
        div.innerHTML = ""<img align='absmiddle' border='0' src='{iconLoadingUrl}' /> º”‘ÿ÷–£¨«Î…‘∫Ú..."";
        img.parentNode.appendChild(div);
        $(div).addClass('loading');
        loadingChannels(tr, img, div, classID);
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
            script += string.Format(@"
function loadingChannels(tr, img, div, classID){{
    var url = '{0}';
    var pars = 'classCode={1}&publishmentSystemID={2}&parentID=' + classID + '&loadingType={3}&additional={4}';

    jQuery.post(url, pars, function(data, textStatus)
    {{
        $($.parseHTML(data)).insertAfter($(tr));
        img.setAttribute('isAjax', 'false');
        img.parentNode.removeChild(div);
    }});
    completedClassID = classID;
}}

function loadingChannelsOnLoad(paths){{
    if (paths && paths.length > 0){{
        var nodeIDs = paths.split(',');
        var classID = nodeIDs[0];
        var img = $('#' + classID);
        if (img.attr('isOpen') == 'false'){{
            displayChildren(img[0]);
//            if (completedClassID && completedClassID == classID){{
//                if (paths.indexOf(',') != -1){{
//                    setTimeout(""loadingChannelsOnLoad("" + paths + "")"", 3000);
//                }}
//            }} 
        }}
    }}
}}
</script>
", PageUtility.GetCMSServiceUrlByPage(JsManager.CMSService.OtherServiceName, "GetLoadingGovPublicCategories"), classCode, publishmentSystemID, EGovPublicCategoryLoadingTypeUtils.GetValue(loadingType), RuntimeUtils.EncryptStringByTranslate(TranslateUtils.NameValueCollectionToString(additional)));

            GovPublicCategoryTreeItem item = new GovPublicCategoryTreeItem();
            script = script.Replace("{iconEmptyUrl}", item.iconEmptyUrl);
            script = script.Replace("{iconFolderUrl}", item.iconFolderUrl);
            script = script.Replace("{iconMinusUrl}", item.iconMinusUrl);
            script = script.Replace("{iconPlusUrl}", item.iconPlusUrl);

            script = script.Replace("{iconLoadingUrl}", PageUtils.GetIconUrl("loading.gif"));

            script = script.Replace("loadingChannels", string.Format("loadingChannels_{0}", classCode));
            script = script.Replace("displayChildren", string.Format("displayChildren_{0}", classCode));

            return script;
        }

        public static string GetScriptOnLoad(string path)
        {
            return string.Format(@"
<script language=""JavaScript"">
$(document).ready(function(){{
    loadingChannelsOnLoad('{0}');
}});
</script>
", path);
        }

    }
}
