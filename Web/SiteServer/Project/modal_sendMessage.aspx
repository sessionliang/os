<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.Modal.SendMessage" Trace="false"%>

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
      <td class="center" width="120">发送到：</td>
      <td><asp:TextBox ID="tbMessageTo" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
      <td class="center" width="120">发送内容：</td>
      <td><asp:TextBox ID="tbMessage" TextMode="MultiLine" rows="4" width="90%" runat="server"></asp:TextBox></td>
    </tr>
  </table>

</form>
</body>
</html>
