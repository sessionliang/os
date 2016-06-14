<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundAnalysisEvaluation" EnableViewState="false" %>

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
            <asp:LinkButton class="btn" ID="Image" Style="margin-bottom: 0px;" Text="图标展示" runat="server" Visible="false" />
        </div>  
        <div class="popover popover-static">
            <h3 class="popover-title">按栏目统计</h3>
            <div class="popover-content">
                <table class="table table-bordered table-hover">
                    <tr class="info thead">
                        <td>栏目 </td> 
                        <td width="70">评价数量 </td>
                        <td width="100">综合平均得分 </td>
                    </tr>
                    <asp:Repeater ID="rptContents" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Literal ID="ltlNode" runat="server"></asp:Literal></td> 
                                <td> 
                                    <asp:Literal ID="ltlContentCount" runat="server"></asp:Literal></td>
                                <td>
                                    <asp:Literal ID="ltlContentEvaluation" runat="server"></asp:Literal></td>
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
