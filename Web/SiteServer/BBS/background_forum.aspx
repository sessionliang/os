<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundForum" enableViewState = "false" %>

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
    <td></td>
    <td width="100p">版主</td>
    <td width="100">版块索引</td>
    <td width="30">上升</td>
    <td width="30">下降</td>
    <td width="50">&nbsp;</td>
    <td width="50"></td>
    <td width="20">
      <input onclick="_checkFormAll(this.checked)" type="checkbox" />
    </td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr treeItemLevel="<%# GetTreeItemLevel((int)Container.DataItem) %>" >
            <td>
                <asp:Literal ID="ltlTitle" runat="server"></asp:Literal>
            </td>
            <td>
                <nobr><asp:Literal ID="ltlModerator" runat="server"></asp:Literal></nobr>
            </td>
            <td align="align-right">
                <nobr><asp:Literal ID="ltlIndexName" runat="server"></asp:Literal></nobr>
            </td>
            <td class="center">
                <asp:Literal ID="ltlUpLink" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlDownLink" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlDeleteLink" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlCheckBox" runat="server"></asp:Literal>
            </td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button class="btn btn-success" id="AddForum" Text="快速添加" runat="server" />
    <asp:Button class="btn" id="Translate" Text="版块合并" runat="server" />
    <!--<asp:Button class="btn" id="btnState" Text="状 态" runat="server" />-->
    <asp:Button class="btn" id="Delete" Text="删 除" runat="server" />
    <asp:Button class="btn" id="Import" Text="导 入" runat="server" Visible="false" />
    <asp:Button class="btn" id="Export" Text="导 出" runat="server" Visible="false" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->