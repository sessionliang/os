<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.Modal.ContentCheck" Trace="false" %>

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
                <td width="120">内容标题：</td>
                <td>
                    <asp:Literal ID="ltlTitles" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td>设置审核状态：</td>
                <td>
                    <asp:RadioButtonList ID="rblCheckType" runat="server"></asp:RadioButtonList></td>
            </tr>
            <tr>
                <td>
                    <bairong:Help HelpText="定时审核" Text="定时审核：" runat="server"></bairong:Help>
                </td>
                <td>
                    <asp:DropDownList ID="IsCheckTask" OnSelectedIndexChanged="IsCheckTask_SelectedIndexChanged" AutoPostBack="true" runat="server" Width="55">
                        <asp:ListItem Text="是" />
                        <asp:ListItem Text="否" Selected="True" />
                    </asp:DropDownList>
                    <asp:PlaceHolder ID="PlaceHolder_CheckTask" runat="server">&nbsp;审核任务开始时刻：<bairong:DateTimeTextBox ID="DateOnlyOnce" class="input-small" Width="136" Columns="12" runat="server" ShowTime="true" />
                        <asp:RequiredFieldValidator ControlToValidate="DateOnlyOnce" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                    </asp:PlaceHolder>
                </td>
            </tr>
            <tr>
                <td>
                    <bairong:Help HelpText="定时下架" Text="定时下架：" runat="server"></bairong:Help>
                </td>
                <td>
                    <asp:DropDownList ID="IsUnCheckTask" OnSelectedIndexChanged="IsUnCheckTask_SelectedIndexChanged" AutoPostBack="true" runat="server" Width="55">
                        <asp:ListItem Text="是" />
                        <asp:ListItem Text="否" Selected="True" />
                    </asp:DropDownList>
                    <asp:PlaceHolder ID="PlaceHolder_UnCheckTask" runat="server">&nbsp;下架任务开始时刻：
                        <bairong:DateTimeTextBox ID="DateUnCheck" class="input-small" Width="136" Columns="12" runat="server" ShowTime="true" />
                        <asp:RequiredFieldValidator ControlToValidate="DateUnCheck" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                    </asp:PlaceHolder>
                </td>
            </tr>

            <tr>
                <td>转移到栏目：</td>
                <td>
                    <asp:DropDownList ID="ddlTranslateNodeID" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>原因：</td>
                <td>
                    <asp:TextBox ID="tbCheckReasons" TextMode="MultiLine" Width="98%" Rows="3" runat="server" />
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
