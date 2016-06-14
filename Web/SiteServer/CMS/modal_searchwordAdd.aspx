<%@ Page Language="C#" ValidateRequest="false" Inherits="SiteServer.CMS.BackgroundPages.Modal.SearchwordAdd" Trace="false" %>

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
    <script type="text/javascript" charset="utf-8" src="../../sitefiles/bairong/scripts/independent/validate.js"></script>
    <form class="form-inline" enctype="multipart/form-data" method="post" runat="server">
        <asp:Button ID="btnSubmit" UseSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" Style="display: none" />
        <bairong:Alerts runat="server"></bairong:Alerts>

        <table class="table table-noborder table-hover">
            <tr>
                <td style="width: 150px;">搜索关键词：</td>
                <td>
                    <asp:TextBox runat="server" ID="tbSearchword"></asp:TextBox>
                    <asp:RequiredFieldValidator
                        ControlToValidate="tbSearchword"
                        ErrorMessage=" *" ForeColor="red"
                        Display="Dynamic"
                        runat="server" />
                </td>
            </tr>
            <tr>
                <td style="width: 150px;">被搜索次数：</td>
                <td>
                    <asp:TextBox runat="server" ID="tbSearchCount"></asp:TextBox>
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
