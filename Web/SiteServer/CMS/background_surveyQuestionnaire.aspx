﻿<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundSurveyQuestionnaire" %>

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

        <script type="text/javascript">
            $(document).ready(function () {
                loopRows(document.getElementById('contents'), function (cur) { cur.onclick = chkSelect; });
            });
        </script>

        <div class="well well-small">
            <table class="table table-noborder">
                <tr>
                    <td>调查问卷页面：<asp:Literal ID="ltlTarget" runat="server" />
                    </td>
                </tr>
            </table>
        </div>

        <table id="contents" class="table table-bordered table-hover">
            <tr class="info thead">
                <td width="30">&nbsp;</td>
                <td width="180">调查人</td>
                <td>反馈描述</td>
                <td width="70">综合评分</td>
                <td width="70">IP地址</td>
                <td width="110">评论时间</td>
                <td width="30">&nbsp;</td>
                <td width="20">
                    <input type="checkbox" onclick="selectRows(document.getElementById('contents'), this.checked);">
                </td>
            </tr>
            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <tr>
                        <td class="center">
                            <asp:Literal ID="ltlIndex" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ltlUserName" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ltlContent" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlCompositeScore" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlIPAddress" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlAddDate" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <input type="checkbox" name="ContentIDCollection" value='<%#DataBinder.Eval(Container.DataItem, "SQID")%>' />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>

        <bairong:SqlPager ID="spContents" runat="server" class="table table-pager" />

        <ul class="breadcrumb breadcrumb-button">
            <asp:Button class="btn btn-success" ID="btnAdd" Text="添加评论" runat="server" Visible="false" />
            <asp:PlaceHolder ID="phCheck" runat="server" Visible="false">
                <asp:Button class="btn" ID="Check" Text="审核通过" runat="server" />
            </asp:PlaceHolder>
            <asp:Button class="btn btn-success" ID="btnRecommend" Text="设置为精彩评论" runat="server" Visible="false" />
            <asp:Button class="btn" ID="btnRecommendFalse" Text="设置为非精彩评论" runat="server" Visible="false" />
            <asp:PlaceHolder ID="phDelete" runat="server" Visible="false">
                <asp:Button class="btn" ID="btnDelete" Text="删 除" runat="server" />
            </asp:PlaceHolder>
            <asp:Button class="btn" ID="btnExport" runat="server" Text="导出Excel"></asp:Button>
            <asp:Button class="btn" ID="btnAnalysis" runat="server" Text="字段统计"></asp:Button>
            <asp:Button class="btn" ID="btnReturn" CausesValidation="false" OnClick="Return_OnClick" Text="返 回" runat="server" />
        </ul>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
