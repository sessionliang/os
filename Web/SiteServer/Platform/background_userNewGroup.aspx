<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundUserNewGroup" %>

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
        <asp:Literal ID="ltlBreadCrumb" runat="server" />
        <bairong:Alerts runat="server" />

        <table class="table table-bordered table-hover">
            <tr class="info thead">
                <td>用户组</td>
                <td width="100">用户总数</td>
                <td width="150">添加时间</td>
                <td width="150">&nbsp;</td>
                <td width="50">&nbsp;</td>
                <td width="20">
                    <input onclick="_checkFormAll(this.checked)" type="checkbox" />
                </td>
            </tr>
            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Literal ID="ltlItemName" runat="server"></asp:Literal></td>
                        <td class="center">
                            <asp:Literal ID="ltlUserCount" runat="server"></asp:Literal></td>
                        <td class="center">
                            <asp:Literal ID="ltlCreateDate" runat="server"></asp:Literal></td>
                        <td class="center">
                            <asp:HyperLink ID="hlEditLink" Text="编辑" runat="server"></asp:HyperLink>&nbsp;&nbsp;
                             <asp:HyperLink ID="hlUserLink" Text="用户" runat="server"></asp:HyperLink>&nbsp;&nbsp;
              <asp:HyperLink ID="hlMLibLink" Text="投稿设置" runat="server"></asp:HyperLink>
                        </td>
                        <td class="center">
                            <asp:HyperLink ID="hlDelete" Text="删除" runat="server"></asp:HyperLink>
                        </td>
                        <td class="center" width="20px">
                            <asp:Literal ID="ltlSelect" runat="server"></asp:Literal></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>

        <bairong:SqlPager ID="spContents" runat="server" class="table table-pager" />

        <ul class="breadcrumb breadcrumb-button">
            <asp:Button class="btn btn-success" ID="Add" Text="添加用户组" runat="server" />
            <asp:Button class="btn" ID="Delete" Text="删 除" runat="server" />
        </ul>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
