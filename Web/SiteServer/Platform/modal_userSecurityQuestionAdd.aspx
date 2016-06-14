<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.Modal.UserSecurityQuestionAdd" %>

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
        <bairong:Alerts runat="server" />

        <div class="popover popover-static">
            <h3 class="popover-title"></h3>
            <div class="popover-content">

                <table class="table noborder table-hover">
                    <tr>
                        <td width="120">问题：</td>
                        <td>
                            <asp:TextBox ID="tbQuestion" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator
                                ControlToValidate="tbQuestion"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td width="120">是否可用：</td>
                        <td>
                            <asp:RadioButtonList ID="rbIsEnable" runat="server" RepeatDirection="Horizontal"></asp:RadioButtonList>
                            <asp:RequiredFieldValidator
                                ControlToValidate="rbIsEnable"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic"
                                runat="server" />
                        </td>
                    </tr>
                </table>

            </div>
        </div>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
