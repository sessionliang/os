<%@ Page Language="C#" Inherits="SiteServer.WeiXin.BackgroundPages.BackgroundWifiNodeClient" %>

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

        <script type="text/javascript">
            $(document).ready(function () {
                loopRows(document.getElementById('contents'), function (cur) { cur.onclick = chkSelect; });
                $(".popover-hover").popover({ trigger: 'hover', html: true });
            });
        </script>

        <table id="contents" class="table table-bordered table-hover">
            <tr class="info thead">
                <td>ID</td>
                <td>连接开始时间</td>
                <td>节点ID</td>
                <td>上行</td>
                <td>下行</td>
                <td>IP</td>
                <td>MAC地址</td>
                <td>上次更新时间</td>
                <td>上网类型</td>
                <td>用户类型</td>

            </tr>
            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <tr>
                        <td class="center">
                            <asp:Literal ID="ltlID" runat="server"></asp:Literal>
                        </td> 
                        <td class="center">
                            <asp:Literal ID="ltlCreatedAt" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ltlNodeID" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlInCome" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlOutGo" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlIP" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlMac" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlUpdatedAt" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlAuthType" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlUserType" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <ul class="breadcrumb breadcrumb-button">
            <asp:Button class="btn btn-success" ID="btnReturn" Text="返 回" runat="server" />
        </ul>
    </form>
</body>
</html>
