<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.BackgroundSpecItem" %>

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

  <asp:dataGrid id="dgContents" showHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="info thead" CssClass="table table-bordered table-hover" gridlines="none" runat="server">
    <Columns>
    <asp:TemplateColumn HeaderText="排序">
      <ItemTemplate> &nbsp;<%#Container.ItemIndex + 1%> </ItemTemplate>
      <ItemStyle Width="50" cssClass="center" />
    </asp:TemplateColumn>
    <asp:TemplateColumn HeaderText="规格值名称">
      <ItemTemplate> &nbsp;<%#DataBinder.Eval(Container.DataItem,"Title")%> </ItemTemplate>
      <ItemStyle HorizontalAlign="left" />
    </asp:TemplateColumn>
    <asp:TemplateColumn HeaderText="规格值图片">
      <ItemTemplate><asp:Literal ID="ltlIconUrl" runat="server"></asp:Literal></ItemTemplate>
      <ItemStyle HorizontalAlign="left" />
    </asp:TemplateColumn>
    <asp:TemplateColumn HeaderText="上升">
      <ItemTemplate>
        <asp:Literal ID="UpLink" runat="server"></asp:Literal>
      </ItemTemplate>
      <ItemStyle Width="50" cssClass="center" />
    </asp:TemplateColumn>
    <asp:TemplateColumn HeaderText="下降">
      <ItemTemplate>
        <asp:Literal ID="DownLink" runat="server"></asp:Literal>
      </ItemTemplate>
      <ItemStyle Width="50" cssClass="center" />
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
    <asp:Button class="btn btn-primary" id="btnAdd" text="添加规格值" runat="server"/>
    <asp:Button class="btn" id="btnReturn" text="返 回" runat="server"/>
  </ul>
</form>
</body>
</html>