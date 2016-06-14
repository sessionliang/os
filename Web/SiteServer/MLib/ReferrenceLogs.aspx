<%@ Page Language="C#" AutoEventWireup="true" Inherits="SiteServer.CMS.BackgroundPages.MLib.ReferenceLogs" %>


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
            <asp:Literal ID="ltlContentButtons" runat="server"></asp:Literal>
            <div id="contentSearch" style="margin-top: 10px;">
                时间从：
      <bairong:DateTimeTextBox ID="DateFrom" class="input-small" Columns="12" runat="server" />
                到:
                
      <bairong:DateTimeTextBox ID="DateTo" class="input-small" Columns="12" runat="server" />
                <asp:Button class="btn" OnClick="Search_OnClick" ID="Search" Text="搜 索" runat="server" />
            </div>
        </div>

        <table id="contents" class="table table-bordered table-hover">
            <tr class="info thead">
                <td width="30">序号</td>
                <td>引用方</td>
                <td width="80">引用时间</td>
                <td width="80">操作管理员</td>
            </tr>
            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <tr>
                        <td class="center">
                            <asp:Literal ID="ltlID" runat="server"></asp:Literal>
                        </td>
                        <td class="center" nowrap>
                            <asp:Literal ID="ltlRType" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlOpreateDate" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlOpreator" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>

        <bairong:SqlPager ID="spContents" runat="server" class="table table-pager" />

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
