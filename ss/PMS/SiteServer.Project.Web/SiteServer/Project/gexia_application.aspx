<%@ Page Language="C#" Inherits="SiteServer.GeXia.BackgroundPages.BackgroundApplication" %>

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
  <asp:Literal id="ltlBreadCrumb" runat="server" />
  <bairong:alerts runat="server" />

  <div class="well well-small">
    <table class="table table-noborder">
      <tr>
        <td>
          申请种类：
          <asp:DropDownList ID="ddlApplicationType" class="input-small" runat="server"></asp:DropDownList>
          申请类型：
          <asp:TextBox ID="tbApplyResource" Size="20" runat="server"></asp:TextBox>
          关键字：
          <asp:TextBox ID="tbKeyword" Size="20" runat="server"></asp:TextBox>
          <asp:Button OnClick="Search_OnClick" Text="搜 索" class="btn" style="margin-bottom: 0px" runat="server"></asp:Button>
        </td>
        <td class="pull-right">合计：<code><asp:Literal ID="ltlCount" runat="server" /></code></td>
      </tr>
    </table>
  </div>

  <table class="table table-bordered table-hover">
    <tr class="info thead">
        <td align="center">序号</td>
        <td align="center">申请种类</td>
        <td align="center">申请类型</td>
        <td align="center">联系人</td>
        <td align="center">联系邮箱</td>
        <td align="center">移动电话</td>
        <td align="center">QQ</td>
        <td align="center">固定电话</td>
        <td align="center">地区</td>
        <td align="center">单位类型</td>
        <td align="center">单位名称</td>
        <td align="center">添加日期</td>
        <td align="center" width="30"></td>
        <td align="center" width="30"></td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
            <td>
                <asp:Literal ID="ltlID" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlApplicationType" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlApplyResource" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlContactPerson" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlEmail" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlMobile" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlQQ" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlTelephone" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlLocation" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlOrgType" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlOrgName" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlAddDate" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlHandle" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlDelete" runat="server"></asp:Literal>
            </td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

</form>
</body>
</html>