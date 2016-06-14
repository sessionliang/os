<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.Modal.OrderSetting" Trace="false"%>

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
<bairong:alerts runat="server" />

  <table class="table table-noborder table-hover">
    <tr>
      <td class="center">当前状态：</td>
      <td><asp:DropDownList ID="ddlStatus" runat="server"></asp:DropDownList></td>
    </tr>
  </table>

</form>
</body>
</html>
