<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundAttachmentType" %>

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
  <bairong:alerts text="在此限定指定类型附件的最大尺寸，设置为0可禁止用户上传此类型附件。" runat="server"></bairong:alerts>

  <asp:dataGrid id="MyDataGrid" runat="server" showHeader="true"
      ShowFooter="false"
      AutoGenerateColumns="false"
      DataKeyField="ID"
      HeaderStyle-CssClass="info thead"
      CssClass="table table-bordered table-hover"
      gridlines="none">
    <Columns>
      <asp:TemplateColumn HeaderText="附件后缀">
				<ItemTemplate>
					&nbsp;<asp:Literal ID="ltlFileExtName" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle HorizontalAlign="left" />
			</asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="附件最大大小">
				<ItemTemplate>
					&nbsp;<asp:Literal ID="ltlMaxSize" runat="server"></asp:Literal> K
				</ItemTemplate>
				<ItemStyle HorizontalAlign="left" />
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
    <asp:Button class="btn btn-success" id="AddButton" Text="添加限制" runat="server" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->