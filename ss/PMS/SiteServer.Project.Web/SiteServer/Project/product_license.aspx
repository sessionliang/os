<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundLicense" %>

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
          授权域名：
          <asp:TextBox ID="tbDomain" Size="20" runat="server"></asp:TextBox>
          授权产品：
          <asp:DropDownList ID="ddlProductID" runat="server"></asp:DropDownList>
        </td>
      </tr>
      <tr>
        <td>
          站点名称：
          <asp:TextBox ID="tbSiteName" Size="20" runat="server"></asp:TextBox>
          客户简称：
          <asp:TextBox ID="tbClientName" Size="20" runat="server"></asp:TextBox>
          <asp:Button OnClick="Search_OnClick" Text="搜 索" class="btn" style="margin-bottom: 0px" runat="server"></asp:Button>
        </td>
      </tr>
    </table>
  </div>

  <table class="table table-bordered table-hover">
    <tr class="info thead">
        <td align="center">授权对象</td>
        <td align="center">授权产品</td>
        <td align="center">站点数</td>
        <td align="center">发放日期</td>
        <td align="center">站点名称</td>
        <td align="center">客户简称</td>
        <td align="center">关联客户/订单</td>
        <td align="center">到期时间</td>
        <td align="center">备注</td>
        <td align="center">编辑</td>
        <td align="center">删除</td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
            <td>
                <asp:Literal ID="ltlLicenseString" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlProductID" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlMaxSiteNumber" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlLicenseDate" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlSiteName" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlClientName" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlAccountOrder" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlExpireDate" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlSummary" runat="server"></asp:Literal>
            </td>
            <td align="center">
                <a href="product_licenseAdd.aspx?id=<%#DataBinder.Eval(Container.DataItem, "ID")%>">编辑</a>
            </td>
            <td align="center">
                <a href="product_license.aspx?Delete=True&ID=<%# DataBinder.Eval(Container.DataItem,"ID")%>" onClick="javascript:return confirm('此操作将删除此许可证，确认吗？');">删除</a>
            </td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button class="btn btn-success" id="AddButton" Text="制作许可证" runat="server" />
  </ul>

</form>
</body>
</html>