<%@ Page Language="C#" ValidateRequest="false" Inherits="SiteServer.WeiXin.BackgroundPages.Modal.AppointmentContentDetail" Trace="false" %>

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
        <bairong:Alerts runat="server"></bairong:Alerts>
        <table class="table table-noborder">
            <tr>
                <td>预约名称：</td>
                <td>
                    <asp:Literal ID="ltlAppointementTitle" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>姓名：</td>
                <td>
                    <asp:Literal ID="ltlRealName" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>电话：</td>
                <td>
                    <asp:Literal ID="ltlMobile" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>邮箱：</td>
                <td>
                    <asp:Literal ID="ltlEmail" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>预约时间：</td>
                <td>
                    <asp:Literal ID="ltlAddDate" runat="server"></asp:Literal>
                </td>
            </tr>
            <asp:Literal ID="ltlExtendVal" runat="server"></asp:Literal>
        </table>
    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->

