﻿<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.BackgroundTemplateCss" %>

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
  <bairong:alerts text="样式文件在站点目录 css 中，模板中使用 &amp;lt;link rel=&quot;stylesheet&quot; type=&quot;text/css&quot; href=&quot;{stl.siteurl}/css/样式文件.css&quot; /&gt; 引用。" runat="server" />

  <asp:dataGrid id="dgContents" showHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="info thead" CssClass="table table-bordered table-hover" gridlines="none" runat="server">
    <Columns>
      <asp:TemplateColumn
      HeaderText="文件名称">
        <ItemTemplate> <%# Container.DataItem %> </ItemTemplate>
        <ItemStyle HorizontalAlign="left" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="文件编码">
        <ItemTemplate> <%# GetCharset((string)Container.DataItem) %> </ItemTemplate>
        <ItemStyle Width="160" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn>
        <ItemTemplate> <a href="<%=PublishmentSystemUrl%>/css/<%# Container.DataItem %>" target="_blank">查看</a> </ItemTemplate>
        <ItemStyle Width="70" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn>
        <ItemTemplate> <a href="background_templateCssAdd.aspx?PublishmentSystemID=<%=base.PublishmentSystemID%>&FileName=<%# Container.DataItem %>">编辑</a> </ItemTemplate>
        <ItemStyle Width="70" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn>
        <ItemTemplate> <a href="background_templateCss.aspx?PublishmentSystemID=<%=base.PublishmentSystemID%>&Delete=True&FileName=<%# Container.DataItem %>" onClick="javascript:return confirm('此操作将删除样式文件“<%# Container.DataItem %>”，确认吗？');">删除</a> </ItemTemplate>
        <ItemStyle Width="70" cssClass="center" />
      </asp:TemplateColumn>
    </Columns>
  </asp:dataGrid>

  <ul class="breadcrumb breadcrumb-button">
    <input type=button class="btn btn-success" onClick="location.href='background_templateCssAdd.aspx?PublishmentSystemID=<%=base.PublishmentSystemID%>';" value="添 加" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->