<%@ Page Language="C#" ValidateRequest="false" Inherits="SiteServer.STL.BackgroundPages.BackgroundSearchwordInputTemplateSubmit" %>

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

        <div class="popover popover-static">
            <h3 class="popover-title">站内搜索模板</h3>
            <div class="popover-content">

                <table class="table noborder table-hover">
                    <tr style="display:none;">
                        <td width="155">
                            <bairong:Help HelpText="网站留言名称" Text="网站留言名称：" runat="server"></bairong:Help>
                        </td>
                        <td>
                            <asp:Literal ID="ltlSearchwordInputName" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td width="155">
                            <bairong:Help HelpText="调用标签" Text="调用标签：" runat="server"></bairong:Help>
                        </td>
                        <td>
                            <asp:Literal ID="ltlElement" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td width="155">模板显示方式： </td>
                        <td>
                            <asp:RadioButtonList ID="rblIsTemplate" AutoPostBack="true" OnSelectedIndexChanged="rblIsTemplate_SelectedIndexChanged" RepeatDirection="Horizontal" class="noborder" runat="server"></asp:RadioButtonList></td>
                    </tr>
                    <asp:PlaceHolder ID="phTemplate" runat="server">
                        <tr>
                            <td width="155">
                                <bairong:Help HelpText="重置为默认模板" Text="重置为默认模板：" runat="server"></bairong:Help>
                            </td>
                            <td>
                                <asp:Button ID="IsCreateTemplate" AutoPostBack="true" OnClick="IsCreateTemplate_CheckedChanged" Text="重置为默认模板"  runat="server"></asp:Button>
                                (提示：可以点此重置为默认模板！) </td>
                        </tr>
                        <tr>
                            <td width="155">
                                <bairong:Help HelpText="模板内容" Text="模板内容：" runat="server"></bairong:Help>
                            </td>
                            <td>
                                <asp:TextBox Width="98%" TextMode="MultiLine" ID="Content" runat="server" Rows="20" Wrap="false" />
                                <asp:RequiredFieldValidator ControlToValidate="Content" ErrorMessage="模板内容必须填写，可以点击“生成默认模板”生成" Display="Dynamic" runat="server" /></td>
                        </tr>
                        <tr>
                            <td width="155">
                                <bairong:Help HelpText="CSS样式" Text="CSS样式：" runat="server"></bairong:Help>
                            </td>
                            <td>
                                <asp:TextBox Width="98%" TextMode="MultiLine" ID="Style" runat="server" Rows="10" Wrap="false" /></td>
                        </tr>
                        <tr>
                            <td width="155">
                                <bairong:Help HelpText="JS脚本" Text="JS脚本：" runat="server"></bairong:Help>
                            </td>
                            <td>
                                <asp:TextBox Width="98%" TextMode="MultiLine" ID="Script" runat="server" Rows="10" Wrap="false" /></td>
                        </tr>
                    </asp:PlaceHolder>
                </table>

                <hr />
                <table class="table noborder">
                    <tr>
                        <td class="center">
                            <asp:Button class="btn btn-primary" ID="Submit" Text="确 定" OnClick="Submit_OnClick" runat="server" />
                            <asp:Button class="btn btn-info" ID="Preview" Text="预 览" runat="server" />
                        </td>
                    </tr>
                </table>

            </div>
        </div>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
