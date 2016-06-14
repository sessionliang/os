<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.BackgroundShipment" %>

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
      <td width="200">配送方式名称</td>
      <td>配送方式描述</td>
      <td width="160">送货时间</td>
      <td width="80">是否启用</td>
      <td width="50"></td>
      <td width="50"></td>
      <td width="50"></td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
          <td><asp:Literal ID="ltlShipmentName" runat="server"></asp:Literal></td>
          <td><asp:Literal ID="ltlDescription" runat="server"></asp:Literal></td>
          <td class="center">
            <asp:Literal ID="ltlShipmentPeriod" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlIsEnabled" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlConfigUrl" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlIsEnabledUrl" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlDeleteUrl" runat="server"></asp:Literal>
          </td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <ul class="breadcrumb breadcrumb-button">
    <a class="btn btn-success" href="background_shipmentAdd.aspx?PublishmentSystemID=<%=PublishmentSystemID%>">添加配送方式</a>
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->