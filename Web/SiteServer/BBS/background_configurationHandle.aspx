<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundConfigurationHandle" enableViewState = "false" %>

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

  <asp:Literal ID="ltlScript" runat="server"></asp:Literal>

  <table class="table table-bordered table-hover">
    <tr class="info thead">
      <td>版块名称</td>
      <td class="center" width="70">是否监控</td>
      <td class="center" width="70">&nbsp;</td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr treeItemLevel="<%# GetTreeItemLevel((int)Container.DataItem) %>" >
          <td><asp:Literal ID="ltlTitle" runat="server"></asp:Literal></td>
          <td class="center">
            <asp:Literal ID="ltlIsHandle" runat="server"></asp:Literal>
          </td>
          <td class="center"><asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal></td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <br />

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->