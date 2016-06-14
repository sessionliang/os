<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.ConsoleSiteDownloadAnalysisAll" EnableViewState="false" %>

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
        <asp:Literal ID="ltlBreadCrumb" runat="server" />
        <bairong:Alerts runat="server" />

        <div class="well well-small">
            <table class="table table-noborder">
                <tr>
                    <td>
                        <%if (!string.IsNullOrEmpty(this.returnUrl))
                          {%>
                        <input type="button" onclick="window.location.href = '<%=this.returnUrl%>    ';" value="返 回" class="btn" />
                        <%} %>
                    </td>
                </tr>
            </table>
        </div>

        <div class="popover popover-static">
            <h3 class="popover-title">应用文件下载量统计</h3>
            <div class="popover-content">

                <table class="table table-bordered table-hover">
                    <tr class="info thead">
                        <td>应用名称</td>
                        <td>文件下载量</td>
                    </tr>

                    <asp:Repeater runat="server" ID="rpContents">
                        <ItemTemplate>
                            <tr>
                                <td style="text-align: left">
                                    <asp:Literal ID="ltlPublishmentSystemName" runat="server"></asp:Literal></td>
                                <td style="text-align: center">
                                    <asp:Literal ID="ltlDownloadNum" runat="server"></asp:Literal></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>

                <bairong:SqlPager ID="spContents" runat="server" class="table table-pager" />
            </div>
        </div>
    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
