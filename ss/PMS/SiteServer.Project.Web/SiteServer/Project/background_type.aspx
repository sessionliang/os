<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundType" %>

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
<bairong:alerts runat="server"></bairong:alerts>
<DIV class="column">
<div class="columntitle">办件类型管理</div>

		<asp:dataGrid id="dgContents" DataKeyField="TypeID" showHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="info thead" CssClass="table table-bordered table-hover" runat="server">
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
	</ASP:DataGrid>

	
</DIV>

<table width="95%" class="center">
	<tr>
		<td>
			<table cellpadding="0" cellspacing="0" width="100%">
				<tr><td>
					<asp:Button class="btn" id="AddButton" Text="新 增" runat="server" />
				</td></tr>
			</table>
		</td>
	</tr>
</table>



</form>

</body>
</html>