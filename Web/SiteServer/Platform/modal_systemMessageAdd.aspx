<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.Modal.SystemMessageAdd" %>

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

        <div class="popover popover-static">
            <h3 class="popover-title"></h3> 
            <div class="popover-content">

                <table class="table noborder table-hover">
                    <asp:PlaceHolder ID="phMessage" runat="server">
                        <tr>
                            <td width="120">通知标题：</td>
                            <td>
                                <asp:TextBox ID="tbMessageTitle" Width="360" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator
                                    ControlToValidate="tbMessageTitle"
                                    ErrorMessage=" *" ForeColor="red"
                                    Display="Dynamic"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td width="120">通知内容：</td>
                            <td>
                                <bairong:BREditor ID="breMessageContent" runat="server"></bairong:BREditor>
                            </td>
                        </tr>
                        <tr>
                            <td width="120">状态：</td>
                            <td>
                                <asp:DropDownList ID="ddlIsViewed" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                </table>

            </div>
        </div>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
