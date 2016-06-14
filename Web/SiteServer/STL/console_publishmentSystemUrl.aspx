<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.ConsolePublishmentSystemUrl" enableViewState = "false" %>

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

  <asp:dataGrid id="dgContents" showHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="info thead" CssClass="table table-bordered table-hover" gridlines="none" runat="server">
    <Columns>
      <asp:TemplateColumn HeaderText="应用名称">
        <ItemTemplate>
          <asp:Literal ID="ltlName" runat="server"></asp:Literal>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="文件夹">
        <ItemTemplate>
          <asp:Literal ID="ltlDir" runat="server"></asp:Literal>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="生成页面URL前缀">
        <ItemTemplate>
          <asp:Literal ID="ltlPublishmentSystemUrl" runat="server"></asp:Literal>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="网站部署方式">
        <ItemTemplate>
          <asp:Literal ID="ltlIsMultiDeployment" runat="server"></asp:Literal>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="外网访问地址">
        <ItemTemplate>
          <asp:Literal ID="ltlOuterUrl" runat="server"></asp:Literal>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="内网访问地址">
        <ItemTemplate>
          <asp:Literal ID="ltlInnerUrl" runat="server"></asp:Literal>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="功能页面访问方式">
        <ItemTemplate>
          <asp:Literal ID="ltlFuncFilesType" runat="server"></asp:Literal>
        </ItemTemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn>
        <ItemTemplate>
          <asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="50" cssClass="center" />
      </asp:TemplateColumn>
    </Columns>
  </asp:dataGrid>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->