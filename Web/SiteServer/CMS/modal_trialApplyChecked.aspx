<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.Modal.TrialApplyChecked" Trace="false" %>

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
        <table class="table table-noborder table-hover">
            <tr>
                <td>是否审核通过：</td>
                <td>
                    <asp:RadioButtonList ID="rblIsChecked" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblIsChecked_OnSelectedIndexChanged" RepeatDirection="Horizontal"></asp:RadioButtonList></td>
            </tr>
            <asp:PlaceHolder ID="phIsChecked" runat="server" Visible="false">
                <tr>
                    <td>是否回馈试用报告：</td>
                    <td>
                        <asp:RadioButtonList ID="rblIsReport" runat="server" RepeatDirection="Horizontal"></asp:RadioButtonList></td>
                </tr>
                <tr>
                    <td>是否手机提醒：</td>
                    <td>
                        <asp:RadioButtonList ID="rblIsMobile" runat="server" RepeatDirection="Horizontal"></asp:RadioButtonList></td>
                </tr>
            </asp:PlaceHolder>
        </table>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
