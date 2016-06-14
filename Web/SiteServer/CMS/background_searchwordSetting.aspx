<%@ Page Language="C#" ValidateRequest="false" Inherits="SiteServer.CMS.BackgroundPages.BackgroundSearchwordSetting" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <!--#include file="../inc/header.aspx"-->
    <style>
        .td table tr td {
            border: 0;
        }
    </style>
</head>

<body>
    <!--#include file="../inc/openWindow.html"-->
    <form class="form-inline" runat="server">
        <asp:Literal ID="ltlBreadCrumb" runat="server" />
        <bairong:Alerts Text="只有同时满足一下条件的关键词才能出现在前台搜索框的联想关键词。" runat="server" />

        <div class="popover popover-static">
            <h3 class="popover-title">其他设置</h3>
            <div class="popover-content">
                <table class="table table-bordered table-hover">
                    <tr>
                        <td width="200">搜索结果不少于：</td>
                        <td>
                            <asp:TextBox ID="tbSearchResultCountLimit" runat="server"></asp:TextBox>
                            <span class="gray">0代表不限制</span>
                        </td>
                    </tr>
                    <tr>
                        <td>搜索次数不少于：</td>
                        <td>
                            <asp:TextBox ID="tbSearchCountLimit" runat="server"></asp:TextBox>
                            <span class="gray">0代表不限制</span>
                        </td>
                    </tr>
                    <tr>
                        <td>每次联想关键字不多于：</td>
                        <td>
                            <asp:TextBox ID="tbSearchOutputLimit" runat="server"></asp:TextBox>
                            <span class="gray">0代表不限制</span>
                        </td>
                    </tr>
                    <tr>
                        <td>前台联想词排序规则：</td>
                        <td class="td">
                            <asp:RadioButtonList ID="rblSearchSort" runat="server" RepeatDirection="Horizontal"></asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <hr />
        <table class="table noborder">
            <tr>
                <td class="center">
                    <asp:Button class="btn btn-primary" ID="Submit" Text="修 改" OnClick="Submit_OnClick" runat="server" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
