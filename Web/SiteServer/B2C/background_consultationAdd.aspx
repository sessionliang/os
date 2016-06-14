<%@ Page Language="C#" ValidateRequest="false" Inherits="SiteServer.B2C.BackgroundPages.BackgroundConsultationAdd" %>

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

        <div class="popover popover-static">
            <h3 class="popover-title">
                <asp:Literal ID="ltlPageTitle" runat="server" /></h3>
            <div class="popover-content">

                <table class="table noborder">
                    <tr>
                        <td align="right">问题类型：</td>
                        <td>
                            <asp:DropDownList ID="type" runat="server"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td width="80" align="right">咨询内容：</td>
                        <td colspan="3">
                            <asp:TextBox ID="question" runat="server" TextMode="MultiLine" Width="100%" Height="150"></asp:TextBox>
                        </td>
                    </tr>
                </table>

                <hr />
                <table class="table noborder">
                    <tr>
                        <td class="center">
                            <asp:Button class="btn btn-primary" ID="Submit" Text="确 定" OnClick="Submit_OnClick" runat="server" />
                            <asp:Button class="btn" ID="Return" Text="返 回" OnClick="Return_OnClick" CausesValidation="false" runat="server" />
                        </td>
                    </tr>
                </table>

            </div>
        </div>


    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
