<%@ Page Language="C#" Inherits="SiteServer.WeiXin.BackgroundPages.BackgroundWifiNode" %>

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
                <td>路由ID</td>
                <td>路由名称</td>
                <td>微信绑定</td>
                <td>微信ID</td>
                <td>在线人数</td>
                <td>新粉丝</td>
                <td>旧粉丝</td>
                <td>游客</td>
                <td></td>
                <td></td>
                <td width="20">
                    <input type="checkbox" onclick="selectRows(document.getElementById('contents'), this.checked);" /></td>
            </tr>
            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <tr>
                        <td class="center">
                            <asp:Literal ID="ltlNodeID" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ltlNodeName" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlWxBind" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlWechatID" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlOnline" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlNewFans" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlFans" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlVisiter" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlbtnEdite" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlSelectClient" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <input type="checkbox" name="WifiNodeIDCollection" value='<%#DataBinder.Eval(Container.DataItem, "id")%>' />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <ul class="breadcrumb breadcrumb-button">
            <asp:Button class="btn btn-success" ID="btnAdd" Text="添 加" runat="server" />
            <asp:Button class="btn" ID="btnDelete" Text="删 除" runat="server" />
        </ul>

    </form>
</body>
</html>
