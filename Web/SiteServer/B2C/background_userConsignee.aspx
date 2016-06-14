  <%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.BackgroundUserConsignee" %>

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
  <asp:Literal id="ltlBreadCrumb" runat="server" />
  <bairong:alerts runat="server" />

  <table class="table table-bordered table-hover">
    <tr class="info thead">
      <td width="120">收货人</td>
      <td>收货地址</td>
      <td width="100">手机</td>
      <td width="100">固定电话</td>
      <td width="120">邮箱</td>
      <td width="80">默认地址</td>
      <td width="50"></td>
      <td width="50"></td>
      <td width="50"></td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
          <td><asp:Literal ID="ltlConsignee" runat="server"></asp:Literal></td>
          <td><asp:Literal ID="ltlLocation" runat="server"></asp:Literal></td>
          <td>
            <asp:Literal ID="ltlMobile" runat="server"></asp:Literal>
          </td>
          <td>
            <asp:Literal ID="ltlTel" runat="server"></asp:Literal>
          </td>
          <td>
            <asp:Literal ID="ltlEmail" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlIsDefault" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlIsDefaultUrl" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlDeleteUrl" runat="server"></asp:Literal>
          </td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button class="btn btn-success" id="btnAdd" Text="新 增" runat="server" />
    <asp:Button class="btn" text="返 回" onclick="Return_OnClick" runat="server" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->