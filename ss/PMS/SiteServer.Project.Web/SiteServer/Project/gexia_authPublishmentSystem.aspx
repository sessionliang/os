<%@ Page Language="C#" Inherits="SiteServer.GeXia.BackgroundPages.BackgroundAuthPublishmentSystem" %>

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
      <td>用户</td>
      <td>用户类型</td>
      <td>应用类型</td>
      <td>应用ID</td>
      <td>创建时间</td>
      <td>到期时间</td>
      <td>绑定域名</td>
      <td>自定义版权</td>
      <td width="20">
        <input onclick="_checkFormAll(this.checked)" type="checkbox" />
      </td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
          <tr>
            <td class="center">
              <asp:Literal ID="ltlAuthUserName" runat="server"></asp:Literal>
            </td>
            <td class="center">
              <asp:Literal ID="ltlAuthUserType" runat="server"></asp:Literal>
            </td>
            <td class="center">
              <asp:Literal ID="ltlPublishmentSystemType" runat="server"></asp:Literal>
            </td>
            <td class="center">
              <asp:Literal ID="ltlPublishmentSystemID" runat="server"></asp:Literal>
            </td>
            <td class="center">
              <asp:Literal id="ltlAddDate" runat="server" />
            </td>
            <td class="center">
              <asp:Literal id="ltlIsExpiration" runat="server" />
            </td>
            <td class="center">
              <asp:Literal id="ltlIsDomain" runat="server" />
            </td>
            <td class="center">
              <asp:Literal id="ltlIsPoweredBy" runat="server" />
            </td>
            <td class="center">
              <asp:Literal ID="ltlSelect" runat="server"></asp:Literal>
            </td>
          </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button class="btn btn-success" id="btnAdd" Text="新 增" runat="server" />
    <asp:Button class="btn btn-danger" id="btnDelete" Text="删 除" runat="server" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->