<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundDocumentLeft" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
</head>

<body>
<!--#include file="../inc/openWindow.html"-->
<form id="myForm" style="margin:0" runat="server">

  <table class="table table-bordered table-hover" width="95%">
  <tr class="info thead">
    <td>文档大类</td><td>文档小类</td>
  </tr>
  <asp:repeater id="rptContents" runat="server">
    <ItemTemplate>
      <tr>
        <td>
          <asp:Literal ID="ltlTypeName" runat="server"></asp:Literal>
        </td>
        <td>

        </td>
      </tr>
      <asp:Literal ID="ltlSubTypes" runat="server"></asp:Literal>
    </ItemTemplate>
  </asp:repeater>
  </table>

</form>
</body>
</html>