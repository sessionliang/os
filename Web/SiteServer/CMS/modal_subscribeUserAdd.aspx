<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.Modal.SubscribeUserAdd" Trace="false" %>

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
                <td width="120">邮箱：</td>
                <td>
                    <asp:TextBox Columns="60" ID="tbEmail" runat="server" />
                    <asp:RequiredFieldValidator ControlToValidate="tbEmail" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                    <asp:RegularExpressionValidator
                        runat="server"
                        ControlToValidate="tbEmail"
                        ValidationExpression="^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$"
                        ErrorMessage=" *" ForeColor="red"
                        Display="Dynamic" /></td>
            </tr>
            <tr>
                <td>订阅内容：</td>
                <td>
                    <asp:CheckBoxList ID="cbSubscribe" runat="server" RepeatDirection="Horizontal" RepeatColumns="4" />
                </td>
            </tr>
            <tr>
                <td>是否短信通知：</td>
                <td>
                    <asp:RadioButtonList ID="reMobile" OnSelectedIndexChanged="reMobile_OnSelectedIndexChanged" AutoPostBack="true" RepeatDirection="Horizontal" class="noborder" runat="server">
                        <asp:ListItem Value="1">是</asp:ListItem>
                        <asp:ListItem Value="0" Selected="True">否</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <asp:PlaceHolder ID="phMobile" runat="server" Visible="false">
                <tr>
                    <td>手机号：</td>
                    <td>
                        <asp:TextBox Columns="60" ID="tbMobile" MaxLength="11" runat="server" />
                        <asp:RegularExpressionValidator
                            runat="server"
                            ControlToValidate="tbMobile"
                            ValidationExpression="^(0|86|17951)?(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$"
                            ErrorMessage=" *" ForeColor="red"
                            Display="Dynamic" /></td>
                </tr>
            </asp:PlaceHolder>
        </table>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
