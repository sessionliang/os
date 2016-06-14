<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.BackgroundTemplateInclude" %>

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
  <bairong:alerts text="包含文件在站点目录 include 中，模板中使用 &amp;lt;stl:include file=&quot;/include/包含文件.html&quot;&gt;&amp;lt;/stl:include&gt; 引用。" runat="server" />

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
        <ItemTemplate> <a href="<%=PublishmentSystemUrl%>/include/<%# Container.DataItem %>" target="_blank">查看</a> </ItemTemplate>
        <ItemStyle Width="70" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn>
        <ItemTemplate> <a href="background_templateIncludeAdd.aspx?PublishmentSystemID=<%=base.PublishmentSystemID%>&FileName=<%# Container.DataItem %>">编辑</a> </ItemTemplate>
        <ItemStyle Width="70" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn>
        <ItemTemplate> <a href="background_templateInclude.aspx?PublishmentSystemID=<%=base.PublishmentSystemID%>&Delete=True&FileName=<%# Container.DataItem %>" onClick="javascript:return confirm('此操作将删除包含文件“<%# Container.DataItem %>”，确认吗？');">删除</a> </ItemTemplate>
        <ItemStyle Width="70" cssClass="center" />
      </asp:TemplateColumn>
    </Columns>
  </asp:dataGrid>

  <ul class="breadcrumb breadcrumb-button">
    <input type=button class="btn btn-success" onClick="location.href='background_templateIncludeAdd.aspx?PublishmentSystemID=<%=base.PublishmentSystemID%>';" value="添 加" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->