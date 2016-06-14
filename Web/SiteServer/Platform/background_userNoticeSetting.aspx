<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.BackgroundUserNoticeSetting" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <!--#include file="../inc/header.aspx"-->
    <script type="text/javascript">
        function IsRequiredNotice(noticeType) {
            $("#td_" + noticeType).css("color", "red");
        }
    </script>
</head>

<body>
    <!--#include file="../inc/openWindow.html"-->
    <form class="form-inline" runat="server">
        <asp:Literal ID="ltlBreadCrumb" runat="server" />
        <bairong:Alerts runat="server" />

        <div class="popover popover-static">
            <h3 class="popover-title">消息提醒设置</h3>
            <div class="popover-content">

                <table class="table noborder table-hover">
                    <tr class="info thead">
                        <td width="65%"></td>
                        <td>手机</td>
                        <td>邮箱</td>
                        <td>站内信</td>
                    </tr>
                    <tr style="border-bottom: 1px solid #ccc;">
                        <%= GetNoticeSetting(BaiRong.Model.EUserNoticeType.WelcomeAfterRegiste)%>
                    </tr>
                    <tr style="border-bottom: 1px solid #ccc;">
                        <%= GetNoticeSetting(BaiRong.Model.EUserNoticeType.ValidateForRegiste)%>
                    </tr>
                    <tr style="border-bottom: 1px solid #ccc;">
                        <%= GetNoticeSetting(BaiRong.Model.EUserNoticeType.FindPassword)%>
                    </tr>
                    <tr style="border-bottom: 1px solid #ccc;">
                        <%= GetNoticeSetting(BaiRong.Model.EUserNoticeType.FindPasswordAfter)%>
                    </tr>
                    <tr style="border-bottom: 1px solid #ccc;">
                        <%= GetNoticeSetting(BaiRong.Model.EUserNoticeType.BindEmail)%>
                    </tr>
                    <tr style="border-bottom: 1px solid #ccc;">
                        <%= GetNoticeSetting(BaiRong.Model.EUserNoticeType.BindPhone)%>
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
