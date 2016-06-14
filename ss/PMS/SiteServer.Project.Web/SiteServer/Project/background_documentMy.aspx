<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundDocumentMy" %>
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

	<asp:dataGrid id="dgContents" DataKeyField="DocumentID" showHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="info thead" CssClass="table table-bordered table-hover" runat="server">
		<Columns>
      <asp:TemplateColumn HeaderText="文档类别">
				<ItemTemplate>
					<asp:Literal ID="ltlTypeName" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle cssClass="center" />
			</asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="文档文件">
				<ItemTemplate>
					<asp:Literal ID="ltlFileName" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle cssClass="center" />
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="版本">
				<ItemTemplate>
					<asp:Literal ID="ltlVersion" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle cssClass="center" />
			</asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="文档说明">
				<ItemTemplate>
					<asp:Literal ID="ltlDescription" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle cssClass="center" />
			</asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="添加人">
				<ItemTemplate>
					<asp:Literal ID="ltlUserName" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle cssClass="center" Width="100" />
			</asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="添加时间">
				<ItemTemplate>
					<asp:Literal ID="ltlAddDate" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle cssClass="center" Width="120" />
			</asp:TemplateColumn>
			<asp:TemplateColumn>
				<ItemTemplate>
					<asp:Literal ID="ltlDeleteUrl" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle Width="50" cssClass="center" />
			</asp:TemplateColumn>
		</Columns>
	</ASP:DataGrid>

</form>
</body>
</html>