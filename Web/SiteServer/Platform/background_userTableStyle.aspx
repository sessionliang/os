<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.BackgroundUserTableStyle" %>

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
        <asp:HiddenField ID="hidCurrentTab" runat="server" Value="1" />
        <ul class="nav nav-pills">
            <li id="tab1" class="active"><a href="javascript:;" onclick="_toggleTab(1,2);setCurrentTab(1);">系统默认字段</a></li>
            <li id="tab2"><a href="javascript:;" onclick="_toggleTab(2,2);setCurrentTab(2);">自定义字段</a></li>
        </ul>
        <script type="text/javascript">
            function setCurrentTab(num) {
                location.href = '<%=GetRedirectUrl(string.Empty)%>' + num;
            }
            $(function () {
                var current = $("#<%=hidCurrentTab.ClientID%>").val() || 1;
                _toggleTab(current, 2);
            });
        </script>
        <div id="column1" class="popover popover-static">
            <h3 class="popover-title">系统默认字段</h3>
            <div class="popover-content">
                <table class="table table-bordered table-hover">
                    <tr class="info thead">
                        <td>字段名</td>
                        <td>显示名称</td>
                        <td>表单提交类型</td>
                        <td>是否显示</td>
                        <td>需要验证</td>
                        <td style="display: none;">上升</td>
                        <td style="display: none;">下降</td>
                        <td style="display: none;">显示样式</td>
                        <td style="display: none;">表单验证</td>
                    </tr>
                    <asp:Repeater runat="server" ID="rpTableStyleHidden">
                        <ItemTemplate>
                            <tr>
                                <td width="140" class="center">
                                    <asp:Literal ID="AttributeName" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="DisplayName" runat="server"></asp:Literal>
                                </td>
                                <td width="120" class="center">
                                    <asp:Literal ID="InputType" runat="server"></asp:Literal>
                                </td>
                                <td width="70" class="center">
                                    <asp:Literal ID="IsVisible" runat="server"></asp:Literal>
                                </td>
                                <td width="70" class="center">
                                    <asp:Literal ID="IsValidate" runat="server"></asp:Literal>
                                </td>
                                <td width="40" class="center" style="display: none;">
                                    <asp:HyperLink ID="UpLinkButton" runat="server"><img src="../Pic/icon/up.gif" border="0" alt="上升" /></asp:HyperLink>
                                </td>
                                <td width="40" class="center" style="display: none;">
                                    <asp:HyperLink ID="DownLinkButton" runat="server"><img src="../Pic/icon/down.gif" border="0" alt="下降" /></asp:HyperLink>
                                </td>
                                <td width="120" class="center" style="display: none;">
                                    <asp:Literal ID="EditStyle" runat="server"></asp:Literal>
                                </td>
                                <td width="80" class="center" style="border-bottom: 0px; display: none;">
                                    <asp:Literal ID="EditValidate" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
        <div id="column2" style="display: none" class="popover popover-static">
            <h3 class="popover-title">自定义字段</h3>
            <div class="popover-content">
                <table class="table table-bordered table-hover">
                    <tr class="info thead">
                        <td>字段名</td>
                        <td>显示名称</td>
                        <td>表单提交类型</td>
                        <td>是否显示</td>
                        <td>需要验证</td>
                        <td>上升</td>
                        <td>下降</td>
                        <td>显示样式</td>
                        <td>表单验证</td>
                    </tr>
                    <asp:Repeater runat="server" ID="rpTableStyleBasic">
                        <ItemTemplate>
                            <tr>
                                <td width="140" class="center">
                                    <asp:Literal ID="AttributeName" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="DisplayName" runat="server"></asp:Literal>
                                </td>
                                <td width="120" class="center">
                                    <asp:Literal ID="InputType" runat="server"></asp:Literal>
                                </td>
                                <td width="70" class="center">
                                    <asp:Literal ID="IsVisible" runat="server"></asp:Literal>
                                </td>
                                <td width="70" class="center">
                                    <asp:Literal ID="IsValidate" runat="server"></asp:Literal>
                                </td>
                                <td width="40" class="center">
                                    <asp:HyperLink ID="UpLinkButton" runat="server"><img src="../Pic/icon/up.gif" border="0" alt="上升" /></asp:HyperLink>
                                </td>
                                <td width="40" class="center">
                                    <asp:HyperLink ID="DownLinkButton" runat="server"><img src="../Pic/icon/down.gif" border="0" alt="下降" /></asp:HyperLink>
                                </td>
                                <td width="120" class="center">
                                    <asp:Literal ID="EditStyle" runat="server"></asp:Literal>
                                </td>
                                <td width="80" class="center">
                                    <asp:Literal ID="EditValidate" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                <ul class="breadcrumb breadcrumb-button">
                    <asp:Button class="btn btn-success" ID="AddStyle" Text="新增字段" runat="server" />
                </ul>
            </div>
        </div>
    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
