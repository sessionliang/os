<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundSusceptivityPostPassed" %>

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
        <td class="center">
                序号
        </td>
        <td class="center">
            主题
        </td>
        <td class="center">
            发布者
        </td>
        <td class="center">
            审核状态
        </td>
        <td class="center">
            审核员
        </td>
        <td>
            操作时间
        </td>
        <td></td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
            <td class="center" style="width: 30px;">
                <asp:Literal ID="ltlNum" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlThread" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlUser" runat="server"></asp:Literal>
            </td>
            <td class="center">
                审核通过
            </td>
            <td class="center">
                <asp:Literal ID="ltlAssessor" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlTime" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlLookUp" runat="server"></asp:Literal>
            </td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->