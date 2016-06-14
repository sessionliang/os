<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundUserNewGroupUser" %>

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

        <div class="well well-small">
            <table class="table table-noborder">
                <tr>
                    <td> 
                        关键字：
          <asp:TextBox ID="tbKeyword" MaxLength="500" Size="45" runat="server" />
                        <asp:Button class="btn" OnClick="Search_OnClick" ID="Search" Text="搜 索" runat="server" />
                    </td>
                </tr>
            </table>
        </div>

        <table class="table table-bordered table-hover">
            <tr class="info thead">
                <td>登录名</td>
                <td>显示名</td>
                <%--                <td>用户等级</td>--%>
                <td>注册时间</td>
                <td>最后活动时间</td>
                <td>登录次数</td>
                <td>积分</td>
                <td>用户组</td>
                <asp:Literal ID="ltlColumnHeader" runat="server" />
                <td width="20">
                    <input onclick="_checkFormAll(this.checked)" type="checkbox" />
                </td>
            </tr>
            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Literal ID="ltlUserName" runat="server"></asp:Literal></td>
                        <td>
                            <asp:Literal ID="ltlDisplayName" runat="server"></asp:Literal></td>
                        <td>
                            <asp:Literal ID="ltlCreationDate" runat="server"></asp:Literal></td>
                        <td>
                            <asp:Literal ID="ltlLastActivityDate" runat="server"></asp:Literal></td>
                        <td>
                            <asp:Literal ID="ltlLoginCount" runat="server"></asp:Literal></td>
                        <td>
                            <asp:Literal ID="ltlCredits" runat="server"></asp:Literal></td>
                        <td>
                            <asp:Literal ID="ltlUserGroupName" runat="server"></asp:Literal></td>
                        <asp:Literal ID="ltlColumns" runat="server" />
                        <td class="center">
                            <asp:Literal ID="ltlSelect" runat="server"></asp:Literal></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <bairong:SqlCountPager ID="spContents" runat="server" class="table table-pager" />
        <ul class="breadcrumb breadcrumb-button">
            <asp:Button class="btn" ID="SetMLibValidityDate" Text="设置投稿有效期" runat="server" />
            <asp:Button class="btn" ID="Lock" Text="锁定用户" runat="server" />
            <asp:Button class="btn" ID="UnLock" Text="解除锁定" runat="server" />
            <asp:Button class="btn" ID="SendMail" Text="发送邮件" runat="server" />
            <asp:Button class="btn" ID="SendSMS" Text="发送短信" runat="server" Visible="false" />
            <asp:Button class="btn" ID="SendMsg" Text="发送站内信" runat="server" Visible="false" />
            <asp:Button class="btn" ID="Delete" Text="删 除" runat="server" />
            <asp:Button class="btn" ID="btnReturn" Text="返 回" OnClick="Return_OnClick" runat="server" />
        </ul>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
