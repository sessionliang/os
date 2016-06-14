<%@ Page Language="C#" Trace="false" Inherits="SiteServer.CMS.BackgroundPages.Modal.SearchwordExport" %>

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
        <asp:Button ID="btnSubmit" UseSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" Style="display: none" />
        <bairong:Alerts Text="在此导出搜索关键词数据至Excel中" runat="server"></bairong:Alerts>

        <asp:PlaceHolder ID="phExport" runat="server">
            <table class="table table-noborder table-hover">
                <tr>
                    <td class="center" valign="top">
                        <table class="center" width="95%">
                            <tr>
                                <td>搜索结果数大于：</td>
                                <td>
                                    <asp:TextBox ID="tbSearchResultCount" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>搜索次数大于：</td>
                                <td>
                                    <asp:TextBox ID="tbSearchCount" runat="server"></asp:TextBox></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:PlaceHolder>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
