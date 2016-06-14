<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.ConsoleIndependentTemplate" %>

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
  <asp:Literal id="ltlBreadCrumb" runat="server" />
  <bairong:alerts runat="server" />

  <asp:dataGrid id="dgContents" showHeader="true" AutoGenerateColumns="false" DataKeyField="Name" HeaderStyle-CssClass="info thead" CssClass="table table-bordered table-hover" gridlines="none" runat="server">
    <Columns>
      <asp:TemplateColumn HeaderText="独立模板名称">
        <ItemTemplate><asp:Literal ID="ltlTemplateName" runat="server"></asp:Literal></ItemTemplate>
        <ItemStyle HorizontalAlign="left"/>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="应用类型">
        <ItemTemplate><asp:Literal ID="ltlPublishmentSystemType" runat="server"></asp:Literal></ItemTemplate>
        <ItemStyle cssClass="center"/>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="包含模板">
        <ItemTemplate><asp:Literal ID="ltlTemplateTypes" runat="server"></asp:Literal></ItemTemplate>
        <ItemStyle cssClass="center"/>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="独立模板文件夹">
        <ItemTemplate><asp:Literal ID="ltlDirectoryName" runat="server"></asp:Literal></ItemTemplate>
        <ItemStyle HorizontalAlign="left"/>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="独立模板介绍">
        <ItemTemplate><asp:Literal ID="ltlDescription" runat="server"></asp:Literal></ItemTemplate>
        <ItemStyle HorizontalAlign="left" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="创建日期">
        <ItemTemplate><asp:Literal ID="ltlCreationDate" runat="server"></asp:Literal></ItemTemplate>
        <ItemStyle Width="70" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn>
        <ItemTemplate>
          <asp:Literal ID="ltlDownloadUrl" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="180" cssClass="center"/>
      </asp:TemplateColumn>
      <asp:TemplateColumn>
        <ItemTemplate>
          <asp:Literal ID="ltlDeleteUrl" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="50" cssClass="center"/>
      </asp:TemplateColumn>
    </Columns>
  </asp:dataGrid>

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button class="btn btn-success" id="Import" Text="导入独立模板" runat="server" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->