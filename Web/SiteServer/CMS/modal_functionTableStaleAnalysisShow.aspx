 <%@ Page Language="C#" ValidateRequest="false" Inherits="SiteServer.CMS.BackgroundPages.Modal.FunctionStyleAnalysisShow" Trace="false" %>

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
        <table class="table table-bordered table-striped"> 
            <tr>
                <asp:Repeater ID="MyRepeater" runat="server">
                    <ItemTemplate>
                        <asp:Literal ID="ltlHtml" runat="server" />
                    </ItemTemplate>
                </asp:Repeater>
            </tr>
        </table>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
