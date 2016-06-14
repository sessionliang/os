using System.Text;
using BaiRong.Core;
using BaiRong.Model;
using System.Collections.Specialized;
using System;

namespace BaiRong.Core
{
    public enum EDepartmentLoadingType
    {
        AdministratorTree,                       //platform/background_departmentTree.aspx 右侧 background_administrator.aspx
        ContentList,                       //platform/background_department.aspx
        DepartmentSelect,                          //wcm/modal_govPublicCategoryDepartmentSelect.aspx
        ContentTree,                             //wcm/background_govPublicContentTree.aspx
        GovPublicDepartment,                     //wcm/background_govPublicDepartment.aspx
        List                                    //纯展示名称
    }

    public class EDepartmentLoadingTypeUtils
    {
        public static string GetValue(EDepartmentLoadingType type)
        {
            if (type == EDepartmentLoadingType.AdministratorTree)
            {
                return "AdministratorTree";
            }
            else if (type == EDepartmentLoadingType.ContentList)
            {
                return "ContentList";
            }
            else if (type == EDepartmentLoadingType.DepartmentSelect)
            {
                return "DepartmentSelect";
            }
            else if (type == EDepartmentLoadingType.ContentTree)
            {
                return "ContentTree";
            }
            else if (type == EDepartmentLoadingType.GovPublicDepartment)
            {
                return "GovPublicDepartment";
            }
            else if (type == EDepartmentLoadingType.List)
            {
                return "List";
            }           
            else
            {
                throw new Exception();
            }
        }

        public static EDepartmentLoadingType GetEnumType(string typeStr)
        {
            EDepartmentLoadingType retval = EDepartmentLoadingType.AdministratorTree;

            if (Equals(EDepartmentLoadingType.AdministratorTree, typeStr))
            {
                retval = EDepartmentLoadingType.AdministratorTree;
            }
            else if (Equals(EDepartmentLoadingType.ContentList, typeStr))
            {
                retval = EDepartmentLoadingType.ContentList;
            }
            else if (Equals(EDepartmentLoadingType.DepartmentSelect, typeStr))
            {
                retval = EDepartmentLoadingType.DepartmentSelect;
            }
            else if (Equals(EDepartmentLoadingType.ContentTree, typeStr))
            {
                retval = EDepartmentLoadingType.ContentTree;
            }
            else if (Equals(EDepartmentLoadingType.GovPublicDepartment, typeStr))
            {
                retval = EDepartmentLoadingType.GovPublicDepartment;
            }
            else if (Equals(EDepartmentLoadingType.List, typeStr))
            {
                retval = EDepartmentLoadingType.List;
            }

            return retval;
        }

        public static bool Equals(EDepartmentLoadingType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EDepartmentLoadingType type)
        {
            return Equals(type, typeStr);
        }
    }

    public class DepartmentTreeItem
    {
        private string iconFolderUrl;
        private readonly string iconEmptyUrl;
        private readonly string iconMinusUrl;
        private readonly string iconPlusUrl;

        private DepartmentInfo departmentInfo;

        public static DepartmentTreeItem CreateInstance(DepartmentInfo departmentInfo)
        {
            DepartmentTreeItem item = new DepartmentTreeItem();
            item.departmentInfo = departmentInfo;

            return item;
        }

        private DepartmentTreeItem()
        {
            string treeDirectoryUrl = PageUtils.GetIconUrl("tree");
            this.iconFolderUrl = PageUtils.Combine(treeDirectoryUrl, "folder.gif");
            this.iconEmptyUrl = PageUtils.Combine(treeDirectoryUrl, "empty.gif");
            this.iconMinusUrl = PageUtils.Combine(treeDirectoryUrl, "minus.png");
            this.iconPlusUrl = PageUtils.Combine(treeDirectoryUrl, "plus.png");
        }

        public string GetItemHtml(EDepartmentLoadingType loadingType, NameValueCollection additional, bool isOpen)
        {
            StringBuilder htmlBuilder = new StringBuilder();
            int parentsCount = this.departmentInfo.ParentsCount;

            if (loadingType == EDepartmentLoadingType.AdministratorTree || loadingType == EDepartmentLoadingType.DepartmentSelect || loadingType == EDepartmentLoadingType.ContentTree)
            {
                parentsCount = parentsCount + 1;
            }
            
            for (int i = 0; i < parentsCount; i++)
            {
                htmlBuilder.AppendFormat(@"<img align=""absmiddle"" src=""{0}"" />", this.iconEmptyUrl);
            }

            if (this.departmentInfo.ChildrenCount > 0)
            {
                if (isOpen)
                {
                    htmlBuilder.AppendFormat(@"<img align=""absmiddle"" style=""cursor:pointer"" onClick=""displayChildren(this);"" isAjax=""false"" isOpen=""true"" id=""{0}"" src=""{1}"" />", this.departmentInfo.DepartmentID, this.iconMinusUrl);
                }
                else
                {
                    htmlBuilder.AppendFormat(@"<img align=""absmiddle"" style=""cursor:pointer"" onClick=""displayChildren(this);"" isAjax=""true"" isOpen=""false"" id=""{0}"" src=""{1}"" />", this.departmentInfo.DepartmentID, this.iconPlusUrl);
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

            if (loadingType == EDepartmentLoadingType.AdministratorTree)
            {
                string module = additional["module"];
                string linkUrl = PageUtils.GetPlatformUrl(string.Format("background_administrator.aspx?DepartmentID={0}&module={1}", departmentInfo.DepartmentID, module));

                htmlBuilder.AppendFormat("<a href='{0}' isLink='true' onclick='fontWeightLink(this)' target='department'>{1}</a>", linkUrl, this.departmentInfo.DepartmentName);
            }
            else if (loadingType == EDepartmentLoadingType.DepartmentSelect)
            {
                string linkUrl = string.Format(additional["UrlFormatString"], departmentInfo.DepartmentID);
                //string linkUrl = string.Format("modal_govPublicCategoryDepartmentSelect.aspx?PublishmentSystemID={0}&DepartmentID={1}", additional["PublishmentSystemID"], departmentInfo.DepartmentID);

                htmlBuilder.AppendFormat("<a href='{0}'>{1}</a>", linkUrl, this.departmentInfo.DepartmentName);
            }
            else if (loadingType == EDepartmentLoadingType.ContentTree)
            {
                string linkUrl = PageUtils.GetWCMUrl(string.Format("background_govPublicContent.aspx?PublishmentSystemID={0}&DepartmentID={1}", additional["PublishmentSystemID"], departmentInfo.DepartmentID));

                htmlBuilder.AppendFormat("<a href='{0}' isLink='true' onclick='fontWeightLink(this)' target='content'>{1}</a>", linkUrl, this.departmentInfo.DepartmentName);
            }
            else
            {
                htmlBuilder.Append(this.departmentInfo.DepartmentName);
            }

            if (loadingType == EDepartmentLoadingType.AdministratorTree)
            {
                if (this.departmentInfo.CountOfAdmin >= 0)
                {
                    htmlBuilder.Append("&nbsp;");
                    htmlBuilder.AppendFormat(@"<span style=""font-size:8pt;font-family:arial"" class=""gray"">({0})</span>", this.departmentInfo.CountOfAdmin);
                }
            }

            htmlBuilder.Replace("displayChildren", "displayChildren_Department");

            return htmlBuilder.ToString();
        }

        public static string GetScript(EDepartmentLoadingType loadingType, NameValueCollection additional)
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
        div.innerHTML = ""<img align='absmiddle' border='0' src='{iconLoadingUrl}' /> 栏目加载中，请稍候..."";
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
", PageUtils.GetPlatformSystemServiceUrl("GetLoadingDepartments"), EDepartmentLoadingTypeUtils.GetValue(loadingType), RuntimeUtils.EncryptStringByTranslate(TranslateUtils.NameValueCollectionToString(additional)));

            DepartmentTreeItem item = new DepartmentTreeItem();
            script = script.Replace("{iconEmptyUrl}", item.iconEmptyUrl);
            script = script.Replace("{iconFolderUrl}", item.iconFolderUrl);
            script = script.Replace("{iconMinusUrl}", item.iconMinusUrl);
            script = script.Replace("{iconPlusUrl}", item.iconPlusUrl);

            script = script.Replace("{iconLoadingUrl}", PageUtils.GetIconUrl("loading.gif"));

            script = script.Replace("loadingChannels", "loadingChannels_Department");
            script = script.Replace("displayChildren", "displayChildren_Department");

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
