<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.BackgroundConfiguration" %>

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
        <bairong:Alerts runat="server"></bairong:Alerts>

        <div class="popover popover-static">
            <h3 class="popover-title">商品配置</h3>
            <div class="popover-content">

                <table class="table noborder table-hover">
                    <tr>
                        <td width="120">商品类型：</td>
                        <td>
                            <asp:DropDownList ID="ddlIsVirtualGoods" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td width="120">是否使用品牌栏目：</td>
                        <td>
                            <asp:DropDownList ID="ddlIsUseBrandNode" runat="server" OnSelectedIndexChanged="ddlIsUseBrandNode_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </td>
                    </tr>
                    <asp:PlaceHolder runat="server" ID="phBrandNode">
                        <tr>
                            <td width="120">品牌类型：</td>
                            <td>
                                <asp:DropDownList ID="ddlBrandNode" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                </table>

                <hr />
                <table class="table noborder">
                    <tr>
                        <td class="center">
                            <asp:Button class="btn btn-primary" ID="Submit" Text="确 定" OnClick="Submit_OnClick" runat="server" />
                        </td>
                    </tr>
                </table>

            </div>
        </div>

    </form>
</body>
</html>
<!-- check for 3.6.4 html permissions -->
