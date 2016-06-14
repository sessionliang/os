<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.Modal.UserConfigPermissionAdd" Trace="false"%>

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
      <td width="120">权限标识：</td>
      <td><asp:TextBox id="tbName" MaxLength="50" Size="30" runat="server"/>
        <asp:RequiredFieldValidator
			ControlToValidate="tbName"
			ErrorMessage=" *" foreColor="red"
			Display="Dynamic"
			runat="server"
			/></td>
    </tr>
    <tr>
      <td width="120">权限名称：</td>
      <td><asp:TextBox id="tbText" MaxLength="50" Size="30" runat="server"/>
        <asp:RequiredFieldValidator
			ControlToValidate="tbText"
			ErrorMessage=" *" foreColor="red"
			Display="Dynamic"
			runat="server"
			/></td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->