<%@ Page Language="C#" Inherits="SiteServer.WeiXin.BackgroundPages.BackgroundRight" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <link href="css/add.css" rel="stylesheet" />
    <!--#include file="../inc/header.aspx"-->
</head>

<body>
    <!--#include file="../inc/openWindow.html"-->
    <form class="form-inline" runat="server">
        <asp:Literal ID="ltlBreadCrumb" runat="server" />
        <bairong:Alerts runat="server" />
        <div class="well well-small">
            <table class="table table-noborder">
                <tr>
                    <td>
                        <div>
                            <h4 class="heading-inline">
                                <asp:Literal ID="ltlWelcome" runat="server"></asp:Literal>
                                &nbsp;&nbsp;<small></small>
                            </h4>
                        </div>

                    </td>
                </tr>
            </table>
        </div>
        <div class="popover popover-static">
            <h3 class="popover-title">
                <lan>微信公众号</lan>
            </h3>
            <div class="popover-content">
                <table class="table noborder table-hover">
                    <tr>
                        <td width="150">URL：</td>
                        <td>
                            <asp:Literal ID="ltlURL" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>Token：</td>
                        <td>
                            <asp:Literal ID="ltlToken" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </table>
                <asp:Literal ID="ltlBinding" runat="server"></asp:Literal>
                <asp:Literal ID="ltlDelete" runat="server" />
            </div>
        </div>         
    </form>
</body>
</html>

