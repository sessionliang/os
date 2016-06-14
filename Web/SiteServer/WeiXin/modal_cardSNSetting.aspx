<%@ Page Language="C#" ValidateRequest="false" Inherits="SiteServer.WeiXin.BackgroundPages.Modal.CardSNSetting" Trace="false" %>

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
        <asp:Button ID="btnSubmit" UseSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" Style="display: none" />
        <bairong:Alerts runat="server"></bairong:Alerts>

        <link href="css/emotion.css" rel="stylesheet">

        <table class="table table-noborder">
            <tr id="IsDisabledRow" runat="server">
                <td width="120">使用状态：</td>
                <td>
                    <asp:DropDownList ID="ddlIsDisabled" class="input-medium" runat="server" />
                </td>
            </tr>
            <tr id="IsBindingRow" runat="server">
                <td width="120">绑定状态：</td>
                <td>
                    <asp:DropDownList ID="ddlIsBinding" class="input-medium" runat="server" />
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
