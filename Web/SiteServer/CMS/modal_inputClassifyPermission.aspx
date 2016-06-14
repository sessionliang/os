<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.Modal.InputClassifyPermission" Trace="false" %>

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
        <asp:Button ID="btnSubmit" UseSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" Style="display: none" />
        <bairong:Alerts runat="server"></bairong:Alerts>

        <table class="table table-noborder table-hover">
            <tr>

                <td width="150">
                    <bairong:Help HelpText="选择角色" Text="选择角色：" runat="server"></bairong:Help>
                    <asp:DropDownList ID="ddlRole" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged"></asp:DropDownList></td>
                <td rowspan="2">
                    <bairong:Help HelpText="选择分类" Text="选择分类：" runat="server"></bairong:Help>
                    <asp:ListBox ID="ClassifyCollection" SelectionMode="Multiple" Rows="18" Style="width: auto;" runat="server"></asp:ListBox></td>
            </tr>
            <tr height="40px;">
                <td width="150">
                    <bairong:Help HelpText="选择权限" Text="选择权限：" runat="server"></bairong:Help>
                    <asp:DropDownList ID="ddlPermission" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPermission_SelectedIndexChanged"></asp:DropDownList>
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
