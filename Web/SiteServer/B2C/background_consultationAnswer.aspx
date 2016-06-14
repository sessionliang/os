<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.BackgroundConsultationAnswer" %>

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

        <div class="well well-small">
            <h5>咨询：<asp:Literal ID="ltlPageTitle" runat="server" />
            </h5>
        </div>

        <asp:PlaceHolder ID="phAnswer" runat="server">

            <hr />

            <div class="popover popover-static">
                <h3 class="popover-title">回复</h3>
                <div class="popover-content">

                    <table class="table noborder table-noborder">
                        <tr>
                            <td>
                                <asp:TextBox ID="answer" runat="server" TextMode="MultiLine" Width="100%" Height="150"></asp:TextBox>
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

        </asp:PlaceHolder>

    </form>
</body>

</html>
