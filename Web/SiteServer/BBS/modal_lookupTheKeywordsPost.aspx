<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.Modal.LookupTheKeywordsPost" %>

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
            <td>
                <asp:Literal ID="ltlTitle" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="ltlThread" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="ltlContent" runat="server"></asp:Literal>
            </td>
        </tr>
    </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->