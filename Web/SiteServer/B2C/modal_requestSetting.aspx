<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.Modal.RequestSetting" Trace="false"%>

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

  <table class="table table-noborder table-hover">
    <tr>
      <td class="center" width="120">工单状态：</td>
      <td><asp:DropDownList ID="ddlStatus" runat="server"></asp:DropDownList></td>
    </tr>
    <tr>
      <td class="center">负责人：</td>
      <td><asp:DropDownList ID="ddlChargeUserName" runat="server"></asp:DropDownList></td>
    </tr>
  </table>

</form>
</body>
</html>
