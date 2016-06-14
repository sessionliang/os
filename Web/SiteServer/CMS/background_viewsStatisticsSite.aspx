<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundViewsStatisticsSite" %>

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
        <bairong:Alerts text="站点需要智能推送功能时，请选择启用智能推送，并选择时间段统计，设置成功后请重新生成站点需要统计栏目的内容页面，系统会自动添加统计标。当会员登录状态下访问内容页面时，系统就会进行内容所属栏目的统计。在需要智能推送栏目的页面使用 stl:ipushcontents 标签（参照 stl:contents 标签）。"  runat="server" /> 
        <div class="popover popover-static">
            <h3 class="popover-title">智能推送设置</h3>
            <div class="popover-content">

                <table class="table noborder table-hover">
                    <tr>
                        <td width="220">是否启用智能推送统计：</td>
                        <td>
                            <asp:RadioButtonList ID="IsIntelligentPushCount" RepeatDirection="Horizontal" class="radiobuttonlist" OnSelectedIndexChanged="IsIntelligentPushCount_SelectedIndexChanged"
                                AutoPostBack="true" runat="server"></asp:RadioButtonList>
                            <span class="gray">需要重新生成页面</span>
                        </td>
                    </tr>
                    <asp:PlaceHolder ID="phDate" runat="server">
                        <tr>
                            <td width="220">选择推送时间段统计：</td>
                            <td>
                                <asp:RadioButtonList ID="rblDate" RepeatDirection="Vertical" class="radiobuttonlist" runat="server"></asp:RadioButtonList>
                                <span class="gray">系统按选择的时间段，给会员推送时间段内访问量最大的栏目(时间区间如例：选择推送时间段为一月内，如果会员访问日期大于等于10号，取本月，如果日期小于10号取上月，以此类推其他选项)</span>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                </table>

                <hr />
                <table class="table noborder">
                    <tr>
                        <td class="center">
                            <asp:Button class="btn btn-primary" ID="Submit" Text="确 定" OnClick="Submit_OnClick" runat="server" />
                        </td>
                    </tr>
                </table>

            </div>
        </div>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
