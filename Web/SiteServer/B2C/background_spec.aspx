<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.BackgroundSpec" %>

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
        <ItemTemplate>&nbsp;<%#Container.ItemIndex + 1%></ItemTemplate>
        <ItemStyle Width="50" cssClass="center" />
      </asp:TemplateColumn>
    <asp:TemplateColumn HeaderText="规格名称">
      <ItemTemplate> &nbsp;<%#DataBinder.Eval(Container.DataItem,"SpecName")%> </ItemTemplate>
      <ItemStyle cssClass="center" Width="80" />
    </asp:TemplateColumn>
    <asp:TemplateColumn HeaderText="是否多选项">
      <ItemTemplate>
        <asp:Literal ID="ltlIsMultiple" runat="server"></asp:Literal>
      </ItemTemplate>
      <ItemStyle cssClass="center" Width="80" />
    </asp:TemplateColumn>
    <asp:TemplateColumn HeaderText="是否必选项">
      <ItemTemplate>
        <asp:Literal ID="ltlIsRequired" runat="server"></asp:Literal>
      </ItemTemplate>
      <ItemStyle cssClass="center" Width="80" />
    </asp:TemplateColumn>
    <asp:TemplateColumn HeaderText="规格值">
      <ItemTemplate>
        <div class="specItem">
          <asp:Literal ID="ltlItem" runat="server"></asp:Literal>
        </div>
      </ItemTemplate>
      <ItemStyle HorizontalAlign="left" />
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
        <asp:Literal ID="ltlItemUrl" runat="server"></asp:Literal>
      </ItemTemplate>
      <ItemStyle Width="70" cssClass="center" />
    </asp:TemplateColumn>
    <asp:TemplateColumn>
      <ItemTemplate>
        <asp:Literal ID="ltlDefaultUrl" runat="server"></asp:Literal>
      </ItemTemplate>
      <ItemStyle Width="70" cssClass="center" />
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
    <asp:Button class="btn btn-success" id="btnAdd" text="添加规格" runat="server"/>
    <asp:Button class="btn" id="btnSync" onclick="Sync_OnClick" Text="同步到所有子栏目" runat="server" />
    <input type="button" class="btn" onClick="javascript:location.href='background_channel.aspx?PublishmentSystemID=<%=base.PublishmentSystemID%>';" value="返 回" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6.4 html permissions -->