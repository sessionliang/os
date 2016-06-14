using System.Text;
using BaiRong.Core;
using BaiRong.Model;
using System.Collections.Specialized;
using System;

namespace BaiRong.Core
{
    public enum EAreaLoadingType
    {
        Management,                     //platform/background_area.aspx
    }

    public class EAreaLoadingTypeUtils
    {
        public static string GetValue(EAreaLoadingType type)
        {
            if (type == EAreaLoadingType.Management)
            {
                return "Management";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EAreaLoadingType GetEnumType(string typeStr)
        {
            EAreaLoadingType retval = EAreaLoadingType.Management;

            if (Equals(EAreaLoadingType.Management, typeStr))
            {
                retval = EAreaLoadingType.Management;
            }

            return retval;
        }

        public static bool Equals(EAreaLoadingType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EAreaLoadingType type)
        {
            return Equals(type, typeStr);
        }
    }

    public class AreaTreeItem
    {
        private string iconFolderUrl;
        private readonly string iconEmptyUrl;
        private readonly string iconMinusUrl;
        private readonly string iconPlusUrl;

        private AreaInfo areaInfo;

        public static AreaTreeItem CreateInstance(AreaInfo areaInfo)
        {
            AreaTreeItem item = new AreaTreeItem();
            item.areaInfo = areaInfo;

            return item;
        }

        private AreaTreeItem()
        {
            string treeDirectoryUrl = PageUtils.GetIconUrl("tree");
            this.iconFolderUrl = PageUtils.Combine(treeDirectoryUrl, "folder.gif");
            this.iconEmptyUrl = PageUtils.Combine(treeDirectoryUrl, "empty.gif");
            this.iconMinusUrl = PageUtils.Combine(treeDirectoryUrl, "minus.png");
            this.iconPlusUrl = PageUtils.Combine(treeDirectoryUrl, "plus.png");
        }

        public string GetItemHtml(EAreaLoadingType loadingType, NameValueCollection additional, bool isOpen)
        {
            StringBuilder htmlBuilder = new StringBuilder();
            int parentsCount = this.areaInfo.ParentsCount;
            
            for (int i = 0; i < parentsCount; i++)
            {
                htmlBuilder.AppendFormat(@"<img align=""absmiddle"" src=""{0}"" />", this.iconEmptyUrl);
            }

            if (this.areaInfo.ChildrenCount > 0)
            {
                if (isOpen)
                {
                    htmlBuilder.AppendFormat(@"<img align=""absmiddle"" style=""cursor:pointer"" onClick=""displayChildren(this);"" isAjax=""false"" isOpen=""true"" id=""{0}"" src=""{1}"" />", this.areaInfo.AreaID, this.iconMinusUrl);
                }
                else
                {
                    htmlBuilder.AppendFormat(@"<img align=""absmiddle"" style=""cursor:pointer"" onClick=""displayChildren(this);"" isAjax=""true"" isOpen=""false"" id=""{0}"" src=""{1}"" />", this.areaInfo.AreaID, this.iconPlusUrl);
                }
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

            htmlBuilder.Append(this.areaInfo.AreaName);

            htmlBuilder.Replace("displayChildren", "displayChildren_Area");

            return htmlBuilder.ToString();
        }

        public static string GetScript(EAreaLoadingType loadingType, NameValueCollection additional)
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
    var isByAjax = img.getAttribute('isAjax') == 'true';
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

    if (isToOpen && isByAjax)
    {
        var div = document.createElement('div');
        div.innerHTML = ""<img align='absmiddle' border='0' src='{iconLoadingUrl}' /> ¿∏ƒøº”‘ÿ÷–£¨«Î…‘∫Ú..."";
        img.parentNode.appendChild(div);
        $(div).addClass('loading');
        loadingChannels(tr, img, div, nodeID);
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
function loadingChannels(tr, img, div, nodeID){{
    var url = '{0}';
    var pars = 'parentID=' + nodeID + '&loadingType={1}&additional={2}';

    jQuery.post(url, pars, function(data, textStatus)
    {{
        $($.parseHTML(data)).insertAfter($(tr));
        img.setAttribute('isAjax', 'false');
        img.parentNode.removeChild(div);
    }});
    completedNodeID = nodeID;
}}

function loadingChannelsOnLoad(paths){{
    if (paths && paths.length > 0){{
        var nodeIDs = paths.split(',');
        var nodeID = nodeIDs[0];
        var img = $('#' + nodeID);
        if (img.attr('isOpen') == 'false'){{
            displayChildren(img[0]);
//            if (completedNodeID && completedNodeID == nodeID){{
//                if (paths.indexOf(',') != -1){{
//                    setTimeout(""loadingChannelsOnLoad("" + paths + "")"", 3000);
//                }}
//            }} 
        }}
    }}
}}
</script>
", PageUtils.GetPlatformSystemServiceUrl("GetLoadingAreas"), EAreaLoadingTypeUtils.GetValue(loadingType), RuntimeUtils.EncryptStringByTranslate(TranslateUtils.NameValueCollectionToString(additional)));

            AreaTreeItem item = new AreaTreeItem();
            script = script.Replace("{iconEmptyUrl}", item.iconEmptyUrl);
            script = script.Replace("{iconFolderUrl}", item.iconFolderUrl);
            script = script.Replace("{iconMinusUrl}", item.iconMinusUrl);
            script = script.Replace("{iconPlusUrl}", item.iconPlusUrl);

            script = script.Replace("{iconLoadingUrl}", PageUtils.GetIconUrl("loading.gif"));

            script = script.Replace("loadingChannels", "loadingChannels_Area");
            script = script.Replace("displayChildren", "displayChildren_Area");

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
