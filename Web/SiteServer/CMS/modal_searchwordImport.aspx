<%@ Page Language="C#" Trace="false" Inherits="SiteServer.CMS.BackgroundPages.Modal.SearchwordImport" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <!--#include file="../inc/header.aspx"-->
</head>

<body>
    <!--#include file="../inc/openWindow.html"-->
    <form class="form-inline" enctype="multipart/form-data" method="post" runat="server">
        <asp:Button ID="btnSubmit" UseSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" Style="display: none" />
        <bairong:Alerts runat="server"></bairong:Alerts>

        <table class="table table-noborder table-hover">
            <tr>
                <td>
                    <bairong:Help HelpText="选择需要导入的Excel文件" Text="Excel文件：" runat="server"></bairong:Help>
                </td>
                <td>
                    <input type="file" id="myFile" size="35" runat="server" />
                    <asp:RequiredFieldValidator ControlToValidate="myFile" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" /></td>
            </tr>
            <tr>
                <td>
                    <bairong:Help HelpText="遇到同名关键词是否覆盖" Text="是否覆盖同名关键词：" runat="server"></bairong:Help>
                </td>
                <td>
                    <asp:RadioButtonList ID="IsOverride" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="覆盖" Value="True" Selected="true"></asp:ListItem>
                        <asp:ListItem Text="不覆盖" Value="False"></asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td>
                    <bairong:Help HelpText="是否保留关键词搜索结果次数" Text="是否保留关键词搜索结果次数：" runat="server"></bairong:Help>
                </td>
                <td>
                    <asp:RadioButtonList ID="IsSearchResultCount" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="是" Value="True"></asp:ListItem>
                        <asp:ListItem Text="否" Value="False" Selected="true"></asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td>
                    <bairong:Help HelpText="是否保留关键词搜索次数" Text="是否保留关键词搜索次数：" runat="server"></bairong:Help>
                </td>
                <td>
                    <asp:RadioButtonList ID="IsSearchCount" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="是" Value="True" Selected="true"></asp:ListItem>
                        <asp:ListItem Text="否" Value="False"></asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td>
                    <bairong:Help HelpText="设置从第几条开始导入" Text="从第几条开始导入：" runat="server"></bairong:Help>
                </td>
                <td>
                    <asp:TextBox class="input-mini" ID="ImportStart" runat="server" />
                    <span class="gray">默认为第一条</span>
                </td>
            </tr>
            <tr>
                <td>
                    <bairong:Help HelpText="设置共导入几条" Text="共导入几条：" runat="server"></bairong:Help>
                </td>
                <td>
                    <asp:TextBox class="input-mini" ID="ImportCount" runat="server" />
                    <span class="gray">默认为全部导入</span>
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
