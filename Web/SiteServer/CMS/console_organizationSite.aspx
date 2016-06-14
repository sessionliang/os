<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundConfiguration" %>

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

        <div class="popover popover-static">
            <h3 class="popover-title">分支机构设置</h3>
            <div class="popover-content">

                <table class="table noborder table-hover">
                    <tr>
                        <td style="width: 200px;">百度地图AK：</td>
                        <td>
                            <asp:TextBox class="input" ID="OrganizationBaiduAK" runat="server" /> 
                            <span class="gray">分支机构可以启用百度地图，请输入AK值，AK值为空时将不能加载百度地图</span>
                        </td>
                    </tr>
                    <tr>
                        <td>是否启用跨域：</td>
                        <td>
                            <asp:RadioButtonList ID="OrganizationIsCrossDomain" RepeatDirection="Horizontal" class="radiobuttonlist" runat="server"></asp:RadioButtonList>
                        </td>
                    </tr>
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
<!-- check for 3.6 html permissions -->
