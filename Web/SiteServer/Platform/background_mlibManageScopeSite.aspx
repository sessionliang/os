<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundMLibManageScopeSite" %>

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
            <h3 class="popover-title">投稿范围设置</h3>
            <div class="popover-content">

                <table class="table noborder">
                    <tr class="info">
                        <td colspan="2">请设置稿件可投递的站点和栏目：点击下列链接，进入对应站点的栏目范围设置界面</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Literal ID="ltlPublishmentSystems" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <span>注：点击网站选择稿件允许投递的栏目&nbsp;&nbsp;<img src="../pic/canedit.gif" align="absmiddle" />稿件可投递的网站&nbsp;
                                            <img src="../pic/cantedit.gif" align="absmiddle" />不在投递范围的网站</span>
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
