<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.BackgroundConfiguration" %>

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
        <bairong:Alerts Text="在此设置用户中心访问方式及访问地址" runat="server"></bairong:Alerts>

        <div class="popover popover-static">
            <h3 class="popover-title">平台参数设置</h3>
            <div class="popover-content">

                <table class="table noborder table-hover">
                    <tr>
                        <td width="140">平台访问方式：</td>
                        <td>
                            <asp:RadioButtonList ID="rblIsRelatedUrl" AutoPostBack="true" OnSelectedIndexChanged="rblIsRelatedUrl_SelectedIndexChanged" RepeatDirection="Horizontal" runat="server"></asp:RadioButtonList>
                        </td>
                    </tr>
                    <asp:PlaceHolder ID="phRootUrl" runat="server">
                        <tr>
                            <td>平台访问域名：</td>
                            <td>
                                <asp:TextBox ID="tbRootUrl" Columns="40" MaxLength="200" Style="ime-mode: disabled;" runat="server" />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="tbRootUrl" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" />
                                <span class="gray">此设置将修改平台的访问地址，修改前请先确认此地址能够被访问</span>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <tr>
                        <td width="140">是否启用异步生成：</td>
                        <td>
                            <asp:RadioButtonList ID="IsUseAjaxCreatePage" RepeatDirection="Horizontal" class="radiobuttonlist" runat="server"></asp:RadioButtonList>
                            <span class="gray">注意，禁用之后可能会影响到系统的一些自动任务，比如：批量操作内容之后的自动生成。</span>
                        </td>
                    </tr>
                    <tr>
                        <td width="140">是否Xss过滤：</td>
                        <td>
                            <asp:RadioButtonList ID="IsFilterXss" RepeatDirection="Horizontal" class="radiobuttonlist" runat="server"></asp:RadioButtonList>
                            <span class="gray">注意，禁用会有网站被攻击风险，一般使用于内网使用。</span>
                        </td>
                    </tr>
                    <tr>
                        <td width="140">是否启用服务组件生成页面：</td>
                        <td>
                            <asp:RadioButtonList ID="isSiteServerServiceCreate" RepeatDirection="Horizontal" class="radiobuttonlist" runat="server" AutoPostBack="true" OnSelectedIndexChanged="isSiteServerServiceCreate_SelectedIndexChanged"></asp:RadioButtonList>
                            <span class="gray">建议在多个编辑人员的情况下启用。注意，启用之前请先安装SiteServer服务组件。</span>
                        </td>
                    </tr>
                    <tr>
                        <td width="140">管理员是否可以查看其他人添加的内容：</td>
                        <td>
                            <asp:RadioButtonList ID="rblIsViewContentOnlySelf" runat="server" class="radiobuttonlist" RepeatDirection="Horizontal"></asp:RadioButtonList>
                            <span class="gray">注意：超级管理员、站点管理员、具有审核权限的管理员，此设置无效。</span>
                        </td>
                    </tr>
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
