<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.Modal.OrganizationTranslate" Trace="false" %>

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
                <td width="120">
                    <bairong:Help HelpText="对所选分支机构转移的分类" Text="分类：" runat="server"></bairong:Help>
                </td>
                <td>
                    <asp:DropDownList ID="ParentItemID" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="ParentItemID_SelectedIndexChanged" class="noborder" runat="server"></asp:DropDownList></td>

            </tr>
            <tr>
                <td width="120">
                    <bairong:Help HelpText="对所选分支机构转移的区域" Text="所在区域：" runat="server"></bairong:Help>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAreaID" AutoPostBack="true" runat="server"></asp:DropDownList>
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
