<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundLink" %>

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
      <td class="center">链接名称</td>
      <td class="center">链接地址</td>
      <td class="center">LOGO</td>
      <td></td>
      <td></td>
      <td></td>
      <td width="20">
        <input onclick="_checkFormAll(this.checked)" type="checkbox" />
      </td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
          <td class="center" style="width: 30px;"><asp:Literal ID="ltlNum" runat="server"></asp:Literal>
            <asp:Literal ID="ltlID" runat="server" Visible="false"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlLinkName" runat="server"></asp:Literal></td>
          <td class="center" ><asp:Literal ID="ltlLinkUrl" runat="server"></asp:Literal></td>
          <td class="center" ><asp:Literal ID="ltlIconUrl" runat="server"></asp:Literal></td>
          <td class="center" ><asp:HyperLink ID="hlUpLink" runat="server"><img src="../Pic/icon/up.gif" border="0" alt="上升" /></asp:HyperLink></td>
          <td class="center" ><asp:HyperLink ID="hlDownLink" runat="server"><img src="../Pic/icon/down.gif" border="0" alt="下降" /></asp:HyperLink></td>
          <td class="center" style="width: 80px;"><asp:Literal ID="ltlEdit" runat="server"></asp:Literal></td>
          <td class="center" style="width: 50px;"><asp:CheckBox ID="chk_ID" runat="server" /></td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button ID="btnAdd" Cssclass="btn btn-success" runat="server" Text="添 加"/>
    <asp:Button ID="btnDelete" Cssclass="btn" runat="server" Text="删 除" OnClick="btnDelete_Click" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->