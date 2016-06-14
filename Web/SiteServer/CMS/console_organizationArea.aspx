<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.ConsoleOrganizationArea" EnableViewState="false" %>

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
                <td>区域名称</td>
                <td width="150">机构数量</td>
                <td width="30">上升</td>
                <td width="30">下降</td>
                <td width="50">&nbsp;</td>
                <td width="20"></td>
            </tr>
            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <asp:Literal ID="ltlHtml" runat="server" />
                </ItemTemplate>
            </asp:Repeater>
        </table>

        <ul class="breadcrumb breadcrumb-button">
            <asp:PlaceHolder ID="PlaceHolder_AddChannel" runat="server">
                <asp:Button class="btn btn-success" ID="AddChannel2" Text="添 加" runat="server" />

                <asp:PlaceHolder ID="PlaceHolder_Delete" runat="server">
                    <asp:Button class="btn" ID="Delete" Text="删 除" runat="server" />
                </asp:PlaceHolder>
            </asp:PlaceHolder>
        </ul>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
