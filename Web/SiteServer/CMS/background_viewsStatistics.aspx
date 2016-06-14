<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundViewsStatistics" %>

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
                    <td>会员名：
          <asp:TextBox ID="UserName"
              MaxLength="20"
              Size="37"
              runat="server" />
                        统计时间：从
          <bairong:DateTimeTextBox ID="DateFrom" class="input-small" Columns="12" runat="server" />
                        &nbsp;到&nbsp;
          <bairong:DateTimeTextBox ID="DateTo" class="input-small" Columns="12" runat="server" />

                        <asp:Button class="btn" OnClick="Search_OnClick" ID="Search" Text="搜 索" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <table class="table table-bordered table-hover">
            <tr class="info thead">
                <td class="center" style="width: 180px;">会员</td>
                <td class="center" style="width: 80px;">栏目</td>
                <!--<td class="center" style="width: 80px;">年-月</td>-->
                <td class="center" style="width: 80px;">浏览量</td>  
            </tr>
            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Literal ID="ItemUser" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ItemNode" runat="server"></asp:Literal>
                        </td>
                       <!-- <td>
                            <asp:Literal ID="ItemYearMonth" runat="server"></asp:Literal>
                        </td> -->
                        <td class="center">
                            <asp:Literal ID="ItemStasCount" runat="server"></asp:Literal>
                        </td>  
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>

        <bairong:SqlPager ID="spContents" runat="server" class="table table-pager" />

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
