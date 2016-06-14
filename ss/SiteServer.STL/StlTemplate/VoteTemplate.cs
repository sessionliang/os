using System.Text;
using BaiRong.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;
using BaiRong.Core.Data.Provider;

using System;
using System.Collections.Specialized;
using System.Collections;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.STL.Parser;

using SiteServer.STL.Parser.StlElement;
using SiteServer.CMS.Services;
using SiteServer.CMS.Core;

namespace SiteServer.STL.StlTemplate
{
    public class VoteTemplate
    {
        private PublishmentSystemInfo publishmentSystemInfo;
        private int nodeID;
        private VoteContentInfo contentInfo;
        private TagStyleInfo tagStyleInfo;
        private TagStyleVoteInfo tagStyleVoteInfo;

        public VoteTemplate(PublishmentSystemInfo publishmentSystemInfo, int nodeID, VoteContentInfo contentInfo, TagStyleInfo tagStyleInfo, TagStyleVoteInfo tagStyleVoteInfo)
        {
            this.publishmentSystemInfo = publishmentSystemInfo;
            this.nodeID = nodeID;
            this.contentInfo = contentInfo;
            this.tagStyleInfo = tagStyleInfo;
            this.tagStyleVoteInfo = tagStyleVoteInfo;
        }

        public string GetTemplate(bool isTemplate, string inputTemplateString)
        {
            StringBuilder inputBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(inputTemplateString))
            {
                inputBuilder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.GetScript());
                inputBuilder.Append(this.ReplacePlaceHolder(inputTemplateString));
            }
            else
            {
                if (isTemplate)
                {
                    if (!string.IsNullOrEmpty(this.tagStyleInfo.ScriptTemplate))
                    {
                        inputBuilder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.tagStyleInfo.ScriptTemplate);
                    }
                    inputBuilder.Append(this.ReplacePlaceHolder(this.tagStyleInfo.ContentTemplate));
                }
                else
                {
                    inputBuilder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.GetScript());
                    inputBuilder.Append(this.ReplacePlaceHolder(this.GetFileInputTemplate()));
                }
            }

            return inputBuilder.ToString();
        }

        public string GetScript()
        {
            string script = @"
$(document).ready(function(e) {
    $.getJSON(""[serviceUrl]&_r=""+Math.random(), function(data){
            display_vote_[contentID](data);
    });
});

function display_vote_[contentID](data){
     if(typeof(data) == ""string"")
         data = eval(""(""+data+"")"");
	$('#frmVote_[contentID]').setTemplate($('#voteTemplate_[contentID]').html()).processTemplate(data);
	
	var width = $('.vote_right').width();
	
	 for(var row in data.table){
		$('#vote_color_' + data.table[row].optionID).stop().animate({width:(data.table[row].voteNum/data.totalVoteNum)*width + 'px'}, 'slow');
		$('#vote_num_' + data.table[row].optionID).html(data.table[row].voteNum + ' (' + Math.round(data.table[row].voteNum / data.totalVoteNum * 10000) / 100.00 + '%)');
	 }
	
    $('.votingFrame .item').hover(
	function(){
		$(this).css('background-color','#F2F2F2');
	},
	function(){
		$(this).css('background-color','transparent');
	});

    var allSeconds = data.limitAllSeconds;//总共秒
    var timeCounter = setInterval(function () {
        getTimeDetail_[contentID](allSeconds, timeCounter);
        allSeconds--;
    }, 1000);
	
}

function stlVoteCallback_[contentID](jsonString){
	$('#frmVote_[contentID]').hideLoading();
	var obj = eval('(' + jsonString + ')');
	if (obj){
		if (obj.isSuccess == 'false'){
			$('#voteSuccess_[contentID]').hide();
			$('#voteFailure_[contentID]').show();
			$('#voteFailureText_[contentID]').html(obj.message);
		}else{
            //display_vote_[contentID](obj);

            $('#frmVote_[contentID] .num').html(parseInt($('#frmVote_[contentID] .num').html()) + 1);
            $('input:[inputType][name=""voteOption_[contentID]""]:checked').replaceWith(""<span class='icon_succS'></span>"");
            $('input:[inputType][name=""voteOption_[contentID]""]').replaceWith(""<span class='icon_none'></span>"");	

			$('#voteSuccess_[contentID]').show();
			$('#voteFailure_[contentID]').hide();		
		}
	}
}
function validate_vote_[contentID](inputType, inputName, maxSelectNum){
    if (inputType == 'radio' && selectNum == 1) return true;
	var selectNum = $('input:' + inputType + '[name=""' + inputName + '""]:checked').length;
	if(selectNum == 0)
	{
		$('#voteFailureText_[contentID]').html('投票失败，您至少需要选择一项进行投票！');
		$('#voteFailure_[contentID]').show();
		return false; 
	}
	else if (selectNum > maxSelectNum)
	{
		$('#voteFailureText_[contentID]').html('投票失败，您最多能选择' + maxSelectNum + '项进行投票！');
		$('#voteFailure_[contentID]').show();
		return false; 
	}
	return true;
}
function submit_vote_[contentID]()
{
	if (validate_vote_[contentID]('[inputType]', 'voteOption_[contentID]', [maxSelectNum]))
	{
        if([isCore]){
            vote_clickFun_[contentID]();
        }
        else{
		    $('#frmVote_[contentID]').showLoading();
		    $('#voteButton_[contentID]').hide();
		
		    var frmVote = document.getElementById('frmVote_[contentID]');
		    frmVote.action = '[actionUrl]';
		    frmVote.target = 'iframeVote_[contentID]';
		    frmVote.submit();
        }
	}
}

function getTimeDetail_[contentID](allSeconds, timeCounter) {
    var timeStr = '';
    if (allSeconds < 0) {
        clearInterval(timeCounter);
        $('#timeSpan_[contentID]').html('投票已经结束');
        $('#voteButton_[contentID]').remove();
    }
    else {
        if (allSeconds >= 0 && allSeconds < 60) {
            timeStr = '0天' + '0小时' + '0分钟' + parseInt(allSeconds % 60) + '秒';
        }
        else if (allSeconds >= 60 && allSeconds < 60 * 60) {
            timeStr = '0天' + '0小时' + parseInt(allSeconds / 60) + '分钟' + parseInt(allSeconds % 60) + '秒';
        }
        else if (allSeconds >= 60 * 60 && allSeconds < 60 * 60 * 24) {
            timeStr = '0天' + parseInt(allSeconds / (60 * 60)) + '小时' + parseInt((allSeconds % (60 * 60)) / 60) + '分钟' + parseInt((allSeconds % (60 * 60)) % 60) + '秒';
        }
        else if (allSeconds >= 60 * 60 * 24) {
            timeStr = parseInt(allSeconds / (60 * 60 * 24)) + '天' + parseInt(allSeconds % (60 * 60 * 24) / (60 * 60)) + '小时' + parseInt(((allSeconds % (60 * 60 * 24)) % (60 * 60)) / 60) + '分钟' + parseInt((((allSeconds % (60 * 60 * 24)) % (60 * 60)) % 60) % 60) + '秒';
        }
        $('#timeSpan_[contentID]').html('距离投票结束还有' + timeStr);
    }
}
";

            if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
            {
                //web api sofuny
                script += string.Format(@"
    function vote_clickFun_{0}(){{
        //if (checkFormValueById('frmVote_{0}')){{
            //openModal();
            if(window.FormData !== undefined){{
            $.ajax({{
                url: '[actionUrl]',
                type: ""POST"",
                mimeType:""multipart/form-data"",
                contentType: false,
                processData: false,
                cache: false,
                xhrFields: {{   
                    withCredentials: true   
                }},
                data: new FormData($(""#frmVote_{0}"")[0]), //$(""#frmVote_{0}"").serialize(),
                success: function(json, textStatus, jqXHR){{
                    execFun(json);
                }}
            }});
            }}
            else{{
                //generate a random id
                var  iframeId = 'unique' + (new Date().getTime());
                //create an empty iframe
                var iframe = $('<iframe src=""javascript:false;"" name=""'+iframeId+'"" />');
                //hide it
                iframe.hide();
                //set form target to iframe
                formObj.attr('target',iframeId);
                //Add iframe to body
                iframe.appendTo('body');
                iframe.load(function(e){{
                    var doc = getDoc(iframe[0]);
                    var docRoot = doc.body ? doc.body : doc.documentElement;
                    var data = docRoot.innerHTML;
                    //data is returned from server.
                        execFun(data);
                }});
            }}
            return false;
        }}
    //}}
    
    function execFun(json){{
        if(!!json){{
            if(typeof(json) == ""string"")
                json = eval(""(""+json+"")"");
            if(!!json.scriptString){{
                //eval(""(""+json.scriptString+"")"");
                $(json.scriptString).appendTo(document.body);
            }}
        }}
    }}
", this.contentInfo.ID);
            }

            string serviceUrl = PageService.Vote.GetServiceUrl(this.publishmentSystemInfo.PublishmentSystemID, this.contentInfo.NodeID, this.contentInfo.ID);
            script = script.Replace("[isCore]", PageUtility.IsCorsCrossDomain(publishmentSystemInfo).ToString().ToLower());
            script = script.Replace("[serviceUrl]", serviceUrl);
            script = script.Replace("[contentID]", this.contentInfo.ID.ToString());
            if (this.contentInfo.MaxSelectNum == 1)
            {
                script = script.Replace("[inputType]", "radio");
            }
            else
            {
                script = script.Replace("[inputType]", "checkbox");
            }

            script = script.Replace("[maxSelectNum]", contentInfo.MaxSelectNum.ToString());
            script = script.Replace("[actionUrl]", PageUtility.Services.GetActionUrlOfVote(this.publishmentSystemInfo, this.nodeID, this.contentInfo.ID));

            return script;
        }

        public string GetFileInputTemplate()
        {
            string content = FileUtils.ReadText(PageUtility.Services.GetPath("vote/inputTemplate.html"), ECharset.utf_8);

            content = content.Replace("[contentID]", this.contentInfo.ID.ToString());
            if (this.contentInfo.MaxSelectNum == 1)
            {
                content = content.Replace("[inputType]", "radio");
            }
            else
            {
                content = content.Replace("[inputType]", "checkbox");
            }

            return content;
        }

        public static string GetCallbackScript(PublishmentSystemInfo publishmentSystemInfo, int nodeID, int contentID, bool isSuccess, string failureMessage)
        {

            string jsonString = PageService.Vote.GetJsonString(publishmentSystemInfo, nodeID, contentID, isSuccess, failureMessage);

            string retval = string.Empty;
            if (PageUtility.IsAgentCrossDomain(publishmentSystemInfo))
            {
                string script = string.Format("<script>window.parent.parent.stlVoteCallback_[contentID]('{0}');</script>", jsonString);
                string proxyUrl = PageUtility.GetProxyUrl(publishmentSystemInfo, script);
                retval = string.Format(@"<script>document.write(""<iframe src='{0}' style='display:none'></iframe>"");</script>", proxyUrl);
            }
            else if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
            {
                retval = string.Format("<script>window.stlVoteCallback_[contentID]('{0}');</script>", jsonString);
            }
            else
            {
                retval = string.Format("<script>window.parent.stlVoteCallback_[contentID]('{0}');</script>", jsonString);
            }

            return retval.Replace("[contentID]", contentID.ToString());
        }

        private string ReplacePlaceHolder(string fileInputTemplate)
        {
            StringBuilder parsedContent = new StringBuilder();
            parsedContent.AppendFormat(@"
<form id=""frmVote_[contentID]"" name=""frmVote_[contentID]"" style=""margin:0;padding:0"" method=""post"" enctype=""multipart/form-data"">
<script type=""text/template"" id=""voteTemplate_[contentID]"">  
  {0}
</script>
</form>
<iframe id=""iframeVote_[contentID]"" name=""iframeVote_[contentID]"" width=""0"" height=""0"" frameborder=""0""></iframe>
", fileInputTemplate);

            parsedContent.Replace("[contentID]", this.contentInfo.ID.ToString());

            return parsedContent.ToString();
        }
    }
}
