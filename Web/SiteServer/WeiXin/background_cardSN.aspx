  <%@ Page Language="C#" Inherits="SiteServer.WeiXin.BackgroundPages.BackgroundCardSN" %>

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
            <table class="table table-noborder">
                <tr>
                    <td> 
                        会员卡号：<asp:TextBox ID="tbCardSN" Width="120" runat="server" />&nbsp;&nbsp;
                        用户名：<asp:TextBox ID="tbUserName" Width="120" runat="server" />&nbsp;&nbsp;  
                        手机号：<asp:TextBox ID="tbMobile" Width="120" runat="server" />&nbsp;&nbsp;
                        <asp:Button class="btn" OnClick="Search_OnClick" ID="Search" Text="搜 索" runat="server" />
                    </td>
                </tr>
            </table>
            <div class="btn-toolbar" role="toolbar">
                <div class="btn-group">
                     <asp:Button id="wxCard" runat="server" CssClass="btn btn-default active"  Width="200"  OnClick="Search_OnClick" Text="微信会员"/>
                     <asp:Button id="entityCard" runat="server" CssClass="btn btn-default " Width="200" OnClick="Search_OnClick"  Text="实体卡会员"/> 
                     <input type ="hidden" runat="server" id="isEntity" value="false" />
                     </div>
            </div>
        </div>

        <table id="contents" class="table table-bordered table-hover">
            <tr class="info thead">
                <td width="20"></td>
                <td>卡号</td>
                <td>姓名</td>
                <td>手机</td>
                <td>金额</td>
                <td>积分</td>
                <td>领卡时间</td> 
                <td>状态</td>
                <td></td>
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
                        <td>
                            <asp:Literal ID="ltlSN" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ltlUserName" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlMobile" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlAmount" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlCredits" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlAddDate" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlIsDisabled" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlConsumeUrl" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlRechargeUrl" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlCreditesUrl" runat="server"></asp:Literal>
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

             <asp:Button class="btn btn-success" id="btnAdd" Text="添加" runat="server" />

            <asp:Button class="btn" ID="btnDelete" Text="删 除" runat="server" />

             <asp:Button class="btn" ID="btnStatus" Text="设置状态" runat="server" />

             <asp:Button class="btn" id="btnExport" Text="导出CSV" runat="server" />

             <asp:Button class="btn" ID="btnReturn" Text="返 回" runat="server" />
        </ul>

    </form>
</body>
</html>
