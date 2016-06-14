<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundHotfix" %>

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
        <td>升级版本</td>
        <td>升级补丁</td>
        <td>发布日期</td>
        <td>文字说明</td>
        <td>是否开启</td>
        <td>是否限制</td>
        <td width="60">升级次数</td>
        <td width="80">升级详情</td>
        <td width="80"></td>
        <td width="80"></td>
        <td width="60"></td>
        <td width="60"></td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
            <td>
                <asp:Literal ID="ltlVersion" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlHotfix" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlPubDate" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlMessage" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlIsEnabled" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlIsRestrict" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlDownloadCount" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlDownloadUrl" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlFileUrl" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlPageUrl" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <a href="product_hotfix.aspx?Delete=True&ID=<%# DataBinder.Eval(Container.DataItem,"ID")%>" onClick="javascript:return confirm('此操作将删除此升级包，确认吗？');">删除</a>
            </td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button class="btn btn-success" id="AddButton" Text="新增升级包" runat="server" />
  </ul>

</form>
</body>
</html>