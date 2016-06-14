<%@ Page Language="C#" AutoEventWireup="true" Inherits="SiteServer.B2C.BackgroundPages.BackgroundOrderReturnList2" %>

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

        <ul class="nav nav-pills" id="myTab">
            <li><a href="background_orderReturnList.aspx?publishmentSystemID=<%=this.PublishmentSystemID %>">未处理(<asp:Literal runat="server" ID="ltlWCLCount"></asp:Literal>)</a></li>
            <li><a href="background_orderReturnList1.aspx?publishmentSystemID=<%=this.PublishmentSystemID %>">处理中(<asp:Literal runat="server" ID="ltlCLZCount"></asp:Literal>)</a></li>
            <li class="active"><a href="javascript:;">已处理(<asp:Literal runat="server" ID="ltlYCLCount"></asp:Literal>)</a></li>
        </ul>

        <div class="well well-small">
            <table class="table table-noborder">
                <tr>
                    <td>服务类型：
          <asp:DropDownList ID="ddlReturnType" class="input-small" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server"></asp:DropDownList>
                        申请状态：
          <asp:DropDownList ID="ddlAuditStatus" class="input-small" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server"></asp:DropDownList>
                        收货状态：
          <asp:DropDownList ID="ddlReturnOrderStatus" class="input-small" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server"></asp:DropDownList>
                        退款状态：
          <asp:DropDownList ID="ddlReturnMoneyStatus" class="input-small" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>订单号：
          <asp:TextBox ID="tbReturnSN" class="input-small" runat="server"></asp:TextBox>
                        联系人：
          <asp:TextBox ID="tbContact" class="input-small" runat="server"></asp:TextBox>
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
                <td>返修/退货编号</td>
                <td>订单编号</td>
                <td>服务类型</td>
                <td>商品规格号</td>
                <td>商品名称</td>
                <td>商品价格</td>
                <td>商品数量</td>
                <td>退款金额</td>
                <td>申请用户</td>
                <td>联系人姓名</td>
                <td>手机号</td>
                <td>申请原因</td>
                <td>申请时间</td>
                <td>申请状态</td>
                <%--<td width="60">操作</td>--%>
                <td width="20">
                    <input type="checkbox" onclick="selectRows(document.getElementById('contents'), this.checked);" />
                </td>
            </tr>
            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <tr>
                        <td class="center">
                            <asp:Literal ID="ltlItemIndex" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlOIRID" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlOrderSN" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlType" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlGoodsSN" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlGoodsTitle" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlPriceSale" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlReturnCount" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlReturnMoney" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ltlApplyUser" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ltlContact" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ltlContactPhone" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ltlDescription" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlApplyDate" runat="server"></asp:Literal>
                        </td>

                        <td class="center">
                            <asp:Literal ID="ltlAuditStatus" runat="server"></asp:Literal>
                        </td>
                        <td class="center"  style="display:none;">
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
