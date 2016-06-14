<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.Modal.SpecDefault" Trace="false" %>

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

        <table class="table table-noborder table-hover">
            <tr>
                <td width="120">规格名称：</td>
                <td>
                    <asp:Literal ID="ltlSpec" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td>默认选择：</td>
                <td>
                    <asp:CheckBoxList ID="cblDefault" class="checkboxlist" RepeatDirection="Horizontal" RepeatColumns="3" runat="server"></asp:CheckBoxList>
                    <asp:RadioButtonList ID="rblDefault" class="radiobuttonlist" RepeatDirection="Horizontal" RepeatColumns="3" runat="server"></asp:RadioButtonList>
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
<!-- check for 3.6.4 html permissions -->
