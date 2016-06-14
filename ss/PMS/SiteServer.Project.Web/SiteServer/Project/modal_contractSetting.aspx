<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.Modal.ContractSetting" Trace="false"%>

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
      <td class="center" width="120">是否寄出：</td>
      <td><asp:DropDownList ID="ddlIsContract" runat="server"></asp:DropDownList></td>
    </tr>
    <tr>
      <td class="center" width="120">寄出日期：</td>
      <td><bairong:DateTimeTextBox id="tbContractDate" runat="server" /></td>
    </tr>
    <tr>
      <td class="center" width="120">是否收到：</td>
      <td><asp:DropDownList ID="ddlIsConfirm" runat="server"></asp:DropDownList></td>
    </tr>
    <tr>
      <td class="center" width="120">收到日期：</td>
      <td><bairong:DateTimeTextBox id="tbConfirmDate" runat="server" /></td>
    </tr>
  </table>

</form>
</body>
</html>
