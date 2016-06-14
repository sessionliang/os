<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundMLibManage" %>

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
                    <td>站点：
          <asp:DropDownList ID="ddlPublishmentSystem" AutoPostBack="true" OnSelectedIndexChanged="ddlPublishmentSystem_OnSelectedIndexChanged" runat="server"></asp:DropDownList>
                        栏目：
          <asp:DropDownList ID="NodeIDDropDownList" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server"></asp:DropDownList>
                        内容状态：
          <asp:DropDownList ID="State" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" class="input-small" runat="server"></asp:DropDownList>
                        <asp:CheckBox ID="IsDuplicate" class="checkbox inline" Text="包含重复标题" runat="server"></asp:CheckBox>
                    </td>
                </tr>
                <tr>
                    <td>时间：从
          <bairong:DateTimeTextBox ID="DateFrom" class="input-small" Columns="12" runat="server" />
                        &nbsp;到&nbsp;
          <bairong:DateTimeTextBox ID="DateTo" class="input-small" Columns="12" runat="server" />
                        <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible="false">目标：
          <asp:DropDownList ID="SearchType" class="input-small" runat="server"></asp:DropDownList>
                        </asp:PlaceHolder>
                        标题：
          <asp:TextBox ID="Keyword"
              MaxLength="500"
              Size="37"
              runat="server" />
                        用户：
          <asp:TextBox ID="MemberName"
              MaxLength="500"
              Size="37"
              runat="server" />
                        <asp:Button class="btn" OnClick="Search_OnClick" ID="Search" Text="搜 索" runat="server" />
                    </td>
                </tr>
            </table>
        </div>

        <table id="contents" class="table table-bordered table-hover">
            <tr class="info thead">
                <td>内容标题(点击查看) </td>
                <td>栏目</td>
                <td>用户</td>
                <td width="120">时间</td>
                <asp:Literal ID="ltlColumnHeadRows" runat="server"></asp:Literal>
                <td width="50">状态 </td>
                <td width="30">&nbsp;</td>
                <asp:Literal ID="ltlCommandHeadRows" runat="server" Visible="false"></asp:Literal>
                <td width="20">
                    <input type="checkbox" onclick="selectRows(document.getElementById('contents'), this.checked);">
                </td>
            </tr>
            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Literal ID="ltlItemTitle" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ltlChannel" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ltlMemberName" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ltlAddTime" runat="server"></asp:Literal>
                        </td>
                        <asp:Literal ID="ltlColumnItemRows" runat="server"></asp:Literal>
                        <td class="center" nowrap>
                            <asp:Literal ID="ltlItemStatus" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ltlItemEditUrl" runat="server"></asp:Literal>
                        </td>
                        <asp:Literal ID="ltlCommandItemRows" runat="server" Visible="false"></asp:Literal>
                        <td class="center">
                            <asp:Literal ID="ltlSelect" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>

        <bairong:SqlPagerByDataReader ID="spContents" runat="server" class="table table-pager" />

        <ul class="breadcrumb breadcrumb-button">
            <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible="false">
                <asp:Button class="btn" ID="AddContent" OnClick="AddContent_OnClick" Text="添加信息" runat="server" />
                <asp:Button class="btn" ID="SelectButton" Text="选择显示项" runat="server" />
                <asp:Button class="btn" ID="AddToGroup" Text="添加到内容组" runat="server" />
                <asp:Button class="btn" ID="Translate" Text="转 移" runat="server" />
                <asp:PlaceHolder ID="CheckPlaceHolder" runat="server">
                    <asp:Button class="btn" ID="Check" Text="审 核" runat="server" />
                </asp:PlaceHolder>
            </asp:PlaceHolder>
            <asp:Button class="btn" ID="Delete" Text="删 除" runat="server" />
            <asp:Button class="btn" ID="Generate" Text="生 成" runat="server" />
        </ul>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
