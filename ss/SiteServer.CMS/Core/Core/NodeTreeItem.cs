using System.Text;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Model;
using System.Collections.Specialized;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Core.Security;

namespace SiteServer.CMS.Core
{
    public class NodeTreeItem
    {
        private string iconFolderUrl;
        private string iconOpenedFolderUrl;
        private readonly string iconEmptyUrl;
        private readonly string iconMinusUrl;
        private readonly string iconPlusUrl;

        private bool enabled = true;
        private NodeInfo nodeInfo;

        public static NodeTreeItem CreateInstance(NodeInfo nodeInfo, bool enabled)
        {
            NodeTreeItem item = new NodeTreeItem();
            item.enabled = enabled;
            item.nodeInfo = nodeInfo;

            return item;
        }

        private NodeTreeItem()
        {
            string treeDirectoryUrl = PageUtils.GetIconUrl("tree");
            this.iconFolderUrl = PageUtils.Combine(treeDirectoryUrl, "folder.gif");
            this.iconOpenedFolderUrl = PageUtils.Combine(treeDirectoryUrl, "openedfolder.gif");
            this.iconEmptyUrl = PageUtils.Combine(treeDirectoryUrl, "empty.gif");
            this.iconMinusUrl = PageUtils.Combine(treeDirectoryUrl, "minus.png");
            this.iconPlusUrl = PageUtils.Combine(treeDirectoryUrl, "plus.png");
        }

        public string GetItemHtml(ELoadingType loadingType, string returnUrl, NameValueCollection additional)
        {
            StringBuilder htmlBuilder = new StringBuilder();
            int parentsCount = this.nodeInfo.ParentsCount;
            if (loadingType == ELoadingType.GovPublicChannelAdd || loadingType == ELoadingType.GovPublicChannelTree)
            {
                parentsCount = parentsCount - 1;
            }
            else if (loadingType == ELoadingType.GovPublicChannel || loadingType == ELoadingType.GovInteractChannel)
            {
                parentsCount = parentsCount - 2;
            }
            for (int i = 0; i < parentsCount; i++)
            {
                htmlBuilder.AppendFormat(@"<img align=""absmiddle"" src=""{0}"" />", this.iconEmptyUrl);
            }

            if (this.nodeInfo.ChildrenCount > 0)
            {
                if (nodeInfo.PublishmentSystemID == nodeInfo.NodeID)
                {
                    htmlBuilder.AppendFormat(@"<img align=""absmiddle"" style=""cursor:pointer"" onClick=""displayChildren(this);"" isAjax=""false"" isOpen=""true"" id=""{0}"" src=""{1}"" />", this.nodeInfo.NodeID, this.iconMinusUrl);
                }
                else
                {
                    htmlBuilder.AppendFormat(@"<img align=""absmiddle"" style=""cursor:pointer"" onClick=""displayChildren(this);"" isAjax=""true"" isOpen=""false"" id=""{0}"" src=""{1}"" />", this.nodeInfo.NodeID, this.iconPlusUrl);
                }
            }
            else
            {
                htmlBuilder.AppendFormat(@"<img align=""absmiddle"" src=""{0}"" />", this.iconEmptyUrl);
            }

            if (!string.IsNullOrEmpty(this.iconFolderUrl))
            {
                if (this.nodeInfo.NodeID > 0)
                {
                    htmlBuilder.AppendFormat(@"<a href=""{0}"" target=""_blank"" title=""‰Ø¿¿“≥√Ê""><img align=""absmiddle"" border=""0"" src=""{1}"" /></a>", PageUtility.ServiceSTL.Utils.GetRedirectUrl(this.nodeInfo.NodeID, true), this.iconFolderUrl);
                }
                else
                {
                    htmlBuilder.AppendFormat(@"<img align=""absmiddle"" src=""{0}"" />", this.iconFolderUrl);
                }
            }

            htmlBuilder.Append("&nbsp;");

            if (this.enabled)
            {
                if (loadingType == ELoadingType.ContentTree)
                {
                    string linkUrl = BackgroundContent.GetRedirectUrl(nodeInfo.PublishmentSystemID, nodeInfo.NodeID);

                    htmlBuilder.AppendFormat("<a href='{0}' isLink='true' onclick='fontWeightLink(this)' target='content'>{1}</a>", linkUrl, this.nodeInfo.NodeName);
                }
                else if (loadingType == ELoadingType.ChannelSelect)
                {
                    string linkUrl = PageUtils.GetCMSUrl(string.Format("modal_channelSelect.aspx?PublishmentSystemID={0}&NodeID={1}", nodeInfo.PublishmentSystemID, nodeInfo.NodeID));
                    if (additional != null)
                    {
                        if (!string.IsNullOrEmpty(additional["linkUrl"]))
                        {
                            linkUrl = additional["linkUrl"] + nodeInfo.NodeID;
                        }
                        else
                        {
                            foreach (string key in additional.Keys)
                            {
                                linkUrl += string.Format("&{0}={1}", key, additional[key]);
                            }
                        }
                    }
                    htmlBuilder.AppendFormat("<a href='{0}'>{1}</a>", linkUrl, this.nodeInfo.NodeName);
                }
                else if (loadingType == ELoadingType.GovPublicChannelAdd)
                {
                    if (EContentModelTypeUtils.Equals(nodeInfo.ContentModelID, EContentModelType.GovPublic))
                    {
                        string linkUrl = PageUtils.GetWCMUrl(string.Format("modal_govPublicCategoryChannelSelect.aspx?PublishmentSystemID={0}&NodeID={1}", nodeInfo.PublishmentSystemID, nodeInfo.NodeID));
                        htmlBuilder.AppendFormat("<a href='{0}'>{1}</a>", linkUrl, this.nodeInfo.NodeName);
                    }
                    else
                    {
                        htmlBuilder.Append(this.nodeInfo.NodeName);
                    }
                }
                else if (loadingType == ELoadingType.GovPublicChannelTree)
                {
                    string linkUrl = BackgroundContent.GetRedirectUrl(nodeInfo.PublishmentSystemID, nodeInfo.NodeID);

                    htmlBuilder.AppendFormat("<a href='{0}' isLink='true' onclick='fontWeightLink(this)' target='content'>{1}</a>", linkUrl, this.nodeInfo.NodeName);
                }
                else if (loadingType == ELoadingType.EvaluationNodeTree)
                {
                    string linkUrl = BackgroundEvaluationContent.GetRedirectUrl(nodeInfo.PublishmentSystemID, nodeInfo.NodeID);

                    htmlBuilder.AppendFormat("<a href='{0}' isLink='true' onclick='fontWeightLink(this)' target='content'>{1}</a>", linkUrl, this.nodeInfo.NodeName);
                }
                else if (loadingType == ELoadingType.TrialApplyNodeTree)
                {
                    string linkUrl = BackgroundTrialApplyContent.GetRedirectUrl(nodeInfo.PublishmentSystemID, nodeInfo.NodeID);

                    htmlBuilder.AppendFormat("<a href='{0}' isLink='true' onclick='fontWeightLink(this)' target='content'>{1}</a>", linkUrl, this.nodeInfo.NodeName);
                }
                else if (loadingType == ELoadingType.TrialReportNodeTree)
                {
                    string linkUrl = BackgroundTrialReportContent.GetRedirectUrl(nodeInfo.PublishmentSystemID, nodeInfo.NodeID);

                    htmlBuilder.AppendFormat("<a href='{0}' isLink='true' onclick='fontWeightLink(this)' target='content'>{1}</a>", linkUrl, this.nodeInfo.NodeName);
                }
                else if (loadingType == ELoadingType.SurveyNodeTree)
                {
                    string linkUrl = BackgroundSurveyContent.GetRedirectUrl(nodeInfo.PublishmentSystemID, nodeInfo.NodeID);

                    htmlBuilder.AppendFormat("<a href='{0}' isLink='true' onclick='fontWeightLink(this)' target='content'>{1}</a>", linkUrl, this.nodeInfo.NodeName);
                }
                else if (loadingType == ELoadingType.TrialAnalysisNodeTree)
                {
                    string linkUrl = BackgroundAnalysisFilesTrian.GetRedirectUrl(nodeInfo.PublishmentSystemID, nodeInfo.NodeID);

                    htmlBuilder.AppendFormat("<a href='{0}' isLink='true' onclick='fontWeightLink(this)' target='content'>{1}</a>", linkUrl, this.nodeInfo.NodeName);
                }
                else if (loadingType == ELoadingType.SurveyAnalysisNodeTree)
                {
                    string linkUrl = BackgroundAnalysisFilesSurvey.GetRedirectUrl(nodeInfo.PublishmentSystemID, nodeInfo.NodeID);

                    htmlBuilder.AppendFormat("<a href='{0}' isLink='true' onclick='fontWeightLink(this)' target='content'>{1}</a>", linkUrl, this.nodeInfo.NodeName);
                }
                else if (loadingType == ELoadingType.CompareNodeTree)
                {
                    string linkUrl = BackgroundCompareContent.GetRedirectUrl(nodeInfo.PublishmentSystemID, nodeInfo.NodeID);

                    htmlBuilder.AppendFormat("<a href='{0}' isLink='true' onclick='fontWeightLink(this)' target='content'>{1}</a>", linkUrl, this.nodeInfo.NodeName);
                } 
                else
                {
                    if (AdminUtility.HasChannelPermissions(nodeInfo.PublishmentSystemID, nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ChannelEdit))
                    {
                        string onClickUrl = ChannelLoading.GetChannelEditOpenWindowString(nodeInfo.PublishmentSystemID, nodeInfo.NodeID, returnUrl);
                        htmlBuilder.AppendFormat(@"<a href=""javascript:;;"" onClick=""{0}"" title=""øÏÀŸ±‡º≠¿∏ƒø"">{1}</a>", onClickUrl, this.nodeInfo.NodeName);

                    }
                    else
                    {
                        htmlBuilder.AppendFormat(@"<a href=""javascript:;;"">{0}</a>", this.nodeInfo.NodeName);
                    }
                }
            }
            else
            {
                htmlBuilder.Append(this.nodeInfo.NodeName);
            }

            if (this.nodeInfo.PublishmentSystemID != 0)
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(this.nodeInfo.PublishmentSystemID);

                htmlBuilder.Append("&nbsp;");

                htmlBuilder.Append(NodeManager.GetNodeTreeLastImageHtml(publishmentSystemInfo, nodeInfo));

                if (!ELoadingTypeUtils.Equals(ELoadingType.EvaluationNodeTree, loadingType) && !ELoadingTypeUtils.Equals(ELoadingType.TrialApplyNodeTree, loadingType) && !ELoadingTypeUtils.Equals(ELoadingType.TrialReportNodeTree, loadingType) && !ELoadingTypeUtils.Equals(ELoadingType.SurveyNodeTree, loadingType))
                    if (this.nodeInfo.ContentNum >= 0)
                    {
                        htmlBuilder.Append("&nbsp;");
                        htmlBuilder.AppendFormat(@"<span style=""font-size:8pt;font-family:arial"" class=""gray"">({0})</span>", this.nodeInfo.ContentNum);
                    }
            }

            return htmlBuilder.ToString();
        }

        public static string GetScript(PublishmentSystemInfo publishmentSystemInfo, ELoadingType loadingType, NameValueCollection additional)
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
            string ajaxUrl = PageUtility.GetCMSServiceUrlByPage(JsManager.CMSService.OtherServiceName, "GetLoadingChannels");
           
            if (EPublishmentSystemTypeUtils.IsB2C(publishmentSystemInfo.PublishmentSystemType))
            {
                ajaxUrl = PageUtils.GetB2CUrl("background_serviceB2C.aspx?type=GetLoadingChannels");
            }
            script += string.Format(@"
function loadingChannels(tr, img, div, nodeID){{
    var url = '{0}';
    var pars = 'publishmentSystemID={1}&parentID=' + nodeID + '&loadingType={2}&additional={3}';

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
            if (completedNodeID && completedNodeID == nodeID){{
                if (paths.indexOf(',') != -1){{
paths = paths.substring(paths.indexOf(',') + 1);
                    setTimeout(""loadingChannelsOnLoad('"" + paths + ""')"", 1000);
                }}
            }} 
        }}
    }}
}}
</script>
", ajaxUrl, publishmentSystemInfo.PublishmentSystemID, ELoadingTypeUtils.GetValue(loadingType), RuntimeUtils.EncryptStringByTranslate(TranslateUtils.NameValueCollectionToString(additional)));

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
$(document).ready(function(){{
    loadingChannelsOnLoad('{0}');
}});
</script>
", path);
        }

    }
}
