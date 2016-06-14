<%@ Page Language="C#" ValidateRequest="false" Inherits="BaiRong.BackgroundPages.BackgroundUserMessage" %>

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
            <h3 class="popover-title">短消息群发</h3>
            <div class="popover-content">

                <table class="table noborder table-hover">
                    <tr>
                        <td width="140">短消息发送对象：</td>
                        <td>
                            <asp:RadioButtonList ID="rblSelect" RepeatDirection="Horizontal" class="noborder" OnSelectedIndexChanged="rblSelect_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:RadioButtonList>
                        </td>
                    </tr>
                    <asp:PlaceHolder ID="phUser" Visible="false" runat="server">
                        <tr>
                            <td>用户名：</td>
                            <td>
                                <asp:TextBox Width="360" Rows="4" TextMode="MultiLine" ID="tbUserNameList" runat="server" />
                                <asp:RequiredFieldValidator ControlToValidate="tbUserNameList" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                                <asp:Literal ID="ltlSelectUser" runat="server" />
                                <br>
                                <span class="gray">（要发送的用户名列表，多个用户以“,”分割）</span>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <tr>
                        <td>短消息标题：</td>
                        <td>
                            <asp:TextBox ID="tbTitle" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>短消息正文：</td>
                        <td>
                            <bairong:BREditor ID="breContent" runat="server"></bairong:BREditor>
                        </td>
                    </tr>
                </table>

                <hr />
                <table class="table noborder">
                    <tr>
                        <td class="center">
                            <asp:Button class="btn btn-primary" ID="Submit" Text="发 送" OnClick="Submit_OnClick" runat="server" />
                        </td>
                    </tr>
                </table>

            </div>
        </div>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
