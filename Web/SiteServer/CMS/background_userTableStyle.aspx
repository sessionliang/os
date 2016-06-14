<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundUserTableStyle" %>

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

  <asp:dataGrid id="MyDataGrid" runat="server" showHeader="true"
      ShowFooter="false"
      AutoGenerateColumns="false"
      HeaderStyle-CssClass="info thead"
      CssClass="table table-bordered table-hover"
      gridlines="none">
    <Columns>
      <asp:TemplateColumn
        HeaderText="字段名">
        <ItemTemplate>
          <asp:Label ID="AttributeName" runat="server"></asp:Label>
        </ItemTemplate>
        <ItemStyle Width="140" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="显示名称">
        <ItemTemplate>
          <asp:Label ID="DisplayName" runat="server"></asp:Label>
        </ItemTemplate>
        <ItemStyle HorizontalAlign="left" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="表单提交类型">
        <ItemTemplate>
          <asp:Label ID="InputType" runat="server"></asp:Label>
        </ItemTemplate>
        <ItemStyle Width="100" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="是否显示">
        <ItemTemplate>
          <asp:Label ID="IsVisible" runat="server"></asp:Label>
        </ItemTemplate>
        <ItemStyle Width="70" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="需要验证">
        <ItemTemplate>
          <asp:Label ID="IsValidate" runat="server"></asp:Label>
        </ItemTemplate>
        <ItemStyle Width="70" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="上升">
        <ItemTemplate>
          <asp:HyperLink ID="UpLinkButton" runat="server"><img src="../Pic/icon/up.gif" border="0" alt="上升" /></asp:HyperLink>
        </ItemTemplate>
        <ItemStyle Width="40" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="下降">
        <ItemTemplate>
          <asp:HyperLink ID="DownLinkButton" runat="server"><img src="../Pic/icon/down.gif" border="0" alt="下降" /></asp:HyperLink>
        </ItemTemplate>
        <ItemStyle Width="40" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="显示样式">
        <ItemTemplate>
          <asp:Label ID="EditStyle" runat="server"></asp:Label>
        </ItemTemplate>
        <ItemStyle Width="120" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="表单验证">
        <ItemTemplate>
          <asp:Label ID="EditValidate" runat="server"></asp:Label>
        </ItemTemplate>
        <ItemStyle Width="80" cssClass="center" />
      </asp:TemplateColumn>
    </Columns>
  </asp:dataGrid>

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button class="btn btn-success" id="AddStyle" Text="新增字段" runat="server" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->