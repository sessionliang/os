<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.Modal.UploadImageSingle" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
</head>

<body>
<!--#include file="../inc/openWindow.html"-->
<form class="form-inline" enctype="multipart/form-data" method="post" runat="server">
<asp:Button id="btnSubmit" useSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" style="display:none" />
<bairong:alerts runat="server"></bairong:alerts>  
  	<table class="table table-noborder table-hover">
		<tr>
			<td width="120">选择上传的图片：</td>
			<td>
				<input type=file  id="myFile" size="45" runat="server"/>
				<asp:RequiredFieldValidator ControlToValidate="myFile" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
			</td>
		</tr>
    </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->