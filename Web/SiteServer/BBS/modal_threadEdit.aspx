<%@ Page Language="C#" AutoEventWireup="true" Inherits="SiteServer.BBS.BackgroundPages.Modal.ThreadEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="/BBS/css/share.css" rel="stylesheet" type="text/css" />
    <link href="/BBS/css/other.css" rel="stylesheet" type="text/css" />
    <link href="/BBS/css/dialog.css" rel="stylesheet" type="text/css" />
    <link href="/BBS/css/popout.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/BBS/js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/BBS/js/jquery.form.js"></script>
    <script type="text/javascript" src="/BBS/js/jquery.bgiframe.min.js"></script>
    <script type="text/javascript" src="/BBS/js/jquery.loading.js"></script>
    <script type="text/javascript">
        var bbsUrl = '/BBS'; var loading;</script>
    <script type="text/javascript" src="/BBS/js/ready.js"></script>
    <script type="text/javascript" language="javascript" src="/BBS/js/jquery.idTabs.min.js"></script>
    <link href="/BBS/js/jquery_select/selectStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/BBS/js/jquery_select/jquery.select-1.3.6.js"></script>
    <script type="text/javascript" src="/BBS/js/jquery.upload.js"></script>
    <script type="text/javascript" src="/BBS/editor/kindeditor-min.js"></script>
    <script type="text/javascript" src="/BBS/js/editor.js"></script>
    <%if (base.isPoll)
      {%><link href="/BBS/css/vote.css" rel="stylesheet" type="text/css" /><script language="javascript"
          type="text/javascript" src="/BBS/js/DatePicker/WdatePicker.js"></script>
    <script language="javascript"
type="text/javascript">
          function addItems() {
              if (document.getElementById('voteItems2').style.display == "none") {
                  $('#voteItems2').show();
              } else if (document.getElementById('voteItems3').style.display == "none") {
                  $('#voteItems3').show();
              } else if (document.getElementById('voteItems4').style.display == "none") {
                  $('#voteItems4').show();
                  $('#vote_addicon').hide();
              }
          }
    </script><%}%>
    <script language="javascript" type="text/javascript">
        KE.show({
            id: 'content',
            imageUploadJson: '/BBS/editor/upload_json.ashx',
            allowFileManager: false,
            items: [
		'fullscreen', 'undo', 'redo', 'print', 'cut', 'copy', 'paste',
		'plainpaste', 'wordpaste', '|', 'justifyleft', 'justifycenter', 'justifyright',
		'justifyfull', 'insertorderedlist', 'insertunorderedlist', 'indent', 'outdent', 'subscript',
		'superscript', '|', 'selectall', '-',
		'title', 'fontname', 'fontsize', '|', 'textcolor', 'bgcolor', 'bold',
		'italic', 'underline', 'strikethrough', 'removeformat', '|', 'image',
		'flash', 'media', 'advtable', 'hr', 'emoticons', 'link', 'unlink', '|', 'ubb_code', 'ubb_hide'
	],
            afterCreate: function (id) {
                KE.event.ctrl(document, 13, function () {
                    KE.util.setData(id);
                    onPostPageSubmit(document.forms['contentForm']);
                });
                KE.event.ctrl(KE.g[id].iframeDoc, 13, function () {
                    KE.util.setData(id);
                    onPostPageSubmit(document.forms['contentForm']);
                });
            }
        });
        $(document).ready(function () {
            if (Storage.load('title')) {
                $('#title', $('#contentForm')).val(Storage.load('title'));
                Storage.save('title', '', 1);
            }
            if (Storage.load('content')) {
                $('#content', $('#contentForm')).val(Storage.load('content'));
                Storage.save('content', '', 1);
            }
        });

        function onPostPageSubmit(form) {
            KE.sync("content");
            if ($('#content', form).val() == '') {
                failureMessage('请填写内容', function () { KE.util.focus("content"); });
            } else {
                var loading = new ol.loading();
                loading.show();
                $(form).ajaxSubmit({
                    dataType: 'json',
                    success: function (data) {
                        loading.hide();
                        if (data.success == 'true') {
                            parent.location.href = "background_thread.aspx?PublishmentSystemID=<%=base.PublishmentSystemID%>";
                        } else {
                            failureMessage(data.errorMessage);
                        }
                    }
                });
            }
        }

    </script>
    <style type="text/css">        
.submit_btn { width:76px; height:32px; border:0; background:url(/BBS/submit_btn.gif) no-repeat right top; text-align:center; margin:10px 15px 15px 10px; font-size:14px; color:#fff; float:left; cursor:pointer }
    </style>
</head>
<body>
    <script type="text/javascript" src="/BBS/js/dialog.js"></script>
    <script type="text/javascript" src="/BBS/js/popout.js"></script>
    <div class="hd">
        <div class="share_area">
            <div class="share_tit">
                <h3>
                    <asp:Literal ID="ltlOperate" runat="server"></asp:Literal>
                </h3>
            </div>
            <div class="share_table" style="padding: 0px;">
                <form id="contentForm" onsubmit="onPostPageSubmit(this);return false;" action="/BBS/ajax/form.aspx?action=postAllInOne&PublishmentSystemID=<%=base.PublishmentSystemID%>"
                method="post">
                <input id="forumID" name="forumID" type="hidden" value="<%=base.forumID%>" />
                <input id="threadID" name="threadID" type="hidden" value="<%=base.threadID%>" />
                <input id="postID" name="postID" type="hidden" value="<%=base.postID%>" />
                <input id="postType" name="postType" type="hidden" value="<%=base.postType%>" />
                <input id="fileCount" name="fileCount" type="hidden" value="<%=base.fileCount%>" />
                <table cellspacing="0" cellpadding="0">
                    <tr>
                        <td valign="top" style="padding: 10px;">
                            <table>
                                <tr>
                                    <td>
                                        <div style="padding: 5px;">
                                            <%=GetCategorySelectHtml()%>
                                            <input id="title" name="title" type="text" class="txtTitle" value="<%=base.GetTitle()%>" />
                                        </div>
                                    </td>
                                </tr>
                                <%if (base.isPoll)
                                  {%><tr>
                                      <td>
                                          <div class="lv_form">
                                              <fieldset>
                                                  <p class="lv_mt">
                                                      投票选项：<span class="gray9">可设置最多20项，每项最多20个汉字</span></p>
                                                  <p class="lv_inputsub">
                                                      <span>1.</span><input type="text" id="PollItems1" name="PollItems" /></p>
                                                  <p class="lv_inputsub">
                                                      <span>2.</span><input type="text" id="PollItems2" name="PollItems" /></p>
                                                  <p class="lv_inputsub">
                                                      <span>3.</span><input type="text" id="PollItems3" name="PollItems" /></p>
                                                  <p class="lv_inputsub">
                                                      <span>4.</span><input type="text" id="PollItems4" name="PollItems" /></p>
                                                  <p class="lv_inputsub">
                                                      <span>5.</span><input type="text" id="PollItems5" name="PollItems" /></p>
                                                  <div id="voteItems2" style="display: none">
                                                      <p class="lv_inputsub">
                                                          <span>6.</span><input type="text" id="PollItems6" name="PollItems" /></p>
                                                      <p class="lv_inputsub">
                                                          <span>7.</span><input type="text" id="PollItems7" name="PollItems" /></p>
                                                      <p class="lv_inputsub">
                                                          <span>8.</span><input type="text" id="PollItems8" name="PollItems" /></p>
                                                      <p class="lv_inputsub">
                                                          <span>9.</span><input type="text" id="PollItems9" name="PollItems" /></p>
                                                      <p class="lv_inputsub">
                                                          <span>10.</span><input type="text" id="PollItems10" name="PollItems" /></p>
                                                  </div>
                                                  <div id="voteItems3" style="display: none">
                                                      <p class="lv_inputsub">
                                                          <span>11.</span><input type="text" id="PollItems11" name="PollItems" /></p>
                                                      <p class="lv_inputsub">
                                                          <span>12.</span><input type="text" id="PollItems12" name="PollItems" /></p>
                                                      <p class="lv_inputsub">
                                                          <span>13.</span><input type="text" id="PollItems13" name="PollItems" /></p>
                                                      <p class="lv_inputsub">
                                                          <span>14.</span><input type="text" id="PollItems14" name="PollItems" /></p>
                                                      <p class="lv_inputsub">
                                                          <span>15.</span><input type="text" id="PollItems15" name="PollItems" /></p>
                                                  </div>
                                                  <div id="voteItems4" style="display: none">
                                                      <p class="lv_inputsub">
                                                          <span>16.</span><input type="text" id="PollItems16" name="PollItems" /></p>
                                                      <p class="lv_inputsub">
                                                          <span>17.</span><input type="text" id="PollItems17" name="PollItems" /></p>
                                                      <p class="lv_inputsub">
                                                          <span>18.</span><input type="text" id="PollItems18" name="PollItems" /></p>
                                                      <p class="lv_inputsub">
                                                          <span>19.</span><input type="text" id="PollItems19" name="PollItems" /></p>
                                                      <p class="lv_inputsub">
                                                          <span>20.</span><input type="text" id="PollItems20" name="PollItems" /></p>
                                                  </div>
                                                  <p id="vote_addicon">
                                                      <span class="vote_addicon"></span><a href="javascript:;" onclick="addItems()">增加选项</a></p>
                                                  <p>
                                                      <span class="lv_results">投票结果：</span><input type="radio" name="IsVoteFirst" value="False"
                                                          checked="checked" class="lv_input2"><label class="label12">任何人可见</label><input type="radio"
                                                              name="IsVoteFirst" value="True" class="lv_input2"><label class="label12">投票后可见</label></input></input></p>
                                                  <p>
                                                      <label for="">
                                                          单选/多选：</label><select id="MaxNum" name="MaxNum"><option selected="selected" value="1">
                                                              单选</option>
                                                              <option value="0">多选</option>
                                                          </select></p>
                                                  <p>
                                                      <label for="">
                                                          截止时间：</label><input type="text" value="<%=GetPollDeadline()%>" class="lv_calendar"
                                                              name="Deadline" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm',isShowClear:false})" /></p>
                                              </fieldset>
                                              <asp:Literal ID="ltlPollScript" runat="server"></asp:Literal></div>
                                      </td>
                                  </tr>
                                <%}%>
                                <tr>
                                    <td>
                                        <div style="padding: 5px;">
                                            <asp:PlaceHolder ID="phReference" Visible="false" runat="server">
                                                <div class="reg" style="padding-left: 20px; line-height: 15px; padding-bottom: 5px;">
                                                    <asp:Literal ID="ltlReference" runat="server"></asp:Literal>
                                                </div>
                                            </asp:PlaceHolder>
                                            <textarea id="content" name="content" cols="100" rows="8" style="width: 100%; height: 350px;
                                                visibility: hidden;"><%=base.GetContent()%></textarea>
                                        </div>
                                    </td>
                                </tr>
                                <!--
                                <tr>
                                    <td>
                                        <div class="posting_function">
                                            <div id="filetabs">
                                                <ul class="tab cl">
                                                    <li><a class="selected" href="#filetab1">上传附件(图片)</a></li>
                                                    <li><a href="#filetab2">更多选项</a></li>
                                                </ul>
                                                <div class="tabContent" id="filetab1">
                                                    <script type="text/javascript" src="/BBS/js/swfUpload/swfupload.js"></script>
                                                    <script type="text/javascript" src="/BBS/js/swfUpload/handlers.js"></script>
                                                    <script type="text/javascript">
function uploadSuccess(file, response) {
	if (response) {
	 response = eval("(" + response + ")");
	 
	 if (response.success == 'true') {
		addToList(response.id, response.fileName, response.tips, response.description, response.price);
	 } else {
		 $('#img_upload_txt').text(response.message);
	 }
	}
}

function addToList(id, fileName, tips, description, price){
	var count = parseInt($('#fileCount').val()) + 1;
	var $el = $('<tr id="fileTr_' + count + '">' + $('#fileTr_0').html().replace(/_0/g, '_' + count) + '</tr>');
	$el.insertAfter($('#fileTr_' + (count - 1)));
	$('#fileID_' + count).val(id);
	$('#fileLink_' + count).html(fileName);
	$('#fileLink_' + count).attr('title', tips);
	$('#fileLink_' + count).click(function(){
		KE.insertHtml('content', "[attachment id=" + id + "]");
	});
	$('#fileDescription_' + count).val(description);
	$('#filePrice_' + count).val(price);
	$('#fileCount').val(count);
}

function removeFromList(trID){
	$(trID).remove();
	var count = parseInt($('#fileCount').val());
	$('#fileCount').val(count - 1);
}

var swfu;
$(document).ready(function(){
	$("select").sSelect();
	$("#filetabs ul").idTabs();
	
	if (navigator.userAgent.indexOf("Firefox")== -1){
		swfu = new SWFUpload({
		// Backend Settings
		upload_url: "/BBS/ajax/upload.aspx",
	
		// File Upload Settings
		file_size_limit : "2 MB",
		file_types : "<%=GetUploadTypes(false)%>",
		file_types_description : "附件",
		file_upload_limit : 0,    // Zero means unlimited
	
		// Event Handler Settings - these functions as defined in Handlers.js
		//  The handlers are not part of SWFUpload but are part of my website and control how
		//  my website reacts to the SWFUpload events.
		swfupload_preload_handler : preLoad,
		swfupload_load_failed_handler : loadFailed,
		file_queue_error_handler : fileQueueError,
		file_dialog_complete_handler : fileDialogComplete,
		upload_error_handler : uploadError,
		upload_success_handler : uploadSuccess,
		upload_complete_handler : uploadComplete,
	
		// Button settings
		button_image_url : "/BBS/js/swfUpload/button.png",
		button_placeholder_id : "swfUploadPlaceholder",
		button_width: 114,
		button_height: 22,
		button_text : '多个文件上传',
		button_text_top_padding: 1,
		button_text_left_padding: 5,
	
		// Flash Settings
		flash_url : "/BBS/js/swfUpload/swfupload.swf",	// Relative to this file
		flash9_url : "/BBS/js/swfUpload/swfupload_FP9.swf",	// Relative to this file
	
		// Debug Settings
		debug: false
		});	
	}
	
	new AjaxUpload('uploadFile', {
	 action: "/BBS/ajax/upload.aspx",
	 name: "Filedata",
	 data: {},
	 onSubmit: function(file, ext) {
		 var reg = /^(<%=GetUploadTypes(true)%>)$/i;
		 if (ext && reg.test(ext)) {
			 $('#img_upload_txt').text('上传中... ');
		 } else {
			 $('#img_upload_txt').text('系统不允许上传指定的格式');
			 return false;
		 }
	 },
	 onComplete: function(file, response) {
		$('#img_upload_txt').text(' ');
		 if (response) {
			 response = eval("(" + response + ")");
			 if (response.success == 'true') {
				 addToList(response.id, response.fileName, response.tips, response.description, response.price);
			 } else {
				 $('#img_upload_txt').text(response.message);
			 }
		 }
	 }
	});	
	<%=GetAddToListScript()%>
});
                                                    </script>
                                                    <style type="text/css">
                                                        .tab a
                                                        {
                                                            margin-top: -2px;
                                                        }
                                                        .upload
                                                        {
                                                            background-image: url(js/swfUpload/button.png);
                                                            width: 104px;
                                                            height: 18px;
                                                            text-align: left;
                                                            padding: 2px 5px;
                                                            cursor: default;
                                                            text-decoration: none;
                                                            display: block;
                                                        }
                                                    </style>
                                                    <table id="filesTable" width="100%">
                                                        <tr>
                                                            <td>
                                                                文件名
                                                            </td>
                                                            <td>
                                                                描述
                                                            </td>
                                                            <td>
                                                                价格
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr id="fileTr_0" style="display: none">
                                                            <td>
                                                                <p>
                                                                    <span>
                                                                        <input id="fileID_0" name="fileID" type="hidden" />
                                                                        <a id="fileLink_0" class="xi2" href="javascript:;"></a></span>
                                                                </p>
                                                            </td>
                                                            <td>
                                                                <input id="fileDescription_0" name="fileDescription" class="txt" size="18" />
                                                            </td>
                                                            <td>
                                                                <input id="filePrice_0" name="filePrice" class="txt" value="0" size="1" />
                                                            </td>
                                                            <td>
                                                                <a onclick="removeFromList(this.parentNode.parentNode)" href="javascript:;">删除</a>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <p class="notice">
                                                        <%=GetUploadTips()%></p>
                                                    <br />
                                                    <table width="260" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td>
                                                                <a id="uploadFile" class="upload">单个文件上传</a>
                                                            </td>
                                                            <td>
                                                                <span id="swfUploadPlaceholder"></span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <span id="img_upload_txt" style="clear: both; font-size: 12px; color: #FF3737;">
                                                    </span>
                                                </div>
                                                <div class="tabContent" id="filetab2">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <ul>
                                                                    <li>
                                                                        <label for="isSignature">
                                                                            <input type="checkbox" id="isSignature" name="isSignature" value="true" />
                                                                            &nbsp;使用签名</label>
                                                                    </li>
                                                                </ul>
                                                            </td>
                                                            <td valign="top">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                            <ul>
                                                <li>
                                                    <input type="submit" value="提交" class="submit_btn" />
                                                </li>
                                            </ul>
                                        </div>
                                        <div id="face_right" class="face_right">
                                            <div id="tabs" class="face_tit">
                                                <ul>
                                                    <li class="current"><a id="yoci" href="/BBS/ajax/face.aspx?faceName=yoci">悠嘻猴</a></li><li>
                                                        <a id="tuzki" href="/BBS/ajax/face.aspx?faceName=tuzki">兔斯基</a></li><li><a id="oniontou"
                                                            href="/BBS/ajax/face.aspx?faceName=oniontou">洋葱头</a></li>
                                                </ul>
                                            </div>
                                            <div id="tab-contents">
                                                <div id="tabs-yoci" class="face_info">
                                                    <a href="javascript:void(0);">
                                                        <img src="/BBS/smile/yoci/一无所有.gif" border="0" /></a><a href="javascript:void(0);"><img
                                                            src="/BBS/smile/yoci/七窍生烟.gif" border="0" /></a><a href="javascript:void(0);"><img
                                                                src="/BBS/smile/yoci/不可以.gif" border="0" /></a><a href="javascript:void(0);"><img
                                                                    src="/BBS/smile/yoci/不好意思.gif" border="0" /></a><a href="javascript:void(0);"><img
                                                                        src="/BBS/smile/yoci/不懂.gif" border="0" /></a><a href="javascript:void(0);"><img
                                                                            src="/BBS/smile/yoci/不给糖就捣蛋.gif" border="0" /></a><a href="javascript:void(0);"><img
                                                                                src="/BBS/smile/yoci/不至于吧.gif" border="0" /></a><a href="javascript:void(0);"><img
                                                                                    src="/BBS/smile/yoci/不行了.GIF" border="0" /></a><a href="javascript:void(0);"><img
                                                                                        src="/BBS/smile/yoci/不要了拉.gif" border="0" /></a><a href="javascript:void(0);"><img
                                                                                            src="/BBS/smile/yoci/不要拉.gif" border="0" /></a><a href="javascript:void(0);"><img
                                                                                                src="/BBS/smile/yoci/乖.gif" border="0" /></a><a href="javascript:void(0);"><img src="/BBS/smile/yoci/交给我吧.gif"
                                                                                                    border="0" /></a><a href="javascript:void(0);"><img src="/BBS/smile/yoci/什么问题.gif"
                                                                                                        border="0" /></a><a href="javascript:void(0);"><img src="/BBS/smile/yoci/体操.gif"
                                                                                                            border="0" /></a><a href="javascript:void(0);"><img src="/BBS/smile/yoci/你好.gif"
                                                                                                                border="0" /></a><a href="javascript:void(0);"><img src="/BBS/smile/yoci/健身.gif"
                                                                                                                    border="0" /></a><div class="face_page">
                                                                                                                        <span class="face_cur_page">1</span><a href="/BBS/ajax/face.aspx?faceName=yoci&page=2">2</a><a
                                                                                                                            href="/BBS/ajax/face.aspx?faceName=yoci&page=3">3</a><a href="/BBS/ajax/face.aspx?faceName=yoci&page=4">4</a><a
                                                                                                                                href="/BBS/ajax/face.aspx?faceName=yoci&page=5">5</a><a href="/BBS/ajax/face.aspx?faceName=yoci&page=6">6</a><a
                                                                                                                                    href="/BBS/ajax/face.aspx?faceName=yoci&page=7">7</a><a href="/BBS/ajax/face.aspx?faceName=yoci&page=8">8</a><a
                                                                                                                                        href="/BBS/ajax/face.aspx?faceName=yoci&page=9">9</a></div>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                -->
                                <tr><td>
                                                    <input type="submit" value="提交" class="submit_btn" /></td></tr>
                            </table>
                        </td>
                    </tr>
                </table>
                </form>
            </div>
        </div>
    </div>
    <div id="dialog_window" style="z-index: 300009; display: block; display: none; width: 400px;
        height: 288px; top: 79px; left: 400px" class="window window_current">
        <iframe class="fullscreen_bg_iframe" height="100%" width="100%"></iframe>
    </div>
    <asp:Literal ID="ltlScripts" runat="server"></asp:Literal>
</body>
</html>