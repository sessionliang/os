﻿<%@ Page Language="C#" Inherits="SiteServer.WCM.BackgroundPages.BackgroundGovInteractType" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.WCM.Controls" Assembly="SiteServer.WCM" %>
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

  <asp:dataGrid id="dgContents" showHeader="true" AutoGenerateColumns="false" DataKeyField="TypeID" HeaderStyle-CssClass="info thead" CssClass="table table-bordered table-hover" runat="server">
  	<Columns>
        <asp:TemplateColumn HeaderText="办件类型">
			<ItemTemplate>
				<asp:Literal ID="ltlTypeName" runat="server"></asp:Literal>
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
    <asp:Button class="btn btn-success" id="AddButton" Text="新 增" runat="server" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->