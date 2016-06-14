<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.Modal.FormPageAdd" Trace="false"%>

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

  <table class="table noborder table-hover">
    <tr>
      <td width="120">标题：</td>
      <td><asp:TextBox Columns="25" MaxLength="50" id="tbTitle" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbTitle" errorMessage=" *" foreColor="red" display="Dynamic"
            runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbTitle"
            ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->