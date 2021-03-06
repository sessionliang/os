<%@ Page Language="C#" ValidateRequest="false" Inherits="SiteServer.WeiXin.BackgroundPages.Modal.CardCredits" Trace="false" %>

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
        <bairong:Code Type="ajaxupload" runat="server" />

        <table class="table table-noborder">
            <asp:PlaceHolder ID="phKeyWord" runat="server">
                <tr>
                    <td width="130">
                        <bairong:Help HelpText="选择会员卡" Text="选择会员卡：" runat="server"></bairong:Help>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCard" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td width="130">
                        <bairong:Help HelpText="选择方式" Text="选择方式：" runat="server"></bairong:Help>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlKeyWordType" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>
                        <bairong:Help HelpText="卡号/手机号" Text="卡号/手机号：" runat="server"></bairong:Help>
                    </td>
                    <td>
                        <asp:TextBox ID="tbKeyWord" MaxLength="50" Size="20" runat="server" />
                        <asp:RequiredFieldValidator
                            ControlToValidate="tbKeyWord"
                            ErrorMessage=" *" ForeColor="red"
                            Display="Dynamic"
                            runat="server" /></td>
                </tr>
            </asp:PlaceHolder>
            <tr>
                <td>
                    <bairong:Help HelpText="选择操作" Text="选择操作：" runat="server"></bairong:Help>
                </td>
                <td>
                    <asp:DropDownList ID="ddlOperatType" runat="server"></asp:DropDownList>
                </td>
            </tr>

            <tr>
                <td>
                    <bairong:Help HelpText="积分值" Text="积分值：" runat="server"></bairong:Help>
                </td>
                <td>
                    <asp:TextBox ID="tbCredits" MaxLength="50" Size="20" runat="server" />
                    <asp:RequiredFieldValidator
                        ControlToValidate="tbCredits"
                        ErrorMessage=" *" ForeColor="red"
                        Display="Dynamic"
                        runat="server" />
                    <asp:RegularExpressionValidator
                        runat="server"
                        ControlToValidate="tbCredits"
                        ValidationExpression="^[0-9]*$"
                        ErrorMessage="不合法" ForeColor="red"
                        Display="Dynamic" />
                </td>
            </tr>

        </table>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
