<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.Modal.OrderRefundSetting" Trace="false"%>

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
      <td class="center" width="120">是否退款：</td>
      <td><asp:DropDownList ID="ddlIsRefund" runat="server"></asp:DropDownList></td>
    </tr>
    <tr>
      <td class="center" width="120">退款日期：</td>
      <td><bairong:DateTimeTextBox id="tbRefundDate" runat="server" /></td>
    </tr>
    <tr>
      <td class="center" width="120">是否确认：</td>
      <td><asp:DropDownList ID="ddlIsConfirm" runat="server"></asp:DropDownList></td>
    </tr>
    <tr>
      <td class="center" width="120">确认日期：</td>
      <td><bairong:DateTimeTextBox id="tbConfirmDate" runat="server" /></td>
    </tr>
  </table>

</form>
</body>
</html>
