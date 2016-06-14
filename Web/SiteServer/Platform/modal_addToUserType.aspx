<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.Modal.AddToUserType" Trace="false"%>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>
<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
</head>

<body>
<!--#include file="../inc/openWindow.html"-->
<form class="form-inline" runat="server">
<asp:Button id="btnSubmit" useSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" style="display:none" />
<bairong:alerts runat="server"></bairong:alerts>

	<table class="table table-noborder table-hover">
	    <tr>
	      <td width="100" height="35">用户类型：</td>
	      <td><asp:DropDownList ID="TypeIDList" runat="server"/></td>
	    </tr>
	 </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->