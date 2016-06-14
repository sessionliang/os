<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.BackgroundStorage" %>

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

  <asp:dataGrid id="dgContents" showHeader="true" AutoGenerateColumns="false" DataKeyField="StorageID" HeaderStyle-CssClass="info thead" CssClass="table table-bordered table-hover" runat="server">
    <Columns>
      <asp:BoundColumn
        HeaderText="空间名称"
        DataField="StorageName" >
        <ItemStyle HorizontalAlign="left" />
      </asp:BoundColumn>
      <asp:TemplateColumn HeaderText="空间域名">
        <ItemTemplate>
          <asp:Literal ID="ltlStorageURL" runat="server"></asp:Literal>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="空间类型">
        <ItemTemplate>
          <asp:Literal ID="ltlStorageType" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="120" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="是否启用">
        <ItemTemplate>
          <asp:Literal ID="ltlIsEnabledHtml" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="60" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn>
        <ItemTemplate>
          <asp:Literal ID="ltlEditHtml" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="50" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn>
        <ItemTemplate>
          <asp:Literal ID="ltlTestHtml" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="50" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn>
        <ItemTemplate>
          <asp:Literal ID="ltlEnabledHtml" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="40" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn>
        <ItemTemplate>
          <asp:Literal ID="ltlDeleteHtml" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="40" cssClass="center" />
      </asp:TemplateColumn>
    </Columns>
  </asp:dataGrid>

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button class="btn btn-success" id="btnAdd" Text="添加空间" runat="server" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->