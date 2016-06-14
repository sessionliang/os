<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundFormPage" %>

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
  <bairong:alerts text="点击分页标题可以进入分页项维护界面" runat="server" />

  <asp:dataGrid id="dgContents" showHeader="true" AutoGenerateColumns="false" DataKeyField="ID" HeaderStyle-CssClass="info thead" CssClass="table table-bordered table-hover" gridlines="none" runat="server">
    <Columns>
    <asp:TemplateColumn HeaderText="分页标题">
      <ItemTemplate>
        <asp:Literal ID="ltlTitle" runat="server"></asp:Literal>
      </ItemTemplate>
      <ItemStyle cssClass="center" />
    </asp:TemplateColumn>
    <asp:TemplateColumn HeaderText="上升">
      <ItemTemplate>
        <asp:HyperLink ID="hlUpLinkButton" runat="server"><img src="../../SiteFiles/bairong/icons/up.gif" border="0" alt="上升" /></asp:HyperLink>
      </ItemTemplate>
      <ItemStyle Width="40" cssClass="center" />
    </asp:TemplateColumn>
    <asp:TemplateColumn HeaderText="下降">
      <ItemTemplate>
        <asp:HyperLink ID="hlDownLinkButton" runat="server"><img src="../../SiteFiles/bairong/icons/down.gif" border="0" alt="下降" /></asp:HyperLink>
      </ItemTemplate>
      <ItemStyle Width="40" cssClass="center" />
    </asp:TemplateColumn>
        <asp:TemplateColumn>
      <ItemTemplate>
        <asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
      </ItemTemplate>
      <ItemStyle Width="50" cssClass="center" />
    </asp:TemplateColumn>
    <asp:TemplateColumn>
      <ItemTemplate>
        <asp:Literal ID="ltlDeleteUrl" runat="server"></asp:Literal>
      </ItemTemplate>
      <ItemStyle Width="50" cssClass="center" />
    </asp:TemplateColumn>
  </Columns>
  </asp:dataGrid>

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button class="btn btn-success" id="btnAdd" Text="新增分页" runat="server" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->