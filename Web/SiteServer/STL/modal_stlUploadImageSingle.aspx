<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.Modal.StlTemplate.StlUploadImageSingle" Trace="false" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
</head>

<body>
<!--#include file="../inc/openWindow.html"-->
<form class="form-inline" enctype="multipart/form-data" method="post" runat="server">
<bairong:alerts runat="server"></bairong:alerts>

	<table class="table table-noborder table-hover">
		<tr>
			<td width="120">选择上传的图片：</td>
			<td>
				<input type=file  id="hifUpload" size="45" runat="server"/> 
				<asp:RequiredFieldValidator ControlToValidate="hifUpload" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
			</td>
		</tr>
	</table>

	  <hr />
	  <table class="table noborder table-condensed">
	    <tr>
	      <td class="center">
	        <asp:Button class="btn btn-primary" id="btnSubmit" text="确 定"  runat="server" onClick="Submit_OnClick" />
	      </td>
	    </tr>
	  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->