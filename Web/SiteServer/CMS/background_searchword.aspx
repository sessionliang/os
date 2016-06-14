<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundSearchword" %>

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
                    <td>搜索结果数：从
          <asp:TextBox ID="tbSearchResultCountFrom" Width="7%" runat="server"></asp:TextBox>
                        到：
          <asp:TextBox ID="tbSearchResultCountTo" Width="7%"  runat="server"></asp:TextBox>
                        被搜索次数：从
          <asp:TextBox ID="tbSearchCountFrom" Width="7%"  runat="server"></asp:TextBox>
                        到：
          <asp:TextBox ID="tbSearchCountTo" Width="7%"  runat="server"></asp:TextBox>
                        关键词：
          <asp:TextBox ID="tbKeyword" Width="7%"  runat="server"></asp:TextBox>
                        <asp:Button class="btn" OnClick="Search_OnClick" ID="Search" Text="搜 索" runat="server" />
                    </td>
                </tr>
            </table>
        </div>

        <table id="contents" class="table table-bordered table-hover">
            <tr class="info thead">
                <td class="center" style="width: 70%;">搜索关键词</td>
                <td class="center" style="width: 80px;">搜索结果数</td>
                <td class="center" style="width: 80px;">被搜索次数</td>
                <td class="center" style="width: 40px;">&nbsp;</td>
                <td class="center" style="width: 40px;">&nbsp;</td>
                <td width="20" class="center">
                    <input type="checkbox" onclick="selectRows(document.getElementById('contents'), this.checked);">
                </td>
            </tr>
            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <tr>
                        <td class="center">
                            <asp:Literal ID="ltlSearchword" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlSearchResultCount" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlSearchCount" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlEdit" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlDelete" runat="server"></asp:Literal>
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

            <asp:Button class="btn btn-success" ID="btnAdd" Text="添加关键词" runat="server" />
            <asp:Button class="btn" ID="btnDelete" OnClick="btnDelete_OnClick" Text="删 除" runat="server" />
            <asp:Button class="btn" ID="btnUpdateBatch" OnClick="btnUpdateBatch_OnClick" Text="更新搜索结果" runat="server" />
            <asp:Button class="btn" ID="btnUpdateAll" OnClick="btnUpdateAll_OnClick" Text="更新所有搜索结果" runat="server" />
            <asp:Button class="btn" ID="btnImport" Text="导入关键词" runat="server" />
            <asp:Button class="btn" ID="btnExport" Text="导出关键词" runat="server" />
        </div>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
