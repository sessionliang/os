<%@ Page Language="C#" Inherits="SiteServer.WeiXin.BackgroundPages.BackgroundCouponSN" %>

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
                <td width="20"></td>
                <td>SN码</td>
                <td>状态</td>
                <td>领取时间</td>
                <td>领取者</td>
                <td>领取者手机</td>
                <td>领取者邮箱</td>
                <td>使用时间</td>
                <td>使用者</td>
                <td width="20">
                    <input type="checkbox" onclick="selectRows(document.getElementById('contents'), this.checked);" /></td>
            </tr>
            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <tr>
                        <td class="center">
                            <asp:Literal ID="ltlItemIndex" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ltlSN" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlStatus" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlHoldDate" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlHoldRealName" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlHoldMobile" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlHoldEmail" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlCashDate" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlCashUserName" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <input type="checkbox" name="IDCollection" value='<%#DataBinder.Eval(Container.DataItem, "ID")%>' />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>

        <bairong:SqlPager ID="spContents" runat="server" class="table table-pager" />

        <ul class="breadcrumb breadcrumb-button">
            <asp:Button class="btn btn-success" ID="btnAdd" Text="添 加" runat="server" />
            <asp:Button class="btn" ID="btnSetting" Text="设置状态" runat="server" />
            <asp:Button class="btn" ID="btnUpFile" Text="上传SN码" runat="server" />
            <a class="btn" href="../../SiteFiles/Services/WeiXin/coupon/sncode-example.csv">下载示例SN码文件</a>
            <asp:Button class="btn" ID="btnReturn" Text="返 回" runat="server" />
        </ul>

    </form>
</body>
</html>
