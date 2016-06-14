<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundTemplate" %>

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

  <asp:dataGrid id="dgContents" showHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="info thead" CssClass="table table-bordered table-hover" runat="server">
  	<Columns>
		<asp:TemplateColumn
			HeaderText="模板名称">
			<ItemTemplate>
                <asp:Literal ID="ltlTemplateName" runat="server"></asp:Literal>
			</ItemTemplate>
			<ItemStyle Width="140" cssClass="center"/>
		</asp:TemplateColumn>
		<asp:BoundColumn
			HeaderText="模板文件夹"
			DataField="Name" >
			<ItemStyle Width="100" cssClass="center" />
		</asp:BoundColumn>
		<asp:TemplateColumn
			HeaderText="模板介绍">
			<ItemTemplate>
                <asp:Literal ID="ltlDescription" runat="server"></asp:Literal>
			</ItemTemplate>
			<ItemStyle HorizontalAlign="left" />
		</asp:TemplateColumn>
		<asp:TemplateColumn
			HeaderText="样图">
			<ItemTemplate>
                <asp:Literal ID="ltlSamplePic" runat="server"></asp:Literal>
			</ItemTemplate>
			<ItemStyle Width="130" cssClass="center"/>
		</asp:TemplateColumn>
		<asp:BoundColumn
			HeaderText="建立日期"
			DataField="CreationTime"
			DataFormatString="{0:yyyy-MM-dd}"
			ReadOnly="true">
			<ItemStyle Width="70" cssClass="center" />
		</asp:BoundColumn>
		<asp:TemplateColumn HeaderText="当前使用模板">
			<ItemTemplate>
				<asp:Literal ID="ltlIsDefault" runat="server"></asp:Literal>
			</ItemTemplate>
			<ItemStyle Width="100" cssClass="center" />
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
			<ItemStyle Width="70" cssClass="center" />
		</asp:TemplateColumn>
        <asp:TemplateColumn>
			<ItemTemplate>
				<asp:Literal ID="ltlCreateUrl" runat="server"></asp:Literal>
			</ItemTemplate>
			<ItemStyle Width="70" cssClass="center" />
		</asp:TemplateColumn>
        <asp:TemplateColumn>
			<ItemTemplate>
				<asp:Literal ID="ltlDeleteUrl" runat="server"></asp:Literal>
			</ItemTemplate>
			<ItemStyle Width="70" cssClass="center" />
		</asp:TemplateColumn>
	</Columns>
  </asp:dataGrid>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->