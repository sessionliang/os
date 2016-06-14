<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundProjectLeft" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
</head>

<body>
<!--#include file="../inc/openWindow.html"-->
<form id="myForm" style="margin:0" runat="server">

	<asp:dataGrid id="dgContents" DataKeyField="ProjectID" showHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="info thead" CssClass="table table-bordered table-hover" runat="server">
		<Columns>
	    <asp:TemplateColumn HeaderText="项目名称">
				<ItemTemplate>
					<asp:Literal ID="ltlProjectName" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle cssClass="center" />
			</asp:TemplateColumn>
	    <asp:TemplateColumn HeaderText="进入">
				<ItemTemplate>
					<asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle Width="100" cssClass="center" />
			</asp:TemplateColumn>
		</Columns>
	</ASP:DataGrid>

</form>
</body>
</html>