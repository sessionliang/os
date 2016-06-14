<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.Modal.KeywordAdd" Trace="false" %>

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
                <td width="80">敏感词：</td>
                <td>
                    <asp:TextBox Columns="30" ID="tbKeyword" runat="server" />
                    <asp:RequiredFieldValidator ControlToValidate="tbKeyword" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" /></td>
            </tr>
            <tr>
                <td>替换为：</td>
                <td>
                    <asp:TextBox Columns="30" ID="tbAlternative" runat="server" />
                    <br />
                    <span class="gray">可以为空</span></td>
            </tr>
            <tr>
                <td>等级：</td>
                <td>
                    <asp:DropDownList ID="ddlGrade" runat="server"></asp:DropDownList></td>
            </tr>
        </table>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
