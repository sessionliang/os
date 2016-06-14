<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.Modal.SubscribeAdd" Trace="false" %>

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
                <td width="80">内容名称：</td>
                <td>
                    <asp:TextBox Columns="60" ID="tbSubscribeName" runat="server" />
                    <asp:RequiredFieldValidator ControlToValidate="tbSubscribeName" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" /></td>
            </tr>
            <tr>
                <td>内容值：</td>
                <td>
                    <asp:TextBox Columns="60" ID="tbSubscribeValue" runat="server" />
                    <asp:RequiredFieldValidator ControlToValidate="tbSubscribeValue" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" /><span class="gray">多个用‘，’或空格隔开</span>
                    <br />
                    <span class="gray">请填写站内信息管理【信息订阅】栏目下存在的内容标签和栏目名称</span></td>
            </tr>
            <tr>
                <td>内容类型：</td>
                <td>
                    <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal">
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td>状态：</td>
                <td>
                    <asp:RadioButtonList ID="rblEnabled" runat="server" RepeatDirection="Horizontal">
                    </asp:RadioButtonList></td>
            </tr>
        </table>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
