<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundAnalysisContentHits" %>

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
            <%if (!string.IsNullOrEmpty(this.returnUrl))
              {%>
            <input type="button" onclick="window.location.href = '<%=this.returnUrl%>    ';" value="返 回" class="btn" />
            <%} %>
        </div>


        <table class="table table-bordered table-hover">
            <tr class="info thead">
                <td>内容标题(点击查看)</td>
                <td>所属栏目</td>
                <td width="60" class="center">点击量</td>
                <td width="60" class="center">日点击量</td>
                <td width="60" class="center">周点击量</td>
                <td width="60" class="center">月点击量</td>
                <td width="120" class="center">最后点击时间</td>
            </tr>
            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Literal ID="ltlItemTitle" runat="server"></asp:Literal></td>
                        <td>
                            <asp:Literal ID="ltlChannel" runat="server"></asp:Literal></td>
                        <td class="center">
                            <asp:Literal ID="ltlHits" runat="server"></asp:Literal></td>
                        <td class="center">
                            <asp:Literal ID="ltlHitsByDay" runat="server"></asp:Literal></td>
                        <td class="center">
                            <asp:Literal ID="ltlHitsByWeek" runat="server"></asp:Literal></td>
                        <td class="center">
                            <asp:Literal ID="ltlHitsByMonth" runat="server"></asp:Literal></td>
                        <td class="center">
                            <asp:Literal ID="ltlLastHitsDate" runat="server"></asp:Literal></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>

        <bairong:SqlPager ID="spContents" runat="server" class="table table-pager" />

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
