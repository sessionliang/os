<%@ Page Language="C#" ValidateRequest="false" Inherits="SiteServer.CMS.BackgroundPages.Modal.WebsiteMessageContentReply" %>

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
            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <tr>
                        <td width="80">
                            <asp:Literal ID="ltlDataKey" runat="server" />：
                        </td>
                        <td>
                            <asp:Literal ID="ltlDataValue" runat="server" /></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr>
                <td>回复模板：</td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlReplayTemplate" AutoPostBack="true" OnSelectedIndexChanged="ddlReplayTemplate_SelectedIndexChanged"></asp:DropDownList></td>
            </tr>
            <asp:PlaceHolder runat="server" ID="phSendSMS" Visible="false">
                <tr>
                    <td style="width: 150px;">是否发送手机短信：</td>
                    <td>
                        <asp:RadioButtonList runat="server" ID="rblIsSendSMS" RepeatDirection="Horizontal"></asp:RadioButtonList></td>
                </tr>
            </asp:PlaceHolder>
            <tr>
                <td colspan="2">
                    <bairong:BREditor ID="breReply" runat="server"></bairong:BREditor>
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
