<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundAnalysisFilesEvaluation" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
<!DOCTYPE html>
<html>
<head>
    <met a charset="utf-8">
    <!--#include file="../inc/header.aspx"-->
</head>

<body>
    <!--#include file="../inc/openWindow.html"-->
    <form class="form-inline" runat="server">
        <asp:Literal ID="ltlBreadCrumb" runat="server" />
        <bairong:Alerts ID="alertsID" runat="server" />

        <div class="well well-small">
            <table class="table table-noborder">
                <tr>
                    <td width="500">开始时间：
    <bairong:DateTimeTextBox ID="StartDate" class="input-small" Columns="30" runat="server" />
                        结束时间：
    <bairong:DateTimeTextBox ID="EndDate" class="input-small" Columns="30" runat="server" />
                        <asp:Button class="btn" ID="Analysis" Style="margin-bottom: 0px;" Text="分 析" runat="server" /></td>

                    <td>栏目：<asp:Literal ID="ltlTarget" runat="server" />（综合平均分：<asp:Literal ID="ltlAvgCompositeScore" runat="server" />）
                    </td>
                </tr>
            </table>
        </div>
        <table id="contents" class="table table-bordered table-hover">
            <tr class="info thead">
                <td width="180" rowspan="2">内容页面</td>
                <asp:Literal ID="ltlColumnHeadRows" runat="server"></asp:Literal>
            </tr>
            <asp:Literal ID="ltlColumnHeadTitleRows" runat="server"></asp:Literal>
            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Literal ID="ltlContentTitle" runat="server"></asp:Literal>
                        </td>
                        <asp:Literal ID="ltlColumnItemRows" runat="server"></asp:Literal>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <bairong:SqlPager ID="spContents" runat="server" class="table table-pager" />

        <ul class="breadcrumb breadcrumb-button">
            <asp:Button class="btn" ID="btnReturn" CausesValidation="false" OnClick="Return_OnClick" Text="返 回" runat="server" />
        </ul>
    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
