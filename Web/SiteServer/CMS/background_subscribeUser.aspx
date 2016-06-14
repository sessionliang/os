<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundSubscribeUser" %>

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

        <div class="well well-small">
            <table class="table table-noborder">
                <tr>
                    <td>
                        <asp:PlaceHolder ID="PhState" runat="server">订阅状态：
          <asp:DropDownList ID="State" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" class="input-small" runat="server" /></asp:PlaceHolder>
                        每页显示条数：
          <asp:DropDownList ID="PageNum" class="input-small" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server">
              <asp:ListItem Text="默认" Value="0" Selected="true"></asp:ListItem>
              <asp:ListItem Text="10" Value="10"></asp:ListItem>
              <asp:ListItem Text="16" Value="16"></asp:ListItem>
              <asp:ListItem Text="20" Value="20"></asp:ListItem>
              <asp:ListItem Text="30" Value="30"></asp:ListItem>
              <asp:ListItem Text="50" Value="50"></asp:ListItem>
              <asp:ListItem Text="100" Value="100"></asp:ListItem>
              <asp:ListItem Text="200" Value="200"></asp:ListItem>
              <asp:ListItem Text="300" Value="300"></asp:ListItem>
          </asp:DropDownList>
                        排序：
          <asp:DropDownList ID="ddlOrder" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" class="input-small" runat="server">
              <asp:ListItem Text="默认" Value="0" Selected="true"></asp:ListItem>
              <asp:ListItem Text="订阅时间" Value="AddDate"></asp:ListItem>
              <asp:ListItem Text="推送次数" Value="PushNum"></asp:ListItem>
          </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>订阅时间：从
          <bairong:DateTimeTextBox ID="DateFrom" class="input-small" Columns="12" runat="server" />
                        &nbsp;到&nbsp;
          <bairong:DateTimeTextBox ID="DateTo" class="input-small" Columns="12" runat="server" />
                        手机号：
          <asp:TextBox ID="Mobile"
              MaxLength="20"
              Size="37"
              runat="server" />
                        邮箱：
          <asp:TextBox ID="Email"
              MaxLength="200"
              Size="37"
              runat="server" />
                        <asp:Button class="btn" OnClick="Search_OnClick" ID="Search" Text="搜 索" runat="server" />
                    </td>
                </tr>
            </table>
        </div>

        <table id="contents" class="table table-bordered table-hover">
            <tr class="info thead">
                <td width="120">邮箱 </td>
                <td width="80">姓名 </td>
                <td width="180">订阅内容</td>
                <td width="60">手机号 </td>
                <td width="60">订阅时间 </td>
                <td width="60">推送次数 </td>
                <td width="60">订阅状态 </td>
                <td width="30">&nbsp;</td>
                <td width="30">&nbsp;</td>
                <td width="20">
                    <input type="checkbox" onclick="selectRows(document.getElementById('contents'), this.checked);">
                </td>
            </tr>
            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Literal ID="ltlItemEmail" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ltlItemUser" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ltlItemSubscribeName" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ltlItemMobile" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlItemAddDate" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlItemPushNum" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlItemStatus" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlItemEditUrl" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlItemStatusUrl" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlSelect" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>

        <bairong:SqlPager ID="spContents" runat="server" class="table table-pager" />

        <ul class="breadcrumb breadcrumb-button">
            <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                <asp:Button class="btn btn-success" ID="AddSubscribeUser" Text="新 增" runat="server" />
                <asp:Button class="btn" ID="CancelButton" Text="取消订阅" runat="server" />
                <asp:PlaceHolder ID="PhState2" runat="server">
                    <asp:Button class="btn" ID="SubscribeButton" Text="订 阅" runat="server" />
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="CheckPlaceHolder" runat="server">
                    <asp:Button class="btn" ID="ManualPush" Text="手动推送" runat="server" />
                </asp:PlaceHolder>
                <asp:Button class="btn" ID="Delete" Text="删 除" runat="server" />
            </asp:PlaceHolder>
        </ul>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
