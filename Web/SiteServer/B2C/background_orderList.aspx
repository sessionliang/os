<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.BackgroundOrderList" %>

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

        <div class="well well-small">
            <asp:HyperLink ID="hlSetting" NavigateUrl="javascript:;" runat="server" Text="设置属性"></asp:HyperLink>
        </div>

        <div class="well well-small">
            <table class="table table-noborder">
                <tr>
                    <td>订单状态：
          <asp:DropDownList ID="ddlOrderStatus" class="input-small" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server"></asp:DropDownList>
                        支付状态：
          <asp:DropDownList ID="ddlPaymentStatus" class="input-small" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server"></asp:DropDownList>
                        发货状态：
          <asp:DropDownList ID="ddlShipmentStatus" class="input-small" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>订单号：
          <asp:TextBox ID="tbOrderSN" class="input-small" runat="server"></asp:TextBox>
                        收货人：
          <asp:TextBox ID="tbConsignee" class="input-small" runat="server"></asp:TextBox>
                        关键字：
          <asp:TextBox ID="tbKeyword" class="input-small" runat="server"></asp:TextBox>
                        <asp:Button OnClick="Search_OnClick" Text="搜 索" class="btn" Style="margin-bottom: 0px" runat="server"></asp:Button>
                    </td>
                </tr>
            </table>
        </div>

        <table id="contents" class="table table-bordered table-hover">
            <tr class="info thead">
                <td width="20"></td>
                <td>订单号</td>
                <td>订单金额</td>
                <td>会员用户名</td>
                <td>订单状态</td>
                <td>支付状态</td>
                <td>发货状态</td>
                <td width="60">下单时间</td>
                <td width="60">支付时间</td>
                <td width="60">操作</td>
                <td width="20">
                    <input type="checkbox" onclick="selectRows(document.getElementById('contents'), this.checked);" /></td>
            </tr>
            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <asp:Literal ID="ltlTr" runat="server"></asp:Literal>
                    <td class="center">
                        <asp:Literal ID="ltlItemIndex" runat="server"></asp:Literal>
                    </td>
                    <td class="center">
                        <asp:Literal ID="ltlOrderSN" runat="server"></asp:Literal>
                    </td>
                    <td class="center">
                        <asp:Literal ID="ltlPriceActual" runat="server"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal ID="ltlUserName" runat="server"></asp:Literal>
                    </td>
                    <td class="center">
                        <asp:Literal ID="ltlOrderStatus" runat="server"></asp:Literal>
                    </td>
                    <td class="center">
                        <asp:Literal ID="ltlPaymentStatus" runat="server"></asp:Literal>
                    </td>
                    <td class="center">
                        <asp:Literal ID="ltlShipmentStatus" runat="server"></asp:Literal>
                    </td>
                    <td class="center">
                        <asp:Literal ID="ltlTimeOrder" runat="server"></asp:Literal>
                    </td>
                    <td class="center">
                        <asp:Literal ID="ltlTimePayment" runat="server"></asp:Literal>
                    </td>
                    <td class="center">
                        <asp:Literal ID="ltlBtnOperate" runat="server"></asp:Literal>
                    </td>
                    <td class="center">
                        <input type="checkbox" name="IDCollection" value='<%#DataBinder.Eval(Container.DataItem, "ID")%>' />
                    </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>

        <bairong:SqlPager ID="spContents" runat="server" class="table table-pager" />

    </form>
</body>
</html>
