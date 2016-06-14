<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundMLibManageScope" %>

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
        <bairong:Alerts Text="本页面设置对应站点的具体栏目，确认后需投稿设置页面再次点击确认才能最终保存" runat="server" />

        <script type="text/javascript" language="javascript">
            function ChannelPermissions_CheckAll(chk) {
                var oEvent = document.getElementById('ChannelSelectControl');
                var chks = oEvent.getElementsByTagName("INPUT");
                for (var i = 0; i < chks.length; i++) {
                    if (chks[i].type == "checkbox") chks[i].checked = chk.checked;
                }
            }
        </script>

        <asp:PlaceHolder ID="ChannelPermissionsPlaceHolder" runat="server">
            <div class="popover popover-static">
                <h3 class="popover-title">选择当前站点可投递的栏目  -- 当前站点：<asp:Literal ID="llPublishmentSystem" runat="server"></asp:Literal></h3>
                <div class="popover-content">

                    <label class="checkbox">
                        <input type="checkbox" onclick="ChannelPermissions_CheckAll(this)" id="CheckAll1" name="CheckAll1">
                        全选</label>
                    <div class="tips">注：从下边选择允许投稿的栏目，只有选中的栏目才属于稿件可投递的范围。</div>

                    <asp:Literal ID="NodeTree" runat="server"></asp:Literal>

                </div>
            </div>
        </asp:PlaceHolder>

        <hr />
        <table class="table noborder">
            <tr>
                <td class="center">
                    <asp:Button class="btn btn-primary" ID="Submit" Text="确 定" OnClick="Submit_OnClick" runat="server" />
                    <asp:Button class="btn" ID="Return" Text="返 回" OnClick="Return_OnClick" runat="server" />
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
