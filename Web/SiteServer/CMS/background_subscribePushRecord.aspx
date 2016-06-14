<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundSubscribePushRecord" %>

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
        <div class="well well-small">
            <table class="table table-noborder">
                <tr>
                    <td>手机号：
          <asp:TextBox ID="Mobile"
              MaxLength="20"
              Size="37"
              runat="server" />
                        邮箱：
          <asp:TextBox ID="Email"
              MaxLength="200"
              Size="37"
              runat="server" /><asp:PlaceHolder ID="PhState" runat="server">推送状态：
          <asp:DropDownList ID="State" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" class="input-small" runat="server" /></asp:PlaceHolder>
                    </td>
                </tr>
                <tr>
                    <td>订阅时间：从
          <bairong:DateTimeTextBox ID="DateFrom" class="input-small" Columns="12" runat="server" />
                        &nbsp;到&nbsp;
          <bairong:DateTimeTextBox ID="DateTo" class="input-small" Columns="12" runat="server" />

                        <asp:Button class="btn" OnClick="Search_OnClick" ID="Search" Text="搜 索" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <table class="table table-bordered table-hover">
            <tr class="info thead">
                <td class="center" style="width: 180px;">邮箱</td>
                <td class="center" style="width: 80px;">手机</td>
                <td class="center" style="width: 80px;">订阅内容</td>
                <td class="center" style="width: 80px;">推送类型</td>
                <td class="center" style="width: 80px;">推送时间</td>
                <td class="center" style="width: 60px;">状态</td>
                <td class="center" style="width: 40px;">&nbsp;</td>
                <td width="20" class="center">
                    <input onclick="_checkFormAll(this.checked)" type="checkbox" />
                </td>
            </tr>
            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Literal ID="ItemEmail" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ItemMobile" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ItemSubscribeName" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ItemPushType" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ItemAddDate" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ItemPushStatu" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ItemEidtRow" runat="server"></asp:Literal>
                        </td>

                        <td class="center">
                            <input type="checkbox" name="IDsCollection" value='<%#DataBinder.Eval(Container.DataItem, "RecordID")%>' />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>

        <bairong:SqlPager ID="spContents" runat="server" class="table table-pager" />

        <ul class="breadcrumb breadcrumb-button">
            <asp:Button class="btn" ID="ManualPush" Text="再次推送" runat="server" />
        </ul>
    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
