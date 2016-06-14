<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.Modal.NoticeTemplateEdit" %>

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

        <table class="table noborder table-hover">
            <asp:PlaceHolder ID="phEmail" runat="server" Visible="false">
                <tr>
                    <td width="70">邮件模板：</td>
                    <td>
                        <asp:DropDownList ID="ddlEmail" runat="server" OnSelectedIndexChanged="ddlEmail_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td width="70">邮件标题：</td>
                    <td>
                        <asp:TextBox ID="tbEmailTitle" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="tbEmailTitle" runat="server" ErrorMessage=" *" ForeColor="Red"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td width="70">邮件内容：</td>
                    <td>
                        <asp:TextBox ID="tbEmailContent" runat="server" TextMode="MultiLine" Width="90%" Height="150"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="tbEmailContent" runat="server" ErrorMessage=" *" ForeColor="Red"></asp:RequiredFieldValidator></td>
                </tr>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="phPhone" runat="server" Visible="false">
                <tr>
                    <td width="70">短信模板：</td>
                    <td>
                        <asp:DropDownList ID="ddlPhone" runat="server" OnSelectedIndexChanged="ddlPhone_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td width="70">短信内容：</td>
                    <td>
                        <asp:TextBox ID="tbPhoneContent" runat="server" TextMode="MultiLine" Width="90%" Height="150"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="tbPhoneContent" runat="server" ErrorMessage=" *" ForeColor="Red"></asp:RequiredFieldValidator></td>
                </tr>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="phMessage" runat="server" Visible="false">
                <tr>
                    <td width="70">站内信模板：</td>
                    <td>
                        <asp:DropDownList ID="ddlMessage" runat="server" OnSelectedIndexChanged="ddlMessage_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td width="80">站内信标题：</td>
                    <td>
                        <asp:TextBox ID="tbMessageTitle" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="tbMessageTitle" runat="server" ErrorMessage=" *" ForeColor="Red"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td width="80">站内信内容：</td>
                    <td>
                        <asp:TextBox ID="tbMessageContent" runat="server" TextMode="MultiLine" Width="90%" Height="150"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="tbMessageContent" runat="server" ErrorMessage=" *" ForeColor="Red"></asp:RequiredFieldValidator></td>
                </tr>
            </asp:PlaceHolder>
        </table>
    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
