<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundDownload" %>

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
        <td align="center">域名</td>
        <td align="center">版本</td>
        <td align="center">升级日期</td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
            <td>
                <asp:Literal ID="ltlDomain" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlVersion" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlDownloadDate" runat="server"></asp:Literal>
            </td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

</form>
</body>
</html>