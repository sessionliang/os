﻿<%@ Page Language="C#" Inherits="UserCenter.WCM.Pages.Modal.TextEditorImportWord" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>
<%@ Register TagPrefix="user" Namespace="UserCenter.Controls" Assembly="UserCenter.Pages" %>
<%@ Register TagPrefix="site" Namespace="SiteServer.WCM.Controls" Assembly="SiteServer.WCM" %>
<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
</head>

<body>
<!--#include file="../inc/openWindow.html"-->
<bairong:code type="ajaxUpload" runat="server" />
<script type="text/javascript" language="javascript">
$(document).ready(function(){
	new AjaxUpload('uploadFile', {
	 action: "modal_textEditorImportWord.aspx?upload=true",
	 name: "filedata",
	 data: {},
	 onSubmit: function(file, ext) {
		 var reg = /^(doc|docx)$/i;
		 if (ext && reg.test(ext)) {
			 $('#img_upload_txt').text('上传中... ');
		 } else {
			 $('#img_upload_txt').text('系统不允许上传指定的格式');
			 return false;
		 }
	 },
	 onComplete: function(file, response) {
		$('#img_upload_txt').text('');
		 if (response) {
			 response = eval("(" + response + ")");
			 if (response.success == 'true') {
				 $('#fileSelect').append('<h5>' + response.fileName + '<h5>');
				 $('#fileNames').val($('#fileNames').val() + '|' + response.fileName);
			 } else {
				 $('#img_upload_txt').text(response.message);
			 }
		 }
	 }
	});
});
</script>
<form class="form-horizontal form-inline" runat="server">
<asp:Button id="btnSubmit" useSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" style="display:none" />
<bairong:alerts text="请点击按钮上传Word文件" runat="server"></bairong:alerts>

	<input type="hidden" id="fileNames" name="fileNames" value="" />
	<div class="control-group">
		<label class="control-label">请选择Word文件</label>
		<div class="controls" id="fileSelect">
		  <div id="uploadFile" class="btn btn-success">选 择</div>
		  <span id="img_upload_txt" style="clear:both; font-size:12px; color:#FF3737;"></span>
		</div>
	</div>
	<div class="center">
		<asp:CheckBox CssClass="checkbox inline" id="cbIsClearFormat" Checked="true" runat="server" Text="清除格式"/>
		<asp:CheckBox CssClass="checkbox inline" id="cbIsFirstLineIndent" Checked="true" runat="server" Text="首行缩进"/>
		<asp:CheckBox CssClass="checkbox inline" id="cbIsClearFontSize" Checked="true" runat="server" Text="清除字号"/>
		<asp:CheckBox CssClass="checkbox inline" id="cbIsClearFontFamily" Checked="true" runat="server" Text="清除字体"/>
		<asp:CheckBox CssClass="checkbox inline" id="cbIsClearImages" runat="server" Text="清除图片"/>
        <asp:CheckBox CssClass="checkbox inline" id="cbIsClearEmptyRows" runat="server" Text="清除空行"/>
        <asp:CheckBox CssClass="checkbox inline" id="cbIsCenterTables" runat="server" Text="表格居中"/>
    </div>
  
</form>
</body>
</html>