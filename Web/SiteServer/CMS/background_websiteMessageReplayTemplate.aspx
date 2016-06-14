<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundWebsiteMessageReplayTemplate" %>

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

        <script type="text/javascript">
            $(document).ready(function () {
                loopRows(document.getElementById('contents'), function (cur) { cur.onclick = chkSelect; });
                $(".popover-hover").popover({ trigger: 'hover', html: true });
            });
        </script>

        <table id="contents" class="table table-bordered table-hover">
            <tr class="info thead">
                <td class="center" style="width: 70%;">标题</td>
                <td class="center" style="width: 80px;">添加时间</td>
                <td class="center" style="width: 40px;">&nbsp;</td>
                <td width="20" class="center">
                    <input type="checkbox" onclick="selectRows(document.getElementById('contents'), this.checked);">
                </td>
            </tr>
            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <tr>
                        <td class="center">
                            <asp:Literal ID="ItemTitle" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ItemDateTime" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ItemEidtRow" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <input type="checkbox" name="ContentIDCollection" value='<%#DataBinder.Eval(Container.DataItem, "ID")%>' />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>

        <bairong:SqlPager ID="spContents" runat="server" class="table table-pager" />

        <div class="well well-small">

            <asp:Button class="btn btn-success" ID="btnAdd" Text="新 增" runat="server" />
            <asp:Button class="btn" ID="btnDelete" OnClick="btnDelete_OnClick" Text="删 除" runat="server" />

        </div>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
