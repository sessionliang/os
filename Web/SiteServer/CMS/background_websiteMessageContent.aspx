<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundWebsiteMessageContent" %>

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

            <asp:Button class="btn btn-success" ID="btnAdd" Text="添加留言" runat="server" />
            <asp:Button class="btn" ID="btnDelete" OnClick="btnDelete_OnClick" Text="删 除" runat="server" />
            <asp:Button class="btn" ID="btnExportExcel" runat="server" Text="导出"></asp:Button>
            <asp:Button class="btn" ID="btnTranslate" Text="转 移" runat="server" />
            <asp:Button class="btn" ID="btnTaxis" Text="排 序" runat="server" />
            <asp:Button class="btn" ID="btnCheck" OnClick="btnCheck_OnClick" Text="审 核" runat="server" />
            <asp:Button class="btn" ID="btnSelectColumns" Text="显示项" runat="server" />


            <div id="contentSearch" style="margin-top: 10px;">
                时间从：
      <bairong:DateTimeTextBox ID="DateFrom" class="input-small" Columns="12" runat="server" />
                到：
      <bairong:DateTimeTextBox ID="DateTo" class="input-small" Columns="12" runat="server" />
                目标：
            <asp:DropDownList ID="SearchType" class="input-medium" runat="server"></asp:DropDownList>
                关键字：
      <asp:TextBox class="input-medium" ID="Keyword" runat="server" />
                <asp:Button class="btn" ID="Search" OnClick="Search_OnClick" Text="搜 索" runat="server" />
            </div>
        </div>

        <table id="contents" class="table table-bordered table-hover">
            <tr class="info thead">
                <asp:Literal ID="ltlColumnHeadRows" runat="server"></asp:Literal>
                <td class="center" style="width: 80px;">添加时间</td>
                <td class="center" style="width: 40px;">&nbsp;</td>
                <td class="center" style="width: 40px;">&nbsp;</td>
                <asp:Literal ID="ltlHeadRowReply" runat="server"></asp:Literal>
                <td width="20" class="center">
                    <input type="checkbox" onclick="selectRows(document.getElementById('contents'), this.checked);">
                </td>
            </tr>
            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <tr>
                        <asp:Literal ID="ColumnItemRows" runat="server"></asp:Literal>
                        <td class="center">
                            <asp:Literal ID="ItemDateTime" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ItemEidtRow" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ItemViewRow" runat="server"></asp:Literal>
                        </td>
                        <asp:Literal ID="ItemRowReply" runat="server"></asp:Literal>
                        <td class="center">
                            <input type="checkbox" name="ContentIDCollection" value='<%#DataBinder.Eval(Container.DataItem, "ID")%>' />
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
