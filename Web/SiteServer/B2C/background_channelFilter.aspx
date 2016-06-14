<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.BackgroundChannelFilter" %>

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

  <asp:dataGrid id="dgContents" showHeader="true" AutoGenerateColumns="false" DataKeyField="FilterID" HeaderStyle-CssClass="info thead" CssClass="table table-bordered table-hover" gridlines="none" runat="server">

    <Columns>
      <asp:TemplateColumn HeaderText="筛选属性">
        <ItemTemplate>&nbsp;<asp:Literal ID="ltlAttributeName" runat="server"></asp:Literal></ItemTemplate>
        <ItemStyle Width="120" HorizontalAlign="left" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="显示名称">
        <ItemTemplate>&nbsp;<asp:Literal ID="ltlDisplayName" runat="server"></asp:Literal></ItemTemplate>
        <ItemStyle Width="120" HorizontalAlign="left" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="筛选值">
        <ItemTemplate>&nbsp;<asp:Literal ID="ltlValues" runat="server"></asp:Literal></ItemTemplate>
        <ItemStyle />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="值类型">
        <ItemTemplate> <asp:Literal ID="ltlIsDefaultValues" runat="server"></asp:Literal> </ItemTemplate>
        <ItemStyle Width="80" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="上升">
        <ItemTemplate>
          <asp:HyperLink ID="hlUpLinkButton" runat="server"><img src="../Pic/icon/up.gif" border="0" alt="上升" /></asp:HyperLink>
        </ItemTemplate>
        <ItemStyle Width="60" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="下降">
        <ItemTemplate>
          <asp:HyperLink ID="hlDownLinkButton" runat="server"><img src="../Pic/icon/down.gif" border="0" alt="下降" /></asp:HyperLink>
        </ItemTemplate>
        <ItemStyle Width="60" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn>
        <ItemTemplate> <asp:Literal ID="ltlEditValuesUrl" runat="server"></asp:Literal> </ItemTemplate>
        <ItemStyle Width="80" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn>
        <ItemTemplate> <asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal> </ItemTemplate>
        <ItemStyle Width="80" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn>
        <ItemTemplate> <asp:Literal ID="ltlDeleteUrl" runat="server"></asp:Literal> </ItemTemplate>
        <ItemStyle Width="60" cssClass="center" />
      </asp:TemplateColumn>
    </Columns>
  </asp:dataGrid>

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button class="btn btn-success" id="AddFilter" Text="添加筛选属性" runat="server" />
    <asp:Button class="btn" onclick="Setting_OnClick" Text="同步到所有子栏目" runat="server" />
    <input type="button" class="btn" onClick="javascript:location.href='background_channel.aspx?PublishmentSystemID=<%=base.PublishmentSystemID%>';" value="返 回" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6.4 html permissions -->