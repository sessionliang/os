<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundAnalysisAdministrator" EnableViewState="false" %>

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
        <bairong:Alerts runat="server" />
        <asp:Literal ID="ltlBreadCrumb" runat="server" />

        <div class="well well-small">
            开始时间：
    <bairong:DateTimeTextBox ID="StartDate" class="input-small" Columns="30" runat="server" />
            结束时间：
    <bairong:DateTimeTextBox ID="EndDate" class="input-small" Columns="30" runat="server" />
            <asp:Button class="btn" ID="Analysis" Style="margin-bottom: 0px;" OnClick="Analysis_OnClick" Text="分 析" runat="server" />
            <asp:LinkButton class="btn" ID="Image" Style="margin-bottom: 0px;" Text="图标展示" runat="server" />
        </div>

        <div class="popover popover-static">
            <h3 class="popover-title">按栏目统计</h3>
            <div class="popover-content">
                <table class="table table-bordered table-hover">
                    <tr class="info thead">
                        <td>栏目名 </td>
                        <td width="70">新增内容 </td>
                        <td width="70">修改内容 </td>
                        <td width="70">新增评论 </td>
                    </tr>
                    <asp:Repeater ID="rptChannels" runat="server">
                        <ItemTemplate>
                            <bairong:NoTagText ID="ElHtml" runat="server" />
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>

        <div class="popover popover-static">
            <h3 class="popover-title">按管理员统计</h3>
            <div class="popover-content">
                <table class="table table-bordered table-hover">
                    <tr class="info thead">
                        <td>登录名 </td>
                        <td>显示名 </td>
                        <td width="70">新增内容 </td>
                        <td width="70">更新内容 </td>
                        <td width="70">评论内容 </td>
                    </tr>
                    <asp:Repeater ID="rptContents" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Literal ID="ltlUserName" runat="server"></asp:Literal></td>
                                <td>
                                    <asp:Literal ID="ltlDisplayName" runat="server"></asp:Literal></td>
                                <td>
                                    <asp:Literal ID="ltlContentAdd" runat="server"></asp:Literal></td>
                                <td>
                                    <asp:Literal ID="ltlContentUpdate" runat="server"></asp:Literal></td>
                                <td>
                                    <asp:Literal ID="ltlContentComment" runat="server"></asp:Literal></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>

        <bairong:SqlPager ID="spContents" runat="server" class="table table-pager" />

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
