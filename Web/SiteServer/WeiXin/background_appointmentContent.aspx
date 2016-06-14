<%@ Page Language="C#" Inherits="SiteServer.WeiXin.BackgroundPages.BackgroundAppointmentContent" %>

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
                <td>预约名称</td>
                <td>姓名</td>
                <td>电话</td>
                <td>邮箱</td>
                <td>预约时间</td>
                <td>预约状态</td>
                <td>留言</td>
                <%--<asp:Literal ID="ltlExtendTitle" runat="server"></asp:Literal>--%>
                <td></td>
                <td></td>
                <td width="20">
                    <input type="checkbox" onclick="selectRows(document.getElementById('contents'), this.checked);" /></td>
            </tr>
            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <tr>
                        <td class="center">
                            <asp:Literal ID="ltlItemIndex" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlAppointementTitle" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlRealName" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlMobile" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlEmail" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlAddDate" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlStatus" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ltlMessage" runat="server"></asp:Literal>
                        </td>
                        <%-- <asp:Literal ID="ltlExtendVal" runat="server"></asp:Literal>--%>
                        <td class="center">
                            <asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlSelectUrl" runat="server"></asp:Literal>
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
            <asp:Button class="btn btn-success" ID="btnHandle" Text="预约处理" runat="server" />
            <asp:Button class="btn" ID="btnDelete" Text="删 除" runat="server" />
            <asp:Button class="btn" ID="btnExport" Text="导出CSV" runat="server" />
            <asp:Button class="btn" ID="btnReturn" Text="返 回" runat="server" />
        </ul>

    </form>
</body>
</html>
