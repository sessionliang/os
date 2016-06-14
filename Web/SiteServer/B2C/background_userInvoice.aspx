<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.BackgroundUserInvoice" %>

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
      <td width="120">发票类型</td>
      <td>发票抬头（普通发票）</td>
      <td>详细信息（增值税发票）</td>
      <td>收票人地址</td>
      <td width="50">默认</td>
      <td width="50"></td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
          <td class="center">
            <asp:Literal ID="ltlIsVat" runat="server"></asp:Literal>
          </td>
          <td>
            <asp:Literal ID="ltlCompany" runat="server"></asp:Literal>
          </td>
          <td>
            <asp:Literal ID="ltlVat" runat="server"></asp:Literal>
          </td>
          <td>
            <asp:Literal ID="ltlConsignee" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlIsDefault" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlDeleteUrl" runat="server"></asp:Literal>
          </td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button class="btn" text="返 回" onclick="Return_OnClick" runat="server" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->