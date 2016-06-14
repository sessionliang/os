<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundKeywordsCategory" %>

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
      <td class="center">序号</td>
      <td class="center">名称</td>
      <td class="center">词库数量</td>
      <td class="center">是否开启</td>
      <td></td>
      <td></td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
          <td class="center" style="width: 30px;">
              <asp:Literal ID="ltlNum" runat="server"></asp:Literal>
              <asp:Literal ID="ltlCategoryID" runat="server" Visible="false"></asp:Literal>
          </td>
          <td class="center">
              <asp:Literal ID="ltlName" runat="server"></asp:Literal>
          </td>
          <td class="center">
              <asp:Literal ID="ltlCount" runat="server"></asp:Literal>
          </td>
          <td class="center">
              <asp:Literal ID="ltlisOpen" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
          </td>
          <td class="center" style="width: 80px;">
            <asp:Literal ID="ltlDeleteUrl" runat="server"></asp:Literal>
          </td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button ID="btnAddCategory" Cssclass="btn btn-success" runat="server" Text="添 加" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->