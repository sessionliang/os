<%@ Page Language="C#" Inherits="SiteServer.WCM.BackgroundPages.Modal.GovInteractTypeAdd" Trace="false"%>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.WCM.Controls" Assembly="SiteServer.WCM" %>
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
      <td width="120">办件类型名称：</td>
      <td><asp:TextBox id="tbTypeName" Columns="25" MaxLength="50" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbTypeName" errorMessage=" *" foreColor="red" display="Dynamic"
						runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbTypeName"
						ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->