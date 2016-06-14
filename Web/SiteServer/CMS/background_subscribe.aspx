<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundSubscribe" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
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
                <td class="center" style="width: 20px;">序号</td>
                <td class="center" style="width: 180px;">订阅内容</td>
                <td class="center" style="width: 80px;">订阅次数</td>
                <td class="center" style="width: 80px;">内容类型</td>
                <td class="center" style="width: 60px;">状态</td>
                <td class="center" style="width: 40px;">&nbsp;</td>
                <td class="center" style="width: 40px;">&nbsp;</td>
                <td width="20" class="center">
                    <input onclick="_checkFormAll(this.checked)" type="checkbox" />
                </td>
            </tr>
            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Literal ID="ItemNum" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ItemSubscribeName" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ItemSubscribeNum" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ItemContentType" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ItemEnabled" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ItemEidtRow" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ItemDelRow" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <input type="checkbox" name="ContentIDCollection" value='<%#DataBinder.Eval(Container.DataItem, "ItemID")%>' />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>

        <bairong:SqlPager ID="spContents" runat="server" class="table table-pager" />

        <ul class="breadcrumb breadcrumb-button">
            <asp:Button class="btn btn-success" ID="AddButton" Text="添加" runat="server" />
            <asp:Button class="btn" ID="Delete" Text="删 除" runat="server" />
            <asp:Button class="btn" ID="btnTrue" Text="恢 复" runat="server" />
            <asp:Button class="btn" ID="btnFalse" Text="暂 停" runat="server" />
        </ul>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
