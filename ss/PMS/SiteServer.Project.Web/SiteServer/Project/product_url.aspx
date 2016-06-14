<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundUrl" %>

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
          域名：
          <asp:TextBox ID="tbDomain" Size="20" runat="server"></asp:TextBox>
          授权：
          <asp:DropDownList ID="ddlIsLicense" class="input-medium" runat="server"></asp:DropDownList>
          <asp:Button OnClick="Search_OnClick" Text="搜 索" class="btn" style="margin-bottom: 0px" runat="server"></asp:Button>
        </td>
      </tr>
    </table>
  </div>

  <table class="table table-bordered table-hover">
    <tr class="info thead">
        <td align="center">使用的产品</td>
        <td align="center">域名</td>
        <td align="center">版本</td>
        <td align="center">数据库</td>
        <td align="center">添加日期</td>
        <td align="center">最后活动</td>
        <td align="center">活动次数</td>
        <td align="center">备注</td>
        <td align="center">操作</td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
            <td>
                <asp:Literal ID="ltlProductID" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlDomain" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlVersion" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlDbType" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlAddDate" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlLastActivity" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlCountOfActivity" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlSummary" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlIsLicense" runat="server"></asp:Literal>
            </td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button class="btn" id="AddButton" Text="添加网站" runat="server" />
  </ul>

</form>
</body>
</html>