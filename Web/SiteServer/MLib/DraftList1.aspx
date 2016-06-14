<%@ Page Language="C#" AutoEventWireup="true" Inherits="SiteServer.CMS.BackgroundPages.MLib.DraftList1" %>


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
        <ul class="nav nav-pills">
            <li id="tab1" class="" <%=IsShowTab %>><a href="DraftList.aspx?PublishmentSystemID=<%=Request.QueryString["PublishmentSystemID"] %>">待审核草稿(<asp:Literal runat="server" ID="ltlContentCount1"></asp:Literal>)</a></li>
            <li id="tab2" class="active"><a href="javascript:;">所有草稿(<asp:Literal runat="server" ID="ltlContentCount2"></asp:Literal>)</a></li>
        </ul>
        <bairong:Alerts runat="server" />

        <script type="text/javascript">
            $(document).ready(function () {
                loopRows(document.getElementById('contents'), function (cur) { cur.onclick = chkSelect; });
                $(".popover-hover").popover({ trigger: 'hover', html: true });
            });
        </script>

        <div class="well well-small">
            <div id="contentSearch" style="margin-top: 10px;">
                时间从：
      <bairong:DateTimeTextBox ID="DateFrom" class="input-small" Columns="12" runat="server" />
                目标：
      <asp:DropDownList ID="SearchType" class="input-medium" runat="server"></asp:DropDownList>
                关键字：
      <asp:TextBox class="input-medium" ID="Keyword" runat="server" />
                审核状态：
      <asp:DropDownList ID="ddlStatus" class="input-medium" runat="server">
      </asp:DropDownList>
                <asp:Button class="btn" OnClick="Search_OnClick" ID="Search" Text="搜 索" runat="server" />
            </div>
        </div>

        <table id="contents" class="table table-bordered table-hover">
            <tr class="info thead">
                <td width="30">序号</td>
                <td>内容标题(点击查看) </td>
                <asp:Literal ID="ltlColumnHeadRows" runat="server"></asp:Literal>
                <td width="50">状态 </td>
                <td width="50">&nbsp;</td>
                <asp:Literal ID="ltlCommandHeadRows" runat="server"></asp:Literal>
                <td width="20">
                    <input type="checkbox" onclick="selectRows(document.getElementById('contents'), this.checked);">
                </td>
            </tr>
            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <tr>
                        <td class="center">
                            <asp:Literal ID="ltlID" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ltlItemTitle" runat="server"></asp:Literal>
                            <asp:Literal ID="ltlGatherIamgeUrl" runat="server"></asp:Literal>
                        </td>
                        <asp:Literal ID="ltlColumnItemRows" runat="server"></asp:Literal>
                        <td class="center" nowrap>
                            <asp:Literal ID="ltlItemStatus" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlItemEditUrl" runat="server"></asp:Literal>
                        </td>
                        <asp:Literal ID="ltlCommandItemRows" runat="server"></asp:Literal>
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
