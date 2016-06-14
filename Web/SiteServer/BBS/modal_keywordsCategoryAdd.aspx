<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.Modal.KeywordsCategoryAdd" %>

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
<asp:Button id="btnSubmit" useSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" style="display:none" />
<bairong:alerts runat="server"></bairong:alerts>

    <table class="table table-noborder table-hover">
    <tr>
        <td>分类名称：</td>
        <td>
            <asp:TextBox Width="220" ID="tbName" runat="server" />
            <asp:RequiredFieldValidator ID="valrName" runat="server" ErrorMessage=" *" foreColor="red" ControlToValidate="tbName"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td>是否开启：</td>
        <td>
            <asp:RadioButtonList ID="rblOpen" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Selected="True" Text="开启" Value="true"></asp:ListItem>
                <asp:ListItem Text="停用" Value="False"></asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->