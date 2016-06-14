<%@ Page Language="C#" Trace="false" EnableViewState="false" Inherits="SiteServer.CMS.BackgroundPages.BackgroundBasePage" %>


<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="utf-8">
    <!--#include file="../inc/header.aspx"-->

    <script>
        window.onload = function () {
            displayChildren($("tr td img").first()[0]);
        };
    </script>
</head>

<body>
    <!--#include file="../inc/openWindow.html"-->
    <form runat="server">
        <table class="table noborder table-condensed table-hover">
            <tr class="info thead">
                <td onclick="location.reload();">
                    <lan>订阅内容</lan>
                </td>
            </tr>
            <site:Tree ID="SubscribeTree" ClassifyType="Subscribe" runat="server"></site:Tree>
        </table>
    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
