<%@ Page Language="C#" ValidateRequest="false" Inherits="SiteServer.STL.BackgroundPages.BackgroundWebsiteMessagePreview" %>

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
            <h3 class="popover-title">网站留言</h3>
            <div class="popover-content">

                <table class="table noborder table-hover">
                    <tr style="display: none;">
                        <td width="155">网站留言名称： </td>
                        <td>
                            <asp:Literal ID="ltlWebsiteMessageName" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td width="155">调用标签： </td>
                        <td>
                            <asp:Literal ID="ltlWebsiteMessageCode" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td width="155">修改样式： </td>
                        <td>&nbsp;&nbsp;
            <input type="button" class="btn" onclick="location.href='/siteserver/cms/background_websiteMessageSetting.aspx?PublishmentSystemID=<%=base.PublishmentSystemID%>&WebsiteMessageName=Default';" value="设 置" />
                            &nbsp;&nbsp;&nbsp;
            <input type="button" class="btn" onclick="location.href='background_websiteMessageTemplateSubmit.aspx?PublishmentSystemID=<%=base.PublishmentSystemID%>&WebsiteMessageName=Default';" value="修改模板" /></td>
                    </tr>
                </table>

            </div>
        </div>

    </form>

    <hr />

    <div style="margin: 0 10px 0 10px;">
        <div class="popover popover-static">
            <h3 class="popover-title">预览</h3>
            <div class="popover-content">
                <br>
                <asp:Literal ID="ltlForm" runat="server"></asp:Literal>
            </div>
        </div>
    </div>

</body>
</html>
<!-- check for 3.6 html permissions -->
