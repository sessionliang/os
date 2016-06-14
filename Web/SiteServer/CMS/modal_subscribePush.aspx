<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.Modal.SubscribePush" Trace="false" %>

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
        <asp:Button ID="btnSubmit" UseSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" Style="display: none" />
        <bairong:Alerts runat="server"></bairong:Alerts>
        <div class="popover popover-static">
            <h3 class="popover-title">手动推送订阅信息</h3>
            <div class="popover-content">
                <table class="table table-noborder table-hover">
                    <tr>
                        <td width="80">邮箱：</td>
                        <td>
                            <asp:Literal ID="ltlEmail" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>确定给以上邮箱推送订阅内容吗？
                        </td>
                    </tr>
                </table>

                <table class="table noborder">
                    <tr>
                        <td class="center">
                            <asp:Button class="btn btn-primary" ID="Submit" Text="确 定" OnClick="Submit_OnClick" runat="server" />
                            <asp:Button class="btn btn-primary" ID="btReturn" OnClick="btReturn_OnClick" Text="返 回" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
